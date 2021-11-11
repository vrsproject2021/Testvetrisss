using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VETRISAccountsScheduler.Core;
using QBVetrisLib;

namespace VETRISAccountsScheduler
{
    public partial class frmTestcs : Form
    {
        public frmTestcs()
        {
            InitializeComponent();
        }

        private void frmTestcs_Load(object sender, EventArgs e)
        {

        }

        #region btnUpdateBA_Click
        private void btnUpdateBA_Click(object sender, EventArgs e)
        {
            string strResult = string.Empty;
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
            AccountUpdate objAU = new AccountUpdate();
            int intServiceID = 8;
            string strSvcName = "VETRIS Accounts Update Service";
            string strConfigPath = Application.StartupPath;

            try
            {
                txtResult.Text = "Fetching billing accounts to update...";

                if (objAU.FetchBillingAccountUpdateList(strConfigPath, ref ds, ref strCatchMsg))
                {
                    txtResult.Text += "\n" + ds.Tables["BillingAccounts"].Rows.Count.ToString() + " record(s) fetched.";

                    foreach (DataRow dr in ds.Tables["BillingAccounts"].Rows)
                    {

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

                        var customer = new CustomerEntity
                        {
                            FirstName = strCode,
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

                        txtResult.Text += "\n" + "Updating billing account " + strName;

                        if (!driver.IsBillingAccountExists(strQBName, ref strCatchMessage))
                        {
                            CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Creating billing account " + strName, false);
                            customer = driver.CreateBillingAccount(customer, ref strCatchMessage);
                            if (customer != null)
                            {
                                CoreCommon.doLog(strConfigPath, intServiceID, strSvcName, "Successfully created billing account " + strName, false);
                                strListID = customer.ListID.Trim();

                                objAU.BILLING_ACCOUNT_ID = Id;
                                objAU.DEBTOR_ID = strListID;
                                strCatchMessage = "";

                                if (!objAU.UpdateBillingAccount(strConfigPath, intServiceID, strSvcName, ref strCatchMessage))
                                {
                                    if (strCatchMessage.Trim() != string.Empty)
                                        txtResult.Text += "\n" + "UpdateBillingAccounts()==>UpdateBillingAccount()- Core::Exception - " + strCatchMessage;
                                }
                            }
                            else
                            {
                                txtResult.Text += "\n" + "Quick books error to create billing account " + strName + " :: Error - " + strCatchMessage.Trim();
                            }
                        }
                        else
                        {
                            txtResult.Text += "\n" + "Editing billing account " + strName;
                            var custDtls = driver.getBillingAccounts(ref strCatchMessage, strQBName).FirstOrDefault();
                            if (custDtls != null)
                            {
                                if (customer.ListID == custDtls.ListID)
                                {
                                    customer.EditSequence = custDtls.EditSequence;
                                    bRet = driver.UpdateBillingAccount(customer, ref strCatchMessage);
                                    if (bRet)
                                    {
                                        txtResult.Text += "\n" + "Successfully edited billing account " + strName;
                                    }
                                    else
                                    {
                                        txtResult.Text += "\n" + "Quick books error to update billing account " + strName + " :: Error - " + strCatchMessage.Trim();
                                    }
                                }
                            }
                            else
                            {
                                txtResult.Text += "\n" + "Billing account " + strName + " could not be found in Quick Books :: Error - " + strCatchMessage.Trim();
                            }

                        }
                    }



                }
                else
                {
                    txtResult.Text += "\n" + "UpdateBillingAccounts()==>FetchBillingAccountUpdateList():Core-Exception: " + strCatchMsg;
                }
            }
            catch (Exception ex)
            {
                txtResult.Text += "\n" + "UpdateBillingAccounts() - Exception: " + ex.Message;


            }
            finally
            {
                objAU = null; ds.Dispose(); driver.Dispose(); driver = null;

            }
        } 
        #endregion

        #region btnAcctQuery_Click
        private void btnAcctQuery_Click(object sender, EventArgs e)
        {

        } 
        #endregion


    }
}
