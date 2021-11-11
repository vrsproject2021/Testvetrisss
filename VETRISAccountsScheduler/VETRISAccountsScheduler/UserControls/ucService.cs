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
using System.Threading;
using VETRISAccountsScheduler.Core;
using QBVetrisLib;
using GemBox.Spreadsheet;
using System.IO;

namespace VETRISAccountsScheduler.UserControls
{
    public partial class ucService : UserControl
    {
        #region Members & Variables
        private static string strWinHdr = System.Configuration.ConfigurationManager.AppSettings["WinHdr"];
        public delegate void IdentityUpdateHandler(object sender, ApplicationDelegateEventArgs e);
        public event IdentityUpdateHandler IdentityUpdated;
        //public delegate string WaitDelegate(string name);

        int intServiceID = 8;
        string strSvcName = "VETRIS Accounts Update Service";
        string strConfigPath = Application.StartupPath;
        int intFreq = 180;

        Thread threadBAUpdate;
        //Thread threadAP;
        #endregion

        public ucService()
        {
            InitializeComponent();
        }

        #region ucServices_Load
        private void ucService_Load(object sender, EventArgs e)
        {

            GetServiceDetails();
        }
        #endregion

        #region GetServiceDetails
        private void GetServiceDetails()
        {
            string strCatchMessage = string.Empty;
            Scheduler objCore = new Scheduler();

            try
            {

                objCore.SERVICE_ID = intServiceID;

                try
                {

                    if (objCore.GetServiceDetails(strConfigPath, ref strCatchMessage))
                    {

                        intFreq = objCore.FREQUENCY;
                        strSvcName = objCore.SERVICE_NAME;
                        timerBAUpdate.Interval = intFreq * 1000;
                    }
                    else
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Core::GetServiceDetails - Error : " + strCatchMessage, true);

                }
                catch (Exception ex)
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetServiceDetails() - Error: " + ex.Message, true);

                }



            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetServiceDetails() - Exception: " + expErr.Message, true);
            }
            finally
            { objCore = null; }

        }
        #endregion

        #region Accounts Receivable Service

        #region btnASStart_Click
        private void btnASStart_Click(object sender, EventArgs e)
        {
            lblASProcess.Visible = true;
            lblASProcess.Text = "Starting application...Please Wait...";
            lblASProcess.Refresh();
            //doProcess_Wait();
            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName + "(AR)", "Starting application...", false);
            timerBAUpdate.Start();
        }
        #endregion

        #region btnASStop_Click
        private void btnASStop_Click(object sender, EventArgs e)
        {
            Label.CheckForIllegalCrossThreadCalls = false;
            Button.CheckForIllegalCrossThreadCalls = false;
            lblASProcess.Visible = true;
            lblASProcess.Text = "Stopping application...Please Wait...";
            lblASProcess.Refresh();
            //doProcess_Wait();
            timerBAUpdate.Stop();
            lblASStatus.ForeColor = Color.Red;
            lblASStatus.Text = "(Stopped...)";
            lblASStatus.Refresh();
            btnASStop.Visible = false;
            btnASStart.Visible = true;
            lblASProcess.Visible = false;
            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName + "(AR)", "Application stopped...", false);
        }
        #endregion

        #region timerBAUpdate_Tick
        private void timerBAUpdate_Tick(object sender, EventArgs e)
        {
            Label.CheckForIllegalCrossThreadCalls = false;
            Button.CheckForIllegalCrossThreadCalls = false;
            lblASStatus.ForeColor = Color.Blue;
            lblASStatus.Text = "(Running...)";
            lblASStatus.Refresh();
            btnASStop.Visible = true;
            btnASStart.Visible = false;
            btnASStop.Left = btnASStart.Left;
            btnASStop.Top = btnASStart.Top;
            lblASProcess.Visible = false;
            threadBAUpdate = new Thread(doProcess);
            threadBAUpdate.Start();

        }
        #endregion

        #region doProcess
        private void doProcess()
        {
            string strStatus = string.Empty;
            DateTime dtDE = DateTime.Today.AddDays(-1);
            strStatus = CheckDayEndStatus(dtDE);
            if (strStatus == "Y")
            {
                UpdateBillingAccounts();
            }
            else
            {
                if (strStatus == "N") GetDayEndReportData(dtDE); ;
            }
            

            //GetDayEndReportData(new DateTime(2020,12,9));
            //GenerateDayEndReport(new DateTime(2020, 12, 9));
        }
        #endregion

        #region UpdateBillingAccounts
        private void UpdateBillingAccounts()
        {

            //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Started Updating Billing Accounts", false);
            #region Members & Variables
            string strCatchMsg = string.Empty;

            DataSet ds = new DataSet();
            Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
            string strCode = string.Empty;
            string strName = string.Empty;
            string strQBName = string.Empty;
            string strAddress1 = string.Empty;
            string strAddress2 = string.Empty;
            string strCity = string.Empty;
            string strZip = string.Empty;
            string strStateName = string.Empty;
            string strCountryName = string.Empty;
            string strEmailID = string.Empty;
            string strPhoneNo = string.Empty;
            bool bIsActive = false;
            string strListID = string.Empty;
            string strCatchMessage = string.Empty;
            QBDriver driver = new QBDriver();
            bool bRet = false;
            bool bUpdate = false;
            AccountUpdate objAU = new AccountUpdate();
            #endregion


            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Fetching billing accounts to update...", false);
                if (objAU.FetchBillingAccountUpdateList(strConfigPath, ref ds, ref strCatchMsg))
                {
                    if (ds.Tables["BillingAccounts"].Rows.Count > 0)
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ds.Tables["BillingAccounts"].Rows.Count.ToString() + " record(s) fetched.", false);


                    foreach (DataRow dr in ds.Tables["BillingAccounts"].Rows)
                    {
                        #region Populate Variables
                        Id = new Guid(Convert.ToString(dr["id"]));
                        strCode = Convert.ToString(dr["code"]).Trim();
                        strName = Convert.ToString(dr["name"]);
                        strQBName = Convert.ToString(dr["qb_name"]);
                        strAddress1 = Convert.ToString(dr["address_1"]).Trim();
                        strAddress2 = Convert.ToString(dr["address_2"]).Trim();
                        strCity = Convert.ToString(dr["city"]).Trim();
                        strZip = Convert.ToString(dr["zip"]).Trim();
                        strStateName = Convert.ToString(dr["state_name"]).Trim();
                        strCountryName = Convert.ToString(dr["country_name"]).Trim();
                        strEmailID = Convert.ToString(dr["email_id"]).Trim();
                        strPhoneNo = Convert.ToString(dr["phone_no"]).Trim();
                        if (Convert.ToString(dr["is_active"]) == "Y") bIsActive = true;
                        else if (Convert.ToString(dr["is_active"]) == "N") bIsActive = false;
                        strListID = Convert.ToString(dr["debtor_id"]).Trim();
                        #endregion

                        #region Populate Customer Enitity
                        var customer = new CustomerEntity
                        {
                            AccountNumber = strCode,
                            Name = strName,
                            FullName = strName,
                            CompanyName = strName,
                            ListID = strListID,
                            Phone = strPhoneNo,
                            ExternalGUID = Id.ToString(),
                            Email = strEmailID,
                            IsActive = bIsActive,
                            BillAddress = new Address
                            {
                                Address1 = strAddress1,
                                Address2 = strAddress2,
                                City = strCity,
                                PostalCode = strZip,
                                Country = strCountryName,
                                State = strStateName
                            }
                        };
                        #endregion

                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Updating billing account " + strName, false);

                        if (!driver.IsBillingAccountExists(strQBName, ref strCatchMessage))
                        {
                            #region Create Billing Account in Quick Books
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Creating billing account " + strName, false);
                            customer = driver.CreateBillingAccount(customer, ref strCatchMessage);
                            if (customer != null)
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Successfully created billing account " + strName, false);
                                strListID = customer.ListID.Trim();
                                bUpdate = true;
                            }
                            else
                            {
                                bUpdate = false;
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Quick books error to create billing account " + strName + " :: Error - " + strCatchMessage.Trim(), true);
                            }
                            #endregion
                        }
                        else
                        {
                            #region Edit Billing Account In Quick Books
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Editing billing account " + strName, false);
                            var custDtls = driver.getBillingAccounts(ref strCatchMessage, strQBName).FirstOrDefault();
                            if (custDtls != null)
                            {
                                if (customer.ListID.Trim() == string.Empty)
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Getting List ID Of billing account " + strName, false);
                                    strListID = custDtls.ListID.Trim();
                                    strQBName = custDtls.Name;
                                    bUpdate = true;
                                }
                                else if (customer.ListID == custDtls.ListID)
                                {
                                    customer.EditSequence = custDtls.EditSequence;
                                    strQBName = custDtls.Name;
                                    bRet = driver.UpdateBillingAccount(customer, ref strCatchMessage);
                                    if (bRet)
                                    {
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Successfully edited billing account " + strName, false);
                                        strListID = custDtls.ListID.Trim();
                                    }
                                    else
                                    {
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Quick books error to update billing account " + strName + " :: Error - " + strCatchMessage.Trim(), true);
                                    }
                                    bUpdate = bRet;
                                }
                            }
                            else
                            {
                                bUpdate = false;
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Billing account " + strName + " could not be found in Quick Books :: Error - " + strCatchMessage.Trim(), true);
                            }
                            #endregion

                        }

                        #region Update VETRIS DB
                        if (bUpdate)
                        {
                            objAU.BILLING_ACCOUNT_ID = Id;
                            objAU.QB_NAME = strQBName.Trim();
                            objAU.DEBTOR_ID = strListID;
                            strCatchMessage = "";

                            if (!objAU.UpdateBillingAccount(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                            {
                                if (strCatchMessage.Trim() != string.Empty)
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateBillingAccounts()==>UpdateBillingAccount()- Core::Exception - " + strCatchMessage, false);
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateBillingAccounts()==>FetchBillingAccountUpdateList():Core-Exception: " + strCatchMsg, true);
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "UpdateBillingAccounts() :: " + strName + " - Exception: " + ex.Message, true);
            }
            finally
            {
                objAU = null; ds.Dispose(); driver.Dispose(); driver = null;
            }
            PostVouchers();
        }
        #endregion

        #region PostVouchers
        private void PostVouchers()
        {

            //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Started Updating Billing Accounts", false);
            #region Members & Variables
            DataSet ds = new DataSet();
            string strCatchMessage = string.Empty;
            AccountUpdate objAU = new AccountUpdate();
            #endregion


            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Fetching Voucher Data...", false);
                if (objAU.FetchListForVoucher(strConfigPath, ref ds, ref strCatchMessage))
                {
                    if (ds.Tables["Invoice"].Rows.Count > 0)
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ds.Tables["Invoice"].Rows.Count.ToString() + " record(s) for invoice posting fetched.", false);
                        PostInvoiceVouchers(ds.Tables["Invoice"]);
                    }
                    if (ds.Tables["InvoiceReverse"].Rows.Count > 0)
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ds.Tables["InvoiceReverse"].Rows.Count.ToString() + " record(s) for invoice reverse posting fetched.", false);
                        PostInvoiceReverseVouchers(ds.Tables["InvoiceReverse"]);
                    }
                    if (ds.Tables["Payments"].Rows.Count > 0)
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ds.Tables["Payments"].Rows.Count.ToString() + " record(s) for payment posting fetched.", false);
                        PostPaymentVouchers(ds.Tables["Payments"]);
                    }
                    if (ds.Tables["Refunds"].Rows.Count > 0)
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, ds.Tables["Refunds"].Rows.Count.ToString() + " record(s) for refund posting fetched.", false);
                        PostRefundVouchers(ds.Tables["Refunds"]);
                    }
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "PostVouchers()==>FetchListForVoucher():Core-Exception: " + strCatchMessage, true);
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "PostVouchers() - Exception: " + ex.Message, true);
            }
            finally
            {
                objAU = null; ds.Dispose();
            }

        }
        #endregion

        #region PostInvoiceVouchers
        private void PostInvoiceVouchers(DataTable dtbl)
        {
            #region Members & Variables
            Guid BillAcctId = new Guid("00000000-0000-0000-0000-000000000000");
            Guid CycleId = new Guid("00000000-0000-0000-0000-000000000000");
            string strBillAcctName = string.Empty;
            string strCycleName = string.Empty;
            string strInvoiceNo = string.Empty;
            string strInvoiceSrl = string.Empty;
            DateTime dtInv = DateTime.Today;
            string strNarrationHdr = string.Empty;
            string strCatchMessage = string.Empty;
            string strQBError = string.Empty;
            QBDriver driver = new QBDriver();
            bool bUpdate = false;
            AccountUpdate objAUVch = new AccountUpdate();
            DataSet ds = new DataSet();
            //DataTable dtblPost = new DataTable();
            //int intSrl = 0;
            #endregion

            try
            {
                foreach (DataRow dr in dtbl.Rows)
                {
                    #region Populate Variables
                    BillAcctId = new Guid(Convert.ToString(dr["billing_account_id"]));
                    CycleId = new Guid(Convert.ToString(dr["billing_cycle_id"]));
                    strBillAcctName = Convert.ToString(dr["billing_account_name"]).Trim();
                    strCycleName = Convert.ToString(dr["billing_cycle_name"]);
                    #endregion

                    #region Fetch Voucher Details
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Fetching invoice voucher details of billing account :" + strBillAcctName + " of billing cycle " + strCycleName, false);
                    objAUVch.BILLING_ACCOUNT_ID = BillAcctId;
                    objAUVch.BILLING_CYCLE_ID = CycleId;

                    if (!objAUVch.FetchInvoiceVoucher(strConfigPath, intServiceID, strSvcName, ref ds, ref strCatchMessage))
                    {
                        if (strCatchMessage.Trim() != string.Empty)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "PostVouchers()==>PostInvoiceVouchers()==>Core:FetchInvoiceVoucher()::Exception - " + strCatchMessage, false);
                        }
                    }
                    else
                    {
                        #region Create Voucher
                        strQBError = string.Empty;
                        foreach (DataRow drHdr in ds.Tables["VoucherHdr"].Rows)
                        {
                            strBillAcctName = Convert.ToString(drHdr["billing_account_name"]).Trim();
                            strCycleName = Convert.ToString(drHdr["billing_cycle_name"]).Trim();
                            strInvoiceNo = Convert.ToString(drHdr["invoice_no"]).Trim();
                            strInvoiceSrl = Convert.ToString(drHdr["invoice_srl_no"]).Trim();
                            dtInv = Convert.ToDateTime(drHdr["invoice_date"]);
                            strNarrationHdr = Convert.ToString(drHdr["narration_hdr"]).Trim();
                        }
                        var voucher = new JournalEntity();

                        voucher.TxnDate = dtInv;
                        voucher.RefNumber = strInvoiceSrl;
                        voucher.Remarks = strNarrationHdr;


                        voucher.Lines = new List<JournalDetailEntity>();
                        //dtblPost = CreatePostingTable();

                        foreach (DataRow drDtls in ds.Tables["VoucherDtls"].Rows)
                        {
                            var vchLine = new JournalDetailEntity
                            {
                                AccountNumber = Convert.ToString(drDtls["gl_code"]).Trim(),
                                DebitAmount = Math.Round(Convert.ToDouble(drDtls["amount_dr"]), 2),
                                CreditAmount = Math.Round(Convert.ToDouble(drDtls["amount_cr"]), 2),
                                CustomerFullName = Convert.ToString(drDtls["billing_account_name"]),
                                Remarks = Convert.ToString(drDtls["narration"]).Trim()
                            };
                            voucher.Lines.Add(vchLine);

                            //DataRow drPost = dtblPost.NewRow();
                            //intSrl = intSrl + 1;
                            //drPost["srl_no"] = intSrl;
                            //drPost["gl_code"] = Convert.ToString(drDtls["gl_code"]).Trim();
                            //drPost["dr_amount"] = Math.Round(Convert.ToDouble(drDtls["amount_dr"]), 2);
                            //drPost["cr_amount"] = Math.Round(Convert.ToDouble(drDtls["amount_cr"]), 2);
                            //dtblPost.Rows.Add(drPost);
                        }
                        #endregion

                        //Post Voucher to QB
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Posting invoice voucher details of billing account :" + strBillAcctName + " of billing cycle " + strCycleName, false);
                        try
                        {
                            bUpdate = driver.CreateJournal(voucher, ref strQBError);
                        }
                        catch (Exception ex)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "PostVouchers()==>CreateJournal() - Exception: " + ex.Message, true);
                            bUpdate = false;
                        }


                        #region Update VETRIS database
                        objAUVch.BILLING_ACCOUNT_ID = BillAcctId;
                        objAUVch.BILLING_CYCLE_ID = CycleId;
                        objAUVch.POSTING_TYPE = "Y";
                        strCatchMessage = string.Empty;

                        if (bUpdate)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Sucessfully posted invoice voucher details of billing account :" + strBillAcctName + " of billing cycle " + strCycleName + " to Quick Books. Updating VETRIS", false);
                            objAUVch.IS_SUCCESS = "Y";
                            objAUVch.POSTING_ID = voucher.TxnID;
                        }
                        else
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Quick books error while posting invoice # " + strInvoiceNo + " dtd " + dtInv.ToString("MMM dd, yyyy") + " of billing account " + strBillAcctName + " for the billing Cycle " + strCycleName + " :: Error - " + strQBError.Trim(), true);
                            objAUVch.IS_SUCCESS = "N";
                            objAUVch.POSTING_ID = string.Empty;
                        }

                        if (!objAUVch.UpdateInvoicePosting(strConfigPath, intServiceID, strSvcName,ref strCatchMessage))
                        {
                            if (strCatchMessage.Trim() != string.Empty)
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "PostVouchers()==>PostInvoiceVouchers()==>Core:UpdateInvoicePosting()::Exception - " + strCatchMessage, false);
                        }

                        #endregion

                        voucher = null;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "PostVouchers()==>PostInvoiceVouchers() - Exception: " + ex.Message, true);
            }
            finally
            {
                objAUVch = null; driver.Dispose(); driver = null; ds.Dispose(); dtbl.Dispose(); dtbl = null; 
                //dtblPost.Dispose(); dtblPost = null;
            }


        }
        #endregion

        #region PostInvoiceReverseVouchers
        private void PostInvoiceReverseVouchers(DataTable dtbl)
        {
            #region Members & Variables
            Guid BillAcctId = new Guid("00000000-0000-0000-0000-000000000000");
            Guid CycleId = new Guid("00000000-0000-0000-0000-000000000000");
            string strBillAcctName = string.Empty;
            string strCycleName = string.Empty;
            string strInvoiceNo = string.Empty;
            string strInvoiceSrl = string.Empty;
            DateTime dtInv = DateTime.Today;
            string strNarrationHdr = string.Empty;
            string strCatchMessage = string.Empty;
            string strQBError = string.Empty;
            QBDriver driver = new QBDriver();
            bool bUpdate = false;
            AccountUpdate objAUVch = new AccountUpdate();
            DataSet ds = new DataSet();
            //DataTable dtblPost = new DataTable();
            //int intSrl = 0;
            #endregion

            try
            {
                foreach (DataRow dr in dtbl.Rows)
                {
                    #region Populate Variables
                    BillAcctId = new Guid(Convert.ToString(dr["billing_account_id"]));
                    CycleId = new Guid(Convert.ToString(dr["billing_cycle_id"]));
                    strBillAcctName = Convert.ToString(dr["billing_account_name"]).Trim();
                    strCycleName = Convert.ToString(dr["billing_cycle_name"]);
                    #endregion

                    #region Fetch Voucher Details
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Fetching reversed invoice voucher details of billing account :" + strBillAcctName + " of billing cycle " + strCycleName, false);
                    objAUVch.BILLING_ACCOUNT_ID = BillAcctId;
                    objAUVch.BILLING_CYCLE_ID = CycleId;

                    if (!objAUVch.FetchReverseInvoiceVoucher(strConfigPath, intServiceID, strSvcName, ref ds, ref strCatchMessage))
                    {
                        if (strCatchMessage.Trim() != string.Empty)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "PostVouchers()==>PostInvoiceVouchers()==>Core:FetchReverseInvoiceVoucher()::Exception - " + strCatchMessage, false);
                        }
                    }
                    else
                    {
                        #region Create Voucher
                        strQBError = string.Empty;
                        foreach (DataRow drHdr in ds.Tables["VoucherHdr"].Rows)
                        {
                            strBillAcctName = Convert.ToString(drHdr["billing_account_name"]).Trim();
                            strCycleName = Convert.ToString(drHdr["billing_cycle_name"]).Trim();
                            strInvoiceNo = Convert.ToString(drHdr["invoice_no"]).Trim();
                            strInvoiceSrl = Convert.ToString(drHdr["invoice_srl_no"]).Trim();
                            dtInv = Convert.ToDateTime(drHdr["invoice_date"]);
                            strNarrationHdr = Convert.ToString(drHdr["narration_hdr"]).Trim();
                        }
                        var voucher = new JournalEntity();

                        voucher.TxnDate = dtInv;
                        voucher.RefNumber = strInvoiceSrl;
                        voucher.Remarks = strNarrationHdr;

                        voucher.Lines = new List<JournalDetailEntity>();
                        //dtblPost = CreatePostingTable();

                        foreach (DataRow drDtls in ds.Tables["VoucherDtls"].Rows)
                        {
                            var vchLine = new JournalDetailEntity
                            {
                                AccountNumber = Convert.ToString(drDtls["gl_code"]).Trim(),
                                DebitAmount = Convert.ToDouble(drDtls["amount_dr"]),
                                CreditAmount = Convert.ToDouble(drDtls["amount_cr"]),
                                CustomerFullName = Convert.ToString(drDtls["billing_account_name"]),
                                Remarks = Convert.ToString(drDtls["narration"]).Trim()
                            };
                            voucher.Lines.Add(vchLine);

                            //DataRow drPost = dtblPost.NewRow();
                            //intSrl = intSrl + 1;
                            //drPost["srl_no"] = intSrl;
                            //drPost["gl_code"] = Convert.ToString(drDtls["gl_code"]).Trim();
                            //drPost["dr_amount"] = Math.Round(Convert.ToDouble(drDtls["amount_dr"]), 2);
                            //drPost["cr_amount"] = Math.Round(Convert.ToDouble(drDtls["amount_cr"]), 2);
                            //dtblPost.Rows.Add(drPost);
                        }
                        #endregion

                        //Post Voucher to QB
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Posting invoice reversal voucher details of billing account :" + strBillAcctName + " of billing cycle " + strCycleName, false);
                        bUpdate = driver.CreateJournal(voucher, ref strQBError);

                        #region Update VETRIS database
                        objAUVch.BILLING_ACCOUNT_ID = BillAcctId;
                        objAUVch.BILLING_CYCLE_ID = CycleId;
                        objAUVch.POSTING_TYPE = "R";
                        strCatchMessage = string.Empty;

                        if (bUpdate)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Sucessfully posted invoice reversal voucher details of billing account :" + strBillAcctName + " of billing cycle " + strCycleName + " to Quick Books. Updating VETRIS...", false);
                            objAUVch.IS_SUCCESS = "Y";
                            objAUVch.POSTING_ID = voucher.TxnID;
                        }
                        else
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Quick books error while reverse posting invoice # " + strInvoiceNo + " dtd " + dtInv.ToString("MMM dd, yyyy") + " of billing account " + strBillAcctName + " for the billing Cycle " + strCycleName + " :: Error - " + strQBError.Trim(), true);
                            objAUVch.IS_SUCCESS = "N";
                            objAUVch.POSTING_ID = string.Empty;
                        }

                        if (!objAUVch.UpdateInvoicePosting(strConfigPath, intServiceID, strSvcName,ref strCatchMessage))
                        {
                            if (strCatchMessage.Trim() != string.Empty)
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "PostVouchers()==>PostInvoiceVouchers()==>Core:UpdateInvoicePosting()::Exception - " + strCatchMessage, false);
                        }
                        voucher = null;
                        #endregion
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "PostVouchers()==>PostInvoiceReverseVouchers() - Exception: " + ex.Message, true);
            }
            finally
            {
                objAUVch = null; driver.Dispose(); driver = null; ds.Dispose(); dtbl.Dispose(); dtbl = null;
                //dtblPost.Dispose(); dtblPost = null;
            }


        }
        #endregion

        #region PostPaymentVouchers
        private void PostPaymentVouchers(DataTable dtbl)
        {
            #region Members & Variables
            Guid BillAcctId = new Guid("00000000-0000-0000-0000-000000000000");
            Guid PaymentId = new Guid("00000000-0000-0000-0000-000000000000");
            string strBillAcctName = string.Empty;
            string strPayRefNo = string.Empty;
            string strPayRefSrl = string.Empty;
            DateTime dtPayRef = DateTime.Today;
            string strNarrationHdr = string.Empty;
            string strCatchMessage = string.Empty;
            string strQBError = string.Empty;
            QBDriver driver = new QBDriver();
            bool bUpdate = false;
            AccountUpdate objAUVch = new AccountUpdate();
            DataSet ds = new DataSet();
            //DataTable dtblPost = new DataTable();
            //int intSrl = 0;
            #endregion

            try
            {
                foreach (DataRow dr in dtbl.Rows)
                {
                    #region Populate Variables
                    BillAcctId = new Guid(Convert.ToString(dr["billing_account_id"]));
                    PaymentId = new Guid(Convert.ToString(dr["id"]));
                    strBillAcctName = Convert.ToString(dr["billing_account_name"]).Trim();
                    strPayRefNo = Convert.ToString(dr["payref_no"]);
                    dtPayRef = Convert.ToDateTime(dr["payref_date"]);
                    #endregion

                    #region Fetch Voucher Details
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Fetching payment voucher details of billing account :" + strBillAcctName + ", Payment Ref # " + strPayRefNo + " dtd " + dtPayRef.ToString("MMM dd, yyyy"), false);
                    objAUVch.BILLING_ACCOUNT_ID = BillAcctId;
                    objAUVch.PAYMENT_ID = PaymentId;

                    if (!objAUVch.FetchPaymentVoucher(strConfigPath, intServiceID, strSvcName, ref ds, ref strCatchMessage))
                    {
                        if (strCatchMessage.Trim() != string.Empty)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "PostVouchers()==>PostPaymentVouchers()==>Core:FetchPaymentVoucher()::Exception - " + strCatchMessage, false);
                        }
                    }
                    else
                    {
                        #region Create Voucher
                        strQBError = string.Empty;
                        foreach (DataRow drHdr in ds.Tables["VoucherHdr"].Rows)
                        {
                            strBillAcctName = Convert.ToString(drHdr["billing_account_name"]).Trim();
                            strPayRefNo = Convert.ToString(dr["payref_no"]);
                            strPayRefSrl = strPayRefNo.Substring(strPayRefNo.LastIndexOf("/") + 1, (strPayRefNo.Trim().Length - (strPayRefNo.Trim().LastIndexOf("/") + 1)));
                            dtPayRef = Convert.ToDateTime(dr["payref_date"]);
                            strNarrationHdr = Convert.ToString(drHdr["narration_hdr"]).Trim();
                        }
                        var voucher = new JournalEntity();

                        voucher.TxnDate = dtPayRef;
                        voucher.RefNumber = strPayRefSrl;
                        voucher.Remarks = strNarrationHdr;

                        voucher.Lines = new List<JournalDetailEntity>();
                        //dtblPost = CreatePostingTable();

                        foreach (DataRow drDtls in ds.Tables["VoucherDtls"].Rows)
                        {
                            var vchLine = new JournalDetailEntity
                            {
                                AccountNumber = Convert.ToString(drDtls["gl_code"]).Trim(),
                                DebitAmount = Convert.ToDouble(drDtls["amount_dr"]),
                                CreditAmount = Convert.ToDouble(drDtls["amount_cr"]),
                                CustomerFullName = Convert.ToString(drDtls["billing_account_name"]),
                                Remarks = Convert.ToString(drDtls["narration"]).Trim()
                            };

                            //DataRow drPost = dtblPost.NewRow();
                            //intSrl = intSrl + 1;
                            //drPost["srl_no"] = intSrl;
                            //drPost["gl_code"] = Convert.ToString(drDtls["gl_code"]).Trim();
                            //drPost["dr_amount"] = Math.Round(Convert.ToDouble(drDtls["amount_dr"]), 2);
                            //drPost["cr_amount"] = Math.Round(Convert.ToDouble(drDtls["amount_cr"]), 2);
                            //dtblPost.Rows.Add(drPost);

                            voucher.Lines.Add(vchLine);

                        }
                        #endregion

                        //Post Voucher to QB
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Posting payment voucher details of billing account :" + strBillAcctName + ", Payment Ref # " + strPayRefNo + " dtd " + dtPayRef.ToString("MMM dd, yyyy"), false);
                        bUpdate = driver.CreateJournal(voucher, ref strQBError);

                        #region Update VETRIS database
                        objAUVch.BILLING_ACCOUNT_ID = BillAcctId;
                        objAUVch.PAYMENT_ID = PaymentId;
                        strCatchMessage = string.Empty;

                        if (bUpdate)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Sucessfully posted payment voucher details of billing account :" + strBillAcctName + ", Payment Ref # " + strPayRefNo + " dtd " + dtPayRef.ToString("MMM dd, yyyy") + " to Quick Books. Updating VETRIS", false);
                            objAUVch.POSTING_ID = voucher.TxnID;
                            objAUVch.IS_SUCCESS = "Y";
                        }
                        else
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Quick books error while posting payment ref # " + strPayRefNo + " dtd " + dtPayRef.ToString("MMM dd, yyyy") + " of billing account " + strBillAcctName + " :: Error - " + strQBError.Trim(), true);
                            objAUVch.POSTING_ID = string.Empty;
                            objAUVch.IS_SUCCESS = "N";
                        }

                        if (!objAUVch.UpdatePaymentPosting(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                        {
                            if (strCatchMessage.Trim() != string.Empty)
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "PostVouchers()==>PostPaymentVouchers()==>Core:UpdatePaymentPosting()::Exception - " + strCatchMessage, false);
                        }
                        voucher = null;
                        #endregion
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "PostVouchers()==>PostPaymentVouchers() - Exception: " + ex.Message, true);
            }
            finally
            {
                objAUVch = null; driver.Dispose(); driver = null; ds.Dispose(); dtbl.Dispose(); dtbl = null;
                //dtblPost.Dispose() ; dtblPost = null;
            }


        }
        #endregion

        #region PostRefundVouchers
        private void PostRefundVouchers(DataTable dtbl)
        {
            #region Members & Variables
            Guid BillAcctId = new Guid("00000000-0000-0000-0000-000000000000");
            Guid RefundId = new Guid("00000000-0000-0000-0000-000000000000");
            string strBillAcctName = string.Empty;
            string strRefundRefNo = string.Empty;
            string strRefundRefSrl = string.Empty;
            DateTime dtPayRef = DateTime.Today;
            string strNarrationHdr = string.Empty;
            string strCatchMessage = string.Empty;
            string strQBError = string.Empty;
            QBDriver driver = new QBDriver();
            bool bUpdate = false;
            AccountUpdate objAUVch = new AccountUpdate();
            DataSet ds = new DataSet();
            //DataTable dtblPost = new DataTable();
            //int intSrl = 0;
            #endregion

            try
            {
                foreach (DataRow dr in dtbl.Rows)
                {
                    #region Populate Variables
                    BillAcctId = new Guid(Convert.ToString(dr["billing_account_id"]));
                    RefundId = new Guid(Convert.ToString(dr["id"]));
                    strBillAcctName = Convert.ToString(dr["billing_account_name"]).Trim();
                    strRefundRefNo = Convert.ToString(dr["refundref_no"]);
                    dtPayRef = Convert.ToDateTime(dr["refundref_date"]);
                    #endregion

                    #region Fetch Voucher Details
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Fetching refund voucher details of billing account :" + strBillAcctName + ", Refund Ref # " + strRefundRefNo + " dtd " + dtPayRef.ToString("MMM dd, yyyy"), false);
                    objAUVch.BILLING_ACCOUNT_ID = BillAcctId;
                    objAUVch.REFUND_ID = RefundId;

                    if (!objAUVch.FetchRefundVoucher(strConfigPath, intServiceID, strSvcName, ref ds, ref strCatchMessage))
                    {
                        if (strCatchMessage.Trim() != string.Empty)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "PostVouchers()==>PostRefundVouchers()==>Core:FetchRefundVoucher()::Exception - " + strCatchMessage, false);
                        }
                    }
                    else
                    {
                        #region Create Voucher
                        strQBError = string.Empty;
                        foreach (DataRow drHdr in ds.Tables["VoucherHdr"].Rows)
                        {
                            strBillAcctName = Convert.ToString(drHdr["billing_account_name"]).Trim();
                            strRefundRefNo = Convert.ToString(dr["refundref_no"]);
                            strRefundRefSrl = strRefundRefNo.Substring(strRefundRefNo.LastIndexOf("/") + 1, (strRefundRefNo.Trim().Length - (strRefundRefNo.Trim().LastIndexOf("/") + 1)));
                            dtPayRef = Convert.ToDateTime(dr["refundref_date"]);
                            strNarrationHdr = Convert.ToString(drHdr["narration_hdr"]).Trim();
                        }
                        var voucher = new JournalEntity();

                        voucher.TxnDate = dtPayRef;
                        voucher.RefNumber = strRefundRefSrl;
                        voucher.Remarks = strNarrationHdr;

                        voucher.Lines = new List<JournalDetailEntity>();
                        foreach (DataRow drDtls in ds.Tables["VoucherDtls"].Rows)
                        {
                            var vchLine = new JournalDetailEntity
                            {
                                AccountNumber = Convert.ToString(drDtls["gl_code"]).Trim(),
                                DebitAmount = Convert.ToDouble(drDtls["amount_dr"]),
                                CreditAmount = Convert.ToDouble(drDtls["amount_cr"]),
                                CustomerFullName = Convert.ToString(drDtls["billing_account_name"]),
                                Remarks = Convert.ToString(drDtls["narration"]).Trim()
                            };
                            voucher.Lines.Add(vchLine);

                            //DataRow drPost = dtblPost.NewRow();
                            //intSrl = intSrl + 1;
                            //drPost["srl_no"] = intSrl;
                            //drPost["gl_code"] = Convert.ToString(drDtls["gl_code"]).Trim();
                            //drPost["dr_amount"] = Math.Round(Convert.ToDouble(drDtls["amount_dr"]), 2);
                            //drPost["cr_amount"] = Math.Round(Convert.ToDouble(drDtls["amount_cr"]), 2);
                            //dtblPost.Rows.Add(drPost);
                        }
                        #endregion

                        //Post Voucher to QB
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Posting refund voucher details of billing account :" + strBillAcctName + ", Refund Ref # " + strRefundRefNo + " dtd " + dtPayRef.ToString("MMM dd, yyyy"), false);
                        bUpdate = driver.CreateJournal(voucher, ref strQBError);

                        #region Update VETRIS database
                        objAUVch.BILLING_ACCOUNT_ID = BillAcctId;
                        objAUVch.REFUND_ID = RefundId;
                        strCatchMessage = string.Empty;

                        if (bUpdate)
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Sucessfully posted refund voucher details of billing account :" + strBillAcctName + ", Refund Ref # " + strRefundRefNo + " dtd " + dtPayRef.ToString("MMM dd, yyyy") + " to Quick Books. Updating VETRIS", false);
                            objAUVch.POSTING_ID = voucher.TxnID;
                            objAUVch.IS_SUCCESS = "Y";
                        }
                        else
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Quick books error while posting Refund Ref # " + strRefundRefNo + " dtd " + dtPayRef.ToString("MMM dd, yyyy") + " of billing account " + strBillAcctName + " :: Error - " + strQBError.Trim(), true);
                            objAUVch.POSTING_ID = string.Empty;
                            objAUVch.IS_SUCCESS = "N";
                        }

                        if (!objAUVch.UpdateRefundPosting(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                        {
                            if (strCatchMessage.Trim() != string.Empty)
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "PostVouchers()==>PostRefundVouchers()==>Core:UpdateRefundPosting()::Exception - " + strCatchMessage, false);
                        }
                        voucher = null;
                        #endregion
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "PostVouchers()==>PostRefundVouchers() - Exception: " + ex.Message, true);
            }
            finally
            {
                objAUVch = null; driver.Dispose(); driver = null; ds.Dispose(); dtbl.Dispose(); dtbl = null;
                //dtblPost.Dispose() ; dtblPost = null;
            }


        }
        #endregion

        #region CreatePostingTable
        private DataTable CreatePostingTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("srl_no", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("gl_code", System.Type.GetType("System.String"));
            dtbl.Columns.Add("dr_amount", System.Type.GetType("System.Double"));
            dtbl.Columns.Add("cr_amount", System.Type.GetType("System.Double"));
            dtbl.TableName = "Posting";
            return dtbl;
        }
        #endregion

        #endregion

        #region Day End

        #region CheckDayEndStatus
        private string CheckDayEndStatus(DateTime dtDayEnd)
        {
            string strStatus = string.Empty;
            AccountUpdate objAU = new AccountUpdate();
            bool bRet = false;
            string strCatchMessage = string.Empty;

            try
            {
                objAU.DAY_END_DATE = dtDayEnd;
                bRet = objAU.CheckDayEndStatus(strConfigPath, intServiceID, strSvcName, ref strCatchMessage);
                if (!bRet)
                {
                    strStatus = "N";
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetDayEndReportData()==>Core:CheckDayEndStatus()- Exception : " + strCatchMessage, true);
                }
                else
                    strStatus = objAU.DAY_END_STATUS;
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "CheckDayEndStatus() - Exception: " + ex.Message, true);
                strStatus = "N";
            }
            finally
            {
                objAU = null;
            }

            return strStatus;
        }
        #endregion

        #region GetDayEndReportData
        private void GetDayEndReportData(DateTime dtDayEnd)
        {

            //CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Started Updating Billing Accounts", false);
            #region Members & Variables
            DataTable dtbl = new DataTable();
            string strCatchMessage = string.Empty;
            AccountUpdate objAU = new AccountUpdate();
            QBDriver driver = new QBDriver();
            string error = null;
            //string[] arrTxnID= new string[0];
            string strTxnID = string.Empty;
            DateTime dtCreate = DateTime.Now;
            DateTime dtModify = DateTime.Now;
            DateTime dtTran = DateTime.Now;
            string strRefNo = string.Empty;
            string strGLCode = string.Empty;
            double dblCrAmt = 0;
            double dblDrAmt = 0;
            string strDrCrName = string.Empty;
            int intRecID = 0;
            string errRegion = string.Empty;
            #endregion


            try
            {

                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Fetching Day End Data...", false);

                // All Journal entry transactions max 10,000 entries
                //var data = driver.GetJournlaEntries(ref error, null, null);
                // All transactions for a date 10th December 2020
                errRegion = "driver.GetJournlEntries";
                var data = driver.GetJournlEntries(ref error, dtDayEnd, dtDayEnd);
                //arrTxnID = new string[data.Count];
                if (data != null)
                {

                    if (data.Count > 0)
                    {
                        errRegion = "data.Count > 0";
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Data fetched...organising data for " + dtDayEnd.ToString("MM/dd/yyyy"), false);
                        dtbl = CreateDayendReportTable();

                        #region get and populate data
                        for (int i = 0; i < data.Count; i++)
                        {
                            //arrTxnID[i] = data[i].TxnID.Trim();
                            strRefNo = data[i].RefNumber;
                            dtCreate = Convert.ToDateTime(data[i].TimeCreated);
                            dtModify = Convert.ToDateTime(data[i].TimeModified);
                            dtTran = Convert.ToDateTime(data[i].TxnDate);
                            strTxnID = data[i].TxnID.Trim();

                            if (data[i].Lines.Count > 0)
                            {
                                for (int j = 0; j < data[i].Lines.Count; j++)
                                {
                                    strGLCode = data[i].Lines[j].AccountNumber;
                                    if (data[i].Lines[j].CreditAmount != null) dblCrAmt = Convert.ToDouble(data[i].Lines[j].CreditAmount); else dblCrAmt = 0;
                                    if (data[i].Lines[j].DebitAmount != null) dblDrAmt = Convert.ToDouble(data[i].Lines[j].DebitAmount); else dblDrAmt = 0;
                                    if (data[i].Lines[j].CustomerFullName != null) strDrCrName = data[i].Lines[j].CustomerFullName; else strDrCrName = string.Empty;

                                    DataRow dr = dtbl.NewRow();
                                    intRecID = intRecID + 1;
                                    dr["srl_no"] = intRecID;
                                    dr["ref_no"] = strRefNo;
                                    dr["date_created"] = dtCreate;
                                    dr["date_modified"] = dtModify;
                                    dr["date_txn"] = dtTran;
                                    dr["txn_id"] = strTxnID;
                                    dr["gl_code"] = strGLCode;
                                    dr["dr_amount"] = dblDrAmt;
                                    dr["cr_amount"] = dblCrAmt;
                                    dr["dr_cr_name"] = strDrCrName;
                                    dtbl.Rows.Add(dr);
                                }
                            }
                            else
                            {
                                DataRow dr = dtbl.NewRow();
                                intRecID = intRecID + 1;
                                dr["srl_no"] = intRecID;
                                dr["ref_no"] = strRefNo;
                                dr["date_created"] = dtCreate;
                                dr["date_modified"] = dtModify;
                                dr["date_txn"] = dtTran;
                                dr["txn_id"] = strTxnID;
                                dr["gl_code"] = string.Empty;
                                dr["dr_amount"] = 0;
                                dr["cr_amount"] = 0;
                                dr["dr_cr_name"] = string.Empty;
                                dtbl.Rows.Add(dr);
                            }
                        }
                        #endregion

                        #region Save Data
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Saving day end data for " + dtDayEnd.ToString("MM/dd/yyyy"), false);
                        objAU.DAY_END_DATE = dtDayEnd;
                        if (objAU.SaveDayEndAccountPosting(strConfigPath, intServiceID, strSvcName, dtbl, ref strCatchMessage))
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Day end accounts data saved succesfully for the date " + dtDayEnd.ToString("MM/dd/yyyy"), false);
                            objAU = null;
                            GenerateDayEndReport(dtDayEnd);
                        }
                        else
                        {
                            if (strCatchMessage.Trim() != string.Empty)
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetDayEndReportData()==>Core:SaveDayEndAccountPosting()- Exception : " + strCatchMessage, true);
                        }
                        #endregion
                    }
                    else
                    {
                        errRegion = "if (data.Count <= 0)";
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Updating day end processing status for " + dtDayEnd.ToString("MM/dd/yyyy"), false);
                        objAU.DAY_END_DATE = dtDayEnd;
                        if (objAU.UpdateDayEndProcessStatus(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Updated day end processing status succesfully for the date " + dtDayEnd.ToString("MM/dd/yyyy"), false);
                           
                        }
                        else
                        {
                            if (strCatchMessage.Trim() != string.Empty)
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetDayEndReportData()==>Core:UpdateDayEndProcessStatus()- Exception : " + strCatchMessage, true);
                        }
                    }

                }
                else
                {
                    errRegion = "data == null";
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetDayEndReportData()==>GetJournlaEntries()- Error : " + Convert.ToString(error), true);
                }


                //var trans = driver.GetJournlaEntries(ref error, new DateTime(2020, 12, 9), new DateTime(2020, 12, 9), arrTxnID).FirstOrDefault();
                // Particular Transaction : supply transaction id from database
                //var trans = driver.GetJournlaEntries(ref error, null, null, "4-1590393931").FirstOrDefault();

                //else
                //{
                //    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "PostVouchers()==>FetchListForVoucher():Core-Exception: " + strCatchMessage, true);
                //}
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetDayEndReportData() -" + errRegion + "- Exception: " + ex.Message, true);
            }
            finally
            {
                dtbl.Dispose(); dtbl = null; objAU = null;
                driver.Dispose(); driver = null;
            }

        }
        #endregion

        #region CreateDayendReportTable
        private DataTable CreateDayendReportTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("srl_no", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("ref_no", System.Type.GetType("System.String"));
            dtbl.Columns.Add("date_created", System.Type.GetType("System.DateTime"));
            dtbl.Columns.Add("date_modified", System.Type.GetType("System.DateTime"));
            dtbl.Columns.Add("date_txn", System.Type.GetType("System.DateTime"));
            dtbl.Columns.Add("txn_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("gl_code", System.Type.GetType("System.String"));
            dtbl.Columns.Add("dr_amount", System.Type.GetType("System.Double"));
            dtbl.Columns.Add("cr_amount", System.Type.GetType("System.Double"));
            dtbl.Columns.Add("dr_cr_name", System.Type.GetType("System.String"));
            dtbl.TableName = "DayEndReport";
            return dtbl;
        }
        #endregion

        #region GenerateDayEndReport
        private void GenerateDayEndReport(DateTime dtDayEnd)
        {
            DataSet ds = new DataSet();
            AccountUpdate objAU = new AccountUpdate();
            bool bRet = false;
            string strRptFileName = string.Empty;
            string strCatchMessage = string.Empty;

            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Generation of day end report started for " + dtDayEnd.ToString("MM/dd/yyyy"), false);
                SetLicenseKey();
                objAU.DAY_END_DATE = dtDayEnd;
                bRet = objAU.FetchDayEndVoucherPostingReport(strConfigPath,ref ds, ref strCatchMessage);
                if (bRet)
                {
                    if(ds.Tables.Count>0)
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Creating day end report...", false);
                        if (CreateDayEndReport(ds, dtDayEnd, ref strRptFileName))
                        {
                            GenerateDayEndMail(strRptFileName, dtDayEnd);
                        }
                        else
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetDayEndReportData()==>GenerateDayEndReport()==>CreateDayEndReport()- Error : Failed to create the day end report", true);
                    }
                    
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetDayEndReportData()==>GenerateDayEndReport()==>Core:FetchDayeEndVoucherPostingReport()- Exception : " + strCatchMessage, true);
                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetDayEndReportData()==>GenerateDayEndReport()- Exception : " + ex.Message, true);
            }
            finally
            {
                objAU = null;
                ds.Dispose();
            }
        }
        #endregion

        #region SetLicenseKey
        private void SetLicenseKey()
        {
            try
            {
                CoreCommon.GetReportLicenseKey(Application.StartupPath);
                SpreadsheetInfo.SetLicense(CoreCommon.REPORT_LICENSE_KEY);

            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetDayEndReportData()==>GenerateDayEndReport()==>SetLicenseKey()- Exception : " + expErr.Message, true);
            }
        }
        #endregion

        #region CreateDayEndReport
        private bool CreateDayEndReport(DataSet ds, DateTime dtDayEnd, ref string strReportName)
        {
            bool bRet = false;
            ExcelFile objExcelFile = null;
            ExcelWorksheet objExcelWorksheet1 = null;
            ExcelWorksheet objExcelWorksheet2 = null;
            //ExcelWorksheet objExcelWorksheet3 = null;
            //ExcelWorksheet objExcelWorksheet4 = null;
            //ExcelWorksheet objExcelWorksheet5 = null;
            //ExcelWorksheet objExcelWorksheet6 = null;
            //ExcelWorksheet objExcelWorksheet7 = null;
            ExcelWorksheet objExcelWorksheet8 = null;
            string strRName = "DayEndAccountsSnapshot_"  + dtDayEnd.ToString("MMddyyyy") + ".xlsx";
            int intRowIndex = 0;
            int intNoOfCols = 0;
            double dblSubTotDr = 0;
            double dblSubTotCr = 0;
            double dblGrandTotDr = 0;
            double dblGrandTotCr = 0;
            double dblGrandTotDrVRS = 0;
            double dblGrandTotCrVRS = 0;
            double dblGrandTotDrVar = 0;
            double dblGrandTotCrVar = 0;
            double dblGrandTot = 0;

            try
            {
                
                strReportName =Application.StartupPath + "/Temp/" + strRName;
                if (System.IO.Directory.Exists(Application.StartupPath + "/Temp") == false) { System.IO.Directory.CreateDirectory(Application.StartupPath + "/Temp"); }
                if (System.IO.File.Exists(strReportName) == true) { System.IO.File.Delete(strReportName); }

                objExcelFile = new ExcelFile();
                
                objExcelWorksheet2 = objExcelFile.Worksheets.Add("AccountSummary");
                //objExcelWorksheet3 = objExcelFile.Worksheets.Add("InvoiceApproved");
                //objExcelWorksheet4 = objExcelFile.Worksheets.Add("InvoiceDisapproved");
                //objExcelWorksheet5 = objExcelFile.Worksheets.Add("PaymentReceived");
                //objExcelWorksheet6 = objExcelFile.Worksheets.Add("PaymentRefunded");
                //objExcelWorksheet7 = objExcelFile.Worksheets.Add("PaymentDone");
                objExcelWorksheet8 = objExcelFile.Worksheets.Add("PostingFailed");
                objExcelWorksheet1 = objExcelFile.Worksheets.Add("VouchersPosted");

                intRowIndex = 1;

                #region CellStyles
                CellStyle TitleCellStyle = new GemBox.Spreadsheet.CellStyle();
                TitleCellStyle.VerticalAlignment = VerticalAlignmentStyle.Center;
                TitleCellStyle.Font.Color = System.Drawing.Color.Black;
                //TitleCellStyle.FillPattern.SetSolid(System.Drawing.Color.LightGray);
                TitleCellStyle.Font.Weight = ExcelFont.BoldWeight;
                TitleCellStyle.Font.Size = 10 * 20;
                TitleCellStyle.Font.Name = "Arial";

                CellStyle HeaderCellStyle = new GemBox.Spreadsheet.CellStyle();
                HeaderCellStyle.VerticalAlignment = VerticalAlignmentStyle.Center;
                //HeaderCellStyle.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                HeaderCellStyle.WrapText = true;
                HeaderCellStyle.Font.Color = System.Drawing.Color.Black;
                //HeaderCellStyle.FillPattern.SetSolid(System.Drawing.Color.LightGray);
                HeaderCellStyle.Font.Weight = ExcelFont.BoldWeight;
                HeaderCellStyle.Font.Size = 9 * 20;
                HeaderCellStyle.Font.Name = "Arial";
                HeaderCellStyle.Borders.SetBorders(GemBox.Spreadsheet.MultipleBorders.Outside, System.Drawing.Color.Black, GemBox.Spreadsheet.LineStyle.Thin);

                CellStyle DataCellStyle = new GemBox.Spreadsheet.CellStyle();
                DataCellStyle.VerticalAlignment = VerticalAlignmentStyle.Center;
                DataCellStyle.WrapText = true;
                DataCellStyle.Font.Color = System.Drawing.Color.Black;
                DataCellStyle.Font.Size = 9 * 20;
                DataCellStyle.Font.Name = "Arial";
                //DataCellStyle.Borders.SetBorders(GemBox.Spreadsheet.MultipleBorders.Bottom, System.Drawing.Color.Black, GemBox.Spreadsheet.LineStyle.Thin);

                #endregion

                #region Vouchers Posted

                #region Set Column Width
                intNoOfCols = 10;
                objExcelWorksheet1.Columns[1].Width = 3000;//Date Created
                objExcelWorksheet1.Columns[2].Width = 3000;//Reference #
                objExcelWorksheet1.Columns[3].Width = 4000;//Transaction ID
                objExcelWorksheet1.Columns[4].Width = 6000;//Voucher Type
                objExcelWorksheet1.Columns[5].Width = 6000;//VETRIS Reference #
                objExcelWorksheet1.Columns[6].Width = 8000;//Debtor/Creditor
                objExcelWorksheet1.Columns[7].Width = 2500;//GL Code
                objExcelWorksheet1.Columns[8].Width = 13500;//GL Desc
                objExcelWorksheet1.Columns[9].Width = 3500;// Amount Dr.
                objExcelWorksheet1.Columns[10].Width = 3500;// Amount Cr.

                for (int i = 1; i < intNoOfCols + 1; i++)
                {
                    objExcelWorksheet1.Columns[i].Style.WrapText = true;
                }
                #endregion

                #region Create Title
                objExcelWorksheet1.Cells[intRowIndex, 1].Value = "Voucher(s) Posted To Quick Books On " + dtDayEnd.ToString("MMMM dd, yyyy");
                objExcelWorksheet1.Cells[intRowIndex, 1].Style = TitleCellStyle;
                objExcelWorksheet1.Cells[intRowIndex, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                CellRange objCellRangeT1 = objExcelWorksheet1.Cells.GetSubrangeAbsolute(intRowIndex, 1, intRowIndex, intNoOfCols);
                objCellRangeT1.Merged = true;
                #endregion

                #region Create Header
                intRowIndex = intRowIndex + 2;
                objExcelWorksheet1.Rows[intRowIndex].Height = 500;

                objExcelWorksheet1.Cells[intRowIndex, 1].Value = "Date Created";
                CellRange objCellRange0 = objExcelWorksheet1.Cells.GetSubrange("B" + Convert.ToString(intRowIndex + 1), "B" + Convert.ToString(intRowIndex + 1));
                objCellRange0.Merged = true;
                objCellRange0.Style = HeaderCellStyle;
                objCellRange0.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                objExcelWorksheet1.Cells[intRowIndex, 2].Value = "Reference #";
                CellRange objCellRange1 = objExcelWorksheet1.Cells.GetSubrange("C" + Convert.ToString(intRowIndex + 1), "C" + Convert.ToString(intRowIndex + 1));
                objCellRange1.Merged = true;
                objCellRange1.Style = HeaderCellStyle;
                objCellRange1.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                objExcelWorksheet1.Cells[intRowIndex, 3].Value = "Transaction ID";
                CellRange objCellRange2 = objExcelWorksheet1.Cells.GetSubrange("D" + Convert.ToString(intRowIndex + 1), "D" + Convert.ToString(intRowIndex + 1));
                objCellRange2.Merged = true;
                objCellRange2.Style = HeaderCellStyle;
                objCellRange2.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                objExcelWorksheet1.Cells[intRowIndex, 4].Value = "Voucher Type";
                CellRange objCellRange3 = objExcelWorksheet1.Cells.GetSubrange("E" + Convert.ToString(intRowIndex + 1), "E" + Convert.ToString(intRowIndex + 1));
                objCellRange3.Merged = true;
                objCellRange3.Style = HeaderCellStyle;
                objCellRange3.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                objExcelWorksheet1.Cells[intRowIndex, 5].Value = "VETRIS Reference #";
                CellRange objCellRange4 = objExcelWorksheet1.Cells.GetSubrange("F" + Convert.ToString(intRowIndex + 1), "F" + Convert.ToString(intRowIndex + 1));
                objCellRange4.Merged = true;
                objCellRange4.Style = HeaderCellStyle;
                objCellRange4.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                objExcelWorksheet1.Cells[intRowIndex, 6].Value = "Debtor/Creditor";
                CellRange objCellRange5 = objExcelWorksheet1.Cells.GetSubrange("G" + Convert.ToString(intRowIndex + 1), "G" + Convert.ToString(intRowIndex + 1));
                objCellRange5.Merged = true;
                objCellRange5.Style = HeaderCellStyle;
                objCellRange5.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                objExcelWorksheet1.Cells[intRowIndex, 7].Value = "G/L Code";
                CellRange objCellRange6 = objExcelWorksheet1.Cells.GetSubrange("H" + Convert.ToString(intRowIndex + 1), "H" + Convert.ToString(intRowIndex + 1));
                objCellRange6.Merged = true;
                objCellRange6.Style = HeaderCellStyle;
                objCellRange6.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                objExcelWorksheet1.Cells[intRowIndex, 8].Value = "G/L Description";
                CellRange objCellRange7 = objExcelWorksheet1.Cells.GetSubrange("I" + Convert.ToString(intRowIndex + 1), "I" + Convert.ToString(intRowIndex + 1));
                objCellRange7.Merged = true;
                objCellRange7.Style = HeaderCellStyle;
                objCellRange7.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                objExcelWorksheet1.Cells[intRowIndex, 9].Value = "Amount Dr. ($)";
                CellRange objCellRange8 = objExcelWorksheet1.Cells.GetSubrange("J" + Convert.ToString(intRowIndex + 1), "J" + Convert.ToString(intRowIndex + 1));
                objCellRange8.Merged = true;
                objCellRange8.Style = HeaderCellStyle;
                objCellRange8.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                objExcelWorksheet1.Cells[intRowIndex, 10].Value = "Amount Cr. ($)";
                CellRange objCellRange9 = objExcelWorksheet1.Cells.GetSubrange("K" + Convert.ToString(intRowIndex + 1), "K" + Convert.ToString(intRowIndex + 1));
                objCellRange9.Merged = true;
                objCellRange9.Style = HeaderCellStyle;
                objCellRange9.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                #endregion

                #region Create Details
                intRowIndex = intRowIndex + 1;
                if (ds.Tables["VoucherPosted"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["VoucherPosted"].Rows)
                    {
                        for (int i = 1; i < intNoOfCols + 1; i++)
                        {
                            objExcelWorksheet1.Cells[intRowIndex, i].Style = DataCellStyle;
                        }

                        if (dr["date_created"] != DBNull.Value)
                        {
                            objExcelWorksheet1.Cells[intRowIndex, 1].Value = dr["date_created"];
                            objExcelWorksheet1.Cells[intRowIndex, 1].Style.NumberFormat = "MM-dd-yyyy"; 
                            objExcelWorksheet1.Cells[intRowIndex, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                        }

                        objExcelWorksheet1.Cells[intRowIndex, 2].Value = dr["ref_no"];
                        objExcelWorksheet1.Cells[intRowIndex, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                        objExcelWorksheet1.Cells[intRowIndex, 3].Value = dr["txn_id"];
                        objExcelWorksheet1.Cells[intRowIndex, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                        objExcelWorksheet1.Cells[intRowIndex, 4].Value = dr["txn_type"];
                        objExcelWorksheet1.Cells[intRowIndex, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                        objExcelWorksheet1.Cells[intRowIndex, 5].Value = dr["txn_ref_no"];
                        objExcelWorksheet1.Cells[intRowIndex, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                        objExcelWorksheet1.Cells[intRowIndex, 6].Value = dr["dr_cr_name"];
                        objExcelWorksheet1.Cells[intRowIndex, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                        objExcelWorksheet1.Cells[intRowIndex, 7].Value = dr["gl_code"];
                        objExcelWorksheet1.Cells[intRowIndex, 7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                        objExcelWorksheet1.Cells[intRowIndex, 8].Value = dr["gl_desc"];
                        objExcelWorksheet1.Cells[intRowIndex, 8].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                        objExcelWorksheet1.Cells[intRowIndex, 9].Value = dr["dr_amount"];
                        objExcelWorksheet1.Cells[intRowIndex, 9].Style.NumberFormat = "#0.00"; 
                        objExcelWorksheet1.Cells[intRowIndex, 9].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        objExcelWorksheet1.Cells[intRowIndex, 10].Value = dr["cr_amount"];
                        objExcelWorksheet1.Cells[intRowIndex, 10].Style.NumberFormat = "#0.00"; 
                        objExcelWorksheet1.Cells[intRowIndex, 10].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        dblGrandTotDr = dblGrandTotDr + Convert.ToDouble(dr["dr_amount"]);
                        dblGrandTotCr = dblGrandTotCr + Convert.ToDouble(dr["cr_amount"]);

                        if(Convert.ToString(dr["initialise_sub_total"]) == "Y")
                        {
                            dblSubTotDr = 0;
                            dblSubTotCr = 0;
                        }

                        dblSubTotDr = dblSubTotDr + Convert.ToDouble(dr["dr_amount"]);
                        dblSubTotCr = dblSubTotCr + Convert.ToDouble(dr["cr_amount"]);

                        if (Convert.ToString(dr["print_sub_total"]) == "Y")
                        {
                            intRowIndex = intRowIndex + 1;

                            objExcelWorksheet1.Cells[intRowIndex, 1].Value = "Sub Total";
                            CellRange objCellRangeST = objExcelWorksheet1.Cells.GetSubrangeAbsolute(intRowIndex, 1, intRowIndex, 8);
                            objCellRangeST.Merged = true;
                            objCellRangeST.Style = HeaderCellStyle;
                            objCellRangeST.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                            objExcelWorksheet1.Cells[intRowIndex, 9].Value = dblSubTotDr;
                            objExcelWorksheet1.Cells[intRowIndex, 9].Style = HeaderCellStyle;
                            objExcelWorksheet1.Cells[intRowIndex, 9].Style.NumberFormat = "#0.00"; 
                            objExcelWorksheet1.Cells[intRowIndex, 9].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                            objExcelWorksheet1.Cells[intRowIndex, 10].Value = dblSubTotCr;
                            objExcelWorksheet1.Cells[intRowIndex, 10].Style = HeaderCellStyle;
                            objExcelWorksheet1.Cells[intRowIndex, 10].Style.NumberFormat = "#0.00";
                            objExcelWorksheet1.Cells[intRowIndex, 10].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                            intRowIndex = intRowIndex + 1;
                        }

                        intRowIndex = intRowIndex + 1;
                    }

                    objExcelWorksheet1.Cells[intRowIndex, 1].Value = "Grand Total";
                    CellRange objCellRangeGT1 = objExcelWorksheet1.Cells.GetSubrangeAbsolute(intRowIndex, 1, intRowIndex, 8);
                    objCellRangeGT1.Merged = true;
                    objCellRangeGT1.Style = HeaderCellStyle;
                    objCellRangeGT1.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet1.Cells[intRowIndex, 9].Value = dblGrandTotDr;
                    objExcelWorksheet1.Cells[intRowIndex, 9].Style = HeaderCellStyle;
                    objExcelWorksheet1.Cells[intRowIndex, 9].Style.NumberFormat = "#0.00"; 
                    objExcelWorksheet1.Cells[intRowIndex, 9].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet1.Cells[intRowIndex, 10].Value = dblGrandTotCr;
                    objExcelWorksheet1.Cells[intRowIndex, 10].Style = HeaderCellStyle;
                    objExcelWorksheet1.Cells[intRowIndex, 10].Style.NumberFormat = "#0.00";
                    objExcelWorksheet1.Cells[intRowIndex, 10].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                }
                #endregion

                #endregion

                intRowIndex = 1;

                #region Account Wise Summary


                #region Set Column Width
                intNoOfCols = 8;
                objExcelWorksheet2.Columns[1].Width = 2500;//GL Code
                objExcelWorksheet2.Columns[2].Width = 13500;//GL Desc
                objExcelWorksheet2.Columns[3].Width = 3500;// Amount Dr.(QB)
                objExcelWorksheet2.Columns[4].Width = 3500;// Amount Cr.(QB)
                objExcelWorksheet2.Columns[5].Width = 3500;// Amount Dr.(VETRIS)
                objExcelWorksheet2.Columns[6].Width = 3500;// Amount Cr.(VETRIS)
                objExcelWorksheet2.Columns[7].Width = 3500;// Amount Dr.(Variance)
                objExcelWorksheet2.Columns[8].Width = 3500;// Amount Cr.(Variance)

                for (int i = 1; i < intNoOfCols + 1; i++)
                {
                    objExcelWorksheet2.Columns[i].Style.WrapText = true;
                }
                #endregion

                #region All
                #region Create Title
                objExcelWorksheet2.Cells[intRowIndex, 1].Value = "Account Wise Summary On " + dtDayEnd.ToString("MMMM dd, yyyy");
                objExcelWorksheet2.Cells[intRowIndex, 1].Style = TitleCellStyle;
                objExcelWorksheet2.Cells[intRowIndex, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                CellRange objCellRangeT2 = objExcelWorksheet2.Cells.GetSubrangeAbsolute(intRowIndex, 1, intRowIndex, intNoOfCols);
                objCellRangeT2.Merged = true;
                #endregion

                #region Create Header
                intRowIndex = intRowIndex + 2;

                #region First Row
                objExcelWorksheet2.Rows[intRowIndex].Height = 500;

                objExcelWorksheet2.Cells[intRowIndex, 1].Value = "G/L Code";
                CellRange objCellRange10 = objExcelWorksheet2.Cells.GetSubrange("B" + Convert.ToString(intRowIndex + 1), "B" + Convert.ToString(intRowIndex + 2));
                objCellRange10.Merged = true;
                objCellRange10.Style = HeaderCellStyle;
                objCellRange10.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                objExcelWorksheet2.Cells[intRowIndex, 2].Value = "G/L Description";
                CellRange objCellRange11 = objExcelWorksheet2.Cells.GetSubrange("C" + Convert.ToString(intRowIndex + 1), "C" + Convert.ToString(intRowIndex + 2));
                objCellRange11.Merged = true;
                objCellRange11.Style = HeaderCellStyle;
                objCellRange11.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                objExcelWorksheet2.Cells[intRowIndex, 3].Value = "As per Quick Books";
                CellRange objCellRange12 = objExcelWorksheet2.Cells.GetSubrange("D" + Convert.ToString(intRowIndex + 1), "E" + Convert.ToString(intRowIndex + 1));
                objCellRange12.Merged = true;
                objCellRange12.Style = HeaderCellStyle;
                objCellRange12.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                objExcelWorksheet2.Cells[intRowIndex, 5].Value = "As per VETRIS";
                CellRange objCellRange13 = objExcelWorksheet2.Cells.GetSubrange("F" + Convert.ToString(intRowIndex + 1), "G" + Convert.ToString(intRowIndex + 1));
                objCellRange13.Merged = true;
                objCellRange13.Style = HeaderCellStyle;
                objCellRange13.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                objExcelWorksheet2.Cells[intRowIndex, 7].Value = "Variance";
                CellRange objCellRange47 = objExcelWorksheet2.Cells.GetSubrange("H" + Convert.ToString(intRowIndex + 1), "I" + Convert.ToString(intRowIndex + 1));
                objCellRange47.Merged = true;
                objCellRange47.Style = HeaderCellStyle;
                objCellRange47.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                #endregion

                intRowIndex = intRowIndex + 1;

                #region Second Row

                objExcelWorksheet2.Cells[intRowIndex, 3].Value = "Amount Dr. ($)";
                //CellRange objCellRange12 = objExcelWorksheet2.Cells.GetSubrange("C" + Convert.ToString(intRowIndex + 1), "C" + Convert.ToString(intRowIndex + 1));
                //objCellRange12.Merged = true;
                objExcelWorksheet2.Cells[intRowIndex, 3].Style = HeaderCellStyle;
                objExcelWorksheet2.Cells[intRowIndex, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                objExcelWorksheet2.Cells[intRowIndex, 4].Value = "Amount Cr. ($)";
                //CellRange objCellRange13 = objExcelWorksheet2.Cells.GetSubrange("D" + Convert.ToString(intRowIndex + 1), "D" + Convert.ToString(intRowIndex + 1));
                //objCellRange13.Merged = true;
                objExcelWorksheet2.Cells[intRowIndex, 4].Style = HeaderCellStyle;
                objExcelWorksheet2.Cells[intRowIndex, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                objExcelWorksheet2.Cells[intRowIndex, 5].Value = "Amount Dr. ($)";
                //CellRange objCellRange12 = objExcelWorksheet2.Cells.GetSubrange("C" + Convert.ToString(intRowIndex + 1), "C" + Convert.ToString(intRowIndex + 1));
                //objCellRange12.Merged = true;
                objExcelWorksheet2.Cells[intRowIndex, 5].Style = HeaderCellStyle;
                objExcelWorksheet2.Cells[intRowIndex, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                objExcelWorksheet2.Cells[intRowIndex, 6].Value = "Amount Cr. ($)";
                //CellRange objCellRange13 = objExcelWorksheet2.Cells.GetSubrange("D" + Convert.ToString(intRowIndex + 1), "D" + Convert.ToString(intRowIndex + 1));
                //objCellRange13.Merged = true;
                objExcelWorksheet2.Cells[intRowIndex, 6].Style = HeaderCellStyle;
                objExcelWorksheet2.Cells[intRowIndex, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                objExcelWorksheet2.Cells[intRowIndex, 7].Value = "Amount Dr. ($)";
                //CellRange objCellRange12 = objExcelWorksheet2.Cells.GetSubrange("C" + Convert.ToString(intRowIndex + 1), "C" + Convert.ToString(intRowIndex + 1));
                //objCellRange12.Merged = true;
                objExcelWorksheet2.Cells[intRowIndex, 7].Style = HeaderCellStyle;
                objExcelWorksheet2.Cells[intRowIndex, 7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                objExcelWorksheet2.Cells[intRowIndex, 8].Value = "Amount Cr. ($)";
                //CellRange objCellRange13 = objExcelWorksheet2.Cells.GetSubrange("D" + Convert.ToString(intRowIndex + 1), "D" + Convert.ToString(intRowIndex + 1));
                //objCellRange13.Merged = true;
                objExcelWorksheet2.Cells[intRowIndex, 8].Style = HeaderCellStyle;
                objExcelWorksheet2.Cells[intRowIndex, 8].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                #endregion

                #endregion

                #region Create Details
                intRowIndex = intRowIndex + 1;
                dblGrandTotDr = 0;
                dblGrandTotCr = 0;

                if (ds.Tables["AcctSummary"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["AcctSummary"].Rows)
                    {
                        for (int i = 1; i < intNoOfCols + 1; i++)
                        {
                            objExcelWorksheet2.Cells[intRowIndex, i].Style = DataCellStyle;
                        }

                        objExcelWorksheet2.Cells[intRowIndex, 1].Value = dr["gl_code"];
                        objExcelWorksheet2.Cells[intRowIndex, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                        objExcelWorksheet2.Cells[intRowIndex, 2].Value = dr["gl_desc"];
                        objExcelWorksheet2.Cells[intRowIndex, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                        objExcelWorksheet2.Cells[intRowIndex, 3].Value = dr["dr_amount_qb"];
                        objExcelWorksheet2.Cells[intRowIndex, 3].Style.NumberFormat = "#0.00";
                        objExcelWorksheet2.Cells[intRowIndex, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        objExcelWorksheet2.Cells[intRowIndex, 4].Value = dr["cr_amount_qb"];
                        objExcelWorksheet2.Cells[intRowIndex, 4].Style.NumberFormat = "#0.00";
                        objExcelWorksheet2.Cells[intRowIndex, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        objExcelWorksheet2.Cells[intRowIndex, 5].Value = dr["dr_amount_vrs"];
                        objExcelWorksheet2.Cells[intRowIndex, 5].Style.NumberFormat = "#0.00";
                        objExcelWorksheet2.Cells[intRowIndex, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        objExcelWorksheet2.Cells[intRowIndex, 6].Value = dr["cr_amount_vrs"];
                        objExcelWorksheet2.Cells[intRowIndex, 6].Style.NumberFormat = "#0.00";
                        objExcelWorksheet2.Cells[intRowIndex, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        objExcelWorksheet2.Cells[intRowIndex, 7].Value = dr["dr_amount_variance"];
                        objExcelWorksheet2.Cells[intRowIndex, 7].Style.NumberFormat = "#0.00";
                        objExcelWorksheet2.Cells[intRowIndex, 7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        objExcelWorksheet2.Cells[intRowIndex, 8].Value = dr["cr_amount_variance"];
                        objExcelWorksheet2.Cells[intRowIndex, 8].Style.NumberFormat = "#0.00";
                        objExcelWorksheet2.Cells[intRowIndex, 8].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        dblGrandTotDr = dblGrandTotDr + Convert.ToDouble(dr["dr_amount_qb"]);
                        dblGrandTotCr = dblGrandTotCr + Convert.ToDouble(dr["cr_amount_qb"]);
                        dblGrandTotDrVRS = dblGrandTotDrVRS + Convert.ToDouble(dr["dr_amount_vrs"]);
                        dblGrandTotCrVRS = dblGrandTotCrVRS + Convert.ToDouble(dr["cr_amount_vrs"]);
                        dblGrandTotDrVar = dblGrandTotDrVar + Convert.ToDouble(dr["dr_amount_variance"]);
                        dblGrandTotCrVar = dblGrandTotCrVar + Convert.ToDouble(dr["cr_amount_variance"]);

                        intRowIndex = intRowIndex + 1;
                    }

                    objExcelWorksheet2.Cells[intRowIndex, 1].Value = "Grand Total";
                    CellRange objCellRangeGT2 = objExcelWorksheet2.Cells.GetSubrangeAbsolute(intRowIndex, 1, intRowIndex, 2);
                    objCellRangeGT2.Merged = true;
                    objCellRangeGT2.Style = HeaderCellStyle;
                    objCellRangeGT2.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet2.Cells[intRowIndex, 3].Value = dblGrandTotDr;
                    objExcelWorksheet2.Cells[intRowIndex, 3].Style = HeaderCellStyle;
                    objExcelWorksheet2.Cells[intRowIndex, 3].Style.NumberFormat = "#0.00";
                    objExcelWorksheet2.Cells[intRowIndex, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet2.Cells[intRowIndex, 4].Value = dblGrandTotCr;
                    objExcelWorksheet2.Cells[intRowIndex, 4].Style = HeaderCellStyle;
                    objExcelWorksheet2.Cells[intRowIndex, 4].Style.NumberFormat = "#0.00";
                    objExcelWorksheet2.Cells[intRowIndex, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet2.Cells[intRowIndex, 5].Value = dblGrandTotDrVRS;
                    objExcelWorksheet2.Cells[intRowIndex, 5].Style = HeaderCellStyle;
                    objExcelWorksheet2.Cells[intRowIndex, 5].Style.NumberFormat = "#0.00";
                    objExcelWorksheet2.Cells[intRowIndex, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet2.Cells[intRowIndex, 6].Value = dblGrandTotCrVRS;
                    objExcelWorksheet2.Cells[intRowIndex, 6].Style = HeaderCellStyle;
                    objExcelWorksheet2.Cells[intRowIndex, 6].Style.NumberFormat = "#0.00";
                    objExcelWorksheet2.Cells[intRowIndex, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet2.Cells[intRowIndex, 7].Value = dblGrandTotDrVar;
                    objExcelWorksheet2.Cells[intRowIndex, 7].Style = HeaderCellStyle;
                    objExcelWorksheet2.Cells[intRowIndex, 7].Style.NumberFormat = "#0.00";
                    objExcelWorksheet2.Cells[intRowIndex, 7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet2.Cells[intRowIndex, 8].Value = dblGrandTotCrVar;
                    objExcelWorksheet2.Cells[intRowIndex, 8].Style = HeaderCellStyle;
                    objExcelWorksheet2.Cells[intRowIndex, 8].Style.NumberFormat = "#0.00";
                    objExcelWorksheet2.Cells[intRowIndex, 8].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                }
                #endregion
                #endregion

                intRowIndex = intRowIndex + 3;

                #region Invoicing

                #region Create Title
                objExcelWorksheet2.Cells[intRowIndex, 1].Value = "Account Wise Summary For Invoice/Invoice Reversal On " + dtDayEnd.ToString("MMMM dd, yyyy");
                CellRange objCellRangeT2_1 = objExcelWorksheet2.Cells.GetSubrangeAbsolute(intRowIndex, 1, intRowIndex, intNoOfCols);
                objCellRangeT2_1.Merged = true;
                objCellRangeT2_1.Style = HeaderCellStyle;
                objCellRangeT2_1.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                #endregion

                #region Create Header
                intRowIndex = intRowIndex + 1;

                #region First Row
                objExcelWorksheet2.Rows[intRowIndex].Height = 500;

                objExcelWorksheet2.Cells[intRowIndex, 1].Value = "G/L Code";
                CellRange objCellRange10_1 = objExcelWorksheet2.Cells.GetSubrange("B" + Convert.ToString(intRowIndex + 1), "B" + Convert.ToString(intRowIndex + 2));
                objCellRange10_1.Merged = true;
                objCellRange10_1.Style = HeaderCellStyle;
                objCellRange10_1.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                objExcelWorksheet2.Cells[intRowIndex, 2].Value = "G/L Description";
                CellRange objCellRange11_1 = objExcelWorksheet2.Cells.GetSubrange("C" + Convert.ToString(intRowIndex + 1), "C" + Convert.ToString(intRowIndex + 2));
                objCellRange11_1.Merged = true;
                objCellRange11_1.Style = HeaderCellStyle;
                objCellRange11_1.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                objExcelWorksheet2.Cells[intRowIndex, 3].Value = "As per Quick Books";
                CellRange objCellRange12_1 = objExcelWorksheet2.Cells.GetSubrange("D" + Convert.ToString(intRowIndex + 1), "E" + Convert.ToString(intRowIndex + 1));
                objCellRange12_1.Merged = true;
                objCellRange12_1.Style = HeaderCellStyle;
                objCellRange12_1.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                objExcelWorksheet2.Cells[intRowIndex, 5].Value = "As per VETRIS";
                CellRange objCellRange13_1 = objExcelWorksheet2.Cells.GetSubrange("F" + Convert.ToString(intRowIndex + 1), "G" + Convert.ToString(intRowIndex + 1));
                objCellRange13_1.Merged = true;
                objCellRange13_1.Style = HeaderCellStyle;
                objCellRange13_1.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                objExcelWorksheet2.Cells[intRowIndex, 7].Value = "Variance";
                CellRange objCellRange47_1 = objExcelWorksheet2.Cells.GetSubrange("H" + Convert.ToString(intRowIndex + 1), "I" + Convert.ToString(intRowIndex + 1));
                objCellRange47_1.Merged = true;
                objCellRange47_1.Style = HeaderCellStyle;
                objCellRange47_1.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                #endregion

                intRowIndex = intRowIndex + 1;

                #region Second Row

                objExcelWorksheet2.Cells[intRowIndex,3].Value = "Amount Dr. ($)";
                //CellRange objCellRange12 = objExcelWorksheet2.Cells.GetSubrange("C" + Convert.ToString(intRowIndex + 1), "C" + Convert.ToString(intRowIndex + 1));
                //objCellRange12.Merged = true;
                objExcelWorksheet2.Cells[intRowIndex, 3].Style = HeaderCellStyle;
                objExcelWorksheet2.Cells[intRowIndex, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                objExcelWorksheet2.Cells[intRowIndex, 4].Value = "Amount Cr. ($)";
                //CellRange objCellRange13 = objExcelWorksheet2.Cells.GetSubrange("D" + Convert.ToString(intRowIndex + 1), "D" + Convert.ToString(intRowIndex + 1));
                //objCellRange13.Merged = true;
                objExcelWorksheet2.Cells[intRowIndex, 4].Style = HeaderCellStyle;
                objExcelWorksheet2.Cells[intRowIndex, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                objExcelWorksheet2.Cells[intRowIndex, 5].Value = "Amount Dr. ($)";
                //CellRange objCellRange12 = objExcelWorksheet2.Cells.GetSubrange("C" + Convert.ToString(intRowIndex + 1), "C" + Convert.ToString(intRowIndex + 1));
                //objCellRange12.Merged = true;
                objExcelWorksheet2.Cells[intRowIndex, 5].Style = HeaderCellStyle;
                objExcelWorksheet2.Cells[intRowIndex, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                objExcelWorksheet2.Cells[intRowIndex, 6].Value = "Amount Cr. ($)";
                //CellRange objCellRange13 = objExcelWorksheet2.Cells.GetSubrange("D" + Convert.ToString(intRowIndex + 1), "D" + Convert.ToString(intRowIndex + 1));
                //objCellRange13.Merged = true;
                objExcelWorksheet2.Cells[intRowIndex, 6].Style = HeaderCellStyle;
                objExcelWorksheet2.Cells[intRowIndex, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                objExcelWorksheet2.Cells[intRowIndex, 7].Value = "Amount Dr. ($)";
                //CellRange objCellRange12 = objExcelWorksheet2.Cells.GetSubrange("C" + Convert.ToString(intRowIndex + 1), "C" + Convert.ToString(intRowIndex + 1));
                //objCellRange12.Merged = true;
                objExcelWorksheet2.Cells[intRowIndex, 7].Style = HeaderCellStyle;
                objExcelWorksheet2.Cells[intRowIndex, 7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                objExcelWorksheet2.Cells[intRowIndex, 8].Value = "Amount Cr. ($)";
                //CellRange objCellRange13 = objExcelWorksheet2.Cells.GetSubrange("D" + Convert.ToString(intRowIndex + 1), "D" + Convert.ToString(intRowIndex + 1));
                //objCellRange13.Merged = true;
                objExcelWorksheet2.Cells[intRowIndex, 8].Style = HeaderCellStyle;
                objExcelWorksheet2.Cells[intRowIndex, 8].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                #endregion

                #endregion

                #region Create Details
                intRowIndex = intRowIndex + 1;
                dblGrandTotDr = 0;
                dblGrandTotCr = 0;
                dblGrandTotDrVRS = 0;
                dblGrandTotCrVRS = 0;
                dblGrandTotDrVar=0;
                dblGrandTotCrVar = 0;

                if (ds.Tables["AcctSummaryInv"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["AcctSummaryInv"].Rows)
                    {
                        for (int i = 1; i < intNoOfCols + 1; i++)
                        {
                            objExcelWorksheet2.Cells[intRowIndex, i].Style = DataCellStyle;
                        }

                        objExcelWorksheet2.Cells[intRowIndex, 1].Value = dr["gl_code"];
                        objExcelWorksheet2.Cells[intRowIndex, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                        objExcelWorksheet2.Cells[intRowIndex, 2].Value = dr["gl_desc"];
                        objExcelWorksheet2.Cells[intRowIndex, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                        objExcelWorksheet2.Cells[intRowIndex, 3].Value = dr["dr_amount_qb"];
                        objExcelWorksheet2.Cells[intRowIndex, 3].Style.NumberFormat = "#0.00";
                        objExcelWorksheet2.Cells[intRowIndex, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        objExcelWorksheet2.Cells[intRowIndex, 4].Value = dr["cr_amount_qb"];
                        objExcelWorksheet2.Cells[intRowIndex, 4].Style.NumberFormat = "#0.00";
                        objExcelWorksheet2.Cells[intRowIndex, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        objExcelWorksheet2.Cells[intRowIndex, 5].Value = dr["dr_amount_vrs"];
                        objExcelWorksheet2.Cells[intRowIndex, 5].Style.NumberFormat = "#0.00";
                        objExcelWorksheet2.Cells[intRowIndex, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        objExcelWorksheet2.Cells[intRowIndex, 6].Value = dr["cr_amount_vrs"];
                        objExcelWorksheet2.Cells[intRowIndex, 6].Style.NumberFormat = "#0.00";
                        objExcelWorksheet2.Cells[intRowIndex, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        objExcelWorksheet2.Cells[intRowIndex, 7].Value = dr["dr_amount_variance"];
                        objExcelWorksheet2.Cells[intRowIndex, 7].Style.NumberFormat = "#0.00";
                        objExcelWorksheet2.Cells[intRowIndex, 7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        objExcelWorksheet2.Cells[intRowIndex, 8].Value = dr["cr_amount_variance"];
                        objExcelWorksheet2.Cells[intRowIndex, 8].Style.NumberFormat = "#0.00";
                        objExcelWorksheet2.Cells[intRowIndex, 8].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        dblGrandTotDr = dblGrandTotDr + Convert.ToDouble(dr["dr_amount_qb"]);
                        dblGrandTotCr = dblGrandTotCr + Convert.ToDouble(dr["cr_amount_qb"]);
                        dblGrandTotDrVRS = dblGrandTotDrVRS + Convert.ToDouble(dr["dr_amount_vrs"]);
                        dblGrandTotCrVRS = dblGrandTotCrVRS + Convert.ToDouble(dr["cr_amount_vrs"]);
                        dblGrandTotDrVar = dblGrandTotDrVar + Convert.ToDouble(dr["dr_amount_variance"]);
                        dblGrandTotCrVar = dblGrandTotCrVar + Convert.ToDouble(dr["cr_amount_variance"]);

                        intRowIndex = intRowIndex + 1;
                    }

                    objExcelWorksheet2.Cells[intRowIndex, 1].Value = "Grand Total";
                    CellRange objCellRangeGT2 = objExcelWorksheet2.Cells.GetSubrangeAbsolute(intRowIndex, 1, intRowIndex, 2);
                    objCellRangeGT2.Merged = true;
                    objCellRangeGT2.Style = HeaderCellStyle;
                    objCellRangeGT2.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet2.Cells[intRowIndex, 3].Value = dblGrandTotDr;
                    objExcelWorksheet2.Cells[intRowIndex, 3].Style = HeaderCellStyle;
                    objExcelWorksheet2.Cells[intRowIndex, 3].Style.NumberFormat = "#0.00";
                    objExcelWorksheet2.Cells[intRowIndex, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet2.Cells[intRowIndex, 4].Value = dblGrandTotCr;
                    objExcelWorksheet2.Cells[intRowIndex, 4].Style = HeaderCellStyle;
                    objExcelWorksheet2.Cells[intRowIndex, 4].Style.NumberFormat = "#0.00";
                    objExcelWorksheet2.Cells[intRowIndex, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet2.Cells[intRowIndex, 5].Value = dblGrandTotDrVRS;
                    objExcelWorksheet2.Cells[intRowIndex, 5].Style = HeaderCellStyle;
                    objExcelWorksheet2.Cells[intRowIndex, 5].Style.NumberFormat = "#0.00";
                    objExcelWorksheet2.Cells[intRowIndex, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet2.Cells[intRowIndex, 6].Value = dblGrandTotCrVRS;
                    objExcelWorksheet2.Cells[intRowIndex, 6].Style = HeaderCellStyle;
                    objExcelWorksheet2.Cells[intRowIndex, 6].Style.NumberFormat = "#0.00";
                    objExcelWorksheet2.Cells[intRowIndex, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet2.Cells[intRowIndex, 7].Value = dblGrandTotDrVar;
                    objExcelWorksheet2.Cells[intRowIndex, 7].Style = HeaderCellStyle;
                    objExcelWorksheet2.Cells[intRowIndex, 7].Style.NumberFormat = "#0.00";
                    objExcelWorksheet2.Cells[intRowIndex, 7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet2.Cells[intRowIndex, 8].Value = dblGrandTotCrVar;
                    objExcelWorksheet2.Cells[intRowIndex, 8].Style = HeaderCellStyle;
                    objExcelWorksheet2.Cells[intRowIndex, 8].Style.NumberFormat = "#0.00";
                    objExcelWorksheet2.Cells[intRowIndex, 8].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                }
                #endregion

                #endregion

                intRowIndex = intRowIndex + 3;

                #region Payment/Refunds

                #region Create Title
                objExcelWorksheet2.Cells[intRowIndex, 1].Value = "Account Wise Summary For Payments/Refunds On " + dtDayEnd.ToString("MMMM dd, yyyy");
                CellRange objCellRangeT2_2 = objExcelWorksheet2.Cells.GetSubrangeAbsolute(intRowIndex, 1, intRowIndex, intNoOfCols);
                objCellRangeT2_2.Merged = true;
                objCellRangeT2_2.Style = HeaderCellStyle;
                objCellRangeT2_2.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                #endregion

                #region Create Header
                intRowIndex = intRowIndex + 1;

                #region First Row
                objExcelWorksheet2.Rows[intRowIndex].Height = 500;

                objExcelWorksheet2.Cells[intRowIndex, 1].Value = "G/L Code";
                CellRange objCellRange10_2 = objExcelWorksheet2.Cells.GetSubrange("B" + Convert.ToString(intRowIndex + 1), "B" + Convert.ToString(intRowIndex + 2));
                objCellRange10_2.Merged = true;
                objCellRange10_2.Style = HeaderCellStyle;
                objCellRange10_2.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                objExcelWorksheet2.Cells[intRowIndex, 2].Value = "G/L Description";
                CellRange objCellRange11_2 = objExcelWorksheet2.Cells.GetSubrange("C" + Convert.ToString(intRowIndex + 1), "C" + Convert.ToString(intRowIndex + 2));
                objCellRange11_2.Merged = true;
                objCellRange11_2.Style = HeaderCellStyle;
                objCellRange11_2.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                objExcelWorksheet2.Cells[intRowIndex, 3].Value = "As per Quick Books";
                CellRange objCellRange12_2 = objExcelWorksheet2.Cells.GetSubrange("D" + Convert.ToString(intRowIndex + 1), "E" + Convert.ToString(intRowIndex + 1));
                objCellRange12_2.Merged = true;
                objCellRange12_2.Style = HeaderCellStyle;
                objCellRange12_2.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                objExcelWorksheet2.Cells[intRowIndex, 5].Value = "As per VETRIS";
                CellRange objCellRange13_2 = objExcelWorksheet2.Cells.GetSubrange("F" + Convert.ToString(intRowIndex + 1), "G" + Convert.ToString(intRowIndex + 1));
                objCellRange13_2.Merged = true;
                objCellRange13_2.Style = HeaderCellStyle;
                objCellRange13_2.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                objExcelWorksheet2.Cells[intRowIndex, 7].Value = "Variance";
                CellRange objCellRange47_2 = objExcelWorksheet2.Cells.GetSubrange("H" + Convert.ToString(intRowIndex + 1), "I" + Convert.ToString(intRowIndex + 1));
                objCellRange47_2.Merged = true;
                objCellRange47_2.Style = HeaderCellStyle;
                objCellRange47_2.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                #endregion

                intRowIndex = intRowIndex + 1;

                #region Second Row

                objExcelWorksheet2.Cells[intRowIndex, 3].Value = "Amount Dr. ($)";
                objExcelWorksheet2.Cells[intRowIndex, 3].Style = HeaderCellStyle;
                objExcelWorksheet2.Cells[intRowIndex, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                objExcelWorksheet2.Cells[intRowIndex, 4].Value = "Amount Cr. ($)";
                objExcelWorksheet2.Cells[intRowIndex, 4].Style = HeaderCellStyle;
                objExcelWorksheet2.Cells[intRowIndex, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                objExcelWorksheet2.Cells[intRowIndex, 5].Value = "Amount Dr. ($)";
                objExcelWorksheet2.Cells[intRowIndex, 5].Style = HeaderCellStyle;
                objExcelWorksheet2.Cells[intRowIndex, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                objExcelWorksheet2.Cells[intRowIndex, 6].Value = "Amount Cr. ($)";
                objExcelWorksheet2.Cells[intRowIndex, 6].Style = HeaderCellStyle;
                objExcelWorksheet2.Cells[intRowIndex, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                objExcelWorksheet2.Cells[intRowIndex, 7].Value = "Amount Dr. ($)";
                objExcelWorksheet2.Cells[intRowIndex, 7].Style = HeaderCellStyle;
                objExcelWorksheet2.Cells[intRowIndex, 7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                objExcelWorksheet2.Cells[intRowIndex, 8].Value = "Amount Cr. ($)";
                objExcelWorksheet2.Cells[intRowIndex, 8].Style = HeaderCellStyle;
                objExcelWorksheet2.Cells[intRowIndex, 8].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                #endregion

                #endregion

                #region Create Details
                intRowIndex = intRowIndex + 1;
                dblGrandTotDr = 0;
                dblGrandTotCr = 0;
                dblGrandTotDrVRS = 0;
                dblGrandTotCrVRS = 0;
                dblGrandTotDrVar = 0;
                dblGrandTotCrVar = 0;

                if (ds.Tables["AcctSummaryPmt"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["AcctSummaryPmt"].Rows)
                    {
                        for (int i = 0; i < intNoOfCols; i++)
                        {
                            objExcelWorksheet2.Cells[intRowIndex, i].Style = DataCellStyle;
                        }

                        objExcelWorksheet2.Cells[intRowIndex, 1].Value = dr["gl_code"];
                        objExcelWorksheet2.Cells[intRowIndex, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                        objExcelWorksheet2.Cells[intRowIndex, 2].Value = dr["gl_desc"];
                        objExcelWorksheet2.Cells[intRowIndex, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                        objExcelWorksheet2.Cells[intRowIndex, 3].Value = dr["dr_amount_qb"];
                        objExcelWorksheet2.Cells[intRowIndex, 3].Style.NumberFormat = "#0.00";
                        objExcelWorksheet2.Cells[intRowIndex, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        objExcelWorksheet2.Cells[intRowIndex, 4].Value = dr["cr_amount_qb"];
                        objExcelWorksheet2.Cells[intRowIndex, 4].Style.NumberFormat = "#0.00";
                        objExcelWorksheet2.Cells[intRowIndex, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        objExcelWorksheet2.Cells[intRowIndex, 5].Value = dr["dr_amount_vrs"];
                        objExcelWorksheet2.Cells[intRowIndex, 5].Style.NumberFormat = "#0.00";
                        objExcelWorksheet2.Cells[intRowIndex, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        objExcelWorksheet2.Cells[intRowIndex, 6].Value = dr["cr_amount_vrs"];
                        objExcelWorksheet2.Cells[intRowIndex, 6].Style.NumberFormat = "#0.00";
                        objExcelWorksheet2.Cells[intRowIndex, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        objExcelWorksheet2.Cells[intRowIndex, 7].Value = dr["dr_amount_variance"];
                        objExcelWorksheet2.Cells[intRowIndex, 7].Style.NumberFormat = "#0.00";
                        objExcelWorksheet2.Cells[intRowIndex, 7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        objExcelWorksheet2.Cells[intRowIndex, 8].Value = dr["cr_amount_variance"];
                        objExcelWorksheet2.Cells[intRowIndex, 8].Style.NumberFormat = "#0.00";
                        objExcelWorksheet2.Cells[intRowIndex, 8].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        dblGrandTotDr = dblGrandTotDr + Convert.ToDouble(dr["dr_amount_qb"]);
                        dblGrandTotCr = dblGrandTotCr + Convert.ToDouble(dr["cr_amount_qb"]);
                        dblGrandTotDrVRS = dblGrandTotDrVRS + Convert.ToDouble(dr["dr_amount_vrs"]);
                        dblGrandTotCrVRS = dblGrandTotCrVRS + Convert.ToDouble(dr["cr_amount_vrs"]);
                        dblGrandTotDrVar = dblGrandTotDrVar + Convert.ToDouble(dr["dr_amount_variance"]);
                        dblGrandTotCrVar = dblGrandTotCrVar + Convert.ToDouble(dr["cr_amount_variance"]);

                        intRowIndex = intRowIndex + 1;
                    }

                    objExcelWorksheet2.Cells[intRowIndex, 1].Value = "Grand Total";
                    CellRange objCellRangeGT2 = objExcelWorksheet2.Cells.GetSubrangeAbsolute(intRowIndex, 1, intRowIndex, 2);
                    objCellRangeGT2.Merged = true;
                    objCellRangeGT2.Style = HeaderCellStyle;
                    objCellRangeGT2.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet2.Cells[intRowIndex, 3].Value = dblGrandTotDr;
                    objExcelWorksheet2.Cells[intRowIndex, 3].Style = HeaderCellStyle;
                    objExcelWorksheet2.Cells[intRowIndex, 3].Style.NumberFormat = "#0.00";
                    objExcelWorksheet2.Cells[intRowIndex, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet2.Cells[intRowIndex, 4].Value = dblGrandTotCr;
                    objExcelWorksheet2.Cells[intRowIndex, 4].Style = HeaderCellStyle;
                    objExcelWorksheet2.Cells[intRowIndex, 4].Style.NumberFormat = "#0.00";
                    objExcelWorksheet2.Cells[intRowIndex, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet2.Cells[intRowIndex, 5].Value = dblGrandTotDrVRS;
                    objExcelWorksheet2.Cells[intRowIndex, 5].Style = HeaderCellStyle;
                    objExcelWorksheet2.Cells[intRowIndex, 5].Style.NumberFormat = "#0.00";
                    objExcelWorksheet2.Cells[intRowIndex, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet2.Cells[intRowIndex, 6].Value = dblGrandTotCrVRS;
                    objExcelWorksheet2.Cells[intRowIndex, 6].Style = HeaderCellStyle;
                    objExcelWorksheet2.Cells[intRowIndex, 6].Style.NumberFormat = "#0.00";
                    objExcelWorksheet2.Cells[intRowIndex, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet2.Cells[intRowIndex, 7].Value = dblGrandTotDrVar;
                    objExcelWorksheet2.Cells[intRowIndex, 7].Style = HeaderCellStyle;
                    objExcelWorksheet2.Cells[intRowIndex, 7].Style.NumberFormat = "#0.00";
                    objExcelWorksheet2.Cells[intRowIndex, 7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet2.Cells[intRowIndex, 8].Value = dblGrandTotCrVar;
                    objExcelWorksheet2.Cells[intRowIndex, 8].Style = HeaderCellStyle;
                    objExcelWorksheet2.Cells[intRowIndex, 8].Style.NumberFormat = "#0.00";
                    objExcelWorksheet2.Cells[intRowIndex, 8].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                }
                #endregion

                #endregion

                #endregion

                #region Suspended
                //intRowIndex = 1;

                //#region Invoice Approved

                //#region Set Column Width
                //intNoOfCols = 4;
                //objExcelWorksheet3.Columns[0].Width = 6000;//Invoice #
                //objExcelWorksheet3.Columns[1].Width = 3000;//Invoice Date
                //objExcelWorksheet3.Columns[2].Width = 8000;//Billing Account
                //objExcelWorksheet3.Columns[3].Width = 3500;// Amount


                //for (int i = 0; i < intNoOfCols; i++)
                //{
                //    objExcelWorksheet3.Columns[i].Style.WrapText = true;
                //}
                //#endregion

                //#region Create Title
                //objExcelWorksheet3.Cells[intRowIndex, 0].Value = "Invoice Approved In VETRIS On " + dtDayEnd.ToString("MMMM dd, yyyy");
                //objExcelWorksheet3.Cells[intRowIndex, 0].Style = TitleCellStyle;
                //objExcelWorksheet3.Cells[intRowIndex, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                //CellRange objCellRangeT3 = objExcelWorksheet3.Cells.GetSubrangeAbsolute(intRowIndex, 0, intRowIndex, intNoOfCols - 1);
                //objCellRangeT3.Merged = true;
                //#endregion

                //#region Create Header
                //intRowIndex = intRowIndex + 2;
                //objExcelWorksheet3.Rows[intRowIndex].Height = 500;

                
                //objExcelWorksheet3.Cells[intRowIndex, 0].Value = "Invoice #";
                //CellRange objCellRange19 = objExcelWorksheet3.Cells.GetSubrange("A" + Convert.ToString(intRowIndex + 1), "A" + Convert.ToString(intRowIndex + 1));
                //objCellRange19.Merged = true;
                //objCellRange19.Style = HeaderCellStyle;
                //objCellRange19.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet3.Cells[intRowIndex, 1].Value = "Date";
                //CellRange objCellRange20 = objExcelWorksheet3.Cells.GetSubrange("B" + Convert.ToString(intRowIndex + 1), "B" + Convert.ToString(intRowIndex + 1));
                //objCellRange20.Merged = true;
                //objCellRange20.Style = HeaderCellStyle;
                //objCellRange20.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet3.Cells[intRowIndex, 2].Value = "Billing Account";
                //CellRange objCellRange21 = objExcelWorksheet3.Cells.GetSubrange("C" + Convert.ToString(intRowIndex + 1), "C" + Convert.ToString(intRowIndex + 1));
                //objCellRange21.Merged = true;
                //objCellRange21.Style = HeaderCellStyle;
                //objCellRange21.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet3.Cells[intRowIndex, 3].Value = "Amount ($)";
                //CellRange objCellRange22 = objExcelWorksheet3.Cells.GetSubrange("D" + Convert.ToString(intRowIndex + 1), "D" + Convert.ToString(intRowIndex + 1));
                //objCellRange22.Merged = true;
                //objCellRange22.Style = HeaderCellStyle;
                //objCellRange22.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                //#endregion

                //#region Create Details
                //intRowIndex = intRowIndex + 1;
                //dblGrandTot = 0;

                //if (ds.Tables["InvoiceApproved"].Rows.Count > 0)
                //{
                //    foreach (DataRow dr in ds.Tables["InvoiceApproved"].Rows)
                //    {
                //        for (int i = 0; i < intNoOfCols; i++)
                //        {
                //            objExcelWorksheet3.Cells[intRowIndex, i].Style = DataCellStyle;
                //        }

                        
                //        objExcelWorksheet3.Cells[intRowIndex, 0].Value = dr["invoice_no"];
                //        objExcelWorksheet3.Cells[intRowIndex, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet3.Cells[intRowIndex, 1].Value = dr["invoice_date"];
                //        objExcelWorksheet3.Cells[intRowIndex, 1].Style.NumberFormat = "MM-dd-yyyy";
                //        objExcelWorksheet3.Cells[intRowIndex, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet3.Cells[intRowIndex, 2].Value = dr["billing_account"];
                //        objExcelWorksheet3.Cells[intRowIndex, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet3.Cells[intRowIndex, 3].Value = dr["amount"];
                //        objExcelWorksheet3.Cells[intRowIndex, 3].Style.NumberFormat = "#0.00";
                //        objExcelWorksheet3.Cells[intRowIndex, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                //        dblGrandTot = dblGrandTot + Convert.ToDouble(dr["amount"]);


                //        intRowIndex = intRowIndex + 1;
                //    }

                //    objExcelWorksheet3.Cells[intRowIndex, 0].Value = "Grand Total";
                //    CellRange objCellRangeGT4 = objExcelWorksheet3.Cells.GetSubrangeAbsolute(intRowIndex, 0, intRowIndex, 2);
                //    objCellRangeGT4.Merged = true;
                //    objCellRangeGT4.Style = HeaderCellStyle;
                //    objCellRangeGT4.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                //    objExcelWorksheet3.Cells[intRowIndex, 3].Value = dblGrandTot;
                //    objExcelWorksheet3.Cells[intRowIndex, 3].Style = HeaderCellStyle;
                //    objExcelWorksheet3.Cells[intRowIndex, 3].Style.NumberFormat = "#0.00";
                //    objExcelWorksheet3.Cells[intRowIndex, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                //}
                //#endregion

                //#endregion

                //intRowIndex = 1;

                //#region Invoice Disapproved

                //#region Set Column Width
                //intNoOfCols = 4;
                //objExcelWorksheet4.Columns[0].Width = 6000;//Invoice #
                //objExcelWorksheet4.Columns[1].Width = 3000;//Invoice Date
                //objExcelWorksheet4.Columns[2].Width = 8000;//Billing Account
                //objExcelWorksheet4.Columns[3].Width = 3500;// Amount


                //for (int i = 0; i < intNoOfCols; i++)
                //{
                //    objExcelWorksheet4.Columns[i].Style.WrapText = true;
                //}
                //#endregion

                //#region Create Title
                //objExcelWorksheet4.Cells[intRowIndex, 0].Value = "Invoice Disapproved In VETRIS On " + dtDayEnd.ToString("MMMM dd, yyyy");
                //objExcelWorksheet4.Cells[intRowIndex, 0].Style = TitleCellStyle;
                //objExcelWorksheet4.Cells[intRowIndex, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                //CellRange objCellRangeT4 = objExcelWorksheet4.Cells.GetSubrangeAbsolute(intRowIndex, 0, intRowIndex, intNoOfCols - 1);
                //objCellRangeT4.Merged = true;
                //#endregion

                //#region Create Header
                //intRowIndex = intRowIndex + 2;
                //objExcelWorksheet4.Rows[intRowIndex].Height = 500;


                //objExcelWorksheet4.Cells[intRowIndex, 0].Value = "Invoice #";
                //CellRange objCellRange23 = objExcelWorksheet4.Cells.GetSubrange("A" + Convert.ToString(intRowIndex + 1), "A" + Convert.ToString(intRowIndex + 1));
                //objCellRange23.Merged = true;
                //objCellRange23.Style = HeaderCellStyle;
                //objCellRange23.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet4.Cells[intRowIndex, 1].Value = "Date";
                //CellRange objCellRange24 = objExcelWorksheet4.Cells.GetSubrange("B" + Convert.ToString(intRowIndex + 1), "B" + Convert.ToString(intRowIndex + 1));
                //objCellRange24.Merged = true;
                //objCellRange24.Style = HeaderCellStyle;
                //objCellRange24.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet4.Cells[intRowIndex, 2].Value = "Billing Account";
                //CellRange objCellRange25 = objExcelWorksheet4.Cells.GetSubrange("C" + Convert.ToString(intRowIndex + 1), "C" + Convert.ToString(intRowIndex + 1));
                //objCellRange25.Merged = true;
                //objCellRange25.Style = HeaderCellStyle;
                //objCellRange25.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet4.Cells[intRowIndex, 3].Value = "Amount ($)";
                //CellRange objCellRange26 = objExcelWorksheet4.Cells.GetSubrange("D" + Convert.ToString(intRowIndex + 1), "D" + Convert.ToString(intRowIndex + 1));
                //objCellRange26.Merged = true;
                //objCellRange26.Style = HeaderCellStyle;
                //objCellRange26.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                //#endregion

                //#region Create Details
                //intRowIndex = intRowIndex + 1;
                //dblGrandTot = 0;

                //if (ds.Tables["InvoiceDispproved"].Rows.Count > 0)
                //{
                //    foreach (DataRow dr in ds.Tables["InvoiceDispproved"].Rows)
                //    {
                //        for (int i = 0; i < intNoOfCols; i++)
                //        {
                //            objExcelWorksheet4.Cells[intRowIndex, i].Style = DataCellStyle;
                //        }


                //        objExcelWorksheet4.Cells[intRowIndex, 0].Value = dr["invoice_no"];
                //        objExcelWorksheet4.Cells[intRowIndex, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet4.Cells[intRowIndex, 1].Value = dr["invoice_date"];
                //        objExcelWorksheet4.Cells[intRowIndex, 1].Style.NumberFormat = "MM-dd-yyyy";
                //        objExcelWorksheet4.Cells[intRowIndex, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet4.Cells[intRowIndex, 2].Value = dr["billing_account"];
                //        objExcelWorksheet4.Cells[intRowIndex, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet4.Cells[intRowIndex, 3].Value = dr["amount"];
                //        objExcelWorksheet4.Cells[intRowIndex, 3].Style.NumberFormat = "#0.00";
                //        objExcelWorksheet4.Cells[intRowIndex, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                //        dblGrandTot = dblGrandTot + Convert.ToDouble(dr["amount"]);


                //        intRowIndex = intRowIndex + 1;
                //    }

                //    objExcelWorksheet4.Cells[intRowIndex, 0].Value = "Grand Total";
                //    CellRange objCellRangeGT5 = objExcelWorksheet4.Cells.GetSubrangeAbsolute(intRowIndex, 0, intRowIndex, 2);
                //    objCellRangeGT5.Merged = true;
                //    objCellRangeGT5.Style = HeaderCellStyle;
                //    objCellRangeGT5.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                //    objExcelWorksheet4.Cells[intRowIndex, 3].Value = dblGrandTot;
                //    objExcelWorksheet4.Cells[intRowIndex, 3].Style = HeaderCellStyle;
                //    objExcelWorksheet4.Cells[intRowIndex, 3].Style.NumberFormat = "#0.00";
                //    objExcelWorksheet4.Cells[intRowIndex, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                //}
                //#endregion

                //#endregion

                //intRowIndex = 1;

                //#region Payments Received

                //#region Set Column Width
                //intNoOfCols = 8;
                //objExcelWorksheet5.Columns[0].Width = 6000;//Ref #
                //objExcelWorksheet5.Columns[1].Width = 3000;//Ref Date
                //objExcelWorksheet5.Columns[2].Width = 3000;//Mode
                //objExcelWorksheet5.Columns[3].Width = 6500;//Processing Ref #
                //objExcelWorksheet5.Columns[4].Width = 5000;//Processing Ref Date
                //objExcelWorksheet5.Columns[5].Width = 4000;//Payment Gateway
                //objExcelWorksheet5.Columns[6].Width = 8000;//Billing Account
                //objExcelWorksheet5.Columns[7].Width = 3500;// Amount


                //for (int i = 0; i < intNoOfCols; i++)
                //{
                //    objExcelWorksheet5.Columns[i].Style.WrapText = true;
                //}
                //#endregion

                //#region Create Title
                //objExcelWorksheet5.Cells[intRowIndex, 0].Value = "Payment(s) Received In VETRIS On " + dtDayEnd.ToString("MMMM dd, yyyy");
                //objExcelWorksheet5.Cells[intRowIndex, 0].Style = TitleCellStyle;
                //objExcelWorksheet5.Cells[intRowIndex, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                //CellRange objCellRangeT5 = objExcelWorksheet5.Cells.GetSubrangeAbsolute(intRowIndex, 0, intRowIndex, intNoOfCols - 1);
                //objCellRangeT5.Merged = true;
                //#endregion

                //#region Create Header
                //intRowIndex = intRowIndex + 2;
                //objExcelWorksheet5.Rows[intRowIndex].Height = 500;

                //objExcelWorksheet5.Cells[intRowIndex, 0].Value = "Reference #";
                //CellRange objCellRange27 = objExcelWorksheet5.Cells.GetSubrange("A" + Convert.ToString(intRowIndex + 1), "A" + Convert.ToString(intRowIndex + 1));
                //objCellRange27.Merged = true;
                //objCellRange27.Style = HeaderCellStyle;
                //objCellRange27.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet5.Cells[intRowIndex, 1].Value = "Date";
                //CellRange objCellRange28 = objExcelWorksheet5.Cells.GetSubrange("B" + Convert.ToString(intRowIndex + 1), "B" + Convert.ToString(intRowIndex + 1));
                //objCellRange28.Merged = true;
                //objCellRange28.Style = HeaderCellStyle;
                //objCellRange28.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet5.Cells[intRowIndex, 2].Value = "Mode";
                //CellRange objCellRange29 = objExcelWorksheet5.Cells.GetSubrange("C" + Convert.ToString(intRowIndex + 1), "C" + Convert.ToString(intRowIndex + 1));
                //objCellRange29.Merged = true;
                //objCellRange29.Style = HeaderCellStyle;
                //objCellRange29.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet5.Cells[intRowIndex, 3].Value = "Processing Ref. #";
                //CellRange objCellRange30 = objExcelWorksheet5.Cells.GetSubrange("D" + Convert.ToString(intRowIndex + 1), "D" + Convert.ToString(intRowIndex + 1));
                //objCellRange30.Merged = true;
                //objCellRange30.Style = HeaderCellStyle;
                //objCellRange30.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet5.Cells[intRowIndex, 4].Value = "Processing Ref. Date";
                //CellRange objCellRange31 = objExcelWorksheet5.Cells.GetSubrange("E" + Convert.ToString(intRowIndex + 1), "E" + Convert.ToString(intRowIndex + 1));
                //objCellRange31.Merged = true;
                //objCellRange31.Style = HeaderCellStyle;
                //objCellRange31.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet5.Cells[intRowIndex, 5].Value = "Payment Gateway";
                //CellRange objCellRange32 = objExcelWorksheet5.Cells.GetSubrange("F" + Convert.ToString(intRowIndex + 1), "F" + Convert.ToString(intRowIndex + 1));
                //objCellRange32.Merged = true;
                //objCellRange32.Style = HeaderCellStyle;
                //objCellRange32.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet5.Cells[intRowIndex, 6].Value = "Billing Account";
                //CellRange objCellRange33 = objExcelWorksheet5.Cells.GetSubrange("G" + Convert.ToString(intRowIndex + 1), "G" + Convert.ToString(intRowIndex + 1));
                //objCellRange33.Merged = true;
                //objCellRange33.Style = HeaderCellStyle;
                //objCellRange33.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet5.Cells[intRowIndex, 7].Value = "Amount ($)";
                //CellRange objCellRange34 = objExcelWorksheet5.Cells.GetSubrange("H" + Convert.ToString(intRowIndex + 1), "H" + Convert.ToString(intRowIndex + 1));
                //objCellRange34.Merged = true;
                //objCellRange34.Style = HeaderCellStyle;
                //objCellRange34.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                //#endregion

                //#region Create Details
                //intRowIndex = intRowIndex + 1;
                //dblGrandTot = 0;

                //if (ds.Tables["PaymentReceived"].Rows.Count > 0)
                //{
                //    foreach (DataRow dr in ds.Tables["PaymentReceived"].Rows)
                //    {
                //        for (int i = 0; i < intNoOfCols; i++)
                //        {
                //            objExcelWorksheet5.Cells[intRowIndex, i].Style = DataCellStyle;
                //        }


                //        objExcelWorksheet5.Cells[intRowIndex, 0].Value = dr["payref_no"];
                //        objExcelWorksheet5.Cells[intRowIndex, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet5.Cells[intRowIndex, 1].Value = dr["payref_date"];
                //        objExcelWorksheet5.Cells[intRowIndex, 1].Style.NumberFormat = "MM-dd-yyyy";
                //        objExcelWorksheet5.Cells[intRowIndex, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet5.Cells[intRowIndex, 2].Value = dr["payment_mode"];
                //        objExcelWorksheet5.Cells[intRowIndex, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet5.Cells[intRowIndex, 3].Value = dr["processing_ref_no"];
                //        objExcelWorksheet5.Cells[intRowIndex, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet5.Cells[intRowIndex, 4].Value = dr["processing_ref_date"];
                //        objExcelWorksheet5.Cells[intRowIndex, 4].Style.NumberFormat = "MM-dd-yyyy HH:mm";
                //        objExcelWorksheet5.Cells[intRowIndex, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet5.Cells[intRowIndex, 5].Value = dr["payment_gateway"];
                //        objExcelWorksheet5.Cells[intRowIndex, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet5.Cells[intRowIndex, 6].Value = dr["billing_account"];
                //        objExcelWorksheet5.Cells[intRowIndex, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet5.Cells[intRowIndex, 7].Value = dr["payment_amount"];
                //        objExcelWorksheet5.Cells[intRowIndex, 7].Style.NumberFormat = "#0.00";
                //        objExcelWorksheet5.Cells[intRowIndex, 7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                //        dblGrandTot = dblGrandTot + Convert.ToDouble(dr["payment_amount"]);


                //        intRowIndex = intRowIndex + 1;
                //    }

                //    objExcelWorksheet5.Cells[intRowIndex, 0].Value = "Grand Total";
                //    CellRange objCellRangeGT5 = objExcelWorksheet5.Cells.GetSubrangeAbsolute(intRowIndex, 0, intRowIndex, 6);
                //    objCellRangeGT5.Merged = true;
                //    objCellRangeGT5.Style = HeaderCellStyle;
                //    objCellRangeGT5.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                //    objExcelWorksheet5.Cells[intRowIndex, 7].Value = dblGrandTot;
                //    objExcelWorksheet5.Cells[intRowIndex, 7].Style = HeaderCellStyle;
                //    objExcelWorksheet5.Cells[intRowIndex, 7].Style.NumberFormat = "#0.00";
                //    objExcelWorksheet5.Cells[intRowIndex, 7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                //}
                //#endregion

                //#endregion

                //intRowIndex = 1;

                //#region Payments Refunded

                //#region Set Column Width
                //intNoOfCols = 8;
                //objExcelWorksheet6.Columns[0].Width = 6000;//Ref #
                //objExcelWorksheet6.Columns[1].Width = 3000;//Ref Date
                //objExcelWorksheet6.Columns[2].Width = 3000;//Mode
                //objExcelWorksheet6.Columns[3].Width = 6500;//Processing Ref #
                //objExcelWorksheet6.Columns[4].Width = 5000;//Processing Ref Date
                //objExcelWorksheet6.Columns[5].Width = 4000;//Payment Gateway
                //objExcelWorksheet6.Columns[6].Width = 8000;//Billing Account
                //objExcelWorksheet6.Columns[7].Width = 3500;// Amount


                //for (int i = 0; i < intNoOfCols; i++)
                //{
                //    objExcelWorksheet6.Columns[i].Style.WrapText = true;
                //}
                //#endregion

                //#region Create Title
                //objExcelWorksheet6.Cells[intRowIndex, 0].Value = "Payment(s) Refunded In VETRIS On " + dtDayEnd.ToString("MMMM dd, yyyy");
                //objExcelWorksheet6.Cells[intRowIndex, 0].Style = TitleCellStyle;
                //objExcelWorksheet6.Cells[intRowIndex, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                //CellRange objCellRangeT6 = objExcelWorksheet6.Cells.GetSubrangeAbsolute(intRowIndex, 0, intRowIndex, intNoOfCols - 1);
                //objCellRangeT6.Merged = true;
                //#endregion

                //#region Create Header
                //intRowIndex = intRowIndex + 2;
                //objExcelWorksheet6.Rows[intRowIndex].Height = 500;

                //objExcelWorksheet6.Cells[intRowIndex, 0].Value = "Reference #";
                //CellRange objCellRange35 = objExcelWorksheet6.Cells.GetSubrange("A" + Convert.ToString(intRowIndex + 1), "A" + Convert.ToString(intRowIndex + 1));
                //objCellRange35.Merged = true;
                //objCellRange35.Style = HeaderCellStyle;
                //objCellRange35.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet6.Cells[intRowIndex, 1].Value = "Date";
                //CellRange objCellRange36 = objExcelWorksheet6.Cells.GetSubrange("B" + Convert.ToString(intRowIndex + 1), "B" + Convert.ToString(intRowIndex + 1));
                //objCellRange36.Merged = true;
                //objCellRange36.Style = HeaderCellStyle;
                //objCellRange36.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet6.Cells[intRowIndex, 2].Value = "Mode";
                //CellRange objCellRange37 = objExcelWorksheet6.Cells.GetSubrange("C" + Convert.ToString(intRowIndex + 1), "C" + Convert.ToString(intRowIndex + 1));
                //objCellRange37.Merged = true;
                //objCellRange37.Style = HeaderCellStyle;
                //objCellRange37.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet6.Cells[intRowIndex, 3].Value = "Processing Ref. #";
                //CellRange objCellRange38 = objExcelWorksheet6.Cells.GetSubrange("D" + Convert.ToString(intRowIndex + 1), "D" + Convert.ToString(intRowIndex + 1));
                //objCellRange38.Merged = true;
                //objCellRange38.Style = HeaderCellStyle;
                //objCellRange38.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet6.Cells[intRowIndex, 4].Value = "Processing Ref. Date";
                //CellRange objCellRange39 = objExcelWorksheet6.Cells.GetSubrange("E" + Convert.ToString(intRowIndex + 1), "E" + Convert.ToString(intRowIndex + 1));
                //objCellRange39.Merged = true;
                //objCellRange39.Style = HeaderCellStyle;
                //objCellRange39.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet6.Cells[intRowIndex, 5].Value = "Payment Gateway";
                //CellRange objCellRange40 = objExcelWorksheet6.Cells.GetSubrange("F" + Convert.ToString(intRowIndex + 1), "F" + Convert.ToString(intRowIndex + 1));
                //objCellRange40.Merged = true;
                //objCellRange40.Style = HeaderCellStyle;
                //objCellRange40.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet6.Cells[intRowIndex, 6].Value = "Billing Account";
                //CellRange objCellRange41 = objExcelWorksheet6.Cells.GetSubrange("G" + Convert.ToString(intRowIndex + 1), "G" + Convert.ToString(intRowIndex + 1));
                //objCellRange41.Merged = true;
                //objCellRange41.Style = HeaderCellStyle;
                //objCellRange41.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet6.Cells[intRowIndex, 7].Value = "Amount ($)";
                //CellRange objCellRange42 = objExcelWorksheet6.Cells.GetSubrange("H" + Convert.ToString(intRowIndex + 1), "H" + Convert.ToString(intRowIndex + 1));
                //objCellRange42.Merged = true;
                //objCellRange42.Style = HeaderCellStyle;
                //objCellRange42.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                //#endregion

                //#region Create Details
                //intRowIndex = intRowIndex + 1;
                //dblGrandTot = 0;

                //if (ds.Tables["PaymentRefund"].Rows.Count > 0)
                //{
                //    foreach (DataRow dr in ds.Tables["PaymentRefund"].Rows)
                //    {
                //        for (int i = 0; i < intNoOfCols; i++)
                //        {
                //            objExcelWorksheet6.Cells[intRowIndex, i].Style = DataCellStyle;
                //        }


                //        objExcelWorksheet6.Cells[intRowIndex, 0].Value = dr["refundref_no"];
                //        objExcelWorksheet6.Cells[intRowIndex, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet6.Cells[intRowIndex, 1].Value = dr["refundref_date"];
                //        objExcelWorksheet6.Cells[intRowIndex, 1].Style.NumberFormat = "MM-dd-yyyy";
                //        objExcelWorksheet6.Cells[intRowIndex, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet6.Cells[intRowIndex, 2].Value = dr["payment_mode"];
                //        objExcelWorksheet6.Cells[intRowIndex, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet6.Cells[intRowIndex, 3].Value = dr["processing_ref_no"];
                //        objExcelWorksheet6.Cells[intRowIndex, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet6.Cells[intRowIndex, 4].Value = dr["processing_ref_date"];
                //        objExcelWorksheet6.Cells[intRowIndex, 4].Style.NumberFormat = "MM-dd-yyyy HH:mm";
                //        objExcelWorksheet6.Cells[intRowIndex, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet6.Cells[intRowIndex, 5].Value = dr["payment_gateway"];
                //        objExcelWorksheet6.Cells[intRowIndex, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet6.Cells[intRowIndex, 6].Value = dr["billing_account"];
                //        objExcelWorksheet6.Cells[intRowIndex, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet6.Cells[intRowIndex, 7].Value = dr["refund_amount"];
                //        objExcelWorksheet6.Cells[intRowIndex, 7].Style.NumberFormat = "#0.00";
                //        objExcelWorksheet6.Cells[intRowIndex, 7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                //        dblGrandTot = dblGrandTot + Convert.ToDouble(dr["refund_amount"]);


                //        intRowIndex = intRowIndex + 1;
                //    }

                //    objExcelWorksheet6.Cells[intRowIndex, 0].Value = "Grand Total";
                //    CellRange objCellRangeGT6 = objExcelWorksheet6.Cells.GetSubrangeAbsolute(intRowIndex, 0, intRowIndex, 6);
                //    objCellRangeGT6.Merged = true;
                //    objCellRangeGT6.Style = HeaderCellStyle;
                //    objCellRangeGT6.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                //    objExcelWorksheet6.Cells[intRowIndex, 7].Value = dblGrandTot;
                //    objExcelWorksheet6.Cells[intRowIndex, 7].Style = HeaderCellStyle;
                //    objExcelWorksheet6.Cells[intRowIndex, 7].Style.NumberFormat = "#0.00";
                //    objExcelWorksheet6.Cells[intRowIndex, 7].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                //}
                //#endregion

                //#endregion

                //intRowIndex = 1;

                //#region Payments Made

                //#region Set Column Width
                //intNoOfCols = 5;
                //objExcelWorksheet7.Columns[0].Width = 6000;//Ref #
                //objExcelWorksheet7.Columns[1].Width = 3000;//Ref Date
                //objExcelWorksheet7.Columns[2].Width = 3000;//Payee Type
                //objExcelWorksheet7.Columns[3].Width = 8000;//Payee Name
                //objExcelWorksheet7.Columns[4].Width = 3500;// Amount

                //for (int i = 0; i < intNoOfCols; i++)
                //{
                //    objExcelWorksheet7.Columns[i].Style.WrapText = true;
                //}
                //#endregion

                //#region Create Title
                //objExcelWorksheet7.Cells[intRowIndex, 0].Value = "Payment(s) Made In VETRIS On " + dtDayEnd.ToString("MMMM dd, yyyy");
                //objExcelWorksheet7.Cells[intRowIndex, 0].Style = TitleCellStyle;
                //objExcelWorksheet7.Cells[intRowIndex, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                //CellRange objCellRangeT7 = objExcelWorksheet7.Cells.GetSubrangeAbsolute(intRowIndex, 0, intRowIndex, intNoOfCols - 1);
                //objCellRangeT7.Merged = true;
                //#endregion

                //#region Create Header
                //intRowIndex = intRowIndex + 2;
                //objExcelWorksheet7.Rows[intRowIndex].Height = 500;

                //objExcelWorksheet7.Cells[intRowIndex, 0].Value = "Reference #";
                //CellRange objCellRange43 = objExcelWorksheet7.Cells.GetSubrange("A" + Convert.ToString(intRowIndex + 1), "A" + Convert.ToString(intRowIndex + 1));
                //objCellRange43.Merged = true;
                //objCellRange43.Style = HeaderCellStyle;
                //objCellRange43.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet7.Cells[intRowIndex, 1].Value = "Date";
                //CellRange objCellRange44 = objExcelWorksheet7.Cells.GetSubrange("B" + Convert.ToString(intRowIndex + 1), "B" + Convert.ToString(intRowIndex + 1));
                //objCellRange44.Merged = true;
                //objCellRange44.Style = HeaderCellStyle;
                //objCellRange44.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet7.Cells[intRowIndex, 2].Value = "Payee Type";
                //CellRange objCellRange45 = objExcelWorksheet7.Cells.GetSubrange("C" + Convert.ToString(intRowIndex + 1), "C" + Convert.ToString(intRowIndex + 1));
                //objCellRange45.Merged = true;
                //objCellRange45.Style = HeaderCellStyle;
                //objCellRange45.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //objExcelWorksheet7.Cells[intRowIndex, 3].Value = "Payee Name";
                //CellRange objCellRange46 = objExcelWorksheet7.Cells.GetSubrange("D" + Convert.ToString(intRowIndex + 1), "D" + Convert.ToString(intRowIndex + 1));
                //objCellRange46.Merged = true;
                //objCellRange46.Style = HeaderCellStyle;
                //objCellRange46.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //#endregion

                //#region Create Details
                //intRowIndex = intRowIndex + 1;
                //dblGrandTot = 0;

                //if (ds.Tables["PaymentMade"].Rows.Count > 0)
                //{
                //    foreach (DataRow dr in ds.Tables["PaymentMade"].Rows)
                //    {
                //        for (int i = 0; i < intNoOfCols; i++)
                //        {
                //            objExcelWorksheet7.Cells[intRowIndex, i].Style = DataCellStyle;
                //        }


                //        objExcelWorksheet7.Cells[intRowIndex, 0].Value = dr["ref_no"];
                //        objExcelWorksheet7.Cells[intRowIndex, 0].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet7.Cells[intRowIndex, 1].Value = dr["ref_date"];
                //        objExcelWorksheet7.Cells[intRowIndex, 1].Style.NumberFormat = "MM-dd-yyyy";
                //        objExcelWorksheet7.Cells[intRowIndex, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet7.Cells[intRowIndex, 2].Value = dr["ref_type"];
                //        objExcelWorksheet7.Cells[intRowIndex, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet7.Cells[intRowIndex, 3].Value = dr["dr_cr_name"];
                //        objExcelWorksheet7.Cells[intRowIndex, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                //        objExcelWorksheet7.Cells[intRowIndex, 4].Value = dr["amount"];
                //        objExcelWorksheet7.Cells[intRowIndex, 4].Style.NumberFormat = "#0.00";
                //        objExcelWorksheet7.Cells[intRowIndex, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                //        dblGrandTot = dblGrandTot + Convert.ToDouble(dr["amount"]);


                //        intRowIndex = intRowIndex + 1;
                //    }

                //    objExcelWorksheet7.Cells[intRowIndex, 0].Value = "Grand Total";
                //    CellRange objCellRangeGT7 = objExcelWorksheet7.Cells.GetSubrangeAbsolute(intRowIndex, 0, intRowIndex, 3);
                //    objCellRangeGT7.Merged = true;
                //    objCellRangeGT7.Style = HeaderCellStyle;
                //    objCellRangeGT7.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                //    objExcelWorksheet7.Cells[intRowIndex, 4].Value = dblGrandTot;
                //    objExcelWorksheet7.Cells[intRowIndex, 4].Style = HeaderCellStyle;
                //    objExcelWorksheet7.Cells[intRowIndex, 4].Style.NumberFormat = "#0.00";
                //    objExcelWorksheet7.Cells[intRowIndex, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                //}
                //#endregion

                //#endregion

                #endregion

                intRowIndex = 1;

                #region Posting Failures
                #region Set Column Width
                intNoOfCols = 5;
                objExcelWorksheet8.Columns[1].Width = 6000;//Voucher Type
                objExcelWorksheet8.Columns[2].Width = 6000;//VETRIS Reference #
                objExcelWorksheet8.Columns[3].Width = 3000;//Ref. Date
                objExcelWorksheet8.Columns[4].Width = 8000;//Debtor/Creditor
                objExcelWorksheet8.Columns[5].Width = 3500;// Amount


                for (int i = 1; i < intNoOfCols + 1; i++)
                {
                    objExcelWorksheet8.Columns[i].Style.WrapText = true;
                }
                #endregion

                #region Create Title
                objExcelWorksheet8.Cells[intRowIndex, 1].Value = "Posting Failure From VETRIS As On " + dtDayEnd.ToString("MMMM dd, yyyy");
                objExcelWorksheet8.Cells[intRowIndex, 1].Style = TitleCellStyle;
                objExcelWorksheet8.Cells[intRowIndex, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                CellRange objCellRangeT8 = objExcelWorksheet8.Cells.GetSubrangeAbsolute(intRowIndex, 1, intRowIndex, intNoOfCols);
                objCellRangeT8.Merged = true;
                #endregion

                #region Create Header
                intRowIndex = intRowIndex + 2;
                objExcelWorksheet8.Rows[intRowIndex].Height = 500;

                objExcelWorksheet8.Cells[intRowIndex, 1].Value = "Voucher Type";
                CellRange objCellRange14 = objExcelWorksheet8.Cells.GetSubrange("B" + Convert.ToString(intRowIndex + 1), "B" + Convert.ToString(intRowIndex + 1));
                objCellRange14.Merged = true;
                objCellRange14.Style = HeaderCellStyle;
                objCellRange14.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                objExcelWorksheet8.Cells[intRowIndex, 2].Value = "Reference #";
                CellRange objCellRange15 = objExcelWorksheet8.Cells.GetSubrange("C" + Convert.ToString(intRowIndex + 1), "C" + Convert.ToString(intRowIndex + 1));
                objCellRange15.Merged = true;
                objCellRange15.Style = HeaderCellStyle;
                objCellRange15.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                objExcelWorksheet8.Cells[intRowIndex, 3].Value = "Date";
                CellRange objCellRange16 = objExcelWorksheet8.Cells.GetSubrange("D" + Convert.ToString(intRowIndex + 1), "D" + Convert.ToString(intRowIndex + 1));
                objCellRange16.Merged = true;
                objCellRange16.Style = HeaderCellStyle;
                objCellRange16.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                objExcelWorksheet8.Cells[intRowIndex, 4].Value = "Debtor/Creditor";
                CellRange objCellRange18 = objExcelWorksheet8.Cells.GetSubrange("E" + Convert.ToString(intRowIndex + 1), "E" + Convert.ToString(intRowIndex + 1));
                objCellRange18.Merged = true;
                objCellRange18.Style = HeaderCellStyle;
                objCellRange18.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                objExcelWorksheet8.Cells[intRowIndex, 5].Value = "Amount ($)";
                CellRange objCellRange17 = objExcelWorksheet8.Cells.GetSubrange("F" + Convert.ToString(intRowIndex + 1), "F" + Convert.ToString(intRowIndex + 1));
                objCellRange17.Merged = true;
                objCellRange17.Style = HeaderCellStyle;
                objCellRange17.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                #endregion

                #region Create Details
                intRowIndex = intRowIndex + 1;
                dblGrandTot = 0;

                if (ds.Tables["PostFailed"].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables["PostFailed"].Rows)
                    {
                        for (int i = 1; i < intNoOfCols + 1; i++)
                        {
                            objExcelWorksheet8.Cells[intRowIndex, i].Style = DataCellStyle;
                        }

                        objExcelWorksheet8.Cells[intRowIndex, 1].Value = dr["ref_type"];
                        objExcelWorksheet8.Cells[intRowIndex, 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                        objExcelWorksheet8.Cells[intRowIndex, 2].Value = dr["ref_no"];
                        objExcelWorksheet8.Cells[intRowIndex, 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                        objExcelWorksheet8.Cells[intRowIndex, 3].Value = dr["ref_date"];
                        objExcelWorksheet8.Cells[intRowIndex, 3].Style.NumberFormat = "MM-dd-yyyy";
                        objExcelWorksheet8.Cells[intRowIndex, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                        objExcelWorksheet8.Cells[intRowIndex, 4].Value = dr["dr_cr_name"];
                        objExcelWorksheet8.Cells[intRowIndex, 4].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;

                        objExcelWorksheet8.Cells[intRowIndex, 5].Value = dr["amount"];
                        objExcelWorksheet8.Cells[intRowIndex, 5].Style.NumberFormat = "#0.00";
                        objExcelWorksheet8.Cells[intRowIndex, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        dblGrandTot = dblGrandTot + Convert.ToDouble(dr["amount"]);


                        intRowIndex = intRowIndex + 1;
                    }

                    objExcelWorksheet8.Cells[intRowIndex, 1].Value = "Grand Total";
                    CellRange objCellRangeGT3 = objExcelWorksheet8.Cells.GetSubrangeAbsolute(intRowIndex, 1, intRowIndex, 4);
                    objCellRangeGT3.Merged = true;
                    objCellRangeGT3.Style = HeaderCellStyle;
                    objCellRangeGT3.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    objExcelWorksheet8.Cells[intRowIndex, 5].Value = dblGrandTot;
                    objExcelWorksheet8.Cells[intRowIndex, 5].Style = HeaderCellStyle;
                    objExcelWorksheet8.Cells[intRowIndex, 5].Style.NumberFormat = "#0.00";
                    objExcelWorksheet8.Cells[intRowIndex, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                }
                #endregion

                #endregion

                objExcelFile.SaveXlsx(strReportName);
                strReportName = strRName;
                bRet = true;

            }
            catch (Exception expErr)
            {
                bRet = false;
                strReportName = "";
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetDayEndReportData()==>GenerateDayEndReport()==>CreateDayEndReport()- Exception : " + expErr.Message, true);
            }
            finally
            {
                if (objExcelFile != null) objExcelFile = null;
                if (objExcelWorksheet1 != null) objExcelWorksheet1 = null;
                if (objExcelWorksheet2 != null) objExcelWorksheet2 = null;
                //if (objExcelWorksheet3 != null) objExcelWorksheet3 = null;
                //if (objExcelWorksheet4 != null) objExcelWorksheet4 = null;
                //if (objExcelWorksheet5 != null) objExcelWorksheet5 = null;
                //if (objExcelWorksheet6 != null) objExcelWorksheet6 = null;
                //if (objExcelWorksheet7 != null) objExcelWorksheet7 = null;
                if (objExcelWorksheet8 != null) objExcelWorksheet8 = null;
            }

            return bRet;
        }
        #endregion

        #region GenerateDayEndMail
        private void GenerateDayEndMail(string strRptFileName, DateTime dtDayEnd)
        {
            DataSet ds = new DataSet();
            AccountUpdate objAU = new AccountUpdate();
            bool bRet = false;
            string strCatchMessage = string.Empty;

            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Getting mail sending parameters...", false);
                bRet = objAU.FetchDayeEndMailSendingParameters(strConfigPath, ref ds, ref strCatchMessage);
                if (bRet)
                {
                    if (ds.Tables.Count > 0)
                    {
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Creating day end report...", false);
                        if (SendDayEndMail(ds, strRptFileName, dtDayEnd))
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Day end email sent successfully.", false);
                            if (File.Exists(Application.StartupPath + "/Temp/" + strRptFileName)) File.Delete(Application.StartupPath + "/Temp/" + strRptFileName);
                        }
                        else
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetDayEndReportData()==>GenerateDayEndReport()==>GenerateDayEndMail() : Failed to send the mail ", true);
                        }
                    }

                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetDayEndReportData()==>GenerateDayEndReport()==>GenerateDayEndMail()==>Core:FetchDayeEndMailSendingParameters()- Exception : " + strCatchMessage, true);
                }

            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetDayEndReportData()==>GenerateDayEndReport()==>GenerateDayEndMail()- Exception : " + ex.Message, true);
            }
            finally
            {
                objAU = null;
                ds.Dispose();
            }
        }
        #endregion

        #region SendDayEndMail
        private bool SendDayEndMail(DataSet ds, string strRptFileName, DateTime dtDayEnd)
        {
            bool bRet = false;
            MailSender objMail = new MailSender();
            string[] arrAttachFile = new string[1];
            string[] arrAttachFileName = new string[1];
            string strCatchMessage = string.Empty;
            StringBuilder sbMail = new StringBuilder();

            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Sending day end email...", false);
                foreach (DataRow dr in ds.Tables["MailParams"].Rows)
                {
                    sbMail.Append("Please check the attached file containing the day end snap shot of the accounts as on " + dtDayEnd.ToString("MMMM dd, yyyy"));
                    sbMail.Append("<br/>");
                    sbMail.Append("This is an automated message from VETRIS.Please do not reply to the message.");

                    objMail.MailServer = Convert.ToString(dr["mail_server"]).Trim();
                    objMail.MailServerPortNo = Convert.ToInt32(dr["mail_server_port"]);
                    if (Convert.ToString(dr["mail_ssl_enabled"]).Trim() != "Y")
                        objMail.MailServerSSLEnabled = false;
                    else
                        objMail.MailServerSSLEnabled = true;
                    objMail.MailServerUserId = Convert.ToString(dr["mail_user_code"]).Trim();
                    objMail.MailFrom = Convert.ToString(dr["mail_user_code"]).Trim();
                    objMail.DecryptPassword = "N";
                    objMail.MailServerPassword = CoreCommon.DecryptString(Convert.ToString(dr["mail_user_pwd"]).Trim());

                    objMail.MailSenderName = Convert.ToString(dr["mail_user_code"]).Trim();
                    objMail.MailTo = Convert.ToString(dr["rec_email_id"]).Trim();
                    objMail.MailCC =string.Empty;
                    objMail.MailSubject = "Day End Accounts Snap Shot Of " + dtDayEnd.ToString("MMMM dd, yyyy");
                    objMail.MailBody = sbMail.ToString(); 
                    objMail.IsMailBodyHTML = true;
                    objMail.Attachments = 1;

                    arrAttachFile[0] = Application.StartupPath +"/Temp/" + strRptFileName;
                    arrAttachFileName[0] = strRptFileName;
                    objMail.AttachedFile = arrAttachFile;
                    objMail.AttachedFileName = arrAttachFileName;
                }

                bRet = objMail.SendMail(ref strCatchMessage);

                if (!bRet)
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetDayEndReportData()==>GenerateDayEndReport()==>GenerateDayEndMail()==>SendDayEndMail()- Error : " + strCatchMessage, true);
                }
            }
            catch (Exception expErr)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "GetDayEndReportData()==>GenerateDayEndReport()==>GenerateDayEndMail()==>SendDayEndMail()- Exception : " + expErr.Message, true);

            }
            finally
            {
                objMail = null;
                strCatchMessage = null;
            }

            return bRet;
        }
        #endregion

        #endregion


        #region Accounts Payable Service

        #region btnAPStart_Click
        //private void btnAPStart_Click(object sender, EventArgs e)
        //{
        //    lblAPProcess.Visible = true;
        //    lblAPProcess.Text = "Starting application...Please Wait...";
        //    lblAPProcess.Refresh();
        //    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName + "(AP)", "Starting application...", false);
        //    timerAP.Start();
        //} 
        #endregion

        #region btnAPStop_Click
        //private void btnAPStop_Click(object sender, EventArgs e)
        //{
        //    Label.CheckForIllegalCrossThreadCalls = false;
        //    Button.CheckForIllegalCrossThreadCalls = false;
        //    lblAPProcess.Visible = true;
        //    lblAPProcess.Text = "Stopping application...Please Wait...";
        //    lblAPProcess.Refresh();
        //    //doProcess_Wait();
        //    timerAP.Stop();
        //    lblAPStatus.ForeColor = Color.Red;
        //    lblAPStatus.Text = "(Stopped...)";
        //    lblAPStatus.Refresh();
        //    btnAPStop.Visible = false;
        //    btnAPStart.Visible = true;
        //    lblAPProcess.Visible = false;
        //    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName + "(AP)", "Application stopped...", false);
        //} 
        #endregion

        #region timerAP_Tick
        //private void timerAP_Tick(object sender, EventArgs e)
        //{
        //    Label.CheckForIllegalCrossThreadCalls = false;
        //    Button.CheckForIllegalCrossThreadCalls = false;
        //    lblAPStatus.ForeColor = Color.Blue;
        //    lblAPStatus.Text = "(Running...)";
        //    lblAPStatus.Refresh();
        //    btnAPStop.Visible = true;
        //    btnAPStart.Visible = false;
        //    btnAPStop.Left = btnAPStart.Left;
        //    btnAPStop.Top = btnAPStart.Top;
        //    lblAPProcess.Visible = false;
        //    threadAP = new Thread(doAP);
        //    threadAP.Start();
        //} 
        #endregion

        #region doProcess
        private void doAP()
        {

            UpdateRadiologists();

        }
        #endregion

        #region UpdateRadiologists
        private void UpdateRadiologists()
        {


            #region Members & Variables
            string strCatchMsg = string.Empty;

            DataSet ds = new DataSet();
            Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
            string strCode = string.Empty;
            string strName = string.Empty;
            string strQBName = string.Empty;
            string strAddress1 = string.Empty;
            string strAddress2 = string.Empty;
            string strCity = string.Empty;
            string strZip = string.Empty;
            string strStateName = string.Empty;
            string strCountryName = string.Empty;
            string strEmailID = string.Empty;
            string strPhoneNo = string.Empty;
            bool bIsActive = false;
            string strListID = string.Empty;
            string strCatchMessage = string.Empty;
            QBDriver driver = new QBDriver();
            bool bRet = false;
            bool bUpdate = false;
            AccountUpdate objAU = new AccountUpdate();
            #endregion


            try
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName + "(AP)", "Fetching radiologist to update...", false);
                if (objAU.FetchRadiologistList(strConfigPath, ref ds, ref strCatchMsg))
                {
                    if (ds.Tables["Radiologists"].Rows.Count > 0)
                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName + "(AP)", ds.Tables["Radiologists"].Rows.Count.ToString() + " record(s) fetched.", false);


                    foreach (DataRow dr in ds.Tables["BillingAccounts"].Rows)
                    {
                        #region Populate Variables
                        Id = new Guid(Convert.ToString(dr["id"]));
                        strCode = Convert.ToString(dr["code"]).Trim();
                        strName = Convert.ToString(dr["name"]);
                        strQBName = Convert.ToString(dr["qb_name"]);
                        strAddress1 = Convert.ToString(dr["address_1"]).Trim();
                        strAddress2 = Convert.ToString(dr["address_2"]).Trim();
                        strCity = Convert.ToString(dr["city"]).Trim();
                        strZip = Convert.ToString(dr["zip"]).Trim();
                        strStateName = Convert.ToString(dr["state_name"]).Trim();
                        strCountryName = Convert.ToString(dr["country_name"]).Trim();
                        strEmailID = Convert.ToString(dr["email_id"]).Trim();
                        strPhoneNo = Convert.ToString(dr["phone_no"]).Trim();
                        if (Convert.ToString(dr["is_active"]) == "Y") bIsActive = true;
                        else if (Convert.ToString(dr["is_active"]) == "N") bIsActive = false;
                        strListID = Convert.ToString(dr["debtor_id"]).Trim();
                        #endregion

                        #region Populate Vendor Enitity
                        var radiologist = new VendorEntity
                        {
                            AccountNumber = strCode,
                            Name = strName,
                            FullName = strName,
                            CompanyName = strName,
                            ListID = strListID,
                            Phone = strPhoneNo,
                            ExternalGUID = Id.ToString(),
                            Email = strEmailID,
                            IsActive = bIsActive,
                            Address = new Address
                            {
                                Address1 = strAddress1,
                                Address2 = strAddress2,
                                City = strCity,
                                PostalCode = strZip,
                                Country = strCountryName,
                                State = strStateName
                            }
                        };
                        #endregion

                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName + "(AP)", "Updating radiologist account ", false);

                        if (!driver.IsVendorNameExists(strQBName, ref strCatchMessage))
                        {
                            #region Create radiologist in Quick Books
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName + "(AP)", "Creating radiologist " + strName, false);
                            radiologist = driver.CreateRadiologist(radiologist, ref strCatchMessage);
                            if (radiologist != null)
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName + "(AP)", "Successfully created radiologist " + strName, false);
                                strListID = radiologist.ListID.Trim();
                                bUpdate = true;
                            }
                            else
                            {
                                bUpdate = false;
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName + "(AP)", "Quick books error to create radiologist " + strName + " :: Error - " + strCatchMessage.Trim(), true);
                            }
                            #endregion
                        }
                        else
                        {
                            #region Edit radiologist In Quick Books
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName + "(AP)", "Editing radiologist " + strName, false);
                            var radDtls = driver.getBillingAccounts(ref strCatchMessage, strQBName).FirstOrDefault();
                            if (radDtls != null)
                            {
                                if (radiologist.ListID.Trim() == string.Empty)
                                {
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName + "(AP)", "Getting List ID Of radiologist " + strName, false);
                                    strListID = radDtls.ListID.Trim();
                                    strQBName = radDtls.Name;
                                    bUpdate = true;
                                }
                                else if (radiologist.ListID == radDtls.ListID)
                                {
                                    radiologist.EditSequence = radDtls.EditSequence;
                                    strQBName = radDtls.Name;
                                    bRet = driver.UpdateRadiologist(radiologist, ref strCatchMessage);
                                    if (bRet)
                                    {
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName + "(AP)", "Successfully edited radiologist " + strName, false);
                                        strListID = radDtls.ListID.Trim();
                                    }
                                    else
                                    {
                                        CoreCommon.doLog(strConfigPath, intServiceID, strSvcName + "(AP)", "Quick books error to update radiologist " + strName + " :: Error - " + strCatchMessage.Trim(), true);
                                    }
                                    bUpdate = bRet;
                                }
                            }
                            else
                            {
                                bUpdate = false;
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName + "(AP)", "Radiologist " + strName + " could not be found in Quick Books :: Error - " + strCatchMessage.Trim(), true);
                            }
                            #endregion

                        }

                        #region Update VETRIS DB
                        if (bUpdate)
                        {
                            objAU.RADIOLOGIST_ID = Id;
                            objAU.QB_NAME = strQBName.Trim();
                            objAU.CREDITOR_ID = strListID;
                            strCatchMessage = "";

                            if (!objAU.UpdateRadiologist(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                            {
                                if (strCatchMessage.Trim() != string.Empty)
                                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName + "(AP)", "UpdateRadiologists()==>UpdateRadiologist()- Core::Exception - " + strCatchMessage, false);
                            }
                        }
                        #endregion
                    }
                }
                else
                {
                    CoreCommon.doLog(strConfigPath, intServiceID, strSvcName + "(AP)", "UpdateRadiologists()==>FetchRadiologistUpdateList():Core-Exception: " + strCatchMsg, true);
                }
            }
            catch (Exception ex)
            {
                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName + "(AP)", "UpdateRadiologists() :: " + strName + " - Exception: " + ex.Message, true);
            }
            finally
            {
                objAU = null; ds.Dispose(); driver.Dispose(); driver = null;
            }
            PostVouchers();
        }
        #endregion

        #endregion

        #region padZero
        public string padZero(int Num)
        {
            return (Num < 10) ? '0' + Convert.ToString(Num) : Convert.ToString(Num);
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
