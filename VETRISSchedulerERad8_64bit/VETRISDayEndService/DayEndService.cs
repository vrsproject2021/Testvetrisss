using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Net;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Configuration;
using System.Security;
using VETRISScheduler.Core;
using eRADCls;

namespace VETRISDayEndService
{
    public partial class DayEndService : ServiceBase
    {
        #region members & variables
        private static int intFreq = 30;
        private static string strURL = string.Empty;
        private static string strConfigPath = AppDomain.CurrentDomain.BaseDirectory;
        private static string strSvcName = "VETRIS Dayend Service";
        private static int intServiceID = 5;
        private static string strDocDCMPath = string.Empty;
        private static string strFTPDLFLDRTMP = string.Empty;
        private static string strMailInvFolder = string.Empty;
        private static string strWS8SRVIP = string.Empty;
        private static string strWS8CLTIP = string.Empty;
        private static string strWS8SRVUID = string.Empty;
        private static string strWS8SRVPWD = string.Empty;
        private static string strTEMPDCMATCHPATH = string.Empty;
        private static string strPACSXFERDLFLDR = string.Empty;
        private static string strPACSARCHALTFLDR = string.Empty;
        Scheduler objCore;
        DayEnd objDE;
        #endregion

        public DayEndService()
        {
            InitializeComponent();
        }

        #region OnStart
        protected override void OnStart(string[] args)
        {

            try
            {

                System.Threading.ThreadStart job_data_synch = new System.Threading.ThreadStart(doProcess);
                System.Threading.Thread thread = new System.Threading.Thread(job_data_synch);
                thread.Start();


            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Error starting Service. " + ex.Message, true);
                EventLog.WriteEntry(strSvcName, "Error Starting Service." + ex.Message, EventLogEntryType.Warning);
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
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Error stopping Service. " + ex.Message, true);
            }
        }
        #endregion

        #region doProcess
        private void doProcess()
        {
            string strCatchMessage = string.Empty;

            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Service started Successfully", false);
                while (true)
                {
                    objCore = new Scheduler();
                    objCore.SERVICE_ID = intServiceID;


                    try
                    {

                        if (objCore.GetServiceDetails(strConfigPath, ref strCatchMessage))
                        {

                            intFreq = objCore.FREQUENCY;
                            strSvcName = objCore.SERVICE_NAME;
                            strDocDCMPath = objCore.DOCUMENT_AND_DCM_PATH;
                            strFTPDLFLDRTMP = objCore.TEMPORARY_DOWNLOAD_FOLDER;
                            strMailInvFolder = objCore.MAIL_INVOICE_FOLDER;
                            strWS8SRVIP = objCore.WS8_SERVER_URL;
                            strWS8CLTIP = objCore.CLIENT_IP_URL;
                            strWS8SRVUID = objCore.WS8_USER_ID;
                            strWS8SRVPWD = objCore.WS8_PASSWORD;
                            strTEMPDCMATCHPATH = objCore.TEMPORARY_DCM_ATTCHMENT_FILE_PATH;
                            strPACSXFERDLFLDR = objCore.PACS_ARCHIVE_FOLDER;
                            strPACSARCHALTFLDR = objCore.PACS_ARCHIVE_ALTERNATE_FOLDER;
                            TruncateDataLog();
                            ProcessLogDB();
                        }
                        else
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Core::GetServiceDetails - Error : " + strCatchMessage, true);

                    }
                    catch (Exception ex)
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcess() - Error: " + ex.Message, true);
                        EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Warning);

                    }


                    objCore = null;
                    System.Threading.Thread.Sleep(intFreq * 1000);



                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcess() - Exception: " + expErr.Message, true);
            }
            finally
            { objCore = null; }
        }
        #endregion

        #region TruncateDataLog
        private void TruncateDataLog()
        {

            string strResult = string.Empty;
            string strCatchMsg = string.Empty;
            objDE = new DayEnd();


            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Truncating Data Log...", false);
                if (objDE.ProcessDayEnd(strConfigPath, intServiceID, strSvcName, ref strCatchMsg))
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Log data truncated ", false);

                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "TruncateDataLog()  - Error: " + strCatchMsg, true);
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "TruncateDataLog() - Exception: " + ex.Message, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);

            }
            finally
            {
                objDE = null;
            }

            DeleteFiles();
        }
        #endregion

        #region DeleteFiles
        private void DeleteFiles()
        {
            string[] arrParentFolders = Directory.GetDirectories(strDocDCMPath);
            string strParFldr = string.Empty;
            string strFileName = string.Empty;
            string strExt = string.Empty;
            string[] arrFiles = new string[0];
            string[] arrDCMFolder = new string[0]; string[] arrDCMFiles = new string[0];
            string[] arrDocFolder = new string[0]; string[] arrDocFiles = new string[0];
            string[] arrImgFolder = new string[0]; string[] arrImgFiles = new string[0];
            int intCount = 0; string strCatchMessage = string.Empty;
            objDE = new DayEnd();

            #region Delete DCM & Doc. files
            try
            {

                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting DCM & Doc. files ...", false);
                for (int i = 0; i < arrParentFolders.Length; i++)
                {
                    strParFldr = arrParentFolders[i].Substring(arrParentFolders[i].LastIndexOf("\\") + 1);

                    switch (strParFldr)
                    {
                        case "DCM":
                            arrDCMFolder = Directory.GetDirectories(arrParentFolders[i]);
                            for (int j = 0; j < arrDCMFolder.Length; j++)
                            {

                                TimeSpan t = DateTime.Now - Directory.GetLastWriteTime(arrDCMFolder[j]);
                                if (t.TotalDays >= 2)
                                {
                                    arrDCMFiles = Directory.GetFiles(arrDCMFolder[j]);

                                    for (int k = 0; k < arrDCMFiles.Length; k++)
                                    {
                                        File.Delete(arrDCMFiles[k]);
                                    }

                                    Directory.Delete(arrDCMFolder[j]);
                                }
                            }
                            break;
                        case "Docs":
                            arrDocFolder = Directory.GetDirectories(arrParentFolders[i]);
                            for (int j = 0; j < arrDocFolder.Length; j++)
                            {

                                TimeSpan t = DateTime.Now - Directory.GetLastWriteTime(arrDocFolder[j]);
                                
                                if (t.TotalDays >= 2)
                                {
                                    arrDocFiles = Directory.GetFiles(arrDocFolder[j]);
                                    for (int k = 0; k < arrDocFiles.Length; k++)
                                    {
                                        File.Delete(arrDocFiles[k]);
                                    }

                                    Directory.Delete(arrDocFolder[j]);
                                }
                            }
                            break;
                        case "Img":
                            arrImgFolder = Directory.GetDirectories(arrParentFolders[i]);
                            for (int j = 0; j < arrImgFolder.Length; j++)
                            {

                                TimeSpan t = DateTime.Now - Directory.GetLastWriteTime(arrImgFolder[j]);
                                if (t.TotalDays >= 2)
                                {
                                    arrImgFiles = Directory.GetFiles(arrImgFolder[j]);
                                    for (int k = 0; k < arrImgFiles.Length; k++)
                                    {
                                        File.Delete(arrImgFiles[k]);
                                    }

                                    Directory.Delete(arrImgFolder[j]);
                                }
                            }
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteFiles() - Deleting DCM & Doc. files - Exception: " + ex.Message, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
            }
            #endregion

            #region Delete .bak files
            try
            {

                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting .bak files from " + strFTPDLFLDRTMP, false);
                arrFiles = Directory.GetFiles(strFTPDLFLDRTMP);

                for (int i = 0; i < arrFiles.Length; i++)
                {
                    strFileName = arrFiles[i].Trim();
                    strExt = arrFiles[i].Trim().Substring(arrFiles[i].Length - 4, 4).ToUpper();
                    if (strExt.ToUpper() == ".BAK")
                    {
                        if (File.Exists(strFileName)) File.Delete(strFileName);
                    }

                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteFiles() - Deleting .bak files - Exception: " + ex.Message + " File : " + strFileName, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
            }
            #endregion

            #region Delete Invoice files
            try
            {
                arrFiles = new string[0];
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting invoice files from " + strMailInvFolder, false);
                arrFiles = Directory.GetFiles(strMailInvFolder);

                for (int i = 0; i < arrFiles.Length; i++)
                {
                    intCount = 0;
                    strCatchMessage = string.Empty;
                    strFileName = arrFiles[i].Trim();

                    if (objDE.CheckAttachmentFileToDelete(strConfigPath, ref intCount, ref strCatchMessage))
                    {
                        if (intCount > 0)
                        {
                            if (File.Exists(strFileName)) File.Delete(strFileName);
                        }
                    }
                    else
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "CheckAttachmentFileToDelete() --> Exception : " + strCatchMessage, true);
                    }
                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteFiles() - Deleting invoice files - Exception: " + ex.Message + " File : " + strFileName, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
            }
            #endregion

            #region Delete Temp Manual files
            try
            {
                if (Directory.Exists(strConfigPath + "\\TempManualFiles"))
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting temporary scheduler files from " + strConfigPath + "\\TempManualFiles", false);
                    arrFiles = Directory.GetFiles(strConfigPath + "\\TempManualFiles");

                    for (int i = 0; i < arrFiles.Length; i++)
                    {
                        strFileName = arrFiles[i].Trim();

                        if (File.Exists(strFileName)) File.Delete(strFileName);
                    }
                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteFiles() - Deleting Temp Manual files - Exception: " + ex.Message + " File : " + strFileName, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
            }
            #endregion

            #region Delete Temp DCM files
            try
            {
                if (Directory.Exists(strTEMPDCMATCHPATH))
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting temporary DCM files", false);
                    arrFiles = Directory.GetFiles(strTEMPDCMATCHPATH);

                    for (int i = 0; i < arrFiles.Length; i++)
                    {
                        strFileName = arrFiles[i].Trim();

                        if (File.Exists(strFileName)) File.Delete(strFileName);
                    }
                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteFiles() - Deleting Temp DCM files - Exception: " + ex.Message + " File : " + strFileName, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
            }
            #endregion

            #region Delete Report files
            try
            {
                if (Directory.Exists(strConfigPath + "\\TempRpts"))
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting temporary scheduler files from " + strConfigPath + "\\TempRpts", false);
                    arrFiles = Directory.GetFiles(strConfigPath + "\\TempRpts");

                    for (int i = 0; i < arrFiles.Length; i++)
                    {
                        strFileName = arrFiles[i].Trim();

                        if (File.Exists(strFileName)) File.Delete(strFileName);
                    }
                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteFiles() - Deleting report files - Exception: " + ex.Message + " File : " + strFileName, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
            }
            #endregion

            //D:\VRSApp\VETRIS\CaseList\MSTemp
            //D:\VRSApp\VETRIS\CaseList\DocPrint\Temp
            //D:\VRSApp\VETRIS\Invoicing\DocumentPrinting\Temp

            objDE = null;
            DeleteUnviewedDICOMRecords();
        }
        #endregion

        #region Delete Unviewed Studies

        #region DeleteUnviewedDICOMRecords
        private void DeleteUnviewedDICOMRecords()
        {
            string strError = string.Empty;
            string strCatchMsg = string.Empty;
            string strReturnMessage = string.Empty;
            objDE = new DayEnd();
            DataSet ds = new DataSet();
            string strRecByDR = string.Empty;
            Guid StudyID = new Guid("00000000-0000-0000-0000-000000000000");
            string strUID = string.Empty;
            bool bReturn = false;
            string InstttutionCode = string.Empty;
            string InstitutionName = string.Empty;
            string strFileName = string.Empty;
            string strFolder = string.Empty;
            string[] arrTemp = new string[0];

            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting UNVIEWED DICOM Record(s)...", false);
                if (objDE.FetchStudiesToDelete(strConfigPath, ref ds, ref strCatchMsg))
                {

                    foreach (DataRow dr in ds.Tables["Study"].Rows)
                    {
                        bReturn = false;
                        StudyID = new Guid(Convert.ToString(dr["study_id"]));
                        strUID = Convert.ToString(dr["study_uid"]).Trim();
                        strRecByDR = Convert.ToString(dr["received_via_dicom_router"]).Trim();
                        InstttutionCode = Convert.ToString(dr["inst_code"]).Trim();
                        InstitutionName = Convert.ToString(dr["inst_name"]).Trim();
                        strFolder = InstttutionCode + "_" + InstitutionName + "_" + strUID;

                        if (strRecByDR == "N")
                        {
                            #region Delete Study From PACS
                            RadWebClass client = new RadWebClass();

                            try
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleteing UNVIEWED Study UID : " + strUID + " from PACS...", false);
                                bReturn = client.DeleteStudyData(string.Empty, strWS8SRVIP, strUID, ref strCatchMsg, ref strError);

                                if (bReturn)
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UNVIEWED Study UID : " + strUID + " deleted from PACS", false);

                                    if (Directory.Exists(strPACSXFERDLFLDR + "\\" + strFolder))
                                    {
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting dicom files for Study UID : " + strUID + " ... from " + strPACSXFERDLFLDR + "\\" + strFolder, false);
                                        arrTemp = Directory.GetFiles(strPACSXFERDLFLDR + "\\" + strFolder);
                                        if (arrTemp.Length > 0)
                                        {
                                            for (int i = 0; i < arrTemp.Length; i++)
                                            {
                                                File.Delete(arrTemp[i]);
                                            }
                                        }

                                        Directory.Delete(strPACSXFERDLFLDR + "\\" + strFolder,true);
                                    }
                                    if (Directory.Exists(strPACSARCHALTFLDR + "\\" + strFolder))
                                    {
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting dicom files for Study UID : " + strUID + " ... from " + strPACSARCHALTFLDR + "\\" + strFolder, false);
                                        arrTemp = Directory.GetFiles(strPACSARCHALTFLDR + "\\" + strFolder);
                                        if (arrTemp.Length > 0)
                                        {
                                            for (int i = 0; i < arrTemp.Length; i++)
                                            {
                                                File.Delete(arrTemp[i]);
                                            }
                                        }

                                        Directory.Delete(strPACSARCHALTFLDR + "\\" + strFolder, true);
                                    }
                                }
                                else
                                {
                                    if (strCatchMsg.Trim() != string.Empty)
                                    {
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, strCatchMsg, true);
                                    }
                                    else
                                    {
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, strError, true);
                                    }

                                }


                            }
                            catch (Exception ex)
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteUnviewedDICOMRecords() - client.DeleteStudyData() - Exception: " + ex.Message, true);
                                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);

                            }
                            finally
                            {
                                client = null;
                            }
                            #endregion
                        }
                        else if (strRecByDR == "Y")
                        {
                            #region Delete study from VETRIS
                            try
                            {
                                DataView dv = new DataView(ds.Tables["File"]);
                                dv.RowFilter = "study_uid='" + strUID.Trim() + "'";

                                if (dv.ToTable().Rows.Count > 0)
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting dicom files for Study UID : " + strUID + " ... from " + strFTPDLFLDRTMP, false);

                                    #region deleting files
                                    foreach (DataRow drStudy in dv.ToTable().Rows)
                                    {
                                        strFileName = Convert.ToString(drStudy["file_name"]).Trim();
                                        if (File.Exists(strFTPDLFLDRTMP + "\\" + strFileName))
                                        {
                                            System.IO.File.Delete(strFTPDLFLDRTMP + "\\" + strFileName);
                                        }
                                    }
                                    #endregion

                                    if(Directory.Exists(strPACSXFERDLFLDR + "\\"  + strFolder))
                                    {
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting dicom files for Study UID : " + strUID + " ... from " + strPACSXFERDLFLDR + "\\" + strFolder, false);
                                        arrTemp = Directory.GetFiles(strPACSXFERDLFLDR + "\\" + strFolder);
                                        if (arrTemp.Length > 0)
                                        {
                                            for (int i = 0; i < arrTemp.Length; i++)
                                            {
                                                File.Delete(arrTemp[i]);
                                            }
                                        }
                                        Directory.Delete(strPACSXFERDLFLDR + "\\" + strFolder, true);
                                    }
                                    if (Directory.Exists(strPACSARCHALTFLDR + "\\" + strFolder))
                                    {
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting dicom files for Study UID : " + strUID + " ... from " + strPACSARCHALTFLDR + "\\" + strFolder, false);
                                        arrTemp = Directory.GetFiles(strPACSARCHALTFLDR + "\\" + strFolder);
                                        if (arrTemp.Length > 0)
                                        {
                                            for (int i = 0; i < arrTemp.Length; i++)
                                            {
                                                File.Delete(arrTemp[i]);
                                            }
                                        }

                                        Directory.Delete(strPACSARCHALTFLDR + "\\" + strFolder, true);
                                    }
                                    
                                }

                                dv.Dispose();

                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Dicom files for Study UID : " + strUID + " deleted, deleting the study...", false);


                            }
                            catch (Exception ex)
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteUnviewedDICOMRecords() - client.DeleteStudyData() - Exception: " + ex.Message, true);
                                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);

                            }
                            #endregion
                        }

                        #region delete study

                        try
                        {
                            objDE.STUDY_ID = StudyID;
                            objDE.STUDY_UID = strUID;

                            if (!objDE.DeleteStudy(strConfigPath, intServiceID, strSvcName, ref strCatchMsg))
                            {
                                if (strCatchMsg.Trim() != string.Empty)
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteUnviewedDICOMRecords() - DeleteStudy() : Core :: Exception: " + strCatchMsg, true);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteUnviewedDICOMRecords() -DeleteStudy()  - Exception: " + ex.Message, true);
                            EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);

                        }
                        #endregion
                    }
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteUnviewedDICOMRecords()  - Error: " + strCatchMsg, true);
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteUnviewedDICOMRecords() - Exception: " + ex.Message, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);

            }
            finally
            {
                objDE = null;
                ds.Dispose();
            }
            DeleteImageFileRecords();
        }
        #endregion

        #region DeleteImageFileRecords
        private void DeleteImageFileRecords()
        {
            string strError = string.Empty;
            string strCatchMsg = string.Empty;
            string strReturnMessage = string.Empty;
            objDE = new DayEnd();
            DataSet ds = new DataSet();
            Guid StudyID = new Guid("00000000-0000-0000-0000-000000000000");
            string strFileName = string.Empty;

            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting Image File Record(s)...", false);
                if (objDE.FetchImageRecordsToDelete(strConfigPath, ref ds, ref strCatchMsg))
                {

                    foreach (DataRow dr in ds.Tables["FileRecord"].Rows)
                    {
                        StudyID = new Guid(Convert.ToString(dr["id"]));
                        strFileName = Convert.ToString(dr["file_name"]).Trim();

                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting image file : " + strFileName + " ... from " + strFTPDLFLDRTMP, false);

                        try
                        {
                            #region deleting files
                            if (File.Exists(strFTPDLFLDRTMP + "\\" + strFileName))
                            {
                                System.IO.File.Delete(strFTPDLFLDRTMP + "\\" + strFileName);
                            }
                            #endregion

                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Image file : " + strFileName + " deleted, deleting the record...", false);

                            #region Deleting record
                            objDE.STUDY_ID = StudyID;
                            if (!objDE.DeleteUngroupedImageRecord(strConfigPath, intServiceID, strSvcName, ref strReturnMessage, ref strCatchMsg))
                            {
                                if (strCatchMsg.Trim() != string.Empty)
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteImageFileRecords() - DeleteUngroupedImageRecord() : Core :: Exception: " + strCatchMsg, true);
                                }
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteImageFileRecords()  - Exception: " + ex.Message, true);
                            EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
                        }

                    }
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteImageFileRecords()  - Error: " + strCatchMsg, true);
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteImageFileRecords() - Exception: " + ex.Message, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);

            }
            finally
            {
                objDE = null;
                ds.Dispose();
            }

            //CheckListenerFiles();
        }
        #endregion

        #endregion

        #region CheckListenerFiles
        private void CheckListenerFiles()
        {
            string strCatchMsg = string.Empty;
            string[] arrFiles = new string[0];
            string[] pathElement = new string[0];
            string strFilePath = string.Empty;
            string strFileName = string.Empty;
            string strFolder = string.Empty;
            objDE = new DayEnd();

            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Checking listener files ...", false);
                arrFiles = Directory.GetFiles(strFTPDLFLDRTMP, "*_S1DXXX*.*");
                foreach (string strFile in arrFiles)
                {
                    strFilePath = strFile.Replace("\\", "/");
                    pathElement = strFilePath.Split('/');
                    strFileName = pathElement[pathElement.Length - 1];

                    if (MIMEAssistant.GetMIMEType(strFilePath) != "application/zip")
                    {
                        if (objDE.CheckListenerFile(strConfigPath, ref strCatchMsg))
                        {
                            if ((objDE.DELETE_FILE == "N") && (objDE.INSTITUTION_CODE != string.Empty))
                            {
                                strFolder = objDE.INSTITUTION_CODE + "_" + objDE.INSTITUTION_NAME + "_" + objDE.STUDY_UID;
                                strFolder = strPACSXFERDLFLDR + "\\" + strFolder;

                                if (!Directory.Exists(strFolder)) Directory.CreateDirectory(strFolder);
                                if (!File.Exists(strFolder + "\\" + strFileName)) File.Move(strFilePath, strFolder + "\\" + strFileName);
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, strFileName + " moved to " + strFolder, false);
                            }
                            else if (objDE.DELETE_FILE == "Y")
                            {
                                if (File.Exists(strFilePath)) File.Delete(strFilePath);
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, strFileName + " deleted", false);
                            }
                        }
                        else
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "CheckListenerFiles() - Core:Exception: " + strCatchMsg, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "CheckListenerFiles() - Exception: " + ex.Message, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                objDE = null;
            }
        }
        #endregion

        #region DeleteEmptyFolders
        private void DeleteEmptyFolders()
        {
            string[] arrParentFolders = Directory.GetDirectories(strDocDCMPath);
            string strParFldr = string.Empty;
            string strFileName = string.Empty;
            string strExt = string.Empty;
            string[] arrFiles = new string[0];
            string[] arrDCMFolder = new string[0]; string[] arrDCMFiles = new string[0];
            string[] arrDocFolder = new string[0]; string[] arrDocFiles = new string[0];
            string[] arrImgFolder = new string[0]; string[] arrImgFiles = new string[0];
            int intCount = 0; string strCatchMessage = string.Empty;
            objDE = new DayEnd();

            #region Delete DCM & Doc. files
            try
            {

                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting DCM & Doc. files ...", false);
                for (int i = 0; i < arrParentFolders.Length; i++)
                {
                    strParFldr = arrParentFolders[i].Substring(arrParentFolders[i].LastIndexOf("\\") + 1);

                    switch (strParFldr)
                    {
                        case "DCM":
                            arrDCMFolder = Directory.GetDirectories(arrParentFolders[i]);
                            for (int j = 0; j < arrDCMFolder.Length; j++)
                            {

                                TimeSpan t = DateTime.Now - Directory.GetLastWriteTime(arrDCMFolder[j]);
                                if (t.TotalDays >= 2)
                                {
                                    arrDCMFiles = Directory.GetFiles(arrDCMFolder[j]);

                                    for (int k = 0; k < arrDCMFiles.Length; k++)
                                    {
                                        File.Delete(arrDCMFiles[k]);
                                    }

                                    Directory.Delete(arrDCMFolder[j]);
                                }
                            }
                            break;
                        case "Docs":
                            arrDocFolder = Directory.GetDirectories(arrParentFolders[i]);
                            for (int j = 0; j < arrDocFolder.Length; j++)
                            {

                                TimeSpan t = DateTime.Now - Directory.GetLastWriteTime(arrDocFolder[j]);

                                if (t.TotalDays >= 2)
                                {
                                    arrDocFiles = Directory.GetFiles(arrDocFolder[j]);
                                    for (int k = 0; k < arrDocFiles.Length; k++)
                                    {
                                        File.Delete(arrDocFiles[k]);
                                    }

                                    Directory.Delete(arrDocFolder[j]);
                                }
                            }
                            break;
                        case "Img":
                            arrImgFolder = Directory.GetDirectories(arrParentFolders[i]);
                            for (int j = 0; j < arrImgFolder.Length; j++)
                            {

                                TimeSpan t = DateTime.Now - Directory.GetLastWriteTime(arrImgFolder[j]);
                                if (t.TotalDays >= 2)
                                {
                                    arrImgFiles = Directory.GetFiles(arrImgFolder[j]);
                                    for (int k = 0; k < arrImgFiles.Length; k++)
                                    {
                                        File.Delete(arrImgFiles[k]);
                                    }

                                    Directory.Delete(arrImgFolder[j]);
                                }
                            }
                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteFiles() - Deleting DCM & Doc. files - Exception: " + ex.Message, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
            }
            #endregion

            #region Delete .bak files
            try
            {

                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting .bak files from " + strFTPDLFLDRTMP, false);
                arrFiles = Directory.GetFiles(strFTPDLFLDRTMP);

                for (int i = 0; i < arrFiles.Length; i++)
                {
                    strFileName = arrFiles[i].Trim();
                    strExt = arrFiles[i].Trim().Substring(arrFiles[i].Length - 4, 4).ToUpper();
                    if (strExt.ToUpper() == ".BAK")
                    {
                        if (File.Exists(strFileName)) File.Delete(strFileName);
                    }

                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteFiles() - Deleting .bak files - Exception: " + ex.Message + " File : " + strFileName, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
            }
            #endregion

            #region Delete Invoice files
            try
            {
                arrFiles = new string[0];
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting invoice files from " + strMailInvFolder, false);
                arrFiles = Directory.GetFiles(strMailInvFolder);

                for (int i = 0; i < arrFiles.Length; i++)
                {
                    intCount = 0;
                    strCatchMessage = string.Empty;
                    strFileName = arrFiles[i].Trim();

                    if (objDE.CheckAttachmentFileToDelete(strConfigPath, ref intCount, ref strCatchMessage))
                    {
                        if (intCount > 0)
                        {
                            if (File.Exists(strFileName)) File.Delete(strFileName);
                        }
                    }
                    else
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "CheckAttachmentFileToDelete() --> Exception : " + strCatchMessage, true);
                    }
                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteFiles() - Deleting invoice files - Exception: " + ex.Message + " File : " + strFileName, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
            }
            #endregion

            #region Delete Temp Manual files
            try
            {
                if (Directory.Exists(strConfigPath + "\\TempManualFiles"))
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting temporary scheduler files from " + strConfigPath + "\\TempManualFiles", false);
                    arrFiles = Directory.GetFiles(strConfigPath + "\\TempManualFiles");

                    for (int i = 0; i < arrFiles.Length; i++)
                    {
                        strFileName = arrFiles[i].Trim();

                        if (File.Exists(strFileName)) File.Delete(strFileName);
                    }
                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteFiles() - Deleting Temp Manual files - Exception: " + ex.Message + " File : " + strFileName, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
            }
            #endregion

            #region Delete Temp DCM files
            try
            {
                if (Directory.Exists(strTEMPDCMATCHPATH))
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting temporary DCM files", false);
                    arrFiles = Directory.GetFiles(strTEMPDCMATCHPATH);

                    for (int i = 0; i < arrFiles.Length; i++)
                    {
                        strFileName = arrFiles[i].Trim();

                        if (File.Exists(strFileName)) File.Delete(strFileName);
                    }
                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "DeleteFiles() - Deleting Temp DCM files - Exception: " + ex.Message + " File : " + strFileName, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
            }
            #endregion

            //D:\VRSApp\VETRIS\CaseList\MSTemp
            //D:\VRSApp\VETRIS\CaseList\DocPrint\Temp
            //D:\VRSApp\VETRIS\Invoicing\DocumentPrinting\Temp

            objDE = null;
            DeleteUnviewedDICOMRecords();
        }
        #endregion

        #region ProcessLogDB
        private void ProcessLogDB()
        {

            string strCatchMsg = string.Empty;
            objDE = new DayEnd();


            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Processing Log DB...", false);
                if (objDE.ProcessLogDBDayEnd(strConfigPath, intServiceID, strSvcName, ref strCatchMsg))
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Log DB processed ", false);

                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "ProcessLogDB()  - Error: " + strCatchMsg, true);
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "ProcessLogDB() - Exception: " + ex.Message, true);
                //EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);

            }
            finally
            {
                objDE = null;
            }
        }
        #endregion
    }
}
