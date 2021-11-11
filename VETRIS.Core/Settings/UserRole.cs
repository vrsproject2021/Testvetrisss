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
    public class UserRole
    {
        #region Constructor
        public UserRole()
        {
        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        string strUserName = string.Empty;

        int intID = 0;
        string strCode = string.Empty;
        string strName = string.Empty;
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

        #region SearchRecords
        public bool SearchRecords(string ConfigPath, ref DataSet ds,ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[7];
            SqlRecordParams[0] = new SqlParameter("@code", SqlDbType.NVarChar, 10); SqlRecordParams[0].Value = strCode;
            SqlRecordParams[1] = new SqlParameter("@name", SqlDbType.NVarChar, 30); SqlRecordParams[1].Value = strName;
            SqlRecordParams[2] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[2].Value = strIsActive;
            SqlRecordParams[3] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = UserID;
            SqlRecordParams[4] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[4].Value = intMenuID;
            SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
            SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "settings_user_role_fetch_brw", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "BrowserList";
                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                    ReturnMessage = Convert.ToString(SqlRecordParams[5].Value);
                }
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, UserRole[] ArrObj,ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0; int intRowID = 0;
            string[] arrTempFiles = new string[0];
            string strFile = string.Empty;

            if (ValidateRecord(ArrObj, ref ReturnMessage, ref intRowID))
            {
                if (GenerateXML(ArrObj, ref CatchMessage))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[6];
                        SqlRecordParams[0] = new SqlParameter("@xml_data", SqlDbType.NText); SqlRecordParams[0].Value = strXML.Trim();
                        SqlRecordParams[1] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserID;
                        SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = intMenuID;
                        SqlRecordParams[3] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[3].Direction = ParameterDirection.Output;
                        SqlRecordParams[4] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[4].Direction = ParameterDirection.Output;
                        SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;


                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_user_role_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[5].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[3].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[4].Value);




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

        #region ValidateRecord
        private bool ValidateRecord(UserRole[] ArrObj, ref string ReturnMessage,ref int RowID)
        {
            bool bReturn = true;
            if (ArrObj.Length == 0)
            {
                ReturnMessage = "067";
            }
            else
            {
                for (int i = 0; i < ArrObj.Length; i=i+5)
                {
                    if (ArrObj[i].CODE.Trim() == string.Empty)
                    {
                        ReturnMessage = "068";
                        intRowID = ArrObj[i].ROW_ID;
                        break;
                    }
                    if (ArrObj[i].NAME.Trim() == string.Empty)
                    {
                        if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                        ReturnMessage = "069";
                        intRowID = ArrObj[i].ROW_ID;
                        break;
                    }
                }
            }

           
            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }
        #endregion

        #region GenerateXML
        private bool GenerateXML(UserRole[] ArrObj,  ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();
           
            try
            {
                if (ArrObj.Length > 0)
                {
                    
                        sbXML.Append("<data>");

                        for (int i = 0; i < ArrObj.Length; i=i+1)
                        {
                            sbXML.Append("<row>");
                            sbXML.Append("<id>" + ArrObj[i].ID.ToString() + "</id>");
                            sbXML.Append("<code><![CDATA[" + ArrObj[i].CODE + "]]></code>");
                            sbXML.Append("<name><![CDATA[" + ArrObj[i].NAME + "]]></name>");
                            sbXML.Append("<is_active><![CDATA[" + ArrObj[i].IS_ACTIVE + "]]></is_active>");
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
