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
    public partial class ucSetupWiz2 : UserControl
    {
        #region Members & Variables
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;
        #endregion

        public ucSetupWiz2()
        {
            InitializeComponent();
        }

        #region ucSetupWiz2_Load
        private void ucSetupWiz2_Load(object sender, EventArgs e)
        {

        } 
        #endregion

        #region llDownload_LinkClicked
        private void llDownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(llDownload.Text); 
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
