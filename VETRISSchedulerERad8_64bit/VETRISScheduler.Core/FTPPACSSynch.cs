using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using VETRISScheduler.DAL;

namespace VETRISScheduler.Core
{
    public class FTPPACSSynch
    {
        #region Constructor
        public FTPPACSSynch()
        {
        }
        #endregion

        #region Variables
        Guid Id = new Guid();
        string strSUID = string.Empty;
        string strSeriesUID = string.Empty;
        string strSOPInstanceUID = string.Empty;
        DateTime dtStudy = DateTime.Now;
        Guid InstID = new Guid();
        string strInsCode = string.Empty;
        string strInstName = string.Empty;
        string strPatientID = string.Empty;
        string strPatientFname = string.Empty;
        string strPatientLname = string.Empty;
        string strFileName = string.Empty;
        string strDCMFileName = string.Empty;
        int intFileCount = 0;
        string strAccnNo = string.Empty;
        string strRefPhys = string.Empty;
        string strManufacturer = string.Empty;
        string strStationName = string.Empty;
        string strModel = string.Empty;
        string strModalityAETitle = string.Empty;
        string strReason = string.Empty;
        DateTime dtDOB = DateTime.Today;
        string strPatientSex = string.Empty;
        string strPatientAge = string.Empty;
        string strModality = string.Empty;
        int intPriorityID = 0;
        string strFailureReason = string.Empty;
        string strImpSessID = string.Empty;
        string strFileType = string.Empty;
        string strIsManual = "N";
        string strIsRcvdLF = "N";
        string strStudyExists = "N";
        string strSyncViaFD = "N";
        #endregion

        #region Properties
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public string STUDY_UID
        {
            get { return strSUID; }
            set { strSUID = value; }
        }
        public string SERIES_UID
        {
            get { return strSeriesUID; }
            set { strSeriesUID = value; }
        }
        public string SOP_INSTANCE_UID
        {
            get { return strSOPInstanceUID; }
            set { strSOPInstanceUID = value; }
        }
        public DateTime STUDY_DATE
        {
            get { return dtStudy; }
            set { dtStudy = value; }
        }
        public Guid INSTITUTION_ID
        {
            get { return InstID; }
            set { InstID = value; }
        }
        public string INSTITUTION_CODE
        {
            get { return strInsCode; }
            set { strInsCode = value; }
        }
        public string INSTITUTION_NAME
        {
            get { return strInstName; }
            set { strInstName = value; }
        }
        public string PATIENT_ID
        {
            get { return strPatientID; }
            set { strPatientID = value; }
        }
        public string PATIENT_FIRST_NAME
        {
            get { return strPatientFname; }
            set { strPatientFname = value; }
        }
        public string PATIENT_LAST_NAME
        {
            get { return strPatientLname; }
            set { strPatientLname = value; }
        }
        public string FILE_NAME
        {
            get { return strFileName; }
            set { strFileName = value; }
        }
        public string DICOM_FILE_NAME
        {
            get { return strDCMFileName; }
            set { strDCMFileName = value; }
        }
        public string FILE_TYPE
        {
            get { return strFileType; }
            set { strFileType = value; }
        }
        public int FILE_COUNT
        {
            get { return intFileCount; }
            set { intFileCount = value; }
        }
        public string ACCESSION_NUMBER
        {
            get { return strAccnNo; }
            set { strAccnNo = value; }
        }
        public string MODALITY
        {
            get { return strModality; }
            set { strModality = value; }
        }
        public string REFERRING_PHYSICIAN
        {
            get { return strRefPhys; }
            set { strRefPhys = value; }
        }
        public string MANUFACTURER
        {
            get { return strManufacturer; }
            set { strManufacturer = value; }
        }
        public string STATION_NAME
        {
            get { return strStationName; }
            set { strStationName = value; }
        }
        public string MODEL
        {
            get { return strModel; }
            set { strModel = value; }
        }
        public string MODALITY_AE_TITLE
        {
            get { return strModalityAETitle; }
            set { strModalityAETitle = value; }
        }
        public string REASON
        {
            get { return strReason; }
            set { strReason = value; }
        }
        public DateTime DATE_OF_BIRTH
        {
            get { return dtDOB; }
            set { dtDOB = value; }
        }
        public string PATIENT_SEX
        {
            get { return strPatientSex; }
            set { strPatientSex = value; }
        }
        public string PATIENT_AGE
        {
            get { return strPatientAge; }
            set { strPatientAge = value; }
        }
        public int PRIORITY_ID
        {
            get { return intPriorityID; }
            set { intPriorityID = value; }
        }
        public string FAILURE_REASON
        {
            get { return strFailureReason; }
            set { strFailureReason = value; }
        }
        public string IMPORT_SESSION_ID
        {
            get { return strImpSessID; }
            set { strImpSessID = value; }
        }
        public string IS_MANUAL_UPLOAD
        {
            get { return strIsManual; }
            set { strIsManual = value; }
        }
        public string RECEIVED_BY_LISTENER
        {
            get { return strIsRcvdLF; }
            set { strIsRcvdLF = value; }
        }
        public string STUDY_EXISTS
        {
            get { return strStudyExists; }
            set { strStudyExists = value; }
        }
        public string SYNCED_BY_FILE_DISTRIBUTION_SERVICE
        {
            get { return strSyncViaFD; }
            set { strSyncViaFD = value; }
        }
        #endregion

        #region SaveData
        public bool SaveData(string ConfigPath, string strSvcName, ref string ReturnMessage, ref string CatchMessage, ref string DelFile)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[28];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                SqlRecordParams[0] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strSUID;
                SqlRecordParams[1] = new SqlParameter("@study_date", SqlDbType.DateTime); SqlRecordParams[1].Value = dtStudy;
                SqlRecordParams[2] = new SqlParameter("@institution_code", SqlDbType.NVarChar, 100); SqlRecordParams[2].Value = strInsCode;
                SqlRecordParams[3] = new SqlParameter("@institution_name", SqlDbType.NVarChar, 100); SqlRecordParams[3].Value = strInstName;
                SqlRecordParams[4] = new SqlParameter("@patient_id", SqlDbType.NVarChar, 20); SqlRecordParams[4].Value = strPatientID;
                SqlRecordParams[5] = new SqlParameter("@patient_fname", SqlDbType.NVarChar, 80); SqlRecordParams[5].Value = strPatientFname;
                SqlRecordParams[6] = new SqlParameter("@patient_lname", SqlDbType.NVarChar, 80); SqlRecordParams[6].Value = strPatientLname;
                SqlRecordParams[7] = new SqlParameter("@file_name", SqlDbType.NVarChar, 250); SqlRecordParams[7].Value = strFileName;
                SqlRecordParams[8] = new SqlParameter("@accession_no", SqlDbType.NVarChar, 20); SqlRecordParams[8].Value = strAccnNo;
                SqlRecordParams[9] = new SqlParameter("@reason", SqlDbType.NVarChar, 500); SqlRecordParams[9].Value = strReason;
                SqlRecordParams[10] = new SqlParameter("@modality", SqlDbType.NVarChar, 50); SqlRecordParams[10].Value = strModality;
                SqlRecordParams[11] = new SqlParameter("@manufacturer_name", SqlDbType.NVarChar, 100); SqlRecordParams[11].Value = strManufacturer;
                SqlRecordParams[12] = new SqlParameter("@device_serial_no", SqlDbType.NVarChar, 20); SqlRecordParams[12].Value = strStationName;
                SqlRecordParams[13] = new SqlParameter("@manufacturer_model_no", SqlDbType.NVarChar, 50); SqlRecordParams[13].Value = strModel;
                SqlRecordParams[14] = new SqlParameter("@modality_ae_title", SqlDbType.NVarChar, 500); SqlRecordParams[14].Value = strModalityAETitle;
                SqlRecordParams[15] = new SqlParameter("@referring_physician", SqlDbType.NVarChar, 200); SqlRecordParams[15].Value = strRefPhys;
                SqlRecordParams[16] = new SqlParameter("@patient_sex", SqlDbType.NVarChar, 10); SqlRecordParams[16].Value = strPatientSex;
                SqlRecordParams[17] = new SqlParameter("@patient_dob", SqlDbType.DateTime); SqlRecordParams[17].Value = dtDOB;
                SqlRecordParams[18] = new SqlParameter("@patient_age", SqlDbType.NVarChar, 50); SqlRecordParams[18].Value = strPatientAge;
                SqlRecordParams[19] = new SqlParameter("@priority_id", SqlDbType.Int); SqlRecordParams[19].Value = intPriorityID;
                SqlRecordParams[20] = new SqlParameter("@import_session_id", SqlDbType.NVarChar, 30); SqlRecordParams[20].Value = strImpSessID;
                SqlRecordParams[21] = new SqlParameter("@is_manual", SqlDbType.NChar, 1); SqlRecordParams[21].Value = strIsManual;
                SqlRecordParams[22] = new SqlParameter("@is_listener_file", SqlDbType.NChar, 1); SqlRecordParams[22].Value = strIsRcvdLF;
                SqlRecordParams[23] = new SqlParameter("@series_uid", SqlDbType.NVarChar, 100); SqlRecordParams[23].Value = strSeriesUID;
                SqlRecordParams[24] = new SqlParameter("@sop_instance_uid", SqlDbType.NVarChar, 100); SqlRecordParams[24].Value = strSOPInstanceUID;
                SqlRecordParams[25] = new SqlParameter("@delete_file", SqlDbType.NChar, 1); SqlRecordParams[25].Direction = ParameterDirection.Output;
                SqlRecordParams[26] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[26].Direction = ParameterDirection.Output;
                SqlRecordParams[27] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[27].Direction = ParameterDirection.Output;

                #region debug log
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of file " + strFileName, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " Study UID : " + strSUID, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " Series UID : " + strSeriesUID, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " Instance # : " + strSOPInstanceUID, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " Study Date : " + dtStudy.ToString(), false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " Inst Code : " + strInsCode, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " Inst Name : " + strInstName, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " Patient ID : " + strPatientID, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " P FNAME : " + strPatientFname, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " P LNAME : " + strPatientLname, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " Accn No : " + strAccnNo, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " Modality : " + strModality, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " Ref Phys : " + strRefPhys, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " Manufacturer : " + strManufacturer, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " Model : " + strModel, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " Modality AE Title : " + strModalityAETitle, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " Reason : " + strReason, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " DOB : " + dtDOB.ToString(), false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " P Sex : " + strPatientSex, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " P Age : " + strPatientAge, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " Priotity ID : " + intPriorityID.ToString(), false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " Session ID : " + strImpSessID, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " Is Manual : " + strIsManual, false);
                //CoreCommon.doLog(ConfigPath, 7, strSvcName, "Saving info of File " + strFileName + " Received By Listener : " + strIsRcvdLF, false);
                #endregion

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_downloaded_file_details_save", SqlRecordParams);

                intReturnType = Convert.ToInt32(SqlRecordParams[27].Value);
                if (intReturnType == 0)
                {
                    ReturnMessage = Convert.ToString(SqlRecordParams[26].Value);
                    bReturn = false;
                    DelFile = Convert.ToString(SqlRecordParams[25].Value);
                    CoreCommon.doLog(ConfigPath, 7, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords()=>File : " + strFileName + " SaveData()::FAILED-" + ReturnMessage, true);
                }
                else if (intReturnType == 2)
                {
                    ReturnMessage = Convert.ToString(SqlRecordParams[26].Value);
                    bReturn = false;
                    DelFile = "Y";
                    CoreCommon.doLog(ConfigPath, 7, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords()=>File : " + strFileName + " SaveData()::FILE TO BE DELETED", false);
                }
                else
                {
                    bReturn = true;
                    CoreCommon.doLog(ConfigPath, 7, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords()=>doCompressFiles()=>UpdateDownloadedFilesRecords()=>File : " + strFileName + " SaveData()::SUCCESS", false);
                }
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region SaveListenerFileData
        public bool SaveListenerFileData(string ConfigPath, string strSvcName, ref string ReturnMessage, ref string CatchMessage,ref string DelFile)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[27];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                SqlRecordParams[0] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strSUID;
                SqlRecordParams[1] = new SqlParameter("@study_date", SqlDbType.DateTime); SqlRecordParams[1].Value = dtStudy;
                SqlRecordParams[2] = new SqlParameter("@institution_code", SqlDbType.NVarChar, 100); SqlRecordParams[2].Value = strInsCode;
                SqlRecordParams[3] = new SqlParameter("@institution_name", SqlDbType.NVarChar, 100); SqlRecordParams[3].Value = strInstName;
                SqlRecordParams[4] = new SqlParameter("@patient_id", SqlDbType.NVarChar, 20); SqlRecordParams[4].Value = strPatientID;
                SqlRecordParams[5] = new SqlParameter("@patient_fname", SqlDbType.NVarChar, 80); SqlRecordParams[5].Value = strPatientFname;
                SqlRecordParams[6] = new SqlParameter("@patient_lname", SqlDbType.NVarChar, 80); SqlRecordParams[6].Value = strPatientLname;
                SqlRecordParams[7] = new SqlParameter("@file_name", SqlDbType.NVarChar, 250); SqlRecordParams[7].Value = strFileName;
                SqlRecordParams[8] = new SqlParameter("@accession_no", SqlDbType.NVarChar, 20); SqlRecordParams[8].Value = strAccnNo;
                SqlRecordParams[9] = new SqlParameter("@reason", SqlDbType.NVarChar, 500); SqlRecordParams[9].Value = strReason;
                SqlRecordParams[10] = new SqlParameter("@modality", SqlDbType.NVarChar, 50); SqlRecordParams[10].Value = strModality;
                SqlRecordParams[11] = new SqlParameter("@manufacturer_name", SqlDbType.NVarChar, 100); SqlRecordParams[11].Value = strManufacturer;
                SqlRecordParams[12] = new SqlParameter("@device_serial_no", SqlDbType.NVarChar, 20); SqlRecordParams[12].Value = strStationName;
                SqlRecordParams[13] = new SqlParameter("@manufacturer_model_no", SqlDbType.NVarChar, 50); SqlRecordParams[13].Value = strModel;
                SqlRecordParams[14] = new SqlParameter("@modality_ae_title", SqlDbType.NVarChar, 500); SqlRecordParams[14].Value = strModalityAETitle;
                SqlRecordParams[15] = new SqlParameter("@referring_physician", SqlDbType.NVarChar, 200); SqlRecordParams[15].Value = strRefPhys;
                SqlRecordParams[16] = new SqlParameter("@patient_sex", SqlDbType.NVarChar, 10); SqlRecordParams[16].Value = strPatientSex;
                SqlRecordParams[17] = new SqlParameter("@patient_dob", SqlDbType.DateTime); SqlRecordParams[17].Value = dtDOB;
                SqlRecordParams[18] = new SqlParameter("@patient_age", SqlDbType.NVarChar, 50); SqlRecordParams[18].Value = strPatientAge;
                SqlRecordParams[19] = new SqlParameter("@priority_id", SqlDbType.Int); SqlRecordParams[19].Value = intPriorityID;
                SqlRecordParams[20] = new SqlParameter("@import_session_id", SqlDbType.NVarChar, 30); SqlRecordParams[20].Value = strImpSessID;
                SqlRecordParams[21] = new SqlParameter("@is_manual", SqlDbType.NChar, 1); SqlRecordParams[21].Value = strIsManual;
                SqlRecordParams[22] = new SqlParameter("@series_uid", SqlDbType.NVarChar, 100); SqlRecordParams[22].Value = strSeriesUID;
                SqlRecordParams[23] = new SqlParameter("@sop_instance_uid", SqlDbType.NVarChar, 100); SqlRecordParams[23].Value = strSOPInstanceUID;
                SqlRecordParams[24] = new SqlParameter("@delete_file", SqlDbType.NChar, 1); SqlRecordParams[24].Direction = ParameterDirection.Output;
                SqlRecordParams[25] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[25].Direction = ParameterDirection.Output;
                SqlRecordParams[26] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[26].Direction = ParameterDirection.Output;

                if(strSyncViaFD=="N") intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_downloaded_listener_file_details_save", SqlRecordParams);
                else if (strSyncViaFD == "Y") intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_downloaded_distributor_file_details_save", SqlRecordParams);

                intReturnType = Convert.ToInt32(SqlRecordParams[26].Value);
                DelFile = Convert.ToString(SqlRecordParams[24].Value);

                if (intReturnType == 0)
                {
                    ReturnMessage = Convert.ToString(SqlRecordParams[25].Value);
                    bReturn = false;
                    
                }
                else if (intReturnType == 2)
                {
                    ReturnMessage = Convert.ToString(SqlRecordParams[25].Value);
                    bReturn = false;
                }
                else
                    bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region SaveImageDownloadInfo
        public bool SaveImageDownloadInfo(string ConfigPath, string strSvcName, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[5];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);


                SqlRecordParams[0] = new SqlParameter("@institution_code", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strInsCode;
                SqlRecordParams[1] = new SqlParameter("@file_name", SqlDbType.NVarChar, 250); SqlRecordParams[1].Value = strFileName;
                SqlRecordParams[2] = new SqlParameter("@import_session_id", SqlDbType.NVarChar, 250); SqlRecordParams[2].Value = strImpSessID;
                SqlRecordParams[3] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_downloaded_img_file_ungrouped_save", SqlRecordParams);

                intReturnType = Convert.ToInt32(SqlRecordParams[4].Value);
                if (intReturnType == 0)
                {
                    ReturnMessage = Convert.ToString(SqlRecordParams[3].Value);
                    bReturn = false;
                }
                else
                    bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region FetchApprovedFilesToTransfer
        public bool FetchApprovedFilesToTransfer(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_approved_files_to_transfer_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "FileList";
                }
                bReturn = true;

            }
            catch (Exception expErr)
            {
                bReturn = false; CatchMessage = expErr.Message;
            }

            return bReturn;
        }
        #endregion

        #region FetchTagsToFormat
        public bool FetchTagsToFormat(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                SqlRecordParams[0] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = InstID;
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_tags_to_format_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "TagList";
                }
                bReturn = true;

            }
            catch (Exception expErr)
            {
                bReturn = false; CatchMessage = expErr.Message;
            }

            return bReturn;
        }
        #endregion

        #region FetchApprovedFilesToDicomise
        public bool FetchApprovedFilesToDicomise(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_approved_grouped_img_files_to_dicomise_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "FileList";
                }
                bReturn = true;

            }
            catch (Exception expErr)
            {
                bReturn = false; CatchMessage = expErr.Message;
            }

            return bReturn;
        }
        #endregion

        #region UpdateTransferToPACSFileCount
        public bool UpdateTransferToPACSFileCount(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@file_name", SqlDbType.VarChar, 250); SqlRecordParams[1].Value = strFileName;
                SqlRecordParams[2] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[2].Direction = ParameterDirection.Output;
                SqlRecordParams[3] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_transfered_file_count_update", SqlRecordParams);

                intReturnType = Convert.ToInt32(SqlRecordParams[3].Value);
                if (intReturnType == 0)
                {
                    ReturnMessage = Convert.ToString(SqlRecordParams[2].Value);
                    bReturn = false;
                }
                else
                    bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region UpdateImageFileTransferToPACSFileCount
        public bool UpdateImageFileTransferToPACSFileCount(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@file_name", SqlDbType.VarChar, 250); SqlRecordParams[1].Value = strFileName;
                SqlRecordParams[2] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[2].Direction = ParameterDirection.Output;
                SqlRecordParams[3] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_transfered_img_file_count_update", SqlRecordParams);

                intReturnType = Convert.ToInt32(SqlRecordParams[3].Value);
                if (intReturnType == 0)
                {
                    ReturnMessage = Convert.ToString(SqlRecordParams[2].Value);
                    bReturn = false;
                }
                else
                    bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region UpdateImageFileDicomDetails
        public bool UpdateImageFileDicomDetails(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[5];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@file_name", SqlDbType.VarChar, 250); SqlRecordParams[1].Value = strFileName;
                SqlRecordParams[2] = new SqlParameter("@dcm_file_name", SqlDbType.VarChar, 250); SqlRecordParams[2].Value = strDCMFileName;
                SqlRecordParams[3] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_img_file_dcm_details_update", SqlRecordParams);

                intReturnType = Convert.ToInt32(SqlRecordParams[4].Value);
                if (intReturnType == 0)
                {
                    ReturnMessage = Convert.ToString(SqlRecordParams[3].Value);
                    bReturn = false;
                }
                else
                    bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region SaveTransferedFileCount
        public bool SaveTransferedFileCount(string ConfigPath, string strSvcName, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[5];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);


                SqlRecordParams[0] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strSUID;
                SqlRecordParams[1] = new SqlParameter("@institution_code", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strInsCode;
                SqlRecordParams[2] = new SqlParameter("@file_xfer_count", SqlDbType.Int); SqlRecordParams[2].Value = intFileCount;
                SqlRecordParams[3] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_downloaded_file_xfer_file_count_save", SqlRecordParams);

                intReturnType = Convert.ToInt32(SqlRecordParams[4].Value);
                if (intReturnType == 0)
                {
                    ReturnMessage = Convert.ToString(SqlRecordParams[3].Value);
                    //CoreCommon.doLog(ConfigPath, 1, strSvcName, "SaveTransferedFileCount() - Error: " + ReturnMessage, true);
                    bReturn = false;
                }
                else
                    bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region FetchWriteBackList (Suspended)
        //public bool FetchWriteBackList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        //{
        //    bool bReturn = false;


        //    try
        //    {
        //        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
        //        ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_downloaded_files_write_back_records_fetch");
        //        if (ds.Tables.Count > 0)
        //        {
        //            ds.Tables[0].TableName = "Details";
        //        }
        //        bReturn = true;

        //    }
        //    catch (Exception expErr)
        //    {
        //        bReturn = false; CatchMessage = expErr.Message;
        //    }

        //    return bReturn;
        //}
        #endregion

        #region UpdateWriteBackStatus
        public bool UpdateWriteBackStatus(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);


                SqlRecordParams[0] = new SqlParameter("@study_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strSUID;
                SqlRecordParams[2] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[2].Direction = ParameterDirection.Output;
                SqlRecordParams[3] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_downloaded_files_write_back_update", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[3].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[2].Value).Trim();

                if (intReturnType == 0)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "Core:UpdateWriteBackStatus() - Error: " + strReturnMessage, true);
                }
                else if (intReturnType == 1)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "Core:UpdateWriteBackStatus() - " + strReturnMessage, false);
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "-" + strSUID; }

            return bReturn;
        }
        #endregion

        #region SaveDicomRouterLog
        public bool SaveDicomRouterLog(string ConfigPath, int intServiceID, string strSvcName, DataTable dtbl, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[8];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                foreach (DataRow dr in dtbl.Rows)
                {
                    try
                    {

                        SqlRecordParams[0] = new SqlParameter("@institution_code", SqlDbType.NVarChar, 5); SqlRecordParams[0].Value = Convert.ToString(dr["institution_code"]).Trim();
                        SqlRecordParams[1] = new SqlParameter("@service_id", SqlDbType.Int); SqlRecordParams[1].Value = Convert.ToInt32(dr["service_id"]);
                        SqlRecordParams[2] = new SqlParameter("@service_name", SqlDbType.NVarChar, 100); SqlRecordParams[2].Value = Convert.ToString(dr["service_name"]).Trim();
                        SqlRecordParams[3] = new SqlParameter("@log_date", SqlDbType.DateTime); SqlRecordParams[3].Value = Convert.ToDateTime(dr["log_date"]);
                        SqlRecordParams[4] = new SqlParameter("@log_message", SqlDbType.VarChar, 8000); SqlRecordParams[4].Value = Convert.ToString(dr["log_message"]).Trim();
                        SqlRecordParams[5] = new SqlParameter("@is_error", SqlDbType.Bit); if (Convert.ToString(dr["is_error"]).Trim() == "Y") SqlRecordParams[5].Value = true; else SqlRecordParams[5].Value = false;
                        SqlRecordParams[6] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[6].Direction = ParameterDirection.Output;
                        SqlRecordParams[7] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[7].Direction = ParameterDirection.Output;

                        intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_dicom_router_log_save", SqlRecordParams);
                        intReturnType = Convert.ToInt32(SqlRecordParams[7].Value);
                        if (intReturnType == 0)
                        {
                            ReturnMessage = Convert.ToString(SqlRecordParams[6].Value);
                            CoreCommon.doLog(ConfigPath, intServiceID, strSvcName, "Core : SaveDicomRouterLog() - Error: " + ReturnMessage, true);
                        }

                        bReturn = true;
                    }
                    catch (Exception ex)
                    {
                        CoreCommon.doLog(ConfigPath, intServiceID, strSvcName, "Core : SaveDicomRouterLog() - Exception: " + ex.Message.Trim(), true);
                    }

                }

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region CreateFileXferFailureNotification
        public bool CreateFileXferFailureNotification(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[7];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                SqlRecordParams[0] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strSUID;
                SqlRecordParams[1] = new SqlParameter("@institution_code", SqlDbType.NVarChar, 5); SqlRecordParams[1].Value = strInsCode;
                SqlRecordParams[2] = new SqlParameter("@institution_name", SqlDbType.NVarChar, 100); SqlRecordParams[2].Value = strInstName;
                SqlRecordParams[3] = new SqlParameter("@file_name", SqlDbType.NVarChar, 100); SqlRecordParams[3].Value = strFileName;
                SqlRecordParams[4] = new SqlParameter("@failure_reason", SqlDbType.NVarChar, 4000); SqlRecordParams[4].Value = strFailureReason;
                SqlRecordParams[5] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_file_xfer_fail_notification_create", SqlRecordParams);

                intReturnType = Convert.ToInt32(SqlRecordParams[6].Value);
                if (intReturnType == 0)
                {
                    ReturnMessage = Convert.ToString(SqlRecordParams[5].Value);
                    bReturn = false;
                }
                else
                    bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region CheckMissingSessionFiles
        public bool CheckMissingSessionFiles(string ConfigPath, ref string CheckStatus,ref string StudyUID,ref string InstitutionName,ref string InstitutionCode,ref string SentToPacs, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[8];
            int intExecReturn = 0;

            try
            {
                SqlRecordParams[0] = new SqlParameter("@session_id", SqlDbType.NVarChar, 20); SqlRecordParams[0].Value = strImpSessID;
                SqlRecordParams[1] = new SqlParameter("@file_name", SqlDbType.NVarChar, 250); SqlRecordParams[1].Value = strFileName;
                SqlRecordParams[2] = new SqlParameter("@file_type", SqlDbType.NChar, 1); SqlRecordParams[2].Value = strFileType;
                SqlRecordParams[3] = new SqlParameter("@is_missing", SqlDbType.NChar, 1); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@institution_code", SqlDbType.NVarChar,5); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@institution_name", SqlDbType.NVarChar, 100); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[6].Direction = ParameterDirection.Output;
                SqlRecordParams[7] = new SqlParameter("@sent_to_pacs", SqlDbType.NChar, 1); SqlRecordParams[7].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_session_file_missing_check", SqlRecordParams);
                CheckStatus = Convert.ToString(SqlRecordParams[3].Value);
                InstitutionCode = Convert.ToString(SqlRecordParams[4].Value).Trim();
                InstitutionName = Convert.ToString(SqlRecordParams[5].Value).Trim();
                StudyUID = Convert.ToString(SqlRecordParams[6].Value).Trim();
                SentToPacs = Convert.ToString(SqlRecordParams[7].Value);
                bReturn = true;

            }
            catch (Exception expErr)
            {
                bReturn = false; CatchMessage = expErr.Message;
            }

            return bReturn;
        }
        #endregion

        #region FetchManuallySubmittedFilesToUpload
        public bool FetchManuallySubmittedFilesToUpload(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_manual_submission_files_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "FileList";
                }
                bReturn = true;

            }
            catch (Exception expErr)
            {
                bReturn = false; CatchMessage = expErr.Message;
            }

            return bReturn;
        }
        #endregion

        #region DeleteManualFileEntry
        public bool DeleteManualFileEntry(string ConfigPath, int ServiceID, string strSvcName, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[3];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);


                SqlRecordParams[0] = new SqlParameter("@file_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[1].Direction = ParameterDirection.Output;
                SqlRecordParams[2] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[2].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_manual_submission_file_delete", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[2].Value);
                ReturnMessage = Convert.ToString(SqlRecordParams[1].Value).Trim();

                if (intReturnType == 0)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "DeleteManualFileEntry() - Error: " + ReturnMessage, true);
                }

                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "-" + Convert.ToString(Id); }

            return bReturn;
        }
        #endregion

        #region UpdateArchivedFileCount
        public bool UpdateArchivedFileCount(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[3];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                SqlRecordParams[0] = new SqlParameter("@study_uid", SqlDbType.NVarChar,100); SqlRecordParams[0].Value = strSUID;
                SqlRecordParams[1] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[1].Direction = ParameterDirection.Output;
                SqlRecordParams[2] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[2].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_archive_file_count_update", SqlRecordParams);

                intReturnType = Convert.ToInt32(SqlRecordParams[2].Value);
                if (intReturnType == 0)
                {
                    ReturnMessage = Convert.ToString(SqlRecordParams[1].Value);
                    bReturn = false;
                }
                else
                    bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion
    }
}
