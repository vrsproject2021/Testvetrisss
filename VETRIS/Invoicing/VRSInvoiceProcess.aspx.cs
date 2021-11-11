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
    [AjaxPro.AjaxNamespace("VRSInvoiceProcess")]
    public partial class VRSInvoiceProcess : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Invoicing.ARInvoiceProcess objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSInvoiceProcess));
            SetAttributes();
            if ((!CallBackBA.CausedCallback) && (!CallBackBAProc.CausedCallback))
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

            if (Request.QueryString["bcid"] != null) hdnBCID.Value = Request.QueryString["bcid"];
            if (Request.QueryString["aid"] != null) hdnAID.Value = Request.QueryString["aid"];

            FetchParameters(UserID);
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
            ddlBillingCycle.Attributes.Add("onchange", "javascript:ddlBillingCycle_OnChange();");
            btnProcess.Attributes.Add("onclick", "javascript:btnProcess_OnClick();");
            btnRefresh.Attributes.Add("onclick", "javascript:ddlBillingCycle_OnChange();");
            btnFiltAppv.Attributes.Add("onclick", "javascript:FilterApproved();");
            btnFiltNotAppv.Attributes.Add("onclick", "javascript:FilterNotApproved();");
            btnApprove.Attributes.Add("onclick", "javascript:btnApprove_OnClick();");
        }
        #endregion

        #region FetchParameters
        private void FetchParameters(Guid UserID)
        {
            objCore = new Core.Invoicing.ARInvoiceProcess();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                objCore.USER_ID = UserID;
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
                    ddlBillingCycle.SelectedValue = hdnBCID.Value;
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

        #region CallBackBA_Callback
        protected void CallBackBA_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadUnprocessedBillingAccountList(e.Parameters);
            grdBA.Width = Unit.Percentage(100);
            grdBA.RenderControl(e.Output);
            spnBAERR.RenderControl(e.Output);
            //spnUser.RenderControl(e.Output);
        }
        #endregion

        #region LoadUnprocessedBillingAccountList
        private void LoadUnprocessedBillingAccountList(string[] arrRecord)
        {
            objCore = new Core.Invoicing.ARInvoiceProcess();
            bool bReturn = false; string strCatchMessage = "";
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();


            try
            {
                objComm.SetRegionalFormat();
                objCore.BILLING_CYCLE_ID = new Guid(arrRecord[0]);


                bReturn = objCore.FetchUnprocessedBillingAccountList(Server.MapPath("~"), ref ds, ref strCatchMessage);


                if (bReturn)
                {

                    grdBA.DataSource = ds.Tables["BillingAccounts"];
                    grdBA.DataBind();
                    grdBA.PageSize = ds.Tables["BillingAccounts"].Rows.Count;
                    grdBA.GroupingNotificationText = ds.Tables["BillingAccounts"].Rows.Count.ToString() + " record(s)";
                    spnBAERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBBAErr\" value=\"\" />";

                }
                else
                {
                    spnBAERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBBAErr\" value=\"" + strCatchMessage + "\" />";
                }

            }
            catch (Exception ex)
            {
                spnBAERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBBAErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose(); objComm = null;

            }

            
        }
        #endregion

        #region CallBackBAProc_Callback
        protected void CallBackBAProc_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadProcessedBillingAccountList(e.Parameters);
            grdBAProc.Width = Unit.Percentage(100);
            grdBAProc.RenderControl(e.Output);
            spnBAProcERR.RenderControl(e.Output);
            //spnUser.RenderControl(e.Output);
        }
        #endregion

        #region LoadProcessedBillingAccountList
        private void LoadProcessedBillingAccountList(string[] arrRecord)
        {
            objCore = new Core.Invoicing.ARInvoiceProcess();
            bool bReturn = false; string strCatchMessage = "";
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();


            try
            {
                objComm.SetRegionalFormat();
                objCore.BILLING_CYCLE_ID = new Guid(arrRecord[0]);
                objCore.STATUS = arrRecord[1].Trim();
                objCore.MENU_ID = Convert.ToInt32(arrRecord[2]);
                objCore.USER_ID = new Guid(arrRecord[3]);

                bReturn = objCore.FetchProcessedBillingAccountList(Server.MapPath("~"), ref ds, ref strCatchMessage);


                if (bReturn)
                {

                    grdBAProc.DataSource = ds.Tables["BillingAccounts"];
                    grdBAProc.DataBind();
                    grdBAProc.PageSize = ds.Tables["BillingAccounts"].Rows.Count;
                    grdBAProc.GroupingNotificationText = ds.Tables["BillingAccounts"].Rows.Count.ToString() + " record(s)";
                    spnBAProcERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBBAProcErr\" value=\"\" />";

                }
                else
                {
                    spnBAProcERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBBAProcErr\" value=\"" + strCatchMessage + "\" />";
                }

            }
            catch (Exception ex)
            {
                spnBAProcERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBBAProcErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose(); objComm = null;

            }


        }
        #endregion

        #region ProcessInvoice(Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] ProcessInvoice(string[] arrRecord, string[] ArrAcct)
        {
            objCore = new Core.Invoicing.ARInvoiceProcess();
            bool bReturn = false; string strReturnMessage = ""; string strCatchMessage = "";
            int intListIndex = 0;
            string[] arrRet = new string[0];
            objComm = new classes.CommonClass();
            Core.Invoicing.ARBillingAccountList[] objAcct = new Core.Invoicing.ARBillingAccountList[0];

            try
            {
                objCore.BILLING_CYCLE_ID = new Guid(arrRecord[0]);
                objCore.MENU_ID = Convert.ToInt32(arrRecord[1]);
                objCore.USER_ID = new Guid(arrRecord[2]);
                objCore.USER_SESSION_ID = new Guid(arrRecord[3]);

                #region Populate Billing Account
                objAcct = new Core.Invoicing.ARBillingAccountList[(ArrAcct.Length)];
                for (int i = 0; i < objAcct.Length; i++)
                {
                    objAcct[i] = new Core.Invoicing.ARBillingAccountList();
                    objAcct[i].ID = new Guid(ArrAcct[intListIndex]);
                    intListIndex = intListIndex + 1;
                }
                #endregion

                intListIndex = 0;

                bReturn = objCore.ProcessInvoice(Server.MapPath("~"), objAcct, ref strReturnMessage, ref strCatchMessage);
                
                if (bReturn)
                {
                    arrRet = new string[2];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMessage.Trim();

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
                        arrRet[1] = strReturnMessage.Trim();
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
                 objComm = null;
                 objCore = null;
            }

            return arrRet;
        }
        #endregion

        #region ReprocessInvoice (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] ReprocessInvoice(string[] ArrRecord)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;

            Core.Invoicing.ARStudyAmendment objSA = new Core.Invoicing.ARStudyAmendment();
            objComm = new classes.CommonClass();


            try
            {
                objSA.BILLING_CYCLE_ID = new Guid(ArrRecord[0].Trim());
                objSA.BILLING_ACCOUNT_ID = new Guid(ArrRecord[1].Trim());
                objSA.USER_ID = new Guid(ArrRecord[2].Trim());
                objSA.MENU_ID = Convert.ToInt32(ArrRecord[3]);

                bReturn = objSA.ReprocessInvoice(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

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
                objSA = null; objComm = null;
                strReturnMsg = null; strCatchMessage = null;
            }
            return arrRet;
        }
        #endregion

        #region CheckRecordLock (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] CheckRecordLock(string[] ArrRecord)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;

            CommonFunctions objCF = new CommonFunctions();



            try
            {
                objCF.RECORD_ID_UI = new Guid(ArrRecord[0].Trim());
                objCF.ADDITIONAL_RECORD_ID_UI = new Guid(ArrRecord[1].Trim());
                objCF.USER_SESSION_ID = new Guid(ArrRecord[2].Trim());
                objCF.USER_ID = new Guid(ArrRecord[3].Trim());
                objCF.MENU_ID = Convert.ToInt32(ArrRecord[4].Trim());

                bReturn = objCF.CheckAdditionalRecordLock(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {

                    arrRet = new string[2];
                    arrRet[0] = "true";
                    
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
                        arrRet[2] = objCF.USER_NAME;
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