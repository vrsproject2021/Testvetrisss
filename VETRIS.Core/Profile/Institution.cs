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

namespace VETRIS.Core.Profile
{
    public class Institution
    {
        #region Constructor
        public Institution()
        {

        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        string strUserName = string.Empty;

        Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
        string strCode = string.Empty;
        string strName = string.Empty;
        string strAddress1 = string.Empty;
        string strAddress2 = string.Empty;
        string strCity = string.Empty;
        int intStateID = 0;
        string strStateName = string.Empty;
        int intCountryID = 231;
        string strCountryName = string.Empty;
        string strZip = string.Empty;
        string strPhone = string.Empty;
        string strMobile = string.Empty;
        string strEmail = string.Empty;
        string strContactPersonName = string.Empty;
        string strContactPersonMobile = string.Empty;
        string strNotes = string.Empty;
        Guid SalespersonId = new Guid("00000000-0000-0000-0000-000000000000");
        decimal decCommission1stYr = 0;// Added on 4th SEP 2019 @BK
        decimal decCommission2ndYr = 0;// Added on 4th SEP 2019 @BK
        decimal decDiscPer = 0;
        string strAccountantName = string.Empty;// Added on 3rd SEP 2019 @BK
        string strIsActive = "Y";
        string strLoginID = string.Empty;
        string strInfoSrc = string.Empty;
        int intBInfoSrc = 0;
        string strExitingBillAcctLink = "N";
        Guid BillAcctId = new Guid("00000000-0000-0000-0000-000000000000");
        string strBillAcctName = string.Empty;
        string strFmtDCMFiles = "N";
        string strDCMFileXferMode = "N";
        string strStudyImgManualRecPath = string.Empty;
        string strUSRUPDURL = string.Empty;
        string strXMLDevice = string.Empty;
        string strXMLPhysician = string.Empty;
        string strXMLUser = string.Empty;
        string strXMLSalesPerson = string.Empty;
        string strXMLFee = string.Empty;
        string strXMLTag = string.Empty;
        string CatchMessage = string.Empty;
        Guid PhysicianID = new Guid("00000000-0000-0000-0000-000000000000");

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
        public string CODE
        {
            get { return strCode; }
            set { strCode = value; }
        }
        public string NAME
        {
            get { return strName; }
            set { strName = value; }
        }
        public string ADDRESS_LINE1
        {
            get { return strAddress1; }
            set { strAddress1 = value; }
        }
        public string ADDRESS_LINE2
        {
            get { return strAddress2; }
            set { strAddress2 = value; }
        }
        public string CITY
        {
            get { return strCity; }
            set { strCity = value; }
        }
        public int STATE_ID
        {
            get { return intStateID; }
            set { intStateID = value; }
        }
        public string STATE_NAME
        {
            get { return strStateName; }
            set { strStateName = value; }
        }
        public int COUNTRY_ID
        {
            get { return intCountryID; }
            set { intCountryID = value; }
        }
        public string COUNTRY_NAME
        {
            get { return strCountryName; }
            set { strCountryName = value; }
        }
        public string ZIP
        {
            get { return strZip; }
            set { strZip = value; }
        }
        public string PHONE
        {
            get { return strPhone; }
            set { strPhone = value; }
        }
        public string MOBILE
        {
            get { return strMobile; }
            set { strMobile = value; }
        }
        public string EMAIL_ID
        {
            get { return strEmail; }
            set { strEmail = value; }
        }
        public string CONTACT_PERSON_NAME
        {
            get { return strContactPersonName; }
            set { strContactPersonName = value; }
        }
        public string CONTACT_PERSION_MOBILE
        {
            get { return strContactPersonMobile; }
            set { strContactPersonMobile = value; }
        }
        public string NOTES
        {
            get { return strNotes; }
            set { strNotes = value; }
        }
        public string LOGIN_ID
        {
            get { return strLoginID; }
            set { strLoginID = value; }
        }
        public Guid SALES_PERSON_ID
        {
            get { return SalespersonId; }
            set { SalespersonId = value; }
        }
        public decimal COMMISSION_1ST_YEAR// Added on 4th SEP 2019 @BK
        {
            get { return decCommission1stYr; }
            set { decCommission1stYr = value; }
        }
        public decimal COMMISSION_2ND_YEAR// Added on 4th SEP 2019 @BK
        {
            get { return decCommission2ndYr; }
            set { decCommission2ndYr = value; }
        }
        public decimal DISCOUNT_PERCENT
        {
            get { return decDiscPer; }
            set { decDiscPer = value; }
        }
        public Guid PHYSICIAN_ID
        {
            get { return PhysicianID; }
            set { PhysicianID = value; }
        }

        public string ACCOUNTANT_NAME // Added on 3rd SEP 2019 @BK
        {
            get { return strAccountantName; }
            set { strAccountantName = value; }
        }
        public string SOURCE_OF_INFORMATION
        {
            get { return strInfoSrc; }
            set { strInfoSrc = value; }
        }
        public int BUSSINESS_SOURCE_ID
        {
            get { return intBInfoSrc; }
            set { intBInfoSrc = value; }

        }
        public string LINKED_TO_EXISTING_BILLING_ACCOUNT
        {
            get { return strExitingBillAcctLink; }
            set { strExitingBillAcctLink = value; }
        }
        public Guid BILLING_ACCOUNT_ID
        {
            get { return BillAcctId; }
            set { BillAcctId = value; }
        }
        public string BILLING_ACCOUNT_NAME
        {
            get { return strBillAcctName; }
            set { strBillAcctName = value; }
        }
        public string FORMAT_DICOM_FILES
        {
            get { return strFmtDCMFiles; }
            set { strFmtDCMFiles = value; }
        }
        public string DICOM_FILES_TRANSFER_METHOD
        {
            get { return strDCMFileXferMode; }
            set { strDCMFileXferMode = value; }
        }
        public string STUDY_IMAGE_FILES_MANUAL_RECEIVING_PATH
        {
            get { return strStudyImgManualRecPath; }
            set { strStudyImgManualRecPath = value; }
        }
        public string IS_ACTIVE
        {
            get { return strIsActive; }
            set { strIsActive = value; }
        }
        public string USER_UPDATE_URL
        {
            get { return strUSRUPDURL; }
            set { strUSRUPDURL = value; }
        }

        #endregion

        #region Browser Methods

        #region FetchBrowserParameters
        public bool FetchBrowserParameters(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_brw_fetch_params");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Country";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region SearchBrowserList
        public bool SearchBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[8];
            SqlRecordParams[0] = new SqlParameter("@code", SqlDbType.NVarChar, 5); SqlRecordParams[0].Value = strCode;
            SqlRecordParams[1] = new SqlParameter("@name", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strName;
            SqlRecordParams[2] = new SqlParameter("@city", SqlDbType.NVarChar, 100); SqlRecordParams[2].Value = strCity;
            SqlRecordParams[3] = new SqlParameter("@zip", SqlDbType.NVarChar, 20); SqlRecordParams[3].Value = strZip;
            SqlRecordParams[4] = new SqlParameter("@country_id", SqlDbType.NVarChar, 20); SqlRecordParams[4].Value = intCountryID;
            SqlRecordParams[5] = new SqlParameter("@state_id", SqlDbType.NVarChar, 20); SqlRecordParams[5].Value = intStateID;
            SqlRecordParams[6] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[6].Value = strIsActive;
            SqlRecordParams[7] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[7].Value = UserID;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "profile_institution_fetch_brw", SqlRecordParams);
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

        #endregion

        #region FetchCountryWiseStates
        public bool FetchCountryWiseStates(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@country_id", SqlDbType.Int); SqlRecordParams[0].Value = intCountryID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "country_wise_state_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "States";
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

        #region Dialog Methods

        #region LoadDetails
        public bool LoadDetails(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[5];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = intMenuID;
                SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_institution_fetch_dtls", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";
                    ds.Tables[1].TableName = "Country";
                    ds.Tables[2].TableName = "States";


                    #region Details

                    foreach (DataRow dr in ds.Tables["Details"].Rows)
                    {
                        strCode = Convert.ToString(dr["code"]).Trim();
                        strName = Convert.ToString(dr["name"]).Trim();
                        strAddress1 = Convert.ToString(dr["address_1"]).Trim();
                        strAddress2 = Convert.ToString(dr["address_2"]).Trim();
                        strCity = Convert.ToString(dr["city"]).Trim();
                        intStateID = Convert.ToInt32(dr["state_id"]);
                        strStateName = Convert.ToString(dr["state_name"]).Trim();
                        intCountryID = Convert.ToInt32(dr["country_id"]);
                        strCountryName = Convert.ToString(dr["coutry_name"]).Trim();
                        strZip = Convert.ToString(dr["zip"]).Trim();
                        strEmail = Convert.ToString(dr["email_id"]).Trim();
                        strPhone = Convert.ToString(dr["phone_no"]).Trim();
                        strMobile = Convert.ToString(dr["mobile_no"]).Trim();
                        strContactPersonName = Convert.ToString(dr["contact_person_name"]).Trim();
                        strContactPersonMobile = Convert.ToString(dr["contact_person_mobile"]).Trim();
                        strExitingBillAcctLink = Convert.ToString(dr["link_existing_bill_acct"]).Trim();
                        BillAcctId = new Guid(Convert.ToString(dr["billing_account_id"]));
                        strBillAcctName = Convert.ToString(dr["billing_acct_name"]).Trim();
                        strDCMFileXferMode = Convert.ToString(dr["dcm_file_xfer_pacs_mode"]).Trim();
                        strStudyImgManualRecPath = Convert.ToString(dr["study_img_manual_receive_path"]).Trim();
                        strIsActive = Convert.ToString(dr["is_active"]).Trim();
                    }


                    #endregion

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

        #region LoadPhysicians
        public bool LoadPhysicians(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_institution_fetch_physicians", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Physicians";
                    //foreach (DataRow dr in ds.Tables["Physicians"].Rows)
                    //{
                    //    if (Convert.ToString(dr["physician_pacs_password"]).Trim() != string.Empty)
                    //        dr["physician_pacs_password"] = CoreCommon.DecryptString(Convert.ToString(dr["physician_pacs_password"]));
                    //}
                    //ds.Tables["Physicians"].AcceptChanges();
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadUsers
        public bool LoadUsers(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_institution_fetch_users", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Users";
                    foreach (DataRow dr in ds.Tables["Users"].Rows)
                    {
                        if (Convert.ToString(dr["password"]).Trim() != string.Empty)
                            dr["password"] = CoreCommon.DecryptString(Convert.ToString(dr["password"]));
                        if (Convert.ToString(dr["pacs_password"]).Trim() != string.Empty)
                            dr["pacs_password"] = CoreCommon.DecryptString(Convert.ToString(dr["pacs_password"]));
                    }
                    ds.Tables["Users"].AcceptChanges();
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadPromotions
        public bool LoadPromotions(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_institution_fetch_promotions", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Promotions";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchPhysicianDetails
        public bool FetchPhysicianDetails(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[3];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@email_id", SqlDbType.NVarChar, 50); SqlRecordParams[0].Value = strEmail;
                SqlRecordParams[1] = new SqlParameter("@mobile_no", SqlDbType.NVarChar, 20); SqlRecordParams[1].Value = strMobile;
                SqlRecordParams[2] = new SqlParameter("@name", SqlDbType.NVarChar, 200); SqlRecordParams[2].Value = strName;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_fetch_physician_dtls", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Physician";
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

        #region FetchPhysicianEmail
        public bool FetchPhysicianEmail(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[2];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@physician_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = PhysicianID;
                SqlRecordParams[1] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = Id;


                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "profile_physician_email_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Emails";
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

        #region FetchUserDetails
        public bool FetchUserDetails(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@login_id", SqlDbType.NVarChar, 50); SqlRecordParams[0].Value = strLoginID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_fetch_user_dtls", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "User";
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
        public bool SaveRecord(string ConfigPath, PhysicianList[] ArrPhysObj, InstitutionUserList[] ArrUserObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;


            if ((GeneratePhysicianXML(ArrPhysObj, ref ReturnMessage, ref CatchMessage)) && (GenerateUserXML(ArrUserObj, ref ReturnMessage, ref CatchMessage)))
            {
                try
                {
                    SqlParameter[] SqlRecordParams = new SqlParameter[13];
                    SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                    SqlRecordParams[1] = new SqlParameter("@email_id", SqlDbType.NVarChar, 50); SqlRecordParams[1].Value = strEmail;
                    SqlRecordParams[2] = new SqlParameter("@phone", SqlDbType.NVarChar, 30); SqlRecordParams[2].Value = strPhone;
                    SqlRecordParams[3] = new SqlParameter("@mobile", SqlDbType.NVarChar, 20); SqlRecordParams[3].Value = strMobile;
                    SqlRecordParams[4] = new SqlParameter("@contact_person_name", SqlDbType.NVarChar, 100); SqlRecordParams[4].Value = strContactPersonName;
                    SqlRecordParams[5] = new SqlParameter("@contact_person_mob", SqlDbType.NVarChar, 100); SqlRecordParams[5].Value = strContactPersonMobile;
                    SqlRecordParams[6] = new SqlParameter("@xml_physician", SqlDbType.NText); if (strXMLPhysician.Trim() != string.Empty) SqlRecordParams[6].Value = strXMLPhysician.Trim(); else SqlRecordParams[6].Value = DBNull.Value;
                    SqlRecordParams[7] = new SqlParameter("@xml_user", SqlDbType.NText); if (strXMLUser.Trim() != string.Empty) SqlRecordParams[7].Value = strXMLUser.Trim(); else SqlRecordParams[7].Value = DBNull.Value;
                    SqlRecordParams[8] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[8].Value = UserID;
                    SqlRecordParams[9] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[9].Value = intMenuID;
                    SqlRecordParams[10] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[10].Direction = ParameterDirection.Output;
                    SqlRecordParams[11] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[11].Direction = ParameterDirection.Output;
                    SqlRecordParams[12] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[12].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "profile_institution_save", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[12].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;

                    strUserName = Convert.ToString(SqlRecordParams[10].Value).Trim();
                    ReturnMessage = Convert.ToString(SqlRecordParams[11].Value).Trim();

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

        #region GeneratePhysicianXML
        private bool GeneratePhysicianXML(PhysicianList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<physician>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        #region Validations
                        if ((ArrObj[i].FIRST_NAME.Trim() + " " + ArrObj[i].LAST_NAME.Trim()).Trim() == string.Empty)
                        {
                            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                            ReturnMessage = "087";
                            strUserName = (i + 1).ToString();
                        }


                        //if (ArrObj[i].EMAIL_ID.Trim() != string.Empty)
                        //{
                        //    if (!CoreCommon.IsEmailValid(ArrObj[i].EMAIL_ID.Trim()))
                        //    {
                        //        if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                        //        ReturnMessage += "089";
                        //        //ReturnMessage += ArrObj[i].EMAIL_ID.Trim();
                        //        strUserName = (i + 1).ToString();
                        //    }
                        //}
                        if (ReturnMessage.Trim() != string.Empty)
                        {
                            break;
                        }
                        #endregion

                        sbXML.Append("<row>");
                        sbXML.Append("<physician_id>" + ArrObj[i].ID.ToString() + "</physician_id>");
                        sbXML.Append("<physician_fname><![CDATA[" + ArrObj[i].FIRST_NAME + "]]></physician_fname>");
                        sbXML.Append("<physician_lname><![CDATA[" + ArrObj[i].LAST_NAME + "]]></physician_lname>");
                        sbXML.Append("<physician_credentials><![CDATA[" + ArrObj[i].CREDENTIALS + "]]></physician_credentials>");
                        sbXML.Append("<physician_email><![CDATA[" + ArrObj[i].EMAIL_ID + "]]></physician_email>");
                        sbXML.Append("<physician_mobile><![CDATA[" + ArrObj[i].MOBILE_NUMBER + "]]></physician_mobile>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    if (ReturnMessage.Trim() != string.Empty)
                    {
                        bReturn = false;
                        sbXML.Clear();
                        strXMLPhysician = string.Empty;
                    }
                    else
                    {
                        bReturn = true;
                        sbXML.Append("</physician>");
                        strXMLPhysician = sbXML.ToString();
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

        #region GenerateUserXML
        private bool GenerateUserXML(InstitutionUserList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<user>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        #region Validations
                        if ((ArrObj[i].LOGIN_ID).Trim() == string.Empty)
                        {
                            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                            ReturnMessage = "115";
                            strUserName = (i + 1).ToString();
                        }


                        if (ArrObj[i].PASSWORD.Trim() == string.Empty)
                        {
                            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                            ReturnMessage += "116";
                            strUserName = (i + 1).ToString();
                        }


                        if (ArrObj[i].EMAIL_ID.Trim() != string.Empty)
                        {
                            if (!CoreCommon.IsEmailValid(ArrObj[i].EMAIL_ID.Trim()))
                            {
                                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                                ReturnMessage += "117";
                                strUserName = (i + 1).ToString();
                            }
                        }

                        if (ArrObj[i].PACS_USER_ID.Trim() == string.Empty)
                        {
                            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                            ReturnMessage += "100";
                            strUserName = (i + 1).ToString();
                        }
                        if (ArrObj[i].PACS_PASSWORD.Trim() == string.Empty)
                        {
                            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                            ReturnMessage += "101";
                            strUserName = (i + 1).ToString();
                        }


                        if (ReturnMessage.Trim() != string.Empty)
                        {
                            break;
                        }
                        #endregion

                        sbXML.Append("<row>");
                        sbXML.Append("<user_user_id><![CDATA[" + ArrObj[i].ID.ToString() + "]]></user_user_id>");
                        sbXML.Append("<user_login_id><![CDATA[" + ArrObj[i].LOGIN_ID + "]]></user_login_id>");
                        sbXML.Append("<user_pwd><![CDATA[" + ArrObj[i].PASSWORD + "]]></user_pwd>");
                        sbXML.Append("<user_pacs_user_id><![CDATA[" + ArrObj[i].LOGIN_ID + "]]></user_pacs_user_id>");
                        sbXML.Append("<user_pacs_password><![CDATA[" + ArrObj[i].PASSWORD + "]]></user_pacs_password>");
                        sbXML.Append("<user_email_id><![CDATA[" + ArrObj[i].EMAIL_ID + "]]></user_email_id>");
                        sbXML.Append("<user_contact_no><![CDATA[" + ArrObj[i].CONTACT_NUMBER + "]]></user_contact_no>");
                        sbXML.Append("<is_active><![CDATA[" + ArrObj[i].IS_ACTIVE + "]]></is_active>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    if (ReturnMessage.Trim() != string.Empty)
                    {
                        bReturn = false;
                        sbXML.Clear();
                        strXMLUser = string.Empty;
                    }
                    else
                    {
                        bReturn = true;
                        sbXML.Append("</user>");
                        strXMLUser = sbXML.ToString();
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

    public class PhysicianList
    {
        #region Constructor
        public PhysicianList()
        {
        }
        #endregion

        #region Variables
        Guid InstId = Guid.Empty;
        Guid Id = Guid.Empty;
        string strFName = string.Empty;
        string strLName = string.Empty;
        string strCred = string.Empty;
        string strLoginEmail = string.Empty;
        string strEmail = string.Empty;
        string strMobile = string.Empty;
        string strPACSUserID = string.Empty;
        string strPACSPwd = string.Empty;
        #endregion

        #region Properties
        public Guid INSTITUTION_ID
        {
            get { return InstId; }
            set { InstId = value; }
        }
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public string FIRST_NAME
        {
            get { return strFName; }
            set { strFName = value; }
        }
        public string LAST_NAME
        {
            get { return strLName; }
            set { strLName = value; }
        }
        public string CREDENTIALS
        {
            get { return strCred; }
            set { strCred = value; }
        }
        public string EMAIL_ID
        {
            get { return strEmail; }
            set { strEmail = value; }
        }
        public string MOBILE_NUMBER
        {
            get { return strMobile; }
            set { strMobile = value; }
        }
        #endregion
    }
    public class InstitutionUserList
    {
        #region Constructor
        public InstitutionUserList()
        {
        }
        #endregion

        #region Variables
        Guid Id = Guid.Empty;
        string strLoginID = string.Empty;
        string strPwd = string.Empty;
        string strEmail = string.Empty;
        string strContactNo = string.Empty;
        string strPACSUserID = string.Empty;
        string strPACSPwd = string.Empty;
        string strIsActive = string.Empty;
        #endregion

        #region Properties
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public string LOGIN_ID
        {
            get { return strLoginID; }
            set { strLoginID = value; }
        }
        public string PASSWORD
        {
            get { return strPwd; }
            set { strPwd = value; }
        }
        public string EMAIL_ID
        {
            get { return strEmail; }
            set { strEmail = value; }
        }
        public string CONTACT_NUMBER
        {
            get { return strContactNo; }
            set { strContactNo = value; }
        }
        public string PACS_USER_ID
        {
            get { return strPACSUserID; }
            set { strPACSUserID = value; }
        }
        public string PACS_PASSWORD
        {
            get { return strPACSPwd; }
            set { strPACSPwd = value; }
        }
        public string IS_ACTIVE
        {
            get { return strIsActive; }
            set { strIsActive = value; }
        }
        #endregion
    }
}
