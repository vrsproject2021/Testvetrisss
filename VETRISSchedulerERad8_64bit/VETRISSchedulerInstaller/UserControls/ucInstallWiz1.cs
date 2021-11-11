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
    public partial class ucInstallWiz1 : UserControl
    {
        #region Members & Variables
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;
        #endregion

        public ucInstallWiz1()
        {
            InitializeComponent();
        }

        #region ucInstallWiz1_Load
        private void ucInstallWiz1_Load(object sender, EventArgs e)
        {
            txtInstPath.Text = frmMain.InstallPath;
            btnInstPath.Focus();
        }
        #endregion

        #region btnPrev_Click
        private void btnPrev_Click(object sender, EventArgs e)
        {
            string _Stat = string.Empty;
            int _Screen = 0;

            
            _Stat = "Install";
            _Screen = 1;
            ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, _Screen);
            IdentityUpdated(this, args);
        }
        #endregion

        #region btnNext_Click
        private void btnNext_Click(object sender, EventArgs e)
        {
            string _Stat = string.Empty;
            int _Screen = 0;


            if (ValidateValues())
            {
                
                _Stat = "Install";
                _Screen = 2;
                ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, _Screen);
                IdentityUpdated(this, args);
            }

        }
        #endregion

        #region ValidateValues
        private bool ValidateValues()
        {
            bool bRet = true;
            string strMsg = string.Empty;

            if (txtInstPath.Text.Trim() == string.Empty)
            {
                strMsg = "Installation Path is required";
            }
            

            
            if (strMsg.Trim() == string.Empty)
            {
                bRet = true;
            }
            else
            {
                bRet = false;
                MessageBox.Show(strMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            return bRet;
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
                ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, 0);
                IdentityUpdated(this, args);
            }
        }
        #endregion

        #region btnInstPath_Click
        private void btnInstPath_Click(object sender, EventArgs e)
        {
            string strWAP = string.Empty;
            strWAP = OpenFolderDialouge(txtInstPath.Text, "Installation Folder Path");
            txtInstPath.Text = frmMain.InstallPath = strWAP;
        }
        #endregion

        #region OpenFolderDialouge
        public static string OpenFolderDialouge(string initialPath, string Description)
        {
            string strPath = string.Empty;
            FolderBrowserDialog FdialogFolder = new FolderBrowserDialog();
            FdialogFolder.RootFolder = Environment.SpecialFolder.Desktop;

            if (System.IO.Directory.Exists(initialPath) == true)
                FdialogFolder.SelectedPath = initialPath;
            else
                FdialogFolder.SelectedPath = "C:\\";

            FdialogFolder.Description = Description;
            if (FdialogFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                strPath = FdialogFolder.SelectedPath;
            }
            FdialogFolder.Dispose();
            FdialogFolder = null;
            return strPath;
        }
        #endregion
    }
}
