using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using VETRISScheduler.DAL;

namespace VETRISScheduler.Core
{
    public class Notification
    {
        #region Constructor
        public Notification()
        {
        }
        #endregion

        #region Variables
        Guid EmailLogID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid SMSLogID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid FaxLogID = new Guid("00000000-0000-0000-0000-000000000000");
        string strMsgID = string.Empty;
        string strProcessed = "N";
        Guid StudyID = new Guid("00000000-0000-0000-0000-000000000000");
        string strSUID = string.Empty;
        Guid BillingAccountID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid BillingCycleID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid InvoiceID = new Guid("00000000-0000-0000-0000-000000000000");
        string strFileName = string.Empty;
        
        #endregion

        #region Properties
        public Guid EMAIL_LOG_ID
        {
            get { return EmailLogID; }
            set { EmailLogID = value; }
        }
        public Guid SMS_LOG_ID
        {
            get { return SMSLogID; }
            set { SMSLogID = value; }
        }
        public Guid FAX_LOG_ID
        {
            get { return FaxLogID; }
            set { FaxLogID = value; }
        }
        public string MESSAGE_SID
        {
            get { return strMsgID; }
            set { strMsgID = value; }
        }
        public string PROCESSED
        {
            get { return strProcessed; }
            set { strProcessed = value; }
        }
        public Guid STUDY_ID
        {
            get { return StudyID; }
            set { StudyID = value; }
        }
        public string STUDY_UID
        {
            get { return strSUID; }
            set { strSUID = value; }
        }
        public Guid BILLING_ACCOUNT_ID
        {
            get { return BillingAccountID; }
            set { BillingAccountID = value; }
        }
        public Guid BILLING_CYCLE_ID
        {
            get { return BillingCycleID; }
            set { BillingCycleID = value; }
        }
        public Guid INVOICE_ID
        {
            get { return InvoiceID; }
            set { InvoiceID = value; }
        }
        public string FILE_NAME
        {
            get { return strFileName; }
            set { strFileName = value; }
        }
        #endregion

        #region FetchNotificationSendingList
        public bool FetchNotificationSendingList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_notification_send_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "MailList";
                    ds.Tables[1].TableName = "SMSList";
                    ds.Tables[2].TableName = "FaxList";
                }
                bReturn = true;

            }
            catch (Exception expErr)
            {
                bReturn = false; CatchMessage = expErr.Message;
            }

            return bReturn;
        }
        #endregion

        #region UpdateMailSendingStatus
        public bool UpdateMailSendingStatus(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage=string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[3];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);


                SqlRecordParams[0] = new SqlParameter("@email_log_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = EmailLogID;
                SqlRecordParams[1] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[1].Direction = ParameterDirection.Output;
                SqlRecordParams[2] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[2].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_mail_sending_status_update", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[2].Value);
                strReturnMessage  = Convert.ToString(SqlRecordParams[1].Value).Trim();

                if (intReturnType == 0)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdateMailSendingStatus() - Error: " + strReturnMessage, true);
                }
                
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "-" + Convert.ToString(EmailLogID); }

            return bReturn;
        }
        #endregion

        #region UpdateSMSSendingStatus
        public bool UpdateSMSSendingStatus(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[5];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);


                SqlRecordParams[0] = new SqlParameter("@sms_log_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = SMSLogID;
                SqlRecordParams[1] = new SqlParameter("@message_sid", SqlDbType.NVarChar, 200); SqlRecordParams[1].Value = strMsgID;
                SqlRecordParams[2] = new SqlParameter("@processed", SqlDbType.NChar,1); SqlRecordParams[2].Value = strProcessed;
                SqlRecordParams[3] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_sms_sending_status_update", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[4].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[3].Value).Trim();

                if (intReturnType == 0)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdateSMSSendingStatus() - Error: " + strReturnMessage, true);
                }

                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "-" + Convert.ToString(SMSLogID); }

            return bReturn;
        }
        #endregion

        #region UpdateFaxSendingStatus
        public bool UpdateFaxSendingStatus(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);


                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = FaxLogID;
                SqlRecordParams[1] = new SqlParameter("@processed", SqlDbType.NChar, 1); SqlRecordParams[1].Value = strProcessed;
                SqlRecordParams[2] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[2].Direction = ParameterDirection.Output;
                SqlRecordParams[3] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_fax_sending_status_update", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[3].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[2].Value).Trim();

                if (intReturnType == 0)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdateFaxSendingStatus() - Error: " + strReturnMessage, true);
                }

                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "-" + Convert.ToString(FaxLogID); }

            return bReturn;
        }
        #endregion

        #region FetchUnassignedStudyList
        public bool FetchUnassignedStudyList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_notification_unassigned_study_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "StudyList";
                }
                bReturn = true;

            }
            catch (Exception expErr)
            {
                bReturn = false; CatchMessage = expErr.Message;
            }

            return bReturn;
        }
        #endregion

        #region CreateUnassignedStudyNotifications
        public bool CreateUnassignedStudyNotifications(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = StudyID;
                SqlRecordParams[1] = new SqlParameter("@study_uid", SqlDbType.NVarChar,100); SqlRecordParams[1].Value = strSUID;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_notification_unassigned_study_notification_create");
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region CreateStudyNotifications
        public bool CreateStudyNotifications(string ConfigPath, ref int EmailCount, ref int SMSCount, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                SqlRecordParams[0] = new SqlParameter("@email_count", SqlDbType.Int); SqlRecordParams[0].Direction = ParameterDirection.Output;
                SqlRecordParams[1] = new SqlParameter("@sms_count", SqlDbType.Int); SqlRecordParams[1].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_notification_rule_notification_create");
                EmailCount = Convert.ToInt32(SqlRecordParams[0].Value);
                SMSCount = Convert.ToInt32(SqlRecordParams[1].Value);
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region FetchInvoiceSendingList
        public bool FetchInvoiceSendingList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_notification_invoice_list_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Invoices";
                }
                bReturn = true;

            }
            catch (Exception expErr)
            {
                bReturn = false; CatchMessage = expErr.Message;
            }

            return bReturn;
        }
        #endregion

        #region CreateInvoiceNotifications
        public bool CreateInvoiceNotifications(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[6];

            try
            {

                SqlRecordParams[0] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = BillingAccountID;
                SqlRecordParams[1] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = BillingCycleID;
                SqlRecordParams[2] = new SqlParameter("@file_name", SqlDbType.NVarChar,4000); SqlRecordParams[2].Value = strFileName;
                SqlRecordParams[3] = new SqlParameter("@invoice_hdr_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = InvoiceID;
                SqlRecordParams[4] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_notification_invoice_sending_create", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[5].Value);
                if(intReturnType == 0)
                    bReturn = false;
                else
                    bReturn = true;
                ReturnMessage = Convert.ToString(SqlRecordParams[4].Value);
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region ReleaseReports
        public bool ReleaseReports(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_notification_release_reports");
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion
    }
}
