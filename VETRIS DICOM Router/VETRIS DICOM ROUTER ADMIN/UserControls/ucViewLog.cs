using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using VETRISRouter.Core;

namespace VETRIS_DICOM_ROUTER_ADMIN.UserControls
{
    public partial class ucViewLog : UserControl
    {
        #region Members & Variables
        private static string strWinHdr = "VETRIS DICOM ROUTER"; //ConfigurationSettings.AppSettings["WinHdr"];
        Scheduler objCoreSh;
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;
        #endregion

        public ucViewLog()
        {
            InitializeComponent();
        }

        #region ucViewLog_Load
        private void ucViewLog_Load(object sender, EventArgs e)
        {
            SetControlValues();
            ResetValues();
            FetchSchedulerLogData();
        }
        #endregion

        #region SetControlValues
        private void SetControlValues()
        {
            for (int intHr = 0; intHr <= 23; intHr++)
            {
                cmbFromHr.Items.Add(padZero(intHr));
                cmbToHr.Items.Add(padZero(intHr));
            }


            for (int intMins = 0; intMins <= 59; intMins++)
            {
                cmbFromMin.Items.Add(padZero(intMins));
                cmbToMin.Items.Add(padZero(intMins));
            }

            cmbType.Items.Add("All");
            cmbType.Items.Add("Information");
            cmbType.Items.Add("Error");

            cmbSvcNm.Items.Add("All");
            cmbSvcNm.Items.Add("Dicom Receiving Service");
            cmbSvcNm.Items.Add("Dicom Sending Service");
        }
        #endregion

        #region padZero
        public string padZero(int LiNum)
        {
            return (LiNum < 10) ? '0' + Convert.ToString(LiNum) : Convert.ToString(LiNum);
        }
        #endregion

        #region ResetValues
        private void ResetValues()
        {
            cmbFromHr.Text = "00"; cmbFromMin.Text = "00";
            cmbToHr.Text = "23"; cmbToMin.Text = "59";
            cmbType.Text = "All";
            cmbSvcNm.Text = "All";
        }
        #endregion

        #region FetchSchedulerLogData
        private void FetchSchedulerLogData()
        {
            objCoreSh = new Scheduler();
            DataTable dtbl = new DataTable();
            string strCatchMsg = string.Empty;



            try
            {

                objCoreSh.FROM_DATE = Convert.ToDateTime(dtpFrom.Value.ToString("ddMMMyyyy") + " " + cmbFromHr.Text + ":" + cmbFromMin.Text + ":00");
                objCoreSh.TO_DATE = Convert.ToDateTime(dtpTo.Value.ToString("ddMMMyyyy") + " " + cmbToHr.Text + ":" + cmbToMin.Text + ":00");

                objCoreSh.LOG_TYPE = cmbType.Text.Substring(0, 1);
                if (cmbSvcNm.Text == "All") objCoreSh.SERVICE_NAME = ""; else objCoreSh.SERVICE_NAME = cmbSvcNm.Text;

                dtbl = objCoreSh.ViewLog(Application.StartupPath, ref strCatchMsg);
                if (strCatchMsg.Trim() != string.Empty)
                    MessageBox.Show(strCatchMsg, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    FillListView(dtbl);
            }
            catch (Exception expErr)
            {
                MessageBox.Show(expErr.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                objCoreSh = null;
                dtbl = null;
            }
        }
        #endregion

        #region FillListView
        private void FillListView(DataTable dtbl)
        {
            if (dtbl == null)
            {
                lvw_log_view.Items.Clear();
                return;
            }
            lvw_log_view.Columns.Clear();
            lvw_log_view.Items.Clear();


            foreach (DataColumn c in dtbl.Columns)
            {
                //adding names of columns as Listview columns				
                ColumnHeader h = new ColumnHeader();

                if (c.ColumnName.ToLower() == "is_error")
                {
                    h.Text = "Type";
                    h.Width = 84;
                    // c.SetOrdinal(0);
                }
                if (c.ColumnName.ToLower() == "service_name")
                {
                    h.Text = "Service";
                    h.Width = 150;
                    // c.SetOrdinal(1);
                }
                if (c.ColumnName.ToLower() == "log_date")
                {
                    h.Text = "Date/Time";
                    h.Width = 72;
                    // c.SetOrdinal(2);
                }

                //if (c.ColumnName.ToLower() == "log_type")
                //{
                //    h.Text = "Type";
                //    h.Width = 84;
                //}
                if (c.ColumnName.ToLower() == "log_message")
                {
                    h.Text = "Message";
                    h.Width = 280;
                    //c.SetOrdinal(3);
                }

                this.lvw_log_view.Columns.Add(h);
            }


            string[] str = new string[dtbl.Columns.Count];

            //adding Datarows as listview Grids
            foreach (DataRow rr in dtbl.Rows)
            {
                for (int col = 0; col <= dtbl.Columns.Count - 1; col++)
                {
                    str[col] = rr[col].ToString();
                }
                ListViewItem ii;
                ii = new ListViewItem(str);
                this.lvw_log_view.Items.Add(ii);
            }
            lvw_log_view.Visible = true;
        }
        #endregion

        #region btnFilter_Click
        private void btnFilter_Click(object sender, EventArgs e)
        {
            FetchSchedulerLogData();
        }
        #endregion

        #region btnReset_Click
        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetValues();
            FetchSchedulerLogData();
        }
        #endregion

        #region btnPurge_Click
        private void btnPurge_Click(object sender, EventArgs e)
        {
            bool bReturn = false;
            string strReturnMessage = ""; string strCatchMsg = "";
            objCoreSh = new Scheduler();
            try
            {
                if (MessageBox.Show("Are you sure to purge the logs?", strWinHdr + " : Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    objCoreSh.FROM_DATE = Convert.ToDateTime(dtpFrom.Value.ToString("ddMMMyyyy") + " " + cmbFromHr.Text + ":" + cmbFromMin.Text + ":00");
                    objCoreSh.TO_DATE = Convert.ToDateTime(dtpTo.Value.ToString("ddMMMyyyy") + " " + cmbToHr.Text + ":" + cmbToMin.Text + ":00");
                    objCoreSh.LOG_TYPE = cmbType.Text.Substring(0, 1);
                    if (cmbSvcNm.Text == "All") objCoreSh.SERVICE_NAME = ""; else objCoreSh.SERVICE_NAME = cmbSvcNm.Text;

                    bReturn = objCoreSh.PurgeLog(Application.StartupPath, ref strReturnMessage, ref strCatchMsg);
                    if (bReturn)
                    {
                        MessageBox.Show("Log purged successfully", strWinHdr + " : Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FetchSchedulerLogData();
                    }
                    else
                    {
                        if (strCatchMsg.Trim() != "")
                        {
                            MessageBox.Show(strCatchMsg, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show(strReturnMessage, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception expErr)
            {
                MessageBox.Show(expErr.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                objCoreSh = null; strReturnMessage = null; strCatchMsg = null;
            }
        }
        #endregion

        #region btnClose_Click
        private void btnClose_Click(object sender, EventArgs e)
        {
            string strMsg = string.Empty; string _Stat = string.Empty;
            _Stat = "Cancel";
            ApplicationDelegateEventArgs args = new ApplicationDelegateEventArgs(_Stat);
            IdentityUpdated(this, args);
        }
        #endregion

    }
}
