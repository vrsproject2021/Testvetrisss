using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using VETRIS.API.Core;

namespace VETRIS.API.Models
{
    public class DicomRouter
    {
        #region Variables
        string strResponseCode = string.Empty;
        string strResponseMessage = string.Empty;
        string strLatestVersion = string.Empty;
        private string strInstName = string.Empty;
        private string strInstCode = string.Empty;
        private string strAddr1 = string.Empty;
        private string strAddr2 = string.Empty;
        private string strZip = string.Empty;
        private string strInstLoginID = string.Empty;
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
        public string LATEST_VERSION
        {
            get { return strLatestVersion; }
            set { strLatestVersion = value; }
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
            get { return strInstLoginID; }
            set { strInstLoginID = value; }
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
        public string RESPONSE_MESSAGE
        {
            get { return strResponseMessage; }
            set { strResponseMessage = value; }
        }
        #endregion

        #region GetLatestVersion
        public bool GetLatestVersion()
        {
            bool bReturn = false;
            string strCatchMsg = string.Empty;
            VETRIS.API.Core.DICOM_ROUTER.DicomRouter objCore = new Core.DICOM_ROUTER.DicomRouter();

            try
            {

                bReturn = objCore.GetLatestVersion(AppDomain.CurrentDomain.BaseDirectory,ref strCatchMsg);
                strLatestVersion = objCore.VERSION;
                strResponseMessage = "SUCCESS";
                bReturn = true;
            }
            catch (Exception expr)
            {
                bReturn = false;
                
                //LsResponseCode = "ERR";
                strResponseMessage = expr.Message;
            }
            finally
            { objCore = null; }

            return bReturn;
        }
        #endregion

        #region FetchInstitutionDetails
        public bool FetchInstitutionDetails()
        {
            bool bReturn = false;
            string strReturnMsg = string.Empty;
            string strCatchMsg = string.Empty;
            VETRIS.API.Core.DICOM_ROUTER.DicomRouter objCore = new Core.DICOM_ROUTER.DicomRouter();

            try
            {
                objCore.INSTITUTION_CODE = strInstCode;
                bReturn = objCore.FetchInstitutionDetails(AppDomain.CurrentDomain.BaseDirectory, ref strReturnMsg, ref strCatchMsg);

                if(bReturn)
                {
                    strInstName = objCore.INSTITUTION_NAME;
                    strAddr1 = objCore.ADDRESS_1;
                    strAddr2 = objCore.ADDRESS_2;
                    strZip = objCore.ZIP;
                    strStudyImgManualRecPath = objCore.STUDY_IMAGE_FILES_MANUAL_RECEIVING_PATH;
                    strInstLoginID = objCore.INSTITUTION_LOGIN_ID;
                    strCompressFiles = objCore.COMPRESS_DICOM_FILES_TO_TRANSFER;
                }
               
                if (strCatchMsg.Trim() == string.Empty)
                    strResponseMessage = strReturnMsg;
                else
                    strResponseMessage = strCatchMsg;

                
            }
            catch (Exception expr)
            {
                bReturn = false;

                //LsResponseCode = "ERR";
                strResponseMessage = expr.Message;
            }
            finally
            { objCore = null; }

            return bReturn;
        }
        #endregion

        #region CheckImportSession
        public bool CheckImportSession()
        {
            bool bReturn = false;
            string strReturnMsg = string.Empty;
            string strCatchMsg = string.Empty;
            VETRIS.API.Core.DICOM_ROUTER.DicomRouter objCore = new Core.DICOM_ROUTER.DicomRouter();

            try
            {
                objCore.INSTITUTION_CODE = strInstCode;
                objCore.IMPORT_SESSION_ID = strImpSessID;
                bReturn = objCore.CheckImportSession(AppDomain.CurrentDomain.BaseDirectory, ref strReturnMsg, ref strCatchMsg);

                if (bReturn)
                {
                    intImpFileCount = objCore.IMPORTED_FILE_COUNT;
                }

                if (strCatchMsg.Trim() == string.Empty)
                    strResponseMessage = strReturnMsg;
                else
                    strResponseMessage = strCatchMsg;


            }
            catch (Exception expr)
            {
                bReturn = false;

                //LsResponseCode = "ERR";
                strResponseMessage = expr.Message;
            }
            finally
            { objCore = null; }

            return bReturn;
        }
        #endregion

        #region UpdateOnlineStatus
        public bool UpdateOnlineStatus()
        {
            bool bReturn = false;
            string strReturnMsg = string.Empty;
            string strCatchMsg = string.Empty;
            VETRIS.API.Core.DICOM_ROUTER.DicomRouter objCore = new Core.DICOM_ROUTER.DicomRouter();

            try
            {
                objCore.INSTITUTION_CODE = strInstCode;
                objCore.VERSION = strLatestVersion;
                bReturn = objCore.UpdateOnlineStatus(AppDomain.CurrentDomain.BaseDirectory, ref strReturnMsg, ref strCatchMsg);

                

                if (strCatchMsg.Trim() == string.Empty)
                    strResponseMessage = strReturnMsg;
                else
                    strResponseMessage = strCatchMsg;


            }
            catch (Exception expr)
            {
                bReturn = false;
                strResponseMessage = expr.Message;
            }
            finally
            { objCore = null; }

            return bReturn;
        }
        #endregion

        #region CreateFileUploadNotification
        public bool CreateFileUploadNotification()
        {
            bool bReturn = false;
            string strReturnMsg = string.Empty;
            string strCatchMsg = string.Empty;
            VETRIS.API.Core.DICOM_ROUTER.DicomRouter objCore = new Core.DICOM_ROUTER.DicomRouter();

            try
            {
                objCore.INSTITUTION_CODE = strInstCode;
                objCore.IMPORT_SESSION_ID = strImpSessID;
                objCore.IMPORTED_FILE_COUNT = intImpFileCount;
                objCore.DATE = dt;
              
                bReturn = objCore.CreateFileUploadNotification(AppDomain.CurrentDomain.BaseDirectory, ref strReturnMsg, ref strCatchMsg);



                if (strCatchMsg.Trim() == string.Empty)
                    strResponseMessage = strReturnMsg;
                else
                    strResponseMessage = strCatchMsg;


            }
            catch (Exception expr)
            {
                bReturn = false;
                strResponseMessage = expr.Message;
            }
            finally
            { objCore = null; }

            return bReturn;
        }
        #endregion

        #region CreateFileDownloadNotification
        public bool CreateFileDownloadNotification()
        {
            bool bReturn = false;
            string strReturnMsg = string.Empty;
            string strCatchMsg = string.Empty;
            VETRIS.API.Core.DICOM_ROUTER.DicomRouter objCore = new Core.DICOM_ROUTER.DicomRouter();

            try
            {
                objCore.INSTITUTION_CODE = strInstCode;
                objCore.IMPORT_SESSION_ID = strImpSessID;
                objCore.IMPORTED_FILE_COUNT = intImpFileCount;
                objCore.DATE = dt;

                bReturn = objCore.CreateFileDownloadNotification(AppDomain.CurrentDomain.BaseDirectory, ref strReturnMsg, ref strCatchMsg);



                if (strCatchMsg.Trim() == string.Empty)
                    strResponseMessage = strReturnMsg;
                else
                    strResponseMessage = strCatchMsg;


            }
            catch (Exception expr)
            {
                bReturn = false;
                strResponseMessage = expr.Message;
            }
            finally
            { objCore = null; }

            return bReturn;
        }
        #endregion

        #region CreateFileTransferNotification
        public bool CreateFileTransferNotification()
        {
            bool bReturn = false;
            string strReturnMsg = string.Empty;
            string strCatchMsg = string.Empty;
            VETRIS.API.Core.DICOM_ROUTER.DicomRouter objCore = new Core.DICOM_ROUTER.DicomRouter();

            try
            {
                objCore.INSTITUTION_CODE = strInstCode;
                objCore.IMPORT_SESSION_ID = strImpSessID;
                objCore.IMPORTED_FILE_COUNT = intImpFileCount;
                objCore.START_DATE = dtStart;
                objCore.END_DATE = dtEnd;

                bReturn = objCore.CreateFileTransferNotification(AppDomain.CurrentDomain.BaseDirectory, ref strReturnMsg, ref strCatchMsg);



                if (strCatchMsg.Trim() == string.Empty)
                    strResponseMessage = strReturnMsg;
                else
                    strResponseMessage = strCatchMsg;


            }
            catch (Exception expr)
            {
                bReturn = false;
                strResponseMessage = expr.Message;
            }
            finally
            { objCore = null; }

            return bReturn;
        }
        #endregion

        #region CreateFileTransferOTNotification
        public bool CreateFileTransferOTNotification()
        {
            bool bReturn = false;
            string strReturnMsg = string.Empty;
            string strCatchMsg = string.Empty;
            VETRIS.API.Core.DICOM_ROUTER.DicomRouter objCore = new Core.DICOM_ROUTER.DicomRouter();

            try
            {
                objCore.INSTITUTION_CODE = strInstCode;
                objCore.IMPORT_SESSION_ID = strImpSessID;
                objCore.IMPORTED_FILE_COUNT = intImpFileCount;
                objCore.TRANSFERRED_FILE_COUNT = intTransferFileCount;
                objCore.DATE = dt;
                objCore.TIME_TAKEN = intTimeTaken;

                bReturn = objCore.CreateFileTransferOTNotification(AppDomain.CurrentDomain.BaseDirectory, ref strReturnMsg, ref strCatchMsg);



                if (strCatchMsg.Trim() == string.Empty)
                    strResponseMessage = strReturnMsg;
                else
                    strResponseMessage = strCatchMsg;


            }
            catch (Exception expr)
            {
                bReturn = false;
                strResponseMessage = expr.Message;
            }
            finally
            { objCore = null; }

            return bReturn;
        }
        #endregion
    }
}