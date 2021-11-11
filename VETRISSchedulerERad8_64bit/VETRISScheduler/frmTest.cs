using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Net;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Configuration;
using System.Security;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Microsoft.Reporting.WebForms;
using VETRISScheduler.Core;
using eRADCls;
using System.Threading.Tasks;

namespace VETRISScheduler
{
    public partial class frmTest : Form
    {
        private static string strFTPHOST = string.Empty;
        private static int intFTPPORT = 0;
        private static string strFTPUSER = string.Empty;
        private static string strFTPPWD = string.Empty;
        private static string strFTPFLDR = string.Empty;
        private static string strFTPDLFLDRTMP = string.Empty;
        private static string strPACSXFERDLFLDR = string.Empty;
        private static string strServerIP = string.Empty;
        private static string strClientIP = string.Empty;
        private static string strWS8SRVUID = string.Empty;
        string[] arrFields = new string[0];
        private static string strDCMRCVRFLDR = string.Empty;
        private static string strFTPSRCFOLDER = string.Empty;

        public frmTest()
        {
            InitializeComponent();
        }


        #region members & variables
        private static int intFreq = 30;
        private static string strURL = string.Empty;
        private static string strConfigPath = "F:\\VetChoice\\VETRIS_SOL_TFS\\VETRIS\\VETRISSchedulerERad8_64bit\\VETRISScheduler";

        #region Mail
        private static string strMailServer = string.Empty;
        private static int intPortNo = 0;
        private static string strSSL = string.Empty;
        private static string strMailUserID = string.Empty;
        private static string strMailUserPwd = string.Empty;
        private static string strMailSender = string.Empty;
        private static string strMailInvFolder = string.Empty;
        private static string strRptServerURL = string.Empty;
        private static string strRptServerFolder = string.Empty;
        #endregion

        #region Writeback
        private static string strXferExePath = string.Empty;
        private static string strXferExeParams = string.Empty;
        private static string strImgtoDCMExePath = string.Empty;
        private static string strDocDCMPath = string.Empty;
        #endregion

        // private static string strSvcName = System.Configuration.ConfigurationManager.AppSettings["ServiceName"];
        Scheduler objCore;
        DataWriteBack objWB;
        CaseStudyUpdate objCU;
        #endregion

        private void frmTest_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strSvcName = string.Empty;
            VETRISScheduler.Core.Scheduler objCore = new Scheduler();
            string strCatchMessage = string.Empty;

            objCore.SERVICE_ID = 1;
            if (objCore.GetServiceDetails(strConfigPath, ref strCatchMessage))
            {

                intFreq = objCore.FREQUENCY;
                strSvcName = objCore.SERVICE_NAME;
                strServerIP = objCore.WS8_SERVER_URL;
                strClientIP = objCore.CLIENT_IP_URL;
                strWS8SRVUID = objCore.WS8_USER_ID;
                arrFields = objCore.FIELD;

                GetData();
            }
        }

        #region GetData
        private void GetData()
        {
            RadWebClass client = new RadWebClass();
            string strResult = string.Empty;
            string sSession = string.Empty;
            string sCatchMsg = string.Empty;
            string sError = string.Empty;
            bool bRet = false;

            try
            {

                //bRet = client.GetSession(strClientIP, strServerIP,strWS8SRVUID, ref sSession, ref sCatchMsg, ref sError);
                if (bRet)
                {
                    bRet = client.UnviewedDataSynch(sSession, strServerIP, arrFields, ref strResult, ref sCatchMsg, ref sError);

                    if (bRet)
                    {
                        strResult = strResult.Trim();
                        textBox1.Text = strResult;
                        PopulateData(strResult);
                    }
                    else
                    {
                        textBox1.Text = "UnviewedDataSynch() - Error: " + sCatchMsg + "[" + sError + "]";

                    }
                }
                else
                {
                    textBox1.Text = "GetSession() - Error: " + sCatchMsg + "[" + sError + "]";

                }
            }
            catch (Exception ex)
            {
                textBox1.Text = "GetData() - Exception: " + ex.Message;
            }
            finally
            {
                objCore = null;
            }


        }
        #endregion

        #region PopulateData
        private void PopulateData(string strResult)
        {
            DataSet ds = new DataSet();
            DataTable dtbl = new DataTable();
            NewDataSynch objDS = new NewDataSynch();
            StringBuilder sb = new StringBuilder();

            try
            {
                System.IO.StringReader xmlSR = new System.IO.StringReader(strResult);
                ds.ReadXml(xmlSR);
                foreach (DataRow dr in ds.Tables["Field"].Rows)
                {
                    for (int i = 0; i < ds.Tables["Field"].Columns.Count; i++)
                    {
                        sb.AppendLine(ds.Tables["Field"].Columns[i].ColumnName + "-->" + Convert.ToString(dr["value"]));
                        Application.DoEvents();
                    }
                    sb.AppendLine("");
                    textBox1.Text += sb.ToString();
                    sb.Clear();
                }

                dtbl = CreateTable(ds);
                textBox1.Text = dtbl.Rows.Count.ToString() + " records downloaded";


                //if (dtbl.Rows.Count > 0) SynchData(dtbl);

            }
            catch (Exception ex)
            {
                textBox1.Text = "PopulateData() - Exception: " + ex.Message;

            }
            finally
            {
                objCore = null;
                objDS = null;
                ds.Dispose();
            }


        }
        #endregion


        #region CreateTable
        private DataTable CreateTable(DataSet ds)
        {
            DataTable dtbl = new DataTable();
            int intRecord = 0;
            int intCol = 0;
            dtbl.Columns.Add("study_uid", System.Type.GetType("System.String"));
            dtbl.Columns.Add("study_date", System.Type.GetType("System.DateTime"));
            dtbl.Columns.Add("received_date", System.Type.GetType("System.DateTime"));
            dtbl.Columns.Add("accession_no", System.Type.GetType("System.String"));
            dtbl.Columns.Add("patient_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("patient_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("patient_dob", System.Type.GetType("System.DateTime"));
            dtbl.Columns.Add("patient_age", System.Type.GetType("System.String"));
            dtbl.Columns.Add("patient_sex", System.Type.GetType("System.String"));
            dtbl.Columns.Add("patient_weight", System.Type.GetType("System.Decimal"));
            dtbl.Columns.Add("species", System.Type.GetType("System.String"));
            dtbl.Columns.Add("breed", System.Type.GetType("System.String"));
            dtbl.Columns.Add("owner", System.Type.GetType("System.String"));
            dtbl.Columns.Add("modality", System.Type.GetType("System.String"));
            dtbl.Columns.Add("body_part", System.Type.GetType("System.String"));
            dtbl.Columns.Add("reason", System.Type.GetType("System.String"));
            dtbl.Columns.Add("institution_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("referring_physician", System.Type.GetType("System.String"));
            dtbl.Columns.Add("manufacturer_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("manufacturer_model_no", System.Type.GetType("System.String"));
            dtbl.Columns.Add("device_serial_no", System.Type.GetType("System.String"));
            dtbl.Columns.Add("sex_neutered", System.Type.GetType("System.String"));
            dtbl.Columns.Add("img_count", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("study_desc", System.Type.GetType("System.String"));
            dtbl.Columns.Add("modality_ae_title", System.Type.GetType("System.String"));
            dtbl.Columns.Add("priority_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("object_count", System.Type.GetType("System.Int32"));
            dtbl.TableName = "Details";



            //DataRow drt = dtbl.NewRow();
            //foreach (DataRow dr in dst.Tables["Field"].Rows)
            //{
            //    if (Convert.ToInt32(dr["Record_Id"].ToString()) == intRecord)
            //    {
            //        drt[intCol] = dr["Value"].ToString();
            //        intCol++;

            //    }
            //    else
            //    {
            //        dtbl.Rows.Add(drt);
            //        intRecord++;
            //        intCol = 0;
            //        drt = dtbl.NewRow();
            //        drt[intCol] = dr["Value"].ToString();
            //        intCol++;
            //    }
            //}

            return dtbl;
        }
        #endregion

        #region SynchData
        private void SynchData(DataTable dtbl)
        {
            NewDataSynch objCore = new NewDataSynch();
            string strCatchMsg = string.Empty;
            string strReturnMsg = string.Empty;
            int intCount = 0;
            StringBuilder sb = new StringBuilder();

            try
            {

                if (objCore.SaveNewSynchedData(strConfigPath, 1, "", dtbl, ref intCount, ref strReturnMsg, ref strCatchMsg))
                {
                    sb.AppendLine(intCount.ToString() + " record(s) synched successfully");

                }
                else
                {
                    if (strCatchMsg.Trim() != "")
                    {
                        sb.AppendLine("SynchData() - Exception: " + strCatchMsg.Trim());
                    }
                    else
                    {
                        sb.AppendLine("SynchData() - Error: " + strReturnMsg.Trim());
                    }
                }

            }
            catch (Exception ex)
            {
                ;

            }
            finally
            {
                objCore = null;

            }


        }
        #endregion




        #region IgnoreBadCertificates
        public static void IgnoreBadCertificates()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
        }
        #endregion

        #region AcceptAllCertifications
        private static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        #endregion


        #region notification
        private void button2_Click(object sender, EventArgs e)
        {
            doNotifyProcess();
        }
        

        #region doNotifyProcess
        private void doNotifyProcess()
        {
            string strCatchMessage = string.Empty;
            string strSvcName = string.Empty;
            try
            {


                objCore = new Scheduler();
                objCore.SERVICE_ID = 4;


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
                        FetchMailSendingList();

                    }
                    else
                        textBox1.Text = "Core::GetServiceDetails - Error : " + strCatchMessage;

                }
                catch (Exception ex)
                {
                    textBox1.Text = "doProcess() - Error : " + ex.Message;

                }

                objCore = null;


            }
            catch (Exception expErr)
            {
                textBox1.Text = "doProcess() - Error : " + expErr.Message;
            }
            finally
            { objCore = null; }
        }
        #endregion

        #region FetchMailSendingList
        private void FetchMailSendingList()
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
            string strCatchMessage = string.Empty;
            string strEmailType = string.Empty;
            string strIsCustomRpt = string.Empty;
            string strPtName = string.Empty;
            Notification objNotify = new Notification();


            try
            {

                textBox1.Text += "Fetching mail sending list...";

                if (objNotify.FetchNotificationSendingList(strConfigPath, ref ds, ref strCatchMsg))
                {


                    textBox1.Text += ds.Tables[0].Rows.Count.ToString() + " record(s) fetched.";
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

                        textBox1.Text += "Sending mail to " + strRecepientAddress;



                        if (CreateMailAndSend(Id, strNotifyText, strSubject, strRecepientAddress, strCCAddress, strAttachment, strMailAcctUserID, strMailAcctPwd, strEmailType, strIsCustomRpt, StudyID, strPtName, ref strCatchMessage))
                        {

                            objNotify.EMAIL_LOG_ID = Id;
                            #region Suspend
                            //                if (!objNotify.UpdateMailSendingStatus(strConfigPath, 4, "", ref strCatchMessage))
                            //                {
                            //                    textBox1.Text += "UpdateMailSendingStatus() - Exception : " + strCatchMessage;

                            //                }
                            #endregion
                        }

                    }
                }
                else
                {
                    textBox1.Text += "FetchMailSendingList()  - Exception: " + strCatchMsg;
                }
            }
            catch (Exception ex)
            {
                textBox1.Text += "FetchMailSendingList() - Exception: " + ex.Message;

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

                textBox1.Text += sb1.ToString();

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

                bReturn = objMail.SendMail(ref CatchMessage);

                if (bReturn)
                {
                    textBox1.Text += "Mail Sent Successfully.";
                }
                else
                {
                    textBox1.Text += "CreateMailAndSend()- " + Convert.ToString(mailLogID) + " - Error: " + CatchMessage;
                }
            }
            catch (Exception ex)
            {
                textBox1.Text += "CreateMailAndSend()- " + Convert.ToString(mailLogID) + " - Exception: " + ex.Message;

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
            string[] arrAttachments = new string[0];
            string[] arrAttachmentName = new string[0];
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
            string[] arrAttachments = new string[0];
            string[] arrAttachmentName = new string[0];
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
            string[] arrAttachments = new string[0];
            string[] arrAttachmentName = new string[0];
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

        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            doWBProcess();
        }

        #region Writeback

        #region doWBProcess
        private void doWBProcess()
        {
            string strCatchMessage = string.Empty;
            StringBuilder sb = new StringBuilder();

            try
            {

                while (true)
                {
                    objCore = new Scheduler();
                    objCore.SERVICE_ID = 2;


                    try
                    {

                        if (objCore.GetServiceDetails(strConfigPath, ref strCatchMessage))
                        {

                            intFreq = objCore.FREQUENCY;
                            //strSvcName = objCore.SERVICE_NAME;
                            strURL = objCore.URL;
                            strXferExePath = objCore.PACS_TRANSFER_EXE_PATH;
                            strXferExeParams = objCore.PACS_TRANSFER_EXE_PARAMS;
                            strImgtoDCMExePath = objCore.IMAGE_TO_DCM_EXE_PATH;
                            strDocDCMPath = objCore.DOCUMENT_AND_DCM_PATH;

                            doWriteBack();

                        }
                        else
                            textBox1.Text = sb.AppendLine("Core::GetServiceDetails - Error : " + strCatchMessage).ToString();


                    }
                    catch (Exception ex)
                    {
                        textBox1.Text = sb.AppendLine("doWBProcess() - Error: " + ex.Message).ToString();

                    }
                    objCore = null;
                }
            }
            catch (Exception expErr)
            {
                textBox1.Text = sb.AppendLine("doWBProcess() - Exception: " + expErr.Message).ToString();
            }
            finally
            { objCore = null; }
        }
        #endregion

        #region doWriteBack
        private void doWriteBack()
        {

            string strResult = string.Empty;
            string strCatchMsg = string.Empty;

            DataSet ds = new DataSet();
            Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
            string strStudyUID = string.Empty;
            string strWBURL = string.Empty;
            string strCatchMessage = string.Empty;
            bool bRet = false;
            int idx = 0;
            string[] arrStudyFields = new string[4];
            string strField = string.Empty;
            string strStudyType = string.Empty;
            string strFound = string.Empty;
            StringBuilder sb = new StringBuilder();
            objWB = new DataWriteBack();


            try
            {
                textBox1.Text += sb.AppendLine("Fetching write back list...").ToString();
                if (objWB.FetchWriteBackList(strConfigPath, ref ds, ref strCatchMsg))
                {
                    textBox1.Text += sb.AppendLine(ds.Tables["Details"].Rows.Count.ToString() + " record(s) fetched.").ToString();
                    strWBURL = strURL;

                    if (ds.Tables["StudyTypeTags"].Rows.Count > 0)
                    {
                        arrStudyFields = new string[ds.Tables["StudyTypeTags"].Rows.Count];

                        foreach (DataRow dr in ds.Tables["StudyTypeTags"].Rows)
                        {
                            arrStudyFields[idx] = Convert.ToString(dr["field_code"]);
                            idx = idx + 1;
                        }
                    }



                    foreach (DataRow dr in ds.Tables["Details"].Rows)
                    {
                        Id = new Guid(Convert.ToString(dr["id"]));
                        strStudyUID = Convert.ToString(dr["study_uid"]).Trim();
                        strWBURL = strURL;

                        #region Build Up Write Back URL
                        strWBURL = strWBURL.Replace("cStudy=", "cStudy=" + strStudyUID);
                        strWBURL = strWBURL.Replace("qe_ACCN=", "qe_ACCN=" + Convert.ToString(dr["accession_no"]).Trim()).Replace(" ", "%20");
                        strWBURL = strWBURL.Replace("qe_PAID=", "qe_PAID=" + Convert.ToString(dr["patient_id"]).Trim()).Replace(" ", "%20");
                        strWBURL = strWBURL.Replace("qe_PANM=", "qe_PANM=" + Convert.ToString(dr["patient_name"]).Trim()).Replace(" ", "^");
                        strWBURL = strWBURL.Replace("qe_PSEX=", "qe_PSEX=" + Convert.ToString(dr["patient_sex"]).Trim()).Replace(" ", "%20");
                        strWBURL = strWBURL.Replace("qe_9PSN=", "qe_9PSN=" + Convert.ToString(dr["patient_sex_neutered"]).Trim()).Replace(" ", "%20");
                        strWBURL = strWBURL.Replace("qe_9PWT=", "qe_9PWT=" + Convert.ToString(dr["patient_weight"]));
                        strWBURL = strWBURL.Replace("qe_PDOB=", "qe_PDOB=" + Convert.ToDateTime(dr["patient_dob_accepted"]).ToString("yyyyMMdd") + "_000000");
                        if (Convert.ToInt32(dr["patient_age_accepted"]) == 0)
                            strWBURL = strWBURL.Replace("qe_PAGE=", "qe_PAGE=000Y");
                        else if (Convert.ToInt32(dr["patient_age_accepted"]) <= 99)
                            strWBURL = strWBURL.Replace("qe_PAGE=", "qe_PAGE=0" + Convert.ToString(Convert.ToInt32(dr["patient_age_accepted"])) + "Y");
                        else
                            strWBURL = strWBURL.Replace("qe_PAGE=", "qe_PAGE=" + Convert.ToString(Convert.ToInt32(dr["patient_age_accepted"])) + "Y");
                        strWBURL = strWBURL.Replace("qe_9SPC=", "qe_9SPC=" + Convert.ToString(dr["species_name"]).Trim()).Replace(" ", "%20");
                        strWBURL = strWBURL.Replace("qe_9BRD=", "qe_9BRD=" + Convert.ToString(dr["breed_name"]).Trim()).Replace(" ", "%20");
                        strWBURL = strWBURL.Replace("qe_9RSP=", "qe_9RSP=" + Convert.ToString(dr["owner_name"]).Trim()).Replace(" ", "^");
                        strWBURL = strWBURL.Replace("qe_PALL=", "qe_PALL=" + Convert.ToString(dr["modality_name"]).Trim()).Replace(" ", "%20");
                        strWBURL = strWBURL.Replace("qe_BDYP=", "qe_BDYP=" + Convert.ToString(dr["body_part_name"]).Trim()).Replace(" ", "%20");
                        strWBURL = strWBURL.Replace("qe_PMAL=", "qe_PMAL=" + Convert.ToString(dr["reason_accepted"]).Trim()).Replace(" ", "%20");
                        //strWBURL = strWBURL.Replace("qe_NIMG=", "qe_NIMG=" + Convert.ToString(dr["img_count"]));
                        strWBURL = strWBURL.Replace("qe_INSN=", "qe_INSN=" + Convert.ToString(dr["institution_name"]).Trim()).Replace(" ", "%20");
                        strWBURL = strWBURL.Replace("qe_PHRF=", "qe_PHRF=" + Convert.ToString(dr["physician_name"]).Trim()).Replace(" ", "^");
                        strWBURL = strWBURL.Replace("qe_STAT=", "qe_STAT=50");

                        DataView dvST = new DataView(ds.Tables["StudyTypes"]);
                        dvST.RowFilter = "study_hdr_id='" + Convert.ToString(Id) + "'";
                        if (dvST.ToTable().Rows.Count > 0)
                        {
                            string[] arr = new string[dvST.ToTable().Rows.Count];
                            idx = 0;

                            foreach (DataRow dr1 in dvST.ToTable().Rows)
                            {
                                strField = Convert.ToString(dr1["write_back_tag"]).Trim();
                                strStudyType = Convert.ToString(dr1["study_type_name"]).Trim();
                                strWBURL = strWBURL.Replace("qe_" + strField + "=", "qe_" + strField + "=" + strStudyType);
                                arr[idx] = strField;
                                idx = idx + 1;
                            }

                            for (int i = 0; i < arrStudyFields.Length; i++)
                            {
                                strFound = "N";
                                for (int j = 0; j < arr.Length; j++)
                                {
                                    if (arr[j] == arrStudyFields[i])
                                    {
                                        strFound = "Y";
                                        break;
                                    }
                                }
                                if (strFound == "N")
                                {
                                    strWBURL = strWBURL.Replace("&qe_" + arrStudyFields[i] + "=", "");
                                }
                            }
                        }
                        else
                        {
                            strWBURL = strWBURL.Replace("&qe_DSCR=", "");
                            strWBURL = strWBURL.Replace("&qe_UDF4=", "");
                            strWBURL = strWBURL.Replace("&qe_UDF7=", "");
                            strWBURL = strWBURL.Replace("&qe_UDF9=", "");
                        }
                        dvST.Dispose();
                        #endregion

                        textBox1.Text += sb.AppendLine("Write Back URL : " + strWBURL).ToString();


                        #region Write back and update status
                        //if (WriteBackData(Id, strStudyUID, strWBURL))
                        //{
                        DataView dv = new DataView(ds.Tables["Documents"]);
                        dv.RowFilter = "study_hdr_id='" + Convert.ToString(Id) + "'";
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            bRet = UploadDocuments(Id, strStudyUID, dv.ToTable());
                        }
                        else
                            bRet = true;

                        if (bRet)
                        {
                            textBox1.Text += sb.AppendLine("Updating status for Study UID : " + strStudyUID).ToString();

                            objCU = new CaseStudyUpdate();
                            objCU.STUDY_ID = Id;
                            objCU.STUDY_UID = strStudyUID;
                            objCU.STATUS_ID = 50;

                            if (!objCU.UpdateStatus(strConfigPath, 2, "Write Back", ref strCatchMessage))
                            {
                                textBox1.Text += sb.AppendLine("WriteBackData() - " + strCatchMessage).ToString();

                            }

                            objCU = null;

                        }

                        dv.Dispose();
                    }
                        #endregion

                }
                //}
                //else
                //{
                //    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doWriteBack() -->FetchWriteBackList()  - Exception: " + strCatchMsg, true);
                //    EventLog.WriteEntry(strSvcName, strCatchMsg, EventLogEntryType.Error);
                //}
            }
            catch (Exception ex)
            {
                textBox1.Text += sb.AppendLine("doWriteBack() - Exception: " + ex.Message).ToString();
            }
            finally
            {
                objWB = null; ds.Dispose();
            }


        }
        #endregion

        #region UploadDocuments
        private bool UploadDocuments(Guid Id, string StudyUID, DataTable dtbl)
        {
            bool bReturn = false;
            StringBuilder sb = new StringBuilder();
            string strFileName = string.Empty;
            string[] arrSrcFiles = new string[0];
            string[] arrTgtFiles = new string[0];
            string strProcSrcPath = strDocDCMPath.Replace("/", "\\") + "\\Docs";
            string strProcTgtPath = strDocDCMPath.Replace("/", "\\") + "\\DCM";
            string strSrcPath = strDocDCMPath.Replace("/", "\\") + "\\Docs\\" + StudyUID;
            string strTgtPath = strDocDCMPath.Replace("/", "\\") + "\\DCM\\" + StudyUID;
            int exitCode;

            if (!Directory.Exists(strSrcPath)) Directory.CreateDirectory(strSrcPath);
            if (!Directory.Exists(strTgtPath)) Directory.CreateDirectory(strTgtPath);

            //if (Directory.Exists(strSrcPath)) CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Folder - " + strSrcPath + " created", false);
            //else CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Folder - " + strSrcPath + " not created", false);
            //if (Directory.Exists(strTgtPath)) CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Folder - " + strTgtPath + " created", false);
            //else CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Folder - " + strTgtPath + " created", false);

            #region Convert to file from bytes
            try
            {
                foreach (DataRow dr in dtbl.Rows)
                {
                    strFileName = Convert.ToString(dr["document_link"]);
                    SetFile((byte[])dr["document_file"], Convert.ToString(dr["document_link"]).Trim(), strSrcPath);
                }

                bReturn = true;
            }
            catch (Exception ex)
            {
                textBox1.Text += sb.AppendLine("UploadDocuments() - Conversion To file - Exception: " + ex.Message).ToString();
                bReturn = false;
            }
            #endregion

            if (bReturn)
            {
                bReturn = false;

                #region Convert to DCM
                //Process ProcImgToDcm = new Process();

                try
                {

                    if (File.Exists(strImgtoDCMExePath))
                    {
                        arrSrcFiles = Directory.GetFiles(strSrcPath, "*.*", SearchOption.AllDirectories);

                        #region OLD
                        //ProcImgToDcm.StartInfo.UseShellExecute = false;
                        //ProcImgToDcm.StartInfo.FileName = strImgtoDCMExePath;
                        //ProcImgToDcm.StartInfo.Arguments = StudyUID + "±" + strProcSrcPath + "±" + strTgtPath;
                        //ProcImgToDcm.StartInfo.RedirectStandardOutput = true;
                        //ProcImgToDcm.Start();

                        //if (ProcImgToDcm.HasExited)
                        //{

                        //}
                        #endregion

                        for (int i = 0; i < arrSrcFiles.Length; i++)
                        {
                            if (!Directory.Exists(strTgtPath))
                            {
                                Directory.CreateDirectory(strTgtPath);
                            }

                            ProcessStartInfo start = new ProcessStartInfo();
                            start.Arguments = arrSrcFiles[i] + " " + strTgtPath + "\\" + " " + StudyUID;
                            start.FileName = strImgtoDCMExePath;
                            start.UseShellExecute = true;
                            start.CreateNoWindow = true;
                            start.WindowStyle = ProcessWindowStyle.Minimized;
                            start.WorkingDirectory = strImgtoDCMExePath.Substring(0, strImgtoDCMExePath.LastIndexOf("/")).Replace("/", "\\");

                            textBox1.Text += sb.AppendLine(arrSrcFiles[i]).ToString();

                            Process proc = new Process();
                            proc.StartInfo.RedirectStandardError = true;
                            proc.StartInfo.RedirectStandardOutput = true;
                            proc = Process.Start(start);
                            proc.WaitForExit();

                            if (proc.HasExited)
                            {

                                exitCode = proc.ExitCode;

                                if (exitCode != 0)
                                    textBox1.Text += sb.AppendLine("UploadDocuments() - Conversion To DCM for Study UID : " + StudyUID + " :: File Name : " + arrSrcFiles[i] + " failed").ToString();

                                //proc.Kill();
                            }
                            //proc.Dispose();
                        }

                        arrTgtFiles = Directory.GetFiles(strTgtPath, "*.*", SearchOption.AllDirectories);

                        if (arrSrcFiles.Length == arrTgtFiles.Length)
                        {
                            textBox1.Text += sb.AppendLine("UploadDocuments() - Conversion To DCM for Study UID : " + StudyUID + " done successfully").ToString();
                            bReturn = true;
                        }
                        else
                        {
                            textBox1.Text += sb.AppendLine("UploadDocuments() - Conversion To DCM for Study UID : " + StudyUID + " failed").ToString();
                            bReturn = false;
                        }


                    }
                    else
                    {
                        textBox1.Text += sb.AppendLine("UploadDocuments() - Conversion To DCM for Study UID : " + StudyUID + " - .exe not found").ToString();
                        bReturn = false;
                    }


                }
                catch (Exception ex)
                {
                    textBox1.Text += sb.AppendLine("UploadDocuments() - Conversion To DCM for Study UID : " + StudyUID + " - Exception: " + ex.Message).ToString();
                    bReturn = false;
                }
                // ProcImgToDcm.Dispose();
                #endregion
            }

            //if (bReturn)
            //{
            //    bReturn = false;

            //    #region Send DCM to PACS
            //    Process ProcXfer = new Process();

            //    try
            //    {
            //        if (File.Exists(strXferExePath))
            //        {
            //            strXferExePath = strXferExePath.Replace("/", "\\");
            //            strTgtPath = strTgtPath.Replace("/", "\\");

            //            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Transfer of documents initiated - " + strXferExePath + " " + strXferExeParams + " " + strTgtPath + "/*.dcm", false);


            //            ProcXfer.StartInfo.UseShellExecute = false;
            //            ProcXfer.StartInfo.FileName = strXferExePath;
            //            ProcXfer.StartInfo.Arguments = strXferExeParams + " " + strTgtPath + "\\*.dcm";
            //            ProcXfer.StartInfo.RedirectStandardOutput = true;
            //            ProcXfer.Start();

            //            bReturn = true;
            //        }
            //        else
            //        {
            //            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - DCM transfer to PACS for Study UID : " + StudyUID + " - .exe not found", true);
            //            bReturn = false;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - DCM transfer to PACS for Study UID : " + StudyUID + " - Exception: " + ex.Message, true);
            //        EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
            //        bReturn = false;
            //    }

            //    ProcXfer.Dispose();
            //    #endregion
            //}
            return bReturn;
        }
        #endregion

        #region SetFile
        private void SetFile(byte[] DocData, string strFileName, string strPath)
        {
            string strFilePath = strPath + "/" + strFileName;
            using (FileStream fs = new FileStream(strFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(DocData, 0, DocData.Length);
                fs.Flush();
                fs.Close();
            }

        }
        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            string[] arrParentFolders = Directory.GetDirectories("G:/VetChoice/DOCDCM");
            string strParFldr = string.Empty;
            string[] arrDCMFolder = new string[0]; string[] arrDCMFiles = new string[0];
            string[] arrDocFolder = new string[0]; string[] arrDocFiles = new string[0];
            string[] arrImgFolder = new string[0]; string[] arrImgFiles = new string[0];

            try
            {
                for (int i = 0; i < arrParentFolders.Length; i++)
                {
                    strParFldr = arrParentFolders[i].Substring(arrParentFolders[i].LastIndexOf("\\") + 1);

                    switch (strParFldr)
                    {
                        case "DCM":
                            arrDCMFolder = Directory.GetDirectories(arrParentFolders[i]);
                            for (int j = 0; j < arrDCMFolder.Length; j++)
                            {

                                TimeSpan t = DateTime.Now - Directory.GetLastWriteTime(arrDCMFolder[j]);
                                if (t.TotalDays >= 2)
                                {
                                    arrDCMFiles = Directory.GetFiles(arrDCMFolder[j]);
                                    for (int k = 0; k < arrDCMFiles.Length; k++)
                                    {
                                        File.Delete(arrDCMFiles[k]);
                                    }

                                    Directory.Delete(arrDCMFolder[j]);
                                }
                            }
                            break;
                        case "Docs":
                            arrDocFolder = Directory.GetDirectories(arrParentFolders[i]);
                            for (int j = 0; j < arrDocFolder.Length; j++)
                            {

                                TimeSpan t = DateTime.Now - Directory.GetLastWriteTime(arrDocFolder[j]);

                                if (t.TotalDays >= 2)
                                {
                                    arrDocFiles = Directory.GetFiles(arrDocFolder[j]);
                                    for (int k = 0; k < arrDocFiles.Length; k++)
                                    {
                                        File.Delete(arrDocFiles[k]);
                                    }

                                    Directory.Delete(arrDocFolder[j]);
                                }
                            }
                            break;
                        case "Img":
                            arrImgFolder = Directory.GetDirectories(arrParentFolders[i]);
                            for (int j = 0; j < arrImgFolder.Length; j++)
                            {

                                TimeSpan t = DateTime.Now - Directory.GetLastWriteTime(arrImgFolder[j]);
                                if (t.TotalDays >= 2)
                                {
                                    arrImgFiles = Directory.GetFiles(arrImgFolder[j]);
                                    for (int k = 0; k < arrImgFiles.Length; k++)
                                    {
                                        File.Delete(arrImgFiles[k]);
                                    }

                                    Directory.Delete(arrImgFolder[j]);
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion


        #region FTP Download

        private void button5_Click(object sender, EventArgs e)
        {
            //strFTPHOST = "rad365tech.com";
            //strFTPUSER = "pavel@rad365tech.com";
            //strFTPPWD = "Rad365@2019";
            strFTPHOST = "38.84.128.79";
            strFTPUSER = "vetris";
            strFTPPWD = "vetris2019";
            strFTPFLDR = "DICOMFilesDownload";
            strFTPDLFLDRTMP = "E:/VetChoice/FTP_DOWNLOAD_TEMP";
            //strPACSXFERDLFLDR = objCore.FOLDER_FOR_PACS_TRANSFER;

            //NetworkCredential credentials = new NetworkCredential(strFTPUSER, strFTPPWD);
            //string strURL = "ftp://" + strFTPHOST;
            doDownloadFiles();
            doDecompressFiles();

        }

        #region doDownloadFiles
        private void doDownloadFiles()
        {
            string[] arrfiles = GetFileList();

            if (arrfiles != null)
            {
                if (arrfiles.Length > 0)
                {
                    foreach (string strfile in arrfiles)
                    {
                        if ((strfile.Trim() != ".") && (strfile.Trim() != ".."))
                            Download(strfile);
                    }
                }
            }
        }
        #endregion

        #region GetFileList
        public string[] GetFileList()
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + strFTPHOST + "/" + strFTPFLDR + "/"));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(strFTPUSER, strFTPPWD);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.Proxy = null;
                reqFTP.KeepAlive = false;
                reqFTP.UsePassive = false;
                response = reqFTP.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                // to remove the trailing '\n'
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                downloadFiles = null;
                return downloadFiles;
            }
        }
        #endregion

        #region Download
        private void Download(string strFileName)
        {
            try
            {
                string uri = "ftp://" + strFTPHOST + "/" + strFTPFLDR + "/" + strFileName;
                Uri serverUri = new Uri(uri);
                if (serverUri.Scheme != Uri.UriSchemeFtp)
                {
                    return;
                }
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + strFTPHOST + "/" + strFTPFLDR + "/" + strFileName));
                reqFTP.Credentials = new NetworkCredential(strFTPUSER, strFTPPWD);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Proxy = null;
                reqFTP.UsePassive = false;

                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream responseStream = response.GetResponseStream();
                FileStream writeStream = new FileStream(strFTPDLFLDRTMP + "\\" + strFileName, FileMode.Create);
                int Length = 2048;
                Byte[] buffer = new Byte[Length];
                int bytesRead = responseStream.Read(buffer, 0, Length);
                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, Length);
                }
                writeStream.Close();
                response.Close();

                DeleteFtpFile(strFileName);
            }

            catch (Exception ex)
            {

                textBox1.Text = ex.Message;
            }
        }
        #endregion

        #region DeleteFtpFile
        private void DeleteFtpFile(string strFileName)
        {
            string strResponse = string.Empty;

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + strFTPHOST + "/" + strFTPFLDR + "/" + strFileName);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(strFTPUSER, strFTPPWD);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    strResponse = response.StatusDescription;
                }


            }

            catch (Exception ex)
            {

                textBox1.Text = ex.Message;
            }
        }
        #endregion

        #region doDecompressFiles
        private void doDecompressFiles()
        {
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string strFileName = string.Empty;
            string strDirName = string.Empty;
            string strTargetPath = string.Empty;
            string strExtractPath = string.Empty;

            try
            {

                arrFiles = Directory.GetFiles(strFTPDLFLDRTMP);

                foreach (string strFile in arrFiles)
                {
                    pathElements = strFile.Split('\\');
                    strFileName = pathElements[(pathElements.Length - 1)];
                    strDirName = strFileName.Substring(0, strFileName.LastIndexOf("."));

                    strExtractPath = strFTPDLFLDRTMP + "\\" + strDirName;
                    //strExtractPath = @".\" + strDirName;


                    ZipFile.ExtractToDirectory(strFile, strExtractPath);
                    UpdateDownloadedFilesRecords(strExtractPath);



                }

            }
            catch (Exception expErr)
            {
                textBox1.Text = expErr.Message;
            }
        }
        #endregion

        #region UpdateDownloadedFilesRecords
        private void UpdateDownloadedFilesRecords(string strFolder)
        {
            string strCatchMessage = string.Empty;
            string strRetMessage = string.Empty;
            string strSUID = string.Empty;
            DateTime dtStudy = DateTime.Now;
            string strInstName = string.Empty;
            string strPatientName = string.Empty;
            string strPatientFname = string.Empty;
            string strPatientLname = string.Empty;
            int intFileCount = 0;
            string strDt = "0000-00-00";
            string[] arrDt = new string[0];
            string[] arrTime = new string[0];
            string[] arrDateTime = new string[0];

            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string strFileName = string.Empty;

            List<string> arrSUID = new List<string>();
            string[] arr = new string[0];

            DicomDecoder dd = new DicomDecoder();
            FTPPACSSynch objFP = new FTPPACSSynch();


            try
            {
                arrFiles = Directory.GetFiles(strFolder);
                intFileCount = arrFiles.Length;

                try
                {
                    foreach (string strFile in arrFiles)
                    {
                        if ((strInstName.Trim() == string.Empty) || (strDt.Trim() == "0000-00-00") || (strPatientName.Trim() == string.Empty))
                        {
                            pathElements = strFile.Split('\\');
                            strFileName = pathElements[(pathElements.Length - 1)];
                            dd.DicomFileName = strFile;
                            List<string> str = dd.dicomInfo;

                            arr = new string[7];
                            arr = GetallTags(str);
                            strSUID = arr[0].Trim();
                            strPatientName = arr[3].Trim();

                            if (strPatientName != string.Empty)
                            {
                                if (strPatientName.Contains(' '))
                                {
                                    strPatientFname = strPatientName.Substring(0, strPatientName.LastIndexOf(' '));
                                    strPatientLname = strPatientName.Substring(strPatientName.LastIndexOf(' '), (strPatientName.Length - strPatientName.LastIndexOf(' ')));
                                }
                                else
                                {
                                    strPatientFname = strPatientName;
                                    strPatientLname = string.Empty;
                                }
                            }



                            strDt = arr[4].Trim();
                            if ((arr[4].Trim() == "0000-00-00") || (arr[4].Trim() == string.Empty)) dtStudy = DateTime.Now;
                            else
                            {
                                arrDateTime = arr[4].Trim().Split(' ');
                                arrDt = arrDateTime[0].Split('-');
                                arrTime = arrDateTime[1].Split(':');

                                dtStudy = new DateTime(Convert.ToInt32(arrDt[0]),
                                                      Convert.ToInt32(arrDt[1]),
                                                      Convert.ToInt32(arrDt[2]),
                                                      Convert.ToInt32(arrTime[0]),
                                                      Convert.ToInt32(arrTime[1]),
                                                      Convert.ToInt32(arrTime[2]));
                            }

                            strInstName = arr[5].Trim();

                            try
                            {

                                objFP.STUDY_UID = strSUID.Trim();
                                objFP.STUDY_DATE = dtStudy;
                                objFP.INSTITUTION_NAME = strInstName.Trim();
                                objFP.PATIENT_FIRST_NAME = strPatientFname.Trim();
                                objFP.PATIENT_LAST_NAME = strPatientLname.Trim();
                                objFP.FILE_COUNT = intFileCount;

                                //if (!objFP.SaveData(strConfigPath, "VETRIS FTP & PACS Synch Service", ref strRetMessage, ref strCatchMessage))
                                //{
                                //    if (strCatchMessage.Trim() != string.Empty)
                                //        textBox1.Text = strCatchMessage;
                                //    else
                                //        textBox1.Text = strRetMessage;
                                //}
                            }
                            catch (Exception expErr)
                            {
                                // CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCompressFiles()=>UpdateDownloadedFilesRecords():: DBUpdate - Exception: " + expErr.Message, true);
                                textBox1.Text = expErr.Message;
                            }

                        }
                        else
                            break;


                    }
                }
                catch (Exception expErr)
                {
                    textBox1.Text = expErr.Message;
                }
            }
            catch (Exception expErr)
            {
                textBox1.Text = expErr.Message;
            }
            finally
            {
                objFP = null; dd = null;
            }
        }
        #endregion

        #region GetStudyUID
        private string GetStudyUID(List<string> str)
        {

            string UserCaseID = string.Empty;
            string s1, s4, s5, s11, s12;

            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                if ((s11.ToUpper() == "0020") && (s12.ToUpper() == "000D"))
                {
                    UserCaseID = s5.Replace("\0", "");
                    break;
                }

            }
            return UserCaseID;

        }
        #endregion





        private void button6_Click(object sender, EventArgs e)
        {
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string strFileName = string.Empty;
            string strDirName = string.Empty;
            string strExtractPath = string.Empty;
            strFTPDLFLDRTMP = "E:/VetChoice/FTP_DOWNLOAD_TEMP";

            try
            {

                arrFiles = Directory.GetFiles(strFTPDLFLDRTMP);

                foreach (string strFile in arrFiles)
                {
                    pathElements = strFile.Split('\\');
                    strFileName = pathElements[(pathElements.Length - 1)];

                    using (ZipArchive archive = ZipFile.OpenRead(strFile))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            //entry.ExtractToFile(Path.Combine(strFTPDLFLDRTMP, entry.Name));
                            entry.ExtractToFile(strFTPDLFLDRTMP + "\\" + entry.FullName);
                            //UpdateDownloadedFilesRecords(entry.Name);
                        }


                    }


                }

            }
            catch (Exception expErr)
            {
                textBox1.Text = expErr.Message;
            }
        }

        #endregion

        private void button7_Click(object sender, EventArgs e)
        {
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string strFileName = string.Empty;
            string strExtractPath = string.Empty;

            string strCatchMessage = string.Empty;
            objCore = new Scheduler();

            try
            {
                strFTPDLFLDRTMP = "E:/VetChoice/FTP_DOWNLOAD_TEMP";
                arrFiles = Directory.GetFiles(strFTPDLFLDRTMP, "*.zip");

                foreach (string strFile in arrFiles)
                {
                    pathElements = strFile.Split('\\');
                    strFileName = pathElements[(pathElements.Length - 1)];


                    using (ZipArchive archive = ZipFile.OpenRead(strFile))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            //entry.ExtractToFile(Path.Combine(strFTPDLFLDRTMP, entry.Name));
                            strExtractPath = strFTPDLFLDRTMP + "\\" + entry.FullName;

                        }
                    }

                    if (File.Exists(strExtractPath))
                    {
                        try
                        {
                            File.Delete(strFile);

                        }
                        catch (Exception expErr)
                        {
                            textBox1.Text = expErr.Message;
                        }
                    }

                }

            }
            catch (Exception expErr)
            {
                textBox1.Text = expErr.Message;
            }
        }

        #region Update DICOM Tags
        private void button8_Click(object sender, EventArgs e)
        {
            //string strSUID = "1.3.6.1.4.1.29565.1.4.2203314842.5688.1564491031.996";
            //string strInstCode = "00001";
            //string strInstName = "Coastal Veterinary Clinic";
            //string strPatientID = "1234";
            //string strPFName = "Dick";
            //string strPLName = "Quickthighs";
            //DateTime dtSTudy = Convert.ToDateTime("02Aug2019 15:06:34");
            //string strFileName = "00001_RAD365_CT.1.3.6.1.4.1.29565.1.4.2203314842.5688.1564491031.996.6.5.4.3.0.dcm";
            //DicomAttributeCollection _baseDataSet = new DicomAttributeCollection();
            //_baseDataSet[DicomTags.InstitutionName].SetStringValue(strInstName);
            //_baseDataSet[DicomTags.PatientId].SetStringValue(strPatientID);
            //_baseDataSet[DicomTags.PatientsName].SetStringValue(String.Format("{0}^{1}^^", strPLName, strPFName));
            //_baseDataSet[DicomTags.StudyDate].SetDateTime(0, dtSTudy);
            //DateTime time = DateTime.MinValue.Add(new TimeSpan(dtSTudy.Hour, dtSTudy.Minute, dtSTudy.Second));
            //_baseDataSet[DicomTags.StudyTime].SetDateTime(0, time);
            //DicomFile dicomFile = new DicomFile("E:\\VetChoice\\FTP_DOWNLOAD_TEMP\\" + strFileName, new DicomAttributeCollection(), _baseDataSet.Copy());
            //dicomFile.Save();
            //UpdateDicomTags(dicomFile, strInstName, strPatientID, strPFName, strPLName, dtSTudy, _baseDataSet);
        }




        //#region UpdateDicomTags
        //private bool UpdateDicomTags(DicomFile dicomFile, string InstitutionName, string PatientID, string PatientFirstName, string PatientLastName, DateTime StudyDate, DicomAttributeCollection _baseDataSet)
        //{
        //    bool bRet = false;

        //    //DicomFile dicomFile = new DicomFile("E:\\VetChoice\\FTP_DOWNLOAD_TEMP\\" + FileName, new DicomAttributeCollection(), _baseDataSet.Copy());
        //  // DicomFile dicomFile = new DicomFile("E:\\VetChoice\\FTP_DOWNLOAD_TEMP\\" + FileName);
        //    try
        //    {

        //        dicomFile.att
        //        //Institution
        //        dicomFile.DataSet[DicomTags.InstitutionName].SetStringValue(InstitutionName);
        //        ////Patient
        //        //dicomFile.DataSet[DicomTags.PatientId].SetStringValue(PatientID);
        //        //dicomFile.DataSet[DicomTags.PatientsName].SetStringValue(String.Format("{0}^{1}^^", PatientLastName, PatientFirstName));
        //        ////Study
        //        //dicomFile.DataSet[DicomTags.StudyDate].SetDateTime(0,StudyDate);
        //        //dicomFile.DataSet[DicomTags.StudyTime].SetDateTime(0, time);


        //        //dicomFile.Save("E:\\VetChoice\\FTP_DOWNLOAD_TEMP\\" + FileName, DicomWriteOptions.Default);
        //        //Platform.CheckForNullReference(dicomFile, "file");

        //       // string orginalInstNameInFile = dicomFile.DataSet[DicomTags.InstitutionName].ToString();
        //        //dicomFile.DataSet[DicomTags.InstitutionName].SetString(0, InstitutionName);

        //        //dicomFile.Save("E:\\VetChoice\\FTP_DOWNLOAD_TEMP\\" + FileName, DicomWriteOptions.Default);
        //        //dicomFile.Save("E:\\VetChoice\\FTP_DOWNLOAD_TEMP\\" + dicomFile, DicomWriteOptions.Default);
        //        dicomFile.Save();
        //        bRet = true;
        //    }
        //    catch (Exception expErr)
        //    {
        //        bRet = false;
        //        textBox1.Text = expErr.Message;
        //    }
        //    return bRet;
        //}
        //#endregion

        #endregion

        #region Transfer DICOM Files
        private void button9_Click(object sender, EventArgs e)
        {
            bool bRet = false;
            string strProcOutput = string.Empty;
            string strProcError = string.Empty;
            string strProcMsg = string.Empty;

            string strXFEREXEPATH = "E:/VetChoice/EXES/SendFileToPACS/storescu.exe";
            string strXFEREXEPARMS = "-v  -aet KPServer -aec PBUILDER -xy 172.21.247.65 104";
            string strXFEREXEPARMSJPGLL = "-v  -R -aet KPServer -aec PBUILDER -xs 172.21.247.65 104";
            string strFolder = "E:/VetChoice/FTP_DOWNLOAD_TEMP";
            string strFile = "00106_Hope_Crossing_Vet_Sammie0001";
            string strRetMsg = string.Empty;

            try
            {
                textBox1.Text = "File sending started with " + strXFEREXEPATH + " " + strXFEREXEPARMS + " " + strFolder + "\\" + strFile;

                Process ProcXfer = new Process();
                ProcXfer.StartInfo.UseShellExecute = false;
                ProcXfer.StartInfo.FileName = strXFEREXEPATH;
                ProcXfer.StartInfo.Arguments = strXFEREXEPARMS + " " + strFolder + "\\" + strFile;
                ProcXfer.StartInfo.RedirectStandardOutput = true;
                ProcXfer.StartInfo.RedirectStandardError = true;
                ProcXfer.Start();

                strProcOutput = ProcXfer.StandardOutput.ReadToEnd();
                strProcError = ProcXfer.StandardError.ReadToEnd();
                strProcMsg = strProcOutput.Trim();

                //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "TransferFileToPacs():Message: " + strProcMsg, false);
                // if (strProcMsg.ToUpper().Contains("RECEIVED STORE RESPONSE (SUCCESS)"))

                if (strProcMsg.ToUpper().Contains("[STATUS=SUCCESS]"))
                {
                    strRetMsg = strProcMsg;
                    bRet = true;
                }
                else
                {
                    textBox1.Text = "File sending started with " + strXFEREXEPATH + " " + strXFEREXEPARMSJPGLL + " " + strFolder + "\\" + strFile;
                    Process ProcXferAlt = new Process();
                    ProcXferAlt.StartInfo.UseShellExecute = false;
                    ProcXferAlt.StartInfo.FileName = strXFEREXEPATH;
                    ProcXferAlt.StartInfo.Arguments = strXFEREXEPARMSJPGLL + " " + strFolder + "\\" + strFile;
                    ProcXferAlt.StartInfo.RedirectStandardOutput = true;
                    ProcXferAlt.StartInfo.RedirectStandardError = true;
                    ProcXferAlt.Start();

                    strProcOutput = ProcXferAlt.StandardOutput.ReadToEnd();
                    strProcError = ProcXferAlt.StandardError.ReadToEnd();
                    strProcMsg = strProcOutput.Trim();


                    if (strProcMsg.ToUpper().Contains("[STATUS=SUCCESS]"))
                    {
                        bRet = true;
                    }

                    strRetMsg = strProcMsg;
                }


            }
            catch (Exception ex)
            {
                bRet = false;
                textBox1.Text = ex.Message;
            }


        }
        #endregion


        #region Modify DCM Tag
        private void button10_Click(object sender, EventArgs e)
        {
            //string strExePath = "E:/VetChoice/EXES/SendFileToPACS/dcmodify.exe";
            string strCatchMessage = string.Empty;
            bool bFmt = false;
            DicomDecoder dd = new DicomDecoder();
            //string strDCMPath = "E:/VetChoice/PACS_ARCHIVE/00177_Veterinary_Ultrasound_Services_1ugjcO_191122230002.dcm";
            //string strDCMPath ="E:/VetChoice/PACS_ARCHIVE/00177_Veterinary Ultrasound Services_1.2.276.0.7230010.3.1.2.16.020191125230429.2/00177_Veterinary_Ultrasound_Services_Qb1VoX_191125230427.dcm";
            string strDCMPath = "E:/VetChoice/FTP_DOWNLOAD_TEMP/00177_Veterinary_Ultrasound_Services_sSxvCh_191130004846.dcm";
            dd.DicomFileName = strDCMPath;
            List<string> str = dd.dicomInfo;
            DataSet dsTags = new DataSet();
            FTPPACSSynch obj = new FTPPACSSynch();

            try
            {
                obj.INSTITUTION_ID = new Guid("9305A40D-706B-47B9-8E2F-A4422E462053"); ;
                if (obj.FetchTagsToFormat(strConfigPath, ref dsTags, ref strCatchMessage))
                {
                    if (dsTags.Tables["TagList"].Rows.Count > 0) bFmt = FormatDCMTags(dsTags.Tables["TagList"], str, strDCMPath);
                }
                else
                    textBox1.Text = strCatchMessage;
            }
            catch (Exception expErr)
            {
                textBox1.Text = expErr.Message;
            }
            finally
            {
                dsTags.Dispose();
                obj = null;
            }

            // FormatDCMTags(str, strDCMPath);



        }

        #region FormatDCMTags
        private bool FormatDCMTags(DataTable dtbl, List<string> str, string strDCMPath)
        {
            bool bRet = true;
            string strTagValue = string.Empty;
            string strTagID = string.Empty;
            string strGroupID = string.Empty;
            string strElementID = string.Empty;
            string strJunk = string.Empty;
            string strProcOutput = string.Empty;
            string strProcError = string.Empty;

            try
            {
                foreach (DataRow dr in dtbl.Rows)
                {

                    strGroupID = Convert.ToString(dr["group_id"]).Trim();
                    strElementID = Convert.ToString(dr["element_id"]).Trim();
                    strTagValue = Convert.ToString(dr["default_value"]).Trim();
                    strJunk = Convert.ToString(dr["junk_characters"]).Trim();

                    strTagID = "(" + strGroupID.ToUpper() + "," + strElementID.ToUpper() + ")";

                    if (strTagValue.Trim() == string.Empty)
                    {
                        if (strJunk.Trim() != string.Empty)
                        {
                            strTagValue = GetTagValue(str, strGroupID, strElementID);
                            if (strJunk.Contains(","))
                            {
                                string[] arrJunk = strJunk.Split(',');
                                for (int i = 0; i < arrJunk.Length; i++)
                                {
                                    if ((arrJunk[i].Trim() != string.Empty) || (arrJunk[i] != null))
                                    {
                                        strTagValue = strTagValue.Replace(arrJunk[i], "");
                                    }
                                }
                            }
                        }
                    }


                    Process ProcFormat = new Process();
                    ProcFormat.StartInfo.UseShellExecute = false;
                    ProcFormat.StartInfo.FileName = "E:\\VetChoice\\EXES\\SendFileToPACS\\dcmodify.exe";
                    ProcFormat.StartInfo.Arguments = "-i \"" + strTagID + "=" + strTagValue + "\"" + " " + strDCMPath;
                    ProcFormat.StartInfo.RedirectStandardOutput = true;
                    ProcFormat.StartInfo.RedirectStandardError = true;
                    ProcFormat.Start();
                    strProcOutput = ProcFormat.StandardOutput.ReadToEnd();
                    strProcError = ProcFormat.StandardError.ReadToEnd();

                    if (strProcOutput.Trim() != string.Empty)
                    {
                        textBox1.Text = strProcOutput.Trim();

                    }
                    //}



                }

            }
            catch (Exception ex)
            {
                bRet = false;
                textBox1.Text = ex.Message;
            }

            return bRet;
        }
        #endregion

        #region FormatDCMTags - Suspended
        //private bool FormatDCMTags(List<string> str, string strDCMPath)
        //{
        //    bool bRet = true;
        //    string strTagValue = string.Empty; string strTagID = string.Empty;
        //    string strProcOutput = string.Empty;
        //    string strProcError = string.Empty;
        //    string strErr = string.Empty;
        //    string strDesc = string.Empty;

        //    try
        //    {
        //        for (int i = 0; i < str.Count; ++i)
        //        {
        //            string s1, s4, s5, s11, s12;
        //            s1 = str[i];

        //            ExtractStrings(s1, out s4, out s5, out s11, out s12);
        //            if ((s11.ToUpper() != "0000") && (s11.ToUpper() != "0002"))
        //            {
        //                if ((s11.ToUpper() == "0028"))
        //                    strDesc = s4;
        //                strTagID = "(" + s11.ToUpper() + "," + s12.ToUpper() + ")";
        //                strTagValue = s5.Replace("\0", "");
        //                strTagValue = strTagValue.Replace("\t", "");
        //                strTagValue = strTagValue.Replace("\n", "");
        //                strTagValue = strTagValue.Trim();



        //                //if (strTagValue == string.Empty) strTagValue = "BLANK";

        //                Process ProcFormat = new Process();
        //                ProcFormat.StartInfo.UseShellExecute = false;
        //                ProcFormat.StartInfo.FileName = "E:\\VetChoice\\EXES\\SendFileToPACS\\dcmodify.exe";
        //                ProcFormat.StartInfo.Arguments = "-i \"" + strTagID + "=" + strTagValue + "\"" + " " + strDCMPath;
        //                ProcFormat.StartInfo.RedirectStandardOutput = true;
        //                ProcFormat.StartInfo.RedirectStandardError = true;
        //                ProcFormat.Start();
        //                strProcOutput = ProcFormat.StandardOutput.ReadToEnd();
        //                strProcError = ProcFormat.StandardError.ReadToEnd();

        //                if (strProcOutput.Trim() != string.Empty)
        //                {
        //                    strErr = strProcOutput.Trim();
        //                }

        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        bRet = false;

        //    }

        //    return bRet;
        //}
        #endregion

        #region GetTagValue
        private string GetTagValue(List<string> str, string strGroupID, string strElementID)
        {

            string strTagValue = string.Empty;
            string s1, s4, s5, s11, s12;

            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                if ((s11.ToUpper() == strGroupID) && (s12.ToUpper() == strElementID))
                {
                    strTagValue = s5.Replace("\0", "");
                    break;
                }

            }
            return strTagValue;

        }
        #endregion

        #endregion



        #region button11_Click
        private void button11_Click(object sender, EventArgs e)
        {
            //string strFileName = "E:\\VetChoice\\PACS_ARCHIVE\\00177_Veterinary_Ultrasound_Services_1ugjcO_191122230002.dcm.bak";
            string strExt = string.Empty;
            string[] arrFiles = new string[0];
            strFTPDLFLDRTMP = "E:/VetChoice/FTP_DOWNLOAD_TEMP";

            arrFiles = Directory.GetFiles(strFTPDLFLDRTMP);

            for (int i = 0; i < arrFiles.Length; i++)
            {

                strExt = arrFiles[i].Substring(arrFiles[i].Length - 4, 4).ToUpper();
                if (strExt.ToUpper() == ".BAK")
                {
                    if (File.Exists(arrFiles[i])) File.Delete(arrFiles[i]);
                }

            }
        }
        #endregion

        private void button12_Click(object sender, EventArgs e)
        {
            string strFolder = strFTPDLFLDRTMP;
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string[] fileElements = new string[0];
            string strFileName = string.Empty;
            string strSID = string.Empty;
            string strIsMissing = string.Empty;
            string strCatchMessage = string.Empty;
            FTPPACSSynch objFP = new FTPPACSSynch();


            try
            {
                // CoreCommon.doLog(strConfigPath, 7, "TEST", "Checking missing session files", false);
                arrFiles = Directory.GetFiles("E:\\VetChoice\\FTP_DOWNLOAD_TEMP");

                foreach (string strFile in arrFiles)
                {
                    pathElements = strFile.Split('\\');
                    strFileName = pathElements[(pathElements.Length - 1)];

                    if (strFileName.Contains('_'))
                    {
                        fileElements = strFileName.Split('_');
                        if (fileElements.Length > 2)
                        {
                            strSID = fileElements[1].Trim();


                            if (CoreCommon.IsDicomFile(strFile))
                            {
                                #region DICOM files
                                strIsMissing = "N";
                                objFP.IMPORT_SESSION_ID = strSID;
                                objFP.FILE_NAME = strFileName.Trim();
                                objFP.FILE_TYPE = "D";

                                //if (objFP.CheckMissingSessionFiles(strConfigPath, ref strIsMissing, ref strCatchMessage))
                                //{
                                //    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCheckMissingSessionFiles() - File : " + strFileName + " - Update DB", false);
                                //    UpdateDownloadedFilesRecords(strFileName);
                                //}
                                //else
                                //{
                                //    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCheckMissingSessionFiles() - File : " + strFileName + " - Error : " + strCatchMessage, true);
                                //}
                                #endregion
                            }
                            else if ((MIMEAssistant.GetMIMEType(strFile) == "image/jpeg") || (MIMEAssistant.GetMIMEType(strFile) == "image/gif") || (MIMEAssistant.GetMIMEType(strFile) == "image/png") || (MIMEAssistant.GetMIMEType(strFile) == "image/bmp"))
                            {
                                #region ImageFiles

                                strIsMissing = "N";
                                objFP.IMPORT_SESSION_ID = strSID;
                                objFP.FILE_NAME = strFileName.Trim();
                                objFP.FILE_TYPE = "I";

                                //if (objFP.CheckMissingSessionFiles(strConfigPath, ref strIsMissing, ref strCatchMessage))
                                //{
                                //    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCheckMissingSessionFiles() - File : " + strFileName + " - Update DB", false);
                                //    UpdateDownloadedFilesRecords(strFileName);
                                //}
                                //else
                                //{
                                //    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCheckMissingSessionFiles() - File : " + strFileName + " - Error : " + strCatchMessage, true);
                                //}

                                #endregion
                            }
                        }
                    }

                }
            }
            catch (Exception expErr)
            {
                ;
                //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doCheckMissingSessionFiles() - Exception: " + expErr.Message, true);
            }
            finally
            {
                objFP = null;
            }
        }

        #region Net Speed
        private void button13_Click(object sender, EventArgs e)
        {
            double dblSpeed = 0;
            System.Net.WebClient wc = new System.Net.WebClient();

            //DateTime Variable To Store Download Start Time.
            DateTime dt1 = DateTime.Now;

            //Number Of Bytes Downloaded Are Stored In ‘data’
            //byte[] data = wc.DownloadData("http://google.com");
            //byte[] data = wc.DownloadData("https://client.vcradiology.com/vetris.api/");
            byte[] data = wc.DownloadData("ftp://vetris:vetris2019@38.84.128.79:21/DICOMFilesDownload");

            //DateTime Variable To Store Download End Time.
            DateTime dt2 = DateTime.Now;

            //To Calculate Speed in Kb Divide Value Of data by 1024 And Then by End Time Subtract Start Time To Know Download Per Second.
            dblSpeed = Math.Round((data.Length / 1024) / (dt2 - dt1).TotalSeconds, 2);

            textBox1.Text = dblSpeed.ToString();
        }
        #endregion

        #region Import Report Data
        private void button14_Click(object sender, EventArgs e)
        {
            //string strResult = string.Empty;
            //string[] arrRecord = new string[0];
            //StringBuilder sb = new StringBuilder();
            //GetReportData(ref strResult);
            //arrRecord = strResult.Split('\t');

            //for (int i = 0; i < arrRecord.Length; i++)
            //{
            //    sb.AppendLine((i + 1).ToString() + ". " + arrRecord[i]);
            //}

            //textBox1.Text = sb.ToString();
            ////textBox1.Text = strResult;

            string strAppvDate = string.Empty;
            string strTblDate = string.Empty;
            strAppvDate = "20200211 094400";
            strTblDate = strAppvDate.Substring(0, 4) + "-" + strAppvDate.Substring(4, 2) + "-" + strAppvDate.Substring(6, 2) + " " + strAppvDate.Substring(9, 2) + ":" + strAppvDate.Substring(11, 2) + ":" + strAppvDate.Substring(13, 2);
            textBox1.Text = strTblDate;
        }

        #region GetReportData
        private void GetReportData(ref string strResult)
        {
            WebClient client = new WebClient();



            try
            {

                IgnoreBadCertificates();
                //strURL = "https://pacs.vcradiology.com/iface/report.jsp?cStudy=1.3.6.1.4.1.19179.9000000000000.20200204.185530.9004349";
                //strURL = "https://172.21.247.65/iface/report.jsp?cStudy=1.3.6.1.4.1.19179.9000000000000.20200204.185530.9004349";
                strURL = "https://172.21.247.65/iface/report.jsp?cStudy=1.2.826.0.1.3680043.2.1074.8245908163300415080095092410533136835";
                byte[] data = client.DownloadData(strURL);
                strResult = System.Text.Encoding.Default.GetString(data);
                strResult = strResult.Replace("### Begin_Table's_Content ###", "");
                strResult = strResult.Replace("### End_Table's_Content ###", "");
                strResult = strResult.Substring(1, strResult.IndexOf("#USERID:") - 1);
                strResult = strResult.Replace("\r", "");
                strResult = strResult.Trim();

                //if (strResult != string.Empty) PopulateData(strResult);
            }
            catch (Exception ex)
            {
                ;
            }
            finally
            {
                objCore = null;
            }


        }
        #endregion


        #endregion

        #region Get Radiologist
        private void button15_Click(object sender, EventArgs e)
        {
            string strSvcName = string.Empty;
            VETRISScheduler.Core.Scheduler objCore = new Scheduler();
            string strCatchMessage = string.Empty;

            objCore.SERVICE_ID = 3;
            if (objCore.GetServiceDetails(strConfigPath, ref strCatchMessage))
            {

                intFreq = objCore.FREQUENCY;
                strSvcName = objCore.SERVICE_NAME;
                strServerIP = objCore.WS8_SERVER_URL;
                strClientIP = objCore.CLIENT_IP_URL;
                strWS8SRVUID = objCore.WS8_USER_ID;
                arrFields = objCore.FIELD;

                //GetRadiologists("1.2.826.0.1.3680043.8.226.601777.1351310275.746426067.853651411");
                //GetRadiologists("2.16.840.1.114440.1.2.5.1866.6295.20200305.154438410.59336339");
                GetRadiologists("1.2.826.0.1.3680043.2.876.10743.3.5.1.20200305120941.0.4");
            }
        }
        #endregion

        #region GetRadiologists
        private void GetRadiologists(string StudyUID)
        {
            RadWebClass client = new RadWebClass();
            string strResult = string.Empty;
            string sSession = string.Empty;
            string sCatchMsg = string.Empty;
            string sError = string.Empty;
            bool bRet = false;
            string PrelimRadilogist = string.Empty;
            string FinalRadilogist = string.Empty;
            DateTime dtRptApprove = DateTime.Now;
            DataSet ds = new DataSet();

            try
            {

                //bRet = client.GetSession(strClientIP, strServerIP,strWS8SRVUID, ref sSession, ref sCatchMsg, ref sError);
                if (bRet)
                {
                    bRet = client.GetStudyData(sSession, strServerIP, StudyUID, ref strResult, ref sCatchMsg, ref sError);

                    if (bRet)
                    {
                        strResult = strResult.Trim();
                        System.IO.StringReader xmlSR = new System.IO.StringReader(strResult);
                        ds.ReadXml(xmlSR);
                    }
                    else
                    {
                        textBox1.Text = sCatchMsg + "[" + sError + "]";

                    }
                }
                else
                {
                    textBox1.Text = sCatchMsg + "[" + sError + "]";

                }
            }
            catch (Exception ex)
            {
                textBox1.Text = ex.Message;

            }
            finally
            {
                objCore = null; ds.Dispose();
            }
        }
        #endregion

        #region Get Study Data

        #region btnGetStudyData_Click
        private void btnGetStudyData_Click(object sender, EventArgs e)
        {
            RadWebClass client = new RadWebClass();
            string strResult = string.Empty;
            string sCatchMsg = string.Empty;
            string sError = string.Empty;
            string strColID = string.Empty;
            string strValue = string.Empty;
            //string StudyUID = "1.2.826.0.1.3680043.2.93.1.4.363610615.28705.1600444416.68300";
            string StudyUID = "1.3.76.2.2.2.5028.4.3481.20200917193029";
            string strWS8SessionID = string.Empty;
            string strWS8SRVIP = "http://pacs.vcradiology.com/epws/API";
            bool bRet = false;
            string[] arrValues = new string[0];


            bRet = client.GetStudyData(strWS8SessionID, strWS8SRVIP, StudyUID, ref strResult, ref sCatchMsg, ref sError);

            if (bRet)
            {

                DataSet ds = new DataSet();
                strResult = strResult.Trim();
                System.IO.StringReader xmlSR = new System.IO.StringReader(strResult);
                ds.ReadXml(xmlSR);



                if (ds.Tables.Contains("Field"))
                {
                    foreach (DataRow dr in ds.Tables["Field"].Rows)
                    {
                        textBox1.Text += Convert.ToString(dr["Colid"]) + " : " + Convert.ToString(dr["Value"]) + "\r\n";
                    }

                }

                textBox1.Text += "===========================================================\r\n";

                DataView dv = new DataView(ds.Tables["Field"]);
                dv.RowFilter = "Colid ='SRFT'";
                textBox1.Text += "SRFT\r\n";
                textBox1.Text += "----\r\n";
                foreach (DataRow dr in ds.Tables["Field"].Rows)
                {
                    textBox1.Text += Convert.ToString(dr["Record_Id"]) + " : " + Convert.ToString(dr["Value"]) + "\r\n";
                }
                dv.Dispose();

                textBox1.Text += "===========================================================\r\n";
                dv = new DataView(ds.Tables["Field"]);
                dv.RowFilter = "Colid ='SRFH'";
                textBox1.Text += "SRFH\r\n";
                textBox1.Text += "----\r\n";
                foreach (DataRow dr in ds.Tables["Field"].Rows)
                {
                    textBox1.Text += Convert.ToString(dr["Record_Id"]) + " : " + Convert.ToString(dr["Value"]) + "\r\n";
                }
                dv.Dispose();

                xmlSR = null;
                ds.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            else
            {
                textBox1.Text += sError.Trim();
            }

        }

        #endregion


        #endregion

        #region Update Status
        private void btnUpdateStat_Click(object sender, EventArgs e)
        {
            string strSUURL = "http://pacs.vcradiology.com/epws/API";
            Guid Id = new Guid("D8347251-6315-4665-8552-28CFF3E71EE0");
            string strStudyUID = "1.3.6.1.4.1.11157.2002478011572111.1594224180.31";
            string[] arrValues = new string[16];
            int intStatusID = 0;
            string strRadiologist = string.Empty;
            bool bStudyExists = false;
            string strCatchMsg = string.Empty;
            DataTable dtbl = new DataTable(); dtbl = CreateAddendumTable();
            CaseStudyUpdate objSU = new CaseStudyUpdate();
            string strCatchMessage = string.Empty;
            arrFields = new string[16];

            #region fields
            arrFields[0] = "STAT";
            arrFields[1] = "TRAD";
            arrFields[2] = "NIMG";
            arrFields[3] = "INSN";
            arrFields[4] = "MFCT";
            arrFields[5] = "MFMD";
            arrFields[6] = "PSAE";
            arrFields[7] = "NOBJ";
            arrFields[8] = "UDF3";
            arrFields[9] = "PALL";
            arrFields[10] = "IAPN";
            arrFields[11] = "ITAN";
            arrFields[12] = "IADT";
            arrFields[13] = "IRDT";
            arrFields[14] = "SRFT";
            arrFields[15] = "";
            #endregion


            if (GetStatus(strStudyUID, ref arrValues, ref bStudyExists, ref dtbl, ref strCatchMsg))
            {
                if (bStudyExists)
                {

                    intStatusID = Convert.ToInt32(arrValues[0]);

                    #region Update Status
                    if (intStatusID > -1)
                    {
                        //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Value Count " + arrValues.Length.ToString(), false);
                        objSU = new CaseStudyUpdate();
                        objSU.STUDY_ID = Id;
                        objSU.STUDY_UID = strStudyUID;
                        objSU.STATUS_ID = intStatusID;
                        objSU.RADIOLOGIST = arrValues[1].Trim();
                        objSU.IMAGE_COUNT = Convert.ToInt32(arrValues[2]);
                        objSU.INSTITUTION_NAME = arrValues[3].Trim();
                        objSU.MANUFACTURER = arrValues[4].Trim();
                        objSU.MODEL = arrValues[5].Trim();
                        objSU.MODALITY_AE_TITLE = arrValues[6].Trim();
                        objSU.OBJECT_COUNT = Convert.ToInt32(arrValues[7]);
                        objSU.SERVICE_CODES = arrValues[8].Trim();
                        objSU.MODALITY = arrValues[9].Trim();
                        objSU.FINAL_RADIOLOGIST = arrValues[10].Trim();
                        objSU.PRELIMINARY_RADIOLOGIST = arrValues[11].Trim();
                        objSU.REPORT_APPROVAL_DATE = Convert.ToDateTime(arrValues[12]);
                        objSU.REPORT_RECORDING_DATE = Convert.ToDateTime(arrValues[13]);
                        //objSU.REPORT_TEXT_HTML = arrValues[14].Trim();
                        //objSU.REPORT_TEXT = arrValues[15].Trim();
                        objSU.REPORT_TEXT_HTML = string.Empty;
                        objSU.REPORT_TEXT = arrValues[14].Trim();
                        objSU.ADDILTIONAL_FIELD = arrFields[arrFields.Length - 1];
                        objSU.ADDILTIONAL_FIELD_VALUE = arrValues[arrValues.Length - 1].Trim();
                        objSU.PACS_IMAGE_VIEW_URL = "https://pacs.vcradiology.com/iface/webViewer.jsp?SYUI=#V1&cUser=#V2&cPasswd=#V3";
                        objSU.PACS_REPORT_VIEW_URL = "https://pacs.vcradiology.com/iface/reportview.jsp?ACCN=#V1&cUser=#V2&cPasswd=#V3";
                        objSU.PACS_STUDY_VIEW_URL = "";
                        if (!objSU.UpdateStatus(strConfigPath, 3, "", ref strCatchMessage))
                        {
                            CoreCommon.doLog(strConfigPath, 3, "", "FetchCaseList() - Update status for Study UID : " + strStudyUID + " - " + strCatchMessage, true);
                        }
                        else
                        {
                            //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Status of Study UID " + strStudyUID + " updated successfully.", false);
                            if ((intStatusID == 80) || (intStatusID == 100))
                            {

                                UpdateReport(intStatusID, dtbl, objSU);
                            }
                            else
                            {
                                dtbl.Dispose();
                                dtbl = null;
                            }
                        }

                        arrValues = new string[0];
                        dtbl.Dispose();
                        dtbl = null;
                    }
                    else
                    {
                        arrValues = new string[0];
                    }
                    #endregion

                }
            }
        }
        #endregion

        #region GetStatus
        private bool GetStatus(string StudyUID, ref string[] arrValues, ref bool StudyExists, ref DataTable dtbl, ref string CatchMessage)
        {
            RadWebClass client = new RadWebClass();
            string strResult = string.Empty;
            string sCatchMsg = string.Empty;
            string sError = string.Empty;
            string strColID = string.Empty;
            string strValue = string.Empty;
            int intRecordID = 0;
            bool bRet = false;


            try
            {



                bRet = true;

                if (bRet)
                {

                    bRet = client.GetStudyData("", "http://pacs.vcradiology.com/epws/API", StudyUID, ref strResult, ref sCatchMsg, ref sError);

                    if (bRet)
                    {

                        DataSet ds = new DataSet();
                        strResult = strResult.Trim();
                        System.IO.StringReader xmlSR = new System.IO.StringReader(strResult);
                        ds.ReadXml(xmlSR);


                        if (ds.Tables.Contains("Field"))
                        {
                            StudyExists = true;


                            #region get status and fields
                            for (int i = 0; i < arrFields.Length; i++)
                            {
                                arrValues[i] = string.Empty;
                                strColID = arrFields[i];
                                //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Column ID : " + strColID + " , Index : " + i.ToString(), false);
                                if (strColID.Trim() != string.Empty)
                                {
                                    DataView dv = new DataView(ds.Tables["Field"]);
                                    dv.RowFilter = "Colid ='" + strColID + "'";


                                    switch (strColID)
                                    {

                                        case "NIMG":
                                        case "NOBJ":
                                            if (dv.ToTable().Rows.Count > 0)
                                            {
                                                if (dv.ToTable().Rows[0]["Value"] != DBNull.Value)
                                                {
                                                    if (IsInteger(Convert.ToString(dv.ToTable().Rows[0]["Value"])))
                                                        arrValues[i] = Convert.ToString(dv.ToTable().Rows[0]["Value"]);
                                                    else
                                                        arrValues[i] = "0";
                                                }
                                                else
                                                    arrValues[i] = "0";
                                            }
                                            else
                                                arrValues[i] = "0";
                                            break;
                                        case "IADT":
                                        case "IRDT":
                                            //case "PDOB":
                                            if (dv.ToTable().Rows.Count > 0)
                                            {
                                                if (dv.ToTable().Rows[0]["Value"] != DBNull.Value)
                                                {
                                                    if (IsDate(Convert.ToString(dv.ToTable().Rows[0]["Value"])))
                                                        arrValues[i] = Convert.ToString(dv.ToTable().Rows[0]["Value"]);
                                                    else
                                                        arrValues[i] = "01jan1900";
                                                }
                                                else if ((dv.ToTable().Rows[0]["Value"] == DBNull.Value) || (Convert.ToString(dv.ToTable().Rows[0]["Value"]).Trim() == string.Empty))
                                                    arrValues[i] = "01jan1900";
                                            }
                                            else
                                                arrValues[i] = "01jan1900";
                                            break;
                                        case "SRFT":
                                            if (dv.ToTable().Rows.Count > 0)
                                            {
                                                foreach (DataRow dr in dv.ToTable().Rows)
                                                {
                                                    intRecordID = Convert.ToInt32(dr["Record_Id"]);
                                                    if (intRecordID == 0)
                                                    {
                                                        if (dr["Value"] != DBNull.Value)
                                                            arrValues[i] = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                                        else
                                                            arrValues[i] = string.Empty;
                                                    }
                                                    else
                                                    {
                                                        DataRow drAddn = dtbl.NewRow();
                                                        dr["srl_no"] = intRecordID;
                                                        if (dr["Value"] != DBNull.Value)
                                                            dr["addendum_text"] = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                                        else
                                                            dr["addendum_text"] = string.Empty;
                                                    }
                                                }
                                            }
                                            else
                                                arrValues[i] = string.Empty;
                                            break;
                                        default:
                                            if (dv.ToTable().Rows.Count > 0)
                                            {
                                                if (dv.ToTable().Rows[0]["Value"] != DBNull.Value)
                                                    arrValues[i] = Convert.ToString(dv.ToTable().Rows[0]["Value"]).Replace("^", " ").Trim();
                                                else
                                                    arrValues[i] = string.Empty;
                                            }
                                            else
                                                arrValues[i] = string.Empty;
                                            break;
                                    }
                                    dv.Dispose();
                                }
                                else
                                    arrValues[i] = string.Empty;
                            }

                            #endregion

                        }
                        else
                            StudyExists = false;

                        xmlSR = null;
                        ds.Dispose();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                    }
                    else
                    {
                        CatchMessage = sError.Trim();
                    }
                }
                else
                {
                    CatchMessage = sError.Trim();

                }

            }
            catch (Exception ex)
            {
                bRet = false;
                if (ex.Message.Contains("System.OutOfMemoryException"))
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }

            }
            finally
            {

                client = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            return bRet;
        }
        #endregion

        #region UpdateReport
        private void UpdateReport(int StatusID, DataTable dtbl, CaseStudyUpdate objSU)
        {
            string strCatchMessage = string.Empty;
            string strUpdate = string.Empty;

            try
            {
                //  CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Getting report for study uid " + objSU.STUDY_UID, false);

                if (!objSU.UpdateReport(strConfigPath, 3, "", dtbl, ref strCatchMessage))
                {
                    // CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateReport() - Update Report for Study UID : " + objSU.STUDY_UID + " - " + strCatchMessage, true);
                }

            }
            catch (Exception ex)
            {
                //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateReport() - Exception: " + ex.Message, true);

            }
        }
        #endregion

        #region CreateAddendumTable
        private DataTable CreateAddendumTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("srl_no", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("addendum_text", System.Type.GetType("System.String"));
            dtbl.TableName = "Addendum";
            return dtbl;
        }
        #endregion

        #region IsDate
        protected bool IsDate(String date)
        {
            DateTime Temp;
            if (DateTime.TryParse(date, out Temp) == true)
                return true;
            else
                return false;
        }
        #endregion

        #region IsDecimal
        protected bool IsDecimal(String decimalValue)
        {
            Decimal Temp;
            if (Decimal.TryParse(decimalValue, out Temp) == true)
                return true;
            else
                return false;
        }
        #endregion

        #region IsInteger
        protected bool IsInteger(String integerValue)
        {
            Decimal Temp;
            if (Decimal.TryParse(integerValue, out Temp) == true)
                return true;
            else
                return false;
        }
        #endregion

        #region PopulateMissingStudy
        private void button16_Click(object sender, EventArgs e)
        {

        }

        #region PopulateData
        //private void PopulateMissingData(string strResult)
        //{
        //    DataSet ds = new DataSet();
        //    DataTable dtbl = new DataTable();
        //    string strColID = string.Empty;
        //    int intRecID = 0;
        //    string strDt = string.Empty;
        //    string strTblDate = string.Empty;

        //    try
        //    {
        //        System.IO.StringReader xmlSR = new System.IO.StringReader(strResult);
        //        ds.ReadXml(xmlSR);

        //        dtbl = CreateMissingRecordTable();
        //        if (ds.Tables.Count == 5)
        //        {
        //            #region Populate Study

        //            for (int i = 0; i < ds.Tables["Field"].Rows.Count; i += dtbl.Columns.Count)
        //            {
        //                DataRow dr = dtbl.NewRow();
        //                intRecID = Convert.ToInt32(ds.Tables["Field"].Rows[i]["Record_Id"]);
        //                DataView dv = new DataView(ds.Tables["Field"]);
        //                dv.RowFilter = "Record_Id=" + Convert.ToString(intRecID);


        //                #region Data Manupulation
        //                foreach (DataRow drRec in dv.ToTable().Rows)
        //                {
        //                    strColID = Convert.ToString(drRec["Colid"]).Trim();
        //                    switch (strColID)
        //                    {
        //                        case "SYUI":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["study_uid"] = Convert.ToString(drRec["Value"]).Trim();
        //                            else
        //                                dr["study_uid"] = string.Empty;
        //                            break;
        //                        case "SYDT":
        //                            if (drRec["Value"] != DBNull.Value)
        //                            {
        //                                if (IsDate(Convert.ToString(drRec["Value"])))
        //                                    dr["study_date"] = Convert.ToDateTime(drRec["Value"]);
        //                                else
        //                                    dr["study_date"] = Convert.ToDateTime("01jan1900");
        //                            }
        //                            else if ((drRec["Value"] == DBNull.Value) || (Convert.ToString(drRec["Value"]).Trim() == string.Empty))
        //                                dr["study_date"] = Convert.ToDateTime("01jan1900");
        //                            break;
        //                        case "RCVD":
        //                            if (drRec["Value"] != DBNull.Value)
        //                            {
        //                                if (IsDate(Convert.ToString(drRec["Value"])))
        //                                    dr["received_date"] = Convert.ToDateTime(drRec["Value"]);
        //                                else
        //                                    dr["received_date"] = DateTime.Now;
        //                            }
        //                            else if ((drRec["Value"] == DBNull.Value) || (Convert.ToString(drRec["Value"]).Trim() == string.Empty))
        //                                dr["received_date"] = DateTime.Now;
        //                            break;
        //                        case "ACCN":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["accession_no"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["accession_no"] = string.Empty;
        //                            break;
        //                        case "PAID":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["patient_id"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["patient_id"] = string.Empty;
        //                            break;
        //                        case "PANM":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["patient_name"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["patient_name"] = string.Empty;
        //                            break;
        //                        case "PDOB":
        //                            if (drRec["Value"] != DBNull.Value)
        //                            {
        //                                strDt = Convert.ToString(drRec["Value"]).Trim();
        //                                if (strDt == "00000000_000000") strTblDate = "01jan1900";
        //                                else if (strDt == "") strTblDate = "01jan1900";
        //                                else strTblDate = strDt.Substring(0, 4) + "-" + strDt.Substring(4, 2) + "-" + strDt.Substring(6, 2);
        //                                if (IsDate(strTblDate)) dr["patient_dob"] = Convert.ToDateTime(strTblDate);
        //                            }
        //                            else
        //                                dr["patient_dob"] = Convert.ToDateTime("01jan1900");
        //                            break;
        //                        case "PAGE":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["patient_age"] = Convert.ToString(drRec["Value"]).Trim();
        //                            else
        //                                dr["patient_age"] = "0";
        //                            break;
        //                        case "PSEX":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["patient_sex"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["patient_sex"] = string.Empty;
        //                            break;
        //                        case "9PWT":
        //                            if (drRec["Value"] != DBNull.Value)
        //                            {
        //                                if (IsDecimal(Convert.ToString(drRec["Value"])))
        //                                    dr["patient_weight_lbs"] = Convert.ToDecimal(drRec["Value"]);
        //                                else
        //                                    dr["patient_weight_lbs"] = 0;
        //                            }
        //                            else if ((drRec["Value"] == DBNull.Value) || (Convert.ToString(drRec["Value"]).Trim() == string.Empty))
        //                                dr["patient_weight_lbs"] = 0;
        //                            break;
        //                        case "9SPC":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["species"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["species"] = string.Empty;
        //                            break;
        //                        case "9BRD":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["breed"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["breed"] = string.Empty;
        //                            break;
        //                        case "9RSP":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["owner"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["owner"] = string.Empty;
        //                            break;
        //                        case "PALL":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["modality"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["modality"] = string.Empty;
        //                            break;
        //                        case "BDYP":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["body_part"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["body_part"] = string.Empty;
        //                            break;
        //                        case "PMAL":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["reason"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["reason"] = string.Empty;
        //                            break;
        //                        case "INSN":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["institution_name"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["institution_name"] = string.Empty;
        //                            break;
        //                        case "PHRF":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["referring_physician"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["referring_physician"] = string.Empty;
        //                            break;
        //                        case "MFCT":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["manufacturer_name"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["manufacturer_name"] = string.Empty;
        //                            break;
        //                        case "MFMD":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["manufacturer_model_no"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["manufacturer_model_no"] = string.Empty;
        //                            break;
        //                        case "STNM":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["device_serial_no"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["device_serial_no"] = string.Empty;
        //                            break;
        //                        case "9PSN":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["spayed_neutered"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["spayed_neutered"] = string.Empty;
        //                            break;
        //                        case "NIMG":
        //                            if (drRec["Value"] != DBNull.Value)
        //                            {
        //                                if (IsInteger(Convert.ToString(drRec["Value"])))
        //                                    dr["img_count"] = Convert.ToInt32(drRec["Value"]);
        //                                else
        //                                    dr["img_count"] = 0;
        //                            }
        //                            else
        //                                dr["img_count"] = 0;
        //                            break;
        //                        case "PSAE":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["modality_ae_title"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["modality_ae_title"] = string.Empty;
        //                            break;
        //                        case "PRST":
        //                            if (drRec["Value"] != DBNull.Value)
        //                            {
        //                                if (IsInteger(Convert.ToString(drRec["Value"])))
        //                                    dr["priority_id"] = Convert.ToInt32(drRec["Value"]);
        //                                else
        //                                    dr["priority_id"] = 0;

        //                            }
        //                            else if ((drRec["Value"] == DBNull.Value) || (Convert.ToString(drRec["Value"]).Trim() == string.Empty))
        //                                dr["priority_id"] = 0;
        //                            break;
        //                        case "TRAD":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["radiologist_name"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["radiologist_name"] = string.Empty;
        //                            break;
        //                        case "STAT":
        //                            if (drRec["Value"] != DBNull.Value)
        //                            {
        //                                if (IsInteger(Convert.ToString(drRec["Value"])))
        //                                    dr["study_status_pacs"] = Convert.ToInt32(drRec["Value"]);
        //                                else
        //                                    dr["study_status_pacs"] = 0;

        //                            }
        //                            else if ((drRec["Value"] == DBNull.Value) || (Convert.ToString(drRec["Value"]).Trim() == string.Empty))
        //                                dr["study_status_pacs"] = 0;
        //                            break;
        //                        case "UDF1":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["sales_person"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["sales_person"] = string.Empty;
        //                            break;
        //                        case "UDF8":
        //                            if (IsDecimal(Convert.ToString(drRec["Value"])))
        //                                dr["patient_weight_kgs"] = Convert.ToDecimal(drRec["Value"]);
        //                            else
        //                                dr["patient_weight_kgs"] = 0;
        //                            break;
        //                        case "DSCR":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["study_type_name_1"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["study_type_name_1"] = string.Empty;
        //                            break;
        //                        case "UDF4":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["study_type_name_2"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["study_type_name_2"] = string.Empty;
        //                            break;
        //                        case "UDF7":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["study_type_name_3"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["study_type_name_1"] = string.Empty;
        //                            break;
        //                        case "UDF9":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["study_type_name_4"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["study_type_name_1"] = string.Empty;
        //                            break;
        //                        case "NOBJ":
        //                            if (drRec["Value"] != DBNull.Value)
        //                            {
        //                                if (Convert.ToInt32(drRec["Value"]) > 0)
        //                                {
        //                                    if (IsInteger(Convert.ToString(drRec["Value"])))
        //                                        dr["object_count"] = Convert.ToInt32(drRec["Value"]) - 1;
        //                                    else
        //                                        dr["object_count"] = 0;
        //                                }
        //                                else if ((drRec["Value"] == DBNull.Value) || (Convert.ToString(drRec["Value"]).Trim() == string.Empty))
        //                                {
        //                                    dr["object_count"] = 0;
        //                                }
        //                            }
        //                            else
        //                                dr["object_count"] = 0;
        //                            break;
        //                        case "UDF3":
        //                            if (drRec["Value"] != DBNull.Value)
        //                                dr["service_codes"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
        //                            else
        //                                dr["service_codes"] = string.Empty;
        //                            break;
        //                        case "9VCD":
        //                            if (drRec["Value"] != DBNull.Value)
        //                            {
        //                                if (IsDate(Convert.ToString(drRec["Value"])))
        //                                    dr["submit_on"] = Convert.ToDateTime(drRec["Value"]);
        //                                else
        //                                    dr["submit_on"] = Convert.ToDateTime("01jan1900");
        //                            }
        //                            else if ((drRec["Value"] == DBNull.Value) || (Convert.ToString(drRec["Value"]).Trim() == string.Empty))
        //                                dr["submit_on"] = Convert.ToDateTime("01jan1900");
        //                            break;
        //                    }
        //                }
        //                #endregion

        //                dtbl.Rows.Add(dr);
        //            }
        //            #endregion

        //            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, dtbl.Rows.Count.ToString() + " missing records downloaded", false);
        //        }
        //        else
        //            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "0 missing records downloaded", false);


        //        if (dtbl.Rows.Count > 0) SynchData(dtbl);

        //    }
        //    catch (Exception ex)
        //    {
        //        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "PopulateData() - Exception: " + ex.Message, true);

        //    }
        //    finally
        //    {
        //        objCore = null;
        //        ds.Dispose();
        //    }


        //}
        #endregion

        #region CreateMissingRecordTable
        private DataTable CreateMissingRecordTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("study_uid", System.Type.GetType("System.String"));
            dtbl.Columns.Add("study_date", System.Type.GetType("System.DateTime"));
            dtbl.Columns.Add("received_date", System.Type.GetType("System.DateTime"));
            dtbl.Columns.Add("accession_no", System.Type.GetType("System.String"));
            dtbl.Columns.Add("reason", System.Type.GetType("System.String"));
            dtbl.Columns.Add("institution_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("manufacturer_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("manufacturer_model_no", System.Type.GetType("System.String"));
            dtbl.Columns.Add("device_serial_no", System.Type.GetType("System.String"));
            dtbl.Columns.Add("modality_ae_title", System.Type.GetType("System.String"));
            dtbl.Columns.Add("referring_physician", System.Type.GetType("System.String"));
            dtbl.Columns.Add("patient_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("patient_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("patient_sex", System.Type.GetType("System.String"));
            dtbl.Columns.Add("patient_dob", System.Type.GetType("System.DateTime"));
            dtbl.Columns.Add("patient_age", System.Type.GetType("System.String"));
            dtbl.Columns.Add("patient_weight_lbs", System.Type.GetType("System.Decimal"));
            dtbl.Columns.Add("modality", System.Type.GetType("System.String"));
            dtbl.Columns.Add("body_part", System.Type.GetType("System.String"));
            dtbl.Columns.Add("species", System.Type.GetType("System.String"));
            dtbl.Columns.Add("breed", System.Type.GetType("System.String"));
            dtbl.Columns.Add("owner", System.Type.GetType("System.String"));
            dtbl.Columns.Add("spayed_neutered", System.Type.GetType("System.String"));
            dtbl.Columns.Add("img_count", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("priority_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("radiologist_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("sales_person", System.Type.GetType("System.String"));
            dtbl.Columns.Add("patient_weight_kgs", System.Type.GetType("System.Decimal"));
            dtbl.Columns.Add("study_status_pacs", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("study_type_name_1", System.Type.GetType("System.String"));
            dtbl.Columns.Add("study_type_name_2", System.Type.GetType("System.String"));
            dtbl.Columns.Add("study_type_name_3", System.Type.GetType("System.String"));
            dtbl.Columns.Add("study_type_name_4", System.Type.GetType("System.String"));
            dtbl.Columns.Add("object_count", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("physician_note", System.Type.GetType("System.String"));
            dtbl.Columns.Add("service_codes", System.Type.GetType("System.String"));
            dtbl.Columns.Add("submit_on", System.Type.GetType("System.DateTime"));
            dtbl.TableName = "Details";
            return dtbl;
        }
        #endregion

        #endregion

        #region DICOM Receiver Files Test
        private void button17_Click(object sender, EventArgs e)
        {
            strDCMRCVRFLDR = "F:/VetChoice/DICOMReceiver";
            strFTPSRCFOLDER = "F:/VetChoice/vetris_ftp/DICOMFilesDownload";
            try
            {
                ArrangeFiles();

            }
            catch (Exception expErr)
            {
                textBox1.Text = "doProcessDownload()==>doArrangeFiles() - Exception: " + expErr.Message + "\r\n";
            }
        }

        #region ArrangeFiles
        private void ArrangeFiles()
        {
            string[] dirs = new string[0];
            string strSID = string.Empty;

            try
            {
                dirs = Directory.GetDirectories(strDCMRCVRFLDR);

                for (int i = 0; i < dirs.Length; i++)
                {
                    if (dirs[i].Trim() != "InfoRequired")
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(dirs[i]);
                        strSID = "S1D" + DateTime.Now.ToString("MMddyyHHmmss") + CoreCommon.RandomString(3);
                        WalkDirectoryTree(dirInfo, strSID);
                        if (Directory.Exists(dirInfo.FullName)) DeleteEmptyDirectoryTree(dirInfo);
                        dirInfo = null;
                    }
                }
            }
            catch (Exception ex)
            {
                textBox1.Text = "doDownloadFiles()=> doArrangeFiles() => ArrangeFiles() - Exception: " + ex.Message + "\r\n";
            }

        }
        #endregion

        #region WalkDirectoryTree
        private void WalkDirectoryTree(System.IO.DirectoryInfo root, string SessionID)
        {
            string[] pathElements = new string[0];
            string[] fileElements = new string[0];
            string[] arr = new string[0];
            DicomDecoder dd = new DicomDecoder();
            //int isDiacom = -1;

            string strSUID = string.Empty;
            string strInstName = string.Empty;
            string strFile = string.Empty;
            string strFilename = string.Empty;
            string strNewFilename = string.Empty;
            string strNewFilePath = string.Empty;
            string strParentFolder = string.Empty;
            string strMIMEType = string.Empty;
            string strDirName = string.Empty;
            string[] arrFiles = new string[0];
            string strPrefix = string.Empty;
            string strExtn = string.Empty;
            string strZipPath = string.Empty;
            string strTargetPath = string.Empty;

            int intRejCount = 0;
            int intIgnoreCount = 0;
            string strCatchMsg = string.Empty;
            FTPPACSSynch objFP = new FTPPACSSynch();
            Scheduler objCore = new Scheduler();

            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;


            #region get file list
            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.*");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException ex)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
                ;
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                //Console.WriteLine(e.Message);
                ;
            }
            #endregion

            if (files != null)
            {
                try
                {
                    foreach (System.IO.FileInfo fi in files)
                    {

                        strFile = fi.FullName;

                        if (CheckValidFileFormat(strFile))
                        {
                            #region queuing files
                            strDirName = root.Name;
                            strFilename = fi.Name;
                            dd.DicomFileName = strFile;
                            List<string> str = dd.dicomInfo;
                            arr = new string[17];
                            arr = GetallTags1(str);

                            if ((arr[5].Trim().ToUpper() != string.Empty) && (arr[0].Trim().ToUpper() != string.Empty))
                            {
                                objCore.INSTITUTION_NAME = arr[5].Trim();
                                if (!objCore.FetchInstitutionInfo(strConfigPath, ref strCatchMsg))
                                {
                                    textBox1.Text = "doProcessDownload()=>doArrangeFiles()=>ArrangeFiles()=>WalkDirectoryTree()=>FetchInstitutionInfo():Core::Exception - " + strCatchMsg + "\r\n";
                                }
                                else
                                {
                                    if (objFP.INSTITUTION_CODE.Trim() == string.Empty)
                                    {
                                        intIgnoreCount = intIgnoreCount + 1;
                                    }
                                    else
                                    {
                                        #region save the file after renaming
                                        strPrefix = CoreCommon.RandomString(6);
                                        strExtn = Path.GetExtension(strFile);

                                        strNewFilename = objFP.INSTITUTION_CODE.Trim() + "_" + SessionID + "_" + objFP.INSTITUTION_NAME.Trim().Replace(" ", "_") + "_" + strPrefix + "_" + strFilename;
                                        strNewFilename = strNewFilename.Replace(" ", "_");
                                        strNewFilename = strNewFilename.Replace("(", "");
                                        strNewFilename = strNewFilename.Replace(")", "");
                                        strNewFilename = strNewFilename.Replace("'", "");
                                        strNewFilename = strNewFilename.Replace("\"", "");
                                        strNewFilename = strNewFilename.Replace("#", "");
                                        strNewFilename = strNewFilename.Replace("&", "");
                                        strNewFilename = strNewFilename.Replace("@", "");
                                        strNewFilePath = root.FullName + "/" + strNewFilename;

                                        if (File.Exists(strFile))
                                        {

                                            File.Move(strFile, strNewFilePath);
                                            if (File.Exists(strFile)) File.Delete(strFile);
                                        }
                                        #endregion

                                        #region compress the file
                                        try
                                        {

                                            if (strExtn.Trim() != string.Empty)
                                            {
                                                if (strNewFilename.Contains(strExtn))
                                                    strNewFilename = strNewFilename.Replace(strExtn, string.Empty);
                                            }

                                            if (File.Exists(root.FullName + "/" + strNewFilename + ".zip")) File.Delete(root.FullName + "/" + strNewFilename + ".zip");
                                            strZipPath = root.FullName + "/" + strNewFilename + ".zip";

                                            using (ZipArchive zip = ZipFile.Open(strZipPath, ZipArchiveMode.Create))
                                            {
                                                zip.CreateEntryFromFile(strNewFilePath, strNewFilename + strExtn);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            textBox1.Text = "doDownloadFiles()=>doArrangeFiles()=>ArrangeFiles()=> WalkDirectoryTree() - File Compression - Exception: " + ex.Message + "\r\n";
                                        }
                                        #endregion

                                        #region Move the .zip file
                                        try
                                        {
                                            strTargetPath = strFTPSRCFOLDER + "\\" + strNewFilename + ".zip";
                                            if (File.Exists(strTargetPath)) File.Delete(strTargetPath);
                                            File.Move(strZipPath, strTargetPath);
                                        }
                                        catch (Exception ex)
                                        {
                                            textBox1.Text = "doDownloadFiles()>doArrangeFiles()=>ArrangeFiles()=> WalkDirectoryTree() - Move Zip file to FTP Folder - Exception: " + ex.Message + "\r\n";
                                        }
                                        #endregion

                                        #region Delete file
                                        try
                                        {
                                            if (File.Exists(strNewFilePath)) File.Delete(strNewFilePath);
                                        }
                                        catch (Exception ex)
                                        {
                                            textBox1.Text = "doDownloadFiles()=> UploadManualSubmissions() - Delte File Entry - Exception: " + ex.Message + "\r\n";
                                        }
                                        #endregion
                                    }
                                }

                            }
                            else
                            {
                                if (File.Exists(strFile))
                                {
                                    File.Move(strFile, root.FullName + "/" + strFilename);
                                    intRejCount = intRejCount + 1;
                                }
                            }
                            #endregion

                        }
                        else
                        {
                            #region delete file
                            if (File.Exists(strFile)) File.Delete(strFile);
                            #endregion
                        }
                    }
                }
                catch (Exception ex)
                {
                    textBox1.Text = "doDownloadFiles()=>doArrangeFiles()=>ArrangeFiles()=> WalkDirectoryTree() - Exception: " + ex.Message + "\r\n";
                }

                try
                {
                    // Now find all the subdirectories under this directory.
                    subDirs = root.GetDirectories();
                    if (subDirs.Length > 0)
                    {
                        foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                        {
                            // Resursive call for each subdirectory.
                            WalkDirectoryTree(dirInfo, SessionID);

                        }
                    }
                    else
                    {
                        if (Directory.Exists(root.FullName))
                        {
                            if (root.GetFiles("*.*").Length == 0) Directory.Delete(root.FullName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    textBox1.Text = "doDownloadFiles()=>doArrangeFiles()=>ArrangeFiles()=> WalkDirectoryTree() - Exception: " + ex.Message + "\r\n";
                }

            }
            objFP = null;
            objCore = null;
        }
        #endregion

        #region CheckValidFileFormat
        private bool CheckValidFileFormat(string strFilePath)
        {
            bool bRet = false;
            string[] pathElements = new string[0];

            if (CoreCommon.IsDicomFile(strFilePath))
            {
                pathElements = strFilePath.Split('\\');
                if (pathElements[pathElements.Length - 1].Trim().ToUpper() == "DICOMDIR")
                    bRet = false;
                else
                    bRet = true;
            }
            else if ((MIMEAssistant.GetMIMEType(strFilePath) == "image/jpeg") || (MIMEAssistant.GetMIMEType(strFilePath) == "image/gif") || (MIMEAssistant.GetMIMEType(strFilePath) == "image/png") || (MIMEAssistant.GetMIMEType(strFilePath) == "image/bmp"))
            {
                bRet = true;
            }
            else
            {
                // MessageBox.Show("Invalid file format : " + strFilePath, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bRet = false;
            }


            return bRet;
        }
        #endregion

        #region DeleteEmptyDirectoryTree
        private void DeleteEmptyDirectoryTree(System.IO.DirectoryInfo root)
        {
            System.IO.DirectoryInfo[] subDirs = null;


            try
            {
                subDirs = root.GetDirectories();
                if (subDirs.Length > 0)
                {
                    foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                    {
                        // Resursive call for each subdirectory.
                        DeleteEmptyDirectoryTree(dirInfo);

                    }
                }
                else
                {
                    if (Directory.Exists(root.FullName))
                    {
                        if (root.GetFiles("*.*").Length == 0) Directory.Delete(root.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                textBox1.Text = "doDownloadFiles()=>doArrangeFiles()=>ArrangeFiles()=> DeleteEmptyDirectoryTree() - Exception: " + ex.Message + "\r\n";
            }



        }
        #endregion

        #region GetallTags1
        private string[] GetallTags1(List<string> str)
        {

            string strDescription = string.Empty;
            string UserCaseID = string.Empty;
            string ModalityID = string.Empty;
            string StrPName = string.Empty;
            string StudyDt = string.Empty;
            string StudyTime = string.Empty;
            string sDt = string.Empty;
            string sTime = string.Empty;
            string studyDtTime = string.Empty;
            string UserSeriesID = string.Empty;
            string SeriesNumber = string.Empty;
            string InstitutionName = string.Empty;
            string PatientID = string.Empty;
            string AccnNo = string.Empty;
            string RefPhys = string.Empty;
            string Manufacturer = string.Empty;
            string StationName = string.Empty;
            string Model = string.Empty;
            string ModalityAETitle = string.Empty;
            string Reason = string.Empty;
            string BirthDt = string.Empty;
            string bDt = string.Empty;
            string PatientSex = string.Empty;
            string PatientAge = string.Empty;
            //string PatientWt = string.Empty;//(0010,1030)
            //string Species = string.Empty;//(0010,2201)
            //string Breed = string.Empty;//(0010,2292)
            //string Owner = string.Empty;//(0010,2297)
            string PriorityID = string.Empty;


            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                string s1, s4, s5, s11, s12;
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                #region commented
                /*if ((s11.ToUpper() == "0008") && (s12.ToUpper() == "103E"))
                {
                    strDescription = s5.Replace("\0", "");
                    strDescription = s5.Replace("<", " ");
                    strDescription = s5.Replace(">", " ");

                }

                else if ((s11.ToUpper() == "0008") && (s12.ToUpper() == "0060"))
                {
                    ModalityID = s5.Replace("\0", "");

                }


                else if ((s11.ToUpper() == "0010") && (s12.ToUpper() == "0010"))
                {
                    Strname = s5.Replace("\0", "");
                    Strname = s5.Replace("^", " ");

                }
                else if ((s11.ToUpper() == "0010") && (s12.ToUpper() == "0030"))
                {
                    DOB = s5.Replace("\0", "");
                    DOB = DOB.Trim();
                    if (DOB != "")
                    {
                        string yy = DOB.Substring(0, 4);
                        string MM = DOB.Substring(4, 2);
                        string DD = DOB.Substring(6, 2);
                        result = yy + "-" + MM + "-" + DD;
                    }
                    else
                    {
                        result = "0000-00-00";
                    }
                }

                else */
                #endregion

                #region Tags
                s5 = s5.Replace("\t", "");
                s5 = s5.Replace("\n", "");

                switch (s11.ToUpper())
                {
                    case "0008":
                        #region s11 =0008
                        switch (s12.ToUpper())
                        {
                            case "0020":
                                StudyDt = s5.Replace("\0", "");
                                StudyDt = StudyDt.Trim();
                                if ((StudyDt.Length == 8))
                                {
                                    string yyyy = StudyDt.Substring(0, 4);
                                    string MM = StudyDt.Substring(4, 2);
                                    string DD = StudyDt.Substring(6, 2);
                                    sDt = yyyy + "-" + MM + "-" + DD;
                                }
                                else
                                {
                                    sDt = "0000-00-00";
                                }
                                break;
                            case "0030":
                                StudyTime = s5.Replace("\0", "");
                                StudyTime = StudyDt.Trim();
                                if ((StudyTime.Length == 13))
                                {
                                    string Hr = StudyDt.Substring(0, 2);
                                    string Min = StudyDt.Substring(2, 2);
                                    string Sec = StudyDt.Substring(4, 2);
                                    sTime = Hr + ":" + Min + ":" + Sec;
                                }
                                else
                                {
                                    sTime = "00:00:00";
                                }
                                break;
                            case "0050":
                                AccnNo = s5.Replace("\0", "");
                                break;
                            case "0060":
                                ModalityID = s5.Replace("\0", "");
                                break;
                            case "0070":
                                Manufacturer = s5.Replace("\0", "");
                                break;
                            case "0080":
                                InstitutionName = s5.Replace("\0", "");
                                InstitutionName = s5.Replace("^", " ");
                                break;
                            case "0090":
                                RefPhys = s5.Replace("\0", "");
                                RefPhys = s5.Replace("^", " ");
                                break;
                            case "1010":
                                StationName = s5.Replace("\0", "");
                                break;
                            case "1090":
                                Model = s5.Replace("\0", "");
                                break;
                            default:
                                break;
                        }
                        #endregion
                        break;
                    case "0010":
                        #region s11 =0010
                        switch (s12.ToUpper())
                        {
                            case "0010":
                                StrPName = s5.Replace("\0", "");
                                StrPName = s5.Replace("^", " ");
                                break;
                            case "0020":
                                PatientID = s5.Replace("\0", "");
                                break;
                            case "0030":
                                BirthDt = s5.Replace("\0", "");
                                BirthDt = BirthDt.Trim();
                                if ((BirthDt.Length == 8))
                                {
                                    string yyyy = BirthDt.Substring(0, 4);
                                    string MM = BirthDt.Substring(4, 2);
                                    string DD = BirthDt.Substring(6, 2);
                                    bDt = yyyy + "-" + MM + "-" + DD;
                                }
                                else
                                {
                                    bDt = "0000-00-00";
                                }
                                break;
                            case "0040":
                                PatientSex = s5.Replace("\0", "");
                                break;
                            case "1010":
                                PatientAge = s5.Replace("\0", "");
                                break;
                        }
                        #endregion
                        break;
                    case "0020":
                        #region s11 =0020
                        switch (s12.ToUpper())
                        {
                            case "000D":
                                UserCaseID = s5.Replace("\0", "");
                                break;
                            case "000E":
                                UserSeriesID = s5.Replace("\0", "");
                                break;
                            case "0011":
                                SeriesNumber = s5.Replace("\0", "");
                                break;
                        }
                        #endregion
                        break;
                    case "0032":
                        #region s11 =0032
                        switch (s12.ToUpper())
                        {
                            case "000C":
                                PriorityID = s5.Replace("\0", "");
                                break;
                        }
                        #endregion
                        break;
                    case "0040":
                        #region s11 =0040
                        switch (s12.ToUpper())
                        {
                            case "0241":
                                ModalityAETitle = s5.Replace("\0", "");
                                break;
                            case "1002":
                                Reason = s5.Replace("\0", "");
                                break;

                        }
                        #endregion
                        break;
                    default:
                        break;
                }
                #endregion
            }

            studyDtTime = sDt + " " + sTime;

            string[] arr = new string[17];
            arr[0] = UserCaseID;
            arr[1] = ModalityID;
            arr[2] = PatientID;
            arr[3] = StrPName;
            arr[4] = studyDtTime;
            arr[5] = InstitutionName;
            arr[6] = AccnNo;
            arr[7] = RefPhys;
            arr[8] = Manufacturer;
            arr[9] = StationName;
            arr[10] = Model;
            arr[11] = ModalityAETitle;
            arr[12] = Reason;
            arr[13] = bDt;
            arr[14] = PatientSex;
            arr[15] = PatientAge;
            arr[16] = PriorityID;

            return arr;

        }
        #endregion

        #endregion

        #region btnCheckOnHoldFiles_Click
        private void btnCheckOnHoldFiles_Click(object sender, EventArgs e)
        {
            string strCatchMessage = string.Empty;
            string strRetMessage = string.Empty;
            string strFilePath = string.Empty;
            string strFileName = string.Empty;
            string strSID = string.Empty;
            string strIsManual = string.Empty;
            string strNewFilename = string.Empty;
            string strNewFilePath = string.Empty;
            string strPrefix = string.Empty;
            string strExtn = string.Empty;

            string[] arrFolders = new string[0];
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string[] arr = new string[0];
            List<string> arrSUID = new List<string>();
            DicomDecoder dd = new DicomDecoder();

            #region TAG Variables
            string strSUID = string.Empty;
            string strInstCode = string.Empty;
            string strInstName = string.Empty;
            #endregion

            FTPPACSSynch objFP = new FTPPACSSynch();
            string strFILESHOLDPATH = "F:/VetChoice/FILES_ON_HOLD";
            string strFTPDLFLDRTMP = "F:/VetChoice/FTP_DOWNLOAD_TEMP";

            try
            {

                arrFiles = Directory.GetFiles(strFILESHOLDPATH);

                foreach (string strFile in arrFiles)
                {

                    pathElements = strFile.Replace("\\", "/").Split('/');
                    strFileName = pathElements[pathElements.Length - 1];
                    strSID = (strFileName.Split('_'))[1].Trim();

                    dd.DicomFileName = strFile;
                    List<string> str = dd.dicomInfo;

                    arr = new string[20];
                    arr = GetallTags(str);
                    strSUID = arr[0].Trim();

                    if (strSUID.Trim() != string.Empty)
                    {
                        textBox1.Text = "File : " + strFileName.Trim() + " get tag data ";
                        try
                        {
                            #region Get File Data
                            strInstName = arr[5].Trim();
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            textBox1.Text += "File : " + strFileName.Trim() + " get tag data :: " + ex.Message.Trim();
                        }

                        if (strInstName.Trim().ToUpper() != string.Empty)
                        {
                            try
                            {
                                objCore.INSTITUTION_NAME = strInstName.Trim();
                                if (!objCore.FetchInstitutionInfo(strConfigPath, ref strCatchMessage))
                                {
                                    textBox1.Text += "FetchInstitutionInfo():Core::Exception - " + strCatchMessage;
                                }

                                if (objCore.INSTITUTION_CODE.Trim() != string.Empty)
                                {
                                    #region save the file after renaming
                                    strPrefix = CoreCommon.RandomString(6);
                                    strExtn = Path.GetExtension(strFilePath);
                                    strInstCode = objCore.INSTITUTION_CODE.Trim();
                                    strInstName = objCore.INSTITUTION_NAME.Trim();
                                    if (strExtn.Trim() != string.Empty)
                                        strNewFilename = objCore.INSTITUTION_CODE.Trim() + "_" + strSID + "_" + objCore.INSTITUTION_NAME.Trim().Replace(" ", "_") + "_" + strPrefix + "_" + strSUID.Replace(".", "").Substring(0, 5) + strExtn;
                                    else
                                        strNewFilename = objCore.INSTITUTION_CODE.Trim() + "_" + strSID + "_" + objCore.INSTITUTION_NAME.Trim().Replace(" ", "_") + "_" + strPrefix + "_" + strSUID.Replace(".", "").Substring(0, 5);

                                    strNewFilename = strNewFilename.Replace(" ", "_");
                                    strNewFilename = strNewFilename.Replace("(", "");
                                    strNewFilename = strNewFilename.Replace(")", "");
                                    strNewFilename = strNewFilename.Replace("'", "");
                                    strNewFilename = strNewFilename.Replace("\"", "");
                                    strNewFilename = strNewFilename.Replace("#", "");
                                    strNewFilename = strNewFilename.Replace("&", "");
                                    strNewFilename = strNewFilename.Replace("@", "");
                                    strNewFilePath = strFILESHOLDPATH + "/" + strNewFilename;


                                    if (File.Exists(strFilePath))
                                    {

                                        File.Move(strFilePath, strNewFilePath);
                                        if (File.Exists(strFilePath)) File.Delete(strFilePath);

                                    }
                                    #endregion

                                    if (File.Exists(strFTPDLFLDRTMP + "/" + strNewFilename)) File.Delete(strFTPDLFLDRTMP + "/" + strNewFilename);
                                    textBox1.Text += "doProcessDownload()=>doCheckOnHoldFiles()=>CheckOnHoldFiles()::File : " + strFileName + " renamed to " + strNewFilename;
                                    File.Move(strFILESHOLDPATH + "/" + strFileName, strFTPDLFLDRTMP + "/" + strNewFilename);
                                    textBox1.Text += "File - " + strNewFilename + " Site code found, moved back to " + strFTPDLFLDRTMP;
                                    //UpdateDownloadedListenerFileRecords(strFileName, strSID);

                                }
                                else
                                {
                                    textBox1.Text += "File - " + strNewFilename + " Site code not found for institution " + strInstName.Trim();
                                }
                            }
                            catch (Exception ex)
                            {
                                textBox1.Text += "File : " + strFileName.Trim() + ":: " + ex.Message.Trim();
                            }
                        }
                        else
                        {
                            textBox1.Text += "File : " + strFileName.Trim() + "  Institution name missing";
                        }
                    }
                }



            }
            catch (Exception expErr)
            {
                textBox1.Text = "Exception: " + expErr.Message;
            }

            finally
            {
                dd = null;
                objFP = null;
            }
        }
        #endregion


        #region GetallTags
        private string[] GetallTags(List<string> str)
        {

            string strDescription = string.Empty;
            string StudyUID = string.Empty;
            string ModalityID = string.Empty;
            string StrPName = string.Empty;
            string StudyDt = string.Empty;
            string StudyTime = string.Empty;
            string sDt = string.Empty;
            string sTime = string.Empty;
            string studyDtTime = string.Empty;
            string SeriesUID = string.Empty;
            string SeriesNumber = string.Empty;
            string InstanceNumber = string.Empty;
            string SOPInstanceUID = string.Empty;
            string InstitutionName = string.Empty;
            string PatientID = string.Empty;
            string AccnNo = string.Empty;
            string RefPhys = string.Empty;
            string Manufacturer = string.Empty;
            string StationName = string.Empty;
            string Model = string.Empty;
            string ModalityAETitle = string.Empty;
            string Reason = string.Empty;
            string BirthDt = string.Empty;
            string bDt = string.Empty;
            string PatientSex = string.Empty;
            string PatientAge = string.Empty;
            //string PatientWt = string.Empty;//(0010,1030)
            //string Species = string.Empty;//(0010,2201)
            //string Breed = string.Empty;//(0010,2292)
            //string Owner = string.Empty;//(0010,2297)
            string PriorityID = string.Empty;


            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                string s1, s4, s5, s11, s12;
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                #region commented
                /*if ((s11.ToUpper() == "0008") && (s12.ToUpper() == "103E"))
                {
                    strDescription = s5.Replace("\0", "");
                    strDescription = s5.Replace("<", " ");
                    strDescription = s5.Replace(">", " ");

                }

                else if ((s11.ToUpper() == "0008") && (s12.ToUpper() == "0060"))
                {
                    ModalityID = s5.Replace("\0", "");

                }


                else if ((s11.ToUpper() == "0010") && (s12.ToUpper() == "0010"))
                {
                    Strname = s5.Replace("\0", "");
                    Strname = s5.Replace("^", " ");

                }
                else if ((s11.ToUpper() == "0010") && (s12.ToUpper() == "0030"))
                {
                    DOB = s5.Replace("\0", "");
                    DOB = DOB.Trim();
                    if (DOB != "")
                    {
                        string yy = DOB.Substring(0, 4);
                        string MM = DOB.Substring(4, 2);
                        string DD = DOB.Substring(6, 2);
                        result = yy + "-" + MM + "-" + DD;
                    }
                    else
                    {
                        result = "0000-00-00";
                    }
                }

                else */
                #endregion

                #region Tags
                s5 = s5.Replace("\t", "");
                s5 = s5.Replace("\n", "");

                switch (s11.ToUpper())
                {
                    case "0008":
                        #region s11 =0008
                        switch (s12.ToUpper())
                        {
                            case "0018":
                                SOPInstanceUID = s5.Replace("\0", "");
                                break;
                            case "0020":
                                StudyDt = s5.Replace("\0", "");
                                StudyDt = StudyDt.Trim();
                                if ((StudyDt.Length == 8))
                                {
                                    string yyyy = StudyDt.Substring(0, 4);
                                    string MM = StudyDt.Substring(4, 2);
                                    string DD = StudyDt.Substring(6, 2);
                                    sDt = yyyy + "-" + MM + "-" + DD;
                                }
                                else
                                {
                                    sDt = "0000-00-00";
                                }
                                break;
                            case "0030":
                                StudyTime = s5.Replace("\0", "");
                                StudyTime = StudyTime.Trim();
                                if ((StudyTime.Length == 6))
                                {
                                    string Hr = StudyTime.Substring(0, 2);
                                    string Min = StudyTime.Substring(2, 2);
                                    string Sec = StudyTime.Substring(4, 2);
                                    sTime = Hr + ":" + Min + ":" + Sec;
                                }
                                else
                                {
                                    sTime = "00:00:00";
                                }
                                break;
                            case "0050":
                                AccnNo = s5.Replace("\0", "");
                                break;
                            case "0060":
                                ModalityID = s5.Replace("\0", "");
                                break;
                            case "0070":
                                Manufacturer = s5.Replace("\0", "");
                                break;
                            case "0080":
                                if (InstitutionName.Trim() == string.Empty)
                                {
                                    InstitutionName = s5.Replace("\0", "");
                                    InstitutionName = s5.Replace("^", " ");
                                }
                                break;
                            case "0090":
                                RefPhys = s5.Replace("\0", "");
                                RefPhys = s5.Replace("^", " ");
                                break;
                            case "1010":
                                StationName = s5.Replace("\0", "");
                                break;
                            case "1090":
                                Model = s5.Replace("\0", "");
                                break;
                            default:
                                break;
                        }
                        #endregion
                        break;
                    case "0010":
                        #region s11 =0010
                        switch (s12.ToUpper())
                        {
                            case "0010":
                                StrPName = s5.Replace("\0", "");
                                StrPName = s5.Replace("^", " ");
                                break;
                            case "0020":
                                PatientID = s5.Replace("\0", "");
                                break;
                            case "0030":
                                BirthDt = s5.Replace("\0", "");
                                BirthDt = BirthDt.Trim();
                                if ((BirthDt.Length == 8))
                                {
                                    string yyyy = BirthDt.Substring(0, 4);
                                    string MM = BirthDt.Substring(4, 2);
                                    string DD = BirthDt.Substring(6, 2);
                                    bDt = yyyy + "-" + MM + "-" + DD;
                                }
                                else
                                {
                                    bDt = "0000-00-00";
                                }
                                break;
                            case "0040":
                                PatientSex = s5.Replace("\0", "");
                                break;
                            case "1010":
                                PatientAge = s5.Replace("\0", "");
                                break;
                        }
                        #endregion
                        break;
                    case "0020":
                        #region s11 =0020
                        switch (s12.ToUpper())
                        {
                            case "000D":
                            case "000d":
                                StudyUID = s5.Replace("\0", "");
                                break;
                            case "000E":
                            case "000e":
                                SeriesUID = s5.Replace("\0", "");
                                break;
                            case "0011":
                                SeriesNumber = s5.Replace("\0", "");
                                break;
                            case "0013":
                                InstanceNumber = s5.Replace("\0", "");
                                break;
                        }
                        #endregion
                        break;
                    case "0032":
                        #region s11 =0032
                        switch (s12.ToUpper())
                        {
                            case "000C":
                                PriorityID = s5.Replace("\0", "");
                                break;
                        }
                        #endregion
                        break;
                    case "0040":
                        #region s11 =0040
                        switch (s12.ToUpper())
                        {
                            case "0241":
                                ModalityAETitle = s5.Replace("\0", "");
                                break;
                            case "1002":
                                Reason = s5.Replace("\0", "");
                                break;

                        }
                        #endregion
                        break;
                    default:
                        break;
                }
                #endregion
            }

            studyDtTime = sDt + " " + sTime;

            string[] arr = new string[20];
            arr[0] = StudyUID;
            arr[1] = ModalityID;
            arr[2] = PatientID;
            arr[3] = StrPName;
            arr[4] = studyDtTime;
            arr[5] = InstitutionName;
            arr[6] = AccnNo;
            arr[7] = RefPhys;
            arr[8] = Manufacturer;
            arr[9] = StationName;
            arr[10] = Model;
            arr[11] = ModalityAETitle;
            arr[12] = Reason;
            arr[13] = bDt;
            arr[14] = PatientSex;
            arr[15] = PatientAge;
            arr[16] = PriorityID;
            arr[17] = SeriesUID;
            arr[18] = InstanceNumber;
            arr[19] = SOPInstanceUID;
            return arr;

        }
        #endregion

        #region ExtractStrings
        void ExtractStrings(string s1, out string s4, out string s5, out string s11, out string s12)
        {
            int ind;
            string s2, s3;
            ind = s1.IndexOf("//");
            s2 = s1.Substring(0, ind);
            s11 = s1.Substring(0, 4);
            s12 = s1.Substring(4, 4);
            s3 = s1.Substring(ind + 2);
            ind = s3.IndexOf(":");
            s4 = s3.Substring(0, ind);
            s5 = s3.Substring(ind + 1);
        }
        #endregion

        #region btnCheckTags_Click
        private void btnCheckTags_Click(object sender, EventArgs e)
        {
            string strCatchMessage = string.Empty;
            string strRetMessage = string.Empty;
            string strFilePath = string.Empty;
            string strFileName = string.Empty;
            string strSID = string.Empty;
            string strIsManual = string.Empty;
            string strNewFilename = string.Empty;
            string strNewFilePath = string.Empty;
            string strPrefix = string.Empty;
            string strExtn = string.Empty;

            string[] arrFolders = new string[0];
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string[] arr = new string[0];
            List<string> arrSUID = new List<string>();
            DicomDecoder dd = new DicomDecoder();


            StringBuilder sb = new StringBuilder();
            string strFTPDLFLDRTMP = "F:/VetChoice/FTP_DOWNLOAD_TEMP/Test";

            try
            {

                arrFiles = Directory.GetFiles(strFTPDLFLDRTMP);

                foreach (string strFile in arrFiles)
                {

                    pathElements = strFile.Replace("\\", "/").Split('/');
                    strFileName = pathElements[pathElements.Length - 1];

                    dd.DicomFileName = strFile;
                    List<string> str = dd.dicomInfo;

                    arr = new string[20];
                    arr = GetallTags(str);

                    sb.AppendLine("File Name : " + strFileName);
                    sb.AppendLine("----------------------------------------------------------------------------------------------------------------------");
                    sb.AppendLine("Study UID : " + arr[0]);
                    sb.AppendLine("Modality ID : " + arr[1]);
                    sb.AppendLine("Patient ID : " + arr[2]);
                    sb.AppendLine("Patient Name : " + arr[3]);
                    sb.AppendLine("Study Date/Time : " + arr[4]);
                    sb.AppendLine("Institution Name : " + arr[5]);
                    sb.AppendLine("Accession No. : " + arr[6]);
                    sb.AppendLine("Ref. Physician : " + arr[7]);
                    sb.AppendLine("Manufacturer : " + arr[8]);
                    sb.AppendLine("Station Name : " + arr[9]);
                    sb.AppendLine("Model : " + arr[10]);
                    sb.AppendLine("Modality AE Title : " + arr[11]);
                    sb.AppendLine("Reason : " + arr[12]);
                    sb.AppendLine("DOB : " + arr[13]);
                    sb.AppendLine("Patient Sex : " + arr[14]);
                    sb.AppendLine("Patient Age : " + arr[15]);
                    sb.AppendLine("Priority ID : " + arr[16]);
                    sb.AppendLine("Series UID : " + arr[17]);
                    sb.AppendLine("Instance No. : " + arr[18]);
                    sb.AppendLine("SOP Instance UID : " + arr[19]);
                    sb.AppendLine("==============================================================================================");

                    textBox1.Text += sb.ToString();

                    sb.Clear();

                }

                //arr[0] = StudyUID;
                //arr[1] = ModalityID;
                //arr[2] = PatientID;
                //arr[3] = StrPName;
                //arr[4] = studyDtTime;
                //arr[5] = InstitutionName;
                //arr[6] = AccnNo;
                //arr[7] = RefPhys;
                //arr[8] = Manufacturer;
                //arr[9] = StationName;
                //arr[10] = Model;
                //arr[11] = ModalityAETitle;
                //arr[12] = Reason;
                //arr[13] = bDt;
                //arr[14] = PatientSex;
                //arr[15] = PatientAge;
                //arr[16] = PriorityID;
                //arr[17] = SeriesUID;
                //arr[18] = InstanceNumber;
                //arr[19] = SOPInstanceUID;

            }
            catch (Exception expErr)
            {
                textBox1.Text = "Exception: " + expErr.Message;
            }

            finally
            {
                dd = null;
            }
        }
        #endregion

        #region btnGetStudyUID_Click
        private void btnGetStudyUID_Click(object sender, EventArgs e)
        {
            string strFTPDLFLDRTMP = "F:/VetChoice/FTP_DOWNLOAD_TEMP";
            DicomAttributeCollection dac;
            DicomUid uid;
            bool success;
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string strFileName = string.Empty;

            try
            {
                arrFiles = Directory.GetFiles(strFTPDLFLDRTMP);

                foreach (string strFile in arrFiles)
                {
                    DicomFile dfile = new DicomFile(strFile);
                    pathElements = strFile.Replace("\\", "/").Split('/');
                    strFileName = pathElements[pathElements.Length - 1];
                    //dfile.Load();

                    //dac = dfile.DataSet;
                    //var suid_idx = DicomTags.StudyInstanceUid;
                    //var suid = dac[suid_idx];
                    //success = dac[DicomTags.StudyInstanceUid].TryGetUid(0, out uid);
                    FileInfo fi = new FileInfo(strFile);
                    BinaryReader br = new BinaryReader(new FileStream(strFile, FileMode.Open, FileAccess.Read), Encoding.ASCII);

                    byte[] bt = new byte[fi.Length];
                    br.Read(bt, 0, bt.Length);

                    dfile = null;


                    //Reader reader = new Reader();.
                    //reader.SetFileName(strFile);
                    //bool ret = reader.Read();
                    ////if (!ret)
                    ////{
                    ////    return 1;
                    ////}
                    //File f = reader.GetFile();
                    //DataSet ds = f.GetDataSet();
                }

            }
            catch (Exception ex)
            {
                textBox1.Text = "Exception: " + ex.Message;
            }




            //if (success)
            //{
            //    // The attribute was present in the collection.  The variable uid
            //    // contains the SopInstanceUid extracted from the DICOM file
            //}
        }
        #endregion

        #region btnSUIDExe_Click
        private void btnSUIDExe_Click(object sender, EventArgs e)
        {
            string strFTPDLFLDRTMP = "F:/VetChoice/FTP_DOWNLOAD_TEMP/Test";
            string strEXE = "F:/VetChoice/EXES/SendFileToPACS/dcmdump.exe";
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string strFileName = string.Empty;
            string strProcOutput = string.Empty;
            string strProcError = string.Empty;
            string strProcMsg = string.Empty;
            string strSUID = string.Empty; string strSeries = string.Empty; string strSOPUID = string.Empty;
            string strSUIDText = string.Empty; string strSeriesText = string.Empty; string strSOPText = string.Empty;
            DicomAttributeCollection dac;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbDs = new StringBuilder();
            int i1 = 0;
            int i2 = 0;

            try
            {
                arrFiles = Directory.GetFiles(strFTPDLFLDRTMP);

                foreach (string strFile in arrFiles)
                {
                    DicomFile dfile = new DicomFile(strFile);
                    pathElements = strFile.Replace("\\", "/").Split('/');
                    strFileName = pathElements[pathElements.Length - 1];


                    Process ProcDCMDump = new Process();
                    ProcDCMDump.StartInfo.UseShellExecute = false;
                    ProcDCMDump.StartInfo.FileName = strEXE;
                    ProcDCMDump.StartInfo.Arguments = "+f " + strFile;
                    ProcDCMDump.StartInfo.RedirectStandardOutput = true;
                    ProcDCMDump.StartInfo.RedirectStandardError = true;
                    ProcDCMDump.Start();
                    strProcOutput = ProcDCMDump.StandardOutput.ReadToEnd();
                    strProcError = ProcDCMDump.StandardError.ReadToEnd();
                    strProcMsg = strProcOutput.Trim();
                    sbDs.Append(strProcMsg);

                    strSUIDText = strProcMsg.Substring(strProcMsg.IndexOf("(0020,000d)"), (strProcMsg.IndexOf("StudyInstanceUID") - strProcMsg.IndexOf("(0020,000d)")) + 1);
                    i1 = strSUIDText.IndexOf("[") + 1;
                    i2 = strSUIDText.IndexOf("]") - (strSUIDText.IndexOf("[") + 1);
                    strSUID = strSUIDText.Substring(strSUIDText.IndexOf("[") + 1, strSUIDText.IndexOf("]") - (strSUIDText.IndexOf("[") + 1));

                    strSeriesText = strProcMsg.Substring(strProcMsg.IndexOf("(0020,000e)"), (strProcMsg.IndexOf("SeriesInstanceUID") - strProcMsg.IndexOf("(0020,000e)")) + 1);
                    i1 = strSeriesText.IndexOf("[") + 1;
                    i2 = strSeriesText.IndexOf("]") - (strSeriesText.IndexOf("[") + 1);
                    strSeries = strSeriesText.Substring(strSeriesText.IndexOf("[") + 1, strSeriesText.IndexOf("]") - (strSeriesText.IndexOf("[") + 1));

                    strSOPText = strProcMsg.Substring(strProcMsg.IndexOf("(0008,0018)"), (strProcMsg.IndexOf("1 SOPInstanceUID") - strProcMsg.IndexOf("(0008,0018)")) + 1);
                    i1 = strSOPText.IndexOf("[") + 1;
                    i2 = strSOPText.IndexOf("]") - (strSOPText.IndexOf("[") + 1);
                    strSOPUID = strSOPText.Substring(strSOPText.IndexOf("[") + 1, strSOPText.IndexOf("]") - (strSOPText.IndexOf("[") + 1));

                    sb.AppendLine("File Name : " + strFileName);
                    sb.AppendLine("----------------------------------------------------------------------------------------------------------------------");
                    sb.AppendLine("Study UID : " + strSUID);
                    sb.AppendLine("Series UID : " + strSeries);
                    sb.AppendLine("SOP Class UID : " + strSOPUID);

                    sb.AppendLine("==============================================================================================");

                    textBox1.Text += sb.ToString();

                    sb.Clear();
                    sbDs.Clear();

                }

            }
            catch (Exception ex)
            {
                textBox1.Text += "Exception: " + ex.Message;
            }

        }
        #endregion

        #region btnPtrnFlList_Click
        private void btnPtrnFlList_Click(object sender, EventArgs e)
        {
            string strFTPDLFLDRTMP = "F:/VetChoice/FTP_DOWNLOAD_TEMP";
            string[] arrFiles = new string[0];
            StringBuilder sb = new StringBuilder();

            try
            {
                arrFiles = Directory.GetFiles(strFTPDLFLDRTMP, "*_S1DXXX*.*");
                foreach (string strFile in arrFiles)
                {
                    sb.AppendLine(strFile);
                    textBox1.Text += sb.ToString();
                    sb.Clear();
                }
            }
            catch (Exception ex)
            {
                textBox1.Text = "Exception: " + ex.Message;
            }
        }
        #endregion

        #region btnGenRTF_Click
        private void btnGenRTF_Click(object sender, EventArgs e)
        {
            string strText = textBox1.Text;
            string strRTF = string.Empty;
            string strFilePath = "F:/VetChoice/VETRIS_SOL_TFS/VETRIS/VETRISSchedulerERad8_64bit/RTFs/test.rtf";

            if (strText.Trim() != string.Empty)
            {
                //strRTF = ConvertToRtf(strText);
                strRTF = ConvertHtmlToText(strText);

                if (File.Exists(strFilePath)) File.Delete(strFilePath);
                //File.Create(strFilePath);
                //File.WriteAllText(strFilePath, strRTF);

                using (StreamWriter outputFile = new StreamWriter(strFilePath))
                {
                    //foreach (string line in lines)
                    //    outputFile.WriteLine(line);
                    outputFile.Write(strRTF);
                }
            }
        }
        private string ConvertToRtf(string text)
        {
            // using default template from wiki
            StringBuilder sb = new StringBuilder(@"{\rtf1\ansi\ansicpg1250\deff0{\fonttbl\f0\fswiss Helvetica;}\f0\pard ");
            foreach (char character in text)
            {
                if (character <= 0x7f)
                {
                    // escaping rtf characters
                    switch (character)
                    {
                        case '\\':
                        case '{':
                        case '}':
                            sb.Append('\\');
                            break;
                        case '\r':
                            sb.Append("\\par");
                            break;
                    }

                    sb.Append(character);
                }
                // converting special characters
                else
                {
                    sb.Append("\\u" + Convert.ToUInt32(character) + "?");
                }
            }
            sb.Append("}");
            return sb.ToString();
        }
        private string ConvertHtmlToText(string source)
        {

            string result;

            // Remove HTML Development formatting
            // Replace line breaks with space
            // because browsers inserts space
            result = source.Replace("\r", " ");
            // Replace line breaks with space
            // because browsers inserts space
            result = result.Replace("\n", " ");
            // Remove step-formatting
            result = result.Replace("\t", string.Empty);
            // Remove repeating speces becuase browsers ignore them
            result = System.Text.RegularExpressions.Regex.Replace(result,
                                                                  @"( )+", " ");

            // Remove the header (prepare first by clearing attributes)
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*head([^>])*>", "<head>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<( )*(/)( )*head( )*>)", "</head>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(<head>).*(</head>)", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // remove all scripts (prepare first by clearing attributes)
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*script([^>])*>", "<script>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<( )*(/)( )*script( )*>)", "</script>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            //result = System.Text.RegularExpressions.Regex.Replace(result,
            //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
            //         string.Empty,
            //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<script>).*(</script>)", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // remove all styles (prepare first by clearing attributes)
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*style([^>])*>", "<style>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<( )*(/)( )*style( )*>)", "</style>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(<style>).*(</style>)", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // insert tabs in spaces of <td> tags
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*td([^>])*>", "\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // insert line breaks in places of <BR> and <LI> tags
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*br( )*>", "\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*li( )*>", "\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // insert line paragraphs (double line breaks) in place
            // if <P>, <DIV> and <TR> tags
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*div([^>])*>", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*tr([^>])*>", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*p([^>])*>", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // Remove remaining tags like <a>, links, images,
            // comments etc - anything thats enclosed inside < >
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<[^>]*>", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // replace special characters:
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&nbsp;", " ",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&bull;", " * ",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&lsaquo;", "<",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&rsaquo;", ">",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&trade;", "(tm)",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&frasl;", "/",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<", "<",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @">", ">",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&copy;", "(c)",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&reg;", "(r)",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // Remove all others. More can be added, see
            // http://hotwired.lycos.com/webmonkey/reference/special_characters/
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&(.{2,6});", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);


            // make line breaking consistent
            result = result.Replace("\n", "\r");

            // Remove extra line breaks and tabs:
            // replace over 2 breaks with 2 and over 4 tabs with 4.
            // Prepare first to remove any whitespaces inbetween
            // the escaped characters and remove redundant tabs inbetween linebreaks
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)( )+(\r)", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\t)( )+(\t)", "\t\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\t)( )+(\r)", "\t\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)( )+(\t)", "\r\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // Remove redundant tabs
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)(\t)+(\r)", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // Remove multible tabs followind a linebreak with just one tab
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)(\t)+", "\r\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // Initial replacement target string for linebreaks
            string breaks = "\r\r\r";
            // Initial replacement target string for tabs
            string tabs = "\t\t\t\t\t";
            for (int index = 0; index < result.Length; index++)
            {
                result = result.Replace(breaks, "\r\r");
                result = result.Replace(tabs, "\t\t\t\t");
                breaks = breaks + "\r";
                tabs = tabs + "\t";
            }

            return result;
        }
        #endregion

        #region btnProcLogDB_Click
        private void btnProcLogDB_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            string strCatchMsg = string.Empty;
            DayEnd objDE = new DayEnd();


            try
            {
                //sb.AppendLine("Processing Log DB...");
                CoreCommon.doLog(strConfigPath, 5, "Day End Service", "Processing Log DB...", false);
                if (objDE.ProcessLogDBDayEnd(strConfigPath, 5, "Day End Service", ref strCatchMsg))
                {
                    CoreCommon.doLog(strConfigPath, 5, "Day End Service", "Log DB processed ", false);

                }
                else
                {
                    CoreCommon.doLog(strConfigPath, 5, "Day End Service", "ProcessLogDB()  - Error: " + strCatchMsg, true);
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, 5, "Day End Service", "ProcessLogDB() - Exception: " + ex.Message, true);
                //EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);

            }
            finally
            {
                objDE = null;
            }
        } 
        #endregion
    }

}
