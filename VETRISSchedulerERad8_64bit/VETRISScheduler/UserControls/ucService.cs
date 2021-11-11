using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using VETRISScheduler.Core;

namespace VETRISScheduler.UserControls
{
    public partial class ucService : UserControl
    {
        #region Members & Variables
        private static string strWinHdr = System.Configuration.ConfigurationManager.AppSettings["WinHdr"];
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;

        private Services objService = null;
        public delegate string WaitDelegate(string name);


        public enum ServiceType { NDS = 1, WB = 2, SU = 3, NS = 4, DE = 5, MSS = 6, FP = 7, AS=8,RA=9, FD=10, LFP=11}
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
            Label.CheckForIllegalCrossThreadCalls = false;
            objService = new Services();
            try
            {
                #region checking new data synch service status
                objService.SERVICE_NAME = "VETRIS New Data Synch Service";
                lblNDSStatus.Text = objService.CheckStatus();

                if (lblNDSStatus.Text.ToUpper().IndexOf("RUN") >= 0)
                {
                    lblNDSStatus.ForeColor = Color.Blue;
                    btnNDSStop.Visible = true;
                    btnNDSStart.Visible = false;
                    btnNDSStop.Left = btnNDSStart.Left;
                    btnNDSStop.Top = btnNDSStart.Top;
                }
                else if (lblNDSStatus.Text.ToUpper().IndexOf("STOP") >= 0)
                {
                    lblNDSStatus.ForeColor = Color.Red;
                    btnNDSStop.Visible = false;
                    btnNDSStart.Visible = true;
                }
                else if (lblNDSStatus.Text.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    lblNDSStatus.ForeColor = Color.Red;
                    btnNDSStop.Visible = false;
                    btnNDSStart.Visible = false;
                }
                else if (lblNDSStatus.Text.ToUpper().IndexOf("PENDING") >= 0)
                {
                    lblNDSStatus.ForeColor = Color.Red;
                    btnNDSStop.Visible = false;
                    btnNDSStart.Visible = false;
                }
                #endregion

                #region checking write back synch service status
                objService.SERVICE_NAME = "VETRIS Data Write Back Service";
                lblWBStatus.Text = objService.CheckStatus();

                if (lblWBStatus.Text.ToUpper().IndexOf("RUN") >= 0)
                {
                    lblWBStatus.ForeColor = Color.Blue;
                    btnWBStop.Visible = true;
                    btnWBStart.Visible = false;
                    btnWBStop.Left = btnWBStart.Left;
                    btnWBStop.Top = btnWBStart.Top;
                }
                else if (lblWBStatus.Text.ToUpper().IndexOf("STOP") >= 0)
                {
                    lblWBStatus.ForeColor = Color.Red;
                    btnWBStop.Visible = false;
                    btnWBStart.Visible = true;
                }
                else if (lblWBStatus.Text.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    lblWBStatus.ForeColor = Color.Red;
                    btnWBStop.Visible = false;
                    btnWBStart.Visible = false;
                }
                else if (lblWBStatus.Text.ToUpper().IndexOf("PENDING") >= 0)
                {
                    lblWBStatus.ForeColor = Color.Red;
                    btnWBStop.Visible = false;
                    btnWBStart.Visible = false;
                }
                #endregion

                #region checking write back synch service status
                objService.SERVICE_NAME = "VETRIS Status Synch Service";
                lblSUStatus.Text = objService.CheckStatus();

                if (lblSUStatus.Text.ToUpper().IndexOf("RUN") >= 0)
                {
                    lblSUStatus.ForeColor = Color.Blue;
                    btnSUStop.Visible = true;
                    btnSUStart.Visible = false;
                    btnSUStop.Left = btnSUStart.Left;
                    btnSUStop.Top = btnSUStart.Top;
                }
                else if (lblSUStatus.Text.ToUpper().IndexOf("STOP") >= 0)
                {
                    lblSUStatus.ForeColor = Color.Red;
                    btnSUStop.Visible = false;
                    btnSUStart.Visible = true;
                }
                else if (lblSUStatus.Text.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    lblSUStatus.ForeColor = Color.Red;
                    btnSUStop.Visible = false;
                    btnSUStart.Visible = false;
                }
                else if (lblSUStatus.Text.ToUpper().IndexOf("PENDING") >= 0)
                {
                    lblSUStatus.ForeColor = Color.Red;
                    btnSUStop.Visible = false;
                    btnSUStart.Visible = false;
                }
                #endregion

                #region checking notification service status
                objService.SERVICE_NAME = "VETRIS Notification Service";
                lblNSStatus.Text = objService.CheckStatus();

                if (lblNSStatus.Text.ToUpper().IndexOf("RUN") >= 0)
                {
                    lblNSStatus.ForeColor = Color.Blue;
                    btnNSStop.Visible = true;
                    btnNSStart.Visible = false;
                    btnNSStop.Left = btnNSStart.Left;
                    btnNSStop.Top = btnNSStart.Top;
                }
                else if (lblNSStatus.Text.ToUpper().IndexOf("STOP") >= 0)
                {
                    lblNSStatus.ForeColor = Color.Red;
                    btnNSStop.Visible = false;
                    btnNSStart.Visible = true;
                }
                else if (lblNSStatus.Text.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    lblNSStatus.ForeColor = Color.Red;
                    btnNSStop.Visible = false;
                    btnNSStart.Visible = false;
                }
                else if (lblNSStatus.Text.ToUpper().IndexOf("PENDING") >= 0)
                {
                    lblNSStatus.ForeColor = Color.Red;
                    btnNSStop.Visible = false;
                    btnNSStart.Visible = false;
                }
                #endregion

                #region checking day end service status
                objService.SERVICE_NAME = "VETRIS Day End Service";
                lblDEStatus.Text = objService.CheckStatus();

                if (lblDEStatus.Text.ToUpper().IndexOf("RUN") >= 0)
                {
                    lblDEStatus.ForeColor = Color.Blue;
                    btnDEStop.Visible = true;
                    btnDEStart.Visible = false;
                    btnDEStop.Left = btnDEStart.Left;
                    btnDEStop.Top = btnDEStart.Top;
                }
                else if (lblDEStatus.Text.ToUpper().IndexOf("STOP") >= 0)
                {
                    lblDEStatus.ForeColor = Color.Red;
                    btnDEStop.Visible = false;
                    btnDEStart.Visible = true;
                }
                else if (lblDEStatus.Text.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    lblDEStatus.ForeColor = Color.Red;
                    btnDEStop.Visible = false;
                    btnDEStart.Visible = false;
                }
                else if (lblDEStatus.Text.ToUpper().IndexOf("PENDING") >= 0)
                {
                    lblDEStatus.ForeColor = Color.Red;
                    btnDEStop.Visible = false;
                    btnDEStart.Visible = false;
                }
                #endregion

                #region checking missising study service status
                objService.SERVICE_NAME = "VETRIS Missing Data Synch Service";
                lblMSSStatus.Text = objService.CheckStatus();

                if (lblMSSStatus.Text.ToUpper().IndexOf("RUN") >= 0)
                {
                    lblMSSStatus.ForeColor = Color.Blue;
                    btnMSSStop.Visible = true;
                    btnMSSStart.Visible = false;
                    btnMSSStop.Left = btnMSSStart.Left;
                    btnMSSStop.Top = btnMSSStart.Top;
                }
                else if (lblMSSStatus.Text.ToUpper().IndexOf("STOP") >= 0)
                {
                    lblMSSStatus.ForeColor = Color.Red;
                    btnMSSStop.Visible = false;
                    btnMSSStart.Visible = true;
                }
                else if (lblMSSStatus.Text.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    lblMSSStatus.ForeColor = Color.Red;
                    btnMSSStop.Visible = false;
                    btnMSSStart.Visible = false;
                }
                else if (lblMSSStatus.Text.ToUpper().IndexOf("PENDING") >= 0)
                {
                    lblMSSStatus.ForeColor = Color.Red;
                    btnMSSStop.Visible = false;
                    btnMSSStart.Visible = false;
                }
                #endregion

                #region checking ftp & pacs synch service status
                objService.SERVICE_NAME = "VETRIS FTP & PACS Synch Service";
                lblFPStatus.Text = objService.CheckStatus();

                if (lblFPStatus.Text.ToUpper().IndexOf("RUN") >= 0)
                {
                    lblFPStatus.ForeColor = Color.Blue;
                    btnFPStop.Visible = true;
                    btnFPStart.Visible = false;
                    btnFPStop.Left = btnFPStart.Left;
                    btnFPStop.Top = btnFPStart.Top;
                }
                else if (lblFPStatus.Text.ToUpper().IndexOf("STOP") >= 0)
                {
                    lblFPStatus.ForeColor = Color.Red;
                    btnFPStop.Visible = false;
                    btnFPStart.Visible = true;
                }
                else if (lblFPStatus.Text.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    lblFPStatus.ForeColor = Color.Red;
                    btnFPStop.Visible = false;
                    btnFPStart.Visible = false;
                }
                else if (lblFPStatus.Text.ToUpper().IndexOf("PENDING") >= 0)
                {
                    lblFPStatus.ForeColor = Color.Red;
                    btnFPStop.Visible = false;
                    btnFPStart.Visible = false;
                }
                #endregion

                #region checking file distribution service status
                objService.SERVICE_NAME = "VETRIS File Distribution Service";
                lblFDStatus.Text = objService.CheckStatus();

                if (lblFDStatus.Text.ToUpper().IndexOf("RUN") >= 0)
                {
                    lblFDStatus.ForeColor = Color.Blue;
                    btnFDStop.Visible = true;
                    btnFDStart.Visible = false;
                    btnFDStop.Left = btnFDStart.Left;
                    btnFDStop.Top = btnFDStart.Top;
                }
                else if (lblFDStatus.Text.ToUpper().IndexOf("STOP") >= 0)
                {
                    lblFDStatus.ForeColor = Color.Red;
                    btnFDStop.Visible = false;
                    btnFDStart.Visible = true;
                }
                else if (lblFDStatus.Text.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    lblFDStatus.ForeColor = Color.Red;
                    btnFDStop.Visible = false;
                    btnFDStart.Visible = false;
                }
                else if (lblFDStatus.Text.ToUpper().IndexOf("PENDING") >= 0)
                {
                    lblFDStatus.ForeColor = Color.Red;
                    btnFDStop.Visible = false;
                    btnFDStart.Visible = false;
                }
                #endregion

                #region checking radiologist assignment synch service status
                objService.SERVICE_NAME = "VETRIS Case Assignment Service";
                lblRAStatus.Text = objService.CheckStatus();

                if (lblRAStatus.Text.ToUpper().IndexOf("RUN") >= 0)
                {
                    lblRAStatus.ForeColor = Color.Blue;
                    btnRAStop.Visible = true;
                    btnRAStart.Visible = false;
                    btnRAStop.Left = btnRAStart.Left;
                    btnRAStop.Top = btnRAStart.Top;
                }
                else if (lblRAStatus.Text.ToUpper().IndexOf("STOP") >= 0)
                {
                    lblRAStatus.ForeColor = Color.Red;
                    btnRAStop.Visible = false;
                    btnRAStart.Visible = true;
                }
                else if (lblRAStatus.Text.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    lblRAStatus.ForeColor = Color.Red;
                    btnRAStop.Visible = false;
                    btnRAStart.Visible = false;
                }
                else if (lblRAStatus.Text.ToUpper().IndexOf("PENDING") >= 0)
                {
                    lblRAStatus.ForeColor = Color.Red;
                    btnRAStop.Visible = false;
                    btnRAStart.Visible = false;
                }
                #endregion

                #region checking listener file processing service status
                objService.SERVICE_NAME = "VETRIS Listener File Processing Service";
                lblLFPStatus.Text = objService.CheckStatus();

                if (lblLFPStatus.Text.ToUpper().IndexOf("RUN") >= 0)
                {
                    lblLFPStatus.ForeColor = Color.Blue;
                    btnLFPStop.Visible = true;
                    btnLFPStart.Visible = false;
                    btnLFPStop.Left = btnLFPStart.Left;
                    btnLFPStop.Top = btnLFPStart.Top;
                }
                else if (lblLFPStatus.Text.ToUpper().IndexOf("STOP") >= 0)
                {
                    lblLFPStatus.ForeColor = Color.Red;
                    btnLFPStop.Visible = false;
                    btnLFPStart.Visible = true;
                }
                else if (lblLFPStatus.Text.ToUpper().IndexOf("INSTALL") >= 0)
                {
                    lblLFPStatus.ForeColor = Color.Red;
                    btnLFPStop.Visible = false;
                    btnLFPStart.Visible = false;
                }
                else if (lblLFPStatus.Text.ToUpper().IndexOf("PENDING") >= 0)
                {
                    lblLFPStatus.ForeColor = Color.Red;
                    btnLFPStop.Visible = false;
                    btnLFPStart.Visible = false;
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

        #region New Data Synch Service

        #region btnNDSStart_Click
        private void btnNDSStart_Click(object sender, EventArgs e)
        {
            lblNDSProcess.Visible = true;
            lblNDSProcess.Text = "Starting Service...Please Wait...";
            lblNDSProcess.Refresh();
            ServiceTypeId = (int)ServiceType.NDS;
            doProcess_Wait();
        } 
        #endregion

        #region btnNDSStop_Click
        private void btnNDSStop_Click(object sender, EventArgs e)
        {
            lblNDSProcess.Visible = true;
            lblNDSProcess.Text = "Stopping Service...Please Wait...";
            lblNDSProcess.Refresh();
            ServiceTypeId = (int)ServiceType.NDS;
            doProcess_Wait();
        } 
        #endregion

        #endregion

        #region Write Back Service

        #region btnWBStart_Click
        private void btnWBStart_Click(object sender, EventArgs e)
        {
            lblWBProcess.Visible = true;
            lblWBProcess.Text = "Starting Service...Please Wait...";
            lblWBProcess.Refresh();
            ServiceTypeId = (int)ServiceType.WB;
            doProcess_Wait();
        } 
        #endregion

        #region btnWBStop_Click
        private void btnWBStop_Click(object sender, EventArgs e)
        {
            lblWBProcess.Visible = true;
            lblWBProcess.Text = "Stopping Service...Please Wait...";
            lblWBProcess.Refresh();
            ServiceTypeId = (int)ServiceType.WB;
            doProcess_Wait();
        } 
        #endregion

        #endregion

        #region Status Update Service

        #region btnSUStart_Click
        private void btnSUStart_Click(object sender, EventArgs e)
        {
            lblSUProcess.Visible = true;
            lblSUProcess.Text = "Starting Service...Please Wait...";
            lblSUProcess.Refresh();
            ServiceTypeId = (int)ServiceType.SU;
            doProcess_Wait();
        } 
        #endregion

        #region btnSUStop_Click
        private void btnSUStop_Click(object sender, EventArgs e)
        {
            lblSUProcess.Visible = true;
            lblSUProcess.Text = "Stopping Service...Please Wait...";
            lblSUProcess.Refresh();
            ServiceTypeId = (int)ServiceType.SU;
            doProcess_Wait();
        } 
        #endregion

        #endregion

        #region Notification Service

        #region btnNSStart_Click
        private void btnNSStart_Click(object sender, EventArgs e)
        {
            lblNSProcess.Visible = true;
            lblNSProcess.Text = "Starting Service...Please Wait...";
            lblNSProcess.Refresh();
            ServiceTypeId = (int)ServiceType.NS;
            doProcess_Wait();
        } 
        #endregion

        #region btnNSStop_Click
        private void btnNSStop_Click(object sender, EventArgs e)
        {
            lblNSProcess.Visible = true;
            lblNSProcess.Text = "Stopping Service...Please Wait...";
            lblNSProcess.Refresh();
            ServiceTypeId = (int)ServiceType.NS;
            doProcess_Wait();
        } 
        #endregion

        #endregion

        #region Day end service

        #region btnDEStart_Click
        private void btnDEStart_Click(object sender, EventArgs e)
        {
            lblDEProcess.Visible = true;
            lblDEProcess.Text = "Starting Service...Please Wait...";
            lblDEProcess.Refresh();
            ServiceTypeId = (int)ServiceType.DE;
            doProcess_Wait();
        }
        
        #endregion

        #region btnDEStop_Click
        private void btnDEStop_Click(object sender, EventArgs e)
        {
            lblDEProcess.Visible = true;
            lblDEProcess.Text = "Stopping Service...Please Wait...";
            lblDEProcess.Refresh();
            ServiceTypeId = (int)ServiceType.DE;
            doProcess_Wait();
        } 
        #endregion

        #endregion

        #region Missing Study Service

        #region btnMSSStart_Click
        private void btnMSSStart_Click(object sender, EventArgs e)
        {
            lblMSSProcess.Visible = true;
            lblMSSProcess.Text = "Starting Service...Please Wait...";
            lblMSSProcess.Refresh();
            ServiceTypeId = (int)ServiceType.MSS;
            doProcess_Wait();
        }
        #endregion

        #region btnMSSStop_Click
        private void btnMSSStop_Click(object sender, EventArgs e)
        {
            lblMSSProcess.Visible = true;
            lblMSSProcess.Text = "Stopping Service...Please Wait...";
            lblMSSProcess.Refresh();
            ServiceTypeId = (int)ServiceType.MSS;
            doProcess_Wait();
        }
        #endregion

        #endregion

        #region FTP & PACS synch service

        #region btnFPStart_Click
        private void btnFPStart_Click(object sender, EventArgs e)
        {
            lblFPProcess.Visible = true;
            lblFPProcess.Text = "Starting Service...Please Wait...";
            lblFPProcess.Refresh();
            ServiceTypeId = (int)ServiceType.FP;
            doProcess_Wait();
        } 
        #endregion

        #region btnFPStop_Click
        private void btnFPStop_Click(object sender, EventArgs e)
        {
            lblFPProcess.Visible = true;
            lblFPProcess.Text = "Stopping Service...Please Wait...";
            lblFPProcess.Refresh();
            ServiceTypeId = (int)ServiceType.FP;
            doProcess_Wait();
        } 
        #endregion

        #endregion

        #region File Distribution service

        #region btnFDStart_Click
        private void btnFDStart_Click(object sender, EventArgs e)
        {
            lblFDProcess.Visible = true;
            lblFDProcess.Text = "Starting Service...Please Wait...";
            lblFDProcess.Refresh();
            ServiceTypeId = (int)ServiceType.FD;
            doProcess_Wait();
        }
        #endregion

        #region btnFDStop_Click
        private void btnFDStop_Click(object sender, EventArgs e)
        {
            lblFDProcess.Visible = true;
            lblFDProcess.Text = "Stopping Service...Please Wait...";
            lblFDProcess.Refresh();
            ServiceTypeId = (int)ServiceType.FD;
            doProcess_Wait();
        }
        #endregion

        #endregion

        #region Listener File Processing service

        #region btnLFPStart_Click
        private void btnLFPStart_Click(object sender, EventArgs e)
        {
            lblLFPProcess.Visible = true;
            lblLFPProcess.Text = "Starting Service...Please Wait...";
            lblLFPProcess.Refresh();
            ServiceTypeId = (int)ServiceType.FD;
            doProcess_Wait();
        }
        #endregion

        #region btnLFPStop_Click
        private void btnLFPStop_Click(object sender, EventArgs e)
        {
            lblLFPProcess.Visible = true;
            lblLFPProcess.Text = "Stopping Service...Please Wait...";
            lblLFPProcess.Refresh();
            ServiceTypeId = (int)ServiceType.LFP;
            doProcess_Wait();
        }
        #endregion

        #endregion

        #region Radiologist Assignment service

        #region btnRAStart_Click
        private void btnRAStart_Click(object sender, EventArgs e)
        {
            lblRAProcess.Visible = true;
            lblRAProcess.Text = "Starting Service...Please Wait...";
            lblRAProcess.Refresh();
            ServiceTypeId = (int)ServiceType.RA;
            doProcess_Wait();
        } 
        #endregion

        #region btnRAStop_Click
        private void btnRAStop_Click(object sender, EventArgs e)
        {
            lblRAProcess.Visible = true;
            lblRAProcess.Text = "Stopping Service...Please Wait...";
            lblRAProcess.Refresh();
            ServiceTypeId = (int)ServiceType.RA;
            doProcess_Wait();
        } 
        #endregion

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
            objService = new Services();
            try
            {
                switch (ServiceTypeId)
                {
                    case 1:
                        objService.SERVICE_NAME = "VETRIS New Data Synch Service";
                        if (btnNDSStart.Visible)
                        {
                            if (objService.Start())
                            {
                                lblNDSStatus.ForeColor = Color.Blue;
                                lblNDSStatus.Text = "(Running...)";
                                lblNDSStatus.Refresh();
                                btnNDSStop.Visible = true;
                                btnNDSStart.Visible = false;
                                btnNDSStop.Left = btnNDSStart.Left;
                                btnNDSStop.Top = btnNDSStart.Top;
                            }
                        }
                        else
                        {
                            if (objService.Stop())
                            {
                                lblNDSStatus.ForeColor = Color.Red;
                                lblNDSStatus.Text = "(Stopped...)";
                                lblNDSStatus.Refresh();
                                btnNDSStop.Visible = false;
                                btnNDSStart.Visible = true;
                            }
                        }
                        lblNDSProcess.Visible = false;
                        break;
                    case 2:
                        objService.SERVICE_NAME = "VETRIS Data Write Back Service";
                        if (btnWBStart.Visible)
                        {
                            if (objService.Start())
                            {
                                lblWBStatus.ForeColor = Color.Blue;
                                lblWBStatus.Text = "(Running...)";
                                lblWBStatus.Refresh();
                                btnWBStop.Visible = true;
                                btnWBStart.Visible = false;
                                btnWBStop.Left = btnWBStart.Left;
                                btnWBStop.Top = btnWBStart.Top;
                            }
                        }
                        else
                        {
                            if (objService.Stop())
                            {
                                lblWBStatus.ForeColor = Color.Red;
                                lblWBStatus.Text = "(Stopped...)";
                                lblWBStatus.Refresh();
                                btnWBStop.Visible = false;
                                btnWBStart.Visible = true;
                            }
                        }
                        lblWBProcess.Visible = false;
                        break;
                    case 3:
                        objService.SERVICE_NAME = "VETRIS Status Synch Service";
                        if (btnSUStart.Visible)
                        {
                            if (objService.Start())
                            {
                                lblSUStatus.ForeColor = Color.Blue;
                                lblSUStatus.Text = "(Running...)";
                                lblSUStatus.Refresh();
                                btnSUStop.Visible = true;
                                btnSUStart.Visible = false;
                                btnSUStop.Left = btnSUStart.Left;
                                btnSUStop.Top = btnSUStart.Top;
                            }
                        }
                        else
                        {
                            if (objService.Stop())
                            {
                                lblSUStatus.ForeColor = Color.Red;
                                lblSUStatus.Text = "(Stopped...)";
                                lblSUStatus.Refresh();
                                btnSUStop.Visible = false;
                                btnSUStart.Visible = true;
                            }
                        }
                        lblSUProcess.Visible = false;
                        break;
                    case 4:
                        objService.SERVICE_NAME = "VETRIS Notification Service";
                        if (btnNSStart.Visible)
                        {
                            if (objService.Start())
                            {
                                lblNSStatus.ForeColor = Color.Blue;
                                lblNSStatus.Text = "(Running...)";
                                lblNSStatus.Refresh();
                                btnNSStop.Visible = true;
                                btnNSStart.Visible = false;
                                btnNSStop.Left = btnNSStart.Left;
                                btnNSStop.Top = btnNSStart.Top;
                            }
                        }
                        else
                        {
                            if (objService.Stop())
                            {
                                lblNSStatus.ForeColor = Color.Red;
                                lblNSStatus.Text = "(Stopped...)";
                                lblNSStatus.Refresh();
                                btnNSStop.Visible = false;
                                btnNSStart.Visible = true;
                            }
                        }
                        lblNSProcess.Visible = false;
                        break;
                    case 5:
                        objService.SERVICE_NAME = "VETRIS Day End Service";
                        if (btnDEStart.Visible)
                        {
                            if (objService.Start())
                            {
                                lblDEStatus.ForeColor = Color.Blue;
                                lblDEStatus.Text = "(Running...)";
                                lblDEStatus.Refresh();
                                btnDEStop.Visible = true;
                                btnDEStart.Visible = false;
                                btnDEStop.Left = btnDEStart.Left;
                                btnDEStop.Top = btnDEStart.Top;
                            }
                        }
                        else
                        {
                            if (objService.Stop())
                            {
                                lblDEStatus.ForeColor = Color.Red;
                                lblDEStatus.Text = "(Stopped...)";
                                lblDEStatus.Refresh();
                                btnDEStop.Visible = false;
                                btnDEStart.Visible = true;
                            }
                        }
                        lblDEProcess.Visible = false;
                        break;
                    case 6:
                        objService.SERVICE_NAME = "VETRIS Missing Data Synch Service";
                        if (btnMSSStart.Visible)
                        {
                            if (objService.Start())
                            {
                                lblMSSStatus.ForeColor = Color.Blue;
                                lblMSSStatus.Text = "(Running...)";
                                lblMSSStatus.Refresh();
                                btnMSSStop.Visible = true;
                                btnMSSStart.Visible = false;
                                btnMSSStop.Left = btnMSSStart.Left;
                                btnMSSStop.Top = btnMSSStart.Top;
                            }
                        }
                        else
                        {
                            if (objService.Stop())
                            {
                                lblMSSStatus.ForeColor = Color.Red;
                                lblMSSStatus.Text = "(Stopped...)";
                                lblMSSStatus.Refresh();
                                btnMSSStop.Visible = false;
                                btnMSSStart.Visible = true;
                            }
                        }
                        lblMSSProcess.Visible = false;
                        break;
                    case 7:
                        objService.SERVICE_NAME = "VETRIS FTP & PACS Synch Service";
                        if (btnFPStart.Visible)
                        {
                            if (objService.Start())
                            {
                                lblFPStatus.ForeColor = Color.Blue;
                                lblFPStatus.Text = "(Running...)";
                                lblFPStatus.Refresh();
                                btnFPStop.Visible = true;
                                btnFPStart.Visible = false;
                                btnFPStop.Left = btnFPStart.Left;
                                btnFPStop.Top = btnFPStart.Top;
                            }
                        }
                        else
                        {
                            if (objService.Stop())
                            {
                                lblFPStatus.ForeColor = Color.Red;
                                lblFPStatus.Text = "(Stopped...)";
                                lblFPStatus.Refresh();
                                btnFPStop.Visible = false;
                                btnFPStart.Visible = true;
                            }
                        }
                        lblFPProcess.Visible = false;
                        break;
                    case 9:
                        objService.SERVICE_NAME = "VETRIS Case Assignment Service";
                        if (btnRAStart.Visible)
                        {
                            if (objService.Start())
                            {
                                lblRAStatus.ForeColor = Color.Blue;
                                lblRAStatus.Text = "(Running...)";
                                lblRAStatus.Refresh();
                                btnRAStop.Visible = true;
                                btnRAStart.Visible = false;
                                btnRAStop.Left = btnRAStart.Left;
                                btnRAStop.Top = btnRAStart.Top;
                            }
                        }
                        else
                        {
                            if (objService.Stop())
                            {
                                lblRAStatus.ForeColor = Color.Red;
                                lblRAStatus.Text = "(Stopped...)";
                                lblRAStatus.Refresh();
                                btnRAStop.Visible = false;
                                btnRAStart.Visible = true;
                            }
                        }
                        lblRAProcess.Visible = false;
                        break;
                    case 10:
                        objService.SERVICE_NAME = "VETRIS File Distribution Service";
                        if (btnFDStart.Visible)
                        {
                            if (objService.Start())
                            {
                                lblFDStatus.ForeColor = Color.Blue;
                                lblFDStatus.Text = "(Running...)";
                                lblFDStatus.Refresh();
                                btnFDStop.Visible = true;
                                btnFDStart.Visible = false;
                                btnFDStop.Left = btnFDStart.Left;
                                btnFDStop.Top = btnFDStart.Top;
                            }
                        }
                        else
                        {
                            if (objService.Stop())
                            {
                                lblFDStatus.ForeColor = Color.Red;
                                lblFDStatus.Text = "(Stopped...)";
                                lblFDStatus.Refresh();
                                btnFDStop.Visible = false;
                                btnFDStart.Visible = true;
                            }
                        }
                        lblFDProcess.Visible = false;
                        break;
                    case 11:
                        objService.SERVICE_NAME = "VETRIS Listener File Processing Service";
                        if (btnLFPStart.Visible)
                        {
                            if (objService.Start())
                            {
                                lblLFPStatus.ForeColor = Color.Blue;
                                lblLFPStatus.Text = "(Running...)";
                                lblLFPStatus.Refresh();
                                btnLFPStop.Visible = true;
                                btnLFPStart.Visible = false;
                                btnLFPStop.Left = btnLFPStart.Left;
                                btnLFPStop.Top = btnLFPStart.Top;
                            }
                        }
                        else
                        {
                            if (objService.Stop())
                            {
                                lblLFPStatus.ForeColor = Color.Red;
                                lblLFPStatus.Text = "(Stopped...)";
                                lblLFPStatus.Refresh();
                                btnLFPStop.Visible = false;
                                btnLFPStart.Visible = true;
                            }
                        }
                        lblLFPProcess.Visible = false;
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
            ApplicationDelegateEventArgs aPDSs = new ApplicationDelegateEventArgs(_Stat);
            IdentityUpdated(this, aPDSs);
        }
        #endregion

       
    }
}
