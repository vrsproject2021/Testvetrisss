using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DICOMRouterInstaller.UserControls
{
    public partial class ucUninstallWiz3 : UserControl
    {
        #region Members & Variables
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;
        #endregion

        public ucUninstallWiz3()
        {
            InitializeComponent();
        }

        #region ucUninstallWiz3_Load
        private void ucUninstallWiz3_Load(object sender, EventArgs e)
        {
            if (frmMain.InstallErr.Trim() == "")
            {
                lblInstallResult.Text = "DICOM Router is uninstalled successfully.";
            }
            else
            {
                lblInstallResult.Text = "DICOM Router is uninstalled with some errors.";
                lblErr.Visible = true;
                txtError.Visible = true;
                txtError.Text = frmMain.InstallErr.Trim();
            }
        } 
        #endregion

        #region btnFinish_Click
        private void btnFinish_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion
    }
}
