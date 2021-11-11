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
    public class ReportDisclaimerReasons
    {
        #region Constructor
        public ReportDisclaimerReasons()
        {
        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        string strUserName = string.Empty;

        int intID = 0;
        string strType = string.Empty;
        string strDesc = string.Empty;
        string strIsActive = "X";
        int intRowID = 0;
        string strXML = string.Empty;
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
        public int ID
        {
            get { return intID; }
            set { intID = value; }
        }
        public string TYPE
        {
            get { return strType; }
            set { strType = value; }
        }
        public string DESCRIPTION
        {
            get { return strDesc; }
            set { strDesc = value; }
        }
        public string IS_ACTIVE
        {
            get { return strIsActive; }
            set { strIsActive = value; }
        }
        public int ROW_ID
        {
            get { return intRowID; }
            set { intRowID = value; }
        }
        #endregion

        #region Browser Methods

        #region SearchBrowserList
        public bool SearchBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[3];
            SqlRecordParams[0] = new SqlParameter("@type", SqlDbType.NVarChar, 30); SqlRecordParams[0].Value = strType;
            SqlRecordParams[1] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[1].Value = strIsActive;
            SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "settings_report_disclaimer_reasons_fetch_brw", SqlRecordParams);
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
            bool bReturn = false; strIsActive = "Y";

            SqlParameter[] SqlRecordParams = new SqlParameter[5];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.Int); SqlRecordParams[0].Value = intID;
                SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = intMenuID;
                SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_report_disclaimer_reasons_fetch_dtls", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";

                    #region Details
                    foreach (DataRow dr in ds.Tables["Details"].Rows)
                    {
                        strType = Convert.ToString(dr["type"]).Trim();
                        strDesc = Convert.ToString(dr["description"]).Trim();
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

        #region SaveRecord
        public bool SaveRecord(string ConfigPath,  ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;
            string[] arrTempFiles = new string[0];
            string strFile = string.Empty;

            if (ValidateRecord(ref ReturnMessage))
            {
                
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[9];
                        SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.Int); SqlRecordParams[0].Value = intID; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
                        SqlRecordParams[1] = new SqlParameter("@type", SqlDbType.NVarChar, 30); SqlRecordParams[1].Value = strType;
                        SqlRecordParams[2] = new SqlParameter("@description", SqlDbType.NText); SqlRecordParams[2].Value = strDesc;
                        SqlRecordParams[3] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[3].Value = strIsActive;
                        SqlRecordParams[4] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = UserID;
                        SqlRecordParams[5] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[5].Value = intMenuID;
                        SqlRecordParams[6] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[6].Direction = ParameterDirection.Output;
                        SqlRecordParams[7] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[7].Direction = ParameterDirection.Output;
                        SqlRecordParams[8] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[8].Direction = ParameterDirection.Output;


                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_report_disclaimer_reasons_save", SqlRecordParams);
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

        #region ValidateRecord
        private bool ValidateRecord(ref string ReturnMessage)
        {
            bool bReturn = true;

            if (strType.Trim() == string.Empty)
            {
                ReturnMessage = "419";
            }
            if (strDesc.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "420";
            }

            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }
        #endregion

        #endregion

        
    }
}
