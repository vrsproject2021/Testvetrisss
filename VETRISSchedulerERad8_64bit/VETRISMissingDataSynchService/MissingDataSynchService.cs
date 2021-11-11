using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Configuration;
using System.Security;
using VETRISScheduler.Core;
using eRADCls;

namespace VETRISMissingDataSynchService
{
    public partial class MissingDataSynchService : ServiceBase
    {
        #region members & variables
        private static int intFreq = 10;
        private static string strURL = string.Empty;
        private static string strWS8SRVIP = string.Empty;
        private static string strWS8CLTIP = string.Empty;
        private static string strWS8SRVUID = string.Empty;
        private static string strWS8SRVPWD = string.Empty;
        private static string strWS8SessionID = string.Empty;
        private static string strAppliedURL = string.Empty;
        private static string strConfigPath = AppDomain.CurrentDomain.BaseDirectory;
        private static string strSvcName = "VETRIS Missing Data Synch Service";
        private static int intServiceID = 6;
        private static string strRestartService = "N";
        string[] arrFields = new string[0];
        Scheduler objCore;
        #endregion

        public MissingDataSynchService()
        {
            InitializeComponent();
        }

        #region OnStart
        protected override void OnStart(string[] args)
        {

            try
            {

                System.Threading.ThreadStart job_data_synch = new System.Threading.ThreadStart(doProcess);
                System.Threading.Thread thread = new System.Threading.Thread(job_data_synch);
                thread.Start();


            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Error starting Service. " + ex.Message, true);
                EventLog.WriteEntry(strSvcName, "Error Starting Service." + ex.Message, EventLogEntryType.Warning);
            }

        }
        #endregion

        #region OnStop
        protected override void OnStop()
        {
            string strRetMessage = string.Empty;
            string strCatchMessage = string.Empty;

            try
            {
                //System.Threading.Thread.Sleep(20000);
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Service stopped successfully.", false);
                base.OnStop();

                if (strRestartService == "Y")
                {
                    try
                    {
                        foreach (var process in Process.GetProcessesByName("VETRISNewDataSynchService"))
                        {
                            process.Kill();
                        }

                        System.Threading.ThreadStart job_data_synch = new System.Threading.ThreadStart(doProcess);
                        System.Threading.Thread thread = new System.Threading.Thread(job_data_synch);
                        thread.Start();
                        strRestartService = "N";
                    }
                    catch (Exception ex)
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Error starting Service. " + ex.Message, true);
                        EventLog.WriteEntry(strSvcName, "Error Starting Service." + ex.Message, EventLogEntryType.Warning);

                        objCore = new Scheduler();
                        if (!objCore.CreateServiceRestartNotification(strConfigPath, intServiceID, ex.Message.Trim(), ref strRetMessage, ref strCatchMessage))
                        {

                            if (strCatchMessage.Trim() != string.Empty)
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "OnStop()=>Core:CreateServiceRestartNotification():: Exception: " + strCatchMessage, true);
                            else
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "OnStop()=>Core:CreateServiceRestartNotification():: Error : " + strRetMessage, true);
                        }
                        objCore = null;

                    }
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Error stopping Service. " + ex.Message, true);
            }
        }
        #endregion

        #region doProcess
        private void doProcess()
        {
            string strCatchMessage = string.Empty;

            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Service started Successfully", false);
                while (true)
                {
                    objCore = new Scheduler();
                    objCore.SERVICE_ID = intServiceID;

                    #region Service Settings
                    try
                    {
                        if (objCore.GetServiceDetails(strConfigPath, ref strCatchMessage))
                        {

                            intFreq      = objCore.FREQUENCY;
                            strSvcName   = objCore.SERVICE_NAME;
                            strWS8SRVIP  = objCore.WS8_SERVER_URL;
                            strWS8CLTIP  = objCore.CLIENT_IP_URL;
                            strWS8SRVUID = objCore.WS8_USER_ID;
                            strWS8SRVPWD = objCore.WS8_PASSWORD;
                            arrFields    = objCore.FIELD;
                        }
                        else
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Core::GetServiceDetails - Error : " + strCatchMessage, true);

                    }
                    catch (Exception ex)
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcess() - Error: " + ex.Message, true);
                        EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Warning);
                        System.Threading.Thread.Sleep(intFreq * 1000);
                    }
                    #endregion

                    GetData();

                    objCore = null;
                    System.Threading.Thread.Sleep(intFreq * 1000);



                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcess() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }
            finally
            { objCore = null; }
        }
        #endregion

        #region GetData
        private void GetData()
        {
            RadWebClass client = new RadWebClass();
            string strResult = string.Empty;
            string sCatchMsg = string.Empty;
            string sError = string.Empty;
            bool bRet = false;

            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Connecting eRAD Server...server IP : " + strWS8SRVIP + " Session : " + strWS8SessionID, false);
                //if (strWS8SessionID.Trim() == string.Empty) bRet = client.GetSession(strWS8CLTIP, strWS8SRVIP, strWS8SRVUID, strWS8SRVPWD ,ref strWS8SessionID, ref sCatchMsg, ref sError);
                //else bRet = true;

                bRet = true;

                if (bRet)
                {
                    #region Status : UNVIEWED (Suspended)
                    //bRet = client.MissingStudy0(strWS8SessionID, strWS8SRVIP, arrFields, ref strResult, ref sCatchMsg, ref sError);
                    //if (bRet)
                    //{
                    //    strResult = strResult.Trim();
                    //    PopulateData(strResult);
                    //}
                    //else
                    //{
                    //    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "MissingStudy0() - Error: " + sCatchMsg + "[" + sError + "]", true);
                    //}
                    #endregion

                    #region Status : READ
                    bRet = client.MissingStudy50(strWS8SessionID, strWS8SRVIP, arrFields, ref strResult, ref sCatchMsg, ref sError);
                    if (bRet)
                    {
                        strResult = strResult.Trim();
                        PopulateData(strResult);
                    }
                    else
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "MissingStudy50() - Error: " + sCatchMsg + "[" + sError + "]", true);
                    }
                    #endregion

                    #region Status : DICTATED
                    bRet = client.MissingStudy60(strWS8SessionID, strWS8SRVIP, arrFields, ref strResult, ref sCatchMsg, ref sError);
                    if (bRet)
                    {
                        strResult = strResult.Trim();
                        PopulateData(strResult);
                    }
                    else
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "MissingStudy60() - Error: " + sCatchMsg + "[" + sError + "]", true);
                    }
                    #endregion

                    #region Status : VIEWED
                    bRet = client.MissingStudy20(strWS8SessionID, strWS8SRVIP, arrFields, ref strResult, ref sCatchMsg, ref sError);
                    if (bRet)
                    {
                        strResult = strResult.Trim();
                        PopulateData(strResult);
                    }
                    else
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "MissingStudy20() - Error: " + sCatchMsg + "[" + sError + "]", true);
                    }
                    #endregion

                    #region Status : Preliminary (Suspended)
                    //bRet = client.MissingStudy80(strWS8SessionID, strWS8SRVIP, arrFields, ref strResult, ref sCatchMsg, ref sError);
                    //if (bRet)
                    //{
                    //    strResult = strResult.Trim();
                    //    PopulateData(strResult);
                    //}
                    //else
                    //{
                    //    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "MissingStudy80() - Error: " + sCatchMsg + "[" + sError + "]", true);
                    //}
                    #endregion

                    #region Status : Final (Suspended)
                    //bRet = client.MissingStudy100(strWS8SessionID, strWS8SRVIP, arrFields, ref strResult, ref sCatchMsg, ref sError);
                    //if (bRet)
                    //{
                    //    strResult = strResult.Trim();
                    //    PopulateData(strResult);
                    //}
                    //else
                    //{
                    //    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "MissingStudy100() - Error: " + sCatchMsg + "[" + sError + "]", true);
                    //}
                    #endregion

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetSession() - Error: " + sCatchMsg + "[" + sError + "]", true);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetData() - Exception: " + ex.Message, true);
                if (ex.Message.Contains("System.OutOfMemoryException"))
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    strRestartService = "Y";
                    System.Threading.Thread.Sleep(intFreq * 1000);
                    objCore = null;
                    objCore = new Scheduler();
                    if (!objCore.CreateServiceRestartNotification(strConfigPath, intServiceID, ex.Message.Trim(), ref sError, ref sCatchMsg))
                    {

                        if (sCatchMsg.Trim() != string.Empty)
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "OnStop()=>Core:CreateServiceRestartNotification():: Exception: " + sCatchMsg, true);
                        else
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "OnStop()=>Core:CreateServiceRestartNotification():: Error : " + sError, true);
                    }
                    objCore = null;
                    OnStop();
                }
            }
            finally
            {
                objCore = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }


        }
        #endregion

        #region PopulateData
        private void PopulateData(string strResult)
        {
            DataSet ds = new DataSet();
            DataTable dtbl = new DataTable();
            string strColID = string.Empty;
            int intRecID = 0;
            string strDt = string.Empty;
            string strTblDate = string.Empty;

            try
            {
                System.IO.StringReader xmlSR = new System.IO.StringReader(strResult);
                ds.ReadXml(xmlSR);

                dtbl = CreateTable();
                if (ds.Tables.Count == 5)
                {
                    #region Populate Study

                    for (int i = 0; i < ds.Tables["Field"].Rows.Count; i += dtbl.Columns.Count)
                    {
                        DataRow dr = dtbl.NewRow();
                        intRecID = Convert.ToInt32(ds.Tables["Field"].Rows[i]["Record_Id"]);
                        DataView dv = new DataView(ds.Tables["Field"]);
                        dv.RowFilter = "Record_Id=" + Convert.ToString(intRecID);


                        #region Data Manupulation
                        foreach (DataRow drRec in dv.ToTable().Rows)
                        {
                            strColID = Convert.ToString(drRec["Colid"]).Trim();
                            switch (strColID)
                            {
                                case "SYUI":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["study_uid"] = Convert.ToString(drRec["Value"]).Trim();
                                    else
                                        dr["study_uid"] = string.Empty;
                                    break;
                                case "SYDT":
                                    if (drRec["Value"] != DBNull.Value)
                                    {
                                        if (IsDate(Convert.ToString(drRec["Value"])))
                                            dr["study_date"] = Convert.ToDateTime(drRec["Value"]);
                                        else
                                            dr["study_date"] = Convert.ToDateTime("01jan1900");
                                    }
                                    else if ((drRec["Value"] == DBNull.Value) || (Convert.ToString(drRec["Value"]).Trim() == string.Empty))
                                        dr["study_date"] = Convert.ToDateTime("01jan1900");
                                    break;
                                case "RCVD":
                                     if (drRec["Value"] != DBNull.Value)
                                    {
                                        if (IsDate(Convert.ToString(drRec["Value"])))
                                            dr["received_date"] = Convert.ToDateTime(drRec["Value"]);
                                        else
                                            dr["received_date"] = DateTime.Now;
                                    }
                                    else if ((drRec["Value"] == DBNull.Value) || (Convert.ToString(drRec["Value"]).Trim() == string.Empty))
                                        dr["received_date"] = DateTime.Now;
                                    break;
                                case "ACCN":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["accession_no"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["accession_no"] = string.Empty;
                                    break;
                                case "PAID":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["patient_id"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["patient_id"] = string.Empty;
                                    break;
                                case "PANM":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["patient_name"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["patient_name"] = string.Empty;
                                    break;
                                case "PDOB":
                                    if (drRec["Value"] != DBNull.Value)
                                    {
                                        strDt = Convert.ToString(drRec["Value"]).Trim();
                                        if (strDt == "00000000_000000") strTblDate = "01jan1900";
                                        else if (strDt == "") strTblDate = "01jan1900";
                                        else strTblDate = strDt.Substring(0, 4) + "-" + strDt.Substring(4, 2) + "-" + strDt.Substring(6, 2);
                                        if (IsDate(strTblDate)) dr["patient_dob"] = Convert.ToDateTime(strTblDate);
                                    }
                                    else
                                        dr["patient_dob"] = Convert.ToDateTime("01jan1900");
                                    break;
                                case "PAGE":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["patient_age"] = Convert.ToString(drRec["Value"]).Trim();
                                    else
                                        dr["patient_age"] = "0";
                                    break;
                                case "PSEX":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["patient_sex"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["patient_sex"] = string.Empty;
                                    break;
                                case "9PWT":
                                    if (drRec["Value"] != DBNull.Value)
                                    {
                                        if (IsDecimal(Convert.ToString(drRec["Value"])))
                                            dr["patient_weight_lbs"] = Convert.ToDecimal(drRec["Value"]);
                                        else
                                            dr["patient_weight_lbs"] = 0;
                                    }
                                    else if ((drRec["Value"] == DBNull.Value) || (Convert.ToString(drRec["Value"]).Trim() == string.Empty))
                                        dr["patient_weight_lbs"] = 0;
                                    break;
                                case "9SPC":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["species"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["species"] = string.Empty;
                                    break;
                                case "9BRD":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["breed"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["breed"] = string.Empty;
                                    break;
                                case "9RSP":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["owner"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["owner"] = string.Empty;
                                    break;
                                case "PALL":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["modality"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["modality"] = string.Empty;
                                    break;
                                case "BDYP":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["body_part"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["body_part"] = string.Empty;
                                    break;
                                case "PMAL":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["reason"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["reason"] = string.Empty;
                                    break;
                                case "INSN":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["institution_name"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["institution_name"] = string.Empty;
                                    break;
                                case "PHRF":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["referring_physician"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["referring_physician"] = string.Empty;
                                    break;
                                case "MFCT":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["manufacturer_name"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["manufacturer_name"] = string.Empty;
                                    break;
                                case "MFMD":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["manufacturer_model_no"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["manufacturer_model_no"] = string.Empty;
                                    break;
                                case "STNM":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["device_serial_no"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["device_serial_no"] = string.Empty;
                                    break;
                                case "9PSN":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["spayed_neutered"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["spayed_neutered"] = string.Empty;
                                    break;
                                case "NIMG":
                                    if (drRec["Value"] != DBNull.Value)
                                    {
                                        if (IsInteger(Convert.ToString(drRec["Value"])))
                                            dr["img_count"] = Convert.ToInt32(drRec["Value"]);
                                        else
                                            dr["img_count"] = 0;
                                    }
                                    else
                                        dr["img_count"] = 0;
                                    break;
                                case "PSAE":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["modality_ae_title"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["modality_ae_title"] = string.Empty;
                                    break;
                                case "PRST":
                                    if (drRec["Value"] != DBNull.Value)
                                    {
                                        if (IsInteger(Convert.ToString(drRec["Value"])))
                                            dr["priority_id"] = Convert.ToInt32(drRec["Value"]);
                                        else
                                            dr["priority_id"] = 0;

                                    }
                                    else if ((drRec["Value"] == DBNull.Value) || (Convert.ToString(drRec["Value"]).Trim() == string.Empty))
                                        dr["priority_id"] = 0;
                                    break;
                                case "TRAD":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["radiologist_name"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["radiologist_name"] = string.Empty;
                                    break;
                                case "STAT":
                                    if (drRec["Value"] != DBNull.Value)
                                    {
                                        if (IsInteger(Convert.ToString(drRec["Value"])))
                                            dr["study_status_pacs"] = Convert.ToInt32(drRec["Value"]);
                                        else
                                            dr["study_status_pacs"] = 0;

                                    }
                                    else if ((drRec["Value"] == DBNull.Value) || (Convert.ToString(drRec["Value"]).Trim() == string.Empty))
                                        dr["study_status_pacs"] = 0;
                                    break;
                                case "UDF1":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["sales_person"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["sales_person"] = string.Empty;
                                    break;
                                case "UDF8":
                                    if (IsDecimal(Convert.ToString(drRec["Value"])))
                                        dr["patient_weight_kgs"] = Convert.ToDecimal(drRec["Value"]);
                                    else
                                        dr["patient_weight_kgs"] = 0;
                                    break;
                                case "DSCR":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["study_type_name_1"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["study_type_name_1"] = string.Empty;
                                    break;
                                case "UDF4":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["study_type_name_2"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["study_type_name_2"] = string.Empty;
                                    break;
                                case "UDF7":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["study_type_name_3"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["study_type_name_1"] = string.Empty;
                                    break;
                                case "UDF9":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["study_type_name_4"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["study_type_name_1"] = string.Empty;
                                    break;
                                case "NOBJ":
                                    if (drRec["Value"] != DBNull.Value)
                                    {
                                        if (Convert.ToInt32(drRec["Value"]) > 0)
                                        {
                                            if (IsInteger(Convert.ToString(drRec["Value"])))
                                                dr["object_count"] = Convert.ToInt32(drRec["Value"]) - 1;
                                            else
                                                dr["object_count"] = 0;
                                        }
                                        else if ((drRec["Value"] == DBNull.Value) || (Convert.ToString(drRec["Value"]).Trim() == string.Empty))
                                        {
                                            dr["object_count"] = 0;
                                        }
                                    }
                                    else
                                        dr["object_count"] = 0;
                                    break;
                                case "UDF3":
                                    if (drRec["Value"] != DBNull.Value)
                                        dr["service_codes"] = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                    else
                                        dr["service_codes"] = string.Empty;
                                    break;
                                case "9VCD":
                                    if (drRec["Value"] != DBNull.Value)
                                    {
                                        if (IsDate(Convert.ToString(drRec["Value"])))
                                            dr["submit_on"] = Convert.ToDateTime(drRec["Value"]);
                                        else
                                            dr["submit_on"] = Convert.ToDateTime("01jan1900");
                                    }
                                    else if ((drRec["Value"] == DBNull.Value) || (Convert.ToString(drRec["Value"]).Trim() == string.Empty))
                                        dr["submit_on"] = Convert.ToDateTime("01jan1900");
                                    break;
                            }
                        }
                        #endregion

                        dtbl.Rows.Add(dr);
                    }
                    #endregion

                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, dtbl.Rows.Count.ToString() + " missing records downloaded", false);
                }
                else
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "0 missing records downloaded", false);


                if (dtbl.Rows.Count > 0) SynchData(dtbl);

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "PopulateData() - Exception: " + ex.Message, true);

            }
            finally
            {
                objCore = null;
                ds.Dispose();
            }


        }
        #endregion

        #region CreateTable
        private DataTable CreateTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("study_uid", System.Type.GetType("System.String"));
            dtbl.Columns.Add("study_date", System.Type.GetType("System.DateTime"));
            dtbl.Columns.Add("received_date", System.Type.GetType("System.DateTime"));
            dtbl.Columns.Add("accession_no", System.Type.GetType("System.String"));
            dtbl.Columns.Add("reason", System.Type.GetType("System.String"));
            dtbl.Columns.Add("institution_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("manufacturer_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("manufacturer_model_no", System.Type.GetType("System.String"));
            dtbl.Columns.Add("device_serial_no", System.Type.GetType("System.String"));
            dtbl.Columns.Add("modality_ae_title", System.Type.GetType("System.String"));
            dtbl.Columns.Add("referring_physician", System.Type.GetType("System.String"));
            dtbl.Columns.Add("patient_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("patient_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("patient_sex", System.Type.GetType("System.String"));
            dtbl.Columns.Add("patient_dob", System.Type.GetType("System.DateTime"));
            dtbl.Columns.Add("patient_age", System.Type.GetType("System.String"));
            dtbl.Columns.Add("patient_weight_lbs", System.Type.GetType("System.Decimal"));
            dtbl.Columns.Add("modality", System.Type.GetType("System.String"));
            dtbl.Columns.Add("body_part", System.Type.GetType("System.String"));
            dtbl.Columns.Add("species", System.Type.GetType("System.String"));
            dtbl.Columns.Add("breed", System.Type.GetType("System.String"));
            dtbl.Columns.Add("owner", System.Type.GetType("System.String"));
            dtbl.Columns.Add("spayed_neutered", System.Type.GetType("System.String"));
            dtbl.Columns.Add("img_count", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("priority_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("radiologist_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("sales_person", System.Type.GetType("System.String"));
            dtbl.Columns.Add("patient_weight_kgs", System.Type.GetType("System.Decimal"));
            dtbl.Columns.Add("study_status_pacs", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("study_type_name_1", System.Type.GetType("System.String"));
            dtbl.Columns.Add("study_type_name_2", System.Type.GetType("System.String"));
            dtbl.Columns.Add("study_type_name_3", System.Type.GetType("System.String"));
            dtbl.Columns.Add("study_type_name_4", System.Type.GetType("System.String"));
            dtbl.Columns.Add("object_count", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("physician_note", System.Type.GetType("System.String"));
            dtbl.Columns.Add("service_codes", System.Type.GetType("System.String"));
            dtbl.Columns.Add("submit_on", System.Type.GetType("System.DateTime"));
            dtbl.TableName = "Details";
            return dtbl;
        }
        #endregion

        #region IsDate
        protected bool IsDate(String date)
        {
            DateTime Temp;
            if (DateTime.TryParse(date, out Temp) == true)
                return true;
            else
                return false;
        }
        #endregion

        #region IsDecimal
        protected bool IsDecimal(String decimalValue)
        {
            Decimal Temp;
            if (Decimal.TryParse(decimalValue, out Temp) == true)
                return true;
            else
                return false;
        }
        #endregion

        #region IsInteger
        protected bool IsInteger(String integerValue)
        {
            Decimal Temp;
            if (Decimal.TryParse(integerValue, out Temp) == true)
                return true;
            else
                return false;
        }
        #endregion

        #region SynchData
        private void SynchData(DataTable dtbl)
        {
            MissingStudySynch objCore = new MissingStudySynch();
            string strCatchMsg = string.Empty;
            string strReturnMsg = string.Empty;
            int intCount = 0;
            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Started synching data...", false);

                if (objCore.SaveMissingSynchedData(strConfigPath, strSvcName, intServiceID, dtbl, ref intCount, ref strReturnMsg, ref strCatchMsg))
                {
                    if (intCount > 0) CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, intCount.ToString() + " record(s) synched successfully", false);
                }
                else
                {
                    if (strCatchMsg.Trim() != "")
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "SynchData() - Exception: " + strCatchMsg.Trim(), true);
                    }
                    else
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "SynchData() - Error: " + strReturnMsg.Trim(), true);
                    }
                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "SynchData() - Exception: " + ex.Message, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }
            finally
            {
                objCore = null;

            }


        }
        #endregion
    }
}
