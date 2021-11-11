using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VETRIS.Core.Dashboard;
using VETRIS.Core.Settings;

namespace VETRIS.Dashboard.Cases
{
    [AjaxPro.AjaxNamespace("VRSCases")]
    public partial class VRSCases : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Dashboard.Dashboard objCore = null;
        classes.CommonClass objComm;
        int dashboardId = 0;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSCases));
            SetPageValue();
        }
        #region SetPageValue
        private void SetPageValue()
        {
            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            hdnUserID.Value = Request.QueryString["uid"];
            hdnDesc.Value = Request.QueryString["desc"];
            hdnMenuId.Value = Request.QueryString["mnuid"];
            hdnRefreshTime.Value = Request.QueryString["sec"];
            hdnIsRefreshBtn.Value = Request.QueryString["isrefresh"];
            hdnreportTitle.Value = Request.QueryString["rt"];
            string strTheme = Request.QueryString["th"];
            hdnTheme.Value = strTheme;
            SetCSS(strTheme);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            dashboardSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/dashboard.css";
        }
        #endregion

        [AjaxPro.AjaxMethod()]
        public DashboardOpenCaseList GetDashboardOpenCase(int id)
        {
            objCore = new Core.Dashboard.Dashboard();
            string strCatchMessage = "";
            bool bReturn = false;
            DataSet ds = new DataSet();
            DashboardOpenCaseList data = new DashboardOpenCaseList();
            List<DashboardData> list = new List<DashboardData>();
            try
            {
                objCore.MENU_ID = id;
                bReturn = objCore.FetchDashboardOpenCase(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow r in ds.Tables["modality_open_case_normal"].Rows)
                        {
                            var d = new DashboardData();
                            d.name = r["code"].ToString();
                            d.value = Convert.ToInt32(r["modality_count"].ToString());
                            list.Add(d);
                        }
                        data.modality_open_case_normal = list;

                        list = new List<DashboardData>();
                        foreach (DataRow r in ds.Tables["status_open_case_normal"].Rows)
                        {
                            var d = new DashboardData();
                            d.name = r["study_status_pacs"].ToString();
                            d.value = Convert.ToInt32(r["count_status"].ToString());
                            list.Add(d);
                        }
                        data.modality_status_normal = list;

                        list = new List<DashboardData>();
                        foreach (DataRow r in ds.Tables["elapsed_time_open_case_normal"].Rows)
                        {
                            var d = new DashboardData();
                            d.name = r["diff_in_minute"].ToString();
                            d.value = Convert.ToInt32(r["time_count"].ToString());
                            list.Add(d);
                        }
                        data.elapsed_time_normal = list;

                        list = new List<DashboardData>();
                        foreach (DataRow r in ds.Tables["modality_open_case_stat"].Rows)
                        {
                            var d = new DashboardData();
                            d.name = r["code"].ToString();
                            d.value = Convert.ToInt32(r["modality_count"].ToString());
                            list.Add(d);
                        }
                        data.modality_open_case_stat = list;

                        list = new List<DashboardData>();
                        foreach (DataRow r in ds.Tables["status_open_case_stat"].Rows)
                        {
                            var d = new DashboardData();
                            d.name = r["study_status_pacs"].ToString();
                            d.value = Convert.ToInt32(r["count_status"].ToString());
                            list.Add(d);
                        }
                        data.modality_status_stat = list;

                        list = new List<DashboardData>();
                        foreach (DataRow r in ds.Tables["elapsed_time_open_case_stat"].Rows)
                        {
                            var d = new DashboardData();
                            d.name = r["diff_in_minute"].ToString();
                            d.value = Convert.ToInt32(r["time_count"].ToString());
                            list.Add(d);
                        }
                        data.elapsed_time_stat = list;
                    }
                }
                
                return data;
            }
            catch (Exception ex)
            {

            }
            return data;
        }
    }
}