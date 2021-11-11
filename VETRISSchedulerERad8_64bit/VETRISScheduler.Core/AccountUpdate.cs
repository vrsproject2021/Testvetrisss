using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using VETRISScheduler.DAL;

namespace VETRISScheduler.Core
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
        string strDebtorID = string.Empty;
        #endregion

        #region Properties
        public Guid BILLING_ACCOUNT_ID
        {
            get { return BillingAcctID; }
            set { BillingAcctID = value; }
        }
        public string DEBTOR_ID
        {
            get { return strDebtorID; }
            set { strDebtorID = value; }
        }

        #endregion

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

        #region UpdateBillingAccount
        public bool UpdateBillingAccount(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);


                SqlRecordParams[0] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = BillingAcctID;
                SqlRecordParams[1] = new SqlParameter("@debtor_id", SqlDbType.NVarChar,20); SqlRecordParams[1].Value = strDebtorID;
                SqlRecordParams[2] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[2].Direction = ParameterDirection.Output;
                SqlRecordParams[3] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_billing_account_update", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[3].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[2].Value).Trim();

                if (intReturnType == 0)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdateBillingAccount() - Error: " + strReturnMessage, true);
                }

                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "- Billing Account ID : " + Convert.ToString(BillingAcctID); }

            return bReturn;
        }
        #endregion
    }
}
