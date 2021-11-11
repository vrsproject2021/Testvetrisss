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
    [AjaxPro.AjaxNamespace("VRSRefunds")]
    public partial class VRSRefunds : System.Web.UI.Page
    {
        #region Members & Variables
        ARRefunds objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSRefunds));
            if (!CallBackBrw.CausedCallback)
            {
                SetPageValue();
            }

            SetAttributes();
        }  
        #endregion


        #region SetPageValue
        private void SetPageValue()
        {
            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"].ToString());
            string strID = string.Empty;
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
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
          
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {

            btnAdd.Attributes.Add("onclick", "javascript:btnBrwAddUI_Onclick('Invoicing/VRSOnlineRefundDlg.aspx');");
            btnRefresh.Attributes.Add("onclick", "javascript:ResetRecord();view_Searchform();");
            btnSearch.Attributes.Add("onclick", "javascript:SearchRecord();view_Searchform();");
            btnClose.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");
            
        }
        #endregion

        #region FetchParameters
        private void FetchParameters(int intMenuId)
        {
            objCore = new ARRefunds();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
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
            objCore = new ARRefunds();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.billing_account_id = new Guid(arrRecord[0].Trim());
                objCore.refund_mode = arrRecord[1].Trim();
                objCore.processing_status = arrRecord[2].Trim();
                //objCore.created_by = new Guid(arrRecord[2].Trim());
                objCore.UserID = new Guid(arrRecord[3].Trim());
                objCore.MenuID = Convert.ToInt32(arrRecord[4].Trim());

                bReturn = objCore.SearchBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    grdBrw.DataSource = ds.Tables["BrowserList"];
                    grdBrw.Levels[0].Columns[2].FormatString = objComm.DateFormat;
                    grdBrw.Levels[0].Columns[8].FormatString = objComm.DateFormat;
                    grdBrw.Levels[0].Columns[11].FormatString = objComm.DateFormat + " HH:mm";
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