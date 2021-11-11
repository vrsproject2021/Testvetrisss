using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Web;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Configuration;
using System.Security;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using VETRISRouter.Core;
using DICOMLib;

namespace DICOMReceiverService
{
    public partial class DICOMReceiverService : ServiceBase
    {
        #region members & variables
        private static int intFreq = 2;
        private static string strURL = string.Empty;
        private static string strConfigPath = AppDomain.CurrentDomain.BaseDirectory;
        private static string strWinHdr = "VETRIS DICOM ROUTER";
        private static int intSvcID = 1;
        private static string strSvcName = "Dicom Receiving Service";
        string strErrRegion = string.Empty;
        private static string strEXEPATH = string.Empty;
        private static string strRCVEXENAME = string.Empty;
        private static string strRCVEXEOPTIONS = string.Empty;
        private static string strRCVDIR = string.Empty;
        private static string strSNDDIR = string.Empty;
        private static string strRCVDIRMANUAL = string.Empty;
        private static string strMANUALUPLDAUTO = string.Empty;
        private static string strIMGMNLUPLDAUTO = string.Empty;
        private static string strRCVIMGDIR = string.Empty;
        private static string strSITECODE = string.Empty;
        private static string strINSTNAME = string.Empty;
        private static string strARCHDIR = string.Empty;
        private static string strCOMPXFERFILE = "Y";

        Scheduler objCore;
        DicomDecoder dd;
        #endregion

        public DICOMReceiverService()
        {
            InitializeComponent();
        }

        #region OnStart
        protected override void OnStart(string[] args)
        {
            try
            {


                //Console.WriteLine("hellorcv-start\n" + Directory.GetCurrentDirectory());
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
                foreach (var process in Process.GetProcessesByName("storescp"))
                {
                    process.Kill();
                }
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
            bool bReturn = false;

            objCore = new Scheduler();

            try
            {
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "Service started Successfully");

                try
                {
                    bReturn = objCore.FetchSchedulerSettings(strConfigPath, ref strCatchMessage);

                    if (bReturn)
                    {
                        strEXEPATH = strConfigPath + "\\Configs\\DICOM-EXEs\\EXEs\\bin\\";
                        strRCVEXENAME = "storescp.exe";
                        strRCVEXEOPTIONS = objCore.RECEIVER_EXE_OPTIONS;
                        strRCVDIR = objCore.RECEIVING_DIRECTORY;
                        strSNDDIR = objCore.SENDER_DIRECTORY;
                        strRCVDIRMANUAL = objCore.RECEIVING_DIRECTORY_FOR_MANUAL_UPLOAD;
                        strMANUALUPLDAUTO = objCore.RECEIVING_DIRECTORY_AUTO_DETECT;
                        strIMGMNLUPLDAUTO = objCore.RECEIVING_DIRECTORY_FOR_IMAGES_AUTO_DETECT;
                        strRCVIMGDIR = objCore.RECEIVING_DIRECTORY_FOR_IMAGES;
                        strINSTNAME = objCore.INSTITUTION_NAME;
                        strSITECODE = objCore.SITE_CODE;
                        strARCHDIR = objCore.ARCHIVE_DIRECTORY;
                        strCOMPXFERFILE = objCore.COMPRESS_FILES_TO_TRANSFER;

                        #region Receiving files

                        if (strRCVDIR.Split('\\')[strRCVDIR.Split('\\').Length - 1] != "DCMXfer")
                        {
                            Process ReceivingProc = new Process();
                            CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "File receiving started with " + strEXEPATH + strRCVEXENAME + " " + strRCVEXEOPTIONS + " ");


                            if (File.Exists(strEXEPATH + strRCVEXENAME) && Directory.Exists(strRCVDIR))
                            {
                                strErrRegion = "Receiving files";
                                ReceivingProc.StartInfo.UseShellExecute = false;
                                ReceivingProc.StartInfo.FileName = strEXEPATH + strRCVEXENAME;
                                ReceivingProc.StartInfo.Arguments = strRCVEXEOPTIONS + " " + strRCVDIR;
                                ReceivingProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                ReceivingProc.StartInfo.CreateNoWindow = true;
                                ReceivingProc.StartInfo.RedirectStandardOutput = true;
                                ReceivingProc.StartInfo.RedirectStandardError = true;
                                try
                                {
                                    ReceivingProc.Start();
                                }
                                catch (Exception ex)
                                {
                                    CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "Exception : " + ex.Message);
                                }
                                finally
                                {
                                    ReceivingProc.Close();
                                    ReceivingProc.Dispose();
                                }
                            }
                        }
                        #endregion

                    }
                    else
                    {
                        CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "GetServiceDetails() - Error : " + strCatchMessage);
                    }

                    while (true)
                    {
                        #region Transfer FIles
                        try
                        {
                            strErrRegion = "TransferFiles()";
                            TransferFiles();
                        }
                        catch (Exception expErr)
                        {
                            CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doProcess() :: " + strErrRegion + " - Exception: " + expErr.Message);
                            System.Threading.Thread.Sleep(intFreq * 1000);
                        }
                        #endregion


                        objCore = null;
                        System.Threading.Thread.Sleep(intFreq * 1000);
                    }
                }
                catch (Exception expErr)
                {
                    CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doProcess() :: " + strErrRegion + " - Exception: " + expErr.Message);
                    System.Threading.Thread.Sleep(intFreq * 1000);
                }

            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doProcess() - Exception: " + expErr.Message);
                System.Threading.Thread.Sleep(intFreq * 1000);

            }
            finally
            { objCore = null; }
        }

        #endregion

        #region TransferFiles
        private void TransferFiles()
        {
            string[] arrRecFiles = new string[0];
            string[] arrSentFiles = new string[0];
            string[] pathElements = new string[0];
            string[] fileElements = new string[0];
            string strFilename = string.Empty;
            string strOrgFileName = string.Empty;
            string strNewFileName = string.Empty;
            string[] arr = new string[0];
            //int isDiacom = -1;
            string strSUID = string.Empty;
            // string strXfer = "Y";
            string strMIMEType = string.Empty;
            string strDirName = string.Empty;
            string strRecDirName = string.Empty;
            string strSID = string.Empty;
            string strPrefix = string.Empty;
            string strExtn = string.Empty;
            string strFileXML = AppDomain.CurrentDomain.BaseDirectory + "\\FileLog\\Files" + DateTime.Today.ToString("yyyyMMdd") + ".xml";

            List<string> arrSUID = new List<string>();
            DataSet ds = new DataSet();
            DataTable dtbl = new DataTable();
            DateTime dtLast = DateTime.Today;


            dd = new DicomDecoder();


            try
            {
                strErrRegion = "Checking Folders";
                CheckFolders();

                // CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "MANUALUPLDAUTO : " + strMANUALUPLDAUTO + " :: RCVDIRMANUAL : " + strRCVDIRMANUAL + " :: RCVDIR : " + strRCVDIR);

                if (strMANUALUPLDAUTO == "N")
                {
                    strErrRegion = "Checking Manual Folders";
                    if (Directory.Exists(strRCVDIRMANUAL)) SearchFilesManual(dd, strRCVDIRMANUAL);
                }
                else
                {
                    strErrRegion = "Checking Auto Detected Manual Folder";
                    if (Directory.Exists(strRCVDIRMANUAL)) SearchFiles(dd, strRCVDIRMANUAL);
                }

                #region  Suspended
                //if (strIMGMNLUPLDAUTO == "N")
                //{
                //    strErrRegion = "Checking Image Folders";
                //    if (Directory.Exists(strRCVIMGDIR))
                //    {
                //        //CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", strErrRegion);
                //        //CheckImageFolders();
                //        SearchImageFilesManual(strRCVIMGDIR);
                //    }
                //}
                //else
                //{
                //    strErrRegion = "Checking Auto Detected Image Folder";
                //    if (Directory.Exists(strRCVIMGDIR)) SearchImageFiles(strRCVIMGDIR);
                //}
                #endregion

                strRecDirName = strRCVDIR.Split('\\')[strRCVDIR.Split('\\').Length - 1];

                if (strRecDirName != "DCMListener")
                {
                    
                    #region rename and move the files in batch
                    CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "Moving files to " + strSNDDIR);

                    Process process = new Process();
                    ProcessStartInfo ProcMoveFiles = new ProcessStartInfo();
                    string strBatchFile = AppDomain.CurrentDomain.BaseDirectory + "AddPrefixAndMoveFiles.bat";
                    string strExecCommand = string.Empty;
                    string strRand = string.Empty;
                    string strSubPrefix = string.Empty;

                    strSID = "S1DXXX" + DateTime.Now.ToString("MMddyyHHmmss");

                    try
                    {

                        strRand = CoreCommon.RandomString(6);
                        strPrefix = strSITECODE + "_" + strSID + "_" + strINSTNAME.Replace(" ", "_") + "_";
                        strPrefix = strPrefix.Replace(" ", "_");
                        strPrefix = strPrefix.Replace(",", "");
                        strPrefix = strPrefix.Replace("(", "");
                        strPrefix = strPrefix.Replace(")", "");
                        strPrefix = strPrefix.Replace("'", "");
                        strPrefix = strPrefix.Replace("\"", "");
                        strPrefix = strPrefix.Replace("#", "");
                        strPrefix = strPrefix.Replace("&", "");
                        strPrefix = strPrefix.Replace("@", "");
                        strPrefix = strPrefix.Replace("__", "_");
                        strSubPrefix = strSITECODE + "_S1DXXX";

                        if (strCOMPXFERFILE == "Y")
                            strExecCommand = "/c " + strBatchFile + " " + @strRCVDIR + "\\*.zip " + @strSNDDIR + "\\ " + strPrefix + " " + strSubPrefix;
                        else
                            strExecCommand = "/c " + strBatchFile + " " + @strRCVDIR + "\\*.* " + @strSNDDIR + "\\ " + strPrefix + " " + strSubPrefix;

                        ProcMoveFiles.UseShellExecute = false;
                        ProcMoveFiles.FileName = System.Environment.GetEnvironmentVariable("COMSPEC");
                        ProcMoveFiles.Arguments = strExecCommand;
                        ProcMoveFiles.WindowStyle = ProcessWindowStyle.Hidden;
                        ProcMoveFiles.CreateNoWindow = true;
                        ProcMoveFiles.RedirectStandardOutput = true;
                        ProcMoveFiles.RedirectStandardError = true;

                        process.StartInfo = ProcMoveFiles;
                        process.Start();
                    }
                    catch (Exception ex)
                    {
                        CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "TransferFiles() => Batch Move :: Exception : " + ex.Message);
                    }
                    finally
                    {
                        process.Close();
                        process.Dispose();
                        ProcMoveFiles = null;
                        process = null;
                    }
                    #endregion
                    
                }

                #region Suspended
                //else if (strRecDirName )
                //{
                //    #region Suspended

                //    //#region File Log
                //    ////if (File.Exists(strFileXML))
                //    ////{
                //    //    //strErrRegion = "FileLog XML Exists";
                //    //    //ds.ReadXml(strFileXML);
                //    //    //if (ds.Tables.Count > 0) ds.Tables[0].TableName = "Files";
                //    //    //else
                //    //    //{
                //    //    //    dtbl = CreateFileTable();
                //    //    //    ds.Tables.Add(dtbl);
                //    //    //}

                //    //    arrRecFiles = Directory.GetFiles(strRCVDIR);

                //    //    foreach (string strFile in arrRecFiles)
                //    //    {
                //    //        if (DateTime.Today > dtLast)
                //    //        {
                //    //            dtLast = DateTime.Today;
                //    //            break;
                //    //        }

                //    //        strErrRegion = "Transferring files";
                //    //        pathElements = strFile.Split('\\');
                //    //        strOrgFileName = pathElements[(pathElements.Length - 1)];
                //    //        strFilename = pathElements[(pathElements.Length - 1)];
                //    //        fileElements = new string[0];
                //    //        strMIMEType = MIMEAssistant.GetMIMEType(strFile);

                //    //        //DataView dv = new DataView(ds.Tables["Files"]);
                //    //        //dv.RowFilter = "file_name ='" + strOrgFileName + "'";

                //    //        //if (dv.ToTable().Rows.Count == 0)
                //    //        //{

                //    //            #region Suspended
                //    //            //if ((strMIMEType == "text/html") || (strMIMEType == "application/msword") || (strMIMEType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                //    //            //    || (strMIMEType == "application/vnd.ms-excel") || (strMIMEType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                //    //            //    || (strMIMEType == "application/vnd.ms-powerpoint") || (strMIMEType == "application/vnd.openxmlformats-officedocument.presentationml.presentation")
                //    //            //    || (strMIMEType == "application/pdf") || (strMIMEType == "application/octet-stream") || (strMIMEType == "text/plain")
                //    //            //    || (strFilename == "DICOMDIR"))
                //    //            //{

                //    //            //    #region archiving files to junk folder
                //    //            //    if (!Directory.Exists(strARCHDIR + "\\Junk"))
                //    //            //    {
                //    //            //        Directory.CreateDirectory(strARCHDIR + "\\Junk");
                //    //            //    }
                //    //            //    strDirName = DateTime.Today.ToString("yyyyMMdd");
                //    //            //    if (!Directory.Exists(strARCHDIR + "\\Junk\\" + strDirName))
                //    //            //    {
                //    //            //        Directory.CreateDirectory(strARCHDIR + "\\Junk\\" + strDirName);
                //    //            //    }
                //    //            //    if (File.Exists(strARCHDIR + "\\Junk\\" + strDirName + "\\" + strFilename))
                //    //            //    {
                //    //            //        File.Delete(strARCHDIR + "\\Junk\\" + strDirName + "\\" + strFilename);
                //    //            //    }
                //    //            //    File.Move(strFile, strARCHDIR + "\\Junk\\" + strDirName + "\\" + strFilename);
                //    //            //    CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "TransferFiles() :: " + strErrRegion + " - File : " + strFilename + " - invalid file type...Archived to junk");

                //    //            //    #endregion

                //    //            //    //System.IO.File.Delete(strFile);
                //    //            //    strXfer = "N";
                //    //            //}
                //    //            #endregion

                //    //            #region DICOMDIR
                //    //            if (strFilename.Contains("DICOMDIR"))
                //    //            {
                //    //                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "TransferFiles() :: " + strErrRegion + " - File : " + strFile + " - invalid file type...Deleted");
                //    //                System.IO.File.Delete(strFile);
                //    //                //strXfer = "N";
                //    //            }
                //    //            #endregion

                //    //            #region Rename File
                //    //            strFilename = strFilename.Replace(" ", "_");
                //    //            strFilename = strFilename.Replace("(", "");
                //    //            strFilename = strFilename.Replace(")", "");
                //    //            strFilename = strFilename.Replace("#", "");
                //    //            strFilename = strFilename.Replace("?", "");
                //    //            strFilename = strFilename.Replace("'", "");

                //    //            if (strFilename.Contains("_"))
                //    //            {
                //    //                fileElements = strFilename.Split('_');
                //    //                strSID = fileElements[0];
                //    //            }
                //    //            else
                //    //                strSID = string.Empty;

                //    //            if (strSID.Length > 3)
                //    //            {
                //    //                if (strSID.Substring(0, 3) != "S1D")
                //    //                {
                //    //                    strSID = "S1DXXX" + DateTime.Now.ToString("MMddyyHHmmss");
                //    //                    strPrefix = CoreCommon.RandomString(6);
                //    //                    strFilename = strSITECODE + "_" + strSID + "_" + strINSTNAME.Replace(" ", "_") + "_" + strPrefix + "_" + strFilename.Replace(strSID + "_", "");
                //    //                }
                //    //            }
                //    //            else
                //    //            {
                //    //                strSID = "S1DXXX" + DateTime.Now.ToString("MMddyyHHmmss");
                //    //                strPrefix = CoreCommon.RandomString(6);
                //    //                strFilename = strSITECODE + "_" + strSID + "_" + strINSTNAME.Replace(" ", "_") + "_" + strPrefix + "_" + strFilename.Replace(strSID + "_", "");
                //    //            }
                //    //            strExtn = Path.GetExtension(strFile);
                //    //            //if (strExtn.Trim() != string.Empty) strFilename = strFilename + strExtn;


                //    //            #endregion

                //    //            #region Transfer File
                //    //            if (CoreCommon.IsDicomFile(strFile))
                //    //            {
                //    //                dd.DicomFileName = strFile;
                //    //                List<string> str = dd.dicomInfo;

                //    //                arr = new string[7];
                //    //                arr = GetallTags(str);
                //    //                strSUID = arr[0].Trim();
                //    //                strNewFileName = strRCVDIR + "\\" + strFilename;

                //    //                //System.IO.File.Copy(strFile, strNewFileName);
                //    //                //System.IO.File.Move(strNewFileName, strSNDDIR + "\\" + strFilename);
                //    //                System.IO.File.Move(strFile, strSNDDIR + "\\" + strFilename);
                //    //                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "SUID : " + strSUID.Trim() + " :: File Name : " + strFilename + " :: transfered for upload");
                //    //            }
                //    //            else if ((MIMEAssistant.GetMIMEType(strFile) == "image/jpeg") || (MIMEAssistant.GetMIMEType(strFile) == "image/gif") || (MIMEAssistant.GetMIMEType(strFile) == "image/png") || (MIMEAssistant.GetMIMEType(strFile) == "image/bmp"))
                //    //            {
                //    //                strNewFileName = strRCVDIR + "\\" + strFilename;
                //    //                System.IO.File.Copy(strFile, strNewFileName);
                //    //                System.IO.File.Move(strNewFileName, strSNDDIR + "\\" + strFilename);
                //    //                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "File Name : " + strFilename + " :: transfered for upload");
                //    //            }
                //    //            else
                //    //            {
                //    //                //CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "TransferFiles() :: " + strErrRegion + " - File : " + strFile + " - SUID Not found...Deleted");
                //    //                //System.IO.File.Delete(strFile);
                //    //                #region archiving files to junk folder
                //    //                if (!Directory.Exists(strARCHDIR + "\\Junk"))
                //    //                {
                //    //                    Directory.CreateDirectory(strARCHDIR + "\\Junk");
                //    //                }
                //    //                strDirName = DateTime.Today.ToString("yyyyMMdd");
                //    //                if (!Directory.Exists(strARCHDIR + "\\Junk\\" + strDirName))
                //    //                {
                //    //                    Directory.CreateDirectory(strARCHDIR + "\\Junk\\" + strDirName);
                //    //                }
                //    //                if (File.Exists(strARCHDIR + "\\Junk\\" + strDirName + "\\" + strFilename))
                //    //                {
                //    //                    File.Delete(strARCHDIR + "\\Junk\\" + strDirName + "\\" + strFilename);
                //    //                }
                //    //                File.Move(strFile, strARCHDIR + "\\Junk\\" + strDirName + "\\" + strFilename);
                //    //                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "TransferFiles() :: " + strErrRegion + " - File : " + strFilename + " - invalid file type...Archived to junk");

                //    //                #endregion
                //    //            }
                //    //            #endregion

                //    //        //    DataRow dr = ds.Tables["Files"].NewRow();
                //    //        //    dr["file_name"] = strOrgFileName;
                //    //        //    ds.Tables["Files"].Rows.Add(dr);
                //    //        //    ds.Tables["Files"].AcceptChanges();
                //    //        //}

                //    //        //dv.Dispose();

                //    //    }
                //    //    arrRecFiles = null;

                //    ////}
                //    ////else
                //    ////{
                //    ////    strErrRegion = "FileLog XML Doesn't Exist";
                //    ////    dtbl = CreateFileTable();
                //    ////    ds.Tables.Add(dtbl);
                //    ////}
                //    //#endregion
                    



                //    //#region adding permission to installed folder
                //    //DirectoryInfo dInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                //    //DirectorySecurity dSecurity = dInfo.GetAccessControl();
                //    //dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                //    //dInfo.SetAccessControl(dSecurity);
                //    //#endregion

                //    //#region Save File Log
                //    //strErrRegion = "Saving FileLog XML";
                //    //if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\FileLog")) Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\FileLog");
                //    //ds.Tables["Files"].WriteXml(strFileXML);
                //    //#endregion

                //    //#region Delete Old Files
                //    //strErrRegion = "DeleteOldFiles()";
                //    //CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "TransferFiles() :: " + strErrRegion + " - Checking old files in File Log");
                //    //DeleteOldFiles();
                //    //#endregion

                //    #endregion

                //    #region for normal receiver
                //    CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "Moving files to " + strSNDDIR);

                //    Process process = new Process();
                //    ProcessStartInfo ProcMoveFiles = new ProcessStartInfo();
                //    string strBatchFile = AppDomain.CurrentDomain.BaseDirectory + "AddPrefixAndMoveFiles.bat";
                //    string strExecCommand = string.Empty;
                //    string strRand = string.Empty;
                //    string strSubPrefix = string.Empty;

                //    strSID = "S1DXXX" + DateTime.Now.ToString("MMddyyHHmmss");

                //    try
                //    {

                //        strRand = CoreCommon.RandomString(6);
                //        strPrefix = strSITECODE + "_" + strSID + "_" + strINSTNAME.Replace(" ", "_") + "_";
                //        strPrefix = strPrefix.Replace(" ", "_");
                //        strPrefix = strPrefix.Replace(",", "");
                //        strPrefix = strPrefix.Replace("(", "");
                //        strPrefix = strPrefix.Replace(")", "");
                //        strPrefix = strPrefix.Replace("'", "");
                //        strPrefix = strPrefix.Replace("\"", "");
                //        strPrefix = strPrefix.Replace("#", "");
                //        strPrefix = strPrefix.Replace("&", "");
                //        strPrefix = strPrefix.Replace("@", "");
                //        strPrefix = strPrefix.Replace("__", "_");
                //        strSubPrefix = strSITECODE + "_S1DXXX";

                //        if (strCOMPXFERFILE == "Y")
                //            strExecCommand = "/c " + strBatchFile + " " + @strRCVDIR + "\\*.zip " + @strSNDDIR + "\\ " + strPrefix + " " + strSubPrefix;
                //        else
                //            strExecCommand = "/c " + strBatchFile + " " + @strRCVDIR + "\\*.* " + @strSNDDIR + "\\ " + strPrefix + " " + strSubPrefix;

                //        ProcMoveFiles.UseShellExecute = false;
                //        ProcMoveFiles.FileName = System.Environment.GetEnvironmentVariable("COMSPEC");
                //        ProcMoveFiles.Arguments = strExecCommand;
                //        ProcMoveFiles.WindowStyle = ProcessWindowStyle.Hidden;
                //        ProcMoveFiles.CreateNoWindow = true;
                //        ProcMoveFiles.RedirectStandardOutput = true;
                //        ProcMoveFiles.RedirectStandardError = true;

                //        process.StartInfo = ProcMoveFiles;
                //        process.Start();
                //    }
                //    catch (Exception ex)
                //    {
                //        CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "TransferFiles() => Batch Move :: Exception : " + ex.Message);
                //    }
                //    finally
                //    {
                //        process.Close();
                //        process.Dispose();
                //        ProcMoveFiles = null;
                //        process = null;
                //    }
                //    #endregion
                //}
                #endregion

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "TransferFiles() :: " + strErrRegion + " - Exception: " + ex.Message);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }
            finally
            {
                arrSUID = null;
                arrRecFiles = null;
                arr = null;
                dd = null;
                ds.Dispose();
            }
        }
        #endregion

        #region CreateFileTable
        private DataTable CreateFileTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("file_name", System.Type.GetType("System.String"));
            dtbl.TableName = "Files";
            return dtbl;
        }
        #endregion

        #region CheckFolders
        private void CheckFolders()
        {
            string[] arrDirs = new string[0];
            string[] arrSubDirs = new string[0];
            string[] arrFiles = new string[0];
            string[] arrDirFiles = new string[0];
            string[] pathElements = new string[0];
            string[] arr = new string[0];
            string strFileName = string.Empty;
            string strDirName = string.Empty;
            string strSubDirName = string.Empty;

            arrDirs = Directory.GetDirectories(strRCVDIR + "\\");


            foreach (string strDir in arrDirs)
            {
                pathElements = strDir.Split('\\');
                strDirName = pathElements[(pathElements.Length - 1)];
                arrSubDirs = Directory.GetDirectories(strRCVDIR + "\\" + strDirName + "\\");
                arrFiles = Directory.GetFiles(strRCVDIR + "\\" + strDirName + "\\");

                #region move files from sub folders
                foreach (string strSubDir in arrSubDirs)
                {

                    pathElements = strSubDir.Split('\\');
                    strSubDirName = pathElements[(pathElements.Length - 1)];
                    arrDirFiles = Directory.GetFiles(strSubDirName + "\\");

                    foreach (string strSubDirFile in arrDirFiles)
                    {
                        pathElements = strSubDirFile.Split('\\');
                        strFileName = pathElements[(pathElements.Length - 1)];
                        strFileName = strFileName.Replace(" ", "_");
                        strFileName = strFileName.Replace("(", "");
                        strFileName = strFileName.Replace(")", "");
                        strFileName = strFileName.Replace("#", "");
                        strFileName = strFileName.Replace("?", "");
                        strFileName = strFileName.Replace("'", "");

                        if (File.Exists(strRCVDIR + "\\" + strFileName))
                        {
                            File.Delete(strRCVDIR + "\\" + strFileName);
                        }
                        File.Move(strSubDirFile, strRCVDIR + "\\" + strFileName);
                    }

                    Directory.Delete(strSubDir);

                }
                #endregion

                #region move files from folders
                foreach (string strFile in arrFiles)
                {

                    pathElements = strFile.Split('\\');
                    strFileName = pathElements[(pathElements.Length - 1)];
                    strFileName = strFileName.Replace(" ", "_");
                    strFileName = strFileName.Replace("(", "");
                    strFileName = strFileName.Replace(")", "");
                    strFileName = strFileName.Replace("#", "");
                    strFileName = strFileName.Replace("?", "");
                    strFileName = strFileName.Replace("'", "");

                    if (File.Exists(strRCVDIR + "\\" + strFileName))
                    {
                        File.Delete(strRCVDIR + "\\" + strFileName);
                    }
                    File.Move(strFile, strRCVDIR + "\\" + strFileName);
                }
                #endregion

                Directory.Delete(strDir);

            }

        }
        #endregion

        #region CheckManualFolders
        private void CheckManualFolders()
        {
            string[] arrDirs = new string[0];
            string[] arrSubDirs = new string[0];
            string[] arrDirFiles = new string[0];
            string[] arrFiles = new string[0];
            string[] arrSubDirFiles = new string[0];
            string[] pathElements = new string[0];
            string strFileName = string.Empty;
            string strDirName = string.Empty;
            string strSubDirName = string.Empty;
            string strFileContentType = string.Empty;

            try
            {
                if (Directory.Exists(strRCVDIRMANUAL))
                {
                    arrDirs = Directory.GetDirectories(strRCVDIRMANUAL + "\\");
                    arrFiles = Directory.GetFiles(strRCVDIRMANUAL + "\\");
                }

                foreach (string strDir in arrDirs)
                {

                    pathElements = strDir.Split('\\');
                    strDirName = pathElements[(pathElements.Length - 1)];
                    arrSubDirs = Directory.GetDirectories(strRCVDIRMANUAL + "\\" + strDirName + "\\");
                    arrDirFiles = Directory.GetFiles(strRCVDIRMANUAL + "\\" + strDirName + "\\");

                    #region move files from sub folders
                    foreach (string strSubDir in arrSubDirs)
                    {
                        pathElements = strSubDir.Split('\\');
                        strSubDirName = pathElements[(pathElements.Length - 1)];
                        arrSubDirFiles = Directory.GetFiles(strSubDirName + "\\");

                        foreach (string strSubDirFile in arrSubDirFiles)
                        {

                            pathElements = strSubDirFile.Split('\\');
                            strFileName = pathElements[(pathElements.Length - 1)];
                            strFileName = strFileName.Replace(" ", "_");
                            strFileName = strFileName.Replace("(", "");
                            strFileName = strFileName.Replace(")", "");

                            if (File.Exists(strRCVDIR + "\\" + strFileName))
                            {
                                File.Delete(strRCVDIR + "\\" + strFileName);
                            }
                            File.Move(strSubDirFile, strRCVDIR + "\\" + strFileName);

                        }

                        Directory.Delete(strSubDir);

                    }
                    #endregion

                    #region move files from folders
                    foreach (string strFile in arrDirFiles)
                    {
                        pathElements = strFile.Split('\\');
                        strFileName = pathElements[(pathElements.Length - 1)];
                        strFileName = strFileName.Replace(" ", "_");
                        strFileName = strFileName.Replace("(", "");
                        strFileName = strFileName.Replace(")", "");

                        if (File.Exists(strRCVDIR + "\\" + strFileName))
                        {
                            File.Delete(strRCVDIR + "\\" + strFileName);
                        }
                        File.Move(strFile, strRCVDIR + "\\" + strFileName);


                    }
                    #endregion

                    Directory.Delete(strDir);
                }

                #region move files from manual folder
                foreach (string strFile in arrFiles)
                {
                    pathElements = strFile.Split('\\');
                    strFileName = pathElements[(pathElements.Length - 1)];
                    strFileName = strFileName.Replace(" ", "_");
                    strFileName = strFileName.Replace("(", "");
                    strFileName = strFileName.Replace(")", "");
                    if (File.Exists(strRCVDIR + "\\" + strFileName))
                    {
                        File.Delete(strRCVDIR + "\\" + strFileName);
                    }
                    File.Move(strFile, strRCVDIR + "\\" + strFileName);
                }
                #endregion
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doProcess() => TransferFiles() => CheckManualFolders() - Exception: " + ex.Message);
            }

        }
        #endregion

        #region SearchFiles
        private void SearchFiles(DicomDecoder dd, string path)
        {
            System.IO.DriveInfo di = new System.IO.DriveInfo(path);
            System.IO.DirectoryInfo rootDir = di.RootDirectory;

            try
            {
                WalkDirectoryTree(rootDir, dd);
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doProcess()=>TransferFiles()=>SearchFiles() :: " + ex.Message);
            }
        }
        #endregion

        #region WalkDirectoryTree
        private void WalkDirectoryTree(System.IO.DirectoryInfo root, DicomDecoder dd)
        {
            string[] pathElements = new string[0];
            string[] fileElements = new string[0];
            string[] arr = new string[0];
            string strSUID = string.Empty;
            string strFile = string.Empty;
            string strFilename = string.Empty;
            string strPrefix = string.Empty;
            string strSID = string.Empty;
            string strExtn = string.Empty;

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
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "TransferFiles()=>WalkDirectoryTree() :: " + strErrRegion + " - Exception: " + ex.Message);
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                //Console.WriteLine(e.Message);
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "TransferFiles()=>WalkDirectoryTree() :: " + strErrRegion + " - Exception: " + ex.Message);
            }
            #endregion

            if (files != null)
            {
                foreach (System.IO.FileInfo fi in files)
                {
                    strFile = fi.FullName;
                    pathElements = strFile.Split('\\');
                    strFilename = pathElements[(pathElements.Length - 1)];
                    fileElements = new string[0];

                    strFilename = strFilename.Replace(" ", "_");
                    strFilename = strFilename.Replace("(", "");
                    strFilename = strFilename.Replace(")", "");
                    strFilename = strFilename.Replace("#", "");
                    strFilename = strFilename.Replace("?", "");
                    strFilename = strFilename.Replace("'", "");

                    if (strFilename.Contains("_"))
                    {
                        fileElements = strFilename.Split('_');
                        strSID = fileElements[1];
                    }
                    else
                        strSID = string.Empty;

                    #region Ignore this
                    //if (strSID.Length > 3)
                    //{
                    //    if (strSID.Substring(0, 3) != "S1D")
                    //    {
                    //        strSID = "S1DXXX";
                    //        strPrefix = CoreCommon.RandomString(6);
                    //        strFilename = strSID + "_" + strPrefix + "_" + strFilename;
                    //    }
                    //}
                    //else
                    //{
                    //    strSID = "S1DXXX";
                    //    strPrefix = CoreCommon.RandomString(6);
                    //    strFilename = strSID + "_" + strPrefix + "_" + strFilename;
                    //}
                    #endregion

                    //strFilename = strSITECODE + "_" + strSID + "_" + strINSTNAME.Replace(" ", "_") + "_" + strFilename.Replace(strSID + "_", "");
                    //strExtn = Path.GetExtension(strFile);
                    //if (strExtn.Trim() != string.Empty) strFilename = strFilename + strExtn;

                    //if (File.Exists(strRCVDIR + "\\" + strFilename)) File.Delete(strRCVDIR + "\\" + strFilename);
                    if (File.Exists(strSNDDIR + "\\" + strFilename)) File.Delete(strSNDDIR + "\\" + strFilename);

                    if (CoreCommon.IsDicomFile(strFile))
                    {
                        #region DICOM Files

                        //System.IO.File.Move(strFile, strRCVDIR + "\\" + strFilename);
                        System.IO.File.Move(strFile, strSNDDIR + "\\" + strFilename);
                        CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "TransferFiles()=>WalkDirectoryTree() :: " + fi.FullName + " MOVED");

                        #endregion
                    }
                    else if ((MIMEAssistant.GetMIMEType(strFile) == "image/jpeg") || (MIMEAssistant.GetMIMEType(strFile) == "image/gif") || (MIMEAssistant.GetMIMEType(strFile) == "image/png") || (MIMEAssistant.GetMIMEType(strFile) == "image/bmp"))
                    {
                        #region Image Files
                        //System.IO.File.Move(strFile, strRCVDIR + "\\" + strFilename);
                        System.IO.File.Move(strFile, strSNDDIR + "\\" + strFilename);
                        CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "TransferFiles()=>WalkDirectoryTree() :: " + fi.FullName + " MOVED");
                        #endregion
                    }



                }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    WalkDirectoryTree(dirInfo, dd);
                }
            }
        }
        #endregion

        #region SearchFilesManual
        private void SearchFilesManual(DicomDecoder dd, string path)
        {
            System.IO.DirectoryInfo rootDir = new DirectoryInfo(path);

            try
            {
                WalkDirectoryTreeManual(rootDir, dd);
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doProcess()=>TransferFiles()=>SearchFilesManual() :: " + ex.Message);
            }
        }
        #endregion

        #region WalkDirectoryTreeManual
        private void WalkDirectoryTreeManual(System.IO.DirectoryInfo root, DicomDecoder dd)
        {
            string[] pathElements = new string[0];
            string[] fileElements = new string[0];
            string[] arr = new string[0];
            //int isDiacom = -1;

            string strSUID = string.Empty;
            string strFile = string.Empty;
            string strFilename = string.Empty;
            string strParentFolder = string.Empty;
            string strMIMEType = string.Empty;
            string strExtn = string.Empty;
            string strDirName = string.Empty;
            string[] arrFiles = new string[0];
            string strPrefix = string.Empty;
            string strSID = string.Empty;

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
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "TransferFiles()=>WalkDirectoryTreeManual() :: " + strErrRegion + " - Exception: " + ex.Message);
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                //Console.WriteLine(e.Message);
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "TransferFiles()=>WalkDirectoryTreeManual() :: " + strErrRegion + " - Exception: " + ex.Message);
            }
            #endregion

            if (files != null)
            {
                strParentFolder = "";

                foreach (System.IO.FileInfo fi in files)
                {
                    strParentFolder = fi.DirectoryName;
                    strFile = fi.FullName;
                    pathElements = strFile.Split('\\');
                    strFilename = pathElements[(pathElements.Length - 1)];
                    fileElements = new string[0];

                    strFilename = strFilename.Replace(" ", "_");
                    strFilename = strFilename.Replace("(", "");
                    strFilename = strFilename.Replace(")", "");
                    strFilename = strFilename.Replace("#", "");
                    strFilename = strFilename.Replace("?", "");
                    strFilename = strFilename.Replace("'", "");

                    if (strFilename.Contains("_"))
                    {
                        fileElements = strFilename.Split('_');
                        strSID = fileElements[1];
                    }
                    else
                        strSID = string.Empty;

                    #region Ignore this
                    //if (strSID.Length > 3)
                    //{
                    //    if (strSID.Substring(0, 3) != "S1D")
                    //    {
                    //        strSID = "S1DXXX";
                    //        strPrefix = CoreCommon.RandomString(6);
                    //        strFilename = strSID + "_" + strPrefix + "_" + strFilename;
                    //    }
                    //}
                    //else
                    //{
                    //    strSID = "S1DXXX";
                    //    strPrefix = CoreCommon.RandomString(6);
                    //    strFilename = strSID + "_" + strPrefix + "_" + strFilename;
                    //}
                    #endregion

                    //strFilename = strSITECODE + "_" + strSID + "_" + strINSTNAME.Replace(" ", "_") + "_" + strFilename.Replace(strSID + "_", "");
                    //strExtn = Path.GetExtension(strFile);
                    //if (strExtn.Trim() != string.Empty) strFilename = strFilename + strExtn;

                    //if (File.Exists(strRCVDIR + "\\" + strFilename)) File.Delete(strRCVDIR + "\\" + strFilename);
                    if (File.Exists(strSNDDIR + "\\" + strFilename)) File.Delete(strSNDDIR + "\\" + strFilename);


                    if (CoreCommon.IsDicomFile(strFile))
                    {
                        #region DICOM Files
                        //if (strFilename.ToUpper().Contains("DICOMDIR"))
                        //{
                        //    #region archiving files to junk folder
                        //    if (!Directory.Exists(strARCHDIR + "\\Junk"))
                        //    {
                        //        Directory.CreateDirectory(strARCHDIR + "\\Junk");
                        //    }
                        //    strDirName = DateTime.Today.ToString("yyyyMMdd");
                        //    if (!Directory.Exists(strARCHDIR + "\\Junk\\" + strDirName))
                        //    {
                        //        Directory.CreateDirectory(strARCHDIR + "\\Junk\\" + strDirName);
                        //    }
                        //    if (File.Exists(strARCHDIR + "\\Junk\\" + strDirName + "\\" + strFilename))
                        //    {
                        //        File.Delete(strARCHDIR + "\\Junk\\" + strDirName + "\\" + strFilename);
                        //    }
                        //    File.Move(strFile, strARCHDIR + "\\Junk\\" + strDirName + "\\" + strFilename);
                        //    CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "TransferFiles() =>WalkDirectoryTreeManual() :: " + strErrRegion + " - File : " + strFilename + " - invalid file type...Archived to junk");

                        //    #endregion
                        //}
                        //else
                        //{

                        //System.IO.File.Move(strFile, strRCVDIR + "\\" + strFilename);



                        System.IO.File.Move(strFile, strSNDDIR + "\\" + strFilename);
                        CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "TransferFiles()=>WalkDirectoryTreeManual() :: " + fi.FullName + " MOVED");

                        //}
                        #endregion
                    }
                    else if ((MIMEAssistant.GetMIMEType(strFile) == "image/jpeg") || (MIMEAssistant.GetMIMEType(strFile) == "image/gif") || (MIMEAssistant.GetMIMEType(strFile) == "image/png") || (MIMEAssistant.GetMIMEType(strFile) == "image/bmp"))
                    {
                        #region Image Files
                        //System.IO.File.Move(strFile, strRCVDIR + "\\" + strFilename);
                        System.IO.File.Move(strFile, strSNDDIR + "\\" + strFilename);
                        CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "TransferFiles()=>WalkDirectoryTreeManual() :: " + fi.FullName + " MOVED");
                        #endregion
                    }
                    else
                    {
                        #region archiving files to junk folder
                        if (!Directory.Exists(strARCHDIR + "\\Junk"))
                        {
                            Directory.CreateDirectory(strARCHDIR + "\\Junk");
                        }
                        strDirName = DateTime.Today.ToString("yyyyMMdd");
                        if (!Directory.Exists(strARCHDIR + "\\Junk\\" + strDirName))
                        {
                            Directory.CreateDirectory(strARCHDIR + "\\Junk\\" + strDirName);
                        }
                        if (File.Exists(strARCHDIR + "\\Junk\\" + strDirName + "\\" + strFilename))
                        {
                            File.Delete(strARCHDIR + "\\Junk\\" + strDirName + "\\" + strFilename);
                        }
                        File.Move(strFile, strARCHDIR + "\\Junk\\" + strDirName + "\\" + strFilename);
                        CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "TransferFiles() =>WalkDirectoryTreeManual() :: " + strErrRegion + " - File : " + strFilename + " - invalid file type...Archived to junk");

                        #endregion
                    }
                }


                if (strParentFolder.Trim() != string.Empty)
                {
                    if (strParentFolder.Trim() != strRCVDIRMANUAL.Trim())
                    {
                        arrFiles = Directory.GetFiles(strParentFolder);
                        if (arrFiles.Length == 0) Directory.Delete(strParentFolder.Trim());
                    }
                }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    WalkDirectoryTreeManual(dirInfo, dd);
                    if ((dirInfo.GetFiles().Length == 0) && (dirInfo.GetDirectories().Length == 0)) dirInfo.Delete();
                }
            }
        }
        #endregion

        #region CheckImageFolders
        private void CheckImageFolders()
        {
            string[] arrDirs = new string[0];
            string[] arrFiles = new string[0];
            string[] arrDirFiles = new string[0];
            string[] pathElements = new string[0];
            string[] arr = new string[0];
            string strSUID = string.Empty;
            string strFileName = string.Empty;
            string strDirName = string.Empty;
            //int exitCode;
            bool bReturn = false;
            string strCatchMessage = string.Empty;
            //CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "In CheckImageFolders()");

            if (Directory.Exists(strRCVIMGDIR))
                arrFiles = Directory.GetFiles(strRCVIMGDIR + "\\");

            // CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "Files found - " + arrFiles.Length.ToString());

            if (arrFiles.Length > 0)
            {
                objCore = new Scheduler();
                //strErrRegion = "DICOMISING image files";
                try
                {
                    bReturn = objCore.FetchSchedulerSettings(strConfigPath, ref strCatchMessage);
                    if (bReturn)
                    {
                        strINSTNAME = objCore.INSTITUTION_NAME;

                        foreach (string strFile in arrFiles)
                        {

                            pathElements = strFile.Split('\\');
                            strFileName = pathElements[(pathElements.Length - 1)];

                            #region Dicomise image files (Suspended)
                            //Process ProcImgToDcm = new Process();
                            //ProcImgToDcm.StartInfo.UseShellExecute = false;
                            //ProcImgToDcm.StartInfo.FileName = strConfigPath + "\\AccesoryDirs\\DICOM-EXEs\\IMGToDCM\\ImageToDicomConverter.exe"; ;
                            //ProcImgToDcm.StartInfo.Arguments = strSUID + "±" + strDir.Trim().Replace(" ", "»") + "±" + strRCVDIR.Trim().Replace(" ", "»") + "±" + strINSTNAME.Replace(" ", "_");
                            //ProcImgToDcm.StartInfo.RedirectStandardOutput = true;
                            //ProcImgToDcm.Start();
                            //ProcImgToDcm.WaitForExit();

                            //exitCode = ProcImgToDcm.ExitCode;

                            //if (ProcImgToDcm.HasExited)
                            //{
                            //    if (exitCode <= 0)
                            //        CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "CheckImageFolders() - Conversion To DCM for Folder : " + strDir + " failed");
                            //}
                            #endregion

                            #region Transfer Files To Receiving Folders
                            //CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "CheckImageFolders() - File : " + strFileName + " Mime Type - " + MIMEAssistant.GetMIMEType(strFile));
                            if ((MIMEAssistant.GetMIMEType(strFile) == "image/jpeg") || (MIMEAssistant.GetMIMEType(strFile) == "image/gif") || (MIMEAssistant.GetMIMEType(strFile) == "image/png") || (MIMEAssistant.GetMIMEType(strFile) == "image/bmp"))
                            {
                                if (!Directory.Exists(strRCVDIR)) Directory.CreateDirectory(strRCVDIR);
                                strFileName = strFileName.Replace(" ", "_");
                                strFileName = strFileName.Replace("(", "");
                                strFileName = strFileName.Replace(")", "");
                                if (File.Exists(strRCVDIR + "\\" + strFileName)) File.Delete(strRCVDIR + "\\" + strFileName);

                                File.Copy(strFile, strRCVDIR + "\\" + strFileName);
                                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "CheckImageFolders() - File : " + strFileName + " transfered to receiving folder");
                            }
                            #endregion

                            #region archiving files
                            if (!Directory.Exists(strARCHDIR + "\\ImageFiles"))
                            {
                                Directory.CreateDirectory(strARCHDIR + "\\ImageFiles");
                            }
                            strDirName = DateTime.Today.ToString("yyyyMMdd");
                            if (!Directory.Exists(strARCHDIR + "\\ImageFiles\\" + strDirName))
                            {
                                Directory.CreateDirectory(strARCHDIR + "\\ImageFiles\\" + strDirName);
                            }
                            if (File.Exists(strARCHDIR + "\\ImageFiles\\" + strDirName + "\\" + strFileName))
                            {
                                File.Delete(strARCHDIR + "\\ImageFiles\\" + strDirName + "\\" + strFileName);
                            }
                            File.Move(strFile, strARCHDIR + "\\ImageFiles\\" + strDirName + "\\" + strFileName);
                            CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "CheckImageFolders() - File : " + strFileName + " Archived");

                            #endregion

                            // strErrRegion = "Deleting Image Directory";

                            // Directory.Delete(strDir);
                        }
                    }
                    else
                    {
                        CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "CheckImageFolders() - Error : " + strCatchMessage);
                    }
                }
                catch (Exception expErr)
                {
                    CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "CheckImageFolders() - Exception: " + expErr.Message);
                    System.Threading.Thread.Sleep(intFreq * 1000);

                }
                finally
                {
                    objCore = null;
                }
            }


            //#region deleting image files not in folder
            //strErrRegion = "Deleting ungrouped image files";

            //arrFiles = Directory.GetFiles(strRCVIMGDIR + "\\");
            //foreach (string strFile in arrFiles)
            //{
            //    File.Delete(strFile);
            //}

            //#endregion
        }
        #endregion

        #region SearchImageFiles
        private void SearchImageFiles(string path)
        {
            System.IO.DriveInfo di = new System.IO.DriveInfo(path);
            System.IO.DirectoryInfo rootDir = di.RootDirectory;

            try
            {
                WalkImageDirectoryTree(rootDir);
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doProcess()=>TransferFiles()=>SearchImageFiles() :: " + ex.Message);
            }
        }
        #endregion

        #region WalkImageDirectoryTree
        private void WalkImageDirectoryTree(System.IO.DirectoryInfo root)
        {
            string[] pathElements = new string[0];
            string[] arr = new string[0];

            string strFile = string.Empty;
            string strFilename = string.Empty;

            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

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
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "TransferFiles()=>WalkImageDirectoryTree() :: " + strErrRegion + " - Exception: " + ex.Message);
            }

            catch (System.IO.DirectoryNotFoundException ex)
            {
                //Console.WriteLine(e.Message);
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "TransferFiles()=>WalkImageDirectoryTree() :: " + strErrRegion + " - Exception: " + ex.Message);
            }

            if (files != null)
            {
                foreach (System.IO.FileInfo fi in files)
                {
                    // In this example, we only access the existing FileInfo object. If we
                    // want to open, delete or modify the file, then
                    // a try-catch block is required here to handle the case
                    // where the file has been deleted since the call to TraverseTree().
                    //Console.WriteLine(fi.FullName);

                    strFile = fi.FullName;
                    pathElements = strFile.Split('\\');
                    strFilename = pathElements[(pathElements.Length - 1)];
                    strFilename = strFilename.Replace(" ", "_");
                    strFilename = strFilename.Replace("(", "");
                    strFilename = strFilename.Replace(")", "");

                    if ((MIMEAssistant.GetMIMEType(strFile) == "image/jpeg") || (MIMEAssistant.GetMIMEType(strFile) == "image/gif") || (MIMEAssistant.GetMIMEType(strFile) == "image/png") || (MIMEAssistant.GetMIMEType(strFile) == "image/bmp"))
                    {

                        if (File.Exists(strRCVDIR + "\\" + strFilename)) File.Delete(strRCVDIR + "\\" + strFilename);
                        System.IO.File.Move(strFile, strRCVDIR + "\\" + strFilename);
                        CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "TransferFiles()=>WalkImageDirectoryTree() :: " + fi.FullName + " MOVED");
                    }

                }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    WalkImageDirectoryTree(dirInfo);
                }
            }
        }
        #endregion

        #region SearchImageFilesManual
        private void SearchImageFilesManual(string path)
        {
            System.IO.DirectoryInfo rootDir = new DirectoryInfo(path);

            try
            {
                WalkImageDirectoryTreeManual(rootDir);
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "doProcess()=>TransferFiles()=>SearchImageFiles() :: " + ex.Message);
            }
        }
        #endregion

        #region WalkImageDirectoryTreeManual
        private void WalkImageDirectoryTreeManual(System.IO.DirectoryInfo root)
        {
            string[] pathElements = new string[0];
            string[] arr = new string[0];

            string strParentFolder = string.Empty;
            string strFile = string.Empty;
            string strFilename = string.Empty;
            string[] arrFiles = new string[0];

            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

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
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "TransferFiles()=>WalkImageDirectoryTree() :: " + strErrRegion + " - Exception: " + ex.Message);
            }

            catch (System.IO.DirectoryNotFoundException ex)
            {
                //Console.WriteLine(e.Message);
                CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "TransferFiles()=>WalkImageDirectoryTree() :: " + strErrRegion + " - Exception: " + ex.Message);
            }

            if (files != null)
            {
                strParentFolder = "";

                foreach (System.IO.FileInfo fi in files)
                {
                    // In this example, we only access the existing FileInfo object. If we
                    // want to open, delete or modify the file, then
                    // a try-catch block is required here to handle the case
                    // where the file has been deleted since the call to TraverseTree().
                    //Console.WriteLine(fi.FullName);

                    strParentFolder = fi.DirectoryName;
                    strFile = fi.FullName;
                    pathElements = strFile.Split('\\');
                    strFilename = pathElements[(pathElements.Length - 1)];
                    strFilename = strFilename.Replace(" ", "_");
                    strFilename = strFilename.Replace("(", "");
                    strFilename = strFilename.Replace(")", "");

                    if ((MIMEAssistant.GetMIMEType(strFile) == "image/jpeg") || (MIMEAssistant.GetMIMEType(strFile) == "image/gif") || (MIMEAssistant.GetMIMEType(strFile) == "image/png") || (MIMEAssistant.GetMIMEType(strFile) == "image/bmp"))
                    {

                        if (File.Exists(strRCVDIR + "\\" + strFilename)) File.Delete(strRCVDIR + "\\" + strFilename);
                        System.IO.File.Move(strFile, strRCVDIR + "\\" + strFilename);
                        CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "TransferFiles()=>WalkImageDirectoryTree() :: " + fi.FullName + " MOVED");
                    }

                }

                if (strParentFolder.Trim() != string.Empty)
                {
                    if (strParentFolder.Trim() != strRCVDIRMANUAL.Trim())
                    {
                        arrFiles = Directory.GetFiles(strParentFolder);
                        if (arrFiles.Length == 0) Directory.Delete(strParentFolder.Trim());
                    }
                }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    WalkImageDirectoryTreeManual(dirInfo);
                    if ((dirInfo.GetFiles().Length == 0) && (dirInfo.GetDirectories().Length == 0)) dirInfo.Delete();
                }
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

        #region SUIDExists
        private bool SUIDExists(List<string> arrSUID, string strSUID)
        {
            bool bReturn = false;

            foreach (string strUID in arrSUID)
            {
                if (strUID == strSUID)
                {
                    bReturn = true;
                    break;
                }
            }

            return bReturn;
        }
        #endregion

        #region DeleteOldFiles
        private void DeleteOldFiles()
        {
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\FileLog"))
            {
                DirectoryInfo info = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\FileLog");
                FileInfo[] files = info.GetFiles("*.xml").OrderByDescending(p => p.CreationTime).ToArray();
                int i = 0;


                try
                {

                    foreach (FileInfo file in files)
                    {
                        i = i + 1;
                        if (i > 1)
                        {
                            DataSet ds = new DataSet();
                            ds.ReadXml(file.FullName);
                            if (ds.Tables.Count > 0)
                            {
                                foreach (DataRow dr in ds.Tables["Files"].Rows)
                                {
                                    if (File.Exists(strRCVDIR + "\\" + Convert.ToString(dr["file_name"])))
                                    {
                                        File.Delete(strRCVDIR + "\\" + Convert.ToString(dr["file_name"]));
                                    }
                                }
                            }
                            ds.Dispose();
                            File.Delete(file.FullName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "Y", "DeleteOldFiles() :: Exception: " + ex.Message);
                    System.Threading.Thread.Sleep(intFreq * 1000);
                }
            }

        }
        #endregion
    }
}
