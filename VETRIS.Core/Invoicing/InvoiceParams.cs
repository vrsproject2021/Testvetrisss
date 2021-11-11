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

namespace VETRIS.Core.Invoicing
{
    public class InvoiceParams
    {
        #region Constractor
        public InvoiceParams()
        {

        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        string strUserName = string.Empty;
        string CatchMessage = string.Empty;
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

        #endregion


        #region LoadRecord
        public bool LoadRecord(string ConfigPath,  ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[5];
            SqlRecordParams[0] = new SqlParameter("@user_role_id", SqlDbType.Int); SqlRecordParams[0].Value = intUserRoleID;
            SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = intMenuID;
            SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
            SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[3].Direction = ParameterDirection.Output;
            SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;
            
            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "invoicing_control_params_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Parameters";
                    //ds.Tables[1].TableName = "Settings";
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
        public bool SaveRecord(string ConfigPath, ControlList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intReturnValue = 0; int intExecReturn = 0;


            if (GenerateSettingsXML(ArrObj, ref ReturnMessage, ref CatchMessage))
            {

                try
                {
                    SqlParameter[] SqlRecordParams = new SqlParameter[6];

                    SqlRecordParams[0] = new SqlParameter("@xml_params", SqlDbType.NText); SqlRecordParams[0].Value = strXML.Trim();
                    SqlRecordParams[1] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserID;
                    SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = intMenuID;
                    SqlRecordParams[3] = new SqlParameter("@user_name", SqlDbType.NVarChar, 30); SqlRecordParams[3].Direction = ParameterDirection.Output;
                    SqlRecordParams[4] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[4].Direction = ParameterDirection.Output;
                    SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "invoicing_control_params_save", SqlRecordParams);
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
                bReturn = false;


            return bReturn;
        }

        #endregion

        #region GenerateSettingsXML
        private bool GenerateSettingsXML(ControlList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intFlag = 0;
            StringBuilder sbXML = new StringBuilder();
            try
            {

                sbXML.Append("<params>");

                for (int i = 0; i < ArrObj.Length; i++)
                {
                    sbXML.Append("<row>");
                    sbXML.Append("<control_code>" + ArrObj[i].CONTROL_CODE + "</control_code>");
                    if (ArrObj[i].DATA_VALUE_CHAR.Trim().Length > 2000)
                    {
                        ReturnMessage = "321";
                        intFlag = 0;
                        break;
                    }
                    sbXML.Append("<data_value_char><![CDATA[" + ArrObj[i].DATA_VALUE_CHAR + "]]></data_value_char>");
                    sbXML.Append("<data_value_int>" + ArrObj[i].DATA_VALUE_INT.ToString() + "</data_value_int>");
                    sbXML.Append("<data_value_dec>" + ArrObj[i].DATA_VALUE_DEC.ToString() + "</data_value_dec>");
                    sbXML.Append("<value_type><![CDATA[" + ArrObj[i].VALUE_TYPE + "]]></value_type>");
                    sbXML.Append("<ui_prefix><![CDATA[" + ArrObj[i].UI_PREFIX + "]]></ui_prefix>");
                    sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                    sbXML.Append("</row>");
                    intFlag = 1;
                }
                if (intFlag == 1)
                {
                    bReturn = true;
                    sbXML.Append("</params>");
                    strXML = sbXML.ToString();
                }
                else
                {
                    bReturn = false;

                }



            }
            catch (Exception expErr)
            {
                bReturn = false;
                CatchMessage = expErr.Message;
            }
            return bReturn;
        }
        #endregion


    }

    public class ControlList
    {
        #region Constructor
        public ControlList()
        {

        }
        #endregion

        #region Variables
        string strCtrlCode = "";
        string strDataValuC = "";
        int intDataValueInt = 0;
        decimal desDataValueDec = 0;
        string strValueType = "";
        string strUiPrefix = "";
        #endregion

        #region Properties
        public string CONTROL_CODE
        {
            get { return strCtrlCode; }
            set { strCtrlCode = value; }
        }
        public string DATA_VALUE_CHAR
        {
            get { return strDataValuC; }
            set { strDataValuC = value; }
        }
        public int DATA_VALUE_INT
        {
            get { return intDataValueInt; }
            set { intDataValueInt = value; }
        }
        public decimal DATA_VALUE_DEC
        {
            get { return desDataValueDec; }
            set { desDataValueDec = value; }
        }
        public string VALUE_TYPE
        {
            get { return strValueType; }
            set { strValueType = value; }
        }
        public string UI_PREFIX
        {
            get { return strUiPrefix; }
            set { strUiPrefix = value; }
        }
        #endregion
    }
}
