using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using VETRIS.Core.MyPayments;
using VETRIS.Core.TransNationalPaymentGateway;

namespace VETRIS.Invoicing
{
    [AjaxPro.AjaxNamespace("VRSOnlineRefundDlg")]
    public partial class VRSOnlineRefundDlg : System.Web.UI.Page
    {
        #region Members & Variables
        ARRefunds objCore = new ARRefunds();
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSOnlineRefundDlg));
            SetAttributes();
            if (!IsPostBack)
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
            ddlAccount.Attributes.Add("onchange", "javascript:ddlAccount_OnChange();");
            //ddlPmtMode.Attributes.Add("onchange", "javascript:ddlPmtMode_OnChange();");

            txtAmount.Attributes.Add("onfocus", "javascript:this.select();");
            txtAmount.Attributes.Add("onkeypress", "javascript:return parent.CheckDecimal(event);");
            txtAmount.Attributes.Add("onblur", "javascript:ResetValueDecimal(this);");
            txtAmount.Attributes.Add("onchange", "javascript:onChange_txtAmount(this);");


        }
        #endregion

        #region LoadDetails
        private void LoadDetails(int intMenuID, Guid UserID)
        {
            objCore = new ARRefunds();
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
                    //txtRefNo.Text = objCore.processing_ref_no;
                    txtOurRefNo.Text = objCore.refundref_no;
                    txtAmount.Text = objComm.IMNumeric(objCore.refund_amount, objComm.DecimalDigit);
                    txtRefDate.Text = objComm.IMDateFormat(objCore.processing_ref_date, objComm.DateFormat);
                    //ddlPmtMode.SelectedValue = objCore.refund_mode;
                    //ddlPmtMode.Enabled = objCore.id.ToString() == "00000000-0000-0000-0000-000000000000";
                    this.hdnIsAdjusted.Value = objCore.isadjusted ? "1" : "0";
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

        #region CallBackInvoiceBrw_Callback
        protected void CallBackInvoiceBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];
            switch (strAction)
            {
                case "L":
                    FetchOnlinePayments(e.Parameters);
                    break;
            }
            grdInvoiceBrw.Width = Unit.Percentage(100);
            grdInvoiceBrw.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region FetchOnlinePayments
        private void FetchOnlinePayments(string[] arr)
        {
            var objCore = new Core.MyPayments.ARRefunds();

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

                    grdInvoiceBrw.DataSource = ds.Tables["Payments"];
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

            objCore = new ARRefunds();
            objComm = new classes.CommonClass();

            try
            {

                objComm.SetRegionalFormat();
                objCore.id = new Guid(ArrRecord[0].Trim());
                objCore.billing_account_id = new Guid(ArrRecord[1].Trim());
                objCore.refundref_no = ArrRecord[2].Trim();
                objCore.refundref_date = Convert.ToDateTime(ArrRecord[3]);
                objCore.processing_ref_date = Convert.ToDateTime(ArrRecord[3]);
                objCore.processing_ref_no = ArrRecord[4].Trim();
                objCore.refund_amount = Convert.ToDecimal(ArrRecord[5]);
                objCore.refund_mode = ArrRecord[6].Trim();
                objCore.created_by = new Guid(ArrRecord[7].Trim());
                objCore.UserID = new Guid(ArrRecord[7].Trim());
                objCore.MenuID = Convert.ToInt32(ArrRecord[8]);
                objCore.date_created = DateTime.Now;
                objCore.ar_payments_id = new Guid(ArrRecord[9].Trim()); ;
                objCore.payment_reference_no = ArrRecord[10].Trim();
                objCore.processing_status = "0";
                objCore.vault_id = Guid.Empty;


                if (objCore.ar_payments_id != null)
                {
                    bReturn = objCore.LoadPaymentAdjustments(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);
                    if (bReturn)
                    {
                        if(ArrRecord[10].Trim()!=objCore.payment_reference_no) {
                             throw new Exception("Payment TRANSACTIONID incorrect!");
                        }
                        if (objCore.Adjustments.Count > 0)
                        {   
                            var total_amount=objCore.refund_amount;
                            var adjustments = new List<RefundAdjustmentRow>();
                            objCore.Adjustments.Where(i=>i.adj_amount>0).ToList().ForEach(adj => {
                                
                                if (total_amount > 0)
                                {
                                    if (total_amount >= adj.adj_amount)
                                    {
                                        adjustments.Add(adj);
                                        total_amount -= adj.adj_amount;
                                        adj.adj_amount = -adj.adj_amount;
                                    }
                                    else
                                    {
                                        adjustments.Add(adj);
                                        adj.adj_amount = -total_amount.Value;
                                        total_amount = 0;
                                    }
                                }
                                
                            });
                            if(total_amount>0){
                                throw new Exception("Cannot adjust invoices!");
                            }
                            objCore.Adjustments=adjustments;
                        }
                    }
                }
                if (bReturn)
                {
                    #region Post refund : transnational api
                    /*Gateway to use : transnational api*/
                    PaymentApi api = new PaymentApi();
                    api.production_url = VETRIS.Global.Transaction_Gateway_Url;
                    api.API_Key = VETRIS.Global.API_Key;
                    Dictionary<string, string> _response = new Dictionary<string, string>();
                    _response = api.OnlineDirectRefund(objCore.payment_reference_no, objCore.refund_amount.Value);
                    if (_response["response"] != "1")
                    {
                        #region Save Refund
                        objCore.processing_ref_no = _response["transactionid"];
                        objCore.processing_status = "0";
                        objCore.remarks = _response["responsetext"];

                        objCore.refundref_no = "generated";
                        objCore.refundref_date = DateTime.Now;
                        objCore.processing_ref_date = DateTime.Now;
                        objCore.processing_pg_name = "TRANSNATIONAL";
                        objCore.refund_mode = "1"; // ONLINE
                        objCore.processing_status = "0"; //Failed
                        objCore.created_by = objCore.UserID;
                        objCore.date_created = DateTime.Now;

                        // do not adjust for failed transaction
                        objCore.Adjustments.Clear();
                        var bLogReturn = objCore.SaveRecord(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                        bReturn = false;
                        strCatchMessage = "Payment Gateway: " + _response["responsetext"];
                        #endregion

                    }
                    else
                    {
                        objCore.remarks = _response["responsetext"];
                        objCore.processing_ref_no = _response["transactionid"];
                        objCore.processing_status = "1";
                        objCore.remarks = _response["responsetext"];
                        bReturn = true;

                    }
                    #endregion
                }
                if (bReturn)
                {

                    objCore.refundref_no = "generated";
                    objCore.refundref_date = DateTime.Now;
                    objCore.processing_ref_date = DateTime.Now;
                    objCore.processing_pg_name = "TRANSNATIONAL";
                    objCore.created_by = objCore.UserID;
                    objCore.date_created = DateTime.Now;

                    // do not adjust for failed transaction
                    if (objCore.refund_mode == "1" && objCore.processing_status == "0")
                    {
                        objCore.Adjustments.Clear();
                    }
                    bReturn = objCore.SaveRecord(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                }
                

                if (bReturn)
                {

                    arrRet = new string[5];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
                    arrRet[2] = objCore.id.ToString();
                    arrRet[3] = objCore.refundref_no.ToString();
                    arrRet[4] = objComm.IMDateFormat(objCore.refundref_no, objComm.DateFormat);
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

        


    }
}