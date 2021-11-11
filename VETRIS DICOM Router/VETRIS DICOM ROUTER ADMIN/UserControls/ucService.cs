using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Configuration;
using VETRISRouter.Core;
using System.IO;

namespace VETRIS_DICOM_ROUTER_ADMIN.UserControls
{
    public partial class ucService : UserControl
    {
        #region Members & Variables
        private static string strWinHdr = "VETRIS DICOM ROUTER";
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;

        private Service objService = null;
        public delegate string WaitDelegate(string name);


        public enum ServiceType { DCMRCV = 1, DCMSND = 2 }
        public int ServiceTypeId = 0;

        private string strServiceName = string.Empty;
        private string strServiceHint = string.Empty;
        private string strServiceStatus = string.Empty;
        #endregion

        public ucService()
        {
            InitializeComponent();
        }

        #region ucServices_Load
        private void ucService_Load(object sender, EventArgs e)
        {
            CheckServiceStatus();
        }
        #endregion

        #region CheckServiceStatus
        private void CheckServiceStatus()
        {
            //Console.WriteLine("hello\n" + Directory.GetCurrentDirectory());
            //MessageBox.Show(Directory.GetCurrentDirectory());
            Label.CheckForIllegalCrossThreadCalls = false;
            objService = new Service();
            try
            {
                #region checking Dicom Receiver Service status
                objService.SERVICE_NAME = "Dicom Receiving Service";
                lblDRStatus.Text = objService.CheckStatus();

                if (lblDRStatus.Text.ToUpper().IndexOf("RUN") >= 0)
                {
                    lblDRStatus.ForeColor = Color.Blue;
                    btnDRStop.Visible = true;
                    btnDRStart.Visible = false;
                    btnDRStop.Left = btnDRStart.Left;
                    btnDRStop.Top = btnDRStart.Top;
                }
                else if (lblDRStatus.Text.ToUpper().IndexOf("STOP") >= 0)
                {
                    lblDRStatus.ForeColor = Color.Red;
                    btnDRStop.Visible = false;
                    btnDRStart.Visible = true;
                }
                else if (lblDRStatus.Text.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    lblDRStatus.ForeColor = Color.Red;
                    btnDRStop.Visible = false;
                    btnDRStart.Visible = false;
                }
                else if (lblDRStatus.Text.ToUpper().IndexOf("PENDING") >= 0)
                {
                    lblDRStatus.ForeColor = Color.Red;
                    btnDRStop.Visible = false;
                    btnDRStart.Visible = false;
                }
                lblDRStatus.Refresh();
                #endregion

                #region checking Dicom Sender Service status
                objService.SERVICE_NAME = "Dicom Sending Service";
                lblDSStatus.Text = objService.CheckStatus();

                if (lblDSStatus.Text.ToUpper().IndexOf("RUN") >= 0)
                {
                    lblDSStatus.ForeColor = Color.Blue;
                    btnDSStop.Visible = true;
                    btnDSStart.Visible = false;
                    btnDSStop.Left = btnDSStart.Left;
                    btnDSStop.Top = btnDSStart.Top;
                }
                else if (lblDSStatus.Text.ToUpper().IndexOf("STOP") >= 0)
                {
                    lblDSStatus.ForeColor = Color.Red;
                    btnDSStop.Visible = false;
                    btnDSStart.Visible = true;
                }
                else if (lblDSStatus.Text.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    lblDSStatus.ForeColor = Color.Red;
                    btnDSStop.Visible = false;
                    btnDSStart.Visible = false;
                }
                else if (lblDSStatus.Text.ToUpper().IndexOf("PENDING") >= 0)
                {
                    lblDSStatus.ForeColor = Color.Red;
                    btnDSStop.Visible = false;
                    btnDSStart.Visible = false;
                }
                #endregion

            }
            catch (Exception LexpErr)
            {
                MessageBox.Show(LexpErr.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                objService = null;
            }

        }
        #endregion

        #region btnDSStart_Click
        private void btnDSStart_Click(object sender, EventArgs e)
        {
            lblDSProcess.Visible = true;
            lblDSProcess.Text = "Starting Service...Please Wait...";
            lblDSProcess.Refresh();
            ServiceTypeId = (int)ServiceType.DCMSND;
            doProcess_Wait();
        }
        #endregion

        #region btnDSStop_Click
        private void btnDSStop_Click(object sender, EventArgs e)
        {
            lblDSProcess.Visible = true;
            lblDSProcess.Text = "Stopping Service...Please Wait...";
            lblDSProcess.Refresh();
            ServiceTypeId = (int)ServiceType.DCMSND;
            doProcess_Wait();
        }
        #endregion

        #region btnDRStop_Click
        private void btnDRStop_Click(object sender, EventArgs e)
        {
            lblDRProcess.Visible = true;
            lblDRProcess.Text = "Stopping Service...Please Wait...";
            lblDRProcess.Refresh();
            ServiceTypeId = (int)ServiceType.DCMRCV;
            doProcess_Wait();

        }
        #endregion

        #region btnDRStart_Click
        private void btnDRStart_Click(object sender, EventArgs e)
        {
            Label.CheckForIllegalCrossThreadCalls = false;
            lblDRProcess.Visible = true;
            lblDRProcess.Text = "Starting Service...Please Wait...";
            lblDRProcess.Refresh();
            ServiceTypeId = (int)ServiceType.DCMRCV;
            doProcess_Wait();

        }
        #endregion

        #region doProcess_Wait
        public string doProcess_Wait()
        {

            WaitDelegate dc = new WaitDelegate(this.doStart);
            AsyncCallback cb = new AsyncCallback(this.GetResultsOnCallback);
            IAsyncResult ar = dc.BeginInvoke("ok", cb, null);
            return "ok";
        }
        #endregion

        #region doStart
        private string doStart(string name)
        {
            Label.CheckForIllegalCrossThreadCalls = false;
            objService = new Service();
            try
            {
                switch (ServiceTypeId)
                {
                    case 1:
                        objService.SERVICE_NAME = "Dicom Receiving Service";
                        if (btnDRStart.Visible)
                        {
                            if (objService.Start())
                            {

                                lblDRStatus.ForeColor = Color.Blue;
                                lblDRStatus.Text = "(Running...)";
                                lblDRStatus.Refresh();
                                btnDRStop.Visible = true;
                                btnDRStart.Visible = false;
                                btnDRStop.Left = btnDRStart.Left;
                                btnDRStop.Top = btnDRStart.Top;
                            }
                        }
                        else
                        {
                            if (objService.Stop())
                            {
                                lblDRStatus.ForeColor = Color.Red;
                                lblDRStatus.Text = "(Stopped...)";
                                lblDRStatus.Refresh();
                                btnDRStop.Visible = false;
                                btnDRStart.Visible = true;
                            }
                        }
                        lblDRProcess.Visible = false;
                        break;

                    case 2:
                        objService.SERVICE_NAME = "Dicom Sending Service";
                        if (btnDSStart.Visible)
                        {
                            if (objService.Start())
                            {
                                lblDSStatus.ForeColor = Color.Blue;
                                lblDSStatus.Text = "(Running...)";
                                lblDSStatus.Refresh();
                                btnDSStop.Visible = true;
                                btnDSStart.Visible = false;
                                btnDSStop.Left = btnDSStart.Left;
                                btnDSStop.Top = btnDSStart.Top;
                            }
                        }
                        else
                        {
                            if (objService.Stop())
                            {
                                lblDSStatus.ForeColor = Color.Red;
                                lblDSStatus.Text = "(Stopped...)";
                                lblDSStatus.Refresh();
                                btnDSStop.Visible = false;
                                btnDSStart.Visible = true;
                            }
                        }
                        lblDSProcess.Visible = false;
                        break;
                }
            }
            catch (Exception LexpErr)
            {
                MessageBox.Show(LexpErr.Message, strWinHdr + " : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                objService = null;
            }
            return "ok";

        }
        #endregion

        #region GetResultsOnCallback
        public void GetResultsOnCallback(IAsyncResult ar)
        {
            WaitDelegate del = (WaitDelegate)((AsyncResult)ar).AsyncDelegate;
            try
            {
                string result;
                result = del.EndInvoke(ar);
            }
            catch { }
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
