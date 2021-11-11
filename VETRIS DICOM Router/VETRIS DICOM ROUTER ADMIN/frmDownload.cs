using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Diagnostics;

namespace VETRIS_DICOM_ROUTER_ADMIN
{
    public partial class frmDownload : Form
    {
        #region Variables
        private static string VetrisURL = string.Empty;
        private static string LatestVersion = string.Empty;
        private static string ArchiveDirectory = string.Empty;
        #endregion

        #region Properties
        public string LATEST_VERSION
        {
            get { return LatestVersion; }
            set { LatestVersion = value; }
        }
        public string VETRIS_URL
        {
            get { return VetrisURL; }
            set { VetrisURL = value; }
        }
        public string ARCHIVE_DIRECTORY
        {
            get { return ArchiveDirectory; }
            set { ArchiveDirectory = value; }
        }
        #endregion

        public frmDownload()
        {
            InitializeComponent();
        }

        #region frmDownload_Load
        private void frmDownload_Load(object sender, EventArgs e)
        {
            lblDL.ForeColor = Color.Black;
            lblDL.Text = "DICOM Router version " + LatestVersion + " is released. Existing version requires update...";

            lblZipResult.Text = "";
            StartDownload();

        }
        #endregion

        #region StartDownload
        private void StartDownload()
        {
            string strDir = ArchiveDirectory + "\\DICOMRouterSetup\\Version_" + LatestVersion;
            string filepath = strDir + "\\DICOM_ROUTER_SETUP.zip";
            WebClient webClient = new WebClient();
            try
            {
                if (Directory.Exists(strDir)) DeleteDirectory(strDir);
                if (!Directory.Exists(strDir)) Directory.CreateDirectory(strDir);
                if (File.Exists(filepath)) File.Delete(filepath);
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                lblResult.ForeColor = Color.Black;
                lblResult.Text = "Downloading DICOM Router version " + LatestVersion + "... Please wait";
                webClient.DownloadFileAsync(new Uri(VetrisURL + "/DownloadRouter/Version_" + LatestVersion + "/DICOM_ROUTER_SETUP.zip"), filepath);

            }
            catch (Exception ex)
            {
                lblResult.ForeColor = Color.Red;
                lblResult.Text = ex.Message;
            }
        }
        #endregion

        #region DeleteDirectory
        private void DeleteDirectory(string DirectoryName)
        {
            string[] arrFiles = new string[0];
            try
            {
                arrFiles = Directory.GetFiles(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\bin");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\bin");
                arrFiles = Directory.GetFiles(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\etc\\dcmtk");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\etc\\dcmtk");
                Directory.Delete(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\etc");
                arrFiles = Directory.GetFiles(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\dcmtk\\wlistdb\\OFFIS");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\dcmtk\\wlistdb\\OFFIS");
                arrFiles = Directory.GetFiles(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\dcmtk\\wlistdb");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\dcmtk\\wlistdb");
                arrFiles = Directory.GetFiles(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\dcmtk\\wlistqry");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\dcmtk\\wlistqry");
                arrFiles = Directory.GetFiles(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\dcmtk");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\dcmtk");
                arrFiles = Directory.GetFiles(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\doc\\dcmtk");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\doc\\dcmtk");
                Directory.Delete(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\doc");
                Directory.Delete(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share");
                Directory.Delete(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs");

                if (Directory.Exists(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\IMGToDCM"))
                {
                    arrFiles = Directory.GetFiles(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\IMGToDCM");
                    for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                    Directory.Delete(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\IMGToDCM");
                    Directory.Delete(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs");
                }
                arrFiles = Directory.GetFiles(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs");
                arrFiles = Directory.GetFiles(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(DirectoryName + "\\DICOM_ROUTER_SETUP\\DicomRouter");
                arrFiles = Directory.GetFiles(DirectoryName + "\\DICOM_ROUTER_SETUP");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(DirectoryName + "\\DICOM_ROUTER_SETUP");
                Directory.Delete(DirectoryName);
            }
            catch (Exception ex)
            {
                lblResult.ForeColor = Color.Red;
                lblResult.Text = ex.Message;
            }
        }
        #endregion

        #region ProgressChanged
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            lblPer.Refresh();
            lblPer.Text = e.ProgressPercentage.ToString() + " %";
        }
        #endregion

        #region Completed
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            lblResult.Refresh();
            lblResult.Text = "Download completed!";
            lblResult.Refresh();
            progressBar.Visible = false; lblPer.Visible = false;
            lblZipResult.Text = "Decompressing the setup files....Please wait";
            DecompressFiles();
        }
        #endregion

        #region DecompressFiles
        private void DecompressFiles()
        {
            string strDir = ArchiveDirectory + "\\DICOMRouterSetup\\Version_" + LatestVersion;
            string strFile = strDir + "\\DICOM_ROUTER_SETUP.zip";
            string DirectoryName = string.Empty;

            try
            {

                DirectoryName = strDir;
                ZipFile.ExtractToDirectory(strFile, DirectoryName);
                StartInstallation();

            }
            catch (Exception expErr)
            {
                lblZipResult.Refresh();
                lblZipResult.ForeColor = Color.Red;
                lblZipResult.Text = expErr.Message;
                lblZipResult.Refresh();
            }

        }
        #endregion

        #region StartInstallation
        private void StartInstallation()
        {
            lblZipResult.Refresh();
            lblZipResult.Text = "Setup Decompressed";
            lblZipResult.Refresh();
            string strExe = ArchiveDirectory + "\\DICOMRouterSetup\\Version_" + LatestVersion + "\\DICOM_ROUTER_SETUP\\DICOMRouterInstaller.exe";

            Process ProcInstall = new Process();
            ProcInstall.StartInfo.UseShellExecute = false;
            ProcInstall.StartInfo.FileName = strExe;
            ProcInstall.StartInfo.RedirectStandardOutput = true;
            ProcInstall.StartInfo.RedirectStandardError = true;
            ProcInstall.Start();
            this.Hide();
            Application.Exit();
        }
        #endregion
    }
}
