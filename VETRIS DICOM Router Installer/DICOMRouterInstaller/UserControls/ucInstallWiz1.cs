using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;

namespace DICOMRouterInstaller.UserControls
{
    public partial class ucInstallWiz1 : UserControl
    {
        #region Members & Variables
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;
        private string VETAPIURL = string.Empty;
        private string RECPATH = string.Empty;
        #endregion

        public ucInstallWiz1()
        {
            InitializeComponent();
        }

        #region ucInstallWiz1_Load
        private void ucInstallWiz1_Load(object sender, EventArgs e)
        {
            PopulateValues();
            txtAddr1.Focus();
            //txtSiteCode.Focus();
            
        }
        #endregion

        #region ucInstallWiz1_Enter
        private void ucInstallWiz1_Enter(object sender, EventArgs e)
        {
            PopulateInstitutionDtls();
        } 
        #endregion

        #region PopulateValues
        private void PopulateValues()
        {
            string strCode = string.Empty;

            foreach (DataRow dr in frmMain.dtbl.Rows)
            {
                strCode = Convert.ToString(dr["control_code"]);

                switch (strCode)
                {
                    case "INSTNAME":
                        txtName.Text = Convert.ToString( dr["control_value"]).Trim();
                        break;
                    case "INSTADDR1":
                        txtAddr1.Text = Convert.ToString(dr["control_value"]).Trim();
                        break;
                    case "INSTADDR2":
                        txtAddr2.Text = Convert.ToString(dr["control_value"]).Trim();
                        break;
                    case "INSTZIP":
                        txtZip.Text = Convert.ToString(dr["control_value"]).Trim();
                        break;
                    case "SITECODE":
                        txtSiteCode.Text = Convert.ToString(dr["control_value"]).Trim();
                        break;
                    case "VETLOGIN":
                        txtVETLoginID.Text = Convert.ToString(dr["control_value"]).Trim();
                        break;
                    case "VETAPIURL":
                        VETAPIURL= Convert.ToString(dr["control_value"]).Trim();
                        break;
                       
                    default:
                        break;
                }
            }
        }
        #endregion

        #region PopulateInstitutionDtls
        private void PopulateInstitutionDtls()
        {
            string strRespMsg = string.Empty;
            string apiUrl = VETAPIURL;
            string json = string.Empty;
            WebClient client = new WebClient();
            string strCode = string.Empty;

            Application.DoEvents();
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\DicomRouter\\DRLicense.lic"))
            {
                try
                {

                    IL.Common.GetInstitutionCode(AppDomain.CurrentDomain.BaseDirectory + "\\DicomRouter\\", ref strCode);
                    txtSiteCode.Text = strCode;
                    if (txtSiteCode.Text.Trim() != string.Empty)
                    {
                        object input = new
                        {
                            institutionCode = txtSiteCode.Text.Trim(),
                        };
                        string inputJson = (new JavaScriptSerializer()).Serialize(input);
                        client.Headers["Content-type"] = "application/json";
                        client.Encoding = Encoding.UTF8;
                        json = client.UploadString(apiUrl + "/DicomRouterInstitutionDetails", inputJson);

                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        IL.DicomRouterInstitutionDetails instDtls = ser.Deserialize<IL.DicomRouterInstitutionDetails>(json);

                        strRespMsg = instDtls.responseStatus.responseMessage;


                        if (strRespMsg.Trim() == "SUCCESS")
                        {
                            txtVETLoginID.Text = instDtls.InstitutionLoginID;
                            txtName.Text = instDtls.InstitutionName;
                            txtAddr1.Text = instDtls.Address_1;
                            txtAddr2.Text = instDtls.Address_2;
                            txtZip.Text = instDtls.Zip;
                            RECPATH = instDtls.StudyImageFilesReceivingPath;
                        }
                        else
                        {
                            MessageBox.Show(strRespMsg, this.Text + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtVETLoginID.ReadOnly = false;
                            txtName.ReadOnly = false;
                            txtZip.ReadOnly = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter the site code", this.Text + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, this.Text + " : Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtVETLoginID.ReadOnly = false;
                    txtName.ReadOnly = false;
                    txtZip.ReadOnly = false;
                }
                finally
                {
                    client.Dispose();
                }
            }
            else
                MessageBox.Show("The license file is missing...Please download the license file by logging into your VETRIS acount", this.Text + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.DoEvents();
        }
        #endregion

        #region btnPrev_Click
        private void btnPrev_Click(object sender, EventArgs e)
        {
            string _Stat = string.Empty;
            int _Screen = 0;

            UpdateValues();
            _Stat = "Install";
            _Screen = 0;
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
                _Screen = 2;
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

            if (txtSiteCode.Text.Trim() == string.Empty)
            {
                strMsg = "Site Code is required";
            }
            if (txtVETLoginID.Text.Trim() == string.Empty)
            {
                if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
                strMsg += "VETRIS Login ID is required";
            }
            if (txtName.Text.Trim() == string.Empty)
            {
                if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
                strMsg += "Institution Name is required";
            }
            if (txtZip.Text.Trim() == string.Empty)
            {
                if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
                strMsg += "Zip is required";
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
                    case "INSTNAME":
                        dr["control_value"] = txtName.Text.Trim();
                        break;
                    case "INSTADDR1":
                        dr["control_value"] = txtAddr1.Text.Trim();
                        break;
                    case "INSTADDR2":
                        dr["control_value"] = txtAddr2.Text.Trim();
                        break;
                    case "INSTZIP":
                        dr["control_value"] = txtZip.Text.Trim();
                        break;
                    case "SITECODE":
                        dr["control_value"] = txtSiteCode.Text.Trim();
                        break;
                    case "VETLOGIN":
                        dr["control_value"] = txtVETLoginID.Text.Trim();
                        break;
                    case "RCVDIRMANUAL":
                        dr["control_value"] = RECPATH.Trim();
                        break;
                    default:
                        break;
                }
            }

            frmMain.dtbl.AcceptChanges();
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
                ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat,0);
                IdentityUpdated(this, args);
            }
        } 
        #endregion

        

        

       
    }
}
