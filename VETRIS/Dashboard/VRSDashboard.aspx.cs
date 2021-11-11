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

namespace VETRIS.Dashboard
{
    [AjaxPro.AjaxNamespace("VRSDashboard")]
    public partial class VRSDashboard : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Dashboard.Dashboard objCore = null;
        classes.CommonClass objComm;
        int dashboardId = 0;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSDashboard));
            SetPageValue();
        }
        #region SetPageValue
        private void SetPageValue()
        {
            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            hdnUserID.Value = Request.QueryString["uid"];
            dashboardId = Convert.ToInt32(Request.QueryString["dashId"]);
            string strTheme = Request.QueryString["th"];
            hdnTheme.Value = strTheme;
            SetCSS(strTheme);
            GetDashboardMenuList(dashboardId);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            dashboardSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/dashboard.css";
            if (strTheme == "")
            {
                vetrisLOGO.Src = strServerPath + "/images/logo/logo_DEFAULT.png";
            }
            else
            {
                vetrisLOGO.Src = strServerPath + "/images/logo/logo_" + strTheme + ".png";
            }
        }
        #endregion

        public void GetDashboardMenuList(int id)
        {
            objCore = new Core.Dashboard.Dashboard();
            DashboardSettings dashboard_setting = null; ;
            string strCatchMessage = "";
            bool bReturn = false;
            DataSet ds = new DataSet();
            List<DashboardData> list = new List<DashboardData>();
            StringBuilder sb = new StringBuilder();
            try
            {
                objCore.USER_ID = new Guid(hdnUserID.Value.ToString());
                objCore.DASHMENU_ID = dashboardId;
                bReturn = objCore.LoadDashboardMenu(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow r in ds.Tables[0].Rows)
                        {
                            dashboard_setting = new DashboardSettings();
                            dashboard_setting.id =  Convert.ToInt32(r["id"].ToString());
                            dashboard_setting.menu_desc = r["menu_desc"].ToString();
                            dashboard_setting.nav_url = r["nav_url"].ToString();
                            dashboard_setting.icon = r["icon"].ToString();
                            dashboard_setting.display_index = Convert.ToInt32(r["display_index"].ToString());
                            dashboard_setting.is_default = r["is_default"].ToString();
                            dashboard_setting.refresh_time = Convert.ToInt32(r["refresh_time"].ToString());
                            dashboard_setting.is_refresh_button = r["is_refresh_button"].ToString();
                            dashboard_setting.title = r["title"].ToString();
                            sb.Append(SetMenuList(dashboard_setting));
                        }
                        dashboardMenu.InnerHtml = sb.ToString();
                        foreach (DataRow r in ds.Tables[1].Rows)
                        {
                            hdnDefaultDesc.Value = r["menu_desc"].ToString();
                            hdnDefaultUrl.Value = r["nav_url"].ToString();
                            hdnMenuId.Value = r["id"].ToString();
                            hdnRefreshTime.Value = r["refresh_time"].ToString();
                            hdnIsRefreshBtn.Value = r["is_refresh_button"].ToString();
                            hdnreportTitle.Value = r["title"].ToString();

                        }
                    }
                }
                else
                {
                    
                }
               
            }
            catch (Exception ex)
            {

            }
            
        }

        private string SetMenuList(DashboardSettings setting)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("<li>");
                sb.Append("<div class='menu_icon' style='display:flex;'>");
                if(string.IsNullOrEmpty(setting.icon))
                    sb.Append("<span class=\"fa fa-chevron-right\" style=\"padding:9px 0px 0px 4px;\"></span>");
                else
                    sb.Append("<img src='../images/menu/" + setting.icon + "' alt='' title='" + setting.menu_desc + "' class='img-responsive dash-img-icon'/>");
                sb.Append("<a href='#' id='aId_" + setting.id +
                    "' onclick=\"javascript:loadDashboardPage('" + setting.menu_desc + "','" + setting.nav_url + "','" + setting.id + "','" + setting.refresh_time + "','" + setting.is_refresh_button + "','" + setting.title + "','" + hdnTheme.Value + "')" +
                    "\">" +
                    setting.menu_desc + "</a></div>");
                sb.Append("</li>");
                return sb.ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}