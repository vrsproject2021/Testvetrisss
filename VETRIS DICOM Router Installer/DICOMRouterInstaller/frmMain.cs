using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DICOMRouterInstaller
{
    public partial class frmMain : Form
    {
        #region Members & Variables
        public static DataTable dtbl;
        public static string InstallPath = string.Empty;
        public static string InstallErr = string.Empty;
        public static string Action = string.Empty;
        public static string PACSServer = string.Empty;
        public static string DBPath = string.Empty;
        public static bool CreateAdminShortCut = false;
        public static bool CreateFileUploadShortCut = true;
        public string UpdateOnly = "N";
        #endregion

        public frmMain()
        {
            InitializeComponent();
        }

        #region frmMain_Load
        private void frmMain_Load(object sender, EventArgs e)
        {
            lblVer.Text = lblVer.Text + " " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            DBPath = Application.StartupPath + "\\DicomRouter\\Configs";
            CreateControlTable();
            LoadFirstWizardScreen();
        }
        #endregion

        #region CreateControlTable
        private void CreateControlTable()
        {
            string strFileName = Application.StartupPath + "\\DicomRouter\\Configs\\Config.xml";
            string strControlCode = string.Empty;
            DataSet ds = new DataSet();
            dtbl = new DataTable();
           
            ds.ReadXml(strFileName);
            ds.Tables[0].TableName = "Control";
            dtbl = ds.Tables[0];

            #region suspended
            //DataRow dr1 = dtbl.NewRow();
            //dr1["control_code"] = "INSTNAME";
            //dr1["control_value"] = "";
            //dtbl.Rows.Add(dr1);

            //DataRow dr2 = dtbl.NewRow();
            //dr2["control_code"] = "INSTADDR1";
            //dr2["control_value"] = "";
            //dtbl.Rows.Add(dr2);

            //DataRow dr3 = dtbl.NewRow();
            //dr3["control_code"] = "INSTADDR2";
            //dr3["control_value"] = "";
            //dtbl.Rows.Add(dr3);

            //DataRow dr4 = dtbl.NewRow();
            //dr4["control_code"] = "INSTZIP";
            //dr4["control_value"] = "";
            //dtbl.Rows.Add(dr4);

            //DataRow dr5 = dtbl.NewRow();
            //dr5["control_code"] = "ARCHDIR";
            //dr5["control_value"] = "";
            //dtbl.Rows.Add(dr5);

            //DataRow dr6 = dtbl.NewRow();
            //dr6["control_code"] = "RCVAETITLE";
            //dr6["control_value"] = "";
            //dtbl.Rows.Add(dr6);

            //DataRow dr7 = dtbl.NewRow();
            //dr7["control_code"] = "RCVPORTNO";
            //dr7["control_value"] = "";
            //dtbl.Rows.Add(dr7);

            //DataRow dr8 = dtbl.NewRow();
            //dr8["control_code"] = "RCVDIR";
            //dr8["control_value"] = "";
            //dtbl.Rows.Add(dr8);

            //DataRow dr9 = dtbl.NewRow();
            //dr9["control_code"] = "RCVDIRMANUAL";
            //dr9["control_value"] = "";
            //dtbl.Rows.Add(dr9);

            //DataRow dr10 = dtbl.NewRow();
            //dr10["control_code"] = "RCVEXEOPTIONS";
            //dr10["control_value"] = "";
            //dtbl.Rows.Add(dr10);

            ////DataRow dr11 = dtbl.NewRow();
            ////dr11["control_code"] = "SNDAETITLE";
            ////dr11["control_value"] = "";
            ////dtbl.Rows.Add(dr11);

            ////DataRow dr12 = dtbl.NewRow();
            ////dr12["control_code"] = "SNDPORTNO";
            ////dr12["control_value"] = "";
            ////dtbl.Rows.Add(dr12);

            ////DataRow dr13 = dtbl.NewRow();
            ////dr13["control_code"] = "PACSSRVRNAME";
            ////dr13["control_value"] = "";
            ////dtbl.Rows.Add(dr13);

            ////DataRow dr13 = dtbl.NewRow();
            ////dr13["control_code"] = "SNDEXEOPTIONS";
            ////dr13["control_value"] = "";
            ////dtbl.Rows.Add(dr13);

            //DataRow dr14 = dtbl.NewRow();
            //dr14["control_code"] = "SNDDIR";
            //dr14["control_value"] = "";
            //dtbl.Rows.Add(dr14);

            //DataRow dr15 = dtbl.NewRow();
            //dr15["control_code"] = "ACCESSORYDIR";
            //dr15["control_value"] = "";
            //dtbl.Rows.Add(dr15);

            //DataRow dr16 = dtbl.NewRow();
            //dr16["control_code"] = "RCVIMGDIR";
            //dr16["control_value"] = "";
            //dtbl.Rows.Add(dr16);

            //DataRow dr17 = dtbl.NewRow();
            //dr17["control_code"] = "SITECODE";
            //dr17["control_value"] = "";
            //dtbl.Rows.Add(dr17);


            //DataRow dr18 = dtbl.NewRow();
            //dr18["control_code"] = "MANUALUPLDAUTO";
            //dr18["control_value"] = "N";
            //dtbl.Rows.Add(dr18);

            //DataRow dr19 = dtbl.NewRow();
            //dr19["control_code"] = "IMGMNLUPLDAUTO";
            //dr19["control_value"] = "N";
            //dtbl.Rows.Add(dr19);

            //DataRow dr20= dtbl.NewRow();
            //dr20["control_code"] = "VETLOGIN";
            //dr20["control_value"] = "";
            //dtbl.Rows.Add(dr20);

            //DataRow dr21 = dtbl.NewRow();
            //dr21["control_code"] = "VETURL";
            //dr21["control_value"] = "https://client.vcradiology.com/vetris/VRSLogin.aspx";
            //dtbl.Rows.Add(dr21);
            #endregion

            foreach(DataRow dr in dtbl.Rows)
            {
                strControlCode = Convert.ToString(dr["control_code"]);

                switch (strControlCode)
                {
                    case "INSTNAME":
                    case "INSTADDR1":
                    case "INSTADDR2":
                    case "INSTZIP":
                    case "ARCHDIR":
                    //case "RCVAETITLE":
                    //case "RCVPORTNO":
                    case "RCVDIR":
                    case "RCVDIRMANUAL":
                    case "RCVEXEOPTIONS":
                    case "SNDDIR":
                    case "ACCESSORYDIR":
                    case "RCVIMGDIR":
                    case "SITECODE":
                    case "VETLOGIN":
                    case "FTPABSPATH":
                        dr["control_value"] = "";
                        break;
                    case "MANUALUPLDAUTO":
                    case "IMGMNLUPLDAUTO":
                        dr["control_value"] = "N";
                        break;
                    case "COMPXFERFILE":
                        dr["control_value"] = "Y";
                        break;
                    case "ARCHFILE":
                        dr["control_value"] = "Y";
                        break;
                    case "FTPSENDMODE":
                        dr["control_value"] = "U";
                        break;
                    case "VETURL":
                        dr["control_value"] = "https://client.vcradiology.com/vetris/";
                        break;
                    case "VETLOGINURL":
                        dr["control_value"] =  "https://client.vcradiology.com/vetris/VRSLogin.aspx";
                        break;
                   
                    default:
                        break;
                }
            }


        }
        #endregion

        #region LoadFirstWizardScreen
        private void LoadFirstWizardScreen()
        {
            if (UpdateOnly == "N")
            {
                UserControls.ucSetupWiz1 ucSetupWiz1 = new UserControls.ucSetupWiz1();
                ucSetupWiz1.Dock = DockStyle.Fill;
                ucSetupWiz1.IdentityUpdated += new UserControls.ucSetupWiz1.IdentityUpdateHandler(ButtonClicked);
                if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                pnlUC.Controls.Add(ucSetupWiz1);
            }
            else if (UpdateOnly == "Y")
            {
                UserControls.ucUpdateWiz2 ucUpdateWiz2 = new UserControls.ucUpdateWiz2();
                ucUpdateWiz2.Dock = DockStyle.Fill;
                ucUpdateWiz2.IdentityUpdated += new UserControls.ucUpdateWiz2.IdentityUpdateHandler(ButtonClicked);
                if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                pnlUC.Controls.Add(ucUpdateWiz2);
            }
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
                        case -1:
                            UserControls.ucSetupWiz2 ucSetupWiz2 = new UserControls.ucSetupWiz2();
                            ucSetupWiz2.Dock = DockStyle.Fill;
                            ucSetupWiz2.IdentityUpdated += new UserControls.ucSetupWiz2.IdentityUpdateHandler(ButtonClicked);
                            if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                            pnlUC.Controls.Add(ucSetupWiz2);
                            break;
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
                        case 4:
                            UserControls.ucInstallWiz4 ucInstallWiz4 = new UserControls.ucInstallWiz4();
                            ucInstallWiz4.Dock = DockStyle.Fill;
                            ucInstallWiz4.IdentityUpdated += new UserControls.ucInstallWiz4.IdentityUpdateHandler(ButtonClicked);
                            if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                            pnlUC.Controls.Add(ucInstallWiz4);
                            break;
                        case 5:
                            UserControls.ucInstallWiz5 ucInstallWiz5 = new UserControls.ucInstallWiz5();
                            ucInstallWiz5.Dock = DockStyle.Fill;
                            ucInstallWiz5.IdentityUpdated += new UserControls.ucInstallWiz5.IdentityUpdateHandler(ButtonClicked);
                            if (pnlUC.Controls.Count > 0) pnlUC.Controls.RemoveAt(0);
                            pnlUC.Controls.Add(ucInstallWiz5);
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
