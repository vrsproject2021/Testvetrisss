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

namespace VETRIS.Masters
{
    [AjaxPro.AjaxNamespace("VRSBillingAcctDlg")]
    public partial class VRSBillingAcctDlg : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Master.BillingAccount objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSBillingAcctDlg));
            SetAttributes();
            if ((!CallBackInst.CausedCallback) && (!CallBackMF.CausedCallback)&& (!CallBackSF.CausedCallback) && (!CallBackContact.CausedCallback) && (!CallBackPhys.CausedCallback))
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
            
            if (Request.QueryString["cf"] != null) hdnCF.Value = "MQ";
            if (Request.QueryString["cf"] != null) hdnCF.Value = Request.QueryString["cf"];
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
            lnkTHEME.Attributes["href"] = strServerPath + "/css/" + strTheme + "/theme.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
            lnkTAB.Attributes["href"] = strServerPath + "/css/" + strTheme + "/tabStyle1.css";
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

            txtCommission1stYr.Attributes.Add("onkeypress", "javascript:return parent.CheckDecimal(event);");
            txtCommission1stYr.Attributes.Add("onfocus", "javascript:this.select();");
            txtCommission1stYr.Attributes.Add("onblur", "javascript:ResetValueDecimal(this);");
            txtCommission2ndYr.Attributes.Add("onkeypress", "javascript:return parent.CheckDecimal(event);");
            txtCommission2ndYr.Attributes.Add("onfocus", "javascript:this.select();");
            txtCommission2ndYr.Attributes.Add("onblur", "javascript:ResetValueDecimal(this);");
           
            //txtDisc.Attributes.Add("onkeypress", "javascript:parent.CheckDecimal(event);");
            //txtDisc.Attributes.Add("onfocus", "javascript:this.select();");
            //txtDisc.Attributes.Add("onblur", "javascript:ResetValueDecimal(this);");
           
            //btnApplyDisc.Attributes.Add("onclick", "javascript:btnApplyDisc_OnChange();");
            btnApplyMF.Attributes.Add("onclick", "javascript:ApplyDefaultFee('M');");
            btnApplySF.Attributes.Add("onclick", "javascript:ApplyDefaultFee('S');");
        }
        #endregion

        #region LoadDetails
        private void LoadDetails(int intMenuID, Guid UserID)
        {
            objCore = new Core.Master.BillingAccount();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            DataSet ds = new DataSet();
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

                    PopulateDropdowns(ds);
                    txtCode.Text = objCore.CODE;
                    txtName.Text = objCore.NAME;
                    txtAddr1.Text = objCore.ADDRESS_LINE1;
                    txtAddr2.Text = objCore.ADDRESS_LINE2;
                    txtCity.Text = objCore.CITY;
                    ddlCountry.SelectedValue = objCore.COUNTRY_ID.ToString();
                    ddlState.SelectedValue = objCore.STATE_ID.ToString();
                    txtZip.Text = objCore.ZIP;
                    if (objCore.IS_ACTIVE == "Y") rdoStatYes.Checked = true;
                    else if (objCore.IS_ACTIVE == "N") rdoStatNo.Checked = true;

                    //txtEmailID.Text = objCore.EMAIL_ID;
                    //txtTel.Text = objCore.PHONE;
                    //txtFax.Text = objCore.FAX;
                    //txtContPerson.Text = objCore.CONTACT_PERSON_NAME;
                    //txtContMobile.Text = objCore.CONTACT_PERSION_MOBILE;
                    //txtContEmail.Text = objCore.CONTACT_PERSION_EMAIL_ID;

                    txtLoginID.Text = objCore.LOGIN_ID;
                    txtPwd.Attributes.Add("value",objCore.LOGIN_PASSWORD);
                    txtLoginEmail.Text = objCore.USER_EMAIL_ID;
                    txtUserMobile.Text = objCore.USER_MOBILE_NUMBER;
                    if (objCore.NOTIFICATION_PREFERENCE == "B") rdoBoth.Checked = true;
                    else if (objCore.NOTIFICATION_PREFERENCE == "E") rdoEmail.Checked = true;
                    else if (objCore.NOTIFICATION_PREFERENCE == "S") rdoSMS.Checked = true;
                   
                    ddlSalesPerson.SelectedValue = objCore.SALES_PERSON_ID.ToString();
                    txtCommission1stYr.Text = objComm.IMNumeric(objCore.COMMISSION_1ST_YEAR, objComm.DecimalDigit);
                    txtCommission2ndYr.Text = objComm.IMNumeric(objCore.COMMISSION_2ND_YEAR, objComm.DecimalDigit);
                    //txtDisc.Text = objComm.IMNumeric(objCore.DISCOUNT_PERCENT, objComm.DecimalDigit);
                    txtAccName.Text = objCore.ACCOUNTANT_NAME;
                   
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

            #region Sales Persons
            DataRow dr4 = ds.Tables["SalesPersons"].NewRow();
            dr4["id"] = "00000000-0000-0000-0000-000000000000";
            dr4["name"] = "Select One";
            ds.Tables["SalesPersons"].Rows.InsertAt(dr4, 0);
            ds.Tables["SalesPersons"].AcceptChanges();

            ddlSalesPerson.DataSource = ds.Tables["SalesPersons"];
            ddlSalesPerson.DataValueField = "id";
            ddlSalesPerson.DataTextField = "name";
            ddlSalesPerson.DataBind();
            #endregion

          
        }
        #endregion

        #region Contact Grid

        #region CallBackContact_Callback
        protected void CallBackContact_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            string strAction = e.Parameters[0];

            switch (strAction)
            {
                case "L":
                    LoadContacts(e.Parameters);
                    break;
                case "U":
                    UpdateContacts(e.Parameters);
                    break;
                case "D":
                    DeleteContact(e.Parameters);
                    break;
            }
            
            
            grdContact.Width = Unit.Percentage(100);
            grdContact.RenderControl(e.Output);
            spnErrCont.RenderControl(e.Output);
        }
        #endregion

        #region LoadContacts
        private void LoadContacts(string[] arrParams)
        {
            objCore = new Core.Master.BillingAccount();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                objCore.ID = new Guid(arrParams[1]);

                bReturn = objCore.LoadContacts(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdContact.DataSource = ds.Tables["Contacts"];
                    grdContact.DataBind();
                    spnErrCont.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCont\" value=\"\" />";
                }
                else
                    spnErrCont.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCont\" value=\"" + strCatchMessage + "\" />";
            }
            catch (Exception ex)
            {
                spnInstERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCont\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region UpdateContacts
        private void UpdateContacts(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrInstRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);
            Guid InstID = new Guid(arrParams[2]);
            objCore = new Core.Master.BillingAccount();
            int intSrl = 0;
            string strCatchMessage = ""; bool bReturn = false;

            try
            {
                dtbl = CreateContactTable();
                if (arrInstRecords.Length > 0)
                {
                    if (arrInstRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrInstRecords.Length; i = i + 8)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["rec_id"] = intSrl;
                            dr["institution_id"] = arrInstRecords[i + 1].Trim();
                            dr["institution_name"] = arrInstRecords[i + 2].Trim();
                            dr["phone_no"] = arrInstRecords[i + 3].Trim();
                            dr["fax_no"] = arrInstRecords[i + 4].Trim();
                            dr["contact_person_name"] = arrInstRecords[i + 5].Trim();
                            dr["contact_person_mobile"] = arrInstRecords[i + 6].Trim();
                            dr["contact_person_email_id"] = arrInstRecords[i + 7].Trim();
                            dtbl.Rows.Add(dr);
                        }

                    }
                }

               

                objCore.INSTITUTION_ID = InstID;
                bReturn = objCore.FechContactDetails(Server.MapPath("~"), ref strCatchMessage);

                if (bReturn)
                {
                    DataRow drNew = dtbl.NewRow();
                    intSrl = intSrl + 1;
                    drNew["rec_id"] = intSrl;
                    drNew["institution_id"] = arrParams[2];
                    drNew["institution_name"] = objCore.INSTITUTION_NAME;
                    drNew["phone_no"] = objCore.PHONE;
                    drNew["fax_no"] = objCore.FAX;
                    drNew["contact_person_name"] = objCore.CONTACT_PERSON_NAME;
                    drNew["contact_person_mobile"] = objCore.CONTACT_PERSION_MOBILE;
                    drNew["contact_person_email_id"] = objCore.CONTACT_PERSION_EMAIL_ID;
                    dtbl.Rows.Add(drNew);
                    
                    spnErrCont.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCont\" value=\"\" />";
                }
                else
                    spnErrCont.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCont\" value=\"" + strCatchMessage + "\" />";


                grdContact.DataSource = dtbl;
                grdContact.DataBind();
                
            }
            catch (Exception ex)
            {
                spnErrCont.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCont\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
                objCore = null;
            }
        }
        #endregion

        #region DeleteContact
        private void DeleteContact(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            Guid InstID = new Guid(arrParams[1]);
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrInstRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;

            try
            {
                dtbl = CreateContactTable();
                if (arrInstRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrInstRecords.Length; i = i + 8)
                    {
                        if (new Guid(arrInstRecords[i + 1]) != InstID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["rec_id"] = intSrl;
                            dr["institution_id"] = arrInstRecords[i + 1].Trim();
                            dr["institution_name"] = arrInstRecords[i + 2].Trim();
                            dr["phone_no"] = arrInstRecords[i + 3].Trim();
                            dr["fax_no"] = arrInstRecords[i + 4].Trim();
                            dr["contact_person_name"] = arrInstRecords[i + 5].Trim();
                            dr["contact_person_mobile"] = arrInstRecords[i + 6].Trim();
                            dr["contact_person_email_id"] = arrInstRecords[i + 7].Trim();
                            dtbl.Rows.Add(dr);
                        }
                    }
                }

                grdContact.DataSource = dtbl;
                grdContact.DataBind();
                spnErrCont.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCont\" value=\"\" />";

            }
            catch (Exception ex)
            {
                spnErrCont.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCont\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region CreateContactTable
        private DataTable CreateContactTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("rec_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("institution_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("institution_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("phone_no", System.Type.GetType("System.String"));
            dtbl.Columns.Add("fax_no", System.Type.GetType("System.String"));
            dtbl.Columns.Add("contact_person_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("contact_person_mobile", System.Type.GetType("System.String"));
            dtbl.Columns.Add("contact_person_email_id", System.Type.GetType("System.String"));
            dtbl.TableName = "Contacts";
            return dtbl;
        }
        #endregion

        #endregion

        #region Physician Grid

        #region CallBackPhys_Callback
        protected void CallBackPhys_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            string strAction = e.Parameters[0];

            switch (strAction)
            {
                case "L":
                    LoadPhysicians(e.Parameters);
                    break;
                case "U":
                    UpdatePhysicians(e.Parameters);
                    break;
                case "D":
                    DeletePhysician(e.Parameters);
                    break;
            }


            grdPhys.Width = Unit.Percentage(100);
            grdPhys.RenderControl(e.Output);
            spnErrPhys.RenderControl(e.Output);
        }
        #endregion

        #region LoadPhysicians
        private void LoadPhysicians(string[] arrParams)
        {
            objCore = new Core.Master.BillingAccount();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                objCore.ID = new Guid(arrParams[1]);

                bReturn = objCore.LoadPhysicians(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    ds.Relations.Add(ds.Tables["Institutions"].Columns["institution_id"], ds.Tables["Physicians"].Columns["institution_id"]);
                    grdPhys.DataSource = ds;
                    grdPhys.DataBind();
                    spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBPhysErr\" value=\"\" />";
                }
                else
                    spnErrCont.InnerHtml = "<input type=\"hidden\" id=\"hdnCBPhysErr\" value=\"" + strCatchMessage + "\" />";
            }
            catch (Exception ex)
            {
                spnInstERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBPhysErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region UpdatePhysicians
        private void UpdatePhysicians(string[] arrParams)
        {
            DataSet ds = new DataSet();
            DataSet dsNew = new DataSet();
            DataTable dtblInst = new DataTable();
            DataTable dtblPhys = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrInstRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);
            string[] arrPhysRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            Guid InstID = new Guid(arrParams[3]);
            objCore = new Core.Master.BillingAccount();
            string strCatchMessage = ""; bool bReturn = false;

            try
            {
                dtblInst = CreateInstTable();
                dtblPhys = CreatePhysicianTable();

                if (arrInstRecords.Length > 0)
                {
                    if (arrInstRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrInstRecords.Length; i = i + 3)
                        {
                            DataRow dr = dtblInst.NewRow();
                            dr["institution_id"] = arrInstRecords[i].Trim();
                            dr["code"] = arrInstRecords[i + 1].Trim();
                            dr["name"] = arrInstRecords[i + 2].Trim();
                            dtblInst.Rows.Add(dr);
                        }

                    }
                }
                if (arrPhysRecords.Length > 0)
                {
                    if (arrPhysRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrPhysRecords.Length; i = i + 7)
                        {
                            DataRow dr = dtblPhys.NewRow();
                            dr["institution_id"] = arrPhysRecords[i].Trim();
                            dr["physician_id"] = arrPhysRecords[i + 1].Trim();
                            dr["physician_fname"] = arrPhysRecords[i + 2].Trim();
                            dr["physician_lname"] = arrPhysRecords[i + 3].Trim();
                            dr["physician_credentials"] = arrPhysRecords[i + 4].Trim();
                            dr["physician_email"] = arrPhysRecords[i + 5].Trim();
                            dr["physician_mobile"] = arrPhysRecords[i + 6].Trim();
                            dtblPhys.Rows.Add(dr);
                        }

                    }
                }


                objCore.INSTITUTION_ID = InstID;
                bReturn = objCore.FetchPhysicianDetails(Server.MapPath("~"), ref dsNew, ref strCatchMessage);

                if (bReturn)
                {
                    foreach (DataRow dr in dsNew.Tables["Institutions"].Rows)
                    {
                        DataRow drNew = dtblInst.NewRow();
                        drNew["institution_id"] = arrParams[3];
                        drNew["code"] = Convert.ToString(dr["code"]).Trim();
                        drNew["name"] = Convert.ToString(dr["name"]).Trim();
                        dtblInst.Rows.Add(drNew);
                    }
                    foreach (DataRow dr in dsNew.Tables["Physicians"].Rows)
                    {
                        DataRow drNew = dtblPhys.NewRow();
                        drNew["institution_id"] = arrParams[3];
                        drNew["physician_id"] = Convert.ToString(dr["physician_id"]);
                        drNew["physician_fname"] = Convert.ToString(dr["physician_fname"]).Trim();
                        drNew["physician_lname"] = Convert.ToString(dr["physician_lname"]).Trim();
                        drNew["physician_credentials"] = Convert.ToString(dr["physician_credentials"]).Trim();
                        drNew["physician_email"] = Convert.ToString(dr["physician_email"]).Trim();
                        drNew["physician_mobile"] = Convert.ToString(dr["physician_mobile"]).Trim();
                        dtblPhys.Rows.Add(drNew);
                    }

                    spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBPhysErr\" value=\"\" />";
                }
                else
                    spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBPhysErr\" value=\"" + strCatchMessage + "\" />";

                ds.Tables.Add(dtblInst);
                ds.Tables.Add(dtblPhys);
                ds.Relations.Add(ds.Tables["Institutions"].Columns["institution_id"], ds.Tables["Physicians"].Columns["institution_id"]);
                grdPhys.DataSource = ds;
                grdPhys.DataBind();

            }
            catch (Exception ex)
            {
                spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBPhysErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtblInst.Dispose();
                dtblPhys.Dispose();
                ds.Dispose();
                dsNew.Dispose();
                objComm = null;
                objCore = null;
            }
        }
        #endregion

        #region DeletePhysician
        private void DeletePhysician(string[] arrParams)
        {
            DataSet ds = new DataSet();
            DataTable dtblInst = new DataTable();
            DataTable dtblPhys = new DataTable();
            objComm = new classes.CommonClass();
            Guid InstID = new Guid(arrParams[1]);
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrInstRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            string[] arrPhysRecords = arrParams[3].Split(arrSep, StringSplitOptions.None);
            dtblInst = CreateInstTable();
            dtblPhys = CreatePhysicianTable();
            int intSrl = 0;

            try
            {
                dtblInst = CreateInstTable();
                dtblPhys = CreatePhysicianTable();

                if (arrInstRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrInstRecords.Length; i = i + 3)
                    {
                        if (new Guid(arrInstRecords[i]) != InstID)
                        {
                            DataRow dr = dtblInst.NewRow();
                            dr["institution_id"] = arrInstRecords[i].Trim();
                            dr["code"] = arrInstRecords[i + 1].Trim();
                            dr["name"] = arrInstRecords[i + 2].Trim();
                            dtblInst.Rows.Add(dr);
                        }
                    }
                }
                if (arrPhysRecords.Length > 0)
                {
                    if (arrPhysRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrPhysRecords.Length; i = i + 7)
                        {
                            if (new Guid(arrPhysRecords[i]) != InstID)
                            {
                                DataRow dr = dtblPhys.NewRow();
                                dr["institution_id"] = arrPhysRecords[i].Trim();
                                dr["physician_id"] = arrPhysRecords[i + 1].Trim();
                                dr["physician_fname"] = arrPhysRecords[i + 2].Trim();
                                dr["physician_lname"] = arrPhysRecords[i + 3].Trim();
                                dr["physician_credentials"] = arrPhysRecords[i + 4].Trim();
                                dr["physician_email"] = arrPhysRecords[i + 5].Trim();
                                dr["physician_mobile"] = arrPhysRecords[i + 6].Trim();
                                dtblPhys.Rows.Add(dr);
                            }
                        }

                    }
                }

                ds.Tables.Add(dtblInst);
                ds.Tables.Add(dtblPhys);
                ds.Relations.Add(ds.Tables["Institutions"].Columns["institution_id"], ds.Tables["Physicians"].Columns["institution_id"]);
                grdPhys.DataSource = ds;
                grdPhys.DataBind();
                spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBPhysErr\" value=\"\" />";

            }
            catch (Exception ex)
            {
                spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBPhysErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtblInst.Dispose();
                dtblPhys.Dispose();
                ds.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region CreatePhysicianTable
        private DataTable CreatePhysicianTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("institution_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("physician_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("physician_fname", System.Type.GetType("System.String"));
            dtbl.Columns.Add("physician_lname", System.Type.GetType("System.String"));
            dtbl.Columns.Add("physician_credentials", System.Type.GetType("System.String"));
            dtbl.Columns.Add("physician_email", System.Type.GetType("System.String"));
            dtbl.Columns.Add("physician_mobile", System.Type.GetType("System.String"));
            dtbl.TableName = "Physicians";
            return dtbl;
        }
        #endregion

        #region CreateInstTable
        private DataTable CreateInstTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("institution_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("code", System.Type.GetType("System.String"));
            dtbl.Columns.Add("name", System.Type.GetType("System.String"));
            dtbl.TableName = "Institutions";
            return dtbl;
        }
        #endregion

        #endregion

        #region Institution Grid

        #region CallBackInst_Callback
        protected void CallBackInst_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadInstitutions(e.Parameters);
            grdInst.Width = Unit.Percentage(99);
            grdInst.RenderControl(e.Output);
            spnInstERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadInstitutions
        private void LoadInstitutions(string[] arrParams)
        {
            objCore = new Core.Master.BillingAccount();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                objCore.ID = new Guid(arrParams[0]);

                bReturn = objCore.LoadInstitutions(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdInst.DataSource = ds.Tables["Institutions"];
                    grdInst.DataBind();
                    spnInstERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrInst\" value=\"\" />";
                }
                else
                    spnInstERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrInst\" value=\"" + strCatchMessage + "\" />";
            }
            catch (Exception ex)
            {
                spnInstERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrInst\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #endregion

        #region Fees Grid

        #region CallBackMF_Callback
        protected void CallBackMF_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            objCore = new Core.Master.BillingAccount();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                objCore.ID = new Guid(e.Parameters[0]);
                objCore.APPLY_DEFAULT_FEES = Convert.ToString(e.Parameters[1]);

                bReturn = objCore.LoadModalityFees(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdMF.DataSource = ds.Tables["ModalityFees"];
                    grdMF.DataBind();
                    spnMFERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrMF\" value=\"\" />";
                }
                else
                    spnMFERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrMF\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnMFERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrMF\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }

            grdMF.Width = Unit.Percentage(100);
            grdMF.RenderControl(e.Output);
            spnMFERR.RenderControl(e.Output);
        }
        #endregion

        #region CallBackSF_Callback
        protected void CallBackSF_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            objCore = new Core.Master.BillingAccount();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                objCore.ID = new Guid(e.Parameters[0]);
                objCore.APPLY_DEFAULT_FEES = Convert.ToString(e.Parameters[1]);

                bReturn = objCore.LoadServiceFees(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdSF.DataSource = ds.Tables["ServiceFees"];
                    grdSF.DataBind();
                    spnSvcErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSF\" value=\"\" />";
                }
                else
                    spnSvcErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSF\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnSvcErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSF\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }

            grdSF.Width = Unit.Percentage(100);
            grdSF.RenderControl(e.Output);
            spnSvcErr.RenderControl(e.Output);
        }
        #endregion

        #endregion

        #region FetchStates (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchStates(string[] arrParams)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objCore = new Core.Master.BillingAccount();

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
        public string[] SaveRecord(string[] ArrRecord, string[] ArrInst, string[] ArrCont, string[] ArrPhys, string[] ArrModFee,string[] ArrSvcFee)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Master.BillingAccount();
            objComm = new classes.CommonClass();

            Core.Master.BAInstitutionList[] objInst = new Core.Master.BAInstitutionList[0];
            Core.Master.ContactList[] objCont = new Core.Master.ContactList[0];
            Core.Master.PhysicianList[] objPhys = new Core.Master.PhysicianList[0];
            Core.Master.ModalityFeeList[] objModalityFees = new Core.Master.ModalityFeeList[0];
            Core.Master.ServiceFeeList[] objServiceFees = new Core.Master.ServiceFeeList[0];

            try
            {
                objCore.ID = new Guid(ArrRecord[0].Trim());
                objCore.CODE = ArrRecord[1].Trim();
                objCore.NAME = ArrRecord[2].Trim();
                objCore.IS_ACTIVE = ArrRecord[3].Trim();
                objCore.ADDRESS_LINE1 = ArrRecord[4].Trim();
                objCore.ADDRESS_LINE2 = ArrRecord[5].Trim();
                objCore.CITY = ArrRecord[6].Trim();
                objCore.COUNTRY_ID = Convert.ToInt32(ArrRecord[7]);
                objCore.STATE_ID = Convert.ToInt32(ArrRecord[8]);
                objCore.ZIP = ArrRecord[9].Trim();
                //objCore.EMAIL_ID = ArrRecord[10].Trim();
                //objCore.PHONE = ArrRecord[11].Trim();
                //objCore.FAX = ArrRecord[12].Trim();
                //objCore.CONTACT_PERSON_NAME = ArrRecord[13].Trim();
                //objCore.CONTACT_PERSION_MOBILE = ArrRecord[14].Trim();
                //objCore.CONTACT_PERSION_EMAIL_ID = ArrRecord[15].Trim();
                objCore.LOGIN_ID = ArrRecord[10].Trim();

                if (ArrRecord[17].Trim() != string.Empty)
                    objCore.LOGIN_PASSWORD = CoreCommon.EncryptString(ArrRecord[11].Trim());
                else
                    objCore.LOGIN_PASSWORD = string.Empty;

                objCore.USER_EMAIL_ID = ArrRecord[12].Trim();
                objCore.USER_MOBILE_NUMBER = ArrRecord[13].Trim();
                objCore.NOTIFICATION_PREFERENCE = ArrRecord[14].Trim();


                objCore.SALES_PERSON_ID = new Guid(ArrRecord[15]);
                objCore.COMMISSION_1ST_YEAR = Convert.ToDecimal(ArrRecord[16]);
                objCore.COMMISSION_2ND_YEAR = Convert.ToDecimal(ArrRecord[17]);
                objCore.DISCOUNT_PERCENT = Convert.ToDecimal(ArrRecord[18]);
                objCore.ACCOUNTANT_NAME = ArrRecord[19].Trim();;
                objCore.USER_ID = new Guid(ArrRecord[20].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[21]);

                objInst = new Core.Master.BAInstitutionList[(ArrInst.Length/3)];

                #region populate institution details
                for (int i = 0; i < objInst.Length; i++)
                {
                    objInst[i] = new Core.Master.BAInstitutionList();
                    objInst[i].ID = new Guid(ArrInst[intListIndex]);
                    objInst[i].CONSULT_APPLICABLE = ArrInst[intListIndex + 1].Trim();
                    objInst[i].STORAGE_APPLICABLE = ArrInst[intListIndex + 2].Trim();
                    intListIndex = intListIndex + 3;
                }
                #endregion

                intListIndex = 0;
                objCont = new Core.Master.ContactList[(ArrCont.Length / 6)];

                #region populate Contact details
                for (int i = 0; i < objCont.Length; i++)
                {
                    objCont[i] = new Core.Master.ContactList();
                    objCont[i].INSTITUTION_ID = new Guid(ArrCont[intListIndex]);
                    objCont[i].PHONE_NUMBER = ArrCont[intListIndex + 1].Trim();
                    objCont[i].FAX_NUMBER = ArrCont[intListIndex + 2].Trim();
                    objCont[i].CONTACT_PERSON = ArrCont[intListIndex + 3].Trim();
                    objCont[i].CONTACT_PERSON_MOBILE_NUMBER = ArrCont[intListIndex + 4].Trim();
                    objCont[i].CONTACT_PERSON_EMAIL_ID = ArrCont[intListIndex + 5].Trim();
                    intListIndex = intListIndex + 6;
                }
                #endregion

                intListIndex = 0;
                objPhys = new Core.Master.PhysicianList[(ArrPhys.Length / 7)];

                #region populate physician details
                for (int i = 0; i < objPhys.Length; i++)
                {
                    objPhys[i] = new Core.Master.PhysicianList();
                    objPhys[i].INSTITUTION_ID = new Guid(ArrPhys[intListIndex]);
                    objPhys[i].ID = new Guid(ArrPhys[intListIndex + 1]);
                    objPhys[i].FIRST_NAME = ArrPhys[intListIndex + 2].Trim();
                    objPhys[i].LAST_NAME = ArrPhys[intListIndex + 3].Trim();
                    objPhys[i].CREDENTIALS = ArrPhys[intListIndex + 4].Trim();
                    objPhys[i].EMAIL_ID = ArrPhys[intListIndex + 5].Trim();
                    objPhys[i].MOBILE_NUMBER = ArrPhys[intListIndex + 6].Trim();
                    intListIndex = intListIndex + 7;
                }
                #endregion

                intListIndex = 0;

                objModalityFees = new Core.Master.ModalityFeeList[(ArrModFee.Length / 5)];

                #region Populate Modality Fees Structure
                for (int i = 0; i < objModalityFees.Length; i++)
                {
                    objModalityFees[i] = new Core.Master.ModalityFeeList();
                    objModalityFees[i].ROW_ID = Convert.ToInt32(ArrModFee[intListIndex]);
                    objModalityFees[i].RATE_ID = new Guid(ArrModFee[intListIndex + 1]);
                    objModalityFees[i].FEE_AMOUNT = Convert.ToDouble(ArrModFee[intListIndex + 2].Trim());
                    objModalityFees[i].FEE_AMOUNT_PER_UNIT = Convert.ToDouble(ArrModFee[intListIndex + 3].Trim());
                    objModalityFees[i].MAXIMUM_STUDY_FEE_AMOUNT = Convert.ToDouble(ArrModFee[intListIndex + 4].Trim());
                    intListIndex = intListIndex + 5;
                }
                #endregion

                intListIndex = 0;

                 objServiceFees = new Core.Master.ServiceFeeList[(ArrSvcFee.Length / 4)];

                #region Populate Service Fees Structure
                for (int i = 0; i < objServiceFees.Length; i++)
                {
                    objServiceFees[i] = new Core.Master.ServiceFeeList();
                    objServiceFees[i].ROW_ID = Convert.ToInt32(ArrSvcFee[intListIndex]);
                    objServiceFees[i].RATE_ID = new Guid(ArrSvcFee[intListIndex + 1]);
                    objServiceFees[i].FEE_AMOUNT = Convert.ToDouble(ArrSvcFee[intListIndex + 2].Trim());
                    objServiceFees[i].FEE_AMOUNT_AFTER_HOURS = Convert.ToDouble(ArrSvcFee[intListIndex + 3].Trim());
                    intListIndex = intListIndex + 4;
                }
                #endregion

                intListIndex = 0;


                bReturn = objCore.SaveRecord(Server.MapPath("~"), objInst, objCont, objPhys,objModalityFees,objServiceFees, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                  
                    arrRet = new string[4];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
                    arrRet[2] = objCore.ID.ToString();
                    arrRet[3] = objCore.CODE;
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

        #region Suspended

        #region SaveRecord (Ajaxpro Method)
        //[AjaxPro.AjaxMethod()]
        //public string[] SaveRecord(string[] ArrRecord, string[] ArrInst, string[] ArrCont, string[] ArrPhys, string[] ArrFee)
        //{
        //    bool bReturn = false;

        //    string[] arrRet = new string[0];
        //    string strReturnMsg = string.Empty;
        //    string strCatchMessage = string.Empty;
        //    int intListIndex = 0;

        //    objCore = new Core.Master.BillingAccount();
        //    objComm = new classes.CommonClass();

        //    Core.Master.BAInstitutionList[] objInst = new Core.Master.BAInstitutionList[0];
        //    Core.Master.ContactList[] objCont = new Core.Master.ContactList[0];
        //    Core.Master.PhysicianList[] objPhys = new Core.Master.PhysicianList[0];
        //    Core.Master.FeeList[] objModalityFees = new Core.Master.FeeList[0];

        //    try
        //    {
        //        objCore.ID = new Guid(ArrRecord[0].Trim());
        //        objCore.CODE = ArrRecord[1].Trim();
        //        objCore.NAME = ArrRecord[2].Trim();
        //        objCore.IS_ACTIVE = ArrRecord[3].Trim();
        //        objCore.ADDRESS_LINE1 = ArrRecord[4].Trim();
        //        objCore.ADDRESS_LINE2 = ArrRecord[5].Trim();
        //        objCore.CITY = ArrRecord[6].Trim();
        //        objCore.COUNTRY_ID = Convert.ToInt32(ArrRecord[7]);
        //        objCore.STATE_ID = Convert.ToInt32(ArrRecord[8]);
        //        objCore.ZIP = ArrRecord[9].Trim();
        //        //objCore.EMAIL_ID = ArrRecord[10].Trim();
        //        //objCore.PHONE = ArrRecord[11].Trim();
        //        //objCore.FAX = ArrRecord[12].Trim();
        //        //objCore.CONTACT_PERSON_NAME = ArrRecord[13].Trim();
        //        //objCore.CONTACT_PERSION_MOBILE = ArrRecord[14].Trim();
        //        //objCore.CONTACT_PERSION_EMAIL_ID = ArrRecord[15].Trim();
        //        objCore.LOGIN_ID = ArrRecord[10].Trim();

        //        if (ArrRecord[17].Trim() != string.Empty)
        //            objCore.LOGIN_PASSWORD = CoreCommon.EncryptString(ArrRecord[11].Trim().ToLower());
        //        else
        //            objCore.LOGIN_PASSWORD = string.Empty;

        //        objCore.USER_EMAIL_ID = ArrRecord[12].Trim();
        //        objCore.USER_MOBILE_NUMBER = ArrRecord[13].Trim();
        //        objCore.NOTIFICATION_PREFERENCE = ArrRecord[14].Trim();


        //        objCore.SALES_PERSON_ID = new Guid(ArrRecord[15]);
        //        objCore.COMMISSION_1ST_YEAR = Convert.ToDecimal(ArrRecord[16]);
        //        objCore.COMMISSION_2ND_YEAR = Convert.ToDecimal(ArrRecord[17]);
        //        objCore.DISCOUNT_PERCENT = Convert.ToDecimal(ArrRecord[18]);
        //        objCore.ACCOUNTANT_NAME = ArrRecord[19].Trim();;
        //        objCore.USER_ID = new Guid(ArrRecord[20].Trim());
        //        objCore.MENU_ID = Convert.ToInt32(ArrRecord[21]);

        //        objInst = new Core.Master.BAInstitutionList[(ArrInst.Length/3)];

        //        #region populate institution details
        //        for (int i = 0; i < objInst.Length; i++)
        //        {
        //            objInst[i] = new Core.Master.BAInstitutionList();
        //            objInst[i].ID = new Guid(ArrInst[intListIndex]);
        //            objInst[i].CONSULT_APPLICABLE = ArrInst[intListIndex + 1].Trim();
        //            objInst[i].STORAGE_APPLICABLE = ArrInst[intListIndex + 2].Trim();
        //            intListIndex = intListIndex + 3;
        //        }
        //        #endregion

        //        intListIndex = 0;
        //        objCont = new Core.Master.ContactList[(ArrCont.Length / 6)];

        //        #region populate Contact details
        //        for (int i = 0; i < objCont.Length; i++)
        //        {
        //            objCont[i] = new Core.Master.ContactList();
        //            objCont[i].INSTITUTION_ID = new Guid(ArrCont[intListIndex]);
        //            objCont[i].PHONE_NUMBER = ArrCont[intListIndex + 1].Trim();
        //            objCont[i].FAX_NUMBER = ArrCont[intListIndex + 2].Trim();
        //            objCont[i].CONTACT_PERSON = ArrCont[intListIndex + 3].Trim();
        //            objCont[i].CONTACT_PERSON_MOBILE_NUMBER = ArrCont[intListIndex + 4].Trim();
        //            objCont[i].CONTACT_PERSON_EMAIL_ID = ArrCont[intListIndex + 5].Trim();
        //            intListIndex = intListIndex + 6;
        //        }
        //        #endregion

        //        intListIndex = 0;
        //        objPhys = new Core.Master.PhysicianList[(ArrPhys.Length / 7)];

        //        #region populate physician details
        //        for (int i = 0; i < objPhys.Length; i++)
        //        {
        //            objPhys[i] = new Core.Master.PhysicianList();
        //            objPhys[i].INSTITUTION_ID = new Guid(ArrPhys[intListIndex]);
        //            objPhys[i].ID = new Guid(ArrPhys[intListIndex + 1]);
        //            objPhys[i].FIRST_NAME = ArrPhys[intListIndex + 2].Trim();
        //            objPhys[i].LAST_NAME = ArrPhys[intListIndex + 3].Trim();
        //            objPhys[i].CREDENTIALS = ArrPhys[intListIndex + 4].Trim();
        //            objPhys[i].EMAIL_ID = ArrPhys[intListIndex + 5].Trim();
        //            objPhys[i].MOBILE_NUMBER = ArrPhys[intListIndex + 6].Trim();
        //            intListIndex = intListIndex + 7;
        //        }
        //        #endregion

        //        intListIndex = 0;

        //        objModalityFees = new Core.Master.FeeList[(ArrFee.Length / 3)];

        //        #region Populate Fees Structure
        //        for (int i = 0; i < objModalityFees.Length; i++)
        //        {
        //            objModalityFees[i] = new Core.Master.FeeList();
        //            objModalityFees[i].ROW_ID = Convert.ToInt32(ArrFee[intListIndex]);
        //            objModalityFees[i].RATE_ID = new Guid(ArrFee[intListIndex + 1]);
        //            objModalityFees[i].FEE_AMOUNT = Convert.ToDouble(ArrFee[intListIndex + 2].Trim());
        //            intListIndex = intListIndex + 3;
        //        }
        //        #endregion

        //        intListIndex = 0;


        //        bReturn = objCore.SaveRecord(Server.MapPath("~"), objInst, objCont, objPhys,objModalityFees, ref strReturnMsg, ref strCatchMessage);

        //        if (bReturn)
        //        {
                  
        //            arrRet = new string[4];
        //            arrRet[0] = "true";
        //            arrRet[1] = strReturnMsg.Trim();
        //            arrRet[2] = objCore.ID.ToString();
        //            arrRet[3] = objCore.CODE;
        //        }
        //        else
        //        {
        //            if (strCatchMessage.Trim() != "")
        //            {
        //                arrRet = new string[2];
        //                arrRet[0] = "catch";
        //                arrRet[1] = strCatchMessage.Trim();
        //            }
        //            else
        //            {
        //                arrRet = new string[3];
        //                arrRet[0] = "false";
        //                arrRet[1] = strReturnMsg.Trim();
        //                arrRet[2] = objCore.USER_NAME;
        //            }
        //        }
        //    }
        //    catch (Exception expErr)
        //    {
        //        bReturn = false;
        //        arrRet = new string[2];
        //        arrRet[0] = "catch";
        //        arrRet[1] = expErr.Message.Trim();
        //    }
        //    finally
        //    {
        //        objCore = null; objComm = null;
        //        strReturnMsg = null; strCatchMessage = null;
        //    }
        //    return arrRet;
        //}
        #endregion  

        #endregion


    }
}