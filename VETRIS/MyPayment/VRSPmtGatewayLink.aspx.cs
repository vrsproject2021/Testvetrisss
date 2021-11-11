using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VETRIS.Core.MyPayments;
using VETRIS.Core.TransNationalPaymentGateway;
using AjaxPro;

namespace VETRIS.MyPayment
{
    [AjaxPro.AjaxNamespace("VRSPmtGatewayLink")]
    public partial class VRSPmtGatewayLink : System.Web.UI.Page
    {
        #region Members & Variables
        ARPayments objCore = new ARPayments();
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSPmtGatewayLink));
            SetPageValue();
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            hdnID.Value = Request.QueryString["aid"];
            SetCSS(Request.QueryString["th"]);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkPMT.Attributes["href"] = strServerPath + "/css/" + strTheme + "/payment.css";
        }
        #endregion

        #region FetchInvoices (AjaxProMethod)
        [AjaxPro.AjaxMethod()]
        public string[] FetchInvoices(string strAcctID,string strUserID)
        {
            ARPayments objCore = new Core.MyPayments.ARPayments();
            string strCatchMessage = string.Empty;

            string[] arrRet = new string[0];
            bool bReturn = false;
            DataSet ds = new DataSet();
            double dblAmt = 0;
            int intIndex = 0;

            objComm = new classes.CommonClass();
            

            try
            {
                objCore.billing_account_id = new Guid(strAcctID);
                objCore.UserID = new Guid(strUserID);
                objComm.SetRegionalFormat();
                bReturn = objCore.LoadMyInvoiceOutstandingDetails(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    arrRet = new string[(ds.Tables["Invoices"].Rows.Count * 4) + 2];
                    arrRet[0] = "true";
                    intIndex = 1;

                    foreach (DataRow dr in ds.Tables["Invoices"].Rows)
                    {
                        arrRet[intIndex] = Convert.ToString(dr["id"]);
                        arrRet[intIndex + 1] = Convert.ToString(dr["invoice_no"]);
                        arrRet[intIndex + 2] = objComm.IMDBDateFormat(dr["invoice_date"]);
                        arrRet[intIndex + 3] = objComm.IMNumeric(dr["balance"], objComm.DecimalDigit);
                        dblAmt = dblAmt + Convert.ToDouble(dr["balance"]);
                        intIndex = intIndex + 4;
                    }
                    arrRet[intIndex] = objComm.IMNumeric(dblAmt, objComm.DecimalDigit).Replace(",","");
                }
                else
                {
                    arrRet = new string[2];

                    arrRet[0] = "catch";
                    arrRet[1] = strCatchMessage;


                }

            }
            catch (Exception ex)
            {
                arrRet = new string[2];
                arrRet[0] = "false";
                arrRet[1] = ex.Message.Trim();

            }
            finally
            {
                ds.Dispose();
                objCore = null;
                objComm = null;
            }
            return arrRet;
        }


        #endregion
    }
}