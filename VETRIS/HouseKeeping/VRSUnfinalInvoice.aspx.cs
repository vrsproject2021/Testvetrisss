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

namespace VETRIS.HouseKeeping
{
    [AjaxPro.AjaxNamespace("VRSUnfinalInvoice")]
    public partial class VRSUnfinalInvoice : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Invoicing.InvoiceProcess objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSUnfinalInvoice));
            SetAttributes();
            if (!CallBackInvoice.CausedCallback) SetPageValue();
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            hdnID.Value = Request.QueryString["id"];
            FetchParameters();
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
            lnkTHEME.Attributes["href"] = strServerPath + "/css/" + strTheme + "/theme.css";
        }
        #endregion
        #region SetAttributes
        private void SetAttributes()
        {
           
            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnOk.Attributes.Add("onclick", "javascript:btnOk_OnClick();");
            btnUnfinal.Attributes.Add("onclick", "javascript:btnUnfinal_OnClick();");
            ddlBillingCycle.Attributes.Add("onchange", "javascript:ddlBillingCycle_OnChange();");
            ddlAccount.Attributes.Add("onchange", "javascript:ddlAccount_OnChange();");
        }
        #endregion

        #region FetchParameters
        private void FetchParameters()
        {
            objCore = new Core.Invoicing.InvoiceProcess();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                bReturn = objCore.FetchParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    #region Billing Cycles
                    DataRow dr1 = ds.Tables["Cycle"].NewRow();
                    dr1["id"] = "00000000-0000-0000-0000-000000000000";
                    dr1["name"] = "Select One";
                    ds.Tables["Cycle"].Rows.InsertAt(dr1, 0);
                    ds.Tables["Cycle"].AcceptChanges();

                    ddlBillingCycle.DataSource = ds.Tables["Cycle"];
                    ddlBillingCycle.DataValueField = "id";
                    ddlBillingCycle.DataTextField = "name";
                    ddlBillingCycle.DataBind();
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

        #region CallBackInvoice_Callback
        protected void CallBackInvoice_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadDetails(e.Parameters);
            grdInvoice.Width = Unit.Percentage(100);
            grdInvoice.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
            spnUser.RenderControl(e.Output);
        }
        #endregion

        #region LoadDetails
        private void LoadDetails(string[] arrRecord)
        {
            objCore = new Core.Invoicing.InvoiceProcess();
            bool bReturn = false; string strReturnMessage = ""; string strCatchMessage = "";
            string strOption = string.Empty;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();


            try
            {
                objComm.SetRegionalFormat();
                objCore.BILLING_CYCLE_ID = new Guid(arrRecord[0]);
                objCore.BILLING_ACCOUNT_ID = new Guid(arrRecord[1]);
                objCore.MENU_ID = Convert.ToInt32(arrRecord[2]);
                objCore.USER_ID = new Guid(arrRecord[3]);
                strOption = arrRecord[4].Trim();

                if (strOption == "P")
                    bReturn = objCore.LoadDetails(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);
                else if (strOption == "V")
                    bReturn = objCore.FetchFinalRecords(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);


                if (bReturn)
                {
                    ds.Relations.Add(ds.Tables[0].Columns["billing_account_id"], ds.Tables[1].Columns["billing_account_id"]);
                    ds.Relations.Add(ds.Tables[1].Columns["institution_id"], ds.Tables[2].Columns["institution_id"]);
                    grdInvoice.Levels[2].Columns[5].FormatString = objComm.DateFormat;
                    grdInvoice.DataSource = ds;
                    grdInvoice.DataBind();
                    grdInvoice.PageSize = ds.Tables["InvoiceHdr"].Rows.Count;
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

                }
                else
                {
                    if (strCatchMessage.Trim() != string.Empty)
                        spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";
                    else
                        spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strReturnMessage + "\" />";
                }

            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose(); objComm = null;

            }

            spnUser.InnerHtml = "<input type=\"hidden\" id=\"hdnUsr\" value=\"" + objCore.USER_NAME + "\" />";
        }
        #endregion      

        #region BulkUnfinal (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] BulkUnfinal(string[] ArrRecord, string[] ArrAcct)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Invoicing.InvoiceProcess();
            objComm = new classes.CommonClass();

            Core.Invoicing.BillingAccountList[] objAcct = new Core.Invoicing.BillingAccountList[0];


            try
            {
                objCore.BILLING_CYCLE_ID = new Guid(ArrRecord[0].Trim());
                objCore.USER_ID = new Guid(ArrRecord[1].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[2]);

                objAcct = new Core.Invoicing.BillingAccountList[(ArrAcct.Length / 2)];

                #region Populate Billing Account
                for (int i = 0; i < objAcct.Length; i++)
                {
                    objAcct[i] = new Core.Invoicing.BillingAccountList();
                    objAcct[i].ID = new Guid(ArrAcct[intListIndex]);
                    objAcct[i].TOTAL_AMOUNT = Convert.ToDouble(ArrAcct[intListIndex + 1]);
                    intListIndex = intListIndex + 2;
                }
                #endregion

                intListIndex = 0;

                bReturn = objCore.BulkUnfinal(Server.MapPath("~"), objAcct, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {

                    arrRet = new string[4];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
                }
                else
                {
                    if (strCatchMessage.Trim() != "")
                    {
                        arrRet = new string[2];
                        arrRet[0] = "catch";
                        arrRet[1] = strCatchMessage.Trim();
                    }
                    else
                    {
                        arrRet = new string[3];
                        arrRet[0] = "false";
                        arrRet[1] = strReturnMsg.Trim();
                        arrRet[2] = objCore.USER_NAME;
                    }
                }
            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = expErr.Message.Trim();
            }
            finally
            {
                objCore = null; objComm = null;
                strReturnMsg = null; strCatchMessage = null;
            }
            return arrRet;
        }
        #endregion

    }
}