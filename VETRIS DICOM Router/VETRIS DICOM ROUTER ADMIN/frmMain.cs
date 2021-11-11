using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using VETRISRouter.Core;

namespace VETRIS_DICOM_ROUTER_ADMIN
{
    public partial class frmMain : Form
    {
        private static string FTPHOST = string.Empty;
        private static string FTPPORT = string.Empty;
        private static string FTPUSER = string.Empty;
        private static string FTPPWD = string.Empty;
        private static string DRSDWLFLDR = string.Empty;
        private static string VETAPIURL = string.Empty;
        private static string VETURL = string.Empty;
        private static string VETLOGINURL = string.Empty;
        private static string ARCHDIR = string.Empty;

        bool toggleLight = true;
        Timer t = new Timer();
        int tickCount = 0;
        string LatestVer = string.Empty;

        #region Variables
        private static string LoginID = string.Empty;
        private static string InstitutionCode = string.Empty;
        private static string ImportSessionID = string.Empty;
        private static int FileCount = 0;
        private static int MenuID = 0;
        #endregion

        #region Properties
        public static int FILE_COUNT
        {
            get { return FileCount; }
            set { FileCount = value; }
        }
        public static string VETRIS_URL
        {
            get { return VETURL; }
            set { VETURL = value; }
        }
        public static string VETRIS_LOGIN_ID
        {
            get { return LoginID; }
            set { LoginID = value; }
        }
        public static string INSTITUTION_CODE
        {
            get { return InstitutionCode; }
            set { InstitutionCode = value; }
        }
        public static string IMPORT_SESSION_ID
        {
            get { return ImportSessionID; }
            set { ImportSessionID = value; }
        }
        public static string VETRIS_API_URL
        {
            get { return VETAPIURL; }
            set { VETAPIURL = value; }
        }
        public static int MENU_ID
        {
            get { return MenuID; }
            set { MenuID = value; }
        }
        #endregion


        public frmMain()
        {
            InitializeComponent();
        }

        #region frmMain_Load
        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text = "VETRIS DICOM ROUTER"; 
            lblVer.Text = "Version : " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            GetSettings();
        }
        #endregion

        #region frmMain_Shown
        private void frmMain_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            GetVersion();
            Application.DoEvents();
        }
        #endregion

        #region GetSettings
        private void GetSettings()
        {
            string strCatchMessage = string.Empty;
            Scheduler objCore = new Scheduler();

            try
            {
                if (objCore.FetchSchedulerSettings(Application.StartupPath, ref strCatchMessage))
                {
                    FTPHOST = objCore.FTP_HOST_NAME;
                    FTPUSER = objCore.FTP_USER_NAME;
                    FTPPWD = objCore.FTP_PASSWORD;
                    VETAPIURL = objCore.VETRIS_API_URL;
                    VETURL = objCore.VETRIS_URL;
                    VETLOGINURL = objCore.VETRIS_LOGIN_URL;
                    DRSDWLFLDR = objCore.FTP_DICOM_ROUTER_DOWNLOAD_FOLDER;
                    ARCHDIR = objCore.ARCHIVE_DIRECTORY;
                }
                else
                    MessageBox.Show(strCatchMessage, this.Text + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text + " : Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                objCore = null;
            }
        }
        #endregion

        #region GetVersion
        private void GetVersion()
        {
            string strCurrentVer = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string strRespMsg = string.Empty;
            string apiUrl = VETAPIURL;
            string json = string.Empty;
            WebClient client = new WebClient();

            try
            {
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                json = client.UploadString(apiUrl + "/DicomRouterLatestVersion", string.Empty);

                JavaScriptSerializer ser = new JavaScriptSerializer();
                AppClasses.DicomRouterLatestVersion Ver = ser.Deserialize<AppClasses.DicomRouterLatestVersion>(json);

                strRespMsg = Ver.responseStatus.responseMessage;


                if (strRespMsg.Trim() == "SUCCESS")
                {
                    LatestVer = Ver.LatestVersion;
                    btnDownload.Text = btnDownload.Text + " " + LatestVer;

                    if (LatestVer != strCurrentVer)
                    {
                        //btnDownload.Visible = true;
                        //t.Interval = 100;
                        //t.Tick += new EventHandler(t_Tick);
                        //t.Start();

                        frmDownload frmDL = new frmDownload();
                        frmDL.LATEST_VERSION = LatestVer;
                        frmDL.ARCHIVE_DIRECTORY = ARCHDIR;
                        frmDL.VETRIS_URL = VETURL;
                        frmDL.ShowDialog();
                    }
                    else
                        LatestVer = strCurrentVer;
                }
                else
                {
                    MessageBox.Show(strRespMsg, this.Text + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text + " : Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                client.Dispose();
            }
        }
        #endregion

        #region t_Tick
        private void t_Tick(object sender, EventArgs e)
        {
            tickCount = tickCount + 1;
            if (tickCount <= 50)
            {
                if (toggleLight)
                {
                    btnDownload.BackColor = Color.Gold;
                    toggleLight = false;
                }
                else
                {
                    btnDownload.BackColor = Color.Wheat;
                    toggleLight = true;
                }
            }
            else
            {
                tickCount = 0;
                btnDownload.BackColor = Color.Gold;
                t.Stop();
            }
        }
        #endregion

        #region btnDownload_Click
        private void btnDownload_Click(object sender, EventArgs e)
        {
            frmDownload frmDL = new frmDownload();
            frmDL.LATEST_VERSION = LatestVer;
            frmDL.ARCHIVE_DIRECTORY = ARCHDIR;
            frmDL.VETRIS_URL = VETURL;
            frmDL.ShowDialog();
        }
        #endregion

        #region btnCheckConn_Click
        private void btnCheckConn_Click(object sender, EventArgs e)
        {
            frmConnectivity frmConn = new frmConnectivity();
            frmConn.FTP_HOST= FTPHOST;
            frmConn.FTP_USER_ID = FTPUSER;
            frmConn.FTP_PASSWORD = FTPPWD;
            frmConn.ShowDialog();
        } 
        #endregion

        #region btnSchedulerSetting_Click
        private void btnSchedulerSetting_Click(object sender, EventArgs e)
        {

            //UserControls.ucSettings ucSettings = new UserControls.ucSettings();
            //ucSettings.Dock = DockStyle.Fill;
            //ucSettings.IdentityUpdated += new UserControls.ucSettings.IdentityUpdateHandler(Cancel_ButtonClicked);
            //if (pnlAction.Controls.Count > 0) pnlAction.Controls.RemoveAt(0);
            //pnlAction.Controls.Add(ucSettings);

            UserControls.ucConfig ucConfig = new UserControls.ucConfig();
            ucConfig.Dock = DockStyle.Fill;
            ucConfig.IdentityUpdated += new UserControls.ucConfig.IdentityUpdateHandler(Cancel_ButtonClicked);
            if (pnlAction.Controls.Count > 0) pnlAction.Controls.RemoveAt(0);
            pnlAction.Controls.Add(ucConfig);
        }
        #endregion

        #region btnStartStopSvc_Click
        private void btnStartStopSvc_Click(object sender, EventArgs e)
        {
            UserControls.ucService ucService = new UserControls.ucService();
            ucService.Dock = DockStyle.Fill;
            ucService.IdentityUpdated += new UserControls.ucService.IdentityUpdateHandler(Cancel_ButtonClicked);
            if (pnlAction.Controls.Count > 0) pnlAction.Controls.RemoveAt(0);
            pnlAction.Controls.Add(ucService);
        }
        #endregion

        #region btnViewLog_Click
        private void btnViewLog_Click(object sender, EventArgs e)
        {
           
            UserControls.ucViewLog ucViewLog = new UserControls.ucViewLog();
            ucViewLog.Dock = DockStyle.Fill;
            ucViewLog.IdentityUpdated += new UserControls.ucViewLog.IdentityUpdateHandler(Cancel_ButtonClicked);
            if (pnlAction.Controls.Count > 0) pnlAction.Controls.RemoveAt(0);
            pnlAction.Controls.Add(ucViewLog);
        }
        #endregion

        #region btnExit_Click
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Cancel_ButtonClicked
        private void Cancel_ButtonClicked(object sender, ApplicationDelegateEventArgs e)
        {
            if (e.Status.ToString() == "Cancel")
            {
                if (pnlAction.Controls.Count > 0)
                {
                    pnlAction.Controls.RemoveAt(0);

                }
            }
            

        }
        #endregion

        #region frmMain_FormClosed
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {

            Application.Exit();
        }

        #endregion


        
    }
}
