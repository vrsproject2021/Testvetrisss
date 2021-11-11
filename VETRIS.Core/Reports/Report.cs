using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using VETRIS.DAL;

namespace VETRIS.Core.Reports
{
    public class Report
    {
        #region Constructor
        public Report()
        {
        }
        #endregion

        #region Variables
        Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public Guid InstitutionId { get; set; }
        public string PromoReasonId { get; set; }
        #endregion

        #region Properties
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public Guid USER_ID
        {
            get { return UserID; }
            set { UserID = value; }
        }
        #endregion

        #region Discount List Properties
        public Guid id { get; set; }
        public string promo_reason { get; set; }
        public DateTime received_date { get; set; }
        public string patient_name { get; set; }
        public string category { get; set; }
        public string modality { get; set; }
        public string priority { get; set; }
        public string institution { get; set; }
        public string billing_account { get; set; }
        public string invoice_no { get; set; }
        public DateTime invoice_date { get; set; }
        public decimal? study_cost { get; set; }
        public string discount_type { get; set; }
        public decimal? discount_percentage { get; set; }
        public decimal? discount_amount { get; set; }

        #endregion

        #region FetchPreliminaryReport
        public bool FetchPreliminaryReport(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
            SqlRecordParams[1] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserID;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "rpt_prelimreport_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Company";
                    ds.Tables[1].TableName = "Details";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchFinalReport
        public bool FetchFinalReport(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
            SqlRecordParams[1] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserID;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "rpt_finalreport_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Company";
                    ds.Tables[1].TableName = "Details";
                    ds.Tables[2].TableName = "Addendum";
                    ds.Tables[3].TableName = "Footer";
                    ds.Tables[4].TableName = "Signage";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchPromoReasonParameters
        public bool FetchPromoReasonParameters(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "rpt_params_promo_reason_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "promo_reasons";
                    ds.Tables[1].TableName = "institutions";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadPromoReportList
        public bool LoadPromoReportList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            SqlRecordParams[0] = new SqlParameter("@fromdate", SqlDbType.Date); SqlRecordParams[0].Value = FromDate.Value;
            SqlRecordParams[1] = new SqlParameter("@todate", SqlDbType.Date); SqlRecordParams[1].Value = ToDate.Value;
            SqlRecordParams[2] = new SqlParameter("@promo_reason_id", SqlDbType.NVarChar); SqlRecordParams[2].Value = PromoReasonId;
            SqlRecordParams[3] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = InstitutionId;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "rpt_promotion_details_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Report";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion
    }
}
