using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.Data;
using System.IO;
using System.Net;
using VETRIS.Core;
using AjaxPro;

namespace VETRIS.CaseList
{
    [AjaxPro.AjaxNamespace("VRSApplyPromotion")]
    public partial class VRSApplyPromotion : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.Case.CaseStudy objCore;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSApplyPromotion));
            SetAttributes();
            SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);

            btnApply1.Attributes.Add("onclick", "javascript:btnApply_OnClick();");
            btnApply2.Attributes.Add("onclick", "javascript:btnApply_OnClick();");
            btnRevert1.Attributes.Add("onclick", "javascript:btnRevert_OnClick();");
            btnRevert2.Attributes.Add("onclick", "javascript:btnRevert_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            txtDiscPer.Attributes.Add("onfocus", "javascript:this.select();");
            txtDiscPer.Attributes.Add("onkeypress", "javascript:return parent.CheckDecimal(event);");
            txtDiscPer.Attributes.Add("onblur", "javascript:ResetValueDecimal(this);");
            txtDiscAmt.Attributes.Add("onfocus", "javascript:this.select();");
            txtDiscAmt.Attributes.Add("onkeypress", "javascript:return parent.CheckDecimal(event);");
            txtDiscAmt.Attributes.Add("onblur", "javascript:ResetValueDecimal(this);");
            rdoPer.Attributes.Add("onclick", "javascript:DiscountBy_OnClick();");
            rdoAmt.Attributes.Add("onclick", "javascript:DiscountBy_OnClick();");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            hdnID.Value = Request.QueryString["id"];
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            Guid SessionID = new Guid(Request.QueryString["sid"]);
            hdnMenuID.Value = intMenuID.ToString();
            hdnUserID.Value = UserID.ToString();
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            hdnDivider.Value = objComm.RecordDivider.ToString();
            hdnDecPlaces.Value = objComm.DecimalDigit.ToString();
            objComm = null;
            SetCSS(Request.QueryString["th"]);
            FetchDiscountDetails(intMenuID, UserID, SessionID);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkCAL.Attributes["href"] = strServerPath + "/css/" + strTheme + "/CalendarStyle.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
        }
        #endregion

        #region FetchDiscountDetails
        private void FetchDiscountDetails(int intMenuID, Guid UserID, Guid SessionID)
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            DataSet ds = new DataSet();
            string[] arrayCode = new string[0];
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.ID = new Guid(hdnID.Value);
                objCore.MENU_ID = intMenuID;
                objCore.USER_ID = UserID;
                objCore.USER_SESSION_ID = SessionID;

                bReturn = objCore.FetchDiscountDetails(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    
                    PopulateDropdowns(ds);
                    lblSUID.Text = objCore.STUDY_UID;
                    lblInstitution.Text = objCore.INSTITUTION_NAME;
                    lblPName.Text = objCore.PATIENT_NAME;
                    if (objCore.DISCOUNT_BY == "N" || objCore.DISCOUNT_BY == "P") rdoPer.Checked = true;
                    else if (objCore.DISCOUNT_BY == "A") rdoAmt.Checked = true;
                    txtDiscPer.Text = objComm.IMNumeric(objCore.DISCOUNT_PERCENT, objComm.DecimalDigit);
                    txtDiscAmt.Text = objComm.IMNumeric(objCore.DISCOUNT_AMOUNT, objComm.DecimalDigit);
                    ddlReason.SelectedValue = objCore.PROMOTION_REASON_ID.ToString();
                    hdnInvoiced.Value = objCore.INVOICED;
                    if (objCore.INVOICED == "Y") lblInvoiced.Text = "INVOICED";
                    txtCost.Text = objComm.IMNumeric(objCore.STUDY_COST,objComm.DecimalDigit);

                }
                else
                {
                    arrayCode = new string[1];
                    if (strCatchMessage.Trim() != string.Empty)
                        arrayCode[0] = strCatchMessage.Trim();
                    else
                        arrayCode[0] = strReturnMessage.Trim();
                    lblMsg.Text = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
                    hdnError.Value = "Y";
                }


            }
            catch (Exception ex)
            {
                arrayCode = new string[1];
                arrayCode[0] = ex.Message.Trim();
                lblMsg.Text = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
                hdnError.Value = "Y";
            }
            finally
            {
                ds.Dispose(); objComm = null; objCore = null;
            }
        }
        #endregion

        #region PopulateDropdowns
        private void PopulateDropdowns(DataSet ds)
        {
            #region Reasons
            DataRow dr1 = ds.Tables["Reasons"].NewRow();
            dr1["id"] = "00000000-0000-0000-0000-000000000000";
            dr1["reason"] = "Select One";
            ds.Tables["Reasons"].Rows.InsertAt(dr1, 0);
            ds.Tables["Reasons"].AcceptChanges();

            ddlReason.DataSource = ds.Tables["Reasons"];
            ddlReason.DataValueField = "id";
            ddlReason.DataTextField = "reason";
            ddlReason.DataBind();
            #endregion

        }
        #endregion

        #region ApplyDiscount (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] ApplyDiscount(string[] ArrRecord)
        {
            bool bReturn = false;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string[] arrayCode = new string[0];
            
            objComm = new classes.CommonClass();
            objCore = new Core.Case.CaseStudy();
            


            try
            {
                objCore.ID = new Guid(ArrRecord[0]);
                objCore.DISCOUNT_BY = ArrRecord[1].Trim();
                objCore.DISCOUNT_PERCENT = Convert.ToDecimal(ArrRecord[2].Trim());
                objCore.DISCOUNT_AMOUNT = Convert.ToDouble(ArrRecord[3].Trim());
                objCore.PROMOTION_REASON_ID = new Guid(ArrRecord[4].Trim());
                objCore.STUDY_COST = Convert.ToDouble(ArrRecord[5].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[6]);
                objCore.USER_ID =  new Guid(ArrRecord[7]);
                objCore.USER_SESSION_ID = new Guid(ArrRecord[8]);

                bReturn = objCore.ApplyDiscount(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[3];
                    arrRet[0] = "true";
                    strReturnMsg = strReturnMsg.Trim();
                    arrayCode = new string[1];
                    arrayCode[0] = strReturnMsg;
                    arrRet[1] = objComm.SetErrorResources(arrayCode, "N", false, string.Empty, string.Empty);
                    arrRet[1] = arrRet[1].Replace("&nbsp;","");
                }
                else
                {

                    if (strCatchMessage.Trim() != "")
                    {
                        arrRet = new string[2];
                        arrRet[0] = "catch";
                        arrayCode = new string[1];
                        arrayCode[0] = strCatchMessage.Trim();
                        arrRet[1] = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
                        arrRet[1] = arrRet[1].Replace("&nbsp;", "");

                    }
                    else
                    {
                        arrRet = new string[2];
                        arrRet[0] = "false";
                        strReturnMsg = strReturnMsg.Trim();
                        arrayCode = strReturnMsg.Split(objComm.RecordDivider);
                        arrRet[1] = objComm.SetErrorResources(arrayCode, "N", true, objCore.USER_NAME, string.Empty);
                        arrRet[1] = arrRet[1].Replace("&nbsp;", "");
                    }
                }
            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrayCode = new string[1];
                arrayCode[0] = expErr.Message.Trim();
                arrRet[1] = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
            }
            finally
            {
                objCore = null; objComm = null;
                strReturnMsg = null; strCatchMessage = null;
            }
            return arrRet;
        }
        #endregion 

        #region RevertDiscount (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] RevertDiscount(string[] ArrRecord)
        {
            bool bReturn = false;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string[] arrayCode = new string[0];

            objComm = new classes.CommonClass();
            objCore = new Core.Case.CaseStudy();



            try
            {
                objCore.ID = new Guid(ArrRecord[0]);
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[1]);
                objCore.USER_ID = new Guid(ArrRecord[2]);
                objCore.USER_SESSION_ID = new Guid(ArrRecord[3]);

                bReturn = objCore.RevertDiscount(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[3];
                    arrRet[0] = "true";
                    strReturnMsg = strReturnMsg.Trim();
                    arrayCode = new string[1];
                    arrayCode[0] = strReturnMsg;
                    arrRet[1] = objComm.SetErrorResources(arrayCode, "N", false, string.Empty, string.Empty);
                    arrRet[1] = arrRet[1].Replace("&nbsp;", "");
                }
                else
                {

                    if (strCatchMessage.Trim() != "")
                    {
                        arrRet = new string[2];
                        arrRet[0] = "catch";
                        arrayCode = new string[1];
                        arrayCode[0] = strCatchMessage.Trim();
                        arrRet[1] = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
                        arrRet[1] = arrRet[1].Replace("&nbsp;", "");

                    }
                    else
                    {
                        arrRet = new string[2];
                        arrRet[0] = "false";
                        strReturnMsg = strReturnMsg.Trim();
                        arrayCode = strReturnMsg.Split(objComm.RecordDivider);
                        arrRet[1] = objComm.SetErrorResources(arrayCode, "N", true, objCore.USER_NAME, string.Empty);
                        arrRet[1] = arrRet[1].Replace("&nbsp;", "");
                    }
                }
            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrayCode = new string[1];
                arrayCode[0] = expErr.Message.Trim();
                arrRet[1] = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
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