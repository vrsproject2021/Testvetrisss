using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Configuration;
using System.Globalization;
using System.IO;
namespace VETRISRouter.Core
{
    public class Scheduler
    {
        #region Variables

        DateTime dtFrom = DateTime.Now;
        DateTime dtTo = DateTime.Now;
        

        int intServiceID = 0;
        
        #region Scheduler Settings
        string strACCESSORYDIR = string.Empty;
        string strDBNAME = string.Empty;
        string strEXEPATH = string.Empty;
        string strRCVEXEOPTIONS = string.Empty;
        string strPACSSRVRNAME = string.Empty;
        string strSNDEXEOPTIONS = string.Empty;
        string strRCVDIR = string.Empty;
        string strRCVDIRMANUAL = string.Empty;
        string strMANUALUPLDAUTO = string.Empty;
        string strIMGMNLUPLDAUTO = string.Empty;
        string strRCVIMGDIR = string.Empty;
        string strSNDDIR = string.Empty;
        string strARCHDIR = string.Empty;

        string strRCVAETITLE = string.Empty;
        string strRCVPORTNO = string.Empty;
        string strADMINPWD = string.Empty;
        string strVETLOGIN = string.Empty;
        string strVETURL = string.Empty;
        string strVETLOGINURL = string.Empty;
        string strVETAPIURL = string.Empty;

        string strTRANSFERFTP = string.Empty;
        string strFTPHOST = string.Empty;
        string strFTPPORT = string.Empty;
        string strFTPUSER = string.Empty;
        string strFTPPWD = string.Empty;
        string strFTPTEMPFLDR = string.Empty;
        string strFTPDWLDFLDR = string.Empty;
        string strFTPLOGDWLDFLDR = string.Empty;
        string strDRSDWLFLDR = string.Empty;

        string strINSTNAME = string.Empty;
        string strINSTADDR1 = string.Empty;
        string strINSTADDR2 = string.Empty;
        string strINSTZIP = string.Empty;
        string strSITECODE = string.Empty;
        string strCOMPXFERFILE = string.Empty;
        string strARCHFILE = string.Empty;

        string strFTPSENDMODE = "U";
        string strFTPABSPATH = string.Empty;

        #endregion

        #region Log
        string strLogType = "";
        string strServiceName = string.Empty;
        #endregion

        #endregion

        #region Constructor
        public Scheduler()
        {
            strACCESSORYDIR = CoreCommon.ACCESSORY_DIR;
            strDBNAME = CoreCommon.DB_NAME;
            strEXEPATH = CoreCommon.EXE_PATH;
            strRCVEXEOPTIONS = CoreCommon.RCVEXE_OPTIONS;

        }
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

        #region Scheduler Settings
        public string ACCESSORY_DIR
        {
            get { return strACCESSORYDIR; }
            set { strACCESSORYDIR = value; }
        }
        public string DB_NAME
        {
            get { return strDBNAME; }
            set { strDBNAME = value; }
        }
        public string EXE_PATH
        {
            get { return strEXEPATH; }
            set { strEXEPATH = value; }
        }

        public string RECEIVER_EXE_OPTIONS
        {
            get { return strRCVEXEOPTIONS; }
            set { strRCVEXEOPTIONS = value; }
        }
        public string PACS_SERVER_NAME
        {
            get { return strPACSSRVRNAME; }
            set { strPACSSRVRNAME = value; }
        }
        public string SENDER_OPTIONS
        {
            get { return strSNDEXEOPTIONS; }
            set { strSNDEXEOPTIONS = value; }
        }
        public string RECEIVING_DIRECTORY
        {
            get { return strRCVDIR; }
            set { strRCVDIR = value; }
        }
        public string SENDER_DIRECTORY
        {
            get { return strSNDDIR; }
            set { strSNDDIR = value; }
        }
        public string RECEIVING_DIRECTORY_FOR_MANUAL_UPLOAD
        {
            get { return strRCVDIRMANUAL; }
            set { strRCVDIRMANUAL = value; }
        }
        public string RECEIVING_DIRECTORY_AUTO_DETECT
        {
            get { return strMANUALUPLDAUTO; }
            set { strMANUALUPLDAUTO = value; }
        }
        public string RECEIVING_DIRECTORY_FOR_IMAGES
        {
            get { return strRCVIMGDIR; }
            set { strRCVIMGDIR = value; }
        }
        public string RECEIVING_DIRECTORY_FOR_IMAGES_AUTO_DETECT
        {
            get { return strIMGMNLUPLDAUTO; }
            set { strIMGMNLUPLDAUTO = value; }
        }
        public string ARCHIVE_DIRECTORY
        {
            get { return strARCHDIR; }
            set { strARCHDIR = value; }
        }

        public string RECEIVER_AETITLE
        {
            get { return strRCVAETITLE; }
            set { strRCVAETITLE = value; }
        }
        public string RECEIVER_PORT_NO
        {
            get { return strRCVPORTNO; }
            set { strRCVPORTNO = value; }
        }
        public string ADMIN_PASSWORD
        {
            get { return strADMINPWD; }
            set { strADMINPWD = value; }
        }
        public string VETRIS_LOGIN_ID
        {
            get { return strVETLOGIN; }
            set { strVETLOGIN = value; }
        }
        public string VETRIS_URL
        {
            get { return strVETURL; }
            set { strVETURL = value; }
        }
        public string VETRIS_API_URL
        {
            get { return strVETAPIURL; }
            set { strVETAPIURL = value; }
        }
        public string VETRIS_LOGIN_URL
        {
            get { return strVETLOGINURL; }
            set { strVETLOGINURL = value; }
        }

        public string FILE_TRANSFER_VIA_FTP
        {
            get { return strTRANSFERFTP; }
            set { strTRANSFERFTP = value; }
        }
        public string FTP_HOST_NAME
        {
            get { return strFTPHOST; }
            set { strFTPHOST = value; }
        }
        public string FTP_PORT_NUMBER
        {
            get { return strFTPPORT; }
            set { strFTPPORT = value; }
        }
        public string FTP_USER_NAME
        {
            get { return strFTPUSER; }
            set { strFTPUSER = value; }
        }
        public string FTP_PASSWORD
        {
            get { return strFTPPWD; }
            set { strFTPPWD = value; }
        }
        public string FTP_TEMPORARY_FOLDER
        {
            get { return strFTPTEMPFLDR; }
            set { strADMINPWD = value; }
        }
        public string FTP_DOWNLOAD_FOLDER
        {
            get { return strFTPDWLDFLDR; }
            set { strFTPDWLDFLDR = value; }
        }
        public string FTP_LOG_DOWNLOAD_FOLDER
        {
            get { return strFTPLOGDWLDFLDR; }
            set { strFTPLOGDWLDFLDR = value; }
        }
        public string FTP_DICOM_ROUTER_DOWNLOAD_FOLDER
        {
            get { return strDRSDWLFLDR; }
            set { strDRSDWLFLDR = value; }
        }
        public string FTP_SENDING_MODE
        {
            get { return strFTPSENDMODE; }
            set { strFTPSENDMODE = value; }
        }
        public string FTP_ABSOLUTE_PATH
        {
            get { return strFTPABSPATH; }
            set { strFTPABSPATH = value; }
        }

        public string INSTITUTION_NAME
        {
            get { return strINSTNAME; }
            set { strINSTNAME = value; }
        }
        public string INSTITUTION_ADDRESS_1
        {
            get { return strINSTADDR1; }
            set { strINSTADDR1 = value; }
        }
        public string INSTITUTION_ADDRESS_2
        {
            get { return strINSTADDR2; }
            set { strINSTADDR2 = value; }
        }
        public string INSTITUTION_ZIP
        {
            get { return strINSTZIP; }
            set { strINSTZIP = value; }
        }
        public string SITE_CODE
        {
            get { return strSITECODE; }
            set { strSITECODE = value; }
        }
        public string COMPRESS_FILES_TO_TRANSFER
        {
            get { return strCOMPXFERFILE; }
            set { strCOMPXFERFILE = value; }
        }
        public string ARCHIVE_FILES_TRANSFERED
        {
            get { return strARCHFILE; }
            set { strARCHFILE = value; }
        }
        #endregion

        #region Log
        public string LOG_TYPE
        {
            get { return strLogType; }
            set { strLogType = value; }
        }
        public string SERVICE_NAME
        {
            get { return strServiceName; }
            set { strServiceName = value; }
        }

        #endregion

        #endregion

        #region SaveDcmLog
        public bool SaveDcmLog(string ConfigPath, string FileName, string StudyUID, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; string ReturnMessage = string.Empty;
            if (!(ConfigPath.Contains("AccesoryDirs") && ConfigPath.Contains(CoreCommon.DB_NAME)))
            {
                ConfigPath = CoreCommon.ACCESSORY_DIR + CoreCommon.DB_NAME;
            }
            if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
            OleDbConnection con = new OleDbConnection(CoreCommon.CONNECTION_STRING);

            try
            {
                string sqlQuery = "INSERT INTO dcm_files_log (`FileName`,`StudyUID`) values (@FileName,@StudyUID)";
                using (OleDbCommand cmd = new OleDbCommand(sqlQuery, con))
                {

                    con.Open();
                    cmd.Parameters.AddWithValue("@FileName", FileName);
                    cmd.Parameters.AddWithValue("@StudyUID", StudyUID);

                    intExecReturn = cmd.ExecuteNonQuery();
                    con.Close();

                    if (intExecReturn != 0) bReturn = true;
                    else bReturn = false;

                }
            }
            catch (Exception ex)
            {
                CatchMessage = ex.Message;
                bReturn = false;
            }
            finally
            {
                con.Close();
            }
            return bReturn;
        }
        #endregion

        #region SaveSchedulerSettings
        public bool SaveSchedulerSettings(string ConfigPath, string ControlCode, string ControlValue, ref string CatchMessage)
        {
            bool bReturn = false;
            DataSet ds = new DataSet();
            string strControlCode = string.Empty;
            int idx = 0;

            

            if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);

            try
            {
              

                ds.ReadXml(CoreCommon.CONNECTION_STRING);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    strControlCode = Convert.ToString(dr["control_code"]);
                    if (strControlCode == ControlCode)
                    {
                        ds.Tables[0].Rows[idx]["control_value"] = ControlValue;
                        ds.WriteXml(CoreCommon.CONNECTION_STRING);
                        bReturn = true;
                        break;
                    }
                    idx = idx + 1;
                }

            }
            catch (Exception ex)
            {
                CatchMessage = ex.Message;
                bReturn = false;
            }
            finally
            {
                ds.Dispose();
            }
            return bReturn;
        }
        #endregion

        #region SaveDevices
        public bool SaveDevices(string ConfigPath, DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
          
            if (CoreCommon.DEVICE_CONNECTION_STRING == string.Empty) CoreCommon.GetDeviceConnectionString(ConfigPath);

            try
            {
                ds.WriteXml(CoreCommon.DEVICE_CONNECTION_STRING);
                bReturn = true;
            }
            catch (Exception ex)
            {
                CatchMessage = ex.Message;
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
            DataSet ds = new DataSet();
            DataSet dsClone = new DataSet();
            DataTable dtbl = new DataTable();
            DataTable dtblLog = new DataTable();
            DataView dv;
            string strFileName = "DRLog.xml";

            

            try
            {
                if (File.Exists(ConfigPath + "\\" + strFileName))
                {

                    ds.ReadXml(ConfigPath + "\\" + strFileName);

                    if (ds.Tables.Count > 0)
                    {
                        dsClone = ds.Clone();
                        dsClone.Tables[0].Columns["service_id"].DataType = typeof(Int32);
                        dsClone.Tables[0].Columns["log_date"].DataType = typeof(DateTime);

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            DataRow newRow = dsClone.Tables[0].NewRow();
                            newRow["service_id"] = Convert.ToInt32(dr["service_id"]);
                            newRow["service_name"] = dr["service_name"];
                            if (Convert.ToString(dr["is_error"]) == "N")
                                newRow["is_error"] = "Information";
                            else
                                newRow["is_error"] = "Error";
                            newRow["log_date"] = Convert.ToDateTime(dr["log_date"]);
                            newRow["log_message"] = dr["log_message"];
                            newRow["sent_to_vetris"] = dr["sent_to_vetris"];
                            dsClone.Tables[0].Rows.Add(newRow);

                        }


                        dtblLog = dsClone.Tables[0];
                        dtblLog.TableName = "Logs";
                        dv = new DataView(dtblLog);

                        dv.RowFilter = "log_date>=#" + dtFrom + "# and log_date<=#" + dtTo + "#";

                        if (strLogType != "A")
                        {
                            if (strLogType == "I")
                            {
                                strLogType = "N";
                            }
                            else if (strLogType == "E")
                            {
                                strLogType = "Y";
                            }

                            dv.RowFilter = "is_error = " + strLogType;
                        }

                        if (strServiceName.Trim() != "")
                        {
                            dv.RowFilter = "service_name ='" + strServiceName.Trim() + "'";
                        }

                        dv.Sort = "log_date desc";
                        CreateLogTable(ref dtbl);

                        foreach (DataRow dr in dv.ToTable().Rows)
                        {
                            DataRow dr1 = dtbl.NewRow();
                            dr1["is_error"] = dr["is_error"];
                            dr1["service_name"] = dr["service_name"];
                            dr1["log_date"] = dr["log_date"];
                            dr1["log_message"] = dr["log_message"];
                            dtbl.Rows.Add(dr1);
                        }

                        dv.Dispose();
                    }
                    else
                        CreateLogTable(ref dtbl);
                }
                else
                    CreateLogTable(ref dtbl);

            }
            catch (Exception ex)
            {
                CatchMessage = ex.Message;
                return null;
            }
            finally
            {
                dtblLog.Dispose();
                ds.Dispose();
                dsClone.Dispose();
            }
            return dtbl;
        }
        #endregion

        #region PurgeLog
        public bool PurgeLog(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; string strQuery = string.Empty;
            DataSet ds = new DataSet();
            DataSet dsClone = new DataSet();
            DataTable dtblLog = new DataTable();
            DataRow[] drr;

            string strFileName = "DRLog.xml";

            

            try
            {
                ds.ReadXml(ConfigPath + "\\" + strFileName);
                dsClone = ds.Clone();
                dsClone.Tables[0].Columns["service_id"].DataType = typeof(Int32);
                dsClone.Tables[0].Columns["log_date"].DataType = typeof(DateTime);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    DataRow newRow = dsClone.Tables[0].NewRow();
                    newRow["service_id"] = Convert.ToInt32(dr["service_id"]);
                    newRow["service_name"] = dr["service_name"];
                    newRow["is_error"] = dr["is_error"];
                    newRow["log_date"] = Convert.ToDateTime(dr["log_date"]);
                    newRow["log_message"] = dr["log_message"];
                    newRow["sent_to_vetris"] = dr["sent_to_vetris"];
                    dsClone.Tables[0].Rows.Add(newRow);

                }


                dtblLog = dsClone.Tables[0];
                dtblLog.TableName = "Logs";

                strQuery = "log_date>=#" + dtFrom + "# and log_date<=#" + dtTo + "#";
                if (strLogType != "A")
                {
                    if (strLogType == "I")
                    {
                        strLogType = "N";
                    }
                    else if (strLogType == "E")
                    {
                        strLogType = "Y";
                    }

                    strQuery = strQuery + " and is_error = '" + strLogType + "'";
                }
                if (strServiceName.Trim() != "")
                {
                    strQuery = strQuery + " and service_name ='" + strServiceName.Trim() + "'";
                }

                drr = dtblLog.Select(strQuery);

                for (int i = 0; i < drr.Length; i++) drr[i].Delete();
                dtblLog.AcceptChanges();
                dtblLog.WriteXml(ConfigPath + "\\" + strFileName);
                bReturn = true;
            }
            catch (Exception ex)
            { CatchMessage = ex.Message; bReturn = false; }
            finally
            { dtblLog.Dispose(); ds.Dispose(); dsClone.Dispose(); }

            return bReturn;
        }
        #endregion

        #region CreateLogTable
        public static void CreateLogTable(ref DataTable dtbl)
        {
            dtbl.Columns.Add("is_error", System.Type.GetType("System.String"));
            dtbl.Columns.Add("service_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("log_date", System.Type.GetType("System.DateTime"));
            dtbl.Columns.Add("log_message", System.Type.GetType("System.String"));

        }
        #endregion

        #region FetchSchedulerSettings
        public bool FetchSchedulerSettings(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = false;
            string strControlCode = string.Empty;
            string[] arrSettings = new string[0];
            string strSettings = string.Empty;
            string strConnPath = string.Empty;
            DataSet ds = new DataSet();

            strConnPath = ConfigPath;
            if(CoreCommon.CONNECTION_STRING== string.Empty) CoreCommon.GetConnectionString(strConnPath);

            try
            {
                
                ds.ReadXml(CoreCommon.CONNECTION_STRING);

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    strControlCode = Convert.ToString(dr1["control_code"]);

                    switch (strControlCode)
                    {
                        case "ACCESSORYDIR":
                            strACCESSORYDIR = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "DBNAME":
                            strDBNAME = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "EXEPATH":
                            strEXEPATH = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "RCVEXEOPTIONS":
                            strRCVEXEOPTIONS = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "RCVDIR":
                            strRCVDIR = Convert.ToString(dr1["control_value"]);
                            break;
                        case "SNDDIR":
                            strSNDDIR = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "ARCHDIR":
                            strARCHDIR = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "RCVDIRMANUAL":
                            strRCVDIRMANUAL = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "RCVIMGDIR":
                            strRCVIMGDIR = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "RCVAETITLE":
                            strRCVAETITLE = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "RCVPORTNO":
                            strRCVPORTNO = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "ADMINPWD":
                            strADMINPWD = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "INSTNAME":
                            strINSTNAME = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "INSTADDR1":
                            strINSTADDR1 = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "INSTADDR2":
                            strINSTADDR2 = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "INSTZIP":
                            strINSTZIP = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "SITECODE":
                            strSITECODE = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "COMPXFERFILE":
                            strCOMPXFERFILE = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "ARCHFILE":
                            strARCHFILE = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "TRANSFERFTP":
                            strTRANSFERFTP = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "MANUALUPLDAUTO":
                            strMANUALUPLDAUTO = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "IMGMNLUPLDAUTO":
                            strIMGMNLUPLDAUTO = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "VETLOGIN":
                            strVETLOGIN = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "VETURL":
                            strVETURL = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "VETLOGINURL":
                            strVETLOGINURL = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "VETAPIURL":
                            strVETAPIURL = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "FTPSENDMODE":
                            strFTPSENDMODE = Convert.ToString(dr1["control_value"]).Trim();
                            break;
                        case "FTPABSPATH":
                            strFTPABSPATH = Convert.ToString(dr1["control_value"]).Trim();
                            break;

                    }
                }


                

                if (CoreCommon.SETTINGS_STRING == string.Empty) CoreCommon.GetSettingsString(ConfigPath);
                strSettings = CoreCommon.SETTINGS_STRING;
                arrSettings = strSettings.Split('±');

                strPACSSRVRNAME = arrSettings[0].Trim();
                strSNDEXEOPTIONS = arrSettings[1].Trim();
                strFTPHOST = arrSettings[2].Trim();
                strFTPPORT = arrSettings[3].Trim();
                strFTPUSER = arrSettings[4].Trim();
                strFTPPWD = arrSettings[5].Trim();
                strFTPDWLDFLDR = arrSettings[6].Trim();
                strFTPLOGDWLDFLDR = arrSettings[7].Trim();
                strDRSDWLFLDR = arrSettings[8].Trim();

                bReturn = true;

            }
            catch (Exception expErr)
            {
                bReturn = false; CatchMessage = expErr.Message;
            }

            finally
            { //con.Close(); 
                ds.Dispose();
            }
            return bReturn;
        }
        #endregion

        #region FetchLogToUpload
        public DataTable FetchLogToUpload(string ConfigPath, ref string CatchMessage)
        {
            DataSet ds = new DataSet();
            DataTable dtbl = new DataTable();
            DataTable dtblLog = new DataTable();
            DataView dv;
            string strFileName = "DRLog.xml";

            

            try
            {
                if (File.Exists(ConfigPath + "\\" + strFileName))
                {
                    ds.ReadXml(ConfigPath + "\\" + strFileName);
                    dtblLog = ds.Tables[0];
                    dtblLog.TableName = "Logs";

                    dv = new DataView(dtblLog);
                    dv.RowFilter = "sent_to_vetris='N'";

                    dtbl = dv.ToTable();
                    dv.Dispose();

                    foreach (DataRow row in dtblLog.Rows)
                    {
                        if (row["sent_to_vetris"].ToString() == "N")
                            row.SetField("sent_to_vetris", "Y");
                    }
                    dtblLog.AcceptChanges();
                    dtblLog.WriteXml(ConfigPath + "\\" + strFileName);
                }
                else
                    CoreCommon.CreateLogTable(ref dtbl);
            }
            catch (Exception ex)
            {
                CatchMessage = ex.Message;
                EventLog.WriteEntry("FetchLogToUpload() :: Exception : ", ex.Message, EventLogEntryType.Error);
                return null;
            }
            finally
            {
                dtblLog.Dispose();
                ds.Dispose();
            }
            return dtbl;
        }
        #endregion

        #region FetchDeviceList
        public bool FetchDeviceList(string ConfigPath,ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            string strConnPath = string.Empty;
            strConnPath = ConfigPath;
            if (CoreCommon.DEVICE_CONNECTION_STRING == string.Empty)  CoreCommon.GetDeviceConnectionString(strConnPath);

            try
            {

                if (File.Exists(CoreCommon.DEVICE_CONNECTION_STRING)) ds.ReadXml(CoreCommon.DEVICE_CONNECTION_STRING);
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
    }
}
