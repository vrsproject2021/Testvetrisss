using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Configuration;
using VETRIS.Core;


namespace VETRIS.CaseList
{
    [AjaxPro.AjaxNamespace("VRSInProgressBrw")]
    public partial class VRSInProgressBrw : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Case.CaseStudy objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSInProgressBrw));
            if (!CallBackBrw.CausedCallback)
                SetPageValue();
            SetAttributes();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {

            btnRefresh.Attributes.Add("onclick", "javascript:ResetRecord();view_Searchform();");
            btnSearch.Attributes.Add("onclick", "javascript:SearchRecord();view_Searchform();");
            btnReset.Attributes.Add("onclick", "javascript:ResetRecord();");
            btnClose.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");
            btnDV.Attributes.Add("onclick", "javascript:btnDV_OnClick();");
            btnGetCase.Attributes.Add("onclick", "javascript:btnGetCase_OnClick();");
            //ddlSpecies.Attributes.Add("onchange", "javascript:ddlSpecies_OnChange();");

            txtFromDt.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgFrom.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtFromDt.ClientID + "','CalFrom');");
            txtTillDt.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgTill.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtTillDt.ClientID + "','CalTill');");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"].ToString());
            string strID = string.Empty;

            SetCSS(Request.QueryString["th"]);
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            txtFromDt.Text = objComm.IMDateFormat(DateTime.Today.AddDays(-7), objComm.DateFormat);
            txtTillDt.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            objComm = null;
            
            FetchSearchParameters(UserID);
            DeleteUserDirectory(UserID);

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

        #region DeleteUserDirectory
        private void DeleteUserDirectory(Guid UserID)
        {
            string[] arrTemp = new string[0];
            string[] arrFiles = new string[0];

            try
            {
                if (Directory.Exists(Server.MapPath("~/Study/Temp/" + UserID.ToString())))
                {
                    if (Directory.Exists(Server.MapPath("~/Study/Temp/" + UserID.ToString())))
                    {
                        arrTemp = Directory.GetFiles(Server.MapPath("~") + "/Study/Temp/" + UserID.ToString());
                        if (arrTemp.Length > 0)
                        {
                            for (int i = 0; i < arrTemp.Length; i++)
                            {
                                File.Delete(arrTemp[i]);
                            }
                        }

                    }


                    Directory.Delete(Server.MapPath("~") + "/Study/Temp/" + UserID.ToString());

                }
            }
            catch (Exception ex)
            {
                ;
            }

            try
            {
                arrTemp = new string[0];
                if (Directory.Exists(Server.MapPath("~/CaseList/Temp/" + UserID.ToString())))
                {

                    arrTemp = Directory.GetFiles(Server.MapPath("~") + "/CaseList/Temp/" + UserID.ToString());
                    if (arrTemp.Length > 0)
                    {
                        for (int i = 0; i < arrTemp.Length; i++)
                        {
                            File.Delete(arrTemp[i]);
                        }
                    }

                    arrTemp = new string[0];
                    arrTemp = Directory.GetDirectories(Server.MapPath("~") + "/CaseList/Temp/" + UserID.ToString());
                    if (arrTemp.Length > 0)
                    {
                        for (int i = 0; i < arrTemp.Length; i++)
                        {
                            arrFiles = new string[0];
                            arrFiles = Directory.GetFiles(arrTemp[i]);
                            for (int j = 0; j < arrFiles.Length; j++)
                            {
                                File.Delete(arrFiles[j]);
                            }

                            Directory.Delete(arrTemp[i]);
                        }
                    }

                    Directory.Delete(Server.MapPath("~") + "/CaseList/Temp/" + UserID.ToString());

                }
            }
            catch (Exception ex)
            {
                ;
            }
        }
        #endregion

        #region FetchSearchParameters
        private void FetchSearchParameters(Guid UserID)
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            int intCnt = 0;
            string strControlCode = string.Empty;
            objComm = new classes.CommonClass();

            try
            {
                objCore.USER_ID = UserID;

                bReturn = objCore.FetchBrowserParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    #region Radiologist Functional Rights
                    foreach (DataRow dr in ds.Tables["RadFnRights"].Rows)
                    {
                        if (hdnRadFnRights.Value.Trim() != string.Empty) hdnRadFnRights.Value += objComm.RecordDivider;
                        hdnRadFnRights.Value += Convert.ToString(dr["right_code"]);
                    }
                    #endregion

                    #region Modality
                    DataRow dr1 = ds.Tables["Modality"].NewRow();
                    dr1["id"] = 0;
                    dr1["name"] = "Select One";
                    ds.Tables["Modality"].Rows.InsertAt(dr1, 0);
                    ds.Tables["Modality"].AcceptChanges();

                    ddlModality.DataSource = ds.Tables["Modality"];
                    ddlModality.DataValueField = "id";
                    ddlModality.DataTextField = "name";
                    ddlModality.DataBind();
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

                    #region Institution
                    intCnt = ds.Tables["Institutions"].Rows.Count;
                    DataRow dr3 = ds.Tables["Institutions"].NewRow();
                    dr3["id"] = "00000000-0000-0000-0000-000000000000";
                    dr3["code"] = "Select One";
                    dr3["name"] = "Select One";
                    ds.Tables["Institutions"].Rows.InsertAt(dr3, 0);
                    ds.Tables["Institutions"].AcceptChanges();

                    ddlInstitution.DataSource = ds.Tables["Institutions"];
                    ddlInstitution.DataValueField = "id";
                    if (hdnRadFnRights.Value.Trim().IndexOf("VWINSTINFO") > -1)
                        ddlInstitution.DataTextField = "name";
                    else
                        ddlInstitution.DataTextField = "code";
                    ddlInstitution.DataBind();

                    if (intCnt == 1) ddlInstitution.SelectedIndex = 1;
                    #endregion

                    #region Status
                    DataRow dr4 = ds.Tables["InProgressStatus"].NewRow();
                    dr4["status_id"] = "-1";
                    dr4["vrs_status_desc"] = "All";
                    dr4["status_desc"] = "All";
                    ds.Tables["InProgressStatus"].Rows.InsertAt(dr4, 0);
                    ds.Tables["InProgressStatus"].AcceptChanges();

                    ddlStatus.DataSource = ds.Tables["InProgressStatus"];
                    ddlStatus.DataValueField = "status_id";
                    if (objCore.USER_ROLE_CODE == "RDL") ddlStatus.DataTextField = "status_desc";
                    else ddlStatus.DataTextField = "vrs_status_desc";
                    ddlStatus.DataBind();
                    #endregion

                    #region Category
                    DataRow dr5 = ds.Tables["Category"].NewRow();
                    dr5["id"] = "0";
                    dr5["name"] = "Select One";
                    ds.Tables["Category"].Rows.InsertAt(dr5, 0);
                    ds.Tables["Category"].AcceptChanges();

                    ddlCategory.DataSource = ds.Tables["Category"];
                    ddlCategory.DataValueField = "id";
                    ddlCategory.DataTextField = "name";
                    ddlCategory.DataBind();
                    #endregion

                    #region Priority
                    //DataRow dr6 = ds.Tables["Priority"].NewRow();
                    //dr6["priority_id"] = 0;
                    //dr6["priority_desc"] = "Select One";
                    //ds.Tables["Priority"].Rows.InsertAt(dr6, 0);
                    //ds.Tables["Priority"].AcceptChanges();

                    foreach (DataRow dr in ds.Tables["Priority"].Rows)
                    {
                        if (hdnPriority.Value.Trim() != string.Empty) hdnPriority.Value += objComm.RecordDivider;
                        hdnPriority.Value += Convert.ToString(dr["priority_id"]) + objComm.RecordDivider;
                        hdnPriority.Value += Convert.ToString(dr["priority_desc"]).Trim();
                    }
                    #endregion

                    #region Priority Dropdown List
                    dr1 = ds.Tables["Priority"].NewRow();
                    dr1["priority_id"] = 0;
                    dr1["priority_desc"] = "Select One";
                    ds.Tables["Priority"].Rows.InsertAt(dr1, 0);
                    ds.Tables["Priority"].AcceptChanges();

                    ddlPriority.DataSource = ds.Tables["Priority"];
                    ddlPriority.DataValueField = "priority_id";
                    ddlPriority.DataTextField = "priority_desc";
                    ddlPriority.DataBind();
                    #endregion

                    #region Radiologist
                    intCnt = ds.Tables["Radiologists"].Rows.Count;
                    DataRow dr6 = ds.Tables["Radiologists"].NewRow();
                    dr6["id"] = "00000000-0000-0000-0000-000000000000";
                    dr6["name"] = "Select One";
                    ds.Tables["Radiologists"].Rows.InsertAt(dr6, 0);
                    ds.Tables["Radiologists"].AcceptChanges();

                    DataRow drNone = ds.Tables["Radiologists"].NewRow();
                    drNone["id"] = "11111111-1111-1111-1111-111111111111";
                    drNone["name"] = "None";
                    ds.Tables["Radiologists"].Rows.InsertAt(drNone, 1);
                    ds.Tables["Radiologists"].AcceptChanges();

                    ddlRadiologist.DataSource = ds.Tables["Radiologists"];
                    ddlRadiologist.DataValueField = "id";
                    ddlRadiologist.DataTextField = "name";
                    ddlRadiologist.DataBind();

                    //if (intCnt == 1) ddlRadiologist.SelectedIndex = 1;
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

                    #region Control Codes
                    foreach (DataRow dr in ds.Tables["APIParams"].Rows)
                    {
                        strControlCode = Convert.ToString(dr["control_code"]);
                        switch (strControlCode)
                        {
                            case "PACSARCHIVEFLDR":
                                hdnPACSARCHIVEFLDR.Value = Convert.ToString(dr["data_type_string"]).Trim();
                                break;
                            case "PACSARCHALTFLDR":
                                hdnPACSARCHALTFLDR.Value = Convert.ToString(dr["data_type_string"]).Trim();
                                break;
                            case "DCMMODIFYEXEPATH":
                                hdnDCMMODIFYEXEPATH.Value = Convert.ToString(dr["data_type_string"]).Trim();
                                break;
                            case "SCHCASVCENBL":
                                hdnSCHCASVCENBL.Value = Convert.ToString(dr["data_type_string"]).Trim();
                                break;
                            default:
                                break;
                        }

                    }
                    #endregion

                    if (objCore.PAS_USER_PASSWORD != string.Empty)
                    {
                        hdnPACSCred.Value = CoreCommon.EncryptString(objCore.PACS_USER_ID + "±" + objCore.PAS_USER_PASSWORD);
                    }

                    hdnAfterHrs.Value = objCore.BEYOND_OPERATION_HOUR;
                }
                else
                    hdnError.Value = strCatchMessage.Trim();

                //ListItem objLI = new ListItem();
                //objLI.Value = "00000000-0000-0000-0000-000000000000";
                //objLI.Text = "Select One";

                //ddlBreed.Items.Add(objLI);


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

        #region CallBackBrw_Callback
        protected void CallBackBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            SearchRecord(e.Parameters);
            grdBrw.Width = Unit.Percentage(99);
            grdBrw.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region SearchRecord
        private void SearchRecord(string[] arrRecord)
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            string strUserRole = string.Empty;
            objComm = new classes.CommonClass();
            string strACCLOCKSTUDY = string.Empty;


            try
            {
                objComm.SetRegionalFormat();
                objCore.PATIENT_NAME = arrRecord[0].Trim();
                objCore.MODALITY_ID = Convert.ToInt32(arrRecord[1]);
                objCore.FILTER_BY_RECEIVED_DATE = arrRecord[2].Trim();
                objCore.RECEIVED_DATE_FROM = Convert.ToDateTime(arrRecord[3]);
                objCore.RECEIVED_DATE_TILL = Convert.ToDateTime(arrRecord[4]);
                objCore.INSTITUTION_ID = new Guid(arrRecord[5]);
                objCore.PACS_STATUS_ID = Convert.ToInt32(arrRecord[6]);
                objCore.CATEGORY_ID = Convert.ToInt32(arrRecord[7]);
                objCore.RADIOLOGIST_ID = new Guid(arrRecord[8].Trim());
                objCore.SPECIES_ID = Convert.ToInt32(arrRecord[9]);
                strACCLOCKSTUDY = arrRecord[10].Trim();
                objCore.USER_ID = new Guid(arrRecord[11].Trim());
                strUserRole = arrRecord[12].Trim();
                objCore.MENU_ID = Convert.ToInt32(arrRecord[13]);
                objCore.USER_SESSION_ID = new Guid(arrRecord[14].Trim());
                objCore.STUDY_UID = arrRecord[15].Trim();
                objCore.PRIORITY_ID = Convert.ToInt32(arrRecord[16]);

                bReturn = objCore.SearchInProgressBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    grdBrw.DataSource = ds.Tables["BrowserList"];
                    grdBrw.Levels[0].Columns[3].FormatString = objComm.DateFormat + " HH:mm";
                    grdBrw.Levels[0].Columns[6].FormatString = objComm.DateFormat + " HH:mm";
                    grdBrw.DataBind();
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
            if ((strUserRole == "SUPP"))
            {
                grdBrw.Levels[0].Columns[1].Visible = true;
                grdBrw.Levels[0].Columns[5].Visible = true;
                grdBrw.Levels[0].Columns[13].Width = 120;
                grdBrw.Levels[0].Columns[16].Visible = true;
                grdBrw.Levels[0].Columns[21].Visible = true;
                grdBrw.Levels[0].Columns[22].Visible = true;


            }
            else if ((strUserRole == "SYSADMIN"))
            {
                grdBrw.Levels[0].Columns[5].Visible = true;
                grdBrw.Levels[0].Columns[13].Width = 120;
                grdBrw.Levels[0].Columns[16].Visible = true;
                grdBrw.Levels[0].Columns[21].Visible = true;
                grdBrw.Levels[0].Columns[22].Visible = true;
            }
            else if ((strUserRole == "RDL"))
            {
                grdBrw.Levels[0].Columns[8].Visible = false;
                grdBrw.Levels[0].Columns[9].Visible = false;
                grdBrw.Levels[0].Columns[10].Visible = true;
                grdBrw.Levels[0].Columns[13].Width = 150;
                grdBrw.Levels[0].Columns[16].Visible = false;
                if (strACCLOCKSTUDY == "Y") grdBrw.Levels[0].Columns[16].Visible = true;
                grdBrw.Levels[0].Columns[22].Visible = true;
                grdBrw.Levels[0].Columns[28].Visible = true;

            }
            else if ((strUserRole == "TRS"))
            {
                grdBrw.Levels[0].Columns[4].Visible = false;
                grdBrw.Levels[0].Columns[5].Visible = true;
                grdBrw.Levels[0].Columns[5].HeadingText = "Time Left";
                grdBrw.Levels[0].Columns[8].Visible = false;
                grdBrw.Levels[0].Columns[9].Visible = false;
                grdBrw.Levels[0].Columns[10].Visible = true;
                grdBrw.Levels[0].Columns[16].Visible = true;
                grdBrw.Levels[0].Columns[22].Visible = true;
                grdBrw.Levels[0].Columns[28].Visible = false;

            }

        }
        #endregion

        #region UpdatePriority (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] UpdatePriority(string[] arrParams)
        {
            string strCatchMessage = ""; bool bReturn = false;
            objCore = new Core.Case.CaseStudy();


            string[] arrRet = new string[0];

            try
            {
                objCore.ID = new Guid(arrParams[0].Trim());
                objCore.PRIORITY_ID = Convert.ToInt32(arrParams[1].Trim());
                objCore.USER_ID = new Guid(arrParams[2].Trim());

                bReturn = objCore.UpdatePriority(Server.MapPath("~"), ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[1];
                    arrRet[0] = "true";
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
                objCore = null;
            }
            return arrRet;
        }
        #endregion

        #region CheckRadiologistLock (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] CheckRadiologistLock(string[] arrParams)
        {
            string strCatchMessage = ""; string strReturnMessage = "";
            bool bReturn = false;
            string strRefresh = "N";
            objCore = new Core.Case.CaseStudy();


            string[] arrRet = new string[0];

            try
            {
                objCore.ID = new Guid(arrParams[0].Trim());
                objCore.PACS_STATUS_ID = Convert.ToInt32(arrParams[1].Trim());
                objCore.USER_ID = new Guid(arrParams[2].Trim());
                objCore.MENU_ID = Convert.ToInt32(arrParams[3]);
                objCore.USER_SESSION_ID = new Guid(arrParams[4].Trim());

                bReturn = objCore.CheckRadiologistLock(Server.MapPath("~"), ref strRefresh, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[4];
                    arrRet[0] = "true";
                    arrRet[1] = strRefresh;
                    arrRet[2] = strReturnMessage.Trim();
                    arrRet[3] = objCore.USER_NAME;
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
                        arrRet = new string[5];
                        arrRet[0] = "false";
                        arrRet[1] = strRefresh;
                        arrRet[2] = strReturnMessage.Trim();
                        arrRet[3] = objCore.PACS_STATUS_DESC;
                        arrRet[4] = objCore.USER_NAME;
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

        #region AcquireRecordLock (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] AcquireRecordLock(string[] arrParams)
        {
            string strCatchMessage = ""; string strReturnMessage = "";
            bool bReturn = false;
            string strRefresh = "N";
            CommonFunctions objCF = new CommonFunctions();


            string[] arrRet = new string[0];

            try
            {
                objCF.RECORD_ID_UI = new Guid(arrParams[0].Trim());
                objCF.USER_ID = new Guid(arrParams[1].Trim());
                objCF.MENU_ID = Convert.ToInt32(arrParams[2]);
                objCF.USER_SESSION_ID = new Guid(arrParams[3].Trim());

                bReturn = objCF.AcquireRecordLockUI(Server.MapPath("~"), ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[1];
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
                        arrRet = new string[2];
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

        #region RadiologistSelfAssignmentSave (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] RadiologistSelfAssignmentSave(string[] arrParams,string[] arrSUID)
        {
            string strCatchMessage = ""; string strReturnMessage = "";
            bool bReturn = false;
            objCore = new Core.Case.CaseStudy();
            string[] arrRet = new string[0];
            Core.Case.StudyUIDList[] objList = new Core.Case.StudyUIDList[0];
            int intListIndex = 0; 

            try
            {
                objCore.RADIOLOGIST_ID = new Guid(arrParams[0].Trim());
                objCore.USER_ID = new Guid(arrParams[1].Trim());
                objCore.MENU_ID = Convert.ToInt32(arrParams[2]);
                objCore.USER_SESSION_ID = new Guid(arrParams[3].Trim());

                #region Populate Study UIDs
                objList = new Core.Case.StudyUIDList[(arrSUID.Length)];

                for (int i = 0; i < objList.Length; i++)
                {
                    objList[i] = new Core.Case.StudyUIDList();
                    objList[i].STUDY_UID = arrSUID[intListIndex].Trim();
                    intListIndex = intListIndex + 1;
                }
                #endregion

                intListIndex = 0;

                bReturn = objCore.RadiologistSelfAssignmentSave(Server.MapPath("~"),objList, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[1];
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
                        arrRet = new string[2];
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

        #region ReleaseStudyAssignment (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] ReleaseStudyAssignment(string[] arrParams)
        {
            string strCatchMessage = ""; string strReturnMessage = "";
            bool bReturn = false;
            objCore = new Core.Case.CaseStudy();
            string[] arrRet = new string[0];

            try
            {
                objCore.ID = new Guid(arrParams[0].Trim());
                objCore.USER_ID = new Guid(arrParams[1].Trim());
                objCore.MENU_ID = Convert.ToInt32(arrParams[2]);
                objCore.USER_SESSION_ID = new Guid(arrParams[3].Trim());

                bReturn = objCore.ReleaseStudyAssignment(Server.MapPath("~"), ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[1];
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
                        arrRet = new string[2];
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

        #region GetCase (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] GetCase(string[] arrParams)
        {
            string strCatchMessage = ""; string strReturnMessage = "";
            bool bReturn = false;
            objCore = new Core.Case.CaseStudy();
            string[] arrRet = new string[0];

            try
            {
                objCore.USER_ID = new Guid(arrParams[0].Trim());
                objCore.MENU_ID = Convert.ToInt32(arrParams[1]);
                objCore.USER_SESSION_ID = new Guid(arrParams[2].Trim());

                bReturn = objCore.GetCase(Server.MapPath("~"), ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[2];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMessage.Trim();

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
                        arrRet = new string[2];
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

        #region CopyFolder (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] CopyFolder(string[] ArrRecord)
        {
            bool bReturn = true;
            string[] arrRet = new string[0];
            string strArchFolderName = string.Empty;
            string strArchFolderPath = string.Empty;
            string strArchFolderAltPath = string.Empty;
            string strTgtFolderPath = string.Empty;
            string strTgtFolderName = string.Empty;
            string strPatientName = string.Empty;

            string strFileName = string.Empty;
            string strDCMMODIFYEXEPATH = string.Empty;
            string strVWINSTINFO = string.Empty;
            string strUserID = string.Empty;
            string[] arrayCode = new string[0];
            string[] arrFiles = new string[0];
            string[] arrFilesAlt = new string[0];
            string[] pathElements = new string[0];
            string strID = string.Empty;
            string strSUID = string.Empty;
            string strInstCode = string.Empty;
            string strInstName = string.Empty;
            string strPhysCode = string.Empty;
            string strSuffix = string.Empty;
            string strNewFileName = string.Empty;
            string strExtn = string.Empty;
            string strOutMsg = string.Empty;
            string strRetMsg = string.Empty;
            bool bArchFldrExists = false;
            bool bArchFldrAltExists = false;

            objComm = new classes.CommonClass();
            objCore = new Core.Case.CaseStudy();


            try
            {
                strID = ArrRecord[0].Trim();
                strSUID = ArrRecord[1].Trim();
                strInstCode = ArrRecord[2].Trim();
                strInstName = ArrRecord[3].Trim();
                strPhysCode = ArrRecord[4].Trim();
                strPatientName = ArrRecord[5].Trim();
                strArchFolderPath = ArrRecord[6].Trim();
                strArchFolderAltPath = ArrRecord[7].Trim();
                strDCMMODIFYEXEPATH = ArrRecord[8].Trim();
                strVWINSTINFO = ArrRecord[9].Trim();
                strUserID = ArrRecord[10].Trim();
                strArchFolderName = strInstCode + "_" + strInstName + "_" + strSUID;

              

                if (Directory.Exists(strArchFolderPath + "\\" + strArchFolderName)) bArchFldrExists = true;
                if (Directory.Exists(strArchFolderAltPath + "\\" + strArchFolderName)) bArchFldrAltExists = true;

                if (bArchFldrExists || bArchFldrAltExists)
                {
                    CreateUserDirectory(new Guid(strUserID));

                    strTgtFolderName = strPatientName;
                    strTgtFolderName = strTgtFolderName.Replace(" ", "_");
                    strTgtFolderName = strTgtFolderName.Replace("(", "");
                    strTgtFolderName = strTgtFolderName.Replace(")", "");
                    strTgtFolderName = strTgtFolderName.Replace("'", "");
                    strTgtFolderName = strTgtFolderName.Replace("\"", "");
                    strTgtFolderName = strTgtFolderName.Replace("\\", "_");
                    strTgtFolderName = strTgtFolderName.Replace("/", "_");
                    strTgtFolderName = strTgtFolderName.Replace("#", "");
                    strTgtFolderName = strTgtFolderName.Replace("&", "");
                    strTgtFolderName = strTgtFolderName.Replace("@", "");
                    strTgtFolderName = strTgtFolderName.Replace("?", "");
                    strTgtFolderName = strTgtFolderName.Replace(",", "");
                    strTgtFolderName = strTgtFolderName.Replace("__", "_");

                    strSuffix = CoreCommon.RandomString(6);
                    strTgtFolderName = strTgtFolderName + "_" + strSuffix;
                    strTgtFolderPath = Server.MapPath("~/CaseList/Temp/" + strUserID + "/" + strTgtFolderName);
                    strTgtFolderPath = strTgtFolderPath.Replace("ajaxpro\\", "");


                    if (!Directory.Exists(strTgtFolderPath)) { Directory.CreateDirectory(strTgtFolderPath); }

                    if (bArchFldrExists) arrFiles = Directory.GetFiles(strArchFolderPath + "\\" + strArchFolderName);
                    if (bArchFldrAltExists) arrFilesAlt = Directory.GetFiles(strArchFolderAltPath + "\\" + strArchFolderName);

                    #region Files In Archive Path
                    if (arrFiles.Length > 0)
                    {
                        for (int i = 0; i < arrFiles.Length; i++)
                        {
                            strExtn = Path.GetExtension(arrFiles[i]);
                            pathElements = arrFiles[i].Split('\\');
                            strFileName = pathElements[(pathElements.Length - 1)];
                            strNewFileName = strPatientName + "_" + (i + 1).ToString();
                            if (strExtn.Trim() != string.Empty) strNewFileName = strNewFileName + strExtn;


                            if (File.Exists(strTgtFolderPath + "/" + strNewFileName)) File.Delete(strTgtFolderPath + "/" + strNewFileName);
                            File.Copy(arrFiles[i], strTgtFolderPath + "/" + strNewFileName);

                            if (strVWINSTINFO == "N")
                            {
                                ModifyDCMFile(strDCMMODIFYEXEPATH, strInstCode, strPhysCode, strTgtFolderPath + "/" + strNewFileName, ref strOutMsg, ref strRetMsg);
                            }

                            if (File.Exists(strTgtFolderPath + "/" + strNewFileName + ".bak")) File.Delete(strTgtFolderPath + "/" + strNewFileName + ".bak");
                        }
                    }
                    #endregion

                    #region Files In Alt. Archive Path
                    if (arrFilesAlt.Length > 0)
                    {
                        for (int i = 0; i < arrFilesAlt.Length; i++)
                        {
                            strExtn = Path.GetExtension(arrFilesAlt[i]);
                            pathElements = arrFilesAlt[i].Split('\\');
                            strFileName = pathElements[(pathElements.Length - 1)];
                            strNewFileName = strPatientName + "_" + (i + 1).ToString();
                            if (strExtn.Trim() != string.Empty) strNewFileName = strNewFileName + strExtn;


                            if (File.Exists(strTgtFolderPath + "/" + strNewFileName)) File.Delete(strTgtFolderPath + "/" + strNewFileName);
                            File.Copy(arrFilesAlt[i], strTgtFolderPath + "/" + strNewFileName);

                            if (strVWINSTINFO == "N")
                            {
                                ModifyDCMFile(strDCMMODIFYEXEPATH, strInstCode, strPhysCode, strTgtFolderPath + "/" + strNewFileName, ref strOutMsg, ref strRetMsg);
                            }

                            if (File.Exists(strTgtFolderPath + "/" + strNewFileName + ".bak")) File.Delete(strTgtFolderPath + "/" + strNewFileName + ".bak");
                        }
                    }
                    #endregion

                    if (arrFiles.Length == 0 && arrFilesAlt.Length == 0)
                    {
                        bReturn = false;
                        arrRet = new string[4];
                        arrRet[0] = "false";
                        arrRet[1] = "469";
                        arrRet[2] = strID;
                        arrRet[3] = strPatientName;
                    }
                    else
                    {
                        arrRet = new string[4];
                        arrayCode = new string[1];
                        arrayCode[0] = "402";
                        arrRet[0] = "true";
                        arrRet[1] = strID;
                        arrRet[2] = strPatientName;
                        arrRet[3] = strTgtFolderName;
                    }
                }
                else
                {
                    bReturn = false;
                    arrRet = new string[4];
                    arrRet[0] = "false";
                    arrRet[1] = "468";
                    arrRet[2] = strID;
                    arrRet[3] = strPatientName;
                }
            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrRet = new string[4];
                arrRet[0] = "catch";
                arrRet[1] = "Patient : " + ArrRecord[5].Trim() + "..." + expErr.Message;
                arrRet[2] = ArrRecord[0].Trim();
                arrRet[3] = ArrRecord[5].Trim();
            }
            finally
            {
                objCore = null; objComm = null;
            }
            return arrRet;
        }
        #endregion

        #region CompressFolder (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] CompressFolder(string[] ArrRecord)
        {
            bool bReturn = true;
            string[] arrRet = new string[0];
            string strTgtFolderName = string.Empty;
            string strTgtFolderPath = string.Empty;
            string strZipPath = string.Empty;
            string strFileName = string.Empty;
            string strID = string.Empty;
            string strPatientName = string.Empty;
            string strUserID = string.Empty;
            string[] arrayCode = new string[0];
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];

            objComm = new classes.CommonClass();
            objCore = new Core.Case.CaseStudy();


            try
            {
                strID = ArrRecord[0].Trim();
                strPatientName = ArrRecord[1].Trim();
                strTgtFolderName = ArrRecord[2].Trim();
                strUserID = ArrRecord[3].Trim();

                strTgtFolderPath = Server.MapPath("~/CaseList/Temp/" + strUserID + "/" + strTgtFolderName);
                strTgtFolderPath = strTgtFolderPath.Replace("ajaxpro\\", "");
                strZipPath = strTgtFolderPath + ".zip";

                if (Directory.Exists(strTgtFolderPath))
                {
                    ZipFile.CreateFromDirectory(strTgtFolderPath, strZipPath);

                    arrFiles = Directory.GetFiles(strTgtFolderPath);

                    if (arrFiles.Length > 0)
                    {
                        for (int i = 0; i < arrFiles.Length; i++)
                        {
                            File.Delete(arrFiles[i]);
                        }
                        if (Directory.GetFiles(strTgtFolderPath).Length == 0)
                        {
                            Directory.Delete(strTgtFolderPath);
                        }

                    }


                    arrRet = new string[4];
                    arrayCode = new string[1];
                    arrayCode[0] = "403";
                    arrRet[0] = "true";
                    //arrRet[1] = objComm.SetErrorResources(arrayCode, "N", false, string.Empty, string.Empty);
                    arrRet[1] = strTgtFolderName + ".zip";
                    arrRet[2] = strID;
                    arrRet[3] = strPatientName;
                }
                else
                {
                    bReturn = false;
                    arrRet = new string[4];
                    arrRet[0] = "false";
                    arrRet[1] = "470";
                    arrRet[2] = strID;
                    arrRet[3] = strPatientName;
                }
            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrRet = new string[4];
                arrRet[0] = "catch";
                arrRet[1] = "Patient : " + ArrRecord[5].Trim() + "..." + expErr.Message;
                arrRet[2] = ArrRecord[0].Trim();
                arrRet[3] = ArrRecord[5].Trim();
            }
            finally
            {
                objCore = null; objComm = null;
            }
            return arrRet;
        }
        #endregion
        
        #region ModifyDCMFile
        private bool ModifyDCMFile(string strDCMMODIFYEXEPATH, string strInsName, string strPhysName, string strDCMPath, ref string strOutputMsg, ref string strReturnMessage)
        {
            bool bRet = false;
            string strProcOutput = string.Empty;
            string strProcError = string.Empty;

            /*
             * (0008,0080) Institution
             * (0008,0081) Institution Address
             * (0008,0090) Referring Physician
             * (0008,1050) Performing Physician
             */

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
                bRet = false;
                strReturnMessage = ex.Message.Trim();
            }

            if (bRet)
            {
                try
                {
                    Process ProcModInstAddr = new Process();
                    ProcModInstAddr.StartInfo.UseShellExecute = false;
                    ProcModInstAddr.StartInfo.FileName = strDCMMODIFYEXEPATH;
                    ProcModInstAddr.StartInfo.Arguments = "-i \"(0008,0081)=" + string.Empty + "\"" + " " + strDCMPath;
                    ProcModInstAddr.StartInfo.RedirectStandardOutput = true;
                    ProcModInstAddr.StartInfo.RedirectStandardError = true;
                    ProcModInstAddr.Start();
                    strProcOutput = ProcModInstAddr.StandardOutput.ReadToEnd();
                    strProcError = ProcModInstAddr.StandardError.ReadToEnd();
                    strOutputMsg = strProcOutput.Trim();
                    bRet = true;


                }
                catch (Exception ex)
                {
                    bRet = false;
                    strReturnMessage = ex.Message.Trim();
                }
            }


            if (bRet)
            {
                try
                {
                    Process ProcModRefPhys = new Process();
                    ProcModRefPhys.StartInfo.UseShellExecute = false;
                    ProcModRefPhys.StartInfo.FileName = strDCMMODIFYEXEPATH;
                    ProcModRefPhys.StartInfo.Arguments = "-i \"(0008,0090)=" + strPhysName + "\"" + " " + strDCMPath;
                    ProcModRefPhys.StartInfo.RedirectStandardOutput = true;
                    ProcModRefPhys.StartInfo.RedirectStandardError = true;
                    ProcModRefPhys.Start();
                    strProcOutput = ProcModRefPhys.StandardOutput.ReadToEnd();
                    strProcError = ProcModRefPhys.StandardError.ReadToEnd();
                    strOutputMsg = strProcOutput.Trim();
                    bRet = true;


                }
                catch (Exception ex)
                {
                    bRet = false;
                    strReturnMessage = ex.Message.Trim();
                }

            }

            if (bRet)
            {
                try
                {
                    Process ProcModPerfPhys = new Process();
                    ProcModPerfPhys.StartInfo.UseShellExecute = false;
                    ProcModPerfPhys.StartInfo.FileName = strDCMMODIFYEXEPATH;
                    ProcModPerfPhys.StartInfo.Arguments = "-i \"(0008,1050)=" + strPhysName + "\"" + " " + strDCMPath;
                    ProcModPerfPhys.StartInfo.RedirectStandardOutput = true;
                    ProcModPerfPhys.StartInfo.RedirectStandardError = true;
                    ProcModPerfPhys.Start();
                    strProcOutput = ProcModPerfPhys.StandardOutput.ReadToEnd();
                    strProcError = ProcModPerfPhys.StandardError.ReadToEnd();
                    strOutputMsg = strProcOutput.Trim();
                    bRet = true;


                }
                catch (Exception ex)
                {
                    bRet = false;
                    strReturnMessage = ex.Message.Trim();
                }

            }
            return bRet;
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

        #region CheckFileCount (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] CheckFileCount(string[] ArrRecord)
        {
            bool bReturn = true;
            string[] arrRet = new string[0];
            string strArchFolderPath = string.Empty;
            string strArchFolderAltPath = string.Empty;
            string strArchFolderName = string.Empty;
            string strPatientName = string.Empty;

            string strUserID = string.Empty;
            string[] arrayCode = new string[0];
            string[] arrFiles = new string[0];
            string[] arrFilesAlt = new string[0];

            string strID = string.Empty;
            string strSUID = string.Empty;
            string strInstCode = string.Empty;
            string strInstName = string.Empty;
           
            bool bArchFldrExists = false;
            bool bArchFldrAltExists = false;
            int intActualFileCount = 0;

            objComm = new classes.CommonClass();
            objCore = new Core.Case.CaseStudy();

            string strReturnMessage = string.Empty;
            string strCatchMessage = string.Empty;


            try
            {
                strID = ArrRecord[0].Trim();
                strSUID = ArrRecord[1].Trim();
                strInstCode = ArrRecord[2].Trim();
                strInstName = ArrRecord[3].Trim();
                strPatientName = ArrRecord[4].Trim();
                strArchFolderPath = ArrRecord[5].Trim();
                strArchFolderAltPath = ArrRecord[6].Trim();
                strArchFolderName = strInstCode + "_" + strInstName + "_" + strSUID;

                if (Directory.Exists(strArchFolderPath + "\\" + strArchFolderName)) bArchFldrExists = true;
                if (Directory.Exists(strArchFolderAltPath + "\\" + strArchFolderName)) bArchFldrAltExists = true;

                if (bArchFldrExists || bArchFldrAltExists)
                {
                    
                    if (bArchFldrExists) arrFiles = Directory.GetFiles(strArchFolderPath + "\\" + strArchFolderName);
                    if (bArchFldrAltExists) arrFilesAlt = Directory.GetFiles(strArchFolderAltPath + "\\" + strArchFolderName);

                    intActualFileCount = arrFiles.Length + arrFilesAlt.Length;

                    if (intActualFileCount == 0)
                    {
                        bReturn = false;
                        arrRet = new string[4];
                        arrRet[0] = "false";
                        arrRet[1] = "469";
                        arrRet[2] = strID;
                        arrRet[3] = strPatientName;
                    }
                    else
                    {

                        objCore.ID = new Guid (strID);
                        objCore.ACTUAL_FILE_COUNT = intActualFileCount;

                        bReturn = objCore.CheckArchivedFiles(Server.MapPath("~"), ref strReturnMessage, ref strCatchMessage);

                        if (bReturn)
                        {
                            arrRet = new string[5];
                            arrRet[0] = "true";
                            arrRet[1] = strReturnMessage;
                            arrRet[2] = strID;
                            arrRet[3] = strPatientName;
                            arrRet[4] = objCore.PENDING_FILE_COUNT.ToString();
                        }
                        else
                        {
                            arrRet = new string[4];
                            arrRet[0] = "false";
                            if (strCatchMessage.Trim() != string.Empty)
                                arrRet[1] = strCatchMessage;
                            else
                                arrRet[1] = strReturnMessage;
                            arrRet[2] = strID;
                            arrRet[3] = strPatientName;
                        }

                        
                    }
                }
                else
                {
                    bReturn = false;
                    arrRet = new string[4];
                    arrRet[0] = "false";
                    arrRet[1] = "468";
                    arrRet[2] = strID;
                    arrRet[3] = strPatientName;
                }
            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrRet = new string[4];
                arrRet[0] = "catch";
                arrRet[1] = "Patient : " + strPatientName + "..." + expErr.Message;
                arrRet[2] = strID;
                arrRet[3] = strPatientName;
            }
            finally
            {
                objCore = null; objComm = null;
            }
            return arrRet;
        }
        #endregion
    }
}