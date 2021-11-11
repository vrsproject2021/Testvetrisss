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
using ClearCanvas.Dicom;
using AjaxPro;

namespace VETRIS.Study
{
    [AjaxPro.AjaxNamespace("VRSProcImageDlg")]
    public partial class VRSProcImageDlg : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.Study.ProcessImage objCore;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSProcImageDlg));
            SetAttributes();
            if ((!CallBackFiles.CausedCallback) && (!CallBackST.CausedCallback) && (!CallBackSelST.CausedCallback) && (!CallBackDoc.CausedCallback))
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);

            btnSave1.Attributes.Add("onclick", "javascript:btnSubmit_OnClick('N');");
            btnSave2.Attributes.Add("onclick", "javascript:btnSubmit_OnClick('N');");
            btnSubmit1.Attributes.Add("onclick", "javascript:btnSubmit_OnClick('Y','X');");
            btnSubmit2.Attributes.Add("onclick", "javascript:btnSubmit_OnClick('Y','X');");
            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");
            btnFilter.Attributes.Add("onclick", "javascript:btnFilter_OnClick();");

            txtDOS.Attributes.Add("onblur", "javascript:txtDOS_OnBlur();");
            imgDOS.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtDOS.ClientID + "','CalDOS');");

            ddlModality.Attributes.Add("onchange", "javascript:LoadStudyTypes();");
            //ddlCategory.Attributes.Add("onchange", "javascript:LoadStudyTypes();");
            ddlInstitution.Attributes.Add("onchange", "javascript:ddlInstitution_OnChange();");
            ddlSpecies.Attributes.Add("onchange", "javascript:ddlSpecies_OnChange();");

            //txtFromDt.Attributes.Add("onblur", "javascript:txtFromDt_OnBlur();");
            //imgFrom.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtFromDt.ClientID + "','CalFrom');");
            ddlCountry.Attributes.Add("onchange", "javascript:ddlCountry_OnChange();");
            ddlDOBMonth.Attributes.Add("onchange", "javascript:ddlDOBMonth_OnChange();");
            ddlDOBDay.Attributes.Add("onchange", "javascript:ddlDOBDay_OnChange();");
            ddlDOBYear.Attributes.Add("onchange", "javascript:ddlDOBYear_OnChange();");

            txtPWt.Attributes.Add("onkeypress", "javascript:return parent.CheckDecimal(event);");
            txtPWt.Attributes.Add("onblur", "javascript:ResetValueDecimal(this,3);");
            txtPWt.Attributes.Add("onfocus", "javascript:this.select();");
            rdoConsY.Attributes.Add("onclick", "javascript:Consult_OnClick();");
            rdoConsN.Attributes.Add("onclick", "javascript:Consult_OnClick();");
            ddlPriority.Attributes.Add("onchange", "javascript:ddlPriority_OnChange();");

            //lblViewLog.Attributes.Add("onclick", "javascript:ViewLogUI('" + intMenuID.ToString() + "');");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            //hdnID.Value = Request.QueryString["id"];
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            Guid SessionID = new Guid(Request.QueryString["sid"]);
            string strTheme = Request.QueryString["th"];
            DateTime dtDOB = DateTime.Today.AddDays(-1);
            txtImgCnt.Text = "0";
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            PopulateTimeDropDowns();
            txtDOS.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);// +" " + DateTime.Now.ToString("HH:mm");
            ddlHr.SelectedValue =  objComm.padZero(DateTime.Now.Hour);
            ddlMin.SelectedValue =  objComm.padZero(DateTime.Now.Minute);
            //txtFromDt.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            txtPWt.Text = objComm.IMNumeric(0, 3);
            objComm = null;
            PopulateDOBDays(DateTime.Today);
            PopulateDOBYears();
            ddlDOBMonth.SelectedValue = dtDOB.Month.ToString();
            ddlDOBYear.SelectedValue = dtDOB.Year.ToString();
            ddlDOBDay.SelectedValue = dtDOB.Day.ToString();
            txtAge.Text = "0 year 0 month 0 days";
            hdnFilePath.Value = Server.MapPath("~");
            LoadHeader(intMenuID, UserID, SessionID);
            
            SetCSS(strTheme);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css?v=" + DateTime.Now.Ticks.ToString();
            lnkCAL.Attributes["href"] = strServerPath + "/css/" + strTheme + "/CalendarStyle.css?v=" + DateTime.Now.Ticks.ToString();
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css?v=" + DateTime.Now.Ticks.ToString();
        }
        #endregion

        #region PopulateTimeDropDowns
        private void PopulateTimeDropDowns()
        {
            for (int i = 0; i <= 23; i++)
            {
                ListItem item = new ListItem();
                item.Value = objComm.padZero(i);
                item.Text = objComm.padZero(i);
                ddlHr.Items.Add(item);
            }
            for (int i = 0; i <= 59; i++)
            {
                ListItem item = new ListItem();
                item.Value = objComm.padZero(i);
                item.Text = objComm.padZero(i);
                ddlMin.Items.Add(item);
            }
        }
        #endregion

        #region CreateUserDirectory
        private void CreateUserDirectory(Guid UserID)
        {
            if (!Directory.Exists(Server.MapPath("~/Study/Temp/" + UserID.ToString())))
            {
                Directory.CreateDirectory(Server.MapPath("~/Study/Temp/" + UserID.ToString()));
            }
        }
        #endregion

        #region LoadHeader
        private void LoadHeader(int intMenuID, Guid UserID, Guid SessionID)
        {
            objCore = new Core.Study.ProcessImage();
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
                    PopulateDropDowns(ds);
                    lblSUID.Text = objCore.STUDY_UID; hdnSUID.Value = objCore.STUDY_UID;
                    
                    txtPID.Text = objCore.PATIENT_ID;
                    txtPFName.Text = objCore.PATIENT_FIRST_NAME;
                    txtPLName.Text = objCore.PATIENT_LAST_NAME;

                    if (objCore.STUDY_DATE.Year > 1900) txtDOS.Text = objComm.IMDateFormat(objCore.STUDY_DATE, objComm.DateFormat);
                    else txtDOS.Text = string.Empty;

                    txtInstName.Text = objCore.INSTITUTION_NAME.Trim();
                    hdnInstID.Value = objCore.INSTITUTION_ID.ToString();
                    if (objCore.INSTITUTION_CODE.Trim() != string.Empty)
                    {
                        txtPID.Text = objCore.INSTITUTION_CODE.Trim() + "-" + Convert.ToString(objCore.PATIENT_ID_LAST_SERIAL + 1);
                    }
                    if (objCore.CONSULT_APPLIED == "Y") rdoConsY.Checked = true; else rdoConsN.Checked = true;
                    ddlModality.SelectedValue = objCore.MODALITY_ID.ToString();
                    if(objCore.CATEGORY_ID>0) ddlCategory.SelectedValue = objCore.CATEGORY_ID.ToString();
                    txtImgCnt.Text = objCore.FILE_COUNT.ToString();
                    hdnSeriesUID.Value = objCore.SERIES_UID;
                    hdnSeriesNo.Value = objCore.SERIES_NUMBER;
                    hdnFTPDLFLDRTMP.Value = objCore.FTD_DOWNLOAD_PATH;
                    hdnAppv.Value = objCore.APPROVED;
                    hdnInstConsAppl.Value = objCore.INSTITUTION_CONSULTATION_APPLICABLE;
                    hdnAfterHrs.Value = objCore.BEYOND_OPERATION_HOUR;
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

        #region PopulateDropDowns
        private void PopulateDropDowns(DataSet ds)
        {
            #region Filter Institution
            DataRow dr1 = ds.Tables["Institutions"].NewRow();
            dr1["id"] = "00000000-0000-0000-0000-000000000000";
            dr1["name"] = "Filter by institution";
            ds.Tables["Institutions"].Rows.InsertAt(dr1, 0);
            ds.Tables["Institutions"].AcceptChanges();

            ddlFiltInst.DataSource = ds.Tables["Institutions"];
            ddlFiltInst.DataValueField = "id";
            ddlFiltInst.DataTextField = "name";
            ddlFiltInst.DataBind();
            if (ddlFiltInst.Items.Count == 2) ddlFiltInst.SelectedIndex = 1;

            ds.Tables["Institutions"].Rows[0]["name"] = "Select One";
            ds.Tables["Institutions"].AcceptChanges();
            ddlInstitution.DataSource = ds.Tables["Institutions"];
            ddlInstitution.DataValueField = "id";
            ddlInstitution.DataTextField = "name";
            ddlInstitution.DataBind();
            if (ddlInstitution.Items.Count == 2)
            {
                ddlInstitution.SelectedIndex = 1;
                hdnInstID.Value = ddlInstitution.Items[ddlInstitution.SelectedIndex].Value.ToString();
            }
            
            #endregion

            #region Modality
            DataRow dr2 = ds.Tables["Modality"].NewRow();
            dr2["id"] = "0";
            dr2["name"] = "Select One";
            ds.Tables["Modality"].Rows.InsertAt(dr2, 0);
            ds.Tables["Modality"].AcceptChanges();

            ddlModality.DataSource = ds.Tables["Modality"];
            ddlModality.DataValueField = "id";
            ddlModality.DataTextField = "name";
            ddlModality.DataBind();
            #endregion

            #region Species
            DataRow dr3 = ds.Tables["Species"].NewRow();
            dr3["id"] = 0;
            dr3["name"] = "Select One";
            ds.Tables["Species"].Rows.InsertAt(dr3, 0);
            ds.Tables["Species"].AcceptChanges();

            ddlSpecies.DataSource = ds.Tables["Species"];
            ddlSpecies.DataValueField = "id";
            ddlSpecies.DataTextField = "name";
            ddlSpecies.DataBind();
            #endregion

            #region Physicians
            DataRow dr4 = ds.Tables["Physicians"].NewRow();
            dr4["id"] = "00000000-0000-0000-0000-000000000000";
            dr4["name"] = "Select One";
            ds.Tables["Physicians"].Rows.InsertAt(dr4, 0);
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

            DataRow dr5 = ds.Tables["Priority"].NewRow();
            dr5["priority_id"] = "0";
            dr5["priority_desc"] = "Select One";
            ds.Tables["Priority"].Rows.InsertAt(dr5, 0);
            ds.Tables["Priority"].AcceptChanges();

            ddlPriority.DataSource = ds.Tables["Priority"];
            ddlPriority.DataValueField = "priority_id";
            ddlPriority.DataTextField = "priority_desc";
            ddlPriority.DataBind();
            #endregion

            #region Category
            ddlCategory.DataSource = ds.Tables["Category"];
            ddlCategory.DataValueField = "id";
            ddlCategory.DataTextField = "name";
            ddlCategory.DataBind();

            DataView dv = new DataView(ds.Tables["Category"]);
            dv.RowFilter = "is_default='Y'";
            if (dv.ToTable().Rows.Count > 0) ddlCategory.SelectedValue = Convert.ToString(dv.ToTable().Rows[0]["id"]);
            dv.Dispose();
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

        #region File Grid

        #region CallBackFiles_Callback
        protected void CallBackFiles_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadFiles(e.Parameters);
            grdFiles.Width = Unit.Percentage(98);
            grdFiles.RenderControl(e.Output);
            spnErrFiles.RenderControl(e.Output);
        }
        #endregion

        #region LoadFiles
        private void LoadFiles(string[] arrParams)
        {
            objCore = new Core.Study.ProcessImage();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";
            string strFTPDLFLDRTMP=string.Empty;
            string strUserID = string.Empty;
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.ID = new Guid(arrParams[0]);
                objCore.INSTITUTION_ID = new Guid(arrParams[1]);
                strFTPDLFLDRTMP = arrParams[2].Trim();
                strUserID = arrParams[3].Trim();
                objCore.USER_ID = new Guid(strUserID);

                bReturn = objCore.LoadFiles(Server.MapPath("~"), ref ds, ref strCatchMessage);
               
                if (bReturn)
                {
                    grdFiles.DataSource = ds.Tables["Files"];
                    grdFiles.Levels[0].Columns[3].FormatString = objComm.DateFormat + " HH:mm";
                    grdFiles.DataBind();

                    foreach (DataRow dr in ds.Tables["Files"].Rows)
                    {

                        strFileName = Convert.ToString(dr["file_name"]).Trim();
                        if (File.Exists(strFTPDLFLDRTMP + "/" + strFileName))
                        {
                            if (File.Exists(Server.MapPath("~") + "/Study/Temp/" + strUserID + "/" + strFileName)) File.Delete(Server.MapPath("~") + "/Study/Temp/" + strUserID + "/" + strFileName);
                            File.Copy(Path.Combine(strFTPDLFLDRTMP, strFileName), Path.Combine(Server.MapPath("~") + "/Study/Temp/" + strUserID, strFileName), true);
                        }

                    }


                    spnErrFiles.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrFiles\" value=\"\" />";
                }
                else
                    spnErrFiles.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrFiles\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnErrFiles.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrFiles\" value=\"" + ex.Message.Trim() + "\" />";
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

        #region Study Type Grid

        #region CallBackST_Callback
        protected void CallBackST_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadStudyTypes(e.Parameters);
            grdST.Width = Unit.Percentage(99);
            grdST.RenderControl(e.Output);
            spnErrST.RenderControl(e.Output);
            spnInvBy.RenderControl(e.Output);
        }
        #endregion

        #region LoadStudyTypes
        private void LoadStudyTypes(string[] arrParams)
        {
            objCore = new Core.Study.ProcessImage();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";

            try
            {

                objCore.ID = new Guid(arrParams[0]);
                objCore.MODALITY_ID = Convert.ToInt32(arrParams[1]);
                objCore.INSTITUTION_ID =new Guid(arrParams[2]);

                bReturn = objCore.LoadStudyTypes(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdST.DataSource = ds.Tables["StudyTypes"];
                    grdST.DataBind();
                    spnInvBy.InnerHtml = "<input type=\"hidden\" id=\"hdnModInvBy\" value=\"\" />";

                    foreach (DataRow dr in ds.Tables["TrackBy"].Rows)
                    {
                        spnInvBy.InnerHtml = "<input type=\"hidden\" id=\"hdnModInvBy\" value=\"" + Convert.ToString(dr["invoice_by"]) + "\" />";
                    }

                    spnErrST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrST\" value=\"\" />";
                }
                else
                {
                    spnInvBy.InnerHtml = "<input type=\"hidden\" id=\"hdnModInvBy\" value=\"\" />";
                    spnErrST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrST\" value=\"" + strCatchMessage + "\" />";
                }



            }
            catch (Exception ex)
            {
                spnErrST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrST\" value=\"" + ex.Message.Trim() + "\" />";
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
                //case "L":
                //    LoadSelectedStudyTypes(e.Parameters);
                //    break;
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
        //private void LoadSelectedStudyTypes(string[] arrParams)
        //{
        //    objCore = new Core.Case.CaseStudy();
        //    string strCatchMessage = ""; bool bReturn = false;
        //    DataSet ds = new DataSet();
        //    Guid UserID = Guid.Empty;
        //    string strFileName = "";

        //    try
        //    {

        //        objCore.ID = new Guid(arrParams[1]);
        //        objCore.MODALITY_ID = Convert.ToInt32(arrParams[2]);

        //        bReturn = objCore.LoadSelectedStudyTypes(Server.MapPath("~"), ref ds, ref strCatchMessage);
        //        if (bReturn)
        //        {
        //            grdSelST.DataSource = ds.Tables["SelStudyTypes"];
        //            grdSelST.DataBind();


        //            spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelST\" value=\"\" />";
        //        }
        //        else
        //            spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelST\" value=\"" + strCatchMessage + "\" />";



        //    }
        //    catch (Exception ex)
        //    {
        //        spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelST\" value=\"" + ex.Message.Trim() + "\" />";
        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //        objCore = null;
        //    }
        //}
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
                spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelST\" value=\"\" />";
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

        #region Document Grid

        #region CallBackDoc_Callback
        protected void CallBackDoc_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];

            switch (strAction)
            {
                //case "L":
                //    LoadUploadedDocs(e.Parameters);
                //    break;
                case "A":
                    AddDocument(e.Parameters);
                    break;
                case "D":
                    DeleteDocument(e.Parameters);
                    break;
            }
            grdDoc.Width = Unit.Percentage(100);
            grdDoc.RenderControl(e.Output);
            spnERRDoc.RenderControl(e.Output);
        }
        #endregion

        #region LoadUploadedDocs
        //private void LoadUploadedDocs(string[] arrParams)
        //{
        //    objCore = new Core.Study.ProcessImage()
        //    string strCatchMessage = ""; bool bReturn = false;
        //    DataSet ds = new DataSet();
        //    Guid UserID = Guid.Empty;
        //    string strFileName = "";

        //    try
        //    {

        //        objCore.ID = new Guid(arrParams[1]);
        //        UserID = new Guid(arrParams[2]);

        //        bReturn = objCore.LoadHeaderDocuments(Server.MapPath("~"), ref ds, ref strCatchMessage);
        //        if (bReturn)
        //        {
        //            grdDoc.DataSource = ds.Tables["Documents"];
        //            grdDoc.DataBind();

        //            foreach (DataRow dr in ds.Tables["Documents"].Rows)
        //            {

        //                strFileName = Convert.ToString(dr["document_link"]);
        //                SetFile((byte[])dr["document_file"], Convert.ToString(dr["document_link"]).Trim(), "CaseList/Temp/" + UserID.ToString());

        //            }

        //            spnERRDoc.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDoc\" value=\"\" />";
        //        }
        //        else
        //            spnERRDoc.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDoc\" value=\"" + strCatchMessage + "\" />";



        //    }
        //    catch (Exception ex)
        //    {
        //        spnERRDoc.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDoc\" value=\"" + ex.Message.Trim() + "\" />";
        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //        objCore = null;
        //    }
        //}
        #endregion

        #region AddDocument
        private void AddDocument(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);
            string[] arrNew = arrParams[2].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;

            try
            {
                dtbl = CreateDocTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 5)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["document_srl_no"] = intSrl;
                            dr["document_id"] = arrRecords[i + 1].Trim();
                            dr["document_name"] = arrRecords[i + 2].Trim();
                            dr["document_link"] = arrRecords[i + 3].Trim();
                            dr["document_file_type"] = arrRecords[i + 4].Trim();
                            dr["del_doc"] = "";
                            dtbl.Rows.Add(dr);
                        }

                    }
                }

                DataRow drNew = dtbl.NewRow();
                intSrl = intSrl + 1;
                drNew["document_srl_no"] = intSrl;
                drNew["document_id"] = arrNew[1].Trim();
                drNew["document_name"] = arrNew[2].Trim();
                drNew["document_link"] = arrNew[3].Trim();
                drNew["document_file_type"] = arrNew[4].Trim();
                drNew["del_doc"] = "";
                dtbl.Rows.Add(drNew);

                grdDoc.DataSource = dtbl;
                grdDoc.DataBind();
                spnERRDoc.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDoc\" value=\"\" />";
            }
            catch (Exception ex)
            {
                spnERRDoc.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDoc\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region DeleteDocument
        private void DeleteDocument(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            int intID = Convert.ToInt32(arrParams[1]);
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;

            try
            {
                dtbl = CreateDocTable();
                if (arrRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrRecords.Length; i = i + 5)
                    {
                        if (Convert.ToInt32(arrRecords[i]) != intID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["document_srl_no"] = intSrl;
                            dr["document_id"] = arrRecords[i + 1].Trim();
                            dr["document_name"] = arrRecords[i + 2].Trim();
                            dr["document_link"] = arrRecords[i + 3].Trim();
                            dr["document_file_type"] = arrRecords[i + 4].Trim();
                            dr["del_doc"] = "";
                            dtbl.Rows.Add(dr);
                        }
                    }
                }

                grdDoc.DataSource = dtbl;
                grdDoc.DataBind();
                spnERRDoc.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDoc\" value=\"\" />";

            }
            catch (Exception ex)
            {
                spnERRDoc.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDoc\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region CreateDocTable
        private DataTable CreateDocTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("document_srl_no", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("document_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("document_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("document_link", System.Type.GetType("System.String"));
            dtbl.Columns.Add("document_file_type", System.Type.GetType("System.String"));
            dtbl.Columns.Add("del_doc", System.Type.GetType("System.String"));
            dtbl.TableName = "Documents";
            return dtbl;
        }
        #endregion

        #region SetFile
        private void SetFile(byte[] DocData, string strFileName, string strPath)
        {
            string strFilePath = Server.MapPath("~") + "/" + strPath + "/" + strFileName;
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

        #region FetchBreeds (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchBreeds(string[] arrParams)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objCore = new Core.Study.ProcessImage();

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
            objCore = new Core.Study.ProcessImage();

            int i = 0;
            string[] arrRet = new string[0];

            try
            {

                objCore.INSTITUTION_ID = new Guid(arrParams[0].Trim());
                bReturn = objCore.FetchInstitutionWisePhysician(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[(ds.Tables["Physicians"].Rows.Count * 2) + 6];
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
                        arrRet[2] = "Select One";
                        i = 3;
                    }
                    if (ds.Tables["Consult"].Rows.Count > 0)
                    {
                        arrRet[i] = Convert.ToString(ds.Tables["Consult"].Rows[0]["consult_applicable"]);
                        arrRet[i + 1] = Convert.ToString(ds.Tables["Consult"].Rows[0]["code"]);
                        arrRet[i + 2] = Convert.ToString(Convert.ToInt32(ds.Tables["Consult"].Rows[0]["patient_id_srl"]) + 1);
                    }
                    else
                    {
                        arrRet[i] = "N";
                        arrRet[i + 1] = "";
                        arrRet[i + 2] = "";
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

        #region DeleteImageFile (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] DeleteImageFile(string[] ArrRecord)
        {
            bool bReturn = false;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            objComm = new classes.CommonClass();
            objCore = new Core.Study.ProcessImage();
            string strFileName = string.Empty; Guid UserID = Guid.Empty;
            string strFolder = string.Empty;

            try
            {
                objCore.ID = new Guid(ArrRecord[0]);
                objCore.FTD_DOWNLOAD_PATH = ArrRecord[1].Trim();
                objCore.FILE_NAME = ArrRecord[2].Trim();
                objCore.USER_ID = new Guid(ArrRecord[3]); 
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[4]);

                bReturn = objCore.DeleteFile(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

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

        #region GetServiceAvailabilityMessage (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] GetServiceAvailabilityMessage(string[] arrParams)
        {
            string strReturn = string.Empty;
            string strReturnMessage = ""; string strCatchMessage = ""; bool bReturn = false;
            objCore = new Core.Study.ProcessImage();

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

        #region Save Study
        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord, string[] ArrFiles, string[] ArrSTs, string[] ArrDocs)
        {
            bool bReturn = false;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            objComm = new classes.CommonClass();
            objCore = new Core.Study.ProcessImage();
            Core.Study.FileList[] objList = new Core.Study.FileList[0];
            int intListIndex = 0;
            Core.Study.StudyTypeList[] objSTs = new Core.Study.StudyTypeList[0];
            Core.Study.DocumentList[] objDocs = new Core.Study.DocumentList[0];
            string strFileName = string.Empty; Guid UserID = Guid.Empty;

            try
            {
                objComm.SetRegionalFormat();
                objCore.ID = new Guid(ArrRecord[0]);
                if (ArrRecord[1].Trim() == string.Empty) objCore.STUDY_UID = GenrateStudyUID(); else objCore.STUDY_UID = ArrRecord[1].Trim();
                objCore.FILE_COUNT = Convert.ToInt32(ArrRecord[2]);
                objCore.INSTITUTION_ID = new Guid(ArrRecord[3]);
                objCore.STUDY_DATE = Convert.ToDateTime(ArrRecord[4]);
                objCore.MODALITY_ID = Convert.ToInt32(ArrRecord[5]);
                objCore.PATIENT_ID = ArrRecord[6].Trim();
                objCore.PATIENT_FIRST_NAME = ArrRecord[7].Trim();
                objCore.PATIENT_LAST_NAME = ArrRecord[8].Trim();
                objCore.PATIENT_NAME = ArrRecord[7].Trim() + " " + ArrRecord[8].Trim();
                if (ArrRecord[9].Trim() == string.Empty) objCore.SERIES_UID = GenrateSeriesUID(); else objCore.SERIES_UID = ArrRecord[9].Trim();
                if (ArrRecord[10].Trim() == string.Empty) objCore.SERIES_NUMBER = CreateSeriesNumber(); else objCore.SERIES_NUMBER = ArrRecord[10].Trim();

                objCore.PATIENT_WEIGHT = Convert.ToDecimal(ArrRecord[11]);
                objCore.WEIGHT_UOM = ArrRecord[12].Trim();
                objCore.PATIENT_DOB = Convert.ToDateTime(ArrRecord[13]);
                objCore.PATIENT_AGE = ArrRecord[14].Trim();
                objCore.PATIENT_GENDER = ArrRecord[15].Trim();
                objCore.SPAYED_NEUTERED = ArrRecord[16].Trim();
                objCore.SPECIES_ID = Convert.ToInt32(ArrRecord[17]);
                objCore.BREED_ID = new Guid(ArrRecord[18]);
                objCore.OWNER_FIRST_NAME = ArrRecord[19].Trim();
                objCore.OWNER_LAST_NAME = ArrRecord[20].Trim();
                objCore.ACCESSION_NO = ArrRecord[21].Trim();
                objCore.REASON = ArrRecord[22].Trim();
                objCore.PHYSICIAN_ID = new Guid(ArrRecord[23]);
                objCore.PRIORITY_ID = Convert.ToInt32(ArrRecord[24]);
                objCore.APPROVED = ArrRecord[25].Trim();
                objCore.MERGE_STATUS = ArrRecord[26].Trim();
                objCore.PHYSICIAN_NOTE = ArrRecord[27].Trim();
                objCore.CONSULT_APPLIED = ArrRecord[28].Trim();
                objCore.CATEGORY_ID = Convert.ToInt32(ArrRecord[29]);
                objCore.SUBMIT_PRIORITY = ArrRecord[30].Trim();
                objCore.SENDER_TIME_ZONE_OFFSET_MINUTES = Convert.ToInt32(ArrRecord[31]);
                objCore.PATIENT_COUNTRY_ID = Convert.ToInt32(ArrRecord[32]);
                objCore.PATIENT_STATE_ID = Convert.ToInt32(ArrRecord[33]);
                objCore.PATIENT_CITY = ArrRecord[34].Trim();
                objCore.CHECK_DOB = ArrRecord[35].Trim();
                objCore.USER_ID = new Guid(ArrRecord[36]); UserID = new Guid(ArrRecord[36]);
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[37]);
                objCore.USER_SESSION_ID = new Guid(ArrRecord[38]);

                objList = new Core.Study.FileList[(ArrFiles.Length / 2)];

                #region populate file details
                for (int i = 0; i < objList.Length; i++)
                {
                    objList[i] = new Core.Study.FileList();
                    objList[i].UNGROUP_ID = new Guid(ArrFiles[intListIndex]);
                    objList[i].NAME = ArrFiles[intListIndex + 1].Trim();
                    intListIndex = intListIndex + 2;
                }
                #endregion

                #region Populate study types
                intListIndex = 0;
                objSTs = new Core.Study.StudyTypeList[(ArrSTs.Length)];

                for (int i = 0; i < objSTs.Length; i++)
                {
                    objSTs[i] = new Core.Study.StudyTypeList();
                    objSTs[i].SERIAL_NUMBER = i + 1;
                    objSTs[i].ID = new Guid(ArrSTs[intListIndex]);
                    intListIndex = intListIndex + 1;
                }
                #endregion

                #region Populate documents
                intListIndex = 0;
                objDocs = new Core.Study.DocumentList[(ArrDocs.Length / 5)];

                for (int i = 0; i < objDocs.Length; i++)
                {
                    objDocs[i] = new Core.Study.DocumentList();
                    objDocs[i].SERIAL_NUMBER = Convert.ToInt32(ArrDocs[intListIndex]);
                    objDocs[i].ID = new Guid(ArrDocs[intListIndex + 1]);
                    objDocs[i].NAME = ArrDocs[intListIndex + 2].Trim();
                    objDocs[i].FILE_NAME = ArrDocs[intListIndex + 3].Trim();
                    objDocs[i].FILE_TYPE = ArrDocs[intListIndex + 4].Trim();

                    strFileName = Server.MapPath("~/CaseList/Temp/" + UserID.ToString() + "/") + ArrDocs[intListIndex + 3].Trim();
                    strFileName = strFileName.Replace("ajaxpro\\", "");
                    objDocs[i].FILE_CONTENT = GetFileBytes(strFileName);


                    intListIndex = intListIndex + 5;
                }
                intListIndex = 0;
                #endregion

                bReturn = objCore.SaveRecord(Server.MapPath("~"), objList, objSTs, objDocs, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[6];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
                    arrRet[2] = Convert.ToString(objCore.ID);
                    arrRet[3] = objCore.STUDY_UID;
                    arrRet[4] = objCore.SERIES_UID;
                    arrRet[5] = objCore.SERIES_NUMBER;
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

        #region GenrateStudyUID
        private string GenrateStudyUID()
        {
           
            string strSUID = "";
            strSUID = DicomUid.GenerateUid().UID;
            return strSUID;
        }
        #endregion

        #region GenrateSeriesUID
        private string GenrateSeriesUID()
        {

            string strSeriesUID = "";
            strSeriesUID = DicomUid.GenerateUid().UID;
            return strSeriesUID;
        }
        #endregion

        #region CreateSeriesNumber
        private string CreateSeriesNumber()
        {
            Random random = new Random();
            string combination = "0123456789";
            StringBuilder sbRef = new StringBuilder();
            string strSeriesNo = "";

            for (int i = 0; i < 6; i++)
            {
                sbRef.Append(combination[random.Next(combination.Length)]);

            }
            strSeriesNo = sbRef.ToString();
            return strSeriesNo;
        }
        #endregion
        #endregion
    }
}