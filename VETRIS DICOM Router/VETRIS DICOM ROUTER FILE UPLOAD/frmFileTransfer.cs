using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net;
using System.Web;
using System.IO;
using System.Web.Script.Serialization;
using VETRISRouter.Core;

namespace VETRIS_DICOM_ROUTER_FILE_UPLOAD
{
    public partial class frmFileTransfer : BaseForm
    {
        #region Variables
        private static string VetrisURL = string.Empty;
        private static string LoginID = string.Empty;
        private static string InstitutionCode = string.Empty;
        private static string ImportSessionID = string.Empty;
        private static string VetrisAPIURL = string.Empty;
        private static int FileCount = 0;
        private static int FileXferCount = 0;
        private static string SenderDir = string.Empty;
        private static string ArchiveDir = string.Empty;
        private static string ArchiveFiles = string.Empty;
        private static int MenuID = 0;

        const int ERROR_SHARING_VIOLATION = 32;
        const int ERROR_LOCK_VIOLATION = 33;
        #endregion

        #region Properties
        public int FILE_COUNT
        {
            get { return FileCount; }
            set { FileCount = value; }
        }
        public string VETRIS_URL
        {
            get { return VetrisURL; }
            set { VetrisURL = value; }
        }
        public string VETRIS_LOGIN_ID
        {
            get { return LoginID; }
            set { LoginID = value; }
        }
        public string INSTITUTION_CODE
        {
            get { return InstitutionCode; }
            set { InstitutionCode = value; }
        }
        public string IMPORT_SESSION_ID
        {
            get { return ImportSessionID; }
            set { ImportSessionID = value; }
        }
        public string VETRIS_API_URL
        {
            get { return VetrisAPIURL; }
            set { VetrisAPIURL = value; }
        }
        public string ARCHIVE_DIRTECTORY
        {
            get { return ArchiveDir; }
            set { ArchiveDir = value; }
        }
        public string SENDER_DIRECTORY
        {
            get { return SenderDir; }
            set { SenderDir = value; }
        }
        public string ARCHIVE_FILES_TRANSFERRED
        {
            get { return ArchiveFiles; }
            set { ArchiveFiles = value; }
        }
        public int MENU_ID
        {
            get { return MenuID; }
            set { MenuID = value; }
        }
        #endregion

        //Thread threadInput;

        //private readonly SynchronizationContext synchContext;
        //private DateTime dt = DateTime.Now;
        
        string strProcPer = "0%";
        string strProcText = string.Empty;
        int intProgVal = 0;


        public frmFileTransfer()
        {
            InitializeComponent();
            //synchContext = SynchronizationContext.Current;
           
        }

        #region frmFileTransfer_Load
        private void frmFileTransfer_Load(object sender, EventArgs e)
        {
            //lblTot.Text = FileCount.ToString();
            //lblTot2.Text = FileCount.ToString();

            pnlMsg.Top = pnlProc.Top;
            pnlMsg.Left = pnlProc.Left;
            pnlConfirm.Top = pnlMsg.Top;
            pnlConfirm.Left = pnlMsg.Left;
        } 
        #endregion

        #region frmFileTransfer_LoadCompleted
        private void frmFileTransfer_LoadCompleted()
        {
            CheckSession();
        }
        
        #endregion

        #region CheckSession
        private void CheckSession()
        {
            //SetScanner(true);
            string strRespMsg = string.Empty;
            string apiUrl = VetrisAPIURL;
            string json = string.Empty;
            WebClient client = new WebClient();
            string inputJson = string.Empty;
            bool bProcess = false;
            string strText = string.Empty;
            string strTxtPath = string.Empty;
            string[] arrFiles = new string[0];
            string[] arrZips = new string[0];
            int intCount = 0;
            int intInc =0;
            DateTime dtStart = DateTime.Now;
            DateTime dtLast = DateTime.Now;
            int intTimeTaken = 0;
           

            //this.Invoke((MethodInvoker)delegate
            //{
                try
                {

                    #region Session File Counting
                    lblProgDtls.Refresh();
                    lblProgDtls.Text = "Preparing file(s) for upload";
                    lblProgDtls.Refresh();
                    strProcText = "Preparing file(s) for upload";

                    #region notify the upload to support (suspended)

                    //object inputNotify1 = new
                    //{
                    //    institutionCode = InstitutionCode.Trim(),
                    //    importSessionID = ImportSessionID.Trim(),
                    //    importFileCount = FileCount,
                    //    uploadDate = DateTime.Now,
                    //};
                    //inputJson = (new JavaScriptSerializer()).Serialize(inputNotify1);
                    //client.Headers["Content-type"] = "application/json";
                    //client.Encoding = Encoding.UTF8;
                    //json = client.UploadString(apiUrl + "/DicomRouterCreateUploadNotification", inputJson);

                    //JavaScriptSerializer serNotify1 = new JavaScriptSerializer();
                    //AppClasses.DicomRouterCreateUploadNotification createNotify1 = serNotify1.Deserialize<AppClasses.DicomRouterCreateUploadNotification>(json);

                    //strRespMsg = createNotify1.responseStatus.responseMessage;
                    ////if (strRespMsg.Trim() != "SUCCESS")
                    ////{
                    ////    CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "SUID : " + strSUID.Trim() + " :: File Name : " + strFilename + " :: transfered for upload");
                    ////}

                    #endregion

                    #region Processing for upload
                    while (arrFiles.Length < FileCount)
                    {
                        if (Directory.Exists(ArchiveDir + "\\" + ImportSessionID))
                        {
                            arrFiles = new string[0];
                            arrFiles = Directory.GetFiles(ArchiveDir + "\\" + ImportSessionID);
                            //lblCount2.Text = arrFiles.Length.ToString();
                            //lblCount2.Refresh();

                            pbProc.Refresh();
                            intInc = Convert.ToInt32(Math.Round((((Convert.ToDecimal(arrFiles.Length)) / Convert.ToDecimal(FileCount)) * 10), 0));
                            intProgVal = pbProc.Value + (intInc - pbProc.Value);
                            pbProc.Value = intProgVal;
                            pbProc.Refresh();
                            lblProg.Refresh();
                            lblProg.Text = pbProc.Value.ToString() + "%";
                            lblProg.Refresh();
                            strProcPer = intProgVal.ToString() + "%";
                        }
                        if (arrFiles.Length == FileCount) break;
                        Application.DoEvents();
                    }

                    lblProgDtls.Refresh();
                    lblProgDtls.Text = "Uploading file(s)";
                    lblProgDtls.Refresh();
                    strProcText = "Uploading file(s)";

                    while (arrFiles.Length > 0)
                    {
                        arrFiles = new string[0];
                        arrFiles = Directory.GetFiles(SenderDir, InstitutionCode + "_" + ImportSessionID + "_*.zip");
                        intCount = (FileCount - arrFiles.Length);
                        //lblCount.Text = (FileCount - arrFiles.Length).ToString();
                        //lblCount.Refresh();

                        pbProc.Refresh();
                        intInc = Convert.ToInt32(Math.Round((((Convert.ToDecimal(intCount)) / Convert.ToDecimal(FileCount)) * 50), 0));
                        intProgVal = pbProc.Value + (intInc - (pbProc.Value - 10));
                        pbProc.Value = intProgVal;
                        pbProc.Refresh();
                        lblProg.Refresh();
                        lblProg.Text = pbProc.Value.ToString() + "%";
                        lblProg.Refresh();
                        strProcPer = intProgVal.ToString() + "%";

                        if (arrFiles.Length == 0)
                        {
                            bProcess = true;
                            break;
                        }
                        Application.DoEvents();
                    }
                    #endregion

                    #endregion

                    intCount = 0;
                    intInc = 0;

                    if (ArchiveFiles == "N")
                    {
                        #region delete files from archive
                        try
                        {
                            arrFiles = new string[0];
                            arrFiles = Directory.GetFiles(ArchiveDir + "\\" + ImportSessionID);
                            foreach (string strFile in arrFiles)
                            {
                                if (File.Exists(strFile)) File.Delete(strFile);
                            }
                            if (Directory.GetFiles(ArchiveDir + "\\" + ImportSessionID).Length == 0) Directory.Delete(ArchiveDir + "\\" + ImportSessionID);
                        }
                        catch(Exception ex)
                        {
                            ;
                        }

                        #endregion
                    }

                    if (bProcess)
                    {
                        lblProgDtls.Refresh();
                        lblProgDtls.Text = "Upload completed. Processing file(s)";
                        lblProgDtls.Refresh();
                        strProcText = "Upload completed. Processing file(s)";

                        #region Processing
                        while (FileXferCount < FileCount)
                        {
                            lblWait.Visible = true;
                            object input = new
                            {
                                institutionCode = InstitutionCode.Trim(),
                                importSessionID = ImportSessionID.Trim(),
                            };
                            inputJson = (new JavaScriptSerializer()).Serialize(input);
                            client.Headers["Content-type"] = "application/json";
                            client.Encoding = Encoding.UTF8;
                            json = client.UploadString(apiUrl + "/DicomRouterCheckSession", inputJson);

                            JavaScriptSerializer ser = new JavaScriptSerializer();
                            AppClasses.DicomRouterCheckSession checkSess = ser.Deserialize<AppClasses.DicomRouterCheckSession>(json);

                            strRespMsg = checkSess.responseStatus.responseMessage;
                            FileXferCount = checkSess.ImportedFileCount;

                            if (strRespMsg.Trim() != "SUCCESS")
                            {
                                pnlMsg.Visible = true;
                                lblMsg.Text = strRespMsg;
                                break;
                            }

                            pbProc.Refresh();
                            intInc = Convert.ToInt32(Math.Round((((Convert.ToDecimal(FileXferCount)) / Convert.ToDecimal(FileCount)) * 40), 0));
                            intProgVal = pbProc.Value + (intInc - (pbProc.Value - 60));
                            pbProc.Value = intProgVal;
                            pbProc.Refresh();
                            lblProg.Refresh();
                            lblProg.Text = pbProc.Value.ToString() + "%";
                            lblProg.Refresh();
                            strProcPer = intProgVal.ToString() + "%";

                            if (FileXferCount == FileCount) break;
                            else
                            {
                                if((DateTime.Now - dtLast).Minutes > 5)
                                {
                                    intTimeTaken = (DateTime.Now - dtStart).Minutes;

                                    #region notify the longer time to support
                                    object inputNotify1 = new
                                    {
                                        institutionCode = InstitutionCode.Trim(),
                                        importSessionID = ImportSessionID.Trim(),
                                        importFileCount = FileCount,
                                        transferFileCount = FileXferCount,
                                        uploadDate = dtStart,
                                        timeTakenInMinutes = intTimeTaken
                                    };
                                    inputJson = (new JavaScriptSerializer()).Serialize(inputNotify1);
                                    client.Headers["Content-type"] = "application/json";
                                    client.Encoding = Encoding.UTF8;
                                    json = client.UploadString(apiUrl + "/DicomRouterCreateFileTransferOTNotification", inputJson);

                                    JavaScriptSerializer serNotify1 = new JavaScriptSerializer();
                                    AppClasses.DicomRouterCreateFileTransferOTNotification createNotify1 = serNotify1.Deserialize<AppClasses.DicomRouterCreateFileTransferOTNotification>(json);

                                    strRespMsg = createNotify1.responseStatus.responseMessage;

                                    ////if (strRespMsg.Trim() != "SUCCESS")
                                    ////{
                                    ////    CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "SUID : " + strSUID.Trim() + " :: File Name : " + strFilename + " :: transfered for upload");
                                    ////}

                                    #endregion

                                    dtLast = DateTime.Now;
                                }
                            }
                            Application.DoEvents();
                        }
                        #endregion

                        pnlProc.Visible = false;
                        lblWait.Visible = false;
                        pnlConfirm.Visible = true;
                        this.Cursor = System.Windows.Forms.Cursors.Default;

                        #region notify the download to support (suspended)

                        //object inputNotify2 = new
                        //{
                        //    institutionCode = InstitutionCode.Trim(),
                        //    importSessionID = ImportSessionID.Trim(),
                        //    importFileCount = FileXferCount,
                        //    downloadDate = DateTime.Now,
                        //};
                        //inputJson = (new JavaScriptSerializer()).Serialize(inputNotify2);
                        //client.Headers["Content-type"] = "application/json";
                        //client.Encoding = Encoding.UTF8;
                        //json = client.UploadString(apiUrl + "/DicomRouterCreateDownloadNotification", inputJson);

                        //JavaScriptSerializer serNotify2 = new JavaScriptSerializer();
                        //AppClasses.DicomRouterCreateDownloadNotification createNotify2 = serNotify2.Deserialize<AppClasses.DicomRouterCreateDownloadNotification>(json);

                        //strRespMsg = createNotify2.responseStatus.responseMessage;
                        ////if (strRespMsg.Trim() != "SUCCESS")
                        ////{
                        ////    CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "SUID : " + strSUID.Trim() + " :: File Name : " + strFilename + " :: transfered for upload");
                        ////}

                        #endregion

                        #region notify the tranfer to support

                        object inputNotify2 = new
                        {
                            institutionCode = InstitutionCode.Trim(),
                            importSessionID = ImportSessionID.Trim(),
                            importFileCount = FileXferCount,
                            uploadDate = dtStart,
                            downloadDate = DateTime.Now,
                        };
                        inputJson = (new JavaScriptSerializer()).Serialize(inputNotify2);
                        client.Headers["Content-type"] = "application/json";
                        client.Encoding = Encoding.UTF8;
                        json = client.UploadString(apiUrl + "/DicomRouterCreateFileTransferNotification", inputJson);

                        JavaScriptSerializer serNotify2 = new JavaScriptSerializer();
                        AppClasses.DicomRouterCreateFileTransferNotification createNotify2 = serNotify2.Deserialize<AppClasses.DicomRouterCreateFileTransferNotification>(json);

                        strRespMsg = createNotify2.responseStatus.responseMessage;
                        ////if (strRespMsg.Trim() != "SUCCESS")
                        ////{
                        ////    CoreCommon.doLog(strConfigPath, intSvcID, strSvcName, "N", "SUID : " + strSUID.Trim() + " :: File Name : " + strFilename + " :: transfered for upload");
                        ////}

                        #endregion
                    }


                }
                catch (Exception ex)
                {
                    pnlProc.Visible = false;
                    pnlMsg.Visible = true;
                    lblMsg.Text = ex.Message;
                    //timer1.Stop();
                    //SetScanner(false);
                }
                finally
                {
                    client.Dispose();
                }
            //});
            //timer1.Stop();
            //SetScanner(false);
        }
        #endregion

        #region SetScanner
        private void SetScanner(bool displayScanner)
        {
            if (displayScanner)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    //pnlConfirm.Visible = false;
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                });
            }
            else
            {
                this.Invoke((MethodInvoker)delegate
                {
                    if (pnlMsg.Visible == false)
                    {
                        pnlProc.Visible = false;
                        lblWait.Visible = false;
                        pnlConfirm.Visible = true;
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                    }
                });
            }
        }
        #endregion

        #region timer1_Tick
        private void timer1_Tick(object sender, EventArgs e)
        {
            //threadInput = new Thread(CheckSession);
            //threadInput.Start();
        }
        #endregion

        #region btnYes_Click
        private void btnYes_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(VetrisURL + "?UID=" + LoginID + "&INS=" + InstitutionCode + "&MID=" + MenuID.ToString());
            this.Close();
        }
        #endregion

        #region btnNo_Click
        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
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

    }
}
