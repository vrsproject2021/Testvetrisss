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
    public partial class ucInstallWiz2 : UserControl
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

        public ucInstallWiz2()
        {
            InitializeComponent();
        }

        #region ucInstallWiz2_Load
        private void ucInstallWiz2_Load(object sender, EventArgs e)
        {
           

            pbInstall.Minimum = 0;
            pbInstall.Maximum = 110;

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
           
            strTargetPath = frmMain.InstallPath + "\\VETRISScheduler";

            try
            {
                if (InstallationProcessStarted == true)
                {
                    if (dtblServices != null)
                    {
                        
                        //copy the files to tergate location.
                        intProgressCount = intProgressCount + 1;//2
                        lblProgress.Refresh();
                        lblProgress.Text = "Copying files...";
                        lblProgress.Refresh();
                        IL.Common.CopyDirectory(strSourcePath, strTargetPath);
                        pbInstall.Value = intProgressCount * 10;
                        pbInstall.Refresh();

                        intProgressCount = intProgressCount + 1;//3
                        lblProgress.Refresh();
                        lblProgress.Text = "Installing Services...";
                        lblProgress.Refresh();
                        pbInstall.Value = intProgressCount * 10;
                        pbInstall.Refresh();

                        //install the services
                        foreach (DataRow drService in dtblServices.Rows)
                        {
                            intProgressCount = intProgressCount + 1;
                            lblProgress.Refresh();
                            lblProgress.Text = "Installing Service " + Convert.ToString(drService["description"]);
                            lblProgress.Refresh();
                            DoInstallationProcess(Convert.ToInt32(drService["id"]));
                            pbInstall.Value = intProgressCount * 10;//10
                            pbInstall.Refresh();
                        }

                        // Create desktop shortcut, All program Shortcut
                        intProgressCount = intProgressCount + 1;//11
                        lblProgress.Refresh();
                        lblProgress.Text = "Creating shortcut...";
                        lblProgress.Refresh();
                        IL.Common.CreateShortcutToDesktop(strShortCutLinkName, strShortCutLinkName, strTargetPath + "\\" + strApplicationName);
                        IL.Common.CreateShortcutToAllProgram(strShortCutLinkName, strShortCutLinkName, strTargetPath + "\\" + strApplicationName);
                        pbInstall.Value = intProgressCount * 10;
                        pbInstall.Refresh();
                       
                    }
                   
                    InstallationProcessStarted = false;
                    pbInstall.Value = 0;
                    pbInstall.Minimum = 0;
                    pbInstall.Maximum = 110;

                    //MessageBox.Show("The installation is complete");

                }
            }
            catch (Exception ex)
            {
                frmMain.InstallErr += ex.Message;
            }

            string _Stat = string.Empty;
            int _Screen = 0;
            _Stat = "Install";
            _Screen = 3;
            ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, _Screen);
            IdentityUpdated(this, args);
        }
        #endregion

        #region DoInstallationProcess
        private void DoInstallationProcess(Int32 intServiceId)
        {
            string strServiceName = string.Empty;
            string strServiceDesc = string.Empty;
            string strServiceExeName = string.Empty;
            string strFilePath = string.Empty;
            DataRow drService = dtblServices.Select("id = " + intServiceId)[0];
            strServiceName = Convert.ToString(drService["name"]);
            strServiceDesc = Convert.ToString(drService["description"]);
            strServiceExeName = Convert.ToString(drService["exe_name"]);
            strFilePath =  strTargetPath + "\\" + strServiceExeName + ".exe";

            if (IL.ServiceTools.ServiceInstaller.ServiceIsInstalled(strServiceName) == false)
            {
                if (IL.ServiceTools.ServiceInstaller.InstallService(strServiceName, strServiceDesc, strFilePath, false) == true)
                {

                }
                else
                {

                }
            }

        }
        #endregion

    }
}
