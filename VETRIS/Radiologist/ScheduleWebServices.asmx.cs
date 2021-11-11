using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Data;
using VETRIS.Core;
using System.Data.SqlClient;
using VETRIS.Core.Radiologist;

namespace VETRIS.Radiologist
{

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    [ScriptService]
    public class ScheduleWebServices : System.Web.Services.WebService
    {
        classes.CommonClass objComm;


        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public void GetParameters(string userId)
        {
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            List<Radiolgist> readers = new List<Radiolgist>();
            List<RadiolgistGroup> groups = new List<RadiolgistGroup>();
            List<TimezoneData> timezones = new List<TimezoneData>();
            List<ReaderRoleInfo> roleInfo = new List<ReaderRoleInfo>();
            Guid _userId = Guid.Empty;
            if (!string.IsNullOrEmpty(userId)) _userId = new Guid(userId);
            string error = "";
            var bResult = ProductivityScheduling.LoadParameters(Server.MapPath("~"), _userId, ref readers, ref groups, ref timezones, ref roleInfo, ref error);
            if (bResult)
            {
                Context.Response.Write(JsonConvert.SerializeObject(new { readers, groups, timezones, roleInfo = roleInfo.FirstOrDefault() }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
                return;
            }
            Context.Response.StatusCode = 404;
            Context.Response.Write(JsonConvert.SerializeObject(new { error = error }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));

        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public void GetSchedules(string tz, string from, string to, string readerId, int readerGroupId)
        {

            var targetTz = TimeZoneInfo.FindSystemTimeZoneById(tz);
            if (targetTz == null)
            {
                Context.Response.StatusCode = 404;
                Context.Response.Write(JsonConvert.SerializeObject(new { error = "442" }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
                return;
            }
            
            string error = "";
            var defaultTz = ProductivityScheduling.GetDefaltTZ(Server.MapPath("~"), ref error);
            if (defaultTz == null)
            {
                Context.Response.StatusCode = 404;
                Context.Response.Write(JsonConvert.SerializeObject(new { error = "442" }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
                return;
            }
            var tzDef = TimeZoneInfo.FindSystemTimeZoneById(defaultTz.Name);

            var tzDate = Convert.ToDateTime(from);
            var offset = targetTz.GetUtcOffset(tzDate).Hours;
            var tzs = (offset < 0 ? "-" : "+") + string.Format("{0:00.00}", Math.Abs(offset));
            var zone = tzs.Split('.')[0] + string.Format("{0:00}", Convert.ToDouble("." + tzs.Split('.')[1]) * 60);


            var defFrom = TimeZoneInfo.ConvertTime(Convert.ToDateTime(string.Format("{0} {1}", from, zone)), tzDef);

            tzDate = Convert.ToDateTime(to);
            offset = targetTz.GetUtcOffset(tzDate).Hours;
            tzs = (offset < 0 ? "-" : "+") + string.Format("{0:00.00}", Math.Abs(offset));
            zone = tzs.Split('.')[0] + string.Format("{0:00}", Convert.ToDouble("." + tzs.Split('.')[1]) * 60);

            var defTo = TimeZoneInfo.ConvertTime(Convert.ToDateTime(string.Format("{0} {1}", to, zone)), tzDef);
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            List<ProcuctivitySchedule> schedules = new List<ProcuctivitySchedule>();
            List<StatInfo> stats = new List<StatInfo>();
            List<RadiolgistPerformance> readers = new List<RadiolgistPerformance>();
            Guid _readerId = Guid.Empty;
            if (!string.IsNullOrEmpty(readerId)) _readerId = new Guid(readerId);

            var bResult = ProductivityScheduling.LoadSchedule(Server.MapPath("~"), defFrom, defTo, _readerId, readerGroupId, ref schedules, ref error);
            var b2Result = ProductivityScheduling.LoadRadiologistPerformance(Server.MapPath("~"), defFrom, defTo, _readerId, readerGroupId, ref readers, ref stats, ref error);

            if (bResult)
            {
                var schedule_output = new Dictionary<string, List<ScheduleOutput>>();
                var stats_output = new Dictionary<string, List<StatsOutput>>();
                #region convert to required format
                schedules.ForEach(s =>
                {
                    var targetOffset = targetTz.GetUtcOffset(s.StartTime);
                    //var targetOffset = TimeSpan.FromHours(tz);
                    var defOffset = new DateTimeOffset(s.StartTime, tzDef.GetUtcOffset(s.StartTime));
                    var fromTime = defOffset.ToOffset(targetOffset);
                    targetOffset = targetTz.GetUtcOffset(s.EndTime);
                    defOffset = new DateTimeOffset(s.EndTime, tzDef.GetUtcOffset(s.EndTime));
                    var toTime = defOffset.ToOffset(targetOffset);
                    var date = string.Format("{0:yyyy-MM-dd}", fromTime);
                    var record = new ScheduleOutput
                    {
                        Id = s.Id,
                        Range = new string[] { string.Format("{0:HH:mm}", fromTime), string.Format("{0:HH:mm}", toTime) },
                        ReaderId = s.ReaderId,
                        Notes = s.Notes,
                        Original = new ScheduleOriginal
                        {
                            Date = string.Format("{0:yyyy-MM-dd}", s.StartTime),
                            Range = new string[] { string.Format("{0:HH:mm}", s.StartTime), string.Format("{0:HH:mm}", s.EndTime) }
                        }
                    };
                    var existing = schedule_output.Keys.Any(i => i == date);
                    if (!existing)
                    {
                        schedule_output.Add(date, new List<ScheduleOutput> { record });
                    }
                    else
                    {
                        schedule_output[date].Add(record);
                    }
                });
                #endregion
                #region convert stats
                var statsdata = new List<StatInfoTimeline>();
                stats.ForEach(s =>
                {
                    var targetOffset = targetTz.GetUtcOffset(s.Date);
                    var defOffset = new DateTimeOffset(s.Date, tzDef.GetUtcOffset(s.Date));
                    var fromTime = defOffset.ToOffset(targetOffset);

                    var hh = fromTime.Hour;
                    var index = hh;
                    if (hh > 23) index = 23;
                    if (s.Type == 10)
                    {
                        var d = new StatInfoTimeline() { Date = fromTime.Date, Stat1 = s.Stat, TimelineIndex = index, DayOfWeek = (int)s.Date.DayOfWeek };
                        statsdata.Add(d);
                    } else if (s.Type == 20)
                    {
                        var d = new StatInfoTimeline() { Date = fromTime.Date, Stat2 = s.Stat, TimelineIndex = index, DayOfWeek = (int)s.Date.DayOfWeek };
                        statsdata.Add(d);
                    }
                });
                statsdata = statsdata.GroupBy(i => new { 
                    i.Date, i.TimelineIndex, i.DayOfWeek 
                })
                .Select(i => new StatInfoTimeline
                { 
                    Date = i.Key.Date, 
                    TimelineIndex = i.Key.TimelineIndex, 
                    DayOfWeek = i.Key.DayOfWeek,
                    Stat1 = i.Sum(j => j.Stat1),
                    Stat2 = i.Sum(j => j.Stat2)
                })
                .ToList();

                for (DateTime d = defFrom.Date; d <= defTo.Date; d=d.AddDays(1))
                {    var dow = (int)d.DayOfWeek;
                     var data = statsdata.Where(i => i.Date <= d && i.DayOfWeek == dow).ToList();
                     var days1 = data.Where(i => i.Stat1 > 0).Select(i => i.Date).Distinct().Count();
                     var days2 = data.Where(i => i.Stat2 > 0).Select(i => i.Date).Distinct().Count();
                     var counts = data.Where(i => i.Date <= d && i.DayOfWeek == dow)
                                   .GroupBy(i => i.TimelineIndex)
                                   .Select(i => new StatsOutput { 
                                       TimelineIndex = i.Key, 
                                       Count1 = i.Sum(j => j.Stat1) /(days1==0?1:days1),
                                       Count2 = i.Sum(j => j.Stat2) / (days2 == 0 ? 1 : days2)
                                   })
                                   .Where(i=>i.Count1+i.Count2>0)
                                   .ToList();
                     stats_output.Add(d.ToString("yyyy-MM-dd"), counts);
                }
                
                #endregion
                Context.Response.Write(JsonConvert.SerializeObject(new { performance = readers, schedules = schedule_output, stats = stats_output }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
                return;
            }
            Context.Response.StatusCode = 404;
            Context.Response.Write(JsonConvert.SerializeObject(new { error = error }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public SaveOutput SaveSchedule(string id, string date, string startTime, string endTime, string notes, string readerId, string userId, int menuId)
        {

            //Context.Response.Clear();
            //Context.Response.ContentType = "application/json";

            var input = new ScheduleInput();
            if (string.IsNullOrEmpty(id)) input.Id = Guid.Empty;
            else input.Id = new Guid(id);
            if (string.IsNullOrEmpty(readerId)) input.ReaderId = Guid.Empty;
            else input.ReaderId = new Guid(readerId);
            if (string.IsNullOrEmpty(userId)) input.UserId = Guid.Empty;
            else input.UserId = new Guid(userId);
            input.Date = date;
            input.StartTime = startTime;
            input.EndTime = endTime;
            input.Notes = notes;

            string returnMessage = "", catchMessage = "";
            var bResult = ProductivityScheduling.SaveSchedule(Server.MapPath("~"), input, ref returnMessage, ref catchMessage);
            if (bResult)
            {

                Context.Response.StatusCode = 200;
                //Context.Response.Write(JsonConvert.SerializeObject(new { status="success", id = input.Id, returnMessage = returnMessage }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
                return new SaveOutput { status = "success", Id = input.Id, returnMessage = returnMessage };
            }
            Context.Response.StatusCode = 404;
            //Context.Response.Write(JsonConvert.SerializeObject(new {status="error", returnMessage = returnMessage, catchMessage = catchMessage }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
            return new SaveOutput { status = "error", returnMessage = returnMessage, catchMessage = catchMessage };
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public SaveOutput CreateSchedule(string date1, string date2, int days, List<int> weekDays, string startTime, string endTime, string notes, string readerId, string userId, int menuId)
        {

            //Context.Response.Clear();
            //Context.Response.ContentType = "application/json";

            var input = new MultiScheduleInput();
            if (string.IsNullOrEmpty(readerId)) input.ReaderId = Guid.Empty;
            else input.ReaderId = new Guid(readerId);
            if (string.IsNullOrEmpty(userId)) input.UserId = Guid.Empty;
            else input.UserId = new Guid(userId);
            input.Date1 = date1;
            input.Date2 = date2;
            input.StartTime = startTime;
            input.EndTime = endTime;
            input.Notes = notes;
            input.NextDays = days;
            input.WeekDays = weekDays;


            string returnMessage = "", catchMessage = "";
            var bResult = ProductivityScheduling.CreateSchedule(Server.MapPath("~"), input, ref returnMessage, ref catchMessage);
            if (bResult)
            {

                Context.Response.StatusCode = 200;
                return new SaveOutput { status = "success", returnMessage = returnMessage };
            }
            Context.Response.StatusCode = 404;
            return new SaveOutput { status = "error", returnMessage = returnMessage, catchMessage = catchMessage };
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public void DeleteSchedule(string id, string userId, int menuId)
        {

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            var input = new ScheduleInput();
            if (string.IsNullOrEmpty(id)) input.Id = Guid.Empty;
            else input.Id = new Guid(id);
            input.ReaderId = Guid.Empty;
            if (string.IsNullOrEmpty(userId)) input.UserId = Guid.Empty;
            else input.UserId = new Guid(userId);
            string returnMessage = "", catchMessage = "";
            var bResult = ProductivityScheduling.DeleteSchedule(Server.MapPath("~"), input, ref returnMessage, ref catchMessage);
            if (bResult)
            {
                Context.Response.StatusCode = 200;
                Context.Response.Write(JsonConvert.SerializeObject(new { status = "success", returnMessage = returnMessage }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
                return;
            }
            Context.Response.StatusCode = 404;
            Context.Response.Write(JsonConvert.SerializeObject(new { status = "error", returnMessage = returnMessage, catchMessage = catchMessage }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));

        }

    }

    public class SaveOutput
    {
        public string status { get; set; }
        public Guid? Id { get; set; }
        public string returnMessage { get; set; }
        public string catchMessage { get; set; }
    }
}
