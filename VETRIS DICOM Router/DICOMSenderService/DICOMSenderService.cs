using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Net;
using System.Net.Http;
using System.IO;
using System.IO.Compression;
using System.Configuration;
using System.Security;
using System.Drawing.Imaging;
using Microsoft.VisualBasic;
using DICOMLib;
using VETRISRouter.Core;

namespace DICOMSenderService
{
    public partial class DICOMSenderService : ServiceBase
    {
        #region members & variables
        private static int intFreq = 10;
        private static string strURL = string.Empty;
        private static string strConfigPath = AppDomain.CurrentDomain.BaseDirectory;
        private static string strWinHdr = "VETRIS DICOM ROUTER";
        private static int intSvcID = 2;
        private static string strSvcName = "Dicom Sending Service";

        string strEXEPATH = string.Empty;
        string strSNDEXENAME = string.Empty;
        string strSNDEXEOPTIONS = string.Empty;
        string strRCVDIR = string.Empty;
        string strSNDDIR = string.Empty;
        string strARCHDIR = string.Empty;
        string strPACSSRVRNAME = string.Empty;
        string strINSTNAME = string.Empty;
        string strSITECODE = string.Empty;

        string strTRANSFERFTP = string.Empty;
        string strFTPHOST = string.Empty;
        string strFTPPORT = string.Empty;
        string strFTPUSER = string.Empty;
        string strFTPPWD = string.Empty;
        string strFTPTEMPFLDR = string.Empty;
        string strFTPDWLDFLDR = string.Empty;
        string strFTPLOGDWLDFLDR = string.Empty;
        string strVETAPIURL = string.Empty;
        string strCOMPXFERFILE = "Y";
        string strARCHFILE = "Y";
        string strFTPSENDMODE = "U";
        string strFTPABSPATH = string.Empty;

        string strErrRegion = string.Empty;


        Scheduler objCore;
        DicomDecoder dd;
        #endregion

        public DICOMSenderService()
        {
            InitializeComponent();
        }

        #region OnStart
        protected override void OnStart(string[] args)
        {
            try
            {
                System.Threading.Thread thread = new System.Threading.Thread(doProcess);
                thread.Start();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(strWinHdr + " : " + strSvcName, "Error Starting Service." + ex.Message, EventLogEntryType.Warning);
            }
        }
        #endregion

        #region OnStop
        protected override void OnStop()
        {
            try
            {

                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "Service stopped successfully.");
                base.OnStop();
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "Error stopping Service. " + ex.Message);
                EventLog.WriteEntry(strWinHdr + " : " + strSvcName, "Error stopping Service." + ex.Message, EventLogEntryType.Warning);
            }
        }
        #endregion

        #region doProcess
        private void doProcess()
        {


            string strCatchMessage = string.Empty;
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string strFileName = string.Empty;
            bool bReturn = false;
            string strErrRegion = string.Empty;
            string output = string.Empty;
            string err = string.Empty;
            string strProcMsg = string.Empty;
            int intFlag = 0;
            string strSUID = string.Empty;
            string[] arr = new string[0];

            try
            {
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "Service started Successfully");
                while (true)
                {
                    objCore = new Scheduler();
                    dd = new DicomDecoder();

                    try
                    {

                        bReturn = objCore.FetchSchedulerSettings(AppDomain.CurrentDomain.BaseDirectory, ref strCatchMessage);

                        if (bReturn)
                        {
                            strEXEPATH = strConfigPath + "Configs\\DICOM-EXEs\\EXEs\\bin\\";
                            strSNDEXENAME = "storescu.exe";
                            strSNDEXEOPTIONS = objCore.SENDER_OPTIONS;
                            strRCVDIR = objCore.RECEIVING_DIRECTORY;
                            strSNDDIR = objCore.SENDER_DIRECTORY;
                            strARCHDIR = objCore.ARCHIVE_DIRECTORY;
                            strPACSSRVRNAME = objCore.PACS_SERVER_NAME;
                            strINSTNAME = objCore.INSTITUTION_NAME;
                            strSITECODE = objCore.SITE_CODE;

                            strTRANSFERFTP = objCore.FILE_TRANSFER_VIA_FTP;
                            strFTPHOST = objCore.FTP_HOST_NAME;
                            strFTPPORT = objCore.FTP_PORT_NUMBER;
                            strFTPUSER = objCore.FTP_USER_NAME;
                            strFTPPWD = objCore.FTP_PASSWORD;
                            //strFTPTEMPFLDR = objCore.FTP_TEMPORARY_FOLDER;
                            strFTPDWLDFLDR = objCore.FTP_DOWNLOAD_FOLDER;
                            strFTPLOGDWLDFLDR = objCore.FTP_LOG_DOWNLOAD_FOLDER;
                            strVETAPIURL = objCore.VETRIS_API_URL;
                            strCOMPXFERFILE = objCore.COMPRESS_FILES_TO_TRANSFER;
                            strARCHFILE = objCore.ARCHIVE_FILES_TRANSFERED;
                            strFTPSENDMODE = objCore.FTP_SENDING_MODE;
                            strFTPABSPATH = objCore.FTP_ABSOLUTE_PATH;

                            if (strTRANSFERFTP == "N")
                            {

                                #region Sending files to PACS
                                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "File sending started with " + strEXEPATH + strSNDEXENAME + " " + strSNDEXEOPTIONS);

                                if (File.Exists(strEXEPATH + strSNDEXENAME) && Directory.Exists(strSNDDIR))
                                {
                                    strErrRegion = "Sending files to PACS";
                                    arrFiles = Directory.GetFiles(strSNDDIR);

                                    foreach (string strFile in arrFiles)
                                    {
                                        intFlag = 0;
                                        Process myProc = new Process();
                                        myProc.StartInfo.UseShellExecute = false;
                                        myProc.StartInfo.FileName = strEXEPATH + strSNDEXENAME;
                                        myProc.StartInfo.Arguments = strSNDEXEOPTIONS + " " + strFile;
                                        myProc.StartInfo.RedirectStandardOutput = true;
                                        myProc.StartInfo.RedirectStandardError = true;
                                        myProc.Start();


                                        output = myProc.StandardOutput.ReadToEnd();
                                        err = myProc.StandardError.ReadToEnd();

                                        strProcMsg = output.Trim();

                                        //if (strProcMsg.ToUpper().Contains("RECEIVED STORE RESPONSE (SUCCESS)"))
                                        if (strProcMsg.ToUpper().Contains("[STATUS=SUCCESS]"))
                                        {
                                            intFlag = 1;
                                        }

                                        #region Transfer to Archive
                                        strErrRegion = "Transfer to archive";
                                        if (Directory.Exists(strARCHDIR))
                                        {
                                            pathElements = strFile.Split('\\');
                                            strFileName = pathElements[(pathElements.Length - 1)];

                                            dd.DicomFileName = strFile;
                                            List<string> str = dd.dicomInfo;
                                            arr = new string[7];
                                            arr = GetallTags(str);
                                            strSUID = arr[0].Trim();

                                            if (intFlag == 1)
                                            {
                                                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "SUCCESS :: Upload to PACS :: SUID : " + strSUID.Trim() + " :: File Name : " + strFileName);

                                                if (!System.IO.Directory.Exists(strARCHDIR + "\\" + strSUID))
                                                {
                                                    System.IO.Directory.CreateDirectory(strARCHDIR + "\\" + strSUID);
                                                }

                                                if (File.Exists(strARCHDIR + "\\" + strSUID + "\\" + strFileName))
                                                {
                                                    System.IO.File.Delete(strARCHDIR + "\\" + strSUID + "\\" + strFileName);
                                                }

                                                System.IO.File.Move(strFile, strARCHDIR + "\\" + strSUID + "\\" + strFileName);
                                            }
                                            else
                                            {
                                                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "FAILED :: Upload to PACS :: SUID : " + strSUID.Trim() + " :: File Name : " + strFileName);

                                                if (!System.IO.Directory.Exists(strARCHDIR + "\\Not_Transfered\\" + strSUID))
                                                {
                                                    System.IO.Directory.CreateDirectory(strARCHDIR + "\\Not_Transfered\\" + strSUID);
                                                }
                                                if (File.Exists(strARCHDIR + "\\Not_Transfered\\" + strSUID + "\\" + strFileName))
                                                {
                                                    System.IO.File.Delete(strARCHDIR + "\\Not_Transfered\\" + strSUID + "\\" + strFileName);
                                                }

                                                System.IO.File.Move(strFile, strARCHDIR + "\\Not_Transfered\\" + strSUID + "\\" + strFileName);
                                            }
                                        }
                                        #endregion
                                    }

                                }
                                #endregion
                            }
                            else if (strTRANSFERFTP == "Y")
                            {
                                if (strCOMPXFERFILE == "Y")
                                {
                                    #region Compressing & Archiving FIles
                                    doCompressFiles();
                                    #endregion

                                    if (strFTPSENDMODE == "U")
                                    {
                                        #region Uploading files to FTP
                                        doFTPUpload();
                                        #endregion
                                    }
                                    else
                                    {
                                        doMoveFiles();
                                    }
                                }
                                else
                                {
                                    if (strFTPSENDMODE == "U")
                                    {
                                        #region Uploading files to FTP Without Compressing
                                        doFTPUploadWithoutCompress();
                                        #endregion
                                    }
                                    else
                                    {
                                        doMoveFiles();
                                    }
                                }
                                

                                #region Uploading log XML files to FTP
                                if (strRCVDIR.Trim().Split('\\')[strRCVDIR.Split('\\').Length - 1] != "DCMXfer")
                                {
                                    doUploadLog();
                                }
                                #endregion
                            }

                            doUpdateOnlineStatus();
                        }
                        else
                        {
                            CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "doProcess() - Error : " + strCatchMessage);
                        }

                    }
                    catch (Exception expErr)
                    {
                        CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doProcess() :: " + strErrRegion + " - Exception: " + expErr.Message);
                        EventLog.WriteEntry(strWinHdr + " : " + strSvcName,  expErr.Message, EventLogEntryType.Warning);
                        System.Threading.Thread.Sleep(intFreq * 1000);
                    }

                    objCore = null;
                    System.Threading.Thread.Sleep(intFreq * 1000);
                }
            }
            catch (Exception expErr)
            {
                EventLog.WriteEntry(strWinHdr + " : " + strSvcName, "Error Starting Service." + expErr.Message, EventLogEntryType.Warning);
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doProcess() - Exception: " + expErr.Message);

                System.Threading.Thread.Sleep(intFreq * 1000);

            }
            finally
            { objCore = null; dd = null; }
        }

        #endregion

        #region doArrangeFiles
        private void doArrangeFiles()
        {
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string strFilename = string.Empty;
            string[] arr = new string[0];
            string strSUID = string.Empty;
            string strFolder = string.Empty;
            List<string> arrSUID = new List<string>();

            dd = new DicomDecoder();

            try
            {

                arrFiles = Directory.GetFiles(strSNDDIR);

                foreach (string strFile in arrFiles)
                {
                    pathElements = strFile.Split('\\');
                    strFilename = pathElements[(pathElements.Length - 1)];

                    if (MIMEAssistant.GetMIMEType(strFile) != "application/zip")
                    {
                        dd.DicomFileName = strFile;
                        List<string> str = dd.dicomInfo;

                        arr = new string[7];
                        arr = GetallTags(str);
                        strSUID = arr[0].Trim();

                        if (strSUID.Trim() != string.Empty)
                        {
                            strFolder = strSNDDIR + "\\" + strSITECODE + "_" + strINSTNAME.Replace(" ", "_") + "_" + strSUID.Trim();
                            if (!(Directory.Exists(strFolder)))
                            {
                                Directory.CreateDirectory(strFolder);
                                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "Folder : " + strFolder.Trim() + " created");
                            }
                            System.IO.File.Move(strFile, strFolder + "\\" + strFilename);
                            CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "SUID : " + strSUID.Trim() + " :: File Name : " + strFilename + " moved");

                        }
                        else
                        {
                            CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doArrangeFiles() :: " + strErrRegion + " - File : " + strFile + " - SUID Not found");
                            System.IO.File.Delete(strFile);
                        }
                    }
                }

            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doArrangeFiles() :: " + strErrRegion + " - Exception: " + expErr.Message);
            }
        }
        #endregion

        #region doCompressFiles
        private void doCompressFiles()
        {
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string[] fileElements = new string[0];
            string strFileName = string.Empty;
            string strDirName = string.Empty;
            string strTargetPath = string.Empty;
            string strExtractPath = string.Empty;
            string strExtn = string.Empty;
            string strSID = string.Empty;

            string[] arr = new string[0];
            string strSUID = string.Empty;
            List<string> arrSUID = new List<string>();
            dd = new DicomDecoder();


            try
            {

                arrFiles = Directory.GetFiles(strSNDDIR);

                foreach (string strFile in arrFiles)
                {
                    if (MIMEAssistant.GetMIMEType(strFile) != "application/zip")
                    {

                        pathElements = strFile.Split('\\');
                        fileElements = new string[0];
                        strFileName = pathElements[(pathElements.Length - 1)];

                        fileElements = strFileName.Split('_');
                        strSID = fileElements[1];

                        //strFileName = strSITECODE + "_" + strSID + "_" + strINSTNAME.Replace(" ", "_") + "_" + strFileName.Replace(strSID + "_", "");



                        strExtn = Path.GetExtension(strFile);
                        if (strExtn.Trim() != string.Empty)
                        {
                            if (strFileName.Contains(strExtn))
                                strFileName = strFileName.Replace(strExtn, string.Empty);
                        }

                        if (CoreCommon.IsDicomFile(strFile))
                        {
                            dd.DicomFileName = strFile;
                            List<string> str = dd.dicomInfo;
                            arr = new string[7];
                            arr = GetallTags(str);
                            strSUID = arr[0].Trim();
                        }
                        else
                            strSUID = string.Empty;


                        if (File.Exists(strSNDDIR + "\\" + strFileName + ".zip")) File.Delete(strSNDDIR + "\\" + strFileName + ".zip");
                        strTargetPath = strSNDDIR + "\\" + strFileName + ".zip";

                        using (ZipArchive zip = ZipFile.Open(strTargetPath, ZipArchiveMode.Create))
                        {
                            zip.CreateEntryFromFile(strFile, strFileName + strExtn);
                        }

                        //ZipFile.CreateFromDirectory(strFile, strTargetPath);
                        CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "File : " + strFile.Trim() + " compressed as " + strSNDDIR + "\\" + strFileName + ".zip");

                        if (strSID.Substring(0, 6) == "S1DXXX")
                        {
                            if (strARCHFILE == "Y")
                            {
                                strErrRegion = "Transfer files to archive";
                                doArchiveFile(strFile, strSID);
                            }
                            else
                            {
                                strErrRegion = "Delete transfered file";
                                File.Delete(strFile);
                            }
                        }
                        else
                        {
                            strErrRegion = "Transfer files to archive";
                            doArchiveFile(strFile, strSID);
                        }


                        //strErrRegion = "Uploading Files To FTP";
                        //doFTPUpload(strSNDDIR + "\\" + strFileName + ".zip", strFile);
                    }

                }

            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doCompressFiles() :: " + strErrRegion + " - Exception: " + expErr.Message);

            }
            finally
            {
                dd = null;
            }

        }
        #endregion

        #region doArchiveFile
        private void doArchiveFile(string strFilePath, string strSID)
        {
            string[] pathElements = new string[0];
            string strFilename = string.Empty;
            string strDirName = string.Empty;

            try
            {

                pathElements = strFilePath.Split('\\');
                strFilename = pathElements[(pathElements.Length - 1)];
                //if (strSUID.Trim() != string.Empty) strDirName = strSITECODE + "_" + strINSTNAME.Replace(" ", "_") + "_" + strSUID;
                //else strDirName = strSITECODE + "_" + strINSTNAME.Replace(" ", "_") + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                strDirName = strSID;

                if (!Directory.Exists(strARCHDIR + "\\" + strDirName)) Directory.CreateDirectory(strARCHDIR + "\\" + strDirName);
                if (File.Exists(strARCHDIR + "\\" + strDirName + "\\" + strFilename)) File.Delete(strARCHDIR + "\\" + strDirName + "\\" + strFilename);
                File.Move(strFilePath, strARCHDIR + "\\" + strDirName + "\\" + strFilename);

                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "File : " + strFilename.Trim() + " archived to " + strDirName);
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doArchiveDirectory() :: " + strErrRegion + " - Exception: " + expErr.Message);

            }
        }
        #endregion

        #region doFTPUpload
        private void doFTPUpload()
        {
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string[] fileElements = new string[0];
            string strFilename = string.Empty;
            string strSessID = string.Empty;
            
            string strTxtPath = string.Empty;
            string strText = string.Empty;
            FtpWebRequest request;



            try
            {
                arrFiles = Directory.GetFiles(strSNDDIR, "*.zip");
                foreach (string strFile in arrFiles)
                {
                    if (IsZipValid(strFile))
                    {
                        pathElements = strFile.Split('\\');
                        strFilename = pathElements[(pathElements.Length - 1)];
                        fileElements = strFilename.Split('_');
                        strSessID = fileElements[1];

                        //CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "Uploading file : " + strFile.Trim());

                        request = WebRequest.Create(new Uri(string.Format(@"ftp://{0}/{1}/{2}", strFTPHOST, strFTPDWLDFLDR, strFilename))) as FtpWebRequest;
                        request.Method = WebRequestMethods.Ftp.UploadFile;
                        request.UseBinary = true;
                        request.UsePassive = true;
                        request.KeepAlive = true;
                        request.Credentials = new NetworkCredential(strFTPUSER, strFTPPWD);


                        using (FileStream fs = File.OpenRead(strFile))
                        {
                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            fs.Close();

                            Stream requestStream = request.GetRequestStream();
                            requestStream.Write(buffer, 0, buffer.Length);
                            requestStream.Flush();
                            requestStream.Close();
                            CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "File : " + strFile.Trim() + " uploaded to ftp");
                            File.Delete(strFile);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doFTPUpload() :: File :" + strFilename.Trim() + " - FAILED to upload :: " + ex.Message);
            }
            finally
            {
                request = null;
            }



        }
        #endregion

        #region doFTPUploadWithoutCompress
        private void doFTPUploadWithoutCompress()
        {
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string[] fileElements = new string[0];
            string strFilename = string.Empty;
            string strSessID = string.Empty;

            string strTxtPath = string.Empty;
            string strText = string.Empty;
            FtpWebRequest request;



            try
            {
                arrFiles = Directory.GetFiles(strSNDDIR);
                foreach (string strFile in arrFiles)
                {
                    //if (IsZipValid(strFile))
                    //{
                        pathElements = strFile.Split('\\');
                        strFilename = pathElements[(pathElements.Length - 1)];
                        fileElements = strFilename.Split('_');
                        strSessID = fileElements[1];

                        
                        //CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "Uploading file : " + strFile.Trim());

                        request = WebRequest.Create(new Uri(string.Format(@"ftp://{0}/{1}/{2}", strFTPHOST, strFTPDWLDFLDR, strFilename))) as FtpWebRequest;
                        request.Method = WebRequestMethods.Ftp.UploadFile;
                        request.UseBinary = true;
                        request.UsePassive = true;
                        request.KeepAlive = true;   
                        request.Credentials = new NetworkCredential(strFTPUSER, strFTPPWD);


                        using (FileStream fs = File.OpenRead(strFile))
                        {
                            byte[] buffer = new byte[fs.Length];
                            fs.Read(buffer, 0, buffer.Length);
                            fs.Close();

                            Stream requestStream = request.GetRequestStream();
                            requestStream.Write(buffer, 0, buffer.Length);
                            requestStream.Flush();
                            requestStream.Close();
                            CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "File : " + strFile.Trim() + " uploaded to ftp");

                            if (strSessID.Substring(0, 6) == "S1DXXX")
                            {
                                if (strARCHFILE == "Y")
                                {
                                    strErrRegion = "Transfer files to archive";
                                    doArchiveFile(strFile, strSessID);
                                }
                                else
                                {
                                    strErrRegion = "Delete transfered file";
                                    File.Delete(strFile);
                                }
                            }
                            else
                            {
                                strErrRegion = "Transfer files to archive";
                                doArchiveFile(strFile, strSessID);
                            }
                        }
                    //}
                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doFTPUpload() :: File :" + strFilename.Trim() + " - FAILED to upload :: " + ex.Message);
            }
            finally
            {
                request = null;
            }



        }
        #endregion

        #region Suspended
        //#region doMoveFiles
        //private void doMoveFiles()
        //{
        //    string[] arrFiles = new string[0];
        //    string[] pathElements = new string[0];
        //    string[] fileElements = new string[0];
        //    string strFilename = string.Empty;
        //    string strSessID = string.Empty;

        //    string strTxtPath = string.Empty;
        //    string strText = string.Empty;




        //    try
        //    {
        //        if(strCOMPXFERFILE=="Y")arrFiles = Directory.GetFiles(strSNDDIR, "*.zip");
        //        else arrFiles = Directory.GetFiles(strSNDDIR);

        //        foreach (string strFile in arrFiles)
        //        {
        //            pathElements = strFile.Split('\\');
        //            strFilename = pathElements[(pathElements.Length - 1)];
        //            fileElements = strFilename.Split('_');
        //            strSessID = fileElements[1];

        //            if (strCOMPXFERFILE == "Y")
        //            {
        //                if (IsZipValid(strFile))
        //                {
        //                    File.Move(strFile, strFTPABSPATH + "\\" + strFilename);
        //                    CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "File : " + strFile.Trim() + " moved to " + strFTPABSPATH);
        //                }
        //            }
        //            else
        //            {
        //                File.Move(strFile, strFTPABSPATH + "\\" + strFilename);
        //                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "File : " + strFile.Trim() + " moved to " + strFTPABSPATH);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doMoveFiles() :: File :" + strFilename.Trim() + " - FAILED to move :: " + ex.Message);
        //    }
        //    finally
        //    {
        //        arrFiles = null;
        //    }



        //}
        //#endregion
        #endregion

        #region doMoveFiles
        private void doMoveFiles()
        {
            Process process = new Process();
            ProcessStartInfo ProcMoveFiles = new ProcessStartInfo();
            string strBatchFile = AppDomain.CurrentDomain.BaseDirectory + "MoveDCMFiles.bat";
            //int exitCode = 0;
            string strExecCommand = string.Empty;

            try
            {
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "Moving files to " + strFTPABSPATH);

               if (strCOMPXFERFILE == "Y")
                    strExecCommand = "/c " + strBatchFile + " " + @strSNDDIR + "\\*.zip " + @strFTPABSPATH + "\\";
                else
                    strExecCommand = "/c " + strBatchFile + " " + @strSNDDIR + "\\*.* " + @strFTPABSPATH + "\\";
                
                ProcMoveFiles.UseShellExecute = false;
                ProcMoveFiles.FileName = System.Environment.GetEnvironmentVariable("COMSPEC");
                ProcMoveFiles.Arguments = strExecCommand;
                ProcMoveFiles.WindowStyle = ProcessWindowStyle.Hidden;
                ProcMoveFiles.CreateNoWindow = true;
                ProcMoveFiles.RedirectStandardOutput = true;
                ProcMoveFiles.RedirectStandardError = true;

                process.StartInfo = ProcMoveFiles;
                process.Start();
                //process.WaitForExit();
                //exitCode = process.ExitCode;
                //process.Close();


            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doMoveFiles() :: Exception : " + ex.Message);
            }
            finally
            {
                process.Close();
                process.Dispose();
                ProcMoveFiles = null;
                process=null;
            }
            

        }
        #endregion

        #region doUploadLog
        private void doUploadLog()
        {
            #region Variables
            string strCatchMessage = string.Empty;
            string strFileXML = string.Empty;
            string strFileName = string.Empty;
            string strExtn = string.Empty;
            string strTargetPath = string.Empty;

            FtpWebRequest request;
            DataTable dtbl = new DataTable();
            string[] pathElements = new string[0];
            #endregion

            objCore = new Scheduler();

            try
            {
                dtbl = objCore.FetchLogToUpload(AppDomain.CurrentDomain.BaseDirectory, ref strCatchMessage);

                if (dtbl != null)
                {
                    if (dtbl.Rows.Count > 0)
                    {

                        #region create XML file
                        if (!Directory.Exists(strSNDDIR + "\\Logs"))
                        {
                            Directory.CreateDirectory(strSNDDIR + "\\Logs");
                        }
                        strFileXML = strSNDDIR + "\\Logs\\" + strSITECODE + "_" + strINSTNAME.Replace(" ", "_") + "_Log" + DateTime.Now.ToString("ddMMyyHHmmss") + ".xml";
                        dtbl.TableName = "log";
                        dtbl.WriteXml(strFileXML);
                        #endregion

                        #region CompressXML
                        try
                        {
                            pathElements = strFileXML.Split('\\');
                            strFileName = pathElements[(pathElements.Length - 1)];
                            strExtn = Path.GetExtension(strFileXML);
                            strFileName = strFileName.Replace(strExtn, string.Empty);

                            if (File.Exists(strSNDDIR + "\\Logs\\" + strFileName + ".zip")) File.Delete(strSNDDIR + "\\Logs\\" + strFileName + ".zip");
                            strTargetPath = strSNDDIR + "\\Logs\\" + strFileName + ".zip";
                            //CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "File : " + strFileName.Trim() + " Extension : " + strExtn);

                            using (ZipArchive zip = ZipFile.Open(strTargetPath, ZipArchiveMode.Create))
                            {

                                zip.CreateEntryFromFile(strFileXML, strFileName + strExtn);
                            }
                            //CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "File : " + strFileXML.Trim() + " compressed as " + strTargetPath);
                            File.Delete(strFileXML);
                        }
                        catch (Exception ex)
                        {
                            CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doUploadLog() :: Compress File - " + strFileXML + " - Exception: " + ex.Message);
                        }
                        #endregion

                        #region Upload File To Ftp
                        try
                        {
                            strFileName = strFileName + ".zip";
                            //CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "File : " + strFileName.Trim());
                            request = WebRequest.Create(new Uri(string.Format(@"ftp://{0}/{1}/{2}", strFTPHOST, strFTPLOGDWLDFLDR, strFileName))) as FtpWebRequest;
                            request.Method = WebRequestMethods.Ftp.UploadFile;
                            request.UseBinary = true;
                            request.UsePassive = true;
                            request.KeepAlive = true;
                            request.Credentials = new NetworkCredential(strFTPUSER, strFTPPWD);

                            using (FileStream fs = File.OpenRead(strSNDDIR + "\\Logs\\" + strFileName))
                            {
                                byte[] buffer = new byte[fs.Length];
                                fs.Read(buffer, 0, buffer.Length);
                                fs.Close();

                                Stream requestStream = request.GetRequestStream();
                                requestStream.Write(buffer, 0, buffer.Length);
                                requestStream.Flush();
                                requestStream.Close();
                                //CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "File : " + strFileName.Trim() + " uploaded to ftp");
                            }

                            File.Delete(strSNDDIR + "\\Logs\\" + strFileName);
                        }
                        catch (Exception ex)
                        {
                            CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doUploadLog() :: File :" + strFileName.Trim() + " - FAILED to upload :: " + ex.Message);
                        }
                        #endregion

                    }

                }
                else
                    CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doUploadLog() :: " + strCatchMessage);


            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doUploadLog() :: " + ex.Message);
            }
            finally
            {
                request = null;
            }



        }
        #endregion

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
            string UserCaseID = string.Empty;
            string ModalityID = string.Empty;
            string Strname = string.Empty;
            string DOB = string.Empty;
            string result = string.Empty;
            string UserSeriesID = string.Empty;
            string SeriesNumber = string.Empty;

            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                string s1, s4, s5, s11, s12;
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);


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
                if ((s11.ToUpper() == "0020") && (s12.ToUpper() == "000D"))
                {
                    UserCaseID = s5.Replace("\0", "");

                }
                else if ((s11.ToUpper() == "0020") && (s12.ToUpper() == "000E"))
                {
                    UserSeriesID = s5.Replace("\0", "");
                    break;
                }

                else if ((s11.ToUpper() == "0020") && (s12.ToUpper() == "0011"))
                {
                    SeriesNumber = s5.Replace("\0", "");

                }
            }
            string[] arr = new string[7];
            arr[0] = UserCaseID;
            arr[1] = UserSeriesID;
            arr[2] = SeriesNumber;

            /*arr[1] = strDescription;
            arr[2] = ModalityID;
            arr[3] = Strname;
            arr[4] = result;*/

            return arr;

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

        #region doUpdateOnlineStatus
        private void doUpdateOnlineStatus()
        {
            
            string strCurrentVer = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string strRespMsg = string.Empty;
            string apiUrl = strVETAPIURL;
            string json = string.Empty;
            WebClient client = new WebClient();

            try
            {

                object input = new
                {
                    institutionCode = strSITECODE,
                    versionNo = strCurrentVer,
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                json = client.UploadString(apiUrl + "/DicomRouterUpdateOnlineStatus", inputJson);

                JavaScriptSerializer ser = new JavaScriptSerializer();
                ServiceClass.DicomRouterOnlineStatusResponseDetails resp = ser.Deserialize<ServiceClass.DicomRouterOnlineStatusResponseDetails>(json);

                strRespMsg = resp.responseStatus.responseMessage;


                if (strRespMsg.Trim() != "SUCCESS")
                {
                    CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doUpdateOnlineStatus() - Error: " + strRespMsg);
                }
                ser = null;
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doUpdateOnlineStatus() - Exception: " + expErr.Message);

            }
            
        }
        #endregion

    }
}
