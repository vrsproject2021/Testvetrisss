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
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows;
using IWshRuntimeLibrary;

namespace DICOMRouterInstaller.UserControls
{
    public partial class ucSetupWiz1 : UserControl
    {
        #region Members & Variables
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;
        public enum ServiceType { DCMRCV = 1, DCMSND = 2 }
        public int ServiceTypeId = 0;
        private string strInstallPath = string.Empty;
        private IL.Service objService = null;
        #endregion

        public ucSetupWiz1()
        {
            InitializeComponent();
        }

        #region ucSetupWiz1_Load
        private void ucSetupWiz1_Load(object sender, EventArgs e)
        {
            int[] arrStat = new int[0];
            arrStat = new int[2];
            arrStat = CheckServiceStatus();

            if ((arrStat[0] != 3) || (arrStat[1] != 3))
            {
                rdoInstall.Enabled = false;
                rdoUpdate.Checked = true;
            }
            else if ((arrStat[0] == 3) || (arrStat[1] == 3))
            {
                rdoInstall.Checked = true;
                rdoUpdate.Enabled = false;
                rdoUninstall.Enabled = false;
                
            }

            if (frmMain.Action == "Install") rdoInstall.Checked = true;
            else if (frmMain.Action == "Update") rdoUpdate.Checked=true;
            else if (frmMain.Action == "Uninstall") rdoUninstall.Checked = true;


        }  
        #endregion

        #region CheckOldInstallation
        private bool CheckOldInstallation()
        {
            bool bRet = true;
            int intFlag1 = 0;
            int intFlag2 = 0;
            string strStatus = string.Empty;
            objService = new IL.Service();

            try
            {
                #region checking Dicom Receiver Service status
                objService.SERVICE_NAME = "RadicomReceiverService";
                strStatus = objService.CheckStatus();

                if (strStatus.ToUpper().IndexOf("RUN") >= 0)
                {
                    intFlag1 = 1;
                    objService.SERVICE_NAME = "RadicomReceiverService";
                    if (!objService.Stop())
                    {
                        MessageBox.Show(objService.ERROR.Trim(), " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        bRet = false;
                    }
                    else
                    {
                        DoUninstallProcess("RadicomReceiverService");
                        bRet = true;
                    }
                }
                else if (strStatus.ToUpper().IndexOf("STOP") >= 0)
                {
                    intFlag1 = 1;
                    DoUninstallProcess("RadicomReceiverService");
                    bRet = true;
                }
                else if (strStatus.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    intFlag1 = 0;
                    bRet = true;
                }
               
                #endregion

                #region checking Dicom Sender Service status
                objService.SERVICE_NAME = "RadicomSenderService";
                strStatus = objService.CheckStatus();

                if (strStatus.ToUpper().IndexOf("RUN") >= 0)
                {
                    intFlag2 = 1;
                    objService.SERVICE_NAME = "RadicomSenderService";
                    if (!objService.Stop())
                    {
                        MessageBox.Show(objService.ERROR.Trim(), " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        bRet = false;
                    }
                    else
                    {
                        DoUninstallProcess("RadicomSenderService");
                        bRet = true;
                    }
                }
                else if (strStatus.ToUpper().IndexOf("STOP") >= 0)
                {
                    intFlag2 = 1;
                    DoUninstallProcess("RadicomSenderService");
                    bRet = true;
                }
                else if (strStatus.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    intFlag2 = 0;
                    bRet = true;
                }
               
                #endregion

                if (intFlag1 == 1)
                {
                    foreach (var process in Process.GetProcessesByName("storescp"))
                    {
                        process.Kill();
                    }
                    foreach (var process in Process.GetProcessesByName("RadicomReceiverService"))
                    {
                        process.Kill();
                    }
                }
                if (intFlag2 == 1)
                {
                    foreach (var process in Process.GetProcessesByName("RadicomSenderService"))
                    {
                        process.Kill();
                    }
                }

                if (intFlag1 == 1 || intFlag2 == 1)
                {
                    System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(frmMain.InstallPath);
                    DirectorySecurity dSecurity = dInfo.GetAccessControl();
                    dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                    dInfo.SetAccessControl(dSecurity);
                    IL.Common.DeleteInstallationFolder(dInfo, true);

                    IL.Common.DeleteDeskTopShortcut("VETRIS DICOM ROUTER");
                    IL.Common.DeleteAllProgramShortcut("VETRIS DICOM ROUTER");
                }
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

        #region DoUninstallProcess
        private void DoUninstallProcess(string strServiceName)
        {

            string strFilePath = string.Empty;
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

        #region btnNext_Click
        private void btnNext_Click(object sender, EventArgs e)
        {
            string _Stat = string.Empty;
            int _Screen = 0;
            int[] arrStat = new int[0];

            if (CheckOldInstallation())
            {

                if (rdoInstall.Checked)
                {
                    #region For Install
                    //if (CheckAccessEngine())
                    //{
                    arrStat = new int[2];
                    arrStat = CheckServiceStatus();

                    if ((arrStat[0] != 3) || (arrStat[1] != 3))
                    {
                        MessageBox.Show("DICOM Router is already installed.\r\nPlease uninstall the application to continue.", " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        _Stat = "Install";
                        _Screen = 1;
                        ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, _Screen);
                        IdentityUpdated(this, args);
                    }
                    //}
                    //else
                    //{
                    //    _Stat = "Install";
                    //    _Screen = -1;
                    //    ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, _Screen);
                    //    IdentityUpdated(this, args);
                    //}
                    #endregion
                }
                else if (rdoUpdate.Checked)
                {
                    #region For Update
                    arrStat = new int[2];
                    arrStat = CheckServiceStatus();
                    if ((arrStat[0] == 3) || (arrStat[1] == 3))
                    {
                        MessageBox.Show("DICOM Router is not found to be installed.", " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if ((arrStat[0] == 1) || (arrStat[1] == 1))
                    {
                        //MessageBox.Show("One or more windows services of DICOM Router is running.\r\nPlease stop the service(s) before updating.", " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        IL.Service objService = new IL.Service();
                        if (arrStat[0] == 1)
                        {
                            objService.SERVICE_NAME = "DICOMReceiverService";
                            if (!objService.Stop())
                            {
                                MessageBox.Show(objService.ERROR.Trim(), " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        if (arrStat[1] == 1)
                        {
                            objService.SERVICE_NAME = "DICOMSenderService";
                            if (!objService.Stop())
                            {
                                MessageBox.Show(objService.ERROR.Trim(), " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        objService = null;

                        _Stat = "Update";
                        _Screen = 1;
                        frmMain.InstallPath = strInstallPath;
                        ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, _Screen);
                        IdentityUpdated(this, args);
                    }
                    else
                    {

                        _Stat = "Update";
                        _Screen = 1;
                        frmMain.InstallPath = strInstallPath;
                        ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, _Screen);
                        IdentityUpdated(this, args);

                    }
                    #endregion

                }
                else if (rdoUninstall.Checked)
                {
                    #region For Uninstall
                    arrStat = new int[2];
                    arrStat = CheckServiceStatus();

                    if ((arrStat[0] == 3) || (arrStat[1] == 3))
                    {
                        MessageBox.Show("DICOM Router is not found to be installed.", " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if ((arrStat[0] == 1) || (arrStat[1] == 1))
                    {
                        //MessageBox.Show("One or more windows services of DICOM Router is running.\r\nPlease stop the service(s) before uninstalling.", " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        IL.Service objService = new IL.Service();
                        if (arrStat[0] == 1)
                        {
                            objService.SERVICE_NAME = "DICOMReceiverService";
                            if (!objService.Stop())
                            {
                                MessageBox.Show(objService.ERROR.Trim(), " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        if (arrStat[1] == 1)
                        {
                            objService.SERVICE_NAME = "DICOMSenderService";
                            if (!objService.Stop())
                            {
                                MessageBox.Show(objService.ERROR.Trim(), " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        objService = null;

                        _Stat = "Uninstall";
                        _Screen = 1;
                        frmMain.InstallPath = strInstallPath;
                        ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, _Screen);
                        IdentityUpdated(this, args);
                    }
                    else
                    {

                        _Stat = "Uninstall";
                        _Screen = 1;
                        frmMain.InstallPath = strInstallPath;
                        ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, _Screen);
                        IdentityUpdated(this, args);
                    }
                    #endregion
                }
            }

            frmMain.Action = _Stat;
        } 
        #endregion

        #region CheckServiceStatus
        private int[] CheckServiceStatus()
        {
            
            Label.CheckForIllegalCrossThreadCalls = false;
            string strStatus = string.Empty;
            int[] arrStat = new int[2];
            int intStatID1 = 0;
            int intStatID2 = 0;
            objService = new IL.Service();

            try
            {
                #region checking Dicom Receiver Service status
                objService.SERVICE_NAME = "DICOMReceiverService";
                strStatus = objService.CheckStatus();

                if (strStatus.ToUpper().IndexOf("RUN") >= 0)
                {
                    intStatID1 = 1;
                }
                else if (strStatus.ToUpper().IndexOf("STOP") >= 0)
                {
                    intStatID1 = 2;
                }
                else if (strStatus.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    intStatID1 = 3;
                }
                else if (strStatus.ToUpper().IndexOf("PENDING") >= 0)
                {
                    intStatID1 = 4;
                }
                #endregion

                #region checking Dicom Sender Service status
                objService.SERVICE_NAME = "DICOMSenderService";
                strStatus= objService.CheckStatus();

                if (strStatus.ToUpper().IndexOf("RUN") >= 0)
                {
                    intStatID2 = 1;
                }
                else if (strStatus.ToUpper().IndexOf("STOP") >= 0)
                {
                    intStatID2 = 2;
                }
                else if (strStatus.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    intStatID2 = 3;
                }
                else if (strStatus.ToUpper().IndexOf("PENDING") >= 0)
                {
                    intStatID2 = 4;
                }
                #endregion

                strInstallPath = objService.SERVICE_EXECUTABLE_PATH.Trim();
                if(strInstallPath.Trim() != string.Empty) strInstallPath = strInstallPath.Substring(0, strInstallPath.LastIndexOf("\\"));

            }
            catch (Exception expErr)
            {
                MessageBox.Show(expErr.Message, " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                objService = null;
            }

            arrStat[0] = intStatID1;
            arrStat[1] = intStatID2;
            return arrStat;

        }
        #endregion

        #region btnCancel_Click
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string strMsg = string.Empty; string _Stat = string.Empty;
            strMsg = "Are you sure to quit the set up ?";
            DialogResult result = MessageBox.Show(strMsg, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

                _Stat = "Exit";
                ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat,0);
                IdentityUpdated(this, args);
            }
        } 
        #endregion

        #region CheckAccessEngine
        private bool CheckAccessEngine()
        {
            bool bReturn = false;
            string AccessDBAsValue = string.Empty;
            string[] arrKey = new string[0];
            Microsoft.Win32.RegistryKey rkACDBKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes");

            if (rkACDBKey != null)
            {
              
                arrKey = rkACDBKey.GetSubKeyNames();
                foreach (string subKeyName in rkACDBKey.GetSubKeyNames())
                {

                    if (subKeyName.Contains("Microsoft.ACE.OLEDB"))
                    {
                        bReturn = true;
                        break;
                    }
                }
            }

            return bReturn;
        } 
        #endregion
       
    }
}
