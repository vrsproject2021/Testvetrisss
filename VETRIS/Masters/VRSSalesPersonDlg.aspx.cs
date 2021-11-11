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
     [AjaxPro.AjaxNamespace("VRSSalesPersonDlg")]
    public partial class VRSSalesPersonDlg : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Master.SalesPerson objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSSalesPersonDlg));
            SetAttributes();
            if (!CallBackInst.CausedCallback)
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

            btnAddInst.Attributes.Add("onclick", "javascript:btnAddInst_OnClick();");
        }
        #endregion

        #region LoadDetails
        private void LoadDetails(int intMenuID, Guid UserID)
        {
            objCore = new Core.Master.SalesPerson();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objCore.ID = new Guid(hdnID.Value);
                objCore.MENU_ID = intMenuID;
                objCore.USER_ID = UserID;

                bReturn = objCore.LoadDetails(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {

                    PopulateDropdowns(ds);
                    txtCode.Text = objCore.CODE;
                    txtFName.Text = objCore.FIRST_NAME;
                    txtLName.Text = objCore.LAST_NAME;

                    txtAddr1.Text = objCore.ADDRESS_LINE1;
                    txtAddr2.Text = objCore.ADDRESS_LINE2;
                    txtCity.Text = objCore.CITY;
                    ddlCountry.SelectedValue = objCore.COUNTRY_ID.ToString();
                    ddlState.SelectedValue = objCore.STATE_ID.ToString();
                    txtZip.Text = objCore.ZIP;

                    txtEmailID.Text = objCore.EMAIL_ID;
                    txtTel.Text = objCore.PHONE;
                    txtMobile.Text = objCore.MOBILE;

                    txtLoginID.Text = objCore.LOGIN_ID;
                    txtPwd.Attributes.Add("value",objCore.LOGIN_PASSWORD);
                    txtPACSUserID.Text = objCore.PACS_USER_ID;
                    txtPACSPwd.Attributes.Add("value",objCore.PACS_USER_PASSWORD); 
                    //txtPACSPwd.Attributes.Add("value", objCore.PACS_USER_PASSWORD);

                    if (objCore.IS_ACTIVE == "Y") rdoStatYes.Checked = true;
                    else if (objCore.IS_ACTIVE == "N") rdoStatNo.Checked = true;

                    if (objCore.NOTIFICATION_PREFERENCE == "B") rdoBoth.Checked = true;
                    else if (objCore.NOTIFICATION_PREFERENCE == "E") rdoEmail.Checked = true;
                    else if (objCore.NOTIFICATION_PREFERENCE == "S") rdoSMS.Checked = true;

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

            #region Institutions

            DataRow dr3 = ds.Tables["Institutions"].NewRow();
            dr3["id"] = "00000000-0000-0000-0000-000000000000";
            dr3["name"] = "Select One";
            ds.Tables["Institutions"].Rows.InsertAt(dr3, 0);
            ds.Tables["Institutions"].AcceptChanges();

            foreach (DataRow dr in ds.Tables["Institutions"].Rows)
            {
                if (hdnInstitutions.Value.Trim() != string.Empty) hdnInstitutions.Value += objComm.RecordDivider;
                hdnInstitutions.Value += Convert.ToString(dr["id"]) + objComm.RecordDivider;
                hdnInstitutions.Value += Convert.ToString(dr["name"]).Trim();
            }

            #endregion

            #region Users
            DataRow dr4 = ds.Tables["Users"].NewRow();
            dr4["id"] = "00000000-0000-0000-0000-000000000000";
            dr4["name"] = "Select One";
            ds.Tables["Users"].Rows.InsertAt(dr4, 0);
            ds.Tables["Users"].AcceptChanges();

            foreach (DataRow dr in ds.Tables["Users"].Rows)
            {
                if (hdnUsers.Value.Trim() != string.Empty) hdnUsers.Value += objComm.RecordDivider;
                hdnUsers.Value += Convert.ToString(dr["id"]) + objComm.RecordDivider;
                hdnUsers.Value += Convert.ToString(dr["name"]).Trim();
            }
            #endregion

        }
        #endregion

        #region Institution Grid

        #region CallBackInst_Callback
        protected void CallBackInst_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];

            switch (strAction)
            {
                case "L":
                    LoadInstitutions(e.Parameters);
                    break;
                case "A":
                    AddInstitution(e.Parameters);
                    break;
                case "D":
                    DeletePhysician(e.Parameters);
                    break;
            }

            grdInst.Width = Unit.Percentage(100);
            grdInst.RenderControl(e.Output);
            spnErrInst.RenderControl(e.Output);
        }
        #endregion

        #region LoadInstitutions
        private void LoadInstitutions(string[] arrParams)
        {
            objCore = new Core.Master.SalesPerson();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";

            try
            {

                objCore.ID = new Guid(arrParams[1]);

                bReturn = objCore.LoadInstitutions(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                   

                    grdInst.DataSource = ds.Tables["Institutions"];
                    grdInst.DataBind();


                    spnErrInst.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrInst\" value=\"\" />";
                }
                else
                    spnErrInst.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrInst\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnErrInst.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrInst\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region AddInstitution
        private void AddInstitution(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);

            int intSrl = 0;

            try
            {
                dtbl = CreateTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 3)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["rec_id"] = intSrl;
                            dr["institution_id"] = arrRecords[i + 1].Trim();
                            dr["salesperson_user_id"] = arrRecords[i + 2].Trim();
                            dr["del"] = "";
                            dtbl.Rows.Add(dr);
                        }

                    }
                }

                DataRow drNew = dtbl.NewRow();
                intSrl = intSrl + 1;
                drNew["rec_id"] = intSrl;
                drNew["institution_id"] = "00000000-0000-0000-0000-000000000000";
                drNew["salesperson_user_id"] = "00000000-0000-0000-0000-000000000000";
                drNew["del"] = "";
                dtbl.Rows.Add(drNew);

                grdInst.DataSource = dtbl;
                grdInst.DataBind();
                spnErrInst.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrInst\" value=\"\" />";
            }
            catch (Exception ex)
            {
                spnErrInst.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrInst\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region DeletePhysician
        private void DeletePhysician(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            int intID = Convert.ToInt32(arrParams[1]);
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;

            try
            {
                dtbl = CreateTable();
                if (arrRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrRecords.Length; i = i + 3)
                    {
                        if (Convert.ToInt32(arrRecords[i]) != intID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["rec_id"] = intSrl;
                            dr["institution_id"] = arrRecords[i + 1].Trim();
                            dr["salesperson_user_id"] = arrRecords[i + 2].Trim();
                            dr["del"] = "";
                            dtbl.Rows.Add(dr);
                        }
                    }
                }

                grdInst.DataSource = dtbl;
                grdInst.DataBind();
                spnErrInst.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrInst\" value=\"\" />";

            }
            catch (Exception ex)
            {
                spnErrInst.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrInst\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region CreateTable
        private DataTable CreateTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("rec_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("institution_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("salesperson_user_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("del", System.Type.GetType("System.String"));
            dtbl.TableName = "Institutions";
            return dtbl;
        }
        #endregion

        #endregion

        #region FetchStates (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchStates(string[] arrParams)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objCore = new Core.Master.SalesPerson();

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
        public string[] SaveRecord(string[] ArrRecord,string[] ArrInst)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Master.SalesPerson();
            objComm = new classes.CommonClass();

            Core.Master.InstitutionList[] objInst = new Core.Master.InstitutionList[0];

            try
            {
                objCore.ID = new Guid(ArrRecord[0].Trim());
                objCore.FIRST_NAME= ArrRecord[1].Trim();
                objCore.LAST_NAME = ArrRecord[2].Trim();
                objCore.IS_ACTIVE = ArrRecord[3].Trim();
                objCore.ADDRESS_LINE1 = ArrRecord[4].Trim();
                objCore.ADDRESS_LINE2 = ArrRecord[5].Trim();
                objCore.CITY = ArrRecord[6].Trim();
                objCore.COUNTRY_ID = Convert.ToInt32(ArrRecord[7]);
                objCore.STATE_ID = Convert.ToInt32(ArrRecord[8]);
                objCore.ZIP = ArrRecord[9].Trim();
                objCore.EMAIL_ID = ArrRecord[10].Trim();
                objCore.PHONE = ArrRecord[11].Trim();
                objCore.MOBILE = ArrRecord[12].Trim();
                objCore.LOGIN_ID = ArrRecord[13].Trim();
                if (ArrRecord[14].Trim() != string.Empty) objCore.LOGIN_PASSWORD = CoreCommon.EncryptString(ArrRecord[14].Trim());
                else objCore.LOGIN_PASSWORD = string.Empty;

                objCore.PACS_USER_ID = ArrRecord[15].Trim();
                if (ArrRecord[16].Trim() != string.Empty) objCore.PACS_USER_PASSWORD = CoreCommon.EncryptString(ArrRecord[16].Trim());
                else objCore.PACS_USER_PASSWORD = string.Empty;
                objCore.NOTIFICATION_PREFERENCE = ArrRecord[17].Trim();
                objCore.USER_ID = new Guid(ArrRecord[18].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[19]);

                objInst = new Core.Master.InstitutionList[(ArrInst.Length/4)];

                #region populate Institution details
                for (int i = 0; i < objInst.Length; i++)
                {
                    objInst[i] = new Core.Master.InstitutionList();
                    objInst[i].ID = new Guid(ArrInst[intListIndex]);
                    objInst[i].COMMISSION_1ST_YEAR = Convert.ToDecimal(ArrInst[intListIndex + 2].Trim());
                    objInst[i].COMMISSION_2ND_YEAR = Convert.ToDecimal(ArrInst[intListIndex + 3].Trim());
                    intListIndex = intListIndex + 4;
                }
                #endregion


                bReturn = objCore.SaveRecord(Server.MapPath("~"),objInst, ref strReturnMsg, ref strCatchMessage);

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