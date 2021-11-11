using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using VETRIS.Core.Invoicing;


namespace VETRIS.Invoicing
{
    [AjaxPro.AjaxNamespace("VRSAcctOBDlg")]
    public partial class VRSAcctOBDlg : System.Web.UI.Page
    {
        AROpeningBalance objCore=null;
        classes.CommonClass objComm;
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSAcctOBDlg));
            SetAttributes();
            SetPageValue();
        }

        #region SetAttributes
        private void SetAttributes()
        {

            btnAdd1.Attributes.Add("onclick", "javascript:btnNew_OnClick();");
            btnAdd2.Attributes.Add("onclick", "javascript:btnNew_OnClick();");
            //btnDelete1.Attributes.Add("onclick", "javascript:btnDelete_OnClick();");
            //btnDelete2.Attributes.Add("onclick", "javascript:btnDelete_OnClick();");
            btnSave1.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnSave2.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

            txtAmount.Attributes.Add("onfocus", "javascript:this.select();");
            txtAmount.Attributes.Add("onkeypress", "javascript:return parent.CheckDecimal(event);");
            txtAmount.Attributes.Add("onblur", "javascript:ResetValueDecimal(this);");
            txtOpBalDate.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgOpBalDt.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtOpBalDate.ClientID + "','CalOpBalDate');");

        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"].ToString());
            string strID = string.Empty;
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            txtOpBalDate.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            txtAmount.Text = objComm.IMNumeric("0",objComm.DecimalDigit);
            hdnID.Value = Request.QueryString["id"];
            FetchParameters(UserID);
            LoadDetail(UserID,intMenuID);
            string strTheme = Request.QueryString["th"];
            SetCSS(strTheme);
        }
        #endregion
        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
            lnkCAL.Attributes["href"] = strServerPath + "/css/" + strTheme + "/CalendarStyle.css";
        }
        #endregion

        #region FetchParameters
        private void FetchParameters(Guid userId)
        {
            objCore = new AROpeningBalance();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                objCore.UserID = userId;
                bReturn = objCore.FetchParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    #region Billing Accounts
                    DataRow dr2 = ds.Tables["Account"].NewRow();
                    dr2["id"] = "00000000-0000-0000-0000-000000000000";
                    dr2["name"] = "Select One";
                    ds.Tables["Account"].Rows.InsertAt(dr2, 0);
                    ds.Tables["Account"].AcceptChanges();

                    ddlAccount.DataSource = ds.Tables["Account"];
                    ddlAccount.DataValueField = "id";
                    ddlAccount.DataTextField = "name";
                    ddlAccount.DataBind();
                    #endregion

                    #region Year
                    ddlYear.DataSource = ds.Tables["Year"];
                    ddlYear.DataValueField = "year_value";
                    ddlYear.DataTextField = "year_value";
                    ddlYear.DataBind();
                    #endregion

                }
                else
                    hdnError.Value = strCatchMessage.Trim();
            }
            catch (Exception ex)
            {
                hdnError.Value = ex.Message.Trim();
            }
            finally
            {
                ds.Dispose();
            }

        }
        #endregion
        
        #region LoadDetail
        private void LoadDetail(Guid userId, int menuId)
        {
            objCore = new AROpeningBalance();
            string strCatchMessage = ""; string strReturnMessage = "";
            bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = userId;
            string strFileName = "";

            try
            {
                objCore.id = new Guid(hdnID.Value);
                objCore.UserID = userId;
                objCore.MenuID = menuId;

                bReturn = objCore.FetchRecords(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    hdnID.Value = objCore.id.ToString();
                    ddlAccount.SelectedValue = objCore.billing_account_id.ToString();
                    ddlYear.SelectedValue = objCore.opbal_date.Year.ToString();
                    txtInvoiceNo.Text = objCore.invoice_no;
                    if (objCore.id.ToString() == "00000000-0000-0000-0000-000000000000")
                        txtOpBalDate.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
                    else
                        txtOpBalDate.Text = objComm.IMDateFormat(objCore.opbal_date, objComm.DateFormat);
                    txtAmount.Text = objCore.opbal_amount.ToString();
                   
                    ddlAccount.Enabled = !objCore.isadjusted;
                    ddlYear.Enabled = !objCore.isadjusted;
                    txtOpBalDate.Enabled = !objCore.isadjusted;
                    CalOpBalDate.Enabled = !objCore.isadjusted;
                    txtInvoiceNo.Enabled = !objCore.isadjusted;
                    txtAmount.Enabled = !objCore.isadjusted;
                    btnSave1.Visible = !objCore.isadjusted;
                    btnSave2.Visible = !objCore.isadjusted;
                    //if (objCore.id.ToString() != "00000000-0000-0000-0000-000000000000")
                    //{
                    //    btnDelete1.Visible = !objCore.isadjusted;
                    //    btnDelete2.Visible = !objCore.isadjusted;
                    //}
                    //else
                    //{
                    //    btnDelete1.Visible = false;
                    //    btnDelete2.Visible = false;
                    //}
                }
               


            }
            catch (Exception ex)
            {
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion


        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord)
        {
            bool bReturn = false;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            int intCompanyID = 0; string strFileName = string.Empty; Guid UserID = Guid.Empty;
            objComm = new classes.CommonClass();
            objCore = new AROpeningBalance();
            int intListIndex = 0;
           

            try
            {

                objCore.id = new Guid(ArrRecord[0]);
                objCore.billing_account_id = new Guid(ArrRecord[1]);
                objCore.opbal_date = Convert.ToDateTime(ArrRecord[2].Trim());
                objCore.invoice_no = ArrRecord[3].Trim();
                objCore.opbal_amount = Convert.ToDecimal(ArrRecord[4]);
                objCore.UserID = UserID = new Guid(ArrRecord[5]);
                objCore.MenuID = Convert.ToInt32(ArrRecord[6]);


                bReturn = objCore.SaveRecord(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[2];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
                }
                else
                {

                    if (strCatchMessage.Trim() != "")
                    {
                        arrRet = new string[2];
                        arrRet[0] = "catch";
                        arrRet[1] = strCatchMessage.Trim();

                    }
                    else
                    {
                        arrRet = new string[3];
                        arrRet[0] = "false";
                        arrRet[1] = strReturnMsg.Trim();
                        arrRet[2] = objCore.UserID.ToString();
                    }
                }
            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = expErr.Message.Trim();
            }
            finally
            {
                objCore = null; objComm = null;
                strReturnMsg = null; strCatchMessage = null;
            }
            return arrRet;
        }
        #endregion
    }
}