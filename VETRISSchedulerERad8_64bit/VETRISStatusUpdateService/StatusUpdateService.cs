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

namespace VETRISStatusUpdateService
{
    public partial class StatusUpdateService : ServiceBase
    {

        #region members & variables
        private static int intFreq = 60;
        private static string strURL = string.Empty;
        private static string strRptURL = string.Empty;
        private static string strPACSImgViewURL = string.Empty;
        private static string strPACSStudyViewURL = string.Empty;
        private static string strPACSRptViewURL = string.Empty;
        private static string strConfigPath = AppDomain.CurrentDomain.BaseDirectory;
        private static string strSvcName = "VETRIS Status Update Service";
        private static int intServiceID = 3;
        private static string strWS8SRVIP = string.Empty;
        private static string strWS8CLTIP = string.Empty;
        private static string strWS8SRVUID = string.Empty;
        private static string strWS8SRVPWD = string.Empty;
        private static string strWS8SessionID = string.Empty;
        private static string strRestartService = "N";
        string[] arrFields = new string[0];
        Scheduler objCore;
        CaseStudyUpdate objSU;
        #endregion

        public StatusUpdateService()
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
                        foreach (var process in Process.GetProcessesByName("VETRISStatusUpdateService"))
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
                //    else
                //    {
                //        //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Service stopped successfully.", false);
                //        base.OnStop();
                //    }
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


                    try
                    {

                        if (objCore.GetServiceDetails(strConfigPath, ref strCatchMessage))
                        {

                            intFreq = objCore.FREQUENCY;
                            strSvcName = objCore.SERVICE_NAME;
                            strURL = objCore.URL;
                            strRptURL = objCore.REPORT_FETCH_URL;
                            strPACSImgViewURL = objCore.PACS_IMAGE_VIEW_URL;
                            strPACSStudyViewURL = objCore.PACS_STUDY_VIEW_URL;
                            strPACSRptViewURL = objCore.PACS_REPORT_VIEW_URL;
                            strWS8SRVIP = objCore.WS8_SERVER_URL;
                            strWS8CLTIP = objCore.CLIENT_IP_URL;
                            strWS8SRVUID = objCore.WS8_USER_ID;
                            strWS8SRVPWD = objCore.WS8_PASSWORD;
                            arrFields = objCore.FIELD;
                            FetchCaseList();
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


                    objCore = null;
                    System.Threading.Thread.Sleep(intFreq * 1000);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();


                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcess() - Exception: " + expErr.Message, true);
            }
            finally
            { objCore = null; }
        }
        #endregion

        #region FetchCaseList
        private void FetchCaseList()
        {

            string strResult = string.Empty;
            string strCatchMsg = string.Empty;

            DataSet ds = new DataSet();
            Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
            string strStudyUID = string.Empty;
            string strSUURL = string.Empty;
            int intStatusID = 0;
            string strRadiologist = string.Empty;
            string strPrelimRptUpdated = string.Empty;
            string strFinalRptUpdated = string.Empty;
            string strPrelimSMSUpdated = string.Empty;
            string strFinalSMSUpdated = string.Empty;
            string strArchived = string.Empty;
            string strCatchMessage = string.Empty;
            string[] arrValues = new string[0];
            bool bStudyExists = false;
            string strStep = "";

            objSU = new CaseStudyUpdate();


            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Fetching study list...", false);
                //strStep = "1";
                if (objSU.FetchCaseList(strConfigPath, ref ds, ref strCatchMsg))
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ds.Tables[0].Rows.Count.ToString() + " record(s) fetched.", false);
                    //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Field Count " + arrFields.Length.ToString(), false);

                    strStep = "2";
                    foreach (DataRow dr in ds.Tables["StudyList"].Rows)
                    {
                        arrValues = new string[arrFields.Length];
                        //strStep = "3";
                        strSUURL = strURL;
                        Id = new Guid(Convert.ToString(dr["id"]));
                        strStudyUID = Convert.ToString(dr["study_uid"]).Trim();
                        strPrelimRptUpdated = Convert.ToString(dr["prelim_rpt_updated"]).Trim();
                        strFinalRptUpdated = Convert.ToString(dr["final_rpt_updated"]).Trim();
                        strPrelimSMSUpdated = Convert.ToString(dr["prelim_sms_updated"]).Trim();
                        strFinalSMSUpdated = Convert.ToString(dr["final_sms_updated"]).Trim();
                        strArchived = Convert.ToString(dr["archived"]).Trim();
                        bStudyExists = false;
                        strCatchMsg = string.Empty;
                        DataTable dtbl = new DataTable(); dtbl = CreateAddendumTable();
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Updating SUID " + strStudyUID, false);
                        strStep = "4";
                        if (GetStatus(strStudyUID, ref arrValues, ref bStudyExists, ref dtbl, ref strCatchMsg))
                        {
                            strStep = "5";
                            if (bStudyExists)
                            {
                                strStep = "6";
                                intStatusID = Convert.ToInt32(arrValues[0]);
                                if (strArchived == "N")
                                {
                                    #region Update Status
                                    if (intStatusID > -1)
                                    {
                                        //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Value Count " + arrValues.Length.ToString(), false);
                                        //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "SUID " + strStudyUID, true);
                                        objSU = new CaseStudyUpdate();
                                        objSU.STUDY_ID = Id;
                                        objSU.STUDY_UID = strStudyUID;
                                        objSU.STATUS_ID = intStatusID;
                                        if(arrValues[1].Trim() != string.Empty) objSU.RADIOLOGIST = arrValues[1].Trim();
                                        else objSU.RADIOLOGIST = arrValues[17].Trim();
                                        objSU.IMAGE_COUNT = Convert.ToInt32(arrValues[2]);
                                        objSU.INSTITUTION_NAME = arrValues[3].Trim();
                                        objSU.MANUFACTURER = arrValues[4].Trim();
                                        objSU.MODEL = arrValues[5].Trim();
                                        objSU.MODALITY_AE_TITLE = arrValues[6].Trim();
                                        objSU.OBJECT_COUNT = Convert.ToInt32(arrValues[7]);
                                        objSU.SERVICE_CODES = arrValues[8].Trim();
                                        objSU.MODALITY = arrValues[9].Trim();
                                        objSU.FINAL_RADIOLOGIST = arrValues[10].Trim();
                                        objSU.PRELIMINARY_RADIOLOGIST = arrValues[11].Trim();
                                        objSU.REPORT_APPROVAL_DATE = Convert.ToDateTime(arrValues[12]);
                                        objSU.REPORT_RECORDING_DATE = Convert.ToDateTime(arrValues[13]);
                                        objSU.REPORT_TEXT_HTML = arrValues[14].Trim();
                                        objSU.REPORT_TEXT = arrValues[15].Trim();
                                        objSU.DICTATION_RADIOLOGIST = arrValues[16].Trim();
                                        objSU.ADDILTIONAL_FIELD = arrFields[arrFields.Length - 1];
                                        objSU.ADDILTIONAL_FIELD_VALUE = arrValues[arrValues.Length - 1].Trim();
                                        objSU.PACS_IMAGE_VIEW_URL = strPACSImgViewURL;
                                        objSU.PACS_REPORT_VIEW_URL = strPACSRptViewURL;
                                        objSU.PACS_STUDY_VIEW_URL = strPACSStudyViewURL;


                                        strStep = "7";
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Updating status of Study UID " + strStudyUID, false);
                                        try
                                        {
                                            if (!objSU.UpdateStatus(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                                            {
                                                strStep = "8";
                                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchCaseList() - Update status for Study UID : " + strStudyUID + " - " + strCatchMessage, true);
                                            }
                                            else
                                            {
                                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Status of Study UID " + strStudyUID + " updated successfully.", false);
                                                //if ((intStatusID == 60) || (intStatusID == 80) || (intStatusID == 100))
                                                ////if (intStatusID == 60)
                                                //{
                                                //    #region Update Report
                                                //    try
                                                //    {
                                                //        strStep = "9";
                                                //UpdateReport(intStatusID, dtbl, objSU);
                                                //    }
                                                //    catch (Exception ex)
                                                //    {
                                                //        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ex.Message, false);
                                                //    }
                                                //    #endregion
                                                //}
                                                //else
                                                //{
                                                dtbl = null;
                                                //}
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ex.Message, false);
                                        }

                                        arrValues = new string[0];
                                        dtbl = null;
                                    }
                                    else
                                    {
                                        arrValues = new string[0];
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchCaseList() - Update status for Study UID : " + strStudyUID + " - Failed to fetch the status", true);
                                    }
                                    #endregion
                                }
                            }
                            else
                            {

                                #region Delete Study
                                try
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting Study UID : " + strStudyUID, false);
                                    arrValues = new string[0];
                                    dtbl = null;
                                    objSU = new CaseStudyUpdate();
                                    objSU.STUDY_UID = strStudyUID;

                                    if (!objSU.DeleteStudy(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                                    {
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchCaseList() - Delete Study UID : " + strStudyUID + " - " + strCatchMessage, true);

                                    }
                                }
                                catch (Exception ex)
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ex.Message, false);
                                }

                                #endregion
                            }
                        }
                        else
                        {
                            if (strCatchMsg.Trim() == "No matching study was found,")
                            {
                                #region Delete Study
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting Study UID : " + strStudyUID, false);
                                arrValues = new string[0];
                                dtbl = null;
                                objSU = new CaseStudyUpdate();
                                objSU.STUDY_UID = strStudyUID;

                                try
                                {
                                    if (!objSU.DeleteStudy(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                                    {
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchCaseList() - Delete Study UID : " + strStudyUID + " - " + strCatchMessage, true);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ex.Message, false);
                                }


                                #endregion
                            }
                        }
                        arrValues = new string[0];
                        dtbl = null;
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                    }



                }
                else
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchCaseList()  - Exception -1: " + strCatchMsg, true);
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchCaseList() :: SUID : " + strStudyUID + " - Exception - 2 (Step " + strStep + "): " + ex.Message, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            finally
            {
                objSU = null; ds.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }


        }
        #endregion

        #region FetchCaseList1
        //private void FetchCaseList1()
        //{

        //    string strResult = string.Empty;
        //    string strCatchMsg = string.Empty;

        //    DataSet ds = new DataSet();
        //    Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
        //    string strStudyUID = string.Empty;
        //    string strSUURL = string.Empty;
        //    int intStatusID = 0;
        //    string strRadiologist = string.Empty;
        //    string strPrelimRptUpdated = string.Empty;
        //    string strFinalRptUpdated = string.Empty;
        //    string strPrelimSMSUpdated = string.Empty;
        //    string strFinalSMSUpdated = string.Empty;
        //    string strCatchMessage = string.Empty;
        //    string[] arrValues = new string[0];
        //    bool bStudyExists = false;
        //    string strStep = "";
        //    objSU = new CaseStudyUpdate();


        //    try
        //    {
        //        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Fetching study list...", false);
        //        //strStep = "1";
        //        if (objSU.FetchCaseList(strConfigPath, ref ds, ref strCatchMsg))
        //        {
        //            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ds.Tables[0].Rows.Count.ToString() + " record(s) fetched.", false);
        //            //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Field Count " + arrFields.Length.ToString(), false);

        //            //strStep = "2";
        //            foreach (DataRow dr in ds.Tables["StudyList"].Rows)
        //            {
        //                arrValues = new string[arrFields.Length];
        //                //strStep = "3";
        //                strSUURL = strURL;
        //                Id = new Guid(Convert.ToString(dr["id"]));
        //                strStudyUID = Convert.ToString(dr["study_uid"]).Trim();
        //                strPrelimRptUpdated = Convert.ToString(dr["prelim_rpt_updated"]).Trim();
        //                strFinalRptUpdated = Convert.ToString(dr["final_rpt_updated"]).Trim();
        //                strPrelimSMSUpdated = Convert.ToString(dr["prelim_sms_updated"]).Trim();
        //                strFinalSMSUpdated = Convert.ToString(dr["final_sms_updated"]).Trim();
        //                bStudyExists = false;
        //                strCatchMsg = string.Empty;

        //                //strStep = "4";
        //                if (GetStatus(strStudyUID, ref arrValues, ref bStudyExists, ref strCatchMsg))
        //                {
        //                    //strStep = "5";
        //                    if (bStudyExists)
        //                    {
        //                        //strStep = "6";
        //                        intStatusID = Convert.ToInt32(arrValues[0]);

        //                        #region Update Status
        //                        if (intStatusID > -1)
        //                        {
        //                            //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Value Count " + arrValues.Length.ToString(), false);
        //                            objSU = new CaseStudyUpdate();
        //                            objSU.STUDY_ID = Id;
        //                            objSU.STUDY_UID = strStudyUID;
        //                            objSU.STATUS_ID = intStatusID;
        //                            objSU.RADIOLOGIST = arrValues[1].Trim();
        //                            objSU.IMAGE_COUNT = Convert.ToInt32(arrValues[2]);
        //                            objSU.INSTITUTION_NAME = arrValues[3].Trim();
        //                            objSU.MANUFACTURER = arrValues[4].Trim();
        //                            objSU.MODEL = arrValues[5].Trim();
        //                            objSU.MODALITY_AE_TITLE = arrValues[6].Trim();
        //                            objSU.OBJECT_COUNT = Convert.ToInt32(arrValues[7]);
        //                            objSU.SERVICE_CODES = arrValues[8].Trim();
        //                            objSU.MODALITY = arrValues[9].Trim();
        //                            objSU.FINAL_RADIOLOGIST = arrValues[10].Trim();
        //                            objSU.PRELIMINARY_RADIOLOGIST = arrValues[11].Trim();
        //                            objSU.REPORT_APPROVAL_DATE = Convert.ToDateTime(arrValues[12]);
        //                            objSU.REPORT_RECORDING_DATE = Convert.ToDateTime(arrValues[13]);
        //                            //objSU.REPORT_TEXT_HTML = arrValues[14].Trim();
        //                            //objSU.REPORT_TEXT = arrValues[15].Trim();
        //                            objSU.REPORT_TEXT_HTML = string.Empty;
        //                            objSU.REPORT_TEXT = arrValues[14].Trim();
        //                            objSU.PACS_IMAGE_VIEW_URL = strPACSImgViewURL;
        //                            objSU.PACS_REPORT_VIEW_URL = strPACSRptViewURL;
        //                            objSU.PACS_STUDY_VIEW_URL = strPACSStudyViewURL;
        //                            objSU.ACCN_NO = arrValues[15];
        //                            objSU.PATIENT_ID = arrValues[16];
        //                            objSU.PATIENT_NAME = arrValues[17].Trim();
        //                            objSU.SEX = arrValues[18];
        //                            objSU.DOB = Convert.ToDateTime(arrValues[19]);
        //                            objSU.AGE = arrValues[20];
        //                            objSU.PHYSICIAN = arrValues[21].Trim();
        //                            objSU.SPECIES = arrValues[22].Trim();
        //                            objSU.BREED = arrValues[23].Trim();

        //                            strStep = "7";
        //                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Updating status of Study UID " + strStudyUID, false);
        //                            if (!objSU.UpdateStatus1(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
        //                            {
        //                                strStep = "8";
        //                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchCaseList() - Update status for Study UID : " + strStudyUID + " - " + strCatchMessage, true);

        //                            }
        //                            else
        //                            {
        //                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Status of Study UID " + strStudyUID + " updated successfully.", false);
        //                                if ((intStatusID == 80) || (intStatusID == 100))
        //                                {
        //                                    strStep = "9";
        //                                    UpdateReport(intStatusID, strPrelimRptUpdated, strFinalRptUpdated, strPrelimSMSUpdated, strFinalSMSUpdated, objSU);
        //                                }
        //                            }

        //                            arrValues = new string[0];
        //                        }
        //                        else
        //                        {
        //                            arrValues = new string[0];
        //                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchCaseList() - Update status for Study UID : " + strStudyUID + " - Failed to fetch the status", true);
        //                        }
        //                        #endregion
        //                    }
        //                    else
        //                    {

        //                        #region Delete Study
        //                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting Study UID : " + strStudyUID, false);
        //                        arrValues = new string[0];
        //                        objSU = new CaseStudyUpdate();
        //                        objSU.STUDY_UID = strStudyUID;

        //                        if (!objSU.DeleteStudy(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
        //                        {
        //                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchCaseList() - Delete Study UID : " + strStudyUID + " - " + strCatchMessage, true);

        //                        }
        //                        #endregion
        //                    }
        //                }
        //                else
        //                {
        //                    if (strCatchMsg.Trim() == "No matching study was found,")
        //                    {
        //                        #region Delete Study
        //                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting Study UID : " + strStudyUID, false);
        //                        arrValues = new string[0];
        //                        objSU = new CaseStudyUpdate();
        //                        objSU.STUDY_UID = strStudyUID;
        //                        strStep = "10";
        //                        if (!objSU.DeleteStudy(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
        //                        {
        //                            strStep = "11";
        //                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchCaseList() - Delete Study UID : " + strStudyUID + " - " + strCatchMessage, true);

        //                        }
        //                        #endregion
        //                    }
        //                }
        //                arrValues = new string[0];
        //                GC.Collect();
        //                GC.WaitForPendingFinalizers();
        //                GC.Collect();
        //            }
        //        }
        //        else
        //        {
        //            GC.Collect();
        //            GC.WaitForPendingFinalizers();
        //            GC.Collect();
        //            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchCaseList()  - Exception -1: " + strCatchMsg, true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchCaseList() - Exception - 2 (Step " + strStep + "): " + ex.Message, true);
        //        EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //        GC.Collect();
        //    }
        //    finally
        //    {
        //        objSU = null; ds.Dispose();
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //        GC.Collect();
        //    }


        //}
        #endregion

        //#region GetStatus
        //private bool GetStatus(string strSUURL, ref int intStatusID, ref string[] arrValues)
        //{
        //    WebClient client = new WebClient();
        //    string strResult = string.Empty;
        //    string[] arrData = new string[0];
        //    //string[] arrData = new string[0];
        //    string strField = string.Empty;
        //    string[] arrRecSep = { "\n" };
        //    string[] arrFldSep = { "\t" };
        //    bool bRet = false;

        //    intStatusID = -1;

        //    try
        //    {
        //        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Connecting eRAD Server...", false);
        //        IgnoreBadCertificates();
        //        byte[] data = client.DownloadData(strSUURL);
        //        strResult = System.Text.Encoding.Default.GetString(data);
        //        strResult = strResult.Replace("### Begin_Table's_Content ###", "");
        //        strResult = strResult.Replace("### End_Table's_Content ###", "");

        //        if (strResult.IndexOf("#USERID:") > 0)
        //        {
        //            strResult = strResult.Substring(1, strResult.IndexOf("#USERID:") - 1);
        //            strResult = strResult.Replace("\r", "");
        //            strResult = strResult.Trim();
        //        }
        //        else 
        //            strResult = "";

        //        if (strResult != string.Empty)
        //        {
        //            arrData = strResult.Split(arrFldSep, StringSplitOptions.None);
        //            arrValues = new string[arrData.Length];


        //            arrValues[0] = arrData[0];//status
        //            intStatusID = Convert.ToInt32(arrValues[0]);

        //            arrValues[1] = arrData[1].Replace("^", " ").Trim();//radiologist
        //            if (arrValues[1] == "___NULL___") arrValues[1] = "";

        //            arrValues[2] = arrData[2];//Image Count
        //            if (arrValues[2] == "___NULL___") arrValues[2] = "0";

        //            arrValues[3] = arrData[3].Replace("^", " ").Trim();//Institution name
        //            if (arrValues[3] == "___NULL___") arrValues[3] = "";

        //            arrValues[4] = arrData[4].Replace("^", " ").Trim();//Manufacturer name
        //            if (arrValues[4] == "___NULL___") arrValues[4] = "";

        //            arrValues[5] = arrData[5].Replace("^", " ").Trim();//Manufacturer Model
        //            if (arrValues[5] == "___NULL___") arrValues[5] = "";

        //            arrValues[6] = arrData[6].Replace("^", " ").Trim();//Modality AE Title
        //            if (arrValues[6] == "___NULL___") arrValues[6] = "";

        //            arrValues[7] = arrData[7];//object Count
        //            if (arrValues[7] == "___NULL___") arrValues[7] = "0";

        //            arrValues[8] = arrData[8].Replace("^", " ").Trim();//Services:UDF3
        //            if (arrValues[8] == "___NULL___") arrValues[8] = "";

        //            arrValues[9] = arrData[9].Replace("^", " ").Trim();//modality
        //            if (arrValues[9] == "___NULL___") arrValues[9] = "";
        //        }


        //        bRet = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        bRet = false;
        //        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetStatus() - Exception: " + ex.Message + "- arrData.length " + arrData.Length, true);
        //        EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);

        //    }

        //    return bRet;  
        //}
        //#endregion

        #region GetStatus
        private bool GetStatus(string StudyUID, ref string[] arrValues, ref bool StudyExists, ref DataTable dtbl, ref string CatchMessage)
        {
            RadWebClass client = new RadWebClass();
            string strResult = string.Empty;
            string sCatchMsg = string.Empty;
            string sError = string.Empty;
            string strColID = string.Empty;
            string strValue = string.Empty;
            int intRecordID = 0;
            bool bRet = false;


            try
            {

                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Connecting eRAD Server...server IP : " + strWS8SRVIP + " Session : " + strWS8SessionID, false);
                //if (strWS8SessionID.Trim() == string.Empty) bRet = client.GetSession(strWS8CLTIP, strWS8SRVIP, strWS8SRVUID,strWS8SRVPWD, ref strWS8SessionID, ref sCatchMsg, ref sError);
                //else bRet = true;

                bRet = true;

                if (bRet)
                {

                    bRet = client.GetStudyData(strWS8SessionID, strWS8SRVIP, StudyUID, ref strResult, ref sCatchMsg, ref sError);

                    if (bRet)
                    {

                        DataSet ds = new DataSet();
                        strResult = strResult.Trim();
                        System.IO.StringReader xmlSR = new System.IO.StringReader(strResult);
                        ds.ReadXml(xmlSR);

                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Getting status for Study UID : " + StudyUID, false);

                        if (ds.Tables.Contains("Field"))
                        {
                            StudyExists = true;
                            //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Field Count : " + arrFields.Length.ToString(), false);
                            //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Value Count : " + arrValues.Length.ToString(), false);

                            #region get status and fields
                            for (int i = 0; i < arrFields.Length; i++)
                            {
                                arrValues[i] = string.Empty;
                                strColID = arrFields[i];
                                //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Column ID : " + strColID + " , Index : " + i.ToString(), false);
                                if (strColID.Trim() != string.Empty)
                                {
                                    DataView dv = new DataView(ds.Tables["Field"]);
                                    dv.RowFilter = "Colid ='" + strColID + "'";


                                    switch (strColID)
                                    {
                                        #region NIMG,NOBJ
                                        case "NIMG":
                                        case "NOBJ":
                                            if (dv.ToTable().Rows.Count > 0)
                                            {
                                                if (dv.ToTable().Rows[0]["Value"] != DBNull.Value)
                                                {
                                                    if (IsInteger(Convert.ToString(dv.ToTable().Rows[0]["Value"])))
                                                        arrValues[i] = Convert.ToString(dv.ToTable().Rows[0]["Value"]);
                                                    else
                                                        arrValues[i] = "0";
                                                }
                                                else
                                                    arrValues[i] = "0";
                                            }
                                            else
                                                arrValues[i] = "0";
                                            break;
                                        #endregion

                                        #region IADT,IRDT
                                        case "IADT":
                                        case "IRDT":
                                            //case "PDOB":
                                            if (dv.ToTable().Rows.Count > 0)
                                            {
                                                if (dv.ToTable().Rows[0]["Value"] != DBNull.Value)
                                                {
                                                    if (IsDate(Convert.ToString(dv.ToTable().Rows[0]["Value"])))
                                                        arrValues[i] = Convert.ToString(dv.ToTable().Rows[0]["Value"]);
                                                    else
                                                        arrValues[i] = "01jan1900";
                                                }
                                                else if ((dv.ToTable().Rows[0]["Value"] == DBNull.Value) || (Convert.ToString(dv.ToTable().Rows[0]["Value"]).Trim() == string.Empty))
                                                    arrValues[i] = "01jan1900";
                                            }
                                            else
                                                arrValues[i] = "01jan1900";
                                            break;
                                        #endregion

                                        #region SRFH
                                        case "SRFH ":
                                            if (dv.ToTable().Rows.Count > 0)
                                            {
                                                foreach (DataRow dr in dv.ToTable().Rows)
                                                {
                                                    #region reverse
                                                    intRecordID = Convert.ToInt32(dr["Record_Id"]);
                                                    if (intRecordID == dv.ToTable().Rows.Count - 1)
                                                    {
                                                        if (dv.ToTable().Rows[0]["Value"] != DBNull.Value)
                                                            arrValues[i] = Convert.ToString(dv.ToTable().Rows[0]["Value"]).Replace("^", " ").Trim();
                                                        else
                                                            arrValues[i] = string.Empty;
                                                    }
                                                    else
                                                    {
                                                        DataRow[] foundRow = dtbl.Select("srl_no =" + (intRecordID + 1).ToString());
                                                        if (foundRow.Length == 0)
                                                        {
                                                            DataRow drAddn = dtbl.NewRow();
                                                            drAddn["srl_no"] = intRecordID + 1;
                                                            if (dr["Value"] != DBNull.Value)
                                                                drAddn["addendum_text_html"] = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                                            else
                                                                drAddn["addendum_text_html"] = string.Empty;
                                                            dtbl.Rows.Add(drAddn);
                                                        }
                                                        else
                                                        {
                                                            if (dr["Value"] != DBNull.Value)
                                                                foundRow[0]["addendum_text_html"] = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                                            else
                                                                foundRow[0]["addendum_text_html"] = string.Empty;
                                                        }
                                                        dtbl.AcceptChanges();
                                                    }
                                                    #endregion

                                                    #region normal
                                                    //intRecordID = Convert.ToInt32(dr["Record_Id"]);
                                                    //if (intRecordID == 0)
                                                    //{
                                                    //    if (dv.ToTable().Rows[0]["Value"] != DBNull.Value)
                                                    //        arrValues[i] = Convert.ToString(dv.ToTable().Rows[0]["Value"]).Replace("^", " ").Trim();
                                                    //    else
                                                    //        arrValues[i] = string.Empty;
                                                    //}
                                                    //else
                                                    //{
                                                    //    DataRow[] foundRow = dtbl.Select("srl_no =" + intRecordID.ToString());
                                                    //    if (foundRow.Length == 0)
                                                    //    {
                                                    //        DataRow drAddn = dtbl.NewRow();
                                                    //        drAddn["srl_no"] = intRecordID;
                                                    //        if (dr["Value"] != DBNull.Value)
                                                    //            drAddn["addendum_text_html"] = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                                    //        else
                                                    //            drAddn["addendum_text_html"] = string.Empty;
                                                    //        dtbl.Rows.Add(drAddn);
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        if (dr["Value"] != DBNull.Value)
                                                    //            foundRow[0]["addendum_text_html"] = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                                    //        else
                                                    //            foundRow[0]["addendum_text_html"] = string.Empty;
                                                    //    }
                                                    //    dtbl.AcceptChanges();
                                                    //}
                                                    #endregion

                                                }
                                            }
                                            else
                                                arrValues[i] = string.Empty;

                                            #region Suspended
                                            //if (dv.ToTable().Rows.Count > 0)
                                            //{
                                            //    foreach (DataRow dr in dv.ToTable().Rows)
                                            //    {
                                            //        intRecordID = Convert.ToInt32(dr["Record_Id"]);
                                            //        if (intRecordID == 0)
                                            //        {
                                            //            if (dr["Value"] != DBNull.Value)
                                            //                arrValues[i] = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                            //            else
                                            //                arrValues[i] = string.Empty;
                                            //        }
                                            //        else if (intRecordID > 0)
                                            //        {
                                            //            DataRow[] foundRow = dtbl.Select("srl_no =" + intRecordID.ToString());
                                            //            if (foundRow.Length == 0)
                                            //            {
                                            //                DataRow drAddn = dtbl.NewRow();
                                            //                drAddn["srl_no"] = intRecordID;
                                            //                if (dr["Value"] != DBNull.Value)
                                            //                    drAddn["addendum_text_html"] = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                            //                else
                                            //                    drAddn["addendum_text_html"] = string.Empty;
                                            //                dtbl.Rows.Add(drAddn);
                                            //            }
                                            //            else
                                            //            {
                                            //                if (dr["Value"] != DBNull.Value)
                                            //                    foundRow[0]["addendum_text_html"] = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                            //                else
                                            //                    foundRow[0]["addendum_text_html"] = string.Empty;
                                            //            }
                                            //            dtbl.AcceptChanges();
                                            //        }
                                            //    }
                                            //}
                                            //else
                                            //    arrValues[i] = string.Empty;
                                            #endregion
                                            break;
                                        #endregion

                                        #region SRFT
                                        case "SRFT":
                                            if (dv.ToTable().Rows.Count > 0)
                                            {
                                                foreach (DataRow dr in dv.ToTable().Rows)
                                                {
                                                    intRecordID = Convert.ToInt32(dr["Record_Id"]);

                                                    #region Reverse
                                                    if (intRecordID == dv.ToTable().Rows.Count - 1)
                                                    {
                                                        if (dv.ToTable().Rows[0]["Value"] != DBNull.Value)
                                                            arrValues[i] = Convert.ToString(dv.ToTable().Rows[0]["Value"]).Replace("^", " ").Trim();
                                                        else
                                                            arrValues[i] = string.Empty;
                                                    }
                                                    else
                                                    {
                                                        DataRow[] foundRow = dtbl.Select("srl_no =" + (intRecordID + 1).ToString());
                                                        if (foundRow.Length == 0)
                                                        {
                                                            DataRow drAddn = dtbl.NewRow();
                                                            drAddn["srl_no"] = intRecordID + 1;
                                                            if (dr["Value"] != DBNull.Value)
                                                                drAddn["addendum_text"] = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                                            else
                                                                drAddn["addendum_text"] = string.Empty;
                                                            dtbl.Rows.Add(drAddn);
                                                        }
                                                        else
                                                        {
                                                            if (dr["Value"] != DBNull.Value)
                                                                foundRow[0]["addendum_text"] = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                                            else
                                                                foundRow[0]["addendum_text"] = string.Empty;
                                                        }
                                                        dtbl.AcceptChanges();
                                                    }
                                                    #endregion

                                                    #region Normal
                                                    //if (intRecordID == 0)
                                                    //{
                                                    //    if (dv.ToTable().Rows[0]["Value"] != DBNull.Value)
                                                    //        arrValues[i] = Convert.ToString(dv.ToTable().Rows[0]["Value"]).Replace("^", " ").Trim();
                                                    //    else
                                                    //        arrValues[i] = string.Empty;
                                                    //}
                                                    //else
                                                    //{
                                                    //    DataRow[] foundRow = dtbl.Select("srl_no =" + (intRecordID + 1).ToString());
                                                    //    if (foundRow.Length == 0)
                                                    //    {
                                                    //        DataRow drAddn = dtbl.NewRow();
                                                    //        drAddn["srl_no"] = intRecordID;
                                                    //        if (dr["Value"] != DBNull.Value)
                                                    //            drAddn["addendum_text"] = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                                    //        else
                                                    //            drAddn["addendum_text"] = string.Empty;
                                                    //        dtbl.Rows.Add(drAddn);
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        if (dr["Value"] != DBNull.Value)
                                                    //            foundRow[0]["addendum_text"] = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                                    //        else
                                                    //            foundRow[0]["addendum_text"] = string.Empty;
                                                    //    }
                                                    //    dtbl.AcceptChanges();
                                                    //}
                                                    #endregion

                                                }



                                                #region Suspended
                                                //foreach (DataRow dr in dv.ToTable().Rows)
                                                //{
                                                //    intRecordID = Convert.ToInt32(dr["Record_Id"]);
                                                //    if (intRecordID == 0)
                                                //    {
                                                //        if (dr["Value"] != DBNull.Value)
                                                //            arrValues[i] = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                                //        else
                                                //            arrValues[i] = string.Empty;
                                                //    }
                                                //    else if (intRecordID > 0)
                                                //    {
                                                //        DataRow[] foundRow = dtbl.Select("srl_no =" + intRecordID.ToString());
                                                //        if (foundRow.Length == 0)
                                                //        {
                                                //            DataRow drAddn = dtbl.NewRow();
                                                //            drAddn["srl_no"] = intRecordID;
                                                //            if (dr["Value"] != DBNull.Value)
                                                //                drAddn["addendum_text"] = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                                //            else
                                                //                drAddn["addendum_text"] = string.Empty;
                                                //            dtbl.Rows.Add(drAddn);
                                                //        }
                                                //        else
                                                //        {
                                                //            if (dr["Value"] != DBNull.Value)
                                                //                foundRow[0]["addendum_text"] = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                                //            else
                                                //                foundRow[0]["addendum_text"] = string.Empty;
                                                //        }
                                                //        dtbl.AcceptChanges();
                                                //    }
                                                //}
                                                #endregion
                                            }
                                            else
                                                arrValues[i] = string.Empty;
                                            break;
                                        #endregion

                                        #region default
                                        default:
                                            if (dv.ToTable().Rows.Count > 0)
                                            {
                                                if (dv.ToTable().Rows[0]["Value"] != DBNull.Value)
                                                    arrValues[i] = Convert.ToString(dv.ToTable().Rows[0]["Value"]).Replace("^", " ").Trim();
                                                else
                                                    arrValues[i] = string.Empty;
                                            }
                                            else
                                                arrValues[i] = string.Empty;
                                            break;
                                        #endregion
                                    }
                                    dv.Dispose();
                                }
                                else
                                    arrValues[i] = string.Empty;

                            }
                            #endregion

                        }
                        else
                            StudyExists = false;


                        xmlSR = null;
                        ds.Dispose();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        GC.Collect();
                    }
                    else
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetStatus() - Study UID : " + StudyUID + " Error: " + sCatchMsg + "[" + sError + "]", true);
                        CatchMessage = sError.Trim();
                    }
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetSession() - Study UID : " + StudyUID + " Error: " + sCatchMsg + "[" + sError + "]", true);
                    CatchMessage = sError.Trim();

                }

            }
            catch (Exception ex)
            {
                bRet = false;
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetStatus() - Study UID : " + StudyUID + " Exception: " + ex.Message, true);
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

                client = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            return bRet;
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

        #region UpdateReport
        private void UpdateReport(int StatusID, DataTable dtbl, CaseStudyUpdate objSU)
        {
            string strCatchMessage = string.Empty;
            string strUpdate = string.Empty;

            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Getting report for study uid " + objSU.STUDY_UID, false);

                if (!objSU.UpdateReport(strConfigPath, intServiceID, strSvcName, dtbl, ref strCatchMessage))
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateReport() - Update Report for Study UID : " + objSU.STUDY_UID + " - " + strCatchMessage, true);
                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateReport() - Exception: " + ex.Message, true);

            }
        }
        #endregion

        #region CreateAddendumTable
        private DataTable CreateAddendumTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("srl_no", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("addendum_text", System.Type.GetType("System.String"));
            dtbl.Columns.Add("addendum_text_html", System.Type.GetType("System.String"));
            dtbl.TableName = "Addendum";
            return dtbl;
        }
        #endregion
    }
}
