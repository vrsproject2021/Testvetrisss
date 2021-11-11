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
    [AjaxPro.AjaxNamespace("VRSMailInvoice")]
    public partial class VRSMailInvoice : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Invoicing.InvoiceProcess objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSMailInvoice));
            SetAttributes();
            SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnSend1.Attributes.Add("onclick", "javascript:btnSend_OnClick();");
            btnSend2.Attributes.Add("onclick", "javascript:btnSend_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:parent.HideDataList('Y');");
            btnClose2.Attributes.Add("onclick", "javascript:parent.HideDataList('Y');");
        } 
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            hdnUserID.Value = Request.QueryString["uid"];
            hdnInvFile.Value = Request.QueryString["inv"];
            hdnInvFilePath.Value = Server.MapPath("~") + "/Invoicing/MailTemp/" + hdnInvFile.Value;
            hdnAcctID.Value = Request.QueryString["acctid"];
            hdnCycleID.Value = Request.QueryString["cycid"];
            if (Request.QueryString["instid"] != null) hdnInstID.Value = Request.QueryString["instid"];
            string strTheme = Request.QueryString["th"];
            hdnType.Value = Request.QueryString["type"];

            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            hdnDivider.Value = objComm.RecordDivider.ToString();
            objComm = null;

            //txtSubject.Text = "Invoice for " + hdnCycleName.Value;
            lblAttach.Text = lblAttach.Text + " <a href='#' style='color: blue; text-decoration: underline' onclick='javascript:ShowInvoice();'>" + hdnInvFile.Value + "</a>";
            hdnRootDir.Value = ConfigurationManager.AppSettings["RootDirectory"];
            FetchMailParameters();

            SetCSS(strTheme);
        }
        #endregion

        #region FetchMailParameters
        private void FetchMailParameters()
        {
            objCore = new Core.Invoicing.InvoiceProcess();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            string[] arrayCode = new string[0];
            objComm = new classes.CommonClass();

            StringBuilder sb = new StringBuilder();
            string strCompany = string.Empty;
            string strSender = string.Empty;
            string strCycle = string.Empty;
            string strDtFrom = string.Empty;
            string strDtTo = string.Empty;
            string strDtDue = string.Empty;
            string strMailText= string.Empty;
            string strLoginURL = string.Empty;

            try
            {
                objComm.SetRegionalFormat();
                objCore.BILLING_CYCLE_ID = new Guid(hdnCycleID.Value);
                objCore.BILLING_ACCOUNT_ID = new Guid(hdnAcctID.Value);
                objCore.INSTITUTION_ID = new Guid(hdnInstID.Value);
                objCore.MAIL_TYPE = hdnType.Value;
                objCore.USER_ID = new Guid(hdnUserID.Value);


                bReturn = objCore.FetchMailParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    #region PopulateMailTo
                    foreach (DataRow dr in ds.Tables["MaitTo"].Rows)
                    {
                        if (sb.ToString().Trim() != string.Empty) sb.Append(";");
                        sb.Append(Convert.ToString(dr["contact_person_email_id"]).Trim()) ;
                    }
                    txtTo.Text = sb.ToString();
                    #endregion

                    sb.Clear();

                    #region PopulateMailCC
                    foreach (DataRow dr in ds.Tables["MaitCC"].Rows)
                    {
                        if (sb.ToString().Trim() != string.Empty) sb.Append(";");
                        sb.Append(Convert.ToString(dr["email_id"]).Trim());
                    }
                    txtCC.Text = sb.ToString();
                    #endregion

                    sb.Clear();

                    foreach (DataRow dr in ds.Tables["BillingCycle"].Rows)
                    {
                        strCycle = Convert.ToString(dr["name"]).Trim();
                        strDtFrom = objComm.IMDateFormat(dr["date_from"],objComm.DateFormat);
                        strDtTo = objComm.IMDateFormat(dr["date_till"], objComm.DateFormat);
                        strDtDue = objComm.IMDateFormat(dr["due_date"], objComm.DateFormat);
                    }

                    foreach (DataRow dr in ds.Tables["Company"].Rows)
                    {
                        strCompany = Convert.ToString(dr["company_name"]).Trim();
                        hdnSENDMAILID.Value = Convert.ToString(dr["sender_email_id"]).Trim();
                        hdnMAILSVRNAME.Value = Convert.ToString(dr["mail_server"]).Trim();
                        hdnMAILSVRPORT.Value = Convert.ToString(dr["mail_server_port"]).Trim();
                        hdnMAILSVRUSRCODE.Value = Convert.ToString(dr["mail_user_code"]).Trim();
                        hdnMAILSVRUSRPWD.Value = Convert.ToString(dr["mail_user_pwd"]).Trim();
                        hdnMAILSSLENABLED.Value = Convert.ToString(dr["mail_ssl_enabled"]).Trim();
                        strMailText = Convert.ToString(dr["mail_text"]).Trim();
                        strLoginURL = Convert.ToString(dr["login_url"]).Trim() + "?AID=" + hdnAcctID.Value;
                    }

                    foreach (DataRow dr in ds.Tables["Sender"].Rows)
                    {
                        strSender = Convert.ToString(dr["name"]).Trim();
                    }

                    txtSubject.Text = strCompany + " Invoice for " + strCycle + " (from " + strDtFrom +" to " + strDtTo +")";
                    strMailText = strMailText.Replace("[FROM_DATE]", strDtFrom);
                    strMailText = strMailText.Replace("[TILL_DATE]", strDtTo);
                    strMailText = strMailText.Replace("[DUE_DATE]", strDtDue);
                    strMailText = strMailText.Replace("[PAYMENT_URL]", strLoginURL);

                    //sb.AppendLine("Dear Valued Client,");
                    //sb.AppendLine("");
                    //sb.AppendLine("");
                    //sb.AppendLine("Please find attached Invoice for the period " + strDtFrom + " to " + strDtTo);
                    //sb.AppendLine("");
                    //sb.AppendLine("");
                    //sb.AppendLine("");
                    //sb.AppendLine("Regards,");
                    //sb.AppendLine(strSender);
                    //sb.AppendLine(strCompany);

                    //txtBody.Text = sb.ToString();

                    rtbMailText.Text = strMailText;

                }
                else
                {
                    arrayCode = new string[1];
                    arrayCode[0] = strCatchMessage.Trim();
                    lblMsg.Text = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
                    hdnError.Value = "Y";
                }
            }
            catch (Exception ex)
            {
                arrayCode = new string[1];
                arrayCode[0] = ex.Message.Trim();
                lblMsg.Text = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
                hdnError.Value = "Y";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
                objComm = null;
            }
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
            rtbMailText.ContentsCss = strServerPath + "/css/" + strTheme + "/contents.css";
        }
        #endregion

        #region SendMail(Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SendMail(string[] ArrMail)
        {
            bool bReturn = false;
            string[] arrRet = new string[3];
            MailSender objMail = new MailSender();
            objComm = new classes.CommonClass();
            string[] arrAttachFile = new string[1];
            string[] arrAttachFileName = new string[1];
            string strCatchMessage = string.Empty;
            string[] arrayCode = new string[0];
            string strMsg = string.Empty;

            try
            {
                objMail.MailServer = ArrMail[0];
                objMail.MailServerPortNo = Convert.ToInt32(ArrMail[1]);
                if (ArrMail[2] != "Y")
                    objMail.MailServerSSLEnabled = false;
                else
                    objMail.MailServerSSLEnabled = true;
                objMail.MailServerUserId = ArrMail[3];
                objMail.MailFrom = ArrMail[3];
                objMail.DecryptPassword = "N";
                objMail.MailServerPassword = CoreCommon.DecryptString(ArrMail[4]);

                objMail.MailSenderName = ArrMail[5];
                objMail.MailTo = ArrMail[6];
                objMail.MailCC = ArrMail[7];
                objMail.MailSubject = ArrMail[8];
                objMail.MailBody = ArrMail[9];
                objMail.IsMailBodyHTML = true;
                objMail.Attachments = 1;

                arrAttachFile[0] = ArrMail[10];
                arrAttachFileName[0] = ArrMail[11];
                objMail.AttachedFile = arrAttachFile;
                objMail.AttachedFileName = arrAttachFileName;

                bReturn = objMail.SendMail(ref strCatchMessage);

                if (bReturn)
                {
                    arrayCode = new string[1];
                    arrayCode[0] = "273";
                    strMsg = objComm.SetErrorResources(arrayCode, "N", false, string.Empty, string.Empty);

                    arrRet = new string[2];
                    arrRet[0] = "true";
                    arrRet[1] = strMsg;
                }
                else
                {
                    arrayCode = new string[1];
                    arrayCode[0] = strCatchMessage.Trim();
                    strMsg = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);

                    arrRet = new string[2];
                    arrRet[0] = "false";
                    arrRet[1] = strMsg;
                }
            }
            catch (Exception expErr)
            {
                bReturn = false;
                string[] larry = new string[1];
                larry[0] = expErr.Message.ToString();
                arrRet[0] = "catch";
                arrRet[1] = objComm.SetErrorResources(larry, "N", true, "", "");

            }
            finally
            {
                objCore = null; objComm = null;
                strCatchMessage = null;
            }
            return arrRet;
        }
        #endregion
    }
}