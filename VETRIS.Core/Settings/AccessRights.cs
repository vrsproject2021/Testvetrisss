using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace VETRIS.Core.Settings
{
    public class AccessRights
    {
        #region Constructor
        public AccessRights()
        {
        }
        #endregion

        #region Variables
        int intUserRoleID = 0;
        int intMenuID = 0;
        Guid lUserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intID = 0;
        string strXMLMenu = string.Empty;
        string strXMLFns = string.Empty;
        string strUserName = string.Empty;
        int intDelModuleID = 0;
        int intDelMenuID = 0;
        int intDelFunctionID = 0;
        #endregion

        #region Properties
        public int USER_ROLE_ID
        {
            get { return intUserRoleID; }
            set { intUserRoleID = value; }
        }
        public int MENU_ID
        {
            get { return intMenuID; }
            set { intMenuID = value; }
        }
        public int ID
        {
            get { return intID; }
            set { intID = value; }
        }
        public Guid USER_ID
        {
            get { return lUserID; }
            set { lUserID = value; }
        }
        public string USER_NAME
        {
            get { return strUserName; }
            set { strUserName = value; }
        }
        public int DELETED_MODULE_ID
        {
            get { return intDelModuleID; }
            set { intDelModuleID = value; }
        }
        public int DELETED_MENU_ID
        {
            get { return intDelMenuID; }
            set { intDelMenuID = value; }
        }
        #endregion

        #region FetchUserRoles
        public bool FetchUserRoles(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

        

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_access_rights_user_roles_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "BrowserList";
                    bReturn = true;
                }

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchAccessRights
        public bool FetchAccessRights(string ConfigPath,ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[5];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.Int); SqlRecordParams[0].Value = intID;
            SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = intMenuID;
            SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = lUserID;
            SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[3].Direction = ParameterDirection.Output;
            SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_access_rights_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "MenuRights";
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

        #region FetchAssignRights
        public bool FetchAssignRights(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[5];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.Int); SqlRecordParams[0].Value = intID;
            SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = intMenuID;
            SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = lUserID;
            SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[3].Direction = ParameterDirection.Output;
            SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_assign_rights_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "MenuRights";
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
        public bool SaveRecord(string ConfigPath, MenuList[] ArrMenuObj,  ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRecord(ArrMenuObj,  ref ReturnMessage))
            {
                if (GenerateMenuXML(ArrMenuObj, ref CatchMessage))
                {
                    SqlParameter[] SqlRecordParams = new SqlParameter[7];
                    SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.Int); SqlRecordParams[0].Value = intID;
                    SqlRecordParams[1] = new SqlParameter("@xml_menu", SqlDbType.NText); SqlRecordParams[1].Value = strXMLMenu;
                    SqlRecordParams[2] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = lUserID;
                    SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[3].Value = intMenuID;
                    SqlRecordParams[4] = new SqlParameter("@user_name", SqlDbType.NVarChar, 30); SqlRecordParams[4].Direction = ParameterDirection.Output;
                    SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
                    SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;
                    try
                    {

                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_access_rights_save", SqlRecordParams);
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
                }
                else
                    bReturn = false;
            }
            else
                bReturn = false;

            return bReturn;
        }

        #endregion

        #region DeleteRecord
        public bool DeleteRecord(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intReturnValue = 0; int intExecReturn = 0;


            try
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[7];
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.Int); SqlRecordParams[0].Value = intID;
                SqlRecordParams[1] = new SqlParameter("@del_menu_id", SqlDbType.Int); SqlRecordParams[1].Value = intDelMenuID;
                SqlRecordParams[2] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = lUserID;
                SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[3].Value = intMenuID;
                SqlRecordParams[4] = new SqlParameter("@user_name", SqlDbType.NVarChar, 30); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_access_rights_delete", SqlRecordParams);
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

        #region ValidateRecord
        private bool ValidateRecord(MenuList[] ArrMenuObj, ref string ReturnMessage)
        {
            bool bReturn = true;
            if (intID == 0)
            {
                ReturnMessage = "096";
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

        
    }

    public class MenuList
    {
        #region Constructor
        public MenuList()
        {
        }
        #endregion

        #region Variables
        int intMenuID = 0;
        #endregion

        #region Properties
        public int MENU_ID
        {
            get { return intMenuID; }
            set { intMenuID = value; }
        }
        #endregion
    }
}
