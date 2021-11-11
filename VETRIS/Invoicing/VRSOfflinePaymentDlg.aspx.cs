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
    [AjaxPro.AjaxNamespace("VRSOfflinePaymentDlg")]
    public partial class VRSOfflinePaymentDlg : System.Web.UI.Page
    {
        #region Members & Variables
        ARPayments objCore = new ARPayments();
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSOfflinePaymentDlg));
            SetAttributes();
            if(!IsPostBack)
                SetPageValue();
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            objComm = new classes.CommonClass();
            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);

            if (Request.QueryString["aid"] != null) hdnAID.Value = Request.QueryString["aid"];

            hdnID.Value = Request.QueryString["id"];
            objComm.SetRegionalFormat();
            txtRefDate.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            hdnIsAdjusted.Value = "0";
            LoadDetails(intMenuID, UserID);
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
            lnkCAL.Attributes["href"] = strServerPath + "/css/" + strTheme + "/CalendarStyle.css";
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnAdd1.Attributes.Add("onclick", "javascript:btnNew_OnClick();");
            btnAdd2.Attributes.Add("onclick", "javascript:btnNew_OnClick();");
            btnSave1.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnSave2.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnAutoAdjust.Attributes.Add("onclick", "javascript:AutoAdjust();");
            btnClearSelection.Attributes.Add("onclick", "javascript:ClearSelectedRows(true);");
            ddlAccount.Attributes.Add("onchange", "javascript:ddlAccount_OnChange();");
            ddlPmtMode.Attributes.Add("onchange", "javascript:ddlPmtMode_OnChange();");

            txtAmount.Attributes.Add("onfocus", "javascript:this.select();");
            txtAmount.Attributes.Add("onkeypress", "javascript:return parent.CheckDecimal(event);");
            txtAmount.Attributes.Add("onblur", "javascript:ResetValueDecimal(this);");


        }
        #endregion

        #region LoadDetails
        private void LoadDetails(int intMenuID, Guid UserID)
        {
            objCore = new ARPayments();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.id = new Guid(hdnID.Value);
                objCore.MenuID = intMenuID;
                objCore.UserID = UserID;

                bReturn = objCore.LoadDetails(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {

                    PopulateDropdowns(ds);
                    ddlAccount.Enabled = objCore.id.ToString() == "00000000-0000-0000-0000-000000000000";
                    ddlAccount.SelectedValue = objCore.billing_account_id.ToString();
                    //btnAutoAdjust.Visible = objCore.id.ToString() == "00000000-0000-0000-0000-000000000000";
                    //btnClearSelection.Visible = objCore.id.ToString() == "00000000-0000-0000-0000-000000000000";
                    //btnSave1.Visible = objCore.payment_mode == "0" && !objCore.isadjusted;
                    //btnSave2.Visible = objCore.payment_mode == "0" && !objCore.isadjusted;
                    //txtAmount.ReadOnly = objCore.payment_mode == "1" || objCore.isadjusted;
                    //txtRefNo.ReadOnly = objCore.payment_mode == "1" || objCore.isadjusted;
                    txtRefNo.Text = objCore.processing_ref_no;
                    txtOurRefNo.Text = objCore.payref_no;
                    txtAmount.Text = objComm.IMNumeric(objCore.payment_amount,objComm.DecimalDigit);
                    txtRefDate.Text = objComm.IMDateFormat(objCore.processing_ref_date, objComm.DateFormat);
                    ddlPmtMode.SelectedValue = objCore.payment_mode;
                    //ddlPmtMode.Enabled = objCore.id.ToString() == "00000000-0000-0000-0000-000000000000";
                    this.hdnIsAdjusted.Value = objCore.isadjusted?"1":"0";
                }
                else
                {
                    if (strCatchMessage.Trim() != string.Empty)
                        hdnError.Value = "catch" + objComm.RecordDivider + strCatchMessage.Trim();
                    else
                        hdnError.Value = "false" + objComm.RecordDivider + strReturnMessage.Trim();
                }

            }
            catch (Exception ex)
            {
                hdnError.Value = "catch" + objComm.RecordDivider + ex.Message.Trim();
            }
            finally
            {
                ds.Dispose(); objCore = null; objComm = null;
            }
        }
        #endregion

        #region PopulateDropdowns
        private void PopulateDropdowns(DataSet ds)
        {

            #region Billing Accounts
            DataRow dr2 = ds.Tables["Accounts"].NewRow();
            dr2["id"] = "00000000-0000-0000-0000-000000000000";
            dr2["name"] = "Select One";
            ds.Tables["Accounts"].Rows.InsertAt(dr2, 0);
            ds.Tables["Accounts"].AcceptChanges();

            ddlAccount.DataSource = ds.Tables["Accounts"];
            ddlAccount.DataValueField = "id";
            ddlAccount.DataTextField = "name";
            ddlAccount.DataBind();
            #endregion

           
        }
        #endregion

        #region SaveRecord (Ajaxpro Method)
        /// <summary>
        /// Off line save
        /// </summary>
        /// <param name="ArrRecord"></param>
        /// <returns></returns>
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new ARPayments();
            objComm = new classes.CommonClass();

            try
            {

                objComm.SetRegionalFormat();
                objCore.id = new Guid(ArrRecord[0].Trim());
                objCore.billing_account_id = new Guid(ArrRecord[1].Trim());
                objCore.payref_no = ArrRecord[2].Trim();
                objCore.payref_date = Convert.ToDateTime(ArrRecord[3]);
                objCore.processing_ref_date =  Convert.ToDateTime(ArrRecord[3]);
                objCore.processing_ref_no = ArrRecord[4].Trim();
                objCore.payment_amount = Convert.ToDecimal(ArrRecord[5]);
                objCore.payment_mode = ArrRecord[6].Trim();
                objCore.created_by = new Guid(ArrRecord[7].Trim());
                objCore.UserID = new Guid(ArrRecord[7].Trim());
                objCore.MenuID = Convert.ToInt32(ArrRecord[8]);
                objCore.date_created = DateTime.Now;
                objCore.payment_tool = string.Empty;
                objCore.name = string.Empty;
                objCore.auth_code = string.Empty;
                objCore.cvv_response = string.Empty;
                objCore.avs_response = string.Empty;
                objCore.processing_status = "1";
                objCore.vault_id = Guid.Empty;

                var invoices = Newtonsoft.Json.JsonConvert.DeserializeObject<List<dynamic>>(ArrRecord[9]);
                if (invoices.Count > 0)
                {
                    invoices.ForEach(adj =>
                    {
                        var balance = Convert.ToDecimal(adj.balance.Value.ToString());
                        var adjamt = Convert.ToDecimal(adj.adj_amount.Value.ToString());
                        if (balance < adjamt)
                        {
                            throw new Exception(string.Format("Adjusted amount {0:N2} cannot be greater than Balance {1:N2}.", adjamt, balance));
                        }

                        var adjustment = new PaymentAdjustmentRow
                        {
                            invoice_header_id = new Guid(Convert.ToString(adj["invoice_header_id"])),
                            invoice_no = Convert.ToString(adj["invoice_no"]),
                            invoice_date = Convert.ToDateTime(Convert.ToString(adj["invoice_date"])),
                            adj_amount = Convert.ToDecimal(adjamt)
                            //invoice_header_id = new Guid(adj.invoice_header_id.Value),
                            //adj_amount = adjamt
                        };
                        objCore.Adjustments.Add(adjustment);
                    });
                }
                else
                {
                    bReturn = false;
                    strReturnMsg = "480";
                    throw new Exception(strReturnMsg);
                }

                #region check adjustments
                bReturn = objCore.ValidatePayment(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);
                if (!bReturn)
                {
                    throw new Exception(strReturnMsg);
                }
                #endregion

                bReturn = objCore.SaveRecord(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {

                    arrRet = new string[5];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
                    arrRet[2] = objCore.id.ToString();
                    arrRet[3] = objCore.payref_no.ToString();
                    arrRet[4] = objComm.IMDateFormat(objCore.payref_date,objComm.DateFormat);
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
                        arrRet[2] = objCore.UserName;
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

        #region CallBackInvoiceBrw_Callback
        protected void CallBackInvoiceBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];
            switch (strAction)
            {
                case "L":
                    FetchInvoices(e.Parameters);
                    break;

            }
            grdInvoiceBrw.Width = Unit.Percentage(100);
            grdInvoiceBrw.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
            
        }
        #endregion

        #region FetchInvoices
        private void FetchInvoices(string[] arr)
        {
            var objCore = new Core.MyPayments.ARPayments();

            string strCatchMessage = "";
            string strReturnMessage = string.Empty;

            bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.UserID = new Guid(arr[1]);
                objCore.MenuID = Convert.ToInt32(arr[2]);
                if (!string.IsNullOrEmpty(arr[3]))
                    objCore.id = new Guid(arr[3]);
                if (!string.IsNullOrEmpty(arr[4]))
                    objCore.billing_account_id = new Guid(arr[4]);

                bReturn = objCore.LoadDetails(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {

                    grdInvoiceBrw.DataSource = ds.Tables["Invoices"];
                    grdInvoiceBrw.Levels[0].Columns[2].FormatString = objComm.DateFormat;
                    grdInvoiceBrw.DataBind();
                    hdnIsAdjusted.Value = objCore.isadjusted ? "1" : "0";
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
        
        
    }
}