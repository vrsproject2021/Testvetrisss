using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using VETRISScheduler.Core;
using DICOMLib;


namespace VETRISListenerFileProcess
{
    public partial class ListenerFileProcessService : ServiceBase
    {
        #region members & variables
        private static int intFreq = 10;
        private static int intServiceID = 11;

        private static string strDCMRCVRFLDR = string.Empty;
        private static string strXFEREXEPATH = string.Empty;
        private static string strXFEREXEPATHALT = string.Empty;
        private static string strXFEREXEPARMS = string.Empty;
        private static string strXFEREXEPARMSJPGLL = string.Empty;
        private static string strXFEREXEPARMJ2KLL = string.Empty;
        private static string strXFEREXEPARMJ2KLS = string.Empty;
        private static string strXFEREXEPARMSSENDDCM = string.Empty;
        private static string strDCMMODIFYEXEPATH = string.Empty;
        private static string strPACSXFERDLFLDR = string.Empty;
        private static string strENBLLSPACSXFER = string.Empty;
        private static string strUMDCMFILES = string.Empty;
        private static string strREJDCMFILES = string.Empty;
        private static string strDCMDMPEXEPATH = string.Empty;

        private static string strConfigPath = AppDomain.CurrentDomain.BaseDirectory;
        private static string strSvcName = "VETRIS Listener File Processing Service";

        string strErrRegion = string.Empty;


        Scheduler objCore;
        ListenerFileProcess objLFP;

        // private static string storePath = "E:\\GTR"; // all dicom files
        //private static string pendingPacsPath = "E:\\PendingPACS"; // dicom files to be sent to PACS
        List<ModalityData> modalities = new List<ModalityData>();
        string strCatchMessage = "";




        #endregion

        public ListenerFileProcessService()
        {
            InitializeComponent();
        }

        #region OnStart
        protected override void OnStart(string[] args)
        {
            GetServiceDetails(strConfigPath, ref strCatchMessage);
            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Service started Successfully", false);

            System.Threading.ThreadStart job_file_rec = new System.Threading.ThreadStart(doProcessReceivedFiles);
            System.Threading.Thread threadFileRec = new System.Threading.Thread(job_file_rec);
            threadFileRec.Start();

            if (strENBLLSPACSXFER == "Y")
            {
                System.Threading.ThreadStart job_xfer = new System.Threading.ThreadStart(doSendFilesToPACS);
                System.Threading.Thread threadXfer = new System.Threading.Thread(job_xfer);
                threadXfer.Start();
            }
        }
        #endregion


        #region OnStop
        protected override void OnStop()
        {
            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Service stopped successfully.", false);
                base.OnStop();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(strSvcName, "Error Stopping Service." + ex.Message, EventLogEntryType.Error);
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Error stopping Service. " + ex.Message, true);
            }
        }
        #endregion

        #region GetServiceDetails
        private bool GetServiceDetails(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                #region Service Control Params
                objCore = new Scheduler();
                objCore.SERVICE_ID = intServiceID;
                if (objCore.GetServiceDetails(strConfigPath, ref strCatchMessage))
                {
                    intFreq = objCore.FREQUENCY;
                    strSvcName = objCore.SERVICE_NAME;

                    strDCMRCVRFLDR = objCore.DICOM_RECEIVER_FOLDER;
                    strXFEREXEPATH = objCore.PACS_TRANSFER_EXE_PATH;
                    strXFEREXEPATHALT = objCore.PACS_TRANSFER_EXE_ALTERNATE_PATH;
                    strXFEREXEPARMS = objCore.PACS_TRANSFER_EXE_PARAMS;
                    strXFEREXEPARMSJPGLL = objCore.PACS_TRANSFER_EXE_PARAMS_FOR_JPG_LOSSLESS;
                    strXFEREXEPARMJ2KLL = objCore.PACS_TRANSFER_EXE_PARAMS_FOR_JPG2K_LOSSLESS;
                    strXFEREXEPARMJ2KLS = objCore.PACS_TRANSFER_EXE_PARAMS_FOR_JPG2K_LOSSY;
                    strXFEREXEPARMSSENDDCM = objCore.PACS_TRANSFER_EXE_PARAMS_FOR_SEND_DCM;
                    strPACSXFERDLFLDR = objCore.FOLDER_FOR_PACS_TRANSFER;
                    strENBLLSPACSXFER = objCore.ENABLE_PACS_TRANSFER_FOR_LISTENER_FILES;
                    strUMDCMFILES = objCore.UNKNOWN_MODALITY_DICOM_FILES;
                    strREJDCMFILES = objCore.REJECTED_DICOM_FILES_PATH;
                    strDCMDMPEXEPATH = objCore.DICOM_TAG_DUMP_EXE_PATH;
                    bReturn = true;
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetServiceDetails() => Core::GetServiceDetails - Error : " + strCatchMessage, true);
                    bReturn = false;
                }
                #endregion

                #region Fetch Modality
                objLFP = new ListenerFileProcess();

                if (objLFP.FetchModalityList(strConfigPath, ref ds, ref strCatchMessage))
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow r in ds.Tables[0].Rows)
                        {
                            var d = new ModalityData();
                            d.Code = r["code"].ToString();
                            d.Path = r["file_receive_path"].ToString();
                            d.Tags = r["dicom_tag"].ToString();
                            modalities.Add(d);
                        }

                    }
                    bReturn = true;
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetServiceDetails() => Core::FetchModalityList - Error : " + strCatchMessage, true);
                    bReturn = false;
                }

                #endregion

            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetServiceDetails() - Exception: " + expErr.Message, true);
            }
            finally
            {
                objCore = null;
                objLFP = null;
                ds.Dispose();
            }
            return bReturn;
        }
        #endregion

        #region doProcessReceivedFiles
        private void doProcessReceivedFiles()
        {
            try
            {
                while (true)
                {
                    DirectoryProcess();
                    System.Threading.Thread.Sleep(intFreq * 1000);
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessReceivedFiles() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }
        }
        #endregion

        #region DirectoryProcess
        public void DirectoryProcess()
        {

            var count = 0;
            var uids = Directory.GetDirectories(strDCMRCVRFLDR).ToList();
            bool exists = false;

            try
            {
                foreach (var uid in uids)
                {
                    List<string> files = GetFiles(uid);
                    //
                    string destFolder = GetDestinationFolder(files);
                    if (destFolder == null) destFolder = strUMDCMFILES;


                    try
                    {
                        // copy to destination folder all dicom files and also copy to pending to be sent to PACS
                        foreach (var f in files)
                        {

                            var fileName = Path.GetFileName(f); // dicom file name only with extension if any
                            exists = System.IO.Directory.Exists(destFolder);

                            if (!exists) System.IO.Directory.CreateDirectory(destFolder);
                            if (strENBLLSPACSXFER == "Y")
                            {
                                File.Copy(f, Path.Combine(destFolder, fileName), true);// to be processed later
                            }
                            else
                            {
                                if (File.Exists(Path.Combine(destFolder, fileName))) File.Delete(Path.Combine(destFolder, fileName));
                                File.Move(f, Path.Combine(destFolder, fileName));// to be processed later
                            }
                            if (strENBLLSPACSXFER == "Y")
                            {
                                if (File.Exists(Path.Combine(strPACSXFERDLFLDR, fileName))) File.Delete(Path.Combine(strPACSXFERDLFLDR, fileName));
                                File.Move(f, Path.Combine(strPACSXFERDLFLDR, fileName));
                            }
                            count++; // keep count of processed files
                        }
                    }
                    catch (Exception expErr)
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessReceivedFiles()=>DirectoryProcess() - Copying & Moving Files - Exception: " + expErr.Message, true);
                    }

                    //Directory.Delete(uid,true); // delete src folder along with content files and directories
                }
                //if (strENBLLSPACSXFER == "Y" && count > 0) SendToPACS(uid); // Send to PACS
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessReceivedFiles()=>DirectoryProcess() - Exception: " + expErr.Message, true);
            }
        }
        #endregion

        #region doSendFilesToPACS
        private void doSendFilesToPACS()
        {
            try
            {
                while (true)
                {
                    SendToPACS();
                    System.Threading.Thread.Sleep(intFreq * 1000);
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFilesToPACS() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }
        }
        #endregion

        #region SendToPACS
        private void SendToPACS()
        {
            string strRetMsg = string.Empty;
            string strFileName = string.Empty;
            string strSUID = string.Empty;
            DicomDecoder dd;
            string[] arr = new string[0];

            List<string> files = Directory.GetFiles(strPACSXFERDLFLDR).ToList();

            try
            {
                foreach (var file in files)
                {
                    strFileName = Path.GetFileName(file);



                    // send to pacs
                    bool success = TransferFileToPacs(strPACSXFERDLFLDR, file, ref strRetMsg);
                    if (success)
                    {
                        if (File.Exists(file)) File.Delete(file);
                    }
                    else
                    {
                        #region if Transfer to PACS fails

                        #region Get Study UID
                        dd = new DicomDecoder();
                        dd.DicomFileName = file;
                        List<string> str = dd.dicomInfo;
                        arr = new string[20];
                        arr = GetallTags(str);
                        strSUID = arr[0].Trim();
                        if (strSUID.Trim() == string.Empty) strSUID = GetStudyUIDFromDump(file);
                        dd = null;
                        #endregion

                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FAILED :: transfer to PACS :: SUID : " + strSUID.Trim() + " :: File Name : " + strFileName, true);
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Reason :: " + strRetMsg, true);
                        if (!Directory.Exists(strREJDCMFILES)) Directory.CreateDirectory(strREJDCMFILES);
                        if (File.Exists(Path.Combine(strREJDCMFILES, strFileName))) File.Delete(Path.Combine(strREJDCMFILES, strFileName));
                        File.Move(file, Path.Combine(strREJDCMFILES, strFileName));
                        CreateFileXferFailureNotification(strSUID, strFileName, strRetMsg);
                        #endregion
                    }
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessReceivedFiles()=>DirectoryProcess()=>SendToPACS() - Exception: " + expErr.Message, true);
            }
        }
        #endregion

        #region CreateFileXferFailureNotification
        private bool CreateFileXferFailureNotification(string strStudyUID, string strFileName, string strReason)
        {

            string strRetMessage = string.Empty;
            string strCatchMessage = string.Empty;
            bool bReturn = false;
            ListenerFileProcess objLFP1 = new ListenerFileProcess();

            try
            {
                objLFP1.STUDY_UID = strStudyUID;
                objLFP1.FILE_NAME = strFileName;
                objLFP1.FAILURE_REASON = strReason.Trim();

                if (!objLFP1.CreateFileXferFailureNotification(strConfigPath, ref strRetMessage, ref strCatchMessage))
                {
                    bReturn = false;
                    if (strCatchMessage.Trim() != string.Empty)
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "SendToPACS()=>CreateFileXferFailureNotification()=>Core:CreateFileXferFailureNotification():: Exception: " + strCatchMessage, true);
                    else
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "SendToPACS()=>CreateFileXferFailureNotification()=>Core:CreateFileXferFailureNotification():: Error : " + strRetMessage, true);
                }
                else
                    bReturn = true;
            }
            catch (Exception ex)
            {

                bReturn = false;
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "SendToPACS()=>CreateFileXferFailureNotification():: Exception: " + ex.Message, true);
            }
            finally
            {
                objLFP1 = null;
            }

            return bReturn;
        }
        #endregion

        #region Utilities

        /// <summary>
        /// Get all content files recursively in subdirectories also
        /// </summary>
        /// <param name="uid"></param>
        /// <returns>List of Files with path</returns>
        private List<string> GetFiles(string uid)
        {
            List<string> files = Directory.GetFiles(uid).ToList();
            var dirs = Directory.GetDirectories(uid).ToList();
            if (dirs.Count > 0)
            {
                foreach (var dir in dirs)
                {
                    var _files = GetFiles(dir);
                    if (_files.Count > 0) files.AddRange(_files);
                }
            }
            return files;
        }

        private string GetDestinationFolder(List<string> files)
        {
            foreach (var f in files)
            {
                var isDicom = CoreCommon.IsDicomFile(f);
                if (isDicom)
                {
                    var m = GetModality(f);
                    if (m != null)
                    {
                        return m.Path; // modality wise folder
                    }

                }

            }
            return null;
        }

        private ModalityData GetModality(string dicomFile)
        {
            // Logic for getting Modality from dicomfile supplied
            DicomDecoder decoder = new DicomDecoder();
            decoder.DicomFileName = dicomFile;
            var info = decoder.dicomInfo;
            var dicomTags = new string[20];
            dicomTags = GetallTags(info);

            string tagString = dicomTags[1].Trim(); //Modality
            foreach (var m in modalities)
            {
                var tags = m.Tags.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Where(i => !string.IsNullOrEmpty(i)).ToList();
                if (tags.Contains(tagString))
                {
                    return m;
                }
            }

            return null;

        }

        #endregion

        #region ExtractStrings
        void ExtractStrings(string s1, out string s4, out string s5, out string s11, out string s12)
        {
            int ind;
            string s2, s3;
            ind = s1.IndexOf("//");
            s2 = s1.Substring(0, ind);
            s11 = s1.Substring(0, 4);
            s12 = s1.Substring(4, 4);
            s3 = s1.Substring(ind + 2);
            ind = s3.IndexOf(":");
            s4 = s3.Substring(0, ind);
            s5 = s3.Substring(ind + 1);
        }
        #endregion

        #region GetallTags
        private string[] GetallTags(List<string> str)
        {

            string strDescription = string.Empty;
            string StudyUID = string.Empty;
            string ModalityID = string.Empty;
            string StrPName = string.Empty;
            string StudyDt = string.Empty;
            string StudyTime = string.Empty;
            string sDt = string.Empty;
            string sTime = string.Empty;
            string studyDtTime = string.Empty;
            string SeriesUID = string.Empty;
            string SeriesNumber = string.Empty;
            string InstanceNumber = string.Empty;
            string SOPInstanceUID = string.Empty;
            string InstitutionName = string.Empty;
            string PatientID = string.Empty;
            string AccnNo = string.Empty;
            string RefPhys = string.Empty;
            string Manufacturer = string.Empty;
            string StationName = string.Empty;
            string Model = string.Empty;
            string ModalityAETitle = string.Empty;
            string Reason = string.Empty;
            string BirthDt = string.Empty;
            string bDt = string.Empty;
            string PatientSex = string.Empty;
            string PatientAge = string.Empty;
            //string PatientWt = string.Empty;//(0010,1030)
            //string Species = string.Empty;//(0010,2201)
            //string Breed = string.Empty;//(0010,2292)
            //string Owner = string.Empty;//(0010,2297)
            string PriorityID = "0";


            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                string s1, s4, s5, s11, s12;
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);



                #region Tags
                s5 = s5.Replace("\t", "");
                s5 = s5.Replace("\n", "");

                switch (s11.ToUpper())
                {
                    case "0008":
                        #region s11 =0008
                        switch (s12.ToUpper())
                        {
                            case "0018":
                                SOPInstanceUID = s5.Replace("\0", "");
                                break;
                            case "0020":
                                StudyDt = s5.Replace("\0", "");
                                StudyDt = StudyDt.Trim();
                                if ((StudyDt.Length == 8))
                                {
                                    string yyyy = StudyDt.Substring(0, 4);
                                    string MM = StudyDt.Substring(4, 2);
                                    string DD = StudyDt.Substring(6, 2);
                                    sDt = yyyy + "-" + MM + "-" + DD;
                                }
                                else
                                {
                                    sDt = "0000-00-00";
                                }
                                break;
                            case "0030":
                                StudyTime = s5.Replace("\0", "");
                                StudyTime = StudyTime.Trim();
                                if ((StudyTime.Length == 6))
                                {
                                    string Hr = StudyTime.Substring(0, 2);
                                    string Min = StudyTime.Substring(2, 2);
                                    string Sec = StudyTime.Substring(4, 2);
                                    sTime = Hr + ":" + Min + ":" + Sec;
                                }
                                else
                                {
                                    sTime = "00:00:00";
                                }
                                break;
                            case "0050":
                                AccnNo = s5.Replace("\0", "");
                                break;
                            case "0060":
                                ModalityID = s5.Replace("\0", "");
                                break;
                            case "0070":
                                Manufacturer = s5.Replace("\0", "");
                                break;
                            case "0080":
                                if (InstitutionName.Trim() == string.Empty)
                                {
                                    InstitutionName = s5.Replace("\0", "");
                                    InstitutionName = s5.Replace("^", " ");
                                }
                                break;
                            case "0090":
                                RefPhys = s5.Replace("\0", "");
                                RefPhys = s5.Replace("^", " ");
                                break;
                            case "1010":
                                StationName = s5.Replace("\0", "");
                                break;
                            case "1090":
                                Model = s5.Replace("\0", "");
                                break;
                            default:
                                break;
                        }
                        #endregion
                        break;
                    case "0010":
                        #region s11 =0010
                        switch (s12.ToUpper())
                        {
                            case "0010":
                                StrPName = s5.Replace("\0", "");
                                StrPName = s5.Replace("^", " ");
                                break;
                            case "0020":
                                PatientID = s5.Replace("\0", "");
                                break;
                            case "0030":
                                BirthDt = s5.Replace("\0", "");
                                BirthDt = BirthDt.Trim();
                                if ((BirthDt.Length == 8))
                                {
                                    string yyyy = BirthDt.Substring(0, 4);
                                    string MM = BirthDt.Substring(4, 2);
                                    string DD = BirthDt.Substring(6, 2);
                                    bDt = yyyy + "-" + MM + "-" + DD;
                                }
                                else
                                {
                                    bDt = "0000-00-00";
                                }
                                break;
                            case "0040":
                                PatientSex = s5.Replace("\0", "");
                                break;
                            case "1010":
                                PatientAge = s5.Replace("\0", "");
                                break;
                        }
                        #endregion
                        break;
                    case "0020":
                        #region s11 =0020
                        switch (s12.ToUpper())
                        {
                            case "000D":
                            case "000d":
                                if (StudyUID.Trim() == string.Empty)
                                {
                                    StudyUID = s5.Replace("\0", "");
                                }
                                break;
                            case "000E":
                            case "000e":
                                SeriesUID = s5.Replace("\0", "");
                                break;
                            case "0011":
                                SeriesNumber = s5.Replace("\0", "");
                                break;
                            case "0013":
                                InstanceNumber = s5.Replace("\0", "");
                                break;
                        }
                        #endregion
                        break;
                    case "0032":
                        #region s11 =0032
                        switch (s12.ToUpper())
                        {
                            case "000C":
                                if (IsInteger(s5.Replace("\0", "")))
                                {
                                    PriorityID = s5.Replace("\0", "");
                                }
                                break;
                        }
                        #endregion
                        break;
                    case "0040":
                        #region s11 =0040
                        switch (s12.ToUpper())
                        {
                            case "0241":
                                ModalityAETitle = s5.Replace("\0", "");
                                break;
                            case "1002":
                                Reason = s5.Replace("\0", "");
                                break;

                        }
                        #endregion
                        break;
                    default:
                        break;
                }
                #endregion
            }

            studyDtTime = sDt + " " + sTime;

            string[] arr = new string[20];
            arr[0] = StudyUID;
            arr[1] = ModalityID;
            arr[2] = PatientID;
            arr[3] = StrPName;
            arr[4] = studyDtTime;
            arr[5] = InstitutionName;
            arr[6] = AccnNo;
            arr[7] = RefPhys;
            arr[8] = Manufacturer;
            arr[9] = StationName;
            arr[10] = Model;
            arr[11] = ModalityAETitle;
            arr[12] = Reason;
            arr[13] = bDt;
            arr[14] = PatientSex;
            arr[15] = PatientAge;
            arr[16] = PriorityID;
            arr[17] = SeriesUID;
            arr[18] = InstanceNumber;
            arr[19] = SOPInstanceUID;
            return arr;

        }
        #endregion

        #region GetStudyUIDFromDump
        private string GetStudyUIDFromDump(string strFile)
        {

            string[] pathElements = new string[0];
            string strFileName = string.Empty;
            string strProcOutput = string.Empty;
            string strProcError = string.Empty;
            string strProcMsg = string.Empty;
            string strSUID = string.Empty;
            string strSUIDText = string.Empty;
            Process ProcDCMDump = new Process();

            try
            {
                pathElements = strFile.Replace("\\", "/").Split('/');
                strFileName = pathElements[pathElements.Length - 1];

                ProcDCMDump.StartInfo.UseShellExecute = false;
                ProcDCMDump.StartInfo.FileName = strDCMDMPEXEPATH;
                ProcDCMDump.StartInfo.Arguments = "+f " + strFile;
                ProcDCMDump.StartInfo.RedirectStandardOutput = true;
                ProcDCMDump.StartInfo.RedirectStandardError = true;
                ProcDCMDump.Start();
                strProcOutput = ProcDCMDump.StandardOutput.ReadToEnd();
                strProcError = ProcDCMDump.StandardError.ReadToEnd();
                strProcMsg = strProcOutput.Trim();

                strSUIDText = strProcMsg.Substring(strProcMsg.IndexOf("(0020,000d)"), (strProcMsg.IndexOf("StudyInstanceUID") - strProcMsg.IndexOf("(0020,000d)")) + 1);
                strSUID = strSUIDText.Substring(strSUIDText.IndexOf("[") + 1, strSUIDText.IndexOf("]") - (strSUIDText.IndexOf("[") + 1));
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetStudyUIDFromDump()::File - " + strFileName + " Getting Study UID...Exception : " + ex.Message, true);
            }
            finally
            {
                ProcDCMDump.Close();
                ProcDCMDump.Dispose();
                ProcDCMDump = null;
            }

            return strSUID;

        }
        #endregion

        #region IsInteger
        protected bool IsInteger(String integerValue)
        {
            Decimal Temp;
            if (Decimal.TryParse(integerValue, out Temp) == true)
                return true;
            else
                return false;
        }
        #endregion

        #region TransferFileToPacs
        private bool TransferFileToPacs(string strFolder, string strFile, ref string strRetMsg)
        {
            bool bRet = false;
            string strProcOutput = string.Empty;
            string strProcError = string.Empty;
            string strProcMsg = string.Empty;

            try
            {
                #region NORMAL
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File sending started with " + strXFEREXEPATH + " " + strXFEREXEPARMS + " " + strFolder + "\\" + strFile, false);
                Process ProcXfer = new Process();
                ProcXfer.StartInfo.UseShellExecute = false;
                ProcXfer.StartInfo.FileName = strXFEREXEPATH;
                ProcXfer.StartInfo.Arguments = strXFEREXEPARMS + " " + strFile; //strFolder + "\\" + strFile;
                ProcXfer.StartInfo.RedirectStandardOutput = true;
                ProcXfer.StartInfo.RedirectStandardError = true;
                ProcXfer.Start();
                strProcOutput = ProcXfer.StandardOutput.ReadToEnd();
                strProcError = ProcXfer.StandardError.ReadToEnd();
                strProcMsg = strProcOutput.Trim();

                //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "TransferFileToPacs():Message: " + strProcMsg, false);
                // if (strProcMsg.ToUpper().Contains("RECEIVED STORE RESPONSE (SUCCESS)"))

                if ((strProcMsg.ToUpper().Contains("[STATUS=SUCCESS]")) || (strProcMsg.ToUpper().Contains("RECEIVED STORE RESPONSE (SUCCESS)")))
                {
                    strRetMsg = strProcMsg;
                    bRet = true;
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File sending failed with " + strXFEREXEPATH + " " + strXFEREXEPARMS + " " + strFolder + "\\" + strFile, true);
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File sending started with " + strXFEREXEPATH + " " + strXFEREXEPARMSJPGLL + " " + strFolder + "\\" + strFile, false);
                    bRet = false;
                }
                #endregion

                #region JPG Lossless
                if (bRet == false)
                {
                    Process ProcXferAlt = new Process();
                    ProcXferAlt.StartInfo.UseShellExecute = false;
                    ProcXferAlt.StartInfo.FileName = strXFEREXEPATH;
                    ProcXferAlt.StartInfo.Arguments = strXFEREXEPARMSJPGLL + " " + strFolder + "\\" + strFile;
                    ProcXferAlt.StartInfo.RedirectStandardOutput = true;
                    ProcXferAlt.StartInfo.RedirectStandardError = true;
                    ProcXferAlt.Start();
                    strProcOutput = ProcXferAlt.StandardOutput.ReadToEnd();
                    strProcError = ProcXferAlt.StandardError.ReadToEnd();
                    strProcMsg = strProcOutput.Trim();


                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "TransferFileToPacs():Message: " + strProcMsg, false);
                    if ((strProcMsg.ToUpper().Contains("[STATUS=SUCCESS]")) || (strProcMsg.ToUpper().Contains("RECEIVED STORE RESPONSE (SUCCESS)")))
                    {
                        strRetMsg = strProcMsg;
                        bRet = true;
                    }
                    else
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File sending failed with " + strXFEREXEPATH + " " + strXFEREXEPARMSJPGLL + " " + strFolder + "\\" + strFile, true);
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File sending started with " + strXFEREXEPATH + " " + strXFEREXEPARMJ2KLL + " " + strFolder + "\\" + strFile, false);
                        bRet = false;
                    }
                }
                #endregion

                #region JPG 2K Lossless
                if (bRet == false)
                {
                    Process ProcXferJ2k = new Process();
                    ProcXferJ2k.StartInfo.UseShellExecute = false;
                    ProcXferJ2k.StartInfo.FileName = strXFEREXEPATH;
                    ProcXferJ2k.StartInfo.Arguments = strXFEREXEPARMJ2KLL + " " + strFolder + "\\" + strFile;
                    ProcXferJ2k.StartInfo.RedirectStandardOutput = true;
                    ProcXferJ2k.StartInfo.RedirectStandardError = true;
                    ProcXferJ2k.Start();
                    strProcOutput = ProcXferJ2k.StandardOutput.ReadToEnd();
                    strProcError = ProcXferJ2k.StandardError.ReadToEnd();
                    strProcMsg = strProcOutput.Trim();

                    if ((strProcMsg.ToUpper().Contains("[STATUS=SUCCESS]")) || (strProcMsg.ToUpper().Contains("RECEIVED STORE RESPONSE (SUCCESS)")))
                    {
                        strRetMsg = strProcMsg;
                        bRet = true;
                    }
                    else
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File sending failed with " + strXFEREXEPATH + " " + strXFEREXEPARMJ2KLL + " " + strFolder + "\\" + strFile, true);
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File sending started with " + strXFEREXEPATH + " " + strXFEREXEPARMJ2KLS + " " + strFolder + "\\" + strFile, false);
                        bRet = false;
                    }
                }
                #endregion

                #region JPG 2K Lossy
                if (bRet == false)
                {
                    Process ProcXferJ2kL = new Process();
                    ProcXferJ2kL.StartInfo.UseShellExecute = false;
                    ProcXferJ2kL.StartInfo.FileName = strXFEREXEPATH;
                    ProcXferJ2kL.StartInfo.Arguments = strXFEREXEPARMJ2KLS + " " + strFolder + "\\" + strFile;
                    ProcXferJ2kL.StartInfo.RedirectStandardOutput = true;
                    ProcXferJ2kL.StartInfo.RedirectStandardError = true;
                    ProcXferJ2kL.Start();
                    strProcOutput = ProcXferJ2kL.StandardOutput.ReadToEnd();
                    strProcError = ProcXferJ2kL.StandardError.ReadToEnd();
                    strProcMsg = strProcOutput.Trim();

                    if ((strProcMsg.ToUpper().Contains("[STATUS=SUCCESS]")) || (strProcMsg.ToUpper().Contains("RECEIVED STORE RESPONSE (SUCCESS)")))
                    {
                        strRetMsg = strProcMsg;
                        bRet = true;
                    }
                    else
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File sending failed with " + strXFEREXEPATH + " " + strXFEREXEPARMJ2KLS + " " + strFolder + "\\" + strFile, true);
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File sending started with " + strXFEREXEPATHALT + " " + strXFEREXEPARMSSENDDCM + " " + strFolder + "\\" + strFile, false);
                        strRetMsg = strProcMsg;
                        bRet = false;
                    }
                }
                #endregion

                #region DCM Send
                if (bRet == false)
                {
                    Process ProcXferDCMSend = new Process();
                    ProcXferDCMSend.StartInfo.UseShellExecute = false;
                    ProcXferDCMSend.StartInfo.FileName = strXFEREXEPATHALT;
                    ProcXferDCMSend.StartInfo.Arguments = strXFEREXEPARMSSENDDCM + " " + strFolder + "\\" + strFile;
                    ProcXferDCMSend.StartInfo.RedirectStandardOutput = true;
                    ProcXferDCMSend.StartInfo.RedirectStandardError = true;
                    ProcXferDCMSend.Start();
                    strProcOutput = ProcXferDCMSend.StandardOutput.ReadToEnd();
                    strProcError = ProcXferDCMSend.StandardError.ReadToEnd();
                    strProcMsg = strProcOutput.Trim();

                    if ((strProcMsg.ToUpper().Contains("[STATUS=SUCCESS]")) || (strProcMsg.ToUpper().Contains("RECEIVED STORE RESPONSE (SUCCESS)")) || (strProcMsg.ToUpper().Contains("I:   * WITH STATUS SUCCESS  : 1")))
                    {
                        strRetMsg = strProcMsg;
                        bRet = true;
                    }
                    else
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File sending failed with " + strXFEREXEPATHALT + " " + strXFEREXEPARMSSENDDCM + " " + strFolder + "\\" + strFile, true);
                        strRetMsg = strProcMsg;

                        if (strRetMsg.Contains("Invalid SOP Class"))
                        {
                            Process ProcXferModifySOP = new Process();
                            ProcXferModifySOP.StartInfo.UseShellExecute = false;
                            ProcXferModifySOP.StartInfo.FileName = strDCMMODIFYEXEPATH;
                            ProcXferModifySOP.StartInfo.Arguments = "-gin" + " " + strFolder + "\\" + strFile;
                            ProcXferModifySOP.StartInfo.RedirectStandardOutput = true;
                            ProcXferModifySOP.StartInfo.RedirectStandardError = true;
                            ProcXferModifySOP.Start();
                            strProcOutput = ProcXferModifySOP.StandardOutput.ReadToEnd();
                            strProcError = ProcXferModifySOP.StandardError.ReadToEnd();
                            strProcMsg = strProcOutput.Trim();
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "SOP class modified with " + strDCMMODIFYEXEPATH + " -gin " + strFolder + "\\" + strFile, true);
                        }

                        bRet = false;
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                bRet = false;
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>SendFileToPACS()=>TransferFilesToPacs():: Exception: " + ex.Message, true);
            }

            return bRet;
        }
        #endregion
    }


}
