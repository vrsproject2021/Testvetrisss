using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using VETRISScheduler.DAL;

namespace VETRISScheduler.Core
{
    public class ListenerFileProcess
    {
        #region Constructor
        public ListenerFileProcess()
        {
        }
        #endregion

        #region Variables
        string strSUID = string.Empty;
        string strFileName = string.Empty;
        string strFailureReason = string.Empty;
        #endregion

        #region Properties

        public string STUDY_UID
        {
            get { return strSUID; }
            set { strSUID = value; }
        }
        public string FILE_NAME
        {
            get { return strFileName; }
            set { strFileName = value; }
        }
        public string FAILURE_REASON
        {
            get { return strFailureReason; }
            set { strFailureReason = value; }
        }
        #endregion

        #region FetchModalityList
        public bool FetchModalityList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "dashboard_modality_list_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Modality";
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

        #region CreateFileXferFailureNotification
        public bool CreateFileXferFailureNotification(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[5];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                SqlRecordParams[0] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strSUID;
                SqlRecordParams[1] = new SqlParameter("@file_name", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strFileName;
                SqlRecordParams[2] = new SqlParameter("@failure_reason", SqlDbType.NVarChar, 4000); SqlRecordParams[2].Value = strFailureReason;
                SqlRecordParams[3] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_listener_file_xfer_fail_notification_create", SqlRecordParams);

                intReturnType = Convert.ToInt32(SqlRecordParams[4].Value);
                if (intReturnType == 0)
                {
                    ReturnMessage = Convert.ToString(SqlRecordParams[3].Value);
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


    public class ModalityData
    {
        public string Code { get; set; } // CT, MR
        public string Tags { get; set; } // to be checked with dicom file modality. it can be comma seperated
        public string Path { get; set; } // to be stored in path
    } 

}
