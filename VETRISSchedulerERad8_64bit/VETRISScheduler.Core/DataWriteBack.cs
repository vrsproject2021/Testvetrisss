using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using VETRISScheduler.DAL;

namespace VETRISScheduler.Core
{
    public class DataWriteBack
    {
        #region Constructor
        public DataWriteBack()
        {
        }
        #endregion

        #region Variables
        Guid UserId = Guid.Empty;
        string strUserType = string.Empty;
        Guid Id = Guid.Empty;
        string strSUID = string.Empty;
        int intStatusID = 0;
        string strIsAddendum = string.Empty;
        int intAddendumSrl = 0;
        #endregion

        #region Properties
        public Guid USER_ID
        {
            get { return UserId; }
            set { UserId = value; }
        }
        public string USER_TYPE
        {
            get { return strUserType; }
            set { strUserType = value; }
        }
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public string STUDY_UID
        {
            get { return strSUID; }
            set { strSUID = value; }
        }
        public int STATUS_ID
        {
            get { return intStatusID; }
            set { intStatusID = value; }
        }
        public string IS_ADDENDUM
        {
            get { return strIsAddendum; }
            set { strIsAddendum = value; }
        }
        public int ADDENDUM_SERIAL
        {
            get { return intAddendumSrl; }
            set { intAddendumSrl = value; }
        }
        #endregion

        #region FetchWriteBackList
        public bool FetchWriteBackList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
          

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_write_back_records_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";
                    ds.Tables[1].TableName = "StudyTypes";
                    ds.Tables[2].TableName = "Documents";
                    ds.Tables[3].TableName = "DCMFiles";
                    ds.Tables[4].TableName = "StudyTypeTags";
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

        #region UpdateWriteBackStatus
        public bool UpdateWriteBackStatus(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[5];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);


                SqlRecordParams[0] = new SqlParameter("@study_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strSUID;
                SqlRecordParams[2] = new SqlParameter("@status_id", SqlDbType.Int); SqlRecordParams[2].Value = intStatusID;
                SqlRecordParams[3] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_case_study_write_back_status_change", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[4].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[3].Value).Trim();

                if (intReturnType == 0)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdateStatus() - Error: " + strReturnMessage, true);
                }
                else if (intReturnType == 1)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdateStatus() - " + strReturnMessage, false);
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "-" + strSUID; }

            return bReturn;
        }
        #endregion

        #region FetchWriteBackReportStudies
        public bool FetchWriteBackReportStudies(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_write_back_report_study_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "StudyIDs";
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

        #region FetchWriteBackReporDetails
        public bool FetchWriteBackReporDetails(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                SqlRecordParams[0] = new SqlParameter("@study_hdr_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@is_addendum", SqlDbType.NChar,1); SqlRecordParams[1].Value = strIsAddendum;

                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_write_back_report_details_fetch",SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "ReportHdr";
                    ds.Tables[1].TableName = "ReportDtls";
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

        #region UpdateReportWriteBackStatus
        public bool UpdateReportWriteBackStatus(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[7];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);


                SqlRecordParams[0] = new SqlParameter("@study_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strSUID;
                SqlRecordParams[2] = new SqlParameter("@status_id", SqlDbType.Int); SqlRecordParams[2].Value = intStatusID;
                SqlRecordParams[3] = new SqlParameter("@is_addendum", SqlDbType.NChar, 1); SqlRecordParams[3].Value = strIsAddendum;
                SqlRecordParams[4] = new SqlParameter("@addendum_srl", SqlDbType.Int); SqlRecordParams[4].Value = intAddendumSrl;
                SqlRecordParams[5] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_write_back_report_status_update", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[6].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[5].Value).Trim();

                if (intReturnType == 0)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdateReportWriteBackStatus() - Error: " + strReturnMessage, true);
                }
                else if (intReturnType == 1)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdateReportWriteBackStatus() - " + strReturnMessage, false);
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "-" + strSUID; }

            return bReturn;
        }
        #endregion

        #region FetchUserUpdationList
        public bool FetchUserUpdationList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_user_update_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Users";
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

        #region UpdateUserUpdateInPACS
        public bool UpdateUserUpdateInPACS(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);


                SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserId;
                SqlRecordParams[1] = new SqlParameter("@user_type", SqlDbType.NVarChar, 5); SqlRecordParams[1].Value = strUserType;
                SqlRecordParams[2] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[2].Direction = ParameterDirection.Output;
                SqlRecordParams[3] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_user_update_save", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[3].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[2].Value).Trim();

                if (intReturnType == 0)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "Core:UpdateUserUpdateInPACS() - Error: " + strReturnMessage, true);
                }

                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "-" + Convert.ToString(UserId); }

            return bReturn;
        }
        #endregion
    }
}
