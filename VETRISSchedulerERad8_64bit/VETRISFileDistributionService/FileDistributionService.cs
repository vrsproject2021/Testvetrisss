using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Net;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;
using System.Configuration;
using System.Security;
using System.IO.Compression;
using System.Runtime.InteropServices;
using VETRISScheduler.Core;

namespace VETRISFileDistributionService
{
    public partial class FileDistributionService : ServiceBase
    {
        #region members & variables
        private static int intFreq = 10;
        private static int intServiceID = 7;

        private static string strDCMMODIFYEXEPATH = string.Empty;
        private static string strDCMDMPEXEPATH = string.Empty;
        private static string strDCMRCVEXEPATH = string.Empty;
        private static string strDCMLSNFLDR1 = string.Empty;
        private static string strDCMLSNFLDR2 = string.Empty;
        private static string strDCMLSNFLDR3 = string.Empty;
        private static string strDCMLSNFLDR4 = string.Empty;
        private static string strUMDCMFILES = string.Empty;
        private static string strFILESHOLDPATH = string.Empty;
        private static string strRCVSYNTAX1 = string.Empty;
        private static string strRCVSYNTAX2 = string.Empty;
        private static string strRCVSYNTAX3 = string.Empty;
        private static string strRCVSYNTAX4 = string.Empty;
        private static string strPACSARCHIVEFLDR = string.Empty;
        private static string strENBLLDS = string.Empty;
        private static int intDSNOOFPORTS = 0;

        private static string strConfigPath = AppDomain.CurrentDomain.BaseDirectory;
        private static string strSvcName = "VETRIS File Distribution Service";

        string strErrRegion = string.Empty;

        Scheduler objCore;
        #endregion

        public FileDistributionService()
        {
            InitializeComponent();
        }

        #region OnStart
        protected override void OnStart(string[] args)
        {

            try
            {
                GetServiceDetails();
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Service started Successfully", false);

                System.Threading.ThreadStart job_file_dist_1 = new System.Threading.ThreadStart(doProcessForwardedFiles1);
                System.Threading.Thread threadFileDist1 = new System.Threading.Thread(job_file_dist_1);
                threadFileDist1.Start();

                System.Threading.ThreadStart job_file_dist_2 = new System.Threading.ThreadStart(doProcessForwardedFiles2);
                System.Threading.Thread threadFileDist2 = new System.Threading.Thread(job_file_dist_2);
                threadFileDist2.Start();

                System.Threading.ThreadStart job_file_dist_3 = new System.Threading.ThreadStart(doProcessForwardedFiles3);
                System.Threading.Thread threadFileDist3 = new System.Threading.Thread(job_file_dist_3);
                threadFileDist3.Start();

                System.Threading.ThreadStart job_file_dist_4 = new System.Threading.ThreadStart(doProcessForwardedFiles4);
                System.Threading.Thread threadFileDist4 = new System.Threading.Thread(job_file_dist_4);
                threadFileDist4.Start();

                System.Threading.ThreadStart job_file_dist_5 = new System.Threading.ThreadStart(doProcessUnknownModalityFiles);
                System.Threading.Thread threadFileDist5 = new System.Threading.Thread(job_file_dist_5);
                threadFileDist5.Start();

                if (strENBLLDS == "Y")
                {
                    if (intDSNOOFPORTS > 0)
                    {
                        System.Threading.ThreadStart job_file_rec_1 = new System.Threading.ThreadStart(doProcessReceive1);
                        System.Threading.Thread threadFileRec1 = new System.Threading.Thread(job_file_rec_1);
                        threadFileRec1.Start();
                    }

                    if (intDSNOOFPORTS > 1)
                    {
                        System.Threading.ThreadStart job_file_rec_2 = new System.Threading.ThreadStart(doProcessReceive2);
                        System.Threading.Thread threadFileRec2 = new System.Threading.Thread(job_file_rec_2);
                        threadFileRec2.Start();
                    }

                    if (intDSNOOFPORTS > 2)
                    {
                        System.Threading.ThreadStart job_file_rec_3 = new System.Threading.ThreadStart(doProcessReceive3);
                        System.Threading.Thread threadFileRec3 = new System.Threading.Thread(job_file_rec_3);
                        threadFileRec3.Start();
                    }

                    if (intDSNOOFPORTS > 3)
                    {
                        System.Threading.ThreadStart job_file_rec_4 = new System.Threading.ThreadStart(doProcessReceive4);
                        System.Threading.Thread threadFileRec4 = new System.Threading.Thread(job_file_rec_4);
                        threadFileRec4.Start();
                    }
                }

                
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(strSvcName, "Error Starting Service." + ex.Message, EventLogEntryType.Error);
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
                EventLog.WriteEntry(strSvcName, "Error Stopping Service." + ex.Message, EventLogEntryType.Error);
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Error stopping Service. " + ex.Message, true);
            }
        }
        #endregion

        #region GetServiceDetails
        private bool GetServiceDetails()
        {
            bool bRet = false;
            string strCatchMessage = string.Empty;
            objCore = new Scheduler();

            try
            {
                objCore.SERVICE_ID = intServiceID;
                if (objCore.GetServiceDetails(strConfigPath, ref strCatchMessage))
                {
                    intFreq = objCore.FREQUENCY;
                    strSvcName = objCore.SERVICE_NAME;

                    strDCMMODIFYEXEPATH = objCore.DICOM_TAG_MODIFY_EXE_PATH;
                    strDCMDMPEXEPATH = objCore.DICOM_TAG_DUMP_EXE_PATH;
                    strDCMRCVEXEPATH = objCore.DICOM_RECEIVER_EXE_PATH;
                    strDCMLSNFLDR1 = objCore.DICOM_LISTENER_FOLDER_1;
                    strDCMLSNFLDR2 = objCore.DICOM_LISTENER_FOLDER_2;
                    strDCMLSNFLDR3 = objCore.DICOM_LISTENER_FOLDER_3;
                    strDCMLSNFLDR4 = objCore.DICOM_LISTENER_FOLDER_4;
                    strUMDCMFILES = objCore.UNKNOWN_MODALITY_DICOM_FILES;
                    strFILESHOLDPATH = objCore.FILES_ON_HOLD_FOLDER;
                    strRCVSYNTAX1 = objCore.DICOM_LISTENER_SYNTAX_1;
                    strRCVSYNTAX2 = objCore.DICOM_LISTENER_SYNTAX_2;
                    strRCVSYNTAX3 = objCore.DICOM_LISTENER_SYNTAX_3;
                    strRCVSYNTAX4 = objCore.DICOM_LISTENER_SYNTAX_4;
                    strENBLLDS = objCore.ENABLE_LISTENING_IN_DISTRIBUTOR_SERVICE;
                    intDSNOOFPORTS = objCore.NUMBER_OF_LISTENING_PORTS_IN_DISTRIBUTOR_SERVICE;

                    bRet = true;
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetServiceDetails() => Core::GetServiceDetails - Error : " + strCatchMessage, true);
                    bRet = false;
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetServiceDetails() - Exception: " + expErr.Message, true);
                bRet = false;
            }
            finally
            {
                objCore = null;
            }
            return bRet;
        }
        #endregion

        #region Receiving Process

        #region doProcessReceive1
        private void doProcessReceive1()
        {
            try
            {
                while (true)
                {
                    ReceiveFiles1();
                    System.Threading.Thread.Sleep(intFreq * 1000);
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessReceive1() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }
        }
        #endregion

        #region ReceiveFiles1
        private void ReceiveFiles1()
        {
            Process ReceivingProc1 = new Process();
            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File receiving started with " + strDCMRCVEXEPATH + " " + strDCMLSNFLDR1, false);

                ReceivingProc1.StartInfo.UseShellExecute = false;
                ReceivingProc1.StartInfo.FileName = strDCMRCVEXEPATH;
                ReceivingProc1.StartInfo.Arguments = strRCVSYNTAX1 + " " + strDCMLSNFLDR1;
                ReceivingProc1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                ReceivingProc1.StartInfo.CreateNoWindow = true;
                ReceivingProc1.StartInfo.RedirectStandardOutput = true;
                ReceivingProc1.StartInfo.RedirectStandardError = true;

                try
                {
                    ReceivingProc1.Start();
                }
                catch (Exception ex)
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "ReceiveFiles1()==>ReceivingProc1==>Exception : " + ex.Message, true);
                }
                finally
                {
                    ReceivingProc1.Close();
                    ReceivingProc1.Dispose();
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "ReceiveFiles1() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);

            }
        } 
        #endregion

        #region doProcessReceive2
        private void doProcessReceive2()
        {
            try
            {
                while (true)
                {
                    ReceiveFiles2();
                    System.Threading.Thread.Sleep(intFreq * 1000);
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessReceive2() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }
        }
        #endregion

        #region ReceiveFiles2
        private void ReceiveFiles2()
        {
            Process ReceivingProc2 = new Process();
            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File receiving started with " + strDCMRCVEXEPATH + " " + strDCMLSNFLDR2, false);

                ReceivingProc2.StartInfo.UseShellExecute = false;
                ReceivingProc2.StartInfo.FileName = strDCMRCVEXEPATH;
                ReceivingProc2.StartInfo.Arguments = strRCVSYNTAX2 + " " + strDCMLSNFLDR2;
                ReceivingProc2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                ReceivingProc2.StartInfo.CreateNoWindow = true;
                ReceivingProc2.StartInfo.RedirectStandardOutput = true;
                ReceivingProc2.StartInfo.RedirectStandardError = true;

                try
                {
                    ReceivingProc2.Start();
                }
                catch (Exception ex)
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "ReceiveFiles2()==>ReceivingProc2==>Exception : " + ex.Message, true);
                }
                finally
                {
                    ReceivingProc2.Close();
                    ReceivingProc2.Dispose();
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "ReceiveFiles2() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);

            }
        }
        #endregion

        #region doProcessReceive3
        private void doProcessReceive3()
        {
            try
            {
                while (true)
                {
                    ReceiveFiles3();
                    System.Threading.Thread.Sleep(intFreq * 1000);
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessReceive3() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }
        }
        #endregion

        #region ReceiveFiles3
        private void ReceiveFiles3()
        {
            Process ReceivingProc3 = new Process();
            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File receiving started with " + strDCMRCVEXEPATH + " " + strDCMLSNFLDR3, false);

                ReceivingProc3.StartInfo.UseShellExecute = false;
                ReceivingProc3.StartInfo.FileName = strDCMRCVEXEPATH;
                ReceivingProc3.StartInfo.Arguments = strRCVSYNTAX3 + " " + strDCMLSNFLDR3;
                ReceivingProc3.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                ReceivingProc3.StartInfo.CreateNoWindow = true;
                ReceivingProc3.StartInfo.RedirectStandardOutput = true;
                ReceivingProc3.StartInfo.RedirectStandardError = true;

                try
                {
                    ReceivingProc3.Start();
                }
                catch (Exception ex)
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "ReceiveFiles3()==>ReceivingProc3==>Exception : " + ex.Message, true);
                }
                finally
                {
                    ReceivingProc3.Close();
                    ReceivingProc3.Dispose();
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "ReceiveFiles3() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);

            }
        }
        #endregion

        #region doProcessReceive4
        private void doProcessReceive4()
        {
            try
            {
                while (true)
                {
                    ReceiveFiles4();
                    System.Threading.Thread.Sleep(intFreq * 1000);
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessReceive4() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }
        }
        #endregion

        #region ReceiveFiles4
        private void ReceiveFiles4()
        {
            Process ReceivingProc4 = new Process();
            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "File receiving started with " + strDCMRCVEXEPATH + " " + strDCMLSNFLDR4, false);

                ReceivingProc4.StartInfo.UseShellExecute = false;
                ReceivingProc4.StartInfo.FileName = strDCMRCVEXEPATH;
                ReceivingProc4.StartInfo.Arguments = strRCVSYNTAX4 + " " + strDCMLSNFLDR4;
                ReceivingProc4.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                ReceivingProc4.StartInfo.CreateNoWindow = true;
                ReceivingProc4.StartInfo.RedirectStandardOutput = true;
                ReceivingProc4.StartInfo.RedirectStandardError = true;

                try
                {
                    ReceivingProc4.Start();
                }
                catch (Exception ex)
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "ReceiveFiles4()==>ReceivingProc4==>Exception : " + ex.Message, true);
                }
                finally
                {
                    ReceivingProc4.Close();
                    ReceivingProc4.Dispose();
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "ReceiveFiles4() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);

            }
        }
        #endregion

        #endregion

        #region Distribution Process

        #region Process Forwarded Files 1

        #region doProcessForwardedFiles1
        private void doProcessForwardedFiles1()
        {
            try
            {

                while (true)
                {
                    ProcessForwardedFiles1();
                    System.Threading.Thread.Sleep(intFreq * 1000);
                }

            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles1() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }


        }
        #endregion

        #region ProcessForwardedFiles1
        private void ProcessForwardedFiles1()
        {
            string[] arrfiles = new string[0];
            string[] pathElements = new string[0];
            string strFileName = string.Empty;
            string strSessionID = string.Empty;
            FileDistribution objFD1 = new FileDistribution();

            try
            {
                arrfiles = Directory.GetFiles(strDCMLSNFLDR1);
                if (arrfiles.Length > 0)
                {
                    strSessionID = "S1DXXX" + DateTime.Now.ToString("MMddyyHHmmss");
                    foreach (string strFile in arrfiles)
                    {
                        if (CoreCommon.IsDicomFile(strFile))
                        {
                            pathElements = strFile.Split('\\');
                            strFileName = pathElements[(pathElements.Length - 1)];
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles1()=>ProcessForwardedFiles1() :: Updating listener file details. File :: " + strFileName, false);

                            objFD1.DICOM_LISTENER_FOLDER   = strDCMLSNFLDR1;
                            objFD1.FILES_ON_HOLD_FOLDER    = strFILESHOLDPATH;
                            objFD1.PACS_ARCHIVE_FOLDER     = strPACSARCHIVEFLDR;
                            objFD1.DICOM_TAG_DUMP_EXE_PATH = strDCMDMPEXEPATH;

                            objFD1.UpdateDownloadedListenerFileRecords(strConfigPath, intServiceID, strSvcName, strFileName, strSessionID);
                        }
                        else
                        {
                            if (File.Exists(strDCMLSNFLDR1 + "\\" + strFileName)) File.Delete(strDCMLSNFLDR1 + "\\" + strFileName);
                        }
                    }
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles11()=>ProcessForwardedFiles1() - Exception: " + expErr.Message, true);
            }
            finally
            {
                objFD1 = null;
            }
        }
        #endregion
        
        #endregion

        #region Process Forwarded Files 2

        #region doProcessForwardedFiles2
        private void doProcessForwardedFiles2()
        {
            try
            {

                while (true)
                {
                    ProcessForwardedFiles2();
                    System.Threading.Thread.Sleep(intFreq * 1000);
                }

            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles2() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }


        }
        #endregion

        #region ProcessForwardedFiles2
        private void ProcessForwardedFiles2()
        {
            string[] arrfiles = new string[0];
            string[] pathElements = new string[0];
            string strFileName = string.Empty;
            string strSessionID = string.Empty;
            FileDistribution objFD2 = new FileDistribution();

            try
            {
                arrfiles = Directory.GetFiles(strDCMLSNFLDR2);
                if (arrfiles.Length > 0)
                {
                    strSessionID = "S1DXXX" + DateTime.Now.ToString("MMddyyHHmmss");
                    foreach (string strFile in arrfiles)
                    {
                        if (CoreCommon.IsDicomFile(strFile))
                        {
                            pathElements = strFile.Split('\\');
                            strFileName = pathElements[(pathElements.Length - 1)];
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles2()=>ProcessForwardedFiles2() :: Updating listener file details. File :: " + strFileName, false);
                            
                            objFD2.DICOM_LISTENER_FOLDER    = strDCMLSNFLDR2;
                            objFD2.FILES_ON_HOLD_FOLDER     = strFILESHOLDPATH;
                            objFD2.PACS_ARCHIVE_FOLDER      = strPACSARCHIVEFLDR;
                            objFD2.DICOM_TAG_DUMP_EXE_PATH  = strDCMDMPEXEPATH;

                            objFD2.UpdateDownloadedListenerFileRecords(strConfigPath, intServiceID, strSvcName, strFileName, strSessionID);
                        }
                        else
                        {
                            if (File.Exists(strDCMLSNFLDR2 + "\\" + strFileName)) File.Delete(strDCMLSNFLDR2 + "\\" + strFileName);
                        }
                    }
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles2()=>ProcessForwardedFiles2() - Exception: " + expErr.Message, true);
            }
            finally
            {
                objFD2 = null;
            }
        }
        #endregion

        #endregion

        #region Process Forwarded Files 3

        #region doProcessForwardedFiles3
        private void doProcessForwardedFiles3()
        {
            try
            {

                while (true)
                {
                    ProcessForwardedFiles3();
                    System.Threading.Thread.Sleep(intFreq * 1000);
                }

            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles3() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }


        }
        #endregion

        #region ProcessForwardedFiles3
        private void ProcessForwardedFiles3()
        {
            string[] arrfiles = new string[0];
            string[] pathElements = new string[0];
            string strFileName = string.Empty;
            string strSessionID = string.Empty;
            FileDistribution objFD3 = new FileDistribution();

            try
            {
                arrfiles = Directory.GetFiles(strDCMLSNFLDR3);
                if (arrfiles.Length > 0)
                {
                    strSessionID = "S1DXXX" + DateTime.Now.ToString("MMddyyHHmmss");
                    foreach (string strFile in arrfiles)
                    {
                        if (CoreCommon.IsDicomFile(strFile))
                        {
                            pathElements = strFile.Split('\\');
                            strFileName = pathElements[(pathElements.Length - 1)];
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles3()=>ProcessForwardedFiles3() :: Updating listener file details. File :: " + strFileName, false);
                           
                            objFD3.DICOM_LISTENER_FOLDER   = strDCMLSNFLDR3;
                            objFD3.FILES_ON_HOLD_FOLDER    = strFILESHOLDPATH;
                            objFD3.PACS_ARCHIVE_FOLDER     = strPACSARCHIVEFLDR;
                            objFD3.DICOM_TAG_DUMP_EXE_PATH = strDCMDMPEXEPATH;

                            objFD3.UpdateDownloadedListenerFileRecords(strConfigPath, intServiceID, strSvcName, strFileName, strSessionID);
                        }
                        else
                        {
                            if (File.Exists(strDCMLSNFLDR3 + "\\" + strFileName)) File.Delete(strDCMLSNFLDR3 + "\\" + strFileName);
                        }
                    }
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles3()=>ProcessForwardedFiles3() - Exception: " + expErr.Message, true);
            }
            finally
            {
                objFD3 = null;
            }
        }
        #endregion

        #endregion

        #region Process Forwarded Files 4

        #region doProcessForwardedFiles4
        private void doProcessForwardedFiles4()
        {
            try
            {

                while (true)
                {
                    ProcessForwardedFiles4();
                    System.Threading.Thread.Sleep(intFreq * 1000);
                }

            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles4() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }


        }
        #endregion

        #region ProcessForwardedFiles4
        private void ProcessForwardedFiles4()
        {
            string[] arrfiles = new string[0];
            string[] pathElements = new string[0];
            string strFileName = string.Empty;
            string strSessionID = string.Empty;
            FileDistribution objFD4 = new FileDistribution();

            try
            {
                arrfiles = Directory.GetFiles(strDCMLSNFLDR4);
                if (arrfiles.Length > 0)
                {
                    strSessionID = "S1DXXX" + DateTime.Now.ToString("MMddyyHHmmss");
                    foreach (string strFile in arrfiles)
                    {
                        if (CoreCommon.IsDicomFile(strFile))
                        {
                            pathElements = strFile.Split('\\');
                            strFileName = pathElements[(pathElements.Length - 1)];
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles4()=>ProcessForwardedFiles4() :: Updating listener file details. File :: " + strFileName, false);

                            objFD4.DICOM_LISTENER_FOLDER   = strDCMLSNFLDR4;
                            objFD4.FILES_ON_HOLD_FOLDER    = strFILESHOLDPATH;
                            objFD4.PACS_ARCHIVE_FOLDER     = strPACSARCHIVEFLDR;
                            objFD4.DICOM_TAG_DUMP_EXE_PATH = strDCMDMPEXEPATH;

                            objFD4.UpdateDownloadedListenerFileRecords(strConfigPath, intServiceID, strSvcName, strFileName, strSessionID);
                        }
                        else
                        {
                            if (File.Exists(strDCMLSNFLDR4 + "\\" + strFileName)) File.Delete(strDCMLSNFLDR4 + "\\" + strFileName);
                        }
                    }
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles4()=>ProcessForwardedFiles4() - Exception: " + expErr.Message, true);
            }
            finally
            {
                objFD4 = null;
            }
        }
        #endregion

        #endregion

        #region Process Unknown Modality Files

        #region doProcessUnknownModalityFiles
        private void doProcessUnknownModalityFiles()
        {
            try
            {

                while (true)
                {
                    ProcessUnknownModalityFiles();
                    System.Threading.Thread.Sleep(intFreq * 1000);
                }

            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles4() - Exception: " + expErr.Message, true);
                System.Threading.Thread.Sleep(intFreq * 1000);
            }


        }
        #endregion

        #region ProcessUnknownModalityFiles
        private void ProcessUnknownModalityFiles()
        {
            string[] arrfiles = new string[0];
            string[] pathElements = new string[0];
            string strFileName = string.Empty;
            string strSessionID = string.Empty;
            FileDistribution objFD5 = new FileDistribution();

            try
            {
                arrfiles = Directory.GetFiles(strDCMLSNFLDR4);
                if (arrfiles.Length > 0)
                {
                    strSessionID = "S1DXXX" + DateTime.Now.ToString("MMddyyHHmmss");
                    foreach (string strFile in arrfiles)
                    {
                        if (CoreCommon.IsDicomFile(strFile))
                        {
                            pathElements = strFile.Split('\\');
                            strFileName = pathElements[(pathElements.Length - 1)];
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles4()=>ProcessForwardedFiles4() :: Updating listener file details. File :: " + strFileName, false);

                            objFD5.DICOM_LISTENER_FOLDER = strUMDCMFILES;
                            objFD5.FILES_ON_HOLD_FOLDER = strFILESHOLDPATH;
                            objFD5.PACS_ARCHIVE_FOLDER = strPACSARCHIVEFLDR;
                            objFD5.DICOM_TAG_DUMP_EXE_PATH = strDCMDMPEXEPATH;

                            objFD5.UpdateDownloadedListenerFileRecords(strConfigPath, intServiceID, strSvcName, strFileName, strSessionID);
                        }
                        else
                        {
                            if (File.Exists(strDCMLSNFLDR4 + "\\" + strFileName)) File.Delete(strDCMLSNFLDR4 + "\\" + strFileName);
                        }
                    }
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "doProcessForwardedFiles4()=>ProcessForwardedFiles4() - Exception: " + expErr.Message, true);
            }
            finally
            {
                objFD5 = null;
            }
        }
        #endregion

        #endregion

        #endregion

        

        
    }
}
