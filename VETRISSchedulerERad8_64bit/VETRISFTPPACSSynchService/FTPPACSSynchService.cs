using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Configuration;
using System.Security;
using System.IO.Compression;
using System.Runtime.InteropServices;
using VETRISScheduler.Core;


namespace VETRISFTPPACSSynchService
{
    public partial class FTPPACSSynchService : ServiceBase
    {
        #region members & variables
        private static int intFreq = 10;
        private static int intServiceID = 7;

        private static string strFTPHOST = string.Empty;
        private static int intFTPPORT = 0;
        private static string strFTPUSER = string.Empty;
        private static string strFTPPWD = string.Empty;
        private static string strFTPFLDR = string.Empty;
        private static string strFTPDLFLDRTMP = string.Empty;
        private static string strPACSXFERDLFLDR = string.Empty;
        private static string strXFEREXEPATH = string.Empty;
        private static string strXFEREXEPATHALT = string.Empty;
        private static string strXFEREXEPARMS = string.Empty;
        private static string strXFEREXEPARMSJPGLL = string.Empty;
        private static string strXFEREXEPARMJ2KLL = string.Empty;
        private static string strXFEREXEPARMJ2KLS = string.Empty;
        private static string strXFEREXEPARMSSENDDCM = string.Empty;
        private static string strPACSARCHIVEFLDR = string.Empty;
        private static string strWRITEBACKURL = string.Empty;
        private static string strIMG2DCMEXEPATH = string.Empty;
        private static string strFTPLOGFLDR = string.Empty;
        private static string strFTPLOGDLFLDRTMP = string.Empty;
        private static string strFTPDLMODE = string.Empty;
        private static string strFTPSRCFOLDER = string.Empty;
        private static string strDCMMODIFYEXEPATH = string.Empty;
        private static string strDCMDMPEXEPATH = string.Empty;
        private static string strDCMRCVRFLDR = string.Empty;
        private static string strFILESHOLDPATH = string.Empty;
        private static string strSENDLFTOPACS = string.Empty;
        private static string strLFPATH = string.Empty;

        private static string strConfigPath = AppDomain.CurrentDomain.BaseDirectory;
        private static string strSvcName = "VETRIS FTP & PACS Synch Service";

        string strErrRegion = string.Empty;
        const int ERROR_SHARING_VIOLATION = 32;
        const int ERROR_LOCK_VIOLATION = 33;

        Scheduler objCore;
        FTPPACSSynch objFP;
        #endregion

        public FTPPACSSynchService()
        {
            InitializeComponent();


        }


        #region OnStart
        protected override void OnStart(string[] args)
        {

            try
            {
                GetServiceDetails();

                System.Threading.ThreadStart job_download_file = new System.Threading.ThreadStart(doProcessDownload);
                System.Threading.Thread threadDownload = new System.Threading.Thread(job_download_file);

                System.Threading.ThreadStart job_forwarded_files = new System.Threading.ThreadStart(doProcessForwardedFiles);
                System.Threading.Thread threadForwardedFiles = new System.Threading.Thread(job_forwarded_files);

                System.Threading.ThreadStart job_decompress = new System.Threading.ThreadStart(doDecompressFiles);
                System.Threading.Thread threadDecompress = new System.Threading.Thread(job_decompress);

                System.Threading.ThreadStart job_upload_files = new System.Threading.ThreadStart(doUploadFiles);
                System.Threading.Thread threadUploadFile = new System.Threading.Thread(job_upload_files);


                threadDownload.Start();
                threadForwardedFiles.Start();
                threadDecompress.Start();
                threadUploadFile.Start();


                //System.Threading.ThreadStart job_xfer = new System.Threading.ThreadStart(doSendFileToPACS);
                //System.Threading.Thread threadXfer = new System.Threading.Thread(job_xfer);
                //threadXfer.Start();

                //System.Threading.ThreadStart job_xfer_img = new System.Threading.ThreadStart(doProcessDownload);
                //System.Threading.Thread threadXferImg = new System.Threading.Thread(doSendImgFileToPACS);
                //threadXferImg.Start();

                //System.Threading.ThreadStart job_del = new System.Threading.ThreadStart(doDeleteZipFiles);
                //System.Threading.Thread threadDel = new System.Threading.Thread(job_del);
                //threadDel.Start();

                //System.Threading.ThreadStart job_send_to_pacs = new System.Threading.ThreadStart(doSendFileToPACS);
                //System.Threading.Thread threadPacs = new System.Threading.Thread(job_send_to_pacs);
                //threadPacs.Start();

                //System.Threading.ThreadStart job_wb = new System.Threading.ThreadStart(doWriteBack);
                //System.Threading.Thread threadWB = new System.Threading.Thread(job_wb);
                //threadWB.Start();

                //System.Threading.ThreadStart job_img_send_to_pacs = new System.Threading.ThreadStart(doSendImgFileToPACS);
                //System.Threading.Thread threadImgPacs = new System.Threading.Thread(job_img_send_to_pacs);
                //threadImgPacs.Start();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(strSvcName, "Error Starting Service." + ex.Message, EventLogEntryType.Error);
            }

        }
        #endregion

        #region OnStop
        protected override void OnStop()
        {
            try
            {
                //System.Threading.Thread.Sleep(20000);
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
        private bool GetServiceDetails()
        {
            bool bRet = false;
            string strCatchMessage = string.Empty;
            objCore = new Scheduler();

            try
            {
                objCore.SERVICE_ID = intServiceID;
                if (objCore.GetServiceDetails(strConfigPath, ref strCatchMessage))
                {
                    intFreq = objCore.FREQUENCY;
                    strSvcName = objCore.SERVICE_NAME;

                    strFTPHOST = objCore.FTP_HOST;
                    intFTPPORT = objCore.FTP_PORT_NUMBER;
                    strFTPUSER = objCore.FTP_USER;
                    strFTPPWD = objCore.FTP_PASSWORD;
                    strFTPFLDR = objCore.FTP_FOLDER;
                    strFTPDLFLDRTMP = objCore.TEMPORARY_DOWNLOAD_FOLDER;
                    strFTPLOGFLDR = objCore.FTP_LOG_FOLDER;
                    strFTPLOGDLFLDRTMP = objCore.TEMPORARY_LOG_DOWNLOAD_FOLDER;
                    strFTPDLMODE = objCore.FTP_DOWNLOAD_MODE;
                    strFTPSRCFOLDER = objCore.FTP_SOURCE_FOLDER;
                    strPACSXFERDLFLDR = objCore.FOLDER_FOR_PACS_TRANSFER;
                    strXFEREXEPATH = objCore.PACS_TRANSFER_EXE_PATH;
                    strXFEREXEPATHALT = objCore.PACS_TRANSFER_EXE_ALTERNATE_PATH;
                    strXFEREXEPARMS = objCore.PACS_TRANSFER_EXE_PARAMS;
                    strXFEREXEPARMSJPGLL = objCore.PACS_TRANSFER_EXE_PARAMS_FOR_JPG_LOSSLESS;
                    strXFEREXEPARMJ2KLL = objCore.PACS_TRANSFER_EXE_PARAMS_FOR_JPG2K_LOSSLESS;
                    strXFEREXEPARMJ2KLS = objCore.PACS_TRANSFER_EXE_PARAMS_FOR_JPG2K_LOSSY;
                    strXFEREXEPARMSSENDDCM = objCore.PACS_TRANSFER_EXE_PARAMS_FOR_SEND_DCM;
                    strPACSARCHIVEFLDR = objCore.PACS_ARCHIVE_FOLDER;
                    strWRITEBACKURL = objCore.WRITE_BACK_URL;
                    strIMG2DCMEXEPATH = objCore.IMAGE_TO_DCM_EXE_PATH;
                    strDCMMODIFYEXEPATH = objCore.DICOM_TAG_MODIFY_EXE_PATH;
                    strDCMDMPEXEPATH = objCore.DICOM_TAG_DUMP_EXE_PATH;
                    strDCMRCVRFLDR = objCore.DICOM_RECEIVER_FOLDER;
                    strFILESHOLDPATH = objCore.FILES_ON_HOLD_FOLDER;
                    strSENDLFTOPACS = objCore.SEND_LISTENER_FILES_TO_PACS;
                    strLFPATH = objCore.LISTENER_FILES_PATH_TO_SYNC;
                    bRet = true;
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetServiceDetails() => Core::GetServiceDetails - Error : " + strCatchMessage, true);
                    bRet = false;
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetServiceDetails() - Exception: " + expErr.Message, true);
                bRet = false;
            }
            finally
            {
                objCore = null;
            }
            return bRet;
        }
        #endregion

        #region Download file(s) from ftp and delete from ftp

        #region doProcessDownload
        private void doProcessDownload()
        {
            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Service started Successfully", false);
                while (true)
                {

                    doDownloadFiles();
                    System.Threading.Thread.Sleep(intFreq * 1000);

                }

            }
            catch (Exception expErr)
            {
                EventLog.WriteEntry(strSvcName, "doProcessDownload()=>Exception : " + expErr.Message, EventLogEntryType.Error);
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }


        }
        #endregion

        #region doDownloadFiles
        private void doDownloadFiles()
        {
            string[] arrfiles = new string[0];
            string strfile = string.Empty;

            try
            {
                arrfiles = GetFileList();
                if (arrfiles.Length > 0)
                {
                    foreach (string strFileName in arrfiles)
                    {
                        if ((strFileName.Trim() != ".") && (strFileName.Trim() != ".."))
                        {
                            if (strFTPDLMODE == "D")
                                Download(strFileName);
                            else if (strFTPDLMODE == "C")
                                MoveFile(strFileName);
                        }
                    }
                }

                #region Suspended
                //if (arrfiles.Length > 0)
                //{
                //    if (arrfiles.Length > 250)
                //    {
                //        for (int i = 0; i < 250; i++)
                //        {
                //            strfile = arrfiles[i];
                //            if ((strfile.Trim() != ".") && (strfile.Trim() != ".."))
                //            {
                //                if (strFTPDLMODE == "D")
                //                    Download(strfile);
                //                else if (strFTPDLMODE == "C")
                //                    MoveFile(strfile);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        foreach (string strFileName in arrfiles)
                //        {
                //            if ((strFileName.Trim() != ".") && (strFileName.Trim() != ".."))
                //            {
                //                if (strFTPDLMODE == "D")
                //                    Download(strFileName);
                //                else if (strFTPDLMODE == "C")
                //                    MoveFile(strFileName);
                //            }
                //        }
                //    }

                //}
                #endregion

            }
            catch (Exception expErr)
            {
                EventLog.WriteEntry(strSvcName, "doProcessDownload()=>doDownloadFiles()=>Exception : " + expErr.Message, EventLogEntryType.Error);
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()=>doDownloadFiles() - Exception: " + expErr.Message, true);
            }
        }
        #endregion

        #region GetFileList
        public string[] GetFileList()
        {
            string[] downloadFiles = new string[0];
            StringBuilder result = new StringBuilder();
            WebResponse response = null;
            StreamReader reader = null;
            string line = string.Empty;

            try
            {
                if (strFTPDLMODE == "D")
                {
                    FtpWebRequest reqFTP;
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + strFTPHOST + "/" + strFTPFLDR + "/"));
                    reqFTP.UseBinary = true;
                    reqFTP.Credentials = new NetworkCredential(strFTPUSER, strFTPPWD);
                    reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                    reqFTP.Proxy = null;
                    reqFTP.KeepAlive = true;
                    reqFTP.UsePassive = false;
                    response = reqFTP.GetResponse();
                    reader = new StreamReader(response.GetResponseStream());
                    line = reader.ReadLine();

                    while (line != null)
                    {
                        result.Append(line);
                        result.Append("\n");
                        line = reader.ReadLine();
                    }
                    // to remove the trailing '\n'
                    if (result.Length > 0)
                    {
                        result.Remove(result.ToString().LastIndexOf('\n'), 1);
                        downloadFiles = result.ToString().Split('\n');
                    }
                }
                else
                {
                    downloadFiles = Directory.GetFiles(strFTPSRCFOLDER);
                }

            }
            catch (Exception ex)
            {
                if (strFTPDLMODE == "D")
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (response != null)
                    {
                        response.Close();
                    }
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()=>doDownloadFiles()=>GetFileList() :: Failed to get file list from ftp", false);
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()=>doDownloadFiles()=>GetFileList() :: Exception -" + ex.Message, false);
                }
            }

            return downloadFiles;
        }
        #endregion

        #region Download
        private void Download(string strFileName)
        {
            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()=>doDownloadFiles()=>Download() :: Downloading file " + strFileName, false);
                if (File.Exists(strFTPDLFLDRTMP + "\\" + strFileName)) File.Delete(strFTPDLFLDRTMP + "\\" + strFileName);
                string uri = "ftp://" + strFTPHOST + "/" + strFTPFLDR + "/" + strFileName;
                Uri serverUri = new Uri(uri);
                if (serverUri.Scheme != Uri.UriSchemeFtp)
                {
                    return;
                }
                FtpWebRequest reqFTP;

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + strFTPHOST + "/" + strFTPFLDR + "/" + strFileName));
                reqFTP.Credentials = new NetworkCredential(strFTPUSER, strFTPPWD);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Proxy = null;
                reqFTP.UsePassive = false;

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                if (size > 0)
                {
                    Stream responseStream = response.GetResponseStream();
                    FileStream writeStream = new FileStream(strFTPDLFLDRTMP + "\\" + strFileName, FileMode.Create);
                    int Length = 32 * 1024;
                    Byte[] buffer = new Byte[Length];
                    int bytesRead = responseStream.Read(buffer, 0, Length);
                    while (bytesRead > 0)
                    {
                        writeStream.Write(buffer, 0, bytesRead);
                        bytesRead = responseStream.Read(buffer, 0, Length);
                    }
                    writeStream.Close();
                    response.Close();
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "ftp://" + strFTPHOST + "/" + strFTPFLDR + "/" + strFileName + " downloaded successfully", false);

                    //if (IsZipValid(strFTPDLFLDRTMP + "\\" + strFileName)) 
                    DeleteFtpFile(strFileName);
                }

            }
            catch (IOException ex)
            {
                ;
                //if (IsFileLocked(ex))
                //{
                //    UnlockFileProcess(strFTPDLFLDRTMP + "\\" + strFileName);
                //}
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doDownloadFiles()=> Download() - Exception: " + ex.Message, true);
            }
        }
        #endregion

        #region MoveFile
        private void MoveFile(string strFilePath)
        {
            string[] pathElement = new string[0];
            string strFileName = string.Empty;
            string strSessionID = string.Empty;


            try
            {

                strFilePath = strFilePath.Replace("\\", "/");
                pathElement = strFilePath.Split('/');
                strFileName = pathElement[pathElement.Length - 1];

                if (strFileName.Substring(0, 4) != "RLF_")
                {
                    //strSessionID = (strFileName.Split('_'))[1].Trim();

                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()=>doDownloadFiles()=>MoveFile() :: Moving file " + strFileName, false);
                    if (File.Exists(strFTPDLFLDRTMP + "\\" + strFileName)) File.Delete(strFTPDLFLDRTMP + "\\" + strFileName);
                    File.Move(strFilePath, strFTPDLFLDRTMP + "\\" + strFileName);
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, strFileName + " Moved successfully to " + strFTPDLFLDRTMP, false);

                    #region Suspended
                    //if (MIMEAssistant.GetMIMEType(strFTPDLFLDRTMP + "\\" + strFileName) != "application/zip")
                    //{
                    //    if (CoreCommon.IsDicomFile(strFTPDLFLDRTMP + "\\" + strFileName))
                    //    {

                    //        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()=>doDownloadFiles()=>MoveFile() :: Updating listener file details. File :: " + strFileName, false);
                    //        UpdateDownloadedListenerFileRecords(strFileName, strSessionID);
                    //    }
                    //    else
                    //    {
                    //        if (File.Exists(strFTPDLFLDRTMP + "\\" + strFileName)) File.Delete(strFTPDLFLDRTMP + "\\" + strFileName);
                    //    }
                    //}
                    #endregion
                }
                else
                {
                    if (File.Exists(strLFPATH + "/" + strFileName)) File.Delete(strLFPATH + "/" + strFileName);
                    File.Move(strFilePath, strLFPATH + "/" + strFileName);
                }


            }
            catch (IOException ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doDownloadFiles()=> Download() => MoveFile() - Exception: " + ex.Message, true);
                //if (IsFileLocked(ex))
                //{
                //    UnlockFileProcess(strFTPDLFLDRTMP + "\\" + strFileName);
                //}
            }
            catch (Exception ex)
            {
                //EventLog.WriteEntry(strSvcName, "doDownloadFiles()=> Download() => MoveFile()=>Exception : " + ex.Message, EventLogEntryType.Error);
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doDownloadFiles()=> Download() => MoveFile() - Exception: " + ex.Message, true);
            }
        }
        #endregion

        #region DeleteFtpFile
        private void DeleteFtpFile(string strFileName)
        {
            string strResponse = string.Empty;

            try
            {

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + strFTPHOST + "/" + strFTPFLDR + "/" + strFileName);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(strFTPUSER, strFTPPWD);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    strResponse = response.StatusDescription;
                }

                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "ftp Response : " + strResponse, false);

            }

            catch (Exception ex)
            {

                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doDownloadFiles()=>Download()=>DeleteFtpFile() - Exception: " + ex.Message, true);
            }
        }
        #endregion

        #endregion

        #region Process Forwarded Files

        #region doProcessForwardedFiles
        private void doProcessForwardedFiles()
        {
            try
            {

                while (true)
                {
                    ProcessForwardedFiles();
                    System.Threading.Thread.Sleep(intFreq * 1000);
                }

            }
            catch (Exception expErr)
            {
                //EventLog.WriteEntry(strSvcName, "doProcessForwardedFiles()=>Exception : " + expErr.Message, EventLogEntryType.Error);
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }


        }
        #endregion

        #region ProcessForwardedFiles
        private void ProcessForwardedFiles()
        {
            string[] arrfiles = new string[0];
            string[] pathElements = new string[0];
            string strFileName = string.Empty;
            string strSessionID = string.Empty;

            try
            {
                arrfiles = Directory.GetFiles(strFTPDLFLDRTMP, "*_S1DXXX*.*");
                if (arrfiles.Length > 0)
                {
                    foreach (string strFile in arrfiles)
                    {
                        if (CoreCommon.IsDicomFile(strFile))
                        {
                            pathElements = strFile.Split('\\');
                            strFileName = pathElements[(pathElements.Length - 1)];
                            strSessionID = (strFileName.Split('_'))[1].Trim();
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles() :: Updating listener file details. File :: " + strFileName, false);
                            UpdateDownloadedListenerFileRecords(strFileName, strSessionID);
                        }
                        else
                        {
                            if (File.Exists(strFTPDLFLDRTMP + "\\" + strFileName)) File.Delete(strFTPDLFLDRTMP + "\\" + strFileName);
                        }
                    }
                }
            }
            catch (Exception expErr)
            {
                //EventLog.WriteEntry(strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>Exception : " + expErr.Message, EventLogEntryType.Error);
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles() - Exception: " + expErr.Message, true);
            }
        }
        #endregion

        #region UpdateDownloadedListenerFileRecords
        private void UpdateDownloadedListenerFileRecords(string strFileName, string strSessionID)
        {
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

            Scheduler objCore1 = new Scheduler();
            FTPPACSSynch objFP1 = new FTPPACSSynch();

            try
            {

                strFilePath = strFTPDLFLDRTMP + "\\" + strFileName;
                //strInstCode = (strFileName.Split('_'))[0].Trim();
                strSID = (strFileName.Split('_'))[1].Trim();
                if (strSID.Substring(0, 1) == "M") strIsManual = "Y";
                else strIsManual = "N";

                dd.DicomFileName = strFilePath;
                List<string> str = dd.dicomInfo;

                arr = new string[20];
                arr = GetallTags(str);
                strSUID = arr[0].Trim();
                if (strSUID.Trim() == string.Empty) strSUID = GetStudyUIDFromDump(strFilePath);

                if (strSUID.Trim() != string.Empty)
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File : " + strFileName.Trim() + " get tag data ", false);

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
                        if (strSeriesUID == string.Empty) strSeriesUID = GetSeriesUIDFromDump(strFilePath);
                        strSOPInstanceUID = arr[19].Trim();
                        if (strSOPInstanceUID == string.Empty) strSOPInstanceUID = GetSOPInstanceUIDFromDump(strFilePath);


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
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File : " + strFileName.Trim() + " get tag data  :: " + ex.Message.Trim(), true);
                    }

                    if (strInstName.Trim().ToUpper() != string.Empty && strInstName.Trim().ToUpper() != "Y")
                    {
                        #region check institution info
                        objCore1.INSTITUTION_NAME = strInstName.Trim();
                        if (!objCore1.FetchInstitutionInfo(strConfigPath, ref strCatchMessage))
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>UpdateDownloadedListenerFileRecords()=>FetchInstitutionInfo():Core::Exception - " + strCatchMessage, true);
                        }
                        #endregion

                        if (objCore1.INSTITUTION_CODE.Trim() == string.Empty)
                        {
                            #region Put the files on hold and create new institution
                            if (File.Exists(strFILESHOLDPATH + "/" + strFileName)) File.Delete(strFILESHOLDPATH + "/" + strFileName);
                            File.Move(strFilePath, strFILESHOLDPATH + "/" + strFileName);
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>UpdateDownloadedListenerFileRecords()::File - " + strFileName + " put on hold. Site code not found", false);

                            if (objCore1.INSTITUTION_ID == new Guid("00000000-0000-0000-0000-000000000000"))
                            {
                                #region create new institution
                                objCore1.INSTITUTION_NAME = arr[5].Trim();
                                if (objCore1.CreateNewInstitution(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                                {
                                    #region create notification
                                    if (!objCore1.CreateNewInstitutionNotification(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                                    {
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>CreateNewInstitutionNotification():Core::Exception - " + strCatchMessage, true);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>CreateNewInstitution():Core::Exception - " + strCatchMessage, true);
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
                            strInstCode = objCore1.INSTITUTION_CODE.Trim();
                            strInstName = objCore1.INSTITUTION_NAME.Trim();
                            if (strExtn.Trim() != string.Empty)
                                strNewFilename = objCore1.INSTITUTION_CODE.Trim() + "_" + strSessionID + "_" + objCore1.INSTITUTION_NAME.Trim().Replace(" ", "_") + "_" + strPrefix + "_" + strSUID.Replace(".", "").Substring(0, 5) + strExtn;
                            else
                                strNewFilename = objCore1.INSTITUTION_CODE.Trim() + "_" + strSessionID + "_" + objCore1.INSTITUTION_NAME.Trim().Replace(" ", "_") + "_" + strPrefix + "_" + strSUID.Replace(".", "").Substring(0, 5);

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
                            strNewFilePath = strFTPDLFLDRTMP + "/" + strNewFilename;

                            if (File.Exists(strFilePath))
                            {

                                File.Move(strFilePath, strNewFilePath);
                                if (File.Exists(strFilePath)) File.Delete(strFilePath);
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>UpdateDownloadedListenerFileRecords() File : " + strFileName + " renamed to " + strNewFilename, false);
                            }
                            #endregion

                            if (strSeriesUID.Trim() != string.Empty && strSOPInstanceUID.Trim() != string.Empty)
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File : " + strNewFilename.Trim() + " Update DB ", false);

                                #region Update DB
                                try
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File : " + strNewFilename.Trim() + "- SUID : " + strSUID.Trim(), false);
                                    objFP1.STUDY_UID = strSUID.Trim();
                                    objFP1.SERIES_UID = strSeriesUID.Trim();
                                    objFP1.SOP_INSTANCE_UID = strSOPInstanceUID.Trim();
                                    objFP1.STUDY_DATE = dtStudy;
                                    objFP1.INSTITUTION_CODE = strInstCode;
                                    objFP1.INSTITUTION_NAME = strInstName.Trim();
                                    objFP1.PATIENT_ID = strPatientID.Trim();
                                    objFP1.PATIENT_FIRST_NAME = strPatientFname.Trim();
                                    objFP1.PATIENT_LAST_NAME = strPatientLname.Trim();
                                    objFP1.FILE_NAME = strNewFilename;
                                    objFP1.ACCESSION_NUMBER = strAccnNo.Trim();
                                    objFP1.MODALITY = strModalityID;
                                    objFP1.REFERRING_PHYSICIAN = strRefPhys.Trim();
                                    objFP1.MANUFACTURER = strManufacturer.Trim();
                                    objFP1.STATION_NAME = strStationName.Trim();
                                    objFP1.MODEL = strModel.Trim();
                                    objFP1.MODALITY_AE_TITLE = strModalityAETitle.Trim();
                                    objFP1.REASON = strReason.Trim();
                                    objFP1.DATE_OF_BIRTH = dtDOB;
                                    objFP1.PATIENT_SEX = strPatientSex.Trim();
                                    objFP1.PATIENT_AGE = strPatientAge.Trim();
                                    objFP1.PRIORITY_ID = intPriorityID;
                                    objFP1.IMPORT_SESSION_ID = strSID.Trim();
                                    objFP1.IS_MANUAL_UPLOAD = strIsManual;

                                    #region debug log
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of file " + strFileName, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Study UID : " + objFP1.STUDY_UID, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Study Date : " + objFP1.STUDY_DATE.ToString(), false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Inst Code : " + objFP1.INSTITUTION_CODE, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Inst Name : " + objFP1.INSTITUTION_NAME, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Patient ID : " + objFP1.PATIENT_ID, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " P FNAME : " + objFP1.PATIENT_FIRST_NAME, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " P LNAME : " + objFP1.PATIENT_LAST_NAME, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Accn No : " + objFP1.ACCESSION_NUMBER, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Modality : " + objFP1.MODALITY, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Ref Phys : " + objFP1.REFERRING_PHYSICIAN, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Manufacturer : " + objFP1.MANUFACTURER, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Model : " + objFP1.MODEL, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Modality AE Title : " + objFP1.MODALITY_AE_TITLE, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Reason : " + objFP1.REASON, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " DOB : " + objFP1.DATE_OF_BIRTH.ToString(), false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " P Sex : " + objFP1.PATIENT_SEX, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " P Age : " + objFP1.PATIENT_AGE, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Priotity ID : " + objFP1.PRIORITY_ID.ToString(), false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Session ID : " + objFP1.IMPORT_SESSION_ID, false);
                                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Is Manual : " + objFP1.IS_MANUAL_UPLOAD, false);
                                    #endregion


                                    if (!objFP1.SaveListenerFileData(strConfigPath, strSvcName, ref strRetMessage, ref strCatchMessage, ref strDelFile))
                                    {
                                        if (strCatchMessage.Trim() != string.Empty)
                                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>UpdateDownloadedListenerFileRecords()=>SaveListenerFileData()::Error:Exception: " + strCatchMessage.Trim(), true);
                                        else
                                        {
                                            #region delete file if duplicate
                                            if (strDelFile == "Y")
                                            {
                                                if (File.Exists(strNewFilePath))
                                                {
                                                    File.Delete(strNewFilePath);
                                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>MoveFile()=>UpdateDownloadedListenerFileRecords():: Deleted file " + strNewFilename + ", as study already has this file saved", true);
                                                }
                                                else
                                                {
                                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>MoveFile()=>UpdateDownloadedListenerFileRecords()=>SaveListenerFileData()::Error: " + strRetMessage.Trim(), true);
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
                                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>MoveFile()=>UpdateDownloadedListenerFileRecords():: Deleted file " + strNewFilename + ", as study already has this file saved", true);
                                            }
                                            #endregion
                                        }
                                        else if (strSENDLFTOPACS == "N")
                                        {
                                            #region Transfer forwarded file to archive
                                            TransferForwardeFileToArchive(strInstCode, strInstName.Trim(), strSUID, strNewFilename, strNewFilePath, false, string.Empty, string.Empty);
                                            #endregion
                                        }

                                    }
                                }
                                catch (Exception expErr)
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>UpdateDownloadedListenerFileRecords()::Exception: " + expErr.Message, true);
                                }
                                #endregion
                            }
                            else
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File : " + strNewFilename.Trim() + " Rejected. Series UID & SOP Instance UID missing ", true);
                            }
                        }
                    }
                    else
                    {
                        #region put file on hold
                        if (File.Exists(strFILESHOLDPATH + "/" + strFileName)) File.Delete(strFILESHOLDPATH + "/" + strFileName);
                        File.Move(strFilePath, strFILESHOLDPATH + "/" + strFileName);
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>UpdateDownloadedListenerFileRecords()::File - " + strFileName + " put on hold. Institution name missing", false);
                        #endregion
                    }


                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File : " + strFileName.Trim() + " Study UID missing ", true);
                }
            }
            catch (Exception expErr)
            {
                //EventLog.WriteEntry(strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>UpdateDownloadedFilesRecords() - Exception : " + expErr.Message, EventLogEntryType.Error);
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>UpdateDownloadedFilesRecords():: DataUpdate - Exception: " + expErr.Message, true);
            }

            finally
            {
                objFP1 = null; dd = null; objCore1 = null;
            }
        }
        #endregion

        #region TransferForwardeFileToArchive
        private bool TransferForwardeFileToArchive(string InstitutionCode, string InstitutionName, string StudyUID, string FileName, string FilePathToMove, bool IsImageFile, string ImageFilePath, string ImageFileName)
        {
            string strFolder = InstitutionCode + "_" + InstitutionName + "_" + StudyUID;
            string strRetMessage = string.Empty;
            string strCatchMessage = string.Empty;

            bool bRet = true;
            FTPPACSSynch objFPArch1 = new FTPPACSSynch();

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

                
                objFPArch1.STUDY_UID = StudyUID;
                if (!objFPArch1.UpdateArchivedFileCount(strConfigPath, ref strRetMessage, ref strCatchMessage))
                {
                    if (strCatchMessage.Trim() != string.Empty)
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>TransferForwardeFileToArchive()=>UpdateArchivedFileCount()- File :" + FileName + "::Exception: " + strCatchMessage.Trim(), true);
                    else
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>TransferForwardeFileToArchive()=>UpdateArchivedFileCount()- File :" + FileName + "::Error: " + strRetMessage.Trim(), true);
                }
                //else
                //    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Archived file count of file :" + FileName + ", Study UID :" + StudyUID + " updated", false);
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>TransferForwardeFileToArchive()- File :" + FileName + ":: Exception: " + ex.Message, true);
                bRet = false;
            }
            finally
            {
                objFPArch1 = null;
            }

            return bRet;
        }
        #endregion

        #endregion

        #region Decompression of files and data updation in  database

        #region doDecompressFiles
        private void doDecompressFiles()
        {
            try
            {
                while (true)
                {
                    DecompressFiles();
                    //doDeleteZipFiles();
                    //doSendFileToPACS();
                    //doSendImgFileToPACS();
                    //doCheckOnHoldFiles();
                    //doCheckMissingSessionFiles();


                    System.Threading.Thread.Sleep(intFreq * 1000);
                }
            }
            catch (Exception expErr)
            {
                //EventLog.WriteEntry(strSvcName, "doDecompressFiles()=>Exception : " + expErr.Message, EventLogEntryType.Error);
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doDecompressFiles() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }


        }
        #endregion

        #region DecompressFiles
        private void DecompressFiles()
        {
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string strFileName = string.Empty;
            string strDirName = string.Empty;
            string strExtractPath = string.Empty;
            string strCatchMessage = string.Empty;
            string strDecompFile = string.Empty;
            string strSID = string.Empty;
            objCore = new Scheduler();

            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Getting files to decompress ", false);
                arrFiles = Directory.GetFiles(strFTPDLFLDRTMP, "*.zip");

                foreach (string strFile in arrFiles)
                {
                    FileInfo fi = new FileInfo(strFile);
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Decompressing file " + fi.FullName + " Length :" + fi.Length.ToString(), false);
                    if (fi.Length > 0)
                    {
                        pathElements = strFile.Split('\\');
                        strFileName = pathElements[(pathElements.Length - 1)];

                        try
                        {
                            using (ZipArchive archive = ZipFile.OpenRead(strFile))
                            {
                                foreach (ZipArchiveEntry entry in archive.Entries)
                                {
                                    strDecompFile = entry.FullName;
                                    strDecompFile = strDecompFile.Replace(" ", "_");
                                    strDecompFile = strDecompFile.Replace(",", "");
                                    strDecompFile = strDecompFile.Replace("(", "");
                                    strDecompFile = strDecompFile.Replace(")", "");
                                    strDecompFile = strDecompFile.Replace("'", "");
                                    strDecompFile = strDecompFile.Replace("\"", "");
                                    strDecompFile = strDecompFile.Replace("/", "_");
                                    strDecompFile = strDecompFile.Replace("\\", "_");
                                    strDecompFile = strDecompFile.Replace("#", "");
                                    strDecompFile = strDecompFile.Replace("&", "");
                                    strDecompFile = strDecompFile.Replace("@", "");
                                    strDecompFile = strDecompFile.Replace("?", "");
                                    strDecompFile = strDecompFile.Replace("__", "_");

                                    strExtractPath = strFTPDLFLDRTMP + "\\" + strDecompFile;
                                    if (!File.Exists(strExtractPath))
                                    {
                                        entry.ExtractToFile(strExtractPath);
                                        archive.Dispose();
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File : " + strFileName.Trim() + " decompressed ", false);
                                        UpdateDownloadedFilesRecords(strDecompFile, strFile);
                                    }
                                    else
                                    {
                                        if (File.Exists(strFile))
                                        {
                                            archive.Dispose();
                                            File.Delete(strFile);
                                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File : " + strFileName.Trim() + " deleted", false);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception expErr)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doDecompressFiles()=>DecompressFiles() - File : " + strFile + " - Exception: " + expErr.Message, true);
                        }
                    }
                    else
                        File.Delete(strFile);

                    fi = null;

                }

            }
            catch (IOException ex)
            {
                ;
                //if (IsFileLocked(ex))
                //{
                //    UnlockFileProcess(strExtractPath);
                //}
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doDecompressFiles()=>DecompressFiles() - Exception: " + expErr.Message, true);
            }
            doDeleteZipFiles(); 
        }
        #endregion

        #region Suspended
        //#region ArrangeFiles
        //private void ArrangeFiles()
        //{

        //    string strSUID = string.Empty;
        //    string strInstCode = string.Empty;
        //    string strInstName = string.Empty;

        //    string[] arrFiles = new string[0];
        //    string[] pathElements = new string[0];
        //    string[] arr = new string[0];
        //    string[] arrInst = new string[0];

        //    string strFileName = string.Empty;
        //    string strFolder = string.Empty;
        //    DicomDecoder dd = new DicomDecoder();


        //    try
        //    {
        //        arrFiles = Directory.GetFiles(strFTPDLFLDRTMP);
        //        foreach (string strFile in arrFiles)
        //        {
        //            pathElements = strFile.Split('\\');
        //            strFileName = pathElements[(pathElements.Length - 1)];
        //            arrInst = strFileName.Split('_');

        //            dd.DicomFileName = strFile;
        //            List<string> str = dd.dicomInfo;

        //            arr = new string[7];
        //            arr = GetallTags(str);

        //            strInstCode = arrInst[0];
        //            strSUID = arr[0].Trim();
        //            strInstName = arr[5].Trim();

        //            strFolder = strInstCode.Trim() + "_" + strInstName.ToUpper().Trim().Replace(' ', '_') + "_" + strSUID;
        //            if (!Directory.Exists(strFolder)) Directory.CreateDirectory(strFolder);
        //            if (File.Exists(strFolder + "\\" + strFile)) File.Delete(strFolder + "\\" + strFile);
        //            File.Move(strFile, strFolder + "\\" + strFile);
        //            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File : " + strFileName.Trim() + " moved to folder " + strFolder, false);
        //        }
        //        UpdateDownloadedFilesRecords();
        //    }
        //    catch (Exception expErr)
        //    {
        //        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCompressFiles()=>ArrangeFiles() - Exception: " + expErr.Message, true);
        //    }
        //    finally
        //    {
        //        dd = null;
        //    }
        //}
        //#endregion

        //#region UpdateDownloadedFilesRecords
        //private void UpdateDownloadedFilesRecords(string  strFileName)
        //{
        //    string strCatchMessage = string.Empty;
        //    string strRetMessage = string.Empty;
        //    string strSUID = string.Empty;
        //    DateTime dtStudy = DateTime.Now;
        //    string strInstCode = string.Empty;
        //    string strInstName = string.Empty;
        //    string strPatientName = string.Empty;
        //    string strPatientFname = string.Empty;
        //    string strPatientLname = string.Empty;
        //    int intFileCount = 0;
        //    string strDt = "0000-00-00";

        //    string[] arrDt = new string[0];
        //    string[] arrTime = new string[0];
        //    string[] arrDateTime = new string[0];
        //    string[] arrFolders = new string[0];
        //    string[] arrFiles = new string[0];
        //    string[] pathElements = new string[0];
        //    string[] arr = new string[0];

        //    string strFileName = string.Empty;
        //    List<string> arrSUID = new List<string>();
        //    DicomDecoder dd = new DicomDecoder();
        //    objFP = new FTPPACSSynch();

        //    try
        //    {
        //        arrFolders = Directory.GetDirectories(strFTPDLFLDRTMP);

        //        foreach (string strFolder in arrFolders)
        //        {
        //            arrFiles = Directory.GetFiles(strFolder);
        //            intFileCount = arrFiles.Length;
        //            strInstCode = (strFolder.Split('_'))[0].Trim();

        //            #region Get File Data and update db
        //            foreach (string strFile in arrFiles)
        //            {
        //                if ((strInstName.Trim() == string.Empty) || (strDt.Trim() == "0000-00-00") || (strPatientName.Trim() == string.Empty))
        //                {
        //                    pathElements = strFile.Split('\\');
        //                    strFileName = pathElements[(pathElements.Length - 1)];
        //                    dd.DicomFileName = strFile;
        //                    List<string> str = dd.dicomInfo;

        //                    arr = new string[7];
        //                    arr = GetallTags(str);
        //                    strSUID = arr[0].Trim();
        //                    strPatientName = arr[3].Trim();

        //                    if (strPatientName != string.Empty)
        //                    {
        //                        if (strPatientName.Contains(' '))
        //                        {
        //                            strPatientFname = strPatientName.Substring(0, strPatientName.LastIndexOf(' '));
        //                            strPatientLname = strPatientName.Substring(strPatientName.LastIndexOf(' '), (strPatientName.Length - strPatientName.LastIndexOf(' ')));
        //                        }
        //                        else
        //                        {
        //                            strPatientFname = strPatientName;
        //                            strPatientLname = string.Empty;
        //                        }
        //                    }



        //                    strDt = arr[4].Trim();
        //                    if ((arr[4].Trim() == "0000-00-00") || (arr[4].Trim() == string.Empty)) dtStudy = DateTime.Now;
        //                    else
        //                    {
        //                        arrDateTime = arr[4].Trim().Split(' ');
        //                        arrDt = arrDateTime[0].Split('-');
        //                        arrTime = arrDateTime[1].Split(':');

        //                        dtStudy = new DateTime(Convert.ToInt32(arrDt[0]),
        //                                               Convert.ToInt32(arrDt[1]),
        //                                               Convert.ToInt32(arrDt[2]),
        //                                               Convert.ToInt32(arrTime[0]),
        //                                               Convert.ToInt32(arrTime[1]),
        //                                               Convert.ToInt32(arrTime[2]));
        //                    }

        //                    strInstName = arr[5].Trim();

        //                    #region Update DB
        //                    try
        //                    {

        //                        objFP.STUDY_UID = strSUID.Trim();
        //                        objFP.STUDY_DATE = dtStudy;
        //                        objFP.INSTITUTION_CODE = strInstCode;
        //                        objFP.INSTITUTION_NAME = strInstName.Trim();
        //                        objFP.PATIENT_FIRST_NAME = strPatientFname.Trim();
        //                        objFP.PATIENT_LAST_NAME = strPatientLname.Trim();
        //                        objFP.FILE_COUNT = intFileCount;

        //                        if (!objFP.SaveData(strConfigPath, strSvcName, ref strRetMessage, ref strCatchMessage))
        //                        {
        //                            if (strCatchMessage.Trim() != string.Empty)
        //                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords():: DBUpdate - Error:Exception: " + strCatchMessage.Trim(), true);
        //                            else
        //                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords():: DBUpdate - Error: " + strRetMessage.Trim(), true);
        //                        }
        //                    }
        //                    catch (Exception expErr)
        //                    {
        //                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords():: DBUpdate - Exception: " + expErr.Message, true);
        //                    }
        //                    #endregion

        //                }
        //                else
        //                    break;


        //            }
        //            #endregion
        //        }
        //    }
        //    catch (Exception expErr)
        //    {
        //        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords():: DataUpdate - Exception: " + expErr.Message, true);
        //    }

        //    finally
        //    {
        //        objFP = null; dd = null;
        //    }
        //}
        //#endregion
        #endregion

        #region UpdateDownloadedFilesRecords
        private void UpdateDownloadedFilesRecords(string strFileName, string strFileToDelete)
        {
            string strCatchMessage = string.Empty;
            string strRetMessage = string.Empty;
            string strFilePath = string.Empty;
            string strSID = string.Empty;
            string strIsManual = "N";
            string strIsLF = "N";
            string strDelFile = "N";

            string[] arrFolders = new string[0];
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string[] arr = new string[0];
            List<string> arrSUID = new List<string>();
            DicomDecoder dd = new DicomDecoder();

            #region TAG Variables
            string strSUID = string.Empty;
            string strSeriesUID = string.Empty;
            string strSOPInstanceUID = string.Empty;
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


            objFP = new FTPPACSSynch();

            try
            {

                strFilePath = strFTPDLFLDRTMP + "\\" + strFileName;
                strInstCode = (strFileName.Split('_'))[0].Trim();
                

                if (CoreCommon.IsDicomFile(strFilePath))
                {
                    strSID = (strFileName.Split('_'))[1].Trim();
                    if (strSID.Substring(0, 1) == "M") strIsManual = "Y";
                    else if (strSID.Substring(0, 1) == "L") strIsLF = "Y";

                    #region DCM Files
                    dd.DicomFileName = strFilePath;
                    List<string> str = dd.dicomInfo;

                    arr = new string[20];
                    arr = GetallTags(str);
                    strSUID = arr[0].Trim();
                    if (strSUID.Trim() == string.Empty) GetStudyUIDFromDump(strFilePath);

                    if (strSUID.Trim() != string.Empty)
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedFilesRecords()=>File : " + strFileName.Trim() + " get tag data ", false);

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

                            if (arr[16].Trim() != string.Empty) intPriorityID = Convert.ToInt32(arr[16].Trim());
                            strSeriesUID = arr[17].Trim();
                            if (strSeriesUID == string.Empty) strSeriesUID = GetSeriesUIDFromDump(strFilePath);
                            strSOPInstanceUID = arr[19].Trim();
                            if (strSOPInstanceUID == string.Empty) strSOPInstanceUID = GetSOPInstanceUIDFromDump(strFilePath);


                            strDt = arr[4].Trim();
                            if ((arr[4].Trim() == "0000-00-00") || (arr[4].Trim() == string.Empty)) dtStudy = DateTime.Now;
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
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedFilesRecords()=>Get File Data =>File : " + strFileName.Trim() + " get tag data  :: " + ex.Message.Trim(), true);
                        }

                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedFilesRecords()=>File : " + strFileName.Trim() + " Update DB ", false);


                        #region Update DB
                        try
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateDownloadedFilesRecords()=>Update DB=>File : " + strFileName.Trim() + "- SUID : " + strSUID.Trim(), false);
                            objFP.STUDY_UID = strSUID.Trim();
                            objFP.SERIES_UID = strSeriesUID.Trim();
                            objFP.SOP_INSTANCE_UID = strSOPInstanceUID.Trim();
                            objFP.STUDY_DATE = dtStudy;
                            objFP.INSTITUTION_CODE = strInstCode;
                            objFP.INSTITUTION_NAME = strInstName.Trim();
                            objFP.PATIENT_ID = strPatientID.Trim();
                            objFP.PATIENT_FIRST_NAME = strPatientFname.Trim();
                            objFP.PATIENT_LAST_NAME = strPatientLname.Trim();
                            objFP.FILE_NAME = strFileName;
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
                            objFP.RECEIVED_BY_LISTENER = strIsLF;

                            #region debug log
                            //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of file " + strFileName, false);
                            //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Study UID : " + objFP.STUDY_UID, false);
                            //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Series UID : " + objFP.SERIES_UID, false);
                            //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Instance # : " + objFP.SOP_INSTANCE_UID, false);
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
                            //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving info of File " + strFileName + " Received By Listener : " + objFP.RECEIVED_BY_LISTENER, false);
                            #endregion


                            if (!objFP.SaveData(strConfigPath, strSvcName, ref strRetMessage, ref strCatchMessage, ref strDelFile))
                            {
                                if (strCatchMessage.Trim() != string.Empty)
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords()=>SaveData()::Error:Exception: " + strCatchMessage.Trim(), true);
                                else
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords()=>SaveData()::Error: " + strRetMessage.Trim(), true);

                                if (strDelFile == "Y")
                                {
                                    if (File.Exists(strFilePath))
                                    {
                                        File.Delete(strFilePath);
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords():: Deleted file " + strFileName + ", as study already exists", true);
                                    }
                                    if (File.Exists(strFileToDelete))
                                    {
                                        File.Delete(strFileToDelete);
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords():: Deleted file " + strFileToDelete, true);
                                    }
                                }
                            }
                            else
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords()=>SaveData()::File: " + strFileName.Trim() + " updated", false);
                            }
                        }
                        catch (Exception expErr)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords()::Exception: " + expErr.Message, true);
                        }
                        #endregion
                    }
                    #endregion
                }
                else if ((MIMEAssistant.GetMIMEType(strFilePath) == "image/jpeg") || (MIMEAssistant.GetMIMEType(strFilePath) == "image/gif") || (MIMEAssistant.GetMIMEType(strFilePath) == "image/png") || (MIMEAssistant.GetMIMEType(strFilePath) == "image/bmp"))
                {
                    strSID = (strFileName.Split('_'))[1].Trim();
                    if (strSID.Substring(0, 1) == "M") strIsManual = "Y";
                    else if (strSID.Substring(0, 1) == "L") strIsLF = "Y";

                    #region Image Files

                    if (strIsManual == "N")
                    {
                        #region Update DB
                        try
                        {

                            objFP.INSTITUTION_CODE = strInstCode;
                            objFP.FILE_NAME = strFileName;
                            objFP.IMPORT_SESSION_ID = strSID;

                            if (!objFP.SaveImageDownloadInfo(strConfigPath, strSvcName, ref strRetMessage, ref strCatchMessage))
                            {
                                if (strCatchMessage.Trim() != string.Empty)
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords()=>SaveImageDownloadInfo()::Error:Exception: " + strCatchMessage.Trim(), true);
                                else
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords()=>SaveImageDownloadInfo()::Error: " + strRetMessage.Trim(), true);
                            }
                        }
                        catch (Exception expErr)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords()::Exception: " + expErr.Message, true);
                        }
                        #endregion
                    }

                    #endregion

                }
                else if (MIMEAssistant.GetMIMEType(strFilePath) == "application/xml")
                {
                    #region XML FIles

                    DataTable dtbl = CreateDRLogTable();
                    DataSet ds = new DataSet();

                    #region Update DB
                    try
                    {
                        ds.ReadXml(strFilePath);

                        foreach (DataRow drLog in ds.Tables[0].Rows)
                        {
                            DataRow dr = dtbl.NewRow();

                            dr["institution_code"] = strInstCode;
                            dr["service_id"] = Convert.ToInt32(drLog["service_id"]); ;
                            dr["service_name"] = Convert.ToString(drLog["service_name"]);
                            dr["log_date"] = Convert.ToDateTime(drLog["log_date"]);
                            dr["log_message"] = Convert.ToString(drLog["log_message"]);
                            dr["is_error"] = Convert.ToString(drLog["is_error"]);

                            dtbl.Rows.Add(dr);
                        }


                        if (!objFP.SaveDicomRouterLog(strConfigPath, intServiceID, strSvcName, dtbl, ref strRetMessage, ref strCatchMessage))
                        {
                            if (strCatchMessage.Trim() != string.Empty)
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords()=>SaveDicomRouterLog()::Error:Exception: " + strCatchMessage.Trim(), true);
                            else
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords()=>SaveDicomRouterLog()::Error: " + strRetMessage.Trim(), true);
                        }
                        else
                        {
                            File.Delete(strFilePath);
                            if (strFileToDelete.Trim() != string.Empty)
                            {
                                if (File.Exists(strFileToDelete)) File.Delete(strFileToDelete);
                            }
                        }
                    }
                    catch (Exception expErr)
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords()::Exception: " + expErr.Message, true);
                    }
                    finally
                    {
                        ds.Dispose();
                        dtbl = null;
                    }
                    #endregion

                    #endregion
                }

            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doDecompressFiles()=>UpdateDownloadedFilesRecords():: DataUpdate - Exception: " + expErr.Message, true);
            }

            finally
            {
                objFP = null; dd = null;
            }
        }
        #endregion

        #region doDeleteZipFiles
        private void doDeleteZipFiles()
        {
            try
            {
                DeleteZipFiles();

            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doDeleteZipFiles() - Exception: " + expErr.Message, true);
            }

            doSendFileToPACS();
        }
        #endregion

        #region doSendFileToPACS
        private void doSendFileToPACS()
        {
            try
            {
                //while (true)
                //{
                SendFileToPACS();
                //}
            }
            catch (Exception expErr)
            {
                //EventLog.WriteEntry(strSvcName, "doSendFileToPACS()=>Exception : " + expErr.Message, EventLogEntryType.Error);
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }

            doSendImgFileToPACS();
        }
        #endregion

        #region doSendImgFileToPACS
        private void doSendImgFileToPACS()
        {
            try
            {
                //while (true)
                //{
                SendImageFileToPACS();
                //}

            }
            catch (Exception expErr)
            {
                //EventLog.WriteEntry(strSvcName, "doDecompressFiles()=>Exception : " + expErr.Message, EventLogEntryType.Error);
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }

            doCheckOnHoldFiles();
        }
        #endregion

        #endregion

        #region Delete .zip file(s) that has been extracted

        #region DeleteZipFiles
        private void DeleteZipFiles()
        {
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string strFileName = string.Empty;
            string strExtractPath = string.Empty;
            string strDecompFile = string.Empty;
            string strCatchMessage = string.Empty;


            try
            {

                arrFiles = Directory.GetFiles(strFTPDLFLDRTMP, "*.zip");
                if (arrFiles.Length > 0) CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleteting .zip files decompressed", false);

                foreach (string strFile in arrFiles)
                {
                    FileInfo fi = new FileInfo(strFile);
                    if (fi.Length > 0)
                    {
                        pathElements = strFile.Split('\\');
                        strFileName = pathElements[(pathElements.Length - 1)];
                        strExtractPath = string.Empty;

                        try
                        {
                            using (ZipArchive archive = ZipFile.OpenRead(strFile))
                            {
                                foreach (ZipArchiveEntry entry in archive.Entries)
                                {
                                    strDecompFile = entry.FullName;
                                    strDecompFile = strDecompFile.Replace(" ", "_");
                                    strDecompFile = strDecompFile.Replace("(", "");
                                    strDecompFile = strDecompFile.Replace(")", "");
                                    //entry.ExtractToFile(Path.Combine(strFTPDLFLDRTMP, entry.Name));
                                    strExtractPath = strFTPDLFLDRTMP + "\\" + strDecompFile;
                                }
                            }
                        }
                        catch (Exception expErr)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doDeleteZipFiles()=>DeleteZipFiles() - File :" + strFile + " - Exception: " + expErr.Message, true);
                        }

                        if (strExtractPath.Trim() != string.Empty)
                        {
                            if ((File.Exists(strExtractPath)) || (strExtractPath.Substring(strExtractPath.LastIndexOf("."), 4) == ".xml"))
                            {
                                try
                                {
                                    File.Delete(strFile);
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File : " + strFileName.Trim() + " deleted", false);
                                }
                                catch (IOException ex)
                                {
                                    ;
                                    //if (IsFileLocked(ex))
                                    //{
                                    //    UnlockFileProcess(strFile);
                                    //}
                                }
                                catch (Exception expErr)
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doDeleteZipFiles()=>DeleteZipFiles() :: File.Detete() - Exception: " + expErr.Message, true);
                                }
                            }
                        }
                    }
                    fi = null;
                }

            }
            catch (IOException ex)
            {
                ;
                //if (IsFileLocked(ex))
                //{
                //    UnlockFileProcess(strExtractPath);
                //}
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doDeleteZipFiles()=>DeleteZipFiles() - Exception: " + expErr.Message, true);
            }

        }
        #endregion

        #endregion

        #region Transfer files to PACS

        #region SendFileToPACS
        private void SendFileToPACS()
        {

            DataSet ds = new DataSet();
            string strFileName = string.Empty;
            Guid Id = Guid.Empty;

            string strCatchMessage = string.Empty;
            string strRetMsg = string.Empty;
            string[] arr = new string[0];
            DicomDecoder dd = new DicomDecoder();
            string strFilePath = string.Empty;
            string strFmtDCMFile = "N";
            string strSID = string.Empty;
            bool bRet = true;
            bool bFmt = true;

            #region TAG Variables
            string strSUID = string.Empty;
            Guid InstID = Guid.Empty;
            string strInstCode = string.Empty;
            string strInstName = string.Empty;
            string strPatientID = string.Empty;
            string strPFName = string.Empty;
            string strPLName = string.Empty;
            DateTime dtSTudy = DateTime.Now;
            #endregion


            try
            {

                if (GetFilesToTransfer(ref ds))
                {
                    foreach (DataRow dr in ds.Tables["FileList"].Rows)
                    {
                        bFmt = false;
                        Id = new Guid(Convert.ToString(dr["id"]));
                        strFileName = Convert.ToString(dr["file_name"]).Trim();
                        strSUID = Convert.ToString(dr["study_uid"]).Trim();
                        InstID = new Guid(Convert.ToString(dr["institution_id"]));
                        strInstCode = Convert.ToString(dr["institution_code"]).Trim();
                        strInstName = Convert.ToString(dr["institution_name"]).Trim();
                        strPatientID = Convert.ToString(dr["patient_id"]).Trim();
                        strPFName = Convert.ToString(dr["patient_fname"]).Trim();
                        strPLName = Convert.ToString(dr["patient_lname"]).Trim();
                        dtSTudy = Convert.ToDateTime(dr["study_date"]);
                        strFmtDCMFile = Convert.ToString(dr["format_dcm_files"]).Trim();
                        strSID = Convert.ToString(dr["import_session_id"]).Trim();

                        if (File.Exists(strFTPDLFLDRTMP + "\\" + strFileName))
                        {
                            strFilePath = strFTPDLFLDRTMP + "/" + strFileName;

                            if ((strSID.Substring(0, 6) == "S1DXXX") && (strSENDLFTOPACS == "N"))
                            {
                                TransferToArchive(strInstCode, strInstName, strSUID, strFileName, strFTPDLFLDRTMP + "\\" + strFileName, false, string.Empty, string.Empty);
                            }
                            else
                            {
                                dd.DicomFileName = strFilePath;
                                List<string> str = dd.dicomInfo;

                                #region Format DCM files
                                if (strFmtDCMFile == "Y")
                                {
                                    DataSet dsTags = new DataSet();
                                    FTPPACSSynch obj = new FTPPACSSynch();
                                    try
                                    {
                                        obj.INSTITUTION_ID = InstID;
                                        if (obj.FetchTagsToFormat(strConfigPath, ref dsTags, ref strCatchMessage))
                                        {
                                            if (dsTags.Tables["TagList"].Rows.Count > 0) bFmt = FormatDCMTags(dsTags.Tables["TagList"], strSUID, str, strFilePath);
                                        }
                                        else
                                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>SendFileToPACS(): Core()-FetchTagsToFormat :: Error - " + strCatchMessage, false);
                                    }
                                    catch (Exception expErr)
                                    {
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>SendFileToPACS() - FetchTagsToFormat :: Exception - " + expErr.Message, false);
                                    }
                                    finally
                                    {
                                        dsTags.Dispose();
                                        obj = null;
                                    }
                                }
                                else
                                    bFmt = true;
                                #endregion

                                if (bFmt)
                                {
                                    arr = new string[20];
                                    arr = GetallTags(str);

                                    #region Modify Institution Name
                                    if ((arr[5].Trim().ToUpper() != strInstName.ToUpper()) || (arr[0].Trim().ToUpper() != strSUID.ToUpper()))
                                    {
                                        if (!ModifyInstitutionName(strInstName, strSUID, strFilePath))
                                        {
                                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Failed to modify institution name :: SUID : " + strSUID.Trim() + " :: File Name : " + strFileName, true);
                                        }
                                    }
                                    #endregion

                                    bRet = true;
                                    if (bRet)
                                    {
                                        if (TransferFileToPacs(strFTPDLFLDRTMP, strFileName, ref strRetMsg))
                                        {
                                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "SUCCESS :: Upload to PACS :: SUID : " + strSUID.Trim() + " :: File Name : " + strFileName, false);
                                            if (UpdateTransferToPACSFileCount(Id, strFileName))
                                            {
                                                TransferToArchive(strInstCode, strInstName, strSUID, strFileName, strFTPDLFLDRTMP + "\\" + strFileName, false, string.Empty, string.Empty);
                                            }
                                        }
                                        else
                                        {
                                            #region if Transfer to PACS fails
                                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FAILED :: Upload to PACS :: SUID : " + strSUID.Trim() + " :: File Name : " + strFileName, true);
                                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Reason :: " + strRetMsg, true);
                                            CreateFileXferFailureNotification(strSUID, strFileName, strInstCode, strInstName, strRetMsg);

                                            if (strSID.Substring(0, 6) == "S1DXXX")
                                            {
                                                //UpdateTransferToPACSFileCount(Id, strFileName);
                                                TransferToArchive(strInstCode, strInstName, strSUID, strFileName, strFTPDLFLDRTMP + "\\" + strFileName, false, string.Empty, string.Empty);
                                            }
                                            #endregion
                                        }

                                    }
                                    //else
                                    //{
                                    //    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Failed to modify institution name :: SUID : " + strSUID.Trim() + " :: File Name : " + strFileName, true);
                                    //}

                                }
                            }

                        }
                    }
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>SendFileToPACS() : File - " + strFileName + " :: Exception - " + expErr.Message, false);
            }
            finally
            {
                ds.Dispose();
            }

            System.Threading.Thread.Sleep(intFreq * 1000);
        }
        #endregion

        #region GetFilesToTransfer
        private bool GetFilesToTransfer(ref DataSet ds)
        {

            string strRetMessage = string.Empty;
            string strCatchMessage = string.Empty;
            bool bReturn = false;
            objFP = new FTPPACSSynch();

            try
            {
                if (!objFP.FetchApprovedFilesToTransfer(strConfigPath, ref ds, ref strCatchMessage))
                {
                    bReturn = false;
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>SendFileToPACS()=>GetFilesToTransfer()=>Core:FetchDownloadFiesToTransfer():: Exception: " + strCatchMessage, true);
                }
                else
                    bReturn = true;
            }
            catch (Exception ex)
            {

                bReturn = false;
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>GetFilesToTransfer():: Exception: " + ex.Message, true);
            }
            finally
            {
                objFP = null;
            }

            return bReturn;
        }
        #endregion

        #region UpdateTransferToPACSFileCount
        private bool UpdateTransferToPACSFileCount(Guid Id, string strFileName)
        {

            string strRetMessage = string.Empty;
            string strCatchMessage = string.Empty;
            bool bReturn = false;
            objFP = new FTPPACSSynch();

            try
            {
                objFP.ID = Id;
                objFP.FILE_NAME = strFileName;

                if (!objFP.UpdateTransferToPACSFileCount(strConfigPath, ref strRetMessage, ref strCatchMessage))
                {
                    bReturn = false;
                    if (strCatchMessage.Trim() != string.Empty)
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>SendFileToPACS()=>UpdateTransferToPACSFileCount()=>Core:UpdateTransferToPACSFileCount():: Exception: " + strCatchMessage, true);
                    else
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>SendFileToPACS()=>UpdateTransferToPACSFileCount()=>Core:UpdateTransferToPACSFileCount():: Error : " + strRetMessage, true);
                }
                else
                    bReturn = true;
            }
            catch (Exception ex)
            {

                bReturn = false;
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>UpdateTransferToPACSFileCount():: Exception: " + ex.Message, true);
            }
            finally
            {
                objFP = null;
            }

            return bReturn;
        }
        #endregion

        #region ModifyInstitutionName
        private bool ModifyInstitutionName(string strInsName, string strSUID, string strDCMPath)
        {
            bool bRet = true;
            string strProcOutput = string.Empty;
            string strProcError = string.Empty;

            try
            {
                Process ProcModInst = new Process();
                ProcModInst.StartInfo.UseShellExecute = false;
                ProcModInst.StartInfo.FileName = strDCMMODIFYEXEPATH;
                ProcModInst.StartInfo.Arguments = "-i \"(0008,0080)=" + strInsName + "\"" + " " + strDCMPath;
                ProcModInst.StartInfo.RedirectStandardOutput = true;
                ProcModInst.StartInfo.RedirectStandardError = true;
                ProcModInst.Start();
                strProcOutput = ProcModInst.StandardOutput.ReadToEnd();
                strProcError = ProcModInst.StandardError.ReadToEnd();


                if (strProcOutput.Trim() != string.Empty)
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>SendFileToPACS()=>ModifyInstitutionName() - Update Institution :: Error - " + strProcOutput.Trim(), false);
                }



            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>SendFileToPACS()=>ModifyInstitutionName() - Update Institution :: Exception - " + ex.Message, false);
            }

            if (bRet)
            {
                try
                {
                    Process ProcModSUID = new Process();
                    ProcModSUID.StartInfo.UseShellExecute = false;
                    ProcModSUID.StartInfo.FileName = strDCMMODIFYEXEPATH;
                    ProcModSUID.StartInfo.Arguments = "-i \"(0020,000D)=" + strSUID + "\"" + " " + strDCMPath;
                    ProcModSUID.StartInfo.RedirectStandardOutput = true;
                    ProcModSUID.StartInfo.RedirectStandardError = true;
                    ProcModSUID.Start();
                    strProcOutput = ProcModSUID.StandardOutput.ReadToEnd();
                    strProcError = ProcModSUID.StandardError.ReadToEnd();


                    if (strProcOutput.Trim() != string.Empty)
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>SendFileToPACS()=>ModifyInstitutionName() - Update SUID :: Error - " + strProcOutput.Trim(), false);
                    }


                }
                catch (Exception ex)
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>SendFileToPACS()=>ModifyInstitutionName() - Update SUID :: Exception - " + ex.Message, false);
                }
            }

            return bRet;
        }
        #endregion

        #region FormatDCMTags
        private bool FormatDCMTags(DataTable dtbl, string strSUID, List<string> str, string strDCMPath)
        {
            bool bRet = true;
            string strTagValue = string.Empty;
            string strTagID = string.Empty;
            string strGroupID = string.Empty;
            string strElementID = string.Empty;
            string strJunk = string.Empty;
            string strProcOutput = string.Empty;
            string strProcError = string.Empty;

            try
            {
                foreach (DataRow dr in dtbl.Rows)
                {

                    strGroupID = Convert.ToString(dr["group_id"]).Trim();
                    strElementID = Convert.ToString(dr["element_id"]).Trim();
                    strTagValue = Convert.ToString(dr["default_value"]).Trim();
                    strJunk = Convert.ToString(dr["junk_characters"]).Trim();

                    strTagID = "(" + strGroupID.ToUpper() + "," + strElementID.ToUpper() + ")";

                    if (strTagValue.Trim() == string.Empty)
                    {
                        if (strJunk.Trim() != string.Empty)
                        {
                            strTagValue = GetTagValue(str, strGroupID, strElementID);
                            if (strJunk.Contains(","))
                            {
                                string[] arrJunk = strJunk.Split(',');
                                for (int i = 0; i < arrJunk.Length; i++)
                                {
                                    if ((arrJunk[i].Trim() != string.Empty) || (arrJunk[i] != null))
                                    {
                                        strTagValue = strTagValue.Replace(arrJunk[i], "");
                                    }
                                }
                            }
                            else
                                strTagValue = strTagValue.Replace(strJunk.Trim(), "");
                        }
                    }


                    Process ProcFormat = new Process();
                    ProcFormat.StartInfo.UseShellExecute = false;
                    ProcFormat.StartInfo.FileName = strDCMMODIFYEXEPATH;
                    ProcFormat.StartInfo.Arguments = "-i \"" + strTagID + "=" + strTagValue + "\"" + " " + strDCMPath;
                    ProcFormat.StartInfo.RedirectStandardOutput = true;
                    ProcFormat.StartInfo.RedirectStandardError = true;
                    ProcFormat.Start();
                    strProcOutput = ProcFormat.StandardOutput.ReadToEnd();
                    strProcError = ProcFormat.StandardError.ReadToEnd();

                    if (strProcOutput.Trim() != string.Empty)
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>SendFileToPACS()=>FormatDCMTags() :: Error - " + strProcOutput.Trim(), true);
                    }

                }

                //update study uid
                strTagID = "(0020,000D)";
                Process ProcFormatUID = new Process();
                ProcFormatUID.StartInfo.UseShellExecute = false;
                ProcFormatUID.StartInfo.FileName = strDCMMODIFYEXEPATH;
                ProcFormatUID.StartInfo.Arguments = "-i \"" + strTagID + "=" + strSUID + "\"" + " " + strDCMPath;
                ProcFormatUID.StartInfo.RedirectStandardOutput = true;
                ProcFormatUID.StartInfo.RedirectStandardError = true;
                ProcFormatUID.Start();
                strProcOutput = ProcFormatUID.StandardOutput.ReadToEnd();
                strProcError = ProcFormatUID.StandardError.ReadToEnd();

                if (strProcOutput.Trim() != string.Empty)
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>SendFileToPACS()=>FormatDCMTags()-Update SUID :: Error - " + strProcOutput.Trim(), true);
                }


            }
            catch (Exception ex)
            {
                bRet = false;
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>SendFileToPACS()=>FormatDCMTags() :: Exception - " + ex.Message, true);
            }

            return bRet;
        }
        #endregion

        #region CreateFileXferFailureNotification
        private bool CreateFileXferFailureNotification(string strStudyUID, string strFileName, string strInsCode, string strInsName, string strReason)
        {

            string strRetMessage = string.Empty;
            string strCatchMessage = string.Empty;
            bool bReturn = false;
            objFP = new FTPPACSSynch();

            try
            {
                objFP.STUDY_UID = strStudyUID;
                objFP.FILE_NAME = strFTPDLFLDRTMP + "/" + strFileName;
                objFP.INSTITUTION_CODE = strInsCode;
                objFP.INSTITUTION_NAME = strInsName;
                objFP.FAILURE_REASON = strReason.Trim();

                if (!objFP.CreateFileXferFailureNotification(strConfigPath, ref strRetMessage, ref strCatchMessage))
                {
                    bReturn = false;
                    if (strCatchMessage.Trim() != string.Empty)
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>SendFileToPACS()=>UpdateTransferToPACSFileCount()=>Core:CreateFileXferFailureNotification():: Exception: " + strCatchMessage, true);
                    else
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>SendFileToPACS()=>CreateFileXferFailureNotification()=>Core:CreateFileXferFailureNotification():: Error : " + strRetMessage, true);
                }
                else
                    bReturn = true;
            }
            catch (Exception ex)
            {

                bReturn = false;
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>CreateFileXferFailureNotification():: Exception: " + ex.Message, true);
            }
            finally
            {
                objFP = null;
            }

            return bReturn;
        }
        #endregion

        #region Transfer image files to PACS

        #region SendImageFileToPACS
        private void SendImageFileToPACS()
        {

            DataSet ds = new DataSet();
            string strFileName = string.Empty;
            Guid Id = Guid.Empty;
            string strCatchMessage = string.Empty;
            string strFileDCM = string.Empty;
            string strRetMsg = string.Empty;
            string strSID = string.Empty;
            string strIsManual = string.Empty;

            #region TAG Variables
            string strSUID = string.Empty;
            string strInstCode = string.Empty;
            string strInstName = string.Empty;
            string strPatientID = string.Empty;
            string strPFName = string.Empty;
            string strPLName = string.Empty;
            string strPName = string.Empty;
            DateTime dtStudy = DateTime.Now;
            string strModality = string.Empty;
            string strSeriesInstanceID = string.Empty;
            string strSeriesNo = string.Empty;
            #endregion


            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Getting files to dicomise", false);
                if (GetFilesToDicomise(ref ds))
                {
                    foreach (DataRow dr in ds.Tables["FileList"].Rows)
                    {
                        Id = new Guid(Convert.ToString(dr["id"]));
                        strSUID = Convert.ToString(dr["study_uid"]).Trim();
                        strFileName = Convert.ToString(dr["file_name"]).Trim();
                        strInstCode = Convert.ToString(dr["institution_code"]).Trim();
                        strInstName = Convert.ToString(dr["institution_name"]).Trim();
                        strPatientID = Convert.ToString(dr["patient_id"]).Trim();
                        strPFName = Convert.ToString(dr["patient_fname"]).Trim();
                        strPLName = Convert.ToString(dr["patient_lname"]).Trim();
                        strPName = strPLName + "^" + strPFName;
                        dtStudy = Convert.ToDateTime(dr["study_date"]);
                        strModality = Convert.ToString(dr["modality"]).Trim();
                        strSeriesInstanceID = Convert.ToString(dr["series_instance_uid"]).Trim();
                        strSeriesNo = Convert.ToString(dr["series_no"]).Trim();
                        strSID = Convert.ToString(dr["import_session_id"]).Trim();


                        if (strSID.Substring(0, 1) == "M") strIsManual = "Y";
                        else strIsManual = "N";

                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File to dicomise " + strFileName, false);

                        if (File.Exists(strFTPDLFLDRTMP + "\\" + strFileName))
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Dicomising " + strFileName, false);
                            if (DicomiseImageFile(Id, strFTPDLFLDRTMP + "\\" + strFileName, strSUID, strInstName, strPatientID, strPName, dtStudy.ToString("ddMMMyyyy") + "_" + dtStudy.ToString("HH:mm:ss"), strModality, strSeriesInstanceID, strSeriesNo))
                            {
                                if (strFileName.Contains('.')) strFileDCM = strFileName.Substring(0, strFileName.LastIndexOf('.'));
                                strFileDCM = strFileDCM + ".dcm";

                                if (TransferFileToPacs(strFTPDLFLDRTMP, strFileDCM, ref strRetMsg))
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "SUCCESS :: Upload to PACS :: SUID : " + strSUID.Trim() + " :: File Name : " + strFileDCM, false);

                                    if (UpdateImageFileTransferToPACSFileCount(Id, strFileName))
                                    {
                                        TransferToArchive(strInstCode, strInstName, strSUID, strFileDCM, strFTPDLFLDRTMP + "\\" + strFileDCM, true, strFTPDLFLDRTMP + "\\" + strFileName, strFileName);
                                    }
                                }
                                else
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FAILED :: Upload to PACS :: SUID : " + strSUID.Trim() + " :: File Name : " + strFileDCM, true);
                                    CreateFileXferFailureNotification(strSUID, strFileDCM, strInstCode, strInstName, strRetMsg);
                                }
                            }

                        }
                        else
                        {
                            if (strIsManual == "N") CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File " + strFTPDLFLDRTMP + "\\" + strFileName + "does not exist", false);
                        }
                    }
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendImgFileToPACS()=>SendImageFileToPACS() :: Exception - " + expErr.Message, false);
            }
            finally
            {
                ds.Dispose();
            }

            System.Threading.Thread.Sleep(intFreq * 1000);
        }
        #endregion

        #region GetFilesToDicomise
        private bool GetFilesToDicomise(ref DataSet ds)
        {

            string strRetMessage = string.Empty;
            string strCatchMessage = string.Empty;
            bool bReturn = false;
            objFP = new FTPPACSSynch();

            try
            {
                if (!objFP.FetchApprovedFilesToDicomise(strConfigPath, ref ds, ref strCatchMessage))
                {
                    bReturn = false;
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendImgFileToPACS()=>SendImageFileToPACS()=>GetFilesToDicomise()=>Core:FetchApprovedFilesToDicomise():: Exception: " + strCatchMessage, true);
                }
                else
                    bReturn = true;
            }
            catch (Exception ex)
            {

                bReturn = false;
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendImgFileToPACS()=>GetFilesToDicomise():: Exception: " + ex.Message, true);
            }
            finally
            {
                objFP = null;
            }

            return bReturn;
        }
        #endregion

        #region DicomiseImageFile
        private bool DicomiseImageFile(Guid Id, string FilePath, string StudyUID, string InstitutionName, string PatientID, string PatientName,
                                        string StudyDate, string Modality, string SeriesUID, string SeriesNumber)
        {
            bool bRet = false;
            string strSUID = string.Empty;
            int exitCode;
            string strFileName = string.Empty;
            string strFileDCM = string.Empty;
            string[] arr = new string[0];
            string strRetMessage = string.Empty;
            string strCatchMessage = string.Empty;
            string strProcArgs = string.Empty;
            objFP = new FTPPACSSynch();

            try
            {
                arr = FilePath.Split('\\');
                strFileName = arr[arr.Length - 1];
                strProcArgs = StudyUID + "±" +
                             string.Empty + "±" +
                             FilePath.Trim().Replace(" ", "»") + "±" +
                             strFTPDLFLDRTMP.Trim().Replace(" ", "»") + "±" +
                             "Y" + "±" +
                             InstitutionName.Replace(" ", "_") + "±" +
                             PatientID + "±" +
                             PatientName + "±" +
                             StudyDate + "±" +
                             Modality + "±" +
                             SeriesUID + "±" +
                             SeriesNumber;

                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DicomiseImageFile() - Exe/Params : " + strIMG2DCMEXEPATH.Trim() + "/" + strProcArgs, false);

                Process ProcImgToDcm = new Process();
                ProcImgToDcm.StartInfo.UseShellExecute = false;
                ProcImgToDcm.StartInfo.FileName = strIMG2DCMEXEPATH.Trim();
                ProcImgToDcm.StartInfo.Arguments = strProcArgs;
                ProcImgToDcm.StartInfo.RedirectStandardOutput = true;
                ProcImgToDcm.Start();
                ProcImgToDcm.WaitForExit();

                exitCode = ProcImgToDcm.ExitCode;

                if (ProcImgToDcm.HasExited)
                {
                    if (exitCode <= 0)
                    {
                        bRet = false;
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DicomiseImageFile() - Conversion To DCM for File : " + strFileName + " failed", true);
                    }
                    else
                    {
                        bRet = true;
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DicomiseImageFile() - Conversion To DCM for File : " + strFileName + " succeeded", false);
                        if (strFileName.Contains('.')) strFileDCM = strFileName.Substring(0, strFileName.LastIndexOf('.'));
                        strFileDCM = strFileDCM + ".dcm";

                        objFP.ID = Id;
                        objFP.FILE_NAME = strFileName.Trim();
                        objFP.DICOM_FILE_NAME = strFileDCM.Trim();


                        if (File.Exists(strFTPDLFLDRTMP.Trim() + "\\" + strFileDCM.Trim()))
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File : " + strFTPDLFLDRTMP.Trim() + "\\" + strFileDCM.Trim(), true);


                        if (!objFP.UpdateImageFileDicomDetails(strConfigPath, ref strRetMessage, ref strCatchMessage))
                        {
                            if (strCatchMessage.Trim() != string.Empty)
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendImgFileToPACS()=>SendImageFileToPACS()=>UpdateImageFileDicomDetails()=>Core:UpdateImageFileDicomDetails():: Exception: " + strCatchMessage, true);
                            else
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendImgFileToPACS()=>SendImageFileToPACS()=>UpdateImageFileDicomDetails()=>Core:UpdateImageFileDicomDetails():: Error : " + strRetMessage, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bRet = false;
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>SendImageFileToPACS()=>DicomiseImageFile():: Exception: " + ex.Message, true);
            }
            finally
            {
                objFP = null;
            }
            return bRet;
        }
        #endregion

        #region UpdateImageFileTransferToPACSFileCount
        private bool UpdateImageFileTransferToPACSFileCount(Guid Id, string strFileName)
        {

            string strRetMessage = string.Empty;
            string strCatchMessage = string.Empty;
            bool bReturn = false;
            objFP = new FTPPACSSynch();

            try
            {
                //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "SendImageFileToPACS()=>UpdateImageFileTransferToPACSFileCount():: Id : " + Convert.ToString(Id) + " File : " + strFileName, false);
                objFP.ID = Id;
                objFP.FILE_NAME = strFileName;

                if (!objFP.UpdateImageFileTransferToPACSFileCount(strConfigPath, ref strRetMessage, ref strCatchMessage))
                {
                    bReturn = false;
                    if (strCatchMessage.Trim() != string.Empty)
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendImgFileToPACS()=>SendImageFileToPACS()=>UpdateImageFileTransferToPACSFileCount()=>Core:UpdateImageFileTransferToPACSFileCount():: Exception: " + strCatchMessage, true);
                    else
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendImgFileToPACS()=>SendImageFileToPACS()=>UpdateImageFileTransferToPACSFileCount()=>Core:UpdateImageFileTransferToPACSFileCount():: Error : " + strRetMessage, true);
                }
                else
                    bReturn = true;
            }
            catch (Exception ex)
            {

                bReturn = false;
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendImgFileToPACS()=>UpdateImageFileTransferToPACSFileCount():: Exception: " + ex.Message, true);
            }
            finally
            {
                objFP = null;
            }

            return bReturn;
        }
        #endregion

        #endregion

        #region Transfer File Common Methods

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
                ProcXfer.StartInfo.Arguments = strXFEREXEPARMS + " " + strFolder + "\\" + strFile;
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

        #region TransferToArchive
        private bool TransferToArchive(string InstitutionCode, string InstitutionName, string StudyUID, string FileName, string FilePathToMove, bool IsImageFile, string ImageFilePath, string ImageFileName)
        {
            string strFolder = InstitutionCode + "_" + InstitutionName + "_" + StudyUID;
            string strRetMessage = string.Empty;
            string strCatchMessage = string.Empty;
            bool bRet = true;
            FTPPACSSynch objFPArch2 = new FTPPACSSynch();

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

                if (IsImageFile)
                {
                    if (!System.IO.Directory.Exists(strPACSARCHIVEFLDR + "\\" + strFolder + "\\Images"))
                    {
                        System.IO.Directory.CreateDirectory(strPACSARCHIVEFLDR + "\\" + strFolder + "\\Images");
                    }

                    if (File.Exists(strPACSARCHIVEFLDR + "\\" + strFolder + "\\Images\\" + ImageFileName))
                    {
                        System.IO.File.Delete(strPACSARCHIVEFLDR + "\\" + strFolder + "\\Images\\" + ImageFileName);
                    }

                    System.IO.File.Move(ImageFilePath, strPACSARCHIVEFLDR + "\\" + strFolder + "\\Images\\" + ImageFileName);
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ImageFileName + " archived", false);
                }

                objFPArch2.STUDY_UID = StudyUID;
                if (!objFPArch2.UpdateArchivedFileCount(strConfigPath, ref strRetMessage, ref strCatchMessage))
                {
                    if (strCatchMessage.Trim() != string.Empty)
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>TransferForwardeFileToArchive()=>UpdateArchivedFileCount()- File :" + FileName + "::Exception: " + strCatchMessage.Trim(), true);
                    else
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles()=>ProcessForwardedFiles()=>TransferForwardeFileToArchive()=>UpdateArchivedFileCount()- File :" + FileName + "::Error: " + strRetMessage.Trim(), true);
                }
                //else
                //    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Archived file count of file :" + FileName + ", Study UID :" + StudyUID + " updated", false);
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doSendFileToPACS()=>SendFileToPACS()=>TransferToArchive()- File :" + FileName + ":: Exception: " + ex.Message, true);
                bRet = false;
            }
            finally
            {
                objFPArch2 = null;
            }

            return bRet;
        }
        #endregion

        #endregion

        #endregion


        #region DICOM File Methods

        #region GetStudyUID
        private string GetStudyUID(List<string> str)
        {

            string UserCaseID = string.Empty;
            string s1, s4, s5, s11, s12;

            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                if ((s11.ToUpper() == "0020") && (s12.ToUpper() == "000D"))
                {
                    UserCaseID = s5.Replace("\0", "");
                    break;
                }

            }
            return UserCaseID;

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

        #region GetSeriesUIDFromDump
        private string GetSeriesUIDFromDump(string strFile)
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
        private string GetSOPInstanceUIDFromDump(string strFile)
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

        #region GetInstitutionName
        private string GetInstitutionName(List<string> str)
        {

            string InstName = string.Empty;
            string s1, s4, s5, s11, s12;

            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                if ((s11.ToUpper() == "0008") && (s12.ToUpper() == "0080"))
                {
                    InstName = s5.Replace("\0", "");
                    break;
                }

            }
            return InstName;

        }
        #endregion

        #region GetTagValue
        private string GetTagValue(List<string> str, string strGroupID, string strElementID)
        {

            string strTagValue = string.Empty;
            string s1, s4, s5, s11, s12;

            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                if ((s11.ToUpper() == strGroupID) && (s12.ToUpper() == strElementID))
                {
                    strTagValue = s5.Replace("\0", "");
                    break;
                }

            }
            return strTagValue;

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

                #region commented
                /*if ((s11.ToUpper() == "0008") && (s12.ToUpper() == "103E"))
                {
                    strDescription = s5.Replace("\0", "");
                    strDescription = s5.Replace("<", " ");
                    strDescription = s5.Replace(">", " ");

                }

                else if ((s11.ToUpper() == "0008") && (s12.ToUpper() == "0060"))
                {
                    ModalityID = s5.Replace("\0", "");

                }


                else if ((s11.ToUpper() == "0010") && (s12.ToUpper() == "0010"))
                {
                    Strname = s5.Replace("\0", "");
                    Strname = s5.Replace("^", " ");

                }
                else if ((s11.ToUpper() == "0010") && (s12.ToUpper() == "0030"))
                {
                    DOB = s5.Replace("\0", "");
                    DOB = DOB.Trim();
                    if (DOB != "")
                    {
                        string yy = DOB.Substring(0, 4);
                        string MM = DOB.Substring(4, 2);
                        string DD = DOB.Substring(6, 2);
                        result = yy + "-" + MM + "-" + DD;
                    }
                    else
                    {
                        result = "0000-00-00";
                    }
                }

                else */
                #endregion

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

        #region IgnoreBadCertificates
        public static void IgnoreBadCertificates()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
        }
        #endregion

        #region AcceptAllCertifications
        private static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        #endregion

        #endregion

        #region CreateDRLogTable
        private DataTable CreateDRLogTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("institution_code", System.Type.GetType("System.String"));
            dtbl.Columns.Add("service_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("service_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("log_date", System.Type.GetType("System.DateTime"));
            dtbl.Columns.Add("log_message", System.Type.GetType("System.String"));
            dtbl.Columns.Add("is_error", System.Type.GetType("System.String"));
            dtbl.TableName = "Log";
            return dtbl;
        }
        #endregion

        #region doCheckMissingSessionFiles
        private void doCheckMissingSessionFiles()
        {
            string strFolder = strFTPDLFLDRTMP;
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string[] fileElements = new string[0];
            string strFileName = string.Empty;
            string strSID = string.Empty;
            string strIsMissing = string.Empty;
            string strInstCode = string.Empty;
            string strInstName = string.Empty;
            string strSUID = string.Empty;
            string strSentToPacs = string.Empty;
            string strCatchMessage = string.Empty;
            string strExtn = string.Empty;
            FTPPACSSynch objFPMS = new FTPPACSSynch();

            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Checking missing session files", false);
                arrFiles = Directory.GetFiles(strFTPDLFLDRTMP);

                foreach (string strFile in arrFiles)
                {
                    pathElements = strFile.Split('\\');
                    strFileName = pathElements[(pathElements.Length - 1)];

                    if (strFileName.Contains('_'))
                    {
                        fileElements = strFileName.Split('_');
                        if (fileElements.Length > 2)
                        {
                            strSID = fileElements[1].Trim();
                            strExtn = Path.GetExtension(strFile);

                            if (strSID.Substring(0, 6) != "S1DXXX")
                            {
                                if (CoreCommon.IsDicomFile(strFile))
                                {
                                    #region DICOM files
                                    if (strExtn.ToUpper() != ".BAK")
                                    {
                                        strIsMissing = "N";
                                        objFPMS.IMPORT_SESSION_ID = strSID;
                                        objFPMS.FILE_NAME = strFileName.Trim();
                                        objFPMS.FILE_TYPE = "D";
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Checking missing info for file : " + strFileName + " Session :" + strSID, false);
                                        if (objFPMS.CheckMissingSessionFiles(strConfigPath, ref strIsMissing, ref strSUID, ref strInstName, ref strInstCode, ref strSentToPacs, ref strCatchMessage))
                                        {
                                            if (strIsMissing == "Y")
                                            {
                                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCheckMissingSessionFiles() - Missing File : " + strFileName + " - Update DB", false);
                                                UpdateDownloadedFilesRecords(strFileName, string.Empty);
                                            }
                                            else
                                            {
                                                if (strSentToPacs == "Y")
                                                {
                                                    TransferToArchive(strInstCode, strInstName, strSUID, strFileName, strFTPDLFLDRTMP + "\\" + strFileName, false, string.Empty, string.Empty);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCheckMissingSessionFiles() - File : " + strFileName + " - Error : " + strCatchMessage, true);
                                        }
                                    }
                                    #endregion
                                }
                                else if ((MIMEAssistant.GetMIMEType(strFile) == "image/jpeg") || (MIMEAssistant.GetMIMEType(strFile) == "image/gif") || (MIMEAssistant.GetMIMEType(strFile) == "image/png") || (MIMEAssistant.GetMIMEType(strFile) == "image/bmp"))
                                {
                                    #region ImageFiles

                                    strIsMissing = "N";
                                    objFPMS.IMPORT_SESSION_ID = strSID;
                                    objFPMS.FILE_NAME = strFileName.Trim();
                                    objFPMS.FILE_TYPE = "I";

                                    if (objFPMS.CheckMissingSessionFiles(strConfigPath, ref strIsMissing, ref strSUID, ref strInstName, ref strInstCode, ref strSentToPacs, ref strCatchMessage))
                                    {
                                        if (strIsMissing == "Y")
                                        {
                                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCheckMissingSessionFiles() - File : " + strFileName + " - Update DB", false);
                                            UpdateDownloadedFilesRecords(strFileName, string.Empty);
                                        }
                                        else if (strSentToPacs == "Y")
                                        {
                                            if (File.Exists(strFile)) File.Delete(strFile);
                                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCheckMissingSessionFiles() - File : " + strFileName + " - Deleted", false);
                                        }
                                    }
                                    else
                                    {
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCheckMissingSessionFiles() - File : " + strFileName + " - Error : " + strCatchMessage, true);
                                    }

                                    #endregion
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCheckMissingSessionFiles() - Exception: " + expErr.Message, true);
            }
            finally
            {
                objFPMS = null;
            }
        }
        #endregion

        #region IsFileLocked
        private bool IsFileLocked(Exception exception)
        {
            int errorCode = Marshal.GetHRForException(exception) & ((1 << 16) - 1);
            return errorCode == ERROR_SHARING_VIOLATION || errorCode == ERROR_LOCK_VIOLATION;
        }
        #endregion

        #region UnlockFileProcess
        private void UnlockFileProcess(string strFilePath)
        {
            List<Process> ProcList = HandleFileLock.GetProcessesLockingFile(strFilePath);
            foreach (var process in ProcList)
            {
                process.Kill();
            }
        }
        #endregion

        #region IsZipValid
        private bool IsZipValid(string path)
        {
            try
            {
                using (var zipFile = ZipFile.OpenRead(path))
                {
                    var entries = zipFile.Entries;
                    return true;
                }
            }
            catch (InvalidDataException)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region Upload Files from databse to repository folder

        #region doUploadFiles
        private void doUploadFiles()
        {
            try
            {
                while (true)
                {
                    UploadManualSubmissions();
                    System.Threading.Thread.Sleep(intFreq * 1000);
                }
            }
            catch (Exception expErr)
            {
                //EventLog.WriteEntry(strSvcName, "doUploadFiles()=>Exception : " + expErr.Message, EventLogEntryType.Error);
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doUploadFiles() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }


        }
        #endregion

        #region UploadManualSubmissions
        private void UploadManualSubmissions()
        {
            //string[] pathElement = new string[0];
            //string strFileName = string.Empty;
            FTPPACSSynch objFPMS = new FTPPACSSynch();
            DataSet ds = new DataSet();
            Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
            string strCatchMsg = string.Empty;
            string strReturnMsg = string.Empty;
            string strInstitutionCode = string.Empty;
            string strInstitutionName = string.Empty;
            string strSourcePath = string.Empty;
            string strFile = string.Empty;
            string strFilePath = string.Empty;
            string strExtn = string.Empty;
            string strSID = string.Empty;
            string strZipPath = string.Empty;
            string strTargetPath = string.Empty;

            try
            {
                if (!objFPMS.FetchManuallySubmittedFilesToUpload(strConfigPath, ref ds, ref strCatchMsg))
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doUploadFiles()=>UploadManualSubmissions()=>FetchManuallySubmittedFilesToUpload():Core::Exception - " + strCatchMsg, true);
                }
                else
                {
                    strSourcePath = strConfigPath + "\\TempManualFiles";
                    foreach (DataRow dr in ds.Tables["FileList"].Rows)
                    {
                        Id = new Guid(Convert.ToString(dr["file_id"]).Trim());
                        strSID = Convert.ToString(dr["session_id"]).Trim();
                        strFile = Convert.ToString(dr["file_name"]).Trim();
                        strInstitutionCode = Convert.ToString(dr["institution_code"]).Trim();
                        strInstitutionName = Convert.ToString(dr["institution_name"]).Trim();
                        strFile = strFile.Replace(strSID + "_", "");
                        strFile = strInstitutionCode + "_" + strSID + "_" + strInstitutionName.Replace(" ", "_") + "_" + strFile;

                        strFile = strFile.Replace(" ", "_");
                        strFile = strFile.Replace(",", "");
                        strFile = strFile.Replace("(", "");
                        strFile = strFile.Replace(")", "");
                        strFile = strFile.Replace("'", "");
                        strFile = strFile.Replace("\"", "_");
                        strFile = strFile.Replace("/", "_");
                        strFile = strFile.Replace("\\", "_");
                        strFile = strFile.Replace("#", "");
                        strFile = strFile.Replace("&", "");
                        strFile = strFile.Replace("@", "");
                        strFile = strFile.Replace("?", "");
                        strFile = strFile.Replace("__", "_");

                        strFilePath = strSourcePath + "/" + strFile.Trim();
                        SetFile((byte[])dr["file_content"], strSourcePath, strFilePath);
                        strExtn = Path.GetExtension(strFilePath);

                        #region Compress File
                        try
                        {
                            if (strExtn.Trim() != string.Empty)
                            {
                                if (strFile.Contains(strExtn))
                                    strFile = strFile.Replace(strExtn, string.Empty);
                            }

                            if (File.Exists(strSourcePath + "\\" + strFile + ".zip")) File.Delete(strSourcePath + "\\" + strFile + ".zip");
                            strZipPath = strSourcePath + "\\" + strFile + ".zip";

                            using (ZipArchive zip = ZipFile.Open(strZipPath, ZipArchiveMode.Create))
                            {
                                zip.CreateEntryFromFile(strFilePath, strFile + strExtn);
                            }
                        }
                        catch (Exception ex)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doUploadFiles()=>UploadManualSubmissions() - File Compression - Exception: " + ex.Message, true);
                        }
                        #endregion

                        #region Move the .zip file
                        try
                        {
                            strTargetPath = strFTPDLFLDRTMP + "\\" + strFile + ".zip";
                            if (File.Exists(strTargetPath)) File.Delete(strTargetPath);
                            File.Move(strZipPath, strTargetPath);
                        }
                        catch (Exception ex)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doUploadFiles()=>UploadManualSubmissions() - Move Zip file to FTP Folder - Exception: " + ex.Message, true);
                        }
                        #endregion

                        #region Delete file entry
                        try
                        {
                            objFPMS.ID = Id;
                            if (!objFPMS.DeleteManualFileEntry(strConfigPath, intServiceID, strSvcName, ref strReturnMsg, ref strCatchMsg))
                            {
                                if (strCatchMsg.Trim() != string.Empty) CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()==>doUploadFiles()==>DeleteManualFileEntry():Core::Exception - " + strCatchMsg, true);
                            }
                        }
                        catch (Exception ex)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doUploadFiles()=>UploadManualSubmissions() - Delte File Entry - Exception: " + ex.Message, true);
                        }
                        #endregion

                    }
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doUploadFiles()=>UploadManualSubmissions() - Exception: " + ex.Message, true);
            }
            finally
            {
                objFPMS = null;
                ds.Dispose();
            }
        }
        #endregion

        #region File Methods

        #region SetFile
        private void SetFile(byte[] FileData, string strDirPath, string strFilePath)
        {


            if (!Directory.Exists(strDirPath)) Directory.CreateDirectory(strDirPath);

            using (FileStream fs = new FileStream(strFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(FileData, 0, FileData.Length);
                fs.Flush();
                fs.Close();
            }

        }
        #endregion

        #region GetFileBytes
        private byte[] GetFileBytes(string strFileName)
        {
            byte[] buff = File.ReadAllBytes(strFileName);
            return buff;
        }
        #endregion

        #endregion

        #endregion

        #region Arrange Recieved DCM Files

        #region doArrangeFiles
        private void doArrangeFiles()
        {

            try
            {
                ArrangeFiles();

            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()==>doArrangeFiles() - Exception: " + expErr.Message, true);
            }
        }
        #endregion

        #region ArrangeFiles
        private void ArrangeFiles()
        {
            string[] dirs = new string[0];
            string strSID = string.Empty;

            try
            {
                dirs = Directory.GetDirectories(strDCMRCVRFLDR);

                for (int i = 0; i < dirs.Length; i++)
                {
                    // CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "ArrangeFiles() =>Directory List: " + dirs[i].Trim(), false);
                    if (dirs[i].Trim() != strDCMRCVRFLDR + "\\InfoRequired")
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(dirs[i]);
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "ArrangeFiles() =>Directory: " + dirInfo.FullName, false);
                        strSID = "S1D" + DateTime.Now.ToString("MMddyyHHmmss") + CoreCommon.RandomString(3);
                        WalkDirectoryTree(dirInfo, strSID);
                        if (Directory.Exists(dirInfo.FullName)) DeleteEmptyDirectoryTree(dirInfo);
                        dirInfo = null;
                    }
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doDownloadFiles()=> doArrangeFiles() => ArrangeFiles() - Exception: " + ex.Message, true);
            }

        }
        #endregion

        #region WalkDirectoryTree
        private void WalkDirectoryTree(System.IO.DirectoryInfo root, string SessionID)
        {
            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "WalkDirectoryTree =>Directory: " + root.FullName, false);
            string[] pathElements = new string[0];
            string[] fileElements = new string[0];
            string[] arr = new string[0];
            DicomDecoder dd = new DicomDecoder();
            //int isDiacom = -1;

            string strSUID = string.Empty;
            string strInstName = string.Empty;
            string strFile = string.Empty;
            string strFilename = string.Empty;
            string strNewFilename = string.Empty;
            string strNewFilePath = string.Empty;
            string strParentFolder = string.Empty;
            string strMIMEType = string.Empty;
            string strDirName = string.Empty;
            string[] arrFiles = new string[0];
            string strPrefix = string.Empty;
            string strExtn = string.Empty;
            string strZipPath = string.Empty;
            string strTargetPath = string.Empty;

            int intRejCount = 0;
            int intIgnoreCount = 0;
            string strCatchMsg = string.Empty;
            objFP = new FTPPACSSynch();
            objCore = new Scheduler();

            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;


            #region get file list
            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.*");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException ex)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
                ;
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                //Console.WriteLine(e.Message);
                ;
            }
            #endregion

            if (files != null)
            {
                try
                {
                    foreach (System.IO.FileInfo fi in files)
                    {

                        strFile = fi.FullName;

                        if (CheckValidFileFormat(strFile))
                        {
                            #region queuing files
                            strDirName = root.Name;
                            strFilename = fi.Name;
                            dd.DicomFileName = strFile;
                            List<string> str = dd.dicomInfo;
                            arr = new string[20];
                            arr = GetallTags(str);

                            if ((arr[5].Trim().ToUpper() != string.Empty) && (arr[0].Trim().ToUpper() != string.Empty))
                            {
                                objCore.INSTITUTION_NAME = arr[5].Trim();
                                if (!objCore.FetchInstitutionInfo(strConfigPath, ref strCatchMsg))
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()=>doArrangeFiles()=>ArrangeFiles()=>WalkDirectoryTree()=>FetchInstitutionInfo():Core::Exception - " + strCatchMsg, true);
                                }
                                else
                                {
                                    if (objCore.INSTITUTION_CODE.Trim() == string.Empty)
                                    {
                                        if (objCore.INSTITUTION_ID == new Guid("00000000-0000-0000-0000-000000000000"))
                                        {
                                            #region create new institution
                                            objCore.INSTITUTION_NAME = arr[5].Trim();
                                            if (objCore.CreateNewInstitution(strConfigPath, intServiceID, strSvcName, ref strCatchMsg))
                                            {
                                                #region create notification
                                                if (!objCore.CreateNewInstitutionNotification(strConfigPath, intServiceID, strSvcName, ref strCatchMsg))
                                                {
                                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()=>doArrangeFiles()=>ArrangeFiles()=>WalkDirectoryTree()=>CreateNewInstitutionNotification():Core::Exception - " + strCatchMsg, true);
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()=>doArrangeFiles()=>ArrangeFiles()=>WalkDirectoryTree()=>CreateNewInstitution():Core::Exception - " + strCatchMsg, true);
                                            }
                                            #endregion
                                        }
                                        intIgnoreCount = intIgnoreCount + 1;

                                    }
                                    else
                                    {
                                        #region save the file after renaming
                                        strPrefix = CoreCommon.RandomString(6);
                                        strExtn = Path.GetExtension(strFile);

                                        strNewFilename = objCore.INSTITUTION_CODE.Trim() + "_" + SessionID + "_" + objCore.INSTITUTION_NAME.Trim().Replace(" ", "_") + "_" + strPrefix + "_" + strFilename;
                                        strNewFilename = strNewFilename.Replace(" ", "_");
                                        strNewFilename = strNewFilename.Replace("(", "");
                                        strNewFilename = strNewFilename.Replace(")", "");
                                        strNewFilename = strNewFilename.Replace("'", "");
                                        strNewFilename = strNewFilename.Replace("\"", "");
                                        strNewFilename = strNewFilename.Replace("#", "");
                                        strNewFilename = strNewFilename.Replace("&", "");
                                        strNewFilename = strNewFilename.Replace("@", "");
                                        strNewFilePath = root.FullName + "/" + strNewFilename;

                                        if (File.Exists(strFile))
                                        {

                                            File.Move(strFile, strNewFilePath);
                                            if (File.Exists(strFile)) File.Delete(strFile);
                                        }
                                        #endregion

                                        #region compress the file
                                        try
                                        {

                                            if (strExtn.Trim() != string.Empty)
                                            {
                                                if (strNewFilename.Contains(strExtn))
                                                    strNewFilename = strNewFilename.Replace(strExtn, string.Empty);
                                            }
                                            if (File.Exists(root.FullName + "/" + strNewFilename + ".zip")) File.Delete(root.FullName + "/" + strNewFilename + ".zip");
                                            strZipPath = root.FullName + "/" + strNewFilename + ".zip";

                                            using (ZipArchive zip = ZipFile.Open(strZipPath, ZipArchiveMode.Create))
                                            {
                                                zip.CreateEntryFromFile(strNewFilePath, strNewFilename + strExtn);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doDownloadFiles()=>doArrangeFiles()=>ArrangeFiles()=> WalkDirectoryTree() - File Compression - Exception: " + ex.Message, true);
                                        }
                                        #endregion

                                        #region Move the .zip file
                                        try
                                        {
                                            strTargetPath = strFTPSRCFOLDER + "\\" + strNewFilename + ".zip";
                                            if (File.Exists(strTargetPath)) File.Delete(strTargetPath);
                                            File.Move(strZipPath, strTargetPath);
                                        }
                                        catch (Exception ex)
                                        {
                                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doDownloadFiles()>doArrangeFiles()=>ArrangeFiles()=> WalkDirectoryTree() - Move Zip file to FTP Folder - Exception: " + ex.Message, true);
                                        }
                                        #endregion

                                        #region Delete file
                                        try
                                        {
                                            if (File.Exists(strNewFilePath)) File.Delete(strNewFilePath);
                                        }
                                        catch (Exception ex)
                                        {
                                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doDownloadFiles()=> UploadManualSubmissions() - Delete File Entry - Exception: " + ex.Message, true);
                                        }
                                        #endregion
                                    }
                                }

                            }
                            else
                            {
                                if (File.Exists(strFile))
                                {
                                    File.Move(strFile, root.FullName + "/" + strFilename);
                                    intRejCount = intRejCount + 1;
                                }
                            }
                            #endregion

                        }
                        else
                        {
                            #region delete file
                            if (File.Exists(strFile)) File.Delete(strFile);
                            #endregion
                        }
                    }
                }
                catch (Exception ex)
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doDownloadFiles()=>doArrangeFiles()=>ArrangeFiles()=> WalkDirectoryTree() File Queuing - Exception: " + ex.Message, true);
                }

                try
                {
                    // Now find all the subdirectories under this directory.
                    subDirs = root.GetDirectories();
                    if (subDirs.Length > 0)
                    {
                        foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                        {
                            // Resursive call for each subdirectory.
                            WalkDirectoryTree(dirInfo, SessionID);

                        }
                    }
                    else
                    {
                        if (Directory.Exists(root.FullName))
                        {
                            if (root.GetFiles("*.*").Length == 0) Directory.Delete(root.FullName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doDownloadFiles()=>doArrangeFiles()=>ArrangeFiles()=> WalkDirectoryTree() Sub Directory Deletion - Exception: " + ex.Message, true);
                }

            }

            objFP = null;
            objCore = null;
        }
        #endregion

        #region CheckValidFileFormat
        private bool CheckValidFileFormat(string strFilePath)
        {
            bool bRet = false;
            string[] pathElements = new string[0];

            if (CoreCommon.IsDicomFile(strFilePath))
            {
                pathElements = strFilePath.Split('\\');
                if (pathElements[pathElements.Length - 1].Trim().ToUpper() == "DICOMDIR")
                    bRet = false;
                else
                    bRet = true;
            }
            else if ((MIMEAssistant.GetMIMEType(strFilePath) == "image/jpeg") || (MIMEAssistant.GetMIMEType(strFilePath) == "image/gif") || (MIMEAssistant.GetMIMEType(strFilePath) == "image/png") || (MIMEAssistant.GetMIMEType(strFilePath) == "image/bmp"))
            {
                bRet = true;
            }
            else
            {
                // MessageBox.Show("Invalid file format : " + strFilePath, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bRet = false;
            }


            return bRet;
        }
        #endregion

        #region DeleteEmptyDirectoryTree
        private void DeleteEmptyDirectoryTree(System.IO.DirectoryInfo root)
        {
            System.IO.DirectoryInfo[] subDirs = null;
            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteEmptyDirectoryTree =>Directory: " + root.FullName, false);

            try
            {
                subDirs = root.GetDirectories();
                if (subDirs.Length > 0)
                {
                    foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                    {
                        // Resursive call for each subdirectory.
                        DeleteEmptyDirectoryTree(dirInfo);

                    }
                }
                else
                {
                    if (Directory.Exists(root.FullName))
                    {
                        if (root.GetFiles("*.*").Length == 0) Directory.Delete(root.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doDownloadFiles()=>doArrangeFiles()=>ArrangeFiles()=> DeleteEmptyDirectoryTree() - Exception: " + ex.Message, true);
            }



        }
        #endregion

        #endregion

        #region Check On Hold DCM Files

        #region doCheckOnHoldFiles
        private void doCheckOnHoldFiles()
        {

            try
            {
                CheckOnHoldFiles();

            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()==>doCheckOnHoldFiles() - Exception: " + expErr.Message, true);
            }
            doCheckMissingSessionFiles(); 
        }
        #endregion

        #region CheckOnHoldFiles
        private void CheckOnHoldFiles()
        {
            string strCatchMessage = string.Empty;
            string strRetMessage = string.Empty;
            string strFilePath = string.Empty;
            string strFileName = string.Empty;
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
            List<string> arrSUID = new List<string>();
            DicomDecoder dd = new DicomDecoder();

            #region TAG Variables
            string strSUID = string.Empty;
            string strInstCode = string.Empty;
            string strInstName = string.Empty;
            #endregion


            objFP = new FTPPACSSynch();
            objCore = new Scheduler();

            try
            {

                arrFiles = Directory.GetFiles(strFILESHOLDPATH);

                foreach (string strFile in arrFiles)
                {

                    pathElements = strFile.Replace("\\", "/").Split('/');
                    strFileName = pathElements[pathElements.Length - 1];
                    strSID = (strFileName.Split('_'))[1].Trim();

                    dd.DicomFileName = strFile;
                    List<string> str = dd.dicomInfo;

                    arr = new string[20];
                    arr = GetallTags(str);
                    strSUID = arr[0].Trim();
                    if (strSUID.Trim() == string.Empty) strSUID = GetStudyUIDFromDump(strFile);

                    if (strSUID.Trim() != string.Empty)
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()==>doCheckOnHoldFiles()==>CheckOnHoldFiles()::File : " + strFileName.Trim() + " get tag data ", false);

                        try
                        {
                            #region Get File Data
                            strInstName = arr[5].Trim();
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()==>doCheckOnHoldFiles()==>CheckOnHoldFiles()::File : " + strFileName.Trim() + " get tag data  :: " + ex.Message.Trim(), true);
                        }

                        if (strInstName.Trim().ToUpper() != string.Empty && strInstName.Trim().ToUpper() != "Y")
                        {
                            #region check by institution Name
                            try
                            {
                                objCore.INSTITUTION_NAME = strInstName.Trim();
                                if (!objCore.FetchInstitutionInfo(strConfigPath, ref strCatchMessage))
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()==>doCheckOnHoldFiles()==>CheckOnHoldFiles()=>FetchInstitutionInfo():Core::Exception - " + strCatchMessage, true);
                                }

                                if (objCore.INSTITUTION_CODE.Trim() != string.Empty)
                                {


                                    #region save the file after renaming
                                    strPrefix = CoreCommon.RandomString(6);
                                    strExtn = Path.GetExtension(strFilePath);
                                    strInstCode = objCore.INSTITUTION_CODE.Trim();
                                    strInstName = objCore.INSTITUTION_NAME.Trim();
                                    if (strExtn.Trim() != string.Empty)
                                        strNewFilename = objCore.INSTITUTION_CODE.Trim() + "_" + strSID + "_" + objCore.INSTITUTION_NAME.Trim().Replace(" ", "_") + "_" + strPrefix + "_" + strSUID.Replace(".", "").Substring(0, 5) + strExtn;
                                    else
                                        strNewFilename = objCore.INSTITUTION_CODE.Trim() + "_" + strSID + "_" + objCore.INSTITUTION_NAME.Trim().Replace(" ", "_") + "_" + strPrefix + "_" + strSUID.Replace(".", "").Substring(0, 5);

                                    strNewFilename = strNewFilename.Replace(" ", "_");
                                    strNewFilename = strNewFilename.Replace("(", "");
                                    strNewFilename = strNewFilename.Replace(")", "");
                                    strNewFilename = strNewFilename.Replace("'", "");
                                    strNewFilename = strNewFilename.Replace("\"", "");
                                    strNewFilename = strNewFilename.Replace("#", "");
                                    strNewFilename = strNewFilename.Replace("&", "");
                                    strNewFilename = strNewFilename.Replace("@", "");
                                    strNewFilename = strNewFilename.Replace("?", "");
                                    strNewFilename = strNewFilename.Replace("\\", "");
                                    strNewFilename = strNewFilename.Replace("/", "_");

                                    strNewFilePath = strFILESHOLDPATH + "/" + strNewFilename;

                                    if (File.Exists(strFilePath))
                                    {

                                        File.Move(strFilePath, strNewFilePath);
                                        if (File.Exists(strFilePath)) File.Delete(strFilePath);

                                    }
                                    #endregion

                                    if (File.Exists(strFTPDLFLDRTMP + "/" + strNewFilename)) File.Delete(strFTPDLFLDRTMP + "/" + strNewFilename);
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()=>doCheckOnHoldFiles()=>CheckOnHoldFiles()::File : " + strFileName + " renamed to " + strNewFilename, false);
                                    File.Move(strFILESHOLDPATH + "/" + strFileName, strFTPSRCFOLDER + "/" + strNewFilename);
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()==>doCheckOnHoldFiles()==>CheckOnHoldFiles()::File - " + strNewFilename + " Site code found, moved back to " + strFTPSRCFOLDER, false);
                                    //objFP = null;
                                    //UpdateDownloadedListenerFileRecords(strFileName, strSID);

                                }
                                else
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()==>doCheckOnHoldFiles()==>CheckOnHoldFiles()::File : " + strFileName.Trim() + ":: Site code not found for institution " + strInstName.Trim(), true);
                                }
                            }
                            catch (Exception ex)
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()==>doCheckOnHoldFiles()==>CheckOnHoldFiles()::File : " + strFileName.Trim() + ":: " + ex.Message.Trim(), true);
                            }
                            #endregion
                        }
                        else if (strSUID.Trim() != string.Empty)
                        {
                            #region check by study UID
                            try
                            {
                                objCore.STUDY_UID = strSUID.Trim();
                                if (!objCore.FetchInstitutionInfoByStudyUID(strConfigPath, ref strCatchMessage))
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()==>doCheckOnHoldFiles()==>CheckOnHoldFiles()=>FetchInstitutionInfoByStudyUID():Core::Exception - " + strCatchMessage, true);
                                }

                                if (objCore.STUDY_EXISTS == "Y")
                                {
                                    if (objCore.INSTITUTION_CODE.Trim() != string.Empty)
                                    {
                                        #region save the file after renaming
                                        strPrefix = CoreCommon.RandomString(6);
                                        strExtn = Path.GetExtension(strFilePath);
                                        strInstCode = objCore.INSTITUTION_CODE.Trim();
                                        strInstName = objCore.INSTITUTION_NAME.Trim();

                                        ModifyInstitutionName(strInstName, strSUID, strFile);

                                        if (strExtn.Trim() != string.Empty)
                                            strNewFilename = objCore.INSTITUTION_CODE.Trim() + "_" + strSID + "_" + objCore.INSTITUTION_NAME.Trim().Replace(" ", "_") + "_" + strPrefix + "_" + strSUID.Replace(".", "").Substring(0, 5) + strExtn;
                                        else
                                            strNewFilename = objCore.INSTITUTION_CODE.Trim() + "_" + strSID + "_" + objCore.INSTITUTION_NAME.Trim().Replace(" ", "_") + "_" + strPrefix + "_" + strSUID.Replace(".", "").Substring(0, 5);

                                        strNewFilename = strNewFilename.Replace(" ", "_");
                                        strNewFilename = strNewFilename.Replace("(", "");
                                        strNewFilename = strNewFilename.Replace(")", "");
                                        strNewFilename = strNewFilename.Replace("'", "");
                                        strNewFilename = strNewFilename.Replace("\"", "");
                                        strNewFilename = strNewFilename.Replace("#", "");
                                        strNewFilename = strNewFilename.Replace("&", "");
                                        strNewFilename = strNewFilename.Replace("@", "");
                                        strNewFilename = strNewFilename.Replace("?", "");
                                        strNewFilename = strNewFilename.Replace("\\", "");
                                        strNewFilename = strNewFilename.Replace("/", "_");

                                        strNewFilePath = strFILESHOLDPATH + "/" + strNewFilename;

                                        if (File.Exists(strFilePath))
                                        {

                                            File.Move(strFilePath, strNewFilePath);
                                            if (File.Exists(strFilePath)) File.Delete(strFilePath);

                                        }
                                        #endregion

                                        if (File.Exists(strFTPDLFLDRTMP + "/" + strNewFilename)) File.Delete(strFTPDLFLDRTMP + "/" + strNewFilename);
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()=>doCheckOnHoldFiles()=>CheckOnHoldFiles()::File : " + strFileName + " renamed to " + strNewFilename, false);
                                        File.Move(strFILESHOLDPATH + "/" + strFileName, strFTPSRCFOLDER + "/" + strNewFilename);
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()==>doCheckOnHoldFiles()==>CheckOnHoldFiles()::File - " + strNewFilename + " Site code found, moved back to " + strFTPSRCFOLDER, false);
                                        //objFP = null;
                                        //UpdateDownloadedListenerFileRecords(strFileName, strSID);

                                    }
                                    else
                                    {
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()==>doCheckOnHoldFiles()==>CheckOnHoldFiles()::File : " + strFileName.Trim() + ":: Site code not found for institution " + strInstName.Trim(), true);
                                    }
                                }
                                else
                                {
                                    if (File.Exists(strFile)) File.Delete(strFile);
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()=>doCheckOnHoldFiles()=>CheckOnHoldFiles()::File : " + strFileName + " deleted. Study UID " + strSUID + " does not exist.", false);
                                }
                            }
                            catch (Exception ex)
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()==>doCheckOnHoldFiles()==>CheckOnHoldFiles()::File : " + strFileName.Trim() + ":: " + ex.Message.Trim(), true);
                            }
                            #endregion
                        }
                        else
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()==>doCheckOnHoldFiles()==>CheckOnHoldFiles()::File - " + strFileName + "  Institution name missing", false);
                        }
                    }
                    else
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()==>doCheckOnHoldFiles()==>CheckOnHoldFiles()::File : " + strFileName.Trim() + " Study UID missing ", true);
                    }
                }



            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessDownload()==>doCheckOnHoldFiles()==>CheckOnHoldFiles():: Exception: " + expErr.Message, true);
            }

            finally
            {
                dd = null;
                objFP = null;
                objCore = null;
            }

        }
        #endregion

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
