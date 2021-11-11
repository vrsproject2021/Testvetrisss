using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VETRIS_DICOM_ROUTER_ADMIN.UserControls
{
    public partial class ucConfig : UserControl
    {
        private static string strWinHdr = "VETRIS DICOM ROUTER";

        #region Members & Variables
        VETRISRouter.Core.Scheduler objCore;
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;
        string strPACSSRVRNAME = string.Empty;
        #endregion

        public ucConfig()
        {
            InitializeComponent();
        }

        #region ucConfig_Load
        private void ucConfig_Load(object sender, EventArgs e)
        {
            dgvDevice.AllowUserToAddRows = false;
            LoadSettings();
        } 
        #endregion

        #region LoadSettings
        private void LoadSettings()
        {
            bool bReturn = false;
            string strCatchMsg = "";
            objCore = new VETRISRouter.Core.Scheduler();


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
                    LoadDevices();
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

        #region LoadDevices
        private void LoadDevices()
        {
            bool bReturn = false;
            string strCatchMsg = "";
            objCore = new VETRISRouter.Core.Scheduler();
            DataSet ds = new DataSet();
            

            try
            {

                bReturn = objCore.FetchDeviceList(Application.StartupPath,ref ds, ref strCatchMsg);
                if (bReturn)
                {
                    if (ds.Tables.Count > 0)
                    {
                        BindDevices(ds.Tables[0]);
                    }
                }
                else
                    MessageBox.Show(strCatchMsg, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception expErr)
            { MessageBox.Show(expErr.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally
            { objCore = null; ds.Dispose(); }
        }
        #endregion

        #region BindDevices
        public void BindDevices(DataTable dtbl)
        {
            dgvDevice.Columns.Clear();
            dtbl.Columns.Remove(dtbl.Columns["exeopt"]);
            dtbl.AcceptChanges();
            dgvDevice.DataSource = dtbl;

            dgvDevice.Columns[0].HeaderCell.Style.BackColor = Color.Silver;
            dgvDevice.Columns[0].Name = "id";
            dgvDevice.Columns[0].HeaderText = "Sl. #";
            dgvDevice.Columns[0].ReadOnly = true;
            dgvDevice.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvDevice.Columns[1].HeaderCell.Style.BackColor = Color.Silver;
            dgvDevice.Columns[1].Name = "aetitle";
            dgvDevice.Columns[1].HeaderText = "AE Title";
            dgvDevice.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvDevice.Columns[2].HeaderCell.Style.BackColor = Color.Silver;
            dgvDevice.Columns[2].Name = "port_no";
            dgvDevice.Columns[2].HeaderText = "Port No.";
            dgvDevice.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            DataGridViewImageColumn col = new DataGridViewImageColumn();
            col.HeaderCell.Style.BackColor = Color.Silver;
            col.Name = "del";
            col.HeaderText = "Delete";
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.Image = Image.FromFile(Application.StartupPath + "\\Configs\\delete.png");
            
            dgvDevice.Columns.Add(col);
        }
        #endregion

        #region btnAddDevice_Click
        private void btnAddDevice_Click(object sender, EventArgs e)
        {
            int intSrl = 0;
            DataTable dtbl = CreateDeviceTable();
            
            for (int i = 0; i < dgvDevice.RowCount; i++)
            {
                DataRow dr = dtbl.NewRow();
                intSrl = i + 1;
                dr["id"] = intSrl;
                dr["aetitle"] = dgvDevice.Rows[i].Cells[2].Value.ToString().Trim();
                dr["port_no"] = dgvDevice.Rows[i].Cells[3].Value.ToString().Trim();
                dtbl.Rows.Add(dr);
            }

            DataRow drNew = dtbl.NewRow();
            intSrl = intSrl + 1;
            drNew["id"] = intSrl;
            drNew["aetitle"] = "";
            drNew["port_no"] = "";
            dtbl.Rows.Add(drNew);

            BindDevices(dtbl);
        } 
        #endregion

        #region CreateDeviceTable
        private DataTable CreateDeviceTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("aetitle", System.Type.GetType("System.String"));
            dtbl.Columns.Add("port_no", System.Type.GetType("System.String"));
            dtbl.Columns.Add("exeopt", System.Type.GetType("System.String"));
            return dtbl;
        }
        #endregion

        #region dgvDevice_CellClick
        private void dgvDevice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool bReturn = false;
            string strCatchMessage = "";
            objCore = new VETRISRouter.Core.Scheduler();
            string strErr = string.Empty;
            int intID = 0;
            int intRowIndex = 0;
            int intSrl = 0;
            DataSet ds = new DataSet("DeviceData");
            DataTable dtbl = CreateDeviceTable();

            if (e.ColumnIndex == 0)
            {
                try
                {
                    intRowIndex = dgvDevice.SelectedCells[0].RowIndex;
                    intID = Convert.ToInt32(dgvDevice.Rows[intRowIndex].Cells[1].Value.ToString().Trim());

                    for (int i = 0; i < dgvDevice.RowCount; i++)
                    {
                        if (Convert.ToInt32(dgvDevice.Rows[i].Cells[1].Value.ToString().Trim()) != intID)
                        {
                            intSrl = intSrl + 1;
                            DataRow dr = dtbl.NewRow();
                            dr["aetitle"] = dgvDevice.Rows[i].Cells["aetitle"].Value.ToString().Trim();
                            dr["port_no"] = dgvDevice.Rows[i].Cells["port_no"].Value.ToString().Trim();
                            dr["exeopt"] = "-v " + dgvDevice.Rows[i].Cells["port_no"].Value.ToString().Trim() + " xs -aet " + dgvDevice.Rows[i].Cells["aetitle"].Value.ToString().Trim() + " - od"; 
                            dtbl.Rows.Add(dr);
                        }
                    }

                    dtbl.TableName = "Device";
                    ds.Tables.Add(dtbl);

                    bReturn = objCore.SaveDevices(Application.StartupPath, ds, ref strCatchMessage);
                    if (!bReturn) { strErr += "Device Record(s) :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; }
                    else { BindDevices(ds.Tables[0]); }
                    ds.Dispose();
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
        }
        #endregion

        #region btnSave_Click
        private void btnSave_Click(object sender, EventArgs e)
        {
            bool bReturn = false;
            string strCatchMessage = "";
            objCore = new VETRISRouter.Core.Scheduler();
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
                   
                    //objCore.RECEIVER_EXE_OPTIONS = "-v -fe .dcm -aet " + txtRCVAETITLE.Text.Trim() + " " + txtRCVPORTNO.Text.Trim() + " -od";
                    //objCore.RECEIVER_EXE_OPTIONS = "-v " + txtRCVPORTNO.Text.Trim() + " +xs -aet " + txtRCVAETITLE.Text.Trim() + " -od";
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

                    #region SaveDevices
                    DataSet ds = new DataSet("DeviceData");
                    DataTable dtbl = CreateDeviceTable();
                    int intSrl = 0;

                    for (int i = 0; i < dgvDevice.RowCount; i++)
                    {
                        DataRow dr = dtbl.NewRow();
                        intSrl = i + 1;
                        dr["id"] = intSrl;
                        dr["aetitle"] = dgvDevice.Rows[i].Cells["aetitle"].Value.ToString().Trim();
                        dr["port_no"] = dgvDevice.Rows[i].Cells["port_no"].Value.ToString().Trim();
                        dr["exeopt"] = "-v " + dgvDevice.Rows[i].Cells["port_no"].Value.ToString().Trim() + " xs -aet " + dgvDevice.Rows[i].Cells["aetitle"].Value.ToString().Trim() + " - od"; 
                        dtbl.Rows.Add(dr);
                        
                    }
                    dtbl.TableName = "Device";
                    ds.Tables.Add(dtbl);
                    

                    bReturn = objCore.SaveDevices(Application.StartupPath, ds, ref strCatchMessage);
                    if (!bReturn) { strErr += "Device Record(s) :: " + strCatchMessage + "\r\n"; strCatchMessage = ""; } else { BindDevices(ds.Tables[0]); }
                    ds.Dispose();

                    #endregion

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







    }
}
