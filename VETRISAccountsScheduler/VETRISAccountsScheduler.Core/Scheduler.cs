using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using VETRISAccountsScheduler.DAL;

namespace VETRISAccountsScheduler.Core
{
    public class Scheduler
    {
        #region Constructor
        public Scheduler()
        {
        }
        #endregion

        #region Variables
        DateTime dtFrom = DateTime.Now;
        DateTime dtTo = DateTime.Now;
        int intServiceID = 0;
        string strServiceName = string.Empty;
        int intFrequency = 0;
        string strLogType = "";
        #endregion

        #region Properties
        public DateTime FROM_DATE
        {
            get { return dtFrom; }
            set { dtFrom = value; }
        }
        public DateTime TO_DATE
        {
            get { return dtTo; }
            set { dtTo = value; }
        }

        public int SERVICE_ID
        {
            get { return intServiceID; }
            set { intServiceID = value; }
        }
        public string SERVICE_NAME
        {
            get { return strServiceName; }
            set { strServiceName = value; }
        }
        public int FREQUENCY
        {
            get { return intFrequency; }
            set { intFrequency = value; }
        }
        public string LOG_TYPE
        {
            get { return strLogType; }
            set { strLogType = value; }
        }
        #endregion

        #region GetServiceDetails
        public bool GetServiceDetails(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = false;
            DataSet ds = new DataSet();
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            StringBuilder sb = new StringBuilder();
            string strControlCode = string.Empty;
            

            try
            {
                SqlRecordParams[0] = new SqlParameter("@service_id", SqlDbType.Int); SqlRecordParams[0].Value = intServiceID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_service_details_fetch_ws8", SqlRecordParams);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    strServiceName = Convert.ToString(dr["service_name"]).Trim();
                    intFrequency = Convert.ToInt32(dr["frequency"]);
                }
                bReturn = true;

            }
            catch (SqlException expErr)
            {
                CatchMessage = expErr.Message;
                bReturn = false;
            }
            finally
            {
                ds.Dispose();
            }

            return bReturn;

        }
        #endregion

        #region ViewLog
        public DataTable ViewLog(string ConfigPath, ref string CatchMessage)
        {
            SqlParameter[] sqlParams = new SqlParameter[4];

            try
            {

                sqlParams[0] = new SqlParameter("@from_date", SqlDbType.DateTime); sqlParams[0].Value = dtFrom;
                sqlParams[1] = new SqlParameter("@to_date", SqlDbType.DateTime); sqlParams[1].Value = dtTo;
                sqlParams[2] = new SqlParameter("@service_name", SqlDbType.NVarChar, 50); sqlParams[2].Value = strServiceName;
                sqlParams[3] = new SqlParameter("@log_type", SqlDbType.NChar, 1); sqlParams[3].Value = strLogType;

                DataTable dtbl = null;
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                dtbl = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_log_fetch", sqlParams).Tables[0];
                return dtbl;
            }
            catch (SqlException expErr)
            { CatchMessage = expErr.Message; return null; }
            finally
            { }
        }
        #endregion

        #region PurgeLog
        public bool PurgeLog(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] sqlParams = new SqlParameter[6];

            sqlParams[0] = new SqlParameter("@from_date", SqlDbType.DateTime); sqlParams[0].Value = dtFrom;
            sqlParams[1] = new SqlParameter("@to_date", SqlDbType.DateTime); sqlParams[1].Value = dtTo;
            sqlParams[2] = new SqlParameter("@service_name", SqlDbType.NVarChar, 30); sqlParams[2].Value = strServiceName;
            sqlParams[3] = new SqlParameter("@log_type", SqlDbType.NChar, 1); sqlParams[3].Value = strLogType;
            sqlParams[4] = new SqlParameter("@error_msg", SqlDbType.VarChar, 100); sqlParams[4].Direction = ParameterDirection.Output;
            sqlParams[5] = new SqlParameter("@return_type", SqlDbType.Int); sqlParams[5].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_log_purge", sqlParams);
                intReturnType = Convert.ToInt32(sqlParams[5].Value);
                if (intReturnType == 1) { bReturn = true; }
                ReturnMessage = Convert.ToString(sqlParams[4].Value);
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }
            return bReturn;
        }
        #endregion

        #region CreateServiceRestartNotification
        public bool CreateServiceRestartNotification(string ConfigPath, int ServiceID, string Reason, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                SqlRecordParams[0] = new SqlParameter("@service_id", SqlDbType.Int); SqlRecordParams[0].Value = ServiceID;
                SqlRecordParams[1] = new SqlParameter("@restart_reason", SqlDbType.NVarChar, 4000); SqlRecordParams[1].Value = Reason;
                SqlRecordParams[2] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[2].Direction = ParameterDirection.Output;
                SqlRecordParams[3] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_service_restart_notification_create", SqlRecordParams);

                intReturnType = Convert.ToInt32(SqlRecordParams[3].Value);
                if (intReturnType == 0)
                {
                    ReturnMessage = Convert.ToString(SqlRecordParams[2].Value);
                    bReturn = false;
                }
                else
                    bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion
    }
}
