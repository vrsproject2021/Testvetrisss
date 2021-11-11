using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VETRIS.Core.MyPayments;
using VETRIS.Core.TransNationalPaymentGateway;

namespace VETRIS.MyPayment
{
    [AjaxPro.AjaxNamespace("VRSGetPaymentDlg")]
    public partial class VRSGetPaymentDlg : System.Web.UI.Page
    {


        #region Members & Variables
        ARPayments objCore = new ARPayments();
        public List<TransactionVault> vaults = new List<TransactionVault>();
        classes.CommonClass objComm;
        private string _userIp = null;

        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSGetPaymentDlg));
            SetAttributes();
            if (!IsPostBack)
                SetPageValue();
            if (IsPostBack)
            {
                if (usingOption.Value=="" && rodAnother.Checked)
                {
                    usingOption.Value = "0";
                    rdoCC.Checked = true;
                    SetPageValue();
                }
                if (usingOption.Value == "1" && rdoCC.Checked)
                {
                    usingOption.Value = "0";
                    rdoCC.Checked = true;
                    SetPageValue();
                }
                if (usingOption.Value == "0" && rdoACH.Checked)
                {
                    usingOption.Value = "1";
                    rdoACH.Checked = true;
                    SetPageValue();
                }
                //if (usingOption.Value != "")
                //{
                //    if(rdoCC.Checked) {
                //        if (string.IsNullOrEmpty(fname.Value))
                //        {
                //            SetPageValue();
                //        }

                //    }
                //    if (usingOption.Value != "1" && rdoACH.Checked)
                //    {
                //        usingOption.Value = "1";
                //        rdoACH.Checked = true;
                //        if (string.IsNullOrEmpty(fname.Value))
                //        {
                //            SetPageValue();
                //        }

                //    }
                //}
                
                
            }
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            if (this.usingOption.Value == "")
            {
                this.payButton.Attributes.Remove("onclick");
                this.payButton.Attributes.Add("onclick", "javascript:btnPay_OnClick();return false;");
            }
            else
                this.payButton.Attributes.Remove("onclick");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            Guid AcctID = new Guid(Request.QueryString["aid"]);

            hdnAID.Value = Request.QueryString["aid"];
            if (Request.QueryString["cf"] != null)
            {
                hdnCF.Value = Request.QueryString["cf"];
            }

            _userIp = Request.UserHostAddress;
            if (_userIp == "::1") _userIp = "127.0.0.1";  // localhost
            hdnID.Value = Request.QueryString["id"];
            UserIp.Value = _userIp;
            objCore.UserID = UserID;
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            objComm = null;
            LoadCustomerRecord(AcctID,intMenuID, UserID);
            string strTheme = Request.QueryString["th"];
            SetCSS(strTheme);
        }

        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkPMT.Attributes["href"] = strServerPath + "/css/" + strTheme + "/payment.css";
        }
        #endregion

        #region Load Billing account details for payment gateway
        private void LoadCustomerRecord(Guid AcctID, int MenuId, Guid UserId)
        {
            ARPayments objCore = new Core.MyPayments.ARPayments();
            TransactionVault vaultCore = new TransactionVault();

            string strCatchMessage = "";
            string strReturnMessage = string.Empty;

            string strCatchMessage2 = "";
            string strReturnMessage2 = string.Empty;

            bool bReturn = false;
            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();
            objComm = new classes.CommonClass();
            

            try
            {
                objComm.SetRegionalFormat();
                objCore.billing_account_id = AcctID;


                vaultCore.billing_account_id = AcctID;
  

                bReturn = objCore.LoadCustomerInfo(Server.MapPath("~"), ref ds, ref strCatchMessage);
                var cReturn = vaultCore.LoadDetails(Server.MapPath("~"), ref ds2, ref strCatchMessage2);

                if (bReturn)
                {
                    //this.fname.Value = objCore.name;
                    this.address.Value = objCore.address_1 + " " + objCore.address_2;
                    this.city.Value = objCore.city;
                    this.state.Value = objCore.state;
                    this.zip.Value = objCore.zip;
                    this.country.Value = objCore.country;
                    this.phone.Value = objCore.contact_no;
                }
                if (cReturn)
                {
                    foreach (DataRow dr in ds2.Tables["VaultRecords"].Rows)
                    {
                        var item = new TransactionVault();
                        item.billing_account_id = new Guid(dr["billing_account_id"].ToString());
                        item.vault_id = new Guid(dr["vault_id"].ToString());
                        item.vault_type = dr["vault_type"].ToString();
                        item.vault_card = dr["vault_card"].ToString();
                        item.vault_card_type = dr["vault_card_type"].ToString();
                        item.vault_exp = dr["vault_exp"].ToString();
                        item.last_used = Convert.ToInt32(dr["last_used"].ToString());
                        vaults.Add(item);
                    }

                }

                this.hdnHasVault.Value = vaults.Count.ToString();
                if (this.initialGo.Value == "1")
                {
                    this.initialGo.Value = "0";
                    if (Convert.ToInt32(this.hdnHasVault.Value) > 0)
                    {
                        this.usingOption.Value = "";
                        this.rdoACH.Checked = false;
                        this.rdoCC.Checked = false;
                    }
                    else
                    {
                        this.usingOption.Value = "0";
                        this.rdoACH.Checked = false;
                        this.rdoCC.Checked = true;
                    }
                }
                else
                {
                    if (this.rdoCC.Checked) this.usingOption.Value = "0";
                    if (this.rdoACH.Checked) this.usingOption.Value = "1";
                    if (!this.rdoCC.Checked && !this.rdoACH.Checked)
                    {
                        this.usingOption.Value = "0";
                        this.rdoCC.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                ds.Dispose();
                objCore = null;
                objComm = null;
            }

        }
        #endregion

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord)
        {
            bool bReturn = false, cbReturn = false, CardSaveRequired = false;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strReturnMsg1 = string.Empty; string strCatchMessage1 = string.Empty;
            int intCompanyID = 0;
            int intListIndex = 0;
            string transactionType = null;
            string vault_id = null; // vault_id
            string cvv = null; // card cvv when use saved card
            List<string> cardOrAchInfo = new List<string>();
            Guid newVaultId = Guid.Empty;
            Guid VaultID = Guid.Empty;
            string authcode = string.Empty;
            string cvvresponse = string.Empty;
            string avsresponse = string.Empty;

            objComm = new classes.CommonClass();
            objCore = new ARPayments();


            try
            {
                objComm.SetRegionalFormat();
                objCore.UserID = new Guid(ArrRecord[0]);
                objCore.MenuID = Convert.ToInt32(ArrRecord[1]);
                _userIp = ArrRecord[3];
                objCore.created_by = objCore.UserID;
                objCore.payment_amount = Convert.ToDecimal(ArrRecord[2]);
                var payment_token = ArrRecord[5] ?? "";
                var alreadyHasVault = Convert.ToInt32(ArrRecord[17] ?? "");
                var billing_account_id = ArrRecord[18];
                var isSaveInstruction = ArrRecord[14] ?? ""; // U - Use card, TS - Transaction save, blank - do not save card info
                

                if (string.IsNullOrEmpty(billing_account_id))
                {
                    throw new Exception("Billing account id was not supplied.");
                }

                if (isSaveInstruction == "U") // use saved card
                {
                    vault_id = ArrRecord[15];
                    if (string.IsNullOrEmpty(vault_id))
                        throw new Exception("Saved card not selected.");
                    cvv = ArrRecord[16];
                    if (string.IsNullOrEmpty(cvv))
                        throw new Exception("CVV was not supplied.");

                }
                else
                {
                    transactionType = ArrRecord[15]; // c-> Credit Card or a-> ACH
                    if (transactionType != "c" && isSaveInstruction == "TS")
                    {
                        throw new Exception("Only card transaction can be saved.");
                    }
                    cardOrAchInfo = (ArrRecord[16] ?? "").Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (cardOrAchInfo.Count != 3)
                    {
                        throw new Exception("Card information was not correct.");
                    }
                }


                objCore.name = (ArrRecord[6].Trim() + " " + ArrRecord[7].Trim()).Trim();
                objCore.address_1 = ArrRecord[8] ?? "";
                objCore.city = ArrRecord[9] ?? "";
                objCore.state = ArrRecord[10] ?? "";
                objCore.country = ArrRecord[11] ?? "";
                objCore.zip = ArrRecord[12] ?? "";
                objCore.contact_no = ArrRecord[13] ?? "";
                objCore.payment_tool = ArrRecord[15].Trim().ToUpper();


                if ((string.IsNullOrEmpty(isSaveInstruction) || isSaveInstruction == "TS") && string.IsNullOrEmpty(payment_token))
                {
                    throw new Exception("Token was not supplied.");
                }
                if ((string.IsNullOrEmpty(isSaveInstruction) || isSaveInstruction == "TS"))
                    bReturn = ValidatePayment(ref strReturnMsg, strCatchMessage);
                else if (isSaveInstruction == "U")
                {
                    if (this.objCore.payment_amount <= 0)
                    {
                        strReturnMsg = "Amount must be greater than zero.";
                        throw new Exception(strReturnMsg);
                    }
                    bReturn = true;
                }

                if (bReturn)
                {
                    #region Set Adjustment Invoice(s)
                    var invoices = Newtonsoft.Json.JsonConvert.DeserializeObject<List<dynamic>>(ArrRecord[4]);
                    if (invoices.Count > 0)
                    {
                        try
                        {
                            invoices.ForEach(adj =>
                            {
                                var adjustment = new PaymentAdjustmentRow
                                {
                                    invoice_header_id = new Guid(adj.invoice_header_id.Value),
                                    invoice_no = adj.invoice_no.Value,
                                    invoice_date = Convert.ToDateTime(adj.invoice_date.Value),
                                    adj_amount = Convert.ToDecimal(adj.adj_amount.Value.ToString())
                                };
                                objCore.Adjustments.Add(adjustment);
                            });
                        }
                        catch (Exception)
                        {

                            try
                            {
                                invoices.ForEach(adj =>
                                {
                                    var adjustment = new PaymentAdjustmentRow
                                    {
                                        invoice_header_id = new Guid(Convert.ToString(adj["invoice_header_id"])),
                                        invoice_no = Convert.ToString(adj["invoice_no"]),
                                        invoice_date = Convert.ToDateTime(Convert.ToString(adj["invoice_date"])),
                                        adj_amount = Convert.ToDecimal(Convert.ToString(adj["adj_amount"]))
                                    };
                                    objCore.Adjustments.Add(adjustment);
                                });
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }
                        

                        //invoices.ForEach(row =>
                        //{
                        //    Dictionary<string, string> adj = (Dictionary<string, string>)row;
                        //    var adjustment = new PaymentAdjustmentRow
                        //    {
                        //        invoice_header_id = new Guid(Convert.ToString(adj["invoice_header_id"])),
                        //        invoice_no = Convert.ToString(adj["invoice_no"]),
                        //        invoice_date = string.Format("{0:" +objComm.DateFormat +"}", Convert.ToDateTime(Convert.ToString(adj["invoice_dt"]))),
                        //        //invoice_date = objComm.IMDBDateFormat(Convert.ToDateTime(adj["invoice_dt"])),
                        //        adj_amount = Convert.ToDecimal(Convert.ToString(adj["adj_amount"]))
                        //    };
                        //    objCore.Adjustments.Add(adjustment);
                        //});
                    }
                    else
                    {
                        bReturn = false;
                        strReturnMsg = "480";
                        throw new Exception(strReturnMsg);
                    }
                    #endregion

                    #region check adjustments
                    objCore.billing_account_id = new Guid(billing_account_id);
                    bReturn = objCore.ValidatePayment(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);
                    if (!bReturn)
                    {
                        throw new Exception(strReturnMsg);
                    }
                    #endregion

                    #region Post Payment : transnational api
                    /*Gateway to use : transnational api*/
                    PaymentApi api = new PaymentApi();
                    api.production_url = VETRIS.Global.Transaction_Gateway_Url;
                    api.API_Key = VETRIS.Global.API_Key;
                    Dictionary<string, string> _response = new Dictionary<string, string>();

                    if (isSaveInstruction == "U")
                    {
                        _response = api.OnlineDirectPay(
                        type: "sale",
                        amount: objCore.payment_amount.Value,
                        currency: "USD",
                        cvv: cvv,
                        customer_vault_id: vault_id
                        );
                    }
                    else if (isSaveInstruction == "TS")
                    {
                        CardSaveRequired = true;
                        newVaultId = Guid.NewGuid(); // need to save this billing_id

                        _response = api.OnlineDirectPay(
                        payment_token: payment_token,
                        type: "sale",
                        amount: objCore.payment_amount.Value,
                        firstname: ArrRecord[6],
                        lastname: ArrRecord[7],
                        address1: objCore.address_1,
                        address2: "",
                        city: objCore.city,
                        state: objCore.state,
                        country: objCore.country,
                        zip: objCore.zip,
                        customer_vault: "add_customer",
                        customer_vault_id: newVaultId.ToString()
                        );
                    }
                    else
                    {
                        _response = api.OnlineDirectPay(
                        payment_token: payment_token,
                        type: "sale",
                        amount: objCore.payment_amount.Value,
                        firstname: ArrRecord[6],
                        lastname: ArrRecord[7],
                        address1: objCore.address_1,
                        address2: "",
                        city: objCore.city,
                        state: objCore.state,
                        country: objCore.country,
                        zip: objCore.zip
                        );
                    }
                    #endregion


                    if (_response["response"] != "1")
                    {
                        #region Save Payment
                        objCore.processing_ref_no = _response["transactionid"];
                        objCore.processing_status = "0";
                        objCore.remarks = _response["responsetext"];

                        objCore.payref_no = "generated";
                        objCore.payref_date = DateTime.Now;
                        objCore.processing_ref_date = DateTime.Now;
                        objCore.processing_pg_name = "TRANSNATIONAL";
                        objCore.payment_mode = "1"; // ONLINE
                        objCore.processing_status = "0"; //Failed
                        objCore.created_by = objCore.UserID;
                        objCore.date_created = DateTime.Now;

                        // do not adjust for failed transaction
                        objCore.Adjustments.Clear();
                        var bLogReturn = objCore.SaveRecord(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                        bReturn = false;
                        strCatchMessage = "Payment Gateway: " + _response["responsetext"];
                        #endregion

                    }
                    else
                    {
                        objCore.remarks = _response["responsetext"];
                        objCore.processing_ref_no = _response["transactionid"];
                        objCore.processing_status = "1";
                        bReturn = true;
                        ///---------------RESPONSE VARIABLES TO BE USED IN MAIL-------------------
                        authcode = _response["authcode"];
                        cvvresponse = _response["cvvresponse"];
                        avsresponse = _response["avsresponse"];
                        //------------------------------------------------------------------------
                        
                        // save card information first
                        if (isSaveInstruction == "TS")
                        {
                            #region update vault details
                            TransactionVault tvault = new TransactionVault()
                            {
                                id = Guid.NewGuid(),
                                billing_account_id = new Guid(billing_account_id),
                                vault_id = newVaultId,
                                vault_card = cardOrAchInfo[0],
                                vault_exp = cardOrAchInfo[1],
                                vault_card_type = cardOrAchInfo[2],
                                vault_type = transactionType == "c" ? "card" : "check",
                                holder_name=(ArrRecord[6].Trim() + " " + ArrRecord[7].Trim()).Trim(),
                                user_id = objCore.UserID,
                                menu_id = objCore.MenuID
                            };
                            cbReturn = tvault.SaveRecord(Server.MapPath("~"), ref strReturnMsg1, ref strCatchMessage1);
                            if (cbReturn)
                            {
                                VaultID = tvault.vault_id;
                            }
                            #endregion
                        }
                        else if (isSaveInstruction == "U")
                        {
                            #region Update last used vault
                            TransactionVault tvault = new TransactionVault()
                            {
                                vault_id = new Guid(vault_id),
                                user_id = objCore.UserID,
                                menu_id = objCore.MenuID
                            };
                            cbReturn = tvault.UpdateLastUsedDate(Server.MapPath("~"), ref strCatchMessage1);
                            if (cbReturn)
                            {
                                VaultID = tvault.vault_id;
                            }
                            #endregion
                        }
                    }
                }

                if (bReturn)
                {

                    objCore.payref_no = "generated";
                    objCore.payref_date = DateTime.Now;
                    objCore.processing_ref_date = DateTime.Now;
                    objCore.processing_pg_name = "TRANSNATIONAL";
                    objCore.payment_mode = "1"; // ONLINE
                    objCore.auth_code = authcode.Trim();
                    objCore.cvv_response = cvvresponse.Trim();
                    objCore.avs_response = avsresponse.Trim();
                    objCore.vault_id = VaultID;
                    objCore.created_by = objCore.UserID;
                    objCore.date_created = DateTime.Now;

                    // do not adjust for failed transaction
                    if (objCore.payment_mode == "1" && objCore.processing_status == "0")
                    {
                        objCore.Adjustments.Clear();
                    }
                    bReturn = objCore.SaveRecord(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                }

                if (bReturn)
                {
                    arrRet = new string[3];
                    arrRet[0] = "true";
                    arrRet[1] = Convert.ToString(objCore.id);
                    arrRet[2] = strReturnMsg.Trim();
                }
                else
                {
                    arrRet = new string[2];
                    if (strCatchMessage.Trim() != "")
                    {
                        arrRet[0] = "catch";
                        arrRet[1] = strCatchMessage.Trim();

                    }
                    else
                    {
                        arrRet[0] = "false";
                        arrRet[1] = strReturnMsg.Trim();
                        //arrRet[2] = objCore.USER_NAME;
                    }
                }
            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrRet[0] = "catch";
                arrRet[1] = expErr.Message.Trim();
            }
            finally
            {
                objCore = null;
                objComm = null;
                strReturnMsg = null; strCatchMessage = null;
            }
            return arrRet;
        }

        #region ValidatePayment
        private bool ValidatePayment(ref string strReturnMsg, string strCatchMessage)
        {

            if (this.objCore.payment_amount <= 0)
            {
                strReturnMsg = "Amount must be greater than zero.";
                return false;
            }
            if (string.IsNullOrEmpty(this.objCore.name))
            {
                strReturnMsg = "Name is required.";
                return false;
            }
            if (string.IsNullOrEmpty(this.objCore.address_1))
            {
                strReturnMsg = "Address is required.";
                return false;
            }
            if (string.IsNullOrEmpty(this.objCore.city))
            {
                strReturnMsg = "City is required.";
                return false;
            }
            if (string.IsNullOrEmpty(this.objCore.state))
            {
                strReturnMsg = "State is required.";
                return false;
            }
            if (string.IsNullOrEmpty(this.objCore.country))
            {
                strReturnMsg = "Country is required.";
                return false;
            }

            return true;
        }
        #endregion

        #endregion


    }
}