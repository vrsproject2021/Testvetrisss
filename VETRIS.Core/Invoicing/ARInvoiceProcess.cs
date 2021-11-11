using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using VETRIS.DAL;

namespace VETRIS.Core.Invoicing
{
    public class ARInvoiceProcess
    {
        #region Constructor
        public ARInvoiceProcess()
        {

        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        Guid CycleID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid AccountID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid InstID = new Guid("00000000-0000-0000-0000-000000000000");
        string strStatus = "A";
        string strUserName = string.Empty;
        string strXMLInst = string.Empty;
        string strXMLStudy = string.Empty;
        string strXMLAcct = string.Empty;
        int intProcCount = 0;
        int intAppvCount = 0;
        string strMailType = string.Empty;
        Guid SessionID = new Guid("00000000-0000-0000-0000-000000000000");
        #endregion

        #region Properties
        public int MENU_ID
        {
            get { return intMenuID; }
            set { intMenuID = value; }
        }
        public Guid USER_ID
        {
            get { return UserID; }
            set { UserID = value; }
        }
        public int USER_ROLE_ID
        {
            get { return intUserRoleID; }
            set { intUserRoleID = value; }
        }
        public Guid USER_SESSION_ID
        {
            get { return SessionID; }
            set { SessionID = value; }
        }
        public Guid BILLING_CYCLE_ID
        {
            get { return CycleID; }
            set { CycleID = value; }
        }
        public Guid BILLING_ACCOUNT_ID
        {
            get { return AccountID; }
            set { AccountID = value; }
        }
        public Guid INSTITUTION_ID
        {
            get { return InstID; }
            set { InstID = value; }
        }
        public string STATUS
        {
            get { return strStatus; }
            set { strStatus = value; }
        }
        public int RECORD_PROCESSED_COUNT
        {
            get { return intProcCount; }
            set { intProcCount = value; }
        }
        public int RECORD_APPROVED_COUNT
        {
            get { return intAppvCount; }
            set { intAppvCount = value; }
        }
        public string USER_NAME
        {
            get { return strUserName; }
            set { strUserName = value; }
        }
        public string MAIL_TYPE
        {
            get { return strMailType; }
            set { strMailType = value; }
        }
        #endregion

        #region FetchParameters
        public bool FetchParameters(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserID;
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_invoicing_params_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Cycle";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchUnprocessedBillingAccountList
        public bool FetchUnprocessedBillingAccountList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = CycleID;
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_invoice_process_billing_account_list_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "BillingAccounts";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchProcessedBillingAccountList
        public bool FetchProcessedBillingAccountList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = CycleID;
                SqlRecordParams[1] = new SqlParameter("@approved", SqlDbType.NChar, 1); SqlRecordParams[1].Value = strStatus;
                SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = intMenuID;
                SqlRecordParams[3] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = UserID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_invoice_process_billing_account_processed_list_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "BillingAccounts";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region ProcessInvoice
        public bool ProcessInvoice(string ConfigPath, ARBillingAccountList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intReturnValue = 0; int intExecReturn = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[8];

            if (ValidateRecord(ArrObj, ref ReturnMessage))
            {
                try
                {
                    if ((GenerateBAXML(ArrObj, ref CatchMessage)))
                    {
                        SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = CycleID;
                        SqlRecordParams[1] = new SqlParameter("@xml_account", SqlDbType.NText); SqlRecordParams[1].Value = strXMLAcct;
                        SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.BigInt); SqlRecordParams[2].Value = intMenuID;
                        SqlRecordParams[3] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = UserID;
                        SqlRecordParams[4] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = SessionID;
                        SqlRecordParams[5] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[5].Direction = ParameterDirection.Output;
                        SqlRecordParams[6] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[6].Direction = ParameterDirection.Output;
                        SqlRecordParams[7] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[7].Direction = ParameterDirection.Output;

                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_invoice_process", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[7].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[5].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[6].Value).Trim();

                    }
                    else
                        bReturn = false;
                }
                catch (Exception expErr)
                { bReturn = false; CatchMessage = expErr.Message; }
            }
            else
                bReturn = false;

            return bReturn;
        }
        #endregion

        #region ValidateProcess
        private bool ValidateRecord(ARBillingAccountList[] ArrObj, ref string ReturnMessage)
        {
            bool bReturn = true;



            if (ArrObj.Length == 0)
            {
               
                ReturnMessage += "508";
            }
            

            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }


        #endregion

        #region GenerateBAXML
        private bool GenerateBAXML(ARBillingAccountList[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<account>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<id><![CDATA[" + ArrObj[i].ID.ToString() + "]]></id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</account>");
                    strXMLAcct = sbXML.ToString();


                }
                bReturn = true;
            }
            catch (Exception LexpErr)
            {
                bReturn = false;
                CatchMessage = LexpErr.Message;
            }
            return bReturn;
        }
        #endregion

        #region FetchProcessedRecords
        public bool FetchProcessedRecords(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[5];

            try
            {
                
                    SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = CycleID;
                    SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = AccountID;
                    SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = intMenuID;
                    SqlRecordParams[3] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = UserID;
                    SqlRecordParams[4] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = SessionID;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "ar_invoice_process_view_fetch", SqlRecordParams);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "InvoiceHdr";
                        ds.Tables[1].TableName = "InvoiceDtlsHdr";
                        ds.Tables[2].TableName = "InvoiceDtls";
                        bReturn = true;
                    }
                    else
                        bReturn = false;
               
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region ApproveInvoice
        public bool ApproveInvoice(string ConfigPath, ARInvoiceBillingAccountList[] ArrAcctObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateApprove(ArrAcctObj, ref ReturnMessage))
            {
                if (GenerateAccountXML(ArrAcctObj, ref CatchMessage))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[8];
                        SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = CycleID;
                        SqlRecordParams[1] = new SqlParameter("@xml_account", SqlDbType.NText); SqlRecordParams[1].Value = strXMLAcct.Trim();
                        SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = intMenuID;
                        SqlRecordParams[3] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = UserID;
                        SqlRecordParams[4] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = SessionID;
                        SqlRecordParams[5] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[5].Direction = ParameterDirection.Output;
                        SqlRecordParams[6] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[6].Direction = ParameterDirection.Output;
                        SqlRecordParams[7] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[7].Direction = ParameterDirection.Output;

                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_invoice_approve", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[7].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[5].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[6].Value).Trim();


                    }
                    catch (Exception expErr)
                    { bReturn = false; CatchMessage = expErr.Message; }
                }
                else
                {
                    bReturn = false;

                }

            }
            else
            {
                bReturn = false;
            }

            return bReturn;
        }
        #endregion

        #region ValidateApprove
        private bool ValidateApprove(ARInvoiceBillingAccountList[] ArrAcctObj, ref string ReturnMessage)
        {
            bool bReturn = true;


            if (CycleID == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                ReturnMessage = "229";
            }

            if (ArrAcctObj.Length == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "235";
            }
            
            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }


        #endregion

        #region GenerateAccountXML
        private bool GenerateAccountXML(ARInvoiceBillingAccountList[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<account>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<billing_account_id><![CDATA[" + ArrObj[i].ID.ToString() + "]]></billing_account_id>");
                        sbXML.Append("<total_amount>" + ArrObj[i].TOTAL_AMOUNT.ToString() + "</total_amount>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</account>");
                    strXMLAcct = sbXML.ToString();


                }
                bReturn = true;
            }
            catch (Exception LexpErr)
            {
                bReturn = false;
                CatchMessage = LexpErr.Message;
            }
            return bReturn;
        }
        #endregion

        #region Mail Sending

        #region FetchMailParameters
        public bool FetchMailParameters(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            try
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[5];
                SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = CycleID;
                SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = AccountID;
                SqlRecordParams[2] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = InstID;
                SqlRecordParams[3] = new SqlParameter("@mail_type", SqlDbType.NChar, 1); SqlRecordParams[3].Value = strMailType;
                SqlRecordParams[4] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = UserID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "invoicing_process_mail_param_fetch", SqlRecordParams);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "BillingCycle";
                    ds.Tables[1].TableName = "MaitTo";
                    ds.Tables[2].TableName = "MaitCC";
                    ds.Tables[3].TableName = "Sender";
                    ds.Tables[4].TableName = "Company";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }
            finally { ds.Dispose(); }


            return bReturn;
        }
        #endregion

        #endregion
    }

    public class ARBillingAccountList
    {
        #region Constructor
        public ARBillingAccountList()
        {
        }
        #endregion

        #region Variables
        Guid Id = Guid.Empty;
        #endregion

        #region Properties
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
        #endregion
    }

    public class ARInvoiceBillingAccountList
    {
        #region Constructor
        public ARInvoiceBillingAccountList()
        {
        }
        #endregion

        #region Variables
        Guid Id = Guid.Empty;
        double dblTotAmt = 0;
        #endregion

        #region Properties
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public double TOTAL_AMOUNT
        {
            get { return dblTotAmt; }
            set { dblTotAmt = value; }
        }
        #endregion
    }

}
