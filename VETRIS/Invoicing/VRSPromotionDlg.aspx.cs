using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using System.Net;
using System.Configuration;
using VETRIS.Core;
using AjaxPro;

namespace VETRIS.Invoicing
{
    [AjaxPro.AjaxNamespace("VRSPromotionDlg")]
    public partial class VRSPromotionDlg : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Invoicing.Promotion objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSPromotionDlg));
            SetAttributes();
            if ((!CallBackPromo.CausedCallback))
                SetPageValue();
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            objComm = new classes.CommonClass();
            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);

            hdnID.Value = Request.QueryString["id"];
            
            objComm.SetRegionalFormat();
            txtCreatedDate.Text     = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            txtFromDate.Text        = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            txtToDate.Text          = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            string strTheme = Request.QueryString["th"];
            SetCSS(strTheme);
            LoadDetails(intMenuID, UserID);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkTHEME.Attributes["href"] = strServerPath + "/css/" + strTheme + "/theme.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
            lnkCAL.Attributes["href"] = strServerPath + "/css/" + strTheme + "/CalendarStyle.css";
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnAdd1.Attributes.Add("onclick", "javascript:btnNew_OnClick();");
            btnAdd2.Attributes.Add("onclick", "javascript:btnNew_OnClick();");
            btnSave1.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnSave2.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

            ddlType.Attributes.Add("onchange", "javascript:ddlType_OnChange();");
            ddlAccount.Attributes.Add("onchange", "javascript:ddlAccount_OnChange();");

            txtFromDate.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgFromDt.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtFromDate.ClientID + "','CalFromDate');");
            txtToDate.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgToDt.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtToDate.ClientID + "','CalToDate');");
            btnAddPromo.Attributes.Add("onclick", "javascript:btnAddPromo_OnClick();");
           

        }
        #endregion

        #region LoadDetails
        private void LoadDetails(int intMenuID, Guid UserID)
        {
            objCore = new Core.Invoicing.Promotion();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();
            objCore = new Core.Invoicing.Promotion();

            try
            {
                objComm.SetRegionalFormat();
                objCore.ID = new Guid(hdnID.Value);
                objCore.MENU_ID = intMenuID;
                objCore.USER_ID = UserID;

                bReturn = objCore.LoadDetails(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {

                    PopulateDropdowns(ds);
                    ddlType.SelectedValue = objCore.TYPE.ToString();
                    ddlReason.SelectedValue = objCore.REASON_ID.ToString();
                    ddlAccount.SelectedValue = objCore.BILLING_ACCOUNT_ID.ToString();
                    if (objCore.STATUS == "Y") rdoStatYes.Checked = true;
                    else if (objCore.STATUS == "N") rdoStatNo.Checked = true;
                    txtCreatedDate.Text = objComm.IMDateFormat(objCore.CREATED_DATE, objComm.DateFormat);
                    if (objCore.FROM_DATE.Year > 1900) txtFromDate.Text = objComm.IMDateFormat(objCore.FROM_DATE, objComm.DateFormat); else txtFromDate.Text = "";
                    txtToDate.Text = objComm.IMDateFormat(objCore.TO_DATE, objComm.DateFormat);
                    

                }
                else
                {
                    if (strCatchMessage.Trim() != string.Empty)
                        hdnError.Value = "catch" + objComm.RecordDivider + strCatchMessage.Trim();
                    else
                        hdnError.Value = "false" + objComm.RecordDivider + strReturnMessage.Trim();
                }

            }
            catch (Exception ex)
            {
                hdnError.Value = "catch" + objComm.RecordDivider + ex.Message.Trim();
            }
            finally
            {
                ds.Dispose(); objCore = null; objComm = null;
            }
        }
        #endregion

        #region PopulateDropdowns
        private void PopulateDropdowns(DataSet ds)
        {
           
            #region Billing Accounts
            DataRow dr2 = ds.Tables["Accounts"].NewRow();
            dr2["id"] = "00000000-0000-0000-0000-000000000000";
            dr2["name"] = "Select One";
            ds.Tables["Accounts"].Rows.InsertAt(dr2, 0);
            ds.Tables["Accounts"].AcceptChanges();

            ddlAccount.DataSource = ds.Tables["Accounts"];
            ddlAccount.DataValueField = "id";
            ddlAccount.DataTextField = "name";
            ddlAccount.DataBind();
            #endregion

            #region Promo Reasons
            DataRow dr3 = ds.Tables["Reasons"].NewRow();
            dr3["id"] = "00000000-0000-0000-0000-000000000000";
            dr3["reason"] = "Select One";
            ds.Tables["Reasons"].Rows.InsertAt(dr3, 0);
            ds.Tables["Reasons"].AcceptChanges();

            ddlReason.DataSource = ds.Tables["Reasons"];
            ddlReason.DataValueField = "id";
            ddlReason.DataTextField = "reason";
            ddlReason.DataBind();
            #endregion
        }
        #endregion

        #region Promotion Grid

        #region CallBackPromo_Callback
        protected void CallBackPromo_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = string.Empty;
            strAction = e.Parameters[0];

            switch (strAction)
            {
                case "L":
                    LoadPromoDetails(e.Parameters);
                    break;
                case "A":
                    AddPromotion(e.Parameters);
                    break;
                case "D":
                    DeletePromotion(e.Parameters);
                    break;
            }

            grdPromo.Width = Unit.Percentage(100);
            grdPromo.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
            spnInst.RenderControl(e.Output);
            spnMod.RenderControl(e.Output);
        }
        #endregion

        #region LoadPromoDetails
        private void LoadPromoDetails(string[] arrParams)
        {
            objCore = new Core.Invoicing.Promotion();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            StringBuilder sbInst = new StringBuilder();
            StringBuilder sbMod = new StringBuilder();
            objComm = new classes.CommonClass();

            try
            {
                objCore.ID = new Guid(arrParams[1]);
                objCore.BILLING_ACCOUNT_ID = new Guid(arrParams[2]);
                objCore.TYPE = arrParams[3];

                bReturn = objCore.LoadPromotions(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdPromo.DataSource = ds.Tables["Promotions"];
                    grdPromo.DataBind();
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

                    if (ds.Tables["Institutions"].Rows.Count > 0)
                    {
                        DataRow dr1 = ds.Tables["Institutions"].NewRow();
                        dr1["institution_id"] = "00000000-0000-0000-0000-000000000000";
                        dr1["institution_name"] = "Select One";
                        ds.Tables["Institutions"].Rows.InsertAt(dr1, 0);
                        ds.Tables["Institutions"].AcceptChanges();

                        foreach (DataRow dr in ds.Tables["Institutions"].Rows)
                        {
                            if (sbInst.ToString().Trim() != string.Empty) sbInst.Append(objComm.RecordDivider);
                            sbInst.Append(Convert.ToString(dr["institution_id"]) + objComm.RecordDivider);
                            sbInst.Append(Convert.ToString(dr["institution_name"]).Trim());
                        }
                    }
                    

                    if (ds.Tables["Modality"].Rows.Count > 0)
                    {
                        DataRow dr2 = ds.Tables["Modality"].NewRow();
                        dr2["id"] = "0";
                        dr2["name"] = "Select One";
                        ds.Tables["Modality"].Rows.InsertAt(dr2, 0);
                        ds.Tables["Modality"].AcceptChanges();

                        foreach (DataRow dr in ds.Tables["Modality"].Rows)
                        {
                            if (sbMod.ToString().Trim() != string.Empty) sbMod.Append(objComm.RecordDivider);
                            sbMod.Append(Convert.ToString(dr["id"]) + objComm.RecordDivider);
                            sbMod.Append(Convert.ToString(dr["name"]).Trim());
                        }
                    }

                    spnInst.InnerHtml = "<input type=\"hidden\" id=\"hdnInst\" value=\"" + sbInst.ToString() + "\" />";
                    spnMod.InnerHtml = "<input type=\"hidden\" id=\"hdnMod\" value=\"" + sbMod.ToString() + "\" />";
                }
                else
                {
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";
                    spnInst.InnerHtml = "<input type=\"hidden\" id=\"hdnInst\" value=\"\" />";
                    spnMod.InnerHtml = "<input type=\"hidden\" id=\"hdnMod\" value=\"\" />";
                }


                if (objCore.TYPE == "D") grdPromo.Levels[0].Columns[4].Visible = true;
                else if (objCore.TYPE == "F") grdPromo.Levels[0].Columns[5].Visible = true;
            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
                spnInst.InnerHtml = "<input type=\"hidden\" id=\"hdnInst\" value=\"\" />";
                spnMod.InnerHtml = "<input type=\"hidden\" id=\"hdnMod\" value=\"\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
                objComm = null;
            }
        }
        #endregion

        #region AddPromotion
        private void AddPromotion(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);
            string strInst = arrParams[2];
            string strMod = arrParams[3];
            string strPromoType = arrParams[4];

            int intSrl = 0;

            try
            {
                dtbl = CreatePromoTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 6)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["line_no"] = intSrl;
                            dr["id"] = arrRecords[i + 1].Trim();
                            dr["institution_id"] = arrRecords[i + 2].Trim();
                            dr["modality_id"] = Convert.ToInt32(arrRecords[i + 3]);
                            dr["discount_percent"] = Convert.ToDecimal(arrRecords[i + 4]);
                            dr["free_credits"] = Convert.ToInt32(arrRecords[i + 5]);
                            dr["del"] = "";
                            dtbl.Rows.Add(dr);
                        }

                    }
                }

                DataRow drNew = dtbl.NewRow();
                intSrl = intSrl + 1;
                drNew["line_no"] = intSrl;
                drNew["id"] = "00000000-0000-0000-0000-000000000000";
                drNew["institution_id"] = "00000000-0000-0000-0000-000000000000";
                drNew["modality_id"] = 0;
                drNew["discount_percent"] = 0;
                drNew["free_credits"] = 0;
                drNew["del"] = "";
                dtbl.Rows.Add(drNew);

                grdPromo.DataSource = dtbl;
                grdPromo.DataBind();
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
            spnInst.InnerHtml = "<input type=\"hidden\" id=\"hdnInst\" value=\"" + strInst + "\" />";
            spnMod.InnerHtml = "<input type=\"hidden\" id=\"hdnMod\" value=\"" + strMod + "\" />";
            if (strPromoType == "D") grdPromo.Levels[0].Columns[4].Visible = true;
            else if (strPromoType == "F") grdPromo.Levels[0].Columns[5].Visible = true;
        }
        #endregion

        #region DeletePromotion
        private void DeletePromotion(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            int intID = Convert.ToInt32(arrParams[1]);
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            string strInst = arrParams[3];
            string strMod = arrParams[4];
            string strPromoType = arrParams[5];
            int intSrl = 0;

            try
            {
                dtbl = CreatePromoTable();
                if (arrRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrRecords.Length; i = i + 6)
                    {
                        if (Convert.ToInt32(arrRecords[i]) != intID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["line_no"] = intSrl;
                            dr["id"] = arrRecords[i + 1].Trim();
                            dr["institution_id"] = arrRecords[i + 2].Trim();
                            dr["modality_id"] = Convert.ToInt32(arrRecords[i + 3]);
                            dr["discount_percent"] = Convert.ToDecimal(arrRecords[i + 4]);
                            dr["free_credits"] = Convert.ToInt32(arrRecords[i + 5]);
                            dr["del"] = "";
                            dtbl.Rows.Add(dr);
                        }
                    }
                }

                grdPromo.DataSource = dtbl;
                grdPromo.DataBind();
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }

            spnInst.InnerHtml = "<input type=\"hidden\" id=\"hdnInst\" value=\"" + strInst + "\" />";
            spnMod.InnerHtml = "<input type=\"hidden\" id=\"hdnMod\" value=\"" + strMod + "\" />";
            if (strPromoType == "D") grdPromo.Levels[0].Columns[4].Visible = true;
            else if (strPromoType == "F") grdPromo.Levels[0].Columns[5].Visible = true;
        }
        #endregion

        #region CreatePromoTable
        private DataTable CreatePromoTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("line_no", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("institution_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("modality_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("discount_percent", System.Type.GetType("System.Decimal"));
            dtbl.Columns.Add("free_credits", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("del", System.Type.GetType("System.String"));
            dtbl.TableName = "Promotions";
            return dtbl;
        }
        #endregion

        #endregion

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord, string[] ArrPromo)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Invoicing.Promotion();
            objComm = new classes.CommonClass();

            Core.Invoicing.PromotionList[] objPromo = new Core.Invoicing.PromotionList[0];
            
           
            try
            {
                objCore.ID                  = new Guid(ArrRecord[0].Trim());
                objCore.TYPE                = ArrRecord[1].Trim();
                objCore.BILLING_ACCOUNT_ID  = new Guid(ArrRecord[2].Trim());
                objCore.FROM_DATE           = Convert.ToDateTime(ArrRecord[3].Trim());
                objCore.TO_DATE             = Convert.ToDateTime(ArrRecord[4].Trim());
                objCore.STATUS              = ArrRecord[5].Trim();
                objCore.REASON_ID           = new Guid(ArrRecord[6].Trim());
                objCore.USER_ID             = new Guid(ArrRecord[7].Trim());
                objCore.MENU_ID             = Convert.ToInt32(ArrRecord[8]);

                objPromo = new Core.Invoicing.PromotionList[(ArrPromo.Length/6)];

                #region populate promotion details
                for (int i = 0; i < objPromo.Length; i++)
                {
                    objPromo[i]                  = new Core.Invoicing.PromotionList();
                    objPromo[i].LINE_NUMBER      = Convert.ToInt32(ArrPromo[intListIndex]);
                    objPromo[i].ID               = new Guid(ArrPromo[intListIndex + 1]);
                    objPromo[i].INSTITUTION_ID   = new Guid(ArrPromo[intListIndex + 2]);
                    objPromo[i].MODALITY_ID      = Convert.ToInt32(ArrPromo[intListIndex + 3]);
                    objPromo[i].DISCOUNT_PERCENT = Convert.ToDecimal(ArrPromo[intListIndex + 4]);
                    objPromo[i].FREE_CREDITS     = Convert.ToInt32(ArrPromo[intListIndex + 5]);
                    intListIndex = intListIndex + 6;
                }
                #endregion

                intListIndex = 0;

                bReturn = objCore.SaveRecord(Server.MapPath("~"), objPromo, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {

                    arrRet = new string[4];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
                    arrRet[2] = objCore.ID.ToString();
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
                        arrRet[2] = objCore.USER_NAME;
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