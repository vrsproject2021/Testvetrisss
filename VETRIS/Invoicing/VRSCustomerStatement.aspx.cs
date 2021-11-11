using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using VETRIS.Core.MyPayments;

namespace VETRIS.Invoicing
{
    [AjaxPro.AjaxNamespace("VRSCustomerStatement")]
    public partial class VRSCustomerStatement : System.Web.UI.Page
    {
        #region Members & Variables
        ARPayments objCore = null;
        classes.CommonClass objComm;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSCustomerStatement));
            SetAttributes(); 
            if (!CallBackBrw.CausedCallback) SetPageValue();
        }
        #region SetPageValue
        private void SetPageValue()
        {
            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"].ToString());
            objComm = new classes.CommonClass();
            FetchParameters(intMenuID);
            string strTheme = Request.QueryString["th"];
            SetCSS(strTheme);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkTHEME.Attributes["href"] = strServerPath + "/css/" + strTheme + "/theme.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
           
        }
        #endregion
        #region SetAttributes
        private void SetAttributes()
        {

            ddlAccount.Attributes.Add("onchange", "javascript:ddlAccount_OnChange();");
            btnClose.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");
            
        }
        #endregion

        #region FetchParameters
        private void FetchParameters(int intMenuId)
        {
            objCore = new ARPayments();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                bReturn = objCore.FetchParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

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
            objCore = new ARPayments();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.billing_account_id = new Guid(arrRecord[0].Trim());
                objCore.UserID = new Guid(arrRecord[3].Trim());
                objCore.MenuID = Convert.ToInt32(arrRecord[4].Trim());

                bReturn = objCore.SearchCustomerStatementBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    ds.Relations.Add(ds.Tables[0].Columns["billing_account_id"], 
                                     ds.Tables[1].Columns["billing_account_id"]);
                    ds.Relations.Add(ds.Tables[1].Columns["invoice_id"],
                                     ds.Tables[2].Columns["invoice_id"]);

                    grdBrw.Levels[1].Columns[2].FormatString = objComm.DateFormat;
                    grdBrw.Levels[2].Columns[2].FormatString = objComm.DateFormat;
                    grdBrw.PageSize = ds.Tables[0].Rows.Count;
                    grdBrw.DataSource = ds;
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