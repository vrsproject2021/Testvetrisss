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
using ClearCanvas.Dicom;
using DICOMLib;
using AjaxPro;
using DICOMLib;

namespace VETRIS.CaseList
{
    [AjaxPro.AjaxNamespace("VRSManualSubmission")]
    public partial class VRSManualSubmission : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.Case.CaseStudy objCore;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSManualSubmission));
            SetAttributes();
            if ((!CallBackST.CausedCallback) && (!CallBackSelST.CausedCallback) && (!CallBackSF.CausedCallback))
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            btnSubmit1.Attributes.Add("onclick", "javascript:btnSubmit_OnClick('Y','X');");
            btnSubmit2.Attributes.Add("onclick", "javascript:btnSubmit_OnClick('Y','X');");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

            txtPLName.Attributes.Add("onchange", "javascript:txtPLName_OnChange();");

            txtDOS.Attributes.Add("onblur", "javascript:txtDOS_OnBlur();");
            imgDOS.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtDOS.ClientID + "','CalDOS');");

            ddlModality.Attributes.Add("onchange", "javascript:LoadStudyTypes();");
            ddlInstitution.Attributes.Add("onchange", "javascript:ddlInstitution_OnChange();");
            ddlSpecies.Attributes.Add("onchange", "javascript:ddlSpecies_OnChange();");

            //txtFromDt.Attributes.Add("onblur", "javascript:txtFromDt_OnBlur();");
            //imgFrom.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtFromDt.ClientID + "','CalFrom');");
            ddlDOBMonth.Attributes.Add("onchange", "javascript:ddlDOBMonth_OnChange();");
            ddlDOBDay.Attributes.Add("onchange", "javascript:ddlDOBDay_OnChange();");
            ddlDOBYear.Attributes.Add("onchange", "javascript:ddlDOBYear_OnChange();");

            txtPWt.Attributes.Add("onkeypress", "javascript:return parent.CheckDecimal(event);");
            txtPWt.Attributes.Add("onblur", "javascript:ResetValueDecimal(this,3);");
            txtPWt.Attributes.Add("onfocus", "javascript:this.select();");
            ddlPriority.Attributes.Add("onchange", "javascript:ddlPriority_OnChange();");

            //rdoDCM.Attributes.Add("onclick", "javascript:ToggleDCM();");
            //rdoImg.Attributes.Add("onclick", "javascript:ToggleImg();");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            Guid UserID = new Guid(Request.QueryString["uid"]);
            string strTheme = Request.QueryString["th"];
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            PopulateTimeDropDowns();
            txtDOS.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);// +" " + DateTime.Now.ToString("HH:mm");
            CalDOS.SelectedDate = DateTime.Today;
            ddlHr.SelectedValue = objComm.padZero(DateTime.Now.Hour);
            ddlMin.SelectedValue = objComm.padZero(DateTime.Now.Minute);
            //txtFromDt.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            //CalFrom.SelectedDate = DateTime.Today;
            txtPWt.Text = objComm.IMNumeric(0, 3);
            objComm = null;
            hdnFilePath.Value = Server.MapPath("~");
            //hdnTempIMGFolder.Value = Server.MapPath("~") + "/CaseList/IMGTemp/" + UserID.ToString();
            //hdnTempDCMFolder.Value = Server.MapPath("~") + "/CaseList/DCMTemp/" + UserID.ToString();
            PopulateDOBYears();
            hdnTempFolder.Value = Server.MapPath("~") + "/CaseList/MSTemp/" + UserID.ToString();
            FetchParameters(UserID);
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
            if (!Directory.Exists(Server.MapPath("~/CaseList/Temp/" + UserID.ToString())))
            {
                Directory.CreateDirectory(Server.MapPath("~/CaseList/Temp/" + UserID.ToString()));
            }
        }
        #endregion

        #region FetchParameters
        private void FetchParameters(Guid UserID)
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; bool bReturn = false; string strControlCode = string.Empty;
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            objComm = new classes.CommonClass();
            int intCnt = 0;

            try
            {
                objCore.USER_ID = UserID;

                bReturn = objCore.FetchBrowserParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    #region Institution
                    intCnt = ds.Tables["Institutions"].Rows.Count;
                    DataRow dr1 = ds.Tables["Institutions"].NewRow();
                    dr1["id"] = "00000000-0000-0000-0000-000000000000";
                    dr1["name"] = "Select One";
                    ds.Tables["Institutions"].Rows.InsertAt(dr1, 0);
                    ds.Tables["Institutions"].AcceptChanges();

                    ddlInstitution.DataSource = ds.Tables["Institutions"];
                    ddlInstitution.DataValueField = "id";
                    ddlInstitution.DataTextField = "name";
                    ddlInstitution.DataBind();

                    if (intCnt == 1) ddlInstitution.SelectedIndex = 1; else ddlInstitution.SelectedIndex = 0;
                    #endregion

                    #region Params
                    foreach (DataRow dr in ds.Tables["APIParams"].Rows)
                    {
                        strControlCode = Convert.ToString(dr["control_code"]).Trim();
                        switch (strControlCode)
                        {
                            case "DCMMODIFYEXEPATH":
                                hdnDCMMODIFYEXEPATH.Value = Convert.ToString(dr["data_type_string"]).Trim();
                                break;
                            case "FTPABSPATH":
                                hdnFTPABSPATH.Value = Convert.ToString(dr["data_type_string"]).Trim();
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion

                    #region Species
                    DataRow dr2 = ds.Tables["Species"].NewRow();
                    dr2["id"] = 0;
                    dr2["name"] = "Select One";
                    ds.Tables["Species"].Rows.InsertAt(dr2, 0);
                    ds.Tables["Species"].AcceptChanges();

                    ddlSpecies.DataSource = ds.Tables["Species"];
                    ddlSpecies.DataValueField = "id";
                    ddlSpecies.DataTextField = "name";
                    ddlSpecies.DataBind();
                    #endregion

                    #region Breed
                    ListItem li = new ListItem("Please select a species","00000000-0000-0000-0000-000000000000");
                    ddlBreed.Items.Add(li);
                    #endregion

                    #region Priority

                    foreach (DataRow dr in ds.Tables["Priority"].Rows)
                    {
                        if (hdnPriority.Value.Trim() != string.Empty) hdnPriority.Value += objComm.RecordDivider;
                        hdnPriority.Value += Convert.ToString(dr["priority_id"]) + objComm.RecordDivider + Convert.ToString(dr["is_stat"]);
                    }

                    DataRow dr3 = ds.Tables["Priority"].NewRow();
                    dr3["priority_id"] = "0";
                    dr3["priority_desc"] = "Select One";
                    ds.Tables["Priority"].Rows.InsertAt(dr3, 0);
                    ds.Tables["Priority"].AcceptChanges();

                    ddlPriority.DataSource = ds.Tables["Priority"];
                    ddlPriority.DataValueField = "priority_id";
                    ddlPriority.DataTextField = "priority_desc";
                    ddlPriority.DataBind();
                    #endregion

                    #region Category
                    //DataRow dr4 = ds.Tables["Category"].NewRow();
                    //dr4["id"] = "0";
                    //dr4["name"] = "Select One";
                    //ds.Tables["Category"].Rows.InsertAt(dr4, 0);
                    //ds.Tables["Category"].AcceptChanges();

                    ddlCategory.DataSource = ds.Tables["Category"];
                    ddlCategory.DataValueField = "id";
                    ddlCategory.DataTextField = "name";
                    ddlCategory.DataBind();

                    DataView dv = new DataView(ds.Tables["Category"]);
                    dv.RowFilter = "is_default='Y'";
                    if(dv.ToTable().Rows.Count>0) ddlCategory.SelectedValue = Convert.ToString(dv.ToTable().Rows[0]["id"]);
                    dv.Dispose();
                    #endregion

                    #region Physicians
                    if (ds.Tables["Physicians"].Rows.Count > 0)
                    {
                        DataRow dr5 = ds.Tables["Physicians"].NewRow();
                        dr5["id"] = "00000000-0000-0000-0000-000000000000";
                        dr5["name"] = "Select One";
                        ds.Tables["Physicians"].Rows.InsertAt(dr5, 0);
                        ds.Tables["Physicians"].AcceptChanges();
                    }
                    else
                    {
                        DataRow dr5 = ds.Tables["Physicians"].NewRow();
                        dr5["id"] = "00000000-0000-0000-0000-000000000000";
                        dr5["name"] = "Please select an institution";
                        ds.Tables["Physicians"].Rows.InsertAt(dr5, 0);
                        ds.Tables["Physicians"].AcceptChanges();
                    }

                    ddlPhys.DataSource = ds.Tables["Physicians"];
                    ddlPhys.DataValueField = "id";
                    ddlPhys.DataTextField = "name";
                    ddlPhys.DataBind();
                    #endregion

                    #region Modality
                    DataRow dr6 = ds.Tables["Modality"].NewRow();
                    dr6["id"] = "0";
                    dr6["name"] = "Select One";
                    ds.Tables["Modality"].Rows.InsertAt(dr6, 0);
                    ds.Tables["Modality"].AcceptChanges();

                    ddlModality.DataSource = ds.Tables["Modality"];
                    ddlModality.DataValueField = "id";
                    ddlModality.DataTextField = "name";
                    ddlModality.DataBind();

                    foreach (DataRow dr in ds.Tables["Modality"].Rows)
                    {
                        if (Convert.ToInt32(dr["id"]) > 0)
                        {
                            if (sb.ToString().Trim().Length > 0) sb.Append(objComm.RecordDivider);
                            sb.Append(Convert.ToString(dr["id"]) + objComm.SecondaryRecordDivider);
                            sb.Append(Convert.ToString(dr["name"]).Trim() + objComm.SecondaryRecordDivider);
                            sb.Append(Convert.ToString(dr["code"]).Trim() + objComm.SecondaryRecordDivider);
                            sb.Append(Convert.ToString(dr["dicom_tag"]).Trim());
                        }
                    }
                    hdnModality.Value = sb.ToString();

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
                    dr7["is_default"] = "N";
                    ds.Tables["Country"].Rows.InsertAt(dr7, 0);
                    ds.Tables["Country"].AcceptChanges();

                    ddlCountry.DataSource = ds.Tables["Country"];
                    ddlCountry.DataValueField = "id";
                    ddlCountry.DataTextField = "name";
                    ddlCountry.DataBind();

                    DataView dvDefCountry = new DataView(ds.Tables["Country"]);
                    dvDefCountry.RowFilter ="is_default='Y'";
                    if (dvDefCountry.ToTable().Rows.Count > 0) ddlCountry.SelectedValue = Convert.ToString(dvDefCountry.ToTable().Rows[0]["id"]);
                    dvDefCountry.Dispose();
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

                    hdnAfterHrs.Value = objCore.BEYOND_OPERATION_HOUR;
                }
                else
                    hdnError.Value = strCatchMessage.Trim();


                CreateUserDirectory(UserID);

            }
            catch (Exception ex)
            {
                hdnError.Value = ex.Message.Trim();
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }

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

        #region  Grids

        #region Study Type Grid

        #region CallBackST_Callback
        protected void CallBackST_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadStudyTypes(e.Parameters);
            grdST.Width = Unit.Percentage(98);
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

            grdSelST.Width = Unit.Percentage(98);
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
            spnVal.RenderControl(e.Output);
        }
        #endregion

        #region LoadFiles
        private void LoadFiles(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);
            string strPID = arrParams[2];
            string strAccnNo = arrParams[3];
            string strFile = string.Empty;
            string strUserID = arrParams[4];

            try
            {
                dtbl = CreateStudyFileTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 4)
                        {
                            DataRow dr = dtbl.NewRow();
                            dr["file_srl_no"] = arrRecords[i];
                            strFile = arrRecords[i + 1].Trim();
                            dr["file_name"] = arrRecords[i + 1].Trim();
                            dr["file_type"] = arrRecords[i + 2].Trim();
                            dr["file_type_desc"] = arrRecords[i + 3].Trim();
                            dtbl.Rows.Add(dr);
                        }

                    }
                }


                grdSF.DataSource = dtbl;
                grdSF.DataBind();
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

                spnVal.InnerHtml = "<input type=\"hidden\" id=\"hdnAccNo\" value=\"" + strAccnNo + "\" />";
                spnVal.InnerHtml += "<input type=\"hidden\" id=\"hdnPid\" value=\"" + strPID + "\" />";
            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
                spnVal.InnerHtml = "<input type=\"hidden\" id=\"hdnAccNo\" value=\"\" />";
                spnVal.InnerHtml += "<input type=\"hidden\" id=\"hdnPid\" value=\"\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
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
                        for (int i = 0; i < arrRecords.Length; i = i + 4)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["file_srl_no"] = intSrl;
                            dr["file_name"] = strFile = arrRecords[i + 1].Trim().Substring(arrRecords[i + 1].Trim().LastIndexOf("/") + 1, (arrRecords[i + 1].Trim().Length - (arrRecords[i + 1].Trim().LastIndexOf("/") + 1)));
                            dr["file_type"] = arrRecords[i + 2].Trim();
                            if (arrRecords[i + 2].Trim() == "D") dr["file_type_desc"] = "DICOM";
                            else if (arrRecords[i + 2].Trim() == "I")
                            {
                                strMimeType = classes.MIMEAssistant.GetMIMEType(Server.MapPath("~") + "/CaseList/MSTemp/" + strUserID + "/" + strFile);
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
                            else if (arrRecords[i + 2].Trim() == "P") dr["file_type_desc"] = "PDF";
                            dtbl.Rows.Add(dr);

                            if (i == 0 && arrRecords[i + 2].Trim() == "D")
                            {
                                dd.DicomFileName = Server.MapPath("~") + "/CaseList/MSTemp/" + strUserID + "/" + arrRecords[i + 1].Trim();
                                List<string> str = dd.dicomInfo;
                                strAccnNo = GetAccessionNo(str);
                                strPID = GetAccessionNo(str);
                            }
                        }

                    }
                }

                for (int i = 0; i < arrNew.Length; i = i + 2)
                {
                    DataRow drNew = dtbl.NewRow();
                    intSrl = intSrl + 1;
                    drNew["file_srl_no"] = intSrl;
                    drNew["file_name"] = strFile = arrNew[i].Trim().Substring(arrNew[i].Trim().LastIndexOf("/") + 1, (arrNew[i].Trim().Length - (arrNew[i].Trim().LastIndexOf("/") + 1)));
                    drNew["file_type"] = arrNew[i + 1];
                    if (arrNew[i + 1].Trim() == "D") drNew["file_type_desc"] = "DICOM";
                    else if (arrNew[i + 1].Trim() == "I")
                    {
                        strMimeType = classes.MIMEAssistant.GetMIMEType(Server.MapPath("~") + "/CaseList/MSTemp/" + strUserID + "/" + strFile);
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

                    if (strAccnNo.Trim() == string.Empty)
                    {
                        if (i == 0 && arrNew[i + 1].Trim() == "D")
                        {
                            dd.DicomFileName = arrNew[i].Trim();
                            List<string> str = dd.dicomInfo;
                            strAccnNo = GetAccessionNo(str);
                            strPID = GetAccessionNo(str);
                        }
                    }

                }

                grdSF.DataSource = dtbl;
                grdSF.DataBind();
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

                spnVal.InnerHtml = "<input type=\"hidden\" id=\"hdnAccNo\" value=\"" + strAccnNo + "\" />";
                spnVal.InnerHtml += "<input type=\"hidden\" id=\"hdnPid\" value=\"" + strPID + "\" />";
            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
                spnVal.InnerHtml = "<input type=\"hidden\" id=\"hdnAccNo\" value=\"\" />";
                spnVal.InnerHtml += "<input type=\"hidden\" id=\"hdnPid\" value=\"\" />";
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
            DICOMLib.DicomDecoder dd = new DICOMLib.DicomDecoder();

            try
            {
                dtbl = CreateStudyFileTable();
                strFolder = strFolder.Replace("\\", "/");

                if (arrRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrRecords.Length; i = i + 4)
                    {
                        if (Convert.ToInt32(arrRecords[i]) != intID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["file_srl_no"] = intSrl;
                            dr["file_name"] = arrRecords[i + 1].Trim();
                            dr["file_type"] = arrRecords[i + 2].Trim();
                            dr["file_type_desc"] = arrRecords[i + 3].Trim();
                            dtbl.Rows.Add(dr);

                            if (intLoop == 0)
                            {
                                dd.DicomFileName = strFolder + "/" + arrRecords[i + 1].Trim();
                                List<string> str = dd.dicomInfo;
                                strAccnNo = GetAccessionNo(str);
                                strPID = GetAccessionNo(str);
                            }

                            intLoop = intLoop + 1;
                        }
                        else
                            strFileName = arrRecords[i + 1].Trim();
                    }
                }

                grdSF.DataSource = dtbl;
                grdSF.DataBind();
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

                spnVal.InnerHtml = "<input type=\"hidden\" id=\"hdnAccNo\" value=\"" + strAccnNo + "\" />";
                spnVal.InnerHtml += "<input type=\"hidden\" id=\"hdnPid\" value=\"" + strPID + "\" />";


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
                spnVal.InnerHtml = "<input type=\"hidden\" id=\"hdnAccNo\" value=\"\" />";
                spnVal.InnerHtml += "<input type=\"hidden\" id=\"hdnPid\" value=\"\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
                dd = null;
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
                    for (int i = 0; i < arrRecords.Length; i = i + 4)
                    {
                        strFileName = arrRecords[i + 1].Trim();
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

                spnVal.InnerHtml = "<input type=\"hidden\" id=\"hdnAccNo\" value=\"\" />";
                spnVal.InnerHtml += "<input type=\"hidden\" id=\"hdnPid\" value=\"\" />";
            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
                spnVal.InnerHtml = "<input type=\"hidden\" id=\"hdnAccNo\" value=\"\" />";
                spnVal.InnerHtml += "<input type=\"hidden\" id=\"hdnPid\" value=\"\" />";
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
            dtbl.Columns.Add("file_srl_no", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("file_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("file_type", System.Type.GetType("System.String"));
            dtbl.Columns.Add("file_type_desc", System.Type.GetType("System.String"));
            dtbl.TableName = "files";
            return dtbl;
        }
        #endregion

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
                        arrRet = new string[6];
                        arrRet[0] = "true";
                        arrRet[1] = "00000000-0000-0000-0000-000000000000";
                        arrRet[2] = "Please select an instititution";
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
        public string[] SaveRecord(string[] ArrRecord, string[] ArrSTs, string[] ArrDCM, string[] ArrImg, string[] ArrDocs)
        {
            bool bReturn = false; bool bFileValid = true;
            string[] arrRet = new string[0];
            string[] arrFiles = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty; string strFile = string.Empty;
            string strFileName = string.Empty; Guid UserID = Guid.Empty; string strOutputMsg = string.Empty;
            string strDCMMODIFYEXEPATH = string.Empty; string strInsName = string.Empty; string strSUID = string.Empty;
            string strExtn = string.Empty;
            int intDCMSrl = 0;
            int intImgSrl = 0;
            int intDocSrl = 0;

            objComm = new classes.CommonClass();
            objCore = new Core.Case.CaseStudy();
            int intListIndex = 0;
            Core.Case.StudyTypeList[] objSTs = new Core.Case.StudyTypeList[0];
            Core.Case.HeaderImageList[] objImg = new Core.Case.HeaderImageList[0];
            Core.Case.HeaderDICOMList[] objDCM = new Core.Case.HeaderDICOMList[0];
            Core.Case.HeaderDocumentList[] objDocs = new Core.Case.HeaderDocumentList[0];
            string[] arrDocRecords = new string[0];
            DICOMLib.DicomDecoder dd = new DICOMLib.DicomDecoder();

            try
            {
                objComm.SetRegionalFormat();

                #region Header Values
                objCore.SESSION_ID = "MS1D" + DateTime.Now.ToString("MMddyyHHmmss") + CoreCommon.RandomString(3);
                objCore.PATIENT_FIRST_NAME = ArrRecord[0].Trim();
                objCore.PATIENT_LAST_NAME = ArrRecord[1].Trim();
                objCore.PATIENT_NAME = (ArrRecord[1].Trim() + " " + ArrRecord[0].Trim()).Trim();
                objCore.PATIENT_GENDER = ArrRecord[2].Trim();
                objCore.OWNER_FIRST_NAME = ArrRecord[3].Trim();
                objCore.OWNER_LAST_NAME = ArrRecord[4].Trim();
                objCore.SEX_NEUTERED = ArrRecord[5].Trim();
                objCore.PATIENT_WEIGHT = Convert.ToDecimal(ArrRecord[6]);
                objCore.WEIGHT_UOM = ArrRecord[7].Trim();
                objCore.PATIENT_DOB = Convert.ToDateTime(ArrRecord[8]);
                objCore.PATIENT_AGE = ArrRecord[9].Trim();
                objCore.SPECIES_ID = Convert.ToInt32(ArrRecord[10]);
                objCore.BREED_ID = new Guid(ArrRecord[11]);
                objCore.STUDY_DATE = Convert.ToDateTime(ArrRecord[12]);
                objCore.PATIENT_ID = ArrRecord[13].Trim();
                objCore.ACCESSION_NO = ArrRecord[14].Trim();
                objCore.REASON = ArrRecord[15].Trim();
                objCore.PHYSICIAN_NOTE = ArrRecord[16].Trim();
                objCore.MODALITY_ID = Convert.ToInt32(ArrRecord[17]);
                objCore.PRIORITY_ID = Convert.ToInt32(ArrRecord[18]);
                objCore.INSTITUTION_ID = new Guid(ArrRecord[19]);
                strInsName = ArrRecord[20].Trim();
                objCore.PHYSICIAN_ID = new Guid(ArrRecord[21]);
                objCore.CONSULT_APPLIED = ArrRecord[22].Trim();
                objCore.CATEGORY_ID = Convert.ToInt32(ArrRecord[23]);
                strDCMMODIFYEXEPATH = ArrRecord[24].Trim();
                objCore.SENDER_TIME_ZONE_OFFSET_MINUTES = Convert.ToInt32(ArrRecord[25]);
                objCore.SUBMIT_PRIORITY = ArrRecord[26].Trim();
                objCore.PATIENT_COUNTRY_ID = Convert.ToInt32(ArrRecord[27]);
                objCore.PATIENT_STATE_ID = Convert.ToInt32(ArrRecord[28]);
                objCore.PATIENT_CITY = ArrRecord[29].Trim();
                objCore.CHECK_DOB = ArrRecord[30].Trim();
                objCore.USER_ID = UserID = new Guid(ArrRecord[31]);
                objCore.USER_SESSION_ID = new Guid(ArrRecord[32]);

                #endregion

                #region Study Types
                objSTs = new Core.Case.StudyTypeList[(ArrSTs.Length)];

                for (int i = 0; i < objSTs.Length; i++)
                {
                    objSTs[i] = new Core.Case.StudyTypeList();
                    objSTs[i].SERIAL_NUMBER = i + 1;
                    objSTs[i].ID = new Guid(ArrSTs[intListIndex]);
                    intListIndex = intListIndex + 1;
                }
                #endregion

                #region DICOM files
                intListIndex = 0;
                objDCM = new Core.Case.HeaderDICOMList[(ArrDCM.Length / 2)];

                for (int i = 0; i < objDCM.Length; i++)
                {
                    strFile = ArrDCM[intListIndex + 1].Trim();
                    strFileName = Server.MapPath("~/CaseList/MSTemp/" + UserID.ToString() + "/") + ArrDCM[intListIndex + 1].Trim();
                    strFileName = strFileName.Replace("ajaxpro\\", "");
                    dd.DicomFileName = strFileName;
                    List<string> str = dd.dicomInfo;

                    if (i == 0)
                    {
                        strSUID = GetStudyUID(str).Trim();
                        objCore.STUDY_UID = strSUID;
                    }

                    #region Update DICOM Files with uniform Study UID & Institution name
                    if ((GetStudyUID(str).Trim() == strSUID) && (GetInstitutionName(str).Trim() == strInsName))
                    {
                        bFileValid = true;
                    }
                    else
                    {
                       
                        ModifyDCMFile(strDCMMODIFYEXEPATH, strInsName, strSUID, strFileName, ref strOutputMsg, ref strReturnMsg);

                        dd.DicomFileName = strFileName;
                        str = dd.dicomInfo;
                        if ((GetStudyUID(str).Trim() != strSUID) || (GetInstitutionName(str).Trim() != strInsName))
                        {
                            bFileValid = false;
                            strReturnMsg = strOutputMsg;
                        }
                        else
                            bFileValid = true;
                    }
                    #endregion


                    if (bFileValid)
                    {
                        objDCM[i] = new Core.Case.HeaderDICOMList();
                        objDCM[i].SERIAL_NUMBER = intDCMSrl + 1;

                        ArrDCM[intListIndex + 1] = ArrDCM[intListIndex + 1].Replace(" ", "_");
                        ArrDCM[intListIndex + 1] = ArrDCM[intListIndex + 1].Replace("(", "");
                        ArrDCM[intListIndex + 1] = ArrDCM[intListIndex + 1].Replace(")", "");
                        ArrDCM[intListIndex + 1] = ArrDCM[intListIndex + 1].Replace("'", "");
                        ArrDCM[intListIndex + 1] = ArrDCM[intListIndex + 1].Replace("\"", "");
                        ArrDCM[intListIndex + 1] = ArrDCM[intListIndex + 1].Replace("/", "_");
                        ArrDCM[intListIndex + 1] = ArrDCM[intListIndex + 1].Replace("\\", "_");
                        ArrDCM[intListIndex + 1] = ArrDCM[intListIndex + 1].Replace("#", "");
                        ArrDCM[intListIndex + 1] = ArrDCM[intListIndex + 1].Replace("&", "");
                        ArrDCM[intListIndex + 1] = ArrDCM[intListIndex + 1].Replace("@", "");
                        ArrDCM[intListIndex + 1] = ArrDCM[intListIndex + 1].Replace("?", "");
                        ArrDCM[intListIndex + 1] = ArrDCM[intListIndex + 1].Replace(",", "");
                        ArrDCM[intListIndex + 1] = ArrDCM[intListIndex + 1].Replace("__", "_");


                        objDCM[i].FILE_NAME = ArrDCM[intListIndex + 1].Trim();
                        objDCM[i].FILE_CONTENT = GetFileBytes(strFileName);
                        intDCMSrl = intDCMSrl + 1;
                        intListIndex = intListIndex + 2;

                        if (File.Exists(strFileName + ".bak"))
                        {
                            File.Delete(strFileName + ".bak");
                        }
                    }
                    else
                    {
                        strReturnMsg = strOutputMsg;
                        break;
                    }
                }
                #endregion

                if (bFileValid)
                {
                    if (ArrDCM.Length == 0)
                    {
                        #region Image Files
                        intListIndex = 0;
                        objImg = new Core.Case.HeaderImageList[(ArrImg.Length / 2)];

                        for (int i = 0; i < objImg.Length; i++)
                        {
                            if (i == 0)
                            {
                                objCore.STUDY_UID = GenrateStudyUID();
                                objCore.SERIES_UID = GenrateSeriesUID();
                                objCore.SERIES_NUMBER = CreateSeriesNumber();
                            }
                            strFileName = Server.MapPath("~/CaseList/MSTemp/" + UserID.ToString() + "/") + ArrImg[intListIndex + 1].Trim();
                            strFileName = strFileName.Replace("ajaxpro\\", "");
                            

                            objImg[i] = new Core.Case.HeaderImageList();
                            //objImg[i].SERIAL_NUMBER = Convert.ToInt32(ArrImg[intListIndex]);
                            objImg[i].SERIAL_NUMBER = intImgSrl + 1;

                            ArrImg[intListIndex + 1] = ArrImg[intListIndex + 1].Replace(" ", "_");
                            ArrImg[intListIndex + 1] = ArrImg[intListIndex + 1].Replace("(", "");
                            ArrImg[intListIndex + 1] = ArrImg[intListIndex + 1].Replace(")", "");
                            ArrImg[intListIndex + 1] = ArrImg[intListIndex + 1].Replace("'", "");
                            ArrImg[intListIndex + 1] = ArrImg[intListIndex + 1].Replace("\"", "");
                            ArrImg[intListIndex + 1] = ArrImg[intListIndex + 1].Replace("/", "_");
                            ArrImg[intListIndex + 1] = ArrImg[intListIndex + 1].Replace("\\", "_");
                            ArrImg[intListIndex + 1] = ArrImg[intListIndex + 1].Replace("#", "");
                            ArrImg[intListIndex + 1] = ArrImg[intListIndex + 1].Replace("&", "");
                            ArrImg[intListIndex + 1] = ArrImg[intListIndex + 1].Replace("@", "");
                            ArrImg[intListIndex + 1] = ArrImg[intListIndex + 1].Replace("?", "");
                            ArrImg[intListIndex + 1] = ArrImg[intListIndex + 1].Replace(",", "");
                            ArrImg[intListIndex + 1] = ArrImg[intListIndex + 1].Replace("__", "_");

                            objImg[i].FILE_NAME = ArrImg[intListIndex + 1].Trim();
                            objImg[i].FILE_CONTENT = GetFileBytes(strFileName);

                            intImgSrl = intImgSrl + 1;
                            intListIndex = intListIndex + 2;
                        }
                        #endregion

                        intListIndex = 0;

                        #region PDF FIles
                        objDocs = new Core.Case.HeaderDocumentList[(ArrDocs.Length / 2)];
                        for (int i = 0; i < objDocs.Length; i++)
                        {
                            strFileName = Server.MapPath("~/CaseList/MSTemp/" + UserID.ToString() + "/") + ArrDocs[intListIndex + 1].Trim();
                            strFileName = strFileName.Replace("ajaxpro\\", "");
                            strExtn = Path.GetExtension(strFileName);

                            objDocs[i] = new Core.Case.HeaderDocumentList();
                            objDocs[i].SERIAL_NUMBER = intDocSrl + 1;
                            objDocs[i].ID = new Guid("00000000-0000-0000-0000-000000000000");
                            objDocs[i].NAME = ArrDocs[intListIndex + 1].Trim();

                            ArrDocs[intListIndex + 1] = ArrDocs[intListIndex + 1].Replace(" ", "_");
                            ArrDocs[intListIndex + 1] = ArrDocs[intListIndex + 1].Replace("(", "");
                            ArrDocs[intListIndex + 1] = ArrDocs[intListIndex + 1].Replace(")", "");
                            ArrDocs[intListIndex + 1] = ArrDocs[intListIndex + 1].Replace("'", "");
                            ArrDocs[intListIndex + 1] = ArrDocs[intListIndex + 1].Replace("\"", "");
                            ArrDocs[intListIndex + 1] = ArrDocs[intListIndex + 1].Replace("/", "_");
                            ArrDocs[intListIndex + 1] = ArrDocs[intListIndex + 1].Replace("\\", "_");
                            ArrDocs[intListIndex + 1] = ArrDocs[intListIndex + 1].Replace("#", "");
                            ArrDocs[intListIndex + 1] = ArrDocs[intListIndex + 1].Replace("&", "");
                            ArrDocs[intListIndex + 1] = ArrDocs[intListIndex + 1].Replace("@", "");
                            ArrDocs[intListIndex + 1] = ArrDocs[intListIndex + 1].Replace("?", "");
                            ArrDocs[intListIndex + 1] = ArrDocs[intListIndex + 1].Replace(",", "");
                            ArrDocs[intListIndex + 1] = ArrDocs[intListIndex + 1].Replace("__", "_");
                            
                            objDocs[i].FILE_NAME = ArrDocs[intListIndex + 1].Trim();
                            objDocs[i].FILE_TYPE = strExtn;
                            objDocs[i].FILE_CONTENT = GetFileBytes(strFileName);

                            intDocSrl = intDocSrl + 1;
                            intListIndex = intListIndex + 2;
                        }
                        #endregion
                    }
                    else
                    {
                        #region Documents
                        intListIndex = 0;
                        arrDocRecords = new string[(ArrImg.Length) + (ArrDocs.Length)];

                        #region Populate Image Files
                        for (int i = 0; i < ArrImg.Length; i=i+2)
                        {
                            arrDocRecords[intListIndex] = ArrImg[i];
                            arrDocRecords[intListIndex + 1] = ArrImg[i + 1];
                            intListIndex = i + 2;
                        }
                        #endregion

                        #region Populate Doc Files
                        for (int i = 0; i < ArrDocs.Length; i = i + 2)
                        {
                            arrDocRecords[intListIndex] = ArrDocs[i];
                            arrDocRecords[intListIndex + 1] = ArrDocs[i + 1];
                            intListIndex = i + 2;
                        }
                        #endregion


                        intListIndex = 0;
                        objDocs = new Core.Case.HeaderDocumentList[(arrDocRecords.Length / 2)];

                        for (int i = 0; i < objDocs.Length; i++)
                        {
                            strFileName = Server.MapPath("~/CaseList/MSTemp/" + UserID.ToString() + "/") + arrDocRecords[intListIndex + 1].Trim();
                            strFileName = strFileName.Replace("ajaxpro\\", "");
                            strExtn = Path.GetExtension(strFileName);

                            objDocs[i] = new Core.Case.HeaderDocumentList();
                            objDocs[i].SERIAL_NUMBER = intDocSrl + 1;
                            objDocs[i].ID = new Guid("00000000-0000-0000-0000-000000000000");
                            objDocs[i].NAME = arrDocRecords[intListIndex + 1].Trim();

                            arrDocRecords[intListIndex + 1] = arrDocRecords[intListIndex + 1].Replace(" ", "_");
                            arrDocRecords[intListIndex + 1] = arrDocRecords[intListIndex + 1].Replace("(", "");
                            arrDocRecords[intListIndex + 1] = arrDocRecords[intListIndex + 1].Replace(")", "");
                            arrDocRecords[intListIndex + 1] = arrDocRecords[intListIndex + 1].Replace("'", "");
                            arrDocRecords[intListIndex + 1] = arrDocRecords[intListIndex + 1].Replace("\"", "");
                            arrDocRecords[intListIndex + 1] = arrDocRecords[intListIndex + 1].Replace("/", "_");
                            arrDocRecords[intListIndex + 1] = arrDocRecords[intListIndex + 1].Replace("\\", "_");
                            arrDocRecords[intListIndex + 1] = arrDocRecords[intListIndex + 1].Replace("#", "");
                            arrDocRecords[intListIndex + 1] = arrDocRecords[intListIndex + 1].Replace("&", "");
                            arrDocRecords[intListIndex + 1] = arrDocRecords[intListIndex + 1].Replace("@", "");
                            arrDocRecords[intListIndex + 1] = arrDocRecords[intListIndex + 1].Replace("?", "");
                            arrDocRecords[intListIndex + 1] = arrDocRecords[intListIndex + 1].Replace(",", "");
                            arrDocRecords[intListIndex + 1] = arrDocRecords[intListIndex + 1].Replace("__", "_");

                            objDocs[i].FILE_NAME = arrDocRecords[intListIndex + 1].Trim();
                            objDocs[i].FILE_TYPE = strExtn;
                            objDocs[i].FILE_CONTENT = GetFileBytes(strFileName);

                            intDocSrl = intDocSrl + 1;
                            intListIndex = intListIndex + 2;
                        }
                        #endregion
                    }

                    intListIndex = 0;
                    bReturn = objCore.SaveManualSubmission(Server.MapPath("~"), objSTs, objDCM, objImg, objDocs, ref strReturnMsg, ref strCatchMessage);

                    #region Post Saving
                    if (bReturn)
                    {
                        arrFiles= Directory.GetFiles(Server.MapPath("~/CaseList/MSTemp/" + UserID.ToString() + "/"));
                        foreach (string file in arrFiles)
                        {
                            if (File.Exists(file)) File.Delete(file);
                        }
                        Directory.Delete(Server.MapPath("~/CaseList/MSTemp/" + UserID.ToString()));

                        arrRet = new string[3];
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
                            arrRet = new string[7];
                            arrRet[0] = "false";
                            arrRet[1] = strReturnMsg.Trim();
                            arrRet[2] = objCore.USER_NAME;
                            arrRet[3] = objCore.NEXT_OPERATION_TIME;
                            arrRet[4] = objCore.DELIVERY_TIME;
                            arrRet[5] = objCore.MESSAGE_DISPLAY;
                            arrRet[6] = objComm.CurrentTimeZone;
                        }
                    }
                    #endregion
                }
                else
                {
                    arrRet = new string[4];
                    arrRet[0] = "false";
                    arrRet[1] = "342";
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

        #region File Update Methods

        #region DICOM

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

        #region GetInstitutionName
        private string GetInstitutionName(List<string> str)
        {

            string InstName = string.Empty;
            string s1, s4, s5, s11, s12;

            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                if ((s11.ToUpper() == "0008") && (s12.ToUpper() == "0080"))
                {
                    InstName = s5.Replace("\0", "");
                    InstName = s5.Replace("^", " ");
                    break;
                }

            }
            return InstName;

        }
        #endregion

        #region GetAccessionNo
        private string GetAccessionNo(List<string> str)
        {

            string AccNo = string.Empty;
            string s1, s4, s5, s11, s12;

            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                if ((s11.ToUpper() == "0008") && (s12.ToUpper() == "0050"))
                {
                    AccNo = s5.Replace("\0", "");
                    break;
                }

            }
            return AccNo;

        }
        #endregion

        #region GetPatientID
        private string GetPatientID(List<string> str)
        {

            string PID = string.Empty;
            string s1, s4, s5, s11, s12;

            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                if ((s11.ToUpper() == "0010") && (s12.ToUpper() == "0020"))
                {
                    PID = s5.Replace("\0", "");
                    break;
                }

            }
            return PID;

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

        #region Images

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

        #endregion
    }
}