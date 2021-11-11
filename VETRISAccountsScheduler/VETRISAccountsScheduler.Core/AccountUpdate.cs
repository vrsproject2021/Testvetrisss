using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using VETRISAccountsScheduler.DAL;

namespace VETRISAccountsScheduler.Core
{
    public class AccountUpdate
    {
        #region Constructor
        public AccountUpdate()
        {
        }
        #endregion

        #region Variables
        Guid BillingAcctID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid BillingCycleID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid PaymentID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid RefundID = new Guid("00000000-0000-0000-0000-000000000000");
        string strQBName = string.Empty;
        string strDebtorID = string.Empty;
        string strPostType = string.Empty;
        string strPostID = string.Empty;
        string strIsSuccess = string.Empty;

        DateTime dtDayEnd = DateTime.Today;
        string strDayEndStat = "N";

        Guid RadID = new Guid("00000000-0000-0000-0000-000000000000");
        string strCreditorID = string.Empty;
        #endregion

        #region Properties
        public Guid BILLING_ACCOUNT_ID
        {
            get { return BillingAcctID; }
            set { BillingAcctID = value; }
        }
        public Guid BILLING_CYCLE_ID
        {
            get { return BillingCycleID; }
            set { BillingCycleID = value; }
        }
        public Guid PAYMENT_ID
        {
            get { return PaymentID; }
            set { PaymentID = value; }
        }
        public Guid REFUND_ID
        {
            get { return RefundID; }
            set { RefundID = value; }
        }
        public string QB_NAME
        {
            get { return strQBName; }
            set { strQBName = value; }
        }
        public string DEBTOR_ID
        {
            get { return strDebtorID; }
            set { strDebtorID = value; }
        }

        public DateTime DAY_END_DATE
        {
            get { return dtDayEnd; }
            set { dtDayEnd = value; }
        }
        public string DAY_END_STATUS
        {
            get { return strDayEndStat; }
            set { strDayEndStat = value; }
        }

        public Guid RADIOLOGIST_ID
        {
            get { return RadID; }
            set { RadID = value; }
        }
        public string CREDITOR_ID
        {
            get { return strCreditorID; }
            set { strCreditorID = value; }
        }
        
        public string POSTING_TYPE
        {
            get { return strPostType; }
            set { strPostType = value; }
        }
        public string POSTING_ID
        {
            get { return strPostID; }
            set { strPostID = value; }
        }
        public string IS_SUCCESS
        {
            get { return strIsSuccess; }
            set { strIsSuccess = value; }
        }
        #endregion

        #region AR

        #region FetchBillingAccountUpdateList
        public bool FetchBillingAccountUpdateList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_accounts_billing_accounts_to_update_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "BillingAccounts";
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

        #region FetchListForVoucher
        public bool FetchListForVoucher(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "invoicing_qb_voucher_data_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Invoice";
                    ds.Tables[1].TableName = "InvoiceReverse";
                    ds.Tables[2].TableName = "Payments";
                    ds.Tables[3].TableName = "Refunds";
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

        #region FetchInvoiceVoucher
        public bool FetchInvoiceVoucher(string ConfigPath, int ServiceID, string ServiceName,ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            int intReturnType = 0;
            string ReturnMessage = string.Empty;

            try
            {
                SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = BillingCycleID;
                SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = BillingAcctID;
                SqlRecordParams[2] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[2].Direction = ParameterDirection.Output;
                SqlRecordParams[3] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "invoicing_qb_posting_details_fetch",SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "VoucherHdr";
                    ds.Tables[1].TableName = "VoucherDtls";
                }
                intReturnType = Convert.ToInt32(SqlRecordParams[3].Value);
                ReturnMessage = Convert.ToString(SqlRecordParams[2].Value).Trim();

                if (intReturnType == 1) bReturn = true;
                else
                {
                    bReturn = false;
                    CoreCommon.doLog(ConfigPath, ServiceID, ServiceName, "FetchInvoiceVoucher() - Error: " + ReturnMessage, true);
                }

            }
            catch (Exception expErr)
            {
                bReturn = false; CatchMessage = expErr.Message;
            }

            return bReturn;
        }
        #endregion

        #region FetchReverseInvoiceVoucher
        public bool FetchReverseInvoiceVoucher(string ConfigPath, int ServiceID, string ServiceName, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            int intReturnType = 0;
            string ReturnMessage = string.Empty;

            try
            {
                SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = BillingCycleID;
                SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = BillingAcctID;
                SqlRecordParams[2] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[2].Direction = ParameterDirection.Output;
                SqlRecordParams[3] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "invoicing_qb_reverse_posting_details_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "VoucherHdr";
                    ds.Tables[1].TableName = "VoucherDtls";
                }
                intReturnType = Convert.ToInt32(SqlRecordParams[3].Value);
                ReturnMessage = Convert.ToString(SqlRecordParams[2].Value).Trim();

                if (intReturnType == 1) bReturn = true;
                else
                {
                    bReturn = false;
                    CoreCommon.doLog(ConfigPath, ServiceID, ServiceName, "FetchReverseInvoiceVoucher() - Error: " + ReturnMessage, true);
                }

            }
            catch (Exception expErr)
            {
                bReturn = false; CatchMessage = expErr.Message;
            }

            return bReturn;
        }
        #endregion

        #region FetchPaymentVoucher
        public bool FetchPaymentVoucher(string ConfigPath, int ServiceID, string ServiceName, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            int intReturnType = 0;
            string ReturnMessage = string.Empty;

            try
            {
                SqlRecordParams[0] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = BillingAcctID;
                SqlRecordParams[1] = new SqlParameter("@payment_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = PaymentID;
                SqlRecordParams[2] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[2].Direction = ParameterDirection.Output;
                SqlRecordParams[3] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_payments_qb_posting_details_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "VoucherHdr";
                    ds.Tables[1].TableName = "VoucherDtls";
                }
                intReturnType = Convert.ToInt32(SqlRecordParams[3].Value);
                ReturnMessage = Convert.ToString(SqlRecordParams[2].Value).Trim();

                if (intReturnType == 1) bReturn = true;
                else
                {
                    bReturn = false;
                    CoreCommon.doLog(ConfigPath, ServiceID, ServiceName, "FetchPaymentVoucher() - Error: " + ReturnMessage, true);
                }

            }
            catch (Exception expErr)
            {
                bReturn = false; CatchMessage = expErr.Message;
            }

            return bReturn;
        }
        #endregion

        #region FetchRefundVoucher
        public bool FetchRefundVoucher(string ConfigPath, int ServiceID, string ServiceName, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            int intReturnType = 0;
            string ReturnMessage = string.Empty;

            try
            {
                SqlRecordParams[0] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = BillingAcctID;
                SqlRecordParams[1] = new SqlParameter("@refund_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = RefundID;
                SqlRecordParams[2] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[2].Direction = ParameterDirection.Output;
                SqlRecordParams[3] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_refund_qb_posting_details_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "VoucherHdr";
                    ds.Tables[1].TableName = "VoucherDtls";
                }
                intReturnType = Convert.ToInt32(SqlRecordParams[3].Value);
                ReturnMessage = Convert.ToString(SqlRecordParams[2].Value).Trim();

                if (intReturnType == 1) bReturn = true;
                else
                {
                    bReturn = false;
                    CoreCommon.doLog(ConfigPath, ServiceID, ServiceName, "FetchRefundVoucher() - Error: " + ReturnMessage, true);
                }

            }
            catch (Exception expErr)
            {
                bReturn = false; CatchMessage = expErr.Message;
            }

            return bReturn;
        }
        #endregion

        #region UpdateBillingAccount
        public bool UpdateBillingAccount(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];

            try
            {
                
                SqlRecordParams[0] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = BillingAcctID;
                SqlRecordParams[1] = new SqlParameter("@debtor_id", SqlDbType.NVarChar, 20); SqlRecordParams[1].Value = strDebtorID;
                SqlRecordParams[2] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[2].Direction = ParameterDirection.Output;
                SqlRecordParams[3] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_billing_account_update", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[3].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[2].Value).Trim();
                

                if (intReturnType == 0)
                {
                    bReturn = false;
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdateBillingAccount() - Error: " + strReturnMessage, true);
                }
                else
                    bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "- Billing Account ID : " + Convert.ToString(BillingAcctID); }

            return bReturn;
        }
        #endregion

        #region UpdateInvoicePosting
        public bool UpdateInvoicePosting(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[7];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = BillingCycleID;
                SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = BillingAcctID;
                SqlRecordParams[2] = new SqlParameter("@posting_type", SqlDbType.NChar, 1); SqlRecordParams[2].Value = strPostType;
                SqlRecordParams[3] = new SqlParameter("@posting_id", SqlDbType.NVarChar, 20); SqlRecordParams[3].Value = strPostID;
                SqlRecordParams[4] = new SqlParameter("@is_success", SqlDbType.NChar, 1); SqlRecordParams[4].Value = strIsSuccess;
                //SqlRecordParams[5] = new SqlParameter("@TVP_data", SqlDbType.Structured); SqlRecordParams[5].Value = dtbl;
                SqlRecordParams[5] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "invoicing_qb_invoice_update", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[6].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[5].Value).Trim();

                if (intReturnType == 0)
                {
                    bReturn = false;
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdateInvoicePosting() - Error: " + strReturnMessage, true);
                }
                else
                    bReturn = true;
                
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "- Billing Account ID : " + Convert.ToString(BillingAcctID) + ",Billing Cycle ID:" + Convert.ToString(BillingCycleID); }

            return bReturn;
        }
        #endregion

        #region UpdatePaymentPosting
        public bool UpdatePaymentPosting(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[6];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@payment_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = PaymentID;
                SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = BillingAcctID;
                SqlRecordParams[2] = new SqlParameter("@posting_id", SqlDbType.NVarChar, 20); SqlRecordParams[2].Value = strPostID;
                SqlRecordParams[3] = new SqlParameter("@is_success", SqlDbType.NChar, 1); SqlRecordParams[3].Value = strIsSuccess;
                SqlRecordParams[4] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_payments_qb_posting_update", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[5].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[4].Value).Trim();

                if (intReturnType == 0)
                {
                    bReturn = false;
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdatePaymentPosting() - Error: " + strReturnMessage, true);
                }
                else
                    bReturn = true;

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "- Billing Account ID : " + Convert.ToString(BillingAcctID) + ",Payment ID:" + Convert.ToString(PaymentID); }

            return bReturn;
        }
        #endregion

        #region UpdateRefundPosting
        public bool UpdateRefundPosting(string ConfigPath, int ServiceID, string strSvcName,ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[6];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@refund_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = RefundID;
                SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = BillingAcctID;
                SqlRecordParams[2] = new SqlParameter("@posting_id", SqlDbType.NVarChar, 20); SqlRecordParams[2].Value = strPostID;
                SqlRecordParams[3] = new SqlParameter("@is_success", SqlDbType.NChar, 1); SqlRecordParams[3].Value = strIsSuccess;
                //SqlRecordParams[4] = new SqlParameter("@TVP_data", SqlDbType.Structured); SqlRecordParams[4].Value = dtbl;
                SqlRecordParams[4] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_refunds_qb_posting_update", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[5].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[4].Value).Trim();

                if (intReturnType == 0)
                {
                    bReturn = false;
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdateRefundPosting() - Error: " + strReturnMessage, true);
                }
                else
                    bReturn = true;

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "- Billing Account ID : " + Convert.ToString(BillingAcctID) + ",Refund ID:" + Convert.ToString(RefundID); }

            return bReturn;
        }
        #endregion

        #endregion

        #region DayEnd

        #region CheckDayEndStatus
        public bool CheckDayEndStatus(string ConfigPath, int ServiceID, string ServiceName, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];
            int intExecReturn = 0;

            try
            {
                SqlRecordParams[0] = new SqlParameter("@day_end_date", SqlDbType.DateTime); SqlRecordParams[0].Value = dtDayEnd;
                SqlRecordParams[1] = new SqlParameter("@status", SqlDbType.VarChar, 500); SqlRecordParams[1].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "day_end_accounts_posting_status_check", SqlRecordParams);
                strDayEndStat = Convert.ToString(SqlRecordParams[1].Value).Trim();
                bReturn = true;
            }
            catch (Exception expErr)
            {
                bReturn = false; CatchMessage = expErr.Message;
            }

            return bReturn;
        }
        #endregion

        #region SaveDayEndAccountPosting
        public bool SaveDayEndAccountPosting(string ConfigPath, int ServiceID, string strSvcName, DataTable dtbl, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];

            try
            {

                SqlRecordParams[0] = new SqlParameter("@day_end_date", SqlDbType.DateTime); SqlRecordParams[0].Value = dtDayEnd;
                SqlRecordParams[1] = new SqlParameter("@TVP_data", SqlDbType.Structured); SqlRecordParams[1].Value = dtbl;
                SqlRecordParams[2] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[2].Direction = ParameterDirection.Output;
                SqlRecordParams[3] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "day_end_accounts_posting_data_save", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[3].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[2].Value).Trim();


                if (intReturnType == 0)
                {
                    bReturn = false;
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "SaveDayEndAccountPosting() - Error: " + strReturnMessage, true);
                }
                else
                    bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region FetchDayEndVoucherPostingReport
        public bool FetchDayEndVoucherPostingReport(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@day_end_date", SqlDbType.DateTime); SqlRecordParams[0].Value = dtDayEnd;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "day_end_accounts_posting_qb_report_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "VoucherPosted";
                    ds.Tables[1].TableName = "AcctSummary";
                    ds.Tables[2].TableName = "AcctSummaryInv";
                    ds.Tables[3].TableName = "AcctSummaryPmt";
                    ds.Tables[4].TableName = "InvoiceApproved";
                    ds.Tables[5].TableName = "InvoiceDispproved";
                    ds.Tables[6].TableName = "PaymentReceived";
                    ds.Tables[7].TableName = "PaymentRefund";
                    ds.Tables[8].TableName = "PaymentMade";
                    ds.Tables[9].TableName = "PostFailed";
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

        #region FetchDayeEndMailSendingParameters
        public bool FetchDayeEndMailSendingParameters(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
             
            try
            {
              
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "day_end_account_post_mail_sending_params_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "MailParams";
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

        #region UpdateDayEndProcessStatus
        public bool UpdateDayEndProcessStatus(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[3];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@day_end_date", SqlDbType.DateTime); SqlRecordParams[0].Value = dtDayEnd;
                SqlRecordParams[1] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[1].Direction = ParameterDirection.Output;
                SqlRecordParams[2] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[2].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "day_end_accounts_posting_proc_status_update", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[2].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[1].Value).Trim();

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #endregion


        #region AP

        #region FetchRadiologistList
        public bool FetchRadiologistList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_accounts_radiologist_to_update_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Radiologists";
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

        #region UpdateRadiologist
        public bool UpdateRadiologist(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];

            try
            {

                SqlRecordParams[0] = new SqlParameter("@radiologist_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = RadID;
                SqlRecordParams[1] = new SqlParameter("@creditor_id", SqlDbType.NVarChar, 20); SqlRecordParams[1].Value = strCreditorID;
                SqlRecordParams[2] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[2].Direction = ParameterDirection.Output;
                SqlRecordParams[3] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_radiologist_update", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[3].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[2].Value).Trim();

                if (intReturnType == 0)
                {
                    bReturn = false;
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdateRadiologist() - Error: " + strReturnMessage, true);
                }
                else
                    bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "- Radiologist ID : " + Convert.ToString(RadID); }

            return bReturn;
        }
        #endregion
        #endregion
    }
}
