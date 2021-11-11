using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VETRIS.Core.MyPayments
{
    public class ARReconciliation
    {
        #region Variables
        public int UserId { get; set; }
        public int MenuId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string UserName { get; set; }
        string strUserName = string.Empty;
        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRecord(ref ReturnMessage))
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[3];

                try
                {
                    var strTransactions = ToTransactionXML();
                    SqlRecordParams[0] = new SqlParameter("@xml_transactions", SqlDbType.NText);
                    if (string.IsNullOrEmpty(strTransactions)) SqlRecordParams[0].Value = DBNull.Value;
                    else SqlRecordParams[0].Value = strTransactions;
                    
                    SqlRecordParams[1] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[1].Direction = ParameterDirection.Output;
                    SqlRecordParams[2] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[2].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "brs_transactions_save", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[2].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;

                    
                    ReturnMessage = Convert.ToString(SqlRecordParams[1].Value);
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

            //if (billing_account_id == new Guid("00000000-0000-0000-0000-000000000000"))
            //{
            //    ReturnMessage = "225";
            //}
            //if (string.IsNullOrEmpty(processing_ref_no.Trim()))
            //{
            //    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
            //    ReturnMessage += "316";

            //}
            //if (payment_amount <= 0)
            //{
            //    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
            //    ReturnMessage += "318";
            //}
            //if (string.IsNullOrEmpty(processing_ref_no.Trim()))
            //{
            //    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
            //    ReturnMessage += "317";
            //}

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

        #region Transactions
        public List<TransactionsRow> transactions { get; set; }
        public string ToTransactionXML()
        {
            if (transactions.Count == 0) return null;
            var xml = "<data>";
            int i = 0;
            foreach (var item in transactions)
            {
                xml += item.ToXml(++i);
            }
            xml += "</data>";
            return xml;
        }
        #endregion

        #region ReconcilationBrowserList
        public bool ReconcilationBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];
            SqlRecordParams[0] = new SqlParameter("@fromdate", SqlDbType.Date); SqlRecordParams[0].Value = FromDate.Value;
            SqlRecordParams[1] = new SqlParameter("@todate", SqlDbType.Date); SqlRecordParams[1].Value = ToDate.Value;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "brs_transactions_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Transactions";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion
    }

    public class TransactionsRow
    {
        public DateTime date { get; set; }
        public string transaction_id { get; set; }
        public string transaction_type { get; set; }
        public string condition { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string company { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string country { get; set; }
        public string cc_number { get; set; }
        public string cc_type { get; set; }
        public string check_aba { get; set; }
        public string check_name { get; set; }
        public string avs_response { get; set; }
        public string check_account { get; set; }
        public string currency { get; set; }
        public decimal amount { get; set; }
        public string action_type { get; set; }
        public string response_text { get; set; }
        public bool success { get; set; }
        public decimal? requested_amount { get; set; }
        public Guid? ar_payments_id { get; set; }
        public Guid? ar_refunds_id { get; set; }
        public Guid? billing_account_id { get; set; }

        public string ToXml(int rowid)
        {
            var row = "<row>";
            row += "<rowid>" + rowid.ToString() + "</rowid>";
            row += "<date><![CDATA[" + date.ToString("yyyy-MM-dd HH:mm:ss") + "]]></date>";
            row += "<transaction_id>" + transaction_id + "</transaction_id>";
            row += "<transaction_type>" + transaction_type + "</transaction_type>";
            row += "<condition>" + condition + "</condition>";
            row += "<first_name>" + first_name + "</first_name>";
            row += "<last_name>" + last_name + "</last_name>";
            row += "<address_1>" + address_1 + "</address_1>";
            row += "<address_2>" + address_2 + "</address_2>";
            row += "<company>" + company + "</company>";
            row += "<city>" + city + "</city>";
            row += "<state>" + state + "</state>";
            row += "<postal_code>" + postal_code + "</postal_code>";
            row += "<country>" + country + "</country>";
            row += "<cc_number>" + cc_number + "</cc_number>";
            row += "<cc_type>" + cc_type + "</cc_type>";
            row += "<check_aba>" + check_aba + "</check_aba>";
            row += "<check_name>" + check_name + "</check_name>";
            row += "<avs_response>" + avs_response + "</avs_response>";
            row += "<check_account>" + check_account + "</check_account>";
            row += "<currency>" + currency + "</currency>";
            row += "<amount>" + string.Format("{0:0.00}", amount) + "</amount>";
            row += "<action_type>" + action_type + "</action_type>";
            row += "<response_text>" + response_text + "</response_text>";
            row += "<success>" + (success?1:0) + "</success>";
            row += "<requested_amount>" + (requested_amount!=null? string.Format("{0:0.00}", requested_amount):"0") + "</requested_amount>";
            row += "</row>";
            row = row.Replace("&", "&amp;");
            return row;
        }
    }
}
