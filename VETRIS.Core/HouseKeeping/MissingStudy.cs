using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using VETRIS.DAL;

namespace VETRIS.Core.HouseKeeping
{
    public class MissingStudy
    {
        #region Constructor
        public MissingStudy()
        {
        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        string strUserName = string.Empty;

        Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
        string strPatientID = string.Empty;
        string strStudyUID = string.Empty;
        string strStudyDesc = string.Empty;
        string strAccnNo = string.Empty;
        string strModality = string.Empty;
        string strModalityAETitle = string.Empty;
        string strBodyPart = string.Empty;
        int intStatus = 0;
        string strSalesPerson = string.Empty;
        DateTime dtStudyDate = DateTime.Today;
        DateTime dtRecDate = DateTime.Today;
        string strPatientName = string.Empty;
        string strPatientGender = string.Empty;
        string strSpayedNeutered = string.Empty;
        DateTime dtPatientDob = DateTime.Today;
        string strPatientAge = string.Empty;
        decimal decPatientWtLbs = 0;
        decimal decPatientWtKgs = 0;

        string strOwnerPACS = string.Empty;
        string strOwnerFN = string.Empty;
        string strOwnerLN = string.Empty;
        string strSpeciesName = string.Empty;

        string strBreedName = string.Empty;

        string strInstitutionName = string.Empty;

        string strMfgName = string.Empty;
        string strDeviceSrlNo = string.Empty;
        string strModelNo = string.Empty;

        string strRefPhy = string.Empty;
        string strStudyTypeName1 = string.Empty;
        string strStudyTypeName2 = string.Empty;
        string strStudyTypeName3 = string.Empty;
        string strStudyTypeName4 = string.Empty;

        string strReason = string.Empty;
        string strPhysNote = string.Empty;
        int intImgCnt = 0;
        int intObjCnt = 0;
        string strPACSURL = string.Empty;
        string strIMGVWRURL = string.Empty;
        string strRadiologistName = string.Empty;
        string strPrelimRadiologist = string.Empty;
        string strFinalRadiologist = string.Empty;
        string strPACIMGCNTURL = string.Empty;
        string strPACSUserID = string.Empty;
        string strPACSPwd = string.Empty;
        int intPriorityID = 0;
        string strXML = string.Empty;

        string strWS8SRVIP = string.Empty;
        string strWS8CLTIP = string.Empty;
        string strWS8SRVUID = string.Empty;
        string strWS8SRVPWD = string.Empty;
        string strAPIVER = string.Empty;
        string strWS8URLMSK = string.Empty;
        #endregion

        #region Properties
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
        public decimal PATIENT_WEIGHT_POUNDS
        {
            get { return decPatientWtLbs; }
            set { decPatientWtLbs = value; }
        }
        public decimal PATIENT_WEIGHT_KGS
        {
            get { return decPatientWtKgs; }
            set { decPatientWtKgs = value; }
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
        public string SPECIES_NAME
        {
            get { return strSpeciesName; }
            set { strSpeciesName = value; }
        }

        public string BREED_NAME
        {
            get { return strBreedName; }
            set { strBreedName = value; }
        }
        public string REFERRING_PHYSICIAN
        {
            get { return strRefPhy; }
            set { strRefPhy = value; }
        }
        public string STUDY_UID
        {
            get { return strStudyUID; }
            set { strStudyUID = value; }
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
        public string MODALITY_AE_TITLE
        {
            get { return strModalityAETitle; }
            set { strModalityAETitle = value; }
        }
        public string MODALITY
        {
            get { return strModality; }
            set { strModality = value; }
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

        public DateTime STUDY_DATE
        {
            get { return dtStudyDate; }
            set { dtStudyDate = value; }
        }
        public DateTime RECEIVED_DATE
        {
            get { return dtRecDate; }
            set { dtRecDate = value; }
        }
        public string INSTITUTION_NAME
        {
            get { return strInstitutionName; }
            set { strInstitutionName = value; }
        }


        public string MANUFACTURER_NAME
        {
            get { return strMfgName; }
            set { strMfgName = value; }
        }
        public string DEVICE_SERIAL_NUMBER
        {
            get { return strDeviceSrlNo; }
            set { strDeviceSrlNo = value; }
        }
        public string MANUFACTURER_MODEL_NUMBER
        {
            get { return strModelNo; }
            set { strModelNo = value; }
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
        public int IMAGE_COUNT
        {
            get { return intImgCnt; }
            set { intImgCnt = value; }
        }
        public int OBJECT_COUNT
        {
            get { return intObjCnt; }
            set { intObjCnt = value; }
        }

        public string STUDY_TYPE_NAME_1
        {
            get { return strStudyTypeName1; }
            set { strStudyTypeName1 = value; }
        }
        public string STUDY_TYPE_NAME_2
        {
            get { return strStudyTypeName2; }
            set { strStudyTypeName2 = value; }
        }
        public string STUDY_TYPE_NAME_3
        {
            get { return strStudyTypeName3; }
            set { strStudyTypeName3 = value; }
        }
        public string STUDY_TYPE_NAME_4
        {
            get { return strStudyTypeName4; }
            set { strStudyTypeName4 = value; }
        }
        public string SALES_PERSON
        {
            get { return strSalesPerson; }
            set { strSalesPerson = value; }
        }
        public string RADIOLOGIST_NAME
        {
            get { return strRadiologistName; }
            set { strRadiologistName = value; }
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
        public string PACS_IMAGE_COUNT_URL
        {
            get { return strPACIMGCNTURL; }
            set { strPACIMGCNTURL = value; }
        }
        public string PACS_USER_ID
        {
            get { return strPACSUserID; }
            set { strPACSUserID = value; }
        }
        public string PACS_USER_PASSWORD
        {
            get { return strPACSPwd; }
            set { strPACSPwd = value; }
        }
        public string PACS_STUDY_CHECK_URL
        {
            get { return strPACSURL; }
            set { strPACSURL = value; }
        }
        public int PRIORITY_ID
        {
            get { return intPriorityID; }
            set { intPriorityID = value; }
        }

        public string WEB_SERVICE_SERVER_URL
        {
            get { return strWS8SRVIP; }
            set { strWS8SRVIP = value; }
        }
        public string WEB_SERVICE_SERVER_URL_MASKED
        {
            get { return strWS8URLMSK; }
            set { strWS8URLMSK = value; }
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
        public string API_VERSION
        {
            get { return strAPIVER; }
            set { strAPIVER = value; }
        }
        #endregion

        #region FetchParameters
        public bool FetchParameters(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            DataSet ds = new DataSet();

            try
            {
                SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "hk_missing_study_fetch_params", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Params";

                    foreach (DataRow dr in ds.Tables["Params"].Rows)
                    {
                        strPACIMGCNTURL = Convert.ToString(dr["PACIMGCNTURL"]).Trim();
                        strPACSUserID   = Convert.ToString(dr["pacs_login_id"]).Trim();
                        strPACSPwd      = Convert.ToString(dr["pacs_password"]).Trim();
                        strPACSURL      = Convert.ToString(dr["NEWDATAURL"]).Trim();
                        strWS8SRVIP     = Convert.ToString(dr["WS8SRVIP"]).Trim();
                        strWS8URLMSK    = Convert.ToString(dr["WS8URLMSK"]).Trim();
                        strWS8CLTIP     = Convert.ToString(dr["WS8CLTIP"]).Trim();
                        strWS8SRVUID    = Convert.ToString(dr["WS8SRVUID"]).Trim();
                        strWS8SRVPWD    = Convert.ToString(dr["WS8SRVPWD"]).Trim();
                        strAPIVER       = Convert.ToString(dr["APIVER"]).Trim();
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

        #region SaveSynchedData
        public bool SaveSynchedData(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[40];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                SqlRecordParams[0] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strStudyUID;
                SqlRecordParams[1] = new SqlParameter("@study_date", SqlDbType.DateTime); SqlRecordParams[1].Value = dtStudyDate;
                SqlRecordParams[2] = new SqlParameter("@received_date", SqlDbType.DateTime); SqlRecordParams[2].Value = dtRecDate;
                SqlRecordParams[3] = new SqlParameter("@accession_no", SqlDbType.NVarChar, 20); SqlRecordParams[3].Value = strAccnNo;
                SqlRecordParams[4] = new SqlParameter("@reason", SqlDbType.NVarChar, 2000); SqlRecordParams[4].Value = strReason;
                SqlRecordParams[5] = new SqlParameter("@institution_name", SqlDbType.NVarChar, 100); SqlRecordParams[5].Value = strInstitutionName;
                SqlRecordParams[6] = new SqlParameter("@manufacturer_name", SqlDbType.NVarChar, 100); SqlRecordParams[6].Value = strMfgName;
                SqlRecordParams[7] = new SqlParameter("@device_serial_no", SqlDbType.NVarChar, 20); SqlRecordParams[7].Value = strDeviceSrlNo;
                SqlRecordParams[8] = new SqlParameter("@referring_physician", SqlDbType.NVarChar, 200); SqlRecordParams[8].Value = strRefPhy;
                SqlRecordParams[9] = new SqlParameter("@patient_id", SqlDbType.NVarChar, 20); SqlRecordParams[9].Value = strPatientID;
                SqlRecordParams[10] = new SqlParameter("@patient_name", SqlDbType.NVarChar, 100); SqlRecordParams[10].Value = strPatientName;
                SqlRecordParams[11] = new SqlParameter("@patient_sex", SqlDbType.NVarChar, 10); SqlRecordParams[11].Value = strPatientGender;
                SqlRecordParams[12] = new SqlParameter("@patient_dob", SqlDbType.DateTime); SqlRecordParams[12].Value = dtPatientDob;
                SqlRecordParams[13] = new SqlParameter("@patient_age", SqlDbType.NVarChar, 50); SqlRecordParams[13].Value = strPatientAge;
                SqlRecordParams[14] = new SqlParameter("@patient_weight_lbs", SqlDbType.Decimal); SqlRecordParams[14].Value = decPatientWtLbs;
                SqlRecordParams[15] = new SqlParameter("@owner_name", SqlDbType.NVarChar, 100); SqlRecordParams[15].Value = strOwnerPACS;
                SqlRecordParams[16] = new SqlParameter("@species", SqlDbType.NVarChar, 30); SqlRecordParams[16].Value = strSpeciesName;
                SqlRecordParams[17] = new SqlParameter("@breed", SqlDbType.NVarChar, 50); SqlRecordParams[17].Value = strBreedName;
                SqlRecordParams[18] = new SqlParameter("@modality", SqlDbType.NVarChar, 50); SqlRecordParams[18].Value = strModality;
                SqlRecordParams[19] = new SqlParameter("@body_part", SqlDbType.NVarChar, 50); SqlRecordParams[19].Value = strBodyPart;
                SqlRecordParams[20] = new SqlParameter("@manufacturer_model_no", SqlDbType.NVarChar, 50); SqlRecordParams[20].Value = strModelNo;
                SqlRecordParams[21] = new SqlParameter("@sex_neutered", SqlDbType.NVarChar, 30); SqlRecordParams[21].Value = strSpayedNeutered;
                SqlRecordParams[22] = new SqlParameter("@img_count", SqlDbType.Int); SqlRecordParams[22].Value = intImgCnt;
                SqlRecordParams[23] = new SqlParameter("@study_desc", SqlDbType.NVarChar, 500); SqlRecordParams[23].Value = strStudyDesc;
                SqlRecordParams[24] = new SqlParameter("@modality_ae_title", SqlDbType.NVarChar, 500); SqlRecordParams[24].Value = strModalityAETitle;
                SqlRecordParams[25] = new SqlParameter("@priority_id", SqlDbType.Int); SqlRecordParams[25].Value = intPriorityID;
                SqlRecordParams[26] = new SqlParameter("@radiologist", SqlDbType.NVarChar, 250); SqlRecordParams[26].Value = strRadiologistName;
                SqlRecordParams[27] = new SqlParameter("@study_status_pacs", SqlDbType.Int); SqlRecordParams[27].Value = intStatus;
                SqlRecordParams[28] = new SqlParameter("@study_type_1", SqlDbType.NVarChar, 50); SqlRecordParams[28].Value = strStudyTypeName1;
                SqlRecordParams[29] = new SqlParameter("@study_type_2", SqlDbType.NVarChar, 50); SqlRecordParams[29].Value = strStudyTypeName2;
                SqlRecordParams[30] = new SqlParameter("@study_type_3", SqlDbType.NVarChar, 50); SqlRecordParams[30].Value = strStudyTypeName3;
                SqlRecordParams[31] = new SqlParameter("@study_type_4", SqlDbType.NVarChar, 50); SqlRecordParams[31].Value = strStudyTypeName4;
                SqlRecordParams[32] = new SqlParameter("@sales_person", SqlDbType.NVarChar, 100); SqlRecordParams[32].Value = strSalesPerson;
                SqlRecordParams[33] = new SqlParameter("@patient_weight_kgs", SqlDbType.Decimal); SqlRecordParams[33].Value = decPatientWtKgs;
                SqlRecordParams[34] = new SqlParameter("@object_count", SqlDbType.Int); SqlRecordParams[34].Value = intObjCnt;
                SqlRecordParams[35] = new SqlParameter("@physician_note", SqlDbType.NVarChar, 2000); SqlRecordParams[35].Value = strPhysNote;
                SqlRecordParams[36] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[36].Value = UserID;
                SqlRecordParams[37] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[37].Value = intMenuID;
                SqlRecordParams[38] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[38].Direction = ParameterDirection.Output;
                SqlRecordParams[39] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[39].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "hk_missing_study_synch_save", SqlRecordParams);

                intReturnType = Convert.ToInt32(SqlRecordParams[39].Value);
                if (intReturnType == 0)
                {
                    bReturn = false;
                }
                else
                {
                    bReturn = true;
                }

                ReturnMessage = Convert.ToString(SqlRecordParams[38].Value);
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region CheckMissingStudies
        public bool CheckMissingStudies(string ConfigPath, MissingStudy[] ArrObj,ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];


            try
            {
                if (GenerateXML(ArrObj, ref CatchMessage))
                {
                    SqlRecordParams[0] = new SqlParameter("@xml_data", SqlDbType.NText); SqlRecordParams[0].Value = strXML;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "hk_missing_study_check", SqlRecordParams);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "Studies";
                        bReturn = true;
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

        #region GenerateXML
        private bool GenerateXML(MissingStudy[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<data>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<study_uid>" + ArrObj[i].STUDY_UID + "</study_uid>");
                        sbXML.Append("<received_date><![CDATA[" + ArrObj[i].RECEIVED_DATE.ToString("ddMMMyyyy HH:mm") + "]]></received_date>");
                        sbXML.Append("<institution_name><![CDATA[" + ArrObj[i].INSTITUTION_NAME + "]]></institution_name>");
                        sbXML.Append("<patient_name><![CDATA[" + ArrObj[i].PATIENT_NAME + "]]></patient_name>");
                        sbXML.Append("<status_id>" + ArrObj[i].STATUS.ToString() + "</status_id>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</data>");
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


    }
}
