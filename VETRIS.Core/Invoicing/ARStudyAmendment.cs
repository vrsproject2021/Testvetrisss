using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using VETRIS.DAL;

namespace VETRIS.Core.Invoicing
{
    public class ARStudyAmendment
    {
        #region Constructor
        public ARStudyAmendment()
        {

        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        Guid CycleID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid AccountID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid InstID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid StudyID = new Guid("00000000-0000-0000-0000-000000000000");
        int intModalityID = 0;
        int intPriorityID = 0;
        int intCategoryID = 0;
        string strPatientName = string.Empty;
        string strSericeCodes = string.Empty;
        string strUserName = string.Empty;
        string strXML = string.Empty;
        string strRateXML = string.Empty;
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
        public Guid BILLING_CYCLE_ID
        {
            get { return CycleID; }
            set { CycleID = value; }
        }
        public Guid BILLING_ACCOUNT_ID
        {
            get { return AccountID; }
            set { AccountID = value; }
        }
        public Guid INSTITUTION_ID
        {
            get { return InstID; }
            set { InstID = value; }
        }
        public string PATIENT_NAME
        {
            get { return strPatientName; }
            set { strPatientName = value; }
        }
        public Guid STUDY_ID
        {
            get { return StudyID; }
            set { StudyID = value; }
        }
        public int MODALITY_ID
        {
            get { return intModalityID; }
            set { intModalityID = value; }
        }
        public int PRIORITY_ID
        {
            get { return intPriorityID; }
            set { intPriorityID = value; }
        }
        public int CATEGORY_ID
        {
            get { return intCategoryID; }
            set { intCategoryID = value; }
        }
        public string SERVICE_CODES
        {
            get { return strSericeCodes; }
            set { strSericeCodes = value; }
        }
        public string USER_NAME
        {
            get { return strUserName; }
            set { strUserName = value; }
        }
        #endregion

        #region FetchParameters
        public bool FetchParameters(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserID;
                SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = AccountID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_study_corrections_params_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Cycle";
                    ds.Tables[1].TableName = "Institution";
                    ds.Tables[2].TableName = "Modality";
                    ds.Tables[3].TableName = "Priority";
                    ds.Tables[4].TableName = "APIParams";
                    ds.Tables[5].TableName = "Category";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadDetails
        public bool LoadDetails(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intReturnValue = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[9];

            try
            {
                if (ValidateLoad(ref ReturnMessage))
                {
                    SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = CycleID;
                    SqlRecordParams[1] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = InstID;
                    SqlRecordParams[2] = new SqlParameter("@patient_name", SqlDbType.NVarChar, 100); SqlRecordParams[2].Value = strPatientName;
                    SqlRecordParams[3] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[3].Value = intModalityID;
                    SqlRecordParams[4] = new SqlParameter("@category_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = intCategoryID;
                    SqlRecordParams[5] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[5].Value = intMenuID;
                    SqlRecordParams[6] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[6].Value = UserID;
                    SqlRecordParams[7] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[7].Direction = ParameterDirection.Output;
                    SqlRecordParams[8] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[8].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "ar_study_amendment_invoice_fetch", SqlRecordParams);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "Details";
                        ds.Tables[1].TableName = "Rates";
                        bReturn = true;
                    }
                    else
                    {
                        intReturnValue = Convert.ToInt32(SqlRecordParams[8].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;


                        ReturnMessage = Convert.ToString(SqlRecordParams[7].Value);
                    }
                }
                else
                    bReturn = false;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region ValidateLoad
        private bool ValidateLoad(ref string ReturnMessage)
        {
            bool bReturn = true;
            if (CycleID == new Guid("00000000-0000-0000-0000-000000000000"))
            {

                ReturnMessage = "229";
            }
            //if (InstID == new Guid("00000000-0000-0000-0000-000000000000"))
            //{
            //    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
            //    ReturnMessage += "280";
            //}

            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }

        #endregion

        #region FetchServiceCodes
        public bool FetchServiceCodes(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = StudyID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_study_correction_service_code_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Services";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region DeleteRecord
        public bool DeleteRecord(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;
            string strArchiveFolder = string.Empty;
            string[] arrFiles = new string[0];

            try
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[8];
                SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = CycleID;
                SqlRecordParams[1] = new SqlParameter("@study_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = StudyID;
                SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = intMenuID;
                SqlRecordParams[3] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = UserID;
                SqlRecordParams[4] = new SqlParameter("@archive_folder", SqlDbType.NVarChar, 700); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[6].Direction = ParameterDirection.Output;
                SqlRecordParams[7] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[7].Direction = ParameterDirection.Output;


                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_study_correction_delete", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[7].Value);
                if (intReturnValue == 1)
                {
                    bReturn = true;
                    strArchiveFolder = Convert.ToString(SqlRecordParams[4].Value).Trim();

                    #region delete files
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
                    #endregion
                }
                else
                    bReturn = false;

                strUserName = Convert.ToString(SqlRecordParams[5].Value).Trim();
                ReturnMessage = Convert.ToString(SqlRecordParams[6].Value).Trim();


            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }




            return bReturn;
        }
        #endregion

        #region ReprocessInvoice
        public bool ReprocessInvoice(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            
                
            try
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[8];
                SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = CycleID;
                SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = AccountID;
                SqlRecordParams[2] = new SqlParameter("@with_tran", SqlDbType.NChar,1); SqlRecordParams[2].Value = "Y";
                SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[3].Value = intMenuID;
                SqlRecordParams[4] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = UserID;
                SqlRecordParams[5] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[6].Direction = ParameterDirection.Output;
                SqlRecordParams[7] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[7].Direction = ParameterDirection.Output;


                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_study_correction_invoice_reprocess", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[7].Value);
                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;

                strUserName = Convert.ToString(SqlRecordParams[5].Value).Trim();
                ReturnMessage = Convert.ToString(SqlRecordParams[6].Value).Trim();


            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, ARStudyAmendment[] ArrStudyObj,ARAmendedRateList[] ArrRateObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRecord(ArrStudyObj, ref ReturnMessage))
            {
                if ((GenerateStudyXML(ArrStudyObj, ref CatchMessage)) && (GenerateRateXML(ArrRateObj, ref CatchMessage)))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[8];
                        SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = CycleID;
                        SqlRecordParams[1] = new SqlParameter("@xml_study", SqlDbType.NText); SqlRecordParams[1].Value = strXML.Trim();
                        SqlRecordParams[2] = new SqlParameter("@xml_rates", SqlDbType.NText); if (strRateXML.Trim() == string.Empty) SqlRecordParams[2].Value = DBNull.Value; else SqlRecordParams[2].Value = strRateXML.Trim();
                        SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[3].Value = intMenuID;
                        SqlRecordParams[4] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = UserID;
                        SqlRecordParams[5] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[5].Direction = ParameterDirection.Output;
                        SqlRecordParams[6] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[6].Direction = ParameterDirection.Output;
                        SqlRecordParams[7] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[7].Direction = ParameterDirection.Output;


                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_study_correction_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[7].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[5].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[6].Value).Trim();


                    }
                    catch (Exception expErr)
                    { bReturn = false; CatchMessage = expErr.Message; }
                }
                else
                {
                    bReturn = false;

                }

            }
            else
            {
                bReturn = false;
            }

            return bReturn;
        }
        #endregion

        #region ValidateRecord
        private bool ValidateRecord(ARStudyAmendment[] ArrStudyObj, ref string ReturnMessage)
        {
            bool bReturn = true;


            if (CycleID == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                ReturnMessage = "229";
            }
            if (ArrStudyObj.Length == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "237";
            }
            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }


        #endregion

        #region GenerateStudyXML
        private bool GenerateStudyXML(ARStudyAmendment[] ArrObj, ref string CatchMessage)
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
                        sbXML.Append("<study_id><![CDATA[" + ArrObj[i].STUDY_ID.ToString() + "]]></study_id>");
                        sbXML.Append("<institution_id><![CDATA[" + ArrObj[i].INSTITUTION_ID.ToString() + "]]></institution_id>");
                        sbXML.Append("<modality_id>" + ArrObj[i].MODALITY_ID.ToString() + "</modality_id>");
                        sbXML.Append("<priority_id>" + ArrObj[i].PRIORITY_ID.ToString() + "</priority_id>");
                        sbXML.Append("<category_id>" + ArrObj[i].CATEGORY_ID.ToString() + "</category_id>");
                        sbXML.Append("<patient_name><![CDATA[" + ArrObj[i].PATIENT_NAME.ToString() + "]]></patient_name>");
                        sbXML.Append("<service_codes><![CDATA[" + ArrObj[i].SERVICE_CODES + "]]></service_codes>");
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

        #region GenerateRateXML
        private bool GenerateRateXML(ARAmendedRateList[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<rate>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<study_id><![CDATA[" + ArrObj[i].STUDY_ID.ToString() + "]]></study_id>");
                        sbXML.Append("<study_uid><![CDATA[" + ArrObj[i].STUDY_UID.ToString() + "]]></study_uid>");
                        sbXML.Append("<institution_id><![CDATA[" + ArrObj[i].INSTITUTION_ID.ToString() + "]]></institution_id>");
                        sbXML.Append("<rate>" + ArrObj[i].RATE.ToString() + "</rate>");
                        sbXML.Append("<head_type><![CDATA[" + ArrObj[i].HEAD_TYPE.Trim() + "]]></head_type>");
                        sbXML.Append("<head_id>" + ArrObj[i].HEAD_ID.ToString() + "</head_id>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</rate>");
                    strRateXML = sbXML.ToString();


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

    public class ARAmendedRateList
    {
        #region Constructor
        public ARAmendedRateList()
        {
        }
        #endregion

        #region Variables
        Guid StudyID = Guid.Empty;
        string strStudyUid = string.Empty;
        Guid InstitutionID = Guid.Empty;
        Double dblRate = 0;
        //Double dblRatePerUnit = 0;
        //Double dblStudyMaxAmt = 0;
        //Double dblRateAfterHours = 0;
        string strHeadType = string.Empty;
        int intHeadID = 0;
        #endregion

        #region Properties
        public Guid STUDY_ID
        {
            get { return StudyID; }
            set { StudyID = value; }
        }
        public string STUDY_UID
        {
            get { return strStudyUid; }
            set { strStudyUid = value; }
        }
        public Guid INSTITUTION_ID
        {
            get { return InstitutionID; }
            set { InstitutionID = value; }
        }
        public double RATE
        {
            get { return dblRate; }
            set { dblRate = value; }
        }
       
        public string HEAD_TYPE
        {
            get { return strHeadType; }
            set { strHeadType = value; }
        }
        public int HEAD_ID
        {
            get { return intHeadID; }
            set { intHeadID = value; }
        }
        #endregion
    }
}
