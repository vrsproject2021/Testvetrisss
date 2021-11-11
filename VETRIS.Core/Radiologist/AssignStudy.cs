using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Reflection;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using VETRIS.DAL;

namespace VETRIS.Core.Radiologist
{
    public class AssignStudy
    {
        #region Constructor
        public AssignStudy()
        {
        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid SessionID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        string strUserName = string.Empty;

        Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
        Guid[] StudyId = new Guid[0];
        string strPatientID = string.Empty;
        string strPatientIDPACS = string.Empty;
        string strStudyUID = string.Empty;
        string strStudyDesc = string.Empty;
        string strAccnNoPACS = string.Empty;
        string strAccnNo = string.Empty;
        int intModalityID = 0;
        string strModality = string.Empty;
        string strModalityName = string.Empty;
        int intCategoryID = 0;
        string strCategoryName = string.Empty;
        int intBodyPartID = 0;
        string strBodyPart = string.Empty;
        string strBodyPartName = string.Empty;
        int intStatus = 0;
        string strStatusDesc = string.Empty;
        string strFilterStudyDt = "N";
        string strFilterRecDt = "N";
        DateTime dtRecDateFrom = DateTime.Today.AddDays(-7);
        DateTime dtRecDateTill = DateTime.Today;
        DateTime dtStudyDateFrom = DateTime.Today.AddDays(-7);
        DateTime dtStudyDateTill = DateTime.Today;
        DateTime dtStudyDate = DateTime.Today;
        string strPatientName = string.Empty;
        string strPatientFName = string.Empty;
        string strPatientLName = string.Empty;
        string strPatientNamePACS = string.Empty;
        DateTime dtPatientDobPACS = DateTime.Today;
        string strPatientGender = string.Empty;
        string strPatientGenderPACS = string.Empty;
        string strSexNeutered = string.Empty;
        string strSexNeuteredPACS = string.Empty;
        DateTime dtPatientDob = DateTime.Today;
        string strPatientAgePACS = string.Empty;
        string strPatientAge = string.Empty;
        decimal decPatientWt = 0;
        decimal decPatientWtPACS = 0;
        string strWtUOM = string.Empty;

        string strOwnerPACS = string.Empty;
        string strOwnerFN = string.Empty;
        string strOwnerLN = string.Empty;
        string strSpeciesPACS = string.Empty;
        int intSpeciesID = 0;
        string strSpeciesName = string.Empty;
        string strBreedPACS = string.Empty;
        Guid BreedID = new Guid("00000000-0000-0000-0000-000000000000");
        string strBreedName = string.Empty;
        Guid InstitutionID = new Guid("00000000-0000-0000-0000-000000000000");
        string strInstitutionName = string.Empty;
        string strInstitutionPACS = string.Empty;
        string strInstitutionEmailID = string.Empty;
        string strInstitutionMobileNo = string.Empty;
        Guid PhysicianID = new Guid("00000000-0000-0000-0000-000000000000");
        string strPhysicianName = string.Empty;
        string strRefPhy = string.Empty;
        string strPhysicianEmailID = string.Empty;
        string strPhysicianMobileNo = string.Empty;
        Guid StudyTypeID = new Guid("00000000-0000-0000-0000-000000000000");
        string strStudyTypeName = string.Empty;

        int intStatusIDPACS = 0;
        string strStatusDescPACS = string.Empty;
        string strReason = string.Empty;
        string strReasonPACS = string.Empty;
        string strPhysNote = string.Empty;
        int intImgCntPACS = 0;
        int intImgCnt = 0;
        int intObjCnt = 0;
        string strImgCntAccepted = "N";
        string strWriteBack = "N";
        string strPACSURL = string.Empty;
        string strIMGVWRURL = string.Empty;
        string strRadiologistName = string.Empty;
        string strPrelimRpt = string.Empty;
        string strFinalRpt = string.Empty;
        string strPACIMGCNTURL = string.Empty;
        string strPACSTUDYDELURL = string.Empty;
        int intPriorityID = 0;
        string strPriorityDesc = string.Empty;
        string strMergeStat = string.Empty;
        string strTrackBy = "I";
        string strRecViaDR = "N";
        Guid PromoReasonId = new Guid("00000000-0000-0000-0000-000000000000");
        decimal decDiscPer = 0;
        string strInvoiced = "N";
        string strConsultApplied = "N";
        string strInstConsultAppl = "N";
        string strServiceCodes = string.Empty;

        string strWS8SRVIP = string.Empty;
        string strWS8CLTIP = string.Empty;
        string strWS8SRVUID = string.Empty;
        string strWS8SRVPWD = string.Empty;
        string strAPIVER = string.Empty;
        string strWS8SYVWRURL = string.Empty;

        string strFilter = "A";
        string strRadType = string.Empty;
        Guid RadiologistID = new Guid("00000000-0000-0000-0000-000000000000");
        string strXML = string.Empty;
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
        public Guid[] STUDY_ID
        {
            get { return StudyId; }
            set { StudyId = value; }
        }
        public string PATIENT_ID
        {
            get { return strPatientID; }
            set { strPatientID = value; }
        }
        public string PATIENT_ID_PACS
        {
            get { return strPatientIDPACS; }
            set { strPatientIDPACS = value; }
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
        public string PATIENT_NAME_PACS
        {
            get { return strPatientNamePACS; }
            set { strPatientNamePACS = value; }
        }
        public string PATIENT_GENDER
        {
            get { return strPatientGender; }
            set { strPatientGender = value; }
        }
        public string PATIENT_GENDER_PACS
        {
            get { return strPatientGenderPACS; }
            set { strPatientGenderPACS = value; }
        }
        public string SEX_NEUTERED
        {
            get { return strSexNeutered; }
            set { strSexNeutered = value; }
        }
        public string SEX_NEUTERED_PACS
        {
            get { return strSexNeuteredPACS; }
            set { strSexNeuteredPACS = value; }
        }
        public DateTime PATIENT_DOB
        {
            get { return dtPatientDob; }
            set { dtPatientDob = value; }
        }
        public DateTime PATIENT_DOB_PACS
        {
            get { return dtPatientDobPACS; }
            set { dtPatientDobPACS = value; }
        }
        public decimal PATIENT_WEIGHT
        {
            get { return decPatientWt; }
            set { decPatientWt = value; }
        }
        public decimal PATIENT_WEIGHT_PACS
        {
            get { return decPatientWtPACS; }
            set { decPatientWtPACS = value; }
        }
        public string WEIGHT_UOM
        {
            get { return strWtUOM; }
            set { strWtUOM = value; }
        }
        public string PATIENT_AGE_PACS
        {
            get { return strPatientAgePACS; }
            set { strPatientAgePACS = value; }
        }
        public string PATIENT_AGE
        {
            get { return strPatientAge; }
            set { strPatientAge = value; }
        }
        public string OWNER_NAME_PACS
        {
            get { return strOwnerPACS; }
            set { strOwnerPACS = value; }
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
        public string SPECIES_PACS
        {
            get { return strSpeciesPACS; }
            set { strSpeciesPACS = value; }
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
        public string BREED_PACS
        {
            get { return strBreedPACS; }
            set { strBreedPACS = value; }
        }
        public Guid PHYSICIAN_ID
        {
            get { return PhysicianID; }
            set { PhysicianID = value; }
        }
        public string PHYSICIAN_NAME
        {
            get { return strPhysicianName; }
            set { strPhysicianName = value; }
        }
        public string REFERRING_PHYSICIAN
        {
            get { return strRefPhy; }
            set { strRefPhy = value; }
        }
        public string PHYSICIAN_EMAIL_ID
        {
            get { return strPhysicianEmailID; }
            set { strPhysicianEmailID = value; }
        }
        public string PHYSICIAN_MOBILE_NUMBER
        {
            get { return strPhysicianMobileNo; }
            set { strPhysicianMobileNo = value; }
        }
        public string STUDY_UID
        {
            get { return strStudyUID; }
            set { strStudyUID = value; }
        }
        public string ACCESSION_NO_PACS
        {
            get { return strAccnNoPACS; }
            set { strAccnNoPACS = value; }
        }
        public string ACCESSION_NO
        {
            get { return strAccnNo; }
            set { strAccnNo = value; }
        }
        public string STUDY_DESCRIPTION
        {
            get { return strStudyDesc; }
            set { strStudyDesc = value; }
        }
        public int MODALITY_ID
        {
            get { return intModalityID; }
            set { intModalityID = value; }
        }
        public string MODALITY_NAME
        {
            get { return strModalityName; }
            set { strModalityName = value; }
        }
        public string MODALITY
        {
            get { return strModality; }
            set { strModality = value; }
        }
        public int CATEGORY_ID
        {
            get { return intCategoryID; }
            set { intCategoryID = value; }
        }
        public string CATEGORY_NAME
        {
            get { return strCategoryName; }
            set { strCategoryName = value; }
        }
        public int BODY_PART_ID
        {
            get { return intBodyPartID; }
            set { intBodyPartID = value; }
        }
        public string BODY_PART_NAME
        {
            get { return strBodyPartName; }
            set { strBodyPartName = value; }
        }
        public string BODY_PART
        {
            get { return strBodyPart; }
            set { strBodyPart = value; }
        }
        public int STATUS
        {
            get { return intStatus; }
            set { intStatus = value; }
        }
        public string STATUS_DESC
        {
            get { return strStatusDesc; }
            set { strStatusDesc = value; }
        }
        public string FILTER_BY_RECEIVED_DATE
        {
            get { return strFilterRecDt; }
            set { strFilterRecDt = value; }
        }
        public DateTime RECEIVED_DATE_FROM
        {
            get { return dtRecDateFrom; }
            set { dtRecDateFrom = value; }
        }
        public DateTime RECEIVED_DATE_TILL
        {
            get { return dtRecDateTill; }
            set { dtRecDateTill = value; }
        }
        public string FILTER_BY_STUDY_DATE
        {
            get { return strFilterStudyDt; }
            set { strFilterStudyDt = value; }
        }
        public DateTime STUDY_DATE_FROM
        {
            get { return dtStudyDateFrom; }
            set { dtStudyDateFrom = value; }
        }
        public DateTime STUDY_DATE_TILL
        {
            get { return dtStudyDateTill; }
            set { dtStudyDateTill = value; }
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
        public string INSTITUTION_PACS
        {
            get { return strInstitutionPACS; }
            set { strInstitutionPACS = value; }
        }
        public string INSTITUTION_EMAIL_ID
        {
            get { return strInstitutionEmailID; }
            set { strInstitutionEmailID = value; }
        }
        public string INSTITUTION_MOBILE_NUMBER
        {
            get { return strInstitutionMobileNo; }
            set { strInstitutionMobileNo = value; }
        }
        public string REASON
        {
            get { return strReason; }
            set { strReason = value; }
        }
        public string REASON_PACS
        {
            get { return strReasonPACS; }
            set { strReasonPACS = value; }
        }
        public string PHYSICIAN_NOTE
        {
            get { return strPhysNote; }
            set { strPhysNote = value; }
        }
        public int IMAGE_COUNT
        {
            get { return intImgCnt; }
            set { intImgCnt = value; }
        }
        public int IMAGE_COUNT_PACS
        {
            get { return intImgCntPACS; }
            set { intImgCntPACS = value; }
        }
        public int OBJECT_COUNT
        {
            get { return intObjCnt; }
            set { intObjCnt = value; }
        }
        public string IMAGE_COUNT_ACCEPTED
        {
            get { return strImgCntAccepted; }
            set { strImgCntAccepted = value; }
        }
        public string TRACK_COUNT_BY
        {
            get { return strTrackBy; }
            set { strTrackBy = value; }
        }
        public string WRITE_BACK
        {
            get { return strWriteBack; }
            set { strWriteBack = value; }
        }
        public Guid STUDY_TYPE_ID
        {
            get { return StudyTypeID; }
            set { StudyTypeID = value; }
        }
        public string STUDY_TYPE_NAME
        {
            get { return strStudyTypeName; }
            set { strStudyTypeName = value; }
        }
        public string PACS_URL
        {
            get { return strPACSURL; }
            set { strPACSURL = value; }
        }
        public string PACS_IMAGE_VIEWER_URL
        {
            get { return strIMGVWRURL; }
            set { strIMGVWRURL = value; }
        }
        public int PACS_STATUS_ID
        {
            get { return intStatusIDPACS; }
            set { intStatusIDPACS = value; }
        }
        public string PACS_STATUS_DESC
        {
            get { return strStatusDescPACS; }
            set { strStatusDescPACS = value; }
        }
        public string RADIOLOGIST_NAME
        {
            get { return strRadiologistName; }
            set { strRadiologistName = value; }
        }
        public string PRELIMINARY_REPORT
        {
            get { return strPrelimRpt; }
            set { strPrelimRpt = value; }
        }
        public string FINAL_REPORT
        {
            get { return strFinalRpt; }
            set { strFinalRpt = value; }
        }
        public string PACS_IMAGE_COUNT_URL
        {
            get { return strPACIMGCNTURL; }
            set { strPACIMGCNTURL = value; }
        }
        public string PACS_STUDY_DELETE_URL
        {
            get { return strPACSTUDYDELURL; }
            set { strPACSTUDYDELURL = value; }
        }
        public int PRIORITY_ID
        {
            get { return intPriorityID; }
            set { intPriorityID = value; }
        }
        public string PRIORITY_DESCRIPTION
        {
            get { return strPriorityDesc; }
            set { strPriorityDesc = value; }
        }
        public string MERGE_STATUS
        {
            get { return strMergeStat; }
            set { strMergeStat = value; }
        }
        public string RECEIVE_DUCOM_FILES_VIA_ROUTER
        {
            get { return strRecViaDR; }
            set { strRecViaDR = value; }
        }
        public Guid PROMOTION_REASON_ID
        {
            get { return PromoReasonId; }
            set { PromoReasonId = value; }
        }
        public decimal DISCOUNT_PERCENT
        {
            get { return decDiscPer; }
            set { decDiscPer = value; }
        }
        public string INVOICED
        {
            get { return strInvoiced; }
            set { strInvoiced = value; }
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
        public string SERVICE_CODES
        {
            get { return strServiceCodes; }
            set { strServiceCodes = value; }
        }

        public string FILTER
        {
            get { return strFilter; }
            set { strFilter = value; }
        }
        public string RADIOLOGIST_TYPE
        {
            get { return strRadType; }
            set { strRadType = value; }
        }
        public Guid RADIOLOGIST_ID
        {
            get { return RadiologistID; }
            set { RadiologistID = value; }
        }

        public string WEB_SERVICE_SERVER_URL
        {
            get { return strWS8SRVIP; }
            set { strWS8SRVIP = value; }
        }
        public string WEB_SERVICE_CLIENT_URL
        {
            get { return strWS8CLTIP; }
            set { strWS8CLTIP = value; }
        }
        public string WEB_SERVICE_USER_ID
        {
            get { return strWS8SRVUID; }
            set { strWS8SRVUID = value; }
        }
        public string WEB_SERVICE_PASSWORD
        {
            get { return strWS8SRVPWD; }
            set { strWS8SRVPWD = value; }
        }
        public string WEB_SERVICE_STUDY_VIEW_URL
        {
            get { return strWS8SYVWRURL; }
            set { strWS8SYVWRURL = value; }
        }
        public string API_VERSION
        {
            get { return strAPIVER; }
            set { strAPIVER = value; }
        }
        #endregion

        #region Browser Methods

        #region SearchWorklistBrowserList
        public bool SearchWorklistBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[10];
            SqlRecordParams[0] = new SqlParameter("@patient_name", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strPatientName;
            SqlRecordParams[1] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[1].Value = intModalityID;
            SqlRecordParams[2] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = InstitutionID;
            SqlRecordParams[3] = new SqlParameter("@consider_received_date", SqlDbType.NChar, 1); SqlRecordParams[3].Value = strFilterRecDt;
            SqlRecordParams[4] = new SqlParameter("@received_date_from", SqlDbType.DateTime); SqlRecordParams[4].Value = dtRecDateFrom;
            SqlRecordParams[5] = new SqlParameter("@received_date_till", SqlDbType.DateTime); SqlRecordParams[5].Value = dtRecDateTill;
            SqlRecordParams[6] = new SqlParameter("@status_id", SqlDbType.Int); SqlRecordParams[6].Value = intStatusIDPACS;
            SqlRecordParams[7] = new SqlParameter("@category_id", SqlDbType.Int); SqlRecordParams[7].Value = intCategoryID;
            SqlRecordParams[8] = new SqlParameter("@species_id", SqlDbType.Int); SqlRecordParams[8].Value = intSpeciesID;
            SqlRecordParams[9] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[9].Value = UserID;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "radiologist_case_assign_fetch_brw", SqlRecordParams);
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
                    ds.Tables[4].TableName = "Status";
                    ds.Tables[5].TableName = "WLStatus";
                    ds.Tables[6].TableName = "Category";
                    ds.Tables[7].TableName = "Priority";
                    ds.Tables[8].TableName = "APIParams";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #endregion

        #region Dialog Method

        #region LoadHeader
        public bool LoadHeader(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; string strControlCode = string.Empty;

            SqlParameter[] SqlRecordParams = new SqlParameter[5];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = intMenuID;
                SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_fetch_hdr", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";
                    ds.Tables[1].TableName = "Species";
                    ds.Tables[2].TableName = "Breed";
                    ds.Tables[3].TableName = "Modality";
                    ds.Tables[4].TableName = "Institutions";
                    ds.Tables[5].TableName = "Physicians";
                    ds.Tables[6].TableName = "PrelimReport";
                    ds.Tables[7].TableName = "FinalReport";
                    ds.Tables[8].TableName = "SelectedStudyTypes";
                    ds.Tables[9].TableName = "Priority";
                    ds.Tables[10].TableName = "Consult";
                    ds.Tables[11].TableName = "Category";

                    #region Details

                    foreach (DataRow dr in ds.Tables["Details"].Rows)
                    {

                        strStudyUID = Convert.ToString(dr["study_uid"]).Trim();
                        dtStudyDate = Convert.ToDateTime(dr["study_date"]);
                        strStudyDesc = Convert.ToString(dr["study_desc"]).Trim();
                        strAccnNoPACS = Convert.ToString(dr["accession_no_pacs"]).Trim();
                        strAccnNo = Convert.ToString(dr["accession_no"]).Trim();
                        strPatientIDPACS = Convert.ToString(dr["patient_id_pacs"]).Trim();
                        strPatientID = Convert.ToString(dr["patient_id"]).Trim();
                        strPatientNamePACS = Convert.ToString(dr["patient_name_pacs"]).Trim();
                        strPatientName = Convert.ToString(dr["patient_name"]).Trim();
                        strPatientFName = Convert.ToString(dr["patient_fname"]).Trim();
                        strPatientLName = Convert.ToString(dr["patient_lname"]).Trim();
                        strPatientGenderPACS = Convert.ToString(dr["patient_sex_pacs"]).Trim();
                        strPatientGender = Convert.ToString(dr["patient_sex"]).Trim();
                        strSexNeuteredPACS = Convert.ToString(dr["sex_neutered_pacs"]).Trim();
                        strSexNeutered = Convert.ToString(dr["sex_neutered_accepted"]).Trim();
                        decPatientWt = Convert.ToDecimal(dr["patient_weight"]);
                        decPatientWtPACS = Convert.ToDecimal(dr["patient_weight_pacs"]);
                        strWtUOM = Convert.ToString(dr["wt_uom"]).Trim();
                        dtPatientDobPACS = Convert.ToDateTime(dr["patient_dob_pacs"]);
                        dtPatientDob = Convert.ToDateTime(dr["patient_dob_accepted"]);
                        strPatientAgePACS = Convert.ToString(dr["patient_age_pacs"]);
                        strPatientAge = Convert.ToString(dr["patient_age_accepted"]);
                        strOwnerPACS = Convert.ToString(dr["owner_name_pacs"]).Trim();
                        strOwnerFN = Convert.ToString(dr["owner_first_name"]).Trim();
                        strOwnerLN = Convert.ToString(dr["owner_last_name"]).Trim();
                        strSpeciesPACS = Convert.ToString(dr["species_pacs"]).Trim();
                        intSpeciesID = Convert.ToInt32(dr["species_id"]);
                        strSpeciesName = Convert.ToString(dr["species_name"]).Trim();
                        strBreedPACS = Convert.ToString(dr["breed_pacs"]).Trim();
                        BreedID = new Guid(Convert.ToString(dr["breed_id"]));
                        strBreedName = Convert.ToString(dr["breed_name"]).Trim();
                        strModality = Convert.ToString(dr["modality_pacs"]).Trim();
                        intModalityID = Convert.ToInt32(dr["modality_id"]);
                        strModalityName = Convert.ToString(dr["modality_name"]).Trim();
                        strBodyPart = Convert.ToString(dr["body_part_pacs"]).Trim();
                        intBodyPartID = Convert.ToInt32(dr["body_part_id"]);
                        strBodyPartName = Convert.ToString(dr["body_part_name"]).Trim();
                        strInstitutionPACS = Convert.ToString(dr["institution_name_pacs"]).Trim();
                        InstitutionID = new Guid(Convert.ToString(dr["institution_id"]));
                        strInstitutionName = Convert.ToString(dr["institution_name"]).Trim();
                        strRefPhy = Convert.ToString(dr["referring_physician_pacs"]).Trim();
                        PhysicianID = new Guid(Convert.ToString(dr["physician_id"]));
                        strPhysicianName = Convert.ToString(dr["physician_name"]).Trim();
                        strReasonPACS = Convert.ToString(dr["reason_pacs"]).Trim();
                        strReason = Convert.ToString(dr["reason_accepted"]).Trim();
                        intImgCntPACS = Convert.ToInt32(dr["img_count_pacs"]);
                        intImgCnt = Convert.ToInt32(dr["img_count"]);
                        intObjCnt = Convert.ToInt32(dr["object_count"]);
                        strImgCntAccepted = Convert.ToString(dr["img_count_accepted"]).Trim();
                        strPACSURL = Convert.ToString(dr["pacs_url"]).Trim();
                        strIMGVWRURL = Convert.ToString(dr["image_viewer_url"]).Trim();
                        strPACIMGCNTURL = Convert.ToString(dr["pacs_img_count_url"]).Trim();
                        strPACSTUDYDELURL = Convert.ToString(dr["pacs_study_del_url"]).Trim();
                        intPriorityID = Convert.ToInt32(dr["priority_id"]);
                        strPriorityDesc = Convert.ToString(dr["priority_desc"]).Trim();
                        strTrackBy = Convert.ToString(dr["track_by"]).Trim();
                        strRecViaDR = Convert.ToString(dr["received_via_dicom_router"]).Trim();
                        strPhysNote = Convert.ToString(dr["physician_note"]).Trim();
                        strConsultApplied = Convert.ToString(dr["consult_applied"]).Trim();
                        intCategoryID = Convert.ToInt32(dr["category_id"]);
                        strCategoryName = Convert.ToString(dr["category_name"]).Trim();
                        strServiceCodes = Convert.ToString(dr["service_codes"]).Trim();
                        intStatusIDPACS = Convert.ToInt32(dr["study_status_pacs"]);
                        strStatusDescPACS = Convert.ToString(dr["status_desc"]).Trim();
                        strWS8SRVIP = Convert.ToString(dr["WS8SRVIP"]).Trim();
                        strWS8CLTIP = Convert.ToString(dr["WS8CLTIP"]).Trim();
                        strWS8SRVUID = Convert.ToString(dr["WS8SRVUID"]).Trim();
                        strWS8SRVPWD = Convert.ToString(dr["WS8SRVPWD"]).Trim();
                        strAPIVER = Convert.ToString(dr["APIVER"]).Trim();
                        strWS8SYVWRURL = Convert.ToString(dr["WS8SYVWRURL"]).Trim();
                    }

                    #endregion

                    #region Preliminary Report
                    foreach (DataRow dr in ds.Tables["PrelimReport"].Rows)
                    {
                        strPrelimRpt = Convert.ToString(dr["report_text"]).Trim();
                    }
                    #endregion

                    #region Final Report
                    foreach (DataRow dr in ds.Tables["FinalReport"].Rows)
                    {
                        strFinalRpt = Convert.ToString(dr["report_text"]).Trim();
                    }
                    #endregion

                    foreach (DataRow dr in ds.Tables["Consult"].Rows)
                    {
                        strInstConsultAppl = Convert.ToString(dr["institution_consult_applicable"]).Trim();
                    }


                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                    ReturnMessage = Convert.ToString(SqlRecordParams[3].Value);
                }

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadHeaderDocuments
        public bool LoadHeaderDocuments(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "case_list_fetch_docs", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Documents";
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
            SqlRecordParams[2] = new SqlParameter("@category_id", SqlDbType.Int); SqlRecordParams[2].Value = intCategoryID;

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

        #region LoadSelectedStudyTypes
        public bool LoadSelectedStudyTypes(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
            SqlRecordParams[1] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[1].Value = intModalityID;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "case_list_fetch_selected_study_types", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "SelStudyTypes";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadAssignmentRadiologist
        public bool LoadAssignmentRadiologist(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[7];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
            SqlRecordParams[1] = new SqlParameter("@type", SqlDbType.NChar,1); SqlRecordParams[1].Value = strRadType;
            SqlRecordParams[2] = new SqlParameter("@filter", SqlDbType.NChar,1); SqlRecordParams[2].Value = strFilter;
            SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[3].Value = intMenuID;
            SqlRecordParams[4] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = UserID;
            SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
            SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "radiologist_case_assign_radiologist_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Radiologists";
                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                    ReturnMessage = Convert.ToString(SqlRecordParams[3].Value);
                }
                
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            try
            {
                if (ValidateRecord(ref ReturnMessage))
                {
                    SqlParameter[] SqlRecordParams = new SqlParameter[9];
                    SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                    SqlRecordParams[1] = new SqlParameter("@type", SqlDbType.NChar, 1); SqlRecordParams[1].Value = strRadType;
                    SqlRecordParams[2] = new SqlParameter("@radiologist_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = RadiologistID;
                    SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[3].Value = intMenuID;
                    SqlRecordParams[4] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = UserID;
                    SqlRecordParams[5] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[5].Value = SessionID;
                    SqlRecordParams[6] = new SqlParameter("@user_name", SqlDbType.NVarChar, 250); SqlRecordParams[6].Direction = ParameterDirection.Output;
                    SqlRecordParams[7] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[7].Direction = ParameterDirection.Output;
                    SqlRecordParams[8] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[8].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "radiologist_case_assign_save", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[8].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;

                    strUserName = Convert.ToString(SqlRecordParams[6].Value).Trim();
                    ReturnMessage = Convert.ToString(SqlRecordParams[7].Value).Trim();
                }
                else
                    bReturn = false;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region ValidateRecord
        private bool ValidateRecord(ref string ReturnMessage)
        {
            bool bReturn = true;

            if (RadiologistID == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                ReturnMessage = "295";
            }
            if (strRadType.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "408";
            }
           

           
            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }

        #endregion

        #region UnassignRadiologist
        public bool UnassignRadiologist(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            try
            {
                if (ValidateRecord(ref ReturnMessage))
                {
                    SqlParameter[] SqlRecordParams = new SqlParameter[9];
                    SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                    SqlRecordParams[1] = new SqlParameter("@type", SqlDbType.NChar, 1); SqlRecordParams[1].Value = strRadType;
                    SqlRecordParams[2] = new SqlParameter("@radiologist_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = RadiologistID;
                    SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[3].Value = intMenuID;
                    SqlRecordParams[4] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = UserID;
                    SqlRecordParams[5] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[5].Value = SessionID;
                    SqlRecordParams[6] = new SqlParameter("@user_name", SqlDbType.NVarChar, 250); SqlRecordParams[6].Direction = ParameterDirection.Output;
                    SqlRecordParams[7] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[7].Direction = ParameterDirection.Output;
                    SqlRecordParams[8] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[8].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "radiologist_case_unassign_save", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[8].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;

                    strUserName = Convert.ToString(SqlRecordParams[6].Value).Trim();
                    ReturnMessage = Convert.ToString(SqlRecordParams[7].Value).Trim();
                }
                else
                    bReturn = false;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region LoadMultipleAssignmentRadiologist
        public bool LoadMultipleAssignmentRadiologist(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;

            if (GenerateStudyXML(ref ReturnMessage, ref CatchMessage))
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[7];
                SqlRecordParams[0] = new SqlParameter("@xml_study", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = strXML;
                SqlRecordParams[1] = new SqlParameter("@type", SqlDbType.NChar, 1); SqlRecordParams[1].Value = strRadType;
                SqlRecordParams[2] = new SqlParameter("@filter", SqlDbType.NChar, 1); SqlRecordParams[2].Value = strFilter;
                SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[3].Value = intMenuID;
                SqlRecordParams[4] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = UserID;
                SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;

                try
                {
                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "radiologist_multiple_case_assign_radiologist_fetch", SqlRecordParams);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "Radiologists";
                        bReturn = true;
                    }
                    else
                    {
                        bReturn = false;
                        ReturnMessage = Convert.ToString(SqlRecordParams[3].Value);
                    }

                }
                catch (Exception expErr)
                { bReturn = false; CatchMessage = expErr.Message; }
            }
            else
                bReturn = false;

            return bReturn;
        }
        #endregion

        #region SaveMultipleAssignment
        public bool SaveMultipleAssignment(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            try
            {
                if (ValidateRecord(ref ReturnMessage))
                {
                    if (GenerateStudyXML(ref ReturnMessage, ref CatchMessage))
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[9];
                        SqlRecordParams[0] = new SqlParameter("@type", SqlDbType.NChar, 1); SqlRecordParams[0].Value = strRadType;
                        SqlRecordParams[1] = new SqlParameter("@radiologist_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = RadiologistID;
                        SqlRecordParams[2] = new SqlParameter("@xml_study", SqlDbType.NText); SqlRecordParams[2].Value = strXML;
                        SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[3].Value = intMenuID;
                        SqlRecordParams[4] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = UserID;
                        SqlRecordParams[5] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[5].Value = SessionID;
                        SqlRecordParams[6] = new SqlParameter("@user_name", SqlDbType.NVarChar, 250); SqlRecordParams[6].Direction = ParameterDirection.Output;
                        SqlRecordParams[7] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[7].Direction = ParameterDirection.Output;
                        SqlRecordParams[8] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[8].Direction = ParameterDirection.Output;

                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "radiologist_multiple_case_assign_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[8].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[6].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[7].Value).Trim();
                    }
                    else
                        bReturn = false;
                }
                else
                    bReturn = false;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region GenerateStudyXML
        private bool GenerateStudyXML(ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (StudyId.Length > 0)
                {

                    sbXML.Append("<study>");

                    for (int i = 0; i < StudyId.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<id>" + StudyId[i].ToString() + "</id>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    if (ReturnMessage.Trim() != string.Empty)
                    {
                        bReturn = false;
                        sbXML.Clear();
                        strXML = string.Empty;
                    }
                    else
                    {
                        bReturn = true;
                        sbXML.Append("</study>");
                        strXML = sbXML.ToString();
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

        #endregion
    }
}
