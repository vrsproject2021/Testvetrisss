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
    [AjaxPro.AjaxNamespace("VRSInstitutionDlg")]
    public partial class VRSInstitutionDlg : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Master.Institution objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSInstitutionDlg));
            SetAttributes();
            if ((!CallBackDevice.CausedCallback) && (!CallBackPhys.CausedCallback) && (!CallBackCred.CausedCallback) && (!CallBackTags.CausedCallback) && (!CallBackPromo.CausedCallback)
                && (!CallBackLogo.CausedCallback) && (!CallBackInst.CausedCallback) && (!CallBackInsCtg.CausedCallback)
                //&& (!CallBackFees.CausedCallback)
                )
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
            if (Request.QueryString["cf"] != null) hdnCF.Value = Request.QueryString["cf"];

            hdnID.Value = Request.QueryString["id"];
            hdnFilePath.Value = Server.MapPath("~");
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
            rdoBAYes.Attributes.Add("onclick", "javascript:LinkExistingAccount();");
            rdoBANo.Attributes.Add("onclick", "javascript:LinkExistingAccount();");
            rdoFmtYes.Attributes.Add("onclick", "javascript:UpdateTagFormat();");
            rdoFmtNo.Attributes.Add("onclick", "javascript:UpdateTagFormat();");

            btnAddDevice.Attributes.Add("onclick", "javascript:btnAddDevice_OnClick();");
            btnAddPhys.Attributes.Add("onclick", "javascript:btnAddPhys_OnClick();");
            btnAddCred.Attributes.Add("onclick", "javascript:btnAddCred_OnClick();");

            btnUploadLogo.Attributes.Add("onclick", "javascript:btnUploadLogo_OnClick();");
            btnDelLogo.Attributes.Add("onclick", "javascript:btnDelLogo_OnClick();");

            //txtCommission1stYr.Attributes.Add("onkeypress", "javascript:return parent.CheckDecimal(event);");
            //txtCommission1stYr.Attributes.Add("onfocus", "javascript:this.select();");
            //txtCommission1stYr.Attributes.Add("onblur", "javascript:ResetValueDecimal(this);");
            //txtCommission2ndYr.Attributes.Add("onkeypress", "javascript:return parent.CheckDecimal(event);");
            //txtCommission2ndYr.Attributes.Add("onfocus", "javascript:this.select();");
            //txtCommission2ndYr.Attributes.Add("onblur", "javascript:ResetValueDecimal(this);");
            //txtDisc.Attributes.Add("onkeypress", "javascript:parent.CheckDecimal(event);");
            //txtDisc.Attributes.Add("onfocus", "javascript:this.select();");
            //txtDisc.Attributes.Add("onblur", "javascript:ResetValueDecimal(this);");
            //txtDisc.Attributes.Add("onchange", "javascript:txtDisc_OnChange();");
            //btnApplyDisc.Attributes.Add("onclick", "javascript:btnApplyDisc_OnChange();");
        }
        #endregion

        #region LoadDetails
        private void LoadDetails(int intMenuID, Guid UserID)
        {
            objCore = new Core.Master.Institution();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.ID      = new Guid(hdnID.Value);
                objCore.MENU_ID = intMenuID;
                objCore.USER_ID = UserID;

                bReturn = objCore.LoadDetails(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {

                    PopulateDropdowns(ds);
                    txtCode.Text                    = objCore.CODE;
                    txtName.Text                    = objCore.NAME;
                    txtAddr1.Text                   = objCore.ADDRESS_LINE1;
                    txtAddr2.Text                   = objCore.ADDRESS_LINE2;
                    txtCity.Text                    = objCore.CITY;
                    ddlCountry.SelectedValue        = objCore.COUNTRY_ID.ToString();
                    ddlState.SelectedValue          = objCore.STATE_ID.ToString();
                    txtZip.Text                     = objCore.ZIP;

                    txtEmailID.Text                 = objCore.EMAIL_ID;
                    txtTel.Text                     = objCore.PHONE;
                    txtFax.Text                     = objCore.FAX;
                    txtContPerson.Text              = objCore.CONTACT_PERSON_NAME;
                    txtContMobile.Text              = objCore.CONTACT_PERSION_MOBILE;
                    if (objCore.IS_ACTIVE == "Y") rdoStatYes.Checked = true;
                    else if (objCore.IS_ACTIVE == "N") rdoStatNo.Checked = true;
                    txtNotes.Text                   = objCore.NOTES;
                    //ddlSalesPerson.SelectedValue    = objCore.SALES_PERSON_ID.ToString();
                    //txtCommission1stYr.Text         = objComm.IMNumeric(objCore.COMMISSION_1ST_YEAR, objComm.DecimalDigit);// Added on 4th SEP 2019 @BK
                    //txtCommission2ndYr.Text         = objComm.IMNumeric(objCore.COMMISSION_2ND_YEAR, objComm.DecimalDigit);// Added on 4th SEP 2019 @BK
                    //txtDisc.Text                    = objComm.IMNumeric(objCore.DISCOUNT_PERCENT, objComm.DecimalDigit);
                    //txtAccName.Text                 = objCore.ACCOUNTANT_NAME;// Added on 3rd SEP 2019 @BK
                    ddlInfoSrc.SelectedValue        = Convert.ToString(objCore.BUSSINESS_SOURCE_ID);
                    if (objCore.LINKED_TO_EXISTING_BILLING_ACCOUNT == "Y") rdoBAYes.Checked = true;
                    else if (objCore.LINKED_TO_EXISTING_BILLING_ACCOUNT == "N") rdoBANo.Checked = true;
                    ddlBA.SelectedValue             = Convert.ToString(objCore.BILLING_ACCOUNT_ID);
                    if (objCore.FORMAT_DICOM_FILES == "Y") rdoFmtYes.Checked = true;
                    else if (objCore.FORMAT_DICOM_FILES == "N") rdoFmtNo.Checked = true;

                    if (objCore.DICOM_FILES_TRANSFER_METHOD == "N") rdoNotReg.Checked = true;
                    else if (objCore.DICOM_FILES_TRANSFER_METHOD == "A") rdoAuto.Checked = true;
                    else if (objCore.DICOM_FILES_TRANSFER_METHOD == "M") rdoManual.Checked = true;

                    txtRecPath.Text = objCore.STUDY_IMAGE_FILES_MANUAL_RECEIVING_PATH;

                    if (objCore.CONSULT_APPLICABLE == "Y") rdoConsultY.Checked = true;
                    else if (objCore.CONSULT_APPLICABLE == "N") rdoConsultN.Checked = true;

                    if (objCore.STORAGE_APPLICABLE == "Y") rdoStoreY.Checked = true;
                    else if (objCore.STORAGE_APPLICABLE == "N") rdoStoreN.Checked = true;

                    if (objCore.CUSTOMIZE_REPORTS == "Y") rdoCustRptY.Checked = true;
                    else if (objCore.CUSTOMIZE_REPORTS == "N") rdoCustRptN.Checked = true;

                    if (objCore.FAX_REPORTS == "Y") rdoFaxRptY.Checked = true;
                    else if (objCore.FAX_REPORTS == "N") rdoFaxRptN.Checked = true;

                    if (objCore.REPORT_FORMAT == "P") rdoRFPdf.Checked = true;
                    else if (objCore.REPORT_FORMAT == "R") rdoRFRTF.Checked = true;
                    else if (objCore.REPORT_FORMAT == "B") rdoRFBoth.Checked = true;

                    if (objCore.COMPRESS_DICOM_FILES_TO_TRANSFER == "Y") rdoCompXferY.Checked = true;
                    else if (objCore.COMPRESS_DICOM_FILES_TO_TRANSFER == "N") rdoCompXferN.Checked = true;

                    hdnUsrUpdUrl.Value              = objCore.USER_UPDATE_URL;
                    if (hdnID.Value != "00000000-0000-0000-0000-000000000000")
                    {
                        if (objCore.IMAGE_TYPE.Trim() != string.Empty) LoadLogo(objCore.LOGO, UserID, objCore.IMAGE_TYPE.Trim());
                    }
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

            #region Physicians

            DataRow dr3 = ds.Tables["Physicians"].NewRow();
            dr3["id"] = "00000000-0000-0000-0000-000000000000";
            dr3["name"] = "Select One";
            ds.Tables["Physicians"].Rows.InsertAt(dr3, 0);
            ds.Tables["Physicians"].AcceptChanges();

            foreach (DataRow dr in ds.Tables["Physicians"].Rows)
            {
                if (hdnPhysicians.Value.Trim() != string.Empty) hdnPhysicians.Value += objComm.RecordDivider;
                hdnPhysicians.Value += Convert.ToString(dr["id"]) + objComm.RecordDivider;
                hdnPhysicians.Value += Convert.ToString(dr["name"]).Trim();
            }


            #endregion

            #region Sales Persons
            //DataRow dr4 = ds.Tables["SalesPersons"].NewRow();
            //dr4["id"] = "00000000-0000-0000-0000-000000000000";
            //dr4["name"] = "Select One";
            //ds.Tables["SalesPersons"].Rows.InsertAt(dr4, 0);
            //ds.Tables["SalesPersons"].AcceptChanges();

            //ddlSalesPerson.DataSource = ds.Tables["SalesPersons"];
            //ddlSalesPerson.DataValueField = "id";
            //ddlSalesPerson.DataTextField = "name";
            //ddlSalesPerson.DataBind();
            #endregion

            #region Business Source
            DataRow dr5 = ds.Tables["BusinessSrc"].NewRow();
            dr5["id"] = "0";
            dr5["name"] = "Select One";
            ds.Tables["BusinessSrc"].Rows.InsertAt(dr5, 0);
            ds.Tables["BusinessSrc"].AcceptChanges();

            ddlInfoSrc.DataSource = ds.Tables["BusinessSrc"];
            ddlInfoSrc.DataValueField = "id";
            ddlInfoSrc.DataTextField = "name";
            ddlInfoSrc.DataBind();
            #endregion

            #region Billing Account
            DataRow dr6 = ds.Tables["BillingAccount"].NewRow();
            dr6["id"] = "00000000-0000-0000-0000-000000000000";
            dr6["name"] = "Select One";
            ds.Tables["BillingAccount"].Rows.InsertAt(dr6, 0);
            ds.Tables["BillingAccount"].AcceptChanges();

            ddlBA.DataSource = ds.Tables["BillingAccount"];
            ddlBA.DataValueField = "id";
            ddlBA.DataTextField = "name";
            ddlBA.DataBind();
            #endregion

        }
        #endregion

        #region LoadLogo
        private void LoadLogo(byte[] ImgData, Guid UserID, string ImgType)
        {
            string strFileName = UserID.ToString() + "_Logo_" + System.Guid.NewGuid().ToString("N") + "." + ImgType;
            string strFilePath = Server.MapPath("~") + "/Masters/Logo/Temp/" + strFileName;
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            try
            {
                using (FileStream fs = new FileStream(strFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fs.Write(ImgData, 0, ImgData.Length);
                    fs.Flush();
                    fs.Close();
                }
                hdnFilename.Value = strFileName;
                if (File.Exists(strFilePath))
                {
                    imgLogo.ImageUrl = strServerPath + "/Masters/Logo/Temp/" + strFileName;
                }
                else
                {
                    imgLogo.ImageUrl = strServerPath + "/Masters/Logo/Temp/NoLogo.jpg";
                }

            }
            catch (Exception expErr)
            {
                Response.Write(expErr.Message);
            }

        }
        #endregion

        #region CallBackLogo_Callback
        protected void CallBackLogo_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strImgUrl = Convert.ToString(e.Parameters[0]);
            string strFilePath = string.Empty;
            imgLogo.ImageUrl = strImgUrl.Replace("\\", "/");
            imgLogo.RenderControl(e.Output);
            
        }
        #endregion

        #region Institution Grid

        #region CallBackInst_Callback
        protected void CallBackInst_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadInstitutions(new Guid(e.Parameter));
            grdInst.Width = Unit.Percentage(100);
            grdInst.RenderControl(e.Output);
            spnInstERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadInstitutions
        private void LoadInstitutions(Guid Id)
        {
            objCore = new Core.Master.Institution();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                objCore.ID = Id;
                bReturn = objCore.LoadMismatchInstitution(Server.MapPath("~"), ref ds, ref strCatchMessage);
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

        #region DICOM Tag Grid

        #region CallBackTags_Callback
        protected void CallBackTags_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadTags(e.Parameters);
            grdTags.Width = Unit.Percentage(100);
            grdTags.RenderControl(e.Output);
            spnErrTags.RenderControl(e.Output);
        }
        #endregion

        #region LoadTags
        private void LoadTags(string[] arrParams)
        {
            objCore = new Core.Master.Institution();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
           

            try
            {

                objCore.ID = new Guid(arrParams[0]);

                bReturn = objCore.LoadDiomTags(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdTags.DataSource = ds.Tables["Tags"];
                    grdTags.DataBind();


                    spnErrTags.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrTags\" value=\"\" />";
                }
                else
                    spnErrTags.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrTags\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnErrTags.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrTags\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        
        #endregion

        #region Study Type Category Link Grid

        #region CallBackInsCtg_Callback
        protected void CallBackInsCtg_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadStudyTypeCategory(e.Parameters);
            grdInsCategory.Width = Unit.Percentage(100);
            grdInsCategory.RenderControl(e.Output);
            spnErrInsCtg.RenderControl(e.Output);
        }
        #endregion

        #region LoadStudyTypeCategory
        private void LoadStudyTypeCategory(string[] arrParams)
        {
            objCore = new Core.Master.Institution();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();


            try
            {

                objCore.ID = new Guid(arrParams[0]);

                bReturn = objCore.LoadStudyTypeCategory(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdInsCategory.DataSource = ds.Tables["StudyTypeCategory"];
                    grdInsCategory.DataBind();


                    spnErrInsCtg.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrInsCtg\" value=\"\" />";
                }
                else
                    spnErrInsCtg.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrInsCtg\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnErrInsCtg.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrInsCtg\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #endregion

        #region Device Grid

        #region CallBackDevice_Callback
        protected void CallBackDevice_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];

            switch (strAction)
            {
                case "L":
                    LoadDevices(e.Parameters);
                    break;
                case "A":
                    AddDevice(e.Parameters);
                    break;
                case "D":
                    DeleteDevice(e.Parameters);
                    break;
            }
            grdDevice.Width = Unit.Percentage(100);
            grdDevice.RenderControl(e.Output);
            spnDevERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadDevices
        private void LoadDevices(string[] arrParams)
        {
            objCore = new Core.Master.Institution();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                objCore.ID = new Guid(arrParams[1]);

                bReturn = objCore.LoadDevices(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdDevice.DataSource = ds.Tables["Devices"];
                    grdDevice.DataBind();
                    spnDevERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDev\" value=\"\" />";
                }
                else
                    spnDevERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDev\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnDevERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDev\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region AddDevice
        private void AddDevice(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;

            try
            {
                dtbl = CreateDeviceTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 6)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl                  = intSrl + 1;
                            dr["rec_id"]            = intSrl;
                            dr["device_id"]         = arrRecords[i + 1].Trim();
                            dr["manufacturer"]      = arrRecords[i + 2].Trim();
                            dr["modality"]          = arrRecords[i + 3].Trim();
                            dr["modality_ae_title"] = arrRecords[i + 4].Trim();
                            dr["weight_uom"]        = arrRecords[i + 5].Trim();//--Added on 2nd SEP 2019 @BK
                            dr["del"]               = "";
                            dtbl.Rows.Add(dr);
                        }

                    }
                }

                DataRow drNew = dtbl.NewRow();
                intSrl                      = intSrl + 1;
                drNew["rec_id"]             = intSrl;
                drNew["device_id"]          = "00000000-0000-0000-0000-000000000000";
                drNew["manufacturer"]       = string.Empty;
                drNew["modality"]           = string.Empty;
                drNew["modality_ae_title"]  = string.Empty;
                drNew["weight_uom"]         = string.Empty;//--Added on 2nd SEP 2019 @BK
                drNew["del"]                = "";
                dtbl.Rows.Add(drNew);

                grdDevice.DataSource = dtbl;
                grdDevice.DataBind();
                spnDevERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDev\" value=\"\" />";
            }
            catch (Exception ex)
            {
                spnDevERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDev\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region DeleteDevice
        private void DeleteDevice(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            int intID = Convert.ToInt32(arrParams[1]);
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;

            try
            {
                dtbl = CreateDeviceTable();
                if (arrRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrRecords.Length; i = i + 6)
                    {
                        if (Convert.ToInt32(arrRecords[i]) != intID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl                  = intSrl + 1;
                            dr["rec_id"]            = intSrl;
                            dr["device_id"]         = arrRecords[i + 1].Trim();
                            dr["manufacturer"]      = arrRecords[i + 2].Trim();
                            dr["modality"]          = arrRecords[i + 3].Trim();
                            dr["modality_ae_title"] = arrRecords[i + 4].Trim();
                            dr["weight_uom"] = arrRecords[i + 5].Trim();//--Added on 2nd SEP 2019 @BK
                            dr["del"]               = "";
                            dtbl.Rows.Add(dr);
                        }
                    }
                }

                grdDevice.DataSource = dtbl;
                grdDevice.DataBind();
                spnDevERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDev\" value=\"\" />";

            }
            catch (Exception ex)
            {
                spnDevERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDev\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region CreateDeviceTable
        private DataTable CreateDeviceTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("rec_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("device_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("manufacturer", System.Type.GetType("System.String"));
            dtbl.Columns.Add("modality", System.Type.GetType("System.String"));
            dtbl.Columns.Add("modality_ae_title", System.Type.GetType("System.String"));
            dtbl.Columns.Add("weight_uom", System.Type.GetType("System.String"));//--Added on 2nd SEP 2019 @BK
            dtbl.Columns.Add("del", System.Type.GetType("System.String"));
            dtbl.TableName = "Devices";
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
                case "A":
                    AddPhysician(e.Parameters);
                    break;
                case "D":
                    DeletePhysician(e.Parameters);
                    break;
            }

            if (grdPhys.RecordCount > 10) grdPhys.ShowHeader = true;
            grdPhys.Width = Unit.Percentage(100);
            grdPhys.RenderControl(e.Output);
            spnErrPhys.RenderControl(e.Output);
        }
        #endregion

        #region LoadPhysicians
        private void LoadPhysicians(string[] arrParams)
        {
            objCore = new Core.Master.Institution();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";

            try
            {

                objCore.ID = new Guid(arrParams[1]);

                bReturn = objCore.LoadPhysicians(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdPhys.DataSource = ds.Tables["Physicians"];
                    grdPhys.DataBind();
                   

                    spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPhys\" value=\"\" />";
                }
                else
                    spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPhys\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPhys\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region AddPhysician
        private void AddPhysician(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);
            int intPageCount = 0;

            int intSrl = 0;

            try
            {
                dtbl = CreatePhysicianTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 7)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["rec_id"] = intSrl;
                            dr["physician_id"] = arrRecords[i + 1].Trim();
                            dr["physician_fname"] = arrRecords[i + 2].Trim();
                            dr["physician_lname"] = arrRecords[i + 3].Trim();
                            dr["physician_credentials"] = arrRecords[i + 4].Trim();
                            dr["physician_email"] = arrRecords[i + 5].Trim();
                            dr["physician_mobile"] = arrRecords[i + 6].Trim();
                            dr["del"] = "";
                            dtbl.Rows.Add(dr);
                        }

                    }
                }

                DataRow drNew = dtbl.NewRow();
                intSrl = intSrl + 1;
                drNew["rec_id"] = intSrl;
                drNew["physician_id"] = "00000000-0000-0000-0000-000000000000";
                drNew["physician_fname"] = "";
                drNew["physician_lname"] = "";
                drNew["physician_credentials"] = "";
                drNew["physician_email"] = "";
                drNew["physician_mobile"] = "";
                drNew["del"] = "";
                dtbl.Rows.Add(drNew);

                grdPhys.DataSource = dtbl;
                grdPhys.DataBind();
                                
                spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPhys\" value=\"\" />";
            }
            catch (Exception ex)
            {
                spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPhys\" value=\"" + ex.Message.Trim() + "\" />";
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
                dtbl = CreatePhysicianTable();
                if (arrRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrRecords.Length; i = i + 7)
                    {
                        if (Convert.ToInt32(arrRecords[i]) != intID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["rec_id"] = intSrl;
                            dr["physician_id"] = arrRecords[i + 1].Trim();
                            dr["physician_fname"] = arrRecords[i + 2].Trim();
                            dr["physician_lname"] = arrRecords[i + 3].Trim();
                            dr["physician_credentials"] = arrRecords[i + 4].Trim();
                            dr["physician_email"] = arrRecords[i + 5].Trim();
                            dr["physician_mobile"] = arrRecords[i + 6].Trim();
                            dr["del"] = "";
                            dtbl.Rows.Add(dr);
                        }
                    }
                }

                grdPhys.DataSource = dtbl;
                grdPhys.DataBind();
                spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPhys\" value=\"\" />";

            }
            catch (Exception ex)
            {
                spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPhys\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region CreatePhysicianTable
        private DataTable CreatePhysicianTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("rec_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("physician_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("physician_fname", System.Type.GetType("System.String"));
            dtbl.Columns.Add("physician_lname", System.Type.GetType("System.String"));
            dtbl.Columns.Add("physician_credentials", System.Type.GetType("System.String"));
            dtbl.Columns.Add("physician_email", System.Type.GetType("System.String"));
            dtbl.Columns.Add("physician_mobile", System.Type.GetType("System.String"));
            dtbl.Columns.Add("del", System.Type.GetType("System.String"));
            dtbl.TableName = "Physicians";
            return dtbl;
        }
        #endregion

        #endregion

        #region User Grid

        #region CallBackCred_Callback
        protected void CallBackCred_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];

            switch (strAction)
            {
                case "L":
                    LoadUsers(e.Parameters);
                    break;
                case "A":
                    AddUser(e.Parameters);
                    break;
                case "D":
                    DeleteUser(e.Parameters);
                    break;
            }

            grdCred.Width = Unit.Percentage(100);
            grdCred.RenderControl(e.Output);
            spnErrCred.RenderControl(e.Output);
        }
        #endregion

        #region LoadUsers
        private void LoadUsers(string[] arrParams)
        {
            objCore = new Core.Master.Institution();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";

            try
            {

                objCore.ID = new Guid(arrParams[1]);

                bReturn = objCore.LoadUsers(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdCred.DataSource = ds.Tables["Users"];
                    grdCred.DataBind();


                    spnErrCred.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCred\" value=\"\" />";
                }
                else
                    spnErrCred.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCred\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnErrCred.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCred\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region AddUser
        private void AddUser(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);

            int intSrl = 0;

            try
            {
                dtbl = CreateUserTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 9)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["rec_id"] = intSrl;
                            dr["id"] = arrRecords[i + 1].Trim();
                            dr["login_id"] = arrRecords[i + 2].Trim();
                            dr["password"] = arrRecords[i + 3].Trim();
                            dr["pacs_user_id"] = arrRecords[i + 4].Trim();
                            dr["pacs_password"] = arrRecords[i + 5].Trim();
                            dr["email_id"] = arrRecords[i + 6].Trim();
                            dr["contact_no"] = arrRecords[i + 7].Trim();
                            dr["is_active"] = arrRecords[i + 8].Trim();
                            dtbl.Rows.Add(dr);
                        }

                    }
                }

                DataRow drNew = dtbl.NewRow();
                intSrl = intSrl + 1;
                drNew["rec_id"] = intSrl;
                drNew["id"] = "00000000-0000-0000-0000-000000000000";
                drNew["login_id"] = "";
                drNew["password"] = "";
                drNew["pacs_user_id"] = "";
                drNew["pacs_password"] = "";
                drNew["email_id"] = "";
                drNew["contact_no"] = "";
                drNew["is_active"] = "Y";
                dtbl.Rows.Add(drNew);

                grdCred.DataSource = dtbl;
                grdCred.DataBind();
                spnErrCred.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCred\" value=\"\" />";
            }
            catch (Exception ex)
            {
                spnErrCred.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCred\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region DeleteUser
        private void DeleteUser(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            int intID = Convert.ToInt32(arrParams[1]);
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;

            try
            {
                dtbl = CreateUserTable();
                if (arrRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrRecords.Length; i = i + 9)
                    {
                        if (Convert.ToInt32(arrRecords[i]) != intID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["rec_id"] = intSrl;
                            dr["id"] = arrRecords[i + 1].Trim();
                            dr["login_id"] = arrRecords[i + 2].Trim();
                            dr["password"] = arrRecords[i + 3].Trim();
                            dr["pacs_user_id"] = arrRecords[i + 4].Trim();
                            dr["pacs_password"] = arrRecords[i + 5].Trim();
                            dr["email_id"] = arrRecords[i + 6].Trim();
                            dr["contact_no"] = arrRecords[i + 7].Trim();
                            dr["is_active"] = arrRecords[i + 8].Trim();
                            dtbl.Rows.Add(dr);
                        }
                    }
                }

                grdCred.DataSource = dtbl;
                grdCred.DataBind();
                spnErrCred.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCred\" value=\"\" />";

            }
            catch (Exception ex)
            {
                spnErrCred.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCred\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region CreateUserTable
        private DataTable CreateUserTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("rec_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("login_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("password", System.Type.GetType("System.String"));
            dtbl.Columns.Add("pacs_user_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("pacs_password", System.Type.GetType("System.String"));
            dtbl.Columns.Add("email_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("contact_no", System.Type.GetType("System.String"));
            dtbl.Columns.Add("is_active", System.Type.GetType("System.String"));
            dtbl.TableName = "Users";
            return dtbl;
        }
        #endregion

        #endregion

        #region Fees Grid

        #region CallBackFees_Callback
        //protected void CallBackFees_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        //{
        //    objCore = new Core.Master.Institution();
        //    string strCatchMessage = ""; bool bReturn = false;
        //    DataSet ds = new DataSet();

        //    try
        //    {
        //        objCore.ID = new Guid(e.Parameter);

        //        bReturn = objCore.LoadFees(Server.MapPath("~"), ref ds, ref strCatchMessage);
        //        if (bReturn)
        //        {
        //            grdFees.DataSource = ds.Tables["Fees"];
        //            grdFees.DataBind();
        //            spnFeeERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrFee\" value=\"\" />";
        //        }
        //        else
        //            spnFeeERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrFee\" value=\"" + strCatchMessage + "\" />";



        //    }
        //    catch (Exception ex)
        //    {
        //        spnFeeERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrFee\" value=\"" + ex.Message.Trim() + "\" />";
        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //        objCore = null;
        //    }
           
        //    grdFees.Width = Unit.Percentage(100);
        //    grdFees.RenderControl(e.Output);
        //    spnFeeERR.RenderControl(e.Output);
        //}
        #endregion

        #endregion

        #region Promotion Grid

        #region CallBackPromo_Callback
        protected void CallBackPromo_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadPromotions(e.Parameters);
            grdPromo.Width = Unit.Percentage(100);
            grdPromo.RenderControl(e.Output);
            spnErrPromo.RenderControl(e.Output);
        }
        #endregion

        #region LoadPromotions
        private void LoadPromotions(string[] arrParams)
        {
            objCore = new Core.Master.Institution();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.ID = new Guid(arrParams[0]);

                bReturn = objCore.LoadPromotions(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdPromo.DataSource = ds.Tables["Promotions"];
                    grdPromo.DataBind();

                    grdPromo.Levels[0].Columns[4].FormatString = objComm.DateFormat;
                    grdPromo.Levels[0].Columns[7].FormatString = objComm.DateFormat;
                    grdPromo.Levels[0].Columns[8].FormatString = objComm.DateFormat;

                    spnErrPromo.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPromo\" value=\"\" />";
                }
                else
                    spnErrPromo.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPromo\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnErrPromo.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPromo\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
                objComm = null;
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
            objCore = new Core.Master.Institution();

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

        #region FetchPhysicianDetails (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchPhysicianDetails(string[] arrParams)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objCore = new Core.Master.Institution();
            string[] arrRet = new string[0];

            try
            {

                objCore.EMAIL_ID = arrParams[0].Trim();
                objCore.NAME = arrParams[1].Trim();
                objCore.MOBILE = arrParams[2].Trim();

                bReturn = objCore.FetchPhysicianDetails(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    if (ds.Tables["Physician"].Rows.Count > 0)
                    {

                        arrRet = new string[5];
                        arrRet[0] = "true";
                        foreach (DataRow dr in (ds.Tables["Physician"].Rows))
                        {
                            arrRet[1] = Convert.ToString(dr["id"]);
                            arrRet[2] = Convert.ToString(dr["email_id"]).Trim();
                            arrRet[3] = Convert.ToString(dr["name"]).Trim();
                            arrRet[4] = Convert.ToString(dr["mobile_no"]).Trim();
                        }

                    }
                    else
                    {
                        arrRet = new string[2];
                        arrRet[0] = "true";
                        arrRet[1] = "00000000-0000-0000-0000-000000000000";
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

        #region FetchUserDetails (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchUserDetails(string strLoginID)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objCore = new Core.Master.Institution();
            string[] arrRet = new string[0];

            try
            {

                objCore.LOGIN_ID = strLoginID.Trim();

                bReturn = objCore.FetchUserDetails(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    if (ds.Tables["User"].Rows.Count > 0)
                    {

                        arrRet = new string[9];
                        arrRet[0] = "true";
                        foreach (DataRow dr in (ds.Tables["User"].Rows))
                        {
                            arrRet[1] = Convert.ToString(dr["id"]);
                            arrRet[2] = Convert.ToString(dr["login_id"]);
                            if (Convert.ToString(dr["password"]).Trim() != "")
                                arrRet[3] = CoreCommon.DecryptString(Convert.ToString(dr["password"]).Trim());
                            else
                                arrRet[3] = "";
                            arrRet[4] = Convert.ToString(dr["pacs_user_id"]).Trim();

                            if (Convert.ToString(dr["pacs_password"]).Trim() != "")
                                arrRet[5] = CoreCommon.DecryptString(Convert.ToString(dr["pacs_password"]).Trim());
                            else
                                arrRet[5] = "";
                            arrRet[6] = Convert.ToString(dr["email_id"]).Trim();
                            arrRet[7] = Convert.ToString(dr["contact_no"]).Trim();
                            arrRet[8] = Convert.ToString(dr["is_active"]).Trim();
                        }

                    }
                    else
                    {
                        arrRet = new string[2];
                        arrRet[0] = "true";
                        arrRet[1] = "00000000-0000-0000-0000-000000000000";
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

        #region AutoGenInstCode
        private string AutoGenInstCode()
        {
            string strInstCode = string.Empty;
            VETRIS.Core.Master.Institution objCore1 = new Core.Master.Institution();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {

                bReturn = objCore1.AutoGenInstcodeStr(Server.MapPath("~"), ref strInstCode, ref strCatchMessage);

                if (bReturn)
                {
                    if (!(strInstCode != string.Empty && strInstCode.Trim().Length > 0)) strInstCode = "00000";

                }
                else
                {
                    strInstCode = "00000";
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
                ds.Dispose(); objCore1 = null;
            }
            return strInstCode.Trim();
        }
        #endregion

        #region FetchBillingAccounts (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchBillingAccounts(string strID)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objCore = new Core.Master.Institution();
            string[] arrRet = new string[0];
            int idx = 0;

            try
            {

                objCore.ID = new Guid(strID);
                bReturn = objCore.FetchBillingAccounts(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[(ds.Tables["BillingAccounts"].Rows.Count * 2) + 2];
                    if (ds.Tables["BillingAccounts"].Rows.Count > 0)
                    {
                        arrRet[idx] = "true";
                        idx = idx + 1;

                        foreach (DataRow dr in (ds.Tables["BillingAccounts"].Rows))
                        {
                            arrRet[idx] = Convert.ToString(dr["id"]);
                            arrRet[idx + 1] = Convert.ToString(dr["name"]);
                            idx = idx + 2;
                        }
                        foreach (DataRow dr in (ds.Tables["AccountId"].Rows))
                        {
                            arrRet[idx] = Convert.ToString(dr["billing_account_id"]);
                        }
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
        public string[] SaveRecord(string[] ArrRecord, string[] ArrDevice, string[] ArrPhys, string[] ArrUser, string[] ArrTags, string[] ArrStudyTypeCategory, string[] ArrInst)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            string strFileName = string.Empty; string strFilePath = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Master.Institution();
            objComm = new classes.CommonClass();

            Core.Master.DeviceList[] objDevice = new Core.Master.DeviceList[0];
            Core.Master.PhysicianList[] objPHYS = new Core.Master.PhysicianList[0];
            Core.Master.UserList[] objUser = new Core.Master.UserList[0];
            //Core.Master.FeeList[] objFees = new Core.Master.FeeList[0];
            Core.Master.TagList[] objTags = new Core.Master.TagList[0];
            Core.Master.StudyTypeCategoryList[] objInstCategory = new Core.Master.StudyTypeCategoryList[0];
            Core.Master.AlternateNameList[] objInst = new Core.Master.AlternateNameList[0];

            try
            {
                objCore.ID                                 = new Guid(ArrRecord[0].Trim());
                objCore.CODE                               = ArrRecord[1].Trim(); 
                objCore.NAME                               = ArrRecord[2].Trim();
                objCore.IS_ACTIVE                          = ArrRecord[3].Trim();
                objCore.ADDRESS_LINE1                      = ArrRecord[4].Trim();
                objCore.ADDRESS_LINE2                      = ArrRecord[5].Trim();
                objCore.CITY                               = ArrRecord[6].Trim();
                objCore.COUNTRY_ID                         = Convert.ToInt32(ArrRecord[7]);
                objCore.STATE_ID                           = Convert.ToInt32(ArrRecord[8]);
                objCore.ZIP                                = ArrRecord[9].Trim();
                objCore.EMAIL_ID                           = ArrRecord[10].Trim();
                objCore.PHONE                              = ArrRecord[11].Trim();
                objCore.FAX                                = ArrRecord[12].Trim();
                objCore.CONTACT_PERSON_NAME                = ArrRecord[13].Trim();
                objCore.CONTACT_PERSION_MOBILE             = ArrRecord[14].Trim();
                objCore.NOTES                              = ArrRecord[15].Trim();
                objCore.LINKED_TO_EXISTING_BILLING_ACCOUNT = ArrRecord[16].Trim();
                objCore.BILLING_ACCOUNT_ID                 = new Guid(ArrRecord[17]);
                //objCore.SALES_PERSON_ID         = new Guid(ArrRecord[16]);
                //objCore.COMMISSION_1ST_YEAR     = Convert.ToDecimal(ArrRecord[17]);// Added on 4th SEP 2019 @BK
                //objCore.COMMISSION_2ND_YEAR     = Convert.ToDecimal(ArrRecord[18]);// Added on 4th SEP 2019 @BK
                //objCore.DISCOUNT_PERCENT        = Convert.ToDecimal(ArrRecord[19]);
                //objCore.ACCOUNTANT_NAME         = ArrRecord[19].Trim();// Added on 3rd SEP 2019 @BK
                objCore.BUSSINESS_SOURCE_ID                     = Convert.ToInt32(ArrRecord[18].Trim());
                objCore.FORMAT_DICOM_FILES                      = ArrRecord[19].Trim();
                objCore.DICOM_FILES_TRANSFER_METHOD             = ArrRecord[20].Trim();
                objCore.STUDY_IMAGE_FILES_MANUAL_RECEIVING_PATH = ArrRecord[21].Trim();
                objCore.CONSULT_APPLICABLE                      = ArrRecord[22].Trim();
                objCore.STORAGE_APPLICABLE                      = ArrRecord[23].Trim();
                objCore.CUSTOMIZE_REPORTS                       = ArrRecord[24].Trim();
                objCore.COMPRESS_DICOM_FILES_TO_TRANSFER        = ArrRecord[25].Trim();
                objCore.FAX_REPORTS                             = ArrRecord[26].Trim();
                objCore.REPORT_FORMAT                           = ArrRecord[27].Trim();

                strFileName                                     = ArrRecord[28].Trim();
                if (strFileName.Trim() != string.Empty)
                {
                    strFilePath = Server.MapPath("~") + "\\Masters\\Logo\\Temp\\" + strFileName.ToString();
                    objCore.IMAGE_TYPE = strFileName.ToString().Substring(strFileName.ToString().LastIndexOf(".") + 1);
                }
                else
                {
                    strFilePath = Server.MapPath("~") + "\\Masters\\Logo\\Temp\\NoLogo.jpg";
                    objCore.IMAGE_TYPE = "jpg";
                }

                if (File.Exists(strFilePath)) objCore.GetImage(objCore.ReadImageFile(strFilePath));

                objCore.USER_ID                                 = new Guid(ArrRecord[29].Trim());
                objCore.MENU_ID                                 = Convert.ToInt32(ArrRecord[30]);
                objCore.USER_UPDATE_URL                         = Convert.ToString(ArrRecord[31]);

                objDevice = new Core.Master.DeviceList[(ArrDevice.Length / 5)];

                #region populate device details
                for (int i = 0; i < objDevice.Length; i++)
                {
                    objDevice[i] = new Core.Master.DeviceList();
                    objDevice[i].ID = new Guid(ArrDevice[intListIndex]);
                    objDevice[i].MANUFACTURER = ArrDevice[intListIndex + 1].Trim();
                    objDevice[i].MODALITY = ArrDevice[intListIndex + 2].Trim();
                    objDevice[i].MODALITY_AE_TITLE = ArrDevice[intListIndex + 3].Trim();
                    objDevice[i].WEIGHT_UOM = ArrDevice[intListIndex + 4].Trim();//--Added on 2nd SEP 2019 @BK
                    intListIndex = intListIndex + 5;
                }
                #endregion

                intListIndex = 0;

                objPHYS = new Core.Master.PhysicianList[(ArrPhys.Length / 6)];

                #region Populate physician details
                for (int i = 0; i < objPHYS.Length; i++)
                {
                    objPHYS[i] = new Core.Master.PhysicianList();
                    objPHYS[i].ID = new Guid(ArrPhys[intListIndex]);
                    objPHYS[i].FIRST_NAME = ArrPhys[intListIndex + 1].Trim();
                    objPHYS[i].LAST_NAME = ArrPhys[intListIndex + 2].Trim();
                    objPHYS[i].CREDENTIALS = ArrPhys[intListIndex + 3].Trim();
                    objPHYS[i].EMAIL_ID = ArrPhys[intListIndex + 4].Trim();
                    objPHYS[i].MOBILE_NUMBER = ArrPhys[intListIndex + 5].Trim();
                    intListIndex = intListIndex + 6;
                }
                #endregion

                intListIndex = 0;

                objUser = new Core.Master.UserList[(ArrUser.Length / 8)];

                #region Populate User Details
                for (int i = 0; i < objUser.Length; i++)
                {
                    objUser[i] = new Core.Master.UserList();
                    objUser[i].ID = new Guid(ArrUser[intListIndex]);
                    objUser[i].LOGIN_ID = ArrUser[intListIndex + 1].Trim();
                    if (ArrUser[intListIndex + 2].Trim() != string.Empty)
                    {
                        //ArrUser[intListIndex + 2] = ArrUser[intListIndex + 2].Trim().ToLower();
                        ArrUser[intListIndex + 2] = ArrUser[intListIndex + 2].Trim();
                        ArrUser[intListIndex + 2] = CoreCommon.EncryptString(ArrUser[intListIndex + 2]);
                        objUser[i].PASSWORD = ArrUser[intListIndex + 2];
                    }
                    else
                        objUser[i].PASSWORD = "";
                    
                    objUser[i].PACS_USER_ID = ArrUser[intListIndex + 3].Trim();
                    if (ArrUser[intListIndex + 4].Trim() != string.Empty) ArrUser[intListIndex + 4] = CoreCommon.EncryptString(ArrUser[intListIndex + 4]);
                    objUser[i].PACS_PASSWORD = ArrUser[intListIndex + 4].Trim();
                    objUser[i].EMAIL_ID = ArrUser[intListIndex + 5].Trim();
                    objUser[i].CONTACT_NUMBER = ArrUser[intListIndex + 6].Trim();
                    objUser[i].IS_ACTIVE = ArrUser[intListIndex + 7].Trim();
                    intListIndex = intListIndex + 8;
                }
                #endregion

                intListIndex = 0;

                //objFees = new Core.Master.FeeList[(ArrFee.Length / 3)];

                #region Populate Fees Structure
                //for (int i = 0; i < objFees.Length; i++)
                //{
                //    objFees[i] = new Core.Master.FeeList();
                //    objFees[i].ROW_ID = Convert.ToInt32(ArrFee[intListIndex]);
                //    objFees[i].RATE_ID = new Guid(ArrFee[intListIndex + 1]);
                //    objFees[i].FEE_AMOUNT = Convert.ToDouble(ArrFee[intListIndex + 2].Trim());
                //    intListIndex = intListIndex + 3;
                //}
                #endregion

                //intListIndex = 0;

                objTags = new Core.Master.TagList[(ArrTags.Length / 4)];

                #region Populate Tag Structure
                for (int i = 0; i < objTags.Length; i++)
                {
                    objTags[i] = new Core.Master.TagList();
                    objTags[i].GROUP_ID = ArrTags[intListIndex].Trim();
                    objTags[i].ELEMENT_ID = ArrTags[intListIndex + 1].Trim();
                    objTags[i].DEFAULT_VALUE = ArrTags[intListIndex + 2].Trim();
                    objTags[i].JUNK_CHARACTER= ArrTags[intListIndex + 3].Trim();
                    intListIndex = intListIndex + 4;
                }
                #endregion

                intListIndex = 0;

                objInstCategory = new Core.Master.StudyTypeCategoryList[(ArrStudyTypeCategory.Length / 2)];

                #region Populate Study Type Category
                for (int i = 0; i < objInstCategory.Length; i++)
                {
                    objInstCategory[i] = new Core.Master.StudyTypeCategoryList();
                    objInstCategory[i].CATEGORY_ID = Convert.ToInt32(ArrStudyTypeCategory[intListIndex]);
                    objInstCategory[i].INSTITUTION_ID = Guid.Parse(ArrStudyTypeCategory[intListIndex + 1]);
                    intListIndex = intListIndex + 2;
                }
                #endregion

                intListIndex = 0;

                objInst = new Core.Master.AlternateNameList[(ArrInst.Length / 2)];

                #region Populate Alternate Names
                for (int i = 0; i < objInst.Length; i++)
                {
                    objInst[i] = new Core.Master.AlternateNameList();
                    objInst[i].INSTITUTION_ID = new Guid(ArrInst[intListIndex]);
                    objInst[i].INSTITUTION_NAME = ArrInst[intListIndex + 1].Trim();
                    intListIndex = intListIndex + 2;
                }
                #endregion

                intListIndex = 0;


                bReturn = objCore.SaveRecord(Server.MapPath("~"), objDevice, objPHYS, objUser, objTags, objInstCategory,objInst, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    //string[] ret = CallEradApiUserCreateUpdate(objCore.USER_UPDATE_URL, ArrUser);

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

        #region CallEradApiUserCreateUpdate
        public string[] CallEradApiUserCreateUpdate(string URLUser, string[] ArrUserTemp)
        {
           
            string strOperN = "cOper=new";
            string strOperU = "cOper=update";
            string strUser = "&cUser=";
            string strUPwd = "&qe_UPWD=";
            string strUgrp = "&qe_UGRP=Physician";
            string strUrgt = "&qm_URGT=EOWIN";
            string URL = string.Empty;


            WebClient client = new WebClient();
            string[] arrSep = new string[0];
            string[] arrErr = new string[0];
            string strResult = string.Empty; string strCount = string.Empty;
            string[] arrRet = new string[0];
            string[] err = new string[0];
            Core.Master.UserList[] objUserTemp;

            int intListIndex = 0;

            objUserTemp = new Core.Master.UserList[(ArrUserTemp.Length / 7)];

            for (int i = 0; i < objUserTemp.Length; i++)
            {
                strUser = "&cUser=";
                strUPwd = "&qe_UPWD=";

                objUserTemp[i] = new Core.Master.UserList();
                objUserTemp[i].ID = new Guid(ArrUserTemp[intListIndex]);
                strUser += ArrUserTemp[intListIndex + 1].Trim();
                if (ArrUserTemp[intListIndex + 2].Trim() != string.Empty)
                {
                    strUPwd += CoreCommon.DecryptString(ArrUserTemp[intListIndex + 2].Trim());

                    URL = URLUser + strOperN + strUser + strUPwd + strUgrp  + strUrgt ;

                    try
                    {
                        IgnoreBadCertificates();
                        byte[] dataN = client.DownloadData(URL);
                        strResult = System.Text.Encoding.Default.GetString(dataN);
                        //strResult = strResult.Replace("### Begin_Table's_Content ###", "");
                        //strResult = strResult.Replace("### End_Table's_Content ###", "");

                        strCount = strResult;
                        if (strCount.IndexOf("#RESULT: OK") >= 0)
                        {
                            if (strCount.IndexOf("#WARNING:") >= 0)
                            {
                               // strCount = strCount.Substring(strCount.IndexOf("#WARNING:") - 1, strCount.Length - 1);
                                arrSep = new string[1];
                                arrSep[0] = "\n";
                                arrErr = strCount.Split(arrSep, StringSplitOptions.None);
                            }
                            arrRet = new string[2];
                            arrRet[0] = "true";
                            arrRet[1] = strCount;
                        }
                        else if (strCount.IndexOf("#RESULT: FAILED") >= 0)
                        {
                            URL = URLUser + strOperU + strUser + strUPwd + strUgrp + strUrgt;

                            try
                            {
                                IgnoreBadCertificates();
                                byte[] dataU = client.DownloadData(URL);
                                strResult = System.Text.Encoding.Default.GetString(dataU);
                                //strResult = strResult.Replace("### Begin_Table's_Content ###", "");
                                //strResult = strResult.Replace("### End_Table's_Content ###", "");

                                strCount = strResult;
                                if (strCount.IndexOf("#RESULT: OK") >= 0)
                                {
                                    arrRet = new string[2];
                                    arrRet[0] = "true";
                                    arrRet[1] = strCount;
                                }
                                else
                                {
                                    //strCount = strCount.Substring(strCount.IndexOf("#ERROR:") - 1, strCount.Length - 1);
                                    //strCount = strCount.Replace("\r", "");
                                    //strCount = strCount.Trim();
                                    arrSep= new string[1];
                                    arrSep[0] = "\n" ;
                                    arrErr = strCount.Split(arrSep, StringSplitOptions.None);

                                    arrRet = new string[2];
                                    arrRet[0] = "false";
                                    arrRet[1] = arrErr[arrErr.Length - 1];
                                }
                            }
                            catch (Exception ex)
                            {
                                arrRet = new string[2];
                                arrRet[0] = "catch";
                                arrRet[1] = ex.Message;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        arrRet = new string[2];
                        arrRet[0] = "catch";
                        arrRet[1] = ex.Message;
                    }
                }
                else
                {
                    arrRet = new string[2];
                    arrRet[0] = "false";
                    arrRet[1] = "User without password not allowed in ERAD PACS";
                }
                intListIndex = intListIndex + 7;
            }


            


            return arrRet;
        }
        #endregion

        #region IgnoreBadCertificates
        public static void IgnoreBadCertificates()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
        }
        #endregion

        #region AcceptAllCertifications
        private static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        #endregion
    }
}