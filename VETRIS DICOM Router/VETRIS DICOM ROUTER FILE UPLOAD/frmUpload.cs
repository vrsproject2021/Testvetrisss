using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.Configuration;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using VETRISRouter.Core;
using DICOMLib;

namespace VETRIS_DICOM_ROUTER_FILE_UPLOAD
{
    public partial class frmUpload : Form
    {
        #region Load Variables
        private static string FTPHOST = string.Empty;
        private static string FTPPORT = string.Empty;
        private static string FTPUSER = string.Empty;
        private static string FTPPWD = string.Empty;
        private static string DRSDWLFLDR = string.Empty;
        private static string VETAPIURL = string.Empty;
        private static string VETURL = string.Empty;
        private static string VETLOGINURL = string.Empty;
        private static string ARCHDIR = string.Empty;

        bool toggleLight = true;
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        int tickCount = 0;
        string LatestVer = string.Empty;
        string CurrentVer = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        #endregion

        #region Upload Variables
        private static string strWinHdr = "VETRIS DICOM ROUTER";
        private static string strManualUploadDirPath = string.Empty;
        private static string strRecDirPath = string.Empty;
        private static string strSendDirPath = string.Empty;
        private static string strInstCode = string.Empty;
        private static string strInstName = string.Empty;
        private static string strVETLogin = string.Empty;
        private static string strVETLOGINURL = string.Empty;
        private static string strVETAPIURL = string.Empty;
        private static string strARCHFILE = "Y";

        string DriveName = string.Empty;
        Thread threadInput;
        public delegate string WaitDelegate(string name);
        public enum ServiceType { DCMRCV = 1, DCMSND = 2 }
        public int ServiceTypeId = 0;
        #endregion

        #region Members & Variables
        Scheduler objCore;
        DicomDecoder dd;
        DataTable dtblFiles = new DataTable();
        string strUploadType = "M";
        static OpenFileDialog Fdialog;

        const int ERROR_SHARING_VIOLATION = 32;
        const int ERROR_LOCK_VIOLATION = 33;
        #endregion

        public frmUpload()
        {
            InitializeComponent();
        }

        #region frmUpload_Load
        private void frmUpload_Load(object sender, EventArgs e)
        {
            this.Text = "VETRIS DICOM ROUTER";
            lblVer.Text = "Version : " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();


            Application.DoEvents();
            #region set auto panel
            lblpbInfo.Top = gbAuto.Bottom + 10;
            pb1.Top = lblpbInfo.Bottom + 10;

            dgvPatient.Top = dgvFiles.Top;
            dgvPatient.Left = dgvFiles.Left;
            dgvPatient.Width = dgvFiles.Width;
            dgvPatient.Height = dgvFiles.Height;

            dgvImg.Top = dgvFiles.Top;
            dgvImg.Left = dgvFiles.Left;
            dgvImg.Width = dgvFiles.Width;
            dgvImg.Height = dgvFiles.Height;

            btnClose.Top = lblpbInfo.Bottom + 10;
            rdoPatient.Checked = true;
            dgvFiles.Visible = false;
            dgvImg.Visible = false;
            #endregion

            #region set manual panel
            gbManual.Visible = true;
            gbManual.Top = gbAuto.Top;
            gbManual.Left = gbManual.Left;
            lstFiles.Items.Clear();
            #endregion
            // DeleteTempFiles();

            Application.DoEvents();
            GetSettings();
            GetVersion();

            dtblFiles.Columns.Add("patient_name", System.Type.GetType("System.String"));
            dtblFiles.Columns.Add("file_name", System.Type.GetType("System.String"));

            LoadFileTypes();

            chkShowProg.Checked = true;
            Application.DoEvents();
        }
        #endregion

        #region frmUpload_Shown
        private void frmUpload_Shown(object sender, EventArgs e)
        {

        }
        #endregion

        #region DeleteTempFiles
        private void DeleteTempFiles()
        {

            string[] arrFiles = new string[0];
            string strFilePath = string.Empty;

            try
            {
                if (Directory.Exists(strSendDirPath + "\\Temp"))
                {
                    arrFiles = Directory.GetFiles(strSendDirPath + "\\Temp");
                    foreach (string strFile in arrFiles)
                    {
                        strFilePath = strFile;
                        File.Delete(strFile);
                    }
                }
            }
            catch (IOException ex)
            {
                ;
                //if (IsFileLocked(ex))
                //{
                //    UnlockFileProcess(strFilePath);
                //}
            }
            catch (Exception ex)
            {
                ;
            }
        }
        #endregion

        #region GetSettings
        private void GetSettings()
        {
            string strCatchMessage = string.Empty;
            objCore = new Scheduler();

            try
            {
                if (objCore.FetchSchedulerSettings(Application.StartupPath, ref strCatchMessage))
                {
                    FTPHOST = objCore.FTP_HOST_NAME;
                    FTPUSER = objCore.FTP_USER_NAME;
                    FTPPWD = objCore.FTP_PASSWORD;
                    VETAPIURL = objCore.VETRIS_API_URL;
                    VETURL = objCore.VETRIS_URL;
                    VETLOGINURL = objCore.VETRIS_LOGIN_URL;
                    DRSDWLFLDR = objCore.FTP_DICOM_ROUTER_DOWNLOAD_FOLDER;
                    ARCHDIR = objCore.ARCHIVE_DIRECTORY;

                    strManualUploadDirPath = objCore.RECEIVING_DIRECTORY_FOR_MANUAL_UPLOAD;
                    strRecDirPath = objCore.RECEIVING_DIRECTORY;
                    strSendDirPath = objCore.SENDER_DIRECTORY;
                    strInstCode = objCore.SITE_CODE;
                    strInstName = objCore.INSTITUTION_NAME;
                    strVETLogin = objCore.VETRIS_LOGIN_ID;
                    strVETLOGINURL = objCore.VETRIS_LOGIN_URL;
                    strVETAPIURL = objCore.VETRIS_API_URL;
                    strARCHFILE = objCore.ARCHIVE_FILES_TRANSFERED;
                }
                else
                    MessageBox.Show(strCatchMessage, this.Text + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text + " : Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                objCore = null;
            }
        }
        #endregion

        #region GetVersion
        private void GetVersion()
        {

            string strRespMsg = string.Empty;
            string apiUrl = VETAPIURL;
            string json = string.Empty;
            WebClient client = new WebClient();

            try
            {

                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                json = client.UploadString(apiUrl + "/DicomRouterLatestVersion", string.Empty);

                JavaScriptSerializer ser = new JavaScriptSerializer();
                AppClasses.DicomRouterLatestVersion Ver = ser.Deserialize<AppClasses.DicomRouterLatestVersion>(json);

                strRespMsg = Ver.responseStatus.responseMessage;


                if (strRespMsg.Trim() == "SUCCESS")
                {
                    LatestVer = Ver.LatestVersion;
                    btnDownload.Text = btnDownload.Text + " " + LatestVer;

                    if (LatestVer != CurrentVer)
                    {
                        //btnDownload.Visible = true;
                        //t.Interval = 100;
                        //t.Tick += new EventHandler(t_Tick);
                        //t.Start();

                        frmDownload frmDL = new frmDownload();
                        frmDL.LATEST_VERSION = LatestVer;
                        frmDL.ARCHIVE_DIRECTORY = ARCHDIR;
                        frmDL.VETRIS_URL = VETURL;
                        frmDL.ShowDialog();
                    }
                    else
                        LatestVer = CurrentVer;
                }
                else
                {
                    MessageBox.Show(strRespMsg, this.Text + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text + " : Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                client.Dispose();
            }
        }
        #endregion

        #region t_Tick
        private void t_Tick(object sender, EventArgs e)
        {
            tickCount = tickCount + 1;
            if (tickCount <= 50)
            {
                if (toggleLight)
                {
                    btnDownload.BackColor = Color.Gold;
                    toggleLight = false;
                }
                else
                {
                    btnDownload.BackColor = Color.Wheat;
                    toggleLight = true;
                }
            }
            else
            {
                tickCount = 0;
                btnDownload.BackColor = Color.Gold;
                t.Stop();
            }
        }
        #endregion

        #region btnCheckConn_Click
        private void btnCheckConn_Click(object sender, EventArgs e)
        {
            frmConnectivity frmConn = new frmConnectivity();
            frmConn.FTP_HOST = FTPHOST;
            frmConn.FTP_USER_ID = FTPUSER;
            frmConn.FTP_PASSWORD = FTPPWD;
            frmConn.ShowDialog();
        }
        #endregion

        #region btnDownload_Click
        private void btnDownload_Click(object sender, EventArgs e)
        {
            //    StringBuilder sb = new StringBuilder();
            //    if (IsRunAsAdmin())
            //    {
            //        frmDownload frmDL = new frmDownload();
            //        frmDL.LATEST_VERSION = LatestVer;
            //        frmDL.ARCHIVE_DIRECTORY = ARCHDIR;
            //        frmDL.VETRIS_URL = VETURL;
            //        frmDL.ShowDialog();
            //    }
            //    else
            //    {
            //        sb.AppendLine("Dowloading this setup requires administrative rights");
            //        sb.AppendLine("Close this application,right click on the 'VETRIS DICOM Router Upload Files' icon and click on option 'Run as administrator'");
            //        sb.AppendLine("and then continue to load the application and download the new setup");
            //        MessageBox.Show(sb.ToString(), strWinHdr, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            frmDownload frmDL = new frmDownload();
            frmDL.LATEST_VERSION = LatestVer;
            frmDL.ARCHIVE_DIRECTORY = ARCHDIR;
            frmDL.VETRIS_URL = VETURL;
            frmDL.ShowDialog();

        }
        #endregion

        #region frmUpload_FormClosed
        private void frmUpload_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region LoadFileTypes
        private void LoadFileTypes()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("file_type", System.Type.GetType("System.String"));
            dtbl.TableName = "FileType";

            try
            {
                DataRow dr1 = dtbl.NewRow();
                dr1["id"] = "";
                dr1["file_type"] = "Select One";
                dtbl.Rows.Add(dr1);

                DataRow dr2 = dtbl.NewRow();
                dr2["id"] = "D";
                dr2["file_type"] = "DICOM Files";
                dtbl.Rows.Add(dr2);

                DataRow dr3 = dtbl.NewRow();
                dr3["id"] = "I";
                dr3["file_type"] = "Image Files";
                dtbl.Rows.Add(dr3);

                cmbFileType.DisplayMember = "file_type";
                cmbFileType.ValueMember = "id";
                cmbFileType.DataSource = dtbl;

            }
            catch (Exception expErr)
            { MessageBox.Show(expErr.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally
            { objCore = null; }
        }
        #endregion

        #region btnManual_Click
        private void btnManual_Click(object sender, EventArgs e)
        {
            btnAuto.BackColor = Color.Azure;
            btnManual.BackColor = Color.Gold;
            timer1.Stop();
            timer2.Stop();

            lstDrives.Items.Clear();
            dgvFiles.Columns.Clear();
            dgvFiles.DataSource = CreateUSBFileTable();
            FormatFileGrid();
            dgvPatient.Columns.Clear();
            dgvPatient.DataSource = CreateUSBPatientTable();
            FormatPatientGrid();

            gbAuto.Visible = false;
            gbManual.Visible = true;
            gbManual.Top = gbAuto.Top;
            gbManual.Left = gbManual.Left;
            cmbFileType.SelectedValue = "";

            btnFiles.Enabled = true;
            lstFiles.Items.Clear();

            strUploadType = "M";

        }
        #endregion

        #region btnAuto_Click
        private void btnAuto_Click(object sender, EventArgs e)
        {

            btnAuto.BackColor = Color.Gold;
            btnManual.BackColor = Color.Azure;
            gbAuto.Visible = true;
            gbManual.Visible = false;
            lstFiles.Items.Clear();

            lstDrives.Items.Clear();
            dgvFiles.Columns.Clear();
            dgvFiles.DataSource = CreateUSBFileTable();
            FormatFileGrid();
            dgvFiles.Visible = false;

            dgvPatient.Columns.Clear();
            dgvPatient.DataSource = CreateUSBPatientTable();
            FormatPatientGrid();
            dgvPatient.Visible = true;

            dgvImg.Columns.Clear();
            dgvImg.DataSource = CreateUSBFileTable();
            FormatImageGrid();
            dgvImg.Visible = false;

            strUploadType = "A";

            //timer2.Start();

        }
        #endregion

        #region Auto Detect USB Drives Upload

        #region cmbFileType_SelectedIndexChanged
        private void cmbFileType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFileType.SelectedValue.ToString() == "D")
            {
                timer1.Stop();
                timer3.Stop();
                rdoPatient.Visible = true;
                rdoAllFiles.Visible = true;
                rdoPatient.Checked = true;

                lstDrives.Items.Clear();

                dgvFiles.Columns.Clear();
                dgvFiles.DataSource = CreateUSBFileTable();
                FormatFileGrid();
                dgvFiles.Visible = false;

                dgvImg.Columns.Clear();
                dgvFiles.DataSource = CreateUSBFileImgTable();
                FormatImageGrid();
                dgvImg.Visible = false;

                dgvPatient.Columns.Clear();
                dgvPatient.DataSource = CreateUSBPatientTable();
                FormatPatientGrid();
                dgvPatient.Visible = true;

                timer2.Start();
            }
            else if (cmbFileType.SelectedValue.ToString() == "I")
            {
                timer1.Stop();
                timer2.Stop();
                rdoPatient.Visible = false;
                rdoAllFiles.Visible = false;
                rdoPatient.Checked = true;
                lstDrives.Items.Clear();

                dgvPatient.Columns.Clear();
                dgvPatient.DataSource = CreateUSBPatientTable();
                FormatPatientGrid();
                dgvPatient.Visible = false;

                dgvFiles.Columns.Clear();
                dgvFiles.DataSource = CreateUSBFileTable();
                FormatFileGrid();
                dgvFiles.Visible = false;

                dgvImg.Columns.Clear();
                dgvImg.DataSource = CreateUSBFileImgTable();
                FormatImageGrid();
                dgvImg.Visible = true;
                timer3.Start();

            }
        }
        #endregion

        #region rdoPatient_Click
        private void rdoPatient_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            lstDrives.Items.Clear();
            dgvFiles.Columns.Clear();
            dgvFiles.DataSource = CreateUSBFileTable();
            FormatFileGrid();
            dgvFiles.Visible = false;

            dgvPatient.Columns.Clear();
            dgvPatient.DataSource = CreateUSBPatientTable();
            FormatPatientGrid();
            dgvPatient.Visible = true;
            timer2.Start();
        }
        #endregion

        #region rdoAllFiles_Click
        private void rdoAllFiles_Click(object sender, EventArgs e)
        {
            timer2.Stop();
            lstDrives.Items.Clear();
            dgvPatient.Columns.Clear();
            dgvPatient.DataSource = CreateUSBPatientTable();
            FormatPatientGrid();
            dgvPatient.Visible = false;
            dtblFiles.Rows.Clear();

            dgvFiles.Columns.Clear();
            dgvFiles.DataSource = CreateUSBFileTable();
            FormatFileGrid();
            dgvFiles.Visible = true;
            timer1.Start();
        }

        #endregion

        #region SearchFiles
        private void SearchFiles()
        {
            SetScanner(true);
            DataTable dtbl = CreateUSBFileTable();
            DicomDecoder dd = new DicomDecoder();

            foreach (DataGridViewRow row in dgvFiles.Rows)
            {
                DataRow dr = dtbl.NewRow();
                dr["sel"] = Convert.ToBoolean(row.Cells["sel"].Value);
                dr["patient_name"] = Convert.ToString(row.Cells["patient_name"].Value).Trim();
                dr["file_name"] = Convert.ToString(row.Cells["file_name"].Value).Trim();
                dtbl.Rows.Add(dr);
            }

            System.IO.DriveInfo di = new System.IO.DriveInfo(DriveName);
            System.IO.DirectoryInfo rootDir = di.RootDirectory;
            WalkDirectoryTree(rootDir, dd, dtbl);

            this.Invoke((MethodInvoker)delegate
            {
                dgvFiles.Columns.Clear();

                if (dtbl != null)
                {
                    DataView dv = new DataView(dtbl);
                    dv.Sort = "patient_name asc,file_name asc";
                    dgvFiles.DataSource = dtbl;

                }
                else dgvFiles.DataSource = CreateUSBFileTable();

                FormatFileGrid();
            });
            SetScanner(false);
        }
        #endregion

        #region WalkDirectoryTree
        private void WalkDirectoryTree(System.IO.DirectoryInfo root, DicomDecoder dd, DataTable dtbl)
        {
            string[] pathElements = new string[0];
            string[] arr = new string[0];
            //int isDiacom = -1;
            string strSUID = string.Empty;
            string strPatientName = string.Empty;
            string strFile = string.Empty;
            string strFilename = string.Empty;


            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.*");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException ex)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
                MessageBox.Show(ex.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch (System.IO.DirectoryNotFoundException ex)
            {
                //Console.WriteLine(e.Message);
                MessageBox.Show(ex.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                if (files != null)
                {
                    foreach (System.IO.FileInfo fi in files)
                    {
                        // In this example, we only access the existing FileInfo object. If we
                        // want to open, delete or modify the file, then
                        // a try-catch block is required here to handle the case
                        // where the file has been deleted since the call to TraverseTree().
                        //Console.WriteLine(fi.FullName);

                        strFile = fi.FullName;
                        pathElements = strFile.Split('\\');
                        strFilename = pathElements[(pathElements.Length - 1)];
                        dd.DicomFileName = strFile;
                        List<string> str = dd.dicomInfo;

                        arr = new string[7];
                        arr = GetallTags(str);
                        strSUID = arr[0].Trim();
                        strPatientName = arr[3].Trim();

                        //isDiacom = (int)(dd.typeofDicomFile);

                        if (strSUID.Trim() != string.Empty)
                        {
                            DataView dv = new DataView(dtbl);
                            dv.RowFilter = "patient_name = '" + strPatientName.Replace("^", " ") + "' and file_name='" + strFile + "'";

                            if (dv.ToTable().Rows.Count == 0)
                            {
                                DataRow dr = dtbl.NewRow();
                                dr["sel"] = false;
                                dr["patient_name"] = strPatientName.Replace("^", " ");
                                dr["file_name"] = strFile;
                                dtbl.Rows.Add(dr);
                            }

                            dv.Dispose();
                        }

                    }

                    // Now find all the subdirectories under this directory.
                    subDirs = root.GetDirectories();

                    foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                    {
                        // Resursive call for each subdirectory.
                        WalkDirectoryTree(dirInfo, dd, dtbl);
                    }
                }
            }
            catch { }

        }
        #endregion

        #region SearchPatientFiles
        private void SearchPatientFiles()
        {
            SetScanner(true);
            DataTable dtbl = CreateUSBPatientTable();
            DicomDecoder dd = new DicomDecoder();

            foreach (DataGridViewRow row in dgvPatient.Rows)
            {
                DataRow dr = dtbl.NewRow();
                dr["sel"] = Convert.ToBoolean(row.Cells["sel"].Value);
                dr["patient_name"] = Convert.ToString(row.Cells["patient_name"].Value).Trim();
                dr["file_count"] = Convert.ToInt32(row.Cells["file_count"].Value);
                dtbl.Rows.Add(dr);
            }

            System.IO.DriveInfo di = new System.IO.DriveInfo(DriveName);
            System.IO.DirectoryInfo rootDir = di.RootDirectory;
            WalkPatientDirectoryTree(rootDir, dd, dtbl);

            this.Invoke((MethodInvoker)delegate
            {

                dgvPatient.Columns.Clear();


                if (dtbl != null)
                {
                    DataView dv = new DataView(dtbl);
                    dv.Sort = "patient_name asc";
                    dgvPatient.DataSource = dtbl;

                }
                else dgvPatient.DataSource = CreateUSBPatientTable();

                FormatPatientGrid();
            });

            SetScanner(false);

        }
        #endregion

        #region WalkPatientDirectoryTree
        private void WalkPatientDirectoryTree(System.IO.DirectoryInfo root, DicomDecoder dd, DataTable dtbl)
        {
            string[] pathElements = new string[0];
            string[] arr = new string[0];
            string strSUID = string.Empty;
            string strPatientName = string.Empty;
            string strFile = string.Empty;
            string strFilename = string.Empty;
            int intFileCount = 0;

            DataTable tmp = new DataTable();
            tmp.Columns.Add("patient_name", System.Type.GetType("System.String"));
            tmp.Columns.Add("file_name", System.Type.GetType("System.String"));


            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.*");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException ex)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
                MessageBox.Show(ex.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch (System.IO.DirectoryNotFoundException ex)
            {
                //Console.WriteLine(e.Message);
                MessageBox.Show(ex.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                if (files != null)
                {
                    tmp.Rows.Clear();

                    foreach (System.IO.FileInfo fi in files)
                    {
                        // In this example, we only access the existing FileInfo object. If we
                        // want to open, delete or modify the file, then
                        // a try-catch block is required here to handle the case
                        // where the file has been deleted since the call to TraverseTree().
                        //Console.WriteLine(fi.FullName);

                        strFile = fi.FullName;
                        pathElements = strFile.Split('\\');
                        strFilename = pathElements[(pathElements.Length - 1)];
                        dd.DicomFileName = strFile;
                        List<string> str = dd.dicomInfo;

                        arr = new string[7];
                        arr = GetallTags(str);
                        strSUID = arr[0].Trim();
                        strPatientName = arr[3].Trim();
                        strPatientName = strPatientName.Replace("^", " ").Trim();


                        if (strSUID.Trim() != string.Empty)
                        {
                            DataRow dr = tmp.NewRow();
                            dr["patient_name"] = strPatientName;
                            dr["file_name"] = strFile;
                            tmp.Rows.Add(dr);

                            DataView dvFile = new DataView(dtblFiles);
                            dvFile.RowFilter = "patient_name = '" + strPatientName.Trim() + "' and file_name='" + strFile + "'";
                            if (dvFile.ToTable().Rows.Count == 0)
                            {
                                DataRow drFile = dtblFiles.NewRow();
                                drFile["patient_name"] = strPatientName;
                                drFile["file_name"] = strFile;
                                dtblFiles.Rows.Add(drFile);
                            }

                        }

                    }

                    var distinctNames = (from row in tmp.AsEnumerable()
                                         select row.Field<string>("patient_name")).Distinct();

                    foreach (var name in distinctNames)
                    {
                        DataView dvTmp = new DataView(tmp);
                        dvTmp.RowFilter = "patient_name = '" + name.Trim() + "'";
                        intFileCount = dvTmp.ToTable().Rows.Count;

                        DataView dv = new DataView(dtbl);
                        dv.RowFilter = "patient_name = '" + strPatientName + "'";

                        if (dv.ToTable().Rows.Count == 0)
                        {
                            DataRow dr = dtbl.NewRow();
                            dr["sel"] = false;
                            dr["patient_name"] = strPatientName;
                            dr["file_count"] = intFileCount;
                            dtbl.Rows.Add(dr);
                        }
                        else if (Convert.ToInt32(dv.ToTable().Rows[0]["file_count"]) != intFileCount)
                        {
                            foreach (DataRow dr in dtbl.Rows)
                            {
                                if (Convert.ToString(dr["patient_name"]).Trim() == strPatientName.Trim())
                                {
                                    dr["file_count"] = intFileCount;
                                    break;
                                }
                            }
                        }

                        dvTmp.Dispose();
                        dv.Dispose();
                    }



                    // Now find all the subdirectories under this directory.
                    subDirs = root.GetDirectories();

                    foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                    {
                        // Resursive call for each subdirectory.
                        WalkPatientDirectoryTree(dirInfo, dd, dtbl);
                    }
                }
            }
            catch { }
        }
        #endregion

        #region SearchImageFiles
        private void SearchImageFiles()
        {
            SetScanner(true);
            DataTable dtbl = CreateUSBFileImgTable();


            foreach (DataGridViewRow row in dgvImg.Rows)
            {
                DataRow dr = dtbl.NewRow();
                dr["sel"] = Convert.ToBoolean(row.Cells["sel"].Value);
                dr["file_name"] = Convert.ToString(row.Cells["file_name"].Value).Trim();
                dtbl.Rows.Add(dr);
            }

            System.IO.DriveInfo di = new System.IO.DriveInfo(DriveName);
            System.IO.DirectoryInfo rootDir = di.RootDirectory;
            WalkImageDirectoryTree(rootDir, dtbl);

            this.Invoke((MethodInvoker)delegate
            {
                dgvImg.Columns.Clear();

                if (dtbl != null)
                {

                    DataView dv = new DataView(dtbl);
                    dv.Sort = "file_name asc";
                    dgvImg.DataSource = dtbl;
                    dv.Dispose();
                }
                else dgvImg.DataSource = CreateUSBFileImgTable();

                FormatImageGrid();
            });
            SetScanner(false);
        }
        #endregion

        #region WalkImageDirectoryTree
        private void WalkImageDirectoryTree(System.IO.DirectoryInfo root, DataTable dtbl)
        {
            string[] pathElements = new string[0];
            string[] arr = new string[0];
            string strFile = string.Empty;
            string strFilename = string.Empty;


            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;


            try
            {
                files = root.GetFiles("*.*");
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch (System.IO.DirectoryNotFoundException ex)
            {
                MessageBox.Show(ex.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                if (files != null)
                {
                    foreach (System.IO.FileInfo fi in files)
                    {

                        strFile = fi.FullName;
                        pathElements = strFile.Split('\\');
                        strFilename = pathElements[(pathElements.Length - 1)];

                        if ((MIMEAssistant.GetMIMEType(strFile) == "image/jpeg") || (MIMEAssistant.GetMIMEType(strFile) == "image/gif") || (MIMEAssistant.GetMIMEType(strFile) == "image/png") || (MIMEAssistant.GetMIMEType(strFile) == "image/bmp"))
                        {
                            DataView dv = new DataView(dtbl);
                            dv.RowFilter = "file_name='" + strFile + "'";

                            if (dv.ToTable().Rows.Count == 0)
                            {
                                DataRow dr = dtbl.NewRow();
                                dr["sel"] = false;
                                dr["file_name"] = strFile;
                                dtbl.Rows.Add(dr);
                            }
                        }

                    }

                    // Now find all the subdirectories under this directory.
                    subDirs = root.GetDirectories();

                    foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                    {
                        // Resursive call for each subdirectory.
                        WalkImageDirectoryTree(dirInfo, dtbl);
                    }
                }
            }
            catch { ; }
        }
        #endregion

        #region CreateUSBFileTable
        private DataTable CreateUSBFileTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("sel", System.Type.GetType("System.Boolean"));
            dtbl.Columns.Add("patient_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("file_name", System.Type.GetType("System.String"));
            dtbl.TableName = "Files";
            return dtbl;
        }
        #endregion

        #region CreateUSBPatientTable
        private DataTable CreateUSBPatientTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("sel", System.Type.GetType("System.Boolean"));
            dtbl.Columns.Add("patient_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("file_count", System.Type.GetType("System.Int32"));
            dtbl.TableName = "Patient";
            return dtbl;
        }
        #endregion

        #region CreateUSBFileImgTable
        private DataTable CreateUSBFileImgTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("sel", System.Type.GetType("System.Boolean"));
            dtbl.Columns.Add("file_name", System.Type.GetType("System.String"));
            dtbl.TableName = "Files";
            return dtbl;
        }
        #endregion

        #region FormatFileGrid
        private void FormatFileGrid()
        {

            for (int i = 0; i < dgvFiles.Columns.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        dgvFiles.Columns[i].HeaderText = "Select";
                        dgvFiles.Columns[i].Width = 60;
                        dgvFiles.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;
                    case 1:
                        dgvFiles.Columns[i].HeaderText = "Patient Name";
                        dgvFiles.Columns[i].Width = 300;
                        break;
                    case 2:
                        dgvFiles.Columns[i].HeaderText = "File Name";
                        dgvFiles.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        break;

                }

            }
        }
        #endregion

        #region FormatPatientGrid
        private void FormatPatientGrid()
        {

            for (int i = 0; i < dgvPatient.Columns.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        dgvPatient.Columns[i].HeaderText = "Select";
                        dgvPatient.Columns[i].Width = 60;
                        dgvPatient.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;
                    case 1:
                        dgvPatient.Columns[i].HeaderText = "Patient Name";
                        dgvPatient.Columns[i].Width = 300;
                        break;
                    case 2:
                        dgvPatient.Columns[i].HeaderText = "File Count";
                        dgvPatient.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        break;

                }

            }
        }
        #endregion

        #region FormatImageGrid
        private void FormatImageGrid()
        {

            for (int i = 0; i < dgvImg.Columns.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
                        dgvImg.Columns[i].HeaderText = "Select";
                        dgvImg.Columns[i].Width = 60;
                        dgvImg.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;
                    case 1:
                        dgvImg.Columns[i].HeaderText = "File Name";
                        dgvImg.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        break;

                }

            }
        }
        #endregion

        #region timer1_Tick
        private void timer1_Tick(object sender, EventArgs e)
        {
            DriveInfo[] allDrives = new DriveInfo[0];

            allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType.ToString().ToUpper() == "REMOVABLE")
                {
                    if (d.IsReady)
                    {
                        if (!lstDrives.Items.Contains(d.Name))
                        {
                            lstDrives.Items.Add(d.Name);
                            DriveName = d.Name;
                            threadInput = new Thread(SearchFiles);
                            threadInput.Start();

                        }

                    }
                }
            }

        }
        #endregion

        #region timer2_Tick
        private void timer2_Tick(object sender, EventArgs e)
        {
            DriveInfo[] allDrives = new DriveInfo[0];

            allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType.ToString().ToUpper() == "REMOVABLE")
                {
                    if (d.IsReady)
                    {
                        if (!lstDrives.Items.Contains(d.Name))
                        {
                            lstDrives.Items.Add(d.Name);
                            DriveName = d.Name;
                            threadInput = new Thread(SearchPatientFiles);
                            threadInput.Start();
                        }

                    }
                }
            }
        }
        #endregion

        #region timer3_Tick
        private void timer3_Tick(object sender, EventArgs e)
        {
            DriveInfo[] allDrives = new DriveInfo[0];

            allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType.ToString().ToUpper() == "REMOVABLE")
                {
                    if (d.IsReady)
                    {
                        if (!lstDrives.Items.Contains(d.Name))
                        {
                            lstDrives.Items.Add(d.Name);
                            DriveName = d.Name;
                            threadInput = new Thread(SearchImageFiles);
                            threadInput.Start();
                        }

                    }
                }
            }
        }

        #endregion

        #region btnUploadUSB_Click
        private void btnUploadUSB_Click(object sender, EventArgs e)
        {

            int intFileCount = 0;
            int intPatientCount = 0;
            int intFileUploaded = 0;
            int intTotFiles = 0;
            int intpbProg = 0;
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string[] arr = new string[0];

            string strFile = string.Empty;
            string strFileName = string.Empty;
            string strDirName = string.Empty;
            string strUpldDirName = string.Empty;
            string strTgtDirName = string.Empty;
            string strSUID = string.Empty;
            string strPatientName = string.Empty;
            int intUploadSuccess = 0;
            bool bSel = false;
            string strPrefix = string.Empty;
            string strNewFile = string.Empty;
            string _Stat = string.Empty;
            string strSID = string.Empty;
            bool bRetRecStatus = false;
            string strRecStat = string.Empty;
            bool bRetSendStatus = false;
            string strSendStat = string.Empty;
            bool bProceed = true;
            StringBuilder sb = new StringBuilder();

            try
            {
                

                #region Check Service Status
                CheckServiceStatus(ref bRetRecStatus, ref strRecStat, ref bRetSendStatus, ref strSendStat);

                #region Dicom Receiving Service
                if (bRetRecStatus == false)
                {
                    if (strRecStat.ToUpper().IndexOf("INSTALL") >= 0)
                    {
                        lblpbInfo.Refresh();
                        lblpbInfo.Text = "Dicom Receiving Service is not installed...please contact administrator";
                        lblpbInfo.Refresh();
                        bProceed = false;
                    }
                    else if (strRecStat.ToUpper().IndexOf("PENDING") >= 0)
                    {
                        lblpbInfo.Refresh();
                        lblpbInfo.Text = "Installation of Dicom Receiving Service is pending...please contact administrator";
                        lblpbInfo.Refresh();
                        bProceed = false;
                    }
                    else
                    {
                        lblpbInfo.Refresh();
                        //lblpbInfo.Text = "Dicom Receiving Service is not running...Starting Dicom Receiving Service";
                        lblpbInfo.Text = "Starting Upload...Please Wait";
                        lblpbInfo.Refresh();

                        var proc1 = new ProcessStartInfo();
                        proc1.UseShellExecute = true;
                        proc1.WorkingDirectory = @"C:\Windows\System32";
                        proc1.FileName = @"C:\Windows\System32\cmd.exe";
                        proc1.Verb = "runas";
                        proc1.Arguments = "/c net start DICOMReceiverService";
                        proc1.WindowStyle = ProcessWindowStyle.Hidden;
                        Process.Start(proc1);
                        Process.GetCurrentProcess().WaitForExit(5000);

                        CheckServiceStatus(ref bRetRecStatus, ref strRecStat, ref bRetSendStatus, ref strSendStat);
                        if (strRecStat.ToUpper().IndexOf("RUN") < 0)
                        {
                            lblpbInfo.Refresh();
                            lblpbInfo.Text = "Failed to start Dicom Receiving Service...please contact administrator";
                            lblpbInfo.Refresh();
                            bProceed = false;
                        }
                        else
                        {
                            lblpbInfo.Refresh();
                            lblpbInfo.Text = "Dicom Receiving Service Started";
                            lblpbInfo.Refresh();
                            bProceed = true;
                        }


                    }
                }
                #endregion

                #region Dicom Sending Service
                if (bRetSendStatus == false)
                {
                    if (strSendStat.ToUpper().IndexOf("INSTALL") >= 0)
                    {
                        lblpbInfo.Refresh();
                        lblpbInfo.Text = "Dicom Sending Service is not installed...please contact administrator";
                        lblpbInfo.Refresh();
                        bProceed = false;
                    }
                    else if (strSendStat.ToUpper().IndexOf("PENDING") >= 0)
                    {
                        lblpbInfo.Refresh();
                        lblpbInfo.Text = "Onstallation of Dicom Sending Service is pending...please contact administrator";
                        lblpbInfo.Refresh();
                        bProceed = false;
                    }
                    else
                    {
                        lblpbInfo.Refresh();
                        //lblpbInfo.Text = "Dicom Sending Service is not running...Starting Dicom Sending Service";
                        lblpbInfo.Text = "Starting Upload...Please Wait";
                        lblpbInfo.Refresh();

                        var proc2 = new ProcessStartInfo();
                        proc2.UseShellExecute = true;
                        proc2.WorkingDirectory = @"C:\Windows\System32";
                        proc2.FileName = @"C:\Windows\System32\cmd.exe";
                        proc2.Verb = "runas";
                        proc2.Arguments = "/c net start DICOMSenderService";
                        proc2.WindowStyle = ProcessWindowStyle.Hidden;
                        Process.Start(proc2);
                        Process.GetCurrentProcess().WaitForExit(5000);

                        CheckServiceStatus(ref bRetRecStatus, ref strRecStat, ref bRetSendStatus, ref strSendStat);
                        if (strSendStat.ToUpper().IndexOf("RUN") < 0)
                        {
                            lblpbInfo.Refresh();
                            lblpbInfo.Text = "Failed to start Dicom Sending Service...please contact administrator";
                            lblpbInfo.Refresh();
                            bProceed = false;
                        }
                        else
                        {
                            lblpbInfo.Refresh();
                            lblpbInfo.Text = "Dicom Sending Service Started";
                            lblpbInfo.Refresh();
                            bProceed = true;
                        }


                    }
                }
                #endregion

                #endregion

                #region Proceed to upload
                if (bProceed)
                {
                    if (LatestVer == CurrentVer)
                    {
                        strSID = "S1D" + DateTime.Now.ToString("MMddyyHHmmss") + CoreCommon.RandomString(3);
                        if (cmbFileType.SelectedValue.ToString() == "D")
                        {
                            List<string> arrSUID = new List<string>();
                            dd = new DicomDecoder();

                            #region DICOM Files
                            if (rdoAllFiles.Checked)
                            {

                                #region File Wise Upload
                                try
                                {
                                    timer1.Stop(); timer3.Stop();
                                    intFileCount = dgvFiles.Rows.Count;

                                    if (intFileCount > 0)
                                    {
                                        lblpbInfo.Visible = true;
                                        lblpbInfo.Refresh();
                                        lblpbInfo.Text = "Number of file(s) to upload : " + intFileCount.ToString();
                                        pb1.Visible = true;
                                        pb1.Minimum = 1;
                                        pb1.Maximum = intFileCount;

                                        #region upload files selected
                                        foreach (DataGridViewRow row in dgvFiles.Rows)
                                        {

                                            bSel = Convert.ToBoolean(row.Cells["sel"].Value);
                                            strFile = Convert.ToString(row.Cells["file_name"].Value).Trim();

                                            if (bSel)
                                            {
                                                if (CheckIfDriveIsReady(strFile))
                                                {
                                                    intpbProg = intpbProg + 1;
                                                    pb1.Value = intpbProg;
                                                    pb1.Refresh();
                                                    lblpbInfo.Refresh();
                                                    lblpbInfo.Text = "Uploading file " + intpbProg.ToString() + " of " + intFileCount.ToString();



                                                    pathElements = strFile.Split('\\');
                                                    strFileName = pathElements[(pathElements.Length - 1)];
                                                    dd.DicomFileName = strFile;
                                                    List<string> str = dd.dicomInfo;

                                                    arr = new string[7];
                                                    arr = GetallTags(str);
                                                    strSUID = arr[0].Trim();

                                                    if (strSUID.Trim() != string.Empty)
                                                    {
                                                        strPrefix = CoreCommon.RandomString(6);
                                                        //strFileName = strSID + "_" + strPrefix + "_" + strFileName;
                                                        strFileName = strInstCode + "_" + strSID + "_" + strInstName.Replace(" ", "_") + "_" + strPrefix + "_" + strFileName.Replace(strSID + "_", "");
                                                        if (File.Exists(strManualUploadDirPath + "\\" + strFileName))
                                                        {
                                                            System.IO.File.Delete(strManualUploadDirPath + "\\" + strFileName);
                                                        }
                                                        System.IO.File.Copy(strFile, strManualUploadDirPath + "\\" + strFileName);

                                                        intFileUploaded = intFileUploaded + 1;
                                                        intUploadSuccess = 1;
                                                    }

                                                }
                                                else
                                                {
                                                    intUploadSuccess = 0;
                                                    break;
                                                }
                                            }

                                        }
                                        #endregion

                                        #region post upload
                                        if (intUploadSuccess == 1)
                                        {
                                            lblpbInfo.Refresh(); lblpbInfo.Text = "";
                                            pb1.Refresh();
                                            pb1.Visible = false;
                                            DialogResult result = MessageBox.Show(intFileUploaded.ToString() + " File(s) copied successfully for upload", strWinHdr + " : Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                            if (result == DialogResult.OK)
                                            {
                                                if (chkShowProg.Checked)
                                                {
                                                    lblpbInfo.Visible = false;
                                                    pb1.Visible = false;

                                                    frmFileTransfer frmFT = new frmFileTransfer();
                                                    frmFT.VETRIS_URL = VETLOGINURL;
                                                    frmFT.VETRIS_LOGIN_ID = strVETLogin;
                                                    frmFT.INSTITUTION_CODE = strInstCode;
                                                    frmFT.IMPORT_SESSION_ID = strSID;
                                                    frmFT.VETRIS_API_URL = VETAPIURL;
                                                    frmFT.FILE_COUNT = intFileCount;
                                                    frmFT.SENDER_DIRECTORY = strSendDirPath;
                                                    frmFT.ARCHIVE_DIRTECTORY = ARCHDIR;
                                                    frmFT.ARCHIVE_FILES_TRANSFERRED = strARCHFILE;
                                                    frmFT.ShowDialog();
                                                }
                                                else
                                                {
                                                    result = MessageBox.Show("Do you want to view the study in VETRIS now ?", strWinHdr + " : Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                                    if (result == DialogResult.Yes)
                                                    {
                                                        for (int i = 0; i < 10000; i++) ;
                                                        System.Diagnostics.Process.Start(strVETLOGINURL + "?UID=" + strVETLogin + "&INS=" + strInstCode + "&MID=0");
                                                    }
                                                }
                                            }
                                        }
                                        else if (intUploadSuccess == 0)
                                        {
                                            if (intpbProg == 0)
                                            {
                                                MessageBox.Show("No file(s) found to upload", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                            else
                                            {
                                                MessageBox.Show("File(s) could not be copied for upload...the drive containing the file(s) was not ready", strWinHdr + " : Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                            lblpbInfo.Refresh(); lblpbInfo.Text = "";
                                            pb1.Refresh();
                                            pb1.Visible = false;

                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        MessageBox.Show("No file(s) found to upload", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                timer1.Start();
                                #endregion
                            }
                            else if (rdoPatient.Checked)
                            {
                                #region Patient Wise Upload
                                try
                                {
                                    timer2.Stop();
                                    intPatientCount = dgvPatient.Rows.Count;

                                    if (intPatientCount > 0)
                                    {
                                        lblpbInfo.Visible = true;
                                        lblpbInfo.Refresh();
                                        lblpbInfo.Text = "Number of Patient(s) for whom the file(s) will be uploaded : " + intPatientCount.ToString();
                                        pb1.Minimum = 1;
                                        pb1.Maximum = intPatientCount;
                                        pb1.Visible = true;

                                        #region upload files
                                        foreach (DataGridViewRow row in dgvPatient.Rows)
                                        {

                                            bSel = Convert.ToBoolean(row.Cells["sel"].Value);
                                            strPatientName = Convert.ToString(row.Cells["patient_name"].Value).Trim();

                                            if (bSel)
                                            {
                                                intpbProg = intpbProg + 1;
                                                pb1.Value = intpbProg;
                                                pb1.Refresh();
                                                lblpbInfo.Refresh();
                                                lblpbInfo.Text = "Uploading file(s) of the patient " + strPatientName;



                                                DataView dv = new DataView(dtblFiles);
                                                dv.RowFilter = "patient_name='" + strPatientName + "'";
                                                intFileCount = dv.ToTable().Rows.Count;
                                                intFileUploaded = 0;

                                                foreach (DataRow dr in dv.ToTable().Rows)
                                                {
                                                    strFile = Convert.ToString(dr["file_name"]).Trim();

                                                    if (CheckIfDriveIsReady(strFile))
                                                    {
                                                        pathElements = strFile.Split('\\');
                                                        strFileName = pathElements[(pathElements.Length - 1)];
                                                        dd.DicomFileName = strFile;
                                                        List<string> str = dd.dicomInfo;

                                                        arr = new string[7];
                                                        arr = GetallTags(str);
                                                        strSUID = arr[0].Trim();

                                                        if (strSUID.Trim() != string.Empty)
                                                        {
                                                            strPrefix = CoreCommon.RandomString(6);
                                                            //strFileName = strSID + "_" + strPrefix + "_" + strFileName;
                                                            strFileName = strInstCode + "_" + strSID + "_" + strInstName.Replace(" ", "_") + "_" + strPrefix + "_" + strFileName.Replace(strSID + "_", "");
                                                            if (File.Exists(strManualUploadDirPath + "\\" + strFileName))
                                                            {
                                                                System.IO.File.Delete(strManualUploadDirPath + "\\" + strFileName);
                                                            }

                                                            System.IO.File.Copy(strFile, strManualUploadDirPath + "\\" + strFileName);

                                                            intFileUploaded = intFileUploaded + 1;
                                                            intTotFiles = intTotFiles + 1;
                                                            lblpbInfo.Refresh();
                                                            lblpbInfo.Text = "Uploading file(s) of the patient " + strPatientName + " " + intFileUploaded.ToString() + "/" + intFileCount.ToString() + " uploaded";
                                                            intUploadSuccess = 1;
                                                        }

                                                    }
                                                }
                                                dv.Dispose();
                                            }

                                        }
                                        #endregion

                                        #region upload file result
                                        if (intUploadSuccess == 1)
                                        {
                                            lblpbInfo.Refresh(); lblpbInfo.Text = "";
                                            pb1.Refresh();
                                            pb1.Visible = false;
                                            DialogResult result = MessageBox.Show(intTotFiles.ToString() + " File(s) copied successfully for upload", strWinHdr + " : Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            if (result == DialogResult.OK)
                                            {
                                                if (chkShowProg.Checked)
                                                {
                                                    lblpbInfo.Visible = false;
                                                    pb1.Visible = false;

                                                    frmFileTransfer frmFT = new frmFileTransfer();
                                                    frmFT.VETRIS_URL = VETLOGINURL;
                                                    frmFT.VETRIS_LOGIN_ID = strVETLogin;
                                                    frmFT.INSTITUTION_CODE = strInstCode;
                                                    frmFT.IMPORT_SESSION_ID = strSID;
                                                    frmFT.VETRIS_API_URL = VETAPIURL;
                                                    frmFT.FILE_COUNT = intFileCount;
                                                    frmFT.SENDER_DIRECTORY = strSendDirPath;
                                                    frmFT.ARCHIVE_DIRTECTORY = ARCHDIR;
                                                    frmFT.ARCHIVE_FILES_TRANSFERRED = strARCHFILE;
                                                    frmFT.ShowDialog();
                                                }
                                                else
                                                {
                                                    result = MessageBox.Show("Do you want to view the study in VETRIS now ?", strWinHdr + " : Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                                    if (result == DialogResult.Yes)
                                                    {
                                                        for (int i = 0; i < 10000; i++) ;
                                                        System.Diagnostics.Process.Start(strVETLOGINURL + "?UID=" + strVETLogin + "&INS=" + strInstCode + "&MID=0");
                                                    }
                                                }
                                            }

                                        }
                                        else if (intUploadSuccess == 0)
                                        {
                                            if (intpbProg == 0)
                                            {
                                                MessageBox.Show("No file(s) found to upload", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                            else
                                            {
                                                MessageBox.Show("File(s) could not be copied for upload...the drive containing the file(s) was not ready", strWinHdr + " : Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                            lblpbInfo.Refresh(); lblpbInfo.Text = "";
                                            pb1.Refresh();
                                            pb1.Visible = false;

                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        MessageBox.Show("No file(s) found to upload", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                dtblFiles.Rows.Clear();
                                timer2.Start();
                                #endregion
                            }
                            #endregion
                        }
                        else if (cmbFileType.SelectedValue.ToString() == "I")
                        {
                            #region Image Files
                            try
                            {
                                intFileCount = dgvImg.Rows.Count;

                                if (intFileCount > 0)
                                {
                                    lblpbInfo.Visible = true;
                                    lblpbInfo.Refresh();
                                    lblpbInfo.Text = "Number of file(s) to upload : " + intFileCount.ToString();
                                    pb1.Minimum = 1;
                                    pb1.Maximum = intFileCount;

                                    #region upload files selected
                                    foreach (DataGridViewRow row in dgvFiles.Rows)
                                    {

                                        bSel = Convert.ToBoolean(row.Cells["sel"].Value);
                                        strFile = Convert.ToString(row.Cells["file_name"].Value).Trim();

                                        if (bSel)
                                        {
                                            if (CheckIfDriveIsReady(strFile))
                                            {
                                                intpbProg = intpbProg + 1;
                                                pb1.Value = intpbProg;
                                                lblpbInfo.Refresh();
                                                lblpbInfo.Text = "Uploading file " + intpbProg.ToString() + " of " + intFileCount.ToString();


                                                pathElements = strFile.Split('\\');
                                                strFileName = pathElements[(pathElements.Length - 1)];


                                                if ((MIMEAssistant.GetMIMEType(strFile) == "image/jpeg") || (MIMEAssistant.GetMIMEType(strFile) == "image/gif") || (MIMEAssistant.GetMIMEType(strFile) == "image/png") || (MIMEAssistant.GetMIMEType(strFile) == "image/bmp"))
                                                {
                                                    strPrefix = CoreCommon.RandomString(6);
                                                    //strFileName = strSID + "_" + strPrefix + "_" + strFileName;
                                                    strFileName = strInstCode + "_" + strSID + "_" + strInstName.Replace(" ", "_") + "_" + strPrefix + "_" + strFileName.Replace(strSID + "_", "");
                                                    if (File.Exists(strManualUploadDirPath + "\\" + strFileName))
                                                    {
                                                        System.IO.File.Delete(strManualUploadDirPath + "\\" + strFileName);
                                                    }

                                                    System.IO.File.Copy(strFile, strManualUploadDirPath + "\\" + strFileName);
                                                    intFileUploaded = intFileUploaded + 1;
                                                    intUploadSuccess = 1;
                                                }

                                            }
                                            else
                                            {
                                                intUploadSuccess = 0;
                                                break;
                                            }
                                        }

                                    }
                                    #endregion

                                    #region post upload
                                    if (intUploadSuccess == 1)
                                    {
                                        lblpbInfo.Refresh(); lblpbInfo.Text = "";
                                        pb1.Refresh();
                                        DialogResult result = MessageBox.Show(intFileUploaded.ToString() + " File(s) copied successfully for upload", strWinHdr + " : Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                        if (result == DialogResult.OK)
                                        {
                                            if (chkShowProg.Checked)
                                            {
                                                lblpbInfo.Visible = false;
                                                pb1.Visible = false;

                                                frmFileTransfer frmFT = new frmFileTransfer();
                                                frmFT.VETRIS_URL = VETLOGINURL;
                                                frmFT.VETRIS_LOGIN_ID = strVETLogin;
                                                frmFT.INSTITUTION_CODE = strInstCode;
                                                frmFT.IMPORT_SESSION_ID = strSID;
                                                frmFT.VETRIS_API_URL = VETAPIURL;
                                                frmFT.FILE_COUNT = intFileCount;
                                                frmFT.SENDER_DIRECTORY = strSendDirPath;
                                                frmFT.ARCHIVE_DIRTECTORY = ARCHDIR;
                                                frmFT.ARCHIVE_FILES_TRANSFERRED = strARCHFILE;
                                                frmFT.ShowDialog();
                                            }
                                            else
                                            {
                                                result = MessageBox.Show("Do you want to view the study in VETRIS now ?", strWinHdr + " : Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                                if (result == DialogResult.Yes)
                                                {
                                                    for (int i = 0; i < 10000; i++) ;
                                                    System.Diagnostics.Process.Start(strVETLOGINURL + "?UID=" + strVETLogin + "&INS=" + strInstCode + "&MID=0");
                                                }
                                            }
                                        }
                                    }
                                    else if (intUploadSuccess == 0)
                                    {
                                        if (intpbProg == 0)
                                        {
                                            MessageBox.Show("No file(s) found to upload", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        else
                                        {
                                            MessageBox.Show("File(s) could not be copied for upload...the drive containing the file(s) was not ready", strWinHdr + " : Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        lblpbInfo.Refresh(); lblpbInfo.Text = "";
                                        pb1.Refresh();
                                        pb1.Visible = false;

                                    }
                                    #endregion
                                }
                                else
                                {
                                    MessageBox.Show("No file(s) found to upload", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        sb.Append("The installed version of the router is " + CurrentVer);
                        sb.Append("You should download the latest version " + LatestVer + " in order to continue the file uploading process");
                        MessageBox.Show(sb.ToString(), strWinHdr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region CheckIfDriveIsReady
        private bool CheckIfDriveIsReady(string strFilePath)
        {
            bool bRet = false;
            string[] arr = strFilePath.Split('\\');


            DriveInfo d = new DriveInfo(arr[0]);
            if (d.IsReady) bRet = true;


            return bRet;
        }
        #endregion

        #endregion

        #region Manual Upload

        #region btnFiles_Click
        private void btnFiles_Click(object sender, EventArgs e)
        {
            string strFileName = string.Empty;
            string[] arrWAP = new string[0];
            StringBuilder sb = new StringBuilder();

            arrWAP = OpenFileDialouge(string.Empty, "Browse study files");

            if (arrWAP.Length > 0)
            {
                for (int i = 0; i < arrWAP.Length; i++)
                {
                    if (CheckValidFileFormat(arrWAP[i]))
                    {
                        if (!IsDuplicateFile(arrWAP[i]))
                        {
                            lstFiles.Items.Add(arrWAP[i]);
                        }
                        else
                        {
                            sb.AppendLine("File : " + arrWAP[i] + "is already added in the list.");
                            //MessageBox.Show("File : " + file + "is already added in the list.", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        sb.AppendLine("Invalid file format : " + arrWAP[i]);
                    }


                }
                if (sb.ToString().Trim() != string.Empty)
                {
                    MessageBox.Show(sb.ToString(), strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //lstFiles.Items.AddRange(arrWAP);
            }
            arrWAP = null;

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

        #region OpenFileDialouge
        public static string[] OpenFileDialouge(string initialPath, string Description)
        {
            string[] arrFiles = new string[0];
            Fdialog = new OpenFileDialog();
            int index = 0;
            Fdialog.Multiselect = true;
            Fdialog.InitialDirectory = initialPath;

            Fdialog.RestoreDirectory = true;
            Fdialog.Title = Description;

            Fdialog.ShowDialog();

            arrFiles = new string[Fdialog.FileNames.Length];
            foreach (String file in Fdialog.FileNames)
            {
                arrFiles[index] = file;
                index++;
            }
            Fdialog.Dispose();
            return arrFiles;
        }
        #endregion

        #region btnUpload_Click
        private void btnUpload_Click(object sender, EventArgs e)
        {
            string strSID = string.Empty;
            bool bRetRecStatus = false;
            bool bRetSendStatus = false;
            string strRecStat = string.Empty;
            string strSendStat = string.Empty;
            bool bProceed = true;
            StringBuilder sb = new StringBuilder();

            try
            {
                if (lstFiles.Items.Count > 0)
                {
                    lblpbInfo.Visible = true;

                    #region Check Service Status
                    CheckServiceStatus(ref bRetRecStatus, ref strRecStat, ref bRetSendStatus, ref strSendStat);

                    #region Dicom Receiving Service
                    if (bRetRecStatus == false)
                    {
                        if (strRecStat.ToUpper().IndexOf("INSTALL") >= 0)
                        {
                            lblpbInfo.Refresh();
                            lblpbInfo.Text = "Dicom Receiving Service is not installed...please contact administrator";
                            lblpbInfo.Refresh();
                            bProceed = false;
                        }
                        else if (strRecStat.ToUpper().IndexOf("PENDING") >= 0)
                        {
                            lblpbInfo.Refresh();
                            lblpbInfo.Text = "Installation of Dicom Receiving Service is pending...please contact administrator";
                            lblpbInfo.Refresh();
                            bProceed = false;
                        }
                        else
                        {
                            lblpbInfo.Refresh();
                            //lblpbInfo.Text = "Dicom Receiving Service is not running...Starting Dicom Receiving Service";
                            lblpbInfo.Text = "Starting Upload...Please Wait";
                            lblpbInfo.Refresh();

                            var proc1 = new ProcessStartInfo();
                            proc1.UseShellExecute = true;
                            proc1.WorkingDirectory = @"C:\Windows\System32";
                            proc1.FileName = @"C:\Windows\System32\cmd.exe";
                            proc1.Verb = "runas";
                            proc1.Arguments = "/c net start DICOMReceiverService";
                            proc1.WindowStyle = ProcessWindowStyle.Hidden;
                            Process.Start(proc1);
                            Process.GetCurrentProcess().WaitForExit(5000);

                            CheckServiceStatus(ref bRetRecStatus, ref strRecStat, ref bRetSendStatus, ref strSendStat);
                            if (strRecStat.ToUpper().IndexOf("RUN") < 0)
                            {
                                lblpbInfo.Refresh();
                                lblpbInfo.Text = "Failed to start Dicom Receiving Service...please contact administrator";
                                lblpbInfo.Refresh();
                                bProceed = false;
                            }
                            else
                            {
                                lblpbInfo.Refresh();
                                lblpbInfo.Text = "Dicom Receiving Service Started";
                                lblpbInfo.Refresh();
                                bProceed = true;
                            }


                        }
                    }
                    #endregion

                    #region Dicom Sending Service
                    if (bRetSendStatus == false)
                    {
                        if (strSendStat.ToUpper().IndexOf("INSTALL") >= 0)
                        {
                            lblpbInfo.Refresh();
                            lblpbInfo.Text = "Dicom Sending Service is not installed...please contact administrator";
                            lblpbInfo.Refresh();
                            bProceed = false;
                        }
                        else if (strSendStat.ToUpper().IndexOf("PENDING") >= 0)
                        {
                            lblpbInfo.Refresh();
                            lblpbInfo.Text = "Onstallation of Dicom Sending Service is pending...please contact administrator";
                            lblpbInfo.Refresh();
                            bProceed = false;
                        }
                        else
                        {
                            lblpbInfo.Refresh();
                            //lblpbInfo.Text = "Dicom Sending Service is not running...Starting Dicom Sending Service";
                            lblpbInfo.Text = "Starting Upload...Please Wait";
                            lblpbInfo.Refresh();

                            var proc2 = new ProcessStartInfo();
                            proc2.UseShellExecute = true;
                            proc2.WorkingDirectory = @"C:\Windows\System32";
                            proc2.FileName = @"C:\Windows\System32\cmd.exe";
                            proc2.Verb = "runas";
                            proc2.Arguments = "/c net start DICOMSenderService";
                            proc2.WindowStyle = ProcessWindowStyle.Hidden;
                            Process.Start(proc2);
                            Process.GetCurrentProcess().WaitForExit(5000);

                            CheckServiceStatus(ref bRetRecStatus, ref strRecStat, ref bRetSendStatus, ref strSendStat);
                            if (strSendStat.ToUpper().IndexOf("RUN") < 0)
                            {
                                lblpbInfo.Refresh();
                                lblpbInfo.Text = "Failed to start Dicom Sending Service...please contact administrator";
                                lblpbInfo.Refresh();
                                bProceed = false;
                            }
                            else
                            {
                                lblpbInfo.Refresh();
                                lblpbInfo.Text = "Dicom Sending Service Started";
                                lblpbInfo.Refresh();
                                bProceed = true;
                            }


                        }
                    }
                    #endregion

                    #endregion

                    #region Proceed to upload
                    if (bProceed)
                    {
                        if (LatestVer == CurrentVer)
                        {
                            strSID = "S1D" + DateTime.Now.ToString("MMddyyHHmmss") + CoreCommon.RandomString(3);
                            pb1.Visible = true;
                            UploadFiles(strSID);

                            lblpbInfo.Visible = false;
                            pb1.Visible = false;
                        }
                        else
                        {
                            sb.Append("The installed version of the router is " + CurrentVer);
                            sb.Append("You should download the latest version " + LatestVer + " in order to continue the file uploading process");
                            MessageBox.Show(sb.ToString(), strWinHdr, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    #endregion
                }
                else
                {
                    MessageBox.Show("No files found to upload", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception LexpErr)
            {
                MessageBox.Show(LexpErr.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

        #region UploadFiles
        private void UploadFiles(string strSID)
        {

            int intFileCount = 0;
            int intFileUploaded = 0;
            int intpbProg = 0;
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];
            string[] arr = new string[0];

            string strFileName = string.Empty;
            string strListedFileName = string.Empty;
            string strDirName = string.Empty;
            string strTgtDirName = string.Empty;
            string strPrefix = string.Empty;
            string strSUID = string.Empty;
            string _Stat = string.Empty;

            List<string> arrSUID = new List<string>();
            dd = new DicomDecoder();

            try
            {
                intFileCount = lstFiles.Items.Count;

                if (intFileCount > 0)
                {
                    lblpbInfo.Refresh();
                    lblpbInfo.Text = "Number of file(s) to upload : " + intFileCount.ToString();
                    pb1.Minimum = 1;
                    pb1.Maximum = intFileCount;

                    #region upload files selected
                    foreach (string strFile in lstFiles.Items)
                    {
                        intpbProg = intpbProg + 1;
                        pb1.Value = intpbProg;
                        lblpbInfo.Refresh();
                        lblpbInfo.Text = "Uploading file " + intpbProg.ToString() + " of " + intFileCount.ToString();

                        pathElements = strFile.Split('\\');
                        strFileName = pathElements[(pathElements.Length - 1)];

                        if (CoreCommon.IsDicomFile(strFile))
                        {

                            #region DICOM Files
                            dd.DicomFileName = strFile;
                            List<string> str = dd.dicomInfo;

                            arr = new string[7];
                            arr = GetallTags(str);
                            strSUID = arr[0].Trim();

                            if (strSUID.Trim() != string.Empty)
                            {
                                //strManualUploadDirPath = strManualUploadDirPath + "\\" + strSUID;

                                if (!System.IO.Directory.Exists(strManualUploadDirPath))
                                {
                                    System.IO.Directory.CreateDirectory(strManualUploadDirPath);
                                }

                                if (!SUIDExists(arrSUID, strSUID)) arrSUID.Add(strSUID);

                                strPrefix = CoreCommon.RandomString(6);
                                //strFileName = strSID + "_" + strPrefix + "_" + strFileName;
                                strFileName = strInstCode + "_" + strSID + "_" + strInstName.Replace(" ", "_") + "_" + strPrefix + "_" + strFileName.Replace(strSID + "_", "");

                                if (File.Exists(strManualUploadDirPath + "\\" + strFileName))
                                {
                                    System.IO.File.Delete(strManualUploadDirPath + "\\" + strFileName);
                                }

                                System.IO.File.Copy(strFile, strManualUploadDirPath + "\\" + strFileName);
                                intFileUploaded = intFileUploaded + 1;
                            }
                            #endregion

                        }
                        else if ((MIMEAssistant.GetMIMEType(strFile) == "image/jpeg") || (MIMEAssistant.GetMIMEType(strFile) == "image/gif") || (MIMEAssistant.GetMIMEType(strFile) == "image/png") || (MIMEAssistant.GetMIMEType(strFile) == "image/bmp"))
                        {
                            #region Image Files
                            if (!System.IO.Directory.Exists(strManualUploadDirPath))
                            {
                                System.IO.Directory.CreateDirectory(strManualUploadDirPath);
                            }
                            strPrefix = CoreCommon.RandomString(6);
                            //strFileName = strSID + "_" + strPrefix + "_" + strFileName;
                            strFileName = strInstCode + "_" + strSID + "_" + strInstName.Replace(" ", "_") + "_" + strPrefix + "_" + strFileName.Replace(strSID + "_", "");
                            if (File.Exists(strManualUploadDirPath + "\\" + strFileName))
                            {
                                System.IO.File.Delete(strManualUploadDirPath + "\\" + strFileName);
                            }
                            System.IO.File.Copy(strFile, strManualUploadDirPath + "\\" + strFileName);
                            intFileUploaded = intFileUploaded + 1;
                            #endregion
                        }
                    }
                    #endregion

                    lblpbInfo.Refresh(); lblpbInfo.Text = "";
                    pb1.Refresh();
                    DialogResult result = MessageBox.Show(intFileUploaded.ToString() + " File(s) copied successfully for upload", strWinHdr + " : Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    #region Post Upload
                    if (intFileUploaded > 0)
                    {
                        if (result == DialogResult.OK)
                        {
                            if (chkShowProg.Checked)
                            {
                                lblpbInfo.Visible = false;
                                pb1.Visible = false;
                                lstFiles.Items.Clear();

                                frmFileTransfer frmFT = new frmFileTransfer();
                                frmFT.VETRIS_URL = VETLOGINURL;
                                frmFT.VETRIS_LOGIN_ID = strVETLogin;
                                frmFT.INSTITUTION_CODE = strInstCode;
                                frmFT.IMPORT_SESSION_ID = strSID;
                                frmFT.VETRIS_API_URL = VETAPIURL;
                                frmFT.FILE_COUNT = intFileCount;
                                frmFT.SENDER_DIRECTORY = strSendDirPath;
                                frmFT.ARCHIVE_DIRTECTORY = ARCHDIR;
                                frmFT.ARCHIVE_FILES_TRANSFERRED = strARCHFILE;
                                frmFT.ShowDialog();
                            }
                            else
                            {
                                result = MessageBox.Show("Do you want to view the study in VETRIS now ?", strWinHdr + " : Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (result == DialogResult.Yes)
                                {
                                    for (int i = 0; i < 10000; i++) ;
                                    System.Diagnostics.Process.Start(strVETLOGINURL + "?UID=" + strVETLogin + "&INS=" + strInstCode + "&MID=0");
                                }
                            }

                        }
                    }
                    #endregion
                }
                else
                {
                    MessageBox.Show("No files found to upload", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

        #region SUIDExists
        private bool SUIDExists(List<string> arrSUID, string strSUID)
        {
            bool bReturn = false;

            foreach (string strUID in arrSUID)
            {
                if (strUID == strSUID)
                {
                    bReturn = true;
                    break;
                }
            }

            return bReturn;
        }
        #endregion

        #region GetFileCount
        private void GetFileCount(FileSystemInfo[] FSInfo, ref int intFileCount)
        {
            if (FSInfo == null)
            {
                throw new ArgumentNullException("FSInfo");
            }

            // Iterate through each item.
            foreach (FileSystemInfo i in FSInfo)
            {
                // Check to see if this is a DirectoryInfo object.
                if (i is DirectoryInfo)
                {

                    // Cast the object to a DirectoryInfo object.
                    DirectoryInfo dInfo = (DirectoryInfo)i;

                    // Iterate through all sub-directories.
                    GetFileCount(dInfo.GetFileSystemInfos(), ref intFileCount);
                }
                // Check to see if this is a FileInfo object.
                else if (i is FileInfo)
                {
                    // Add one to the file count.
                    intFileCount++;

                }

            }
        }
        #endregion

        #region Drag & Drop

        #region frmUpload_DragEnter
        private void frmUpload_DragEnter(object sender, DragEventArgs e)
        {
            if (strUploadType == "M")
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true) e.Effect = DragDropEffects.All;
            }
        }
        #endregion

        #region frmUpload_DragDrop
        private void frmUpload_DragDrop(object sender, DragEventArgs e)
        {
            bool IsFile = false;
            bool IsDirectory = false;
            StringBuilder sb = new StringBuilder();

            if (strUploadType == "M")
            {
                string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in droppedFiles)
                {
                    if (File.GetAttributes(file).HasFlag(FileAttributes.Directory))
                        IsDirectory = true;
                    else
                        IsFile = true;

                    if (IsFile)
                    {
                        if (CheckValidFileFormat(file))
                        {
                            if (!IsDuplicateFile(file))
                            {
                                lstFiles.Items.Add(file);
                            }
                            else
                            {
                                sb.AppendLine("File : " + file + "is already added in the list.");
                                //MessageBox.Show("File : " + file + "is already added in the list.", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            sb.AppendLine("Invalid file format : " + file);
                        }

                    }
                    else if (IsDirectory)
                        WalkDirectoryTreeDragDrop(new DirectoryInfo(file));
                }

                if (sb.ToString().Trim() != string.Empty)
                {
                    MessageBox.Show(sb.ToString(), strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region pnlHeader_DragEnter
        private void pnlHeader_DragEnter(object sender, DragEventArgs e)
        {
            if (strUploadType == "M")
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true) e.Effect = DragDropEffects.All;
            }
        }
        #endregion

        #region pnlHeader_DragDrop
        private void pnlHeader_DragDrop(object sender, DragEventArgs e)
        {
            bool IsFile = false;
            bool IsDirectory = false;
            StringBuilder sb = new StringBuilder();

            if (strUploadType == "M")
            {
                string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in droppedFiles)
                {
                    if (File.GetAttributes(file).HasFlag(FileAttributes.Directory))
                        IsDirectory = true;
                    else
                        IsFile = true;

                    if (IsFile)
                    {
                        if (CheckValidFileFormat(file))
                        {
                            if (!IsDuplicateFile(file))
                            {
                                lstFiles.Items.Add(file);
                            }
                            else
                            {
                                sb.AppendLine("File : " + file + "is already added in the list.");
                                //MessageBox.Show("File : " + file + "is already added in the list.", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            sb.AppendLine("Invalid file format : " + file);
                        }

                    }
                    else if (IsDirectory)
                        WalkDirectoryTreeDragDrop(new DirectoryInfo(file));
                }

                if (sb.ToString().Trim() != string.Empty)
                {
                    MessageBox.Show(sb.ToString(), strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region panel2_DragEnter
        private void panel2_DragEnter(object sender, DragEventArgs e)
        {
            if (strUploadType == "M")
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true) e.Effect = DragDropEffects.All;
            }
        }
        #endregion

        #region panel2_DragDrop
        private void panel2_DragDrop(object sender, DragEventArgs e)
        {
            bool IsFile = false;
            bool IsDirectory = false;
            StringBuilder sb = new StringBuilder();

            if (strUploadType == "M")
            {
                string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in droppedFiles)
                {
                    if (File.GetAttributes(file).HasFlag(FileAttributes.Directory))
                        IsDirectory = true;
                    else
                        IsFile = true;

                    if (IsFile)
                    {
                        if (CheckValidFileFormat(file))
                        {
                            if (!IsDuplicateFile(file))
                            {
                                lstFiles.Items.Add(file);
                            }
                            else
                            {
                                sb.AppendLine("File : " + file + "is already added in the list.");
                                //MessageBox.Show("File : " + file + "is already added in the list.", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            sb.AppendLine("Invalid file format : " + file);
                        }

                    }
                    else if (IsDirectory)
                        WalkDirectoryTreeDragDrop(new DirectoryInfo(file));
                }

                if (sb.ToString().Trim() != string.Empty)
                {
                    MessageBox.Show(sb.ToString(), strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region pnlAction_DragEnter
        private void pnlAction_DragEnter(object sender, DragEventArgs e)
        {
            if (strUploadType == "M")
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true) e.Effect = DragDropEffects.All;
            }
        }
        #endregion

        #region pnlAction_DragDrop
        private void pnlAction_DragDrop(object sender, DragEventArgs e)
        {
            bool IsFile = false;
            bool IsDirectory = false;
            StringBuilder sb = new StringBuilder();

            if (strUploadType == "M")
            {
                string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in droppedFiles)
                {
                    if (File.GetAttributes(file).HasFlag(FileAttributes.Directory))
                        IsDirectory = true;
                    else
                        IsFile = true;

                    if (IsFile)
                    {
                        if (CheckValidFileFormat(file))
                        {
                            if (!IsDuplicateFile(file))
                            {
                                lstFiles.Items.Add(file);
                            }
                            else
                            {
                                sb.AppendLine("File : " + file + "is already added in the list.");
                                //MessageBox.Show("File : " + file + "is already added in the list.", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            sb.AppendLine("Invalid file format : " + file);
                        }

                    }
                    else if (IsDirectory)
                        WalkDirectoryTreeDragDrop(new DirectoryInfo(file));
                }

                if (sb.ToString().Trim() != string.Empty)
                {
                    MessageBox.Show(sb.ToString(), strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region lstFiles_DragEnter
        private void lstFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (strUploadType == "M")
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true) e.Effect = DragDropEffects.All;
            }
        }
        #endregion

        #region lstFiles_DragDrop
        private void lstFiles_DragDrop(object sender, DragEventArgs e)
        {
            bool IsFile = false;
            bool IsDirectory = false;
            StringBuilder sb = new StringBuilder();

            if (strUploadType == "M")
            {
                string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in droppedFiles)
                {
                    if (File.GetAttributes(file).HasFlag(FileAttributes.Directory))
                        IsDirectory = true;
                    else
                        IsFile = true;

                    if (IsFile)
                    {
                        if (CheckValidFileFormat(file))
                        {
                            if (!IsDuplicateFile(file))
                            {
                                lstFiles.Items.Add(file);
                            }
                            else
                            {
                                sb.AppendLine("File : " + file + "is already added in the list.");
                                //MessageBox.Show("File : " + file + "is already added in the list.", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            sb.AppendLine("Invalid file format : " + file);
                        }

                    }
                    else if (IsDirectory)
                        WalkDirectoryTreeDragDrop(new DirectoryInfo(file));
                }

                if (sb.ToString().Trim() != string.Empty)
                {
                    MessageBox.Show(sb.ToString(), strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region WalkDirectoryTreeDragDrop
        private void WalkDirectoryTreeDragDrop(System.IO.DirectoryInfo root)
        {
            string[] pathElements = new string[0];
            string[] fileElements = new string[0];
            string[] arr = new string[0];
            //int isDiacom = -1;

            string strSUID = string.Empty;
            string strFile = string.Empty;
            string strFilename = string.Empty;
            string strParentFolder = string.Empty;
            string strMIMEType = string.Empty;
            string strDirName = string.Empty;
            string[] arrFiles = new string[0];
            string strPrefix = string.Empty;
            string strSID = string.Empty;

            System.IO.FileInfo[] files = null;
            System.IO.DirectoryInfo[] subDirs = null;

            #region get file list
            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.*");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException ex)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
                ;
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                //Console.WriteLine(e.Message);
                ;
            }
            #endregion

            if (files != null)
            {

                foreach (System.IO.FileInfo fi in files)
                {

                    strFile = fi.FullName;

                    if (CheckValidFileFormat(strFile))
                    {
                        if (!IsDuplicateFile(strFile))
                        {
                            lstFiles.Items.Add(strFile);
                        }
                    }
                }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (System.IO.DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    WalkDirectoryTreeDragDrop(dirInfo);

                }
            }
        }
        #endregion

        #endregion

        #region btnRemove_Click
        private void btnRemove_Click(object sender, EventArgs e)
        {
            int intFlg = 0;
            if (lstFiles.Items.Count > 0)
            {
                for (int n = lstFiles.Items.Count - 1; n >= 0; --n)
                {
                    if (lstFiles.GetSelected(n))
                    {
                        intFlg = 1;
                        lstFiles.Items.RemoveAt(n);
                    }
                }

                if (intFlg == 0)
                {
                    MessageBox.Show("Pleae select file(s) to remove.", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("There are no file(s) to remove.", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #endregion

        #region CheckValidFileFormat
        private bool CheckValidFileFormat(string strFilePath)
        {
            bool bRet = false;
            string[] pathElements = new string[0];

            if (CoreCommon.IsDicomFile(strFilePath))
            {
                pathElements = strFilePath.Split('\\');
                if (pathElements[pathElements.Length - 1].Trim().ToUpper().Contains("DICOMDIR"))
                    bRet = false;
                else
                    bRet = true;
            }
            else if ((MIMEAssistant.GetMIMEType(strFilePath) == "image/jpeg") || (MIMEAssistant.GetMIMEType(strFilePath) == "image/gif") || (MIMEAssistant.GetMIMEType(strFilePath) == "image/png") || (MIMEAssistant.GetMIMEType(strFilePath) == "image/bmp"))
            {
                bRet = true;
            }
            else
            {
                // MessageBox.Show("Invalid file format : " + strFilePath, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bRet = false;
            }


            return bRet;
        }
        #endregion

        #region IsDuplicateFile
        private bool IsDuplicateFile(string strFilePath)
        {
            bool bRet = false;

            for (int i = 0; i < lstFiles.Items.Count; i++)
            {
                if (lstFiles.Items[i].ToString() == strFilePath)
                {
                    bRet = true;
                    break;
                }
            }

            return bRet;
        }
        #endregion

        #region Common DICOM Methods

        #region GetStudyUID
        private string GetStudyUID(List<string> str)
        {

            string UserCaseID = string.Empty;
            string s1, s4, s5, s11, s12;

            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                if ((s11.ToUpper() == "0020") && (s12.ToUpper() == "000D"))
                {
                    UserCaseID = s5.Replace("\0", "");
                    break;
                }

            }
            return UserCaseID;

        }
        #endregion

        #region ExtractStrings
        void ExtractStrings(string s1, out string s4, out string s5, out string s11, out string s12)
        {
            int ind;
            string s2, s3;
            ind = s1.IndexOf("//");
            s2 = s1.Substring(0, ind);
            s11 = s1.Substring(0, 4);
            s12 = s1.Substring(4, 4);
            s3 = s1.Substring(ind + 2);
            ind = s3.IndexOf(":");
            s4 = s3.Substring(0, ind);
            s5 = s3.Substring(ind + 1);
        }
        #endregion

        #region GetallTags
        private string[] GetallTags(List<string> str)
        {

            string strDescription = string.Empty;
            string UserCaseID = string.Empty;
            string ModalityID = string.Empty;
            string Strname = string.Empty;
            string DOB = string.Empty;
            string result = string.Empty;
            string UserSeriesID = string.Empty;
            string SeriesNumber = string.Empty;
            string PatientName = string.Empty;

            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                string s1, s4, s5, s11, s12;
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                if ((s11.ToUpper() == "0010") && (s12.ToUpper() == "0010"))
                {
                    PatientName = s5.Replace("\0", "");

                }
                else if ((s11.ToUpper() == "0020") && (s12.ToUpper() == "000D"))
                {
                    UserCaseID = s5.Replace("\0", "");

                }
                else if ((s11.ToUpper() == "0020") && (s12.ToUpper() == "000E"))
                {
                    UserSeriesID = s5.Replace("\0", "");
                    break;
                }

                else if ((s11.ToUpper() == "0020") && (s12.ToUpper() == "0011"))
                {
                    SeriesNumber = s5.Replace("\0", "");

                }
            }
            string[] arr = new string[7];
            arr[0] = UserCaseID;
            arr[1] = UserSeriesID;
            arr[2] = SeriesNumber;
            arr[3] = PatientName;

            /*arr[1] = strDescription;
            arr[2] = ModalityID;
            arr[3] = Strname;
            arr[4] = result;*/

            return arr;

        }
        #endregion

        #endregion

        #region SetScanner
        private void SetScanner(bool displayScanner)
        {
            if (displayScanner)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    pnlScan.Visible = true;
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                });
            }
            else
            {
                this.Invoke((MethodInvoker)delegate
                {
                    pnlScan.Visible = false;
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                });
            }
        }
        #endregion

        #region CheckServiceStatus
        private void CheckServiceStatus(ref bool bRetRecStatus, ref string strRecStat, ref bool bRetSendStatus, ref string strSendStat)
        {
            //Console.WriteLine("hello\n" + Directory.GetCurrentDirectory());
            //MessageBox.Show(Directory.GetCurrentDirectory());
            Label.CheckForIllegalCrossThreadCalls = false;
            Service objService = new Service();


            try
            {
                #region checking Dicom Receiver Service status
                objService.SERVICE_NAME = "Dicom Receiving Service";
                strRecStat = objService.CheckStatus();

                if (strRecStat.ToUpper().IndexOf("RUN") >= 0)
                {
                    bRetRecStatus = true;
                }
                else if (strRecStat.ToUpper().IndexOf("STOP") >= 0)
                {
                    bRetRecStatus = false;
                }
                else if (strRecStat.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    bRetRecStatus = false;
                }
                else if (strRecStat.ToUpper().IndexOf("PENDING") >= 0)
                {
                    bRetRecStatus = false;
                }

                #endregion

                #region checking Dicom Sender Service status
                objService.SERVICE_NAME = "Dicom Sending Service";
                strSendStat = objService.CheckStatus();

                if (strSendStat.ToUpper().IndexOf("RUN") >= 0)
                {
                    bRetSendStatus = true;
                }
                else if (strSendStat.ToUpper().IndexOf("STOP") >= 0)
                {
                    bRetSendStatus = false;
                }
                else if (strSendStat.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    bRetSendStatus = false;
                }
                else if (strSendStat.ToUpper().IndexOf("PENDING") >= 0)
                {
                    bRetSendStatus = false;
                }
                #endregion

            }
            catch (Exception LexpErr)
            {
                MessageBox.Show(LexpErr.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bRetRecStatus = false;
                bRetSendStatus = false;
            }
            finally
            {
                objService = null;
            }

        }
        #endregion

        #region btnClose_Click
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region IsFileLocked
        private bool IsFileLocked(Exception exception)
        {
            int errorCode = Marshal.GetHRForException(exception) & ((1 << 16) - 1);
            return errorCode == ERROR_SHARING_VIOLATION || errorCode == ERROR_LOCK_VIOLATION;
        }
        #endregion

        #region UnlockFileProcess
        private void UnlockFileProcess(string strFilePath)
        {
            List<Process> ProcList = AppClasses.HandleFileLock.GetProcessesLockingFile(strFilePath);
            foreach (var process in ProcList)
            {
                process.Kill();
            }
        }
        #endregion

        #region IsRunAsAdmin
        private bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(id);


            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        #endregion



    }
}
