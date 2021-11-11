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


namespace VETRIS.API.Core.DICOM_ROUTER
{
    public class DicomRouter
    {
        #region Constructor
        public DicomRouter()
        {
        }
        #endregion

        #region Variables
        private string strVersion = string.Empty;
        private string strInstName = string.Empty;
        private string strInstCode = string.Empty;
        private string strAddr1 = string.Empty;
        private string strAddr2 = string.Empty;
        private string strZip = string.Empty;
        private string strLogin = string.Empty;
        private string strStudyImgManualRecPath = string.Empty;
        private string strCompressFiles = "Y";
        private string strImpSessID = string.Empty;
        private int intImpFileCount = 0;
        private int intTransferFileCount = 0;
        private DateTime dt = DateTime.Now;
        private DateTime dtStart = DateTime.Now;
        private DateTime dtEnd = DateTime.Now;
        int intTimeTaken = 0;
        #endregion

        #region Properties
        public string VERSION
        {
            get { return strVersion; }
            set { strVersion = value; }
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
        public string ADDRESS_1
        {
            get { return strAddr1; }
            set { strAddr1 = value; }
        }
        public string ADDRESS_2
        {
            get { return strAddr2; }
            set { strAddr2 = value; }
        }
        public string ZIP
        {
            get { return strZip; }
            set { strZip = value; }
        }
        public string INSTITUTION_LOGIN_ID
        {
            get { return strLogin; }
            set { strLogin = value; }
        }
        public string STUDY_IMAGE_FILES_MANUAL_RECEIVING_PATH
        {
            get { return strStudyImgManualRecPath; }
            set { strStudyImgManualRecPath = value; }
        }
        public string COMPRESS_DICOM_FILES_TO_TRANSFER
        {
            get { return strCompressFiles; }
            set { strCompressFiles = value; }
        }
        public string IMPORT_SESSION_ID
        {
            get { return strImpSessID; }
            set { strImpSessID = value; }
        }
        public int IMPORTED_FILE_COUNT
        {
            get { return intImpFileCount; }
            set { intImpFileCount = value; }
        }
        public int TRANSFERRED_FILE_COUNT
        {
            get { return intTransferFileCount; }
            set { intTransferFileCount = value; }
        }
        public DateTime DATE
        {
            get { return dt; }
            set { dt = value; }
        }
        public DateTime START_DATE
        {
            get { return dtStart; }
            set { dtStart = value; }
        }
        public DateTime END_DATE
        {
            get { return dtEnd; }
            set { dtEnd = value; }
        }
        public int TIME_TAKEN
        {
            get { return intTimeTaken; }
            set { intTimeTaken = value; }
        }
        #endregion

        #region GetLatestVersion
        public bool GetLatestVersion(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@version_no", SqlDbType.NVarChar, 50); SqlRecordParams[0].Direction = ParameterDirection.Output;


            try
            {
                if (ConfigPath.Trim() != string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "dicom_router_latest_version_fetch", SqlRecordParams);

                if (SqlRecordParams[0].Value != DBNull.Value) strVersion = Convert.ToString(SqlRecordParams[0].Value); else strVersion = string.Empty;
                bReturn = true;


            }
            catch (Exception expErr)
            {
                bReturn = false;
                CatchMessage = expErr.Message;
            }



            return bReturn;
        }
        #endregion

        #region FetchInstitutionDetails
        public bool FetchInstitutionDetails(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intRetStatus = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[10];
            SqlRecordParams[0] = new SqlParameter("@code", SqlDbType.NVarChar, 5); SqlRecordParams[0].Value = strInstCode;
            SqlRecordParams[1] = new SqlParameter("@name", SqlDbType.NVarChar, 100); SqlRecordParams[1].Direction = ParameterDirection.Output;
            SqlRecordParams[2] = new SqlParameter("@address_1", SqlDbType.NVarChar, 100); SqlRecordParams[2].Direction = ParameterDirection.Output;
            SqlRecordParams[3] = new SqlParameter("@address_2", SqlDbType.NVarChar, 100); SqlRecordParams[3].Direction = ParameterDirection.Output;
            SqlRecordParams[4] = new SqlParameter("@zip", SqlDbType.NVarChar, 10); SqlRecordParams[4].Direction = ParameterDirection.Output;
            SqlRecordParams[5] = new SqlParameter("@login_id", SqlDbType.NVarChar, 30); SqlRecordParams[5].Direction = ParameterDirection.Output;
            SqlRecordParams[6] = new SqlParameter("@study_img_manual_receive_path", SqlDbType.NVarChar, 250); SqlRecordParams[6].Direction = ParameterDirection.Output;
            SqlRecordParams[7] = new SqlParameter("@xfer_files_compress", SqlDbType.NChar, 1); SqlRecordParams[7].Direction = ParameterDirection.Output;
            SqlRecordParams[8] = new SqlParameter("@output_msg", SqlDbType.NVarChar, 100); SqlRecordParams[8].Direction = ParameterDirection.Output;
            SqlRecordParams[9] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[9].Direction = ParameterDirection.Output;


            try
            {
                if (ConfigPath.Trim() != string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "dicom_router_institution_dtls_fetch", SqlRecordParams);
                intRetStatus =Convert.ToInt32(SqlRecordParams[9].Value);

                if (intRetStatus == 1)
                {
                    bReturn = true;
                    strInstName = Convert.ToString(SqlRecordParams[1].Value).Trim();
                    strAddr1 = Convert.ToString(SqlRecordParams[2].Value).Trim();
                    strAddr2 = Convert.ToString(SqlRecordParams[3].Value).Trim();
                    strZip = Convert.ToString(SqlRecordParams[4].Value).Trim();
                    strLogin = Convert.ToString(SqlRecordParams[5].Value).Trim();
                    strStudyImgManualRecPath = Convert.ToString(SqlRecordParams[6].Value).Trim();
                    strCompressFiles = Convert.ToString(SqlRecordParams[7].Value).Trim();
                }
                else
                {
                    bReturn = false;
                }

                ReturnMessage = Convert.ToString(SqlRecordParams[8].Value).Trim();

            }
            catch (Exception expErr)
            {
                bReturn = false;
                CatchMessage = expErr.Message;
            }



            return bReturn;
        }
        #endregion

        #region CheckImportSession
        public bool CheckImportSession(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intRetStatus = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[5];
            SqlRecordParams[0] = new SqlParameter("@institution_code", SqlDbType.NVarChar, 5); SqlRecordParams[0].Value = strInstCode;
            SqlRecordParams[1] = new SqlParameter("@import_session_id", SqlDbType.NVarChar, 30); SqlRecordParams[1].Value = strImpSessID;
            SqlRecordParams[2] = new SqlParameter("@files_imported", SqlDbType.Int); SqlRecordParams[2].Direction = ParameterDirection.Output;
            SqlRecordParams[3] = new SqlParameter("@output_msg", SqlDbType.NVarChar, 100); SqlRecordParams[3].Direction = ParameterDirection.Output;
            SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;


            try
            {
                if (ConfigPath.Trim() != string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "dicom_router_import_session_check", SqlRecordParams);
                intRetStatus = Convert.ToInt32(SqlRecordParams[4].Value);

                if (intRetStatus == 1)
                {
                    bReturn = true;
                    intImpFileCount = Convert.ToInt32(SqlRecordParams[2].Value);
                }
                else
                {
                    bReturn = false;
                }

                ReturnMessage = Convert.ToString(SqlRecordParams[3].Value).Trim();

            }
            catch (Exception expErr)
            {
                bReturn = false;
                CatchMessage = expErr.Message;
            }



            return bReturn;
        }
        #endregion

        #region UpdateOnlineStatus
        public bool UpdateOnlineStatus(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intRetStatus = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            SqlRecordParams[0] = new SqlParameter("@institution_code", SqlDbType.NVarChar, 5); SqlRecordParams[0].Value = strInstCode;
            SqlRecordParams[1] = new SqlParameter("@version_no", SqlDbType.NVarChar, 50); SqlRecordParams[1].Value = strVersion;
            SqlRecordParams[2] = new SqlParameter("@output_msg", SqlDbType.NVarChar, 100); SqlRecordParams[2].Direction = ParameterDirection.Output;
            SqlRecordParams[3] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;


            try
            {
                if (ConfigPath.Trim() != string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "dicom_router_online_status_update", SqlRecordParams);
                intRetStatus = Convert.ToInt32(SqlRecordParams[3].Value);

                if (intRetStatus == 1)
                {
                    bReturn = true;
                   
                }
                else
                {
                    bReturn = false;
                }

                ReturnMessage = Convert.ToString(SqlRecordParams[2].Value).Trim();

            }
            catch (Exception expErr)
            {
                bReturn = false;
                CatchMessage = expErr.Message;
            }



            return bReturn;
        }
        #endregion

        #region CreateFileUploadNotification
        public bool CreateFileUploadNotification(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intRetStatus = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[6];
            SqlRecordParams[0] = new SqlParameter("@institution_code", SqlDbType.NVarChar, 5); SqlRecordParams[0].Value = strInstCode;
            SqlRecordParams[1] = new SqlParameter("@import_session_id", SqlDbType.NVarChar, 30); SqlRecordParams[1].Value = strImpSessID;
            SqlRecordParams[2] = new SqlParameter("@file_count", SqlDbType.Int); SqlRecordParams[2].Value = intImpFileCount;
            SqlRecordParams[3] = new SqlParameter("@proc_start_datetime", SqlDbType.DateTime); SqlRecordParams[3].Value = dt;
            SqlRecordParams[4] = new SqlParameter("@output_msg", SqlDbType.NVarChar, 100); SqlRecordParams[4].Direction = ParameterDirection.Output;
            SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;


            try
            {
                if (ConfigPath.Trim() != string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "dicom_router_upload_notification_create", SqlRecordParams);
                intRetStatus = Convert.ToInt32(SqlRecordParams[5].Value);

                if (intRetStatus == 1)
                {
                    bReturn = true;

                }
                else
                {
                    bReturn = false;
                }

                ReturnMessage = Convert.ToString(SqlRecordParams[4].Value).Trim();

            }
            catch (Exception expErr)
            {
                bReturn = false;
                CatchMessage = expErr.Message;
            }



            return bReturn;
        }
        #endregion

        #region CreateFileDownloadNotification
        public bool CreateFileDownloadNotification(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intRetStatus = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[6];
            SqlRecordParams[0] = new SqlParameter("@institution_code", SqlDbType.NVarChar, 5); SqlRecordParams[0].Value = strInstCode;
            SqlRecordParams[1] = new SqlParameter("@import_session_id", SqlDbType.NVarChar, 30); SqlRecordParams[1].Value = strImpSessID;
            SqlRecordParams[2] = new SqlParameter("@file_count", SqlDbType.Int); SqlRecordParams[2].Value = intImpFileCount;
            SqlRecordParams[3] = new SqlParameter("@download_datetime", SqlDbType.DateTime); SqlRecordParams[3].Value = dt;
            SqlRecordParams[4] = new SqlParameter("@output_msg", SqlDbType.NVarChar, 100); SqlRecordParams[4].Direction = ParameterDirection.Output;
            SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;


            try
            {
                if (ConfigPath.Trim() != string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "dicom_router_download_notification_create", SqlRecordParams);
                intRetStatus = Convert.ToInt32(SqlRecordParams[5].Value);

                if (intRetStatus == 1)
                {
                    bReturn = true;

                }
                else
                {
                    bReturn = false;
                }

                ReturnMessage = Convert.ToString(SqlRecordParams[4].Value).Trim();

            }
            catch (Exception expErr)
            {
                bReturn = false;
                CatchMessage = expErr.Message;
            }



            return bReturn;
        }
        #endregion

        #region CreateFileTransferNotification
        public bool CreateFileTransferNotification(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intRetStatus = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[7];
            SqlRecordParams[0] = new SqlParameter("@institution_code", SqlDbType.NVarChar, 5); SqlRecordParams[0].Value = strInstCode;
            SqlRecordParams[1] = new SqlParameter("@import_session_id", SqlDbType.NVarChar, 30); SqlRecordParams[1].Value = strImpSessID;
            SqlRecordParams[2] = new SqlParameter("@file_count", SqlDbType.Int); SqlRecordParams[2].Value = intImpFileCount;
            SqlRecordParams[3] = new SqlParameter("@upload_datetime", SqlDbType.DateTime); SqlRecordParams[3].Value = dtStart;
            SqlRecordParams[4] = new SqlParameter("@download_datetime", SqlDbType.DateTime); SqlRecordParams[4].Value = dtEnd;
            SqlRecordParams[5] = new SqlParameter("@output_msg", SqlDbType.NVarChar, 100); SqlRecordParams[5].Direction = ParameterDirection.Output;
            SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;


            try
            {
                if (ConfigPath.Trim() != string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "dicom_router_transfer_notification_create", SqlRecordParams);
                intRetStatus = Convert.ToInt32(SqlRecordParams[6].Value);

                if (intRetStatus == 1)
                {
                    bReturn = true;

                }
                else
                {
                    bReturn = false;
                }

                ReturnMessage = Convert.ToString(SqlRecordParams[5].Value).Trim();

            }
            catch (Exception expErr)
            {
                bReturn = false;
                CatchMessage = expErr.Message;
            }



            return bReturn;
        }
        #endregion

        #region CreateFileTransferOTNotification
        public bool CreateFileTransferOTNotification(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intRetStatus = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[8];
            SqlRecordParams[0] = new SqlParameter("@institution_code", SqlDbType.NVarChar, 5); SqlRecordParams[0].Value = strInstCode;
            SqlRecordParams[1] = new SqlParameter("@import_session_id", SqlDbType.NVarChar, 30); SqlRecordParams[1].Value = strImpSessID;
            SqlRecordParams[2] = new SqlParameter("@imported_file_count", SqlDbType.Int); SqlRecordParams[2].Value = intImpFileCount;
            SqlRecordParams[3] = new SqlParameter("@transfer_file_count", SqlDbType.Int); SqlRecordParams[3].Value = intTransferFileCount;
            SqlRecordParams[4] = new SqlParameter("@upload_datetime", SqlDbType.DateTime); SqlRecordParams[4].Value = dt;
            SqlRecordParams[5] = new SqlParameter("@time_taken_mins", SqlDbType.Int); SqlRecordParams[5].Value = intTimeTaken;
            SqlRecordParams[6] = new SqlParameter("@output_msg", SqlDbType.NVarChar, 100); SqlRecordParams[6].Direction = ParameterDirection.Output;
            SqlRecordParams[7] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[7].Direction = ParameterDirection.Output;


            try
            {
                if (ConfigPath.Trim() != string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "dicom_router_transfer_overtime_notification_create", SqlRecordParams);
                intRetStatus = Convert.ToInt32(SqlRecordParams[7].Value);

                if (intRetStatus == 1)
                {
                    bReturn = true;

                }
                else
                {
                    bReturn = false;
                }

                ReturnMessage = Convert.ToString(SqlRecordParams[6].Value).Trim();

            }
            catch (Exception expErr)
            {
                bReturn = false;
                CatchMessage = expErr.Message;
            }



            return bReturn;
        }
        #endregion
    }
}
