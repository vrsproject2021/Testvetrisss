using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Diagnostics;
using DICOMLib;

namespace VETRISScheduler.Core
{
    public class FileDistribution
    {
        #region Constructor
        public FileDistribution()
        {
        }
        #endregion

        #region Variables
        string strDCMDMPEXEPATH = string.Empty;
        string strDCMLSNFLDR = string.Empty;
        string strFILESHOLDPATH = string.Empty;
        string strPACSARCHIVEFLDR = string.Empty;
        #endregion

        #region Properties
        public string DICOM_LISTENER_FOLDER
        {
            get { return strDCMLSNFLDR; }
            set { strDCMLSNFLDR = value; }
        }
        public string FILES_ON_HOLD_FOLDER
        {
            get { return strFILESHOLDPATH; }
            set { strFILESHOLDPATH = value; }
        }
        public string PACS_ARCHIVE_FOLDER
        {
            get { return strPACSARCHIVEFLDR; }
            set { strPACSARCHIVEFLDR = value; }
        }
        public string DICOM_TAG_DUMP_EXE_PATH
        {
            get { return strDCMDMPEXEPATH; }
            set { strDCMDMPEXEPATH = value; }
        }
        #endregion


        #region DICOM File Methods

        #region GetStudyUIDFromDump
        private string GetStudyUIDFromDump(string strConfigPath, int intServiceID, string strSvcName, string strFile)
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

        #region GetSeriesUIDFromDump
        private string GetSeriesUIDFromDump(string strConfigPath, int intServiceID, string strSvcName, string strFile)
        {

            string[] pathElements = new string[0];
            string strFileName = string.Empty;
            string strProcOutput = string.Empty;
            string strProcError = string.Empty;
            string strProcMsg = string.Empty;
            string strSOPInstanceUID = string.Empty;
            string strSOPInstanceUIDText = string.Empty;
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

                strSOPInstanceUIDText = strProcMsg.Substring(strProcMsg.IndexOf("(0020,000e)"), (strProcMsg.IndexOf("SeriesInstanceUID") - strProcMsg.IndexOf("(0020,000e)")) + 1);
                strSOPInstanceUID = strSOPInstanceUIDText.Substring(strSOPInstanceUIDText.IndexOf("[") + 1, strSOPInstanceUIDText.IndexOf("]") - (strSOPInstanceUIDText.IndexOf("[") + 1));
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetStudyUIDFromDump()::File - " + strFileName + " Getting Series UID...Exception : " + ex.Message, true);
            }
            finally
            {
                ProcDCMDump.Close();
                ProcDCMDump.Dispose();
                ProcDCMDump = null;
            }

            return strSOPInstanceUID;

        }
        #endregion

        #region GetSOPInstanceUIDFromDump
        private string GetSOPInstanceUIDFromDump(string strConfigPath, int intServiceID, string strSvcName, string strFile)
        {

            string[] pathElements = new string[0];
            string strFileName = string.Empty;
            string strProcOutput = string.Empty;
            string strProcError = string.Empty;
            string strProcMsg = string.Empty;
            string strSOPInstanceUID = string.Empty;
            string strSOPInstanceUIDText = string.Empty;
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

                strSOPInstanceUIDText = strProcMsg.Substring(strProcMsg.IndexOf("(0008,0018)"), (strProcMsg.IndexOf("1 SOPInstanceUID") - strProcMsg.IndexOf("(0008,0018)")) + 1);
                strSOPInstanceUID = strSOPInstanceUIDText.Substring(strSOPInstanceUIDText.IndexOf("[") + 1, strSOPInstanceUIDText.IndexOf("]") - (strSOPInstanceUIDText.IndexOf("[") + 1));
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetStudyUIDFromDump()::File - " + strFileName + " Getting SOP Instance UID...Exception : " + ex.Message, true);
            }
            finally
            {
                ProcDCMDump.Close();
                ProcDCMDump.Dispose();
                ProcDCMDump = null;
            }

            return strSOPInstanceUID;

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

        #endregion

        #region UpdateDownloadedListenerFileRecords
        public bool UpdateDownloadedListenerFileRecords(string strConfigPath, int intServiceID, string strSvcName, string strFileName, string strSessionID)
        {
            bool bRet = false;
            string strCatchMessage = string.Empty;
            string strRetMessage = string.Empty;
            string strFilePath = string.Empty;
            string strSID = string.Empty;
            string strIsManual = string.Empty;
            string strNewFilename = string.Empty;
            string strNewFilePath = string.Empty;
            string strPrefix = string.Empty;
            string strExtn = string.Empty;

            string[] arrFolders = new string[0];
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string[] arr = new string[0];
            string strDelFile = string.Empty;
            DicomDecoder dd = new DicomDecoder();

            #region TAG Variables
            string strSUID = string.Empty;
            string strSOPInstanceUID = string.Empty;
            string strSeriesUID = string.Empty;
            DateTime dtStudy = DateTime.Now;
            string strInstCode = string.Empty;
            string strInstName = string.Empty;
            string strPatientID = string.Empty;
            string strPatientName = string.Empty;
            string strPatientFname = string.Empty;
            string strPatientLname = string.Empty;
            string strDt = "0000-00-00";
            string strModalityID = string.Empty;
            string strAccnNo = string.Empty;
            string strRefPhys = string.Empty;
            string strManufacturer = string.Empty;
            string strStationName = string.Empty;
            string strModel = string.Empty;
            string strModalityAETitle = string.Empty;
            string strReason = string.Empty;
            DateTime dtDOB = DateTime.Today;
            string strBdt = string.Empty;
            string strPatientSex = string.Empty;
            string strPatientAge = string.Empty;
            int intPriorityID = 0;

            string[] arrDt = new string[0];
            string[] arrTime = new string[0];
            string[] arrDateTime = new string[0];
            #endregion

            Scheduler objCore = new Scheduler();
            FTPPACSSynch objFP = new FTPPACSSynch();

            try
            {

                strFilePath = strDCMLSNFLDR + "\\" + strFileName;
                strSID = strSessionID;
                strIsManual = "N";

                dd.DicomFileName = strFilePath;
                List<string> str = dd.dicomInfo;

                arr = new string[20];
                arr = GetallTags(str);
                strSUID = arr[0].Trim();
                if (strSUID.Trim() == string.Empty) strSUID = GetStudyUIDFromDump(strConfigPath, intServiceID, strSvcName, strFilePath);

                if (strSUID.Trim() != string.Empty)
                {
                    bRet = true;
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedListenerFileRecords()=>File : " + strFileName.Trim() + " get tag data ", false);

                    try
                    {
                        #region Get File Data
                        strModalityID = arr[1].Trim();
                        strPatientID = arr[2].Trim();
                        strPatientName = arr[3].Trim();

                        if (strPatientName != string.Empty)
                        {
                            if (strPatientName.Contains(' '))
                            {
                                strPatientFname = strPatientName.Substring(0, strPatientName.LastIndexOf(' '));
                                strPatientLname = strPatientName.Substring(strPatientName.LastIndexOf(' '), (strPatientName.Length - strPatientName.LastIndexOf(' ')));
                            }
                            else
                            {
                                strPatientFname = strPatientName;
                                strPatientLname = string.Empty;
                            }
                        }

                        strInstName = arr[5].Trim();
                        //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File : " + strFileName.Trim() + " Institution Name ", false);

                        strAccnNo = arr[6].Trim();
                        strRefPhys = arr[7].Trim();
                        strManufacturer = arr[8].Trim();
                        strStationName = arr[9].Trim();
                        strModel = arr[10].Trim();
                        strModalityAETitle = arr[11].Trim();
                        strReason = arr[12].Trim();

                        strPatientSex = arr[14].Trim();
                        strPatientAge = arr[15].Trim();

                        strSeriesUID = arr[17].Trim();
                        if (strSeriesUID == string.Empty) strSeriesUID = GetSeriesUIDFromDump(strConfigPath, intServiceID, strSvcName, strFilePath);
                        strSOPInstanceUID = arr[19].Trim();
                        if (strSOPInstanceUID == string.Empty) strSOPInstanceUID = GetSOPInstanceUIDFromDump(strConfigPath, intServiceID, strSvcName, strFilePath);


                        if (arr[16].Trim() != string.Empty) intPriorityID = Convert.ToInt32(arr[16].Trim());

                        strDt = arr[4].Trim();
                        if ((arr[4].Trim() == "0000-00-00") || (arr[4].Trim() == string.Empty))
                        {
                            dtStudy = DateTime.Now;
                        }
                        else
                        {
                            arrDateTime = arr[4].Trim().Split(' ');
                            arrDt = arrDateTime[0].Split('-');
                            arrTime = arrDateTime[1].Split(':');

                            dtStudy = new DateTime(Convert.ToInt32(arrDt[0]),
                                                    Convert.ToInt32(arrDt[1]),
                                                    Convert.ToInt32(arrDt[2]),
                                                    Convert.ToInt32(arrTime[0]),
                                                    Convert.ToInt32(arrTime[1]),
                                                    Convert.ToInt32(arrTime[2]));
                        }



                        strBdt = arr[13].Trim();
                        if ((arr[13].Trim() == "0000-00-00") || (arr[13].Trim() == string.Empty)) dtStudy = DateTime.Now;
                        else
                        {
                            arrDt = arr[13].Split('-');
                            dtDOB = new DateTime(Convert.ToInt32(arrDt[0]),
                                                 Convert.ToInt32(arrDt[1]),
                                                 Convert.ToInt32(arrDt[2]),
                                                 0, 0, 0);
                        }



                        #endregion
                    }
                    catch (Exception ex)
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedListenerFileRecords()=>File : " + strFileName.Trim() + " get tag data  :: " + ex.Message.Trim(), true);
                    }

                    if (strInstName.Trim().ToUpper() != string.Empty && strInstName.Trim().ToUpper() != "Y")
                    {
                        #region check institution info
                        objCore.INSTITUTION_NAME = strInstName.Trim();
                        if (!objCore.FetchInstitutionInfo(strConfigPath, ref strCatchMessage))
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedListenerFileRecords()=>FetchInstitutionInfo():Core::Exception - " + strCatchMessage, true);
                        }
                        #endregion

                        if (objCore.INSTITUTION_CODE.Trim() == string.Empty)
                        {
                            #region Put the files on hold and create new institution
                            if (File.Exists(strFILESHOLDPATH + "/" + strFileName)) File.Delete(strFILESHOLDPATH + "/" + strFileName);
                            File.Move(strFilePath, strFILESHOLDPATH + "/" + strFileName);
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedListenerFileRecords()::File - " + strFileName + " put on hold. Site code not found", false);

                            if (objCore.INSTITUTION_ID == new Guid("00000000-0000-0000-0000-000000000000"))
                            {
                                #region create new institution
                                objCore.INSTITUTION_NAME = arr[5].Trim();
                                if (objCore.CreateNewInstitution(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                                {
                                    #region create notification
                                    if (!objCore.CreateNewInstitutionNotification(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                                    {
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "CreateNewInstitutionNotification():Core::Exception - " + strCatchMessage, true);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "CreateNewInstitution():Core::Exception - " + strCatchMessage, true);
                                }
                                #endregion
                            }
                            #endregion
                        }
                        else
                        {

                            #region save the file after renaming
                            strPrefix = CoreCommon.RandomString(6);
                            strExtn = Path.GetExtension(strFilePath);
                            strInstCode = objCore.INSTITUTION_CODE.Trim();
                            strInstName = objCore.INSTITUTION_NAME.Trim();
                            if (strExtn.Trim() != string.Empty)
                                strNewFilename = objCore.INSTITUTION_CODE.Trim() + "_" + strSessionID + "_" + objCore.INSTITUTION_NAME.Trim().Replace(" ", "_") + "_" + strPrefix + "_" + strSUID.Replace(".", "").Substring(0, 5) + strExtn;
                            else
                                strNewFilename = objCore.INSTITUTION_CODE.Trim() + "_" + strSessionID + "_" + objCore.INSTITUTION_NAME.Trim().Replace(" ", "_") + "_" + strPrefix + "_" + strSUID.Replace(".", "").Substring(0, 5);

                            strNewFilename = strNewFilename.Replace(" ", "_");
                            strNewFilename = strNewFilename.Replace(",", "");
                            strNewFilename = strNewFilename.Replace("(", "");
                            strNewFilename = strNewFilename.Replace(")", "");
                            strNewFilename = strNewFilename.Replace("'", "");
                            strNewFilename = strNewFilename.Replace("\"", "_");
                            strNewFilename = strNewFilename.Replace("/", "_");
                            strNewFilename = strNewFilename.Replace("\\", "_");
                            strNewFilename = strNewFilename.Replace("#", "");
                            strNewFilename = strNewFilename.Replace("&", "");
                            strNewFilename = strNewFilename.Replace("@", "");
                            strNewFilename = strNewFilename.Replace("?", "");
                            strNewFilename = strNewFilename.Replace("__", "_");
                            strNewFilePath = strDCMLSNFLDR + "/" + strNewFilename;

                            if (File.Exists(strFilePath))
                            {

                                File.Move(strFilePath, strNewFilePath);
                                if (File.Exists(strFilePath)) File.Delete(strFilePath);
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedListenerFileRecords()=>File : " + strFileName + " renamed to " + strNewFilename, false);
                            }
                            #endregion

                            if (strSeriesUID.Trim() != string.Empty && strSOPInstanceUID.Trim() != string.Empty)
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedListenerFileRecords()=>File : " + strNewFilename.Trim() + " Update DB ", false);

                                #region Update DB
                                try
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedListenerFileRecords()=>File : " + strNewFilename.Trim() + "- SUID : " + strSUID.Trim(), false);
                                    objFP.STUDY_UID = strSUID.Trim();
                                    objFP.SERIES_UID = strSeriesUID.Trim();
                                    objFP.SOP_INSTANCE_UID = strSOPInstanceUID.Trim();
                                    objFP.STUDY_DATE = dtStudy;
                                    objFP.INSTITUTION_CODE = strInstCode;
                                    objFP.INSTITUTION_NAME = strInstName.Trim();
                                    objFP.PATIENT_ID = strPatientID.Trim();
                                    objFP.PATIENT_FIRST_NAME = strPatientFname.Trim();
                                    objFP.PATIENT_LAST_NAME = strPatientLname.Trim();
                                    objFP.FILE_NAME = strNewFilename;
                                    objFP.ACCESSION_NUMBER = strAccnNo.Trim();
                                    objFP.MODALITY = strModalityID;
                                    objFP.REFERRING_PHYSICIAN = strRefPhys.Trim();
                                    objFP.MANUFACTURER = strManufacturer.Trim();
                                    objFP.STATION_NAME = strStationName.Trim();
                                    objFP.MODEL = strModel.Trim();
                                    objFP.MODALITY_AE_TITLE = strModalityAETitle.Trim();
                                    objFP.REASON = strReason.Trim();
                                    objFP.DATE_OF_BIRTH = dtDOB;
                                    objFP.PATIENT_SEX = strPatientSex.Trim();
                                    objFP.PATIENT_AGE = strPatientAge.Trim();
                                    objFP.PRIORITY_ID = intPriorityID;
                                    objFP.IMPORT_SESSION_ID = strSID.Trim();
                                    objFP.IS_MANUAL_UPLOAD = strIsManual;
                                    objFP.SYNCED_BY_FILE_DISTRIBUTION_SERVICE = "Y";

                                    #region debug log
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of file " + strFileName, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Study UID : " + objFP.STUDY_UID, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Study Date : " + objFP.STUDY_DATE.ToString(), false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Inst Code : " + objFP.INSTITUTION_CODE, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Inst Name : " + objFP.INSTITUTION_NAME, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Patient ID : " + objFP.PATIENT_ID, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " P FNAME : " + objFP.PATIENT_FIRST_NAME, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " P LNAME : " + objFP.PATIENT_LAST_NAME, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Accn No : " + objFP.ACCESSION_NUMBER, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Modality : " + objFP.MODALITY, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Ref Phys : " + objFP.REFERRING_PHYSICIAN, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Manufacturer : " + objFP.MANUFACTURER, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Model : " + objFP.MODEL, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Modality AE Title : " + objFP.MODALITY_AE_TITLE, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Reason : " + objFP.REASON, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " DOB : " + objFP.DATE_OF_BIRTH.ToString(), false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " P Sex : " + objFP.PATIENT_SEX, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " P Age : " + objFP.PATIENT_AGE, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Priotity ID : " + objFP.PRIORITY_ID.ToString(), false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Session ID : " + objFP.IMPORT_SESSION_ID, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Is Manual : " + objFP.IS_MANUAL_UPLOAD, false);
                                    #endregion


                                    if (!objFP.SaveListenerFileData(strConfigPath, strSvcName, ref strRetMessage, ref strCatchMessage, ref strDelFile))
                                    {
                                        if (strCatchMessage.Trim() != string.Empty)
                                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedListenerFileRecords()=>SaveListenerFileData()::Error:Exception: " + strCatchMessage.Trim(), true);
                                        else
                                        {
                                            #region delete file if duplicate
                                            if (strDelFile == "Y")
                                            {
                                                if (File.Exists(strNewFilePath))
                                                {
                                                    File.Delete(strNewFilePath);
                                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedListenerFileRecords():: Deleted file " + strNewFilename + ", as study already has this file saved", true);
                                                }
                                                else
                                                {
                                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedListenerFileRecords()=>SaveListenerFileData()::Error: " + strRetMessage.Trim(), true);
                                                }
                                            }
                                            #endregion

                                        }
                                    }
                                    else
                                    {
                                        if (strDelFile == "Y")
                                        {
                                            #region delete file if duplicate
                                            if (File.Exists(strNewFilePath))
                                            {
                                                File.Delete(strNewFilePath);
                                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedListenerFileRecords():: Deleted file " + strNewFilename + ", as study already has this file saved", true);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Transfer forwarded file to archive
                                            TransferForwardeFileToArchive(strConfigPath, intServiceID, strSvcName, strInstCode, strInstName.Trim(), strSUID, strNewFilename, strNewFilePath);
                                            #endregion
                                        }

                                    }
                                }
                                catch (Exception expErr)
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedListenerFileRecords()::Exception: " + expErr.Message, true);
                                    bRet = false;
                                }
                                #endregion
                            }
                            else
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedListenerFileRecords()=>File : " + strNewFilename.Trim() + " Rejected. Series UID & SOP Instance UID missing ", true);
                                bRet = false;
                            }
                        }
                    }
                    else
                    {
                        #region put file on hold
                        if (File.Exists(strFILESHOLDPATH + "/" + strFileName)) File.Delete(strFILESHOLDPATH + "/" + strFileName);
                        File.Move(strFilePath, strFILESHOLDPATH + "/" + strFileName);
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedListenerFileRecords()::File - " + strFileName + " put on hold. Institution name missing", false);
                        #endregion
                    }


                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedListenerFileRecords()=>File : " + strFileName.Trim() + " Study UID missing ", true);
                    bRet = false;
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedFilesRecords():: DataUpdate - Exception: " + expErr.Message, true);
                bRet = false;
            }

            finally
            {
                objFP = null; dd = null; objCore = null;
            }
            return bRet;
        }
        #endregion

        #region TransferForwardeFileToArchive
        private bool TransferForwardeFileToArchive(string strConfigPath, int intServiceID, string strSvcName,
                                                   string InstitutionCode, string InstitutionName, string StudyUID, string FileName, string FilePathToMove)
        {
            string strFolder = InstitutionCode + "_" + InstitutionName + "_" + StudyUID;
            string strRetMessage = string.Empty;
            string strCatchMessage = string.Empty;

            bool bRet = true;
            FTPPACSSynch objFPArch = new FTPPACSSynch();

            try
            {
                if (!System.IO.Directory.Exists(strPACSARCHIVEFLDR + "\\" + strFolder))
                {
                    System.IO.Directory.CreateDirectory(strPACSARCHIVEFLDR + "\\" + strFolder);
                }
                if (File.Exists(strPACSARCHIVEFLDR + "\\" + strFolder + "\\" + FileName))
                {
                    System.IO.File.Delete(strPACSARCHIVEFLDR + "\\" + strFolder + "\\" + FileName);
                }

                System.IO.File.Move(FilePathToMove, strPACSARCHIVEFLDR + "\\" + strFolder + "\\" + FileName);
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, FileName + " archived", false);


                objFPArch.STUDY_UID = StudyUID;
                if (!objFPArch.UpdateArchivedFileCount(strConfigPath, ref strRetMessage, ref strCatchMessage))
                {
                    if (strCatchMessage.Trim() != string.Empty)
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>TransferForwardeFileToArchive()=>UpdateArchivedFileCount()- File :" + FileName + "::Exception: " + strCatchMessage.Trim(), true);
                    else
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>TransferForwardeFileToArchive()=>UpdateArchivedFileCount()- File :" + FileName + "::Error: " + strRetMessage.Trim(), true);
                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>TransferForwardeFileToArchive()- File :" + FileName + ":: Exception: " + ex.Message, true);
                bRet = false;
            }
            finally
            {
                objFPArch = null;
            }

            return bRet;
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
    }
}
