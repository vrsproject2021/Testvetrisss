using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VETRIS.Core.TransNationalPaymentGateway
{
    public class TransactionVault
    {
        #region Properties
        public Guid user_id { get; set; }
        public int menu_id { get; set; }
        public Guid id { get; set; }
        public Guid billing_account_id { get; set; }
        public Guid vault_id { get; set; }
        public string vault_type { get; set; }
        public string vault_card { get; set; }
        public string vault_card_type { get; set; }
        public string vault_exp { get; set; }
        public string vault_name { get; set; }
        public string vault_account { get; set; }
        public string vault_aba { get; set; }
        public string holder_name { get; set; }
        public int last_used { get; set; } 
        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[15];

            try
            {
               
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = id; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
                SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = billing_account_id;
                SqlRecordParams[2] = new SqlParameter("@vault_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = vault_id;
                SqlRecordParams[3] = new SqlParameter("@vault_type", SqlDbType.NVarChar, 5); SqlRecordParams[3].Value = vault_type;
                SqlRecordParams[4] = new SqlParameter("@vault_card", SqlDbType.NVarChar, 19); SqlRecordParams[4].Value = vault_card;
                SqlRecordParams[5] = new SqlParameter("@vault_card_type", SqlDbType.NVarChar, 10); SqlRecordParams[5].Value = vault_card_type;
                SqlRecordParams[6] = new SqlParameter("@vault_exp", SqlDbType.NVarChar, 5); SqlRecordParams[6].Value = vault_exp;
                SqlRecordParams[7] = new SqlParameter("@vault_name", SqlDbType.NVarChar, 50); SqlRecordParams[7].Value = vault_name;
                SqlRecordParams[8] = new SqlParameter("@vault_account", SqlDbType.NVarChar, 30); SqlRecordParams[8].Value = vault_account;
                SqlRecordParams[9] = new SqlParameter("@vault_aba", SqlDbType.NVarChar, 30); SqlRecordParams[9].Value = vault_aba;
                SqlRecordParams[10] = new SqlParameter("@holder_name", SqlDbType.NVarChar, 100); SqlRecordParams[10].Value = holder_name;
                SqlRecordParams[11] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[11].Value = user_id;
                SqlRecordParams[12] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[12].Value = menu_id;
                SqlRecordParams[13] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[13].Direction = ParameterDirection.Output;
                SqlRecordParams[14] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[14].Direction = ParameterDirection.Output;


                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "billing_account_vault_save", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[14].Value);
                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;


                ReturnMessage = Convert.ToString(SqlRecordParams[13].Value);

                id = new Guid(Convert.ToString(SqlRecordParams[0].Value));


            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }



            return bReturn;
        } 
        #endregion

        #region LoadDetails
        public bool LoadDetails(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = billing_account_id;


                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "billing_account_vault_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "VaultRecords"; 
                }
                bReturn = true;

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region UpdateLastUsedDate
        public bool UpdateLastUsedDate(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = false;
            int intExecReturn = 0;


            try
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[1];
                SqlRecordParams[0] = new SqlParameter("@vault_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = vault_id;


                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "billing_account_vault_update_status", SqlRecordParams);
                bReturn = true;


            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }



            return bReturn;
        } 
        #endregion

        
    }
}
