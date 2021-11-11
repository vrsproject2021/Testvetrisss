using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.IO;
using System.Net;
using VETRIS.Core;
using eRADCls;
using DICOMLib;
using AjaxPro;
using DICOMLib;

namespace VETRIS.CaseList
{
    [AjaxPro.AjaxNamespace("VRSRADlgNew")]
    public partial class VRSRADlgNew : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.Case.CaseStudy objCore;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSRADlgNew));
            SetAttributes();
            if ((!CallBackST.CausedCallback) && (!CallBackSelST.CausedCallback) && (!CallBackSF.CausedCallback))
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);

            btnSave1.Attributes.Add("onclick", "javascript:btnSubmit_OnClick('N','X');");
            btnSave2.Attributes.Add("onclick", "javascript:btnSubmit_OnClick('N','X');");
            btnSubmit1.Attributes.Add("onclick", "javascript:btnSubmit_OnClick('Y','X');");
            btnSubmit2.Attributes.Add("onclick", "javascript:btnSubmit_OnClick('Y','X');");
            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnDel1.Attributes.Add("onclick", "javascript:btnDel_OnClick();");
            btnDel2.Attributes.Add("onclick", "javascript:btnDel_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

            //txtFromDt.Attributes.Add("onblur", "javascript:txtFromDt_OnBlur();");
            //imgFrom.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtFromDt.ClientID + "','CalFrom');");
            ddlCountry.Attributes.Add("onchange", "javascript:ddlCountry_OnChange();");
            ddlDOBMonth.Attributes.Add("onchange", "javascript:ddlDOBMonth_OnChange();");
            ddlDOBDay.Attributes.Add("onchange", "javascript:ddlDOBDay_OnChange();");
            ddlDOBYear.Attributes.Add("onchange", "javascript:ddlDOBYear_OnChange();");

            ddlModality.Attributes.Add("onchange", "javascript:LoadStudyTypes();");
            //ddlCategory.Attributes.Add("onchange", "javascript:LoadStudyTypes();");
            ddlSpecies.Attributes.Add("onchange", "javascript:ddlSpecies_OnChange();");
            ddlInstitution.Attributes.Add("onchange", "javascript:ddlInstitution_OnChange();");
            txtPWt.Attributes.Add("onkeypress", "javascript:return parent.CheckDecimal(event);");
            txtPWt.Attributes.Add("onblur", "javascript:ResetValueDecimal(this,3);");
            txtPWt.Attributes.Add("onfocus", "javascript:this.select();");
            rdoConsY.Attributes.Add("onclick", "javascript:Consult_OnClick();");
            rdoConsN.Attributes.Add("onclick", "javascript:Consult_OnClick();");
            ddlPriority.Attributes.Add("onchange", "javascript:ddlPriority_OnChange();");

            btnRefreshCount.Attributes.Add("onclick", "javascript:btnRefreshCount_OnClick();");
            //btnUpload.Attributes.Add("onclick", "javascript:btnUpload_OnClick();");


            //lblViewLog.Attributes.Add("onclick", "javascript:ViewLogUI('" + intMenuID.ToString() + "');");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            hdnID.Value = Request.QueryString["id"];
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            Guid SessionID = new Guid(Request.QueryString["sid"]);
            string strTheme = Request.QueryString["th"];
            
            //objComm = new classes.CommonClass();
            //objComm.SetRegionalFormat();
            //txtFromDt.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            //objComm = null;
            //CalFrom.DisabledDates.SelectRange(DateTime.Today.AddDays(1), Convert.ToDateTime("31Dec2050"));
            PopulateDOBDays(DateTime.Today);
            PopulateDOBYears();
            ddlDOBMonth.SelectedValue = DateTime.Today.Month.ToString();
            ddlDOBYear.SelectedValue = DateTime.Today.Year.ToString();
            hdnFilePath.Value = Server.MapPath("~");
            hdnTempSFFolder.Value = Server.MapPath("~") + "/CaseList/Temp/" + UserID.ToString();
            LoadHeader(intMenuID, UserID, SessionID);

            SetCSS(strTheme);
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

        #region CreateUserDirectory
        private void CreateUserDirectory(Guid UserID)
        {
            if (!Directory.Exists(Server.MapPath("~/CaseList/Temp/" + UserID.ToString())))
            {
                Directory.CreateDirectory(Server.MapPath("~/CaseList/Temp/" + UserID.ToString()));
            }
        }
        #endregion

        #region LoadHeader
        private void LoadHeader(int intMenuID, Guid UserID, Guid SessionID)
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objCore.ID = new Guid(hdnID.Value);
                objCore.MENU_ID = intMenuID;
                objCore.USER_ID = UserID;
                objCore.USER_SESSION_ID = SessionID;

                bReturn = objCore.LoadHeader(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    objComm.SetRegionalFormat();

                    if (objCore.STUDY_UID != string.Empty)
                    {

                        PopulateDropdowns(ds);
                        lblSUID.Text = objCore.STUDY_UID; hdnSUID.Value = objCore.STUDY_UID;
                        txtPID.Text = objCore.PATIENT_ID;
                        //txtPName.Text = objCore.PATIENT_NAME;
                        txtPFName.Text = objCore.PATIENT_FIRST_NAME;
                        txtPLName.Text = objCore.PATIENT_LAST_NAME;
                        ddlCountry.SelectedValue = objCore.PATIENT_COUNTRY_ID.ToString();
                        ddlState.SelectedValue = objCore.PATIENT_STATE_ID.ToString();
                        txtCity.Text = objCore.PATIENT_CITY;
                        ddlSex.SelectedValue = objCore.PATIENT_GENDER;
                        ddlSN.SelectedValue = objCore.SEX_NEUTERED;

                        txtPWt.Text = objComm.IMNumeric(objCore.PATIENT_WEIGHT, 3);
                        ddlUOM.SelectedValue = objCore.WEIGHT_UOM;
                        //if (objCore.PATIENT_DOB.Year > 1900) txtFromDt.Text = objComm.IMDateFormat(objCore.PATIENT_DOB, objComm.DateFormat);
                        //else txtFromDt.Text = string.Empty;

                        #region DOB
                        //if (objCore.PATIENT_DOB.Year > DateTime.Today.AddYears(-100).Year)
                        //{
                            ddlDOBDay.Items.Clear();
                            PopulateDOBDays(objCore.PATIENT_DOB);
                            ddlDOBMonth.SelectedValue = objCore.PATIENT_DOB.Month.ToString();
                            ddlDOBDay.SelectedValue = objComm.padZero(objCore.PATIENT_DOB.Day);
                            ddlDOBYear.SelectedValue = objCore.PATIENT_DOB.Year.ToString();
                            hdnDOBDay.Value = objComm.padZero(objCore.PATIENT_DOB.Day);
                        //}
                        #endregion

                        txtAge.Text = objCore.PATIENT_AGE;
                        ddlSpecies.SelectedValue = objCore.SPECIES_ID.ToString();
                        ddlBreed.SelectedValue = objCore.BREED_ID.ToString();
                        txtOwnerFN.Text = objCore.OWNER_FIRST_NAME; txtOwnerLN.Text = objCore.OWNER_LAST_NAME;
                        txtStudyDt.Text = objComm.IMDateFormat(objCore.STUDY_DATE, objComm.DateFormat);
                        txtReason.Text = objCore.REASON;
                        txtPhysNote.Text = objCore.PHYSICIAN_NOTE;
                        txtAccnNo.Text = objCore.ACCESSION_NO;
                        ddlPriority.SelectedValue = objCore.PRIORITY_ID.ToString();
                        ddlModality.SelectedValue = objCore.MODALITY_ID.ToString();
                        if (objCore.CATEGORY_ID > 0)
                            ddlCategory.SelectedValue = objCore.CATEGORY_ID.ToString();
                        else
                            ddlCategory.SelectedValue = "1";
                        ddlInstitution.SelectedValue = objCore.INSTITUTION_ID.ToString();
                        if (objCore.PHYSICIAN_ID.ToString() != "00000000-0000-0000-0000-000000000000") ddlPhys.SelectedValue = objCore.PHYSICIAN_ID.ToString();
                        else if (ddlPhys.Items.Count == 2) ddlPhys.SelectedIndex = 1;
                        txtImgCnt.Text = objCore.IMAGE_COUNT.ToString();
                        txtObjCnt.Text = objCore.OBJECT_COUNT.ToString();
                        if (objCore.IMAGE_COUNT_ACCEPTED == "Y") rdoConfYes.Checked = true;
                        else if (objCore.IMAGE_COUNT_ACCEPTED == "N") rdoConfNo.Checked = true;

                        if (objCore.CONSULT_APPLIED == "Y") rdoConsY.Checked = true;
                        else rdoConsN.Checked = true;

                        hdnPACSURL.Value = objCore.PACS_URL;
                        hdnImgVwrURL.Value = objCore.PACS_IMAGE_VIEWER_URL;
                        hdnImgCntURL.Value = objCore.PACS_IMAGE_COUNT_URL;
                        hdnStudyDelUrl.Value = objCore.PACS_STUDY_DELETE_URL;
                        hdnRecByRouter.Value = objCore.RECEIVE_DUCOM_FILES_VIA_ROUTER;
                        hdnInstConsAppl.Value = objCore.INSTITUTION_CONSULTATION_APPLICABLE;
                        hdnWS8SRVIP.Value = objCore.WEB_SERVICE_SERVER_URL;
                        hdnWS8CLTIP.Value = objCore.WEB_SERVICE_CLIENT_URL;
                        hdnWS8SRVUID.Value = objCore.WEB_SERVICE_USER_ID;
                        hdnWS8SRVPWD.Value = objCore.WEB_SERVICE_PASSWORD;
                        hdnAPIVER.Value = objCore.API_VERSION;
                        hdnFTPABSPATH.Value = objCore.FTP_ABSOLUTE_PATH;
                        hdnDCMMODIFYEXEPATH.Value = objCore.DCM_FILE_MODIFY_EXE_PATH;
                        hdnMergeCount.Value = objCore.STUDIES_TO_MERGE.ToString();
                        hdnAfterHrs.Value = objCore.BEYOND_OPERATION_HOUR;

                        #region Populate Merged Studies
                        int intImgCount = 0;

                        foreach (DataRow dr in ds.Tables["MergedStudies"].Rows)
                        {
                            if(Convert.ToString(dr["merge_compare_none"]) != "N") intImgCount = intImgCount + Convert.ToInt32(dr["image_count"]);
                            if (hdnMergeUID.Value.Trim() != string.Empty) hdnMergeUID.Value += objComm.RecordDivider;
                            hdnMergeUID.Value += Convert.ToString(dr["study_id"]) + objComm.RecordDivider + Convert.ToString(dr["study_uid"]) + objComm.RecordDivider + Convert.ToString(dr["image_count"]) + objComm.RecordDivider + Convert.ToString(dr["merge_compare_none"]);
                        }
                        if (intImgCount > 0)
                        {
                            lblImgCnt.Text = "&nbsp;+ additional " + intImgCount.ToString() + " image(s) for merge/comparison";
                        }
                        #endregion

                        #region PACS Info
                        lblPatientID.Text = objCore.PATIENT_ID_PACS;
                        lblAccnNo.Text = objCore.ACCESSION_NO_PACS;
                        //lblPatientName.Text = objCore.PATIENT_NAME_PACS;
                        //if (objCore.PATIENT_GENDER_PACS == "M") lblSex.Text = "Male";
                        //else if (objCore.PATIENT_GENDER_PACS == "F") lblSex.Text = "Female";
                        //else if (objCore.PATIENT_GENDER_PACS == "O") lblSex.Text = "Others";
                        //lblSN.Text = objCore.SEX_NEUTERED_PACS;
                        //lblWt.Text = objComm.IMNumeric(objCore.PATIENT_WEIGHT_PACS, 3);
                        //if (objCore.PATIENT_DOB_PACS.Year > 1900) lblDOB.Text = objComm.IMDateFormat(objCore.PATIENT_DOB_PACS, objComm.DateFormat);
                        //lblAge.Text = objCore.PATIENT_AGE_PACS;
                        //lblSpecies.Text = objCore.SPECIES_PACS;
                        //lblBreed.Text = objCore.BREED_PACS;
                        //lblOwner.Text = objCore.OWNER_NAME_PACS;
                        //lblReason.Text = objCore.REASON_PACS;
                        //lblModality.Text = objCore.MODALITY;
                        //lblBodyPart.Text = objCore.BODY_PART;
                        //lblDesc.Text = objCore.STUDY_DESCRIPTION;
                        //lblInstitution.Text = objCore.INSTITUTION_PACS;
                        //lblPhysician.Text = objCore.REFERRING_PHYSICIAN;
                        //lblImgCount.Text = objCore.IMAGE_COUNT_PACS.ToString();
                        #endregion
                    }
                    else
                    {
                        hdnError.Value = "false" + objComm.RecordDivider + "094";
                    }
                }
                else
                {
                    if (strCatchMessage.Trim() != string.Empty)
                        hdnError.Value = "catch" + objComm.RecordDivider + strCatchMessage.Trim();
                    else
                        hdnError.Value = "false" + objComm.RecordDivider + strReturnMessage.Trim();
                }

                CreateUserDirectory(UserID);
            }
            catch (Exception ex)
            {
                hdnError.Value = "catch" + objComm.RecordDivider + ex.Message.Trim();
            }
            finally
            {
                ds.Dispose(); objComm = null; objCore = null;
            }
        }
        #endregion

        #region PopulateDOBDays
        private void PopulateDOBDays(DateTime dt)
        {
            classes.CommonClass obj = new classes.CommonClass();
            int intLastDay = obj.GetLastDayOfMonth(dt.Month, dt.Year);
            for (int i = 1; i <= intLastDay; i++)
            {
                ListItem li = new ListItem(obj.padZero(i), obj.padZero(i));
                ddlDOBDay.Items.Add(li);
            }
            obj = null;
        }
        #endregion

        #region PopulateDOBYears
        private void PopulateDOBYears()
        {
            
            int intYearCurr = DateTime.Today.Year;
            //int intYearLast = intYearCurr - 100;
            int intYearLast = 1900;

            for (int i = intYearCurr; i >= intYearLast; i--)
            {
                ListItem li = new ListItem(i.ToString(), i.ToString());
                ddlDOBYear.Items.Add(li);
            }
  
        }
        #endregion

        #region PopulateDropdowns
        private void PopulateDropdowns(DataSet ds)
        {
            #region Species
            DataRow dr1 = ds.Tables["Species"].NewRow();
            dr1["id"] = 0;
            dr1["name"] = "Select One";
            ds.Tables["Species"].Rows.InsertAt(dr1, 0);
            ds.Tables["Species"].AcceptChanges();

            ddlSpecies.DataSource = ds.Tables["Species"];
            ddlSpecies.DataValueField = "id";
            ddlSpecies.DataTextField = "name";
            ddlSpecies.DataBind();
            #endregion

            #region Breed
            DataRow dr2 = ds.Tables["Breed"].NewRow();
            dr2["id"] = "00000000-0000-0000-0000-000000000000";
            if (ds.Tables["Breed"].Rows.Count > 0)
            {
                dr2["name"] = "Select One";
            }
            else
            {
                dr2["name"] = "Please select a species";
            }
            ds.Tables["Breed"].Rows.InsertAt(dr2, 0);
            ds.Tables["Breed"].AcceptChanges();

            ddlBreed.DataSource = ds.Tables["Breed"];
            ddlBreed.DataValueField = "id";
            ddlBreed.DataTextField = "name";
            ddlBreed.DataBind();
            #endregion

            #region Modality
            DataRow dr3 = ds.Tables["Modality"].NewRow();
            dr3["id"] = 0;
            dr3["name"] = "Select One";
            ds.Tables["Modality"].Rows.InsertAt(dr3, 0);
            ds.Tables["Modality"].AcceptChanges();

            ddlModality.DataSource = ds.Tables["Modality"];
            ddlModality.DataValueField = "id";
            ddlModality.DataTextField = "name";
            ddlModality.DataBind();
            #endregion

            #region Institution
            DataRow dr4 = ds.Tables["Institutions"].NewRow();
            dr4["id"] = "00000000-0000-0000-0000-000000000000";
            dr4["name"] = "Select One";
            ds.Tables["Institutions"].Rows.InsertAt(dr4, 0);
            ds.Tables["Institutions"].AcceptChanges();

            ddlInstitution.DataSource = ds.Tables["Institutions"];
            ddlInstitution.DataValueField = "id";
            ddlInstitution.DataTextField = "name";
            ddlInstitution.DataBind();
            #endregion

            #region Physicians
            DataRow dr5 = ds.Tables["Physicians"].NewRow();
            dr5["id"] = "00000000-0000-0000-0000-000000000000";
            dr5["name"] = "Select One";
            ds.Tables["Physicians"].Rows.InsertAt(dr5, 0);
            ds.Tables["Physicians"].AcceptChanges();

            ddlPhys.DataSource = ds.Tables["Physicians"];
            ddlPhys.DataValueField = "id";
            ddlPhys.DataTextField = "name";
            ddlPhys.DataBind();
            #endregion

            #region Priority

            foreach (DataRow dr in ds.Tables["Priority"].Rows)
            {
                if (hdnPriority.Value.Trim() != string.Empty) hdnPriority.Value += objComm.RecordDivider;
                hdnPriority.Value += Convert.ToString(dr["priority_id"]) + objComm.RecordDivider + Convert.ToString(dr["is_stat"]);
            }

            DataRow dr6 = ds.Tables["Priority"].NewRow();
            dr6["priority_id"] = "0";
            dr6["priority_desc"] = "Select One";
            dr6["is_stat"] = "N";
            ds.Tables["Priority"].Rows.InsertAt(dr6, 0);
            ds.Tables["Priority"].AcceptChanges();

            ddlPriority.DataSource = ds.Tables["Priority"];
            ddlPriority.DataValueField = "priority_id";
            ddlPriority.DataTextField = "priority_desc";
            ddlPriority.DataBind();

            
            #endregion

            #region Category
            //DataRow dr7 = ds.Tables["Category"].NewRow();
            //dr7["id"] = "0";
            //dr7["name"] = "Select One";
            //ds.Tables["Category"].Rows.InsertAt(dr7, 0);
            //ds.Tables["Category"].AcceptChanges();

            ddlCategory.DataSource = ds.Tables["Category"];
            ddlCategory.DataValueField = "id";
            ddlCategory.DataTextField = "name";
            ddlCategory.DataBind();
            #endregion

            #region Modality Service Available
            foreach (DataRow dr in ds.Tables["ModalityServiceAvailable"].Rows)
            {

                if (hdnModSvcAvbl.Value.Trim() != string.Empty) hdnModSvcAvbl.Value += objComm.RecordDivider;
                hdnModSvcAvbl.Value += Convert.ToString(dr["service_id"]) + objComm.RecordDivider + Convert.ToString(dr["modality_id"]) + objComm.RecordDivider + Convert.ToString(dr["priority_id"]) + objComm.RecordDivider + Convert.ToString(dr["available"]);
            }
            #endregion

            #region Modality Service Available After Hours
            foreach (DataRow dr in ds.Tables["ModalityServiceAvailableAH"].Rows)
            {

                if (hdnModSvcAvblAH.Value.Trim() != string.Empty) hdnModSvcAvblAH.Value += objComm.RecordDivider;
                hdnModSvcAvblAH.Value += Convert.ToString(dr["service_id"]) + objComm.RecordDivider + Convert.ToString(dr["modality_id"]) + objComm.RecordDivider + Convert.ToString(dr["priority_id"]) + objComm.RecordDivider + Convert.ToString(dr["available"]);
            }
            #endregion

            #region Modality Service Available Exception Institution
            foreach (DataRow dr in ds.Tables["ModalityServiceAvailableExInst"].Rows)
            {

                if (hdnModSvcAvblExInst.Value.Trim() != string.Empty) hdnModSvcAvblExInst.Value += objComm.RecordDivider;
                hdnModSvcAvblExInst.Value += Convert.ToString(dr["service_id"]) + objComm.RecordDivider + Convert.ToString(dr["modality_id"]) + objComm.RecordDivider + Convert.ToString(dr["priority_id"]) + objComm.RecordDivider + Convert.ToString(dr["institution_id"]);
            }
            #endregion

            #region Modality Service Available After Hours Exception Institution
            foreach (DataRow dr in ds.Tables["ModalityServiceAvailableAHExInst"].Rows)
            {

                if (hdnModSvcAvblAHExInst.Value.Trim() != string.Empty) hdnModSvcAvblAHExInst.Value += objComm.RecordDivider;
                hdnModSvcAvblAHExInst.Value += Convert.ToString(dr["service_id"]) + objComm.RecordDivider + Convert.ToString(dr["modality_id"]) + objComm.RecordDivider + Convert.ToString(dr["priority_id"]) + objComm.RecordDivider + Convert.ToString(dr["institution_id"]);
            }
            #endregion

            #region Species Service Available
            foreach (DataRow dr in ds.Tables["SpeciesServiceAvailable"].Rows)
            {

                if (hdnSpcSvcAvbl.Value.Trim() != string.Empty) hdnSpcSvcAvbl.Value += objComm.RecordDivider;
                hdnSpcSvcAvbl.Value += Convert.ToString(dr["service_id"]) + objComm.RecordDivider + Convert.ToString(dr["Species_id"]) + objComm.RecordDivider + Convert.ToString(dr["priority_id"]) + objComm.RecordDivider + Convert.ToString(dr["available"]);
            }
            #endregion

            #region Species Service Available After Hours
            foreach (DataRow dr in ds.Tables["SpeciesServiceAvailableAH"].Rows)
            {

                if (hdnSpcSvcAvblAH.Value.Trim() != string.Empty) hdnSpcSvcAvblAH.Value += objComm.RecordDivider;
                hdnSpcSvcAvblAH.Value += Convert.ToString(dr["service_id"]) + objComm.RecordDivider + Convert.ToString(dr["Species_id"]) + objComm.RecordDivider + Convert.ToString(dr["priority_id"]) + objComm.RecordDivider + Convert.ToString(dr["available"]);
            }
            #endregion

            #region Species Service Available Exception Institution
            foreach (DataRow dr in ds.Tables["SpeciesServiceAvailableExInst"].Rows)
            {

                if (hdnSpcSvcAvblExInst.Value.Trim() != string.Empty) hdnSpcSvcAvblExInst.Value += objComm.RecordDivider;
                hdnSpcSvcAvblExInst.Value += Convert.ToString(dr["service_id"]) + objComm.RecordDivider + Convert.ToString(dr["Species_id"]) + objComm.RecordDivider + Convert.ToString(dr["priority_id"]) + objComm.RecordDivider + Convert.ToString(dr["institution_id"]);
            }
            #endregion

            #region Species Service Available After Hours Exception Institution
            foreach (DataRow dr in ds.Tables["SpeciesServiceAvailableAHExInst"].Rows)
            {

                if (hdnSpcSvcAvblAHExInst.Value.Trim() != string.Empty) hdnSpcSvcAvblAHExInst.Value += objComm.RecordDivider;
                hdnSpcSvcAvblAHExInst.Value += Convert.ToString(dr["service_id"]) + objComm.RecordDivider + Convert.ToString(dr["Species_id"]) + objComm.RecordDivider + Convert.ToString(dr["priority_id"]) + objComm.RecordDivider + Convert.ToString(dr["institution_id"]);
            }
            #endregion

            #region Country
            DataRow dr7 = ds.Tables["Country"].NewRow();
            dr7["id"] = "0";
            dr7["name"] = "Select Country";
            ds.Tables["Country"].Rows.InsertAt(dr7, 0);
            ds.Tables["Country"].AcceptChanges();

            ddlCountry.DataSource = ds.Tables["Country"];
            ddlCountry.DataValueField = "id";
            ddlCountry.DataTextField = "name";
            ddlCountry.DataBind();
            #endregion

            #region States
            DataRow dr8 = ds.Tables["State"].NewRow();
            dr8["id"] = "0";
            dr8["name"] = "Select State";
            ds.Tables["State"].Rows.InsertAt(dr8, 0);
            ds.Tables["State"].AcceptChanges();

            ddlState.DataSource = ds.Tables["State"];
            ddlState.DataValueField = "id";
            ddlState.DataTextField = "name";
            ddlState.DataBind();
            #endregion

        }
        #endregion

        #region Study Type Grid

        #region CallBackST_Callback
        protected void CallBackST_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadStudyTypes(e.Parameters);
            grdST.Width = Unit.Percentage(100);
            grdST.RenderControl(e.Output);
            spnErrST.RenderControl(e.Output);
            spnTrackBy.RenderControl(e.Output);
            spnInvBy.RenderControl(e.Output);
        }
        #endregion

        #region LoadStudyTypes
        private void LoadStudyTypes(string[] arrParams)
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            

            try
            {

                objCore.ID = new Guid(arrParams[0]);
                objCore.MODALITY_ID = Convert.ToInt32(arrParams[1]);
                objCore.INSTITUTION_ID = new Guid(arrParams[2]);

                bReturn = objCore.LoadStudyTypes(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdST.DataSource = ds.Tables["StudyTypes"];
                    grdST.DataBind();
                    spnTrackBy.InnerHtml = "<input type=\"hidden\" id=\"hdnModTrackBy\" value=\"I\" />";
                    spnInvBy.InnerHtml = "<input type=\"hidden\" id=\"hdnModInvBy\" value=\"\" />";

                    foreach (DataRow dr in ds.Tables["TrackBy"].Rows)
                    {
                        spnTrackBy.InnerHtml = "<input type=\"hidden\" id=\"hdnModTrackBy\" value=\"" + Convert.ToString(dr["track_by"]) + "\" />";
                        spnInvBy.InnerHtml = "<input type=\"hidden\" id=\"hdnModInvBy\" value=\"" + Convert.ToString(dr["invoice_by"]) + "\" />";
                    }

                    spnErrST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrST\" value=\"\" />";
                }
                else
                {
                    spnTrackBy.InnerHtml = "<input type=\"hidden\" id=\"hdnModTrackBy\" value=\"I\" />";
                    spnInvBy.InnerHtml = "<input type=\"hidden\" id=\"hdnModInvBy\" value=\"\" />";
                    spnErrST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrST\" value=\"" + strCatchMessage + "\" />";
                }
            }
            catch (Exception ex)
            {
                spnErrST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrST\" value=\"" + ex.Message.Trim() + "\" />";
                spnTrackBy.InnerHtml = "<input type=\"hidden\" id=\"hdnModTrackBy\" value=\"I\" />";
                spnInvBy.InnerHtml = "<input type=\"hidden\" id=\"hdnModInvBy\" value=\"\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #endregion

        #region Selected Study Type Grid

        #region CallBackSelST_Callback
        protected void CallBackSelST_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];
            switch (strAction)
            {
                case "L":
                    LoadSelectedStudyTypes(e.Parameters);
                    break;
                case "U":
                    UpdateStudyTypes(e.Parameters);
                    break;
            }

            grdSelST.Width = Unit.Percentage(99);
            grdSelST.RenderControl(e.Output);
            spnErrSelST.RenderControl(e.Output);
        }
        #endregion

        #region LoadSelectedStudyTypes
        private void LoadSelectedStudyTypes(string[] arrParams)
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strInvBy = "";

            try
            {

                objCore.ID = new Guid(arrParams[1]);
                objCore.MODALITY_ID = Convert.ToInt32(arrParams[2]);
                objCore.INSTITUTION_ID = new Guid(arrParams[3]);
                strInvBy = arrParams[4];

                bReturn = objCore.LoadSelectedStudyTypes(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdSelST.DataSource = ds.Tables["SelStudyTypes"];
                    grdSelST.DataBind();
                    if (strInvBy == "B") grdSelST.GroupingNotificationText = "Study Type Count : " + ds.Tables["SelStudyTypes"].Rows.Count.ToString();

                    spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelST\" value=\"\" />";
                }
                else
                    spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelST\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelST\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region UpdateStudyTypes
        private void UpdateStudyTypes(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);
            string strInvBy = arrParams[2];
            int intSrl = 0;

            try
            {
                dtbl = CreateSTTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 2)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["srl_no"] = intSrl;
                            dr["study_type_id"] = arrRecords[i].Trim();
                            dr["study_type_name"] = arrRecords[i + 1].Trim();
                            dtbl.Rows.Add(dr);
                        }

                    }
                }



                grdSelST.DataSource = dtbl;
                grdSelST.DataBind();
                if (strInvBy == "B") grdSelST.GroupingNotificationText = "Study Type Count : " + dtbl.Rows.Count.ToString();
                spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelST\" value=\"UPDATE\" />";
            }
            catch (Exception ex)
            {
                spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelST\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region CreateSTTable
        private DataTable CreateSTTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("srl_no", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("study_type_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("study_type_name", System.Type.GetType("System.String"));
            dtbl.TableName = "SelStudyTypes";
            return dtbl;
        }
        #endregion

        #endregion

        #region Study File Grid

        #region CallBackSF_Callback
        protected void CallBackSF_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];

            switch (strAction)
            {
                case "L":
                    LoadFiles(e.Parameters);
                    break;
                case "A":
                    AddFiles(e.Parameters);
                    break;
                case "D":
                    DeleteFiles(e.Parameters);
                    break;
                case "C":
                    ClearFiles(e.Parameters);
                    break;
            }
            grdSF.Width = Unit.Percentage(100);
            grdSF.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadFiles
        private void LoadFiles(string[] arrParams)
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";

            try
            {

                objCore.ID = new Guid(arrParams[1]);
                UserID = new Guid(arrParams[2]);

                bReturn = objCore.LoadStudyFiles(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdSF.DataSource = ds.Tables["Files"];
                    grdSF.DataBind();

                    foreach (DataRow dr in ds.Tables["Files"].Rows)
                    {

                        strFileName = Convert.ToString(dr["file_name"]);
                        SetFile((byte[])dr["file_content"], Convert.ToString(dr["file_name"]).Trim(), "CaseList/Temp/" + UserID.ToString());

                    }

                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";
                }
                else
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region AddFiles
        private void AddFiles(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);
            string[] arrNew = arrParams[2].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;
            string strAccnNo = string.Empty;
            string strPID = string.Empty;
            string strFile = string.Empty;
            string strMimeType = string.Empty;
            string strUserID = arrParams[3];
            DICOMLib.DicomDecoder dd = new DICOMLib.DicomDecoder();

            try
            {
                dtbl = CreateStudyFileTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 5)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["id"] = arrRecords[i];
                            dr["file_srl_no"] = intSrl;
                            dr["file_name"] = strFile = arrRecords[i + 2].Trim().Substring(arrRecords[i + 2].Trim().LastIndexOf("/") + 1, (arrRecords[i + 2].Trim().Length - (arrRecords[i + 2].Trim().LastIndexOf("/") + 1)));
                            dr["file_type"] = arrRecords[i + 3].Trim();
                            if (arrRecords[i + 3].Trim() == "D") dr["file_type_desc"] = "DICOM";
                            else if (arrRecords[i + 3].Trim() == "I")
                            {
                                strMimeType = classes.MIMEAssistant.GetMIMEType(Server.MapPath("~") + "/CaseList/Temp/" + strUserID + "/" + strFile);
                                switch (strMimeType)
                                {
                                    case "image/jpeg":
                                        dr["file_type_desc"] = "JPG/JPEG";
                                        break;
                                    case "image/gif":
                                        dr["file_type_desc"] = "GIF";
                                        break;
                                    case "image/png":
                                        dr["file_type_desc"] = "PNG";
                                        break;
                                    case "image/bmp":
                                        dr["file_type_desc"] = "BMP";
                                        break;
                                }

                            }
                            else if (arrRecords[i + 3].Trim() == "P") dr["file_type_desc"] = "PDF";
                            dtbl.Rows.Add(dr);

                           
                        }

                    }
                }

                for (int i = 0; i < arrNew.Length; i = i + 2)
                {
                    DataRow drNew = dtbl.NewRow();
                    intSrl = intSrl + 1;
                    drNew["id"] = "00000000-0000-0000-0000-000000000000";
                    drNew["file_srl_no"] = intSrl;
                    drNew["file_name"] = strFile = arrNew[i].Trim().Substring(arrNew[i].Trim().LastIndexOf("/") + 1, (arrNew[i].Trim().Length - (arrNew[i].Trim().LastIndexOf("/") + 1)));
                    drNew["file_type"] = arrNew[i + 1];
                    if (arrNew[i + 1].Trim() == "D") drNew["file_type_desc"] = "DICOM";
                    else if (arrNew[i + 1].Trim() == "I")
                    {
                        strMimeType = classes.MIMEAssistant.GetMIMEType(Server.MapPath("~") + "/CaseList/Temp/" + strUserID + "/" + strFile);
                        switch (strMimeType)
                        {
                            case "image/jpeg":
                                drNew["file_type_desc"] = "JPG/JPEG";
                                break;
                            case "image/gif":
                                drNew["file_type_desc"] = "GIF";
                                break;
                            case "image/png":
                                drNew["file_type_desc"] = "PNG";
                                break;
                            case "image/bmp":
                                drNew["file_type_desc"] = "BMP";
                                break;
                        }

                    }
                    else if (arrNew[i + 1].Trim() == "P") drNew["file_type_desc"] = "PDF";

                    dtbl.Rows.Add(drNew);

                    

                }

                grdSF.DataSource = dtbl;
                grdSF.DataBind();
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
                dd = null;
            }
        }
        #endregion

        #region DeleteFiles
        private void DeleteFiles(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            int intID = Convert.ToInt32(arrParams[1]);
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            string strFolder = arrParams[3];
            string strFileName = string.Empty;
            int intSrl = 0;
            int intLoop = 0;
            string strAccnNo = string.Empty;
            string strPID = string.Empty;

            try
            {
                dtbl = CreateStudyFileTable();
                strFolder = strFolder.Replace("\\", "/");

                if (arrRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrRecords.Length; i = i + 5)
                    {
                        if (Convert.ToInt32(arrRecords[i + 1]) != intID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["id"] = arrRecords[i].Trim(); ;
                            dr["file_srl_no"] = intSrl;
                            dr["file_name"] = arrRecords[i + 2].Trim();
                            dr["file_type"] = arrRecords[i + 3].Trim();
                            dr["file_type_desc"] = arrRecords[i + 4].Trim();
                            dtbl.Rows.Add(dr);
                            intLoop = intLoop + 1;
                        }
                        else
                            strFileName = arrRecords[i + 2].Trim();
                    }
                }

                grdSF.DataSource = dtbl;
                grdSF.DataBind();
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

                if (strFileName.Trim() != string.Empty)
                {
                    if (File.Exists(strFolder + "/" + strFileName))
                    {
                        File.Delete(strFolder + "/" + strFileName);
                    }
                }

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
        }
        #endregion

        #region ClearFiles
        private void ClearFiles(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();

            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[0].Split(arrSep, StringSplitOptions.None);
            string strFolder = arrParams[1];
            string strFileName = string.Empty;


            try
            {
                dtbl = CreateStudyFileTable();
                if (arrRecords[0].Trim() != "")
                {
                    strFolder = strFolder.Replace("\\", "/");
                    for (int i = 0; i < arrRecords.Length; i = i + 5)
                    {
                        strFileName = arrRecords[i + 2].Trim();
                        if (strFileName.Trim() != string.Empty)
                        {
                            if (File.Exists(strFolder + "/" + strFileName))
                            {
                                File.Delete(strFolder + "/" + strFileName);
                            }
                        }
                    }
                }

                grdSF.DataSource = dtbl;
                grdSF.DataBind();
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
        }
        #endregion

        #region CreateStudyFileTable
        private DataTable CreateStudyFileTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("file_srl_no", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("file_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("file_type", System.Type.GetType("System.String"));
            dtbl.Columns.Add("file_type_desc", System.Type.GetType("System.String"));
            dtbl.TableName = "files";
            return dtbl;
        }
        #endregion

        #endregion

        #region File Methods

        #region SetFile
        private void SetFile(byte[] DocData, string strFileName, string strPath)
        {
            string strFilePath = Server.MapPath("~") + "/" + strPath + "/" + strFileName;

            if (!Directory.Exists(Server.MapPath("~") + "/" + strPath)) Directory.CreateDirectory(Server.MapPath("~") + "/" + strPath);

            using (FileStream fs = new FileStream(strFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(DocData, 0, DocData.Length);
                fs.Flush();
                fs.Close();
            }

        }
        #endregion

        #region GetFileBytes
        private byte[] GetFileBytes(string strFileName)
        {
            byte[] buff = File.ReadAllBytes(strFileName);
            return buff;
        }
        #endregion

        #endregion

        #region FetchStates (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchStates(string[] arrParams)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Core.CommonFunctions objCF = new CommonFunctions();

            int i = 0;
            string[] arrRet = new string[0];

            try
            {

                objCF.RECORD_ID_INT = Convert.ToInt32(arrParams[0].Trim());
                bReturn = objCF.FetchCountryWiseStates(Server.MapPath("~"), ref ds, ref strCatchMessage);

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

        #region FetchLastDayOfMonth (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchLastDayOfMonth(string[] arrParams)
        {
            
            int i = 0;
            string[] arrRet = new string[0];
            int intLastDay = 0;
            objComm = new classes.CommonClass();

            try
            {
                intLastDay = objComm.GetLastDayOfMonth(Convert.ToInt32(arrParams[0]), Convert.ToInt32(arrParams[1]));
                arrRet = new string[2];
                arrRet[0] = "true";
                arrRet[1] = intLastDay.ToString();

            }
            catch (Exception ex)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = ex.Message.ToString();
            }
            finally
            {
                objComm = null;
            }
            
            return arrRet;
        }
        #endregion

        #region FetchBreeds (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchBreeds(string[] arrParams)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objCore = new Core.Case.CaseStudy();

            int i = 0;
            string[] arrRet = new string[0];

            try
            {

                objCore.SPECIES_ID = Convert.ToInt32(arrParams[0].Trim());


                bReturn = objCore.FetchSpeciesWiseBreed(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    if (ds.Tables["Breed"].Rows.Count > 0)
                    {

                        arrRet = new string[(ds.Tables["Breed"].Rows.Count * 2) + 3];
                        arrRet[0] = "true";
                        arrRet[1] = "00000000-0000-0000-0000-000000000000";
                        arrRet[2] = "Select One";
                        i = 3;

                        foreach (DataRow dr in ds.Tables["Breed"].Rows)
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
                        arrRet[1] = "00000000-0000-0000-0000-000000000000";

                        if (Convert.ToInt32(arrParams[0].Trim()) == 0)
                        {
                            arrRet[2] = "Please select a species";
                        }
                        else
                        {
                            arrRet[2] = "No breed found for this species";
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

        #region FetchPhysicians (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchPhysicians(string[] arrParams)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objCore = new Core.Case.CaseStudy();

            int i = 0;
            string[] arrRet = new string[0];

            try
            {

                objCore.INSTITUTION_ID = new Guid(arrParams[0].Trim());
                bReturn = objCore.FetchInstitutionWisePhysician(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[(ds.Tables["Physicians"].Rows.Count * 2) + 4];
                    arrRet[0] = "true";



                    if (ds.Tables["Physicians"].Rows.Count > 0)
                    {
                        arrRet[1] = "00000000-0000-0000-0000-000000000000";
                        arrRet[2] = "Select One";
                        i = 3;

                        foreach (DataRow dr in ds.Tables["Physicians"].Rows)
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
                        arrRet[1] = "00000000-0000-0000-0000-000000000000";
                        arrRet[2] = "Please select an instititution";
                        i = 3;
                    }
                    if (ds.Tables["Consult"].Rows.Count > 0)
                    {
                        arrRet[i] = Convert.ToString(ds.Tables["Consult"].Rows[0]["consult_applicable"]);

                    }
                    else
                    {
                        arrRet[i] = "N";
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

        #region GetImageCount (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] GetImageCount(string Id)
        {

            string strCatchMessage = string.Empty;
            string[] arrRet = new string[0];
            objCore = new Core.Case.CaseStudy();
            bool bReturn = false;

            try
            {
                objCore.ID = new Guid(Id.Trim());

                bReturn = objCore.FetchImageCount(Server.MapPath("~"), ref strCatchMessage);

                if (bReturn)
                {

                    arrRet = new string[3];
                    arrRet[0] = "true";
                    arrRet[1] = objCore.IMAGE_COUNT.ToString();
                    arrRet[2] = objCore.OBJECT_COUNT.ToString();

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
                arrRet[1] = ex.Message;
            }
            finally { objCore = null; }

            return arrRet;
        }
        #endregion

        #region GetImageCountAPI_72 (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] GetImageCountAPI_72(string URL)
        {
            WebClient client = new WebClient();
            string strResult = string.Empty;
            //string strImgCnt = string.Empty;
            //string strObjCnt = string.Empty;
            string strRes = string.Empty;
            string[] arrRet = new string[0];
            string[] err = new string[0];
            string[] arrData = new string[0];
            string[] arrFields = new string[0];
            string[] arrRecSep = { "\n" };
            string[] arrFldSep = { "\t" };
            int intErrFlg = 0;

            try
            {

                IgnoreBadCertificates();
                byte[] data = client.DownloadData(URL);
                strResult = System.Text.Encoding.Default.GetString(data);
                strResult = strResult.Replace("### Begin_Table's_Content ###", "");
                strResult = strResult.Replace("### End_Table's_Content ###", "");

                strRes = strResult;
                if (strRes.IndexOf("#USERID:") > 0)
                {
                    strRes = strRes.Substring(1, strResult.IndexOf("#USERID:") - 1);
                    strRes = strRes.Replace("\r", "");
                    strRes = strRes.Trim();
                    intErrFlg = 1;
                }
                else
                    intErrFlg = 0;

                //strResult = strResult.Substring(1, strResult.IndexOf("#USERID:") - 1);
                //strResult = strResult.Replace("\r", "");
                //strResult = strResult.Trim();

                if (intErrFlg == 1)
                {
                    arrData = strRes.Split(arrRecSep, StringSplitOptions.None);
                    arrFields = arrData[0].Split(arrFldSep, StringSplitOptions.None);

                    arrRet = new string[3];
                    arrRet[0] = "true";
                    arrRet[1] = arrFields[0];
                    if (Convert.ToInt32(arrFields[1]) > 0) arrRet[2] = Convert.ToString(Convert.ToInt32(arrFields[1]));
                    else arrRet[2] = "0";

                }
                else
                {
                    err = strResult.Split('#');
                    arrRet = new string[2];
                    arrRet[0] = "false";
                    if (err.Length == 4)
                    {
                        arrRet[1] = err[3].Replace("ERROR: ", "");
                    }
                    else if (err.Length == 3)
                    {
                        if (err[2].Split(':')[1].Trim() == "OK")
                            arrRet[1] = "Study has been deleted from PACS";
                    }
                }
            }
            catch (Exception ex)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = ex.Message;
            }


            return arrRet;
        }
        #endregion

        #region GetImageCountAPI_80 (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] GetImageCountAPI_80(string[] ArrRecord)
        {
            bool bReturn = false;
            RadWebClass client = new RadWebClass();
            string strSUID = string.Empty; Guid StudyID = Guid.Empty; int intMenuID = 0; Guid UserID = Guid.Empty; Guid SessionID = Guid.Empty;
            string strWSURL = string.Empty; string strClientIP = string.Empty; string strWSUserID = string.Empty; string strWSPwd = string.Empty;
            string strColID = string.Empty; string strValue = string.Empty;
            string[] arrRet = new string[0];
            string strCatchMessage = string.Empty; string strReturnMsg = string.Empty; string strErr = string.Empty;
            string strResult = string.Empty; string strSession = string.Empty;
            string[] arrFields = new string[0];
            string[] err = new string[0];
            DataSet ds = new DataSet();
            int intImgCount = 0;
            int intObjCount = 0;
            objComm = new classes.CommonClass();
            objCore = new Core.Case.CaseStudy();

            try
            {
                StudyID = new Guid(ArrRecord[0].Trim());
                strSUID = ArrRecord[1].Trim().Replace(" ", "");
                strWSURL = ArrRecord[2].Trim();
                strClientIP = ArrRecord[3].Trim();
                strWSUserID = ArrRecord[4].Trim();
                strWSPwd = CoreCommon.DecryptString(ArrRecord[5].Trim());
                UserID = new Guid(ArrRecord[6].Trim());
                intMenuID = Convert.ToInt32(ArrRecord[7].Trim());
                SessionID = new Guid(ArrRecord[8].Trim());

                //bReturn = client.GetSession(strClientIP, strWSURL, strWSUserID,strWSPwd, ref strSession, ref strCatchMessage, ref strErr);
                //if (bReturn)
                //{
                arrFields = new string[2];
                arrFields[0] = "NIMG";
                arrFields[1] = "NOBJ";

                bReturn = client.GetStudyData(strSession, strWSURL, strSUID, ref strResult, ref strCatchMessage, ref strErr);
                if (bReturn)
                {
                    System.IO.StringReader xmlSR = new System.IO.StringReader(strResult);
                    ds.ReadXml(xmlSR);
                    if (ds.Tables.Contains("Field"))
                    {
                        arrRet = new string[4];
                        arrRet[0] = "";

                        #region Get Image Count
                        foreach (DataRow dr in ds.Tables["Field"].Rows)
                        {
                            strColID = Convert.ToString(dr["Colid"]).Trim();
                            strValue = Convert.ToString(dr["Value"]).Trim();
                            switch (strColID)
                            {
                                case "NIMG":
                                    if (dr["Value"] != DBNull.Value)
                                    {
                                        if (IsInteger(Convert.ToString(dr["Value"])))
                                            arrRet[1] = Convert.ToString(dr["Value"]);
                                        else
                                            arrRet[1] = "0";
                                    }
                                    else
                                        arrRet[1] = "0";

                                    intImgCount = Convert.ToInt32(arrRet[1]);
                                    break;
                                case "NOBJ":
                                    if (dr["Value"] != DBNull.Value)
                                    {
                                        if (Convert.ToInt32(dr["Value"]) > 0)
                                        {
                                            if (IsInteger(Convert.ToString(dr["Value"])))
                                                arrRet[2] = Convert.ToString(Convert.ToInt32(dr["Value"]));
                                            else
                                                arrRet[2] = "0";
                                        }
                                        else if ((dr["Value"] == DBNull.Value) || (Convert.ToString(dr["Value"]).Trim() == string.Empty))
                                        {
                                            if (IsInteger(Convert.ToString(dr["Value"])))
                                                arrRet[2] = Convert.ToString(dr["Value"]);
                                            else
                                                arrRet[2] = "0";
                                        }
                                    }
                                    else
                                        arrRet[2] = "0";

                                    intObjCount = Convert.ToInt32(arrRet[2]);
                                    break;
                            }
                        }
                        #endregion

                        objCore.ID = StudyID;
                        objCore.IMAGE_COUNT = intImgCount;
                        objCore.OBJECT_COUNT = intObjCount;
                        objCore.USER_ID = UserID;
                        objCore.MENU_ID = intMenuID;
                        objCore.USER_SESSION_ID = SessionID;

                        bReturn = objCore.UpdateImageObjectCount(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                        if (bReturn)
                        {
                            arrRet[0] = "true";
                        }
                        else
                        {
                            arrRet[0] = "false";
                            if (strReturnMsg.Trim() != string.Empty) arrRet[4] = strReturnMsg; else arrRet[4] = strCatchMessage;
                        }
                    }
                    else
                    {
                        arrRet = new string[2];
                        arrRet[0] = "false";
                        arrRet[1] = "094";
                    }
                }
                else
                {
                    arrRet = new string[2];
                    arrRet[0] = "false";
                    if (strCatchMessage.Trim() != string.Empty)
                        arrRet[1] = strCatchMessage.Trim();
                    else
                        arrRet[1] = strErr.Trim();
                }
                //}


            }
            catch (Exception ex)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = ex.Message;
            }
            finally
            {
                objComm = null;
                objCore = null;
            }


            return arrRet;
        }
        #endregion

        #region GetServiceAvailabilityMessage (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] GetServiceAvailabilityMessage(string[] arrParams)
        {
            string strReturn = string.Empty;
            string strReturnMessage = ""; string strCatchMessage = ""; bool bReturn = false;
            objCore = new Core.Case.CaseStudy();

            int i = 0;
            string[] arrRet = new string[4];

            try
            {

                objCore.SPECIES_ID = Convert.ToInt32(arrParams[0].Trim());
                objCore.MODALITY_ID = Convert.ToInt32(arrParams[1].Trim());
                objCore.INSTITUTION_ID = new Guid(arrParams[2].Trim());
                objCore.PRIORITY_ID = Convert.ToInt32(arrParams[3].Trim());


                bReturn = objCore.GetServiceAvailabilityMessage(Server.MapPath("~"), ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {

                    arrRet = new string[3];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMessage.Trim();

                }
                else
                {
                    arrRet = new string[2];
                    if (strCatchMessage.Trim() != string.Empty)
                    {
                        arrRet[0] = "false";
                        arrRet[1] = strCatchMessage.Trim();
                    }
                    else
                    {
                        arrRet[0] = "false";
                        arrRet[1] = strReturnMessage.Trim();
                    }
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
                objCore = null;
            }
            return arrRet;
        }
        #endregion

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord, string[] ArrSTs, string[] ArrDocs, string[] ArrDCM,string[] ArrMerge)
        {
            bool bReturn = false; bool bFileValid = true;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty; string strExtn = string.Empty;
            string strAccnNoGenerated = string.Empty; string strSUID = string.Empty; string strFile = string.Empty; string strOutputMsg = string.Empty;
            string strFileName = string.Empty; Guid UserID = Guid.Empty; string strDCMMODIFYEXEPATH = string.Empty; string strInsName = string.Empty;
            DICOMLib.DicomDecoder dd = new DICOMLib.DicomDecoder();
            objComm = new classes.CommonClass();
            objCore = new Core.Case.CaseStudy();
            int intListIndex = 0;
            Core.Case.StudyTypeList[] objSTs = new Core.Case.StudyTypeList[0];
            Core.Case.HeaderDocumentList[] objDocs = new Core.Case.HeaderDocumentList[0];
            Core.Case.HeaderDICOMList[] objDCM = new Core.Case.HeaderDICOMList[0];
            Core.Case.StudyToMerge[] objMerge = new Core.Case.StudyToMerge[0];

            try
            {
                objComm.SetRegionalFormat();

                #region Header Values
                objCore.ID = new Guid(ArrRecord[0]);
                objCore.PATIENT_ID = ArrRecord[1].Trim();
                objCore.PATIENT_FIRST_NAME = ArrRecord[2].Trim();
                objCore.PATIENT_LAST_NAME = ArrRecord[3].Trim();
                objCore.PATIENT_NAME = ArrRecord[3].Trim() + " " + ArrRecord[2].Trim();
                objCore.PATIENT_WEIGHT = Convert.ToDecimal(ArrRecord[4]);
                objCore.PATIENT_DOB = Convert.ToDateTime(ArrRecord[5]);
                objCore.PATIENT_AGE = ArrRecord[6].Trim();
                objCore.PATIENT_GENDER = ArrRecord[7].Trim();
                objCore.SEX_NEUTERED = ArrRecord[8].Trim();
                objCore.SPECIES_ID = Convert.ToInt32(ArrRecord[9]);
                objCore.BREED_ID = new Guid(ArrRecord[10]);
                objCore.OWNER_FIRST_NAME = ArrRecord[11].Trim();
                objCore.OWNER_LAST_NAME = ArrRecord[12].Trim();
                objCore.ACCESSION_NO = ArrRecord[13].Trim();
                objCore.MODALITY_ID = Convert.ToInt32(ArrRecord[14]);
                objCore.REASON = ArrRecord[15].Trim();
                objCore.IMAGE_COUNT = Convert.ToInt32(ArrRecord[16]);
                objCore.OBJECT_COUNT = Convert.ToInt32(ArrRecord[17]);
                objCore.IMAGE_COUNT_ACCEPTED = ArrRecord[18].Trim();
                objCore.INSTITUTION_ID = new Guid(ArrRecord[19]);
                objCore.PHYSICIAN_ID = new Guid(ArrRecord[20]);
                objCore.WRITE_BACK = ArrRecord[21].Trim();
                objCore.WEIGHT_UOM = ArrRecord[22].Trim();
                objCore.PRIORITY_ID = Convert.ToInt32(ArrRecord[23]);
                objCore.MERGE_STATUS = ArrRecord[24].Trim();
                objCore.PHYSICIAN_NOTE = ArrRecord[25].Trim();
                objCore.CONSULT_APPLIED = ArrRecord[26].Trim();
                objCore.CATEGORY_ID = Convert.ToInt32(ArrRecord[27]);
                strSUID = ArrRecord[28].Trim();
                objCore.FTP_ABSOLUTE_PATH = ArrRecord[29].Trim();
                objCore.SOURCE_PATH = ArrRecord[30].Trim();
                strDCMMODIFYEXEPATH = ArrRecord[31].Trim();
                strInsName = ArrRecord[32].Trim();
                objCore.SUBMIT_PRIORITY = ArrRecord[33].Trim();
                objCore.SENDER_TIME_ZONE_OFFSET_MINUTES = Convert.ToInt32(ArrRecord[34]);
                objCore.PATIENT_COUNTRY_ID = Convert.ToInt32(ArrRecord[35]);
                objCore.PATIENT_STATE_ID = Convert.ToInt32(ArrRecord[36]);
                objCore.PATIENT_CITY = ArrRecord[37].Trim();
                objCore.CHECK_DOB = ArrRecord[38].Trim();
                objCore.USER_ID = UserID = new Guid(ArrRecord[39]);
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[40]);
                objCore.USER_SESSION_ID = new Guid(ArrRecord[41]);

                #endregion

                #region Populate Study Types
                objSTs = new Core.Case.StudyTypeList[(ArrSTs.Length)];

                for (int i = 0; i < objSTs.Length; i++)
                {
                    objSTs[i] = new Core.Case.StudyTypeList();
                    objSTs[i].SERIAL_NUMBER = i + 1;
                    objSTs[i].ID = new Guid(ArrSTs[intListIndex]);
                    intListIndex = intListIndex + 1;
                }
                #endregion

                #region Populate Documents
                intListIndex = 0;
                objDocs = new Core.Case.HeaderDocumentList[(ArrDocs.Length / 3)];

                for (int i = 0; i < objDocs.Length; i++)
                {
                    strFileName = Server.MapPath("~/CaseList/Temp/" + UserID.ToString() + "/") + ArrDocs[intListIndex + 2].Trim();
                    strFileName = strFileName.Replace("ajaxpro\\", "");
                    strExtn = Path.GetExtension(strFileName);

                    objDocs[i] = new Core.Case.HeaderDocumentList();
                    objDocs[i].SERIAL_NUMBER = Convert.ToInt32(ArrDocs[intListIndex]);
                    objDocs[i].ID = new Guid(ArrDocs[intListIndex + 1]);
                    objDocs[i].NAME = ArrDocs[intListIndex + 2].Trim();
                    objDocs[i].FILE_NAME = ArrDocs[intListIndex + 2].Trim();
                    objDocs[i].FILE_TYPE = strExtn;
                    objDocs[i].FILE_CONTENT = GetFileBytes(strFileName);

                    intListIndex = intListIndex + 3;
                }
                #endregion

                #region DICOM files
                intListIndex = 0;
                objDCM = new Core.Case.HeaderDICOMList[(ArrDCM.Length / 3)];

                for (int i = 0; i < objDCM.Length; i++)
                {
                    strFile = ArrDCM[intListIndex + 2].Trim();
                    strFileName = Server.MapPath("~/CaseList/Temp/" + UserID.ToString() + "/") + ArrDCM[intListIndex + 2].Trim();
                    strFileName = strFileName.Replace("ajaxpro\\", "");
                    dd.DicomFileName = strFileName;
                    List<string> str = dd.dicomInfo;

                    if (GetStudyUID(str).Trim() != strSUID)
                    {
                        if (ModifyDCMFile(strDCMMODIFYEXEPATH, strInsName, strSUID, strFileName, ref strOutputMsg, ref strReturnMsg))
                        {
                            dd.DicomFileName = strFileName;
                            str = dd.dicomInfo;
                            if (GetStudyUID(str).Trim() != strSUID)
                            {
                                bFileValid = false;
                                strReturnMsg = strOutputMsg;
                            }
                            else
                                bFileValid = true;
                        }
                        else
                            bFileValid = false;
                    }

                    if (bFileValid)
                    {

                        objDCM[i] = new Core.Case.HeaderDICOMList();
                        objDCM[i].ID = new Guid(ArrDCM[intListIndex]);
                        objDCM[i].SERIAL_NUMBER = Convert.ToInt32(ArrDCM[intListIndex + 1]);
                        objDCM[i].FILE_NAME = ArrDCM[intListIndex + 2].Trim();
                        objDCM[i].FILE_CONTENT = GetFileBytes(strFileName);
                        intListIndex = intListIndex + 3;

                        if (File.Exists(strFileName + ".bak"))
                        {
                            File.Delete(strFileName + ".bak");
                        }
                    }
                    else
                    {

                        break;
                    }
                }
                #endregion

                #region Populate Merge Studies
                intListIndex = 0;
                objMerge = new Core.Case.StudyToMerge[(ArrMerge.Length / 4)];

                for (int i = 0; i < objMerge.Length; i++)
                {
                    objMerge[i] = new Core.Case.StudyToMerge();
                    objMerge[i].ID = new Guid(ArrMerge[intListIndex]);
                    objMerge[i].STUDY_UID = ArrMerge[intListIndex + 1].Trim();
                    objMerge[i].IMAGE_COUNT = Convert.ToInt32(ArrMerge[intListIndex + 2]);
                    objMerge[i].MERGE_STATUS = ArrMerge[intListIndex + 3].Trim();
                    intListIndex = intListIndex + 4;
                }
                #endregion

                intListIndex = 0;

                if (bFileValid)
                {
                    bReturn = objCore.SaveRecord(Server.MapPath("~"), objSTs, objDocs, objDCM,objMerge, ref strAccnNoGenerated, ref strReturnMsg, ref strCatchMessage);

                    #region Post Saving
                    if (bReturn)
                    {
                        arrRet = new string[3];
                        arrRet[0] = "true";
                        strReturnMsg = strReturnMsg.Trim();


                        if ((objCore.WRITE_BACK == "N") && (objCore.IMAGE_COUNT_ACCEPTED == "N"))
                        {
                            strReturnMsg = strReturnMsg + objComm.RecordDivider + "078";
                        }

                        //if (strAccnNoGenerated == "Y")
                        //{
                        //    strReturnMsg = strReturnMsg + objComm.RecordDivider + "163";
                        //    arrRet[2] = objCore.ACCESSION_NO;
                        //}
                        //else
                        //    arrRet[2] = string.Empty;

                        if (objCore.MERGE_STATUS == "M")
                        {
                            strReturnMsg = strReturnMsg + objComm.RecordDivider + "174";
                        }
                        else if (objCore.MERGE_STATUS == "C")
                        {
                            strReturnMsg = strReturnMsg + objComm.RecordDivider + "175";
                        }
                        else
                            arrRet[2] = string.Empty;


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
                            arrRet = new string[6];
                            arrRet[0] = "false";
                            arrRet[1] = strReturnMsg.Trim();
                            arrRet[2] = objCore.USER_NAME;
                            arrRet[3] = objCore.DELIVERY_TIME;
                            arrRet[4] = objCore.MESSAGE_DISPLAY;
                            arrRet[5] = objComm.CurrentTimeZone;
                        }
                    }
                    #endregion
                }
                else
                {
                    arrRet = new string[4];
                    arrRet[0] = "false";
                    arrRet[1] = "331";
                    arrRet[2] = strFile;
                    arrRet[3] = strReturnMsg;

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
                objCore = null; objComm = null; dd = null;
                strReturnMsg = null; strCatchMessage = null;
            }
            return arrRet;
        }
        #endregion

        #region DICOM File Methods

        #region GetStudyUID
        private string GetStudyUID(List<string> str)
        {

            string StudyUID = string.Empty;
            string s1, s4, s5, s11, s12;

            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                if ((s11.ToUpper() == "0020") && (s12.ToUpper() == "000D"))
                {
                    StudyUID = s5.Replace("\0", "");
                    break;
                }

            }
            return StudyUID;

        }
        #endregion

        #region GetallTags
        private string[] GetallTags(List<string> str)
        {

            string strDescription = string.Empty;
            string StudyUID = string.Empty;
            string ModalityID = string.Empty;
            string StrPName = string.Empty;
            string StudyDt = string.Empty;
            string StudyTime = string.Empty;
            string sDt = string.Empty;
            string sTime = string.Empty;
            string studyDtTime = string.Empty;
            string UserSeriesID = string.Empty;
            string SeriesNumber = string.Empty;
            string InstitutionName = string.Empty;
            string PatientID = string.Empty;
            string AccnNo = string.Empty;
            string RefPhys = string.Empty;
            string Manufacturer = string.Empty;
            string StationName = string.Empty;
            string Model = string.Empty;
            string ModalityAETitle = string.Empty;
            string Reason = string.Empty;
            string BirthDt = string.Empty;
            string bDt = string.Empty;
            string PatientSex = string.Empty;
            string PatientAge = string.Empty;
            //string PatientWt = string.Empty;//(0010,1030)
            //string Species = string.Empty;//(0010,2201)
            //string Breed = string.Empty;//(0010,2292)
            //string Owner = string.Empty;//(0010,2297)
            string PriorityID = string.Empty;


            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                string s1, s4, s5, s11, s12;
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                #region commented
                /*if ((s11.ToUpper() == "0008") && (s12.ToUpper() == "103E"))
                {
                    strDescription = s5.Replace("\0", "");
                    strDescription = s5.Replace("<", " ");
                    strDescription = s5.Replace(">", " ");

                }

                else if ((s11.ToUpper() == "0008") && (s12.ToUpper() == "0060"))
                {
                    ModalityID = s5.Replace("\0", "");

                }


                else if ((s11.ToUpper() == "0010") && (s12.ToUpper() == "0010"))
                {
                    Strname = s5.Replace("\0", "");
                    Strname = s5.Replace("^", " ");

                }
                else if ((s11.ToUpper() == "0010") && (s12.ToUpper() == "0030"))
                {
                    DOB = s5.Replace("\0", "");
                    DOB = DOB.Trim();
                    if (DOB != "")
                    {
                        string yy = DOB.Substring(0, 4);
                        string MM = DOB.Substring(4, 2);
                        string DD = DOB.Substring(6, 2);
                        result = yy + "-" + MM + "-" + DD;
                    }
                    else
                    {
                        result = "0000-00-00";
                    }
                }

                else */
                #endregion

                #region Tags
                s5 = s5.Replace("\t", "");
                s5 = s5.Replace("\n", "");

                switch (s11.ToUpper())
                {
                    case "0008":
                        #region s11 =0008
                        switch (s12.ToUpper())
                        {
                            case "0020":
                                StudyDt = s5.Replace("\0", "");
                                StudyDt = StudyDt.Trim();
                                if ((StudyDt.Length == 8))
                                {
                                    string yyyy = StudyDt.Substring(0, 4);
                                    string MM = StudyDt.Substring(4, 2);
                                    string DD = StudyDt.Substring(6, 2);
                                    sDt = yyyy + "-" + MM + "-" + DD;
                                }
                                else
                                {
                                    sDt = "0000-00-00";
                                }
                                break;
                            case "0030":
                                StudyTime = s5.Replace("\0", "");
                                StudyTime = StudyTime.Trim();
                                if ((StudyTime.Length == 6))
                                {
                                    string Hr = StudyTime.Substring(0, 2);
                                    string Min = StudyTime.Substring(2, 2);
                                    string Sec = StudyTime.Substring(4, 2);
                                    sTime = Hr + ":" + Min + ":" + Sec;
                                }
                                else
                                {
                                    sTime = "00:00:00";
                                }
                                break;
                            case "0050":
                                AccnNo = s5.Replace("\0", "");
                                break;
                            case "0060":
                                ModalityID = s5.Replace("\0", "");
                                break;
                            case "0070":
                                Manufacturer = s5.Replace("\0", "");
                                break;
                            case "0080":
                                if (InstitutionName.Trim() == string.Empty)
                                {
                                    InstitutionName = s5.Replace("\0", "");
                                    InstitutionName = s5.Replace("^", " ");
                                }
                                break;
                            case "0090":
                                RefPhys = s5.Replace("\0", "");
                                RefPhys = s5.Replace("^", " ");
                                break;
                            case "1010":
                                StationName = s5.Replace("\0", "");
                                break;
                            case "1090":
                                Model = s5.Replace("\0", "");
                                break;
                            default:
                                break;
                        }
                        #endregion
                        break;
                    case "0010":
                        #region s11 =0010
                        switch (s12.ToUpper())
                        {
                            case "0010":
                                StrPName = s5.Replace("\0", "");
                                StrPName = s5.Replace("^", " ");
                                break;
                            case "0020":
                                PatientID = s5.Replace("\0", "");
                                break;
                            case "0030":
                                BirthDt = s5.Replace("\0", "");
                                BirthDt = BirthDt.Trim();
                                if ((BirthDt.Length == 8))
                                {
                                    string yyyy = BirthDt.Substring(0, 4);
                                    string MM = BirthDt.Substring(4, 2);
                                    string DD = BirthDt.Substring(6, 2);
                                    bDt = yyyy + "-" + MM + "-" + DD;
                                }
                                else
                                {
                                    bDt = "0000-00-00";
                                }
                                break;
                            case "0040":
                                PatientSex = s5.Replace("\0", "");
                                break;
                            case "1010":
                                PatientAge = s5.Replace("\0", "");
                                break;
                        }
                        #endregion
                        break;
                    case "0020":
                        #region s11 =0020
                        switch (s12.ToUpper())
                        {
                            case "000D":
                                if (StudyUID.Trim() == string.Empty)
                                {
                                    StudyUID = s5.Replace("\0", "");
                                }
                                break;
                            case "000E":
                                UserSeriesID = s5.Replace("\0", "");
                                break;
                            case "0011":
                                SeriesNumber = s5.Replace("\0", "");
                                break;
                        }
                        #endregion
                        break;
                    case "0032":
                        #region s11 =0032
                        switch (s12.ToUpper())
                        {
                            case "000C":
                                PriorityID = s5.Replace("\0", "");
                                break;
                        }
                        #endregion
                        break;
                    case "0040":
                        #region s11 =0040
                        switch (s12.ToUpper())
                        {
                            case "0241":
                                ModalityAETitle = s5.Replace("\0", "");
                                break;
                            case "1002":
                                Reason = s5.Replace("\0", "");
                                break;

                        }
                        #endregion
                        break;
                    default:
                        break;
                }
                #endregion
            }

            studyDtTime = sDt + " " + sTime;

            string[] arr = new string[17];
            arr[0] = StudyUID;
            arr[1] = ModalityID;
            arr[2] = PatientID;
            arr[3] = StrPName;
            arr[4] = studyDtTime;
            arr[5] = InstitutionName;
            arr[6] = AccnNo;
            arr[7] = RefPhys;
            arr[8] = Manufacturer;
            arr[9] = StationName;
            arr[10] = Model;
            arr[11] = ModalityAETitle;
            arr[12] = Reason;
            arr[13] = bDt;
            arr[14] = PatientSex;
            arr[15] = PatientAge;
            arr[16] = PriorityID;

            return arr;

        }
        #endregion

        #region ExtractStrings
        void ExtractStrings(string s1, out string s4, out string s5, out string s11, out string s12)
        {
            int ind;
            string s2, s3;
            ind = s1.IndexOf("//");
            s2 = s1.Substring(0, ind);
            s11 = s1.Substring(0, 4);
            s12 = s1.Substring(4, 4);
            s3 = s1.Substring(ind + 2);
            ind = s3.IndexOf(":");
            s4 = s3.Substring(0, ind);
            s5 = s3.Substring(ind + 1);
        }
        #endregion

        #region ModifyDCMFile
        private bool ModifyDCMFile(string strDCMMODIFYEXEPATH, string strInsName, string strSUID, string strDCMPath, ref string strOutputMsg, ref string strReturnMessage)
        {
            bool bRet = false;
            string strProcOutput = string.Empty;
            string strProcError = string.Empty;

            try
            {
                Process ProcModSUID = new Process();
                ProcModSUID.StartInfo.UseShellExecute = false;
                ProcModSUID.StartInfo.FileName = strDCMMODIFYEXEPATH;
                ProcModSUID.StartInfo.Arguments = "-i \"(0020,000D)=" + strSUID + "\"" + " " + strDCMPath;
                ProcModSUID.StartInfo.RedirectStandardOutput = true;
                ProcModSUID.StartInfo.RedirectStandardError = true;
                ProcModSUID.Start();
                strProcOutput = ProcModSUID.StandardOutput.ReadToEnd();
                strProcError = ProcModSUID.StandardError.ReadToEnd();
                strOutputMsg = strProcOutput.Trim();
                bRet = true;


            }
            catch (Exception ex)
            {
                strReturnMessage = ex.Message.Trim();
            }


            if (bRet)
            {
                try
                {
                    Process ProcModInst = new Process();
                    ProcModInst.StartInfo.UseShellExecute = false;
                    ProcModInst.StartInfo.FileName = strDCMMODIFYEXEPATH;
                    ProcModInst.StartInfo.Arguments = "-i \"(0008,0080)=" + strInsName + "\"" + " " + strDCMPath;
                    ProcModInst.StartInfo.RedirectStandardOutput = true;
                    ProcModInst.StartInfo.RedirectStandardError = true;
                    ProcModInst.Start();
                    strProcOutput = ProcModInst.StandardOutput.ReadToEnd();
                    strProcError = ProcModInst.StandardError.ReadToEnd();
                    strOutputMsg = strProcOutput.Trim();
                    bRet = true;


                }
                catch (Exception ex)
                {
                    strReturnMessage = ex.Message.Trim();
                }

            }

            return bRet;
        }
        #endregion

        #endregion

        #region Delete Study

        #region API 7.2

        #region DeleteStudy_72 (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] DeleteStudy_72(string[] ArrRecord, string strURL)
        {
            bool bReturn = false;
            WebClient client = new WebClient();
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            int intCompanyID = 0; string strFileName = string.Empty; Guid UserID = Guid.Empty;
            string strResult = string.Empty; string strCount = string.Empty; string strRecByRouter = string.Empty;
            string[] err = new string[0];
            objComm = new classes.CommonClass();
            objCore = new Core.Case.CaseStudy();


            try
            {
                strRecByRouter = ArrRecord[2].Trim();


                if (strRecByRouter == "N")
                {
                    #region if DICOM ROUTER != MANUAL
                    IgnoreBadCertificates();
                    byte[] data = client.DownloadData(strURL);
                    strResult = System.Text.Encoding.Default.GetString(data);
                    strResult = strResult.Replace("### Begin_Table's_Content ###", "");
                    strResult = strResult.Replace("### End_Table's_Content ###", "");

                    if (strResult.IndexOf("#ERROR:") <= 0)
                    {
                        objCore.ID = new Guid(ArrRecord[0]);
                        objCore.STUDY_UID = ArrRecord[1].Trim();
                        objCore.USER_ID = UserID = new Guid(ArrRecord[3]);
                        objCore.MENU_ID = Convert.ToInt32(ArrRecord[4]);

                        bReturn = objCore.DeleteStudy(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                        if (bReturn)
                        {
                            arrRet = new string[2];
                            arrRet[0] = "true";

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
                    else
                    {

                        err = strResult.Split('#');
                        arrRet = new string[2];
                        arrRet[0] = "false";

                        if (err.Length == 4)
                        {
                            arrRet[1] = err[3].Replace("ERROR: ", "");
                        }
                        else if (err.Length == 3)
                        {
                            if (err[2].Split(':')[1].Trim() == "OK")
                                arrRet[1] = "Study is already deleted from PACS";
                        }
                    }
                    #endregion
                }
                else if ((strRecByRouter == "Y") || (strRecByRouter == "M"))
                {
                    #region if DICOM ROUTER == MANUAL
                    objCore.ID = new Guid(ArrRecord[0]);
                    objCore.STUDY_UID = ArrRecord[1].Trim();
                    objCore.USER_ID = UserID = new Guid(ArrRecord[3]);
                    objCore.MENU_ID = Convert.ToInt32(ArrRecord[4]);

                    bReturn = objCore.DeleteStudy(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                    if (bReturn)
                    {
                        arrRet = new string[2];
                        arrRet[0] = "true";

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
                    #endregion
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

        #endregion

        #region WS8
        #region DeleteStudy_80 (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] DeleteStudy_80(string[] ArrRecord, string[] ArrWSParams)
        {
            bool bReturn = false;
            RadWebClass client = new RadWebClass();
            string strResult = string.Empty;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty; string strError = string.Empty;
            Guid UserID = Guid.Empty;
            string strRecByRouter = string.Empty;
            string strStudyUID = string.Empty;
            string strWSURL = string.Empty;
            string strClientIP = string.Empty;
            string strWSUserID = string.Empty;
            string strWSPwd = string.Empty;
            string strSession = string.Empty;
            objComm = new classes.CommonClass();
            objCore = new Core.Case.CaseStudy();


            try
            {
                strStudyUID = ArrRecord[1].Trim();
                strRecByRouter = ArrRecord[2].Trim();

                strWSURL = ArrWSParams[0].Trim();
                strClientIP = ArrWSParams[1].Trim();
                strWSUserID = ArrWSParams[2].Trim();
                strWSPwd = CoreCommon.DecryptString(ArrWSParams[3].Trim());

                if (strRecByRouter == "N")
                {
                    #region  if DICOM ROUTER != MANUAL

                    bReturn = client.DeleteStudyData(strSession, strWSURL, strStudyUID, ref strCatchMessage, ref strError);

                    if (bReturn)
                    {
                        objCore.ID = new Guid(ArrRecord[0]);
                        objCore.STUDY_UID = ArrRecord[1].Trim();
                        objCore.USER_ID = UserID = new Guid(ArrRecord[3]);
                        objCore.MENU_ID = Convert.ToInt32(ArrRecord[4]);

                        bReturn = objCore.DeleteStudy(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                        if (bReturn)
                        {
                            
                            arrRet = new string[2];
                            arrRet[0] = "true";

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
                    else
                    {
                        if (strCatchMessage.Trim() != string.Empty)
                        {
                            arrRet = new string[2];
                            arrRet[0] = "catch";
                            arrRet[1] = strCatchMessage.Trim();
                        }
                        else
                        {
                            if (strError.ToUpper().Trim().Contains("NO MATCHING STUDY WAS FOUND"))
                            {
                                objCore.ID = new Guid(ArrRecord[0]);
                                objCore.STUDY_UID = ArrRecord[1].Trim();
                                objCore.USER_ID = UserID = new Guid(ArrRecord[3]);
                                objCore.MENU_ID = Convert.ToInt32(ArrRecord[4]);

                                bReturn = objCore.DeleteStudy(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                                if (bReturn)
                                {
                                    arrRet = new string[2];
                                    arrRet[0] = "true";

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
                            else
                            {
                                arrRet = new string[2];
                                arrRet[0] = "false";
                                arrRet[1] = strError.Trim();
                            }

                        }

                    }

                    #endregion
                }
                else if ((strRecByRouter == "Y") || (strRecByRouter == "M"))
                {
                    #region if DICOM ROUTER == MANUAL
                    objCore.ID = new Guid(ArrRecord[0]);
                    objCore.STUDY_UID = ArrRecord[1].Trim();
                    objCore.USER_ID = UserID = new Guid(ArrRecord[3]);
                    objCore.MENU_ID = Convert.ToInt32(ArrRecord[4]);

                    bReturn = objCore.DeleteStudy(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                    if (bReturn)
                    {
                        arrRet = new string[2];
                        arrRet[0] = "true";

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
                    #endregion
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
        #endregion

        #endregion

        #region IsInteger
        protected bool IsInteger(String integerValue)
        {
            Decimal Temp;
            if (Decimal.TryParse(integerValue, out Temp) == true)
                return true;
            else
                return false;
        }
        #endregion
    }
}