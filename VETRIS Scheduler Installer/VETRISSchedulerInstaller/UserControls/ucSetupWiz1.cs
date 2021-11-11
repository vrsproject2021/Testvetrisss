using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VETRISSchedulerInstaller.UserControls
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
            arrStat = CheckServiceStatus();

            if (arrStat[0] != 3)
            {
                rdoInstall.Enabled = false;
                rdoUpdate.Checked = true;
            }
            else if (arrStat[0] == 3)
            {
                rdoInstall.Checked = true;
                rdoUpdate.Enabled = false;
                rdoUninstall.Enabled = false;

            }

            if (frmMain.Action == "Install") rdoInstall.Checked = true;
            else if (frmMain.Action == "Update") rdoUpdate.Checked = true;
            else if (frmMain.Action == "Uninstall") rdoUninstall.Checked = true;


        }
        #endregion

        #region CheckServiceStatus
        private int[] CheckServiceStatus()
        {

            Label.CheckForIllegalCrossThreadCalls = false;
            string strStatus = string.Empty;
            int[] arrStat = new int[6];
            int intStatID1 = 0;
            int intStatID2 = 0;
            int intStatID3 = 0;
            int intStatID4 = 0;
            int intStatID5 = 0;
            int intStatID6 = 0;
            objService = new IL.Service();

            try
            {
                #region checking VETRIS New Data Synch Service
                objService.SERVICE_NAME = "VETRIS New Data Synch Service";
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

                #region checking VETRIS Write Back Service
                objService.SERVICE_NAME = "VETRIS Write Back Service";
                strStatus = objService.CheckStatus();

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

                #region checking VETRIS Status Update Service
                objService.SERVICE_NAME = "VETRIS Status Update Service";
                strStatus = objService.CheckStatus();

                if (strStatus.ToUpper().IndexOf("RUN") >= 0)
                {
                    intStatID3 = 1;
                }
                else if (strStatus.ToUpper().IndexOf("STOP") >= 0)
                {
                    intStatID3 = 2;
                }
                else if (strStatus.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    intStatID3 = 3;
                }
                else if (strStatus.ToUpper().IndexOf("PENDING") >= 0)
                {
                    intStatID3 = 4;
                }
                #endregion

                #region checking VETRIS Notification Service
                objService.SERVICE_NAME = "VETRIS Notification Service";
                strStatus = objService.CheckStatus();

                if (strStatus.ToUpper().IndexOf("RUN") >= 0)
                {
                    intStatID4 = 1;
                }
                else if (strStatus.ToUpper().IndexOf("STOP") >= 0)
                {
                    intStatID4 = 2;
                }
                else if (strStatus.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    intStatID4 = 3;
                }
                else if (strStatus.ToUpper().IndexOf("PENDING") >= 0)
                {
                    intStatID4 = 4;
                }
                #endregion

                #region checking VETRIS Dayend Service
                objService.SERVICE_NAME = "VETRIS Dayend Service";
                strStatus = objService.CheckStatus();

                if (strStatus.ToUpper().IndexOf("RUN") >= 0)
                {
                    intStatID5 = 1;
                }
                else if (strStatus.ToUpper().IndexOf("STOP") >= 0)
                {
                    intStatID5 = 2;
                }
                else if (strStatus.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    intStatID5 = 3;
                }
                else if (strStatus.ToUpper().IndexOf("PENDING") >= 0)
                {
                    intStatID5 = 4;
                }
                #endregion

                #region checking VETRIS Missing Data Synch Service
                objService.SERVICE_NAME = "VETRIS Missing Data Synch Service";
                strStatus = objService.CheckStatus();

                if (strStatus.ToUpper().IndexOf("RUN") >= 0)
                {
                    intStatID6 = 1;
                }
                else if (strStatus.ToUpper().IndexOf("STOP") >= 0)
                {
                    intStatID6 = 2;
                }
                else if (strStatus.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    intStatID6 = 3;
                }
                else if (strStatus.ToUpper().IndexOf("PENDING") >= 0)
                {
                    intStatID6 = 4;
                }
                #endregion

                strInstallPath = objService.SERVICE_EXECUTABLE_PATH.Trim();
                if (strInstallPath.Trim() != string.Empty) strInstallPath = strInstallPath.Substring(0, strInstallPath.LastIndexOf("\\"));
                frmMain.InstallPath = strInstallPath;

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
            arrStat[2] = intStatID3;
            arrStat[3] = intStatID4;
            arrStat[4] = intStatID5;
            arrStat[5] = intStatID6;
            return arrStat;

        }
        #endregion

        #region btnNext_Click
        private void btnNext_Click(object sender, EventArgs e)
        {
            string _Stat = string.Empty;
            int _Screen = 0;
            int[] arrStat = new int[0];
            arrStat = new int[6];

            if (rdoInstall.Checked)
            {

                arrStat = CheckServiceStatus();

                if (arrStat[0] != 3)
                {
                    MessageBox.Show("VETRIS Scheduler Services is already installed.\r\nPlease uninstall the application to continue.", " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    _Stat = "Install";
                    _Screen = 1;
                    ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, _Screen);
                    IdentityUpdated(this, args);
                }
               
               
            }
            else if (rdoUpdate.Checked)
            {
                arrStat = CheckServiceStatus();
                if (arrStat[0] == 3)
                {
                    MessageBox.Show("VETRIS Scheduler Services is not found to be installed.", " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if ((arrStat[0] == 1) || (arrStat[1] == 1) || (arrStat[2] == 1) || (arrStat[3] == 1) || (arrStat[4] == 1) || (arrStat[5] == 1))
                {
                    MessageBox.Show("One or more windows services of VETRIS Scheduler Services is running.\r\nPlease stop the service(s) before updating.", " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    _Stat = "Update";
                    _Screen = 1;
                    frmMain.InstallPath = strInstallPath;
                    ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, _Screen);
                    IdentityUpdated(this, args);

                }
            }
            else if (rdoUninstall.Checked)
            {
                arrStat = new int[6];
                arrStat = CheckServiceStatus();

                if (arrStat[0] == 3)
                {
                    MessageBox.Show("VETRIS Scheduler Services is not found to be installed.", " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if ((arrStat[0] == 1) || (arrStat[1] == 1) || (arrStat[2] == 1) || (arrStat[3] == 1) || (arrStat[4] == 1) || (arrStat[5] == 1))
                {
                    MessageBox.Show("One or more windows services of VETRIS Scheduler Services is running.\r\nPlease stop the service(s) before uninstalling.", " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    _Stat = "Uninstall";
                    _Screen = 1;
                    frmMain.InstallPath = strInstallPath;
                    ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, _Screen);
                    IdentityUpdated(this, args);

                }
            }

            frmMain.Action = _Stat;
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
    }
}
