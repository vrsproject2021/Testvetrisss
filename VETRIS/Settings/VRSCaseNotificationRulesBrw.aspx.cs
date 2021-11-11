using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using System.Configuration;
using VETRIS.Core;

namespace VETRIS.Settings
{
    public partial class VRSCaseNotificationRulesBrw : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Settings.CaseNotificationRules objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CallBackBrw.CausedCallback)
                SetPageValue();
            SetAttributes();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnAdd.Attributes.Add("onclick", "javascript:btnBrwAdd_Onclick('Settings/VRSCaseNotificationRulesDlg.aspx');");
            btnRefresh.Attributes.Add("onclick", "javascript:ResetRecord();view_Searchform();");
            btnSearch.Attributes.Add("onclick", "javascript:SearchRecord();view_Searchform();");
            btnClose.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"].ToString());
            string strID = string.Empty;
            string strTheme = Request.QueryString["th"];
            SetCSS(strTheme);
            FetchSearchParameters();

        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
        }
        #endregion

        #region FetchSearchParameters
        private void FetchSearchParameters()
        {
            objCore = new Core.Settings.CaseNotificationRules();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                bReturn = objCore.FetchBrowserParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    #region PACS Status
                    DataRow dr1 = ds.Tables["PACSStatus"].NewRow();
                    dr1["status_id"] = -999;
                    dr1["status_desc"] = "Select One";
                    ds.Tables["PACSStatus"].Rows.InsertAt(dr1, 0);
                    ds.Tables["PACSStatus"].AcceptChanges();

                    ddlStudyStatus.DataSource = ds.Tables["PACSStatus"];
                    ddlStudyStatus.DataValueField = "status_id";
                    ddlStudyStatus.DataTextField = "status_desc";
                    ddlStudyStatus.DataBind();
                    #endregion

                    #region Priority
                    DataRow dr2 = ds.Tables["Priority"].NewRow();
                    dr2["priority_id"] = 0;
                    dr2["priority_desc"] = "Select One";
                    ds.Tables["Priority"].Rows.InsertAt(dr2, 0);
                    ds.Tables["Priority"].AcceptChanges();

                    ddlPriority.DataSource = ds.Tables["Priority"];
                    ddlPriority.DataValueField = "priority_id";
                    ddlPriority.DataTextField = "priority_desc";
                    ddlPriority.DataBind();
                    #endregion
                }
                else
                    hdnError.Value = strCatchMessage.Trim();



            }
            catch (Exception ex)
            {
                hdnError.Value = ex.Message.Trim();
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }

        }
        #endregion

        #region CallBackBrw_Callback
        protected void CallBackBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            SearchRecord(e.Parameters);
            grdBrw.Width = Unit.Percentage(100);
            grdBrw.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region SearchRecord
        private void SearchRecord(string[] arrRecord)
        {
            objCore = new Core.Settings.CaseNotificationRules();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objCore.PACS_STATUS_ID = Convert.ToInt32(arrRecord[0]);
                objCore.PRIORITY_ID = Convert.ToInt32(arrRecord[1]);
                objCore.IS_ACTIVE = arrRecord[2].Trim();
                objCore.USER_ID = new Guid(arrRecord[3].Trim());

                bReturn = objCore.SearchBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    grdBrw.DataSource = ds.Tables["BrowserList"];
                    grdBrw.DataBind();
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";
                }
                else
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";

            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }

        }
        #endregion
    }
}