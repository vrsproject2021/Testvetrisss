using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Fax.V1;
using Twilio.Types;
using VETRISScheduler.Core;
using System.Net.Mail;

namespace VETRISNotificationService
{
    public partial class NotificationService : ServiceBase
    {
        #region members & variables
        private static int intFreq = 30;
        private static string strConfigPath = AppDomain.CurrentDomain.BaseDirectory;
        private static string strSvcName = "VETRIS Notification Service";
        private static int intServiceID = 4;
        private static string strMailServer = string.Empty;
        private static int intPortNo = 0;
        private static string strSSL = string.Empty;
        private static string strMailUserID = string.Empty;
        private static string strMailUserPwd = string.Empty;
        private static string strMailSender = string.Empty;
        private static string strSenderNo = string.Empty;
        private static string strAcctSID = string.Empty;
        private static string strAuthToken = string.Empty;
        private static string strMailInvFolder = string.Empty;
        private static string strRptServerURL = string.Empty;
        private static string strRptServerFolder = string.Empty;
        private static string strFTPTempFolder = string.Empty;
        private static string strFAXAPIURL = string.Empty;
        private static string strFAXAUTHUSERID = string.Empty;
        private static string strFAXAUTHPWD = string.Empty;
        private static string strFAXCSID = string.Empty;
        private static string strFAXREFTEXT = string.Empty;
        private static string strFAXREPADDR = string.Empty;
        private static string strFAXCONTACT = string.Empty;
        private static string strFAXFILEFLDR = string.Empty;
        private static int intFAXRETRY = 0;
        private static string strSCHCASVCENBL = string.Empty;

        Scheduler objCore;
        Notification objNotify;

        #endregion

        public NotificationService()
        {
            InitializeComponent();
        }

        #region OnStart
        protected override void OnStart(string[] args)
        {

            try
            {

                System.Threading.ThreadStart job_data_synch = new System.Threading.ThreadStart(doProcess);
                System.Threading.Thread thread = new System.Threading.Thread(job_data_synch);
                thread.Start();


            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Error starting Service. " + ex.Message, true);
                EventLog.WriteEntry(strSvcName, "Error Starting Service." + ex.Message, EventLogEntryType.Warning);
            }

        }
        #endregion

        #region OnStop
        protected override void OnStop()
        {
            try
            {
                //System.Threading.Thread.Sleep(20000);
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Service stopped successfully.", false);
                base.OnStop();
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Error stopping Service. " + ex.Message, true);
            }
        }
        #endregion

        #region doProcess
        public void doProcess()
        {
            string strCatchMessage = string.Empty;

            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Service started Successfully", false);
                while (true)
                {
                    objCore = new Scheduler();
                    objCore.SERVICE_ID = intServiceID;


                    try
                    {

                        if (objCore.GetServiceDetails(strConfigPath, ref strCatchMessage))
                        {

                            intFreq = objCore.FREQUENCY;
                            strSvcName = objCore.SERVICE_NAME;

                            strMailServer = objCore.MAIL_SERVER_NAME;
                            intPortNo = objCore.MAIL_SERVER_PORT_NUMBER;
                            strSSL = objCore.SSL_ENABLED;
                            strMailUserID = objCore.MAIL_SERVER_USER_ID;
                            strMailUserPwd = objCore.MAIL_SERVER_USER_PASSWORD;
                            strMailSender = objCore.MAIL_SENDER_NAME;
                            strMailInvFolder = objCore.MAIL_INVOICE_FOLDER;
                            strRptServerURL = objCore.REPORT_SERVER_URL;
                            strRptServerFolder = objCore.REPORT_SERVER_FOLDER;
                            strFTPTempFolder = objCore.TEMPORARY_DOWNLOAD_FOLDER;
                            strSenderNo = objCore.SENDER_NO;
                            strAcctSID = objCore.ACCOUNT_SID;
                            strAuthToken = objCore.AUTHORISED_TOKEN;

                            strFAXAPIURL = objCore.FAX_API_URL;
                            strFAXAUTHUSERID = objCore.FAX_USER_ID;
                            strFAXAUTHPWD = objCore.FAX_PASSWORD;
                            strFAXCSID = objCore.FAX_CSID;
                            strFAXREFTEXT = objCore.FAX_REFERENCE_TEXT;
                            strFAXREPADDR = objCore.FAX_REPLY_ADDRESS;
                            strFAXCONTACT = objCore.FAX_CONTACT;
                            intFAXRETRY = objCore.FAX_RETRIES_TO_PERFORM;
                            strFAXFILEFLDR = objCore.FAX_FILE_FOLDER;

                            strSCHCASVCENBL = objCore.CASE_ASSIGNMENT_SERVICE_ENABLED;

                            FetchNotificationList();
                            if (strSCHCASVCENBL == "Y") FetchUnassignedStudyList();
                            CreateStudyNotifications();
                            FetchInvoiceSendingList();
                            ReleaseReports();

                        }
                        else
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Core::GetServiceDetails - Error : " + strCatchMessage, true);

                    }
                    catch (Exception ex)
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcess() - Error: " + ex.Message, true);
                        EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Warning);
                        System.Threading.Thread.Sleep(intFreq * 1000);
                    }

                    objCore = null;
                    System.Threading.Thread.Sleep(intFreq * 1000);
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcess() - Exception: " + expErr.Message, true);
            }
            finally
            { objCore = null; }
        }
        #endregion

        #region FetchNotificationList
        private void FetchNotificationList()
        {

            string strResult = string.Empty;
            string strCatchMsg = string.Empty;

            DataSet ds = new DataSet();
            Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
            Guid StudyID = new Guid("00000000-0000-0000-0000-000000000000");
            string strStudyUID = string.Empty;
            string strRecepientAddress = string.Empty;
            string strRecepientName = string.Empty;
            string strCCAddress = string.Empty;
            string strSubject = string.Empty;
            string strAttachment = string.Empty;
            string strMailAcctUserID = string.Empty;
            string strMailAcctPwd = string.Empty;
            string strNotifyText = string.Empty;
            string strRecepientNo = string.Empty;
            string strMsgSID = string.Empty;
            string strFileName = string.Empty;
            string strRptType = string.Empty;
            string strCustomRpt = string.Empty;
            string strCatchMessage = string.Empty;
            string strReturnMessage = string.Empty;
            string strEmailType = string.Empty;
            string strIsCustomRpt = string.Empty;
            string strPtName = string.Empty;
            objNotify = new Notification();


            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Fetching notification sending list...", false);
                if (objNotify.FetchNotificationSendingList(strConfigPath, ref ds, ref strCatchMsg))
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ds.Tables["MailList"].Rows.Count.ToString() + " Email record(s) fetched.", false);
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ds.Tables["SMSList"].Rows.Count.ToString() + " SMS record(s) fetched.", false);
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ds.Tables["FaxList"].Rows.Count.ToString() + " Fax record(s) fetched.", false);

                    #region Mail Sending
                    foreach (DataRow dr in ds.Tables["MailList"].Rows)
                    {

                        Id = new Guid(Convert.ToString(dr["email_log_id"]));
                        StudyID = new Guid(Convert.ToString(dr["study_hdr_id"]));
                        strStudyUID = Convert.ToString(dr["study_uid"]).Trim();
                        strRecepientAddress = Convert.ToString(dr["recipient_address"]).Trim();
                        strRecepientName = Convert.ToString(dr["recipient_name"]).Trim();
                        strCCAddress = Convert.ToString(dr["cc_address"]).Trim();
                        strSubject = Convert.ToString(dr["email_subject"]).Trim();
                        strAttachment = Convert.ToString(dr["file_name"]).Trim();
                        strNotifyText = Convert.ToString(dr["email_text"]).Trim();
                        strMailAcctUserID = Convert.ToString(dr["sender_email_address"]).Trim();
                        strMailAcctPwd = Convert.ToString(dr["sender_email_password"]).Trim();
                        strEmailType = Convert.ToString(dr["email_type"]).Trim();
                        strIsCustomRpt = Convert.ToString(dr["is_custom_rpt"]).Trim();
                        strPtName = Convert.ToString(dr["patient_name"]).Trim();

                        if (strMailAcctUserID.Trim() == "")
                        {
                            strMailAcctUserID = strMailUserID;
                            strMailAcctPwd = strMailUserPwd;
                        }

                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Sending mail to " + strRecepientAddress, false);

                        if (CreateMailAndSend(Id, strNotifyText, strSubject, strRecepientAddress, strCCAddress, strAttachment, strMailAcctUserID, strMailAcctPwd, strEmailType, strIsCustomRpt, StudyID, strPtName, ref strCatchMessage))
                        {
                            objNotify.EMAIL_LOG_ID = Id;
                            if (!objNotify.UpdateMailSendingStatus(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateMailSendingStatus() - Exception : " + strCatchMessage, true);
                            }
                        }

                    }
                    #endregion

                    #region SMS Sending
                    foreach (DataRow dr in ds.Tables["SMSList"].Rows)
                    {

                        Id = new Guid(Convert.ToString(dr["sms_log_id"]));
                        StudyID = new Guid(Convert.ToString(dr["study_hdr_id"]));
                        strStudyUID = Convert.ToString(dr["study_uid"]).Trim();
                        strRecepientNo = Convert.ToString(dr["recipient_no"]).Trim();
                        strRecepientName = Convert.ToString(dr["recipient_name"]).Trim();
                        strNotifyText = Convert.ToString(dr["sms_text"]).Trim();
                        strMsgSID = string.Empty;

                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Sending sms to " + strRecepientNo, false);

                        if (SendSMS(Id, strNotifyText, strRecepientNo, ref strMsgSID, ref strCatchMessage))
                        {

                            objNotify.SMS_LOG_ID = Id;
                            objNotify.MESSAGE_SID = strMsgSID;
                            objNotify.PROCESSED = "Y";

                            if (!objNotify.UpdateSMSSendingStatus(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                            {

                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateSMSSendingStatus() - Exception : " + strCatchMessage, true);
                            }

                        }
                        else
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, strCatchMessage, true);
                            objNotify.SMS_LOG_ID = Id;
                            objNotify.MESSAGE_SID = string.Empty;
                            objNotify.PROCESSED = "N";

                            if (!objNotify.UpdateSMSSendingStatus(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                            {

                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateSMSSendingStatus() - Exception : " + strCatchMessage, true);
                            }
                        }
                    }
                    #endregion

                    #region Fax Sending
                    foreach (DataRow dr in ds.Tables["FaxList"].Rows)
                    {

                        Id = new Guid(Convert.ToString(dr["id"]));
                        StudyID = new Guid(Convert.ToString(dr["study_hdr_id"]));
                        strStudyUID = Convert.ToString(dr["study_uid"]).Trim();
                        strRecepientNo = Convert.ToString(dr["recipient_no"]).Trim();
                        strFileName = Convert.ToString(dr["file_name"]).Trim();
                        strRptType = Convert.ToString(dr["report_type"]).Trim();
                        strCustomRpt = Convert.ToString(dr["custom_report"]).Trim();

                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Sending fax to " + strRecepientNo, false);

                        if (SendFax(Id, StudyID, strRecepientNo, strFileName, strRptType, strCustomRpt, ref strReturnMessage, ref strCatchMessage))
                        {

                            objNotify.FAX_LOG_ID = Id;
                            objNotify.PROCESSED = "Y";

                            if (!objNotify.UpdateFaxSendingStatus(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                            {

                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateFaxSendingStatus() - Exception : " + strCatchMessage, true);
                            }

                        }
                        else
                        {
                            if (strReturnMessage.Trim() != "")
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Sending fax to " + strRecepientNo + " failed (SUID :" + strStudyUID + "). " + strReturnMessage.Trim(), true);
                            else
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Sending fax to " + strRecepientNo + " failed (SUID :" + strStudyUID + "). " + strCatchMessage.Trim(), true);
                        }
                    }

                    #endregion
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchMailSendingList()  - Core::Exception: " + strCatchMsg, true);
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchMailSendingList() - Exception: " + ex.Message, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);

            }
            finally
            {
                objNotify = null; ds.Dispose();
            }


        }
        #endregion

        #region CreateMailAndSend
        private bool CreateMailAndSend(Guid mailLogID, string emailbody, string emailSubject, string MailTo, string MailCC, string Attachments, string MailAcctUserID, string MailAcctPwd, string email_type, string is_custom_rpt, Guid StudyID, string strPtName, ref string CatchMessage)
        {
            bool bReturn = false;
            MailSender objMail = new MailSender();
            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();
            string[] arrAttachments = new string[0];
            string[] arrAttachmentName = new string[0];
            string[] arrPath = new string[0];

            try
            {
                emailbody = emailbody.Replace("\n", "<br/>");
                emailbody = emailbody.Replace("\\n", "<br/>");
                sb.AppendLine(emailbody);

                objMail.MailServer = strMailServer; sb1.AppendLine("Mail Server : " + strMailServer);
                objMail.MailServerPortNo = intPortNo; sb1.AppendLine("Port No : " + intPortNo.ToString());
                if (strSSL != "Y")
                    objMail.MailServerSSLEnabled = false;
                else
                    objMail.MailServerSSLEnabled = true;
                sb1.AppendLine("Mail Server SSL Enabled : " + objMail.MailServerSSLEnabled.ToString());
                objMail.MailServerUserId = MailAcctUserID; sb1.AppendLine("Mail Server User ID : " + MailAcctUserID);
                sb1.AppendLine("Mail Server Password : " + MailAcctPwd);
                objMail.MailServerPassword = CoreCommon.DecryptString(MailAcctPwd);

                objMail.MailFrom = MailAcctUserID; sb1.AppendLine("Mail From : " + objMail.MailFrom);
                objMail.MailTo = MailTo; sb1.AppendLine("Mail To : " + MailTo);
                if (MailCC.Trim() != string.Empty)
                {
                    objMail.MailCC = MailCC; sb1.AppendLine("Mail CC : " + MailCC);
                }
                objMail.MailSubject = emailSubject; sb1.AppendLine("Subject : " + emailSubject);
                objMail.MailBody = sb.ToString();




                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Mail Object : " + sb1.ToString(), false);

                if (email_type == "RPT")
                {
                    if (emailSubject.Contains("Preliminary Report"))
                    {
                        if (is_custom_rpt == "Y")
                        {
                            GenerateCustomPreliminaryReport(StudyID.ToString(), strPtName, mailLogID.ToString(), ref objMail);
                        }
                        else
                        {
                            GeneratePreliminaryReport(StudyID.ToString(), strPtName, mailLogID.ToString(), ref objMail);
                        }
                    }
                    else if (emailSubject.Contains("Final Report"))
                    {
                        if (is_custom_rpt == "Y")
                        {
                            GenerateCustomFinalReport(StudyID.ToString(), strPtName, mailLogID.ToString(), ref objMail);
                        }
                        else
                        {
                            GenerateFinalReport(StudyID.ToString(), strPtName, mailLogID.ToString(), ref objMail);
                        }
                    }

                }
                else
                {
                    #region Attachment
                    if (Attachments.Trim() != string.Empty)
                    {
                        if (Attachments.Trim().Contains('±'))
                        {
                            arrAttachments = Attachments.Trim().Split('±');
                            arrAttachmentName = new string[arrAttachments.Length];
                            for (int i = 0; i < arrAttachmentName.Length; i++)
                            {
                                arrPath = arrAttachments[i].Split('/');
                                arrAttachmentName[i] = arrPath[arrPath.Length - 1];
                            }
                        }
                        else
                        {
                            arrAttachments = new string[1];
                            arrAttachmentName = new string[1];
                            arrAttachments[0] = Attachments;
                            arrPath = arrAttachments[0].Split('/');
                            arrAttachmentName[0] = arrPath[arrPath.Length - 1];
                        }

                        objMail.Attachments = arrAttachments.Length;
                        objMail.AttachedFile = arrAttachments;
                        objMail.AttachedFileName = arrAttachmentName;
                    }
                    #endregion
                }


                bReturn = objMail.SendMail(ref CatchMessage);

                if (bReturn)
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Mail Sent Successfully.", false);
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "CreateMailAndSend()- " + Convert.ToString(mailLogID) + " - Error: " + CatchMessage, true);
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "CreateMailAndSend() - " + Convert.ToString(mailLogID) + " - Exception: " + ex.Message, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);

            }
            finally
            {
                objMail = null;
            }

            return bReturn;
        }
        #endregion

        #region GeneratePreliminaryReport
        private void GeneratePreliminaryReport(string StudyID, string strPatientName, string UserID, ref MailSender objMail)
        {
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];
            string errorMessage = "";
            string errorValue = "N";
            string strTempPath = string.Empty;
            string[] arrAttachments = new string[1];
            string[] arrAttachmentName = new string[1];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(strConfigPath);
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);


                objDoc.ReportServerUrl = new Uri(strRptServerURL);
                objDoc.ReportPath = strRptServerFolder + "/Docs/PrelimRpt";
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

                if (!Directory.Exists(strConfigPath + "/TempRpts"))
                    Directory.CreateDirectory(strConfigPath + "/TempRpts");
                strTempPath = strConfigPath + "/TempRpts";
                System.IO.FileStream objFS = new System.IO.FileStream(strConfigPath + "/TempRpts/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);

                objFS.Write(btData, 0, btData.Length);
                objFS.Close();

                objMail.Attachments = 1;
                strTempPath = strTempPath + "/" + strDocName + ".pdf";
                arrAttachments[0] = strTempPath;
                objMail.AttachedFile = arrAttachments;
                arrAttachmentName[0] = strDocName + ".pdf";
                objMail.AttachedFileName = arrAttachmentName;
            }
            catch (Exception expErr)
            {
                errorValue = "Y";
                errorMessage = expErr.Message.Trim();
            }
            finally
            {
                strReturnMsg = null; strCatchMessage = null;
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }
        }
        #endregion

        #region GenerateCustomPreliminaryReport
        private void GenerateCustomPreliminaryReport(string StudyID, string strPatientName, string UserID, ref MailSender objMail)
        {
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];
            string errorMessage = "";
            string errorValue = "N";
            string strTempPath = string.Empty;
            string[] arrAttachments = new string[1];
            string[] arrAttachmentName = new string[1];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(strConfigPath);
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);


                objDoc.ReportServerUrl = new Uri(strRptServerURL);
                objDoc.ReportPath = strRptServerFolder + "/Docs/CustomPrelimRpt";
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

                if (!Directory.Exists(strConfigPath + "/TempRpts"))
                    Directory.CreateDirectory(strConfigPath + "/TempRpts");
                strTempPath = strConfigPath + "/TempRpts";
                System.IO.FileStream objFS = new System.IO.FileStream(strConfigPath + "/TempRpts/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);

                objFS.Write(btData, 0, btData.Length);
                objFS.Close();

                objMail.Attachments = 1;
                strTempPath = strTempPath + "/" + strDocName + ".pdf";
                arrAttachments[0] = strTempPath;
                objMail.AttachedFile = arrAttachments;
                arrAttachmentName[0] = strDocName + ".pdf";
                objMail.AttachedFileName = arrAttachmentName;
            }
            catch (Exception expErr)
            {
                errorValue = "Y";
                errorMessage = expErr.Message.Trim();
            }
            finally
            {
                strReturnMsg = null; strCatchMessage = null;
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }
        }
        #endregion

        #region GenerateFinalReport
        private void GenerateFinalReport(string StudyID, string strPatientName, string UserID, ref MailSender objMail)
        {
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];
            string errorMessage = "";
            string errorValue = "N";
            string strTempPath = string.Empty;
            string[] arrAttachments = new string[1];
            string[] arrAttachmentName = new string[1];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(strConfigPath);
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);


                objDoc.ReportServerUrl = new Uri(strRptServerURL);
                objDoc.ReportPath = strRptServerFolder + "/Docs/FinalRpt";
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

                if (!Directory.Exists(strConfigPath + "/TempRpts"))
                    Directory.CreateDirectory(strConfigPath + "/TempRpts");
                strTempPath = strConfigPath + "/TempRpts";
                System.IO.FileStream objFS = new System.IO.FileStream(strConfigPath + "/TempRpts/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);

                objFS.Write(btData, 0, btData.Length);
                objFS.Close();

                objMail.Attachments = 1;
                strTempPath = strTempPath + "/" + strDocName + ".pdf";
                arrAttachments[0] = strTempPath;
                objMail.AttachedFile = arrAttachments;
                arrAttachmentName[0] = strDocName + ".pdf";
                objMail.AttachedFileName = arrAttachmentName;
            }
            catch (Exception expErr)
            {
                errorValue = "Y";
                errorMessage = expErr.Message.Trim();
            }
            finally
            {
                strReturnMsg = null; strCatchMessage = null;
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }
        }
        #endregion

        #region GenerateCustomFinalReport
        private void GenerateCustomFinalReport(string StudyID, string strPatientName, string UserID, ref MailSender objMail)
        {
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];
            string errorMessage = "";
            string errorValue = "N";
            string strTempPath = string.Empty;
            string[] arrAttachments = new string[1];
            string[] arrAttachmentName = new string[1];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(strConfigPath);
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);


                objDoc.ReportServerUrl = new Uri(strRptServerURL);
                objDoc.ReportPath = strRptServerFolder + "/Docs/CustomFinalRpt";
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

                if (!Directory.Exists(strConfigPath + "/TempRpts"))
                    Directory.CreateDirectory(strConfigPath + "/TempRpts");
                strTempPath = strConfigPath + "/TempRpts";
                System.IO.FileStream objFS = new System.IO.FileStream(strConfigPath + "/TempRpts/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);

                objFS.Write(btData, 0, btData.Length);
                objFS.Close();

                objMail.Attachments = 1;
                strTempPath = strTempPath + "/" + strDocName + ".pdf";
                arrAttachments[0] = strTempPath;
                objMail.AttachedFile = arrAttachments;
                arrAttachmentName[0] = strDocName + ".pdf";
                objMail.AttachedFileName = arrAttachmentName;
            }
            catch (Exception expErr)
            {
                errorValue = "Y";
                errorMessage = expErr.Message.Trim();
            }
            finally
            {
                strReturnMsg = null; strCatchMessage = null;
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }
        }
        #endregion

        #region SendSMS
        private bool SendSMS(Guid smsLogID, string smsText, string SendTo, ref string MessageSID, ref string CatchMessage)
        {
            bool bReturn = false;
            TwilioClient.Init(strAcctSID, strAuthToken);

            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00) | SecurityProtocolType.Ssl3;

                var to = new PhoneNumber(SendTo);
                var message = MessageResource.Create(
                    to,
                    from: new PhoneNumber(strSenderNo),
                    body: smsText);

                if (message.Sid.Trim() != string.Empty)
                {
                    bReturn = true;
                    MessageSID = message.Sid;
                }
                else
                {
                    bReturn = false;
                    MessageSID = "";
                }

            }
            catch (Twilio.Exceptions.TwilioException expErr)
            {
                bReturn = false;
                CatchMessage = expErr.Message.Trim() + " - SMS Log ID : " + Convert.ToString(smsLogID);
            }

            return bReturn;
        }
        #endregion

        #region SendFax
        private bool SendFax(Guid LogID, Guid StudyID, string strFaxNo, string strFileName, string ReportType, string CustomReport, ref string ReturnMessage, ref string CatchMessage)
        {
            string strCatchMsg = string.Empty;
            string strRetStat = string.Empty;
            string strFilePath = string.Empty;
            bool bReturn = false;
            InterFaxSender objFax;

            try
            {
                if (ReportType == "P" && CustomReport == "Y")
                {
                    bReturn = PrintGenerateCustomPreliminaryReport(StudyID, strFileName, ref CatchMessage);
                }
                else if (ReportType == "P" && CustomReport == "N")
                {
                    bReturn = PrintPreliminaryReport(StudyID, strFileName, ref CatchMessage);
                }
                else if (ReportType == "F" && CustomReport == "Y")
                {
                    bReturn = PrintGenerateCustomFinalReport(StudyID, strFileName, ref CatchMessage);
                }
                else if (ReportType == "F" && CustomReport == "N")
                {
                    bReturn = PrintFinalReport(StudyID, strFileName, ref CatchMessage);
                }

                if (bReturn)
                {
                    bReturn = false;
                    objFax = new InterFaxSender();
                    strFilePath = strFAXFILEFLDR + "/" + strFileName;

                    objFax.URL(strFAXAPIURL);
                    objFax.Authorize(strFAXAUTHUSERID, strFAXAUTHPWD);
                    objFax.CSID(strFAXCSID);
                    objFax.Reference(strFAXREFTEXT);
                    objFax.ReplyAddress(strFAXREPADDR);
                    objFax.Contact(strFAXCONTACT);
                    objFax.RetriesToPerform(intFAXRETRY);
                    objFax.AddFiles(strFilePath);

                    bReturn = objFax.Send(strFaxNo, ref strRetStat, ref strCatchMsg);

                    if (bReturn)
                    {

                        ReturnMessage = strRetStat;
                        if (File.Exists(strFilePath)) File.Delete(strFilePath);
                    }
                    else
                    {
                        if (strCatchMsg.Trim() != string.Empty)
                            CatchMessage = strCatchMsg.Trim();
                        else
                            ReturnMessage = strRetStat.Trim();
                    }

                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchMailSendingList()=>SendFax() - Error: " + CatchMessage, true);
                }

            }
            catch (Exception expErr)
            {
                bReturn = false;
                CatchMessage = expErr.Message.ToString() + " - Fax Log ID : " + Convert.ToString(LogID); ;
            }
            finally
            {
                objFax = null;

            }
            return bReturn;


        }
        #endregion

        #region Print Report

        #region PrintFinalReport
        private bool PrintFinalReport(Guid StudyID, string FileName, ref string CatchMessage)
        {

            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];
            bool bReturn = false;

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(strConfigPath);
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);

                objDoc.ReportServerUrl = new Uri(strRptServerURL);
                objDoc.ReportPath = strRptServerFolder + "/Docs/FinalRpt";
                strDocName = FileName;

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);



                objParam[0] = new ReportParameter();
                objParam[0].Name = "id";
                objParam[0].Values.Add(Convert.ToString(StudyID));

                objParam[1] = new ReportParameter();
                objParam[1].Name = "user_id";
                objParam[1].Values.Add("00000000-0000-0000-0000-000000000000");

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(strFAXFILEFLDR)) Directory.CreateDirectory(strFAXFILEFLDR);
                System.IO.FileStream objFS = new System.IO.FileStream(strFAXFILEFLDR + "/" + strDocName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();

                bReturn = true;
            }
            catch (Exception expErr)
            {
                bReturn = false;
                CatchMessage = "PrintFinalReport():Exception - " + expErr.Message;
            }
            finally
            {
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }
            return bReturn;
        }
        #endregion

        #region PrintPreliminaryReport
        public bool PrintPreliminaryReport(Guid StudyID, string FileName, ref string CatchMessage)
        {
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];
            bool bReturn = false;

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(strConfigPath);
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);

                objDoc.ReportServerUrl = new Uri(strRptServerURL);
                objDoc.ReportPath = strRptServerFolder + "/Docs/PrelimRpt";
                strDocName = FileName;

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "id";
                objParam[0].Values.Add(Convert.ToString(StudyID));

                objParam[1] = new ReportParameter();
                objParam[1].Name = "user_id";
                objParam[1].Values.Add("00000000-0000-0000-0000-000000000000");

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(strFAXFILEFLDR)) Directory.CreateDirectory(strFAXFILEFLDR);
                System.IO.FileStream objFS = new System.IO.FileStream(strFAXFILEFLDR + "/" + strDocName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();

                bReturn = true;
            }
            catch (Exception expErr)
            {
                bReturn = false;
                CatchMessage = "PrintPreliminaryReport():Exception - " + expErr.Message;
            }
            finally
            {
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }
            return bReturn;
        }
        #endregion

        #region PrintGenerateCustomFinalReport
        private bool PrintGenerateCustomFinalReport(Guid StudyID, string FileName, ref string CatchMessage)
        {
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];
            bool bReturn = false;

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(strConfigPath);
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);

                objDoc.ReportServerUrl = new Uri(strRptServerURL);
                objDoc.ReportPath = strRptServerFolder + "/Docs/CustomFinalRpt";
                strDocName = FileName;

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "id";
                objParam[0].Values.Add(Convert.ToString(StudyID));

                objParam[1] = new ReportParameter();
                objParam[1].Name = "user_id";
                objParam[1].Values.Add("00000000-0000-0000-0000-000000000000");

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(strFAXFILEFLDR)) Directory.CreateDirectory(strFAXFILEFLDR);
                System.IO.FileStream objFS = new System.IO.FileStream(strFAXFILEFLDR + "/" + strDocName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();

                bReturn = true;
            }
            catch (Exception expErr)
            {
                bReturn = false;
                CatchMessage = "PrintGenerateCustomFinalReport():Exception - " + expErr.Message;
            }
            finally
            {
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }
            return bReturn;
        }
        #endregion

        #region PrintGenerateCustomPreliminaryReport
        private bool PrintGenerateCustomPreliminaryReport(Guid StudyID, string FileName, ref string CatchMessage)
        {
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];
            bool bReturn = false;

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(strConfigPath);
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);

                objDoc.ReportServerUrl = new Uri(strRptServerURL);
                objDoc.ReportPath = strRptServerFolder + "/Docs/PrelimRpt";
                strDocName = FileName;

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "id";
                objParam[0].Values.Add(Convert.ToString(StudyID));

                objParam[1] = new ReportParameter();
                objParam[1].Name = "user_id";
                objParam[1].Values.Add("00000000-0000-0000-0000-000000000000");

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(strFAXFILEFLDR)) Directory.CreateDirectory(strFAXFILEFLDR);
                System.IO.FileStream objFS = new System.IO.FileStream(strFAXFILEFLDR + "/" + strDocName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();

                bReturn = true;
            }
            catch (Exception expErr)
            {
                bReturn = false;
                CatchMessage = "PrintGenerateCustomPreliminaryReport():Exception - " + expErr.Message;
            }
            finally
            {
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }
            return bReturn;
        }
        #endregion

        #endregion

        #region FetchUnassignedStudyList
        private void FetchUnassignedStudyList()
        {

            string strResult = string.Empty;
            string strCatchMsg = string.Empty;

            DataSet ds = new DataSet();
            string strSUID = string.Empty;
            string strCatchMessage = string.Empty;
            string strReturnMessage = string.Empty;
            objNotify = new Notification();


            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Fetching unassigned study list...", false);
                if (objNotify.FetchUnassignedStudyList(strConfigPath, ref ds, ref strCatchMsg))
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ds.Tables["StudyList"].Rows.Count.ToString() + " record(s) fetched.", false);

                    #region Mail Sending
                    foreach (DataRow dr in ds.Tables["StudyList"].Rows)
                    {
                        objNotify.STUDY_ID = new Guid(Convert.ToString(dr["id"]));
                        objNotify.STUDY_UID = strSUID = Convert.ToString(dr["study_uid"]).Trim();

                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Creating notification for study - " + strSUID + " not assigned in thresh hold period", false);
                        if (!objNotify.CreateUnassignedStudyNotifications(strConfigPath, ref strCatchMsg))
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "CreateStudyNotifications()  - Core:Exception: " + strCatchMsg, true);
                        }
                    }
                    #endregion


                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchMailSendingList()  - Core::Exception: " + strCatchMsg, true);
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchMailSendingList() - Exception: " + ex.Message, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);

            }
            finally
            {
                objNotify = null; ds.Dispose();
            }


        }
        #endregion

        #region CreateStudyNotifications
        private void CreateStudyNotifications()
        {

            string strCatchMsg = string.Empty;
            int intEmailCount = 0;
            int intSMSCount = 0;
            objNotify = new Notification();


            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Creating study notifications", false);
                if (objNotify.CreateStudyNotifications(strConfigPath, ref intEmailCount, ref intSMSCount, ref strCatchMsg))
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, intEmailCount.ToString() + " Email record(s) created.", false);
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, intSMSCount.ToString() + " SMS record(s) created.", false);
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "CreateStudyNotifications()  - Exception: " + strCatchMsg, true);
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "CreateStudyNotifications() - Exception: " + ex.Message, true);
            }
            finally
            {
                objNotify = null;
            }


        }
        #endregion

        #region creation of invoice sending notification

        #region FetchInvoiceSendingList
        private void FetchInvoiceSendingList()
        {

            string strResult = string.Empty;
            string strCatchMsg = string.Empty;

            DataSet ds = new DataSet();
            Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
            Guid AccountID = new Guid("00000000-0000-0000-0000-000000000000");
            string strAcctCode = string.Empty;
            string strAcctName = string.Empty;
            Guid CycleID = new Guid("00000000-0000-0000-0000-000000000000");
            string strCycleName = string.Empty;
            string strInvoiceNo = string.Empty;
            DateTime dtInvDt = DateTime.Today;
            string strFileName = string.Empty;
            string strReturnMessage = string.Empty;
            string strCatchMessage = string.Empty;
            objNotify = new Notification();


            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Fetching invoice sending list...", false);
                if (objNotify.FetchInvoiceSendingList(strConfigPath, ref ds, ref strCatchMsg))
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ds.Tables["Invoices"].Rows.Count.ToString() + " Invoice record(s) fetched.", false);

                    foreach (DataRow dr in ds.Tables["Invoices"].Rows)
                    {

                        Id = new Guid(Convert.ToString(dr["id"]));
                        AccountID = new Guid(Convert.ToString(dr["billing_account_id"]));
                        strAcctCode = Convert.ToString(dr["billing_account_code"]).Trim();
                        strAcctName = Convert.ToString(dr["billing_account_name"]).Trim();
                        CycleID = new Guid(Convert.ToString(dr["billing_cycle_id"]));
                        strCycleName = Convert.ToString(dr["billing_cycle_name"]).Trim();
                        strInvoiceNo = Convert.ToString(dr["invoice_no"]).Trim();
                        dtInvDt = Convert.ToDateTime(dr["invoice_date"]);


                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Creating notification for Invoice # " + strInvoiceNo + " dated " + dtInvDt.ToString("ddMMMyyyy"), false);

                        if (GenerateBillingAccountInvoice(Convert.ToString(CycleID), strCycleName, Convert.ToString(AccountID), strAcctCode, strAcctName, ref strFileName))
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Invoice # " + strInvoiceNo + " dated " + dtInvDt.ToString("ddMMMyyyy") + " generated", false);

                            objNotify.BILLING_ACCOUNT_ID = AccountID;
                            objNotify.BILLING_CYCLE_ID = CycleID;
                            objNotify.INVOICE_ID = Id;
                            objNotify.FILE_NAME = strFileName.Trim();

                            if (!objNotify.CreateInvoiceNotifications(strConfigPath, ref strReturnMessage, ref strCatchMessage))
                            {
                                if (strReturnMessage.Trim() != string.Empty)
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchInvoiceSendingList()-->CreateInvoiceNotifications() - Error : " + strReturnMessage, true);
                                else
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchInvoiceSendingList()-->CreateInvoiceNotifications() - Exception : " + strCatchMessage, true);
                            }
                        }
                        else
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Failed to generate the invoice " + strInvoiceNo + " dated " + dtInvDt.ToString("ddMMMyyyy") + ":: FetchInvoiceSendingList() - Exception : " + strCatchMessage, true);
                        }
                    }



                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchInvoiceSendingList()  - Core::Exception: " + strCatchMsg, true);
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchInvoiceSendingList() - Exception: " + ex.Message, true);
                //EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);

            }
            finally
            {
                objNotify = null; ds.Dispose();
            }


        }
        #endregion

        #region GenerateBillingAccountInvoice
        private bool GenerateBillingAccountInvoice(string CycleID, string CycleName, string AccountID, string AccountCode, string AccountName, ref string strFileName)
        {
            bool bReturn = false;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(strConfigPath);
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);


                objDoc.ReportServerUrl = new Uri(strRptServerURL);
                objDoc.ReportPath = strRptServerFolder + "/Docs/BillingAccountInvoice";
                AccountName = AccountName.Trim().Replace("'", "");
                AccountName = AccountName.Trim().Replace(" ", "_");
                AccountName = AccountName.Trim().Replace("&", "_");
                AccountName = AccountName.Trim().Replace("\\", "_");
                AccountName = AccountName.Trim().Replace("/", "_");
                strDocName = "BillingAccountInvoice_" + AccountName + "_" + AccountCode + "_" + CycleName.ToUpper().Replace(" ", "");

                if (File.Exists(strMailInvFolder + "/" + strDocName + ".pdf")) File.Delete(strMailInvFolder + "/" + strDocName + ".pdf");


                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "billing_cycle_id";
                objParam[0].Values.Add(CycleID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "billing_account_id";
                objParam[1].Values.Add(AccountID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(strMailInvFolder))
                    Directory.CreateDirectory(strMailInvFolder);

                System.IO.FileStream objFS = new System.IO.FileStream(strMailInvFolder + "/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();
                strFileName = strMailInvFolder + "/" + strDocName + ".pdf";
                bReturn = true;

            }
            catch (Exception expErr)
            {
                bReturn = false;
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GenerateBillingAccountInvoice() - Exception: " + expErr.Message, true);
            }
            finally
            {

                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }
            return bReturn;
        }
        #endregion

        #endregion

        #region ReleaseReports
        private void ReleaseReports()
        {

            string strCatchMsg = string.Empty;
            objNotify = new Notification();


            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Releasing Final Reports", false);
                if (!objNotify.ReleaseReports(strConfigPath, ref strCatchMsg))
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "ReleaseReports()- Core:Exception: " + strCatchMsg, true);
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "ReleaseReports() - Exception: " + ex.Message, true);
            }
            finally
            {
                objNotify = null;
            }


        }
        #endregion
    }
}
