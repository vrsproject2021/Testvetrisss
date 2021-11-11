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
using IWshRuntimeLibrary;

namespace DICOMRouterInstaller.UserControls
{
    public partial class ucUpdateWiz2 : UserControl
    {
        #region Members & Variables
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;
        #endregion

        Timer _timer = new Timer();
        DataTable dtblServices = null;
        string strAdminShortCutLinkName = string.Empty;
        string strFileShortCutLinkName = string.Empty;
        string strSourcePath = string.Empty;
        string strApplicationName = string.Empty;
        string strFileApplicationName = string.Empty;
        string strDBPath = string.Empty;

        Int32 intProgressCount = 0;
        Boolean InstallationProcessStarted = false;

        public ucUpdateWiz2()
        {
            InitializeComponent();
        }

        #region ucUpdateWiz2_Load
        private void ucUpdateWiz2_Load(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Installation Path : " + frmMain.InstallPath);
            txtProcess.Text += sb.ToString();
            sb.Clear();

            if (frmMain.InstallPath.Trim() == string.Empty)
            {
                frmMain.CreateAdminShortCut = true;
                frmMain.CreateFileUploadShortCut = true;

                lblProgress.Refresh();
                lblProgress.Text = "Stopping Service(s)...";
                lblProgress.Refresh();

                CheckExistingInstallation();
            }

            lblProgress.Refresh();
            lblProgress.Text = "Getting Service(s)...";
            lblProgress.Refresh();

            AddServiceDetails("Dicom Receiving Service", "DICOMReceiverService");
            AddServiceDetails("Dicom Sending Service", "DICOMSenderService");

            strAdminShortCutLinkName = "VETRIS DICOM Router Services";
            strFileShortCutLinkName = "VETRIS DICOM Router Upload Files";
            strSourcePath = Application.StartupPath + "\\DicomRouter";
            strApplicationName = "VETRIS DICOM ROUTER ADMIN.exe";
            strFileApplicationName = "VETRIS DICOM ROUTER FILE UPLOAD.exe";

            pbInstall.Minimum = 0;
            pbInstall.Maximum = 120;
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
            intProgressCount = 1;
            if (InstallationProcessStarted == false)
            {
                InstallationProcessStarted = true;
                StartInstallationProcess();
            }

            pbInstall.Value = intProgressCount * 10;
            pbInstall.Refresh();
        }
        #endregion

        #region StartInstallationProcess
        private void StartInstallationProcess()
        {
            string strRegion = string.Empty;
            StringBuilder sb = new StringBuilder();

            try
            {
                if (InstallationProcessStarted == true)
                {
                    if (dtblServices != null)
                    {
                        #region Backup DB
                        strRegion = "Backing up DB....";

                        lblProgress.Refresh();
                        lblProgress.Text = "Backing up database ...";
                        lblProgress.Refresh();

                        sb.AppendLine("Backing up DB....");
                        txtProcess.Text += sb.ToString();
                        sb.Clear();

                        intProgressCount = intProgressCount + 1;
                        strDBPath = frmMain.InstallPath + "\\Configs";
                        sb.AppendLine("Installation Path : " + frmMain.InstallPath);
                        txtProcess.Text += sb.ToString();
                        sb.Clear();

                        if (!System.IO.Directory.Exists(Application.StartupPath + "\\Temp"))
                        {
                            System.IO.Directory.CreateDirectory(Application.StartupPath + "\\Temp");
                        }

                        if (System.IO.File.Exists(Application.StartupPath + "\\Temp\\Config.xml"))
                        {
                            System.IO.File.Delete(Application.StartupPath + "\\Temp\\Config.xml");
                        }
                        if (System.IO.File.Exists(Application.StartupPath + "\\Temp\\DRLog.xml"))
                        {
                            System.IO.File.Delete(Application.StartupPath + "\\Temp\\DRLog.xml");
                        }
                        if (System.IO.File.Exists(Application.StartupPath + "\\Temp\\DRLicense.lic"))
                        {
                            System.IO.File.Delete(Application.StartupPath + "\\Temp\\DRLicense.lic");
                        }

                        System.IO.File.Copy(strDBPath + "\\Config.xml", Application.StartupPath + "\\Temp\\Config.xml");
                        sb.AppendLine("Copied " + strDBPath + "\\Config.xml to " + Application.StartupPath + "\\Temp\\Config.xml");
                        txtProcess.Text += sb.ToString();
                        sb.Clear();

                        if (System.IO.File.Exists(frmMain.InstallPath + "\\DRLog.xml")) System.IO.File.Copy(frmMain.InstallPath + "\\DRLog.xml", Application.StartupPath + "\\Temp\\DRLog.xml");
                        if (System.IO.File.Exists(frmMain.InstallPath + "\\DRLicense.lic")) System.IO.File.Copy(frmMain.InstallPath + "\\DRLicense.lic", Application.StartupPath + "\\Temp\\DRLicense.lic");
                        pbInstall.Value = intProgressCount * 10;
                        pbInstall.Refresh();//2
                        #endregion

                        #region Uninstall

                        strRegion = "Uninstalling Router....";

                        #region Uninstall Services
                        foreach (DataRow drService in dtblServices.Rows)
                        {
                            sb.AppendLine("Uninstalling Service " + Convert.ToString(drService["description"]));
                            txtProcess.Text += sb.ToString();
                            sb.Clear();

                            lblProgress.Refresh();
                            lblProgress.Text = "Uninstalling Service " + Convert.ToString(drService["description"]);
                            lblProgress.Refresh();
                            DoUninstallProcess(Convert.ToInt32(drService["id"]));
                            intProgressCount = intProgressCount + 1;//4
                            pbInstall.Value = intProgressCount * 10;
                            pbInstall.Refresh();
                        }
                        #endregion

                        #region delete other files
                        lblProgress.Refresh();
                        lblProgress.Text = "Removing files & folders...";
                        lblProgress.Refresh();
                        //IL.Common.DeleteFilesFromTarget(frmMain.InstallPath);
                        #endregion

                        #region Kill background processes
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
                        #endregion

                        System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(frmMain.InstallPath);
                        DirectorySecurity dSecurity = dInfo.GetAccessControl();
                        dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                        dInfo.SetAccessControl(dSecurity);

                        sb.AppendLine("Deleting Installation Folder " + dInfo.FullName);
                        txtProcess.Text += sb.ToString();
                        sb.Clear();

                        IL.Common.DeleteInstallationFolder(dInfo, true);
                        intProgressCount = intProgressCount + 1;//5
                        pbInstall.Value = intProgressCount * 10;
                        // IL.Common.DeleteEmptyFolders(frmMain.InstallPath);
                        intProgressCount = intProgressCount + 1;//6
                        pbInstall.Value = intProgressCount * 10;
                        pbInstall.Refresh();

                        //delete desktop shortcut, All program Shortcut 
                        lblProgress.Refresh();
                        lblProgress.Text = "Removing shortcuts...";
                        lblProgress.Refresh();

                        sb.AppendLine("Deleting Shortcuts");
                        txtProcess.Text += sb.ToString();
                        sb.Clear();
                        IL.Common.DeleteDeskTopShortcut(strAdminShortCutLinkName);
                        IL.Common.DeleteAllProgramShortcut(strAdminShortCutLinkName);
                        IL.Common.DeleteDeskTopShortcut(strFileShortCutLinkName);
                        IL.Common.DeleteAllProgramShortcut(strFileShortCutLinkName);
                        intProgressCount = intProgressCount + 1;//7
                        pbInstall.Value = intProgressCount * 10;
                        pbInstall.Refresh();

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
                        //}

                        intProgressCount = intProgressCount + 1;//8
                        pbInstall.Value = intProgressCount * 10;
                        pbInstall.Refresh();
                        #endregion

                        #region Install

                        strRegion = "Installing Router....";

                        sb.AppendLine("Creating Folder : " + frmMain.InstallPath);
                        txtProcess.Text += sb.ToString();
                        sb.Clear();

                        if (!Directory.Exists(frmMain.InstallPath)) Directory.CreateDirectory(frmMain.InstallPath);
                        lblProgress.Text = "Copying files...";

                        sb.AppendLine("Copying files.. from " + strSourcePath + " to " + frmMain.InstallPath);
                        txtProcess.Text += sb.ToString();
                        sb.Clear();

                        IL.Common.CopyDirectory(strSourcePath, frmMain.InstallPath);
                        intProgressCount = intProgressCount + 1;//9
                        pbInstall.Value = intProgressCount * 10;
                        pbInstall.Refresh();



                        lblProgress.Refresh();
                        lblProgress.Text = "Restoring database...";

                        if (Directory.Exists(frmMain.InstallPath))
                        {
                            #region adding permission to installed folder
                            DirectoryInfo di = new DirectoryInfo(frmMain.InstallPath);
                            DirectorySecurity dSec = di.GetAccessControl();
                            dSec.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                            di.SetAccessControl(dSec);
                            #endregion



                            if (System.IO.File.Exists(frmMain.InstallPath + "\\Configs\\Config.xml")) System.IO.File.Delete(frmMain.InstallPath + "\\Configs\\Config.xml");
                            if (System.IO.File.Exists(frmMain.InstallPath + "\\DRLog.xml")) System.IO.File.Delete(frmMain.InstallPath + "\\DRLog.xml");
                            if (System.IO.File.Exists(frmMain.InstallPath + "\\DRLicense.lic")) System.IO.File.Delete(frmMain.InstallPath + "\\DRLicense.lic");

                            if (System.IO.File.Exists(Application.StartupPath + "\\Temp\\Config.xml"))
                            {
                                FileInfo fi = new FileInfo(Application.StartupPath + "\\Temp\\Config.xml");

                                System.IO.File.Copy(Application.StartupPath + "\\Temp\\Config.xml", strDBPath + "\\Config.xml");
                                System.IO.File.Delete(Application.StartupPath + "\\Temp\\Config.xml");
                            }
                            if (System.IO.File.Exists(Application.StartupPath + "\\Temp\\DRLicense.lic"))
                            {
                                System.IO.File.Copy(Application.StartupPath + "\\Temp\\DRLicense.lic", frmMain.InstallPath + "\\DRLicense.lic");
                                System.IO.File.Delete(Application.StartupPath + "\\Temp\\DRLicense.lic");
                            }

                            if (System.IO.File.Exists(Application.StartupPath + "\\Temp\\DRLog.xml"))
                            {
                                System.IO.File.Copy(Application.StartupPath + "\\Temp\\DRLog.xml", frmMain.InstallPath + "\\DRLog.xml");
                                System.IO.File.Delete(Application.StartupPath + "\\Temp\\DRLog.xml");
                            }

                            if (System.IO.Directory.Exists(Application.StartupPath + "\\Temp")) System.IO.Directory.Delete(Application.StartupPath + "\\Temp");
                            if (System.IO.Directory.Exists(frmMain.InstallPath + "\\VCDICOMDATA\\DCMXfer")) System.IO.Directory.CreateDirectory(frmMain.InstallPath + "\\VCDICOMDATA\\DCMXfer");

                        }

                        intProgressCount = intProgressCount + 1;//12
                        pbInstall.Value = intProgressCount * 10;
                        pbInstall.Refresh();


                        lblProgress.Refresh();
                        lblProgress.Text = "Installing Services...";

                        //install the services
                        foreach (DataRow drService in dtblServices.Rows)
                        {
                            DoInstallationProcess(Convert.ToInt32(drService["id"]));
                        }
                        intProgressCount = intProgressCount + 1;//10
                        pbInstall.Value = intProgressCount * 10;
                        pbInstall.Refresh();

                        // Create desktop shortcut, All program Shortcut
                        lblProgress.Refresh();
                        lblProgress.Text = "Creating shortcuts...";
                        if (frmMain.CreateAdminShortCut == true)
                        {
                            IL.Common.CreateShortcutToDesktop(strAdminShortCutLinkName, strAdminShortCutLinkName, frmMain.InstallPath + "\\" + strApplicationName);
                            IL.Common.CreateShortcutToAllProgram(strAdminShortCutLinkName, strAdminShortCutLinkName, frmMain.InstallPath + "\\" + strApplicationName);
                        }
                        if (frmMain.CreateFileUploadShortCut == true)
                        {
                            IL.Common.CreateShortcutToDesktop(strFileShortCutLinkName, strFileShortCutLinkName, frmMain.InstallPath + "\\" + strFileApplicationName);
                            IL.Common.CreateShortcutToAllProgram(strFileShortCutLinkName, strFileShortCutLinkName, frmMain.InstallPath + "\\" + strFileApplicationName);
                        }
                        intProgressCount = intProgressCount + 1;//11
                        pbInstall.Value = intProgressCount * 10;
                        pbInstall.Refresh();


                        #endregion
                    }
                    _timer.Stop();
                    InstallationProcessStarted = false;
                    pbInstall.Value = 0;
                    pbInstall.Minimum = 0;
                    pbInstall.Maximum = 120;

                    //MessageBox.Show("The installation is complete");

                }
            }
            catch (Exception ex)
            {
                sb.AppendLine(strRegion + ex.Message);
                txtProcess.Text += sb.ToString();
                sb.Clear();
                frmMain.InstallErr += strRegion + ex.Message;
            }

            string _Stat = string.Empty;
            int _Screen = 0;
            _Stat = "Update";
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

        #region DoInstallationProcess
        private void DoInstallationProcess(Int32 intServiceId)
        {
            string strServiceName = string.Empty;
            string strServiceDesc = string.Empty;
            string strFilePath = string.Empty;
            IL.Service objService = new IL.Service();
            DataRow drService = dtblServices.Select("id = " + intServiceId)[0];
            strServiceName = Convert.ToString(drService["name"]);
            strServiceDesc = Convert.ToString(drService["description"]);
            strFilePath = frmMain.InstallPath + "\\" + strServiceName + ".exe";

            if (IL.ServiceTools.ServiceInstaller.ServiceIsInstalled(strServiceName) == false)
            {
                if (IL.ServiceTools.ServiceInstaller.InstallService(strServiceName, strServiceDesc, strFilePath, false) == true)
                {
                    objService.SERVICE_NAME = strServiceName;
                    objService.Start();
                    // objService.Start();
                    //if (!objService.Start())
                    //{
                    //    frmMain.InstallErr += objService.ERROR.Trim();
                    //}
                    //if (!objService.Stop())
                    //{
                    //    frmMain.InstallErr += objService.ERROR.Trim();
                    //}
                    //if (!objService.Start())
                    //{
                    //    frmMain.InstallErr += objService.ERROR.Trim();
                    //}
                }
                else
                {

                }
            }
            objService = null;
        }
        #endregion

        #region DoConfigureSettings
        private void DoConfigureSettings()
        {
            string strInstallPath = frmMain.InstallPath;
            string strDBPath = strInstallPath + "\\Configs\\DicomDB.accdb";
            string strConn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strDBPath;
            bool bReturn = false;
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

                foreach (DataRow dr in frmMain.dtbl.Rows)
                {
                    strControlCode = Convert.ToString(dr["control_code"]).Trim();
                    strControlValue = Convert.ToString(dr["control_value"]).Trim();

                    switch (strControlCode)
                    {
                        case "ACCESSORYDIR":
                            bReturn = SaveSettings(strConn, "ACCESSORYDIR", strControlValue, ref strCatchMessage);
                            if (!bReturn) { frmMain.InstallErr += "Accessory Directory :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
                            break;
                        case "INSTNAME":
                            bReturn = SaveSettings(strConn, "INSTNAME", strControlValue, ref strCatchMessage);
                            if (!bReturn) { frmMain.InstallErr += "Institution Name :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
                            break;
                        case "INSTADDR1":
                            bReturn = SaveSettings(strConn, "INSTADDR1", strControlValue, ref strCatchMessage);
                            if (!bReturn) { frmMain.InstallErr += "Institution Address Line 1 :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
                            break;
                        case "INSTADDR2":
                            bReturn = SaveSettings(strConn, "INSTADDR2", strControlValue, ref strCatchMessage);
                            if (!bReturn) { frmMain.InstallErr += "Institution Address Line 2 :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
                            break;
                        case "INSTZIP":
                            bReturn = SaveSettings(strConn, "INSTZIP", strControlValue, ref strCatchMessage);
                            if (!bReturn) { frmMain.InstallErr += "Zip :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
                            break;
                        case "RCVDIR":
                            bReturn = SaveSettings(strConn, "RCVDIR", strControlValue, ref strCatchMessage);
                            if (!bReturn) { frmMain.InstallErr += "Receiving Folder Path (Default) :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
                            break;
                        case "RCVDIRMANUAL":
                            bReturn = SaveSettings(strConn, "RCVDIRMANUAL", strControlValue, ref strCatchMessage);
                            if (!bReturn) { frmMain.InstallErr += "Receiving Folder Path (for files uploaded manually) :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
                            break;
                        case "RCVIMGDIR":
                            bReturn = SaveSettings(strConn, "RCVIMGDIR", strControlValue, ref strCatchMessage);
                            if (!bReturn) { frmMain.InstallErr += "Receiving Folder Path (for image files uploaded manually) :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
                            break;
                        case "RCVAETITLE":
                            bReturn = SaveSettings(strConn, "RCVAETITLE", strControlValue, ref strCatchMessage);
                            if (!bReturn) { frmMain.InstallErr += "Receiver AE Title :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
                            break;
                        case "RCVPORTNO":
                            bReturn = SaveSettings(strConn, "RCVPORTNO", strControlValue, ref strCatchMessage);
                            if (!bReturn) { frmMain.InstallErr += "Receiver Port No. :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
                            break;
                        case "RCVEXEOPTIONS":
                            bReturn = SaveSettings(strConn, "RCVEXEOPTIONS", strControlValue, ref strCatchMessage);
                            if (!bReturn) { frmMain.InstallErr += "Receiver exe options :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
                            break;
                        case "SNDDIR":
                            bReturn = SaveSettings(strConn, "SNDDIR", strControlValue, ref strCatchMessage);
                            if (!bReturn) { frmMain.InstallErr += "Sending Folder Path :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
                            break;
                        case "ARCHDIR":
                            bReturn = SaveSettings(strConn, "ARCHDIR", strControlValue, ref strCatchMessage);
                            if (!bReturn) { frmMain.InstallErr += "Archive Folder Path (files to be archived after sending) :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
                            break;
                        case "SNDAETITLE":
                            bReturn = SaveSettings(strConn, "SNDAETITLE", strControlValue, ref strCatchMessage);
                            if (!bReturn) { frmMain.InstallErr += "Sender AE Title :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
                            break;
                        case "SNDPORTNO":
                            bReturn = SaveSettings(strConn, "SNDPORTNO", strControlValue, ref strCatchMessage);
                            if (!bReturn) { frmMain.InstallErr += "Sender Port No. :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
                            break;
                        case "SNDEXEOPTIONS":
                            bReturn = SaveSettings(strConn, "SNDEXEOPTIONS", strControlValue, ref strCatchMessage);
                            if (!bReturn) { frmMain.InstallErr += "Sender Exe Options :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
                            break;
                    }
                }


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

        #region CheckExistingInstallation
        private bool CheckExistingInstallation()
        {
            bool bRet = true;
            int intFlag1 = 0;
            int intFlag2 = 0;
            string strStatus = string.Empty;
            IL.Service objService = new IL.Service();
            StringBuilder sb = new StringBuilder();

            try
            {
                #region checking Dicom Receiver Service status
                objService.SERVICE_NAME = "DICOMReceiverService";
                strStatus = objService.CheckStatus();

                frmMain.InstallPath = objService.SERVICE_EXECUTABLE_PATH.Trim();
                if (frmMain.InstallPath != string.Empty) frmMain.InstallPath = frmMain.InstallPath.Substring(0, frmMain.InstallPath.LastIndexOf("\\"));

                if (strStatus.ToUpper().IndexOf("RUN") >= 0)
                {
                    intFlag1 = 1;
                    objService.SERVICE_NAME = "DICOMReceiverService";
                    if (!objService.Stop())
                    {
                        MessageBox.Show(objService.ERROR.Trim(), " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        bRet = false;
                    }

                }
                else if (strStatus.ToUpper().IndexOf("STOP") >= 0)
                {
                    intFlag1 = 1;
                    bRet = true;
                }
                else if (strStatus.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    intFlag1 = 0;
                    bRet = true;
                }

                #endregion

                #region checking Dicom Sender Service status
                objService.SERVICE_NAME = "DICOMSenderService";
                strStatus = objService.CheckStatus();

                frmMain.InstallPath = objService.SERVICE_EXECUTABLE_PATH.Trim();
                if (frmMain.InstallPath != string.Empty) frmMain.InstallPath = frmMain.InstallPath.Substring(0, frmMain.InstallPath.LastIndexOf("\\"));

                if (strStatus.ToUpper().IndexOf("RUN") >= 0)
                {
                    intFlag2 = 1;
                    objService.SERVICE_NAME = "DICOMSenderService";
                    if (!objService.Stop())
                    {
                        MessageBox.Show(objService.ERROR.Trim(), " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        bRet = false;
                    }
                    else
                    {
                        bRet = true;
                    }
                }
                else if (strStatus.ToUpper().IndexOf("STOP") >= 0)
                {
                    intFlag2 = 1;
                    bRet = true;
                }
                else if (strStatus.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    intFlag2 = 0;
                    bRet = true;
                }

                #endregion


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





                sb.AppendLine("Installation Path : " + frmMain.InstallPath);
                txtProcess.Text += sb.ToString();
                sb.Clear();

                //if (intFlag1 == 1 || intFlag2 == 1)
                //{
                //    System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(frmMain.InstallPath);
                //    DirectorySecurity dSecurity = dInfo.GetAccessControl();
                //    dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                //    dInfo.SetAccessControl(dSecurity);
                //    IL.Common.DeleteInstallationFolder(dInfo, true);

                //    IL.Common.DeleteDeskTopShortcut("VETRIS DICOM ROUTER");
                //    IL.Common.DeleteAllProgramShortcut("VETRIS DICOM ROUTER");
                //}
            }
            catch (Exception expErr)
            {
                bRet = false;
                MessageBox.Show(expErr.Message, " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                objService = null;
            }

            return bRet;
        }
        #endregion
    }
}
