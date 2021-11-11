using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Web;

namespace VETRIS_DICOM_ROUTER_FILE_UPLOAD
{
    public partial class frmConnectivity : Form
    {
        #region Variables
        private static string FTPHost = string.Empty;
        private static string FTPUser = string.Empty;
        private static string FTPPwd = string.Empty;
        #endregion

        #region Properties
        public string FTP_HOST
        {
            get { return FTPHost; }
            set { FTPHost = value; }
        }
        public string FTP_USER_ID
        {
            get { return FTPUser; }
            set { FTPUser = value; }
        }
        public string FTP_PASSWORD
        {
            get { return FTPPwd; }
            set { FTPPwd = value; }
        }
        #endregion

        public frmConnectivity()
        {
            InitializeComponent();
        }

        #region frmConnectivity_Load
        private void frmConnectivity_Load(object sender, EventArgs e)
        {
            pbxOnline.Top = pbxCheck.Top; pbxOnline.Left = pbxCheck.Left;
            pbxOffline.Top = pbxCheck.Top; pbxOffline.Left = pbxCheck.Left;
            timer1.Start();
        }
        #endregion

        #region CheckConnection
        private void CheckConnection()
        {


            FtpWebRequest requestDir = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + FTPHost));
            requestDir.Credentials = new NetworkCredential(FTPUser, FTPPwd);

            try
            {

                requestDir.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = requestDir.GetResponse();
                pbxCheck.Visible = false;
                pbxOnline.Visible = true;
                lblResult.Refresh();
                lblResult.Text = "You are online";
                lblResult.Refresh();


            }
            catch (Exception ex)
            {
                pbxCheck.Visible = false;
                pbxOffline.Visible = true;
                lblResult.Refresh();
                lblResult.Text = "You are offline";
                lblResult.Refresh();
            }

            timer1.Stop();
        }
        #endregion

        #region timer1_Tick
        private void timer1_Tick(object sender, EventArgs e)
        {
            CheckConnection();
        } 
        #endregion
    }
}
