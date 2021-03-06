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
    /// <summary>
    /// Refunds Linked to Payments
    /// </summary>
    public class ARRefunds
    {

        #region Variables
        private Guid _userId = new Guid("00000000-0000-0000-0000-000000000000");
        private Guid _cretated_by = new Guid("00000000-0000-0000-0000-000000000000");
        private Guid _billing_account_id = new Guid("00000000-0000-0000-0000-000000000000");
        private Guid _ar_payments_id = new Guid("00000000-0000-0000-0000-000000000000");
        private Guid _invoice_id = new Guid("00000000-0000-0000-0000-000000000000");
        private Guid _id = new Guid("00000000-0000-0000-0000-000000000000");

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        int intMenuID = 0;
        int intMode = 1;
        string strUserName = string.Empty;
        #endregion

        public ARRefunds()
        {
            this.Adjustments = new List<RefundAdjustmentRow>();
        }

        public Guid InvoiceId { get { return _invoice_id; } set { _invoice_id = value; } }
        public Guid UserID { get { return _userId; } set { _userId = value; } }
        public string UserName { get; set; }
        public int Mode { get { return intMode; } set { intMode = value; } }
        public int MenuID { get { return intMenuID; } set { intMenuID = value; } }

        #region Table properties
        public Guid id { get { return _id; } set { _id = value; } }
        public Guid billing_account_id { get { return _billing_account_id; } set { _billing_account_id = value; } }
        /// <summary>
        /// Reference Payment Id
        /// </summary>
        public Guid ar_payments_id { get { return _ar_payments_id; } set { _ar_payments_id = value; } }
        public string payment_reference_no { get; set; }
        public string refund_mode { get; set; }
        public string refundref_no { get; set; }
        public DateTime refundref_date { get; set; }
        public string processing_ref_no { get; set; }
        public DateTime processing_ref_date { get; set; }
        public string processing_pg_name { get; set; }
        public string processing_status { get; set; }
        public string payment_tool { get; set; }

        public string remarks { get; set; }
        public decimal? refund_amount { get; set; }
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
        public List<RefundAdjustmentRow> Adjustments { get; set; }
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
                SqlParameter[] SqlRecordParams = new SqlParameter[18];

                try
                {

                    SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = id; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
                    SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = billing_account_id;
                    SqlRecordParams[2] = new SqlParameter("@refund_mode", SqlDbType.NVarChar, 1); SqlRecordParams[2].Value = refund_mode;
                    SqlRecordParams[3] = new SqlParameter("@refundref_no", SqlDbType.NVarChar, 50); SqlRecordParams[3].Value = refundref_no; SqlRecordParams[3].Direction = ParameterDirection.InputOutput;
                    SqlRecordParams[4] = new SqlParameter("@refundref_date", SqlDbType.DateTime); SqlRecordParams[4].Value = refundref_date; SqlRecordParams[4].Direction = ParameterDirection.InputOutput;
                    SqlRecordParams[5] = new SqlParameter("@processing_ref_no", SqlDbType.NVarChar, 100); SqlRecordParams[5].Value = processing_ref_no;
                    SqlRecordParams[6] = new SqlParameter("@processing_ref_date", SqlDbType.DateTime); SqlRecordParams[6].Value = processing_ref_date;
                    SqlRecordParams[7] = new SqlParameter("@processing_pg_name", SqlDbType.NVarChar, 50); SqlRecordParams[7].Value = processing_pg_name;
                    SqlRecordParams[8] = new SqlParameter("@processing_status", SqlDbType.NChar, 1); SqlRecordParams[8].Value = processing_status;
                    SqlRecordParams[9] = new SqlParameter("@refund_amount", SqlDbType.Money); SqlRecordParams[9].Value = refund_amount;
                    SqlRecordParams[10] = new SqlParameter("@ar_payments_id", SqlDbType.UniqueIdentifier); SqlRecordParams[10].Value = ar_payments_id;


                    SqlRecordParams[11] = new SqlParameter("@xml_adjustments", SqlDbType.NText);
                    var strAdjustments = ToAdjustmentsXML();
                    SqlRecordParams[11].Value = strAdjustments;

                    SqlRecordParams[12] = new SqlParameter("@remarks", SqlDbType.NVarChar, 150); SqlRecordParams[12].Value = remarks;
                    SqlRecordParams[13] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[13].Value = UserID;
                    SqlRecordParams[14] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[14].Value = intMenuID;
                    SqlRecordParams[15] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[15].Direction = ParameterDirection.Output;
                    SqlRecordParams[16] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[16].Direction = ParameterDirection.Output;
                    SqlRecordParams[17] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[17].Direction = ParameterDirection.Output;


                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_refunds_save", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[17].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;

                    id = new Guid(Convert.ToString(SqlRecordParams[0].Value));
                    refundref_no = Convert.ToString(SqlRecordParams[3].Value);
                    refundref_date = Convert.ToDateTime(SqlRecordParams[4].Value);
                    strUserName = Convert.ToString(SqlRecordParams[15].Value).Trim();
                    ReturnMessage = Convert.ToString(SqlRecordParams[16].Value);

                   
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
        #endregion

        #region ValidateRecord
        private bool ValidateRecord(ref string ReturnMessage)
        {
            bool bReturn = true;

            if (billing_account_id == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                ReturnMessage = "225";
            }
            if (ar_payments_id == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                ReturnMessage = "225";
            }
            if (string.IsNullOrEmpty(processing_ref_no.Trim()))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "316";

            }
            if (refund_amount <= 0)
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
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_refunds_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";
                    ds.Tables[1].TableName = "Accounts";
                    ds.Tables[2].TableName = "Payments";
                    if (id.ToString() != new Guid("00000000-0000-0000-0000-000000000000").ToString())
                    {
                        #region Details
                        foreach (DataRow dr in ds.Tables["Details"].Rows)
                        {
                            id = new Guid(dr["id"].ToString());
                            billing_account_id = new Guid(dr["billing_account_id"].ToString());
                            ar_payments_id = new Guid(dr["ar_payments_id"].ToString());
                            refund_mode = Convert.ToString(dr["refund_mode"]).Trim();
                            refundref_no = Convert.ToString(dr["refundref_no"]).Trim();
                            refundref_date = Convert.ToDateTime(dr["refundref_date"]);
                            processing_ref_no = Convert.ToString(dr["processing_ref_no"]).Trim();
                            processing_ref_date = Convert.ToDateTime(dr["processing_ref_date"]);
                            processing_pg_name = Convert.ToString(dr["processing_pg_name"]).Trim();
                            processing_status = Convert.ToString(dr["processing_status"]).Trim();
                            refund_amount = Convert.ToDecimal(dr["refund_amount"]);
                            remarks = Convert.ToString(dr["remarks"]);
                            created_by = new Guid(dr["created_by"].ToString());
                            date_created = Convert.ToDateTime(dr["date_created"]);
                            updated_by = dr["updated_by"] == DBNull.Value ? (Guid?)null : new Guid(dr["updated_by"].ToString());
                            date_updated = dr["date_updated"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["date_updated"]);
                        }
                        this.isadjusted = ds.Tables["Payments"].AsEnumerable().Where(i => i.Field<decimal>("current_refund") > 0).Count() > 0;

                        #endregion
                    }
                    else
                    {
                        refund_mode = string.Empty;
                        refundref_date = DateTime.Now;
                        refundref_no = "Auto Generated";
                        refund_amount = 0;
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
        #region Payment adjustments fetch
        public bool LoadPaymentAdjustments(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = ar_payments_id;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                var ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_payments_adjustments_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";

                    #region Details
                    this.Adjustments=new List<RefundAdjustmentRow>();
                    foreach (DataRow dr in ds.Tables["Details"].Rows)
                    {
                        var adj = new RefundAdjustmentRow();
                        adj.ar_payments_id = ar_payments_id;
                        adj.invoice_header_id = new Guid(dr["id"].ToString());
                        adj.invoice_no = Convert.ToString(dr["invoice_no"]);
                        adj.invoice_date = Convert.ToDateTime(dr["invoice_date"]);
                        adj.adj_amount = Convert.ToDecimal(dr["adj_amount"]);
                        payment_reference_no = Convert.ToString(dr["processing_ref_no"]);
                        this.Adjustments.Add(adj);
                    }

                    #endregion

                    bReturn = true;
                }
                else
                {
                    bReturn = false;
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
            SqlParameter[] SqlRecordParams = new SqlParameter[7];
            SqlRecordParams[0] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier, 100); SqlRecordParams[0].Value = billing_account_id;
            SqlRecordParams[1] = new SqlParameter("@refund_mode", SqlDbType.NVarChar, 1); SqlRecordParams[1].Value = refund_mode;
            SqlRecordParams[2] = new SqlParameter("@processing_status", SqlDbType.NVarChar, 1); SqlRecordParams[2].Value = processing_status;
            SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[3].Value = MenuID;
            SqlRecordParams[4] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = UserID;
            SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
            SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "ar_refunds_list_fetch", SqlRecordParams);
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

        #region RefundRegisterBrowserList
        public bool RefundRegisterBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[3];
            SqlRecordParams[0] = new SqlParameter("@fromdate", SqlDbType.Date); SqlRecordParams[0].Value = FromDate.Value;
            SqlRecordParams[1] = new SqlParameter("@todate", SqlDbType.Date); SqlRecordParams[1].Value = ToDate.Value;
            SqlRecordParams[2] = new SqlParameter("@payment_mode", SqlDbType.NVarChar, 1); SqlRecordParams[2].Value = refund_mode;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "ar_refunds_register_fetch", SqlRecordParams);
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

    public class RefundAdjustmentRow : PaymentAdjustmentRow
    {
        public Guid ar_payments_id { get; set; }
        public string ToXml(int rowid)
        {
            var row = "<row>";
            row += "<rowid>" + rowid.ToString() + "</rowid>";
            row += "<ar_payments_id>" + ar_payments_id.ToString() + "</ar_payments_id>";
            row += "<invoice_header_id>" + invoice_header_id.ToString() + "</invoice_header_id>";
            row += "<invoice_no><![CDATA[" + invoice_no + "]]></invoice_no>";
            row += "<invoice_date><![CDATA[" + invoice_date.ToString("ddMMMyyyy") + "]]></invoice_date>";
            row += "<adj_amount>" + string.Format("{0:0.00}", adj_amount) + "</adj_amount>";
            row += "</row>";
            return row;
        }
    }




}
