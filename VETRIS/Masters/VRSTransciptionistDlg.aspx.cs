using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using System.Configuration;
using VETRIS.Core;
using AjaxPro;


namespace VETRIS.Masters
{
     [AjaxPro.AjaxNamespace("VRSTransciptionistDlg")]
    public partial class VRSTransciptionistDlg : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Master.Transciptionist objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSTransciptionistDlg));
            SetAttributes();
            if (!CallBackModality.CausedCallback)
                 SetPageValue();
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            string strTheme = Request.QueryString["th"];
            SetCSS(strTheme);
            hdnID.Value = Request.QueryString["id"];
            LoadDetails(intMenuID, UserID);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
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

            txtCode.Attributes.Add("onblur", "javascript:ConvertToUpperCase(this);");
            ddlCountry.Attributes.Add("onchange", "javascript:ddlCountry_OnChange();");

            //txtDefaultFee.Attributes.Add("onfocus", "javascript:this.select();");
            //txtDefaultFee.Attributes.Add("onkeypress", "javascript: return parent.CheckDecimal(event);");
            //txtDefaultFee.Attributes.Add("onblur", "javascript:ResetValueDecimal(this);");
        }
        #endregion

        #region LoadDetails
        private void LoadDetails(int intMenuID, Guid UserID)
        {
            objCore = new Core.Master.Transciptionist();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            DataSet ds = new DataSet();
            int intDefCountryID = 0;
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.ID = new Guid(hdnID.Value);
                objCore.MENU_ID = intMenuID;
                objCore.USER_ID = UserID;

                bReturn = objCore.LoadDetails(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {

                    PopulateDropdowns(ds, ref intDefCountryID);
                    txtCode.Text                = objCore.CODE;
                    txtFName.Text               = objCore.FIRST_NAME;
                    txtLName.Text               = objCore.LAST_NAME;
                    txtAddr1.Text               = objCore.ADDRESS_LINE1;
                    txtAddr2.Text               = objCore.ADDRESS_LINE2;
                    txtCity.Text                = objCore.CITY;
                    ddlCountry.SelectedValue    = objCore.COUNTRY_ID.ToString(); if (objCore.COUNTRY_ID == 0) { ddlCountry.SelectedValue = intDefCountryID.ToString(); }
                    ddlState.SelectedValue      = objCore.STATE_ID.ToString();
                    txtZip.Text                 = objCore.ZIP;
                    txtEmailID.Text             = objCore.EMAIL_ID;
                    txtTel.Text                 = objCore.PHONE;
                    txtMobile.Text              = objCore.MOBILE;

                    txtLoginID.Text             = objCore.LOGIN_ID;
                    txtPwd.Attributes.Add("value",objCore.LOGIN_PASSWORD);


                    //txtDefaultFee.Text          = Convert.ToString(objCore.DEFAULT_FEE);
                    //txtDefaultFee.Text          = objComm.IMNumeric(objCore.DEFAULT_FEE, objComm.DecimalDigit);

                    if (objCore.IS_ACTIVE == "Y") rdoStatYes.Checked = true;
                    else if (objCore.IS_ACTIVE == "N") rdoStatNo.Checked = true;

                    if (objCore.NOTIFICATION_PREFERENCE == "B") rdoBoth.Checked = true;
                    else if (objCore.NOTIFICATION_PREFERENCE == "E") rdoEmail.Checked = true;
                    else if (objCore.NOTIFICATION_PREFERENCE == "S") rdoSMS.Checked = true;

                    txtNotes.Text = objCore.NOTES;

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
        private void PopulateDropdowns(DataSet ds,ref int DefaultCountryID)
        {
            #region Country
            DataRow dr1 = ds.Tables["Country"].NewRow();
            dr1["id"] = 0;
            dr1["name"] = "Select One";
            ds.Tables["Country"].Rows.InsertAt(dr1, 0);
            ds.Tables["Country"].AcceptChanges();

            ddlCountry.DataSource = ds.Tables["Country"];
            ddlCountry.DataValueField = "id";
            ddlCountry.DataTextField = "name";
            ddlCountry.DataBind();

            DataView dv = new DataView(ds.Tables["Country"]);
            dv.RowFilter = "is_default = 'Y'";
            if (dv.ToTable().Rows.Count > 0)
            {
                DefaultCountryID = Convert.ToInt32(dv.ToTable().Rows[0]["id"]);
            }
            #endregion

            #region States
            DataRow dr2 = ds.Tables["States"].NewRow();
            dr2["id"] = 0;
            dr2["name"] = "Select One";
            ds.Tables["States"].Rows.InsertAt(dr2, 0);
            ds.Tables["States"].AcceptChanges();

            ddlState.DataSource = ds.Tables["States"];
            ddlState.DataValueField = "id";
            ddlState.DataTextField = "name";
            ddlState.DataBind();
            #endregion

        }
        #endregion

        #region Modality Grid

        #region CallBackModality_Callback
        protected void CallBackModality_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadModality(e.Parameters);
            grdModality.Width = Unit.Percentage(100);
            grdModality.RenderControl(e.Output);
            spnErr.RenderControl(e.Output);

        }
        #endregion

        #region LoadModality
        private void LoadModality(string[] arrParams)
        {
            objCore = new Core.Master.Transciptionist();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;

            try
            {

                objCore.ID = new Guid(arrParams[0]);
                bReturn = objCore.LoadModality(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdModality.DataSource = ds.Tables["Modality"];
                    grdModality.DataBind();
                    spnErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";
                }
                else
                {

                    spnErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";
                }
            }
            catch (Exception ex)
            {
                spnErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";

            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #endregion

        #region FetchStates (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchStates(string[] arrParams)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objCore = new Core.Master.Transciptionist();

            int i = 0;
            string[] arrRet = new string[0];

            try
            {

                objCore.COUNTRY_ID = Convert.ToInt32(arrParams[0].Trim());
                bReturn = objCore.FetchCountryWiseStates(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    if (ds.Tables["States"].Rows.Count > 0)
                    {

                        arrRet = new string[(ds.Tables["States"].Rows.Count * 2) + 3];
                        arrRet[0] = "true";
                        arrRet[1] = "0";
                        arrRet[2] = "Select One";
                        i = 3;

                        foreach (DataRow dr in ds.Tables["States"].Rows)
                        {
                            arrRet[i] = Convert.ToString(dr["id"]);
                            arrRet[i + 1] = Convert.ToString(dr["name"]).Trim();
                            i = i + 2;
                        }
                    }
                    else
                    {
                        arrRet = new string[3];
                        arrRet[0] = "true";
                        arrRet[1] = "0";
                        arrRet[2] = "Select One";
                    }



                }
                else
                {

                    arrRet = new string[2];
                    arrRet[0] = "false";
                    arrRet[1] = strCatchMessage.Trim();
                }

            }
            catch (Exception ex)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return arrRet;
        }
        #endregion

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord, string[] arrMod)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            Core.Master.TranscriptionistModalityList[] objMod = new Core.Master.TranscriptionistModalityList[0];
            int intListIndex = 0;

            objCore = new Core.Master.Transciptionist();
            objComm = new classes.CommonClass();

            try
            {
                objCore.ID                  = new Guid(ArrRecord[0].Trim());
                objCore.FIRST_NAME          = ArrRecord[1].Trim();
                objCore.LAST_NAME           = ArrRecord[2].Trim();
                objCore.IS_ACTIVE           = ArrRecord[3].Trim();
                objCore.ADDRESS_LINE1       = ArrRecord[4].Trim();
                objCore.ADDRESS_LINE2       = ArrRecord[5].Trim();
                objCore.CITY                = ArrRecord[6].Trim();
                objCore.COUNTRY_ID          = Convert.ToInt32(ArrRecord[7]);
                objCore.STATE_ID            = Convert.ToInt32(ArrRecord[8]);
                objCore.ZIP                 = ArrRecord[9].Trim();
                objCore.EMAIL_ID            = ArrRecord[10].Trim();
                objCore.PHONE               = ArrRecord[11].Trim();
                objCore.MOBILE              = ArrRecord[12].Trim();
                objCore.LOGIN_USER_ID       = new Guid(ArrRecord[13].Trim());
                objCore.LOGIN_ID            = ArrRecord[14].Trim();

                if (ArrRecord[15].Trim() != string.Empty)
                    objCore.LOGIN_PASSWORD  = CoreCommon.EncryptString(ArrRecord[15].Trim());
                else
                    objCore.LOGIN_PASSWORD  = string.Empty;

                objCore.NOTIFICATION_PREFERENCE = ArrRecord[16].Trim();
                objCore.NOTES                   = ArrRecord[17].Trim();
                objCore.USER_ID                 = new Guid(ArrRecord[18].Trim());
                objCore.MENU_ID                 = Convert.ToInt32(ArrRecord[19]);

                objMod = new Core.Master.TranscriptionistModalityList[(arrMod.Length / 3)];

                for (int i = 0; i < objMod.Length; i++)
                {
                    objMod[i] = new Core.Master.TranscriptionistModalityList();
                    objMod[i].SERIAL_NUMBER = i + 1;
                    objMod[i].ID = Convert.ToInt32(arrMod[intListIndex]);
                    objMod[i].DEFAULT_FEE= Convert.ToDouble(arrMod[intListIndex + 1]);
                    objMod[i].ADDITION_FEE_FOR_STAT_PRELIM = Convert.ToDouble(arrMod[intListIndex + 2]);
                    intListIndex = intListIndex + 3;
                }
                intListIndex = 0;

                bReturn                     = objCore.SaveRecord(Server.MapPath("~"), objMod,ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[3];
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