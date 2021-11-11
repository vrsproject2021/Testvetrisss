using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Windows.Forms;

namespace VETRISAccountsScheduler
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        #region frmMain_Load
        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text = "VETRIS - Account Update Scheduler";

        }

        #endregion

        #region btnStartStopSvc_Click
        private void btnStartStopSvc_Click(object sender, EventArgs e)
        {
            UserControls.ucService ucService = new UserControls.ucService();
            ucService.Dock = DockStyle.Fill;
            ucService.IdentityUpdated += new UserControls.ucService.IdentityUpdateHandler(Cancel_ButtonClicked);
            if (pnlAction.Controls.Count > 0) pnlAction.Controls.RemoveAt(0);
            pnlAction.Controls.Add(ucService);
        }
        #endregion

        #region btnViewLog_Click
        private void btnViewLog_Click(object sender, EventArgs e)
        {
            UserControls.ucViewLog ucViewLog = new UserControls.ucViewLog();
            ucViewLog.Dock = DockStyle.Fill;
            ucViewLog.IdentityUpdated += new UserControls.ucViewLog.IdentityUpdateHandler(Cancel_ButtonClicked);
            if (pnlAction.Controls.Count > 0) pnlAction.Controls.RemoveAt(0);
            pnlAction.Controls.Add(ucViewLog);
        } 
        #endregion

        #region btnExit_Click
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region Cancel_ButtonClicked
        private void Cancel_ButtonClicked(object sender, ApplicationDelegateEventArgs e)
        {
            if (e.Status.ToString() == "Cancel")
            {
                if (pnlAction.Controls.Count > 0)
                {
                    pnlAction.Controls.RemoveAt(0);

                }
            }

        }
        #endregion

        
    }
}
