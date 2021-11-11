using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DICOMRouterInstaller.UserControls
{
    public partial class ucInstallWiz3 : UserControl
    {
        #region Members & Variables
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;
        #endregion

        public ucInstallWiz3()
        {
            InitializeComponent();
        }

        #region ucInstallWiz3_Load
        private void ucInstallWiz3_Load(object sender, EventArgs e)
        {
            PopulateValues();
            txtRCVAETITLE.Focus();
        } 
        #endregion

        #region PopulateValues
        private void PopulateValues()
        {
            string strCode = string.Empty;
            string strSettings = string.Empty;
            string[] arrSettings = new string[0];

            foreach (DataRow dr in frmMain.dtbl.Rows)
            {
                strCode = Convert.ToString(dr["control_code"]);

                switch (strCode)
                {
                    case "RCVAETITLE":
                        txtRCVAETITLE.Text = Convert.ToString(dr["control_value"]).Trim();
                        break;
                    case "RCVPORTNO":
                        txtRCVPORTNO.Text = Convert.ToString(dr["control_value"]).Trim();
                        break;
                    case "VETURL":
                        txtURL.Text = Convert.ToString(dr["control_value"]).Trim();
                        break;
                    //case "SNDAETITLE":
                    //    txtSNDAETITLE.Text = Convert.ToString(dr["control_value"]).Trim();
                    //    break;
                    //case "SNDPORTNO":
                    //    txtSNDPORTNO.Text = Convert.ToString(dr["control_value"]).Trim();
                    //    break;
                    //case "PACSSRVRNAME":
                    //    txtPACSSRVRNAME.Text = Convert.ToString(dr["control_value"]).Trim();
                    //    break;
                    default:
                        break;
                }
            }

            GetSettingsString(ref strSettings);
            arrSettings = strSettings.Split('±');

            frmMain.PACSServer = arrSettings[0].Trim();

        }
        #endregion

        #region GetSettingsString
        public static void GetSettingsString(ref string strSettings)
        {
            string strPath = Application.StartupPath + "\\DicomRouter";
            TextReader tr = new StreamReader(strPath + "\\VDR.cfg");
            strSettings = tr.ReadLine();
            strSettings = DecryptString(strSettings);
         
        }
        #endregion

        #region DecryptString
        public static string DecryptString(string toDecryptString)
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

        #region btnPrev_Click
        private void btnPrev_Click(object sender, EventArgs e)
        {
              string _Stat = string.Empty;
            int _Screen = 0;

            UpdateValues();
            _Stat = "Install";
            _Screen = 2;
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
                frmMain.CreateAdminShortCut = chkAdminSC.Checked;
                frmMain.CreateFileUploadShortCut = chkFUSC.Checked;
                _Stat = "Install";
                _Screen = 4;
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

            if (txtRCVAETITLE.Text.Trim() == string.Empty)
            {
                strMsg = "Receiver AE Title is required";
            }
            if (txtRCVPORTNO.Text.Trim() == string.Empty)
            {
                if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
                strMsg += "Receiver Port Number is required";
            }
            if (txtURL.Text.Trim() == string.Empty)
            {
                if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
                strMsg += "VETRSIS URL is required";
            }

            //if (txtSNDAETITLE.Text.Trim() == string.Empty)
            //{
            //    if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
            //    strMsg += "Sender AE Title is required";
            //}
            //if (txtSNDPORTNO.Text.Trim() == string.Empty)
            //{
            //    if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
            //    strMsg += "Sender Port Number is required";
            //}

            //if (txtPACSSRVRNAME.Text.Trim() == string.Empty)
            //{
            //    if (strMsg.Trim() != string.Empty) strMsg += "\r\n";
            //    strMsg += "PACS Server Name/IP is required";
            //}

           

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
                    case "RCVAETITLE":
                        dr["control_value"] = txtRCVAETITLE.Text.Trim();
                        break;
                    case "RCVPORTNO":
                        dr["control_value"] = txtRCVPORTNO.Text.Trim();
                        break;
                    //case "SNDAETITLE":
                    //    dr["control_value"] = txtSNDAETITLE.Text.Trim();
                    //    break;
                    //case "SNDPORTNO":
                    //    dr["control_value"] = txtSNDPORTNO.Text.Trim();
                    //    break;
                    //case "PACSSRVRNAME":
                    //    dr["control_value"] = txtPACSSRVRNAME.Text.Trim();
                    //    break;
                    case "RCVEXEOPTIONS":
                        dr["control_value"] = "-v " + txtRCVPORTNO.Text.Trim() + " -aet " + txtRCVAETITLE.Text.Trim() + " +xs -od";
                        break;
                    //case "SNDEXEOPTIONS":
                    //    dr["control_value"] = "-v +sd +r -aec " + txtSNDAETITLE.Text.Trim() + " " + frmMain.PACSServer.Trim() + " " + txtSNDPORTNO.Text.Trim();
                    //    break;
                    case "COMPXFERFILE":
                        if (chkCompFile.Checked) dr["control_value"] = "Y"; else dr["control_value"] = "N";
                        break;
                    case "ARCHFILE":
                        if (chkArch.Checked) dr["control_value"] = "Y"; else dr["control_value"] = "N";
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
                ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, 0);
                IdentityUpdated(this, args);
            }
        }
        #endregion
    }
}
