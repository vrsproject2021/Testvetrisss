using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.OleDb;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using IWshRuntimeLibrary;

namespace VETRISSchedulerInstaller.UserControls
{
    public partial class ucUninstallWiz2 : UserControl
    {
        #region Members & Variables
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;
        #endregion

        DataTable dtblServices = null;

        //ServiceInstallationSettings FoServiceInstallationSettings;
        string strShortCutLinkName = string.Empty;
        string strSourcePath = string.Empty;
        string strApplicationName = string.Empty;
        string strTargetPath = string.Empty;

        Int32 intProgressCount = 0;
        Boolean InstallationProcessStarted = false;

        public ucUninstallWiz2()
        {
            InitializeComponent();
        }

        #region ucUninstallWiz2_Load
        private void ucUninstallWiz2_Load(object sender, EventArgs e)
        {
            pbInstall.Minimum = 0;
            pbInstall.Maximum = 130;
            
            strShortCutLinkName = "VETRIS - Data Synch Services";
            strSourcePath = Application.StartupPath + "\\VETRISScheduler";
            strApplicationName = "VETRISScheduler.exe";

           
            InstallationProcessStarted = false;
            StartProcess();
        }
        #endregion

        #region AddServiceDetails
        private void AddServiceDetails(string ServiceName, string ServiceDescription, string ServiceExeName)
        {
            Int32 intServiceId = 0;
            if (dtblServices == null)
            {
                dtblServices = new DataTable("Services");
                dtblServices.Columns.Add("id", Type.GetType("System.String"));
                dtblServices.Columns.Add("name", Type.GetType("System.String"));
                dtblServices.Columns.Add("description", Type.GetType("System.String"));
                dtblServices.Columns.Add("exe_name", Type.GetType("System.String"));
            }
            intServiceId = dtblServices.Rows.Count + 1;

            DataRow drSvc = dtblServices.NewRow();
            drSvc["id"] = Convert.ToString(intServiceId);
            drSvc["name"] = Convert.ToString(ServiceName);
            drSvc["description"] = Convert.ToString(ServiceDescription);
            drSvc["exe_name"] = Convert.ToString(ServiceExeName);
            dtblServices.Rows.Add(drSvc);
        }
        #endregion

        #region StartProcess
        private void StartProcess()
        {
            pbInstall.Value = 0;
            intProgressCount = 1;
            lblProgress.Text = "Getting services";

            AddServiceDetails("VETRIS New Data Synch Service", "VETRIS New Data Synch Service", "VETRISNewDataSynchService");
            AddServiceDetails("VETRIS Data Write Back Service", "VETRIS Write Back Service", "VETRISDataWriteBackService");
            AddServiceDetails("VETRIS Status Synch Service", "VETRIS Status Update Service", "VETRISStatusUpdateService");
            AddServiceDetails("VETRIS Notification Service", "VETRIS Notification Service", "VETRISNotificationService");
            AddServiceDetails("VETRIS Day End Service", "VETRIS Dayend Service", "VETRISDayEndService");
            AddServiceDetails("VETRIS Missing Data Synch Service", "VETRIS Missing Data Synch Service", "VETRISMissingDataSynchService");
            AddServiceDetails("VETRIS FTP & PACS Synch Service", "VETRIS FTP & PACS Synch Service", "VETRISFTPPACSSynchService");
            //AddServiceDetails("VETRIS Accounts Update Service", "VETRIS Accounts Update Service", "VETRISAccountUpdate");

            pbInstall.Value = intProgressCount * 10;//1
            pbInstall.Refresh();

            if (InstallationProcessStarted == false)
            {
                InstallationProcessStarted = true;
                StartInstallationProcess();
            }

          
        }
        #endregion

        #region StartInstallationProcess
        private void StartInstallationProcess()
        {
            strTargetPath = frmMain.InstallPath;

            try
            {
                if (InstallationProcessStarted == true)
                {
                    if (dtblServices != null)
                    {
                        
                        //Uninstall Services
                        foreach (DataRow drService in dtblServices.Rows)
                        {
                            lblProgress.Refresh();
                            lblProgress.Text = "Uninstalling Service " + Convert.ToString(drService["description"]);
                            lblProgress.Refresh();
                            DoUninstallProcess(Convert.ToInt32(drService["id"]));
                            intProgressCount = intProgressCount + 1;
                            pbInstall.Value = intProgressCount * 10;//8
                            pbInstall.Refresh();
                        }

                        //delete other files
                        lblProgress.Refresh();
                        lblProgress.Text = "Removing files & folders...";
                        lblProgress.Refresh();
                        IL.Common.DeleteFilesFromTarget(strTargetPath);
                        intProgressCount = intProgressCount + 1;//9
                        pbInstall.Value = intProgressCount * 10;
                        pbInstall.Refresh();
                        if (Directory.Exists(strTargetPath)) IL.Common.DeleteEmptyFolders(strTargetPath);
                        intProgressCount = intProgressCount + 1;//10
                        pbInstall.Value = intProgressCount * 10;
                        pbInstall.Refresh();

                        //delete desktop shortcut, All program Shortcut 
                        lblProgress.Refresh();
                        lblProgress.Text = "Removing shortcuts...";
                        lblProgress.Refresh();
                        IL.Common.DeleteDeskTopShortcut(strShortCutLinkName);
                        IL.Common.DeleteAllProgramShortcut(strShortCutLinkName);
                        intProgressCount = intProgressCount + 1;//11
                        pbInstall.Value = intProgressCount * 10;
                        pbInstall.Refresh();

                        //delete installation folder
                        lblProgress.Refresh();
                        lblProgress.Text = "Removing installation folder...";
                        lblProgress.Refresh();


                        if (Directory.Exists(strTargetPath))
                        {
                            if (System.IO.Directory.GetDirectories(strTargetPath.Trim()).Length == 0)
                            {
                                if (System.IO.Directory.GetFiles(strTargetPath.Trim()).Length == 0)
                                {
                                    Directory.Delete(strTargetPath.Trim());
                                }
                            }
                            
                        }

                        intProgressCount = intProgressCount + 1;
                        pbInstall.Value = intProgressCount * 10;//12
                        pbInstall.Refresh();

                    }
                    InstallationProcessStarted = false;
                    pbInstall.Value = 0;
                    pbInstall.Minimum = 0;
                    pbInstall.Maximum = 120;

                    //MessageBox.Show("The installation is complete");

                }
            }
            catch (Exception ex)
            {
                frmMain.InstallErr += ex.Message;
            }

            string _Stat = string.Empty;
            int _Screen = 0;
            _Stat = "Uninstall";
            _Screen = 3;
            ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, _Screen);
            IdentityUpdated(this, args);
        }
        #endregion

        #region DoUninstallProcess
        private void DoUninstallProcess(Int32 intServiceId)
        {
            string strServiceName = string.Empty;
            string strServiceDesc = string.Empty;
            string strServiceExeName = string.Empty;
            string strFilePath = string.Empty;
            DataRow drService = dtblServices.Select("id = " + intServiceId)[0];
            strServiceName = Convert.ToString(drService["name"]);
            strServiceDesc = Convert.ToString(drService["description"]);
            strServiceExeName = Convert.ToString(drService["exe_name"]);
            strFilePath = strTargetPath + "\\" + strServiceExeName + ".exe";

            IL.ServiceTools.DeployServices objDeployServices;

            try
            {
                objDeployServices = new IL.ServiceTools.DeployServices();
                objDeployServices.SERVICE_NAME = strServiceName;
                objDeployServices.DO_UNINSTALL = true;
                if (objDeployServices.UninstallServices() == true)
                {
                    string[] arrFiles = new string[3];
                    arrFiles[0] = strTargetPath + "\\" + strServiceExeName + ".exe";
                    arrFiles[2] = strTargetPath + "\\" + strServiceExeName + ".exe.InstallState";
                    IL.Common.DeleteFilesFromList(arrFiles);
                }
            }
            catch { }
            finally { objDeployServices = null; }
        }
        #endregion
    }
}
