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
    public class CaseNotificationRules
    {
        #region Constructor
        public CaseNotificationRules()
        {
        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        string strUserName = string.Empty;

        int intRuleNo= 0;
        string strRuleDesc = string.Empty;
        string strUserRoleCode = string.Empty;
        int intPACStatusID = -999;
        int intPriorityID = 0;
        int intTimeEllapsed = 0;
        string strEllapseHr = "00";
        string strEllapseMin = "00";
        int intTimeLeft = 0;
        string strLeftHr = "00";
        string strLeftMin = "00";
        string strNotifyByTime = "E";
        Guid SecondLevRecID = new Guid("00000000-0000-0000-0000-000000000000");
        string strIsActive = "Y";
        int intRowID = 0;
        string strRadXML = string.Empty;
        string strOthXML = string.Empty;

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
        public int RULE_NUMBER
        {
            get { return intRuleNo; }
            set { intRuleNo = value; }
        }
        public string RULE_DESCRIPTION
        {
            get { return strRuleDesc; }
            set { strRuleDesc = value; }
        }
        public int PACS_STATUS_ID
        {
            get { return intPACStatusID; }
            set { intPACStatusID = value; }
        }
        public int PRIORITY_ID
        {
            get { return intPriorityID; }
            set { intPriorityID = value; }
        }
        public int TIME_ELLAPSED_IN_MINUTES
        {
            get { return intTimeEllapsed; }
            set { intTimeEllapsed = value; }
        }
        public string ELLAPSED_HOURS
        {
            get { return strEllapseHr; }
            set { strEllapseHr = value; }
        }
        public string ELLAPSED_MINUTES
        {
            get { return strEllapseMin; }
            set { strEllapseMin = value; }
        }
        public int TIME_LEFT_IN_MINUTES
        {
            get { return intTimeLeft; }
            set { intTimeLeft = value; }
        }
        public string LEFT_HOURS
        {
            get { return strLeftHr; }
            set { strLeftHr = value; }
        }
        public string LEFT_MINUTES
        {
            get { return strLeftMin; }
            set { strLeftMin = value; }
        }
        public string NOTIFY_BY_TIME
        {
            get { return strNotifyByTime; }
            set { strNotifyByTime = value; }
        }
        public string USER_ROLE_CODE
        {
            get { return strUserRoleCode; }
            set { strUserRoleCode = value; }
        }
        public string IS_ACTIVE
        {
            get { return strIsActive; }
            set { strIsActive = value; }
        }
        public Guid SECOND_LEVEL_RECEIPIENT_ID
        {
            get { return SecondLevRecID; }
            set { SecondLevRecID = value; }
        }
        public int ROW_ID
        {
            get { return intRowID; }
            set { intRowID = value; }
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
                    ds.Tables[1].TableName = "PACSStatus";
                    ds.Tables[2].TableName = "Priority";
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
            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            SqlRecordParams[0] = new SqlParameter("@status_id", SqlDbType.Int); SqlRecordParams[0].Value = intPACStatusID;
            SqlRecordParams[1] = new SqlParameter("@prority_id", SqlDbType.Int); SqlRecordParams[1].Value = intPriorityID;
            SqlRecordParams[2] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[2].Value = strIsActive;
            SqlRecordParams[3] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = UserID;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "settings_case_notification_rules_fetch_brw", SqlRecordParams);
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

            SqlParameter[] SqlRecordParams = new SqlParameter[5];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.Int); SqlRecordParams[0].Value = intRuleNo;
                SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = intMenuID;
                SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_case_notification_rule_fetch_dtls", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";
                    ds.Tables[1].TableName = "PACSStatus";
                    ds.Tables[2].TableName = "Priority";

                    #region Details

                    foreach (DataRow dr in ds.Tables["Details"].Rows)
                    {
                        intRuleNo = Convert.ToInt32(dr["rule_no"]);
                        strRuleDesc = Convert.ToString(dr["rule_desc"]).Trim();
                        intPACStatusID = Convert.ToInt32(dr["pacs_status_id"]);
                        intPriorityID = Convert.ToInt32(dr["priority_id"]);
                        strEllapseHr = Convert.ToString(dr["time_ellapsed_hr"]).Trim();
                        strEllapseMin = Convert.ToString(dr["time_ellapsed_mins"]).Trim();
                        strLeftHr = Convert.ToString(dr["time_left_hr"]).Trim();
                        strLeftMin = Convert.ToString(dr["time_left_mins"]).Trim();
                        strNotifyByTime = Convert.ToString(dr["notify_by_time"]).Trim();
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

        #region LoadRecepients
        public bool LoadRecepients(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.Int); SqlRecordParams[0].Value = intRuleNo;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "settings_case_notification_rule_receipient_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "UserRoles";
                    ds.Tables[1].TableName = "Users";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadOtherRecepients
        public bool LoadOtherRecepients(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.Int); SqlRecordParams[0].Value = intRuleNo;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "settings_case_notification_rule_other_receipient_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "UserRoles";
                    ds.Tables[1].TableName = "Users";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadRadiologists
        public bool LoadRadiologists(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.Int); SqlRecordParams[0].Value = intRuleNo;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "settings_case_notification_rule_radiologist_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Radiologists";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, RadiologistList[] ArrRadObj,RecepientList[] ArrOthObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRecord(ArrRadObj,ArrOthObj, ref ReturnMessage))
            {
                if ((GenerateRadiologistXML(ArrRadObj, ref CatchMessage)) && (GenerateOtherXML(ArrOthObj, ref CatchMessage)))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[15];

                        SqlRecordParams[0] = new SqlParameter("@rule_no", SqlDbType.Int); SqlRecordParams[0].Value = intRuleNo; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
                        SqlRecordParams[1] = new SqlParameter("@rule_desc", SqlDbType.NVarChar, 500); SqlRecordParams[1].Value = strRuleDesc;
                        SqlRecordParams[2] = new SqlParameter("@pacs_status_id", SqlDbType.Int); SqlRecordParams[2].Value = intPACStatusID;
                        SqlRecordParams[3] = new SqlParameter("@priority_id", SqlDbType.Int); SqlRecordParams[3].Value = intPriorityID;
                        SqlRecordParams[4] = new SqlParameter("@time_ellapsed_mins", SqlDbType.Int); SqlRecordParams[4].Value = intTimeEllapsed;
                        SqlRecordParams[5] = new SqlParameter("@time_left_mins", SqlDbType.Int); SqlRecordParams[5].Value = intTimeLeft;
                        SqlRecordParams[6] = new SqlParameter("@notify_by_time", SqlDbType.NChar,1); SqlRecordParams[6].Value = strNotifyByTime;
                        SqlRecordParams[7] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[7].Value = strIsActive;
                        SqlRecordParams[8] = new SqlParameter("@xml_radiologists", SqlDbType.NText); if (strRadXML.Trim() != string.Empty) SqlRecordParams[8].Value = strRadXML; else SqlRecordParams[8].Value = DBNull.Value;
                        SqlRecordParams[9] = new SqlParameter("@xml_others", SqlDbType.NText); if (strOthXML.Trim() != string.Empty) SqlRecordParams[9].Value = strOthXML; else SqlRecordParams[9].Value = DBNull.Value;          
                        SqlRecordParams[10] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[10].Value = UserID;
                        SqlRecordParams[11] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[11].Value = intMenuID;
                        SqlRecordParams[12] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[12].Direction = ParameterDirection.Output;
                        SqlRecordParams[13] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[13].Direction = ParameterDirection.Output;
                        SqlRecordParams[14] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[14].Direction = ParameterDirection.Output;


                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_case_notification_rules_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[14].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[12].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[13].Value);
                        intRuleNo = Convert.ToInt32(SqlRecordParams[0].Value);

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
        private bool ValidateRecord(RadiologistList[] ArrRadObj,RecepientList[] ArrOthObj, ref string ReturnMessage)
        {
            bool bReturn = true;
            if (intPACStatusID == -999)
            {
                ReturnMessage = "210";
            }
            if (intPriorityID == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "102";
            }
            if ((ArrRadObj.Length == 0) && (ArrOthObj.Length == 0))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "209";
            }

            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }
        #endregion

        #region GenerateRadiologistXML
        private bool GenerateRadiologistXML(RadiologistList[] ArrObj, ref string CatchMessage)
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
                        sbXML.Append("<radiologist_id><![CDATA[" + Convert.ToString(ArrObj[i].RADIOLOGIST_ID) + "]]></radiologist_id>");
                        sbXML.Append("<user_id><![CDATA[" + Convert.ToString(ArrObj[i].USER_ID) + "]]></user_id>");
                        sbXML.Append("<notify_if_scheduled><![CDATA[" + Convert.ToString(ArrObj[i].NOTIFY_IF_SCHEDULED) + "]]></notify_if_scheduled>");
                        sbXML.Append("<notify_always><![CDATA[" + Convert.ToString(ArrObj[i].NOTIFY_ALWAYS) + "]]></notify_always>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</data>");
                    strRadXML = sbXML.ToString();


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

        #region GenerateOtherXML
        private bool GenerateOtherXML(RecepientList[] ArrObj, ref string CatchMessage)
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
                        sbXML.Append("<user_role_id>" + ArrObj[i].USER_ROLE_ID.ToString() + "</user_role_id>");
                        sbXML.Append("<scheduled><![CDATA[" + Convert.ToString(ArrObj[i].SCHEDULED) + "]]></scheduled>");
                        sbXML.Append("<notify_all><![CDATA[" + Convert.ToString(ArrObj[i].NOTIFY_ALL) + "]]></notify_all>");
                        sbXML.Append("<user_id><![CDATA[" + Convert.ToString(ArrObj[i].USER_ID) + "]]></user_id>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</data>");
                    strOthXML = sbXML.ToString();


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

    public class RadiologistList
    {
        #region Constructor
        public RadiologistList()
        {
        }
        #endregion

        #region Variables
        Guid RadiologistID = Guid.Empty;
        string strScheduled = string.Empty;
        string strNotifyAlways = string.Empty;
        Guid UserId = Guid.Empty;
        #endregion

        #region Properties
        public Guid RADIOLOGIST_ID
        {
            get { return RadiologistID; }
            set { RadiologistID = value; }
        }
        public Guid USER_ID
        {
            get { return UserId; }
            set { UserId = value; }
        }
        public string NOTIFY_IF_SCHEDULED
        {
            get { return strScheduled; }
            set { strScheduled = value; }
        }
        public string NOTIFY_ALWAYS
        {
            get { return strNotifyAlways; }
            set { strNotifyAlways = value; }
        }
        #endregion
    }
    public class RecepientList
    {
        #region Constructor
        public RecepientList()
        {
        }
        #endregion

        #region Variables
        int intRoleID = 0;
        string strScheduled = string.Empty;
        string strNotifyAll = string.Empty;
        Guid UserId = Guid.Empty;
        #endregion

        #region Properties
        public int USER_ROLE_ID
        {
            get { return intRoleID; }
            set { intRoleID = value; }
        }
        public string SCHEDULED
        {
            get { return strScheduled; }
            set { strScheduled = value; }
        }
        public string NOTIFY_ALL
        {
            get { return strNotifyAll; }
            set { strNotifyAll = value; }
        }
        public Guid USER_ID
        {
            get { return UserId; }
            set { UserId = value; }
        }
        #endregion
    }


}
