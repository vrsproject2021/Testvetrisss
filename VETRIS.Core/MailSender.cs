using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using System.Web;

namespace VETRIS.Core
{
    public class MailSender
    {
        #region Constructor
        public MailSender()
        {
        }
        #endregion

        #region Member Variables
        private string mMailSender = string.Empty;
        private string mMailFrom = string.Empty;
        private string mMailTo = string.Empty;
        private string mMailCC = string.Empty;
        private string mMailSubject = string.Empty;
        private string mMailBody = string.Empty;
        private string mSMTPServer = string.Empty;
        private int mPortNo = 0;
        private string mEmailUserId = string.Empty;
        private string mEmailPwd = string.Empty;
        private string mDecryptPwd = "Y";
        private string mPropertyFilePath = string.Empty;
        private string mPropertyId = string.Empty;
        private bool mEmbedContent = false;
        private int mAttachments = 0;
        private string[] mAttachmentFile = null;
        private string[] mAttachmentFileName = null;
        private bool mSSLEnabled = false;
        private bool mIsBodyHtml = false;
        private string appcopd = string.Empty;
        #endregion

        #region Properties
        public string MailSenderName
        {
            set { mMailSender = value; }
            get { return mMailSender; }
        }
        public string MailFrom
        {
            set { mMailFrom = value; }
            get { return mMailFrom; }
        }
        public string MailTo
        {
            set { mMailTo = value; }
            get { return mMailTo; }
        }
        public string MailCC
        {
            set { mMailCC = value; }
            get { return mMailCC; }
        }
        public string MailSubject
        {
            set { mMailSubject = value; }
            get { return mMailSubject; }
        }
        public string MailBody
        {
            set { mMailBody = value; }
            get { return mMailBody; }
        }
        public bool IsMailBodyHTML
        {
            set { mIsBodyHtml = value; }
            get { return mIsBodyHtml; }
        }
        public string SMTPServer
        {
            set { mSMTPServer = value; }
            get { return mSMTPServer; }
        }
        public string PropertyId
        {
            set { mPropertyId = value; }
            get { return mPropertyId; }
        }
        public string PropertyFilePath
        {
            set { mPropertyFilePath = value; }
            get { return mPropertyFilePath; }
        }
        public bool EmbedContent
        {
            set { mEmbedContent = value; }
            get { return mEmbedContent; }
        }
        public string MailServer
        {
            set { mSMTPServer = value; }
            get { return mSMTPServer; }
        }
        public int MailServerPortNo
        {
            set { mPortNo = value; }
            get { return mPortNo; }
        }
        public string MailServerUserId
        {
            set { mEmailUserId = value; }
            get { return mEmailUserId; }
        }
        public string MailServerPassword
        {
            set { mEmailPwd = value; }
            get { return mEmailPwd; }
        }
        public string DecryptPassword
        {
            set { mDecryptPwd = value; }
            get { return mDecryptPwd; }
        }
        public bool MailServerSSLEnabled
        {
            set { mSSLEnabled = value; }
            get { return mSSLEnabled; }
        }
        public int Attachments
        {
            set { mAttachments = value; }
            get { return mAttachments; }
        }
        public string[] AttachedFile
        {
            set { mAttachmentFile = value; }
            get { return mAttachmentFile; }
        }
        public string[] AttachedFileName
        {
            set { mAttachmentFileName = value; }
            get { return mAttachmentFileName; }
        }
        #endregion

        #region SendMail
        public bool SendMail(ref string CatchMessage)
        {
            bool blnReturn = false;
            MailMessage objMailMsg = null;
            MailAddress objMailAddressFrom = null;
            MailAddress objMailAddressTO = null;
            MailAddress objMailAddressCC = null;

            SmtpClient objSMTPClient = null;

            string[] streMailTo = null;
            string[] streMailCC = null;

            try
            {
                objMailAddressFrom = new MailAddress(mMailFrom);
                objMailMsg = new MailMessage();
                objMailMsg.From = objMailAddressFrom;
                
                if (MailTo.ToString().Trim().Contains(";"))
                {
                    streMailTo = MailTo.ToString().Trim().Split(new char[] { ';' });
                    
                    for (int i = 0; i < streMailTo.Length; i++)
                    {
                        objMailAddressTO = new MailAddress(streMailTo[i].ToString().Trim());
                        objMailMsg.To.Add(objMailAddressTO);
                    }
                }
                else
                {
                    objMailAddressTO = new MailAddress(MailTo.Trim());
                    objMailMsg.To.Add(objMailAddressTO);
                }

                if (MailCC.ToString().Trim() != "")
                {
                    if (MailCC.ToString().Trim().Contains(";"))
                    {
                        streMailCC = MailCC.ToString().Trim().Split(';');

                        for (int j = 0; j < streMailCC.Length; j++)
                        {
                            objMailAddressCC = new MailAddress(streMailCC[j].ToString().Trim());
                            objMailMsg.CC.Add(objMailAddressCC);
                        }
                    }
                    else
                    {
                        objMailAddressCC = new MailAddress(MailCC);
                        objMailMsg.CC.Add(objMailAddressCC);
                    }
                }

                if (mEmbedContent)
                {
                    AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mMailBody, null, "text/html");
                    if (File.Exists(mPropertyFilePath + "/logo.jpg"))
                    {
                        LinkedResource imagelinkLogo = new LinkedResource(mPropertyFilePath + "/logo.jpg", "image/jpeg");
                        imagelinkLogo.ContentId = "logo";
                        imagelinkLogo.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                        htmlView.LinkedResources.Add(imagelinkLogo);
                    }
                    if (File.Exists(mPropertyFilePath + "/map.jpg"))
                    {
                        LinkedResource imagelinkMap = new LinkedResource(mPropertyFilePath + "/map.jpg", "image/jpeg");
                        imagelinkMap.ContentId = "map";
                        imagelinkMap.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                        htmlView.LinkedResources.Add(imagelinkMap);
                    }
                    objMailMsg.AlternateViews.Add(htmlView);
                }
                objMailMsg.IsBodyHtml = mIsBodyHtml;
                objMailMsg.Subject = MailSubject;
                objMailMsg.Body = MailBody;
                objMailMsg.Priority = MailPriority.Normal;

                //if (mAttachmentFile.Trim() != string.Empty)
                if (mAttachments > 0)
                {
                    for (int i = 0; i < mAttachments; i++)
                    {
                        System.Net.Mail.Attachment att = new Attachment(mAttachmentFile[i]);
                        objMailMsg.Attachments.Add(att);
                        objMailMsg.Attachments[i].Name = mAttachmentFileName[i];
                    }
                }
                objSMTPClient = new SmtpClient();
                objSMTPClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSMTPClient.UseDefaultCredentials = false;
                if (mDecryptPwd == "Y") appcopd = CoreCommon.DecryptString(mEmailPwd);
                else appcopd = mEmailPwd;
                NetworkCredential oBasicAuth = new NetworkCredential(mEmailUserId, appcopd);
                objSMTPClient.Host = SMTPServer;
                
                objSMTPClient.Credentials = oBasicAuth;
                objSMTPClient.Port = mPortNo;
                objSMTPClient.EnableSsl = mSSLEnabled;
                objSMTPClient.Send(objMailMsg);


                blnReturn = true;

            }
            catch (Exception ex)
            {
                CatchMessage = ex.Message;
                return false;
            }
            finally
            {
                objMailMsg.Dispose();
                objMailMsg = null;
                objMailAddressFrom = null;
                objMailAddressTO = null;
                objMailAddressCC = null;
                objSMTPClient = null;
                streMailTo = null;
                streMailCC = null;

            }
            return blnReturn;
        }
        #endregion
    }
}
