using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Reflection;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using VETRIS.DAL;

namespace VETRIS.Core.Case
{
    public class CaseStudy
    {
        #region Constructor
        public CaseStudy()
        {
        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid SessionID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        string strUserName = string.Empty;
        string strUserRoleCode = string.Empty;

        Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
        string strPatientID = string.Empty;
        string strPatientIDPACS = string.Empty;
        string strStudyUID = string.Empty;
        string strSeriesUID = string.Empty;
        string strSeriesNo = string.Empty;

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
        int intCountryID = 0;
        string strCountryName = string.Empty;
        int intStateID = 0;
        string strStateName = string.Empty;
        string strCity = string.Empty;

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
        string strInstitutionCode = string.Empty;
        string strInstitutionName = string.Empty;
        string strInstitutionPACS = string.Empty;
        string strInstitutionEmailID = string.Empty;
        string strInstitutionMobileNo = string.Empty;
        Guid PhysicianID = new Guid("00000000-0000-0000-0000-000000000000");
        string strPhysicianName = string.Empty;
        string strPhysicianCode = string.Empty;
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
        string strRadiologistName = string.Empty;
        string strDictRpt = string.Empty;
        string strDictRptHTML = string.Empty;
        string strTransRpt = string.Empty;
        string strTransRptHTML = string.Empty;
        string strTranslateRpt = string.Empty;
        string strTranslateRptHTML = string.Empty;
        string strPrelimRpt = string.Empty;
        string strPrelimRptHTML = string.Empty;
        string strFinalRpt = string.Empty;
        string strFinalRptHTML = string.Empty;
        int intRptDisclReasonID = 0;
        string strRptDisclReasonType = string.Empty;
        string strRptDisclReasonDesc = string.Empty;
        Guid AbnormalRptReasonID = new Guid("00000000-0000-0000-0000-000000000000");
        string strAbnormalRptReason = string.Empty;
        string strShowAbRpt = "N";
        string strRptRelPending = "X";

        int intPriorityID = 0;
        string strPriorityDesc = string.Empty;
        string strMergeStat = string.Empty;
        string strTrackBy = "I";
        string strInvoiceBy = string.Empty;
        string strRecViaDR = "N";
        string strBeyondOpHr = "N";
        double dblStudyCost = 0;
        Guid PromoReasonId = new Guid("00000000-0000-0000-0000-000000000000");
        decimal decDiscPer = 0;
        double dblDiscAmt = 0;
        string strDiscBy = "N";
        string strInvoiced = "N";
        string strConsultApplied = "N";
        string strInstConsultAppl = "N";
        string strRateTheReport = "N";
        string strMarkToTeach = "N";
        string strServiceCodes = string.Empty;

        Guid RadiologistId = new Guid("00000000-0000-0000-0000-000000000000");
        Guid PrelimRadiologistId = new Guid("00000000-0000-0000-0000-000000000000");
        Guid FinalRadiologistId = new Guid("00000000-0000-0000-0000-000000000000");
        Guid TranscriptionistId = new Guid("00000000-0000-0000-0000-000000000000");
        string strPrelimRadAssn = string.Empty;
        string strFinalRadAssn = string.Empty;
        string strTranscriptionist = string.Empty;
        Guid RptId = new Guid("00000000-0000-0000-0000-000000000000");
        string strRptType = "N";
        string strRptText = string.Empty;
        string strRptHtml = string.Empty;
        string strStudyText = string.Empty;
        string strRptFinding = string.Empty;
        string strRptConclusion = string.Empty;
        string strRptRate = string.Empty;
        string strSessID = string.Empty;
        int intAddnSrl = 0;
        string strAddnText = string.Empty;
        string strAddnTextHtml = string.Empty;
        string strCustomRpt = string.Empty;

        string strPACSUserID = string.Empty;
        string strPACSUserPwd = string.Empty;
        string strPACIMGCNTURL = string.Empty;
        string strPACSTUDYDELURL = string.Empty;
        string strPACSURL = string.Empty;
        string strIMGVWRURL = string.Empty;
        string strWS8SRVIP = string.Empty;
        string strWS8CLTIP = string.Empty;
        string strWS8SRVUID = string.Empty;
        string strWS8SRVPWD = string.Empty;
        string strAPIVER = string.Empty;
        string strWS8SYVWRURL = string.Empty;
        string strWS8IMGVWRURL = string.Empty;
        string strFTPABSPATH = string.Empty;
        string strDCMMODIFYEXEPATH = string.Empty;
        string strPACSARCHIVEFLDR = string.Empty;
        string strPACSARCHALTFLDR = string.Empty;
        string strVRSAPPLINK = string.Empty;
        string strGOOGLETRANSAPILINK = string.Empty;
        string strGOOGLETRANSAPIKEY = string.Empty;
        string strSourcePath = string.Empty;
        string strStdTZName = string.Empty;
        string strNextOpTime = string.Empty;
        string strStdDelvTime = string.Empty;
        string strStatDelvTime = string.Empty;
        string strDelvTime = string.Empty;
        string strMsgDisp = string.Empty;
        string strSubmitPriority = "Y";
        int intSenderTZOffsetMins = 0;
        int intStudyToMerge = 0;
        string strXML = string.Empty;
        string strCheckDOB = "Y";
        string strSyncMode = string.Empty;

        int intActualFileCount = 0;
        int intPendingFileCount = 0;
        string databaseName = string.Empty;
        #endregion

        #region Properties
        public Guid USER_SESSION_ID
        {
            get { return SessionID; }
            set { SessionID = value; }
        }
        public int MENU_ID
        {
            get { return intMenuID; }
            set { intMenuID = value; }
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
        public string USER_ROLE_CODE
        {
            get { return strUserRoleCode; }
            set { strUserRoleCode = value; }
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
        public int PATIENT_COUNTRY_ID
        {
            get { return intCountryID; }
            set { intCountryID = value; }
        }
        public string PATIENT_COUNTRY_NAME
        {
            get { return strCountryName; }
            set { strCountryName = value; }
        }
        public int PATIENT_STATE_ID
        {
            get { return intStateID; }
            set { intStateID = value; }
        }
        public string PATIENT_STATE_NAME
        {
            get { return strStateName; }
            set { strStateName = value; }
        }
        public string PATIENT_CITY
        {
            get { return strCity; }
            set { strCity = value; }
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
        public string PHYSICIAN_CODE
        {
            get { return strPhysicianCode; }
            set { strPhysicianCode = value; }
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
        public string INSTITUTION_CODE
        {
            get { return strInstitutionCode; }
            set { strInstitutionCode = value; }
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
        public string INVOICING_BY
        {
            get { return strInvoiceBy; }
            set { strInvoiceBy = value; }
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

        public Guid RADIOLOGIST_ID
        {
            get { return RadiologistId; }
            set { RadiologistId = value; }
        }
        public string RADIOLOGIST_NAME
        {
            get { return strRadiologistName; }
            set { strRadiologistName = value; }
        }
        public string DICTATED_REPORT
        {
            get { return strDictRpt; }
            set { strDictRpt = value; }
        }
        public string DICTATED_REPORT_HTML
        {
            get { return strDictRptHTML; }
            set { strDictRptHTML = value; }
        }
        public string TRANSCRIPTIONIST_REPORT
        {
            get { return strTransRpt; }
            set { strTransRpt = value; }
        }
        public string TRANSCRIPTIONIST_REPORT_HTML
        {
            get { return strTransRptHTML; }
            set { strTransRptHTML = value; }
        }
        public string TRANSLATED_REPORT
        {
            get { return strTranslateRpt; }
            set { strTranslateRpt = value; }
        }
        public string TRANSLATED_REPORT_HTML
        {
            get { return strTranslateRptHTML; }
            set { strTranslateRptHTML = value; }
        }
        public string PRELIMINARY_REPORT
        {
            get { return strPrelimRpt; }
            set { strPrelimRpt = value; }
        }
        public string PRELIMINARY_REPORT_HTML
        {
            get { return strPrelimRptHTML; }
            set { strPrelimRptHTML = value; }
        }
        public string FINAL_REPORT
        {
            get { return strFinalRpt; }
            set { strFinalRpt = value; }
        }
        public string FINAL_REPORT_HTML
        {
            get { return strFinalRptHTML; }
            set { strFinalRptHTML = value; }
        }
        public string FINAL_REPORT_RELEASE_PENDING
        {
            get { return strRptRelPending; }
            set { strRptRelPending = value; }
        }
        public int REPORT_DISCLAIMER_REASON_ID
        {
            get { return intRptDisclReasonID; }
            set { intRptDisclReasonID = value; }
        }
        public string REPORT_DISCLAIMER_REASON_TYPE
        {
            get { return strRptDisclReasonType; }
            set { strRptDisclReasonType = value; }
        }
        public string REPORT_DISCLAIMER_REASON_DESCRIPTION
        {
            get { return strRptDisclReasonDesc; }
            set { strRptDisclReasonDesc = value; }
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
        public string DISCOUNT_BY
        {
            get { return strDiscBy; }
            set { strDiscBy = value; }
        }
        public decimal DISCOUNT_PERCENT
        {
            get { return decDiscPer; }
            set { decDiscPer = value; }
        }
        public double DISCOUNT_AMOUNT
        {
            get { return dblDiscAmt; }
            set { dblDiscAmt = value; }
        }
        public double STUDY_COST
        {
            get { return dblStudyCost; }
            set { dblStudyCost = value; }
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

        public Guid PRELIMINARY_RADIOLOGIST_ID
        {
            get { return PrelimRadiologistId; }
            set { PrelimRadiologistId = value; }
        }
        public string PRELIMINARY_RADIOLOGIST_ASSIGNED
        {
            get { return strPrelimRadAssn; }
            set { strPrelimRadAssn = value; }
        }
        public Guid FINAL_RADIOLOGIST_ID
        {
            get { return FinalRadiologistId; }
            set { FinalRadiologistId = value; }
        }
        public string FINAL_RADIOLOGIST_ASSIGNED
        {
            get { return strFinalRadAssn; }
            set { strFinalRadAssn = value; }
        }
        public Guid TRANSCRIPTIONIST_ID
        {
            get { return TranscriptionistId; }
            set { TranscriptionistId = value; }
        }
        public string TRANSCRIPTIONIST_NAME
        {
            get { return strTranscriptionist; }
            set { strTranscriptionist = value; }
        }
        public Guid REPORT_ID
        {
            get { return RptId; }
            set { RptId = value; }
        }
        public string REPORT_TYPE
        {
            get { return strRptType; }
            set { strRptType = value; }
        }
        public string REPORT_TEXT
        {
            get { return strRptText; }
            set { strRptText = value; }
        }
        public string REPORT_TEXT_HTML
        {
            get { return strRptHtml; }
            set { strRptHtml = value; }
        }
        public string STUDY_TEXT
        {
            get { return strStudyText; }
            set { strStudyText = value; }
        }
        public string REPORT_FINDINGS
        {
            get { return strRptFinding; }
            set { strRptFinding = value; }
        }
        public string REPORT_CONCLUSION
        {
            get { return strRptConclusion; }
            set { strRptConclusion = value; }
        }
        public string REPORT_RATING
        {
            get { return strRptRate; }
            set { strRptRate = value; }
        }
        public string RATE_THE_REPORT
        {
            get { return strRateTheReport; }
            set { strRateTheReport = value; }
        }
        public Guid ABNORMAL_REPORT_REASON_ID
        {
            get { return AbnormalRptReasonID; }
            set { AbnormalRptReasonID = value; }
        }
        public string ABNORMAL_REPORT_REASON
        {
            get { return strAbnormalRptReason; }
            set { strAbnormalRptReason = value; }
        }
        public string SHOW_ABNORMAL_REPORTS
        {
            get { return strShowAbRpt; }
            set { strShowAbRpt = value; }
        }
        public int ADDENDUM_SERIAL
        {
            get { return intAddnSrl; }
            set { intAddnSrl = value; }
        }
        public string ADDENDUM_TEXT
        {
            get { return strAddnText; }
            set { strAddnText = value; }
        }
        public string ADDENDUM_TEXT_HTML
        {
            get { return strAddnTextHtml; }
            set { strAddnTextHtml = value; }
        }
        public string CUSTOM_REPORT
        {
            get { return strCustomRpt; }
            set { strCustomRpt = value; }
        }
        public string SOURCE_PATH
        {
            get { return strSourcePath; }
            set { strSourcePath = value; }
        }
        public string SESSION_ID
        {
            get { return strSessID; }
            set { strSessID = value; }
        }
        public string MARKED_FOR_TEACHING
        {
            get { return strMarkToTeach; }
            set { strMarkToTeach = value; }
        }

        public string PACS_USER_ID
        {
            get { return strPACSUserID; }
            set { strPACSUserID = value; }
        }
        public string PAS_USER_PASSWORD
        {
            get { return strPACSUserPwd; }
            set { strPACSUserPwd = value; }
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
        public string WEB_SERVICE_IMAGE_VIEW_URL
        {
            get { return strWS8IMGVWRURL; }
            set { strWS8IMGVWRURL = value; }
        }
        public string API_VERSION
        {
            get { return strAPIVER; }
            set { strAPIVER = value; }
        }

        public string FTP_ABSOLUTE_PATH
        {
            get { return strFTPABSPATH; }
            set { strFTPABSPATH = value; }
        }
        public string DCM_FILE_MODIFY_EXE_PATH
        {
            get { return strDCMMODIFYEXEPATH; }
            set { strDCMMODIFYEXEPATH = value; }
        }
        public string PACS_ARCHIVE_FOLDER
        {
            get { return strPACSARCHIVEFLDR; }
            set { strPACSARCHIVEFLDR = value; }
        }
        public string PACS_ARCHIVE_ALTERNATE_FOLDER
        {
            get { return strPACSARCHALTFLDR; }
            set { strPACSARCHALTFLDR = value; }
        }
        public string VETRIS_APPLICATION_LINK
        {
            get { return strVRSAPPLINK; }
            set { strVRSAPPLINK = value; }
        }
        public string GOOGLE_TRANSLATE_API_LINK
        {
            get { return strGOOGLETRANSAPILINK; }
            set { strGOOGLETRANSAPILINK = value; }
        }
        public string GOOGLE_TRANSLATE_API_KEY
        {
            get { return strGOOGLETRANSAPIKEY; }
            set { strGOOGLETRANSAPIKEY = value; }
        }

        public string STANDARD_TIME_ZONE_NAME
        {
            get { return strStdTZName; }
            set { strStdTZName = value; }
        }
        public string NEXT_OPERATION_TIME
        {
            get { return strNextOpTime; }
            set { strNextOpTime = value; }
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
        public int STUDIES_TO_MERGE
        {
            get { return intStudyToMerge; }
            set { intStudyToMerge = value; }
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
        public int ACTUAL_FILE_COUNT
        {
            get { return intActualFileCount; }
            set { intActualFileCount = value; }
        }
        public int PENDING_FILE_COUNT
        {
            get { return intPendingFileCount; }
            set { intStudyToMerge = value; }
        }
        public string SYNC_MODE
        {
            get { return strSyncMode; }
            set { strSyncMode = value; }
        }

        public string DATABASE_NAME
        {
            get { return databaseName; }
            set { databaseName = value; }
        }
        #endregion

        #region Paging
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public int TotalRecords { get; set; }
        #endregion

        #region Browser Methods

        #region SearchRequireActionBrowserList
        public bool SearchRequireActionBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[10];
            SqlRecordParams[0] = new SqlParameter("@patient_name", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strPatientName;
            SqlRecordParams[1] = new SqlParameter("@modality", SqlDbType.NVarChar, 50); SqlRecordParams[1].Value = strModality;
            SqlRecordParams[2] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = InstitutionID;
            SqlRecordParams[3] = new SqlParameter("@consider_received_date", SqlDbType.NChar, 1); SqlRecordParams[3].Value = strFilterRecDt;
            SqlRecordParams[4] = new SqlParameter("@received_date_from", SqlDbType.DateTime); SqlRecordParams[4].Value = dtRecDateFrom;
            SqlRecordParams[5] = new SqlParameter("@received_date_till", SqlDbType.DateTime); SqlRecordParams[5].Value = dtRecDateTill;
            SqlRecordParams[6] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[6].Value = strStudyUID;
            SqlRecordParams[7] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[7].Value = UserID;
            SqlRecordParams[8] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[8].Value = intMenuID;
            SqlRecordParams[9] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[9].Value = SessionID;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "case_list_req_action_fetch_brw", SqlRecordParams);
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

        #region SearchInProgressBrowserList
        public bool SearchInProgressBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[15];
            SqlRecordParams[0] = new SqlParameter("@patient_name", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strPatientName;
            SqlRecordParams[1] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[1].Value = intModalityID;
            SqlRecordParams[2] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = InstitutionID;
            SqlRecordParams[3] = new SqlParameter("@consider_received_date", SqlDbType.NChar, 1); SqlRecordParams[3].Value = strFilterRecDt;
            SqlRecordParams[4] = new SqlParameter("@received_date_from", SqlDbType.DateTime); SqlRecordParams[4].Value = dtRecDateFrom;
            SqlRecordParams[5] = new SqlParameter("@received_date_till", SqlDbType.DateTime); SqlRecordParams[5].Value = dtRecDateTill;
            SqlRecordParams[6] = new SqlParameter("@status_id", SqlDbType.Int); SqlRecordParams[6].Value = intStatusIDPACS;
            SqlRecordParams[7] = new SqlParameter("@category_id", SqlDbType.Int); SqlRecordParams[7].Value = intCategoryID;
            SqlRecordParams[8] = new SqlParameter("@radiologist_id", SqlDbType.UniqueIdentifier); SqlRecordParams[8].Value = RadiologistId;
            SqlRecordParams[9] = new SqlParameter("@species_id", SqlDbType.Int); SqlRecordParams[9].Value = intSpeciesID;
            SqlRecordParams[10] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[10].Value = UserID;
            SqlRecordParams[11] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[11].Value = intMenuID;
            SqlRecordParams[12] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[12].Value = SessionID;
            SqlRecordParams[13] = new SqlParameter("@study_uid", SqlDbType.NVarChar,250); SqlRecordParams[13].Value = strStudyUID;
            SqlRecordParams[14] = new SqlParameter("@priority_id", SqlDbType.UniqueIdentifier); SqlRecordParams[14].Value = intPriorityID;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "case_list_in_progress_rpt_fetch_brw", SqlRecordParams);
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

        #region SearchPreliminaryBrowserList
        public bool SearchPreliminaryBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[14];
            SqlRecordParams[0] = new SqlParameter("@patient_name", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strPatientName;
            SqlRecordParams[1] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[1].Value = intModalityID;
            SqlRecordParams[2] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = InstitutionID;
            SqlRecordParams[3] = new SqlParameter("@physician_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = PhysicianID;
            SqlRecordParams[4] = new SqlParameter("@consider_received_date", SqlDbType.NChar, 1); SqlRecordParams[4].Value = strFilterRecDt;
            SqlRecordParams[5] = new SqlParameter("@received_date_from", SqlDbType.DateTime); SqlRecordParams[5].Value = dtRecDateFrom;
            SqlRecordParams[6] = new SqlParameter("@received_date_till", SqlDbType.DateTime); SqlRecordParams[6].Value = dtRecDateTill;
            SqlRecordParams[7] = new SqlParameter("@category_id", SqlDbType.Int); SqlRecordParams[7].Value = intCategoryID;
            SqlRecordParams[8] = new SqlParameter("@prelim_radiologist_id", SqlDbType.UniqueIdentifier); SqlRecordParams[8].Value = PrelimRadiologistId;
            SqlRecordParams[9] = new SqlParameter("@final_radiologist_id", SqlDbType.UniqueIdentifier); SqlRecordParams[9].Value = FinalRadiologistId;
            SqlRecordParams[10] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[10].Value = UserID;
            SqlRecordParams[11] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[11].Value = intMenuID;
            SqlRecordParams[12] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[12].Value = SessionID;
            SqlRecordParams[13] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 250); SqlRecordParams[13].Value = strStudyUID;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "case_list_prelim_rpt_fetch_brw", SqlRecordParams);
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

        #region SearchFinalBrowserList
        public bool SearchFinalBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[14];
            SqlRecordParams[0] = new SqlParameter("@patient_name", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strPatientName;
            SqlRecordParams[1] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[1].Value = intModalityID;
            SqlRecordParams[2] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = InstitutionID;
            SqlRecordParams[3] = new SqlParameter("@physician_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = PhysicianID;
            SqlRecordParams[4] = new SqlParameter("@consider_received_date", SqlDbType.NChar, 1); SqlRecordParams[4].Value = strFilterRecDt;
            SqlRecordParams[5] = new SqlParameter("@received_date_from", SqlDbType.DateTime); SqlRecordParams[5].Value = dtRecDateFrom;
            SqlRecordParams[6] = new SqlParameter("@received_date_till", SqlDbType.DateTime); SqlRecordParams[6].Value = dtRecDateTill;
            SqlRecordParams[7] = new SqlParameter("@category_id", SqlDbType.Int); SqlRecordParams[7].Value = intCategoryID;
            SqlRecordParams[8] = new SqlParameter("@approved_by", SqlDbType.UniqueIdentifier); SqlRecordParams[8].Value = FinalRadiologistId;
            SqlRecordParams[9] = new SqlParameter("@radiologist_id", SqlDbType.UniqueIdentifier); SqlRecordParams[9].Value = RadiologistId;
            SqlRecordParams[10] = new SqlParameter("@show_abnormal_rpt", SqlDbType.NChar, 1); SqlRecordParams[10].Value = strShowAbRpt;
            SqlRecordParams[11] = new SqlParameter("@abnormal_report_reason_id", SqlDbType.UniqueIdentifier); SqlRecordParams[11].Value = AbnormalRptReasonID;
            SqlRecordParams[12] = new SqlParameter("@rpt_rel_pending", SqlDbType.NChar, 1); SqlRecordParams[12].Value = strRptRelPending;
            SqlRecordParams[13] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[13].Value = UserID;
            //SqlRecordParams[14] = new SqlParameter("@page_size", SqlDbType.Int); SqlRecordParams[14].Value = PageSize;
            //SqlRecordParams[15] = new SqlParameter("@page_no", SqlDbType.Int); SqlRecordParams[15].Value = PageNo;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "case_list_final_rpt_fetch_brw", SqlRecordParams);
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

        #region SearcArchiveBrowserList
        public bool SearcArchiveBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[16];
            SqlRecordParams[0] = new SqlParameter("@patient_name", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strPatientName;
            SqlRecordParams[1] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[1].Value = intModalityID;
            SqlRecordParams[2] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = InstitutionID;
            SqlRecordParams[3] = new SqlParameter("@physician_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = PhysicianID;
            SqlRecordParams[4] = new SqlParameter("@consider_received_date", SqlDbType.NChar, 1); SqlRecordParams[4].Value = strFilterRecDt;
            SqlRecordParams[5] = new SqlParameter("@received_date_from", SqlDbType.DateTime); SqlRecordParams[5].Value = dtRecDateFrom;
            SqlRecordParams[6] = new SqlParameter("@received_date_till", SqlDbType.DateTime); SqlRecordParams[6].Value = dtRecDateTill;
            SqlRecordParams[7] = new SqlParameter("@category_id", SqlDbType.Int); SqlRecordParams[7].Value = intCategoryID;
            SqlRecordParams[8] = new SqlParameter("@status_id", SqlDbType.Int); SqlRecordParams[8].Value = intStatusIDPACS;
            SqlRecordParams[9] = new SqlParameter("@approved_by", SqlDbType.UniqueIdentifier); SqlRecordParams[9].Value = FinalRadiologistId;
            SqlRecordParams[10] = new SqlParameter("@radiologist_id", SqlDbType.UniqueIdentifier); SqlRecordParams[10].Value = RadiologistId;
            SqlRecordParams[11] = new SqlParameter("@show_abnormal_rpt", SqlDbType.NChar, 1); SqlRecordParams[11].Value = strShowAbRpt;
            SqlRecordParams[12] = new SqlParameter("@abnormal_report_reason_id", SqlDbType.UniqueIdentifier); SqlRecordParams[12].Value = AbnormalRptReasonID;
            SqlRecordParams[13] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[13].Value = UserID;
            SqlRecordParams[14] = new SqlParameter("@page_size", SqlDbType.Int); SqlRecordParams[14].Value = PageSize;
            SqlRecordParams[15] = new SqlParameter("@page_no", SqlDbType.Int); SqlRecordParams[15].Value = PageNo;


            try
            {
                this.TotalRecords = 0;
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "case_list_archive_fetch_brw", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "BrowserList";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        this.TotalRecords = Convert.ToInt32(ds.Tables[0].Rows[0]["total_rows"]);
                    }
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region SearchFinalReportBrowserList
        public bool SearchFinalReportBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[24];
            //SqlParameter[] SqlRecordParams = new SqlParameter[22];
            SqlRecordParams[0] = new SqlParameter("@patient_name", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strPatientName;
            SqlRecordParams[1] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[1].Value = intModalityID;
            SqlRecordParams[2] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = InstitutionID;
            SqlRecordParams[3] = new SqlParameter("@physician_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = PhysicianID;
            SqlRecordParams[4] = new SqlParameter("@consider_received_date", SqlDbType.NChar, 1); SqlRecordParams[4].Value = strFilterRecDt;
            SqlRecordParams[5] = new SqlParameter("@received_date_from", SqlDbType.DateTime); SqlRecordParams[5].Value = dtRecDateFrom;
            SqlRecordParams[6] = new SqlParameter("@received_date_till", SqlDbType.DateTime); SqlRecordParams[6].Value = dtRecDateTill;
            SqlRecordParams[7] = new SqlParameter("@category_id", SqlDbType.Int); SqlRecordParams[7].Value = intCategoryID;
            SqlRecordParams[8] = new SqlParameter("@status_id", SqlDbType.Int); SqlRecordParams[8].Value = intStatusIDPACS;
            SqlRecordParams[9] = new SqlParameter("@approved_by", SqlDbType.UniqueIdentifier); SqlRecordParams[9].Value = FinalRadiologistId;
            SqlRecordParams[10] = new SqlParameter("@radiologist_id", SqlDbType.UniqueIdentifier); SqlRecordParams[10].Value = RadiologistId;
            SqlRecordParams[11] = new SqlParameter("@show_abnormal_rpt", SqlDbType.NChar, 1); SqlRecordParams[11].Value = strShowAbRpt;
            SqlRecordParams[12] = new SqlParameter("@abnormal_report_reason_id", SqlDbType.UniqueIdentifier); SqlRecordParams[12].Value = AbnormalRptReasonID;
            SqlRecordParams[13] = new SqlParameter("@rpt_rel_pending", SqlDbType.NChar, 1); SqlRecordParams[13].Value = strRptRelPending;
            SqlRecordParams[14] = new SqlParameter("@mark_to_teach", SqlDbType.NChar, 1); SqlRecordParams[14].Value = strMarkToTeach;
            SqlRecordParams[15] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[15].Value = UserID;
            SqlRecordParams[16] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[16].Value = intMenuID;
            SqlRecordParams[17] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[17].Value = SessionID;
            SqlRecordParams[18] = new SqlParameter("@page_size", SqlDbType.Int); SqlRecordParams[18].Value = PageSize;
            SqlRecordParams[19] = new SqlParameter("@page_no", SqlDbType.Int); SqlRecordParams[19].Value = PageNo;
            SqlRecordParams[20] = new SqlParameter("@db_name", SqlDbType.Int); SqlRecordParams[20].Value = databaseName;
            SqlRecordParams[21] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 250); SqlRecordParams[21].Value = strStudyUID;
            SqlRecordParams[22] = new SqlParameter("@species_id", SqlDbType.Int); SqlRecordParams[22].Value = 2;
            SqlRecordParams[23] = new SqlParameter("@search_key", SqlDbType.NVarChar, 250); SqlRecordParams[23].Value = "Thorax and abdomen";
           

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "case_list_final_rpt_new_fetch_brw", SqlRecordParams);
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

        #region FetchFinalReportListExcelRecords
        public bool FetchFinalReportListExcelRecords(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[16];
            SqlRecordParams[0] = new SqlParameter("@patient_name", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strPatientName;
            SqlRecordParams[1] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[1].Value = intModalityID;
            SqlRecordParams[2] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = InstitutionID;
            SqlRecordParams[3] = new SqlParameter("@physician_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = PhysicianID;
            SqlRecordParams[4] = new SqlParameter("@consider_received_date", SqlDbType.NChar, 1); SqlRecordParams[4].Value = strFilterRecDt;
            SqlRecordParams[5] = new SqlParameter("@received_date_from", SqlDbType.DateTime); SqlRecordParams[5].Value = dtRecDateFrom;
            SqlRecordParams[6] = new SqlParameter("@received_date_till", SqlDbType.DateTime); SqlRecordParams[6].Value = dtRecDateTill;
            SqlRecordParams[7] = new SqlParameter("@category_id", SqlDbType.Int); SqlRecordParams[7].Value = intCategoryID;
            SqlRecordParams[8] = new SqlParameter("@status_id", SqlDbType.Int); SqlRecordParams[8].Value = intStatusIDPACS;
            SqlRecordParams[9] = new SqlParameter("@approved_by", SqlDbType.UniqueIdentifier); SqlRecordParams[9].Value = FinalRadiologistId;
            SqlRecordParams[10] = new SqlParameter("@radiologist_id", SqlDbType.UniqueIdentifier); SqlRecordParams[10].Value = RadiologistId;
            SqlRecordParams[11] = new SqlParameter("@show_abnormal_rpt", SqlDbType.NChar, 1); SqlRecordParams[11].Value = strShowAbRpt;
            SqlRecordParams[12] = new SqlParameter("@abnormal_report_reason_id", SqlDbType.UniqueIdentifier); SqlRecordParams[12].Value = AbnormalRptReasonID;
            SqlRecordParams[13] = new SqlParameter("@rpt_rel_pending", SqlDbType.NChar, 1); SqlRecordParams[13].Value = strRptRelPending;
            SqlRecordParams[14] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[14].Value = UserID;
            SqlRecordParams[15] = new SqlParameter("@db_name", SqlDbType.Int); SqlRecordParams[15].Value = databaseName;
            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "case_list_final_rpt_excel_fetch", SqlRecordParams);
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

        #region SearchPatient
        public bool SearchPatient(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[8];
            SqlRecordParams[0] = new SqlParameter("@patient_name", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strPatientName;
            SqlRecordParams[1] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[1].Value = intModalityID;
            SqlRecordParams[2] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = InstitutionID;
            SqlRecordParams[3] = new SqlParameter("@consider_received_date", SqlDbType.NChar, 1); SqlRecordParams[3].Value = strFilterRecDt;
            SqlRecordParams[4] = new SqlParameter("@received_date_from", SqlDbType.DateTime); SqlRecordParams[4].Value = dtRecDateFrom;
            SqlRecordParams[5] = new SqlParameter("@received_date_till", SqlDbType.DateTime); SqlRecordParams[5].Value = dtRecDateTill;
            SqlRecordParams[6] = new SqlParameter("@category_id", SqlDbType.Int); SqlRecordParams[6].Value = intCategoryID;
            SqlRecordParams[7] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[7].Value = UserID;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "case_list_patient_search_fetch", SqlRecordParams);
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

        #region SearchWorklistBrowserList
        public bool SearchWorklistBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[9];
            SqlRecordParams[0] = new SqlParameter("@patient_name", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strPatientName;
            SqlRecordParams[1] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[1].Value = intModalityID;
            SqlRecordParams[2] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = InstitutionID;
            SqlRecordParams[3] = new SqlParameter("@consider_received_date", SqlDbType.NChar, 1); SqlRecordParams[3].Value = strFilterRecDt;
            SqlRecordParams[4] = new SqlParameter("@received_date_from", SqlDbType.DateTime); SqlRecordParams[4].Value = dtRecDateFrom;
            SqlRecordParams[5] = new SqlParameter("@received_date_till", SqlDbType.DateTime); SqlRecordParams[5].Value = dtRecDateTill;
            SqlRecordParams[6] = new SqlParameter("@status_id", SqlDbType.Int); SqlRecordParams[6].Value = intStatusIDPACS;
            SqlRecordParams[7] = new SqlParameter("@category_id", SqlDbType.Int); SqlRecordParams[7].Value = intCategoryID;
            SqlRecordParams[8] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[8].Value = UserID;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "case_list_radiologist_worklist_fetch_brw", SqlRecordParams);
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
                    ds.Tables[9].TableName = "RadFnRights";
                    ds.Tables[10].TableName = "Physicians";
                    ds.Tables[11].TableName = "Radiologists";
                    ds.Tables[12].TableName = "FinalRadiologists";
                    ds.Tables[13].TableName = "UserRole";
                    ds.Tables[14].TableName = "PACSCred";
                    ds.Tables[15].TableName = "AbnormalRptReasons";
                    ds.Tables[16].TableName = "BeyondHrs";
                    ds.Tables[17].TableName = "Country";
                    ds.Tables[18].TableName = "State";
                    ds.Tables[19].TableName = "Users";
                    ds.Tables[20].TableName = "ModalityServiceAvailable";
                    ds.Tables[21].TableName = "ModalityServiceAvailableAH";
                    ds.Tables[22].TableName = "ModalityServiceAvailableExInst";
                    ds.Tables[23].TableName = "ModalityServiceAvailableAHExInst";
                    ds.Tables[24].TableName = "SpeciesServiceAvailable";
                    ds.Tables[25].TableName = "SpeciesServiceAvailableAH";
                    ds.Tables[26].TableName = "SpeciesServiceAvailableExInst";
                    ds.Tables[27].TableName = "SpeciesServiceAvailableAHExInst";
                    ds.Tables[28].TableName = "DatabaseList";

                    foreach (DataRow dr in ds.Tables["UserRole"].Rows)
                    {
                        strUserRoleCode = Convert.ToString(dr["user_role_code"]).Trim();
                    }

                    #region PACS Credentials
                    foreach (DataRow dr in ds.Tables["PACSCred"].Rows)
                    {
                        strPACSUserID = Convert.ToString(dr["pacs_user_id"]).Trim();
                        strPACSUserPwd = Convert.ToString(dr["pacs_password"]).Trim();
                        if (strPACSUserPwd.Trim() != string.Empty) strPACSUserPwd = CoreCommon.DecryptString(strPACSUserPwd);
                    }
                    #endregion

                    foreach (DataRow dr in ds.Tables["BeyondHrs"].Rows)
                    {
                        strBeyondOpHr = Convert.ToString(dr["beyond_operation_time"]).Trim();
                    }
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion


        #region UpdatePriority
        public bool UpdatePriority(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[5];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@study_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@priority_id", SqlDbType.Int); SqlRecordParams[1].Value = intPriorityID;
                SqlRecordParams[2] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.VarChar, 500); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_priority_update", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[4].Value);
                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;


                CatchMessage = Convert.ToString(SqlRecordParams[3].Value);

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region ArchiveStudy
        public bool ArchiveStudy(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[6];
            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strStudyUID;
                SqlRecordParams[2] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[3].Value = intMenuID;
                SqlRecordParams[4] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_archive_study", SqlRecordParams);
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

        #region RevertArchiveStudy
        public bool RevertArchiveStudy(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[5];
            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strStudyUID;
                SqlRecordParams[2] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_archive_ra_revert_study", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[4].Value);
                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;

                ReturnMessage = Convert.ToString(SqlRecordParams[3].Value);

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region DeleteArchiveStudy
        public bool DeleteArchiveStudy(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0;
            //int intExecReturn = 0;
            DataSet ds = new DataSet();
            string strFilePath = string.Empty;
            string strArchiveFolder = string.Empty;
            string[] arrFiles = new string[0];


            SqlParameter[] SqlRecordParams = new SqlParameter[7];
            try
            {
                SqlRecordParams[0] = new SqlParameter("@study_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strStudyUID;
                SqlRecordParams[2] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = intMenuID;
                SqlRecordParams[4] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = SessionID;
                SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_archive_study_delete", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[6].Value);
                if (intReturnValue == 1)
                {
                    bReturn = true;

                    #region delete files
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            strFilePath = Convert.ToString(dr["folder"]).Trim() + "/" + Convert.ToString(dr["file_name"]).Trim();
                            if (File.Exists(strFilePath))
                                File.Delete(strFilePath);
                        }

                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            strArchiveFolder = Convert.ToString(dr["archive_folder"]).Trim();
                            if (Directory.Exists(strArchiveFolder))
                            {
                                if (Directory.Exists(strArchiveFolder + "/images"))
                                {
                                    arrFiles = Directory.GetFiles(strArchiveFolder + "/images");
                                    for (int i = 0; i < arrFiles.Length; i++)
                                    {
                                        if (File.Exists(arrFiles[i])) File.Delete(arrFiles[i]);
                                    }
                                    Directory.Delete(strArchiveFolder + "/images");
                                }

                                arrFiles = Directory.GetFiles(strArchiveFolder);
                                for (int i = 0; i < arrFiles.Length; i++)
                                {
                                    if (File.Exists(arrFiles[i])) File.Delete(arrFiles[i]);
                                }
                                Directory.Delete(strArchiveFolder);
                            }
                        }
                    }
                    #endregion
                }
                else
                    bReturn = false;

                ReturnMessage = Convert.ToString(SqlRecordParams[5].Value);

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }
            finally
            {
                ds.Dispose();
            }



            return bReturn;
        }
        #endregion

        #region Apply Discount

        #region FetchDiscountDetails
        public bool FetchDiscountDetails(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; string strControlCode = string.Empty;

            SqlParameter[] SqlRecordParams = new SqlParameter[6];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@study_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = intMenuID;
                SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = SessionID;
                SqlRecordParams[4] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_apply_discount_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Reasons";

                    #region Details
                    if (ds.Tables.Count > 1)
                    {
                        ds.Tables[1].TableName = "Details";
                        foreach (DataRow dr in ds.Tables["Details"].Rows)
                        {

                            strStudyUID = Convert.ToString(dr["study_uid"]).Trim();
                            strInstitutionName = Convert.ToString(dr["institution_name"]).Trim();
                            strPatientName = Convert.ToString(dr["patient_name"]).Trim();
                            strDiscBy = Convert.ToString(dr["discount_type"]).Trim();
                            decDiscPer = Convert.ToDecimal(dr["discount_per"]);
                            dblDiscAmt = Convert.ToDouble(dr["discount_amount"]);
                            PromoReasonId = new Guid(Convert.ToString(dr["promo_reason_id"]).Trim());
                            strInvoiced = Convert.ToString(dr["invoiced"]).Trim();
                            dblStudyCost = Convert.ToDouble(dr["study_cost"]);
                        }
                        bReturn = true;
                    }
                    else
                    {
                        bReturn = false;
                        ReturnMessage = Convert.ToString(SqlRecordParams[3].Value);
                    }
                    #endregion
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

        #region ApplyDiscount
        public bool ApplyDiscount(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateDiscount(ref ReturnMessage))
            {

                SqlParameter[] SqlRecordParams = new SqlParameter[11];
                try
                {
                    SqlRecordParams[0] = new SqlParameter("@study_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                    SqlRecordParams[1] = new SqlParameter("@discount_type", SqlDbType.NChar,1); SqlRecordParams[1].Value = strDiscBy;
                    SqlRecordParams[2] = new SqlParameter("@discount_percent", SqlDbType.Decimal); SqlRecordParams[2].Value = decDiscPer;
                    SqlRecordParams[3] = new SqlParameter("@discount_amount", SqlDbType.Money); SqlRecordParams[3].Value = dblDiscAmt;
                    SqlRecordParams[4] = new SqlParameter("@reason_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = PromoReasonId;
                    SqlRecordParams[5] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[5].Value = intMenuID;
                    SqlRecordParams[6] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[6].Value = UserID;
                    SqlRecordParams[7] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[7].Value = SessionID;
                    SqlRecordParams[8] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[8].Direction = ParameterDirection.Output;
                    SqlRecordParams[9] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[9].Direction = ParameterDirection.Output;
                    SqlRecordParams[10] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[10].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_apply_discount", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[10].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;

                    strUserName = Convert.ToString(SqlRecordParams[8].Value).Trim();
                    ReturnMessage = Convert.ToString(SqlRecordParams[9].Value);

                }
                catch (Exception expErr)
                { bReturn = false; CatchMessage = expErr.Message; }

            }
            else
            {
                bReturn = false;
            }
            return bReturn;
        }
        #endregion

        #region RevertDiscount
        public bool RevertDiscount(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[7];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@study_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = intMenuID;
                SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = SessionID;
                SqlRecordParams[4] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_revert_discount", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[6].Value);
                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;

                strUserName = Convert.ToString(SqlRecordParams[4].Value).Trim();
                ReturnMessage = Convert.ToString(SqlRecordParams[5].Value);

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region ValidateDiscount
        private bool ValidateDiscount(ref string ReturnMessage)
        {
            bool bReturn = true;

            if (PromoReasonId == new Guid("00000000-0000-0000-0000-000000000000"))
            {

                ReturnMessage = "265";
            }

            if (strDiscBy == "P")
            {
                if (decDiscPer <= 0)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "270";
                }
                else if (decDiscPer >100)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "511";
                }
            }
            else if (strDiscBy == "A")
            {
                if (dblDiscAmt > dblStudyCost)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "510";
                }
                else if (dblDiscAmt <= 0)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "509";
                }

            }


            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }

        #endregion

        #endregion

        #region Notification Sending

        #region FetchNotificationParameters
        public bool FetchNotificationParameters(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "case_list_fetch_notification_params", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        strInstitutionName = Convert.ToString(dr["institution_name"]).Trim();
                        strInstitutionEmailID = Convert.ToString(dr["institution_email_id"]).Trim();
                        strInstitutionMobileNo = Convert.ToString(dr["institution_mobile"]).Trim();
                        strPhysicianName = Convert.ToString(dr["physician_name"]).Trim();
                        strPhysicianEmailID = Convert.ToString(dr["physician_email_id"]).Trim();
                        strPhysicianMobileNo = Convert.ToString(dr["physician_mobile"]).Trim();
                    }
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchForwardNotificationParameters
        public bool FetchForwardNotificationParameters(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            try
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[2];
                SqlRecordParams[0] = new SqlParameter("@study_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "case_list_fwd_notification_param_fetch", SqlRecordParams);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Sender";
                    ds.Tables[1].TableName = "Params";
                    ds.Tables[2].TableName = "Texts";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }
            finally { ds.Dispose(); }


            return bReturn;
        }
        #endregion

        #endregion

        #region CheckRadiologistLock
        public bool CheckRadiologistLock(string ConfigPath, ref string Refresh, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[10];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@status_id", SqlDbType.Int); SqlRecordParams[1].Value = intStatusIDPACS;
                SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[3].Value = intMenuID;
                SqlRecordParams[4] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = SessionID;
                SqlRecordParams[5] = new SqlParameter("@refresh", SqlDbType.NChar, 1); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@status_desc", SqlDbType.VarChar, 30); SqlRecordParams[6].Direction = ParameterDirection.Output;
                SqlRecordParams[7] = new SqlParameter("@user_name", SqlDbType.VarChar, 100); SqlRecordParams[7].Direction = ParameterDirection.Output;
                SqlRecordParams[8] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[8].Direction = ParameterDirection.Output;
                SqlRecordParams[9] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[9].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_brw_check_radiologist_lock", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[9].Value);
                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;

                Refresh = Convert.ToString(SqlRecordParams[5].Value);
                strStatusDescPACS = Convert.ToString(SqlRecordParams[6].Value).Trim();
                strUserName = Convert.ToString(SqlRecordParams[7].Value).Trim();
                ReturnMessage = Convert.ToString(SqlRecordParams[8].Value);

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchReportCompareDetails
        public bool FetchReportCompareDetails(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = false;
            DataSet ds = new DataSet();
            SqlParameter[] SqlRecordParams = new SqlParameter[1];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@study_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_compare_reports_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Reports";
                    foreach (DataRow dr in ds.Tables["Reports"].Rows)
                    {
                        PrelimRadiologistId = new Guid(Convert.ToString(dr["prelim_radiologist_id"]));
                        strPrelimRadAssn = Convert.ToString(dr["prelim_radiologist"]).Trim();
                        FinalRadiologistId = new Guid(Convert.ToString(dr["final_radiologist_id"]));
                        strFinalRadAssn = Convert.ToString(dr["final_radiologist"]).Trim();
                        TranscriptionistId = new Guid(Convert.ToString(dr["dict_tanscriptionist_id"]));
                        strTranscriptionist = Convert.ToString(dr["dict_tanscriptionist_name"]).Trim();
                        strCustomRpt = Convert.ToString(dr["custom_report"]).Trim();
                        strPatientName = Convert.ToString(dr["patient_name"]).Trim();
                        strAbnormalRptReason = Convert.ToString(dr["rating_reason"]).Trim();
                    }
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }
            finally
            {
                ds.Dispose();
            }

            return bReturn;
        }
        #endregion

        #region ReleaseFinalReport
        public bool ReleaseFinalReport(string ConfigPath, ref int CheckReleaseTime, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[7];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@study_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = intMenuID;
                SqlRecordParams[2] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = SessionID;
                SqlRecordParams[4] = new SqlParameter("@FNLRPTMANUALRELMIN", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_final_report_release", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[6].Value);
                if (intReturnValue == 1)
                {
                    bReturn = true;
                    CheckReleaseTime = Convert.ToInt32(SqlRecordParams[4].Value);
                }
                else
                    bReturn = false;


                ReturnMessage = Convert.ToString(SqlRecordParams[5].Value);

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region Radiologist Activity

        #region RadiologistSelfAssignmentSave
        public bool RadiologistSelfAssignmentSave(string ConfigPath, StudyUIDList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0; int intRowID = 0;
            string[] arrTempFiles = new string[0];
            string strFile = string.Empty;

            if (ValidateStudyUIDRecord(ArrObj, ref ReturnMessage))
            {
                if (GenerateStudyUIDXML(ArrObj, ref CatchMessage))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[8];
                        SqlRecordParams[0] = new SqlParameter("@radiologist_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = RadiologistId;
                        SqlRecordParams[1] = new SqlParameter("@xml_study", SqlDbType.NText); SqlRecordParams[1].Value = strXML.Trim();
                        SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = intMenuID;
                        SqlRecordParams[3] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = UserID;
                        SqlRecordParams[4] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = SessionID;
                        SqlRecordParams[5] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[5].Direction = ParameterDirection.Output;
                        SqlRecordParams[6] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[6].Direction = ParameterDirection.Output;
                        SqlRecordParams[7] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[7].Direction = ParameterDirection.Output;

                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_radiologist_self_assign", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[7].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[5].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[6].Value);
                    }
                    catch (Exception expErr)
                    { bReturn = false; CatchMessage = expErr.Message; }
                }
                else
                {
                    bReturn = false;
                    strUserName = intRowID.ToString();
                }

            }
            else
            {
                bReturn = false;
            }

            return bReturn;
        }
        #endregion

        #region ValidateStudyUIDRecord
        private bool ValidateStudyUIDRecord(StudyUIDList[] ArrObj, ref string ReturnMessage)
        {
            bool bReturn = true;
            if (ArrObj.Length == 0)
            {
                ReturnMessage = "067";
            }


            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }
        #endregion

        #region GenerateStudyUIDXML
        private bool GenerateStudyUIDXML(StudyUIDList[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<study>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<study_uid><![CDATA[" + ArrObj[i].STUDY_UID + "]]></study_uid>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</study>");
                    strXML = sbXML.ToString();


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

        #region ReleaseStudyAssignment
        public bool ReleaseStudyAssignment(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;
            string[] arrTempFiles = new string[0];
            string strFile = string.Empty;



            try
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[6];
                SqlRecordParams[0] = new SqlParameter("@study_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserID;
                SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = intMenuID;
                SqlRecordParams[3] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = SessionID;
                SqlRecordParams[4] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_radiologist_assign_decline", SqlRecordParams);
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

        #region GetCase
        public bool GetCase(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;
            string[] arrTempFiles = new string[0];
            string strFile = string.Empty;



            try
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[5];
                SqlRecordParams[0] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserID;
                SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = intMenuID;
                SqlRecordParams[2] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = SessionID;
                SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_radiologist_assignment_get", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[4].Value);
                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;
                ReturnMessage = Convert.ToString(SqlRecordParams[3].Value);
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

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
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_fetch_hdr", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";
                    ds.Tables[1].TableName = "Species";
                    ds.Tables[2].TableName = "Breed";
                    ds.Tables[3].TableName = "Modality";
                    ds.Tables[4].TableName = "Institutions";
                    ds.Tables[5].TableName = "Physicians";
                    ds.Tables[6].TableName = "DictReport";
                    ds.Tables[7].TableName = "PrelimReport";
                    ds.Tables[8].TableName = "FinalReport";
                    ds.Tables[9].TableName = "SelectedStudyTypes";
                    ds.Tables[10].TableName = "Priority";
                    ds.Tables[11].TableName = "Consult";
                    ds.Tables[12].TableName = "Category";
                    ds.Tables[13].TableName = "RadFnRights";
                    ds.Tables[14].TableName = "PACSCred";
                    ds.Tables[15].TableName = "RptDisclReasons";
                    ds.Tables[16].TableName = "MergedStudies";
                    ds.Tables[17].TableName = "AbnormalRptReasons";
                    ds.Tables[18].TableName = "Country";
                    ds.Tables[19].TableName = "State";
                    ds.Tables[20].TableName = "ModalityServiceAvailable";
                    ds.Tables[21].TableName = "ModalityServiceAvailableAH";
                    ds.Tables[22].TableName = "ModalityServiceAvailableExInst";
                    ds.Tables[23].TableName = "ModalityServiceAvailableAHExInst";
                    ds.Tables[24].TableName = "SpeciesServiceAvailable";
                    ds.Tables[25].TableName = "SpeciesServiceAvailableAH";
                    ds.Tables[26].TableName = "SpeciesServiceAvailableExInst";
                    ds.Tables[27].TableName = "SpeciesServiceAvailableAHExInst";


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
                        strInstitutionCode = Convert.ToString(dr["institution_code"]).Trim();
                        strRefPhy = Convert.ToString(dr["referring_physician_pacs"]).Trim();
                        PhysicianID = new Guid(Convert.ToString(dr["physician_id"]));
                        strPhysicianCode = Convert.ToString(dr["physician_code"]).Trim();
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
                        strInvoiceBy = Convert.ToString(dr["invoice_by"]).Trim();
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
                        strWS8IMGVWRURL = Convert.ToString(dr["WS8IMGVWRURL"]).Trim();
                        strFTPABSPATH = Convert.ToString(dr["FTPABSPATH"]).Trim();
                        strDCMMODIFYEXEPATH = Convert.ToString(dr["DCMMODIFYEXEPATH"]).Trim();
                        strPACSARCHIVEFLDR = Convert.ToString(dr["PACSARCHIVEFLDR"]).Trim();
                        strPACSARCHALTFLDR = Convert.ToString(dr["PACSARCHALTFLDR"]).Trim();
                        strVRSAPPLINK = Convert.ToString(dr["VRSAPPLINK"]).Trim();
                        strGOOGLETRANSAPILINK = Convert.ToString(dr["GOOGLETRANSAPILINK"]).Trim();
                        strGOOGLETRANSAPIKEY = Convert.ToString(dr["GOOGLETRANSAPIKEY"]).Trim();
                        RadiologistId = new Guid(Convert.ToString(dr["reading_radiologist_id"]));
                        strRadiologistName = Convert.ToString(dr["reading_radiologist_name"]).Trim();
                        PrelimRadiologistId = new Guid(Convert.ToString(dr["prelim_radiologist_id"]));
                        strPrelimRadAssn = Convert.ToString(dr["preliminary_radiologist"]).Trim();
                        FinalRadiologistId = new Guid(Convert.ToString(dr["final_radiologist_id"]));
                        strFinalRadAssn = Convert.ToString(dr["final_radiologist"]).Trim();
                        TranscriptionistId = new Guid(Convert.ToString(dr["dict_tanscriptionist_id"]));
                        strCustomRpt = Convert.ToString(dr["custom_report"]).Trim();
                        intStudyToMerge = Convert.ToInt32(dr["study_to_merge"]);
                        strBeyondOpHr = Convert.ToString(dr["beyond_operation_time"]).Trim();
                        intCountryID = Convert.ToInt32(dr["patient_country_id"]);
                        strCountryName = Convert.ToString(dr["patient_country_name"]).Trim();
                        intStateID = Convert.ToInt32(dr["patient_state_id"]);
                        strStateName = Convert.ToString(dr["patient_state_name"]).Trim();
                        strCity = Convert.ToString(dr["patient_city"]).Trim();
                        strMarkToTeach = Convert.ToString(dr["mark_to_teach"]).Trim();
                        strSyncMode = Convert.ToString(dr["sync_mode"]).Trim();
                    }


                    #endregion

                    #region Dictated Report
                    foreach (DataRow dr in ds.Tables["DictReport"].Rows)
                    {
                        strDictRpt = Convert.ToString(dr["report_text"]).Trim();
                        strDictRptHTML = Convert.ToString(dr["report_text_html"]).Trim();
                        strTransRpt = Convert.ToString(dr["trans_report_text"]).Trim();
                        strTransRptHTML = Convert.ToString(dr["trans_report_text_html"]).Trim();
                        strTranslateRpt = Convert.ToString(dr["translate_report_text"]).Trim();
                        strTranslateRptHTML = Convert.ToString(dr["translate_report_text_html"]).Trim();
                        strRptRate = Convert.ToString(dr["rating"]).Trim();
                        AbnormalRptReasonID = new Guid(Convert.ToString(dr["rating_reason_id"]).Trim());
                        intRptDisclReasonID = Convert.ToInt32(dr["disclaimer_reason_id"]);
                        strRptDisclReasonType = Convert.ToString(dr["disclaimer_reason"]).Trim();
                        strRptDisclReasonDesc = Convert.ToString(dr["disclaimer_desc"]).Trim();

                    }
                    #endregion

                    #region Preliminary Report
                    foreach (DataRow dr in ds.Tables["PrelimReport"].Rows)
                    {
                        strPrelimRpt = Convert.ToString(dr["report_text"]).Trim();
                        strPrelimRptHTML = Convert.ToString(dr["report_text_html"]).Trim();
                        strRptRate = Convert.ToString(dr["rating"]).Trim();
                        AbnormalRptReasonID = new Guid(Convert.ToString(dr["rating_reason_id"]).Trim());
                        intRptDisclReasonID = Convert.ToInt32(dr["disclaimer_reason_id"]);
                        strRptDisclReasonType = Convert.ToString(dr["disclaimer_reason"]).Trim();
                        strRptDisclReasonDesc = Convert.ToString(dr["disclaimer_desc"]).Trim();
                    }
                    #endregion

                    #region Final Report
                    foreach (DataRow dr in ds.Tables["FinalReport"].Rows)
                    {
                        strFinalRpt = Convert.ToString(dr["report_text"]).Trim();
                        strFinalRptHTML = Convert.ToString(dr["report_text_html"]).Trim();
                        AbnormalRptReasonID = new Guid(Convert.ToString(dr["rating_reason_id"]).Trim());
                        intRptDisclReasonID = Convert.ToInt32(dr["disclaimer_reason_id"]);
                        strRptDisclReasonType = Convert.ToString(dr["disclaimer_reason"]).Trim();
                        strRptDisclReasonDesc = Convert.ToString(dr["disclaimer_desc"]).Trim();
                    }
                    #endregion

                    #region Consult
                    foreach (DataRow dr in ds.Tables["Consult"].Rows)
                    {
                        strInstConsultAppl = Convert.ToString(dr["institution_consult_applicable"]).Trim();
                    }
                    #endregion

                    #region PACS Credentials
                    foreach (DataRow dr in ds.Tables["PACSCred"].Rows)
                    {
                        strPACSUserID = Convert.ToString(dr["pacs_user_id"]).Trim();
                        strPACSUserPwd = Convert.ToString(dr["pacs_password"]).Trim();
                        if (strPACSUserPwd.Trim() != string.Empty) strPACSUserPwd = CoreCommon.DecryptString(strPACSUserPwd);
                    }
                    #endregion

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

        #region LoadArchiveHeader
        public bool LoadArchiveHeader(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
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
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_fetch_archive_hdr", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Controls";
                    ds.Tables[1].TableName = "Species";
                    ds.Tables[2].TableName = "Modality";
                    ds.Tables[3].TableName = "Priority";
                    ds.Tables[4].TableName = "Category";
                    ds.Tables[5].TableName = "Institutions";
                    ds.Tables[6].TableName = "RadFnRights";
                    ds.Tables[7].TableName = "PACSCred";
                    ds.Tables[8].TableName = "RptDisclReasons";
                    ds.Tables[9].TableName = "Details";
                    ds.Tables[10].TableName = "Physicians";
                    ds.Tables[11].TableName = "Breed";
                    ds.Tables[12].TableName = "DictReport";
                    ds.Tables[13].TableName = "PrelimReport";
                    ds.Tables[14].TableName = "FinalReport";
                    ds.Tables[15].TableName = "SelectedStudyTypes";

                    #region Controls
                    foreach (DataRow dr in ds.Tables["Controls"].Rows)
                    {

                        strPACSURL = Convert.ToString(dr["pacs_url"]).Trim();
                        strIMGVWRURL = Convert.ToString(dr["image_viewer_url"]).Trim();
                        strPACIMGCNTURL = Convert.ToString(dr["pacs_img_count_url"]).Trim();
                        strPACSTUDYDELURL = Convert.ToString(dr["pacs_study_del_url"]).Trim();
                        strWS8SRVIP = Convert.ToString(dr["WS8SRVIP"]).Trim();
                        strWS8CLTIP = Convert.ToString(dr["WS8CLTIP"]).Trim();
                        strWS8SRVUID = Convert.ToString(dr["WS8SRVUID"]).Trim();
                        strWS8SRVPWD = Convert.ToString(dr["WS8SRVPWD"]).Trim();
                        strAPIVER = Convert.ToString(dr["APIVER"]).Trim();
                        strWS8SYVWRURL = Convert.ToString(dr["WS8SYVWRURL"]).Trim();
                        strWS8IMGVWRURL = Convert.ToString(dr["WS8IMGVWRURL"]).Trim();
                        strFTPABSPATH = Convert.ToString(dr["FTPABSPATH"]).Trim();
                        strDCMMODIFYEXEPATH = Convert.ToString(dr["DCMMODIFYEXEPATH"]).Trim();
                        strPACSARCHIVEFLDR = Convert.ToString(dr["PACSARCHIVEFLDR"]).Trim();
                        strPACSARCHALTFLDR = Convert.ToString(dr["PACSARCHALTFLDR"]).Trim();
                        strVRSAPPLINK = Convert.ToString(dr["VRSAPPLINK"]).Trim();
                    }
                    #endregion

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
                        //strBodyPart = Convert.ToString(dr["body_part_pacs"]).Trim();
                        //intBodyPartID = Convert.ToInt32(dr["body_part_id"]);
                        //strBodyPartName = Convert.ToString(dr["body_part_name"]).Trim();
                        strInstitutionPACS = Convert.ToString(dr["institution_name_pacs"]).Trim();
                        InstitutionID = new Guid(Convert.ToString(dr["institution_id"]));
                        strInstitutionName = Convert.ToString(dr["institution_name"]).Trim();
                        strInstitutionCode = Convert.ToString(dr["institution_code"]).Trim();
                        strRefPhy = Convert.ToString(dr["referring_physician_pacs"]).Trim();
                        PhysicianID = new Guid(Convert.ToString(dr["physician_id"]));
                        strPhysicianName = Convert.ToString(dr["physician_name"]).Trim();
                        strReasonPACS = Convert.ToString(dr["reason_pacs"]).Trim();
                        strReason = Convert.ToString(dr["reason_accepted"]).Trim();
                        intImgCntPACS = Convert.ToInt32(dr["img_count_pacs"]);
                        intImgCnt = Convert.ToInt32(dr["img_count"]);
                        intObjCnt = Convert.ToInt32(dr["object_count"]);
                        strImgCntAccepted = Convert.ToString(dr["img_count_accepted"]).Trim();

                        intPriorityID = Convert.ToInt32(dr["priority_id"]);
                        strPriorityDesc = Convert.ToString(dr["priority_desc"]).Trim();
                        strTrackBy = Convert.ToString(dr["track_by"]).Trim();
                        strInvoiceBy = Convert.ToString(dr["invoice_by"]).Trim();
                        //strRecViaDR = Convert.ToString(dr["received_via_dicom_router"]).Trim();
                        strPhysNote = Convert.ToString(dr["physician_note"]).Trim();
                        strConsultApplied = Convert.ToString(dr["consult_applied"]).Trim();
                        intCategoryID = Convert.ToInt32(dr["category_id"]);
                        strCategoryName = Convert.ToString(dr["category_name"]).Trim();
                        strServiceCodes = Convert.ToString(dr["service_codes"]).Trim();
                        intStatusIDPACS = Convert.ToInt32(dr["study_status_pacs"]);
                        strStatusDescPACS = Convert.ToString(dr["status_desc"]).Trim();

                        PrelimRadiologistId = new Guid(Convert.ToString(dr["prelim_radiologist_id"]));
                        strPrelimRadAssn = Convert.ToString(dr["preliminary_radiologist"]).Trim();
                        FinalRadiologistId = new Guid(Convert.ToString(dr["final_radiologist_id"]));
                        strFinalRadAssn = Convert.ToString(dr["final_radiologist"]).Trim();
                        TranscriptionistId = new Guid(Convert.ToString(dr["dict_tanscriptionist_id"]));
                        strCustomRpt = Convert.ToString(dr["custom_report"]).Trim();
                        intCountryID = Convert.ToInt32(dr["patient_country_id"]);
                        strCountryName = Convert.ToString(dr["patient_country_name"]).Trim();
                        intStateID = Convert.ToInt32(dr["patient_state_id"]);
                        strStateName = Convert.ToString(dr["patient_state_name"]).Trim();
                        strCity = Convert.ToString(dr["patient_city"]).Trim();
                        strMarkToTeach = Convert.ToString(dr["mark_to_teach"]).Trim();
                        strInstConsultAppl = Convert.ToString(dr["institution_consult_applicable"]).Trim();
                        strSyncMode = Convert.ToString(dr["sync_mode"]).Trim();
                        strTrackBy = Convert.ToString(dr["track_by"]).Trim();
                    }


                    #endregion

                    #region Dictated Report
                    foreach (DataRow dr in ds.Tables["DictReport"].Rows)
                    {
                        strDictRpt = Convert.ToString(dr["report_text"]).Trim();
                        strDictRptHTML = Convert.ToString(dr["report_text_html"]).Trim();
                        strTransRpt = Convert.ToString(dr["trans_report_text"]).Trim();
                        strTransRptHTML = Convert.ToString(dr["trans_report_text_html"]).Trim();
                        strTranslateRpt = Convert.ToString(dr["translate_report_text"]).Trim();
                        strTranslateRptHTML = Convert.ToString(dr["translate_report_text_html"]).Trim();
                        strRptRate = Convert.ToString(dr["rating"]).Trim();
                        intRptDisclReasonID = Convert.ToInt32(dr["disclaimer_reason_id"]);
                        strRptDisclReasonType = Convert.ToString(dr["disclaimer_reason"]).Trim();
                        strRptDisclReasonDesc = Convert.ToString(dr["disclaimer_desc"]).Trim();
                    }
                    #endregion

                    #region Preliminary Report
                    foreach (DataRow dr in ds.Tables["PrelimReport"].Rows)
                    {
                        strPrelimRpt = Convert.ToString(dr["report_text"]).Trim();
                        strPrelimRptHTML = Convert.ToString(dr["report_text_html"]).Trim();
                        strRptRate = Convert.ToString(dr["rating"]).Trim();
                        intRptDisclReasonID = Convert.ToInt32(dr["disclaimer_reason_id"]);
                        strRptDisclReasonType = Convert.ToString(dr["disclaimer_reason"]).Trim();
                        strRptDisclReasonDesc = Convert.ToString(dr["disclaimer_desc"]).Trim();
                    }
                    #endregion

                    #region Final Report
                    foreach (DataRow dr in ds.Tables["FinalReport"].Rows)
                    {
                        strFinalRpt = Convert.ToString(dr["report_text"]).Trim();
                        strFinalRptHTML = Convert.ToString(dr["report_text_html"]).Trim();
                        intRptDisclReasonID = Convert.ToInt32(dr["disclaimer_reason_id"]);
                        strRptDisclReasonType = Convert.ToString(dr["disclaimer_reason"]).Trim();
                        strRptDisclReasonDesc = Convert.ToString(dr["disclaimer_desc"]).Trim();
                    }
                    #endregion

                    #region PACS Credentials
                    foreach (DataRow dr in ds.Tables["PACSCred"].Rows)
                    {
                        strPACSUserID = Convert.ToString(dr["pacs_user_id"]).Trim();
                        strPACSUserPwd = Convert.ToString(dr["pacs_password"]).Trim();
                        if (strPACSUserPwd.Trim() != string.Empty) strPACSUserPwd = CoreCommon.DecryptString(strPACSUserPwd);
                    }
                    #endregion

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

        #region LoadHeaderDCMFiles
        public bool LoadHeaderDCMFiles(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "case_list_fetch_dcm_files", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "DCM";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadStudyFiles
        public bool LoadStudyFiles(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "case_list_fetch_study_files", SqlRecordParams);
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

        #region LoadStudyToMerge
        public bool LoadStudyToMerge(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
            SqlRecordParams[1] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strStudyUID;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "case_list_studies_to_merge_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Study";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadReportAddendums
        public bool LoadReportAddendums(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "case_list_fetch_report_addendums", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Addendums";
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
            SqlParameter[] SqlRecordParams = new SqlParameter[3];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
            SqlRecordParams[1] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[1].Value = intModalityID;
            SqlRecordParams[2] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = InstitutionID;

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

        #region LoadPreviousStudyList
        public bool LoadPreviousStudyList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false; string strControlCode = string.Empty;

            SqlParameter[] SqlRecordParams = new SqlParameter[4];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@patient_name", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strPatientName;
                SqlRecordParams[2] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = InstitutionID;
                SqlRecordParams[3] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = UserID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_patient_previous_studies_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "StudyList";
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

        #region FetchRegInstitutionDetails
        public bool FetchRegInstitutionDetails(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@reg_institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = InstitutionID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "reg_institution_wise_physician_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Physicians";
                    ds.Tables[1].TableName = "Institutions_Reg";
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

        #region FetchModalityWiseStudyTypes
        public bool FetchModalityWiseStudyTypes(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[0].Value = intModalityID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "modality_wise_study_type_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "StudyTypes";
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

        #region FetchTypeWiseReportDisclaimerReasonDescription
        public bool FetchTypeWiseReportDisclaimerReasonDescription(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            DataSet ds = new DataSet();

            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.Int); SqlRecordParams[0].Value = intRptDisclReasonID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "type_wise_rpt_disclaimer_desc_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        strRptDisclReasonDesc = Convert.ToString(dr["description"]).Trim();
                    }

                }

                bReturn = true;

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }
            finally
            {
                ds.Dispose();
            }

            return bReturn;
        }
        #endregion

        #region FetchImageCount
        public bool FetchImageCount(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = false;
            DataSet ds = new DataSet();

            SqlParameter[] SqlRecordParams = new SqlParameter[1];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_image_count_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        intImgCnt = Convert.ToInt32(dr["img_count"]);
                        intObjCnt = Convert.ToInt32(dr["object_count"]);
                    }

                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                }

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }
            finally { ds.Dispose(); }

            return bReturn;
        }
        #endregion

        #region CheckArchivedFiles
        public bool CheckArchivedFiles(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[5];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@actual_archived_file_count", SqlDbType.Int); SqlRecordParams[1].Value = intActualFileCount;
                SqlRecordParams[2] = new SqlParameter("@pending_file_count", SqlDbType.Int); SqlRecordParams[2].Direction = ParameterDirection.Output;
                SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_study_file_count_check", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[4].Value);
                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;
                ReturnMessage = Convert.ToString(SqlRecordParams[3].Value);
                intPendingFileCount = Convert.ToInt32(SqlRecordParams[2].Value);
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

        #region UpdateImageObjectCount
        public bool UpdateImageObjectCount(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0;
            int intExecReturn = 0;



            SqlParameter[] SqlRecordParams = new SqlParameter[8];
            try
            {
                SqlRecordParams[0] = new SqlParameter("id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@img_count", SqlDbType.Int); SqlRecordParams[1].Value = intImgCnt;
                SqlRecordParams[2] = new SqlParameter("@object_count", SqlDbType.Int); SqlRecordParams[2].Value = intObjCnt;
                SqlRecordParams[3] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = UserID;
                SqlRecordParams[4] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[4].Value = intMenuID;
                SqlRecordParams[5] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[5].Value = SessionID;
                SqlRecordParams[6] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[6].Direction = ParameterDirection.Output;
                SqlRecordParams[7] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[7].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_img_obj_count_update", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[7].Value);
                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;

                ReturnMessage = Convert.ToString(SqlRecordParams[6].Value);

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }
            

            return bReturn;
        }
        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, StudyTypeList[] ArrobjST, HeaderDocumentList[] ArrObjDocs, HeaderDICOMList[] ArrObjDCM, StudyToMerge[] ArrObjMerge, ref string AccnNoGenerated, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            DataTable dtblST = new DataTable();
            DataTable dtblDoc = new DataTable();
            DataTable dtblDCM = new DataTable();
            DataTable dtblMerge = new DataTable();

            if (ValidateRecord(ArrobjST, ref ReturnMessage))
            {
                if ((GenerateStudyTypeTable(ArrobjST, ref dtblST, ref CatchMessage)) && (GenerateDocumentTable(ArrObjDocs, ref dtblDoc, ref CatchMessage)) && (GenerateDCMTable(ArrObjDCM, ref dtblDCM, ref CatchMessage)) && (GenerateMergeTable(ArrObjMerge, ref dtblMerge, ref CatchMessage)))
                {
                    SqlParameter[] SqlRecordParams = new SqlParameter[48];
                    try
                    {
                        SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                        SqlRecordParams[1] = new SqlParameter("@patient_id", SqlDbType.NVarChar, 20); SqlRecordParams[1].Value = strPatientID;
                        SqlRecordParams[2] = new SqlParameter("@patient_fname", SqlDbType.NVarChar, 40); SqlRecordParams[2].Value = strPatientFName;
                        SqlRecordParams[3] = new SqlParameter("@patient_lname", SqlDbType.NVarChar, 40); SqlRecordParams[3].Value = strPatientLName;
                        SqlRecordParams[4] = new SqlParameter("@patient_weight", SqlDbType.Decimal); SqlRecordParams[4].Value = decPatientWt;
                        SqlRecordParams[5] = new SqlParameter("@patient_dob_accepted", SqlDbType.DateTime); SqlRecordParams[5].Value = dtPatientDob;
                        SqlRecordParams[6] = new SqlParameter("@patient_age_accepted", SqlDbType.NVarChar, 50); SqlRecordParams[6].Value = strPatientAge;
                        SqlRecordParams[7] = new SqlParameter("@patient_sex", SqlDbType.NVarChar, 10); SqlRecordParams[7].Value = strPatientGender;
                        SqlRecordParams[8] = new SqlParameter("@patient_sex_neutered", SqlDbType.NVarChar, 100); SqlRecordParams[8].Value = strSexNeutered;
                        SqlRecordParams[9] = new SqlParameter("@species_id", SqlDbType.Int); SqlRecordParams[9].Value = intSpeciesID;
                        SqlRecordParams[10] = new SqlParameter("@breed_id", SqlDbType.UniqueIdentifier); SqlRecordParams[10].Value = BreedID;
                        SqlRecordParams[11] = new SqlParameter("@owner_first_name", SqlDbType.NVarChar, 100); SqlRecordParams[11].Value = strOwnerFN;
                        SqlRecordParams[12] = new SqlParameter("@owner_last_name", SqlDbType.NVarChar, 100); SqlRecordParams[12].Value = strOwnerLN;
                        SqlRecordParams[13] = new SqlParameter("@accession_no", SqlDbType.NVarChar, 20); SqlRecordParams[13].Value = strAccnNo; SqlRecordParams[13].Direction = ParameterDirection.InputOutput;
                        SqlRecordParams[14] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[14].Value = intModalityID;
                        SqlRecordParams[15] = new SqlParameter("@reason_accepted", SqlDbType.NVarChar, 2000); SqlRecordParams[15].Value = strReason;
                        SqlRecordParams[16] = new SqlParameter("@img_count", SqlDbType.Int); SqlRecordParams[16].Value = intImgCnt;
                        SqlRecordParams[17] = new SqlParameter("@img_count_accepted", SqlDbType.NChar, 1); SqlRecordParams[17].Value = strImgCntAccepted;
                        SqlRecordParams[18] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[18].Value = InstitutionID;
                        SqlRecordParams[19] = new SqlParameter("@physician_id", SqlDbType.UniqueIdentifier); SqlRecordParams[19].Value = PhysicianID;
                        SqlRecordParams[20] = new SqlParameter("@TVP_studytypes", SqlDbType.Structured); SqlRecordParams[20].Value = dtblST;
                        SqlRecordParams[21] = new SqlParameter("@TVP_docs", SqlDbType.Structured); SqlRecordParams[21].Value = dtblDoc;
                        SqlRecordParams[22] = new SqlParameter("@TVP_dcm", SqlDbType.Structured); SqlRecordParams[22].Value = dtblDCM;
                        SqlRecordParams[23] = new SqlParameter("@TVP_merged", SqlDbType.Structured); SqlRecordParams[23].Value = dtblMerge;
                        SqlRecordParams[24] = new SqlParameter("@pacs_wb", SqlDbType.NChar, 1); SqlRecordParams[24].Value = strWriteBack;
                        SqlRecordParams[25] = new SqlParameter("@wt_uom", SqlDbType.NVarChar, 5); SqlRecordParams[25].Value = strWtUOM;
                        SqlRecordParams[26] = new SqlParameter("@priority_id", SqlDbType.Int); SqlRecordParams[26].Value = intPriorityID;
                        SqlRecordParams[27] = new SqlParameter("@object_count", SqlDbType.Int); SqlRecordParams[27].Value = intObjCnt;
                        SqlRecordParams[28] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[28].Value = UserID;
                        SqlRecordParams[29] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[29].Value = intMenuID;
                        SqlRecordParams[30] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[30].Value = SessionID;
                        SqlRecordParams[31] = new SqlParameter("@accn_no_generated", SqlDbType.NChar, 1); SqlRecordParams[31].Direction = ParameterDirection.Output;
                        SqlRecordParams[32] = new SqlParameter("@studies_merged", SqlDbType.NChar, 1); SqlRecordParams[32].Value = strMergeStat;
                        SqlRecordParams[33] = new SqlParameter("@physician_note", SqlDbType.NVarChar, 2000); SqlRecordParams[33].Value = strPhysNote;
                        SqlRecordParams[34] = new SqlParameter("@consult_applied", SqlDbType.NChar, 1); SqlRecordParams[34].Value = strConsultApplied;
                        SqlRecordParams[35] = new SqlParameter("@category_id", SqlDbType.Int); SqlRecordParams[35].Value = intCategoryID;
                        SqlRecordParams[36] = new SqlParameter("@sender_time_offset_mins", SqlDbType.Int); SqlRecordParams[36].Value = intSenderTZOffsetMins;
                        SqlRecordParams[37] = new SqlParameter("@submit_priority", SqlDbType.NChar, 1); SqlRecordParams[37].Value = strSubmitPriority;
                        SqlRecordParams[38] = new SqlParameter("@patient_country_id", SqlDbType.Int); SqlRecordParams[38].Value = intCountryID;
                        SqlRecordParams[39] = new SqlParameter("@patient_state_id", SqlDbType.Int); SqlRecordParams[39].Value = intStateID;
                        SqlRecordParams[40] = new SqlParameter("@patient_city", SqlDbType.NVarChar, 100); SqlRecordParams[40].Value = strCity;
                        SqlRecordParams[41] = new SqlParameter("@delv_time", SqlDbType.NVarChar, 130); SqlRecordParams[41].Direction = ParameterDirection.Output;
                        SqlRecordParams[42] = new SqlParameter("@message_display", SqlDbType.NVarChar, 500); SqlRecordParams[42].Direction = ParameterDirection.Output;
                        SqlRecordParams[43] = new SqlParameter("@institution_name", SqlDbType.NVarChar, 100); SqlRecordParams[43].Direction = ParameterDirection.Output;
                        SqlRecordParams[44] = new SqlParameter("@institution_code", SqlDbType.NVarChar, 5); SqlRecordParams[44].Direction = ParameterDirection.Output;
                        SqlRecordParams[45] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[45].Direction = ParameterDirection.Output;
                        SqlRecordParams[46] = new SqlParameter("@error_code", SqlDbType.VarChar, 500); SqlRecordParams[46].Direction = ParameterDirection.Output;
                        SqlRecordParams[47] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[47].Direction = ParameterDirection.Output;

                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_req_action_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[47].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strAccnNo = Convert.ToString(SqlRecordParams[13].Value).Trim();
                        AccnNoGenerated = Convert.ToString(SqlRecordParams[31].Value).Trim();
                        strDelvTime = Convert.ToString(SqlRecordParams[41].Value).Trim();
                        strMsgDisp = Convert.ToString(SqlRecordParams[42].Value).Trim().Replace("\n", "<br/>");
                        strInstitutionName = Convert.ToString(SqlRecordParams[43].Value).Trim();
                        strInstitutionCode = Convert.ToString(SqlRecordParams[44].Value).Trim();
                        strUserName = Convert.ToString(SqlRecordParams[45].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[46].Value);

                        //if (bReturn)
                        //{
                        //if ((strWriteBack == "Y") && (strMergeStat == "M")) { }
                        //if ((strWriteBack == "Y") && (dtblDCM.Rows.Count > 0))
                        //{
                        //    bReturn = UploadDICOMFiles(dtblDCM, ref CatchMessage);
                        //}
                        //}

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

        #region UploadDICOMFiles
        private bool UploadDICOMFiles(DataTable dtblDCM, ref string CatchMessage)
        {
            bool bReturn = false;
            string strFile = string.Empty;
            string strFilePath = string.Empty;
            string strExtn = string.Empty;
            string strSID = string.Empty;
            string strZipPath = string.Empty;
            string strTargetPath = string.Empty;

            try
            {
                strSID = "S1D" + DateTime.Now.ToString("MMddyyHHmmss") + CoreCommon.RandomString(3);
                foreach (DataRow dr in dtblDCM.Rows)
                {
                    strFile = Convert.ToString(dr["dcm_file_name"]).Trim();
                    strFile = strInstitutionCode + "_" + strSID + "_" + strInstitutionName.Replace(" ", "_") + "_" + strFile.Replace(strSID + "_", "");
                    strFilePath = strSourcePath + "/" + Convert.ToString(dr["dcm_file_name"]).Trim();
                    strExtn = Path.GetExtension(strFilePath);

                    #region Compress FIle
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
                    #endregion

                    strTargetPath = strFTPABSPATH + "\\" + strFile + ".zip";
                    if (File.Exists(strTargetPath)) File.Delete(strTargetPath);
                    File.Move(strZipPath, strTargetPath);

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

        #region ValidateRecord
        private bool ValidateRecord(StudyTypeList[] arrObjST, ref string ReturnMessage)
        {
            bool bReturn = true;

            if (dtPatientDob.Year != 1900)
            {
                if (dtPatientDob > dtStudyDate)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage = "038";
                }
                if (dtPatientDob >= DateTime.Today)
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

            if (strReason.Trim().Length > 2000)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "085";
            }

            if (strPhysNote.Trim().Length > 2000)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "288";
            }

            if (intPriorityID == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "102";
            }

            if (InstitutionID == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "055";
            }

            if (strWriteBack.Trim() == "Y")
            {
                //if (strPatientID.Trim() == string.Empty)
                //{
                //    ReturnMessage = "048";
                //}
                if (strPatientName.Trim() == string.Empty)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage = "049";
                }
                if (strPatientGender.Trim() == string.Empty)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage = "050";
                }
                if (strSexNeutered.Trim() == string.Empty)
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
                //if ((strOwnerFN.Trim() + " " + strOwnerLN.Trim()).Trim() == string.Empty)
                //{
                //    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                //    ReturnMessage += "041";
                //}
                //if (strAccnNo.Trim() == string.Empty)
                //{
                //    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                //    ReturnMessage = "053";
                //}

                if (intModalityID == 0)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "042";
                }
                if (intCategoryID == 0)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "293";
                }

                if (strImgCntAccepted.Trim() == "N")
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "054";
                }

                if (strReason.Trim() == string.Empty)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "077";
                }


                if (PhysicianID == new Guid("00000000-0000-0000-0000-000000000000"))
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "056";
                }
                if (arrObjST.Length == 0)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "063";
                }
                else if (arrObjST.Length > 4)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "064";
                }


            }

            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;
            else
            {
                if (strCheckDOB == "Y")
                {
                    if (strWriteBack.Trim() == "Y")
                    {
                        if (((dtStudyDate.Subtract(dtPatientDob)).Days) / (365.25 / 12) <= 1)
                        {
                            ReturnMessage += "488";
                            bReturn = false;
                        }
                    }
                }
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
        private bool GenerateDocumentTable(HeaderDocumentList[] ArrObj, ref DataTable dtblDoc, ref string CatchMessage)
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

        #region GenerateDCMTable
        private bool GenerateDCMTable(HeaderDICOMList[] ArrObj, ref DataTable dtblDCM, ref string CatchMessage)
        {
            bool bReturn = false;

            dtblDCM.Columns.Add("dcm_file_id", System.Type.GetType("System.Guid"));
            dtblDCM.Columns.Add("dcm_file_srl_no", System.Type.GetType("System.Int32"));
            dtblDCM.Columns.Add("dcm_file_name", System.Type.GetType("System.String"));
            dtblDCM.Columns.Add("dcm_file", System.Type.GetType("System.Byte[]"));


            try
            {
                if (ArrObj.Length > 0)
                {
                    for (int i = 0; i < ArrObj.Length; i++)
                    {
                        DataRow dr = dtblDCM.NewRow();
                        dr["dcm_file_id"] = ArrObj[i].ID;
                        dr["dcm_file_srl_no"] = ArrObj[i].SERIAL_NUMBER;
                        dr["dcm_file_name"] = ArrObj[i].FILE_NAME;
                        dr["dcm_file"] = ArrObj[i].FILE_CONTENT;
                        dtblDCM.Rows.Add(dr);
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

        #region GenerateMergeTable
        private bool GenerateMergeTable(StudyToMerge[] ArrObj, ref DataTable dtblMerge, ref string CatchMessage)
        {
            bool bReturn = false;

            dtblMerge.Columns.Add("srl_no", System.Type.GetType("System.Int32"));
            dtblMerge.Columns.Add("study_hdr_id", System.Type.GetType("System.Guid"));
            dtblMerge.Columns.Add("study_uid", System.Type.GetType("System.String"));
            dtblMerge.Columns.Add("merge_compare_none", System.Type.GetType("System.String"));
            dtblMerge.Columns.Add("image_count", System.Type.GetType("System.Int32"));

            try
            {
                if (ArrObj.Length > 0)
                {
                    for (int i = 0; i < ArrObj.Length; i++)
                    {
                        DataRow dr = dtblMerge.NewRow();
                        dr["srl_no"] = i + 1;
                        dr["study_hdr_id"] = ArrObj[i].ID;
                        dr["study_uid"] = ArrObj[i].STUDY_UID;
                        dr["image_count"] = Convert.ToInt32(ArrObj[i].IMAGE_COUNT);
                        dr["merge_compare_none"] = ArrObj[i].MERGE_STATUS;
                        dtblMerge.Rows.Add(dr);
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

        #region DeleteStudy
        public bool DeleteStudy(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0;
            //int intExecReturn = 0;
            DataSet ds = new DataSet();
            string strFilePath = string.Empty;
            string strArchiveFolder = string.Empty;
            string[] arrFiles = new string[0];


            SqlParameter[] SqlRecordParams = new SqlParameter[7];
            try
            {
                SqlRecordParams[0] = new SqlParameter("@study_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strStudyUID;
                SqlRecordParams[2] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[3].Value = intMenuID;
                SqlRecordParams[4] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_mark_delete", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[6].Value);
                if (intReturnValue == 1)
                {
                    bReturn = true;

                    #region Delete Files
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            strFilePath = Convert.ToString(dr["folder"]).Trim() + "/" + Convert.ToString(dr["file_name"]).Trim();
                            if (File.Exists(strFilePath))
                                File.Delete(strFilePath);
                        }

                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            strArchiveFolder = Convert.ToString(dr["archive_folder"]).Trim();
                            if (Directory.Exists(strArchiveFolder))
                            {
                                arrFiles = Directory.GetFiles(strArchiveFolder);
                                for (int i = 0; i < arrFiles.Length; i++)
                                {
                                    if (File.Exists(arrFiles[i])) File.Delete(arrFiles[i]);
                                }
                                Directory.Delete(strArchiveFolder);
                            }
                        }
                    }
                    #endregion
                }
                else
                    bReturn = false;


                strUserName = Convert.ToString(SqlRecordParams[4].Value).Trim();
                ReturnMessage = Convert.ToString(SqlRecordParams[5].Value);

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }
            finally
            {
                ds.Dispose();
            }



            return bReturn;
        }
        #endregion

        #region SavePreliminaryReport
        public bool SavePreliminaryReport(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;


            if (ValidateReport(ref ReturnMessage))
            {

                SqlParameter[] SqlRecordParams = new SqlParameter[8];
                try
                {
                    SqlRecordParams[0] = new SqlParameter("@study_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                    SqlRecordParams[1] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strStudyUID;
                    SqlRecordParams[2] = new SqlParameter("@report_text", SqlDbType.NText); SqlRecordParams[2].Value = strPrelimRpt;
                    SqlRecordParams[3] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = UserID;
                    SqlRecordParams[4] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[4].Value = intMenuID;
                    SqlRecordParams[5] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[5].Direction = ParameterDirection.Output;
                    SqlRecordParams[6] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[6].Direction = ParameterDirection.Output;
                    SqlRecordParams[7] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[7].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_preliminary_report_save", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[7].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;


                    strUserName = Convert.ToString(SqlRecordParams[5].Value).Trim();
                    ReturnMessage = Convert.ToString(SqlRecordParams[6].Value);

                }
                catch (Exception expErr)
                { bReturn = false; CatchMessage = expErr.Message; }

            }
            else
            {
                bReturn = false;
            }
            return bReturn;
        }
        #endregion

        #region FetchReportViewDetails
        public bool FetchReportViewDetails(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "report_view_details_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";
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

        #region SaveReport
        public bool SaveReport(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (strRptHtml.Trim() == string.Empty) strRptHtml = TextToHtml(strRptText);
            if (strRptText.Trim() == string.Empty) strRptText = HtmlToPlainText(strRptHtml);

            if (ValidateReport(ref ReturnMessage))
            {

                SqlParameter[] SqlRecordParams = new SqlParameter[17];
                try
                {
                    SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = RptId; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
                    SqlRecordParams[1] = new SqlParameter("@study_hdr_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = Id;
                    SqlRecordParams[2] = new SqlParameter("@report_text", SqlDbType.NText); SqlRecordParams[2].Value = strRptText;
                    SqlRecordParams[3] = new SqlParameter("@report_text_html", SqlDbType.NText); SqlRecordParams[3].Value = strRptHtml;
                    SqlRecordParams[4] = new SqlParameter("@disclaimer_reason_id", SqlDbType.Int); SqlRecordParams[4].Value = intRptDisclReasonID;
                    SqlRecordParams[5] = new SqlParameter("@rating_reason_id", SqlDbType.UniqueIdentifier); SqlRecordParams[5].Value = AbnormalRptReasonID;
                    SqlRecordParams[6] = new SqlParameter("@status_id", SqlDbType.Int); SqlRecordParams[6].Value = intStatusIDPACS;
                    SqlRecordParams[7] = new SqlParameter("@write_back", SqlDbType.NChar, 1); SqlRecordParams[7].Value = strWriteBack;
                    SqlRecordParams[8] = new SqlParameter("@rating", SqlDbType.NChar, 1); SqlRecordParams[8].Value = strRptRate;
                    SqlRecordParams[9] = new SqlParameter("@mark_to_teach", SqlDbType.NChar, 1); SqlRecordParams[9].Value = strMarkToTeach;
                    SqlRecordParams[10] = new SqlParameter("@disclaimer_text", SqlDbType.NText); SqlRecordParams[10].Value = strRptDisclReasonDesc;
                    SqlRecordParams[11] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[11].Value = UserID;
                    SqlRecordParams[12] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[12].Value = intMenuID;
                    SqlRecordParams[13] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[13].Value = SessionID;
                    SqlRecordParams[14] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[14].Direction = ParameterDirection.Output;
                    SqlRecordParams[15] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[15].Direction = ParameterDirection.Output;
                    SqlRecordParams[16] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[16].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_report_save", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[16].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;

                    RptId = new Guid(Convert.ToString(SqlRecordParams[0].Value));
                    strUserName = Convert.ToString(SqlRecordParams[14].Value).Trim();
                    ReturnMessage = Convert.ToString(SqlRecordParams[15].Value);

                }
                catch (Exception expErr)
                { bReturn = false; CatchMessage = expErr.Message; }

            }
            else
            {
                bReturn = false;
            }
            return bReturn;
        }
        #endregion

        #region SaveReRate
        public bool SaveReRate(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRatingReason(ref ReturnMessage))
            {

                SqlParameter[] SqlRecordParams = new SqlParameter[9];
                try
                {
                    SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = RptId; 
                    SqlRecordParams[1] = new SqlParameter("@study_hdr_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = Id;
                    SqlRecordParams[2] = new SqlParameter("@rating_reason_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = AbnormalRptReasonID;
                    SqlRecordParams[3] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = UserID;
                    SqlRecordParams[4] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[4].Value = intMenuID;
                    SqlRecordParams[5] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[5].Value = SessionID;
                    SqlRecordParams[6] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[6].Direction = ParameterDirection.Output;
                    SqlRecordParams[7] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[7].Direction = ParameterDirection.Output;
                    SqlRecordParams[8] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[8].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_re_rate_study_report", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[8].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;

                    
                    strUserName = Convert.ToString(SqlRecordParams[6].Value).Trim();
                    ReturnMessage = Convert.ToString(SqlRecordParams[7].Value);

                }
                catch (Exception expErr)
                { bReturn = false; CatchMessage = expErr.Message; }

            }
            else
            {
                bReturn = false;
            }
            return bReturn;
        }
        #endregion

        #region ValidateRatingReason
        private bool ValidateRatingReason(ref string ReturnMessage)
        {
            bool bReturn = true;
            if (strRptRate == "A" && Convert.ToString(AbnormalRptReasonID) == "00000000-0000-0000-0000-000000000000")
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "446";
            }
            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }

        #endregion

        #region ValidateReport
        private bool ValidateReport(ref string ReturnMessage)
        {
            bool bReturn = true;


            if (strRptText.Trim() == string.Empty)
            {
                ReturnMessage += "060";
            }
            if (intStatusIDPACS <= 50)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "210";
            }
            else if (intStatusIDPACS == 60 || intStatusIDPACS == 80)
            {
                if ((strRptRate.Trim() == string.Empty) && (strRateTheReport == "Y"))
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "383";
                }
            }
            if (strRptRate == "A" && Convert.ToString(AbnormalRptReasonID) == "00000000-0000-0000-0000-000000000000")
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "446";
            }
            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }

        #endregion

        #region SaveTranscription
        public bool SaveTranscription(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            strRptText = HtmlToPlainText(strRptHtml);
            if (strTranslateRptHTML.Trim() != string.Empty) strTranslateRpt = HtmlToPlainText(strTranslateRptHTML);


            if (ValidateTranscription(ref ReturnMessage))
            {

                SqlParameter[] SqlRecordParams = new SqlParameter[12];
                try
                {
                    SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = RptId; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
                    SqlRecordParams[1] = new SqlParameter("@study_hdr_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = Id;
                    SqlRecordParams[2] = new SqlParameter("@report_text", SqlDbType.NText); SqlRecordParams[2].Value = strRptText;
                    SqlRecordParams[3] = new SqlParameter("@report_text_html", SqlDbType.NText); SqlRecordParams[3].Value = strRptHtml;
                    SqlRecordParams[4] = new SqlParameter("@translated_report_text", SqlDbType.NText); SqlRecordParams[4].Value = strTranslateRpt;
                    SqlRecordParams[5] = new SqlParameter("@translated_report_text_html", SqlDbType.NText); SqlRecordParams[5].Value = strTranslateRptHTML;
                    SqlRecordParams[6] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[6].Value = UserID;
                    SqlRecordParams[7] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[7].Value = intMenuID;
                    SqlRecordParams[8] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[8].Value = SessionID;
                    SqlRecordParams[9] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[9].Direction = ParameterDirection.Output;
                    SqlRecordParams[10] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[10].Direction = ParameterDirection.Output;
                    SqlRecordParams[11] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[11].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_transcription_save", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[11].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;

                    RptId = new Guid(Convert.ToString(SqlRecordParams[0].Value));
                    strUserName = Convert.ToString(SqlRecordParams[9].Value).Trim();
                    ReturnMessage = Convert.ToString(SqlRecordParams[10].Value);

                }
                catch (Exception expErr)
                { bReturn = false; CatchMessage = expErr.Message; }

            }
            else
            {
                bReturn = false;
            }
            return bReturn;
        }
        #endregion

        #region ValidateTranscription
        private bool ValidateTranscription(ref string ReturnMessage)
        {
            bool bReturn = true;


            if (strRptText.Trim() == string.Empty)
            {
                ReturnMessage += "060";
            }

            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }

        #endregion

        #region SaveReportAddendum
        public bool SaveReportAddendum(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (strRptHtml.Trim() == string.Empty) strRptHtml = TextToHtml(strRptText);
            if (strRptText.Trim() == string.Empty) strRptText = HtmlToPlainText(strRptHtml);
            if (strAddnTextHtml.Trim() == string.Empty) strAddnTextHtml = TextToHtml(strAddnText);
            if (strAddnText.Trim() == string.Empty) strAddnText = HtmlToPlainText(strAddnTextHtml);

            if (ValidateReportAddendum(ref ReturnMessage))
            {

                SqlParameter[] SqlRecordParams = new SqlParameter[14];
                try
                {
                    SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                    SqlRecordParams[1] = new SqlParameter("@report_text", SqlDbType.NText); SqlRecordParams[1].Value = strRptText;
                    SqlRecordParams[2] = new SqlParameter("@report_text_html", SqlDbType.NText); SqlRecordParams[2].Value = strRptHtml;
                    SqlRecordParams[3] = new SqlParameter("@disclaimer_reason_id", SqlDbType.Int); SqlRecordParams[3].Value = intRptDisclReasonID;
                    SqlRecordParams[4] = new SqlParameter("@disclaimer_text", SqlDbType.NText); SqlRecordParams[4].Value = strRptDisclReasonDesc;
                    SqlRecordParams[5] = new SqlParameter("@addendum_text", SqlDbType.NText); SqlRecordParams[5].Value = strAddnText;
                    SqlRecordParams[6] = new SqlParameter("@addendum_text_html", SqlDbType.NText); SqlRecordParams[6].Value = strAddnTextHtml;
                    SqlRecordParams[7] = new SqlParameter("@mark_to_teach", SqlDbType.NChar, 1); SqlRecordParams[7].Value = strMarkToTeach;
                    SqlRecordParams[8] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[8].Value = UserID;
                    SqlRecordParams[9] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[9].Value = intMenuID;
                    SqlRecordParams[10] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[10].Value = SessionID;
                    SqlRecordParams[11] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[11].Direction = ParameterDirection.Output;
                    SqlRecordParams[12] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[12].Direction = ParameterDirection.Output;
                    SqlRecordParams[13] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[13].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_report_addendum_save", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[13].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;


                    strUserName = Convert.ToString(SqlRecordParams[11].Value).Trim();
                    ReturnMessage = Convert.ToString(SqlRecordParams[12].Value);

                }
                catch (Exception expErr)
                { bReturn = false; CatchMessage = expErr.Message; }

            }
            else
            {
                bReturn = false;
            }
            return bReturn;
        }
        #endregion

        #region ValidateReportAddendum
        private bool ValidateReportAddendum(ref string ReturnMessage)
        {
            bool bReturn = true;

            if ((strAddnTextHtml.Trim() == string.Empty) && (strRptHtml.Trim() == string.Empty))
            {
                ReturnMessage = "370";
            }
            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }

        #endregion

        #region UpdateArchiveReportAddendum
        public bool UpdateArchiveReportAddendum(string ConfigPath, DataTable dtblAddendum, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[5];
            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@TVP_addendums", SqlDbType.Structured); SqlRecordParams[1].Value = dtblAddendum;
                SqlRecordParams[2] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_archive_report_addendum_update", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[4].Value);
                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;
                ReturnMessage = Convert.ToString(SqlRecordParams[3].Value);

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchAddendum
        public bool FetchAddendum(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = false;
            DataSet ds = new DataSet();
            SqlParameter[] SqlRecordParams = new SqlParameter[2];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@study_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@addendum_srl", SqlDbType.Int); SqlRecordParams[1].Value = intAddnSrl;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_addendum_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Addendum";
                    foreach (DataRow dr in ds.Tables["Addendum"].Rows)
                    {
                        strAddnText = Convert.ToString(dr["addendum_text"]).Trim();
                    }
                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                }

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }
            finally
            {
                ds.Dispose();
            }

            return bReturn;
        }
        #endregion

        #region HtmlToPlainText
        private string HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);
            var text = html;

            text = text.Replace("<p class=\"pasted\">", string.Empty);
            text = text.Replace("<p>", string.Empty);
            text = text.Replace("</p>", "<br/>");

            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }
        #endregion

        #region TextToHtml
        private string TextToHtml(string text)
        {
            var html = text;
            html = HttpUtility.HtmlEncode(html);
            html = html.Replace("\r\n", "\r");
            html = html.Replace("\n", "\r");
            html = html.Replace("\r", "<br/>");
            html = html.Replace("  ", " &nbsp;");
            return html;
        }

        #endregion

        #endregion

        #region Manual Submission

        #region SaveManualSubmission
        public bool SaveManualSubmission(string ConfigPath, StudyTypeList[] ArrobjST, HeaderDICOMList[] ArrObjDCM, HeaderImageList[] ArrObjImg, HeaderDocumentList[] ArrObjDocs, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            DataTable dtblST = new DataTable();
            DataTable dtblImg = new DataTable();
            DataTable dtblDCM = new DataTable();
            DataTable dtblDoc = new DataTable();

            if (ValidateManualSubmission(ArrobjST, ArrObjDCM, ArrObjImg, ref ReturnMessage))
            {
                if ((GenerateStudyTypeTable(ArrobjST, ref dtblST, ref CatchMessage)) &&
                     (GenerateMSImageTable(ArrObjImg, ref dtblImg, ref CatchMessage)) &&
                     (GenerateMSDCMTable(ArrObjDCM, ref dtblDCM, ref CatchMessage)) &&
                     (GenerateDocumentTable(ArrObjDocs, ref dtblDoc, ref CatchMessage)))
                {
                    SqlParameter[] SqlRecordParams = new SqlParameter[44];
                    try
                    {
                        SqlRecordParams[0] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strStudyUID;
                        SqlRecordParams[1] = new SqlParameter("@series_instance_uid", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strSeriesUID;
                        SqlRecordParams[2] = new SqlParameter("@series_no", SqlDbType.NVarChar, 100); SqlRecordParams[2].Value = strSeriesNo;
                        SqlRecordParams[3] = new SqlParameter("@session_id", SqlDbType.NVarChar, 30); SqlRecordParams[3].Value = strSessID;
                        SqlRecordParams[4] = new SqlParameter("@study_date", SqlDbType.DateTime); SqlRecordParams[4].Value = dtStudyDate;
                        SqlRecordParams[5] = new SqlParameter("@patient_id", SqlDbType.NVarChar, 20); SqlRecordParams[5].Value = strPatientID;
                        SqlRecordParams[6] = new SqlParameter("@patient_fname", SqlDbType.NVarChar, 80); SqlRecordParams[6].Value = strPatientFName;
                        SqlRecordParams[7] = new SqlParameter("@patient_lname", SqlDbType.NVarChar, 80); SqlRecordParams[7].Value = strPatientLName;
                        SqlRecordParams[8] = new SqlParameter("@patient_weight", SqlDbType.Decimal); SqlRecordParams[8].Value = decPatientWt;
                        SqlRecordParams[9] = new SqlParameter("@wt_uom", SqlDbType.NVarChar, 5); SqlRecordParams[9].Value = strWtUOM;
                        SqlRecordParams[10] = new SqlParameter("@patient_dob", SqlDbType.DateTime); SqlRecordParams[10].Value = dtPatientDob;
                        SqlRecordParams[11] = new SqlParameter("@patient_age", SqlDbType.NVarChar, 50); SqlRecordParams[11].Value = strPatientAge;
                        SqlRecordParams[12] = new SqlParameter("@patient_sex", SqlDbType.NVarChar, 10); SqlRecordParams[12].Value = strPatientGender;
                        SqlRecordParams[13] = new SqlParameter("@patient_spayed_neutered", SqlDbType.NVarChar, 30); SqlRecordParams[13].Value = strSexNeutered;
                        SqlRecordParams[14] = new SqlParameter("@species_id", SqlDbType.Int); SqlRecordParams[14].Value = intSpeciesID;
                        SqlRecordParams[15] = new SqlParameter("@breed_id", SqlDbType.UniqueIdentifier); SqlRecordParams[15].Value = BreedID;
                        SqlRecordParams[16] = new SqlParameter("@owner_first_name", SqlDbType.NVarChar, 100); SqlRecordParams[16].Value = strOwnerFN;
                        SqlRecordParams[17] = new SqlParameter("@owner_last_name", SqlDbType.NVarChar, 100); SqlRecordParams[17].Value = strOwnerLN;
                        SqlRecordParams[18] = new SqlParameter("@accession_no", SqlDbType.NVarChar, 20); SqlRecordParams[18].Value = strAccnNo;
                        SqlRecordParams[19] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[19].Value = InstitutionID;
                        SqlRecordParams[20] = new SqlParameter("@physician_id", SqlDbType.UniqueIdentifier); SqlRecordParams[20].Value = PhysicianID;
                        SqlRecordParams[21] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[21].Value = intModalityID;
                        SqlRecordParams[22] = new SqlParameter("@priority_id", SqlDbType.Int); SqlRecordParams[22].Value = intPriorityID;
                        SqlRecordParams[23] = new SqlParameter("@reason", SqlDbType.NVarChar, 2000); SqlRecordParams[23].Value = strReason;
                        SqlRecordParams[24] = new SqlParameter("@physician_note", SqlDbType.NVarChar, 2000); SqlRecordParams[24].Value = strPhysNote;
                        SqlRecordParams[25] = new SqlParameter("@consult_applied", SqlDbType.NVarChar, 2000); SqlRecordParams[25].Value = strConsultApplied;
                        SqlRecordParams[26] = new SqlParameter("@category_id", SqlDbType.Int); SqlRecordParams[26].Value = intCategoryID;
                        SqlRecordParams[27] = new SqlParameter("@TVP_studytypes", SqlDbType.Structured); SqlRecordParams[27].Value = dtblST;
                        SqlRecordParams[28] = new SqlParameter("@TVP_dcm", SqlDbType.Structured); SqlRecordParams[28].Value = dtblDCM;
                        SqlRecordParams[29] = new SqlParameter("@TVP_img", SqlDbType.Structured); SqlRecordParams[29].Value = dtblImg;
                        SqlRecordParams[30] = new SqlParameter("@TVP_docs", SqlDbType.Structured); SqlRecordParams[30].Value = dtblDoc;
                        SqlRecordParams[31] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[31].Value = UserID;
                        SqlRecordParams[32] = new SqlParameter("@sender_time_offset_mins", SqlDbType.Int); SqlRecordParams[32].Value = intSenderTZOffsetMins;
                        SqlRecordParams[33] = new SqlParameter("@submit_priority", SqlDbType.NChar, 1); SqlRecordParams[33].Value = strSubmitPriority;
                        SqlRecordParams[34] = new SqlParameter("@patient_country_id", SqlDbType.Int); SqlRecordParams[34].Value = intCountryID;
                        SqlRecordParams[35] = new SqlParameter("@patient_state_id", SqlDbType.Int); SqlRecordParams[35].Value = intStateID;
                        SqlRecordParams[36] = new SqlParameter("@patient_city", SqlDbType.NVarChar, 100); SqlRecordParams[36].Value = strCity;
                        SqlRecordParams[37] = new SqlParameter("@user_session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[37].Value = SessionID;
                        SqlRecordParams[38] = new SqlParameter("@next_operation_time", SqlDbType.NVarChar, 130); SqlRecordParams[38].Direction = ParameterDirection.Output;
                        SqlRecordParams[39] = new SqlParameter("@delv_time", SqlDbType.NVarChar, 130); SqlRecordParams[39].Direction = ParameterDirection.Output;
                        SqlRecordParams[40] = new SqlParameter("@message_display", SqlDbType.NVarChar, 500); SqlRecordParams[40].Direction = ParameterDirection.Output;
                        SqlRecordParams[41] = new SqlParameter("@error_text", SqlDbType.NVarChar, 500); SqlRecordParams[41].Direction = ParameterDirection.Output;
                        SqlRecordParams[42] = new SqlParameter("@error_code", SqlDbType.VarChar, 500); SqlRecordParams[42].Direction = ParameterDirection.Output;
                        SqlRecordParams[43] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[43].Direction = ParameterDirection.Output;

                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "manual_submission_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[43].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strNextOpTime = Convert.ToString(SqlRecordParams[38].Value).Trim();
                        strDelvTime = Convert.ToString(SqlRecordParams[39].Value).Trim();
                        strMsgDisp = Convert.ToString(SqlRecordParams[40].Value).Trim().Replace("\n", "<br/>");
                        strUserName = Convert.ToString(SqlRecordParams[41].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[42].Value);
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

        #region SaveManualSubmissionTemp
        public bool SaveManualSubmissionTemp(string ConfigPath, StudyTypeList[] ArrobjST, HeaderDICOMList[] ArrObjDCM, HeaderImageList[] ArrObjImg, HeaderDocumentList[] ArrObjDocs, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            DataTable dtblST = new DataTable();
            DataTable dtblImg = new DataTable();
            DataTable dtblDCM = new DataTable();
            DataTable dtblDoc = new DataTable();

            if (ValidateManualSubmission(ArrobjST, ArrObjDCM, ArrObjImg, ref ReturnMessage))
            {
                if ((GenerateStudyTypeTable(ArrobjST, ref dtblST, ref CatchMessage)) &&
                     (GenerateMSImageTable(ArrObjImg, ref dtblImg, ref CatchMessage)) &&
                     (GenerateMSDCMTable(ArrObjDCM, ref dtblDCM, ref CatchMessage)) &&
                     (GenerateDocumentTable(ArrObjDocs, ref dtblDoc, ref CatchMessage)))
                {
                    SqlParameter[] SqlRecordParams = new SqlParameter[35];
                    try
                    {
                        SqlRecordParams[0] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strStudyUID;
                        SqlRecordParams[1] = new SqlParameter("@series_instance_uid", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strSeriesUID;
                        SqlRecordParams[2] = new SqlParameter("@series_no", SqlDbType.NVarChar, 100); SqlRecordParams[2].Value = strSeriesNo;
                        SqlRecordParams[3] = new SqlParameter("@session_id", SqlDbType.NVarChar, 30); SqlRecordParams[3].Value = strSessID;
                        SqlRecordParams[4] = new SqlParameter("@study_date", SqlDbType.DateTime); SqlRecordParams[4].Value = dtStudyDate;
                        SqlRecordParams[5] = new SqlParameter("@patient_id", SqlDbType.NVarChar, 20); SqlRecordParams[5].Value = strPatientID;
                        SqlRecordParams[6] = new SqlParameter("@patient_fname", SqlDbType.NVarChar, 80); SqlRecordParams[6].Value = strPatientFName;
                        SqlRecordParams[7] = new SqlParameter("@patient_lname", SqlDbType.NVarChar, 80); SqlRecordParams[7].Value = strPatientLName;
                        SqlRecordParams[8] = new SqlParameter("@patient_weight", SqlDbType.Decimal); SqlRecordParams[8].Value = decPatientWt;
                        SqlRecordParams[9] = new SqlParameter("@wt_uom", SqlDbType.NVarChar, 5); SqlRecordParams[9].Value = strWtUOM;
                        SqlRecordParams[10] = new SqlParameter("@patient_dob", SqlDbType.DateTime); SqlRecordParams[10].Value = dtPatientDob;
                        SqlRecordParams[11] = new SqlParameter("@patient_age", SqlDbType.NVarChar, 50); SqlRecordParams[11].Value = strPatientAge;
                        SqlRecordParams[12] = new SqlParameter("@patient_sex", SqlDbType.NVarChar, 10); SqlRecordParams[12].Value = strPatientGender;
                        SqlRecordParams[13] = new SqlParameter("@patient_spayed_neutered", SqlDbType.NVarChar, 30); SqlRecordParams[13].Value = strSexNeutered;
                        SqlRecordParams[14] = new SqlParameter("@species_id", SqlDbType.Int); SqlRecordParams[14].Value = intSpeciesID;
                        SqlRecordParams[15] = new SqlParameter("@breed_id", SqlDbType.UniqueIdentifier); SqlRecordParams[15].Value = BreedID;
                        SqlRecordParams[16] = new SqlParameter("@owner_first_name", SqlDbType.NVarChar, 100); SqlRecordParams[16].Value = strOwnerFN;
                        SqlRecordParams[17] = new SqlParameter("@owner_last_name", SqlDbType.NVarChar, 100); SqlRecordParams[17].Value = strOwnerLN;
                        SqlRecordParams[18] = new SqlParameter("@accession_no", SqlDbType.NVarChar, 20); SqlRecordParams[18].Value = strAccnNo;
                        SqlRecordParams[19] = new SqlParameter("@reg_institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[19].Value = InstitutionID;
                        SqlRecordParams[20] = new SqlParameter("@reg_physician_id", SqlDbType.UniqueIdentifier); SqlRecordParams[20].Value = PhysicianID;
                        SqlRecordParams[21] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[21].Value = intModalityID;
                        SqlRecordParams[22] = new SqlParameter("@priority_id", SqlDbType.Int); SqlRecordParams[22].Value = intPriorityID;
                        SqlRecordParams[23] = new SqlParameter("@reason", SqlDbType.NVarChar, 2000); SqlRecordParams[23].Value = strReason;
                        SqlRecordParams[24] = new SqlParameter("@physician_note", SqlDbType.NVarChar, 2000); SqlRecordParams[24].Value = strPhysNote;
                        SqlRecordParams[25] = new SqlParameter("@consult_applied", SqlDbType.NVarChar, 2000); SqlRecordParams[25].Value = strConsultApplied;
                        SqlRecordParams[26] = new SqlParameter("@category_id", SqlDbType.Int); SqlRecordParams[26].Value = intCategoryID;
                        SqlRecordParams[27] = new SqlParameter("@TVP_studytypes", SqlDbType.Structured); SqlRecordParams[27].Value = dtblST;
                        SqlRecordParams[28] = new SqlParameter("@TVP_dcm", SqlDbType.Structured); SqlRecordParams[28].Value = dtblDCM;
                        SqlRecordParams[29] = new SqlParameter("@TVP_img", SqlDbType.Structured); SqlRecordParams[29].Value = dtblImg;
                        SqlRecordParams[30] = new SqlParameter("@TVP_docs", SqlDbType.Structured); SqlRecordParams[30].Value = dtblDoc;
                        SqlRecordParams[31] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[31].Value = UserID;
                        SqlRecordParams[32] = new SqlParameter("@error_text", SqlDbType.NVarChar, 500); SqlRecordParams[32].Direction = ParameterDirection.Output;
                        SqlRecordParams[33] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[33].Direction = ParameterDirection.Output;
                        SqlRecordParams[34] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[34].Direction = ParameterDirection.Output;

                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "manual_submission_temp_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[34].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[32].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[33].Value);
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

        #region GenerateMSDCMTable
        private bool GenerateMSDCMTable(HeaderDICOMList[] ArrObj, ref DataTable dtblDCM, ref string CatchMessage)
        {
            bool bReturn = false;


            dtblDCM.Columns.Add("dcm_file_srl_no", System.Type.GetType("System.Int32"));
            dtblDCM.Columns.Add("dcm_file_name", System.Type.GetType("System.String"));
            dtblDCM.Columns.Add("dcm_file", System.Type.GetType("System.Byte[]"));


            try
            {
                if (ArrObj.Length > 0)
                {
                    for (int i = 0; i < ArrObj.Length; i++)
                    {
                        DataRow dr = dtblDCM.NewRow();
                        dr["dcm_file_srl_no"] = ArrObj[i].SERIAL_NUMBER;
                        dr["dcm_file_name"] = ArrObj[i].FILE_NAME;
                        dr["dcm_file"] = ArrObj[i].FILE_CONTENT;
                        dtblDCM.Rows.Add(dr);
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

        #region GenerateMSImageTable
        private bool GenerateMSImageTable(HeaderImageList[] ArrObj, ref DataTable dtblImg, ref string CatchMessage)
        {
            bool bReturn = false;


            dtblImg.Columns.Add("img_file_srl_no", System.Type.GetType("System.Int32"));
            dtblImg.Columns.Add("img_file_name", System.Type.GetType("System.String"));
            dtblImg.Columns.Add("img_file", System.Type.GetType("System.Byte[]"));


            try
            {
                if (ArrObj.Length > 0)
                {
                    for (int i = 0; i < ArrObj.Length; i++)
                    {
                        DataRow dr = dtblImg.NewRow();
                        dr["img_file_srl_no"] = ArrObj[i].SERIAL_NUMBER;
                        dr["img_file_name"] = ArrObj[i].FILE_NAME;
                        dr["img_file"] = ArrObj[i].FILE_CONTENT;
                        dtblImg.Rows.Add(dr);
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

        #region ValidateManualSubmission
        private bool ValidateManualSubmission(StudyTypeList[] arrObjST, HeaderDICOMList[] arrObjDCM, HeaderImageList[] arrObjImg, ref string ReturnMessage)
        {
            bool bReturn = true;

            if (dtStudyDate.Year == 1900)
            {
                ReturnMessage = "178";
            }
            else
            {
                if (dtStudyDate > DateTime.Now)
                {
                    ReturnMessage = "182";
                }
            }
            if (strPatientName.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "049";
            }
            if (strPatientGender.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "050";
            }
            if (strSexNeutered.Trim() == string.Empty)
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

            if (dtPatientDob.Year != 1900)
            {
                if (dtPatientDob > dtStudyDate)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage = "038";
                }
                if (dtPatientDob >= DateTime.Today)
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

            if (intPriorityID == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "102";
            }
            if (intModalityID == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "042";
            }
            if (intCategoryID == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "293";
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

            if (arrObjST.Length == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "063";
            }
            else if (arrObjST.Length > 4)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "064";
            }

            if (arrObjDCM.Length == 0 && arrObjImg.Length == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "336";
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
    public class HeaderDocumentList
    {
        #region Constructor
        public HeaderDocumentList()
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
    public class HeaderDICOMList
    {
        #region Constructor
        public HeaderDICOMList()
        {
        }
        #endregion

        #region Variables
        Guid DCMFileID = Guid.Empty;
        string strFileName = string.Empty;
        int intSrlNo = 0;
        Byte[] btFile = new Byte[0];
        #endregion

        #region Properties
        public Guid ID
        {
            get { return DCMFileID; }
            set { DCMFileID = value; }
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
        public Byte[] FILE_CONTENT
        {
            get { return btFile; }
            set { btFile = value; }
        }
        #endregion
    }
    public class HeaderImageList
    {
        #region Constructor
        public HeaderImageList()
        {
        }
        #endregion

        #region Variables
        Guid FileID = Guid.Empty;
        string strFileName = string.Empty;
        int intSrlNo = 0;
        Byte[] btFile = new Byte[0];
        #endregion

        #region Properties
        public Guid ID
        {
            get { return FileID; }
            set { FileID = value; }
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
        public Byte[] FILE_CONTENT
        {
            get { return btFile; }
            set { btFile = value; }
        }
        #endregion
    }
    public class StudyToMerge
    {
        #region Constructor
        public StudyToMerge()
        {
        }
        #endregion

        #region Variables
        Guid Id = Guid.Empty;
        string strSUID = string.Empty;
        int intImgCount = 0;
        string strMergeStat = string.Empty;
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
        public int IMAGE_COUNT
        {
            get { return intImgCount; }
            set { intImgCount = value; }
        }
        public string MERGE_STATUS
        {
            get { return strMergeStat; }
            set { strMergeStat = value; }
        }
        #endregion
    }
    public class ReportData
    {
        public Guid Id { get; set; }
        public string Study_Uid { get; set; }
        public string Species_Name { get; set; }
        public DateTime Received_Date { get; set; }
        public DateTime Submitted_Date { get; set; }
        public DateTime Date_Dictated { get; set; }
        public string Patient_Name { get; set; }
        public string Modality_Name { get; set; }
        public string Category_Name { get; set; }
        public string Physician_Name { get; set; }
        public string Institution_Name { get; set; }
        public string Priority_Desc { get; set; }
        public string Image_Object_Count { get; set; }
        public string Radiologist_Pacs { get; set; }
        public string Final_Radiologist { get; set; }
        public string Rating_Reason { get; set; }
        public string Rating { get; set; }

        public DateTime Status_Last_Updated_On { get; set; }
    }
    public class StudyUIDList
    {
        #region Constructor
        public StudyUIDList()
        {
        }
        #endregion

        #region Variables
        string strSUID = string.Empty;
        #endregion

        #region Properties
        public string STUDY_UID
        {
            get { return strSUID; }
            set { strSUID = value; }
        }
        #endregion
    }
}
