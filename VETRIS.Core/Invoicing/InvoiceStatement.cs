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
    public class InvoiceStatement
    {
        #region Constractor
        public InvoiceStatement()
        {

        }

        #endregion

        #region Variables
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid CycleID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid AccountID = new Guid("00000000-0000-0000-0000-000000000000");
       
        #endregion

        #region Properties
        public Guid USER_ID
        {
            get { return UserID; }
            set { UserID = value; }
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
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "invoicing_details_fetch_params", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Cycle";
                    ds.Tables[1].TableName = "Account";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion
    }
}
