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
using IWshRuntimeLibrary; // for Shortcut

namespace DICOMRouterInstaller.UserControls
{
    public partial class ucInstallWiz4 : UserControl
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

        public ucInstallWiz4()
        {
            InitializeComponent();
        }

        #region ucInstallWiz4_Load
        private void ucInstallWiz4_Load(object sender, EventArgs e)
        {
            

            strAdminShortCutLinkName = "VETRIS DICOM Router Services";
            strFileShortCutLinkName = "VETRIS DICOM Router Upload Files";
            strSourcePath = Application.StartupPath + "\\DicomRouter";
            strApplicationName = "VETRIS DICOM ROUTER ADMIN.exe";
            strFileApplicationName = "VETRIS DICOM ROUTER FILE UPLOAD.exe";



            pbInstall.Minimum = 0;
            pbInstall.Maximum = 70;
            InstallationProcessStarted = false;

            lblProgress.Refresh();
            lblProgress.Text = "Getting Services...";
            lblProgress.Refresh();
            intProgressCount = intProgressCount + 1;//1

            AddServiceDetails("Dicom Receiving Service", "DICOMReceiverService");
            AddServiceDetails("Dicom Sending Service", "DICOMSenderService");

            pbInstall.Value = intProgressCount * 10;
            pbInstall.Refresh();

            //_timer.Interval = 1000;
            //_timer.Tick += Timer_Tick;
            //_timer.Start();

            StartProcess();
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
            if (InstallationProcessStarted == false)
            {
                InstallationProcessStarted = true;
                StartInstallationProcess();
            }

            
        }
        #endregion

        #region Timer_Tick
        void Timer_Tick(object sender, EventArgs e)
        {
           
        }
        #endregion

        #region StartInstallationProcess
        private void StartInstallationProcess()
        {
           string strInstallationPath=string.Empty;

            try
            {
                if (InstallationProcessStarted == true)
                {
                    if (dtblServices != null)
                    {
                        strInstallationPath = frmMain.InstallPath;
                        if (!Directory.Exists(strInstallationPath))
                            Directory.CreateDirectory(strInstallationPath);
                        
                        //copy the files to tergate location.
                        intProgressCount = intProgressCount + 1;//2
                        lblProgress.Refresh();
                        lblProgress.Text = "Copying files...";
                        lblProgress.Refresh();
                        IL.Common.CopyDirectory(strSourcePath, strInstallationPath);

                        intProgressCount = intProgressCount + 1; //6
                        lblProgress.Refresh();
                        lblProgress.Text = "Configuring settings...";
                        lblProgress.Refresh();
                        pbInstall.Value = intProgressCount * 10;
                        pbInstall.Refresh();
                        DoConfigureSettings();
                        intProgressCount = intProgressCount + 1; //7
                        pbInstall.Value = intProgressCount * 10;
                        pbInstall.Refresh();

                        lblProgress.Refresh();
                        lblProgress.Text = "Installing Services...";
                        lblProgress.Refresh();
                        //install the services
                        foreach (DataRow drService in dtblServices.Rows)
                        {
                            intProgressCount = intProgressCount + 1;//4
                            lblProgress.Refresh();
                            lblProgress.Text = "Installing Service " + Convert.ToString(drService["description"]);
                            lblProgress.Refresh();
                            DoInstallationProcess(Convert.ToInt32(drService["id"]),strInstallationPath);
                            pbInstall.Value = intProgressCount * 10;
                            pbInstall.Refresh();
                        }

                        // Create desktop shortcut, All program Shortcut
                        intProgressCount = intProgressCount + 1; //5
                        lblProgress.Refresh();
                        lblProgress.Text = "Creating shortcuts...";
                        lblProgress.Refresh();
                        if (frmMain.CreateAdminShortCut == true)
                        {
                            IL.Common.CreateShortcutToDesktop(strAdminShortCutLinkName, strAdminShortCutLinkName, strInstallationPath + "\\" + strApplicationName);
                            IL.Common.CreateShortcutToAllProgram(strAdminShortCutLinkName, strAdminShortCutLinkName, strInstallationPath + "\\" + strApplicationName);
                        }
                        if (frmMain.CreateFileUploadShortCut == true)
                        {
                            IL.Common.CreateShortcutToDesktop(strFileShortCutLinkName, strFileShortCutLinkName, strInstallationPath + "\\" + strFileApplicationName);
                            IL.Common.CreateShortcutToAllProgram(strFileShortCutLinkName, strFileShortCutLinkName, strInstallationPath + "\\" + strFileApplicationName);
                        }
                        pbInstall.Value = intProgressCount * 10;
                        pbInstall.Refresh();
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
            _Stat = "Install";
            _Screen = 5;
            ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, _Screen);
            IdentityUpdated(this, args);
        }
        #endregion

        #region DoInstallationProcess
        private void DoInstallationProcess(Int32 intServiceId, string InstallationPath)
        {
            string strServiceName = string.Empty;
            string strServiceDesc = string.Empty;
            string strFilePath = string.Empty;
            IL.Service objService = new IL.Service();
            DataRow drService = dtblServices.Select("id = " + intServiceId)[0];
            strServiceName = Convert.ToString(drService["name"]);
            strServiceDesc = Convert.ToString(drService["description"]);
            strFilePath = InstallationPath + "\\" + strServiceName + ".exe";


            if (IL.ServiceTools.ServiceInstaller.ServiceIsInstalled(strServiceName) == false)
            {
                if (IL.ServiceTools.ServiceInstaller.InstallService(strServiceName, strServiceDesc, strFilePath, false) == true)
                {
                    objService.SERVICE_NAME = strServiceName;
                    if (!objService.Start())
                    {
                        frmMain.InstallErr += objService.ERROR.Trim();
                    }
                    
                }
                else
                {

                }
            }

            objService = null;

        }
        #endregion

        #region DoConfigureSettings (Suspended)
        //private void DoConfigureSettings()
        //{
        //    string strInstallPath = frmMain.InstallPath;
        //    string strDBPath = strInstallPath + "\\Configs\\DicomDB.accdb";
        //    string strConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strDBPath;
        //    bool bReturn = false;
        //    string strCatchMessage = string.Empty;
            
        //    string strControlCode = string.Empty;
        //    string strControlValue = string.Empty;

        //    if (Directory.Exists(strInstallPath))
        //    {
        //        #region adding permission to installed folder
        //        DirectoryInfo dInfo = new DirectoryInfo(strInstallPath);
        //        DirectorySecurity dSecurity = dInfo.GetAccessControl();
        //        dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
        //        dInfo.SetAccessControl(dSecurity);
        //        #endregion

        //        foreach (DataRow dr in frmMain.dtbl.Rows)
        //        {
        //            strControlCode = Convert.ToString(dr["control_code"]).Trim();
        //            strControlValue = Convert.ToString(dr["control_value"]).Trim();

        //            switch (strControlCode)
        //            {
        //                case "ACCESSORYDIR":
        //                    bReturn = SaveSettings(strConn, "ACCESSORYDIR", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "Accessory Directory :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "INSTNAME":
        //                    bReturn = SaveSettings(strConn, "INSTNAME", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "Institution Name :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "INSTADDR1":
        //                    bReturn = SaveSettings(strConn, "INSTADDR1", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "Institution Address Line 1 :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "INSTADDR2":
        //                    bReturn = SaveSettings(strConn, "INSTADDR2", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "Institution Address Line 2 :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "INSTZIP":
        //                    bReturn = SaveSettings(strConn, "INSTZIP", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "Zip :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "SITECODE":
        //                    bReturn = SaveSettings(strConn, "SITECODE", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "Site Code :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "RCVDIR":
        //                    bReturn = SaveSettings(strConn, "RCVDIR", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "Receiving Folder Path (Default) :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "RCVDIRMANUAL":
        //                    bReturn = SaveSettings(strConn, "RCVDIRMANUAL", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "Receiving Folder Path (for files uploaded manually) :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "MANUALUPLDAUTO":
        //                    bReturn = SaveSettings(strConn, "MANUALUPLDAUTO", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "Receiving Folder Path (for files uploaded manually) - Detect Automatically :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "IMGMNLUPLDAUTO":
        //                    bReturn = SaveSettings(strConn, "IMGMNLUPLDAUTO", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "Receiving Folder Path (for image files uploaded manually) - Detect Automatically :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "RCVIMGDIR":
        //                    bReturn = SaveSettings(strConn, "RCVIMGDIR", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "Receiving Folder Path (for image files uploaded manually) :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "RCVAETITLE":
        //                    bReturn = SaveSettings(strConn, "RCVAETITLE", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "Receiver AE Title :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "RCVPORTNO":
        //                    bReturn = SaveSettings(strConn, "RCVPORTNO", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "Receiver Port No. :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "RCVEXEOPTIONS":
        //                    bReturn = SaveSettings(strConn, "RCVEXEOPTIONS", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "Receiver exe options :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "SNDDIR":
        //                    bReturn = SaveSettings(strConn, "SNDDIR", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "Sending Folder Path :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "ARCHDIR":
        //                    bReturn = SaveSettings(strConn, "ARCHDIR", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "Archive Folder Path (files to be archived after sending) :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "SNDAETITLE":
        //                    bReturn = SaveSettings(strConn, "SNDAETITLE", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "Sender AE Title :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "SNDPORTNO":
        //                    bReturn = SaveSettings(strConn, "SNDPORTNO", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "Sender Port No. :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "SNDEXEOPTIONS":
        //                    bReturn = SaveSettings(strConn, "SNDEXEOPTIONS", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "Sender Exe Options :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "VETLOGIN":
        //                    bReturn = SaveSettings(strConn, "VETLOGIN", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "VETRIS Login ID :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //                case "VETURL":
        //                    bReturn = SaveSettings(strConn, "VETURL", strControlValue, ref strCatchMessage);
        //                    if (!bReturn) { frmMain.InstallErr += "VETRIS URL :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
        //                    break;
        //            }
        //        }


        //    }

        //}
        #endregion

        #region DoConfigureSettings
        private void DoConfigureSettings()
        {
            string strInstallPath = frmMain.InstallPath;
            string strDBPath = strInstallPath + "\\Configs\\Config.xml";
            //string strConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strDBPath;
            string strCatchMessage = string.Empty;

            string strControlCode = string.Empty;
            string strControlValue = string.Empty;

            if (Directory.Exists(strInstallPath))
            {
                #region adding permission to installed folder
                DirectoryInfo dInfo = new DirectoryInfo(strInstallPath);
                DirectorySecurity dSecurity = dInfo.GetAccessControl();
                dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                dInfo.SetAccessControl(dSecurity);
                #endregion

                frmMain.dtbl.WriteXml(strDBPath);

            }

        }
        #endregion

        #region SaveSettings
        public bool SaveSettings(string strConn, string strControlCode, string strControlValue, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; string ReturnMessage = string.Empty;
            string sqlQuery = string.Empty;

            OleDbConnection con = new OleDbConnection(strConn);

            try
            {

                sqlQuery = "update sys_scheduler_settings set control_value = @control_value where control_code =@control_code";

                using (OleDbCommand cmd = new OleDbCommand(sqlQuery, con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("control_value", strControlValue);
                    cmd.Parameters.AddWithValue("control_code", strControlCode);


                    intExecReturn = cmd.ExecuteNonQuery();
                    //con.Close();

                    if (intExecReturn != 0) bReturn = true;
                    else bReturn = false;

                }


            }
            catch (Exception ex)
            {
                CatchMessage = ex.Message;
                bReturn = false;
            }
            finally
            {
                con.Close();
            }
            return bReturn;
        }
        #endregion
    }
}
