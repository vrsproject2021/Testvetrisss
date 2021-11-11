using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows;
using IWshRuntimeLibrary;

namespace DICOMRouterInstaller.UserControls
{
    public partial class ucUninstallWiz2 : UserControl
    {
        #region Members & Variables
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;
        #endregion

        Timer _timer = new Timer();
        DataTable dtblServices = null;

        //ServiceInstallationSettings FoServiceInstallationSettings;
        string strAdminShortCutLinkName = string.Empty;
        string strFileShortCutLinkName = string.Empty;
        string strSourcePath = string.Empty;
        string strApplicationName = string.Empty;
        string strFileApplicationName = string.Empty;

        Int32 intProgressCount = 0;
        Boolean InstallationProcessStarted = false;


        public ucUninstallWiz2()
        {
            InitializeComponent();
        }

        #region ucUninstallWiz2_Load
        private void ucUninstallWiz2_Load(object sender, EventArgs e)
        {
            AddServiceDetails("Dicom Receiving Service", "DICOMReceiverService");
            AddServiceDetails("Dicom Sending Service", "DICOMSenderService");

            strAdminShortCutLinkName = "VETRIS DICOM Router Services";
            strFileShortCutLinkName = "VETRIS DICOM Router Upload Files";
            strSourcePath = Application.StartupPath + "\\DicomRouter";
            strApplicationName = "VETRIS DICOM ROUTER ADMIN.exe";
            strFileApplicationName = "VETRIS DICOM ROUTER FILE UPLOAD.exe";

            pbInstall.Minimum = 0;
            pbInstall.Maximum = 70;
            InstallationProcessStarted = false;

            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;
            _timer.Start();
        } 
        #endregion

        #region AddServiceDetails
        private void AddServiceDetails(string ServiceDescription, string ServiceName)
        {
            Int32 intServiceId = 0;
            if (dtblServices == null)
            {
                dtblServices = new DataTable("Services");
                dtblServices.Columns.Add("id", Type.GetType("System.String"));
                dtblServices.Columns.Add("name", Type.GetType("System.String"));
                dtblServices.Columns.Add("description", Type.GetType("System.String"));
            }
            intServiceId = dtblServices.Rows.Count + 1;

            DataRow drSvc = dtblServices.NewRow();
            drSvc["id"] = Convert.ToString(intServiceId);
            drSvc["name"] = Convert.ToString(ServiceName);
            drSvc["description"] = Convert.ToString(ServiceDescription);
            dtblServices.Rows.Add(drSvc);
        }
        #endregion

        #region StartProcess
        private void StartProcess()
        {
            pbInstall.Value = 0;
            _timer.Interval = 1000;
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }
        #endregion

        #region Timer_Tick
        void Timer_Tick(object sender, EventArgs e)
        {
            if (InstallationProcessStarted == false)
            {
                InstallationProcessStarted = true;
                StartInstallationProcess();
            }

            pbInstall.Value = intProgressCount * 10;
        }
        #endregion

        #region StartInstallationProcess
        private void StartInstallationProcess()
        {
            try
            {
                if (InstallationProcessStarted == true)
                {
                    if (dtblServices != null)
                    {
                        intProgressCount = 0;
                        //Uninstall Services
                        
                       
                        foreach (DataRow drService in dtblServices.Rows)
                        {
                            lblProgress.Refresh();
                            lblProgress.Text = "Uninstalling Service " + Convert.ToString(drService["description"]);
                            lblProgress.Refresh();
                            DoUninstallProcess(Convert.ToInt32(drService["id"]));
                            intProgressCount = intProgressCount + 1;
                            pbInstall.Value = intProgressCount * 10;
                        }

                        //delete other files
                        lblProgress.Refresh();
                        lblProgress.Text = "Removing files & folders...";
                        lblProgress.Refresh();
                        //IL.Common.DeleteFilesFromTarget(frmMain.InstallPath);

                       
                        foreach (var process in Process.GetProcessesByName("storescp"))
                        {
                            process.Kill();
                        }
                        foreach (var process in Process.GetProcessesByName("DICOMReceiverService"))
                        {
                            process.Kill();
                        }
                        foreach (var process in Process.GetProcessesByName("DICOMSenderService"))
                        {
                            process.Kill();
                        }

                        System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(frmMain.InstallPath);
                        DirectorySecurity dSecurity = dInfo.GetAccessControl();
                        dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                        dInfo.SetAccessControl(dSecurity);
                        IL.Common.DeleteInstallationFolder(dInfo,true);

                        intProgressCount = intProgressCount + 1;
                        pbInstall.Value = intProgressCount * 10;
                        //IL.Common.DeleteEmptyFolders(frmMain.InstallPath);
                        intProgressCount = intProgressCount + 1;
                        pbInstall.Value = intProgressCount * 10;

                        //delete desktop shortcut, All program Shortcut 
                        lblProgress.Refresh();
                        lblProgress.Text = "Removing shortcuts...";
                        lblProgress.Refresh();
                        IL.Common.DeleteDeskTopShortcut(strAdminShortCutLinkName);
                        IL.Common.DeleteAllProgramShortcut(strAdminShortCutLinkName);
                        IL.Common.DeleteDeskTopShortcut(strFileShortCutLinkName);
                        IL.Common.DeleteAllProgramShortcut(strFileShortCutLinkName);
                        intProgressCount = intProgressCount + 1;
                        pbInstall.Value = intProgressCount * 10;

                        //delete installation folder
                        //lblProgress.Refresh();
                        //lblProgress.Text = "Removing installation folder...";
                        //lblProgress.Refresh();

                        //if (Directory.Exists(frmMain.InstallPath.Trim()))
                        //{
                        //    if (System.IO.Directory.GetDirectories(frmMain.InstallPath.Trim()).Length == 0)
                        //    {
                        //        if (System.IO.Directory.GetFiles(frmMain.InstallPath.Trim()).Length == 0)
                        //        {
                        //            Directory.Delete(frmMain.InstallPath.Trim());
                        //        }
                        //    }
                        //    else if (System.IO.Directory.GetDirectories(frmMain.InstallPath.Trim()).Length > 0)
                        //    {
                        //        IL.Common.DeleteFilesFromTarget(frmMain.InstallPath);
                        //    }
                        //}

                        //if (Directory.Exists(frmMain.InstallPath.Trim()))
                        //{
                        //    if (System.IO.Directory.GetDirectories(frmMain.InstallPath.Trim()).Length == 0)
                        //    {
                        //        if (System.IO.Directory.GetFiles(frmMain.InstallPath.Trim()).Length == 0)
                        //        {
                        //            Directory.Delete(frmMain.InstallPath.Trim());
                        //        }
                        //    }
                        //}

                        intProgressCount = 7;
                        pbInstall.Value = intProgressCount * 10;

                    }
                    _timer.Stop();
                    InstallationProcessStarted = false;
                    pbInstall.Value = 0;
                    pbInstall.Minimum = 0;
                    pbInstall.Maximum = 70;

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
            string strFilePath = string.Empty;
            DataRow drService = dtblServices.Select("id = " + intServiceId)[0];
            strServiceName = Convert.ToString(drService["name"]);
            strServiceDesc = Convert.ToString(drService["description"]);
            strFilePath = frmMain.InstallPath + "\\" + strServiceName + ".exe";

            IL.ServiceTools.DeployServices objDeployServices;

            try
            {
                objDeployServices = new IL.ServiceTools.DeployServices();
                objDeployServices.SERVICE_NAME = strServiceName;
                objDeployServices.DO_UNINSTALL = true;
                if (objDeployServices.UninstallServices() == true)
                {
                    string[] arrFiles = new string[3];
                    arrFiles[0] = frmMain.InstallPath + "\\" + strServiceName + ".exe";
                    arrFiles[2] = frmMain.InstallPath + "\\" + strServiceName + ".exe.InstallState";
                    IL.Common.DeleteFilesFromList(arrFiles);
                }
            }
            catch { }
            finally { objDeployServices = null; }
        }
        #endregion
    }
}
