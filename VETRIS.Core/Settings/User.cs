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

namespace VETRIS.Core.Settings
{
    public class User
    {
        #region Constructor
        public User()
        {

        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid SessionID = new Guid("00000000-0000-0000-0000-000000000000");
        int intRoleID = 0;
        string strUserName = string.Empty;

        Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
        string strCode = string.Empty;
        string strName = string.Empty;
        string strEmail = string.Empty;
        string strContactNo = string.Empty;
        int intUserRoleID = 0;
        string strRoleCode = string.Empty;
        string strLoginID = string.Empty;
        string strInstName = string.Empty;
        string strBAName = string.Empty;
        string strPwd = string.Empty;
        string strPACSUserID = string.Empty;
        string strPACSUserPwd = string.Empty;
        string strAllowMS = "N";
        string strAllowDB = "N";
        string strIsActive = "Y";
        string strXMLMenu = string.Empty;
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
        public Guid SESSION_ID
        {
            get { return SessionID; }
            set { SessionID = value; }
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
        public string LOGIN_ID
        {
            get { return strLoginID; }
            set { strLoginID = value; }
        }
        public string INSTITUTION_NAME
        {
            get { return strInstName; }
            set { strInstName = value; }
        }
        public string BILLING_ACCOUNT_NAME
        {
            get { return strBAName; }
            set { strBAName = value; }
        }
        public string LOGIN_PASSWORD
        {
            get { return strPwd; }
            set { strPwd = value; }
        }
        public string PACS_USER_ID
        {
            get { return strPACSUserID; }
            set { strPACSUserID = value; }
        }
        public string PACS_USER_PASSWORD
        {
            get { return strPACSUserPwd; }
            set { strPACSUserPwd = value; }
        }
        public int ROLE_ID
        {
            get { return intRoleID; }
            set { intRoleID = value; }
        }
        public string ROLE_CODE
        {
            get { return strRoleCode; }
            set { strRoleCode = value; }
        }
        public string ALLOW_MANUAL_SUBMISSION
        {
            get { return strAllowMS; }
            set { strAllowMS = value; }
        }
        public string ALLOW_DASHBOARD_VIEW
        {
            get { return strAllowDB; }
            set { strAllowDB = value; }
        }
        public string IS_ACTIVE
        {
            get { return strIsActive; }
            set { strIsActive = value; }
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
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_brw_fetch_params");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "UserRoles";
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
            SqlParameter[] SqlRecordParams = new SqlParameter[11];
            SqlRecordParams[0] = new SqlParameter("@code", SqlDbType.NVarChar, 5); SqlRecordParams[0].Value = strCode;
            SqlRecordParams[1] = new SqlParameter("@name", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strName;
            SqlRecordParams[2] = new SqlParameter("@login_id", SqlDbType.NVarChar, 100); SqlRecordParams[2].Value = strLoginID;
            SqlRecordParams[3] = new SqlParameter("@institution_name", SqlDbType.NVarChar, 100); SqlRecordParams[3].Value = strInstName;
            SqlRecordParams[4] = new SqlParameter("@billing_account_name", SqlDbType.NVarChar, 100); SqlRecordParams[4].Value = strBAName;
            SqlRecordParams[5] = new SqlParameter("@allow_manual_submission", SqlDbType.NChar, 1); SqlRecordParams[5].Value = strAllowMS;
            SqlRecordParams[6] = new SqlParameter("@user_role_id", SqlDbType.Int); SqlRecordParams[6].Value = intRoleID;
            SqlRecordParams[7] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[7].Value = strIsActive;
            SqlRecordParams[8] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[8].Value = UserID;
            SqlRecordParams[9] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[9].Value = intMenuID;
            SqlRecordParams[10] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[10].Value = SessionID;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "settings_user_fetch_brw", SqlRecordParams);
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

        #region Dialog Methods

        #region LoadDetails
        public bool LoadDetails(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;

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
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_user_fetch_dtls", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";
                    ds.Tables[1].TableName = "UserRoles";

                    #region Details
                    foreach (DataRow dr in ds.Tables["Details"].Rows)
                    {
                        strCode = Convert.ToString(dr["code"]).Trim();
                        strName = Convert.ToString(dr["name"]).Trim();
                        strEmail = Convert.ToString(dr["email_id"]).Trim();
                        intRoleID = Convert.ToInt32(dr["user_role_id"]);
                        strRoleCode = Convert.ToString(dr["user_role_code"]).Trim();
                        strContactNo = Convert.ToString(dr["contact_no"]).Trim();
                        strLoginID = Convert.ToString(dr["login_id"]).Trim();
                        if (Convert.ToString(dr["password"]).Trim() != "") strPwd = CoreCommon.DecryptString(Convert.ToString(dr["password"]).Trim());
                        strPACSUserID = Convert.ToString(dr["pacs_user_id"]).Trim();
                        if (Convert.ToString(dr["pacs_password"]).Trim() != string.Empty) strPACSUserPwd = CoreCommon.DecryptString(Convert.ToString(dr["pacs_password"]).Trim());
                        strAllowMS = Convert.ToString(dr["allow_manual_submission"]).Trim();
                        strAllowDB = Convert.ToString(dr["allow_dashboard_view"]).Trim();
                        strInstName = Convert.ToString(dr["institution_name"]).Trim();
                        strBAName = Convert.ToString(dr["billing_account_name"]).Trim();
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

        #region FetchUserAccessRights
        public bool FetchUserAccessRights(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_user_access_rights_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Rights";
                    bReturn = true;
                }

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchUserRoleAccessRights
        public bool FetchUserRoleAccessRights(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.Int); SqlRecordParams[0].Value = intRoleID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_user_rolewise_access_rights_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Rights";
                    bReturn = true;
                }

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, MenuList[] ArrMenuObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;
            string[] arrTempFiles = new string[0];
            string strFile = string.Empty;

            if (ValidateRecord(ArrMenuObj, ref ReturnMessage))
            {
                if (GenerateMenuXML(ArrMenuObj, ref CatchMessage))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[20];

                        SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
                        SqlRecordParams[1] = new SqlParameter("@code", SqlDbType.NVarChar, 10); SqlRecordParams[1].Value = strCode;
                        SqlRecordParams[2] = new SqlParameter("@name", SqlDbType.NVarChar, 100); SqlRecordParams[2].Value = strName;
                        SqlRecordParams[3] = new SqlParameter("@email_id", SqlDbType.NVarChar, 100); SqlRecordParams[3].Value = strEmail;
                        SqlRecordParams[4] = new SqlParameter("@contact_no", SqlDbType.NVarChar, 20); SqlRecordParams[4].Value = strContactNo;
                        SqlRecordParams[5] = new SqlParameter("@user_role_id", SqlDbType.Int); SqlRecordParams[5].Value = intRoleID;
                        SqlRecordParams[6] = new SqlParameter("@login_id", SqlDbType.NVarChar, 50); SqlRecordParams[6].Value = strLoginID;
                        SqlRecordParams[7] = new SqlParameter("@password", SqlDbType.NVarChar, 200); SqlRecordParams[7].Value = strPwd;
                        SqlRecordParams[8] = new SqlParameter("@pacs_user_id", SqlDbType.NVarChar, 20); SqlRecordParams[8].Value = strPACSUserID;
                        SqlRecordParams[9] = new SqlParameter("@pacs_password", SqlDbType.NVarChar, 200); SqlRecordParams[9].Value = strPACSUserPwd;
                        SqlRecordParams[10] = new SqlParameter("@allow_manual_submission", SqlDbType.NChar, 1); SqlRecordParams[10].Value = strAllowMS;
                        SqlRecordParams[11] = new SqlParameter("@allow_dashboard_view", SqlDbType.NChar, 1); SqlRecordParams[11].Value = strAllowDB;
                        SqlRecordParams[12] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[12].Value = strIsActive;
                        SqlRecordParams[13] = new SqlParameter("@xml_menu", SqlDbType.NText); SqlRecordParams[13].Value = strXMLMenu;
                        SqlRecordParams[14] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[14].Value = UserID;
                        SqlRecordParams[15] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[15].Value = intMenuID;
                        SqlRecordParams[16] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[16].Value = SessionID;
                        SqlRecordParams[17] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[17].Direction = ParameterDirection.Output;
                        SqlRecordParams[18] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[18].Direction = ParameterDirection.Output;
                        SqlRecordParams[19] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[19].Direction = ParameterDirection.Output;


                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_user_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[19].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[17].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[18].Value);

                        Id = new Guid(Convert.ToString(SqlRecordParams[0].Value));


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
        private bool ValidateRecord(MenuList[] ArrMenuObj, ref string ReturnMessage)
        {
            bool bReturn = true;

            if (strCode.Trim() == string.Empty)
            {
                ReturnMessage = "075";
            }
            if (strName.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "076";
            }
            if (intRoleID == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "079";
            }
            if (strEmail.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "119";
            }
            //else
            //{
            //    if (!CoreCommon.IsEmailValid(strEmail))
            //    {
            //        if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
            //        ReturnMessage += "084";
            //    }
            //}
            if (strLoginID.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "112";
            }
            if (strPwd.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "120";
            }

            if (strPACSUserID.Trim() == string.Empty)
            {
                ReturnMessage = "080";
            }
            if (strPACSUserPwd.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "081";
            }

            if (ArrMenuObj.Length == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "097";
            }

            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }

        #endregion

        #region GenerateMenuXML
        private bool GenerateMenuXML(MenuList[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();
            try
            {

                sbXML.Append("<menu>");

                for (int i = 0; i < ArrObj.Length; i++)
                {
                    sbXML.Append("<row>");
                    sbXML.Append("<menu_id>" + ArrObj[i].MENU_ID.ToString() + "</menu_id>");
                    sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                    sbXML.Append("</row>");
                }

                sbXML.Append("</menu>");
                strXMLMenu = sbXML.ToString();
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
