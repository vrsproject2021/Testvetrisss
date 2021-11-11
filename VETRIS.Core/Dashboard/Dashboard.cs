using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VETRIS.Core.Dashboard
{
    public class Dashboard
    {
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int id = 0;
        int mnuId = 0;
        DateTime fromDate;
        DateTime toDate;
        int? type = null;
        int? modality_id = null;
        int monthCount = 0;
        string orderBy = string.Empty;
        public Guid USER_ID
        {
            get { return UserID; }
            set { UserID = value; }
        }
        public int MENU_ID
        {
            get { return mnuId; }
            set { mnuId = value; }
        }
        public int DASHMENU_ID
        {
            get { return id; }
            set { id = value; }
        }
        public DateTime FROM_DATE
        {
            get { return fromDate; }
            set { fromDate = value; }
        }
        public DateTime TO_DATE
        {
            get { return toDate; }
            set { toDate = value; }
        }
        public int? TYPE
        {
            get { return type; }
            set { type = value; }
        }
        public int? MODALITY_ID
        {
            get { return modality_id; }
            set { modality_id = value; }
        }
        public int MONTH_COUNT
        {
            get { return monthCount; }
            set { monthCount = value; }
        }

        public string ORDER_BY
        {
            get { return orderBy; }
            set { orderBy = value; }
        }

        #region LoadDashboardMenu
        public bool LoadDashboardMenu(string ConfigPath,ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];
            SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserID;
            SqlRecordParams[1] = new SqlParameter("@id", SqlDbType.Int); SqlRecordParams[1].Value = id;
            

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "dashboard_settings_menu", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "DashboardMenuList";
                    ds.Tables[1].TableName = "DefaultReport";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchDashboardOpenCase
        public bool FetchDashboardOpenCase(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            try
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[1];
                SqlRecordParams[0] = new SqlParameter("@mnu_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = mnuId;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "dashboard_open_case", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "modality_open_case_normal";
                    ds.Tables[1].TableName = "status_open_case_normal";
                    ds.Tables[2].TableName = "elapsed_time_open_case_normal";

                    ds.Tables[3].TableName = "modality_open_case_stat";
                    ds.Tables[4].TableName = "status_open_case_stat";
                    ds.Tables[5].TableName = "elapsed_time_open_case_stat";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchModality
        public bool FetchModality(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "dashboard_modality_list_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Modality";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region SearchLineChartNewCases
        public bool SearchLineChartNewCases(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[3];
            SqlRecordParams[0] = new SqlParameter("@fromDate", SqlDbType.Date); SqlRecordParams[0].Value = fromDate;
            SqlRecordParams[1] = new SqlParameter("@toDate", SqlDbType.Date); SqlRecordParams[1].Value = toDate;
            SqlRecordParams[2] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[2].Value = modality_id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "dashboard_new_case_hourly_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "TimeSlot";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region SearchLineChartCompletedCases
        public bool SearchLineChartCompletedCases(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            SqlRecordParams[0] = new SqlParameter("@fromDate", SqlDbType.Date); SqlRecordParams[0].Value = fromDate;
            SqlRecordParams[1] = new SqlParameter("@toDate", SqlDbType.Date); SqlRecordParams[1].Value = toDate;
            SqlRecordParams[2] = new SqlParameter("@type", SqlDbType.Int); SqlRecordParams[2].Value = type;
            SqlRecordParams[3] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[3].Value = modality_id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "dashboard_completed_case_hourly_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "TimeSlot";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region SearchMonthlyRevenueData
        public bool SearchMonthlyRevenueData(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];
            SqlRecordParams[0] = new SqlParameter("@month_count", SqlDbType.Int); SqlRecordParams[0].Value = monthCount;
            SqlRecordParams[1] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[1].Value = modality_id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "dashboard_modality_revenue_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "revenue";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region SearchLineChartSubmittedCases
        public bool SearchLineChartSubmittedCases(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            SqlRecordParams[0] = new SqlParameter("@fromDate", SqlDbType.Date); SqlRecordParams[0].Value = fromDate;
            SqlRecordParams[1] = new SqlParameter("@toDate", SqlDbType.Date); SqlRecordParams[1].Value = toDate;
            SqlRecordParams[2] = new SqlParameter("@type", SqlDbType.Int); SqlRecordParams[2].Value = type;
            SqlRecordParams[3] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[3].Value = modality_id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "dashboard_submitted_case_hourly_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "submitted_cases";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region SearchPeriodicRadiologistProductivity
        public bool SearchPeriodicRadiologistProductivity(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            SqlRecordParams[0] = new SqlParameter("@fromDate", SqlDbType.Date); SqlRecordParams[0].Value = fromDate;
            SqlRecordParams[1] = new SqlParameter("@toDate", SqlDbType.Date); SqlRecordParams[1].Value = toDate;
            SqlRecordParams[2] = new SqlParameter("@modality_id", SqlDbType.Int); SqlRecordParams[2].Value = modality_id;
            SqlRecordParams[3] = new SqlParameter("@orderBy", SqlDbType.NVarChar); SqlRecordParams[3].Value = orderBy;
            
            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "dashboard_periodic_radiologist_productivity", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "radiologist_productivity";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion
    }

    public class DashboardData
    {
        public string name { get; set; }
        public int value { get; set; }
    }
    public class DashboardOpenCaseList
    {
        public DashboardOpenCaseList()
        {
            modality_open_case_normal = new List<DashboardData>();
            modality_status_normal = new List<DashboardData>();
            elapsed_time_normal = new List<DashboardData>();
            modality_status_stat = new List<DashboardData>();
            modality_status_stat = new List<DashboardData>();
            elapsed_time_stat = new List<DashboardData>();
        }
        public List<DashboardData> modality_open_case_normal { get; set; }
        public List<DashboardData> modality_status_normal { get; set; }
        public List<DashboardData> elapsed_time_normal { get; set; }
        public List<DashboardData> modality_open_case_stat { get; set; }
        public List<DashboardData> modality_status_stat { get; set; }
        public List<DashboardData> elapsed_time_stat { get; set; }
    }

    
    public class SlotData
    {
        public string label { get; set; }
        public List<int> data { get; set; }
    }
    public class MonthData
    {
        public string label { get; set; }
        public List<decimal> data { get; set; }
    }
    public class LineData
    {
        public List<string> labels { get; set; }
        public List<SlotData> datasets { get; set; }
        public List<MonthData> monthsets { get; set; }
    }

    public class RevenueData
    {
        public string month { get; set; }
        public decimal invoiced_amount { get; set; }
        public string code { get; set; }

    }
}
