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
    public class Broadcast
    {
         #region Constructor
        public Broadcast()
        {
        }
        #endregion

        #region Variables
        Guid UserID                 = new Guid("00000000-0000-0000-0000-000000000000");
        int intMenuID = 0;
        int intUserRoleID           = 0;
        string strUserName          = string.Empty;
        string strCode              = string.Empty;


        Guid Id                  = new Guid("00000000-0000-0000-0000-000000000000");
        int intRowID                = 0;
        string strName              = string.Empty;
        string strRecipientNo       = string.Empty;
        string strRecipientEmail    = string.Empty;

        string strEmailSubject      = string.Empty;
        string strEmailBody         = string.Empty;
        string strSMSText           = string.Empty;
        string strIsActive          = "X";
        string strXML               = string.Empty;
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
        public string NAME
        {
            get { return strName; }
            set { strName = value; }
        }
        public string RECIPEINT_EMAIL 
        {
            get { return strRecipientEmail; }
            set { strRecipientEmail = value; }
        }
        public string RECIPIENT_NO 
        {
            get { return strRecipientNo; }
            set { strRecipientNo = value; }
        }
        public string EMAIL_SUBJECT 
        {
            get { return strEmailSubject; }
            set { strEmailSubject = value; }
        }
        public string EMAIL_BODY 
        {
            get { return strEmailBody; }
            set { strEmailBody = value; }
        }
        public string SMS_TEXT 
        {
            get { return strSMSText; }
            set { strSMSText = value; }
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

        #region SearchBrowserList
        public bool SearchBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserID;
            //SqlRecordParams[1] = new SqlParameter("@code", SqlDbType.NVarChar, 6); SqlRecordParams[1].Value = strCode;
            //SqlRecordParams[2] = new SqlParameter("@name", SqlDbType.NVarChar, 30); SqlRecordParams[2].Value = strName;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "hk_message_recipient_list_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Institution";
                    ds.Tables[1].TableName = "Radiologist";
                    ds.Tables[2].TableName = "Salesperson";
                    ds.Tables[3].TableName = "Technician";
                    ds.Tables[4].TableName = "Transcriptionist";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

         #region SaveRecord
        public bool SaveRecord(string ConfigPath, Broadcast[] ArrObj,string as_BroadcastFlag, ref string ReturnMessage, ref string CatchMessage)
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
                        SqlParameter[] SqlRecordParams = new SqlParameter[10];
                        SqlRecordParams[0] = new SqlParameter("@xml_data", SqlDbType.NText); SqlRecordParams[0].Value = strXML.Trim();
                        SqlRecordParams[1] = new SqlParameter("@email_subject", SqlDbType.NVarChar,100); SqlRecordParams[1].Value = strEmailSubject;
                        SqlRecordParams[2] = new SqlParameter("@email_body", SqlDbType.NText); SqlRecordParams[2].Value = strEmailBody;
                        SqlRecordParams[3] = new SqlParameter("@sms_text", SqlDbType.NVarChar); SqlRecordParams[3].Value = strSMSText;
                        SqlRecordParams[4] = new SqlParameter("@broadcast_flag", SqlDbType.Char, 1); SqlRecordParams[4].Value = as_BroadcastFlag;
                        
                        SqlRecordParams[5] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[5].Value = UserID;
                        SqlRecordParams[6] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[6].Value = intMenuID;
                        SqlRecordParams[7] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[7].Direction = ParameterDirection.Output;
                        SqlRecordParams[8] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[8].Direction = ParameterDirection.Output;
                        SqlRecordParams[9] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[9].Direction = ParameterDirection.Output;


                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "hk_broadcast_log_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[9].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[7].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[8].Value);




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
        private bool ValidateRecord(Broadcast[] ArrObj, ref string ReturnMessage, ref int RowID)
        {
            bool bReturn = true;
            if (ArrObj.Length == 0)
            {
                ReturnMessage = "175";
            }
            else
            {
                for (int i = 0; i < ArrObj.Length; i = i + 5)
                {
                    if (ArrObj[i].NAME.Trim() == string.Empty)
                    {

                        ReturnMessage = "069";//--- err code not define.
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
        private bool GenerateXML(Broadcast[] ArrObj, ref string CatchMessage)
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
                        sbXML.Append("<id>" + ArrObj[i].ID.ToString() + "</id>");
                        sbXML.Append("<name><![CDATA[" + ArrObj[i].NAME + "]]></name>");
                        sbXML.Append("<email_id><![CDATA[" + ArrObj[i].RECIPEINT_EMAIL + "]]></email_id>");
                        sbXML.Append("<mobile><![CDATA[" + ArrObj[i].RECIPIENT_NO + "]]></mobile>");
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
