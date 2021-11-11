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
using AjaxPro;

namespace VETRIS.Invoicing
{
     [AjaxPro.AjaxNamespace("VRSPromotionBrw")]
    public partial class VRSPromotionBrw : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Invoicing.Promotion objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSPromotionBrw));
            if (!CallBackBrw.CausedCallback)
                SetPageValue();
            SetAttributes();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnAdd.Attributes.Add("onclick", "javascript:btnBrwAddUI_Onclick('Invoicing/VRSPromotionDlg.aspx');");
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
            FetchParameters(intMenuID);
            SetCSS(Request.QueryString["th"]);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
            lnkCAL.Attributes["href"] = strServerPath + "/css/" + strTheme + "/CalendarStyle.css";
        }
        #endregion

        #region FetchParameters
        private void FetchParameters(int intMenuId)
        {
            objCore = new Core.Invoicing.Promotion();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                objCore.MENU_ID = intMenuId;
                bReturn = objCore.FetchParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    #region Users
                    DataRow dr1 = ds.Tables["User"].NewRow();
                    dr1["id"] = "00000000-0000-0000-0000-000000000000";
                    dr1["name"] = "Select One";
                    ds.Tables["User"].Rows.InsertAt(dr1, 0);
                    ds.Tables["User"].AcceptChanges();

                    ddlUser.DataSource = ds.Tables["User"];
                    ddlUser.DataValueField = "id";
                    ddlUser.DataTextField = "name";
                    ddlUser.DataBind();
                    ddlUser.SelectedIndex = 0;
                    #endregion

                    #region Billing Accounts
                    DataRow dr2 = ds.Tables["Account"].NewRow();
                    dr2["id"] = "00000000-0000-0000-0000-000000000000";
                    dr2["name"] = "Select One";
                    ds.Tables["Account"].Rows.InsertAt(dr2, 0);
                    ds.Tables["Account"].AcceptChanges();

                    ddlAccount.DataSource = ds.Tables["Account"];
                    ddlAccount.DataValueField = "id";
                    ddlAccount.DataTextField = "name";
                    ddlAccount.DataBind();
                    ddlAccount.SelectedIndex = 0;
                    #endregion

                    #region Promo Reasons
                    DataRow dr3 = ds.Tables["Reasons"].NewRow();
                    dr3["id"] = "00000000-0000-0000-0000-000000000000";
                    dr3["reason"] = "Select One";
                    ds.Tables["Reasons"].Rows.InsertAt(dr3, 0);
                    ds.Tables["Reasons"].AcceptChanges();

                    ddlReason.DataSource = ds.Tables["Reasons"];
                    ddlReason.DataValueField = "id";
                    ddlReason.DataTextField = "reason";
                    ddlReason.DataBind();
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
            objCore = new Core.Invoicing.Promotion();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.BILLING_ACCOUNT_ID  = new Guid(arrRecord[0].Trim());
                objCore.TYPE                = arrRecord[1].Trim();
                objCore.CREATED_BY          = new Guid(arrRecord[2].Trim());
                objCore.STATUS              = arrRecord[3].Trim();
                objCore.REASON_ID           = new Guid(arrRecord[4].Trim());
                objCore.USER_ID             = new Guid(arrRecord[5].Trim());
                objCore.MENU_ID             = Convert.ToInt32(arrRecord[6].Trim());

                bReturn = objCore.SearchBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    grdBrw.DataSource = ds.Tables["BrowserList"];
                    grdBrw.Levels[0].Columns[2].FormatString = objComm.DateFormat;
                    grdBrw.Levels[0].Columns[8].FormatString = objComm.DateFormat;
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