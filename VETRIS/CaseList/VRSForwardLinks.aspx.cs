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
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using AjaxPro;

namespace VETRIS.CaseList
{
    [AjaxPro.AjaxNamespace("VRSForwardLinks")]
    public partial class VRSForwardLinks : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Case.CaseStudy objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSForwardLinks));
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
            rdoEmail.Attributes.Add("onclick", "javascript:ToggleNotification();");
            rdoSMS.Attributes.Add("onclick", "javascript:ToggleNotification();");
            rdoFax.Attributes.Add("onclick", "javascript:ToggleNotification();");
            chkRpt.Attributes.Add("onclick", "javascript:chkRpt_OnClick();");
            chkImg.Attributes.Add("onclick", "javascript:chkImg_OnClick();");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            hdnUserID.Value = Request.QueryString["uid"];
            hdnStudyID.Value = Request.QueryString["sid"];
            string strTheme = Request.QueryString["th"];
           
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            hdnDivider.Value = objComm.RecordDivider.ToString();
            objComm = null;

            ////txtSubject.Text = "Invoice for " + hdnCycleName.Value;
            //lblAttach.Text = lblAttach.Text + " <a href='#' style='color: blue; text-decoration: underline' onclick='javascript:ShowInvoice();'>" + hdnInvFile.Value + "</a>";
            hdnRootDir.Value = ConfigurationManager.AppSettings["RootDirectory"];
            
           
            FetchNotificationParameters();
            SetCSS(strTheme);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
           
        }
        #endregion

        #region FetchNotificationParameters
        private void FetchNotificationParameters()
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            string[] arrayCode = new string[0];
            objComm = new classes.CommonClass();
            string strSender = string.Empty;
            StringBuilder sb = new StringBuilder();


            try
            {
                objComm.SetRegionalFormat();
                objCore.ID = new Guid(hdnStudyID.Value);
                objCore.USER_ID = new Guid(hdnUserID.Value);


                bReturn = objCore.FetchForwardNotificationParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    foreach (DataRow dr in ds.Tables["Params"].Rows)
                    {
                       
                        hdnMAILSVRNAME.Value = Convert.ToString(dr["mail_server"]).Trim();
                        hdnMAILSVRPORT.Value = Convert.ToString(dr["mail_server_port"]).Trim();
                        hdnMAILSVRUSRCODE.Value = Convert.ToString(dr["mail_user_code"]).Trim();
                        hdnMAILSVRUSRPWD.Value = Convert.ToString(dr["mail_user_pwd"]).Trim();
                        hdnMAILSSLENABLED.Value = Convert.ToString(dr["mail_ssl_enabled"]).Trim();
                        hdnSMSACCTSID.Value = Convert.ToString(dr["smd_acct_sid"]).Trim();
                        hdnSMSAUTHTKNNO.Value = Convert.ToString(dr["smd_auth_token"]).Trim();
                        hdnSMSSENDERNO.Value = Convert.ToString(dr["sms_sender_no"]).Trim();

                        hdnFAXENABLE.Value = Convert.ToString(dr["fax_enabled"]).Trim();
                        hdnFAXAPIURL.Value = Convert.ToString(dr["fax_api_url"]).Trim();
                        hdnFAXAUTHUSERID.Value = Convert.ToString(dr["fax_auth_uid"]).Trim();
                        hdnFAXAUTHPWD.Value = Convert.ToString(dr["fax_auth_pwd"]).Trim();
                        hdnFAXCSID.Value = Convert.ToString(dr["fax_csid"]).Trim();
                        hdnFAXREFTEXT.Value = Convert.ToString(dr["fax_ref_text"]).Trim();
                        hdnFAXREPADDR.Value = Convert.ToString(dr["fax_reply_at"]).Trim();
                        hdnFAXCONTACT.Value = Convert.ToString(dr["fax_contact_at"]).Trim();
                        hdnFAXRETRY.Value = Convert.ToString(dr["fax_retry"]).Trim();
                    }

                    foreach (DataRow dr in ds.Tables["Sender"].Rows)
                    {
                        strSender = Convert.ToString(dr["name"]).Trim();
                    }

                    foreach (DataRow dr in ds.Tables["Texts"].Rows)
                    {
                        txtSubject.Text = Convert.ToString(dr["email_subject"]).Trim();
                        lblEmailPrev.Text = Convert.ToString(dr["email_text"]).Trim();
                        lblSMSPrev.Text = "<span id='spnSMShdr'>" + Convert.ToString(dr["sms_text"]).Trim() + "</span>";
                        hdnFaxRpt.Value = Convert.ToString(dr["fax_rpt"]).Trim();
                        txtFaxNo.Text = Convert.ToString(dr["fax_no"]).Trim();
                        hdnStudyStatus.Value = Convert.ToString(dr["study_status"]).Trim();
                        hdnPName.Value = Convert.ToString(dr["patient_name"]).Trim();
                        hdnVRSPACSLINKURL.Value = Convert.ToString(dr["VRSPACSLINKURL"]).Trim();
                    }

                    //if (hdnFAXENABLE.Value == "Y")
                    //{
                    //    if (hdnFaxRpt.Value == "N")
                    //    {
                    //        rdoFax.Enabled = false;
                    //    }
                    //    else
                    //    {
                    //        rdoFax.Enabled = true;
                    //    }
                    //}
                    //else
                    //{
                    //    rdoFax.Enabled = false;
                    //}
                    

                    //sb.AppendLine("");
                    //sb.AppendLine("");
                    //sb.AppendLine("");
                    //sb.AppendLine("Please find attached Invoice for the period " + strDtFrom + " to " + strDtTo);
                    //sb.AppendLine("");
                    //sb.AppendLine("");
                    //sb.AppendLine("");
                    //sb.AppendLine("Regards,");
                    //sb.AppendLine(strSender);
                    

                    //txtBody.Text = sb.ToString();

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

        #region Print Report

        #region PrintFinalReport(Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] PrintFinalReport(string StudyID, string strPatientName, string UserID)
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
                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/FinalRpt";
                strDocName = "FINAL_REPORT_" + strPatientName.ToUpper().Trim().Replace(" ", "_") + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "id";
                objParam[0].Values.Add(StudyID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "user_id";
                objParam[1].Values.Add(UserID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID)) Directory.CreateDirectory(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID);
                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();

                arrRet = new string[4];
                arrRet[0] = "true";
                arrRet[1] = Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";
                arrRet[2] = strDocName + ".pdf";
                arrRet[3] = ConfigurationManager.AppSettings["ServerPath"] + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";
            }
            catch (Exception expErr)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = expErr.Message.Trim();
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

        #region PrintPreliminaryReport(Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] PrintPreliminaryReport(string StudyID, string strPatientName, string UserID)
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
                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/PrelimRpt";
                strDocName = "PRELIMININARY_REPORT_" + strPatientName.ToUpper().Trim().Replace(" ", "_") + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "id";
                objParam[0].Values.Add(StudyID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "user_id";
                objParam[1].Values.Add(UserID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID))
                    Directory.CreateDirectory(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID);

                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();

                arrRet = new string[4];
                arrRet[0] = "true";
                arrRet[1] = Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";
                arrRet[2] = strDocName + ".pdf";
                arrRet[3] = ConfigurationManager.AppSettings["ServerPath"] + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";

            }
            catch (Exception expErr)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = expErr.Message.Trim();
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

        #region PrintCustomFinalReport(Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] PrintCustomFinalReport(string StudyID, string strPatientName, string UserID)
        {
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];
            string[] arrRet = new string[0];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(Server.MapPath("~"));
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);


                objDoc.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportServerURL"]);
                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/CustomFinalRpt";
                strDocName = "FINAL_REPORT_" + strPatientName.ToUpper().Trim().Replace(" ", "_") + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "id";
                objParam[0].Values.Add(StudyID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "user_id";
                objParam[1].Values.Add(UserID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID))
                    Directory.CreateDirectory(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID);

                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();

                arrRet = new string[4];
                arrRet[0] = "true";
                arrRet[1] = Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";
                arrRet[2] = strDocName + ".pdf";
                arrRet[3] = ConfigurationManager.AppSettings["ServerPath"] + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";

            }
            catch (Exception expErr)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = expErr.Message.Trim();
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

        #region PrintCustomPreliminaryReport(Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] PrintCustomPreliminaryReport(string StudyID, string strPatientName, string UserID, string Format)
        {
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];
            string[] arrRet = new string[0];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(Server.MapPath("~"));
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);


                objDoc.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportServerURL"]);
                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/CustomPrelimRpt";
                strDocName = "PRELIMININARY_REPORT_" + strPatientName.ToUpper().Trim().Replace(" ", "_") + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "id";
                objParam[0].Values.Add(StudyID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "user_id";
                objParam[1].Values.Add(UserID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID))
                    Directory.CreateDirectory(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID);

                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();

                arrRet = new string[4];
                arrRet[0] = "true";
                arrRet[1] = Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";
                arrRet[2] = strDocName + ".pdf";
                arrRet[3] = ConfigurationManager.AppSettings["ServerPath"] + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";

            }
            catch (Exception expErr)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = expErr.Message.Trim();
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

        #endregion

        #region SendMail(Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SendMail(string[] ArrMail)
        {
            bool bReturn = false;
            string[] arrRet = new string[3];
            MailSender objMail = new MailSender();
            objComm = new classes.CommonClass();
            string strCatchMessage = string.Empty;
            string[] arrayCode = new string[0];
            string strMsg = string.Empty;
            string[] arrAttachments = new string[1];
            string[] arrAttachmentName = new string[1];

            try
            {
                objMail.MailServer = ArrMail[0];
                objMail.MailServerPortNo = Convert.ToInt32(ArrMail[1]);
                if (ArrMail[2] != "Y")
                    objMail.MailServerSSLEnabled = false;
                else
                    objMail.MailServerSSLEnabled = true;
                objMail.MailServerUserId = ArrMail[3];
                objMail.MailServerPassword = ArrMail[4];

                objMail.MailFrom = ArrMail[3];
                objMail.MailTo = ArrMail[5];
                objMail.MailCC = ArrMail[6];
                objMail.MailSubject = ArrMail[7];
                objMail.MailBody = ArrMail[8].Replace("\n","<br/>");
                objMail.Attachments = 1;
                arrAttachments[0] = ArrMail[9];
                objMail.AttachedFile = arrAttachments;
                arrAttachmentName[0] = ArrMail[10];
                objMail.AttachedFileName = arrAttachmentName;
                objMail.IsMailBodyHTML = true;

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

        #region SendSMS(Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SendSMS(string[] ArrParms,string[] ArrText)
        {
            bool bReturn = false;
            string strAcctSID = string.Empty;
            string strAuthToken = string.Empty;
            string strSendTo = string.Empty;
            string strNumbers = string.Empty;
            string strSenderNo = string.Empty;
            string strSMSText = string.Empty;
            string MessageSID = string.Empty;
            string[] arrRet = new string[0];
            string[] arrNos = new string[0];
            string[] arrayCode = new string[0];
            string strErrFlag = "N";
            string strMsg = string.Empty;
            StringBuilder sbText = new StringBuilder();

            objComm = new classes.CommonClass();

            strAcctSID = ArrParms[0].Trim();
            strAuthToken = ArrParms[1].Trim();
            strSenderNo = ArrParms[2].Trim();
            strNumbers = ArrParms[3].Trim();

            TwilioClient.Init(strAcctSID, strAuthToken);

            try
            {
                if (strNumbers.Contains(";"))
                {
                    arrNos = strNumbers.Split(';');
                }
                else
                {
                    arrNos = new string[1];
                    arrNos[0] = strNumbers;
                }
                for (int i = 0; i < ArrText.Length; i++)
                {
                    sbText.AppendLine(ArrText[i].Trim());
                }
                strSMSText = sbText.ToString();

                if (strSMSText != string.Empty)
                {
                    System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)(0xc0 | 0x300 | 0xc00) | System.Net.SecurityProtocolType.Ssl3;

                    #region Send SMS
                    for (int j = 0; j < arrNos.Length; j++)
                    {
                        strSendTo = arrNos[j];
                        var to = new PhoneNumber(strSendTo);
                        var message = MessageResource.Create(
                            to,
                            from: new PhoneNumber(strSenderNo),
                            body: strSMSText);

                        if (message.ErrorMessage.Trim() == string.Empty)
                        {
                            arrayCode = new string[1];
                            arrayCode[0] = "289";
                            strMsg = objComm.SetErrorResources(arrayCode, "N", false, string.Empty, string.Empty);

                            arrRet = new string[3];
                            arrRet[0] = "true";
                            arrRet[1] = strMsg;
                            arrRet[2] = message.Sid;

                        }
                        else
                        {
                            arrayCode = new string[1];
                            arrayCode[0] = message.ErrorMessage.Trim();
                            strMsg = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);

                            arrRet = new string[3];
                            arrRet[0] = "false";
                            arrRet[1] = strMsg;
                            arrRet[2] = message.Sid;
                            strErrFlag = "Y";
                            break;
                        }
                    }

                    //if (strErrFlag == "Y") break;
                    #endregion
                }

            }
            catch (Twilio.Exceptions.TwilioException expErr)
            {
                bReturn = false;
                string[] larry = new string[1];
                larry[0] = expErr.Message.ToString();
                arrRet[0] = "catch";
                arrRet[1] = objComm.SetErrorResources(larry, "N", true, "", "");
            }
            finally {
                objComm = null;

            }
            return arrRet;
        }
        #endregion

        #region SendFax(Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SendFax(string[] ArrParms)
        {
            string strCatchMsg = string.Empty;
            string strRetStat = string.Empty;
            bool bReturn = false;
            Core.InterFaxSender objFax;
            string strURL = string.Empty;
            string strAuthUserID = string.Empty;
            string strAuthPwd = string.Empty;
            string strCSID = string.Empty;
            string strRefText = string.Empty;
            string strReply = string.Empty;
            string strContact = string.Empty;
            int intRetry = 0;
            string strFilePath = string.Empty;
            string strFaxNo = string.Empty;

            string[] arrRet = new string[0];
            string[] arrayCode = new string[0];
            string strErrFlag = "N";
            string strMsg = string.Empty;

            objComm = new classes.CommonClass();

            try
            {
                strURL = ArrParms[0].Trim();
                strAuthUserID = ArrParms[1].Trim();
                strAuthPwd = CoreCommon.DecryptString(ArrParms[2].Trim());
                strCSID = ArrParms[3].Trim();
                strRefText = ArrParms[4].Trim();
                strReply = ArrParms[5].Trim();
                strContact = ArrParms[6].Trim();
                intRetry = Convert.ToInt32(ArrParms[7]);
                strFilePath = ArrParms[8].Trim();
                strFaxNo = ArrParms[9].Trim();

                objFax = new InterFaxSender();

                objFax.URL(strURL);
                objFax.Authorize(strAuthUserID, strAuthPwd);
                objFax.CSID(strCSID);
                objFax.Reference(strRefText);
                objFax.ReplyAddress(strReply);
                objFax.Contact(strContact);
                objFax.RetriesToPerform(intRetry);
                objFax.AddFiles(strFilePath);

                bReturn = objFax.Send(strFaxNo, ref strRetStat, ref strCatchMsg);

                if (bReturn)
                {
                    arrayCode = new string[1];
                    arrayCode[0] = strRetStat;
                    strMsg = objComm.SetErrorResources(arrayCode, "N", false, string.Empty, string.Empty);

                    arrRet = new string[2];
                    arrRet[0] = "true";
                    arrRet[1] = strMsg;
                }
                else
                {
                    arrayCode = new string[1];
                    arrayCode[0] = strCatchMsg;
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
                objComm = null; objFax = null;

            }
            return arrRet;
        }
        #endregion
    }
}