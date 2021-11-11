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
using System.Text.RegularExpressions;
using VETRISScheduler.Core;
using eRADCls;

namespace VETRISDataWriteBackService
{
    public partial class DataWriteBackService : ServiceBase
    {
        #region members & variables
        private static int intFreq = 10;
        private static string strURL = string.Empty;
        private static string strXferExePath = string.Empty;
        private static string strXferExeParams = string.Empty;
        private static string strImgtoDCMExePath = string.Empty;
        private static string strPdftoImgExePath = string.Empty;
        private static string strDocDCMPath = string.Empty;
        private static string strUSRUPDURL = string.Empty;
        private static string strWS8SRVIP = string.Empty;
        private static string strWS8CLTIP = string.Empty;
        private static string strWS8SRVUID = string.Empty;
        private static string strWS8SRVPWD = string.Empty;
        private static string strWS8SessionID = string.Empty;

        private static string strXferExeParamsJPGLL = string.Empty;
        private static string strXFEREXEPARMJ2KLL = string.Empty;
        private static string strXFEREXEPARMJ2KLS = string.Empty;
        private static string strXferExeParamsSENDDCM = string.Empty;
        private static string strXFEREXEPATHALT = string.Empty;
        private static string strTEMPDCMATCHPATH = string.Empty;
        private static string strPACSARCHIVEFLDR = string.Empty;

        string[] arrFields = new string[0];
        private static int intServiceID = 2;

        private static string strConfigPath = AppDomain.CurrentDomain.BaseDirectory;
        private static string strSvcName = "VETRIS Write Back Service";

        Scheduler objCore;
        DataWriteBack objWB;

        #endregion

        public DataWriteBackService()
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
            try
            {
                //System.Threading.Thread.Sleep(20000);
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Service stopped successfully.", false);
                base.OnStop();
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
                            //strURL = objCore.URL;
                            strXferExePath = objCore.PACS_TRANSFER_EXE_PATH;
                            strXferExeParams = objCore.PACS_TRANSFER_EXE_PARAMS;
                            strImgtoDCMExePath = objCore.IMAGE_TO_DCM_EXE_PATH;
                            strPdftoImgExePath = objCore.PDF_TO_IMAGE_EXE_PATH;
                            strDocDCMPath = objCore.DOCUMENT_AND_DCM_PATH;
                            strUSRUPDURL = objCore.USER_UPDATE_URL;
                            strWS8SRVIP = objCore.WS8_SERVER_URL;
                            strWS8CLTIP = objCore.CLIENT_IP_URL;
                            strWS8SRVUID = objCore.WS8_USER_ID;
                            strWS8SRVPWD = objCore.WS8_PASSWORD;

                            strXferExeParamsJPGLL = objCore.PACS_TRANSFER_EXE_PARAMS_FOR_JPG_LOSSLESS;
                            strXFEREXEPARMJ2KLL = objCore.PACS_TRANSFER_EXE_PARAMS_FOR_JPG2K_LOSSLESS;
                            strXFEREXEPARMJ2KLS = objCore.PACS_TRANSFER_EXE_PARAMS_FOR_JPG2K_LOSSY;
                            strXferExeParamsSENDDCM = objCore.PACS_TRANSFER_EXE_PARAMS_FOR_SEND_DCM;
                            strXFEREXEPATHALT = objCore.PACS_TRANSFER_EXE_ALTERNATE_PATH;
                            strTEMPDCMATCHPATH = objCore.TEMPORARY_DCM_ATTCHMENT_FILE_PATH;
                            strPACSARCHIVEFLDR = objCore.PACS_ARCHIVE_FOLDER;

                            try
                            {
                                doWriteBack();
                            }
                            catch (Exception ex)
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcess()::doWriteBack() - Exception : " + ex.Message, true);
                            }

                            try
                            {
                                doWriteBackReports();
                            }
                            catch (Exception ex)
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcess()::doWriteBackReports() - Exception : " + ex.Message, true);
                            }

                            //try
                            //{
                            //    doUpdateUsersInPACS();
                            //}
                            //catch (Exception ex)
                            //{
                            //    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcess()::doUpdateUsersInPACS() - Exception : " + ex.Message, true);
                            //}

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

        #region doWriteBack
        private void doWriteBack()
        {

            string strResult = string.Empty;
            string strCatchMsg = string.Empty;

            DataSet ds = new DataSet();
            Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
            string strStudyUID = string.Empty;
            string strInstName = string.Empty;
            string strInstCode = string.Empty;
            string strWBURL = string.Empty;
            string strCatchMessage = string.Empty;
            bool bRet = false;
            int idx = 0;
            string[] arrStudyFields = new string[4];
            string strField = string.Empty;
            string strStudyType = string.Empty;
            string strFound = string.Empty;
            string strReason = string.Empty;
            string strPhysNote = string.Empty;
            StringBuilder sb = new StringBuilder();
            RadWebClass client = new RadWebClass();
            string sCatchMsg = string.Empty;
            string sError = string.Empty;
            string strRetMsg= string.Empty;
            int intStatusID = 0;

            objWB = new DataWriteBack();


            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Fetching write back list...", false);
                if (objWB.FetchWriteBackList(strConfigPath, ref ds, ref strCatchMsg))
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ds.Tables["Details"].Rows.Count.ToString() + " record(s) fetched.", false);
                    strWBURL = strURL;

                    if (ds.Tables["StudyTypeTags"].Rows.Count > 0)
                    {
                        arrStudyFields = new string[ds.Tables["StudyTypeTags"].Rows.Count];

                        foreach (DataRow dr in ds.Tables["StudyTypeTags"].Rows)
                        {
                            arrStudyFields[idx] = Convert.ToString(dr["field_code"]);
                            idx = idx + 1;
                        }
                    }


                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Connecting eRAD Server...server IP : " + strWS8SRVIP + " Session : " + strWS8SessionID, false);
                    //if (strWS8SessionID.Trim() == string.Empty) bRet = client.GetSession(strWS8CLTIP, strWS8SRVIP, strWS8SRVUID, strWS8SRVPWD, ref strWS8SessionID, ref sCatchMsg, ref sError);
                    //else bRet = true;

                    bRet = true;

                    if (bRet)
                    {
                        foreach (DataRow dr in ds.Tables["Details"].Rows)
                        {
                            Id = new Guid(Convert.ToString(dr["id"]));
                            strStudyUID = Convert.ToString(dr["study_uid"]).Trim();

                            try
                            {
                                #region Build Up Write Back Data

                                #region Study Details
                                client.ACCESSION_NO = Convert.ToString(dr["accession_no"]).Trim();
                                client.PATIENT_ID = Convert.ToString(dr["patient_id"]).Trim();
                                client.PATIENT_NAME = Convert.ToString(dr["patient_name"]).Trim().Replace(" ", "^");
                                client.PATIENT_GENDER = Convert.ToString(dr["patient_sex"]).Trim();
                                client.SPAYED_NEUTERED = Convert.ToString(dr["patient_sex_neutered"]).Trim();
                                client.PATIENT_WEIGHT = Convert.ToString(dr["patient_weight"]);
                                client.PATIENT_WEIGHT_KGS = Convert.ToString(dr["patient_weight_kgs"]);

                                if (Convert.ToDateTime(dr["patient_dob_accepted"]).Year <= 1900)
                                    client.PATIENT_DOB = "00000000_000000";
                                else
                                    client.PATIENT_DOB = Convert.ToDateTime(dr["patient_dob_accepted"]).ToString("yyyyMMdd") + "_000000";

                                client.PATIENT_AGE = Convert.ToString(dr["patient_age_accepted"]);// +"Y";
                                client.OWNER_NAME = Convert.ToString(dr["owner_name"]).Trim().Replace(" ", "^");
                                client.SPECIES = Convert.ToString(dr["species_name"]).Trim();
                                client.BREED = Convert.ToString(dr["breed_name"]).Trim();
                                client.MODALITY = Convert.ToString(dr["modality_name"]).Trim();
                                client.BODY_PART = Convert.ToString(dr["body_part_name"]).Trim();
                                client.REASON = Convert.ToString(dr["reason_accepted"]).Trim();
                                client.REFERRING_PHYSICIAN = Convert.ToString(dr["physician_name"]).Trim();
                                client.INSTITUTION_NAME = strInstName =  Convert.ToString(dr["institution_name"]).Trim();
                                client.INSTITUTION_CODE = strInstCode =  Convert.ToString(dr["institution_code"]).Trim();//
                                //client.PHYSICIAN_NOTE = Convert.ToString(dr["physician_note"]).Trim();

                                if (Convert.ToInt32(dr["priority_id"]) > 0)
                                    client.PRIORITY_ID = Convert.ToString(dr["priority_id"]);
                                else
                                    client.PRIORITY_ID = Convert.ToString(dr["priority_id_pacs"]);

                                client.SALES_PERSON = Convert.ToString(dr["salesperson"]).Trim();
                                client.MERGE_STATUS = Convert.ToString(dr["merge_status_desc"]).Trim();
                                client.STATUS = Convert.ToString(dr["study_status"]).Trim();
                                intStatusID = Convert.ToInt32(dr["study_status"]);
                                client.SERVICE_CODES = Convert.ToString(dr["service_codes"]).Trim();

                                if (Convert.ToDateTime(dr["rpt_record_date"]).Year == 1900)
                                    client.RECORDING_DATE = "00000000_000000";
                                else
                                    client.RECORDING_DATE = Convert.ToDateTime(dr["rpt_record_date"]).ToString("yyyyMMdd") + "_000000";

                                if (Convert.ToDateTime(dr["rpt_approve_date"]).Year == 1900)
                                    client.APPROVAL_DATE = "00000000_000000";
                                else
                                    client.APPROVAL_DATE = Convert.ToDateTime(dr["rpt_approve_date"]).ToString("yyyyMMdd") + "_000000";

                                client.DICTATED_BY = Convert.ToString(dr["dict_radiologist_pacs"]).Trim().Replace(" ", "^");
                                client.PRELIMINARY_RADIOLOGIST = Convert.ToString(dr["prelim_radiologist_pacs"]).Trim().Replace(" ", "^");
                                client.FINAL_RADIOLOGIST = Convert.ToString(dr["final_radiologist_pacs"]).Trim().Replace(" ", "^");
                                client.RADIOLOGIST = Convert.ToString(dr["radiologist_pacs"]).Trim();

                                if (Convert.ToDateTime(dr["submit_on"]).Year <= 1900)
                                    client.SUBMISSION_DATETIME = "00000000_000000";
                                else
                                    client.SUBMISSION_DATETIME = Convert.ToDateTime(dr["submit_on"]).ToString("yyyyMMdd_HHmmss");

                                if (Convert.ToDateTime(dr["received_date"]).Year <= 1900)
                                    client.RECEIVED_DATE = "00000000_000000";
                                else
                                    client.RECEIVED_DATE = Convert.ToDateTime(dr["received_date"]).ToString("yyyyMMdd_HHmmss");


                                #endregion

                                #region Study Types
                                DataView dvST = new DataView(ds.Tables["StudyTypes"]);
                                dvST.RowFilter = "study_hdr_id='" + Convert.ToString(Id) + "'";
                                if (dvST.ToTable().Rows.Count > 0)
                                {
                                    string[] arr = new string[dvST.ToTable().Rows.Count];
                                    idx = 0;

                                    foreach (DataRow dr1 in dvST.ToTable().Rows)
                                    {
                                        strField = Convert.ToString(dr1["write_back_tag"]).Trim();
                                        strStudyType = Convert.ToString(dr1["study_type_name"]).Trim();

                                        switch (strField)
                                        {
                                            case "DSCR":
                                                client.STUDY_TYPE_1 = strStudyType;
                                                break;
                                            case "UDF4":
                                                client.STUDY_TYPE_2 = strStudyType;
                                                break;
                                            case "UDF7":
                                                client.STUDY_TYPE_3 = strStudyType;
                                                break;
                                            case "UDF9":
                                                client.STUDY_TYPE_4 = strStudyType;
                                                break;

                                        }

                                        arr[idx] = strField;
                                        idx = idx + 1;
                                    }


                                }
                                #endregion

                                #endregion

                                #region Write back and update status
                                sCatchMsg = string.Empty;
                                sError = string.Empty;
                                bRet = client.WriteBack(strWS8SessionID, strWS8SRVIP, strStudyUID, ref strResult, ref sCatchMsg, ref sError);

                                if (bRet)
                                {
                                     #region Upload Documents
                                     DataView dvDoc = new DataView(ds.Tables["Documents"]);
                                     dvDoc.RowFilter = "study_hdr_id='" + Convert.ToString(Id) + "'";
                                     if (dvDoc.ToTable().Rows.Count > 0)
                                     {
                                        bRet = UploadDocuments(Id, strStudyUID,strInstName,strInstCode, dvDoc.ToTable());
                                     }
                                     else
                                        bRet = true;

                                    dvDoc.Dispose();
                                    #endregion

                                 
                                    if(bRet)
                                    {
                                        #region Transfer DCM Files
                                        DataView dvDCM = new DataView(ds.Tables["DCMFiles"]);
                                        dvDCM.RowFilter = "study_hdr_id='" + Convert.ToString(Id) + "'";
                                        if (dvDCM.ToTable().Rows.Count > 0)
                                        {
                                            bRet = TransferFileToPacs(strStudyUID, strInstName, strInstCode, dvDCM.ToTable());
                                        }
                                        else
                                            bRet = true;

                                        dvDCM.Dispose();
                                        #endregion
                                    }

                                    if (bRet)
                                    {
                                        #region Update Write Back Status
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Updating status for Study UID : " + strStudyUID, false);


                                        objWB.ID = Id;
                                        objWB.STUDY_UID = strStudyUID;
                                        objWB.STATUS_ID = intStatusID;

                                        if (!objWB.UpdateWriteBackStatus(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                                        {
                                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateWriteBackStatus() - " + strCatchMessage, false);
                                        }
                                        #endregion

                                    }
                                }
                                else
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "WriteBack() - Study UID : " + strStudyUID + " - Error: " + sCatchMsg + "[" + sError + "]", true);
                                    if (sError.Trim().ToUpper().Contains("NO MATCHING STUDY WAS FOUND"))
                                    {
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting Study UID : " + strStudyUID, false);

                                        #region "NO MATCHING STUDY WAS FOUND"
                                        CaseStudyUpdate objSU = new CaseStudyUpdate();
                                        objSU.STUDY_UID = strStudyUID;
                                        try
                                        {
                                            if (!objSU.DeleteStudy(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                                            {
                                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doWriteBack()=>Core::DeleteStudy() - Error" + strCatchMessage, false);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doWriteBack()=>Core::DeleteStudy()- Study UID : " + strStudyUID + " - Exception: " + ex.Message, true);
                                        }
                                        objSU = null;
                                        #endregion
                                    }
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Build Data - Study UID : " + strStudyUID + " - Exception: " + ex.Message, true);
                            }
                            finally
                            {
                                objCore = null;
                            }
                        }
                    }
                    else
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetSession() - Error: " + sCatchMsg + "[" + sError + "]", true);
                    }
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doWriteBack() -->FetchWriteBackList()  - Exception: " + strCatchMsg, true);
                    EventLog.WriteEntry(strSvcName, strCatchMsg, EventLogEntryType.Error);
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doWriteBack() - Exception: " + ex.Message, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);

            }
            finally
            {
                objWB = null; ds.Dispose(); client = null;
            }


        }
        #endregion

        #region doWriteBackReports
        private void doWriteBackReports()
        {

            string strResult = string.Empty;
            string strCatchMsg = string.Empty;

            DataSet ds = new DataSet();
            Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
            string strStudyUID = string.Empty;
            int intStatusID = 0;
            string strIsAddendum = string.Empty;
            int intAddendumSrl = 0;
            string strWBURL = string.Empty;
            string strCatchMessage = string.Empty;
            bool bRet = false;
            RadWebClass client = new RadWebClass();
            string sCatchMsg = string.Empty;
            string sError = string.Empty;

            objWB = new DataWriteBack();


            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Fetching report write back list...", false);
                if (objWB.FetchWriteBackReportStudies(strConfigPath, ref ds, ref strCatchMsg))
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ds.Tables["StudyIDs"].Rows.Count.ToString() + " record(s) fetched for report write back.", false);
                    strWBURL = strURL;
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Connecting eRAD Server...server IP : " + strWS8SRVIP + " Session : " + strWS8SessionID, false);
                    bRet = true;

                    if (bRet)
                    {
                        foreach (DataRow dr in ds.Tables["StudyIDs"].Rows)
                        {
                            Id = new Guid(Convert.ToString(dr["study_hdr_id"]));
                            strStudyUID = Convert.ToString(dr["study_uid"]).Trim();
                            intStatusID = Convert.ToInt32(dr["status_id"]);
                            strIsAddendum = Convert.ToString(dr["is_addendum"]).Trim();

                            try
                            {
                                objWB.ID = Id;
                                objWB.IS_ADDENDUM = strIsAddendum;

                                if (objWB.FetchWriteBackReporDetails(strConfigPath, ref ds, ref strCatchMsg))
                                {
                                    
                                    foreach (DataRow drHdr in ds.Tables["ReportHdr"].Rows)
                                    {
                                        #region Build Up Report Write Back Data
                                        client.STATUS = Convert.ToString(drHdr["study_status_pacs"]).Trim();
                                        client.DICTATED_BY = Convert.ToString(drHdr["dict_radiologist_pacs"]).Trim();
                                        client.PRELIMINARY_RADIOLOGIST = Convert.ToString(drHdr["prelim_radiologist_pacs"]).Trim();
                                        client.FINAL_RADIOLOGIST = Convert.ToString(drHdr["final_radiologist_pacs"]).Trim();

                                        if (intStatusID == 60)
                                        {
                                            if (Convert.ToDateTime(drHdr["dict_date"]).Year == 1900)
                                                client.RECORDING_DATE = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                                            else
                                                client.RECORDING_DATE = Convert.ToDateTime(drHdr["dict_date"]).ToString("yyyyMMdd_HHmmss");
                                        }
                                        else if (intStatusID == 80)
                                        {
                                            if (Convert.ToDateTime(drHdr["prelim_date"]).Year == 1900)
                                                client.RECORDING_DATE = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                                            else
                                                client.RECORDING_DATE = Convert.ToDateTime(drHdr["prelim_date"]).ToString("yyyyMMdd_HHmmss");
                                        }
                                        else
                                        {
                                            if (Convert.ToDateTime(drHdr["prelim_date"]) > Convert.ToDateTime(drHdr["dict_date"]))
                                            {
                                                client.RECORDING_DATE = Convert.ToDateTime(drHdr["prelim_date"]).ToString("yyyyMMdd_HHmmss");
                                            }
                                            else if (Convert.ToDateTime(drHdr["dict_date"]) > Convert.ToDateTime(drHdr["prelim_date"]))
                                            {
                                                client.RECORDING_DATE = Convert.ToDateTime(drHdr["dict_date"]).ToString("yyyyMMdd_HHmmss");
                                            }
                                            else
                                            {
                                                client.RECORDING_DATE = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                                            }
                                        }

                                        if (intStatusID == 100)
                                        {
                                            if (Convert.ToDateTime(drHdr["final_date"]).Year == 1900)
                                                client.APPROVAL_DATE = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                                            else
                                                client.APPROVAL_DATE = Convert.ToDateTime(drHdr["final_date"]).ToString("yyyyMMdd_HHmmss");
                                        }
                                        else
                                        {
                                            client.APPROVAL_DATE = "00000000_000000";
                                        }

                                        #region Report Details

                                        client.REPORT_TEXT = string.Empty;
                                        client.IS_ADDENDUM = "N";
                                        intAddendumSrl = 0;

                                        foreach (DataRow drRpt in ds.Tables["ReportDtls"].Rows)
                                        {

                                            client.REPORT_TEXT = Convert.ToString(drRpt["report_text"]).Trim();
                                            client.IS_ADDENDUM = strIsAddendum = Convert.ToString(drRpt["is_addendum"]).Trim();
                                            intAddendumSrl = Convert.ToInt32(drRpt["record_id"]);
                                        }
                                        #endregion 

                                        #endregion

                                        #region Write back and update status
                                        sCatchMsg = string.Empty;
                                        sError = string.Empty;
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Updating write back status of the report for Study UID : " + strStudyUID, false);
                                        bRet = client.ReportWriteBack(strWS8SessionID, strWS8SRVIP, strStudyUID, ref strResult, ref sCatchMsg, ref sError);

                                        if (bRet)
                                        {
                                            #region Write Back Addendum
                                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Write back of the report for Study UID : " + strStudyUID + " successfully done", false);

                                            objWB.ID = Id;
                                            objWB.STUDY_UID = strStudyUID;
                                            objWB.STATUS_ID = intStatusID;
                                            objWB.IS_ADDENDUM = strIsAddendum;
                                            objWB.ADDENDUM_SERIAL = intAddendumSrl;

                                            if (!objWB.UpdateReportWriteBackStatus(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                                            {
                                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateReportWriteBackStatus() - " + strCatchMessage, false);
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "ReportWriteBack() - Study UID : " + strStudyUID + " - Error: " + sCatchMsg + "[" + sError + "]", true);
                                            if (sError.Trim().ToUpper().Contains("REPORT IDENTIFIERS MATCH TO A FINAL REPORT"))
                                            {
                                               
                                                #region "REPORT IDENTIFIERS MATCH TO A FINAL REPORT"
                                                objWB.ID = Id;
                                                objWB.STUDY_UID = strStudyUID;
                                                objWB.STATUS_ID = intStatusID;
                                                objWB.IS_ADDENDUM = strIsAddendum;
                                                objWB.ADDENDUM_SERIAL = intAddendumSrl;

                                                if (!objWB.UpdateReportWriteBackStatus(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                                                {
                                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doWriteBackReports()::Core::UpdateReportWriteBackStatus() - " + strCatchMessage, false);
                                                }
                                                #endregion
                                            }
                                            else if (sError.Trim().ToUpper().Contains("NO MATCHING STUDY WAS FOUND"))
                                            {
                                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Deleting Study UID : " + strStudyUID, false);

                                                #region "NO MATCHING STUDY WAS FOUND"
                                                CaseStudyUpdate objSU = new CaseStudyUpdate();
                                                objSU.STUDY_UID = strStudyUID;

                                                if (!objSU.DeleteStudy(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                                                {
                                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doWriteBackReports()=>Core::DeleteStudy() - " + strCatchMessage, false);
                                                }
                                                objSU = null;
                                                #endregion
                                            }
                                            
                                                
                                        }
                                        #endregion
                                    }

                                }
                                else
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doWriteBackReports() :: FetchWriteBackReporDetails() - Exception: " + strCatchMsg, true);
                                    EventLog.WriteEntry(strSvcName, strCatchMsg, EventLogEntryType.Error);
                                }
                            }
                            catch (Exception ex)
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Report Write Back Build Data - Study UID : " + strStudyUID + " - Exception: " + ex.Message, true);
                            }
                            finally
                            {
                                objCore = null;
                            }
                        }
                    }
                    else
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetSession() - Error: " + sCatchMsg + "[" + sError + "]", true);
                    }
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doWriteBackReports() ::FetchWriteBackReportStudies() - Exception: " + strCatchMsg, true);
                    EventLog.WriteEntry(strSvcName, strCatchMsg, EventLogEntryType.Error);
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doWriteBackReports() - Exception: " + ex.Message, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);

            }
            finally
            {
                objWB = null; ds.Dispose(); client = null;
            }


        }
        #endregion

        #region doUpdateUsersInPACS
        private void doUpdateUsersInPACS()
        {

            RadWebClass client = new RadWebClass();
            DataSet ds = new DataSet();
            Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
            string strUserPACSID = string.Empty;
            string strUserPACSPwd = string.Empty;
            string strUserGroup = string.Empty;
            string strUserName = string.Empty;
            string strUserType = string.Empty;
            string strRights = string.Empty;
            string strEmailID = string.Empty;
            string strContNo = string.Empty;
            string strAppliedURL = string.Empty;
            string strResult = string.Empty;
            string[] arrSep = new string[1];
            string[] arrErr = new string[0];
            string strWarn = string.Empty;
            string strErr = string.Empty;
            string sCatchMsg = string.Empty;
            string sError = string.Empty;
            string strCatchMsg = string.Empty;

            bool bRet = false;

            objWB = new DataWriteBack();

            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Fetching users list to update in PACS", false);
                if (objWB.FetchUserUpdationList(strConfigPath, ref ds, ref strCatchMsg))
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ds.Tables["Users"].Rows.Count.ToString() + " user record(s) fetched.", false);

                    try
                    {

                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Connecting eRAD Server...server IP : " + strWS8SRVIP, false);
                        if (strWS8SessionID.Trim() == string.Empty) bRet = client.GetSession(strWS8CLTIP, strWS8SRVIP, strWS8SRVUID, strWS8SRVPWD, ref strWS8SessionID, ref sCatchMsg, ref sError);
                        else bRet = true;


                        if (bRet)
                        {

                            foreach (DataRow dr in ds.Tables["Users"].Rows)
                            {

                                Id = new Guid(Convert.ToString(dr["user_id"]));
                                strUserPACSID = Convert.ToString(dr["user_pacs_user_id"]).Trim();
                                strUserPACSPwd = CoreCommon.DecryptString(Convert.ToString(dr["user_pacs_password"]).Trim());
                                strUserName = Convert.ToString(dr["user_name"]).Trim();
                                strUserGroup = Convert.ToString(dr["user_group"]).Trim();
                                strUserType = Convert.ToString(dr["user_type"]).Trim();
                                strRights = Convert.ToString(dr["granted_rights_pacs"]).Trim();


                                bRet = client.UpdateUser(strWS8SessionID, strWS8SRVIP, strUserPACSID, strUserPACSPwd, strUserGroup, strRights,
                                                         strUserName, string.Empty, strEmailID, strContNo,
                                                         ref sCatchMsg, ref sError);

                                if (bRet)
                                {

                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "User " + strUserPACSID + " updated in PACS", false);
                                    doUpdateSuccessFlag(Id, strUserPACSID, strUserType, ref strCatchMsg);

                                }
                                else
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateUser() - Error: " + sCatchMsg + "[" + sError + "]", true);

                                }
                            }
                        }
                        else
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetSession() - Error: " + sCatchMsg + "[" + sError + "]", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doUpdateUsersInPACS() - Exception: " + ex.Message, true);

                    }
                    finally
                    {
                        objCore = null;
                    }
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Core:FetchUserUpdationList()  - Exception: " + strCatchMsg, true);
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doUpdateUsersInPACS() - Exception: " + ex.Message, true);


            }
            finally
            {
                objWB = null; ds.Dispose(); client = null;
            }
        }
        #endregion

        #region doUpdateSuccessFlag
        private bool doUpdateSuccessFlag(Guid userID, string loginID, string userType, ref string CatchMessage)
        {
            bool bReturn = false;


            try
            {
                objWB.USER_ID = userID;
                objWB.USER_TYPE = userType;

                bReturn = objWB.UpdateUserUpdateInPACS(strConfigPath, intServiceID, strSvcName, ref CatchMessage);

                if (!bReturn)
                {
                    if (CatchMessage.Trim() != string.Empty) CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doUpdateSuccessFlag()- User :: " + Convert.ToString(loginID) + " - Error: " + CatchMessage, true);
                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doUpdateSuccessFlag() - Exception: " + ex.Message, true);

            }

            return bReturn;
        }
        #endregion

        #region UploadDocuments
        private bool UploadDocuments(Guid Id, string StudyUID,string InstitutionName,string InstitutionCode, DataTable dtbl)
        {
            bool bReturn = false;
            string strFileName = string.Empty;
            string[] arrSrcFiles = new string[0];
            string[] arrTgtFiles = new string[0];
            string strProcDocSrcPath = strDocDCMPath + "/Docs";
            string strProcImgSrcPath = strDocDCMPath + "/Img";
            string strProcTgtPath = strDocDCMPath + "/DCM";
            string strDocSrcPath = strDocDCMPath + "/Docs/" + StudyUID;
            string strDocImgPath = strDocDCMPath + "/Img/" + StudyUID;
            string strTgtPath = strDocDCMPath + "/DCM/" + StudyUID;
            string strArchFolder = strPACSARCHIVEFLDR + "/" + InstitutionCode + "_" + InstitutionName + "_" + StudyUID;
            int exitCode;
            string strFileContentType = string.Empty;
            //string strConvFlg = "N";
            string[] arrDCMFiles = new string[0];
            string[] arrFile = new string[0];
            string[] arrSep = { "\\" };

            if (!Directory.Exists(strDocSrcPath)) Directory.CreateDirectory(strDocSrcPath);
            if (!Directory.Exists(strTgtPath)) Directory.CreateDirectory(strTgtPath);

            //if (Directory.Exists(strDocSrcPath)) CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Folder - " + strDocSrcPath + " created", false);
            //else CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Folder - " + strDocSrcPath + " not created", false);
            //if (Directory.Exists(strTgtPath)) CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Folder - " + strTgtPath + " created", false);
            //else CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Folder - " + strTgtPath + " created", false);

            #region Convert to file from bytes
            try
            {
                foreach (DataRow dr in dtbl.Rows)
                {
                    strFileName = Convert.ToString(dr["document_link"]);
                    SetFile((byte[])dr["document_file"], Convert.ToString(dr["document_link"]).Trim(), strDocSrcPath);
                }

                bReturn = true;
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - Conversion To file - Exception: " + ex.Message, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
                bReturn = false;
            }
            #endregion



            if (bReturn)
            {
                bReturn = false;
                arrSrcFiles = Directory.GetFiles(strDocSrcPath, "*.*", SearchOption.AllDirectories);

                #region check/rearrange/convert pdf files to image
                try
                {
                    if (File.Exists(strPdftoImgExePath))
                    {
                        for (int i = 0; i < arrSrcFiles.Length; i++)
                        {
                            arrFile = arrSrcFiles[i].Split(arrSep, StringSplitOptions.None);
                            strFileName = arrFile[arrFile.Length - 1];

                            strFileContentType = MIMEAssistant.GetMIMEType(arrSrcFiles[i]);
                            if (!Directory.Exists(strDocImgPath)) Directory.CreateDirectory(strDocImgPath);

                            if ((strFileContentType == "image/pjpeg") || (strFileContentType == "image/jpeg") || (strFileContentType == "image/x-png") || (strFileContentType == "image/png") || (strFileContentType == "image/gif") || (strFileContentType == "image/bmp"))
                            {
                                if (File.Exists(strDocImgPath + "/" + strFileName)) File.Delete(strDocImgPath + "/" + strFileName);
                                File.Copy(arrSrcFiles[i], strDocImgPath + "/" + strFileName);
                            }
                            else if ((strFileContentType == "application/pdf"))
                            {

                                Process ProcPdfToImg = new Process();
                                ProcPdfToImg.StartInfo.UseShellExecute = false;
                                ProcPdfToImg.StartInfo.FileName = strPdftoImgExePath;
                                ProcPdfToImg.StartInfo.Arguments = strDocSrcPath + "±" + strDocImgPath + "±" + "\"" + strFileName + "\"";
                                ProcPdfToImg.StartInfo.RedirectStandardOutput = true;
                                ProcPdfToImg.Start();
                                ProcPdfToImg.WaitForExit();

                                exitCode = ProcPdfToImg.ExitCode;
                                if (ProcPdfToImg.HasExited)
                                {
                                    if (exitCode <= 0)
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - check/rearrange/convert pdf files to image for Study UID : " + StudyUID + " :: File Name : " + arrSrcFiles[i] + " failed", true);

                                }
                            }
                            else
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - check/rearrange/convert pdf files to image for Study UID : " + StudyUID + " ::  File format of " + arrSrcFiles[i] + " is not supported", true);
                            }
                        }
                        bReturn = true;
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - check/rearrange/convert pdf files to image for Study UID : " + StudyUID + " - done successfully", false);
                    }
                    else
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - check/rearrange/convert pdf files to image for Study UID : " + StudyUID + " - .exe not found", true);
                        bReturn = false;
                    }
                }
                catch (Exception ex)
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - check/rearrange/convert pdf files to image for Study UID : " + StudyUID + " - Exception: " + ex.Message, true);
                    EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
                    bReturn = false;
                }
                #endregion
            }

            if (bReturn)
            {
                bReturn = false;

                #region Convert to DCM


                try
                {

                    if (File.Exists(strImgtoDCMExePath))
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - Converting To DCM for Study UID : " + StudyUID, false);

                        // #region 
                        Process ProcImgToDcm = new Process();
                        ProcImgToDcm.StartInfo.UseShellExecute = false;
                        ProcImgToDcm.StartInfo.FileName = strImgtoDCMExePath;
                        ProcImgToDcm.StartInfo.Arguments = StudyUID + "±" + 
                                                           strProcImgSrcPath.Trim().Replace(" ", "»") + "±" + 
                                                           string.Empty + "±" + 
                                                           strTgtPath.Trim().Replace(" ", "»") + 
                                                           "±" + 
                                                           "±" + 
                                                           "Y";
                        ProcImgToDcm.StartInfo.RedirectStandardOutput = true;
                        ProcImgToDcm.Start();
                        ProcImgToDcm.WaitForExit();

                        exitCode = ProcImgToDcm.ExitCode;

                        if (ProcImgToDcm.HasExited)
                        {
                            if (exitCode <= 0)
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - Conversion To DCM for Study UID : " + StudyUID + " failed", true);
                        }
                        // #endregion

                        #region Python
                        //for (int i = 0; i < arrSrcFiles.Length; i++)
                        //{
                        //    strConvFlg = "N";
                        //    if (!Directory.Exists(strTgtPath))
                        //    {
                        //        Directory.CreateDirectory(strTgtPath);
                        //    }

                        //    ProcessStartInfo start = new ProcessStartInfo();
                        //    start.Arguments = arrSrcFiles[i] + " " + strTgtPath + "/" + " " + StudyUID;
                        //    start.FileName = strImgtoDCMExePath;
                        //    start.UseShellExecute = true;
                        //    start.CreateNoWindow = true;
                        //    start.WorkingDirectory = strImgtoDCMExePath.Substring(0,strImgtoDCMExePath.LastIndexOf("/"));

                        //    Process proc = new Process();
                        //    proc.StartInfo.RedirectStandardError = true;
                        //    proc.StartInfo.RedirectStandardOutput = true;
                        //    proc = Process.Start(start);
                        //    proc.WaitForExit();

                        //    exitCode = proc.ExitCode;
                        //    if (proc.HasExited)
                        //    {
                        //        if (exitCode != 0)
                        //            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - Conversion To DCM for Study UID : " + StudyUID + " :: File Name : " + arrSrcFiles[i] + " failed", true);
                        //        else
                        //            strConvFlg = "Y";
                        //    }
                        //}
                        #endregion

                        arrSrcFiles = Directory.GetFiles(strDocImgPath, "*.*", SearchOption.AllDirectories);
                        arrTgtFiles = Directory.GetFiles(strTgtPath, "*.*", SearchOption.AllDirectories);

                        if (arrSrcFiles.Length == arrTgtFiles.Length)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - Conversion To DCM for Study UID : " + StudyUID + " done successfully", false);
                            bReturn = true;
                        }
                        else
                        {

                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - Waiting for DCM generation file count for Study UID : " + StudyUID, false);
                            while (arrTgtFiles.Length == arrSrcFiles.Length)
                            {
                                if (arrTgtFiles.Length < arrSrcFiles.Length)
                                {
                                    arrTgtFiles = Directory.GetFiles(strTgtPath, "*.*", SearchOption.AllDirectories);
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - Study UID : " + StudyUID + " :: Doc Count : " + arrSrcFiles.Length.ToString() + " DCM Count : " + arrTgtFiles.Length.ToString(), true);
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - Conversion To DCM for Study UID : " + StudyUID + " done successfully", false);
                            bReturn = true;
                        }

                    }
                    else
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - Conversion To DCM for Study UID : " + StudyUID + " - .exe not found", true);
                        bReturn = false;
                    }


                }
                catch (Exception ex)
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - Conversion To DCM for Study UID : " + StudyUID + " - Exception: " + ex.Message, true);
                    EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
                    bReturn = false;
                }
                #endregion
            }

            if (bReturn)
            {
                bReturn = false;

                #region Copy DCM to Archive
                try
                {
                    if (!Directory.Exists(strArchFolder)) 
                    {
                        Directory.CreateDirectory(strArchFolder);
                    }

                    arrDCMFiles = Directory.GetFiles(strTgtPath ,"*.dcm");

                    foreach(string strFile in arrDCMFiles)
                    {
                        arrFile = strFile.Split('\\');
                        strFileName = arrFile[arrFile.Length - 1];

                        if(!File.Exists(strArchFolder + "\\" + strFileName))
                            File.Copy(strFile,strArchFolder + "\\" + strFileName);

                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - File : " +  strFileName+ " for Study UID : " + StudyUID + " copied to " + strArchFolder, false);
                        
                    }
                    bReturn = true;
                }
                catch (Exception ex)
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - File : " + strFileName + "for Study UID : " + StudyUID + " - Exception: " + ex.Message, true);
                    bReturn = false;
                }
                #endregion
            }

            if (bReturn)
            {
                bReturn = false;

                #region Send DCM to PACS
                Process ProcXfer = new Process();

                try
                {
                    if (File.Exists(strXferExePath))
                    {
                        strXferExePath = strXferExePath.Replace("/", "\\");
                        strTgtPath = strTgtPath.Replace("/", "\\");

                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Transfer of documents initiated - " + strXferExePath + " " + strXferExeParams + " " + strTgtPath + "/*.dcm", false);

                        ProcXfer.StartInfo.UseShellExecute = false;
                        ProcXfer.StartInfo.FileName = strXferExePath;
                        ProcXfer.StartInfo.Arguments = strXferExeParams + " " + strTgtPath + "\\*.dcm";
                        ProcXfer.StartInfo.RedirectStandardOutput = true;
                        ProcXfer.Start();

                        bReturn = true;
                    }
                    else
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - DCM transfer to PACS for Study UID : " + StudyUID + " - .exe not found", true);
                        bReturn = false;
                    }
                }
                catch (Exception ex)
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UploadDocuments() - DCM transfer to PACS for Study UID : " + StudyUID + " - Exception: " + ex.Message, true);
                    EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);
                    bReturn = false;
                }

                ProcXfer.Dispose();
                #endregion
            }
            return bReturn;
        }
        #endregion

        #region TransferFileToPacs
        private bool TransferFileToPacs(string StudyUID,string InstitutionName,string InstitutionCode,DataTable dtbl)
        {
            bool bRet = false;
            string strSUID = string.Empty;
            string strFolder= string.Empty;
            string strFile = string.Empty;
            string strProcOutput = string.Empty;
            string strProcError = string.Empty;
            string strProcMsg = string.Empty;
            string strRetMsg= string.Empty;
            string strArchFolder = strPACSARCHIVEFLDR + "/" + InstitutionCode + "_" + InstitutionName + "_" + StudyUID;
            string strTgtPath = strDocDCMPath + "/DCM/" + StudyUID;
            string[] arrDCMFiles = new string[0];
            int intIdx = 0;
            string[] arrFile = new string[0];
            string strFileName = string.Empty;
            bool bReturn = false;

            try
            {
                strFolder = strTEMPDCMATCHPATH;
                if (!Directory.Exists(strFolder)) Directory.CreateDirectory(strFolder);

                arrDCMFiles = new string[dtbl.Rows.Count];
                foreach (DataRow dr in dtbl.Rows)
                {
                    #region Convert to file from bytes
                    try
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "TransferFileToPacs() - Conversion To file - Creating File: " + Convert.ToString(dr["dcm_file_name"]), false);
                        strSUID = Convert.ToString(dr["study_uid"]).Trim();
                        strFile = Convert.ToString(dr["dcm_file_name"]).Trim();
                        SetFile((byte[])dr["dcm_file"], Convert.ToString(dr["dcm_file_name"]).Trim(), strFolder);
                        arrDCMFiles[intIdx] = strFolder + "\\" + Convert.ToString(dr["dcm_file_name"]).Trim();
                        intIdx = intIdx + 1;
                        bReturn = true;
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "TransferFileToPacs() - Conversion To file - File: " + Convert.ToString(dr["dcm_file_name"]) + " created", false);
                    }

                    catch (Exception ex)
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "TransferFileToPacs() - Conversion To file - Exception: " + ex.Message, true);
                        bReturn = false;
                    }
                    #endregion

                    if (bReturn)
                    {
                        bReturn = false;

                        #region Copy DCM to Archive
                        try
                        {
                            if (!Directory.Exists(strArchFolder))
                            {
                                Directory.CreateDirectory(strArchFolder);
                            }

                            foreach (string fl in arrDCMFiles)
                            {
                                arrFile = fl.Split('\\');
                                strFileName = arrFile[arrFile.Length - 1];

                                if (!File.Exists(strArchFolder + "\\" + strFileName))
                                    File.Copy(fl, strArchFolder + "\\" + strFileName);

                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "TransferFileToPacs() - File : " + strFileName + " for Study UID : " + StudyUID + " copied to " + strArchFolder, true);

                            }
                            bReturn = true;
                        }
                        catch (Exception ex)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "TransferFileToPacs() - File : " + strFileName + "for Study UID : " + StudyUID + " - Exception: " + ex.Message, true);
                            bReturn = false;
                        }
                        #endregion
                    }

                    if (bReturn)
                    {
                        #region NORMAL
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "1-File sending started with " + strXferExePath + " " + strXferExeParams + " " + strFolder + "/" + strFile, false);
                        Process ProcXfer = new Process();
                        ProcXfer.StartInfo.UseShellExecute = false;
                        ProcXfer.StartInfo.FileName = strXferExePath;
                        ProcXfer.StartInfo.Arguments = strXferExeParams + " " + strFolder + "/" + strFile;
                        ProcXfer.StartInfo.RedirectStandardOutput = true;
                        ProcXfer.StartInfo.RedirectStandardError = true;
                        ProcXfer.Start();
                        strProcOutput = ProcXfer.StandardOutput.ReadToEnd();
                        strProcError = ProcXfer.StandardError.ReadToEnd();
                        strProcMsg = strProcOutput.Trim();

                        //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "TransferFileToPacs():Message: " + strProcMsg, false);
                        // if (strProcMsg.ToUpper().Contains("RECEIVED STORE RESPONSE (SUCCESS)"))

                        if ((strProcMsg.ToUpper().Contains("[STATUS=SUCCESS]")) || (strProcMsg.ToUpper().Contains("RECEIVED STORE RESPONSE (SUCCESS)")))
                        {
                            strRetMsg = strProcMsg;
                            bRet = true;
                        }
                        else
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "1-File sending failed with " + strXferExePath + " " + strXferExeParams + " " + strFolder + "/" + strFile, true);
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "1-Reason : " + strProcMsg, true);
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "1-File sending failed with " + strXferExePath + " " + strXferExeParams + " " + strFolder + "/" + strFile, false);
                            bRet = false;
                        }
                        #endregion

                        #region JPG Lossless
                        if (bRet == false)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "2-File sending started with " + strXferExePath + " " + strXferExeParamsJPGLL + " " + strFolder + "/" + strFile, false);
                            Process ProcXferAlt = new Process();
                            ProcXferAlt.StartInfo.UseShellExecute = false;
                            ProcXferAlt.StartInfo.FileName = strXferExePath;
                            ProcXferAlt.StartInfo.Arguments = strXferExeParamsJPGLL + " " + strFolder + "/" + strFile;
                            ProcXferAlt.StartInfo.RedirectStandardOutput = true;
                            ProcXferAlt.StartInfo.RedirectStandardError = true;
                            ProcXferAlt.Start();
                            strProcOutput = ProcXferAlt.StandardOutput.ReadToEnd();
                            strProcError = ProcXferAlt.StandardError.ReadToEnd();
                            strProcMsg = strProcOutput.Trim();


                            //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "TransferFileToPacs():Message: " + strProcMsg, false);
                            if ((strProcMsg.ToUpper().Contains("[STATUS=SUCCESS]")) || (strProcMsg.ToUpper().Contains("RECEIVED STORE RESPONSE (SUCCESS)")))
                            {
                                strRetMsg = strProcMsg;
                                bRet = true;
                            }
                            else
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "2-File sending failed with " + strXferExePath + " " + strXferExeParamsJPGLL + " " + strFolder + "/" + strFile, true);
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "2-Reason : " + strProcMsg, true);
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "2-File sending failed with " + strXferExePath + " " + strXferExeParamsJPGLL + " " + strFolder + "/" + strFile, false);
                                bRet = false;
                            }
                        }
                        #endregion

                        #region JPG 2K Lossless
                        if (bRet == false)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "3-File sending started with " + strXferExePath + " " + strXFEREXEPARMJ2KLL + " " + strFolder + "/" + strFile, false);
                            Process ProcXferJ2k = new Process();
                            ProcXferJ2k.StartInfo.UseShellExecute = false;
                            ProcXferJ2k.StartInfo.FileName = strXferExePath;
                            ProcXferJ2k.StartInfo.Arguments = strXFEREXEPARMJ2KLL + " " + strFolder + "/" + strFile;
                            ProcXferJ2k.StartInfo.RedirectStandardOutput = true;
                            ProcXferJ2k.StartInfo.RedirectStandardError = true;
                            ProcXferJ2k.Start();
                            strProcOutput = ProcXferJ2k.StandardOutput.ReadToEnd();
                            strProcError = ProcXferJ2k.StandardError.ReadToEnd();
                            strProcMsg = strProcOutput.Trim();

                            if ((strProcMsg.ToUpper().Contains("[STATUS=SUCCESS]")) || (strProcMsg.ToUpper().Contains("RECEIVED STORE RESPONSE (SUCCESS)")))
                            {
                                strRetMsg = strProcMsg;
                                bRet = true;
                            }
                            else
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "3-File sending failed with " + strXferExePath + " " + strXFEREXEPARMJ2KLL + " " + strFolder + "/" + strFile, true);
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "3-Reason : " + strProcMsg, true);
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "3-File sending failed with " + strXferExePath + " " + strXFEREXEPARMJ2KLL + " " + strFolder + "/" + strFile, false);
                                bRet = false;
                            }
                        }
                        #endregion

                        #region JPG 2K Lossy
                        if (bRet == false)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "4-File sending started with " + strXferExePath + " " + strXFEREXEPARMJ2KLS + " " + strFolder + "/" + strFile, false);
                            Process ProcXferJ2kL = new Process();
                            ProcXferJ2kL.StartInfo.UseShellExecute = false;
                            ProcXferJ2kL.StartInfo.FileName = strXferExePath;
                            ProcXferJ2kL.StartInfo.Arguments = strXFEREXEPARMJ2KLS + " " + strFolder + "/" + strFile;
                            ProcXferJ2kL.StartInfo.RedirectStandardOutput = true;
                            ProcXferJ2kL.StartInfo.RedirectStandardError = true;
                            ProcXferJ2kL.Start();
                            strProcOutput = ProcXferJ2kL.StandardOutput.ReadToEnd();
                            strProcError = ProcXferJ2kL.StandardError.ReadToEnd();
                            strProcMsg = strProcOutput.Trim();

                            if ((strProcMsg.ToUpper().Contains("[STATUS=SUCCESS]")) || (strProcMsg.ToUpper().Contains("RECEIVED STORE RESPONSE (SUCCESS)")))
                            {
                                strRetMsg = strProcMsg;
                                bRet = true;
                            }
                            else
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "4-File sending failed with " + strXferExePath + " " + strXFEREXEPARMJ2KLS + " " + strFolder + "/" + strFile, true);
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "4-Reason : " + strProcMsg, true);
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "4-File sending failed with " + strXferExePath + " " + strXFEREXEPARMJ2KLS + " " + strFolder + "/" + strFile, false);
                                strRetMsg = strProcMsg;
                                bRet = false;
                            }
                        }
                        #endregion

                        #region DCM Send
                        if (bRet == false)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "5-File sending started with " + strXFEREXEPATHALT + " " + strXferExeParamsSENDDCM + " " + strFolder + "/" + strFile, false);
                            Process ProcXferDCMSend = new Process();
                            ProcXferDCMSend.StartInfo.UseShellExecute = false;
                            ProcXferDCMSend.StartInfo.FileName = strXFEREXEPATHALT;
                            ProcXferDCMSend.StartInfo.Arguments = strXferExeParamsSENDDCM + " " + strFolder + "/" + strFile;
                            ProcXferDCMSend.StartInfo.RedirectStandardOutput = true;
                            ProcXferDCMSend.StartInfo.RedirectStandardError = true;
                            ProcXferDCMSend.Start();
                            strProcOutput = ProcXferDCMSend.StandardOutput.ReadToEnd();
                            strProcError = ProcXferDCMSend.StandardError.ReadToEnd();
                            strProcMsg = strProcOutput.Trim();

                            if ((strProcMsg.ToUpper().Contains("[STATUS=SUCCESS]")) || (strProcMsg.ToUpper().Contains("RECEIVED STORE RESPONSE (SUCCESS)")) || (strProcMsg.ToUpper().Contains("I:   * WITH STATUS SUCCESS  : 1")))
                            {
                                strRetMsg = strProcMsg;
                                bRet = true;
                            }
                            else
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "5-File sending failed with " + strXFEREXEPATHALT + " " + strXferExeParamsSENDDCM + " " + strFolder + "/" + strFile, true);
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "5-Reason : " + strProcMsg, true);
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "5-File sending failed with " + strXFEREXEPATHALT + " " + strXferExeParamsSENDDCM + " " + strFolder + "/" + strFile, false);
                                strRetMsg = strProcMsg;
                                bRet = false;
                            }
                        }
                        #endregion
                    }
                }

            }
            catch (Exception ex)
            {
                bRet = false;
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doWriteBack()=>TransferFilesToPacs():: Exception: " + ex.Message, true);
            }

            return bRet;
        }
        #endregion

        #region SetFile
        private void SetFile(byte[] DocData, string strFileName, string strPath)
        {
            string strFilePath = strPath + "/" + strFileName;
            using (FileStream fs = new FileStream(strFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(DocData, 0, DocData.Length);
                fs.Flush();
                fs.Close();
            }

        }
        #endregion

        #region HtmlToPlainText
        private string HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);
            var text = html;

            text = text.Replace("<p class=\"pasted\">", string.Empty);
            text = text.Replace("<p>", string.Empty);
            text = text.Replace("</p>", "<br/>");

            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }
        #endregion

    }
}
