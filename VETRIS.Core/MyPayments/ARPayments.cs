using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VETRIS.Core.MyPayments
{
    public class ARPayments
    {

        #region Variables
        private Guid _userId = new Guid("00000000-0000-0000-0000-000000000000");
        private Guid _cretated_by = new Guid("00000000-0000-0000-0000-000000000000");
        private Guid _billing_account_id = new Guid("00000000-0000-0000-0000-000000000000");
        private Guid _invoice_id = new Guid("00000000-0000-0000-0000-000000000000");
        private Guid _id = new Guid("00000000-0000-0000-0000-000000000000");

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        int intMenuID = 0;
        int intMode = 1;
        string strUserName = string.Empty;
        #endregion

        public ARPayments()
        {
            this.Adjustments = new List<PaymentAdjustmentRow>();
        }

        public Guid InvoiceId { get { return _invoice_id; } set { _invoice_id = value; } }
        public Guid UserID { get { return _userId; } set { _userId = value; } }
        public string UserName { get; set; }
        public int Mode { get { return intMode; } set { intMode = value; } }
        public int MenuID { get { return intMenuID; } set { intMenuID = value; } }
        public string PaymentRef { get; set; }
        public string ExternalPaymentRef { get; set; }

        #region Table properties
        public Guid id { get { return _id; } set { _id = value; } }
        public Guid billing_account_id { get { return _billing_account_id; } set { _billing_account_id = value; } }
        public string payment_mode { get; set; }
        public string payref_no { get; set; }
        public DateTime payref_date { get; set; }
        public string processing_ref_no { get; set; }
        public DateTime processing_ref_date { get; set; }
        public string processing_pg_name { get; set; }
        public string processing_status { get; set; }
        public string payment_tool { get; set; }
        public string auth_code { get; set; }
        public string cvv_response { get; set; }
        public string avs_response { get; set; }
        public string remarks { get; set; }
        public decimal? payment_amount { get; set; }
        public Guid created_by { get { return _cretated_by; } set { _cretated_by = value; } }
        public DateTime date_created { get; set; }
        public Guid? updated_by { get; set; }
        public DateTime? date_updated { get; set; }
        public bool isadjusted { get; set; }
        #endregion

        #region Extra fields
        public string billing_account_name { get; set; }
        #endregion

        #region Adjustments
        public List<PaymentAdjustmentRow> Adjustments { get; set; }
        public string ToAdjustmentsXML()
        {
            if (Adjustments.Count == 0) return null;
            var xml = "<adjustments>";
            int i = 0;
            foreach (var item in Adjustments)
            {
                xml += item.ToXml(++i);
            }
            xml += "</adjustments>";
            return xml;
        }
        #endregion

        #region Account Info
        public string name { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string email_id { get; set; }
        public string contact_no { get; set; }
        public Guid vault_id { get; set; }
        #endregion

        #region FetchParameters
        public bool FetchParameters(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;


            try
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[1];
                SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier);
                if (UserID.Equals(new Guid("00000000-0000-0000-0000-000000000000")))
                    SqlRecordParams[0].Value = DBNull.Value;
                else
                    SqlRecordParams[0].Value = UserID;
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "mypayment_fetch_params", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Account";
                    ds.Tables[1].TableName = "User";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRecord(ref ReturnMessage))
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[23];

                try
                {

                    SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = id; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
                    SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = billing_account_id;
                    SqlRecordParams[2] = new SqlParameter("@payment_mode", SqlDbType.NVarChar, 1); SqlRecordParams[2].Value = payment_mode;
                    SqlRecordParams[3] = new SqlParameter("@payref_no", SqlDbType.NVarChar, 50); SqlRecordParams[3].Value = payref_no; SqlRecordParams[3].Direction = ParameterDirection.InputOutput;
                    SqlRecordParams[4] = new SqlParameter("@payref_date", SqlDbType.DateTime); SqlRecordParams[4].Value = payref_date; SqlRecordParams[4].Direction = ParameterDirection.InputOutput;
                    SqlRecordParams[5] = new SqlParameter("@processing_ref_no", SqlDbType.NVarChar, 100); SqlRecordParams[5].Value = processing_ref_no;
                    SqlRecordParams[6] = new SqlParameter("@processing_ref_date", SqlDbType.DateTime); SqlRecordParams[6].Value = processing_ref_date;
                    SqlRecordParams[7] = new SqlParameter("@processing_pg_name", SqlDbType.NVarChar, 50); SqlRecordParams[7].Value = processing_pg_name;
                    SqlRecordParams[8] = new SqlParameter("@processing_status", SqlDbType.NChar, 1); SqlRecordParams[8].Value = processing_status;
                    SqlRecordParams[9] = new SqlParameter("@payment_amount", SqlDbType.Money); SqlRecordParams[9].Value = payment_amount;
                    SqlRecordParams[10] = new SqlParameter("@payment_tool", SqlDbType.NChar, 1); SqlRecordParams[10].Value = payment_tool;

                    SqlRecordParams[11] = new SqlParameter("@auth_code", SqlDbType.NVarChar, 50); SqlRecordParams[11].Value = auth_code;
                    SqlRecordParams[12] = new SqlParameter("@cvv_response", SqlDbType.NVarChar, 50); SqlRecordParams[12].Value = cvv_response;
                    SqlRecordParams[13] = new SqlParameter("@avs_response", SqlDbType.NVarChar, 50); SqlRecordParams[13].Value = avs_response;
                    SqlRecordParams[14] = new SqlParameter("@payment_tool_holder_name", SqlDbType.NVarChar, 100); SqlRecordParams[14].Value = name;


                    var strAdjustments = ToAdjustmentsXML();
                    SqlRecordParams[15] = new SqlParameter("@xml_adjustments", SqlDbType.NText);
                    if (string.IsNullOrEmpty(strAdjustments)) SqlRecordParams[14].Value = DBNull.Value;
                    else SqlRecordParams[15].Value = strAdjustments;

                    SqlRecordParams[16] = new SqlParameter("@remarks", SqlDbType.NVarChar, 150); SqlRecordParams[16].Value = remarks;
                    SqlRecordParams[17] = new SqlParameter("@vault_id", SqlDbType.UniqueIdentifier); SqlRecordParams[17].Value = vault_id;
                    
                    SqlRecordParams[18] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[18].Value = UserID;
                    SqlRecordParams[19] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[19].Value = intMenuID;
                    SqlRecordParams[20] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[20].Direction = ParameterDirection.Output;
                    SqlRecordParams[21] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[21].Direction = ParameterDirection.Output;
                    SqlRecordParams[22] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[22].Direction = ParameterDirection.Output;


                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_payments_save", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[22].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;

                    strUserName = Convert.ToString(SqlRecordParams[20].Value).Trim();
                    ReturnMessage = Convert.ToString(SqlRecordParams[21].Value);

                    id = new Guid(Convert.ToString(SqlRecordParams[0].Value));
                    payref_no = Convert.ToString(SqlRecordParams[3].Value);
                    payref_date = Convert.ToDateTime(SqlRecordParams[4].Value);
                }
                catch (Exception expErr)
                { bReturn = false; CatchMessage = expErr.Message; }

            }
            else
            {
                bReturn = false;
            }

            return bReturn;
        }

        public bool ValidatePayment(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[7];

            try
            {

                SqlRecordParams[0] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = billing_account_id;

                var strAdjustments = ToAdjustmentsXML();
                SqlRecordParams[1] = new SqlParameter("@xml_adjustments", SqlDbType.NText);
                if (string.IsNullOrEmpty(strAdjustments)) SqlRecordParams[1].Value = DBNull.Value;
                else SqlRecordParams[1].Value = strAdjustments;

                SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[3].Value = intMenuID;
                SqlRecordParams[4] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;


                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_payments_validate_adjustments", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[6].Value);
                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;

                strUserName = Convert.ToString(SqlRecordParams[4].Value).Trim();
                ReturnMessage = Convert.ToString(SqlRecordParams[5].Value);

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            

            return bReturn;
        }
        #endregion

        #region ValidateRecord
        private bool ValidateRecord(ref string ReturnMessage)
        {
            bool bReturn = true;

            if (billing_account_id == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                ReturnMessage = "225";
            }
            if (string.IsNullOrEmpty(processing_ref_no.Trim()))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "316";
               
            }
            if (payment_amount <= 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "318";
            }
            if (string.IsNullOrEmpty(processing_ref_no.Trim()))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "317";
            }

            //if (!(payment_mode == "0" || payment_mode == "1"))
            //{
            //    ReturnMessage = "Incorrect Payment mode.";
            //    return false;
            //}
            
            
            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }
        #endregion

        #region LoadDetails
        public bool LoadDetails(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[6];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = id;
                SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = billing_account_id;
                SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = intMenuID;
                SqlRecordParams[3] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = UserID;
                SqlRecordParams[4] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_payments_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";
                    ds.Tables[1].TableName = "Accounts";
                    ds.Tables[2].TableName = "Invoices";
                    ds.Tables[3].TableName = "Payments";
                    if (id.ToString() != new Guid("00000000-0000-0000-0000-000000000000").ToString())
                    {
                        #region Details
                        foreach (DataRow dr in ds.Tables["Details"].Rows)
                        {
                            id = new Guid(dr["id"].ToString());
                            billing_account_id = new Guid(dr["billing_account_id"].ToString());
                            payment_mode = Convert.ToString(dr["payment_mode"]).Trim();
                            payref_no = Convert.ToString(dr["payref_no"]).Trim();
                            payref_date = Convert.ToDateTime(dr["payref_date"]);
                            processing_ref_no = Convert.ToString(dr["processing_ref_no"]).Trim();
                            processing_ref_date = Convert.ToDateTime(dr["processing_ref_date"]);
                            processing_pg_name = Convert.ToString(dr["processing_pg_name"]).Trim();
                            processing_status = Convert.ToString(dr["processing_status"]).Trim();
                            payment_amount = Convert.ToDecimal(dr["payment_amount"]);
                            remarks = Convert.ToString(dr["remarks"]);
                            created_by = new Guid(dr["created_by"].ToString());
                            date_created = Convert.ToDateTime(dr["date_created"]);
                            updated_by = dr["updated_by"] == DBNull.Value ? (Guid?)null : new Guid(dr["updated_by"].ToString());
                            date_updated = dr["date_updated"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_updated"]);
                        }
                        this.isadjusted=ds.Tables["Invoices"].AsEnumerable().Where(i => i.Field<decimal>("adjusted") > 0).Count()>0;
                        
                        #endregion
                    }
                    else
                    {
                        payment_mode = string.Empty;
                        payref_date = DateTime.Now;
                        payref_no = "Auto Generated";
                        payment_amount = 0;
                        processing_ref_date = DateTime.Now;
                        processing_ref_no = "";
                        processing_status = "1";
                        date_created = DateTime.Now;
                    }
                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                    ReturnMessage = Convert.ToString(SqlRecordParams[4].Value);
                }

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }

        

        #endregion

        #region SearchBrowserList
        public bool SearchBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[11];
            SqlRecordParams[0] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier, 100); SqlRecordParams[0].Value = billing_account_id;
            SqlRecordParams[1] = new SqlParameter("@payment_mode", SqlDbType.NVarChar, 1); SqlRecordParams[1].Value = payment_mode;
            SqlRecordParams[2] = new SqlParameter("@processing_status", SqlDbType.NVarChar, 1); SqlRecordParams[2].Value = processing_status;
            SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[3].Value = MenuID;
            SqlRecordParams[4] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = UserID;
            SqlRecordParams[5] = new SqlParameter("@fromdate", SqlDbType.Date); SqlRecordParams[5].Value = FromDate.Value;
            SqlRecordParams[6] = new SqlParameter("@todate", SqlDbType.Date); SqlRecordParams[6].Value = ToDate.Value;
            SqlRecordParams[7] = new SqlParameter("@payment_ref", SqlDbType.NVarChar, 100); SqlRecordParams[7].Value = PaymentRef;
            SqlRecordParams[8] = new SqlParameter("@external_payment_ref", SqlDbType.NVarChar, 100); SqlRecordParams[8].Value = ExternalPaymentRef;
            SqlRecordParams[9] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[9].Direction = ParameterDirection.Output;
            SqlRecordParams[10] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[10].Direction = ParameterDirection.Output;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "ar_payments_list_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "BrowserList";
                    ds.Tables[1].TableName = "Account";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region My Payments: Invoices and All Payments

        #region LoadMyPaymentDetails
        public bool LoadMyPaymentDetails(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[3];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = billing_account_id;
                SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = intMenuID;
                SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;


                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_payments_userwise_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Payments";

                }

                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        } 
        #endregion

        #region LoadMyInvoiceOutstandingDetails
        public bool LoadMyInvoiceOutstandingDetails(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[3];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = billing_account_id;
                SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = intMenuID;
                SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;


                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_payments_invoice_outstanding_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Invoices";
                }
                bReturn = true;

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        } 
        #endregion

        #region LoadMyAllInvoices
        public bool LoadMyAllInvoices(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[3];


            try
            {
                //SqlRecordParams[0] = new SqlParameter("@mode", SqlDbType.Int); SqlRecordParams[0].Value = intMode;
                SqlRecordParams[0] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = billing_account_id;
                SqlRecordParams[1] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserID;
                SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = intMenuID;
                //SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[3].Direction = ParameterDirection.Output;
                //SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;


                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_payments_allinvoice_outstanding_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Invoices";


                }

                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        } 
        #endregion

        #region LoadAdjustmentDetails
        public bool LoadAdjustmentDetails(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[3];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@invoice_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = InvoiceId;
                SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = intMenuID;
                SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;


                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_payments_adj_list_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Adjustments";

                }

                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        } 
        #endregion

        #endregion

        #region Ar-Payment Breakup
        public List<ARPaymentLevelRow> AdjustedPayments { get; set; }
        #endregion

        #region Customer statement aging

        #region SearchBrowserList
        public bool SearchCustomerStatementBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier, 100); SqlRecordParams[0].Value = billing_account_id;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "customer_statement_view_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "AccountBalance";
                    ds.Tables[1].TableName = "InvoiceOutstanding";
                    ds.Tables[2].TableName = "PaymentAdjustments";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #endregion

        #region Load Customer Info
        public bool LoadCustomerInfo(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];


            try
            {

                SqlRecordParams[0] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = billing_account_id;


                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_payments_billing_account_info_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "AccountInfo";
                    foreach (DataRow dr in ds.Tables["AccountInfo"].Rows)
                    {
                        name = Convert.ToString(dr["name"]);
                        address_1 = Convert.ToString(dr["address_1"]);
                        address_2 = Convert.ToString(dr["address_2"]).Trim();
                        city = Convert.ToString(dr["city"]).Trim();
                        state = Convert.ToString(dr["state"]);
                        country = Convert.ToString(dr["country"]).Trim();
                        zip = Convert.ToString(dr["zip"]);
                        email_id = Convert.ToString(dr["email_id"]).Trim();
                        contact_no = Convert.ToString(dr["contact_no"]).Trim();
                    }
                    if (string.IsNullOrEmpty(country)) country = "US";
                   
                }

                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }

        #endregion

        #region GetPaymentGatewayParameters
        public PaymentGatewayControlParameter GetPaymentGatewayParameters(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            var __parameter = new PaymentGatewayControlParameter();
            var bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@codes", SqlDbType.VarChar, 100); SqlRecordParams[0].Value = "TNTKNKEY,TNAPIKEY,OLPMTURL";

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                DataSet ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "control_params_fetch_by_codes", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "PgParameters";
                    foreach (DataRow dr in ds.Tables["PgParameters"].Rows)
                    {
                        var _p = Convert.ToString(dr["control_code"]);
                        var _v = Convert.ToString(dr["data_value_char"]);
                        if (_p == "TNTKNKEY") __parameter.TOKENIZATION_API_KEY = _v;
                        if (_p == "TNAPIKEY") __parameter.PAYMENT_GATEWAY_API_KEY = _v;
                        if (_p == "OLPMTURL") __parameter.PAYMENT_GATEWAY_TRANSACTION_URL = _v;
                    }

                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                    ReturnMessage = Convert.ToString(SqlRecordParams[3].Value);
                }

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return __parameter;
        } 
        #endregion

        #region PaymentRegisterBrowserList
        public bool PaymentRegisterBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[3];
            SqlRecordParams[0] = new SqlParameter("@fromdate", SqlDbType.Date); SqlRecordParams[0].Value = FromDate.Value;
            SqlRecordParams[1] = new SqlParameter("@todate", SqlDbType.Date); SqlRecordParams[1].Value = ToDate.Value;
            SqlRecordParams[2] = new SqlParameter("@payment_mode", SqlDbType.NVarChar, 1); SqlRecordParams[2].Value = payment_mode;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "ar_payments_register_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Report";
                    ds.Tables[1].TableName = "AdjustmentInvoice";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion
    }



    public class PaymentAdjustmentRow
    {
        public Guid invoice_header_id { get; set; }
        public string invoice_no { get; set; }
        public DateTime invoice_date { get; set; }
        public decimal adj_amount { get; set; }

        public string ToXml(int rowid)
        {
            var row = "<row>";
            row += "<rowid>" + rowid.ToString() + "</rowid>";
            row += "<invoice_header_id>" + invoice_header_id.ToString() + "</invoice_header_id>";
            row += "<invoice_no><![CDATA[" + invoice_no + "]]></invoice_no>";
            row += "<invoice_date><![CDATA[" + invoice_date.ToString("ddMMMyyyy") + "]]></invoice_date>";
            row += "<adj_amount>" + string.Format("{0:0.00}", adj_amount) + "</adj_amount>";
            row += "</row>";
            return row;
        }
    }

    public class ARPaymentLevelRow
    {
        public Guid Id { get; set; }
        public Guid ArPaymentId { get; set; }
        public DateTime Date { get; set; }
        public string Invoice { get; set; }
        public string Refund { get; set; }
        public decimal AdjustedAmount { get; set; }
    }

    public class PaymentGatewayControlParameter {
        public string PAYMENT_GATEWAY_TRANSACTION_URL { get; set; }
        public string PAYMENT_GATEWAY_API_KEY { get; set; }
        public string TOKENIZATION_API_KEY { get; set; }
    }
}
