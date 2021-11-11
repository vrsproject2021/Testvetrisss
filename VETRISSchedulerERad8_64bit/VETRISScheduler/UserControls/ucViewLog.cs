using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using VETRISScheduler.Core;

namespace VETRISScheduler.UserControls
{
    public partial class ucViewLog : UserControl
    {
        #region Members & Variables
        
        Scheduler objCore;
        string strWinHdr = "VETRIS";
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
            FetchData();
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
            cmbSvcNm.Items.Add("VETRIS New Data Synch Service");
            cmbSvcNm.Items.Add("VETRIS Write Back Service");
            cmbSvcNm.Items.Add("VETRIS Status Update Service");
            cmbSvcNm.Items.Add("VETRIS Notification Service");
            cmbSvcNm.Items.Add("VETRIS Missing Data Synch Service");
            cmbSvcNm.Items.Add("VETRIS Dayend Service");
            cmbSvcNm.Items.Add("VETRIS FTP & PACS Synch Service");
        }
        #endregion

        #region padZero
        public string padZero(int LiNum)
        {
            return (LiNum < 10) ? '0' + Convert.ToString(LiNum) : Convert.ToString(LiNum);
        }
        #endregion

        #region FetchData
        private void FetchData()
        {
            objCore = new Scheduler();
            DataTable dtbl = new DataTable();
            string strCatchMessage = string.Empty;

            try
            {
                objCore.FROM_DATE = Convert.ToDateTime(dtpFrom.Value.ToString("ddMMMyyyy") + " " + cmbFromHr.Text + ":" + cmbFromMin.Text + ":00");
                objCore.TO_DATE = Convert.ToDateTime(dtpTo.Value.ToString("ddMMMyyyy") + " " + cmbToHr.Text + ":" + cmbToMin.Text + ":00");
                objCore.LOG_TYPE = cmbType.Text.Substring(0, 1);
                if (cmbSvcNm.Text == "All") objCore.SERVICE_NAME = ""; else objCore.SERVICE_NAME = cmbSvcNm.Text;

                dtbl = objCore.ViewLog(Application.StartupPath, ref strCatchMessage);
                if (strCatchMessage.Trim() != string.Empty)
                    MessageBox.Show(strCatchMessage, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    FillListView(dtbl);
            }
            catch (Exception expErr)
            {
                MessageBox.Show(expErr.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                objCore = null;
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
                
                if (c.ColumnName.ToLower() == "log_date")
                {
                    h.Text = "Date";
                    h.Width = 72;
                }
                if (c.ColumnName.ToLower() == "log_time")
                {
                    h.Text = "Time";
                    h.Width = 52;
                }
                if (c.ColumnName.ToLower() == "service_name")
                {
                    h.Text = "Service";
                    h.Width = 150;
                }
                if (c.ColumnName.ToLower() == "log_type")
                {
                    h.Text = "Type";
                    h.Width = 84;
                }
                if (c.ColumnName.ToLower() == "log_message")
                {
                    h.Text = "Message";
                    h.Width = 280;
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
                //Application.DoEvents();
            }
            lvw_log_view.Visible = true;
        }
        #endregion

        #region btnFilter_Click
        private void btnFilter_Click(object sender, EventArgs e)
        {
            FetchData();
        }
        #endregion

        #region btnReset_Click
        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetValues();
            FetchData();
        }
        #endregion

        #region btnPurge_Click
        private void btnPurge_Click(object sender, EventArgs e)
        {
            bool bReturn = false;
            string strReturnMessage = ""; string strCatchMessage = "";
            objCore = new Scheduler();
            try
            {
                if (MessageBox.Show("Are you sure to purge the logs?", strWinHdr + " : Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    objCore.FROM_DATE = Convert.ToDateTime(dtpFrom.Value.ToString("ddMMMyyyy") + " " + cmbFromHr.Text + ":" + cmbFromMin.Text + ":00");
                    objCore.TO_DATE = Convert.ToDateTime(dtpTo.Value.ToString("ddMMMyyyy") + " " + cmbToHr.Text + ":" + cmbToMin.Text + ":00");
                    objCore.LOG_TYPE = cmbType.Text.Substring(0, 1);
                    if (cmbSvcNm.Text == "All") objCore.SERVICE_NAME = ""; else objCore.SERVICE_NAME = cmbSvcNm.Text;

                    bReturn = objCore.PurgeLog(Application.StartupPath, ref strReturnMessage, ref strCatchMessage);
                    if (bReturn)
                    {
                        objCore = null;
                        FetchData();
                        MessageBox.Show(strReturnMessage, strWinHdr + " : Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (strCatchMessage.Trim() != "")
                        {
                            MessageBox.Show(strCatchMessage, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show(strReturnMessage, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        objCore = null;
                    }
                }
            }
            catch (Exception expErr)
            {
                MessageBox.Show(expErr.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                strReturnMessage = null; strCatchMessage = null;
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
