using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using VETRISScheduler.DAL;

namespace VETRISScheduler.Core
{
    public class CaseStudyUpdate
    {
        #region Constructor
        public CaseStudyUpdate()
        {
        }
        #endregion

        #region Variables
        Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
        string strSUID = string.Empty;
        int intStatusID = 0;
        string strRadiologist = string.Empty;
        string strInstitutionName = string.Empty;
        string strManufacturer = string.Empty;
        string strModel = string.Empty;
        string strAETitle = string.Empty;
        int intImgCount = 0;
        int intObjCount = 0;
        string strSvcCodes = string.Empty;
        string strDictRadiologist = string.Empty;
        string strPrelimRadiologist = string.Empty;
        string strFinalRadiologist = string.Empty;
        string strModality = string.Empty;
        string strRptText = string.Empty;
        string strRptTextHTML = string.Empty;
        string strPACSImgViewURL = string.Empty;
        string strPACSStudyViewURL = string.Empty;
        string strPACSRptViewURL = string.Empty;
        DateTime dtRptApprove = DateTime.Now;
        DateTime dtRptRecord = DateTime.Now;

        string strAccnNo = string.Empty;
        string strPatientId = string.Empty;
        string strPatientName = string.Empty;
        string strSex = string.Empty;
        DateTime dtDOB = DateTime.Today;
        string strAge = string.Empty;
        string strSpecies = string.Empty;
        string strBreed = string.Empty;
        string strPhysician = string.Empty;
        string strAddlField = string.Empty;
        string strAddlFieldVal = string.Empty;
        #endregion

        #region Properties
        public Guid STUDY_ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public string STUDY_UID
        {
            get { return strSUID; }
            set { strSUID = value; }
        }
        public int STATUS_ID
        {
            get { return intStatusID; }
            set { intStatusID = value; }
        }
        public string REPORT_TEXT
        {
            get { return strRptText; }
            set { strRptText = value; }
        }
        public string REPORT_TEXT_HTML
        {
            get { return strRptTextHTML; }
            set { strRptTextHTML = value; }
        }
        public string RADIOLOGIST
        {
            get { return strRadiologist; }
            set { strRadiologist = value; }
        }
        public string DICTATION_RADIOLOGIST
        {
            get { return strDictRadiologist; }
            set { strDictRadiologist = value; }
        }
        public string PRELIMINARY_RADIOLOGIST
        {
            get { return strPrelimRadiologist; }
            set { strPrelimRadiologist = value; }
        }
        public string FINAL_RADIOLOGIST
        {
            get { return strFinalRadiologist; }
            set { strFinalRadiologist = value; }
        }
        public string MODALITY
        {
            get { return strModality; }
            set { strModality = value; }
        }
        public string INSTITUTION_NAME
        {
            get { return strInstitutionName; }
            set { strInstitutionName = value; }
        }
        public string MANUFACTURER
        {
            get { return strManufacturer; }
            set { strManufacturer = value; }
        }
        public string MODEL
        {
            get { return strModel; }
            set { strModel = value; }
        }
        public string MODALITY_AE_TITLE
        {
            get { return strAETitle; }
            set { strAETitle = value; }
        }
        public int IMAGE_COUNT
        {
            get { return intImgCount; }
            set { intImgCount = value; }
        }
        public int OBJECT_COUNT
        {
            get { return intObjCount; }
            set { intObjCount = value; }
        }
        public string SERVICE_CODES
        {
            get { return strSvcCodes; }
            set { strSvcCodes = value; }
        }
        public string PACS_STUDY_VIEW_URL
        {
            get { return strPACSStudyViewURL; }
            set { strPACSStudyViewURL = value; }
        }
        public string PACS_IMAGE_VIEW_URL
        {
            get { return strPACSImgViewURL; }
            set { strPACSImgViewURL = value; }
        }
        public string PACS_REPORT_VIEW_URL
        {
            get { return strPACSRptViewURL; }
            set { strPACSRptViewURL = value; }
        }
        public DateTime REPORT_RECORDING_DATE
        {
            get { return dtRptRecord; }
            set { dtRptRecord = value; }
        }
        public DateTime REPORT_APPROVAL_DATE
        {
            get { return dtRptApprove; }
            set { dtRptApprove = value; }
        }

        public string ACCN_NO
        {
            get { return strAccnNo; }
            set { strAccnNo = value; }
        }
        public string PATIENT_ID
        {
            get { return strPatientId; }
            set { strPatientId = value; }
        }
        public string PATIENT_NAME
        {
            get { return strPatientName; }
            set { strPatientName = value; }
        }
        public string SEX
        {
            get { return strSex; }
            set { strSex = value; }
        }
        public DateTime DOB
        {
            get { return dtDOB; }
            set { dtDOB = value; }
        }
        public string AGE
        {
            get { return strAge; }
            set { strAge = value; }
        }
        public string SPECIES
        {
            get { return strSpecies; }
            set { strSpecies = value; }
        }
        public string BREED
        {
            get { return strBreed; }
            set { strBreed = value; }
        }
        public string PHYSICIAN
        {
            get { return strPhysician; }
            set { strPhysician = value; }
        }

        public string ADDILTIONAL_FIELD
        {
            get { return strAddlField; }
            set { strAddlField = value; }
        }
        public string ADDILTIONAL_FIELD_VALUE
        {
            get { return strAddlFieldVal; }
            set { strAddlFieldVal = value; }
        }
        #endregion

        #region UpdateStatus
        public bool UpdateStatus(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[21];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@study_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strSUID;
                SqlRecordParams[2] = new SqlParameter("@status_id", SqlDbType.Int); SqlRecordParams[2].Value = intStatusID;
                SqlRecordParams[3] = new SqlParameter("@radiologist", SqlDbType.VarChar, 250); SqlRecordParams[3].Value = strRadiologist;
                SqlRecordParams[4] = new SqlParameter("@institution_name", SqlDbType.VarChar, 100); SqlRecordParams[4].Value = strInstitutionName;
                SqlRecordParams[5] = new SqlParameter("@manufacturer_name", SqlDbType.VarChar, 100); SqlRecordParams[5].Value = strManufacturer;
                SqlRecordParams[6] = new SqlParameter("@manufacturer_model_no", SqlDbType.VarChar, 100); SqlRecordParams[6].Value = strModel;
                SqlRecordParams[7] = new SqlParameter("@modality_ae_title", SqlDbType.VarChar, 50); SqlRecordParams[7].Value = strAETitle;
                SqlRecordParams[8] = new SqlParameter("@image_count", SqlDbType.Int); SqlRecordParams[8].Value = intImgCount;
                SqlRecordParams[9] = new SqlParameter("@object_count", SqlDbType.Int); SqlRecordParams[9].Value = intObjCount;
                SqlRecordParams[10] = new SqlParameter("@service_codes", SqlDbType.NVarChar, 250); SqlRecordParams[10].Value = strSvcCodes;
                SqlRecordParams[11] = new SqlParameter("@dict_radiologist_pacs", SqlDbType.VarChar, 250); SqlRecordParams[11].Value = strDictRadiologist;
                SqlRecordParams[12] = new SqlParameter("@prelim_radiologist_pacs", SqlDbType.VarChar, 250); SqlRecordParams[12].Value = strPrelimRadiologist;
                SqlRecordParams[13] = new SqlParameter("@final_radiologist_pacs", SqlDbType.VarChar, 250); SqlRecordParams[13].Value = strFinalRadiologist;
                SqlRecordParams[14] = new SqlParameter("@modality_pacs", SqlDbType.VarChar, 30); SqlRecordParams[14].Value = strModality;
                SqlRecordParams[15] = new SqlParameter("@rpt_approve_date", SqlDbType.DateTime); SqlRecordParams[15].Value = dtRptApprove;
                SqlRecordParams[16] = new SqlParameter("@rpt_record_date", SqlDbType.DateTime); SqlRecordParams[16].Value = dtRptRecord;
                SqlRecordParams[17] = new SqlParameter("@additional_field", SqlDbType.NVarChar, 5); SqlRecordParams[17].Value = strAddlField;
                SqlRecordParams[18] = new SqlParameter("@additional_field_value", SqlDbType.NVarChar, 2000); SqlRecordParams[18].Value = strAddlFieldVal;
                SqlRecordParams[19] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[19].Direction = ParameterDirection.Output;
                SqlRecordParams[20] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[20].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_case_study_status_update_ws8", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[20].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[19].Value).Trim();

                if (intReturnType == 0)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdateStatus() - Core :: Error: " + strReturnMessage, true);
                }
                else if (intReturnType == 1)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdateStatus() - Core:: " + strReturnMessage, false);
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "-" + strSUID; }
           
            return bReturn;
        }
        #endregion

        #region UpdateStatus1
        public bool UpdateStatus1(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[27];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);


                SqlRecordParams[0] = new SqlParameter("@study_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strSUID;
                SqlRecordParams[2] = new SqlParameter("@status_id", SqlDbType.Int); SqlRecordParams[2].Value = intStatusID;
                SqlRecordParams[3] = new SqlParameter("@radiologist", SqlDbType.VarChar, 250); SqlRecordParams[3].Value = strRadiologist;
                SqlRecordParams[4] = new SqlParameter("@institution_name", SqlDbType.VarChar, 100); SqlRecordParams[4].Value = strInstitutionName;
                SqlRecordParams[5] = new SqlParameter("@manufacturer_name", SqlDbType.VarChar, 100); SqlRecordParams[5].Value = strManufacturer;
                SqlRecordParams[6] = new SqlParameter("@manufacturer_model_no", SqlDbType.VarChar, 100); SqlRecordParams[6].Value = strModel;
                SqlRecordParams[7] = new SqlParameter("@modality_ae_title", SqlDbType.VarChar, 50); SqlRecordParams[7].Value = strAETitle;
                SqlRecordParams[8] = new SqlParameter("@image_count", SqlDbType.Int); SqlRecordParams[8].Value = intImgCount;
                SqlRecordParams[9] = new SqlParameter("@object_count", SqlDbType.Int); SqlRecordParams[9].Value = intObjCount;
                SqlRecordParams[10] = new SqlParameter("@service_codes", SqlDbType.NVarChar, 250); SqlRecordParams[10].Value = strSvcCodes;
                SqlRecordParams[11] = new SqlParameter("@prelim_radiologist_pacs", SqlDbType.VarChar, 250); SqlRecordParams[11].Value = strPrelimRadiologist;
                SqlRecordParams[12] = new SqlParameter("@final_radiologist_pacs", SqlDbType.VarChar, 250); SqlRecordParams[12].Value = strFinalRadiologist;
                SqlRecordParams[13] = new SqlParameter("@modality_pacs", SqlDbType.VarChar, 30); SqlRecordParams[13].Value = strModality;
                SqlRecordParams[14] = new SqlParameter("@rpt_approve_date", SqlDbType.DateTime); SqlRecordParams[14].Value = dtRptApprove;
                SqlRecordParams[15] = new SqlParameter("@rpt_record_date", SqlDbType.DateTime); SqlRecordParams[15].Value = dtRptRecord;

                SqlRecordParams[16] = new SqlParameter("@accession_no", SqlDbType.VarChar, 50); SqlRecordParams[16].Value = strAccnNo;
                SqlRecordParams[17] = new SqlParameter("@patient_id", SqlDbType.VarChar, 50); SqlRecordParams[17].Value = strPatientId;
                SqlRecordParams[18] = new SqlParameter("@patient_name", SqlDbType.VarChar, 250); SqlRecordParams[18].Value = strPatientName;
                SqlRecordParams[19] = new SqlParameter("@patient_sex", SqlDbType.VarChar, 20); SqlRecordParams[19].Value = strSex;
                SqlRecordParams[20] = new SqlParameter("@patient_dob", SqlDbType.DateTime); if (dtDOB != null) SqlRecordParams[20].Value = dtDOB; else SqlRecordParams[20].Value = Convert.ToDateTime("01jan1900");
                SqlRecordParams[21] = new SqlParameter("@referring_physician", SqlDbType.VarChar, 100); SqlRecordParams[21].Value = strPhysician;
                SqlRecordParams[22] = new SqlParameter("@species", SqlDbType.VarChar, 30); SqlRecordParams[22].Value = strSpecies;
                SqlRecordParams[23] = new SqlParameter("@breed", SqlDbType.VarChar, 30); SqlRecordParams[23].Value = strBreed;
                SqlRecordParams[24] = new SqlParameter("@patient_age", SqlDbType.VarChar, 30); SqlRecordParams[24].Value = strAge;


                SqlRecordParams[25] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[25].Direction = ParameterDirection.Output;
                SqlRecordParams[26] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[26].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_case_study_status_update_ws8_1", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[26].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[25].Value).Trim();

                if (intReturnType == 0)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdateStatus() - Error: " + strReturnMessage, true);
                }
                else if (intReturnType == 1)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdateStatus() - " + strReturnMessage, false);
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "-" + strSUID; }

            return bReturn;
        }
        #endregion

        #region FetchCaseList
        public bool FetchCaseList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_status_update_records_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "StudyList";
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

        #region UpdateReport
        public bool UpdateReport(string ConfigPath, int ServiceID, string strSvcName, DataTable dtblAddendum,ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[11];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                SqlRecordParams[0] = new SqlParameter("@study_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strSUID;
                SqlRecordParams[2] = new SqlParameter("@status_id", SqlDbType.Int); SqlRecordParams[2].Value = intStatusID;
                SqlRecordParams[3] = new SqlParameter("@report_text", SqlDbType.NText); SqlRecordParams[3].Value = strRptText;
                SqlRecordParams[4] = new SqlParameter("@report_text_html", SqlDbType.NText); SqlRecordParams[4].Value = strRptTextHTML;
                SqlRecordParams[5] = new SqlParameter("@TVP_addendums", SqlDbType.Structured); SqlRecordParams[5].Value = dtblAddendum;
                SqlRecordParams[6] = new SqlParameter("@PACIMGVWRURL", SqlDbType.NVarChar, 200); SqlRecordParams[6].Value = strPACSImgViewURL;
                SqlRecordParams[7] = new SqlParameter("@PACLOGINURL", SqlDbType.NVarChar, 200); SqlRecordParams[7].Value = strPACSStudyViewURL;
                SqlRecordParams[8] = new SqlParameter("@PACSRPTVWRURL", SqlDbType.NVarChar, 200); SqlRecordParams[8].Value = strPACSRptViewURL;
                SqlRecordParams[9] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[9].Direction = ParameterDirection.Output;
                SqlRecordParams[10] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[10].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_case_study_report_save_ws8", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[10].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[9].Value).Trim();

                if (intReturnType == 0)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdateReport() - Error: " + strReturnMessage, true);
                }
                else if (intReturnType == 1)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdateReport() - " + strReturnMessage, false);
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "-" + strSUID; }
            finally
            {
                dtblAddendum.Dispose();
                dtblAddendum = null;
            }

            return bReturn;
        }
        #endregion

        #region DeleteStudy
        public bool DeleteStudy(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            string strInstCode = string.Empty; string strInstName = string.Empty;
            string strPACSARCHIVEFLDR = string.Empty; string strFTPDLFLDRTMP = string.Empty;
            string strArchFolder = string.Empty;
            string[] arrFiles= new string[0];

            SqlParameter[] SqlRecordParams = new SqlParameter[7];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);


                SqlRecordParams[0] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strSUID;
                SqlRecordParams[1] = new SqlParameter("@institution_code", SqlDbType.NVarChar, 5); SqlRecordParams[1].Direction = ParameterDirection.Output;
                SqlRecordParams[2] = new SqlParameter("@institution_name", SqlDbType.NVarChar,100); SqlRecordParams[2].Direction = ParameterDirection.Output;
                SqlRecordParams[3] = new SqlParameter("@PACSARCHIVEFLDR", SqlDbType.NVarChar, 200); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@FTPDLFLDRTMP", SqlDbType.NVarChar, 200); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_study_delete", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[6].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[5].Value).Trim();

                if (intReturnType == 0)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "DeleteStudy() - Error: " + strReturnMessage, true);
                }
                else if (intReturnType == 1)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "DeleteStudy() - " + strReturnMessage, false);

                    #region Delete Files
                    strInstCode = Convert.ToString(SqlRecordParams[1].Value).Trim();
                    strInstName = Convert.ToString(SqlRecordParams[2].Value).Trim();
                    strPACSARCHIVEFLDR = Convert.ToString(SqlRecordParams[3].Value).Trim();
                    strFTPDLFLDRTMP = Convert.ToString(SqlRecordParams[4].Value).Trim();
                    strArchFolder = strPACSARCHIVEFLDR + "\\" + strInstCode + "_" + strInstName + "_" + strSUID;

                    if (Directory.Exists(strArchFolder))
                    {
                        if (Directory.Exists(strArchFolder + "\\Images"))
                        {
                            arrFiles = Directory.GetFiles(strArchFolder + "\\Images");
                            for (int i = 0; i < arrFiles.Length;i++ )
                            {
                                if (File.Exists(arrFiles[i])) File.Delete(arrFiles[i]);

                            }
                            Directory.Delete(strArchFolder + "\\Images");
                        }

                        arrFiles = new string[0];
                        arrFiles = Directory.GetFiles(strArchFolder);

                        for (int i = 0; i < arrFiles.Length; i++)
                        {
                            if (File.Exists(arrFiles[i])) File.Delete(arrFiles[i]);

                        }
                        Directory.Delete(strArchFolder);
                    }
                    #endregion
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "-" + strSUID; }
            return bReturn;
        }
        #endregion
    }
}
