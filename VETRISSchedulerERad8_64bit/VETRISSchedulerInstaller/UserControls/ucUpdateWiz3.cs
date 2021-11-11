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
    public partial class ucUpdateWiz3 : UserControl
    {
        #region Members & Variables
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;
        #endregion

        public ucUpdateWiz3()
        {
            InitializeComponent();
        }

        #region ucUpdateWiz3_Load
        private void ucUpdateWiz3_Load(object sender, EventArgs e)
        {
            if (frmMain.InstallErr.Trim() == "")
            {
                lblInstallResult.Text = "VETRIS Scheduler Services installation is updated successfully.";
            }
            else
            {
                lblInstallResult.Text = "VETRIS Scheduler Services installation is updated with some errors.";
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
