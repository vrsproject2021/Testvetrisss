using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using System.Security;
using VETRISRouter.Core;

namespace VETRIS_DICOM_ROUTER
{
    public partial class frmPassword : Form
    {
        private static string strWinHdr = "VETRIS DICOM ROUTER";
        public bool ReturnValue { get; set; }

        #region Members & Variables
        private string strPwd = string.Empty;
        Scheduler objCore;
        #endregion

        public frmPassword()
        {
            InitializeComponent();
        }

        #region frmPassword_Load
        private void frmPassword_Load(object sender, EventArgs e)
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
                    strPwd = objCore.ADMIN_PASSWORD;
                    strPwd = CoreCommon.DecryptString(strPwd);
                }
                else
                    MessageBox.Show(strCatchMsg, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception expErr)
            { MessageBox.Show(expErr.Message, strWinHdr + " : Exception", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally
            { objCore = null; }
        }
        #endregion

        #region btnOk_Click
        private void btnOk_Click(object sender, EventArgs e)
        {
            string strExe = AppDomain.CurrentDomain.BaseDirectory + "\\VETRIS DICOM ROUTER ADMIN.exe";

            if (txtPwd.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter the password", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ReturnValue = false;
                txtPwd.Focus();
            }
            else if (txtPwd.Text.Trim() != strPwd)
            {
                MessageBox.Show("Wrong password entered", strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ReturnValue = false;
                txtPwd.Focus();
            }
            else
            {
                //SecureString sstr = new SecureString();
                //sstr.AppendChar('x');
                //sstr.AppendChar('@');
                //sstr.AppendChar('1');
                //sstr.AppendChar('3');
                //sstr.AppendChar('9');
                //sstr.AppendChar('3');
                //sstr.AppendChar('0');
                //sstr.AppendChar('5');

                Process ProcAdmin = new Process();
                ProcAdmin.StartInfo.UseShellExecute = false;
                ProcAdmin.StartInfo.FileName = strExe;
                ProcAdmin.StartInfo.RedirectStandardOutput = true;
                ProcAdmin.StartInfo.RedirectStandardError = true;
                //ProcAdmin.StartInfo.Verb = "runas";
                //ProcAdmin.StartInfo.LoadUserProfile = true;
                //ProcAdmin.StartInfo.UserName = "Administrator";
                //ProcAdmin.StartInfo.Password = sstr;
              
                ProcAdmin.Start();
                Application.Exit();
               
            }
            
        }
        #endregion

        #region btnClose_Click
        private void btnClose_Click(object sender, EventArgs e)
        {
            frmOption frmOpt = new frmOption();
            frmOpt.Show();
            this.Close();
        }
        #endregion
    }
}
