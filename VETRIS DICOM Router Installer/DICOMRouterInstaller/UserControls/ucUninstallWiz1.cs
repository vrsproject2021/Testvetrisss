using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DICOMRouterInstaller.UserControls
{
    public partial class ucUninstallWiz1 : UserControl
    {
        #region Members & Variables
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;
        private string strInstCode = string.Empty;
        private string strInstName = string.Empty;
        #endregion

        public ucUninstallWiz1()
        {
            InitializeComponent();
        }

        #region ucUninstallWiz1_Load
        private void ucUninstallWiz1_Load(object sender, EventArgs e)
        {
            string strInstName = string.Empty;
            GetSettings(ref strInstCode, ref strInstName);
            lblInstName.Text = strInstName + " (" + strInstCode + ")";
            lblInstallPath.Text = frmMain.InstallPath;
        } 
        #endregion

        #region btnPrev_Click
        private void btnPrev_Click(object sender, EventArgs e)
        {
            string _Stat = string.Empty;
            int _Screen = 0;

            _Stat = "Uninstall";
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

            _Stat = "Uninstall";
            _Screen = 2;
            ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, _Screen);
            IdentityUpdated(this, args);
            

        }
        #endregion

        #region btnCancel_Click
        private void btnCancel_Click(object sender, EventArgs e)
        {
            string strMsg = string.Empty; string _Stat = string.Empty;
            strMsg = "Are you sure to quit the uninstallation process ?";
            DialogResult result = MessageBox.Show(strMsg, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

                _Stat = "Exit";
                ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat, 0);
                IdentityUpdated(this, args);
            }
        }
        #endregion

        #region GetSettings
        private bool GetSettings(ref string InstitutionCode, ref string InstitutionName)
        {
            bool bReturn = false;
            string strInstallPath = frmMain.InstallPath;
            string strDBPath = frmMain.DBPath + "\\Config.xml";
            DataSet ds = new DataSet();

            //OleDbConnection con = new OleDbConnection(strConn);
            //string sqlQuery = string.Empty;
            string strControlCode = string.Empty;
            string strControlValue = string.Empty;
            //frmMain.DBPath = strDBPath;

            try
            {
                #region Suspended
                // con.Open();
                //using (dtbl = new DataTable())
                //{
                //    sqlQuery = "select * from sys_scheduler_settings";

                //    using (OleDbDataAdapter adapter = new OleDbDataAdapter(sqlQuery, con))
                //    {
                //        adapter.Fill(dtbl);
                //    }


                //    foreach (DataRow dr in dtbl.Rows)
                //    {
                //        strControlCode = Convert.ToString(dr["control_code"]).Trim();
                //        strControlValue = Convert.ToString(dr["control_value"]).Trim();
                //        if (strControlCode == "INSTNAME") InstitutionName = strControlValue;

                //        DataView dv = new DataView(frmMain.dtbl, "", "", DataViewRowState.CurrentRows);
                //        dv.RowFilter = "control_code='" + strControlCode + "'";
                //        if (dv.ToTable().Rows.Count > 0)
                //        {
                //            dv[0]["control_value"] = strControlValue;
                //            frmMain.dtbl.AcceptChanges();
                //        }
                //        dv.Dispose();
                //    }

                //}
                #endregion

                ds.ReadXml(strDBPath);
                ds.Tables[0].TableName = "Control";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    strControlCode = Convert.ToString(dr["control_code"]);
                    switch (strControlCode)
                    {
                        case "INSTNAME":
                            InstitutionName = Convert.ToString(dr["control_value"]).Trim();
                            break;
                        case "SITECODE":
                            InstitutionCode = Convert.ToString(dr["control_value"]).Trim();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bReturn = false;
            }
            finally
            {
                ds.Dispose();
            }
            return bReturn;
        }
        #endregion
    }
}
