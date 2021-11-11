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

namespace VETRIS.Core.Login
{
    public class Login
    {
        #region Constructor
        public Login()
        {
        }
        #endregion

        #region Variables
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid SessionID = new Guid("00000000-0000-0000-0000-000000000000");
        string strUserCode = string.Empty;
        string strUserName = string.Empty;
        string strUserContNo = string.Empty;
        string strEmailID = string.Empty;
        string strLoginID = string.Empty;
        int intUserRoleID = 0;
        string strUserRoleCode = string.Empty;


        string strPwd = string.Empty;
        string strFirstLogin = string.Empty;
        string strNewPassword = string.Empty;
        string strConfirmPassword = string.Empty;
        string strMailTemplatePath = string.Empty;
        string strVersion = string.Empty;
        string strBetaVer = string.Empty;

        string CatchMessage = string.Empty;
        string strInstCode = string.Empty;
        string strInstName = string.Empty;
        string strChatURL = string.Empty;
        string strENBLCHAT = "Y";
        string strPACSUserID = string.Empty;
        string strPACSPwd = string.Empty;
        Guid BillingAccountID = new Guid("00000000-0000-0000-0000-000000000000");
        string strBillingAccountName = string.Empty;
        Guid RadiologistID = new Guid("00000000-0000-0000-0000-000000000000");
        string RadiologistTimeZone = string.Empty;

        string strSessID = string.Empty;
        string strWS8SRVIP = string.Empty;
        string strWS8CLTIP = string.Empty;
        string strWS8SRVUID = string.Empty;
        string strWS8SRVPWD = string.Empty;
        string strAPIVER = string.Empty;
        string strRPTEGNURL = string.Empty;

        string strAllowMS = string.Empty;
        string strAllowDBView = string.Empty;
        Guid RegistrationId = new Guid("00000000-0000-0000-0000-000000000000");
        string regLoginId = string.Empty;
        int intDefTimeZoneID = 0;
        string strDefTimeZoneStdName = string.Empty;
        string strPrefTheme = "DEFAULT";
        #endregion

        #region Properties
        public Guid USER_ID
        {
            get { return UserID; }
            set { UserID = value; }
        }
        public string USER_CODE
        {
            get { return strUserCode; }
            set { strUserCode = value; }
        }
        public string USER_NAME
        {
            get { return strUserName; }
            set { strUserName = value; }
        }
        public string USER_EMAIL_ID
        {
            get { return strEmailID; }
            set { strEmailID = value; }
        }
        public string USER_CONTACT_NUMBER
        {
            get { return strUserContNo; }
            set { strUserContNo = value; }
        }
        public string LOGIN_ID
        {
            get { return strLoginID; }
            set { strLoginID = value; }
        }
        public int USER_ROLE_ID
        {
            get { return intUserRoleID; }
            set { intUserRoleID = value; }
        }
        public string USER_ROLE_CODE
        {
            get { return strUserRoleCode; }
            set { strUserRoleCode = value; }
        }
        public string PREFERED_THEME
        {
            get { return strPrefTheme; }
            set { strPrefTheme = value; }
        }

        public Guid SESSION_ID
        {
            get { return SessionID; }
            set { SessionID = value; }
        }
        public string PASSWORD
        {
            get { return strPwd; }
            set { strPwd = value; }
        }
        public string FIRST_LOGIN
        {
            get { return strFirstLogin; }
            set { strFirstLogin = value; }
        }
        public string NEW_PASSWORD
        {
            get { return strNewPassword; }
            set { strNewPassword = value; }
        }
        public string CONFIRM_PASSWORD
        {
            get { return strConfirmPassword; }
            set { strConfirmPassword = value; }
        }
        public string MAIL_TEMPLATE_PATH
        {
            get { return strMailTemplatePath; }
            set { strMailTemplatePath = value; }
        }
        public string VERSION
        {
            get { return strVersion; }
            set { strVersion = value; }
        }
        public string BETA_VERSION
        {
            get { return strBetaVer; }
            set { strBetaVer = value; }
        }
        public string INSTITUTION_CODE
        {
            get { return strInstCode; }
            set { strInstCode = value; }
        }
        public string INSTITUTION_NAME
        {
            get { return strInstName; }
            set { strInstName = value; }
        }
        public string CHAT_URL
        {
            get { return strChatURL; }
            set { strChatURL = value; }
        }
        public string ENABLE_CHAT
        {
            get { return strENBLCHAT; }
            set { strENBLCHAT = value; }
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
        public Guid BILLING_ACCOUNT_ID
        {
            get { return BillingAccountID; }
            set { BillingAccountID = value; }
        }
        public string BILLING_ACCOUNT_NAME
        {
            get { return strBillingAccountName; }
            set { strBillingAccountName = value; }
        }
        public Guid RADIOLOGIST_ID
        {
            get { return RadiologistID; }
            set { RadiologistID = value; }
        }

        public string RADIOLOGIST_TIMEZONE
        {
            get { return RadiologistTimeZone; }
            set { RadiologistTimeZone = value; }
        }

        public string ALLOW_MANUAL_SUBMISSION
        {
            get { return strAllowMS; }
            set { strAllowMS = value; }
        }
        public string ALLOW_DASHBOARD_VIEW
        {
            get { return strAllowDBView; }
            set { strAllowDBView = value; }
        }
        public string WEB_SERVICE_SESSION_ID
        {
            get { return strSessID; }
            set { strSessID = value; }
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
        public string API_VERSION
        {
            get { return strAPIVER; }
            set { strAPIVER = value; }
        }
        public string REPORT_ENGINE_URL
        {
            get { return strRPTEGNURL; }
            set { strRPTEGNURL = value; }
        }

        public Guid Registration_ID
        {
            get { return RegistrationId; }
            set { RegistrationId = value; }
        }
        public string REG_LOGIN_ID
        {
            get { return regLoginId; }
            set { regLoginId = value; }
        }

        public int DEFAULT_TIME_ZONE_ID
        {
            get { return intDefTimeZoneID; }
            set { intDefTimeZoneID = value; }
        }
        public string DEFAULT_TIME_ZONE_STANDARD_NAME
        {
            get { return strDefTimeZoneStdName; }
            set { strDefTimeZoneStdName = value; }
        }
        #endregion

        #region CheckLogin
        public bool CheckLogin(string ConfigPath, ref string ReturnMessage, ref string CatchMessage, ref string Text1, ref string Text2)
        {
            bool bReturn = false; int intReturnValue = 0; int intExecReturn = 0;
            DataSet ds = new DataSet();




            if (ValidateLogin(ref ReturnMessage))
            {
                if (strPwd != string.Empty) strPwd = EncryptPassword(strPwd);

                SqlParameter[] SqlRecordParams = new SqlParameter[8];
                SqlRecordParams[0] = new SqlParameter("@login_id", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strLoginID;
                SqlRecordParams[1] = new SqlParameter("@password", SqlDbType.NVarChar, 200); SqlRecordParams[1].Value = strPwd;
                SqlRecordParams[2] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Direction = ParameterDirection.Output;
                SqlRecordParams[3] = new SqlParameter("@code", SqlDbType.NVarChar, 5); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@name", SqlDbType.NVarChar, 100); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[6].Direction = ParameterDirection.Output; 
                SqlRecordParams[7] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[7].Direction = ParameterDirection.Output;

                try
                {
                    if (ConfigPath.Trim() != string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "login_validate", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[7].Value);

                    if (SqlRecordParams[2].Value != DBNull.Value) UserID = new Guid(Convert.ToString(SqlRecordParams[2].Value));
                    if (SqlRecordParams[3].Value != DBNull.Value) strUserCode = Convert.ToString(SqlRecordParams[3].Value); else strUserCode = string.Empty;
                    if (SqlRecordParams[4].Value != DBNull.Value) strUserName = Convert.ToString(SqlRecordParams[4].Value); else strUserName = string.Empty;
                    if (SqlRecordParams[5].Value != DBNull.Value) SessionID = new Guid(Convert.ToString(SqlRecordParams[5].Value)); 


                    if (intReturnValue == 1)
                    {
                        bReturn = true;
                        ReturnMessage = Convert.ToString(SqlRecordParams[6].Value);

                    }
                    else
                    {
                        bReturn = false;
                        ReturnMessage = Convert.ToString(SqlRecordParams[6].Value);
                    }

                }
                catch (Exception LexpErr)
                {
                    bReturn = false;
                    CatchMessage = LexpErr.Message;
                }

            }
            else
            {
                bReturn = false;
            }

            return bReturn;
        }
        #endregion

        #region ValidateLogin
        private bool ValidateLogin(ref string ReturnMessage)
        {
            bool bReturn = true;
            StringBuilder strbErr = new StringBuilder();
            if (strLoginID.Trim() == string.Empty)
            {
                bReturn = false;
                if (strbErr.ToString().Trim() != string.Empty) strbErr.Append(CoreCommon.STRING_DIVIDER);
                strbErr.Append("007");
            }
            if (strPwd.Trim() == string.Empty)
            {
                bReturn = false;
                if (strbErr.ToString().Trim() != string.Empty) strbErr.Append(CoreCommon.STRING_DIVIDER);
                strbErr.Append("008");
            }

            if (!bReturn)
                ReturnMessage = strbErr.ToString();

            return bReturn;
        }
        #endregion

        #region EncryptPassword
        public string EncryptPassword(string toEncryptString)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncryptString);
            string key = "7";
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF7.GetBytes(key));
            hashmd5.Clear();
            byte[] key24Array = new byte[24];
            for (int i = 0; i < 16; i++)
            {
                key24Array[i] = keyArray[i];
            }
            for (int i = 0; i < 7; i++)
            {
                key24Array[i + 16] = keyArray[i];
            }
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = key24Array;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cryptoTransform = tripledes.CreateEncryptor();
            byte[] resultArray = cryptoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tripledes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        #endregion

        #region DeleteLoggedUser
        public bool DeleteLoggedUser(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intReturnValue = 0; int intExecReturn = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];


            SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserID;
            SqlRecordParams[1] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = SessionID;
            SqlRecordParams[2] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[2].Direction = ParameterDirection.Output;
            SqlRecordParams[3] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty)
                    CoreCommon.GetConnectionString(ConfigPath);

                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "logout_user", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[3].Value);

                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;

                ReturnMessage = Convert.ToString(SqlRecordParams[2].Value);

            }
            catch (Exception LexpErr)
            { bReturn = false; CatchMessage = LexpErr.Message; }


            return bReturn;
        }
        #endregion

        #region Change Password

        public bool ChangePassword(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0;
            int intExecReturn = 0;

            if (ValidatePasswordChange(ref ReturnMessage))
            {
                strPwd = EncryptPassword(strPwd);
                strNewPassword = EncryptPassword(strNewPassword);
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);


                SqlParameter[] SqlRecordParams = new SqlParameter[5];
                SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserID;
                SqlRecordParams[1] = new SqlParameter("@old_password", SqlDbType.NVarChar, 200); SqlRecordParams[1].Value = strPwd;
                SqlRecordParams[2] = new SqlParameter("@new_password", SqlDbType.NVarChar, 200); SqlRecordParams[2].Value = strNewPassword;
                SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                try
                {
                    if (CoreCommon.CONNECTION_STRING == string.Empty)
                        CoreCommon.GetConnectionString(ConfigPath);

                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "login_change_password", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[4].Value);
                    if (intReturnValue == 1)
                    {
                        bReturn = true;
                    }
                    else
                        bReturn = false;

                    ReturnMessage = Convert.ToString(SqlRecordParams[3].Value);

                }
                catch (Exception expErr)
                {
                    bReturn = false;
                    CatchMessage = expErr.Message;
                }
            }
            else
                bReturn = false;

            return bReturn;
        }

        #endregion

        #region Registration Change Password

        public bool RegistrationChangePassword(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0;
            int intExecReturn = 0;

            if (ValidatePasswordChange(ref ReturnMessage))
            {
                strPwd = EncryptPassword(strPwd);
                strNewPassword = EncryptPassword(strNewPassword);
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                SqlParameter[] SqlRecordParams = new SqlParameter[7];
                SqlRecordParams[0] = new SqlParameter("@reg_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = RegistrationId;
                SqlRecordParams[1] = new SqlParameter("@login_id", SqlDbType.NVarChar,200); SqlRecordParams[1].Value = regLoginId;
                SqlRecordParams[2] = new SqlParameter("@old_password", SqlDbType.NVarChar, 25); SqlRecordParams[2].Value = strPwd;
                SqlRecordParams[3] = new SqlParameter("@new_password", SqlDbType.NVarChar, 25); SqlRecordParams[3].Value = strNewPassword;
                SqlRecordParams[4] = new SqlParameter("@email_id", SqlDbType.NVarChar, 200); SqlRecordParams[4].Value = strEmailID;
                SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;

                try
                {
                    if (CoreCommon.CONNECTION_STRING == string.Empty)
                        CoreCommon.GetConnectionString(ConfigPath);

                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "reg_login_change_password", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[6].Value);
                    if (intReturnValue == 1)
                    {
                        bReturn = true;
                    }
                    else
                        bReturn = false;

                    ReturnMessage = Convert.ToString(SqlRecordParams[5].Value);

                }
                catch (Exception expErr)
                {
                    bReturn = false;
                    CatchMessage = expErr.Message;
                }
            }
            else
                bReturn = false;

            return bReturn;
        }

        #endregion

        #region Validate Password Change

        private bool ValidatePasswordChange(ref string returnMessage)
        {
            bool bReturn = true;
            StringBuilder sbErr = new StringBuilder();

            if (strPwd.Trim() == string.Empty)
            {
                bReturn = false;
                sbErr.Append("016");
            }

            if (strNewPassword.Trim() == string.Empty)
            {
                bReturn = false;
                if (sbErr.ToString().Trim() != string.Empty) sbErr.Append(CoreCommon.STRING_DIVIDER);
                sbErr.Append("017");
            }

            if (strConfirmPassword.Trim() == string.Empty)
            {
                bReturn = false;
                if (sbErr.ToString().Trim() != string.Empty) sbErr.Append(CoreCommon.STRING_DIVIDER);
                sbErr.Append("018");
            }
            if ((strNewPassword.Trim() != string.Empty) && (strPwd.Trim() != string.Empty))
            {
                if (strNewPassword.Trim() == strPwd.Trim())
                {
                    bReturn = false;
                    if (sbErr.ToString().Trim() != string.Empty) sbErr.Append(CoreCommon.STRING_DIVIDER);
                    sbErr.Append("020");
                }
            }
            if (strNewPassword.Trim() != strConfirmPassword.Trim())
            {
                bReturn = false;
                if (sbErr.ToString().Trim() != string.Empty) sbErr.Append(CoreCommon.STRING_DIVIDER);
                sbErr.Append("019");
            }

            if (!bReturn)
                returnMessage = sbErr.ToString();

            return bReturn;

        }

        #endregion

        #region FetchInstitutionUserPassword
        public bool FetchInstitutionUserPassword(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intReturnValue = 0; int intExecReturn = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[5];
            SqlRecordParams[0] = new SqlParameter("@login_id", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strLoginID;
            SqlRecordParams[1] = new SqlParameter("@institution_code", SqlDbType.NVarChar, 10); SqlRecordParams[1].Value = strInstCode;
            SqlRecordParams[2] = new SqlParameter("@password", SqlDbType.NVarChar, 200); SqlRecordParams[2].Direction = ParameterDirection.Output;
            SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[3].Direction = ParameterDirection.Output;
            SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "login_get_institution_user_pasword", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[4].Value);

                if (intReturnValue == 1)
                {
                    bReturn = true;
                    if (SqlRecordParams[2].Value != DBNull.Value) strConfirmPassword = CoreCommon.DecryptString(Convert.ToString(SqlRecordParams[2].Value));
                }
                else
                {
                    bReturn = false;
                }
                ReturnMessage = Convert.ToString(SqlRecordParams[3].Value);
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region FetchBillingAccountCredentials
        public bool FetchBillingAccountCredentials(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intReturnValue = 0; int intExecReturn = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[6];
            SqlRecordParams[0] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = BillingAccountID;
            SqlRecordParams[1] = new SqlParameter("@login_id", SqlDbType.NVarChar, 100); SqlRecordParams[1].Direction = ParameterDirection.Output;
            SqlRecordParams[2] = new SqlParameter("@login_pwd", SqlDbType.NVarChar, 200); SqlRecordParams[2].Direction = ParameterDirection.Output;
            SqlRecordParams[3] = new SqlParameter("@login_user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Direction = ParameterDirection.Output;
            SqlRecordParams[4] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[4].Direction = ParameterDirection.Output;
            SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "login_get_billing_account_credential", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[5].Value);



                if (intReturnValue == 1)
                {
                    bReturn = true;
                    if (SqlRecordParams[1].Value != DBNull.Value) strLoginID = Convert.ToString(SqlRecordParams[1].Value);
                    if (SqlRecordParams[2].Value != DBNull.Value) strConfirmPassword = CoreCommon.DecryptString(Convert.ToString(SqlRecordParams[2].Value));
                    if (SqlRecordParams[3].Value != DBNull.Value) UserID = new Guid(Convert.ToString(SqlRecordParams[3].Value));
                }
                else
                {
                    bReturn = false;
                }
                ReturnMessage = Convert.ToString(SqlRecordParams[4].Value);
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region FetchPostLoginData
        public bool FetchPostLoginData(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserID;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "login_postlogindata_fetch", SqlRecordParams);

                if (ds.Tables.Count > 0)
                {

                    ds.Tables[0].TableName = "UserDetails";
                    ds.Tables[1].TableName = "Menu";
                    ds.Tables[2].TableName = "PACSDtls";
                    ds.Tables[3].TableName = "DashboardSettings";

                    foreach (DataRow dr in ds.Tables["UserDetails"].Rows)
                    {
                        intUserRoleID = Convert.ToInt32(dr["user_role_id"]);
                        strUserRoleCode = Convert.ToString(dr["user_role_code"]).Trim();
                        strUserCode = Convert.ToString(dr["code"]).Trim();
                        strUserName = Convert.ToString(dr["name"]).Trim();
                        strUserContNo = Convert.ToString(dr["contact_no"]).Trim();
                        strAllowMS = Convert.ToString(dr["allow_manual_submission"]).Trim();
                        strAllowDBView = Convert.ToString(dr["allow_dashboard_view"]).Trim();
                        strInstCode = Convert.ToString(dr["institution_code"]).Trim();
                        strInstName = Convert.ToString(dr["institution_name"]).Trim();
                        strChatURL = Convert.ToString(dr["chat_url"]).Trim();
                        strENBLCHAT = Convert.ToString(dr["enable_chat"]).Trim();
                        BillingAccountID = new Guid(Convert.ToString(dr["billing_account_id"]));
                        strBillingAccountName = Convert.ToString(dr["billing_account_name"]).Trim();
                        RadiologistID = new Guid(Convert.ToString(dr["radiologist_id"]));
                        RadiologistTimeZone = Convert.ToString(dr["radiologistTimeZone"]);
                        strSessID = Convert.ToString(dr["session_id"]).Trim();
                        strWS8SRVIP = Convert.ToString(dr["WS8SRVIP"]).Trim();
                        strWS8CLTIP = Convert.ToString(dr["WS8CLTIP"]).Trim();
                        strWS8SRVUID = Convert.ToString(dr["WS8SRVUID"]).Trim();
                        strWS8SRVPWD = Convert.ToString(dr["WS8SRVPWD"]).Trim();
                        strAPIVER = Convert.ToString(dr["APIVER"]).Trim();
                        strRPTEGNURL = Convert.ToString(dr["RPTEGNURL"]).Trim();
                        intDefTimeZoneID = Convert.ToInt32(dr["default_time_zone_id"]);
                        strDefTimeZoneStdName = Convert.ToString(dr["default_time_zone_name"]).Trim();
                        strPrefTheme = Convert.ToString(dr["theme_pref"]).Trim();
                    }

                    foreach (DataRow dr in ds.Tables["PACSDtls"].Rows)
                    {

                        strPACSUserID = Convert.ToString(dr["pacs_user_id"]);
                        if (Convert.ToString(dr["pacs_password"]).Trim() != string.Empty) strPACSPwd = CoreCommon.DecryptString(Convert.ToString(dr["pacs_password"]));
                    }

                }


                bReturn = true;

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region FetchMenu
        public bool FetchMenu(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[2];
            SqlRecordParams[0] = new SqlParameter("@user_role_id", SqlDbType.Int); SqlRecordParams[0].Value = intUserRoleID;
            SqlRecordParams[1] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserID;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "login_get_menu", SqlRecordParams);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Menu";

                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region FetchMenuRecordCount
        public bool FetchMenuRecordCount(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserID;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "login_menu_record_count_fetch", SqlRecordParams);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "RecordCount";

                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region UserUnlock
        public bool UserUnlock(string ConfigPath, int CompanyID, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intReturnValue = 0; int intExecReturn = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];


            SqlRecordParams[0] = new SqlParameter("@email_id", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strEmailID;
            SqlRecordParams[1] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[1].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty)
                    CoreCommon.GetConnectionString(ConfigPath);

                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "login_unlock_user", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[1].Value);

                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;



            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region UpdatePreferedTheme
        public bool UpdatePreferedTheme(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intReturnValue = 0; int intExecReturn = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];


            SqlRecordParams[0] = new SqlParameter("@theme_pref", SqlDbType.NVarChar, 10); SqlRecordParams[0].Value = strPrefTheme;
            SqlRecordParams[1] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserID;
            SqlRecordParams[2] = new SqlParameter("@error_code", SqlDbType.NVarChar,10); SqlRecordParams[2].Direction = ParameterDirection.Output;
            SqlRecordParams[3] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty)
                    CoreCommon.GetConnectionString(ConfigPath);

                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "login_user_pref_theme_update", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[3].Value);

                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;

                ReturnMessage = Convert.ToString(SqlRecordParams[2].Value).Trim();


            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchUserEmailID
        public bool FetchUserEmailID(string ConfigPath,ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intReturnValue = 0; int intExecReturn = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            

            try
            {
                SqlRecordParams[0] = new SqlParameter("@login_id", SqlDbType.NVarChar,50); SqlRecordParams[0].Value = strLoginID;
                SqlRecordParams[1] = new SqlParameter("@email_id", SqlDbType.NVarChar, 100); SqlRecordParams[1].Direction = ParameterDirection.Output;
                SqlRecordParams[2] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[2].Direction = ParameterDirection.Output;
                SqlRecordParams[3] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "login_user_email_id_fetch", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[3].Value);
                if (intReturnValue == 1)
                {
                    bReturn = true;
                    strEmailID = Convert.ToString(SqlRecordParams[1].Value);
                }
                else
                    bReturn = false;
                
                ReturnMessage = Convert.ToString(SqlRecordParams[2].Value);
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region Forgot Password
        public bool ForgotPassword(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0;
            DataSet ds = new DataSet();
            string[] strArrMailServer = { "", "", "", "", "", "" }; bool bRetSettings = false;
            string strMailSentFrom = string.Empty;
            string strConctrolCode = string.Empty;

            if (ValidateForgotPassword(ref ReturnMessage))
            {

                SqlParameter[] SqlRecordParams = new SqlParameter[3];
                SqlRecordParams[0] = new SqlParameter("@login_id", SqlDbType.NVarChar, 50); SqlRecordParams[0].Value = strLoginID;
                SqlRecordParams[1] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[1].Direction = ParameterDirection.Output;
                SqlRecordParams[2] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[2].Direction = ParameterDirection.Output;

                try
                {
                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                    ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "login_forget_password", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[2].Value);
                    if (intReturnValue == 1)
                    {
                        bRetSettings = true;
                        if (ds.Tables.Count > 0)
                        {
                            ds.Tables[0].TableName = "Settings";
                            try
                            {
                                foreach (DataRow dr in ds.Tables["Settings"].Rows)
                                {
                                    strConctrolCode = Convert.ToString(dr["control_code"]).Trim();

                                    switch (strConctrolCode)
                                    {
                                        case "MAILSVRNAME":
                                            strArrMailServer[0] = Convert.ToString(dr["data_type_string"]).Trim();
                                            break;
                                        case "MAILSVRPORT":
                                            strArrMailServer[1] = Convert.ToString(dr["data_type_number"]).Trim();
                                            break;
                                        case "MAILSVRUSRCODE":
                                            strArrMailServer[2] = Convert.ToString(dr["data_type_string"]).Trim();
                                            break;
                                        case "MAILSVRUSRPWD":
                                            strArrMailServer[3] = CoreCommon.DecryptString(Convert.ToString(dr["data_type_string"]).Trim());
                                            break;
                                        case "MAILSSLENABLED":
                                            strArrMailServer[4] = Convert.ToString(dr["data_type_string"]).Trim();
                                            break;
                                        case "MAILSENDER":
                                            strMailSentFrom = Convert.ToString(dr["data_type_string"]).Trim();
                                            break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                CatchMessage = ex.Message;
                                bRetSettings = false;
                            }



                            strPwd = Convert.ToString(ds.Tables[1].Rows[0]["password"]);
                            strUserName = Convert.ToString(ds.Tables[1].Rows[0]["name"]);
                            strPwd = CoreCommon.DecryptString(strPwd);


                            if (bRetSettings)
                            {

                                SendUserAccountMail(strMailSentFrom, strEmailID, strUserName, strPwd, strArrMailServer);
                            }

                        }
                        bReturn = true;
                    }
                    else
                        bReturn = false;

                    ReturnMessage = Convert.ToString(SqlRecordParams[1].Value);
                }
                catch (Exception expErr)
                {
                    bReturn = false;
                    CatchMessage = expErr.Message;
                }
            }

            return bReturn;
        }

        #endregion

        #region ValidateForgotPassword
        private bool ValidateForgotPassword(ref string ReturnMessage)
        {
            bool bReturn = true;
            StringBuilder strbErr = new StringBuilder();
            if (strLoginID.Trim() == string.Empty)
            {
                bReturn = false;
                if (strbErr.ToString().Trim() != string.Empty) strbErr.Append(CoreCommon.STRING_DIVIDER);
                strbErr.Append("007");
            }

            if (strEmailID.Trim() == string.Empty)
            {
                bReturn = false;
                if (strbErr.ToString().Trim() != string.Empty) strbErr.Append(CoreCommon.STRING_DIVIDER);
                strbErr.Append("121");
            }
            else
            {
                if (!CoreCommon.IsEmailValid(strEmailID.Trim()))
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "084";
                }
            }



            if (!bReturn)
                ReturnMessage = strbErr.ToString();

            return bReturn;
        }
        #endregion

        #region SendUserAccountMail
        private void SendUserAccountMail(string strSenderEmailId, string strEmailId, string strUserName, string strPassword, string[] strArrMailServer)
        {
            string strEmailID = strEmailId;
            StringBuilder sbMailMessage = new StringBuilder();
            MailSender objMail = new MailSender();
            try
            {
                if (strArrMailServer[0].Trim() != "")
                {
                    objMail.SMTPServer = strArrMailServer[0].Trim();
                    objMail.MailServerPortNo = Convert.ToInt32(strArrMailServer[1].Trim());
                    objMail.MailFrom = strSenderEmailId;
                    objMail.MailTo = strEmailID;
                    objMail.MailSubject = "VETRIS : Your Password";
                    sbMailMessage.Append(GenerateUserAccountMailBody(strPassword, strUserName));
                    objMail.MailBody = sbMailMessage.ToString();
                    objMail.EmbedContent = true;
                    objMail.MailFrom = strArrMailServer[2].Trim();
                    objMail.MailServerUserId = strArrMailServer[2].Trim();
                    objMail.MailServerPassword = strArrMailServer[3].Trim();
                    objMail.DecryptPassword = "N";

                    if (strArrMailServer[4].Trim() == "Y")
                        objMail.MailServerSSLEnabled = true;
                    else
                        objMail.MailServerSSLEnabled = false;

                    objMail.SendMail(ref CatchMessage);
                }
            }
            catch (Exception ex)
            {
                ;
            }
            finally
            {
                objMail = null;
            }
        }
        #endregion

        #region GenerateUserAccountMailBody
        private string GenerateUserAccountMailBody(string strPassword, string strUserName)
        {
            StringBuilder sbMailBody = new StringBuilder();
            string strMailText = "";

            sbMailBody.AppendLine("Dear " + strUserName + ",");
            sbMailBody.AppendLine(" ");
            sbMailBody.AppendLine("As per your request, your password is being emailed to you.");
            sbMailBody.AppendLine("Your password is " + strPassword);
            sbMailBody.AppendLine(" ");
            sbMailBody.AppendLine("Regards,");
            sbMailBody.AppendLine("VETRIS SYSTEM ADMIN");

            strMailText = sbMailBody.ToString();
            strMailText = strMailText.Replace("\n", "<br/>");

            return strMailText;
        }
        #endregion

        #region UpdateWebServiceSession
        public bool UpdateWebServiceSession(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intReturnValue = 0; int intExecReturn = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];

            SqlRecordParams[0] = new SqlParameter("@session_id", SqlDbType.NVarChar, 30); SqlRecordParams[0].Value = strSessID;
            SqlRecordParams[1] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserID;
            SqlRecordParams[2] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[2].Direction = ParameterDirection.Output;
            SqlRecordParams[3] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty)
                    CoreCommon.GetConnectionString(ConfigPath);

                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "login_ws_session_create", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[3].Value);

                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;

                ReturnMessage = Convert.ToString(SqlRecordParams[2].Value);

            }
            catch (Exception LexpErr)
            { bReturn = false; CatchMessage = LexpErr.Message; }


            return bReturn;
        }
        #endregion

        #region GetRegistrationData
        public bool GetRegistrationData(string ConfigPath, ref string ReturnMessage, ref string CatchMessage, ref string Text1, ref string Text2)
        {
            bool bReturn = false; int intReturnValue = 0; int intExecReturn = 0;
            DataSet ds = new DataSet();

            SqlParameter[] SqlRecordParams = new SqlParameter[5];
            SqlRecordParams[0] = new SqlParameter("@registration_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = RegistrationId;
            SqlRecordParams[1] = new SqlParameter("@code", SqlDbType.NVarChar, 5); SqlRecordParams[1].Direction = ParameterDirection.Output;
            SqlRecordParams[2] = new SqlParameter("@name", SqlDbType.NVarChar, 100); SqlRecordParams[2].Direction = ParameterDirection.Output;
            SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[3].Direction = ParameterDirection.Output;
            SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

            try
            {
                if (ConfigPath.Trim() != string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "login_temp_registration_dtls_fetch", SqlRecordParams);//login_temp_registration_dtls_fetch
                intReturnValue = Convert.ToInt32(SqlRecordParams[4].Value);

                if (SqlRecordParams[1].Value != DBNull.Value) strUserCode = Convert.ToString(SqlRecordParams[1].Value); else strUserCode = string.Empty;
                if (SqlRecordParams[2].Value != DBNull.Value) strUserName = Convert.ToString(SqlRecordParams[2].Value); else strUserName = string.Empty;

                if (intReturnValue == 1)
                {
                    bReturn = true;
                    ReturnMessage = Convert.ToString(SqlRecordParams[3].Value);
                }
                else
                {
                    bReturn = false;
                    ReturnMessage = Convert.ToString(SqlRecordParams[3].Value);
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

        #region FetchRadiologistProductivityCount
        public bool FetchRadiologistProductivityCount(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[2];
            SqlRecordParams[0] = new SqlParameter("@radiologist_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = RadiologistID;
            SqlRecordParams[1] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserID;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "dashboard_radiologist_productivity", SqlRecordParams);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "RecordCount";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion
    }

    public class RadiologistProductivity
    {
        public string modality { get; set; }
        public int assigned_count { get; set; }
        public int work_progress_count { get; set; }
        public int today_count { get; set; }
        public int this_month_count { get; set; }
        public int last_month_count { get; set; }
        public int this_year_count { get; set; }

    }
}
