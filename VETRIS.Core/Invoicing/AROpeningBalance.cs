using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VETRIS.Core.Invoicing
{
    public class AROpeningBalance
    {
        #region Variables
        private Guid _userId = new Guid("00000000-0000-0000-0000-000000000000");
        private Guid _cretated_by = new Guid("00000000-0000-0000-0000-000000000000");
        private Guid _billing_account_id = new Guid("00000000-0000-0000-0000-000000000000");
        private Guid _invoice_id = new Guid("00000000-0000-0000-0000-000000000000");
        private Guid _id = new Guid("00000000-0000-0000-0000-000000000000");

        int intMenuID = 0;
        int intMode = 1;
        string strUserName = string.Empty;
        #endregion

        public AROpeningBalance()
        {
            Batch = new List<AROpeningBalance>();
        }
        public int Year { get; set; }
        public int row_id { get; set; }

        public Guid UserID { get { return _userId; } set { _userId = value; } }
        public string UserName { get; set; }
        public int Mode { get { return intMode; } set { intMode = value; } }
        public int MenuID { get { return intMenuID; } set { intMenuID = value; } }

        #region Table properties
        public Guid id { get { return _id; } set { _id = value; } }
        public Guid billing_account_id { get { return _billing_account_id; } set { _billing_account_id = value; } }
        public DateTime opbal_date { get; set; }
        public string invoice_no { get; set; }
        public decimal? opbal_amount { get; set; }
        public Boolean isadjusted { get; set; }
        public Guid created_by { get { return _cretated_by; } set { _cretated_by = value; } }
        public DateTime date_created { get; set; }
        public Guid? updated_by { get; set; }
        public DateTime? date_updated { get; set; }
        #endregion

        public List<AROpeningBalance> Batch = new List<AROpeningBalance>();

        #region XML
        public string ToXml(int rowid)
        {
            var row = "<row>";
            row += "<rowid>" + rowid.ToString() + "</rowid>";
            row += "<id>" + id.ToString() + "</id>";
            row += "<billing_account_id>" + billing_account_id.ToString() + "</billing_account_id>";
            row += "<opbal_date>" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", opbal_date) + "</opbal_date>";
            row += "<invoice_no>" + invoice_no + "</invoice_no>";
            row += "<opbal_amount>" + string.Format("{0:0.00}", opbal_amount) + "</opbal_amount>";
            row += "<isadjusted>" + (isadjusted?1:0).ToString() + "</isadjusted>";
            row += "<created_by>" + created_by.ToString() + "</created_by>";
            row += "<date_created>" + string.Format("{0:yyyy-MM-dd HH:mm:ss}", date_created) + "</date_created>";
            row += "</row>";
            return row;
        }

        public string ToBatchXML()
        {
            if (Batch.Count == 0) return null;
            var xml = "<opbal>";
            int i = 0;
            foreach (var item in Batch)
            {
                xml += item.ToXml(++i);
            }
            xml += "</opbal>";
            return xml;
        }
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
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_opening_balance_fetch_params", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Account";
                    ds.Tables[1].TableName = "Year";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion
        #region Fetch
        public bool FetchRecords(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[7];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = id;
                SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = billing_account_id;
                SqlRecordParams[2] = new SqlParameter("@year", SqlDbType.Int); SqlRecordParams[2].Value = Year;
                SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[3].Value = intMenuID;
                SqlRecordParams[4] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = UserID;
                SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_opening_balance_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "OpeningBalance";
                    if (id.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                id = new Guid(dr["id"].ToString());
                                billing_account_id = new Guid(dr["billing_account_id"].ToString());
                                opbal_date = Convert.ToDateTime(dr["opbal_date"].ToString());
                                invoice_no = Convert.ToString(dr["invoice_no"].ToString());
                                opbal_amount = Convert.ToDecimal(dr["opbal_amount"].ToString());
                                isadjusted = Convert.ToBoolean(dr["isadjusted"]);
                                created_by = new Guid(dr["created_by"].ToString());
                                date_created = Convert.ToDateTime(dr["date_created"]);
                            }
                        }
                    }
                    
                    bReturn = true;
                }
                

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

            if (Validate(ref ReturnMessage))
            {
                try
                {
                    
                    SqlParameter[] SqlRecordParams = new SqlParameter[10];
                    SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = id;
                    SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = billing_account_id;
                    SqlRecordParams[2] = new SqlParameter("@opbal_date", SqlDbType.DateTime); SqlRecordParams[2].Value = opbal_date;
                    SqlRecordParams[3] = new SqlParameter("@invoice_no", SqlDbType.NVarChar, 30); SqlRecordParams[3].Value = invoice_no;
                    SqlRecordParams[4] = new SqlParameter("@opbal_amount", SqlDbType.Money); SqlRecordParams[4].Value = opbal_amount;
                    SqlRecordParams[5] = new SqlParameter("@isadjusted", SqlDbType.Bit); SqlRecordParams[5].Value = Year;
                    SqlRecordParams[6] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[6].Value = intMenuID;
                    SqlRecordParams[7] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[7].Value = UserID;
                    SqlRecordParams[8] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[8].Direction = ParameterDirection.Output;
                    SqlRecordParams[9] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[9].Direction = ParameterDirection.Output;


                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_opening_balance_save", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[9].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;

                    
                    ReturnMessage = Convert.ToString(SqlRecordParams[8].Value);

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
        #region Batch Validation
        private bool Validate(ref string ReturnMessage)
        {
            if (billing_account_id.ToString() == "00000000-0000-0000-0000-000000000000")
                ReturnMessage = "Select billing account.";
            if (string.IsNullOrEmpty(invoice_no))
                ReturnMessage = "Invoice number must be entered otherwise you can put in 'On Account'." ;
            if (opbal_amount <= 0)
                ReturnMessage = "Amount must be greater than zero.";
            return true;
        }
        #endregion
    }
}
