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
    public partial class ucInstallWiz5 : UserControl
    {
        #region Members & Variables
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;
        #endregion

        public ucInstallWiz5()
        {
            InitializeComponent();
        }

        #region ucInstallWiz5_Load
        private void ucInstallWiz5_Load(object sender, EventArgs e)
        {
            if (frmMain.InstallErr.Trim() == "")
            {
                lblInstallResult.Text = "Installation completed successfully.You can now run the application";
            }
            else
            {
                lblInstallResult.Text = "Installation completed with some errors.";
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
