using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace DICOMRouterInstaller.UserControls
{
    public partial class ucInstallWiz2 : UserControl
    {
        #region Members & Variables
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;
        #endregion

        public ucInstallWiz2()
        {
            InitializeComponent();
        }

        #region ucInstallWiz2_Load
        private void ucInstallWiz2_Load(object sender, EventArgs e)
        {
            PopulateValues();
            PopulateDefaults();
            btnInstPath.Focus();
        } 
        #endregion

        #region PopulateValues
        private void PopulateValues()
        {
            string strCode = string.Empty;
            txtInstPath.Text = frmMain.InstallPath;

            foreach (DataRow dr in frmMain.dtbl.Rows)
            {
                strCode = Convert.ToString(dr["control_code"]);

                switch (strCode)
                {
                    case "RCVDIR":
                        txtRCVDIR.Text = Convert.ToString(dr["control_value"]).Trim();
                        break;
                    case "RCVDIRMANUAL":
                        txtRCVDIRMANUAL.Text = Convert.ToString(dr["control_value"]).Trim();
                        break;
                    case "MANUALUPLDAUTO":
                        if (Convert.ToString(dr["control_value"]).Trim() == "Y") chkAutoDetect.Checked = true;
                        break;
                    case "RCVIMGDIR":
                        txtRCVIMGDIR.Text = Convert.ToString(dr["control_value"]).Trim();
                        break;
                    case "IMGMNLUPLDAUTO":
                        if (Convert.ToString(dr["control_value"]).Trim() == "Y") chkImgAutoDetect.Checked = true;
                        break;
                    case "SNDDIR":
                        txtSNDDIR.Text = Convert.ToString(dr["control_value"]).Trim();
                        break;
                    case "ARCHDIR":
                        txtARCHDIR.Text = Convert.ToString(dr["control_value"]).Trim();
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        #region PopulateDefaults
        private void PopulateDefaults()
        {
            string[] arr = new string[0];
            arr = Application.StartupPath.Split('\\');
                
            if (txtInstPath.Text.Trim() == string.Empty)
            {
                txtInstPath.Text = arr[0].Trim() + "\\VCDICOMROUTER";
            }
            if (txtRCVDIR.Text.Trim() == string.Empty)
            {
                txtRCVDIR.Text = arr[0].Trim() + "\\VCDICOMDATA\\DCMReceived";
            }
            if (txtRCVDIRMANUAL.Text.Trim() == string.Empty)
            {
                txtRCVDIRMANUAL.Text = arr[0].Trim() + "\\VCDICOMDATA\\DCMReceivedManual";
            }
            if (txtRCVIMGDIR.Text.Trim() == string.Empty)
            {
                txtRCVIMGDIR.Text = arr[0].Trim() + "\\VCDICOMDATA\\DCMImage";
            }
            if (txtSNDDIR.Text.Trim() == string.Empty)
            {
                txtSNDDIR.Text = arr[0].Trim() + "\\VCDICOMDATA\\DCMSent";
            }
            if (txtARCHDIR.Text.Trim() == string.Empty)
            {
                txtARCHDIR.Text = arr[0].Trim() + "\\VCDICOMDATA\\DCMArchived";
            }
        }
        #endregion

        #region btnPrev_Click
        private void btnPrev_Click(object sender, EventArgs e)
        {
            string _Stat = string.Empty;
            int _Screen = 0;

            UpdateValues();
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
                UpdateValues();
                _Stat = "Install";
                _Screen = 3;
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
            string strFolder= string.Empty;
            string strXferFolder = string.Empty;
            string[] arrPath = new string[0];

            try
            {
                if (txtInstPath.Text.Trim() == string.Empty)
                {
                    strMsg = "Installation Path is required";
                }
                else
                {
                    if (!Directory.Exists(txtInstPath.Text.Trim()))
                    {
                        //strMsg = "Invalid 'Installation Path' specified";
                        Directory.CreateDirectory(txtInstPath.Text.Trim());
                    }
                }

                if (txtRCVDIR.Text.Trim() == string.Empty)
                {
                    if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
                    strMsg += "Receiving Folder Path (Default) is required";
                }
                else
                {
                    if (!Directory.Exists(txtRCVDIR.Text.Trim()))
                    {
                        Directory.CreateDirectory(txtRCVDIR.Text.Trim());
                        //if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
                        //strMsg += "Invalid 'Receiving Folder Path (Default)' specified";
                    }

                    strFolder = txtRCVDIR.Text.Trim();
                    arrPath = strFolder.Split('\\');
                    if (arrPath.Length == 1)
                        strXferFolder = strFolder;
                    else
                    {
                        for (int i = 0; i < arrPath.Length - 1; i++)
                        {
                            if (strXferFolder.Trim() != string.Empty) strXferFolder += "\\";
                            strXferFolder += arrPath[i];
                        }
                       
                    }

                    if (!Directory.Exists(strXferFolder)) Directory.CreateDirectory(strXferFolder);
                }

                if (txtRCVDIRMANUAL.Text.Trim() == string.Empty)
                {
                    if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
                    strMsg += "Receiving Folder Path (for files uploaded manually) is required";
                }
                else
                {
                    if (chkAutoDetect.Checked == false)
                    {
                        if (!Directory.Exists(txtRCVDIRMANUAL.Text.Trim()))
                        {
                            //if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
                            //strMsg += "Invalid 'Receiving Folder Path (for files uploaded manually)' specified";
                            Directory.CreateDirectory(txtRCVDIRMANUAL.Text.Trim());
                        }
                    }
                }

                if (txtRCVIMGDIR.Text.Trim() == string.Empty)
                {
                    if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
                    strMsg += "Receiving Folder Path (for image files uploaded manually) is required";
                }
                else
                {
                    if (!Directory.Exists(txtRCVIMGDIR.Text.Trim()))
                    {
                        //if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
                        //strMsg += "Invalid 'Receiving Folder Path (for image files uploaded manually)' specified";
                        Directory.CreateDirectory(txtRCVIMGDIR.Text.Trim());
                    }
                }


                if (txtSNDDIR.Text.Trim() == string.Empty)
                {
                    if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
                    strMsg += "Sending Folder Path is required";
                }
                else
                {
                    if (!Directory.Exists(txtSNDDIR.Text.Trim()))
                    {
                        //if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
                        //strMsg += "Invalid 'Sending Folder Path' specified";
                        Directory.CreateDirectory(txtSNDDIR.Text.Trim());
                    }
                }

                if (txtARCHDIR.Text.Trim() == string.Empty)
                {
                    if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
                    strMsg += "Archive Folder Path (files to be archived after sending) is required";
                }
                else
                {
                    if (!Directory.Exists(txtARCHDIR.Text.Trim()))
                    {
                        //if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
                        //strMsg += "Invalid 'Archive Folder Path (files to be archived after sending)' specified";
                        Directory.CreateDirectory(txtARCHDIR.Text.Trim());
                    }
                }

                if ((txtSNDDIR.Text.Trim() != string.Empty) && (txtRCVDIR.Text.Trim() != string.Empty))
                {
                    if (txtSNDDIR.Text.Trim() == txtRCVDIR.Text.Trim())
                    {
                        if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
                        strMsg += "Receiving Folder Path (Default) cannot be same as Sending Folder Path";
                    }
                }

                if ((txtARCHDIR.Text.Trim() != string.Empty) && (txtRCVDIR.Text.Trim() != string.Empty))
                {
                    if (txtARCHDIR.Text.Trim() == txtRCVDIR.Text.Trim())
                    {
                        if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
                        strMsg += "Receiving Folder Path (Default) cannot be same as Archive Folder Path (files to be archived after sending)";
                    }
                }

                if ((txtSNDDIR.Text.Trim() != string.Empty) && (txtARCHDIR.Text.Trim() != string.Empty))
                {
                    if (txtSNDDIR.Text.Trim() == txtRCVDIR.Text.Trim())
                    {
                        if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
                        strMsg += "Sending Folder Path cannot be same as Archive Folder Path (files to be archived after sending)";
                    }
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
            }
            catch (Exception ex)
            {
                bRet = false;
                MessageBox.Show(ex.Message.Trim(), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return bRet;
        }
        #endregion

        #region UpdateValues
        private void UpdateValues()
        {
            string strCode = string.Empty;
            foreach (DataRow dr in frmMain.dtbl.Rows)
            {
                strCode = Convert.ToString(dr["control_code"]);

                switch (strCode)
                {
                    case "RCVDIR":
                        dr["control_value"] = txtRCVDIR.Text.Trim();
                        break;
                    case "RCVDIRMANUAL":
                        dr["control_value"] = txtRCVDIRMANUAL.Text.Trim();
                        break;
                    case "MANUALUPLDAUTO":
                        if(chkAutoDetect.Checked) dr["control_value"] = "Y";
                        else dr["control_value"] = "N";
                        break;
                    case "RCVIMGDIR":
                        dr["control_value"] = txtRCVIMGDIR.Text.Trim();
                        break;
                    case "IMGMNLUPLDAUTO":
                        if (chkImgAutoDetect.Checked) dr["control_value"] = "Y";
                        else dr["control_value"] = "N";
                        break;
                    case "SNDDIR":
                        dr["control_value"] = txtSNDDIR.Text.Trim();
                        break;
                    case "ARCHDIR":
                        dr["control_value"] = txtARCHDIR.Text.Trim();
                        break;
                    case "ACCESSORYDIR":
                        dr["control_value"] = txtInstPath.Text.Trim() + "\\Configs\\";
                        break;
                    default:
                        break;
                }
            }

            frmMain.dtbl.AcceptChanges();
            frmMain.InstallPath = txtInstPath.Text.Trim();
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
            txtInstPath.Text = strWAP;
        } 
        #endregion

        #region btnRCVDIR_Click
        private void btnRCVDIR_Click(object sender, EventArgs e)
        {
            string strWAP = string.Empty;
            strWAP = OpenFolderDialouge(txtRCVDIR.Text, "Receiving Folder Path (Default)");
            txtRCVDIR.Text = strWAP;
            if (txtRCVDIRMANUAL.Text.Trim() == string.Empty) txtRCVDIRMANUAL.Text = strWAP;
        } 
        #endregion

        #region btnRCVDIRMANUAL_Click
        private void btnRCVDIRMANUAL_Click(object sender, EventArgs e)
        {
            string strWAP = string.Empty;
            strWAP = OpenFolderDialouge(txtRCVDIRMANUAL.Text, "Receiving Folder Path (For Files Uploaded Manually)");
            txtRCVDIRMANUAL.Text = strWAP;
        } 
        #endregion

        #region btnRCVIMGDIR_Click
        private void btnRCVIMGDIR_Click(object sender, EventArgs e)
        {
            string strWAP = string.Empty;
            strWAP = OpenFolderDialouge(txtRCVIMGDIR.Text, "Receiving Folder Path (For Image Files Uploaded Manually)");
            txtRCVIMGDIR.Text = strWAP;
        } 
        #endregion

        #region btnSNDDIR_Click
        private void btnSNDDIR_Click(object sender, EventArgs e)
        {
            string strWAP = string.Empty;
            strWAP = OpenFolderDialouge(txtSNDDIR.Text, "Sending Folder Path");
            txtSNDDIR.Text = strWAP;
        } 
        #endregion

        #region btnARCHDIR_Click
        private void btnARCHDIR_Click(object sender, EventArgs e)
        {
            string strWAP = string.Empty;
            strWAP = OpenFolderDialouge(txtARCHDIR.Text, "Archive Folder Path");
            txtARCHDIR.Text = strWAP;
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
