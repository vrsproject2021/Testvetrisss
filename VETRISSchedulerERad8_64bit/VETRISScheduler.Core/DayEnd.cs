using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using VETRISScheduler.DAL;

namespace VETRISScheduler.Core
{
    public class DayEnd
    {
        #region Constructor
        public DayEnd()
        {
        }
        #endregion

        #region Variables
        Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
        string strSUID = string.Empty;
        string strFileName = string.Empty;
        string strInstCode = string.Empty;
        string strInstName = string.Empty;
        string strDelFile = "N";
        #endregion

        #region Properties
        public Guid STUDY_ID
        {
            get { return Id; }
            set { Id = value; }
        }
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
        public string INSTITUTION_CODE
        {
            get { return strInstCode; }
            set { strInstCode = value; }
        }
        public string INSTITUTION_NAME
        {
            get { return strInstName; }
            set { strInstName = value; }
        }
        public string DELETE_FILE
        {
            get { return strDelFile; }
            set { strDelFile = value; }
        }
        #endregion

        #region ProcessDayEnd
        public bool ProcessDayEnd(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0;
            string strReturnMessage = string.Empty;
           

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);


                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_day_end_process");

                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region FetchStudiesToDelete
        public bool FetchStudiesToDelete(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_day_end_fetch_studies_to_delete");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Study";
                    ds.Tables[1].TableName = "File";
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

        #region FetchImageRecordsToDelete
        public bool FetchImageRecordsToDelete(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_day_end_fetch_image_records_to_delete");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "FileRecord";
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

        #region DeleteDicomRouterStudy (suspended)
        //public bool DeleteDicomRouterStudy(string ConfigPath, int ServiceID, string strSvcName, ref string strReturnMessage,ref string CatchMessage)
        //{
        //    bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
        //    SqlParameter[] SqlRecordParams = new SqlParameter[3];

        //    try
        //    {
        //        SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
        //        SqlRecordParams[1] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[1].Direction = ParameterDirection.Output;
        //        SqlRecordParams[2] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[2].Direction = ParameterDirection.Output;

        //        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
        //        intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_day_end_dicom_record_delete", SqlRecordParams);
        //        intReturnType = Convert.ToInt32(SqlRecordParams[2].Value);
        //        strReturnMessage = Convert.ToString(SqlRecordParams[1].Value).Trim();

        //        if (intReturnType == 0)
        //        {
        //            bReturn = false;
        //            CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "DeleteDicomRouterStudy() - Error: " + strReturnMessage, true);
        //        }
        //        else if (intReturnType == 1)
        //        {
        //            bReturn = true;
        //        }


        //    }
        //    catch (Exception expErr)
        //    { bReturn = false; CatchMessage = expErr.Message; }

        //    return bReturn;
        //}
        #endregion

        #region DeleteUngroupedImageRecord
        public bool DeleteUngroupedImageRecord(string ConfigPath, int ServiceID, string strSvcName,ref string strReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[3];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[1].Direction = ParameterDirection.Output;
                SqlRecordParams[2] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[2].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_day_end_ungrouped_image_record_delete", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[2].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[1].Value).Trim();

                if (intReturnType == 0)
                {
                    bReturn = false;
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "DeleteUngroupedImageRecord() - Error: " + strReturnMessage, true);
                }
                else if (intReturnType == 1)
                {
                    bReturn = true;
                }

                
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region CheckAttachmentFileToDelete
        public bool CheckAttachmentFileToDelete(string ConfigPath, ref int FileCount, ref string CatchMessage)
        {
            bool bReturn = false;
            int intExecReturn = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@file_name", SqlDbType.NVarChar,4000); SqlRecordParams[0].Value = strFileName;
                SqlRecordParams[1] = new SqlParameter("@file_count", SqlDbType.VarChar, 500); SqlRecordParams[1].Direction = ParameterDirection.Output;
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_notification_attachment_check", SqlRecordParams);
                FileCount = Convert.ToInt32(SqlRecordParams[1].Value);
                bReturn = true;

            }
            catch (Exception expErr)
            {
                bReturn = false; CatchMessage = expErr.Message;
            }

            return bReturn;
        }
        #endregion

        #region DeleteStudy
        public bool DeleteStudy(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[3];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);


                SqlRecordParams[0] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strSUID;
                SqlRecordParams[1] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[1].Direction = ParameterDirection.Output;
                SqlRecordParams[2] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[2].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_study_delete", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[2].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[1].Value).Trim();

                if (intReturnType == 0)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "DeleteStudy() - Error: " + strReturnMessage, true);
                }
                else if (intReturnType == 1)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "DeleteStudy() - " + strReturnMessage, false);
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "-" + strSUID; }
            return bReturn;
        }
        #endregion

        #region CheckListenerFile
        public bool CheckListenerFile(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = true;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            DataSet ds = new DataSet();

            try
            {
                SqlRecordParams[0] = new SqlParameter("@file_name", SqlDbType.NVarChar, 500); SqlRecordParams[0].Value = strFileName;
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_downloaded_listener_file_check", SqlRecordParams);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        strSUID = Convert.ToString(ds.Tables[0].Rows[0]["study_uid"]).Trim();
                        strInstCode = Convert.ToString(ds.Tables[0].Rows[0]["institution_code"]).Trim();
                        strInstName = Convert.ToString(ds.Tables[0].Rows[0]["institution_name"]).Trim();
                        strDelFile = "N";
                    }
                    else
                        strDelFile = "Y";

                  
                }
                
                

            }
            catch (Exception expErr)
            {
                bReturn = false; CatchMessage = expErr.Message;
            }
            finally
            {
                ds.Dispose();
            }

            return bReturn;
        }
        #endregion

        #region ProcessLogDBDayEnd
        public bool ProcessLogDBDayEnd(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0;
     
            try
            {
                if (CoreCommon.LOGDB_CONNECTION_STRING == string.Empty) CoreCommon.GetLogDBConnectionString(ConfigPath);
                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.LOGDB_CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_day_end_log_db_process");
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion
    }
}
