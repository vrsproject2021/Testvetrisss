using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace VETRIS_DICOM_ROUTER
{
    public partial class frmOption : Form
    {
        public frmOption()
        {
            InitializeComponent();
        }

        #region frmOption_Load
        private void frmOption_Load(object sender, EventArgs e)
        {
            string s = AppDomain.CurrentDomain.BaseDirectory;

        }
        #endregion

        #region btnOk_Click
        private void btnOk_Click(object sender, EventArgs e)
        {
            string strExe = AppDomain.CurrentDomain.BaseDirectory + "\\VETRIS DICOM ROUTER FILE UPLOAD.exe";
            if (rdoUpload.Checked)
            {
                Process ProcUpload = new Process();
                ProcUpload.StartInfo.UseShellExecute = false;
                ProcUpload.StartInfo.FileName = strExe;
                ProcUpload.StartInfo.RedirectStandardOutput = true;
                ProcUpload.StartInfo.RedirectStandardError = true;
                ProcUpload.Start();
                this.Close();
            }
            else if (rdoAdmin.Checked)
            {
                frmPassword frm = new frmPassword();
                frm.Show();
                this.Hide();
            }
        }
        #endregion

        #region btnClose_Click
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region frmOption_FormClosed
        private void frmOption_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        } 
        #endregion
    }
}
