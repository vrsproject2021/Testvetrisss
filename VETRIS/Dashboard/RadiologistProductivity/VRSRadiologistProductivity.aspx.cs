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

namespace VETRIS.Dashboard.RadiologistProductivity
{
    [AjaxPro.AjaxNamespace("VRSRadiologistProductivity")]
    public partial class VRSRadiologistProductivity : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Dashboard.Dashboard objCore = null;
        classes.CommonClass objComm;
        int dashboardId = 0;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSRadiologistProductivity));
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
            dashboardLightbox.Attributes["href"] = strServerPath + "/css/" + strTheme + "/lightbox.css";
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
            LineData data = null;
            try
            {
                objComm.SetRegionalFormat();
                objCore.FROM_DATE = Convert.ToDateTime(arrRecord[0]);
                objCore.TO_DATE = Convert.ToDateTime(arrRecord[1]);
                objCore.MODALITY_ID = Convert.ToInt32(arrRecord[2]);
                objCore.ORDER_BY =arrRecord[3];

                bReturn = objCore.SearchPeriodicRadiologistProductivity(Server.MapPath("~"), ref ds, ref strCatchMessage);

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
            finally
            {
                ds.Dispose();
                objCore = null;
            }
            return null;
        }
        #endregion
    }
}