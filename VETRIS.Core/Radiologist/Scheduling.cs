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


namespace VETRIS.Core.Radiologist
{
    public class Scheduling
    {
        #region Constructor
        public Scheduling()
        {

        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        string strUserRoleCode = string.Empty;
        string strUserName = string.Empty;

        Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
        Guid RadId = new Guid("00000000-0000-0000-0000-000000000000");
        DateTime dtStart = DateTime.Today;
        DateTime dtEnd = DateTime.Today;
        string strStartTime = "00:00:00";
        string strEndTime = "00:00:00";
        string strForNextDays = "N";
        int intNextDays = 0;
        int intMonthNo = 0;
        int intYearNo = 0;
        string strViewSchedule = "A";
        string strNotes = string.Empty;
        string strRADCALSTARTTIME = "00:00";
        string strXMLWD = string.Empty;
        string strXMLRad = string.Empty;
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
        public string USER_ROLE_CODE
        {
            get { return strUserRoleCode; }
            set { strUserRoleCode = value; }
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
        public Guid RADIOLOGIST_ID
        {
            get { return RadId; }
            set { RadId = value; }
        }
        public string CALENDER_START_TIME
        {
            get { return strRADCALSTARTTIME; }
            set { strRADCALSTARTTIME = value; }
        }
        public DateTime START_DATE
        {
            get { return dtStart; }
            set { dtStart = value; }
        }
        public DateTime END_DATE
        {
            get { return dtEnd; }
            set { dtEnd = value; }
        }
        public string FOR_NEXT_DAYS
        {
            get { return strForNextDays; }
            set { strForNextDays = value; }
        }
        public int NEXT_DAYS
        {
            get { return intNextDays; }
            set { intNextDays = value; }
        }
        public string START_TIME
        {
            get { return strStartTime; }
            set { strStartTime = value; }
        }
        public string END_TIME
        {
            get { return strEndTime; }
            set { strEndTime = value; }
        }
        public int MONTH_NUMBER
        {
            get { return intMonthNo; }
            set { intMonthNo = value; }
        }
        public int YEAR_NUMBER
        {
            get { return intYearNo; }
            set { intYearNo = value; }
        }
        public string NOTES
        {
            get { return strNotes; }
            set { strNotes = value; }
        }
        public string VIEW_SCHEDULE
        {
            get { return strViewSchedule; }
            set { strViewSchedule = value; }
        }
        #endregion

        #region LoadRadiologist
        public bool LoadRadiologist(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            try
            {

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "radiologist_schedule_radiologists_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Radiologist";
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

        #region LoadParameters
        public bool LoadParameters(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "radiologist_schedule_parameters_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Radiologist";
                    ds.Tables[1].TableName = "Params";

                    foreach (DataRow dr in ds.Tables["Params"].Rows)
                    {
                        RadId = new Guid(Convert.ToString(dr["radiologist_id"]));
                        strViewSchedule = Convert.ToString(dr["schedule_view"]).Trim();
                        strUserRoleCode = Convert.ToString(dr["user_role_code"]).Trim();
                        strRADCALSTARTTIME = Convert.ToString(dr["RADCALSTARTTIME"]).Trim();
                    }
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

        #region LoadScheduleDetails
        public bool LoadScheduleDetails(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[3];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@from_date", SqlDbType.DateTime); SqlRecordParams[0].Value = dtStart;
                SqlRecordParams[1] = new SqlParameter("@till_date", SqlDbType.DateTime); SqlRecordParams[1].Value = dtEnd;
                SqlRecordParams[2] = new SqlParameter("@radiologist_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = RadId;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "radiologist_schedule_details_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Schedule";
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

        #region CreateSchedule
        public bool CreateSchedule(string ConfigPath, Weekdays[] ArrWDObj, RadiologistList[] ArrRadObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRecord(ArrRadObj, ref ReturnMessage))
            {
                if ((GenerateWeekdayXML(ArrWDObj, ref CatchMessage) && (GenerateRadiologistXML(ArrRadObj, ref ReturnMessage, ref CatchMessage))))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[12];
                        SqlRecordParams[0] = new SqlParameter("@xml_radiologist", SqlDbType.NText); SqlRecordParams[0].Value = strXMLRad.Trim();
                        SqlRecordParams[1] = new SqlParameter("@start_date", SqlDbType.DateTime); SqlRecordParams[1].Value = dtStart;
                        SqlRecordParams[2] = new SqlParameter("@end_date", SqlDbType.DateTime); SqlRecordParams[2].Value = dtEnd;
                        SqlRecordParams[3] = new SqlParameter("@for_next_days", SqlDbType.NChar, 1); SqlRecordParams[3].Value = strForNextDays;
                        SqlRecordParams[4] = new SqlParameter("@next_days", SqlDbType.Int); SqlRecordParams[4].Value = intNextDays;
                        SqlRecordParams[5] = new SqlParameter("@xml_weekday", SqlDbType.NText); if (strXMLWD.Trim() != string.Empty) SqlRecordParams[5].Value = strXMLWD.Trim(); else SqlRecordParams[5].Value = DBNull.Value;
                        SqlRecordParams[6] = new SqlParameter("@start_time", SqlDbType.NVarChar, 8); SqlRecordParams[6].Value = strStartTime;
                        SqlRecordParams[7] = new SqlParameter("@end_time", SqlDbType.NVarChar, 8); SqlRecordParams[7].Value = strEndTime;
                        SqlRecordParams[8] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[8].Value = UserID;
                        SqlRecordParams[9] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[9].Value = intMenuID;
                        SqlRecordParams[10] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[10].Direction = ParameterDirection.Output;
                        SqlRecordParams[11] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[11].Direction = ParameterDirection.Output;

                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "radiologist_schedule_create", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[11].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        ReturnMessage = Convert.ToString(SqlRecordParams[10].Value).Trim();
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

        #region CancelSchedule
        public bool CancelSchedule(string ConfigPath, Weekdays[] ArrWDObj, RadiologistList[] ArrRadObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRecord(ArrRadObj, ref ReturnMessage))
            {
                if ((GenerateWeekdayXML(ArrWDObj, ref CatchMessage) && (GenerateRadiologistXML(ArrRadObj, ref ReturnMessage, ref CatchMessage))))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[12];
                        SqlRecordParams[0] = new SqlParameter("@xml_radiologist", SqlDbType.NText); SqlRecordParams[0].Value = strXMLRad.Trim();
                        SqlRecordParams[1] = new SqlParameter("@start_date", SqlDbType.DateTime); SqlRecordParams[1].Value = dtStart;
                        SqlRecordParams[2] = new SqlParameter("@end_date", SqlDbType.DateTime); SqlRecordParams[2].Value = dtEnd;
                        SqlRecordParams[3] = new SqlParameter("@for_next_days", SqlDbType.NChar, 1); SqlRecordParams[3].Value = strForNextDays;
                        SqlRecordParams[4] = new SqlParameter("@next_days", SqlDbType.Int); SqlRecordParams[4].Value = intNextDays;
                        SqlRecordParams[5] = new SqlParameter("@xml_weekday", SqlDbType.NText); if (strXMLWD.Trim() != string.Empty) SqlRecordParams[5].Value = strXMLWD.Trim(); else SqlRecordParams[5].Value = DBNull.Value;
                        SqlRecordParams[6] = new SqlParameter("@start_time", SqlDbType.NVarChar, 8); SqlRecordParams[6].Value = strStartTime;
                        SqlRecordParams[7] = new SqlParameter("@end_time", SqlDbType.NVarChar, 8); SqlRecordParams[7].Value = strEndTime;
                        SqlRecordParams[8] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[8].Value = UserID;
                        SqlRecordParams[9] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[9].Value = intMenuID;
                        SqlRecordParams[10] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[10].Direction = ParameterDirection.Output;
                        SqlRecordParams[11] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[11].Direction = ParameterDirection.Output;

                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "radiologist_schedule_cancel", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[11].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        ReturnMessage = Convert.ToString(SqlRecordParams[10].Value).Trim();
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
        private bool ValidateRecord(RadiologistList[] ArrRadObj, ref string ReturnMessage)
        {
            bool bReturn = true;

            if (dtStart.Year == 1900)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "185";
            }
            else
            {
                if (dtStart < DateTime.Today)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage = "186";
                }
            }
            if (strForNextDays.Trim() == "Y")
            {
                if (intNextDays <= 0)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage = "187";
                }
            }
            else
            {
                if (dtEnd.Year == 1900)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage = "188";
                }
                else
                {
                    if (dtStart > dtEnd)
                    {
                        if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                        ReturnMessage = "189";
                    }
                }
            }

            if (ArrRadObj.Length == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "190";
            }

            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }

        #endregion

        #region UpdateSchedule
        public bool UpdateSchedule(string ConfigPath, string SQLQuery, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;



            try
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[5];
                SqlRecordParams[0] = new SqlParameter("@strQuery", SqlDbType.NVarChar, 4000); SqlRecordParams[0].Value = SQLQuery.Trim();
                SqlRecordParams[1] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserID;
                SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = intMenuID;
                SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "radiologist_schedule_update", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[4].Value);
                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;

                ReturnMessage = Convert.ToString(SqlRecordParams[3].Value).Trim();
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }





            return bReturn;
        }
        #endregion

        #region SaveSchedule
        public bool SaveSchedule(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            try
            {
                if (ValidateSchedule(ref ReturnMessage))
                {
                    SqlParameter[] SqlRecordParams = new SqlParameter[10];
                    SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                    SqlRecordParams[1] = new SqlParameter("@radiologist_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = RadId;
                    SqlRecordParams[2] = new SqlParameter("@start_date", SqlDbType.DateTime); SqlRecordParams[2].Value = dtStart;
                    SqlRecordParams[3] = new SqlParameter("@start_time", SqlDbType.NVarChar, 8); SqlRecordParams[3].Value = strStartTime;
                    SqlRecordParams[4] = new SqlParameter("@end_time", SqlDbType.NVarChar, 8); SqlRecordParams[4].Value = strEndTime;
                    SqlRecordParams[5] = new SqlParameter("@notes", SqlDbType.NVarChar, 8); SqlRecordParams[5].Value = strNotes;
                    SqlRecordParams[6] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[6].Value = UserID;
                    SqlRecordParams[7] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[7].Value = intMenuID;
                    SqlRecordParams[8] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[8].Direction = ParameterDirection.Output;
                    SqlRecordParams[9] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[9].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "radiologist_schedule_save", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[9].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;

                    ReturnMessage = Convert.ToString(SqlRecordParams[8].Value).Trim();
                }
                else
                    bReturn = false;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }





            return bReturn;
        }
        #endregion

        #region DeleteSchedule
        public bool DeleteSchedule(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            try
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[5];
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserID;
                SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = intMenuID;
                SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "radiologist_schedule_delete", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[4].Value);
                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;

                ReturnMessage = Convert.ToString(SqlRecordParams[3].Value).Trim();
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }





            return bReturn;
        }
        #endregion

        #region ValidateSchedule
        private bool ValidateSchedule(ref string ReturnMessage)
        {
            bool bReturn = true;

            if (dtStart.Year == 1900)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "185";
            }
            else
            {
                if (dtStart < DateTime.Today)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage = "186";
                }
            }

            if (strNotes.Trim().Length > 250)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "200";
            }


            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }

        #endregion

        #region GenerateWeekdayXML
        private bool GenerateWeekdayXML(Weekdays[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<weekday>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<week_day_no>" + ArrObj[i].DAY_NUMBER.ToString() + "</week_day_no>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</weekday>");
                    strXMLWD = sbXML.ToString();


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

        #region GenerateRadiologistXML
        private bool GenerateRadiologistXML(RadiologistList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<radiologist>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<radiologist_id>" + ArrObj[i].ID.ToString() + "</radiologist_id>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    if (ReturnMessage.Trim() != string.Empty)
                    {
                        bReturn = false;
                        sbXML.Clear();
                        strXMLRad = string.Empty;
                    }
                    else
                    {
                        bReturn = true;
                        sbXML.Append("</radiologist>");
                        strXMLRad = sbXML.ToString();
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


    }

    public class RadiologistList
    {
        #region Constructor
        public RadiologistList()
        {
        }
        #endregion

        #region Variables
        Guid Id = Guid.Empty;
        #endregion

        #region Properties
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
        #endregion
    }
    public class Weekdays
    {
        #region Constructor
        public Weekdays()
        {
        }
        #endregion

        #region Variables
        int intDayNo = 0;
        #endregion

        #region Properties

        public int DAY_NUMBER
        {
            get { return intDayNo; }
            set { intDayNo = value; }
        }

        #endregion
    }
}
