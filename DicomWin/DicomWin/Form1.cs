using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.ServiceProcess;

namespace DicomWin
{
    public partial class Form1 : Form
    {
        #region Windows Messaging Interface declarations
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern Int32 FindWindow(String lpClassName, String lpWindowName);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);
        [DllImport("Shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr ShellExecute(int hwnd, string lpOperation, string lpFile, string lpParameters,
                            string lpDirectory, int nShowCmd);


        public const int WM_USER = 0x400;
        public const int WM_COPYDATA = 0x4A;
        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }
        #endregion

        #region Shared variables
        string studyUID = null;
        string credential = null;
        string userName = null;
        string password = null;
        string server = null;
        string loginsessionId = null;
        bool isLoggedIn = false;
        int hWndPACS = 0;
        Guid? transactionId = null;
        #endregion

        private log4net.ILog logger;
        public Form1()
        {
            logger = log4net.LogManager.GetLogger("Log");
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // do not show any window; also hidden from taskbar
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            // get command line arguments expect one
            var args = Environment.GetCommandLineArgs();
            if (args.Length < 2)
            {
                MessageBox.Show("StudyUID and/or Credential was not supplied!", "eRAD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Debug("Exit application: StudyUID and/or Credential was not supplied!");
                Application.Exit();
                return;
            }
            /*
             *                                                                encrypted(user±password)  
             * vetrisepacs://open?uid=1.2.826.0.1.3680043.2.950.25981.5394.20200826161508&c=ZidbsoHtq7Swso9ez7aX6jkmO0kQN3ic
             * 
             * Here, multiple uids should be seperated with a semicolon;
             * vetrisepacs://open?uid=1.2.826.0.1.3680043.2.950.25981.5394.20200826161508;1.2.392.200036.9125.2.26160195117224.64954176654.330418&c=ZidbsoHtq7Swso9ez7aX6jkmO0kQN3ic
             */

            Regex re = new Regex(@"vetrisepacs\:\/\/open\/?\?uid=(?<uid>.*)&c=(?<cred>.*)", RegexOptions.IgnoreCase);
            var m = re.Match(args[1]);
            if (re.IsMatch(args[1]))
            {
                studyUID = m.Groups["uid"].Value.Trim();
                credential = m.Groups["cred"].Value.Trim();
            }
            else
            {
                if (args.Length < 3)
                {
                    MessageBox.Show("StudyUID and Credential was not supplied!", "eRAD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    logger.Debug("Exit application: StudyUID and/or Credential was not supplied!");
                    Application.Exit();
                    return;
                }
                studyUID = args[1];
                credential = args[2];
            }
            OpenViewer();
        }

        private bool CheckService()
        {
            //eRAD PACS Service  
            logger.Debug("eRAD PACS Service existance check...");
            var service = ServiceController.GetServices()
                .FirstOrDefault(s => s.ServiceName == "eRAD PACS Service");
            if (service == null)
            {
                logger.Debug("eRAD PACS Service no" +
                    "t installed.");
                MessageBox.Show("eRAD PACS is not installed", "eRAD",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            logger.Debug("eRAD PACS Service status check...");

            ServiceController sc = new ServiceController();
            sc.ServiceName = "eRAD PACS Service";

            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                logger.Debug("eRAD PACS Service status: STOPPED.");
                try
                {
                    // Start the service, and wait until its status is "Running".
                    logger.Debug("eRAD PACS Service START and waiting for RUNNING...");
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running);
                    return true;
                }
                catch (InvalidOperationException)
                {
                    logger.Error("eRAD PACS Service CANNOT START SERVICE.");
                    MessageBox.Show("Could not start the eRAD PACS Service.", "eRAD",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            return true;
        }

        private void OpenViewer()
        {
            hWndPACS = 0;

            string filename = Application.StartupPath + "\\dcmwin.cfg";

            TextReader tr = new StreamReader(filename);
            String sViewerOpening = null;
            string sConfig = tr.ReadLine();
            string[] arrCfg = new string[0];
            string[] arrCred = DecryptString(credential).Split('±');  // new line

            userName = string.Empty;
            password = string.Empty;
            server = string.Empty;

            sConfig = DecryptString(sConfig);
            arrCfg = sConfig.Split('±');
            userName = arrCred[0].Trim();  // new line
            password = arrCred[1].Trim();  // new line

            server = arrCfg[2].Trim();
            sViewerOpening = string.Format("eradw://{0}?cuser={1}&cpartner={2:X}", server, userName, this.Handle);
            logger.Debug("Invoke Viewer: URL: " + sViewerOpening);
            var installed = CheckService();
            if (!installed)
            {
                logger.Debug("Service not installed. Exit.");
                Application.Exit();
            }
            logger.Debug("Finding eRAD PACS Message Wnd...");
            hWndPACS = FindWindow(null, "eRAD PACS Message Wnd");
            if (hWndPACS == 0)
            {
                logger.Debug("Open viewer URL:"+ sViewerOpening);
                var ret = ShellExecute(0, "open", sViewerOpening, null, null, SW_HIDE);
                int count = 0;
                while (hWndPACS == 0 && count < 30)
                {
                    Thread.Sleep(2000);  // wait 2 seconds to start
                    logger.Debug("Finding eRAD PACS Message Wnd..."+(count+1).ToString());
                    hWndPACS = FindWindow(null, "eRAD PACS Message Wnd");
                    if (hWndPACS == 0) count++;
                }
                if (hWndPACS == 0)
                {
                    logger.Debug("Finding eRAD PACS Message Wnd...FAILED. eRAD PACS is not running.");
                    MessageBox.Show("eRAD PACS is not running", "eRAD",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
            }


            transactionId = Guid.NewGuid();
            var session = GetSessionData();
            if (session == null)
            {

                CreateSession();
            }
            else
            {
                this.loginsessionId = session.SessionId;
                this.transactionId = session.TransactionId;
                OpenStudy();
            }
            
        }

        private void CreateSession()
        {
            logger.Info("eRAD PACS: Request new session...");
            String msg = String.Format(@"
                <ipcwindowmessage transactionid=""{0}"">
                        <login serverhttp=""http"" servername=""{1}"" serverport=""80""
                        username=""{2}"" password=""{3}"" sessionid="""" serverversion=""8.0.0.0"">
                        </login>
                </ipcwindowmessage>",
                transactionId, server, userName, password);


            var result = SendWindowsStringMessage(hWndPACS, this.Handle.ToInt32(), msg);
        }

        private SessionInfo GetSessionData()
        {
            string fileName = Path.Combine(Path.GetTempPath(), "session__info.json");
            if (!File.Exists(fileName))
            {
                logger.Info("There is no saved session found.");
                return null;
            }
            string data = File.ReadAllText(fileName);
            var session = Newtonsoft.Json.JsonConvert.DeserializeObject<SessionInfo>(DecryptString(data));
            logger.Info($"Existing session ID: {session.SessionId}");
                
            return session;
        }

        private void WriteSessionInfo(SessionInfo info)
        {
            string fileName = Path.Combine(Path.GetTempPath(), "session__info.json");
            var data = EncryptString( Newtonsoft.Json.JsonConvert.SerializeObject(info));
            try
            {
                File.WriteAllText(fileName, data);
            }
            catch (Exception ex) {
                logger.Info($"Writing session data FAILED!");
            }
        }


        #region DecryptString
        private static string DecryptString(string toDecryptString)
        {
            byte[] keyArray;
            byte[] toDecryptArray = Convert.FromBase64String(toDecryptString);
            string key = "7";
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF7.GetBytes(key));
            hashmd5.Clear();
            byte[] key24Array = new byte[24];
            for (int i = 0; i < 16; i++)
            {
                key24Array[i] = keyArray[i];
            }
            for (int i = 0; i < 7; i++)
            {
                key24Array[i + 16] = keyArray[i];
            }
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = key24Array;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cryptoTransform = tripledes.CreateDecryptor();
            byte[] resultArray = cryptoTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
            tripledes.Clear();
            UTF8Encoding encoder = new UTF8Encoding();
            return encoder.GetString(resultArray);
        }
        #endregion

        #region EncryptString
        private static string EncryptString(string toEncryptString)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncryptString);
            string key = "7";
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF7.GetBytes(key));
            hashmd5.Clear();
            byte[] key24Array = new byte[24];
            for (int i = 0; i < 16; i++)
            {
                key24Array[i] = keyArray[i];
            }
            for (int i = 0; i < 7; i++)
            {
                key24Array[i + 16] = keyArray[i];
            }
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = key24Array;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cryptoTransform = tripledes.CreateEncryptor();
            byte[] resultArray = cryptoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tripledes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        #endregion


        protected override void WndProc(ref Message m)
        {

            if (m.Msg == WM_COPYDATA)
            {

                COPYDATASTRUCT cds = new COPYDATASTRUCT();
                cds = (COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(COPYDATASTRUCT));
                if (cds.dwData.ToInt32() == 2)
                {
                    if (cds.cbData > 0)
                    {
                        this.studyUID = cds.lpData;
                        OpenViewer();
                        m.Result = (IntPtr)1;
                    }
                }
                else
                {
                    if (cds.cbData > 0)
                    {
                        XDocument xdoc = XDocument.Parse(cds.lpData);
                        var element = xdoc.Descendants("login").FirstOrDefault();
                        if (element != null)
                        {
                            this.loginsessionId = element.Attributes("loginsessionid").FirstOrDefault().Value;
                            isLoggedIn = element.Attributes("logged").FirstOrDefault().Value.Equals("1");
                            logger.Info("eRAD PACS: New Session ID : " + loginsessionId);
                            var sessionInfo = new SessionInfo() { Date = DateTime.Now, Username = this.userName, SessionId = loginsessionId, TransactionId=this.transactionId.Value };
                            WriteSessionInfo(sessionInfo);
                            OpenStudy();
                        }
                        else
                        {
                            element = xdoc.Descendants("open").FirstOrDefault();
                            if (element != null)
                            {
                                try
                                {
                                    var success = element.Attributes("success").FirstOrDefault().Value;
                                    if (success=="0")
                                    {
                                        logger.Info($"Session expired!");
                                        CreateSession();
                                    }
                                    else if (success == "1")
                                    {
                                        logger.Info($"Mediator closed.");
                                        this.Close();
                                        Application.Exit();
                                    }
                                }
                                catch 
                                {
                                }
                                
                            }
                        }

                        m.Result = (IntPtr)1;
                    }
                }

            }

            base.WndProc(ref m);
        }

        private void OpenStudy()
        {

            String msg = String.Format(@"
                   <ipcwindowmessage transactionid=""{0}""> 
                          <open loginsessionid=""{1}"" studyuids=""{2}""></open> 
                   </ipcwindowmessage>",
             transactionId, this.loginsessionId, studyUID);

            logger.Debug("eRAD PACS: Opening Study with Session ID : " + loginsessionId);
            var result = SendWindowsStringMessage(hWndPACS, this.Handle.ToInt32(), msg);

        }

        public int SendWindowsStringMessage(int hWnd, int wParam, string msg)
        {
            int result = 0;

            if (hWnd > 0)
            {
                byte[] sarr = System.Text.Encoding.Default.GetBytes(msg);
                int len = sarr.Length;
                COPYDATASTRUCT cds;
                cds.dwData = (IntPtr)1;
                cds.lpData = msg;
                cds.cbData = len + 1;
                result = SendMessage(hWnd, WM_COPYDATA, wParam, ref cds);
            }

            return result;
        }
    }
}
