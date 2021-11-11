using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;


namespace VETRIS.Core
{
    public class CommonFunctions
    {
        #region Constructor
        public CommonFunctions()
        {
        }
        #endregion

        #region Variables
        Guid UserID = Guid.Empty;
        Guid RecordID = Guid.Empty;
        Guid AddlRecordID = Guid.Empty;
        Guid SessionID = new Guid("00000000-0000-0000-0000-000000000000");
        int intRecordID = 0;
        int intMenuID=0;
        string strUserName = string.Empty;
        #endregion

        #region Properties
        public Guid USER_SESSION_ID
        {
            get { return SessionID; }
            set { SessionID = value; }
        }
        public Guid USER_ID
        {
            get { return UserID; }
            set { UserID = value; }
        }
        public int RECORD_ID_INT
        {
            get { return intRecordID; }
            set { intRecordID = value; }
        }
        public Guid RECORD_ID_UI
        {
            get { return RecordID; }
            set { RecordID = value; }
        }
        public Guid ADDITIONAL_RECORD_ID_UI
        {
            get { return AddlRecordID; }
            set { AddlRecordID = value; }
        }
        public int MENU_ID
        {
            get { return intMenuID; }
            set { intMenuID = value; }
        }
        public string USER_NAME
        {
            get { return strUserName; }
            set { strUserName = value; }
        }
        #endregion

        #region UnlockUserLockedRecords
        public bool UnlockUserLockedRecords(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[2];
            try
            {
                
                SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserID;
                SqlRecordParams[1] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = SessionID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "common_unlock_user_locked_records", SqlRecordParams);
                bReturn = true;

            }

            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }

        #endregion

        #region ReleaseStudyLock
        public bool ReleaseStudyLock(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false; 

            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            try
            {

                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = RecordID;
                SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = intMenuID;
                SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = SessionID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "common_release_study_lock", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "RecordCount";

                }
                
                bReturn = true;

            }

            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }

        #endregion

        #region LockRecordUI
        public bool LockRecordUI(string ConfigPath,ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intReturnValue = 0; int intExecReturn = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[6];
            try
            {

                SqlRecordParams[0] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[0].Value = intMenuID;
                SqlRecordParams[1] = new SqlParameter("@record_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = RecordID;
                SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = SessionID;
                SqlRecordParams[4] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "common_lock_record_ui", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[5].Value);
                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;

                
                ReturnMessage = Convert.ToString(SqlRecordParams[4].Value);

            }

            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }

        #endregion

        #region AcquireRecordLockUI
        public bool AcquireRecordLockUI(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intReturnValue = 0; int intExecReturn = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[6];
            try
            {

                SqlRecordParams[0] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[0].Value = intMenuID;
                SqlRecordParams[1] = new SqlParameter("@record_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = RecordID;
                SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = SessionID;
                SqlRecordParams[4] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "common_acquire_record_lock_ui", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[5].Value);
                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;


                ReturnMessage = Convert.ToString(SqlRecordParams[4].Value);

            }

            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }

        #endregion

        #region FetchUSTimeZones
        public bool FetchUSTimeZones(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false; 
            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "common_us_time_zone_fetch");

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "TimeZone";
                }
                bReturn = true;

            }

            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }

        #endregion

        #region FetchCountryWiseStates
        public bool FetchCountryWiseStates(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@country_id", SqlDbType.Int); SqlRecordParams[0].Value = intRecordID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "country_wise_state_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "States";
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

        #region CheckAdditionalRecordLock
        public bool CheckAdditionalRecordLock(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnValue = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[8];
            try
            {
                SqlRecordParams[0] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[0].Value = intMenuID;
                SqlRecordParams[1] = new SqlParameter("@record_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = RecordID;
                SqlRecordParams[2] = new SqlParameter("@addl_record_id_ui", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = AddlRecordID;
                SqlRecordParams[3] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = UserID;
                SqlRecordParams[4] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = SessionID;
                SqlRecordParams[5] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[6].Direction = ParameterDirection.Output;
                SqlRecordParams[7] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[7].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "common_check_record_with_addl_rec_ui_lock_ui", SqlRecordParams);
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

            return bReturn;
        }

        #endregion

    }
}
