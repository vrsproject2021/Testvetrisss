using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using VETRISRouter.Core;

namespace VETRIS_DICOM_ROUTER_ADMIN.UserControls
{
    public partial class ucSettings : UserControl
    {
        private static string strWinHdr = "VETRIS DICOM ROUTER";// ConfigurationSettings.AppSettings["WinHdr"];

        #region Members & Variables
        VETRISRouter.Core.Scheduler objCore;
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;
        string strPACSSRVRNAME = string.Empty;
        #endregion

        public ucSettings()
        {
            InitializeComponent();
        }

        #region ucSettings_Load
        private void ucSettings_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }
        #endregion

        #region LoadSettings
        private void LoadSettings()
        {
            bool bReturn = false;
            string strCatchMsg = "";
            objCore = new Scheduler();


            try
            {

                bReturn = objCore.FetchSchedulerSettings(Application.StartupPath, ref strCatchMsg);
                if (bReturn)
                {
                    lblInstName.Text = objCore.INSTITUTION_NAME;
                    lblSiteCode.Text = "( Site Code : " + objCore.SITE_CODE + " )";
                    lblAddr1.Text = objCore.INSTITUTION_ADDRESS_1;
                    lblAddr2.Text = objCore.INSTITUTION_ADDRESS_2;
                    lblZip.Text = "Zip : " + objCore.INSTITUTION_ZIP;
                    txtRCVAETITLE.Text = objCore.RECEIVER_AETITLE;
                    txtRCVPORTNO.Text = objCore.RECEIVER_PORT_NO;
                    txtRcvFolder.Text = objCore.RECEIVING_DIRECTORY;
                    txtRCVDIRMANUAL.Text = objCore.RECEIVING_DIRECTORY_FOR_MANUAL_UPLOAD;
                    if (objCore.RECEIVING_DIRECTORY_AUTO_DETECT == "Y") chkAutoDetect.Checked = true;
                    strPACSSRVRNAME = objCore.PACS_SERVER_NAME;
                    txtSendFolder.Text = objCore.SENDER_DIRECTORY;
                    txtArchFolder.Text = objCore.ARCHIVE_DIRECTORY;
                    if (objCore.COMPRESS_FILES_TO_TRANSFER == "Y") chkCompFile.Checked = true; else chkCompFile.Checked = false;
                    if (objCore.ARCHIVE_FILES_TRANSFERED == "Y") chkArch.Checked = true; else chkArch.Checked = false;
                    txtRCVIMGDIR.Text = objCore.RECEIVING_DIRECTORY_FOR_IMAGES;
                    if (objCore.RECEIVING_DIRECTORY_FOR_IMAGES_AUTO_DETECT == "Y") chkImgAutoDetect.Checked = true;
                    txtVETLOGIN.Text = objCore.VETRIS_LOGIN_ID;
                    txtVETURL.Text = objCore.VETRIS_URL;

                    if (objCore.FTP_SENDING_MODE == "U")
                    {
                        rdoUpload.Checked = true;
                        btnFTPAbsPath.Enabled = false;
                        txtFTPAbsPath.Text = "";
                    }
                    else if (objCore.FTP_SENDING_MODE == "C")
                    {
                        rdoCopy.Checked = true;
                        btnFTPAbsPath.Enabled = true;
                    }
                    txtFTPAbsPath.Text = objCore.FTP_ABSOLUTE_PATH;
                }
                else
                    MessageBox.Show(strCatchMsg, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception expErr)
            { MessageBox.Show(expErr.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally
            { objCore = null; }
        }
        #endregion

        #region btnSave_Click
        private void btnSave_Click(object sender, EventArgs e)
        {
            bool bReturn = false;
            string strCatchMessage = "";
            objCore = new Scheduler();
            string strErr = string.Empty;
            int intFlg = 1;

            try
            {
                #region Validation
                if (txtRcvFolder.Text.Trim() == txtSendFolder.Text.Trim())
                {
                    intFlg = 0;
                    MessageBox.Show("Receiving Folder Path cannot be same as Sending Folder Path", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (txtRcvFolder.Text.Trim() == txtRCVDIRMANUAL.Text.Trim())
                {

                    intFlg = 0;
                    MessageBox.Show("Receiving Folder Path cannot be same as Receiving Folder Path For Files Uploaded Manually", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (txtSendFolder.Text.Trim() == txtRCVDIRMANUAL.Text.Trim())
                {

                    intFlg = 0;
                    MessageBox.Show("Sending Folder Path cannot be same as Receiving Folder Path For Files Uploaded Manually", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                #endregion

                if (intFlg == 1)
                {
                    objCore.RECEIVER_AETITLE = txtRCVAETITLE.Text.Trim();
                    objCore.RECEIVER_PORT_NO = txtRCVPORTNO.Text.Trim();
                    //objCore.RECEIVER_EXE_OPTIONS = "-v -fe .dcm -aet " + txtRCVAETITLE.Text.Trim() + " " + txtRCVPORTNO.Text.Trim() + " -od";
                    objCore.RECEIVER_EXE_OPTIONS = "-v " + txtRCVPORTNO.Text.Trim()  + " +xs -aet " + txtRCVAETITLE.Text.Trim() + " -od";
                    objCore.RECEIVING_DIRECTORY = txtRcvFolder.Text.Trim();
                    objCore.RECEIVING_DIRECTORY_FOR_MANUAL_UPLOAD = txtRCVDIRMANUAL.Text.Trim();
                    if (chkAutoDetect.Checked) objCore.RECEIVING_DIRECTORY_AUTO_DETECT = "Y"; else objCore.RECEIVING_DIRECTORY_AUTO_DETECT = "N";
                    objCore.RECEIVING_DIRECTORY_FOR_IMAGES = txtRCVIMGDIR.Text.Trim();
                    if (chkImgAutoDetect.Checked) objCore.RECEIVING_DIRECTORY_FOR_IMAGES_AUTO_DETECT = "Y"; else objCore.RECEIVING_DIRECTORY_FOR_IMAGES_AUTO_DETECT = "N";
                    //objCore.SENDER_AETITLE = txtSNDAETITLE.Text.Trim();
                    //objCore.PACS_SERVER_NAME = txtPACSSRVRNAME.Text.Trim();
                    //objCore.SENDER_PORT_NO = txtSNDPORTNO.Text.Trim();
                    //objCore.SENDER_OPTIONS = "-v +sd +r -aec " + txtSNDAETITLE.Text + " " + strPACSSRVRNAME + " " + txtSNDPORTNO.Text ;
                    objCore.SENDER_DIRECTORY = txtSendFolder.Text;
                    objCore.ARCHIVE_DIRECTORY = txtArchFolder.Text;
                    objCore.VETRIS_LOGIN_ID = txtVETLOGIN.Text.Trim();
                    objCore.VETRIS_URL = txtVETURL.Text.Trim();
                    if (chkCompFile.Checked) objCore.COMPRESS_FILES_TO_TRANSFER = "Y"; else objCore.COMPRESS_FILES_TO_TRANSFER = "N";
                    if (chkArch.Checked) objCore.ARCHIVE_FILES_TRANSFERED = "Y"; else objCore.ARCHIVE_FILES_TRANSFERED = "N";
                    if (rdoUpload.Checked) objCore.FTP_SENDING_MODE = "U"; else if (rdoCopy.Checked) objCore.FTP_SENDING_MODE = "C";
                    objCore.FTP_ABSOLUTE_PATH = txtFTPAbsPath.Text.Trim();

                    bReturn = objCore.SaveSchedulerSettings(Application.StartupPath, "RCVAETITLE", objCore.RECEIVER_AETITLE, ref strCatchMessage);
                    if (!bReturn) { strErr += "Receiver AE Title :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }

                    bReturn = objCore.SaveSchedulerSettings(Application.StartupPath, "RCVPORTNO", objCore.RECEIVER_PORT_NO, ref strCatchMessage);
                    if (!bReturn) { strErr += "Receiver Port No. :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }

                    bReturn = objCore.SaveSchedulerSettings(Application.StartupPath, "RCVEXEOPTIONS", objCore.RECEIVER_EXE_OPTIONS, ref strCatchMessage);
                    if (!bReturn) { strErr += "Receiver Exe Options :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }

                    bReturn = objCore.SaveSchedulerSettings(Application.StartupPath, "RCVDIR", objCore.RECEIVING_DIRECTORY, ref strCatchMessage);
                    if (!bReturn) { strErr += "Receiving Folder Path :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }

                    bReturn = objCore.SaveSchedulerSettings(Application.StartupPath, "RCVDIRMANUAL", objCore.RECEIVING_DIRECTORY_FOR_MANUAL_UPLOAD, ref strCatchMessage);
                    if (!bReturn) { strErr += "Receiving Folder Path For DICOM Files Uploaded Manually :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }

                    bReturn = objCore.SaveSchedulerSettings(Application.StartupPath, "MANUALUPLDAUTO", objCore.RECEIVING_DIRECTORY_AUTO_DETECT, ref strCatchMessage);
                    if (!bReturn) { strErr += "Receiving Folder Path For DICOM Files Uploaded Manually - Detect Automatically :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }

                    bReturn = objCore.SaveSchedulerSettings(Application.StartupPath, "RCVIMGDIR", objCore.RECEIVING_DIRECTORY_FOR_IMAGES, ref strCatchMessage);
                    if (!bReturn) { strErr += "Receiving Folder Path For Image Files Uploaded Manually :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }

                    bReturn = objCore.SaveSchedulerSettings(Application.StartupPath, "IMGMNLUPLDAUTO", objCore.RECEIVING_DIRECTORY_FOR_IMAGES_AUTO_DETECT, ref strCatchMessage);
                    if (!bReturn) { strErr += "Receiving Folder Path For Image Files Uploaded Manually - Detect Automatically :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }

                    //bReturn = objCore.SaveSchedulerSettings(Application.StartupPath, "SNDAETITLE", objCore.SENDER_AETITLE, ref strCatchMessage);
                    //if (!bReturn) { strErr += "Sending AE Title :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }

                    //bReturn = objCore.SaveSchedulerSettings(Application.StartupPath, "PACSSRVRNAME", objCore.PACS_SERVER_NAME, ref strCatchMessage);
                    //if (!bReturn) { strErr += "PACS Server Name/IP :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }

                    //bReturn = objCore.SaveSchedulerSettings(Application.StartupPath, "SNDPORTNO", objCore.SENDER_PORT_NO, ref strCatchMessage);
                    //if (!bReturn) { strErr += "Sending Port No. :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }

                    //bReturn = objCore.SaveSchedulerSettings(Application.StartupPath, "SNDEXEOPTIONS", objCore.SENDER_OPTIONS, ref strCatchMessage);
                    //if (!bReturn) { strErr += "Sending Exe Options :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }

                    bReturn = objCore.SaveSchedulerSettings(Application.StartupPath, "SNDDIR", objCore.SENDER_DIRECTORY, ref strCatchMessage);
                    if (!bReturn) { strErr += "Sending Folder Path :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }

                    bReturn = objCore.SaveSchedulerSettings(Application.StartupPath, "ARCHDIR", objCore.ARCHIVE_DIRECTORY, ref strCatchMessage);
                    if (!bReturn) { strErr += "Archive Folder Path :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }

                    bReturn = objCore.SaveSchedulerSettings(Application.StartupPath, "VETLOGIN", objCore.VETRIS_LOGIN_ID, ref strCatchMessage);
                    if (!bReturn) { strErr += "VETRIS Login ID :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }

                    bReturn = objCore.SaveSchedulerSettings(Application.StartupPath, "VETURL", objCore.VETRIS_URL, ref strCatchMessage);
                    if (!bReturn) { strErr += "VETRIS URL :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }

                    bReturn = objCore.SaveSchedulerSettings(Application.StartupPath, "COMPXFERFILE", objCore.COMPRESS_FILES_TO_TRANSFER, ref strCatchMessage);
                    if (!bReturn) { strErr += "Compress files while sending to PACS :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }

                    bReturn = objCore.SaveSchedulerSettings(Application.StartupPath, "FTPSENDMODE", objCore.FTP_SENDING_MODE, ref strCatchMessage);
                    if (!bReturn) { strErr += "Send files to FTP folder :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }

                    bReturn = objCore.SaveSchedulerSettings(Application.StartupPath, "FTPABSPATH", objCore.FTP_ABSOLUTE_PATH, ref strCatchMessage);
                    if (!bReturn) { strErr += "FTP folder absolute path :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }

                    if (strErr.Trim() == string.Empty)
                    {
                        MessageBox.Show("Details Saved Successfully", strWinHdr + " : Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //txtSNDAETITLE.Enabled = false;
                        //txtSNDPORTNO.Enabled = false;
                        //txtPACSSRVRNAME.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show(strErr, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                }


            }
            catch (Exception LexpErr)
            {
                MessageBox.Show(LexpErr.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                objCore = null; strCatchMessage = null;
            }
        }
        #endregion

        #region btnClose_Click
        private void btnClose_Click(object sender, EventArgs e)
        {
            string strMsg = string.Empty; string _Stat = string.Empty;
            _Stat = "Cancel";
            ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat);
            IdentityUpdated(this, args);
        }
        #endregion

        #region btnRcvFolder_Click
        private void btnRcvFolder_Click(object sender, EventArgs e)
        {

            string strWAP = string.Empty;
            strWAP = OpenFolderDialouge(txtRcvFolder.Text, "Receiving Folder Path (Default)");
            txtRcvFolder.Text = strWAP;
        }
        #endregion

        #region btnRcvFldrManual_Click
        private void btnRcvFldrManual_Click(object sender, EventArgs e)
        {
            string strWAP = string.Empty;
            strWAP = OpenFolderDialouge(txtRCVDIRMANUAL.Text, "Receiving Folder Path For DICOM Files Uploaded Manually");
            txtRCVDIRMANUAL.Text = strWAP;
        }
        #endregion

        #region btnRCVIMGDIR_Click
        private void btnRCVIMGDIR_Click(object sender, EventArgs e)
        {
            string strWAP = string.Empty;
            strWAP = OpenFolderDialouge(txtRCVDIRMANUAL.Text, "Receiving Folder Path For Image Files Uploaded Manually");
            txtRCVIMGDIR.Text = strWAP;
        }
        #endregion

        #region btnSndFolder_Click
        private void btnSndFolder_Click(object sender, EventArgs e)
        {
            string strWAP = string.Empty;
            strWAP = OpenFolderDialouge(txtSendFolder.Text, "Sending Folder Path");
            txtSendFolder.Text = strWAP;

        }
        #endregion

        #region btnArchFolder_Click
        private void btnArchFolder_Click(object sender, EventArgs e)
        {
            string strWAP = string.Empty;
            strWAP = OpenFolderDialouge(txtArchFolder.Text, "Archive Folder Path");
            txtArchFolder.Text = strWAP;
        }
        #endregion

        #region btnFTPAbsPath_Click
        private void btnFTPAbsPath_Click(object sender, EventArgs e)
        {
            string strWAP = string.Empty;
            strWAP = OpenFolderDialouge(txtArchFolder.Text, "FTP Folder Absolute Path");
            txtFTPAbsPath.Text = strWAP;
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

        #region rdoUpload_Click
        private void rdoUpload_Click(object sender, EventArgs e)
        {
            if (rdoUpload.Checked)
            {
                btnFTPAbsPath.Enabled = false;
                txtFTPAbsPath.Text = "";
            }
        } 
        #endregion

        #region rdoCopy_Click
        private void rdoCopy_Click(object sender, EventArgs e)
        {
            if (rdoCopy.Checked) btnFTPAbsPath.Enabled = true;
        } 
        #endregion

        

    }
}
