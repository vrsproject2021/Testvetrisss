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

namespace VETRIS.Core.Master
{
    public class MasterQuery
    {
        #region Constructor
        public MasterQuery()
        {

        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        string strUserName = string.Empty;

        Guid BAId = new Guid("00000000-0000-0000-0000-000000000000");
        string strBACode = string.Empty;
        string strBAName = string.Empty;
        Guid InstId = new Guid("00000000-0000-0000-0000-000000000000");
        string strInstCode = string.Empty;
        string strInstName = string.Empty;
        string strLoginID = string.Empty;
        string strPwd = string.Empty;
        Guid LoginUserId = new Guid("00000000-0000-0000-0000-000000000000");

        string strBAIsActive = "Y";
        string strInstIsActive = "Y";
        string strUserIsActive = "Y";
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
        public Guid BILLING_ACCOUNT_ID
        {
            get { return BAId; }
            set { BAId = value; }
        }
        public string BILLING_ACCOUNT_CODE
        {
            get { return strBACode; }
            set { strBACode = value; }
        }
        public string BILLING_ACCOUNT_NAME
        {
            get { return strBAName; }
            set { strBAName = value; }
        }
        public string BILLING_ACCOUNT_IS_ACTIVE
        {
            get { return strBAIsActive; }
            set { strBAIsActive = value; }
        }
        public Guid INSTITUTION_ID
        {
            get { return InstId; }
            set { InstId = value; }
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
        public string INSTITUTION_IS_ACTIVE
        {
            get { return strInstIsActive; }
            set { strInstIsActive = value; }
        }
        public Guid LOGIN_USER_ID
        {
            get { return LoginUserId; }
            set { LoginUserId = value; }
        }
        public string LOGIN_ID
        {
            get { return strLoginID; }
            set { strLoginID = value; }
        }
        public string LOGIN_PASSWORD
        {
            get { return strPwd; }
            set { strPwd = value; }
        }
        public string USER_IS_ACTIVE
        {
            get { return strUserIsActive; }
            set { strUserIsActive = value; }
        }
        #endregion

        #region Browser Methods
        #region SearchBrowserList
        public bool SearchBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[9];
            SqlRecordParams[0] = new SqlParameter("@billing_account_code", SqlDbType.NVarChar, 5); SqlRecordParams[0].Value = strBACode;
            SqlRecordParams[1] = new SqlParameter("@billing_account_name", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strBAName;
            SqlRecordParams[2] = new SqlParameter("@billing_account_active", SqlDbType.NChar, 1); SqlRecordParams[2].Value = strBAIsActive;
            SqlRecordParams[3] = new SqlParameter("@institution_code", SqlDbType.NVarChar, 5); SqlRecordParams[3].Value = strInstCode;
            SqlRecordParams[4] = new SqlParameter("@institution_name", SqlDbType.NVarChar, 100); SqlRecordParams[4].Value = strInstName;
            SqlRecordParams[5] = new SqlParameter("@institution_active", SqlDbType.NChar, 1); SqlRecordParams[5].Value = strInstIsActive;
            SqlRecordParams[6] = new SqlParameter("@login_id", SqlDbType.NVarChar, 30); SqlRecordParams[6].Value = strLoginID;
            SqlRecordParams[7] = new SqlParameter("@user_active", SqlDbType.NChar, 1); SqlRecordParams[7].Value = strUserIsActive;
            SqlRecordParams[8] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[8].Value = UserID;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_query_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "BillingAccount";
                    ds.Tables[1].TableName = "Institutions";
                    ds.Tables[2].TableName = "Users";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #endregion
    }
}
