using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VETRIS.Core.Radiologist
{
    public class ProductivityScheduling
    {
        public static TimezoneData GetDefaltTZ(string configPath, ref string CatchMessage)
        {

            TimezoneData data = null;

            try
            {

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(configPath);
                var ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.Text, "SELECT [id],[name],[gmt_diff] Offset,[is_default] IsDefault FROM sys_us_time_zones WHERE is_default='Y'");
                if (ds.Tables.Count == 1)
                {
                    data = ds.Tables[0].TableToList<TimezoneData>().FirstOrDefault();
                }


            }
            catch (Exception expErr)
            {
                CatchMessage = expErr.Message;
            }


            return data;
        }
        public static bool LoadParameters(string configPath, Guid? userId, ref List<Radiolgist> readers, ref List<RadiolgistGroup> groups, ref List<TimezoneData> timezones, ref List<ReaderRoleInfo> roleInfo, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = userId;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(configPath);
                var ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "radiologist_productivity_schedule_parameters_fetch", SqlRecordParams);
                if (ds.Tables.Count == 4)
                {
                    readers = ds.Tables[0].TableToList<Radiolgist>();
                    groups = ds.Tables[1].TableToList<RadiolgistGroup>();
                    timezones = ds.Tables[2].TableToList<TimezoneData>();
                    roleInfo = ds.Tables[3].TableToList<ReaderRoleInfo>();
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
        public static bool LoadRadiologistPerformance(string configPath, DateTime fromDt, DateTime toDt, Guid? readerId, int? readerGroupId, ref List<RadiolgistPerformance> readers, ref List<StatInfo> stats, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@from_date", SqlDbType.DateTime); SqlRecordParams[0].Value = fromDt;
                SqlRecordParams[1] = new SqlParameter("@till_date", SqlDbType.DateTime); SqlRecordParams[1].Value = toDt;
                SqlRecordParams[2] = new SqlParameter("@radiologist_id", SqlDbType.UniqueIdentifier);
                if (readerId != null)
                    SqlRecordParams[2].Value = readerId;
                else
                    SqlRecordParams[2].Value = Guid.Empty;
                SqlRecordParams[3] = new SqlParameter("@group_id", SqlDbType.Int); SqlRecordParams[3].Value = readerGroupId ?? 0;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(configPath);
                var ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "radiologist_productivity_schedule_details_fetch", SqlRecordParams);
                if (ds.Tables.Count == 2)
                {
                    readers = ds.Tables[0].TableToList<RadiolgistPerformance>();
                    stats = ds.Tables[1].TableToList<StatInfo>();
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
        public static bool LoadSchedule(string configPath, DateTime fromDt, DateTime toDt, Guid? readerId, int? readerGroupId, ref List<ProcuctivitySchedule> schedules, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@from_date", SqlDbType.DateTime); SqlRecordParams[0].Value = fromDt;
                SqlRecordParams[1] = new SqlParameter("@till_date", SqlDbType.DateTime); SqlRecordParams[1].Value = toDt;
                SqlRecordParams[2] = new SqlParameter("@radiologist_id", SqlDbType.UniqueIdentifier);
                if (readerId != null)
                    SqlRecordParams[2].Value = readerId;
                else
                    SqlRecordParams[2].Value = Guid.Empty;
                SqlRecordParams[3] = new SqlParameter("@group_id", SqlDbType.Int); SqlRecordParams[3].Value = readerGroupId ?? 0;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(configPath);
                var ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "radiologist_productivity_schedule_fetch", SqlRecordParams);
                if (ds.Tables.Count == 1)
                {
                    schedules = ds.Tables[0].TableToList<ProcuctivitySchedule>();
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

        #region SaveSchedule
        public static bool CreateSchedule(string ConfigPath, MultiScheduleInput input, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            try
            {
                if (ValidateMultiSchedule(input, ref ReturnMessage))
                {
                    var xml = GenerateXML(input, ref ReturnMessage, ref CatchMessage);
                    if (!string.IsNullOrEmpty(CatchMessage))
                    {
                        bReturn = false;
                        return false;
                    }
                    SqlParameter[] SqlRecordParams = new SqlParameter[13];
                    SqlRecordParams[0] = new SqlParameter("@xml_radiologist", SqlDbType.NText); SqlRecordParams[0].Value = xml.Item1;
                    SqlRecordParams[1] = new SqlParameter("@start_date", SqlDbType.DateTime); SqlRecordParams[1].Value = Convert.ToDateTime(input.Date1);
                    SqlRecordParams[2] = new SqlParameter("@end_date", SqlDbType.DateTime); SqlRecordParams[2].Value = Convert.ToDateTime(input.Date2);
                    SqlRecordParams[3] = new SqlParameter("@start_time", SqlDbType.NVarChar, 8); SqlRecordParams[3].Value = input.StartTime;
                    SqlRecordParams[4] = new SqlParameter("@end_time", SqlDbType.NVarChar, 8); SqlRecordParams[4].Value = input.EndTime;
                    SqlRecordParams[5] = new SqlParameter("@for_next_days", SqlDbType.NVarChar, 1); SqlRecordParams[5].Value = input.NextDays > 0 ? "Y" : "N";
                    SqlRecordParams[6] = new SqlParameter("@next_days", SqlDbType.Int); SqlRecordParams[6].Value = input.NextDays;
                    SqlRecordParams[7] = new SqlParameter("@xml_weekday", SqlDbType.NText);
                    if (string.IsNullOrEmpty(xml.Item2))
                        SqlRecordParams[7].Value = DBNull.Value;
                    else
                        SqlRecordParams[7].Value = xml.Item2;
                    SqlRecordParams[8] = new SqlParameter("@notes", SqlDbType.NVarChar, 300); SqlRecordParams[8].Value = input.Notes;
                    SqlRecordParams[9] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[9].Value = input.UserId;
                    SqlRecordParams[10] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[10].Value = input.MenuId;
                    SqlRecordParams[11] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[11].Direction = ParameterDirection.Output;
                    SqlRecordParams[12] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[12].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "radiologist_productivity_schedule_create", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[12].Value);
                    if (intReturnValue == 1)
                    {
                        bReturn = true;
                    }
                    else
                        bReturn = false;

                    ReturnMessage = Convert.ToString(SqlRecordParams[11].Value).Trim();
                }
                else
                    bReturn = false;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }

        #endregion

        #region SaveSchedule
        public static bool SaveSchedule(string ConfigPath, ScheduleInput input, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            try
            {
                if (ValidateSchedule(input, ref ReturnMessage))
                {
                    SqlParameter[] SqlRecordParams = new SqlParameter[11];
                    SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = input.Id;
                    SqlRecordParams[1] = new SqlParameter("@radiologist_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = input.ReaderId;
                    SqlRecordParams[2] = new SqlParameter("@start_date", SqlDbType.DateTime); SqlRecordParams[2].Value = Convert.ToDateTime(input.Date);
                    SqlRecordParams[3] = new SqlParameter("@start_time", SqlDbType.NVarChar, 8); SqlRecordParams[3].Value = input.StartTime;
                    SqlRecordParams[4] = new SqlParameter("@end_time", SqlDbType.NVarChar, 8); SqlRecordParams[4].Value = input.EndTime;
                    SqlRecordParams[5] = new SqlParameter("@notes", SqlDbType.NVarChar, 300); SqlRecordParams[5].Value = input.Notes;
                    SqlRecordParams[6] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[6].Value = input.UserId;
                    SqlRecordParams[7] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[7].Value = input.MenuId;
                    SqlRecordParams[8] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[8].Direction = ParameterDirection.Output;
                    SqlRecordParams[9] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[9].Direction = ParameterDirection.Output;
                    SqlRecordParams[10] = new SqlParameter("@return_id", SqlDbType.UniqueIdentifier); SqlRecordParams[10].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "radiologist_productivity_schedule_save", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[9].Value);
                    if (intReturnValue == 1)
                    {
                        bReturn = true;
                        if (input.Id == Guid.Empty)
                        {
                            input.Id = new Guid(SqlRecordParams[10].Value.ToString());
                        }
                    }
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
        public static bool DeleteSchedule(string ConfigPath, ScheduleInput input, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            try
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[5];
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = input.Id; ;
                SqlRecordParams[1] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = input.UserId;
                SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = input.MenuId;
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
        private static bool ValidateSchedule(ScheduleInput input, ref string ReturnMessage)
        {
            bool bReturn = true;

            if (Convert.ToDateTime(input.Date).Year == 1900)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "185";
            }
            else
            {
                if (Convert.ToDateTime(input.Date) < DateTime.Today)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage = "186";
                }
            }

            if (input.Notes.Trim().Length > 250)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "200";
            }


            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }
        private static bool ValidateMultiSchedule(MultiScheduleInput input, ref string ReturnMessage)
        {
            bool bReturn = true;

            if (Convert.ToDateTime(input.Date1).Year == 1900)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "185";
            }
            else
            {
                if (Convert.ToDateTime(input.Date1) < DateTime.Today)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage = "186";
                }
            }

            if (input.Notes.Trim().Length > 250)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "200";
            }


            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }

        #endregion



        #region GenerateXML
        private static Tuple<string, string> GenerateXML(MultiScheduleInput input, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();
            StringBuilder wkXML = new StringBuilder();
            try
            {

                sbXML.Append("<radiologist>");

                sbXML.Append("<row>");
                sbXML.Append("<radiologist_id>" + input.ReaderId.ToString() + "</radiologist_id>");
                sbXML.Append("<row_id>1</row_id>");
                sbXML.Append("</row>");

                if (ReturnMessage.Trim() != string.Empty)
                {
                    bReturn = false;
                    sbXML.Clear();
                }
                else
                {
                    bReturn = true;
                    sbXML.Append("</radiologist>");
                }


                if (input.WeekDays != null && input.WeekDays.Count > 0)
                {

                    wkXML.Append("<weekday>");

                    for (int i = 0; i < input.WeekDays.Count; i = i + 1)
                    {
                        wkXML.Append("<row>");
                        wkXML.Append("<week_day_no>" + input.WeekDays[i].ToString() + "</week_day_no>");
                        wkXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        wkXML.Append("</row>");
                    }

                    wkXML.Append("</weekday>");


                }
                bReturn = true;

            }
            catch (Exception LexpErr)
            {
                bReturn = false;
                CatchMessage = LexpErr.Message;
            }
            return new Tuple<string, string>(sbXML.ToString(), wkXML.ToString());
        }
        #endregion

        public static string strXMLWD { get; set; }

        public string strXMLRad { get; set; }
    }

    public class Radiolgist
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupColor { get; set; }
        public int ThresholdPerHr { get; set; }
        public int TimeZoneId { get; set; }
        public int Rights { get; set; }

    }
    public class ReaderRoleInfo
    {
        public Guid DefaultReaderId { get; set; }
        public int TimeZoneId { get; set; }
        public string RoleCode { get; set; }

    }
    public class StatInfo
    {
        public int Stat { get; set; }
        public DateTime Date { get; set; }
        public int Type { get; set; }

    }
    public class StatInfoTimeline 
    {
        public DateTime Date { get; set; }
        public int DayOfWeek { get; set; }
        public int TimelineIndex { get; set; }
        public int Stat1 { get; set; }
        public int Stat2 { get; set; }
    }
    public class RadiolgistGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

    }
    public class TimezoneData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Offset { get; set; }
        public string IsDefault { get; set; }

    }

    public class RadiolgistPerformance
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double HrsScheduled { get; set; }
        public double studyCountPerHr { get; set; }

    }
    public class ProcuctivitySchedule
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Notes { get; set; }
        public Guid ReaderId { get; set; }
    }
    public class ScheduleOutput
    {
        public Guid Id { get; set; }
        public string[] Range { get; set; }
        public string Notes { get; set; }
        public Guid ReaderId { get; set; }
        public ScheduleOriginal Original { get; set; }
    }
    public class StatsOutput
    {
        public int TimelineIndex { get; set; }
        public int Count1 { get; set; }
        public int Count2 { get; set; }
    }
    public class ScheduleInput
    {
        public Guid Id { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Notes { get; set; }
        public Guid ReaderId { get; set; }
        public Guid UserId { get; set; }
        public int MenuId { get; set; }


    }
    public class MultiScheduleInput
    {
        public string Date1 { get; set; }
        public string Date2 { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Notes { get; set; }
        public int NextDays { get; set; }
        public List<int> WeekDays { get; set; }
        public Guid ReaderId { get; set; }
        public Guid UserId { get; set; }
        public int MenuId { get; set; }


    }
    public class ScheduleOriginal
    {
        public string Date { get; set; }
        public string[] Range { get; set; }
    }
    public static class CollectionExtensions
    {
        /// <summary>
        /// Data table to list of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> TableToList<T>(this DataTable table) where T : new()
        {
            List<T> list = new List<T>();
            var typeProperties = typeof(T).GetProperties().Select(propertyInfo => new
            {
                PropertyInfo = propertyInfo,
                Type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType
            }).ToList();

            foreach (var row in table.Rows.Cast<DataRow>())
            {
                T obj = new T();
                foreach (var typeProperty in typeProperties)
                {
                    object value = row[typeProperty.PropertyInfo.Name];
                    object safeValue = value == null || DBNull.Value.Equals(value)
                        ? null
                        : Convert.ChangeType(value, typeProperty.Type);

                    typeProperty.PropertyInfo.SetValue(obj, safeValue, null);
                }
                list.Add(obj);
            }
            return list;
        }
    }
}
