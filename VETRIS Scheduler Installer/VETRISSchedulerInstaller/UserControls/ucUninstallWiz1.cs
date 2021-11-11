using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VETRISSchedulerInstaller.UserControls
{
    public partial class ucUninstallWiz1 : UserControl
    {
        #region Members & Variables
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;
        #endregion
        
        public ucUninstallWiz1()
        {
            InitializeComponent();
        }

        #region ucUninstallWiz1_Load
        private void ucUninstallWiz1_Load(object sender, EventArgs e)
        {
            lblInstallPath.Text = frmMain.InstallPath;
        }
        #endregion

        #region btnPrev_Click
        private void btnPrev_Click(object sender, EventArgs e)
        {
            string _Stat = string.Empty;
            int _Screen = 0;

            _Stat = "Uninstall";
            _Screen = 0;
            ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, _Screen);
            IdentityUpdated(this, args);
        }
        #endregion

        #region btnNext_Click
        private void btnNext_Click(object sender, EventArgs e)
        {
            string _Stat = string.Empty;
            int _Screen = 0;

            _Stat = "Uninstall";
            _Screen = 2;
            ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, _Screen);
            IdentityUpdated(this, args);


        }
        #endregion

        #region btnCancel_Click
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string strMsg = string.Empty; string _Stat = string.Empty;
            strMsg = "Are you sure to quit the uninstallation process ?";
            DialogResult result = MessageBox.Show(strMsg, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

                _Stat = "Exit";
                ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, 0);
                IdentityUpdated(this, args);
            }
        }
        #endregion
    }
}
