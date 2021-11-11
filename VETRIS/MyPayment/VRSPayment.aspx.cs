using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using VETRIS.Core.MyPayments;
using VETRIS.Core.TransNationalPaymentGateway;

namespace VETRIS.MyPayment
{
    [AjaxPro.AjaxNamespace("VRSPayment")]
    public partial class VRSPayment : System.Web.UI.Page
    {
        #region Members & Variables
        ARPayments objCore = new ARPayments();
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSPayment));
            SetAttributes();
            if (!CallBackInvoiceBrw.CausedCallback && !CallBackPaymentBrw.CausedCallback && !CallBackAllInvoiceBrw.CausedCallback && !CallBackAdjBrw.CausedCallback)
                SetPageValue();
        } 
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            hdnID.Value = Request.QueryString["id"];
            objCore.UserID = UserID;
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            this.outDiv.Visible = this.rdoOutstanding.Checked;
            this.pmtDiv.Visible = this.rdoOutstanding.Checked;
            this.btnClearSelection.Visible = this.rdoOutstanding.Checked;
            this.invAll.Visible = !this.rdoOutstanding.Checked;
            this.adjDiv.Visible = !this.rdoOutstanding.Checked;
            txtSelectedAmount.Text = objComm.IMNumeric(0,objComm.DecimalDigit);
            objComm = null;
            FetchParameters(UserID);
            string strTheme = Request.QueryString["th"];
            SetCSS(strTheme);
        }

        private void FetchParameters(Guid UserID)
        {
            ARPayments objCore = new Core.MyPayments.ARPayments();
            string strCatchMessage = "";
            string strReturnMessage = string.Empty;


            bool bReturn = false;
            DataSet ds = new DataSet();

            objComm = new classes.CommonClass();
            objCore.UserID = UserID;
            try
            {
                objComm.SetRegionalFormat();
                bReturn = objCore.FetchParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    if (ds.Tables["Account"].Rows.Count>0)
                    {
                        objCore.billing_account_id = new Guid( ds.Tables["Account"].Rows[0]["id"].ToString());
                        this.hdnID.Value = objCore.billing_account_id.ToString();
                    }
                    
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";
                }
                else
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage.Trim() + "\" />";
            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
                objComm = null;
            }

        }

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

            btnPay.Attributes.Add("onclick", "javascript:btnPay_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClearSelection.Attributes.Add("onclick", "javascript:btnClearSelection_OnClick();");
            txtSelectedAmount.Attributes.Add("onfocus", "this.select();");
            txtSelectedAmount.Attributes.Add("onkeypress", "javascript:return parent.CheckDecimal(event);");
            txtSelectedAmount.Attributes.Add("onblur", "javascript:ResetValueDecimal(this);");
        }
        #endregion

        #region Outstanding Invoice Grid

        #region CallBackInvoiceBrw_Callback
        protected void CallBackInvoiceBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            FetchInvoices(e.Parameters);
            grdInvoiceBrw.Width = Unit.Percentage(100);
            grdInvoiceBrw.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region FetchInvoices
        private void FetchInvoices(string[] arr)
        {
            ARPayments objCore = new Core.MyPayments.ARPayments();
            string strCatchMessage = "";
            

            bool bReturn = false;
            DataSet ds = new DataSet();

            objComm = new classes.CommonClass();
           

            try
            {
                objComm.SetRegionalFormat();
                objCore.billing_account_id = new Guid(arr[0]);
                objCore.UserID = new Guid(arr[1]);
                bReturn = objCore.LoadMyInvoiceOutstandingDetails(Server.MapPath("~"), ref ds, ref strCatchMessage);
                //var customerInfo = GetCustomerInfo(objCore); 
                if (bReturn)
                {

                    grdInvoiceBrw.DataSource = ds.Tables["Invoices"];
                    grdInvoiceBrw.Levels[0].Columns[2].FormatString = objComm.DateFormat;
                    grdInvoiceBrw.DataBind();
                    //this.hdnInfo.Value = customerInfo;
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";
                }
                else
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage.Trim() + "\" />";
            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
                objComm = null;
            }

        }

       
        private static string GetCustomerInfo(ARPayments d)
        {
            var info = "";
            var addr = d.address_1 ?? "";
            if (!string.IsNullOrEmpty(d.address_1))
            {
                addr += string.IsNullOrEmpty(addr) ? "" : ", " + d.address_1;
            }
            var data = new
            {
                name = d.name,
                address = addr,
                city = d.city,
                state = d.state,
                country = d.country,
                contact_no = d.contact_no,
                email = d.email_id
            };
            info = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            return info;
        }
        #endregion

        #endregion

        #region Previous Payment Grid

        #region CallBackPaymentBrw_Callback
        protected void CallBackPaymentBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            FetchPayments(e.Parameters);
            grdPaymentBrw.Width = Unit.Percentage(100);
            grdPaymentBrw.RenderControl(e.Output);
            spnERR1.RenderControl(e.Output);
        }
        #endregion

        #region FetchPayments
        private void FetchPayments(string[] arr)
        {
            var objCore = new Core.MyPayments.ARPayments();
            string strCatchMessage = "";
            string strReturnMessage = string.Empty;

            bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();
            objCore.billing_account_id = new Guid(arr[0]);
            objCore.UserID = new Guid(arr[1]);

            try
            {
                bReturn = objCore.LoadMyPaymentDetails(Server.MapPath("~"), ref ds,  ref strCatchMessage);

                if (bReturn)
                {

                    grdPaymentBrw.DataSource = ds.Tables["Payments"];
                    grdPaymentBrw.Levels[0].Columns[2].FormatString = objComm.DateFormat;
                    grdPaymentBrw.DataBind();
                    spnERR1.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPmt\" value=\"\" />";
                }
                else
                    spnERR1.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPmt\" value=\"" + strCatchMessage + "\" />";
            }
            catch (Exception ex)
            {
                spnERR1.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPmt\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }

        } 
        #endregion

        #endregion

        #region All Invoice Grid

        #region CallBackAllInvoiceBrw_Callback
        protected void CallBackAllInvoiceBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
          
            FetchAllInvoices(e.Parameters);
            grdAllInvoiceBrw.Width = Unit.Percentage(100);
            grdAllInvoiceBrw.RenderControl(e.Output);
            spnErrInvAll.RenderControl(e.Output);
        } 
        #endregion

        #region FetchAllInvoices
        private void FetchAllInvoices(string[] arr)
        {
            var objCore = new Core.MyPayments.ARPayments();
            string strCatchMessage = "";

            bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();
            

            try
            {
                objComm.SetRegionalFormat();
                objCore.billing_account_id = new Guid(arr[0]);
                objCore.UserID = new Guid(arr[1]);

                bReturn = objCore.LoadMyAllInvoices(Server.MapPath("~"), ref ds,  ref strCatchMessage);

                if (bReturn)
                {

                    grdAllInvoiceBrw.DataSource = ds.Tables["Invoices"];
                    grdAllInvoiceBrw.DataBind();
                    grdAllInvoiceBrw.Levels[0].Columns[2].FormatString = objComm.DateFormat;
                    spnErrInvAll.InnerHtml = "<input type=\"hidden\" id=\"hdnCBInvAllErr\" value=\"\" />";
                }
                else
                    spnErrInvAll.InnerHtml = "<input type=\"hidden\" id=\"hdnCBInvAllErr\" value=\"" + strCatchMessage.Trim() +"\" />";
            }
            catch (Exception ex)
            {
                spnErrInvAll.InnerHtml = "<input type=\"hidden\" id=\"hdnCBInvAllErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
                objComm = null;
            }

        } 
        #endregion

        #endregion

        #region Invoice Adjustment Grid

        #region CallBackAdjBrw_Callback
        protected void CallBackAdjBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
          
            FetchInvoiceAdjustments(e.Parameters);
            grdAdjBrw.Width = Unit.Percentage(100);
            grdAdjBrw.RenderControl(e.Output);
            spnErrAdj.RenderControl(e.Output);
        }
        #endregion

        #region FetchInvoiceAdjustments
        private void FetchInvoiceAdjustments(string[] arr)
        {
            var objCore = new Core.MyPayments.ARPayments();
            string strCatchMessage = "";

            bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();
            

            try
            {
                objComm.SetRegionalFormat();
                objCore.UserID = new Guid(arr[1]);
                objCore.MenuID = Convert.ToInt32(arr[2]);
                if (!string.IsNullOrEmpty(arr[3])) objCore.InvoiceId = new Guid(arr[3]);
                bReturn = objCore.LoadAdjustmentDetails(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    grdAdjBrw.DataSource = ds.Tables["Adjustments"];
                    grdAdjBrw.DataBind();
                    grdAdjBrw.Levels[0].Columns[2].FormatString = objComm.DateFormat;
                    spnErrAdj.InnerHtml = "<input type=\"hidden\" id=\"hdnCBAdjErr\" value=\"\" />";
                }
                else
                    spnErrAdj.InnerHtml = "<input type=\"hidden\" id=\"hdnCBAdjErr\" value=\"" + strCatchMessage.Trim() + "\" />";
            }
            catch (Exception ex)
            {
                spnErrAdj.InnerHtml = "<input type=\"hidden\" id=\"hdnCBAdjErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
                objComm = null;
            }

        } 
        #endregion

        #endregion


        [AjaxPro.AjaxMethod()]
        public void SelectedRows(SelectedRowData[] arr)
        {
            decimal total_amount = 0;
            foreach (SelectedRowData item in arr)
            {
                var amount = Convert.ToDecimal(item.amount.Replace(",", ""));
                total_amount += amount;
            }
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            //this.txtSelectedAmount.Text = string.Format("{0:N2}", total_amount); 
        }

        protected void rdoOutstanding_CheckedChanged(object sender, EventArgs e)
        {
            this.outDiv.Visible = this.rdoOutstanding.Checked;
            this.pmtDiv.Visible = this.rdoOutstanding.Checked;
            this.btnClearSelection.Visible = this.rdoOutstanding.Checked;
            this.invAll.Visible = !this.rdoOutstanding.Checked;
            this.adjDiv.Visible = !this.rdoOutstanding.Checked;

        }

        protected void rdoAll_CheckedChanged(object sender, EventArgs e)
        {
            this.outDiv.Visible = !this.rdoAll.Checked;
            this.pmtDiv.Visible = !this.rdoAll.Checked;
            this.btnClearSelection.Visible = !this.rdoAll.Checked;
            this.invAll.Visible = this.rdoAll.Checked;
            this.adjDiv.Visible = this.rdoAll.Checked;
        }

       
    }

    public class OnlinePaymentInfo
    {
        public string accountId { get; set; }
        public string cardNo { get; set; }
        public string expiryDate { get; set; }
        public string ccv { get; set; }
        public string amount { get; set; }
    }

    public class SelectedRowData
    {
        public string id { get; set; }
        public string amount { get; set; }
    }
}