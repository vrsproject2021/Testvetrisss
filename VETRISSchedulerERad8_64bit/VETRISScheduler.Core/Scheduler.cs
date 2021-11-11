using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using VETRISScheduler.DAL;

namespace VETRISScheduler.Core
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

        string strSUID = string.Empty;
        Guid InstID = new Guid();
        string strInsCode = string.Empty;
        string strInstName = string.Empty;
        string strStudyExists = "N";

        #region Scheduler Settings
        string strWS8SRVIP = string.Empty;
        string strWS8CLTIP = string.Empty;
        string strAPIVER = string.Empty;
        string strWS8SRVUID = string.Empty;
        string strWS8SRVPWD = string.Empty;

        string strURL = string.Empty;
        string strRptURL = string.Empty;
        string strPACSImgViewURL = string.Empty;
        string strPACSStudyViewURL = string.Empty;
        string strPACSRptViewURL = string.Empty;
        string strUSRUPDURL = string.Empty;
        string strPACSUserID = string.Empty;
        string strPACSUserPwd = string.Empty;
        string strXFEREXEPATH = string.Empty;
        string strXFEREXEPATHALT = string.Empty;
        string strXFEREXEPARMS = string.Empty;
        string strXFEREXEPARMSJPGLL = string.Empty;
        string strXFEREXEPARMJ2KLL = string.Empty;
        string strXFEREXEPARMJ2KLS = string.Empty;
        string strXFEREXEPARMSSENDDCM = string.Empty;
        string strImgtoDCMExePath = string.Empty;
        string strPDFtoImgExePath = string.Empty;
        string strDocDCMExePath = string.Empty;
        string strDocDCMPath = string.Empty;

        string strFTPHOST = string.Empty;
        int intFTPPORT = 0;
        string strFTPUSER = string.Empty;
        string strFTPPWD = string.Empty;
        string strFTPFLDR = string.Empty;
        string strFTPDLFLDRTMP = string.Empty;
        string strFTPLOGFLDR = string.Empty;
        string strFTPLOGDLFLDRTMP = string.Empty;
        string strFTPDLMODE = string.Empty;
        string strFTPSRCFOLDER = string.Empty;
        string strPACSXFERDLFLDR = string.Empty;
        string strPACSARCHIVEFLDR = string.Empty;
        string strPACSARCHALTFLDR = string.Empty;
        string strWRITEBACKURL = string.Empty;
        string strDCMMODIFYEXEPATH = string.Empty;
        string strDCMDMPEXEPATH = string.Empty;
        string strDCMRCVRFLDR = string.Empty;
        string strFILESHOLDPATH = string.Empty;
        string strSENDLFTOPACS = string.Empty;
        string strTEMPDCMATCHPATH = string.Empty;
        string strLFPATH = string.Empty;
        string strSCHCASVCENBL = string.Empty;

        string strDCMRCVEXEPATH = string.Empty;
        string strDCMLSNFLDR1 = string.Empty;
        string strDCMLSNFLDR2 = string.Empty;
        string strDCMLSNFLDR3 = string.Empty;
        string strDCMLSNFLDR4 = string.Empty;
        string strRCVSYNTAX1 = string.Empty;
        string strRCVSYNTAX2 = string.Empty;
        string strRCVSYNTAX3 = string.Empty;
        string strRCVSYNTAX4 = string.Empty;
        string strENBLLDS = string.Empty;
        int intDSNOOFPORTS = 0;
        string strENBLLSPACSXFER = string.Empty;
        string strUMDCMFILES = string.Empty;
        string strREJDCMFILES = string.Empty;

        int intServiceID = 0;
        string strServiceName = string.Empty;
        int intFrequency = 0;
        string strLogType = "";
        string[] arrFields = new string[0];
        #endregion

        #region Mail Settings
        string strMailServer = string.Empty;
        int intPortNo = 0;
        string strSSL = string.Empty;
        string strMailUserID = string.Empty;
        string strMailUserPwd = string.Empty;
        string strMailSender = string.Empty;
        string strMailInvFolder = string.Empty;
        string strRptServerURL = string.Empty;
        string strRptServerFolder = string.Empty;
        #endregion

        #region SMS Settings
        string strSenderNo = string.Empty;
        string strAcctSID = string.Empty;
        string strAuthToken = string.Empty;
        #endregion

        #region Fax Settings
        string strFAXAPIURL = string.Empty;
        string strFAXAUTHUSERID = string.Empty;
        string strFAXAUTHPWD = string.Empty;
        string strFAXCSID = string.Empty;
        string strFAXREFTEXT = string.Empty;
        string strFAXREPADDR = string.Empty;
        string strFAXCONTACT = string.Empty;
        int intFAXRETRY = 0;
        string strFAXFILEFLDR = string.Empty;

        #endregion

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

        public string STUDY_UID
        {
            get { return strSUID; }
            set { strSUID = value; }
        }
        public Guid INSTITUTION_ID
        {
            get { return InstID; }
            set { InstID = value; }
        }
        public string INSTITUTION_CODE
        {
            get { return strInsCode; }
            set { strInsCode = value; }
        }
        public string INSTITUTION_NAME
        {
            get { return strInstName; }
            set { strInstName = value; }
        }
        public string STUDY_EXISTS
        {
            get { return strStudyExists; }
            set { strStudyExists = value; }
        }

        #region Scheduler Settings
        public string WS8_SERVER_URL
        {
            get { return strWS8SRVIP; }
            set { strWS8SRVIP = value; }
        }
        public string CLIENT_IP_URL
        {
            get { return strWS8CLTIP; }
            set { strWS8CLTIP = value; }
        }
        public string API_VERSION
        {
            get { return strAPIVER; }
            set { strAPIVER = value; }
        }
        public string WS8_USER_ID
        {
            get { return strWS8SRVUID; }
            set { strWS8SRVUID = value; }
        }
        public string WS8_PASSWORD
        {
            get { return strWS8SRVPWD; }
            set { strWS8SRVPWD = value; }
        }
        public string URL
        {
            get { return strURL; }
            set { strURL = value; }
        }
        public string PACS_STUDY_VIEW_URL
        {
            get { return strPACSStudyViewURL; }
            set { strPACSStudyViewURL = value; }
        }
        public string PACS_IMAGE_VIEW_URL
        {
            get { return strPACSImgViewURL; }
            set { strPACSImgViewURL = value; }
        }
        public string PACS_REPORT_VIEW_URL
        {
            get { return strPACSRptViewURL; }
            set { strPACSRptViewURL = value; }
        }
        public string PACS_USER_ID
        {
            get { return strPACSUserID; }
            set { strPACSUserID = value; }
        }
        public string PACS_USER_PASSWORD
        {
            get { return strPACSUserPwd; }
            set { strPACSUserPwd = value; }
        }
        public string REPORT_FETCH_URL
        {
            get { return strRptURL; }
            set { strRptURL = value; }
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
        public string PACS_TRANSFER_EXE_PATH
        {
            get { return strXFEREXEPATH; }
            set { strXFEREXEPATH = value; }
        }
        public string PACS_TRANSFER_EXE_ALTERNATE_PATH
        {
            get { return strXFEREXEPATHALT; }
            set { strXFEREXEPATHALT = value; }
        }
        public string PACS_TRANSFER_EXE_PARAMS
        {
            get { return strXFEREXEPARMS; }
            set { strXFEREXEPARMS = value; }
        }
        public string PACS_TRANSFER_EXE_PARAMS_FOR_JPG_LOSSLESS
        {
            get { return strXFEREXEPARMSJPGLL; }
            set { strXFEREXEPARMSJPGLL = value; }
        }
        public string PACS_TRANSFER_EXE_PARAMS_FOR_JPG2K_LOSSLESS
        {
            get { return strXFEREXEPARMJ2KLL; }
            set { strXFEREXEPARMJ2KLL = value; }
        }
        public string PACS_TRANSFER_EXE_PARAMS_FOR_JPG2K_LOSSY
        {
            get { return strXFEREXEPARMJ2KLS; }
            set { strXFEREXEPARMJ2KLS = value; }
        }
        public string PACS_TRANSFER_EXE_PARAMS_FOR_SEND_DCM
        {
            get { return strXFEREXEPARMSSENDDCM; }
            set { strXFEREXEPARMSSENDDCM = value; }
        }
        public string PACS_ARCHIVE_FOLDER
        {
            get { return strPACSARCHIVEFLDR; }
            set { strPACSARCHIVEFLDR = value; }
        }
        public string PACS_ARCHIVE_ALTERNATE_FOLDER
        {
            get { return strPACSARCHALTFLDR; }
            set { strPACSARCHALTFLDR = value; }
        }
        public string IMAGE_TO_DCM_EXE_PATH
        {
            get { return strImgtoDCMExePath; }
            set { strImgtoDCMExePath = value; }
        }
        public string PDF_TO_IMAGE_EXE_PATH
        {
            get { return strPDFtoImgExePath; }
            set { strPDFtoImgExePath = value; }
        }
        public string DICOM_TAG_MODIFY_EXE_PATH
        {
            get { return strDCMMODIFYEXEPATH; }
            set { strDCMMODIFYEXEPATH = value; }
        }
        public string DICOM_TAG_DUMP_EXE_PATH
        {
            get { return strDCMDMPEXEPATH; }
            set { strDCMDMPEXEPATH = value; }
        }
        public string DOCUMENT_AND_DCM_PATH
        {
            get { return strDocDCMPath; }
            set { strDocDCMPath = value; }
        }
        public string TEMPORARY_DCM_ATTCHMENT_FILE_PATH
        {
            get { return strTEMPDCMATCHPATH; }
            set { strTEMPDCMATCHPATH = value; }
        }
        public string USER_UPDATE_URL
        {
            get { return strUSRUPDURL; }
            set { strUSRUPDURL = value; }
        }
        public string FTP_HOST
        {
            get { return strFTPHOST; }
            set { strFTPFLDR = value; }
        }
        public int FTP_PORT_NUMBER
        {
            get { return intFTPPORT; }
            set { intFTPPORT = value; }
        }
        public string FTP_USER
        {
            get { return strFTPUSER; }
            set { strFTPUSER = value; }
        }
        public string FTP_PASSWORD
        {
            get { return strFTPPWD; }
            set { strFTPPWD = value; }
        }
        public string FTP_FOLDER
        {
            get { return strFTPFLDR; }
            set { strFTPFLDR = value; }
        }
        public string TEMPORARY_DOWNLOAD_FOLDER
        {
            get { return strFTPDLFLDRTMP; }
            set { strFTPDLFLDRTMP = value; }
        }
        public string FTP_LOG_FOLDER
        {
            get { return strFTPLOGFLDR; }
            set { strFTPLOGFLDR = value; }
        }
        public string TEMPORARY_LOG_DOWNLOAD_FOLDER
        {
            get { return strFTPLOGDLFLDRTMP; }
            set { strFTPLOGDLFLDRTMP = value; }
        }
        public string FTP_DOWNLOAD_MODE
        {
            get { return strFTPDLMODE; }
            set { strFTPDLMODE = value; }
        }
        public string FTP_SOURCE_FOLDER
        {
            get { return strFTPSRCFOLDER; }
            set { strFTPSRCFOLDER = value; }
        }
        public string FOLDER_FOR_PACS_TRANSFER
        {
            get { return strPACSXFERDLFLDR; }
            set { strPACSXFERDLFLDR = value; }
        }
        public string DICOM_RECEIVER_FOLDER
        {
            get { return strDCMRCVRFLDR; }
            set { strDCMRCVRFLDR = value; }
        }
        public string WRITE_BACK_URL
        {
            get { return strWRITEBACKURL; }
            set { strWRITEBACKURL = value; }
        }
        public string FILES_ON_HOLD_FOLDER
        {
            get { return strFILESHOLDPATH; }
            set { strFILESHOLDPATH = value; }
        }
        public string SEND_LISTENER_FILES_TO_PACS
        {
            get { return strSENDLFTOPACS; }
            set { strSENDLFTOPACS = value; }
        }
        public string[] FIELD
        {
            get { return arrFields; }
            set { arrFields = value; }
        }
        public string LISTENER_FILES_PATH_TO_SYNC
        {
            get { return strLFPATH; }
            set { strLFPATH = value; }
        }
        public string CASE_ASSIGNMENT_SERVICE_ENABLED
        {
            get { return strSCHCASVCENBL ; }
            set { strSCHCASVCENBL = value; }
        }

        public string DICOM_RECEIVER_EXE_PATH
        {
            get { return strDCMRCVEXEPATH; }
            set { strDCMRCVEXEPATH = value; }
        }
        public string DICOM_LISTENER_FOLDER_1
        {
            get { return strDCMLSNFLDR1; }
            set { strDCMLSNFLDR1 = value; }
        }
        public string DICOM_LISTENER_FOLDER_2
        {
            get { return strDCMLSNFLDR2; }
            set { strDCMLSNFLDR2 = value; }
        }
        public string DICOM_LISTENER_FOLDER_3
        {
            get { return strDCMLSNFLDR3; }
            set { strDCMLSNFLDR3 = value; }
        }
        public string DICOM_LISTENER_FOLDER_4
        {
            get { return strDCMLSNFLDR4; }
            set { strDCMLSNFLDR4 = value; }
        }
        public string DICOM_LISTENER_SYNTAX_1
        {
            get { return strRCVSYNTAX1; }
            set { strRCVSYNTAX1 = value; }
        }
        public string DICOM_LISTENER_SYNTAX_2
        {
            get { return strRCVSYNTAX2; }
            set { strRCVSYNTAX2 = value; }
        }
        public string DICOM_LISTENER_SYNTAX_3
        {
            get { return strRCVSYNTAX3; }
            set { strRCVSYNTAX3 = value; }
        }
        public string DICOM_LISTENER_SYNTAX_4
        {
            get { return strRCVSYNTAX4; }
            set { strRCVSYNTAX4 = value; }
        }
        public string ENABLE_LISTENING_IN_DISTRIBUTOR_SERVICE
        {
            get { return strENBLLDS; }
            set { strENBLLDS = value; }
        }
        public int NUMBER_OF_LISTENING_PORTS_IN_DISTRIBUTOR_SERVICE
        {
            get { return intDSNOOFPORTS; }
            set { intDSNOOFPORTS = value; }
        }

        public string ENABLE_PACS_TRANSFER_FOR_LISTENER_FILES
        {
            get { return strENBLLSPACSXFER; }
            set { strENBLLSPACSXFER = value; }
        }
        public string UNKNOWN_MODALITY_DICOM_FILES
        {
            get { return strUMDCMFILES; }
            set { strUMDCMFILES = value; }
        }
        public string REJECTED_DICOM_FILES_PATH
        {
            get { return strREJDCMFILES; }
            set { strREJDCMFILES = value; }
        }

        #endregion

        #region Mail Settings
        public string MAIL_SERVER_NAME
        {
            get { return strMailServer; }
            set { strMailServer = value; }
        }
        public int MAIL_SERVER_PORT_NUMBER
        {
            get { return intPortNo; }
            set { intPortNo = value; }
        }
        public string SSL_ENABLED
        {
            get { return strSSL; }
            set { strSSL = value; }
        }
        public string MAIL_SERVER_USER_ID
        {
            get { return strMailUserID; }
            set { strMailUserID = value; }
        }
        public string MAIL_SERVER_USER_PASSWORD
        {
            get { return strMailUserPwd; }
            set { strMailUserPwd = value; }
        }
        public string MAIL_SENDER_NAME
        {
            get { return strMailSender; }
            set { strMailSender = value; }
        }
        public string MAIL_INVOICE_FOLDER
        {
            get { return strMailInvFolder; }
            set { strMailInvFolder = value; }
        }
        public string REPORT_SERVER_URL
        {
            get { return strRptServerURL; }
            set { strRptServerURL = value; }
        }
        public string REPORT_SERVER_FOLDER
        {
            get { return strRptServerFolder; }
            set { strRptServerFolder = value; }
        }
        #endregion

        #region SMS Settings
        public string SENDER_NO
        {
            get { return strSenderNo; }
            set { strSenderNo = value; }
        }
        public string ACCOUNT_SID
        {
            get { return strAcctSID; }
            set { strAcctSID = value; }
        }
        public string AUTHORISED_TOKEN
        {
            get { return strAuthToken; }
            set { strAuthToken = value; }
        }
        #endregion

        #region Fax Settings
        public string FAX_API_URL
        {
            get { return strFAXAPIURL; }
            set { strFAXAPIURL = value; }
        }
        public string FAX_USER_ID
        {
            get { return strFAXAUTHUSERID; }
            set { strFAXAUTHUSERID = value; }
        }
        public string FAX_PASSWORD
        {
            get { return strFAXAUTHPWD; }
            set { strFAXAUTHPWD = value; }
        }
        public string FAX_CSID
        {
            get { return strFAXCSID; }
            set { strFAXCSID = value; }
        }
        public string FAX_REFERENCE_TEXT
        {
            get { return strFAXREFTEXT; }
            set { strFAXREFTEXT = value; }
        }
        public string FAX_REPLY_ADDRESS
        {
            get { return strFAXREPADDR; }
            set { strFAXREPADDR = value; }
        }
        public string FAX_CONTACT
        {
            get { return strFAXCONTACT; }
            set { strFAXCONTACT = value; }
        }
        public string FAX_FILE_FOLDER
        {
            get { return strFAXFILEFLDR; }
            set { strFAXFILEFLDR = value; }
        }
        public int FAX_RETRIES_TO_PERFORM
        {
            get { return intFAXRETRY; }
            set { intFAXRETRY = value; }
        }
        #endregion


        #endregion

        #region GetServiceDetails
        public bool GetServiceDetails(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = false;
            DataSet ds = new DataSet();
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            StringBuilder sb = new StringBuilder();
            string strControlCode = string.Empty;
            int idx = 0;

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

                switch (intServiceID)
                {
                   
                    case 1:
                        #region New Data Synch Service
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            strControlCode = Convert.ToString(dr["control_code"]).Trim();
                            switch (strControlCode)
                            {
                                case"WS8SRVIP":
                                    strWS8SRVIP = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "WS8CLTIP":
                                    strWS8CLTIP = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "WS8SRVUID":
                                    strWS8SRVUID = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "WS8SRVPWD":
                                    strWS8SRVPWD= Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "LFPATH":
                                    strLFPATH = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "DCMDMPEXEPATH":
                                    strDCMDMPEXEPATH = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "FTPSRCFOLDER":
                                    strFTPSRCFOLDER = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                            }
                        }

                        arrFields = new string[ds.Tables[2].Rows.Count];
                       
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            arrFields[idx] = Convert.ToString(dr["field_code"]);
                            idx = idx + 1;
                        }
                       
                        #endregion
                        break;
                    case 2:
                        #region Write Back Service
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            strControlCode = Convert.ToString(dr["control_code"]).Trim();
                            switch (strControlCode)
                            {
                                case "XFEREXEPATH":
                                    strXFEREXEPATH = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPARMS":
                                    strXFEREXEPARMS = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "IMG2DCMEXEPATH":
                                    strImgtoDCMExePath = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "PDF2IMGEXEPATH":
                                    strPDFtoImgExePath = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "DOCDCMPATH":
                                    strDocDCMPath = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "USRUPDURL":
                                    strUSRUPDURL = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "WS8SRVIP":
                                    strWS8SRVIP = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "WS8CLTIP":
                                    strWS8CLTIP = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "WS8SRVUID":
                                    strWS8SRVUID = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "WS8SRVPWD":
                                    strWS8SRVPWD = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPARMSJPGLL":
                                    strXFEREXEPARMSJPGLL = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPARMJ2KLL":
                                    strXFEREXEPARMJ2KLL = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPARMJ2KLS":
                                    strXFEREXEPARMJ2KLS = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPARMSSENDDCM":
                                    strXFEREXEPARMSSENDDCM = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPATHALT":
                                    strXFEREXEPATHALT = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "TEMPDCMATCHPATH":
                                    strTEMPDCMATCHPATH = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "PACSARCHIVEFLDR":
                                    strPACSARCHIVEFLDR = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                
                            }
                        }
                        #endregion
                        break;
                    case 3:
                        #region Status Update Service
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            strControlCode = Convert.ToString(dr["control_code"]).Trim();

                            switch (strControlCode)
                            {
                                case "NEWDATAURL":
                                    strURL = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "RPTFETCHURL":
                                    strRptURL = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "PACIMGVWRURL":
                                    strPACSImgViewURL = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "PACLOGINURL":
                                    strPACSStudyViewURL = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "PACSRPTVWRURL":
                                    strPACSRptViewURL = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "PACSUSERID":
                                    strPACSUserID = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "PACSUSERPWD":
                                    strPACSUserPwd = Convert.ToString(dr["data_type_string"]).Trim();
                                    strPACSUserPwd = CoreCommon.DecryptString(strPACSUserPwd);
                                    break;
                                case "WS8SRVIP":
                                    strWS8SRVIP = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "WS8CLTIP":
                                    strWS8CLTIP = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "WS8SRVUID":
                                    strWS8SRVUID = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "WS8SRVPWD":
                                    strWS8SRVPWD = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                            }
                        }

                        arrFields = new string[ds.Tables[2].Rows.Count];
                       
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            arrFields[idx] = Convert.ToString(dr["field_code"]);
                            idx = idx + 1;
                        }

                        strPACSImgViewURL = strPACSImgViewURL.Replace("#V2", strPACSUserID);
                        strPACSImgViewURL = strPACSImgViewURL.Replace("#V3", strPACSUserPwd);

                        strPACSStudyViewURL = strPACSStudyViewURL.Replace("#V2", strPACSUserID);
                        strPACSStudyViewURL = strPACSStudyViewURL.Replace("#V3", strPACSUserPwd);

                        strPACSRptViewURL = strPACSRptViewURL.Replace("#V2", strPACSUserID);
                        strPACSRptViewURL = strPACSRptViewURL.Replace("#V3", strPACSUserPwd);
                        #endregion
                        break;
                    case 4:
                        #region Notification Service
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            strControlCode = Convert.ToString(dr["control_code"]);
                            switch (strControlCode)
                            {
                                case "MAILSENDER":
                                    strMailSender = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "MAILSVRNAME":
                                    strMailServer = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "MAILSVRPORT":
                                    intPortNo = Convert.ToInt32(dr["data_type_number"]);
                                    break;
                                case "MAILSSLENABLED":
                                    strSSL = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "MAILSVRUSRCODE":
                                    strMailUserID = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "MAILSVRUSRPWD":
                                    if (Convert.ToString(dr["data_type_string"]).Trim() != "")
                                        strMailUserPwd = CoreCommon.DecryptString(Convert.ToString(dr["data_type_string"]).Trim());
                                    break;
                                case "SMSSENDERNO":
                                    strSenderNo = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "SMSACCTSID":
                                    strAcctSID = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "SMSAUTHTKNNO":
                                    strAuthToken = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "INVMAILFOLDER":
                                    strMailInvFolder = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "RPTSRVURL":
                                    strRptServerURL = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "RPTSRVFLDR":
                                    strRptServerFolder = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "FTPDLFLDRTMP":
                                    strFTPDLFLDRTMP = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "FAXAPIURL":
                                    strFAXAPIURL = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "FAXAUTHUSERID":
                                    strFAXAUTHUSERID = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "FAXAUTHPWD":
                                    if (Convert.ToString(dr["data_type_string"]).Trim() != "")
                                        strFAXAUTHPWD = CoreCommon.DecryptString(Convert.ToString(dr["data_type_string"]).Trim());
                                    break;
                                case "FAXCSID":
                                    strFAXCSID = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "FAXREFTEXT":
                                    strFAXREFTEXT = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "FAXREPADDR":
                                    strFAXREPADDR = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "FAXCONTACT":
                                   strFAXCONTACT = Convert.ToString(dr["data_type_string"]).Trim();
                                   break;
                                case "FAXRETRY":
                                   intFAXRETRY = Convert.ToInt32(dr["data_type_number"]);
                                   break;
                                case "FAXFILEFLDR":
                                   strFAXFILEFLDR = Convert.ToString(dr["data_type_string"]).Trim();
                                   break;
                                case "SCHCASVCENBL":
                                   strSCHCASVCENBL = Convert.ToString(dr["data_type_string"]).Trim();
                                   break;

                            }
                        }
                        #endregion
                        break;
                    case 5:
                        #region Day End Service
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            strControlCode = Convert.ToString(dr["control_code"]);
                            switch (strControlCode)
                            {

                                case "DOCDCMPATH":
                                    strDocDCMPath = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "FTPDLFLDRTMP":
                                    strFTPDLFLDRTMP = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "INVMAILFOLDER":
                                    strMailInvFolder = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "WS8SRVIP":
                                    strWS8SRVIP = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "WS8CLTIP":
                                    strWS8CLTIP = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "WS8SRVUID":
                                    strWS8SRVUID = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "WS8SRVPWD":
                                    strWS8SRVPWD = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "TEMPDCMATCHPATH":
                                    strTEMPDCMATCHPATH = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "PACSXFERDLFLDR":
                                    strPACSXFERDLFLDR = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "PACSARCHALTFLDR":
                                    strPACSARCHALTFLDR = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                            }
                        }
                        #endregion
                        break;
                    case 6:
                        #region Missing Data Synch Service
                       foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            strControlCode = Convert.ToString(dr["control_code"]).Trim();
                            switch (strControlCode)
                            {
                                case"WS8SRVIP":
                                    strWS8SRVIP = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "WS8CLTIP":
                                    strWS8CLTIP = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "WS8SRVUID":
                                    strWS8SRVUID = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "WS8SRVPWD":
                                    strWS8SRVPWD = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                            }
                        }

                        arrFields = new string[ds.Tables[2].Rows.Count];
                       
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            arrFields[idx] = Convert.ToString(dr["field_code"]);
                            idx = idx + 1;
                        }
                        #endregion
                        break;
                    case 7:
                        #region FTP & PACS Synch Service
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            strControlCode = Convert.ToString(dr["control_code"]).Trim();
                            switch (strControlCode)
                            {
                                case "FTPHOST":
                                    strFTPHOST = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "FTPPORT":
                                    intFTPPORT = Convert.ToInt32(dr["data_type_number"]);
                                    break;
                                case "FTPUSER":
                                    strFTPUSER = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "FTPPWD":
                                    strFTPPWD = Convert.ToString(dr["data_type_string"]).Trim();
                                    strFTPPWD = CoreCommon.DecryptString(strFTPPWD);
                                    break;
                                case "PDF2IMGEXEPATH":
                                    strPDFtoImgExePath = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "FTPFLDR":
                                    strFTPFLDR = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "FTPDLFLDRTMP":
                                    strFTPDLFLDRTMP = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "PACSXFERDLFLDR":
                                    strPACSXFERDLFLDR = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPATH":
                                    strXFEREXEPATH = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPATHALT":
                                    strXFEREXEPATHALT = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPARMS":
                                    strXFEREXEPARMS = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPARMSJPGLL":
                                    strXFEREXEPARMSJPGLL = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPARMJ2KLL":
                                    strXFEREXEPARMJ2KLL = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPARMJ2KLS":
                                    strXFEREXEPARMJ2KLS = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPARMSSENDDCM":
                                    strXFEREXEPARMSSENDDCM = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "PACSARCHIVEFLDR":
                                    strPACSARCHIVEFLDR = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "WRITEBACKURL":
                                    strWRITEBACKURL = Convert.ToString(dr["data_type_string"]).Trim();
                                    sb.Append(strWRITEBACKURL);
                                    foreach (DataRow dr1 in ds.Tables[2].Rows)
                                    {
                                        sb.Append("&qe_");
                                        sb.Append(Convert.ToString(dr1["field_code"]) + "=");
                                    }
                                    strWRITEBACKURL = sb.ToString();
                                    break;
                                case "IMG2DCMEXEPATH":
                                    strImgtoDCMExePath = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "DCMMODIFYEXEPATH":
                                    strDCMMODIFYEXEPATH = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "FTPLOGFLDR":
                                    strFTPLOGFLDR = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "FTPLOGDLFLDRTMP":
                                    strFTPLOGDLFLDRTMP = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "FTPDLMODE":
                                    strFTPDLMODE = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "FTPSRCFOLDER":
                                    strFTPSRCFOLDER = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "DCMRCVRFLDR":
                                    strDCMRCVRFLDR = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "FILESHOLDPATH":
                                    strFILESHOLDPATH = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "SENDLFTOPACS":
                                    strSENDLFTOPACS = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "DCMDMPEXEPATH":
                                    strDCMDMPEXEPATH = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "LFPATH":
                                    strLFPATH = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                            }
                        }
                        #endregion
                        break;
                    case 9:
                        #region Case Assignment Service
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            strControlCode = Convert.ToString(dr["control_code"]);
                            switch (strControlCode)
                            {
                                case "SCHCASVCENBL":
                                    strSCHCASVCENBL = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                            }
                        }
                        #endregion
                        break;
                    case 10:
                        #region File Distribution Service
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            strControlCode = Convert.ToString(dr["control_code"]);
                            switch (strControlCode)
                            {
                                case "DCMMODIFYEXEPATH":
                                    strDCMMODIFYEXEPATH = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "DCMDMPEXEPATH":
                                    strDCMDMPEXEPATH = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "DCMRCVEXEPATH":
                                    strDCMRCVEXEPATH = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "DCMLSNFLDR1":
                                    strDCMLSNFLDR1 = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "DCMLSNFLDR2":
                                    strDCMLSNFLDR2 = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "DCMLSNFLDR3":
                                    strDCMLSNFLDR4 = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "DCMLSNFLDR4":
                                    strDCMLSNFLDR4 = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "FILESHOLDPATH":
                                    strFILESHOLDPATH = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "RCVSYNTAX1":
                                    strRCVSYNTAX1 = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "RCVSYNTAX2":
                                    strRCVSYNTAX2 = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "RCVSYNTAX3":
                                    strRCVSYNTAX3 = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "RCVSYNTAX4":
                                    strRCVSYNTAX4 = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "ENBLLDS":
                                    strENBLLDS = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "DSNOOFPORTS":
                                    intDSNOOFPORTS = Convert.ToInt32(dr["data_type_string"]);
                                    break;
                                case "UMDCMFILES":
                                    strUMDCMFILES = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                            }
                        }
                        #endregion
                        break;
                    case 11:
                        #region Listener File Processing Service
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            strControlCode = Convert.ToString(dr["control_code"]);
                            switch (strControlCode)
                            {
                                case "DCMRCVRFLDR":
                                    strDCMRCVRFLDR = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPATH":
                                    strXFEREXEPATH = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPATHALT":
                                    strXFEREXEPATHALT = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPARMS":
                                    strXFEREXEPARMS = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPARMSJPGLL":
                                    strXFEREXEPARMSJPGLL = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPARMJ2KLL":
                                    strXFEREXEPARMJ2KLL = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPARMJ2KLS":
                                    strXFEREXEPARMJ2KLS = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "XFEREXEPARMSSENDDCM":
                                    strXFEREXEPARMSSENDDCM = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "PACSXFERDLFLDR":
                                    strPACSXFERDLFLDR = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "ENBLLSPACSXFER":
                                    strENBLLSPACSXFER = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "UMDCMFILES":
                                    strUMDCMFILES = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "REJDCMFILES":
                                    strREJDCMFILES = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                                case "DCMDMPEXEPATH":
                                    strDCMDMPEXEPATH = Convert.ToString(dr["data_type_string"]).Trim();
                                    break;
                            }
                        }
                        #endregion
                        break;
                }

                if (strWS8SRVPWD.Trim() != string.Empty)
                {
                    strWS8SRVPWD = CoreCommon.DecryptString(strWS8SRVPWD);
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
        public bool CreateServiceRestartNotification(string ConfigPath,int ServiceID,string Reason, ref string ReturnMessage, ref string CatchMessage)
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

        #region FetchInstitutionInfo
        public bool FetchInstitutionInfo(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            DataSet ds = new DataSet();

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                SqlRecordParams[0] = new SqlParameter("@institution_name", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strInstName;
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "schedule_institution_info_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        InstID = new Guid(Convert.ToString(dr["institution_id"]).Trim());
                        strInsCode = Convert.ToString(dr["institution_code"]).Trim();
                        strInstName = Convert.ToString(dr["institution_name"]).Trim();
                    }
                }
                bReturn = true;

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

        #region FetchInstitutionInfoByStudyUID
        public bool FetchInstitutionInfoByStudyUID(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            DataSet ds = new DataSet();

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                SqlRecordParams[0] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strSUID;
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "schedule_institution_info_fetch_by_study_uid", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        InstID = new Guid(Convert.ToString(dr["institution_id"]).Trim());
                        strInsCode = Convert.ToString(dr["institution_code"]).Trim();
                        strInstName = Convert.ToString(dr["institution_name"]).Trim();
                        strStudyExists = Convert.ToString(dr["study_exists"]).Trim();
                    }
                }
                bReturn = true;

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

        #region CreateNewInstitution
        public bool CreateNewInstitution(string ConfigPath, int intServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[3];
            string strReturnMsg = string.Empty;

            try
            {


                SqlRecordParams[0] = new SqlParameter("@institution_name", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strInstName;
                SqlRecordParams[1] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[1].Direction = ParameterDirection.Output;
                SqlRecordParams[2] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[2].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "schedule_institution_new_create", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[2].Value);
                if (intReturnType == 0)
                {
                    strReturnMsg = Convert.ToString(SqlRecordParams[1].Value);
                    CoreCommon.doLog(ConfigPath, intServiceID, strSvcName, "Core : CreateNewInstitution() - Error: " + strReturnMsg, true);
                }

                bReturn = true;

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region CreateNewInstitutionNotification
        public bool CreateNewInstitutionNotification(string ConfigPath, int intServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[3];
            string strReturnMsg = string.Empty;

            try
            {

                SqlRecordParams[0] = new SqlParameter("@institution_name", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strInstName;
                SqlRecordParams[1] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[1].Direction = ParameterDirection.Output;
                SqlRecordParams[2] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[2].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_notification_institution_create", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[2].Value);
                if (intReturnType == 0)
                {
                    strReturnMsg = Convert.ToString(SqlRecordParams[1].Value);
                    CoreCommon.doLog(ConfigPath, intServiceID, strSvcName, "Core : CreateNewInstitutionNotification() - Error: " + strReturnMsg, true);
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
