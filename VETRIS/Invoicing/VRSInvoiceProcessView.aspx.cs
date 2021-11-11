using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using VETRIS.Core;
using AjaxPro;

namespace VETRIS.Invoicing
{
    [AjaxPro.AjaxNamespace("VRSInvoiceProcessView")]
    public partial class VRSInvoiceProcessView : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Invoicing.ARInvoiceProcess objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSInvoiceProcessView));
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

            txtBA.Text = Request.QueryString["cnm"];
            if (Request.QueryString["bcid"] != null) hdnBCID.Value = Request.QueryString["bcid"];
            if (Request.QueryString["aid"] != null) hdnAID.Value = Request.QueryString["aid"];
            SetCSS(Request.QueryString["th"]);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css?v=" + DateTime.Now.Ticks.ToString();
            lnkTHEME.Attributes["href"] = strServerPath + "/css/" + strTheme + "/theme.css?v=" + DateTime.Now.Ticks.ToString();
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css?v=" + DateTime.Now.Ticks.ToString();

        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {

            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
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
            objCore = new Core.Invoicing.ARInvoiceProcess();
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
                objCore.USER_SESSION_ID = new Guid(arrRecord[3]);

                bReturn = objCore.FetchProcessedRecords(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);


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

        #region ApproveInvoice (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] ApproveInvoice(string[] ArrRecord, string[] ArrAcct)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Invoicing.ARInvoiceProcess();
            objComm = new classes.CommonClass();

            Core.Invoicing.ARInvoiceBillingAccountList[] objAcct = new Core.Invoicing.ARInvoiceBillingAccountList[0];
           

            try
            {
                objCore.BILLING_CYCLE_ID = new Guid(ArrRecord[0].Trim());
                objCore.USER_ID = new Guid(ArrRecord[1].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[2]);
                objCore.USER_SESSION_ID = new Guid(ArrRecord[3]);

                objAcct = new Core.Invoicing.ARInvoiceBillingAccountList[(ArrAcct.Length / 2)];

                #region Populate Billing Account
                for (int i = 0; i < objAcct.Length; i++)
                {
                    objAcct[i] = new Core.Invoicing.ARInvoiceBillingAccountList();
                    objAcct[i].ID = new Guid(ArrAcct[intListIndex]);
                    objAcct[i].TOTAL_AMOUNT = Convert.ToDouble(ArrAcct[intListIndex + 1]);
                    intListIndex = intListIndex + 2;
                }
                #endregion

                intListIndex = 0;

                bReturn = objCore.ApproveInvoice(Server.MapPath("~"), objAcct, ref strReturnMsg, ref strCatchMessage);

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

        #region GenerateBillingAccountInvoice(Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] GenerateBillingAccountInvoice(string[] ArrParams)
        {
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];
            string[] arrRet = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];


            try
            {

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(Server.MapPath("~"));
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);


                objDoc.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportServerURL"]);
                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/BillingAccountInvoice";

                ArrParams[3] = ArrParams[3].Trim().Replace(" ", "_");
                ArrParams[3] = ArrParams[3].Trim().Replace("&", "_");

                strDocName = "BillingAccount_Invoice_" + ArrParams[3] + "_" + ArrParams[1].Trim().Replace(" ", "_");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "billing_cycle_id";
                objParam[0].Values.Add(ArrParams[0]);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "billing_account_id";
                objParam[1].Values.Add(ArrParams[2]);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(Server.MapPath("~") + "/Invoicing/MailTemp"))
                    Directory.CreateDirectory(Server.MapPath("~") + "/Invoicing/MailTemp");

                if (File.Exists(Server.MapPath("~") + "/Invoicing/MailTemp/" + strDocName + ".pdf"))
                {
                    File.Delete(Server.MapPath("~") + "/Invoicing/MailTemp/" + strDocName + ".pdf");
                }

                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/Invoicing/MailTemp/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();

                arrRet = new string[3];
                arrRet[0] = "true";
                arrRet[1] = strDocName + ".pdf";
                arrRet[2] = Server.MapPath("~") + "/Invoicing/MailTemp/" + strDocName + ".pdf";

            }
            catch (Exception expErr)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = expErr.Message;
            }
            finally
            {
                strReturnMsg = null; strCatchMessage = null;
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }
            return arrRet;
        }
        #endregion

        #region GenerateInstitutionInvoice(Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] GenerateInstitutionInvoice(string[] ArrParams)
        {
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];
            string[] arrRet = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(Server.MapPath("~"));
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);


                objDoc.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportServerURL"]);
                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/InstitutionInvoice";

                ArrParams[4] = ArrParams[4].Trim().Replace(" ", "_");
                ArrParams[4] = ArrParams[4].Trim().Replace("&", "_");
                strDocName = "Institution_Invoice_" + ArrParams[4].Trim() + "_" + ArrParams[1].Trim().Replace(" ", "_");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "billing_cycle_id";
                objParam[0].Values.Add(ArrParams[0]);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "institution_id";
                objParam[1].Values.Add(ArrParams[3]);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(Server.MapPath("~") + "/Invoicing/MailTemp"))
                    Directory.CreateDirectory(Server.MapPath("~") + "/Invoicing/MailTemp");

                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/Invoicing/MailTemp/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();

                arrRet = new string[3];
                arrRet[0] = "true";
                arrRet[1] = strDocName + ".pdf";
                arrRet[2] = Server.MapPath("~") + "/Invoicing/MailTemp/" + strDocName + ".pdf";


            }
            catch (Exception expErr)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = expErr.Message;
            }
            finally
            {
                strReturnMsg = null; strCatchMessage = null;
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }

            return arrRet;
        }
        #endregion

        #region DeleteInvoiceFile(Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] DeleteInvoiceFile(string strFile)
        {
            string[] arrRet = new string[0];

            try
            {
                arrRet = new string[1];
                arrRet[0] = "true";
                if (File.Exists(strFile))
                {
                    File.Delete(strFile);
                }
            }
            catch (Exception expErr)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = expErr.Message;
            }

            return arrRet;
        }
        #endregion
    }
}