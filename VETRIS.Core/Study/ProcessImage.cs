using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Reflection;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using VETRIS.DAL;

namespace VETRIS.Core.Study
{
    public class ProcessImage
    {
        #region Constructor
        public ProcessImage()
        {
        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid SessionID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        string strUserName = string.Empty;
        DateTime dtStudyDate = DateTime.Today;

        Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
        string strPatientID = string.Empty;
        string strStudyUID = string.Empty;
        string strSeriesUID = string.Empty;
        string strSeriesNo = string.Empty;

        string strFilterDownloadDt = "N";
        DateTime dtDlDateFrom = DateTime.Today.AddDays(-7);
        DateTime dtDlDateTill = DateTime.Today;
        DateTime dtDlDate = DateTime.Today;
        string strPatientName = string.Empty;
        string strPatientFName = string.Empty;
        string strPatientLName = string.Empty;
        Guid InstitutionID = new Guid("00000000-0000-0000-0000-000000000000");
        string strInstitutionName = string.Empty;
        string strInstitutionCode = string.Empty;
        int intModalityID = 0;
        int intCategoryID = 0;
        int intFileCount = 0;
        int intFileXfered = 0;
        string strFTPDLFLDRTMP = string.Empty;
        string strApproved = string.Empty;
        string strXML = string.Empty;

        string strAccnNo = string.Empty;
        string strModality = string.Empty;
        string strModalityName = string.Empty;
        string strPatientGender = string.Empty;
        string strSpayedNeutered = string.Empty;
        DateTime dtPatientDob = DateTime.Today;
        string strPatientAge = string.Empty;
        decimal decPatientWt = 0;
        int intCountryID = 0;
        int intStateID = 0;
        string strCity = string.Empty;
        string strWtUOM = string.Empty;
        string strOwnerFN = string.Empty;
        string strOwnerLN = string.Empty;
        int intSpeciesID = 0;
        string strSpeciesName = string.Empty;
        Guid BreedID = new Guid("00000000-0000-0000-0000-000000000000");
        string strBreedName = string.Empty;
        Guid PhysicianID = new Guid("00000000-0000-0000-0000-000000000000");
        string strPhysicianName = string.Empty;
        int intPriorityID = 0;
        int intPIDSrl = 0;
        string strReason = string.Empty;
        string strPhysNote = string.Empty;
        string strMergeStat = "X";
        string strFileName = string.Empty;
        string strConsultApplied = "N";
        string strInstConsultAppl = "N";

        string strStdTZName = string.Empty;
        string strStdDelvTime = string.Empty;
        string strStatDelvTime = string.Empty;
        string strDelvTime = string.Empty;
        string strMsgDisp = string.Empty;
        string strSubmitPriority = "N";
        int intSenderTZOffsetMins = 0;
        string strBeyondOpHr = "N";
        string strCheckDOB = "Y";
        #endregion

        #region Properties
        public int MENU_ID
        {
            get { return intMenuID; }
            set { intMenuID = value; }
        }
        public Guid USER_SESSION_ID
        {
            get { return SessionID; }
            set { SessionID = value; }
        }
        public Guid USER_ID
        {
            get { return UserID; }
            set { UserID = value; }
        }
        public int USER_ROLE_ID
        {
            get { return intUserRoleID; }
            set { intUserRoleID = value; }
        }
        public string USER_NAME
        {
            get { return strUserName; }
            set { strUserName = value; }
        }
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public string PATIENT_ID
        {
            get { return strPatientID; }
            set { strPatientID = value; }
        }
        public string PATIENT_NAME
        {
            get { return strPatientName; }
            set { strPatientName = value; }
        }
        public string PATIENT_FIRST_NAME
        {
            get { return strPatientFName; }
            set { strPatientFName = value; }
        }
        public string PATIENT_LAST_NAME
        {
            get { return strPatientLName; }
            set { strPatientLName = value; }
        }
        public string PATIENT_GENDER
        {
            get { return strPatientGender; }
            set { strPatientGender = value; }
        }
        public string SPAYED_NEUTERED
        {
            get { return strSpayedNeutered; }
            set { strSpayedNeutered = value; }
        }
        public DateTime PATIENT_DOB
        {
            get { return dtPatientDob; }
            set { dtPatientDob = value; }
        }
        public decimal PATIENT_WEIGHT
        {
            get { return decPatientWt; }
            set { decPatientWt = value; }
        }
        public string WEIGHT_UOM
        {
            get { return strWtUOM; }
            set { strWtUOM = value; }
        }
        public string PATIENT_AGE
        {
            get { return strPatientAge; }
            set { strPatientAge = value; }
        }
        public int PATIENT_COUNTRY_ID
        {
            get { return intCountryID; }
            set { intCountryID = value; }
        }
        public int PATIENT_STATE_ID
        {
            get { return intStateID; }
            set { intStateID = value; }
        }
        public string PATIENT_CITY
        {
            get { return strCity; }
            set { strCity = value; }
        }
        public string OWNER_FIRST_NAME
        {
            get { return strOwnerFN; }
            set { strOwnerFN = value; }
        }
        public string OWNER_LAST_NAME
        {
            get { return strOwnerLN; }
            set { strOwnerLN = value; }
        }
        public int SPECIES_ID
        {
            get { return intSpeciesID; }
            set { intSpeciesID = value; }
        }
        public string SPECIES_NAME
        {
            get { return strSpeciesName; }
            set { strSpeciesName = value; }
        }
        public Guid BREED_ID
        {
            get { return BreedID; }
            set { BreedID = value; }
        }
        public string BREED_NAME
        {
            get { return strBreedName; }
            set { strBreedName = value; }
        }
        public Guid PHYSICIAN_ID
        {
            get { return PhysicianID; }
            set { PhysicianID = value; }
        }
        public string ACCESSION_NO
        {
            get { return strAccnNo; }
            set { strAccnNo = value; }
        }
        public int PRIORITY_ID
        {
            get { return intPriorityID; }
            set { intPriorityID = value; }
        }
        public string STUDY_UID
        {
            get { return strStudyUID; }
            set { strStudyUID = value; }
        }
        public string SERIES_UID
        {
            get { return strSeriesUID; }
            set { strSeriesUID = value; }
        }
        public string SERIES_NUMBER
        {
            get { return strSeriesNo; }
            set { strSeriesNo = value; }
        }
        public string FILTER_BY_STUDY_DATE
        {
            get { return strFilterDownloadDt; }
            set { strFilterDownloadDt = value; }
        }
        public DateTime STUDY_DATE_FROM
        {
            get { return dtDlDateFrom; }
            set { dtDlDateFrom = value; }
        }
        public DateTime STUDY_DATE_TILL
        {
            get { return dtDlDateTill; }
            set { dtDlDateTill = value; }
        }
        public DateTime DOWNLOAD_DATE
        {
            get { return dtDlDate; }
            set { dtDlDate = value; }
        }
        public DateTime STUDY_DATE
        {
            get { return dtStudyDate; }
            set { dtStudyDate = value; }
        }
        public Guid INSTITUTION_ID
        {
            get { return InstitutionID; }
            set { InstitutionID = value; }
        }
        public string INSTITUTION_NAME
        {
            get { return strInstitutionName; }
            set { strInstitutionName = value; }
        }
        public string INSTITUTION_CODE
        {
            get { return strInstitutionCode; }
            set { strInstitutionCode = value; }
        }
        public int PATIENT_ID_LAST_SERIAL
        {
            get { return intPIDSrl; }
            set { intPIDSrl = value; }
        }
        public int MODALITY_ID
        {
            get { return intModalityID; }
            set { intModalityID = value; }
        }
        public int CATEGORY_ID
        {
            get { return intCategoryID; }
            set { intCategoryID = value; }
        }
        public int FILE_COUNT
        {
            get { return intFileCount; }
            set { intFileCount = value; }
        }
        public int FILE_TRANSFERED
        {
            get { return intFileXfered; }
            set { intFileXfered = value; }
        }
        public string FTD_DOWNLOAD_PATH
        {
            get { return strFTPDLFLDRTMP; }
            set { strFTPDLFLDRTMP = value; }
        }
        public string REASON
        {
            get { return strReason; }
            set { strReason = value; }
        }
        public string PHYSICIAN_NOTE
        {
            get { return strPhysNote; }
            set { strPhysNote = value; }
        }
        public string APPROVED
        {
            get { return strApproved; }
            set { strApproved = value; }
        }
        public string MERGE_STATUS
        {
            get { return strMergeStat; }
            set { strMergeStat = value; }
        }
        public string FILE_NAME
        {
            get { return strFileName; }
            set { strFileName = value; }
        }
        public string CONSULT_APPLIED
        {
            get { return strConsultApplied; }
            set { strConsultApplied = value; }
        }
        public string INSTITUTION_CONSULTATION_APPLICABLE
        {
            get { return strInstConsultAppl; }
            set { strInstConsultAppl = value; }
        }
        
        public string STANDARD_TIME_ZONE_NAME
        {
            get { return strStdTZName; }
            set { strStdTZName = value; }
        }
        public string STANDARD_DELIVERY_TIME
        {
            get { return strStdDelvTime; }
            set { strStdDelvTime = value; }
        }
        public string STAT_DELIVERY_TIME
        {
            get { return strStatDelvTime; }
            set { strStatDelvTime = value; }
        }
        public string DELIVERY_TIME
        {
            get { return strDelvTime; }
            set { strDelvTime = value; }
        }
        public string SUBMIT_PRIORITY
        {
            get { return strSubmitPriority; }
            set { strSubmitPriority = value; }
        }
        public string MESSAGE_DISPLAY
        {
            get { return strMsgDisp; }
            set { strMsgDisp = value; }
        }
        public int SENDER_TIME_ZONE_OFFSET_MINUTES
        {
            get { return intSenderTZOffsetMins; }
            set { intSenderTZOffsetMins = value; }
        }
        public string BEYOND_OPERATION_HOUR
        {
            get { return strBeyondOpHr; }
            set { strBeyondOpHr = value; }
        }
        public string CHECK_DOB
        {
            get { return strCheckDOB; }
            set { strCheckDOB = value; }
        }
        #endregion

        #region Browser Methods

        #region SearchBrowser
        public bool SearchBrowser(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[9];

            SqlRecordParams[0] = new SqlParameter("@patient_id", SqlDbType.NVarChar, 200); SqlRecordParams[0].Value = strPatientID;
            SqlRecordParams[1] = new SqlParameter("@patient_name", SqlDbType.NVarChar, 200); SqlRecordParams[1].Value = strPatientName;
            SqlRecordParams[2] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = InstitutionID;
            SqlRecordParams[3] = new SqlParameter("@consider_study_date", SqlDbType.NChar, 1); SqlRecordParams[3].Value = strFilterDownloadDt;
            SqlRecordParams[4] = new SqlParameter("@study_date_from", SqlDbType.DateTime); SqlRecordParams[4].Value = dtDlDateFrom;
            SqlRecordParams[5] = new SqlParameter("@study_date_till", SqlDbType.DateTime); SqlRecordParams[5].Value = dtDlDateTill;
            SqlRecordParams[6] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[6].Value = strStudyUID;
            SqlRecordParams[7] = new SqlParameter("@approved", SqlDbType.NChar, 1); SqlRecordParams[7].Value = strApproved;
            SqlRecordParams[8] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[8].Value = UserID;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "study_rec_img_fetch_brw", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "BrowserList";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchBrowserParameters
        public bool FetchBrowserParameters(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_brw_fetch_params", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Modality";
                    ds.Tables[1].TableName = "Species";
                    ds.Tables[2].TableName = "Institutions";
                    ds.Tables[3].TableName = "InProgressStatus";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #endregion

        #region Dialog Methods

        #region LoadHeader
        public bool LoadHeader(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; string strControlCode = string.Empty;

            SqlParameter[] SqlRecordParams = new SqlParameter[6];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = intMenuID;
                SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = SessionID;
                SqlRecordParams[4] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "study_rec_img_fetch_hdr", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";
                    ds.Tables[1].TableName = "Institutions";
                    ds.Tables[2].TableName = "Modality";
                    ds.Tables[3].TableName = "FTPDownloadPath";
                    ds.Tables[4].TableName = "Species";
                    ds.Tables[5].TableName = "Institution";
                    ds.Tables[6].TableName = "Physicians";
                    ds.Tables[7].TableName = "Priority";
                    ds.Tables[8].TableName = "Consult";
                    ds.Tables[9].TableName = "Category";
                    ds.Tables[10].TableName = "Country";
                    ds.Tables[11].TableName = "State";
                    ds.Tables[12].TableName = "BeyondOpHrs";
                    ds.Tables[13].TableName = "ModalityServiceAvailable";
                    ds.Tables[14].TableName = "ModalityServiceAvailableAH";
                    ds.Tables[15].TableName = "ModalityServiceAvailableExInst";
                    ds.Tables[16].TableName = "ModalityServiceAvailableAHExInst";
                    ds.Tables[17].TableName = "SpeciesServiceAvailable";
                    ds.Tables[18].TableName = "SpeciesServiceAvailableAH";
                    ds.Tables[19].TableName = "SpeciesServiceAvailableExInst";
                    ds.Tables[20].TableName = "SpeciesServiceAvailableAHExInst";
                   

                    #region Details
                    foreach (DataRow dr in ds.Tables["Details"].Rows)
                    {
                        Id = new Guid(Convert.ToString(dr["id"]).Trim());
                        strStudyUID = Convert.ToString(dr["study_uid"]).Trim();
                        dtStudyDate = Convert.ToDateTime(dr["study_date"]);

                        strPatientID = Convert.ToString(dr["patient_id"]).Trim();
                        strPatientFName = Convert.ToString(dr["patient_fname"]).Trim();
                        strPatientLName = Convert.ToString(dr["patient_lname"]).Trim();
                        //InstitutionID = new Guid(Convert.ToString(dr["institution_id"]));
                        //strInstitutionCode = Convert.ToString(dr["institution_code"]).Trim();
                        //strInstitutionName = Convert.ToString(dr["institution_name"]).Trim();
                        intModalityID = Convert.ToInt32(dr["modality_id"]);
                        intCategoryID = Convert.ToInt32(dr["category_id"]);
                        intFileCount = Convert.ToInt32(dr["file_count"]);
                        intFileXfered = Convert.ToInt32(dr["file_xfer_count"]);
                        strSeriesUID = Convert.ToString(dr["series_instance_uid"]).Trim();
                        strSeriesNo = Convert.ToString(dr["series_no"]).Trim();
                        strApproved = Convert.ToString(dr["approve_for_pacs"]).Trim();
                        
                    }
                    #endregion

                    foreach (DataRow dr in ds.Tables["FTPDownloadPath"].Rows)
                    {
                        strFTPDLFLDRTMP = Convert.ToString(dr["FTPDLFLDRTMP"]).Trim();
                    }
                    foreach (DataRow dr in ds.Tables["Institution"].Rows)
                    {
                        InstitutionID = new Guid(Convert.ToString(dr["id"]).Trim());
                        strInstitutionCode = Convert.ToString(dr["code"]).Trim();
                        strInstitutionName = Convert.ToString(dr["name"]).Trim();
                        intPIDSrl = Convert.ToInt32(dr["patient_id_srl"]);
                    }
                    foreach (DataRow dr in ds.Tables["Consult"].Rows)
                    {
                        strInstConsultAppl = Convert.ToString(dr["institution_consult_applicable"]).Trim();
                    }
                    foreach (DataRow dr in ds.Tables["BeyondOpHrs"].Rows)
                    {
                        strBeyondOpHr = Convert.ToString(dr["beyond_operation_time"]).Trim();
                    }
                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                    ReturnMessage = Convert.ToString(SqlRecordParams[4].Value);
                }

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadFiles
        public bool LoadFiles(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[3];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
            SqlRecordParams[1] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = InstitutionID;
            SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "study_rec_img_fetch_files", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Files";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadStudyTypes
        public bool LoadStudyTypes(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[3];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
            SqlRecordParams[1] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[1].Value = intModalityID;
            SqlRecordParams[2] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = InstitutionID;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "case_list_fetch_study_types", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "StudyTypes";
                    ds.Tables[1].TableName = "TrackBy";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchSpeciesWiseBreed
        public bool FetchSpeciesWiseBreed(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@species_id", SqlDbType.Int); SqlRecordParams[0].Value = intSpeciesID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "species_wise_breed_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Breed";
                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                }

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchInstitutionWisePhysician
        public bool FetchInstitutionWisePhysician(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = InstitutionID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "institution_wise_physician_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Physicians";
                    ds.Tables[1].TableName = "Consult";
                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                }

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, FileList[] ArrObj, StudyTypeList[] ArrobjST, DocumentList[] ArrObjDocs, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            DataTable dtblST = new DataTable();
            DataTable dtblDoc = new DataTable();


            if (ValidateRecord(ArrObj, ArrobjST, ref ReturnMessage))
            {
                if ((GenerateXML(ArrObj, ref CatchMessage))
                    && (GenerateStudyTypeTable(ArrobjST, ref dtblST, ref CatchMessage))
                    && (GenerateDocumentTable(ArrObjDocs, ref dtblDoc, ref CatchMessage)))
                {
                    SqlParameter[] SqlRecordParams = new SqlParameter[46];
                    try
                    {
                        SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
                        SqlRecordParams[1] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strStudyUID;
                        SqlRecordParams[2] = new SqlParameter("@study_date", SqlDbType.DateTime); SqlRecordParams[2].Value = dtStudyDate;
                        SqlRecordParams[3] = new SqlParameter("@file_count", SqlDbType.Int); SqlRecordParams[3].Value = intFileCount;
                        SqlRecordParams[4] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = InstitutionID;
                        SqlRecordParams[5] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[5].Value = intModalityID;
                        SqlRecordParams[6] = new SqlParameter("@patient_id", SqlDbType.NVarChar, 20); SqlRecordParams[6].Value = strPatientID;
                        SqlRecordParams[7] = new SqlParameter("@patient_fname", SqlDbType.NVarChar, 80); SqlRecordParams[7].Value = strPatientFName;
                        SqlRecordParams[8] = new SqlParameter("@patient_lname", SqlDbType.NVarChar, 80); SqlRecordParams[8].Value = strPatientLName;
                        SqlRecordParams[9] = new SqlParameter("@series_instance_uid", SqlDbType.NVarChar, 100); SqlRecordParams[9].Value = strSeriesUID;
                        SqlRecordParams[10] = new SqlParameter("@series_no", SqlDbType.NVarChar, 100); SqlRecordParams[10].Value = strSeriesNo;
                        SqlRecordParams[11] = new SqlParameter("@accession_no", SqlDbType.NVarChar, 20); SqlRecordParams[11].Value = strAccnNo;
                        SqlRecordParams[12] = new SqlParameter("@reason", SqlDbType.NVarChar, 2000); SqlRecordParams[12].Value = strReason;
                        SqlRecordParams[13] = new SqlParameter("@physician_id", SqlDbType.UniqueIdentifier); SqlRecordParams[13].Value = PhysicianID;
                        SqlRecordParams[14] = new SqlParameter("@patient_dob", SqlDbType.DateTime); SqlRecordParams[14].Value = dtPatientDob;
                        SqlRecordParams[15] = new SqlParameter("@patient_age", SqlDbType.NVarChar, 50); SqlRecordParams[15].Value = strPatientAge;
                        SqlRecordParams[16] = new SqlParameter("@patient_sex", SqlDbType.NVarChar, 10); SqlRecordParams[16].Value = strPatientGender;
                        SqlRecordParams[17] = new SqlParameter("@spayed_neutered", SqlDbType.NVarChar, 100); SqlRecordParams[17].Value = strSpayedNeutered;
                        SqlRecordParams[18] = new SqlParameter("@patient_weight", SqlDbType.Decimal); SqlRecordParams[18].Value = decPatientWt;
                        SqlRecordParams[19] = new SqlParameter("@wt_uom", SqlDbType.NVarChar, 5); SqlRecordParams[19].Value = strWtUOM;
                        SqlRecordParams[20] = new SqlParameter("@owner_first_name", SqlDbType.NVarChar, 100); SqlRecordParams[20].Value = strOwnerFN;
                        SqlRecordParams[21] = new SqlParameter("@owner_last_name", SqlDbType.NVarChar, 100); SqlRecordParams[21].Value = strOwnerLN;
                        SqlRecordParams[22] = new SqlParameter("@species_id", SqlDbType.Int); SqlRecordParams[22].Value = intSpeciesID;
                        SqlRecordParams[23] = new SqlParameter("@breed_id", SqlDbType.UniqueIdentifier); SqlRecordParams[23].Value = BreedID;
                        SqlRecordParams[24] = new SqlParameter("@priority_id", SqlDbType.Int); SqlRecordParams[24].Value = intPriorityID;
                        SqlRecordParams[25] = new SqlParameter("@approve_for_pacs", SqlDbType.NChar, 1); SqlRecordParams[25].Value = strApproved;
                        SqlRecordParams[26] = new SqlParameter("@physician_note", SqlDbType.NVarChar, 2000); SqlRecordParams[26].Value = strPhysNote;
                        SqlRecordParams[27] = new SqlParameter("@consult_applied", SqlDbType.NVarChar, 2000); SqlRecordParams[27].Value = strConsultApplied;
                        SqlRecordParams[28] = new SqlParameter("@category_id", SqlDbType.Int); SqlRecordParams[28].Value = intCategoryID;
                        SqlRecordParams[29] = new SqlParameter("@xml_files", SqlDbType.NText); SqlRecordParams[29].Value = strXML;
                        SqlRecordParams[30] = new SqlParameter("@TVP_studytypes", SqlDbType.Structured); SqlRecordParams[30].Value = dtblST;
                        SqlRecordParams[31] = new SqlParameter("@TVP_docs", SqlDbType.Structured); SqlRecordParams[31].Value = dtblDoc;
                        SqlRecordParams[32] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[32].Value = UserID;
                        SqlRecordParams[33] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[33].Value = intMenuID;
                        SqlRecordParams[34] = new SqlParameter("@studies_merged", SqlDbType.NChar, 1); SqlRecordParams[34].Value = strMergeStat;
                        SqlRecordParams[35] = new SqlParameter("@sender_time_offset_mins", SqlDbType.Int); SqlRecordParams[35].Value = intSenderTZOffsetMins;
                        SqlRecordParams[36] = new SqlParameter("@submit_priority", SqlDbType.NChar,1); SqlRecordParams[36].Value = strSubmitPriority;
                        SqlRecordParams[37] = new SqlParameter("@patient_country_id", SqlDbType.Int); SqlRecordParams[37].Value = intCountryID;
                        SqlRecordParams[38] = new SqlParameter("@patient_state_id", SqlDbType.Int); SqlRecordParams[38].Value = intStateID;
                        SqlRecordParams[39] = new SqlParameter("@patient_city", SqlDbType.NVarChar, 100); SqlRecordParams[39].Value = strCity;
                        SqlRecordParams[40] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[40].Value = SessionID;
                        SqlRecordParams[41] = new SqlParameter("@delv_time", SqlDbType.NVarChar, 130); SqlRecordParams[41].Direction = ParameterDirection.Output;
                        SqlRecordParams[42] = new SqlParameter("@message_display", SqlDbType.NVarChar, 500); SqlRecordParams[42].Direction = ParameterDirection.Output;
                        SqlRecordParams[43] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[43].Direction = ParameterDirection.Output;
                        SqlRecordParams[44] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[44].Direction = ParameterDirection.Output;
                        SqlRecordParams[45] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[45].Direction = ParameterDirection.Output;

                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "study_rec_img_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[45].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        Id = new Guid(Convert.ToString(SqlRecordParams[0].Value).Trim());
                        strDelvTime = Convert.ToString(SqlRecordParams[41].Value).Trim();
                        strMsgDisp = Convert.ToString(SqlRecordParams[42].Value).Trim().Replace("\n", "<br/>"); 
                        strUserName = Convert.ToString(SqlRecordParams[43].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[44].Value).Trim();

                    }
                    catch (Exception expErr)
                    { bReturn = false; CatchMessage = expErr.Message; }
                }
                else
                    bReturn = false;

            }
            else
            {
                bReturn = false;
            }
            return bReturn;
        }
        #endregion

        #region ValidateRecord
        private bool ValidateRecord(FileList[] ArrObj, StudyTypeList[] arrObjST, ref string ReturnMessage)
        {
            bool bReturn = true;
            if (intFileCount == 0)
            {
                ReturnMessage = "180";
            }
            if (InstitutionID == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "181";
            }
            if (dtStudyDate.Year == 1900)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "178";
            }
            else
            {
                if (dtStudyDate > DateTime.Now)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage = "182";
                }
            }
            if (intModalityID == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "042";
            }
            if (intCategoryID == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "293";
            }
            if (strPatientID.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "048";
            }
            if (strPatientName.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "049";
            }
            if (dtPatientDob.Year != 1900)
            {
                if (dtPatientDob > dtStudyDate)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage = "038";
                }
                if (dtPatientDob > DateTime.Today)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage = "338";
                }
            }
            else
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "037";
            }

            if (strPatientGender.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "050";
            }
            if (strSpayedNeutered.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "051";
            }
            if (intSpeciesID == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "039";
            }
            if (BreedID == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "040";
            }
            if (decPatientWt <= 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "052";
            }
            if (strWtUOM.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "095";
            }
            if (intPriorityID == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "102";
            }
            if (strReason.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "077";
            }
            else if (strReason.Trim().Length > 2000)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "085";
            }
            if (strPhysNote.Trim().Length > 2000)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "288";
            }
            if (InstitutionID == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "055";
            }
            if (PhysicianID == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "056";
            }
            if (ArrObj.Length == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "063";
            }
            if (arrObjST.Length == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "184";
            }
            else if (arrObjST.Length > 4)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "064";
            }

            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;
            else
            {
                if (strCheckDOB == "Y")
                {

                    if (((dtStudyDate.Subtract(dtPatientDob)).Days) / (365.25 / 12) <= 1)
                        {
                            ReturnMessage += "488";
                            bReturn = false;
                        }
                  
                }
            }

            return bReturn;
        }

        #endregion

        #region GenerateXML
        private bool GenerateXML(FileList[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<file>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {


                        sbXML.Append("<row>");
                        sbXML.Append("<ungrouped_id>" + ArrObj[i].UNGROUP_ID.ToString() + "</ungrouped_id>");
                        sbXML.Append("<file_name><![CDATA[" + ArrObj[i].NAME + "]]></file_name>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    bReturn = true;
                    sbXML.Append("</file>");
                    strXML = sbXML.ToString();


                }


            }
            catch (Exception LexpErr)
            {
                bReturn = false;
                CatchMessage = LexpErr.Message;
            }
            return bReturn;
        }
        #endregion

        #region GenerateStudyTypeTable
        private bool GenerateStudyTypeTable(StudyTypeList[] ArrObj, ref DataTable dtblST, ref string CatchMessage)
        {
            bool bReturn = false;

            dtblST.Columns.Add("study_type_id", System.Type.GetType("System.Guid"));
            dtblST.Columns.Add("srl_no", System.Type.GetType("System.Int32"));


            try
            {
                if (ArrObj.Length > 0)
                {
                    for (int i = 0; i < ArrObj.Length; i++)
                    {
                        DataRow dr = dtblST.NewRow();
                        dr["study_type_id"] = ArrObj[i].ID;
                        dr["srl_no"] = ArrObj[i].SERIAL_NUMBER;
                        dtblST.Rows.Add(dr);
                    }

                }
                bReturn = true;
            }
            catch (Exception LexpErr)
            {
                bReturn = false;
                CatchMessage = LexpErr.Message;
            }
            return bReturn;
        }
        #endregion

        #region GenerateDocumentTable
        private bool GenerateDocumentTable(DocumentList[] ArrObj, ref DataTable dtblDoc, ref string CatchMessage)
        {
            bool bReturn = false;

            dtblDoc.Columns.Add("document_id", System.Type.GetType("System.Guid"));
            dtblDoc.Columns.Add("document_name", System.Type.GetType("System.String"));
            dtblDoc.Columns.Add("document_srl_no", System.Type.GetType("System.Int32"));
            dtblDoc.Columns.Add("document_link", System.Type.GetType("System.String"));
            dtblDoc.Columns.Add("document_file_type", System.Type.GetType("System.String"));
            dtblDoc.Columns.Add("document_file", System.Type.GetType("System.Byte[]"));


            try
            {
                if (ArrObj.Length > 0)
                {
                    for (int i = 0; i < ArrObj.Length; i++)
                    {
                        DataRow dr = dtblDoc.NewRow();
                        dr["document_id"] = ArrObj[i].ID;
                        dr["document_name"] = ArrObj[i].NAME;
                        dr["document_srl_no"] = ArrObj[i].SERIAL_NUMBER;
                        dr["document_link"] = ArrObj[i].FILE_NAME;
                        dr["document_file_type"] = ArrObj[i].FILE_TYPE;
                        dr["document_file"] = ArrObj[i].FILE_CONTENT;
                        dtblDoc.Rows.Add(dr);
                    }

                }
                bReturn = true;
            }
            catch (Exception LexpErr)
            {
                bReturn = false;
                CatchMessage = LexpErr.Message;
            }
            return bReturn;
        }
        #endregion

        #region DeleteFile
        public bool DeleteFile(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[6];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserID;
                SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = intMenuID;
                SqlRecordParams[3] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "study_rec_img_file_delete", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[5].Value);

                if (intReturnValue == 1)
                {
                    bReturn = true;
                    try
                    {
                        if (Directory.Exists(strFTPDLFLDRTMP))
                        {
                            if (File.Exists(strFTPDLFLDRTMP + "/" + strFileName))
                                File.Delete(strFTPDLFLDRTMP + "/" + strFileName);
                        }
                    }
                    catch (Exception ex) { ;}
                }
                else
                    bReturn = false;

                strUserName = Convert.ToString(SqlRecordParams[3].Value).Trim();
                ReturnMessage = Convert.ToString(SqlRecordParams[4].Value).Trim();

                

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }



            return bReturn;
        }
        #endregion

        #region GetServiceAvailabilityMessage
        public bool GetServiceAvailabilityMessage(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[6];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@species_id", SqlDbType.Int); SqlRecordParams[0].Value = intSpeciesID;
                SqlRecordParams[1] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[1].Value = intModalityID;
                SqlRecordParams[2] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = InstitutionID;
                SqlRecordParams[3] = new SqlParameter("@priority_id", SqlDbType.Int); SqlRecordParams[3].Value = intPriorityID;
                SqlRecordParams[4] = new SqlParameter("@error_code", SqlDbType.VarChar, 500); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_service_availability_message_get", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[5].Value);

                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;

                ReturnMessage = Convert.ToString(SqlRecordParams[4].Value);

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #endregion
    }

    public class FileList
    {
        #region Constructor
        public FileList()
        {
        }
        #endregion

        #region Variables
        Guid UngroupId = Guid.Empty;
        string strName = string.Empty;
        #endregion

        #region Properties
        public Guid UNGROUP_ID
        {
            get { return UngroupId; }
            set { UngroupId = value; }
        }
        public string NAME
        {
            get { return strName; }
            set { strName = value; }
        }
        #endregion
    }
    public class StudyTypeList
    {
        #region Constructor
        public StudyTypeList()
        {
        }
        #endregion

        #region Variables
        Guid StudyTypeID = Guid.Empty;
        int intSrlNo = 0;
        #endregion

        #region Properties
        public Guid ID
        {
            get { return StudyTypeID; }
            set { StudyTypeID = value; }
        }
        public int SERIAL_NUMBER
        {
            get { return intSrlNo; }
            set { intSrlNo = value; }
        }
        #endregion
    }
    public class DocumentList
    {
        #region Constructor
        public DocumentList()
        {
        }
        #endregion

        #region Variables
        Guid DocumentID = Guid.Empty;
        string strDocumentName = string.Empty;
        string strFileName = string.Empty;
        string strFileType = string.Empty;
        int intSrlNo = 0;
        Byte[] btFile = new Byte[0];
        #endregion

        #region Properties
        public Guid ID
        {
            get { return DocumentID; }
            set { DocumentID = value; }
        }
        public string NAME
        {
            get { return strDocumentName; }
            set { strDocumentName = value; }
        }

        public int SERIAL_NUMBER
        {
            get { return intSrlNo; }
            set { intSrlNo = value; }
        }
        public string FILE_NAME
        {
            get { return strFileName; }
            set { strFileName = value; }
        }
        public string FILE_TYPE
        {
            get { return strFileType; }
            set { strFileType = value; }
        }
        public Byte[] FILE_CONTENT
        {
            get { return btFile; }
            set { btFile = value; }
        }
        #endregion
    }
}
