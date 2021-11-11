using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VETRISSchedulerInstaller
{
    public partial class frmMain : Form
    {
        public static string InstallPath = string.Empty;
        public static string InstallErr = string.Empty;
        public static string Action = string.Empty;

        public frmMain()
        {
            InitializeComponent();
        }

        #region frmMain_Load
        private void frmMain_Load(object sender, EventArgs e)
        {
            lblVer.Text = lblVer.Text + " " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            LoadFirstWizardScreen();
        }
        #endregion

        #region LoadFirstWizardScreen
        private void LoadFirstWizardScreen()
        {
            UserControls.ucSetupWiz1 ucSetupWiz1 = new UserControls.ucSetupWiz1();
            ucSetupWiz1.Dock = DockStyle.Fill;
            ucSetupWiz1.IdentityUpdated += new UserControls.ucSetupWiz1.IdentityUpdateHandler(ButtonClicked);
            if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
            pnlUC.Controls.Add(ucSetupWiz1);
        }
        #endregion

        #region ButtonClicked
        private void ButtonClicked(object sender, ApplicationDelegateEventArgs e)
        {
            string strStat = string.Empty;
            int intScreen = 0;
            strStat = e.Status.ToString();

            switch (strStat)
            {
                case "Cancel":
                    if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                    break;
                case "Exit":
                    if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                    Application.Exit();
                    break;
                case "Install":
                    if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                    intScreen = e.Screen;

                    #region Install
                    switch (intScreen)
                    {
                       
                        case 0:
                            UserControls.ucSetupWiz1 ucSetupWiz1 = new UserControls.ucSetupWiz1();
                            ucSetupWiz1.Dock = DockStyle.Fill;
                            ucSetupWiz1.IdentityUpdated += new UserControls.ucSetupWiz1.IdentityUpdateHandler(ButtonClicked);
                            if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                            pnlUC.Controls.Add(ucSetupWiz1);
                            break;
                        case 1:
                            UserControls.ucInstallWiz1 ucInstallWiz1 = new UserControls.ucInstallWiz1();
                            ucInstallWiz1.Dock = DockStyle.Fill;
                            ucInstallWiz1.IdentityUpdated += new UserControls.ucInstallWiz1.IdentityUpdateHandler(ButtonClicked);
                            if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                            pnlUC.Controls.Add(ucInstallWiz1);
                            break;
                        case 2:
                            UserControls.ucInstallWiz2 ucInstallWiz2 = new UserControls.ucInstallWiz2();
                            ucInstallWiz2.Dock = DockStyle.Fill;
                            ucInstallWiz2.IdentityUpdated += new UserControls.ucInstallWiz2.IdentityUpdateHandler(ButtonClicked);
                            if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                            pnlUC.Controls.Add(ucInstallWiz2);
                            break;
                        case 3:
                            UserControls.ucInstallWiz3 ucInstallWiz3 = new UserControls.ucInstallWiz3();
                            ucInstallWiz3.Dock = DockStyle.Fill;
                            ucInstallWiz3.IdentityUpdated += new UserControls.ucInstallWiz3.IdentityUpdateHandler(ButtonClicked);
                            if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                            pnlUC.Controls.Add(ucInstallWiz3);
                            break;
               
                    }
                    #endregion

                    break;
                case "Uninstall":
                    if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                    intScreen = e.Screen;

                    #region Uninstall
                    switch (intScreen)
                    {
                        case 0:
                            UserControls.ucSetupWiz1 ucSetupWiz1 = new UserControls.ucSetupWiz1();
                            ucSetupWiz1.Dock = DockStyle.Fill;
                            ucSetupWiz1.IdentityUpdated += new UserControls.ucSetupWiz1.IdentityUpdateHandler(ButtonClicked);
                            if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                            pnlUC.Controls.Add(ucSetupWiz1);
                            break;
                        case 1:
                            UserControls.ucUninstallWiz1 ucUninstallWiz1 = new UserControls.ucUninstallWiz1();
                            ucUninstallWiz1.Dock = DockStyle.Fill;
                            ucUninstallWiz1.IdentityUpdated += new UserControls.ucUninstallWiz1.IdentityUpdateHandler(ButtonClicked);
                            if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                            pnlUC.Controls.Add(ucUninstallWiz1);
                            break;
                        case 2:
                            UserControls.ucUninstallWiz2 ucUninstallWiz2 = new UserControls.ucUninstallWiz2();
                            ucUninstallWiz2.Dock = DockStyle.Fill;
                            ucUninstallWiz2.IdentityUpdated += new UserControls.ucUninstallWiz2.IdentityUpdateHandler(ButtonClicked);
                            if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                            pnlUC.Controls.Add(ucUninstallWiz2);
                            break;
                        case 3:
                            UserControls.ucUninstallWiz3 ucUninstallWiz3 = new UserControls.ucUninstallWiz3();
                            ucUninstallWiz3.Dock = DockStyle.Fill;
                            ucUninstallWiz3.IdentityUpdated += new UserControls.ucUninstallWiz3.IdentityUpdateHandler(ButtonClicked);
                            if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                            pnlUC.Controls.Add(ucUninstallWiz3);
                            break;
                    }
                    #endregion

                    break;
                case "Update":
                    if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                    intScreen = e.Screen;

                    #region Update
                    switch (intScreen)
                    {
                        case 0:
                            UserControls.ucSetupWiz1 ucSetupWiz1 = new UserControls.ucSetupWiz1();
                            ucSetupWiz1.Dock = DockStyle.Fill;
                            ucSetupWiz1.IdentityUpdated += new UserControls.ucSetupWiz1.IdentityUpdateHandler(ButtonClicked);
                            if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                            pnlUC.Controls.Add(ucSetupWiz1);
                            break;
                        case 1:
                            UserControls.ucUpdateWiz1 ucUpdateWiz1 = new UserControls.ucUpdateWiz1();
                            ucUpdateWiz1.Dock = DockStyle.Fill;
                            ucUpdateWiz1.IdentityUpdated += new UserControls.ucUpdateWiz1.IdentityUpdateHandler(ButtonClicked);
                            if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                            pnlUC.Controls.Add(ucUpdateWiz1);
                            break;
                        case 2:
                            UserControls.ucUpdateWiz2 ucUpdateWiz2 = new UserControls.ucUpdateWiz2();
                            ucUpdateWiz2.Dock = DockStyle.Fill;
                            ucUpdateWiz2.IdentityUpdated += new UserControls.ucUpdateWiz2.IdentityUpdateHandler(ButtonClicked);
                            if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                            pnlUC.Controls.Add(ucUpdateWiz2);
                            break;
                        case 3:
                            UserControls.ucUpdateWiz3 ucUpdateWiz3 = new UserControls.ucUpdateWiz3();
                            ucUpdateWiz3.Dock = DockStyle.Fill;
                            ucUpdateWiz3.IdentityUpdated += new UserControls.ucUpdateWiz3.IdentityUpdateHandler(ButtonClicked);
                            if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                            pnlUC.Controls.Add(ucUpdateWiz3);
                            break;
                    }
                    #endregion

                    break;
            }


        }
        #endregion

    }
}
