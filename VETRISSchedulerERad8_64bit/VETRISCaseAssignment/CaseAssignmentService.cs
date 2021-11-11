using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using VETRISScheduler.Core;

namespace VETRISCaseAssignment
{
    public partial class CaseAssignmentService : ServiceBase
    {
        #region members & variables
        private static int intFreq = 10;
        private static string strConfigPath = AppDomain.CurrentDomain.BaseDirectory;
        private static string strSvcName = "VETRIS Case Assignment Service";
        private static int intServiceID = 9;
        private static string strSCHCASVCENBL = "Y";

        Scheduler objCore;
        CaseAssignment objCA;

        #endregion

        public CaseAssignmentService()
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
                            strSCHCASVCENBL = objCore.CASE_ASSIGNMENT_SERVICE_ENABLED;

                            
                            if (strSCHCASVCENBL == "Y")
                            {
                                UpdateRoaster();
                                FetchStudiesToAssign();
                            }

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

        #region UpdateRoaster
        private void UpdateRoaster()
        {

            string strCatchMsg = string.Empty;

            DataSet ds = new DataSet();
            objCA = new CaseAssignment();


            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Updating radiologist roaster...", false);
                if (!objCA.UpdateRoaster(strConfigPath, intServiceID, strSvcName, ref strCatchMsg))
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateRoaster()  - Core::Exception: " + strCatchMsg, true);
                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateRoaster() - Exception: " + ex.Message, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);

            }
            finally
            {
                objCA = null; ds.Dispose();
            }
        }
        #endregion

        #region FetchStudiesToAssign
        private void FetchStudiesToAssign()
        {

            string strCatchMsg = string.Empty;

            DataSet ds = new DataSet();
            objCA = new CaseAssignment();


            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Fetching study(ies) to assign...", false);
                if (objCA.FetchStudiesToAssign(strConfigPath, ref ds, ref strCatchMsg))
                {
                    #region Assignment
                    foreach (DataRow dr in ds.Tables["StudyList"].Rows)
                    {
                        objCA.STUDY_ID = new Guid(Convert.ToString(dr["id"]));
                        objCA.STUDY_UID = Convert.ToString(dr["study_uid"]).Trim();

                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Assiging radiologist for Study UID " + objCA.STUDY_UID, false);

                        if (!objCA.AssignRadiologist(strConfigPath, intServiceID, strSvcName, ref strCatchMsg))
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchStudiesToAssign()=>AssignRadiologist()=>Core:Exception::" + strCatchMsg, true);
                        }
                        //else
                        //{
                        //    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Radiologist assigned to Study UID " + objCA.STUDY_UID, false);
                        //}

                        //if (CreateMailAndSend(Id, strNotifyText, strSubject, strRecepientAddress, strCCAddress, strAttachment, strMailAcctUserID, strMailAcctPwd, ref strCatchMessage))
                        //{

                        //    objNotify.EMAIL_LOG_ID = Id;

                        //    if (!objNotify.UpdateMailSendingStatus(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                        //    {

                        //        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateMailSendingStatus() - Exception : " + strCatchMessage, true);
                        //    }

                        //}
                    }
                    #endregion
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchStudiesToAssign()  - Core::Exception: " + strCatchMsg, true);
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "FetchStudiesToAssign() - Exception: " + ex.Message, true);
                EventLog.WriteEntry(strSvcName, ex.Message, EventLogEntryType.Error);

            }
            finally
            {
                objCA = null; ds.Dispose();
            }


        }
        #endregion
    }
}
