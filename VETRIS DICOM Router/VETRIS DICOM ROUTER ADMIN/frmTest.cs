using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Net;
using System.Net.Http;
using System.IO;
using System.IO.Compression;
using System.Configuration;
using System.Security;
using System.Drawing.Imaging;
using Microsoft.VisualBasic;
using System.Windows.Forms;
using VETRISRouter.Core;

namespace VETRIS_DICOM_ROUTER_ADMIN
{
    public partial class frmTest : Form
    {
        private static string strConfigPath = AppDomain.CurrentDomain.BaseDirectory;
        Scheduler objCore;

        public frmTest()
        {
            InitializeComponent();
        }

        private void frmTest_Load(object sender, EventArgs e)
        {
          

        }

      

        #region Update Online Status
        private void button1_Click(object sender, EventArgs e)
        {
            string strCurrentVer = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string strRespMsg = string.Empty;
            string apiUrl = "https://client.vcradiology.com/VETRIS.API/api";
            string json = string.Empty;
            WebClient client = new WebClient();

            try
            {

                object input = new
                {
                    institutionCode = "00037",
                    versionNo = strCurrentVer,
                };
                string inputJson = (new JavaScriptSerializer()).Serialize(input);
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                json = client.UploadString(apiUrl + "/DicomRouterUpdateOnlineStatus", inputJson);

                JavaScriptSerializer ser = new JavaScriptSerializer();
                AppClasses.DicomRouterOnlineStatusResponseDetails resp = ser.Deserialize<AppClasses.DicomRouterOnlineStatusResponseDetails>(json);

                strRespMsg = resp.responseStatus.responseMessage;


                if (strRespMsg.Trim() != "SUCCESS")
                {
                    CoreCommon.doLog(AppDomain.CurrentDomain.BaseDirectory, 2, "Dicom Sending Service", "Y", "doUpdateOnlineStatus() - Error: " + strRespMsg);
                }
                ser = null;

            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(AppDomain.CurrentDomain.BaseDirectory, 2, "Dicom Sending Service", "Y", "doUpdateOnlineStatus() - Exception: " + expErr.Message);

            }
        }
        #endregion


        private void button2_Click(object sender, EventArgs e)
        {
            doMoveFiles();
        }

        #region doMoveFiles
        private void doMoveFiles()
        {
            bool bReturn = false;
            string strCatchMessage = string.Empty;
            string strSNDDIR = "";
            string strFTPABSPATH = "";
            //int exitCode = 0;
            string strExecCommand = string.Empty;
            Process process = new Process();
            ProcessStartInfo ProcMoveFiles = new ProcessStartInfo();
            string strBatchFile = AppDomain.CurrentDomain.BaseDirectory + "MoveDCMFiles.bat";

            try
            {
                objCore = new Scheduler();
                //else arrFiles = Directory.GetFiles(strSNDDIR);
                //strBatchFile = strBatchFile.Replace(" ", "~");

                bReturn = objCore.FetchSchedulerSettings(AppDomain.CurrentDomain.BaseDirectory, ref strCatchMessage);

                if (bReturn)
                {
                    strSNDDIR = objCore.SENDER_DIRECTORY;
                    strFTPABSPATH = objCore.FTP_ABSOLUTE_PATH;
                }


                strBatchFile = "F:\\VCDICOMROUTER\\MoveDCMFiles.bat";

                //ProcessStartInfo ProcMoveFiles = new ProcessStartInfo("cmd.exe",strExecCommand);

                ProcMoveFiles.UseShellExecute = false;
                //ProcMoveFiles.WorkingDirectory = @"C:\Windows\System32";
                strExecCommand = "/c " + strBatchFile + " " + @strSNDDIR + "\\*.* " + @strFTPABSPATH + "\\";
                ProcMoveFiles.FileName = System.Environment.GetEnvironmentVariable("COMSPEC");
                ProcMoveFiles.Arguments = strExecCommand;
                //ProcMoveFiles.Arguments = String.Format("/C {0} {1} {2}", strBatchFile, strSNDDIR + "\\*.*", strFTPABSPATH + "\\");
                //ProcMoveFiles.Arguments = String.Format("{0} {1}", strSNDDIR + "\\*.* ", strFTPABSPATH);
                //ProcMoveFiles.Verb = "runas";
                ProcMoveFiles.WindowStyle = ProcessWindowStyle.Normal;
                ProcMoveFiles.CreateNoWindow = true;
                ProcMoveFiles.RedirectStandardOutput = true;
                ProcMoveFiles.RedirectStandardError = true;

                process.StartInfo = ProcMoveFiles;
                process.Start();
                //process.WaitForExit();
                //exitCode = process.ExitCode;
                //process.Close();
            }
            catch (Exception ex)
            {
                ;
            }
            finally
            {
                objCore = null;
                ProcMoveFiles = null;
                process = null;
            }

        }
        #endregion

        #region btnRecMove_Click
        private void btnRecMove_Click(object sender, EventArgs e)
        {
            string strSID = string.Empty;
            string strPrefix = string.Empty;
            Process process = new Process();
            ProcessStartInfo ProcMoveFiles = new ProcessStartInfo();
            string strBatchFile = @"F:\VCDICOMDATA\AddPrefixAndMoveFiles.bat";
            string strExecCommand = string.Empty;
            string strRand = string.Empty;
            string strSITECODE = "00037";
            string strINSTNAME = "VC RADIOLOGY INC";
            string strCOMPXFERFILE = "N";
            string strSNDDIR = "";
            string strRCVDIR = "";
            bool bReturn = false;
            string strCatchMessage = string.Empty;
            string strSubPrefix = string.Empty;

            objCore = new Scheduler();
            strSID = "S1DXXX" + DateTime.Now.ToString("MMddyyHHmmss");

            try
            {
                
                bReturn = objCore.FetchSchedulerSettings(AppDomain.CurrentDomain.BaseDirectory, ref strCatchMessage);

                if (bReturn)
                {
                    strSNDDIR = objCore.SENDER_DIRECTORY;
                    strRCVDIR = objCore.RECEIVING_DIRECTORY;
                }

                strRand = CoreCommon.RandomString(6);
                strPrefix = strSITECODE + "_" + strSID + "_" + strINSTNAME.Replace(" ", "_") + "_";
                strSubPrefix = strSITECODE + "_S1DXXX";

                if (strCOMPXFERFILE == "Y")
                    strExecCommand = "/c " + strBatchFile + " " + @strRCVDIR + "\\*.zip " + @strSNDDIR + "\\ " + strPrefix + " " + strSubPrefix;
                else
                    strExecCommand = "/c " + strBatchFile + " " + @strRCVDIR + "\\*.* " + @strSNDDIR + "\\ " + strPrefix + " " + strSubPrefix;

                ProcMoveFiles.UseShellExecute = false;
                ProcMoveFiles.FileName = System.Environment.GetEnvironmentVariable("COMSPEC");
                ProcMoveFiles.Arguments = strExecCommand;
                ProcMoveFiles.WindowStyle = ProcessWindowStyle.Hidden;
                ProcMoveFiles.CreateNoWindow=true;
                ProcMoveFiles.RedirectStandardOutput = true;
                ProcMoveFiles.RedirectStandardError = true;

                process.StartInfo = ProcMoveFiles;
                process.Start();

                //process.WaitForExit();
                ////exitCode = process.ExitCode;
                //process.Close();
            }
            catch (Exception ex)
            {
                strCatchMessage = ex.Message;
            }
            finally
            {
                ProcMoveFiles = null;
                process = null;
                objCore = null;
            }
        }
        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            string strFolder = string.Empty;
            strFolder = textBox1.Text;

            //strFolder = @strFolder;
        }

    }
}
