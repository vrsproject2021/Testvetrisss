using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VETRIS.Core.Dashboard;

namespace VETRIS.Dashboard.ModalityRevenue
{
    [AjaxPro.AjaxNamespace("VRSModalityRevenue")]
    public partial class VRSModalityRevenue : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Dashboard.Dashboard objCore = null;
        classes.CommonClass objComm;
        int dashboardId = 0;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSModalityRevenue));
            SetPageValue();
        }

        #region SetPageValue
        private void SetPageValue()
        {
            hdnRefreshTime.Value = Request.QueryString["sec"];
            hdnIsRefreshBtn.Value = Request.QueryString["isrefresh"];
            hdnDesc.Value = Request.QueryString["desc"];
            hdnreportTitle.Value = Request.QueryString["rt"];
            string strTheme = Request.QueryString["th"];
            hdnTheme.Value = strTheme;
            SetCSS(strTheme);
            GetModality();
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
        public void GetModality()
        {
            objCore = new Core.Dashboard.Dashboard();
            string strCatchMessage = "";
            bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                bReturn = objCore.FetchModality(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    #region Modality
                    DataRow dr1 = ds.Tables["Modality"].NewRow();
                    dr1["id"] = 0;
                    dr1["name"] = "All";
                    ds.Tables["Modality"].Rows.InsertAt(dr1, 0);
                    ds.Tables["Modality"].AcceptChanges();

                    ddlModality.DataSource = ds.Tables["Modality"];
                    ddlModality.DataValueField = "id";
                    ddlModality.DataTextField = "name";
                    ddlModality.DataBind();
                    #endregion
                }

            }
            catch (Exception ex)
            {

            }

        }

        #region SearchRecord
        [AjaxPro.AjaxMethod()]
        public dynamic SearchRecord(string[] arrRecord)
        {
            objCore = new Core.Dashboard.Dashboard();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            string strUserRole = string.Empty;
            objComm = new classes.CommonClass();
            try
            {
                objCore.MONTH_COUNT = Convert.ToInt32(arrRecord[0]);
                objCore.MODALITY_ID = Convert.ToInt32(arrRecord[1]);

                bReturn = objCore.SearchMonthlyRevenueData(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        #endregion
    }
}