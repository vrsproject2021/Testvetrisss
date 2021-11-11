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
using System.Net;
using VETRIS.Core;
using eRADCls;
using VETRIS.Core.Radiologist;
using GemBox.Spreadsheet;
using System.Drawing;

namespace VETRIS.CaseList
{
    [AjaxPro.AjaxNamespace("VRSCaseFinalRptBrw")]
    public partial class VRSCaseFinalRptBrw : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Case.CaseStudy objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSCaseFinalRptBrw));
            if (!CallBackBrw.CausedCallback)
                SetPageValue();
            SetAttributes();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {

            btnRefresh.Attributes.Add("onclick", "javascript:ResetRecord();view_Searchform();");
            btnSearch.Attributes.Add("onclick", "javascript:onSearch();");
            btnClose.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");
            ddlInstitution.Attributes.Add("onchange", "javascript:ddlInstitution_OnChange();");
            txtFromDt.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgFrom.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtFromDt.ClientID + "','CalFrom');");
            txtTillDt.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgTill.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtTillDt.ClientID + "','CalTill');");
            chkShowAbRpt.Attributes.Add("onclick", "javascript:chkShowAbRpt_OnClick();");
            btnExcel.Attributes.Add("onclick", "javascript:btnExcel_OnClick();");
            btnDV.Attributes.Add("onclick", "javascript:btnDV_OnClick();");
            ddlDatabase.Attributes.Add("onchange", "javascript:ddlDatabase_OnChange();");
            //ddlDatabase.Attributes.Add("onchange", "javascript:ddlDatabase_onchange();");

        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"].ToString());
            string strID = string.Empty;
            string strTheme = Request.QueryString["th"];
            SetCSS(strTheme);

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
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkCAL.Attributes["href"] = strServerPath + "/css/" + strTheme + "/CalendarStyle.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
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
            string strCatchMessage = ""; bool bReturn = false; int intCnt = 0;
            string strControlCode = string.Empty;
            DataSet ds = new DataSet();
            RadWebClass client = new RadWebClass();
            string strSession = string.Empty;
            string strErr = string.Empty;
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

                    #region Category
                    DataRow dr4 = ds.Tables["Category"].NewRow();
                    dr4["id"] = "0";
                    dr4["name"] = "Select One";
                    ds.Tables["Category"].Rows.InsertAt(dr4, 0);
                    ds.Tables["Category"].AcceptChanges();

                    ddlCategory.DataSource = ds.Tables["Category"];
                    ddlCategory.DataValueField = "id";
                    ddlCategory.DataTextField = "name";
                    ddlCategory.DataBind();
                    #endregion

                    #region API Params
                    foreach (DataRow dr in ds.Tables["APIParams"].Rows)
                    {
                        strControlCode = Convert.ToString(dr["control_code"]).Trim();
                        switch (strControlCode)
                        {
                            case "APIVER":
                                hdnAPIVER.Value = Convert.ToString(dr["data_type_string"]).Trim();
                                break;
                            case "WS8SRVIP":
                                hdnWS8SRVIP.Value = Convert.ToString(dr["data_type_string"]).Trim();
                                break;
                            case "WS8CLTIP":
                                hdnWS8CLTIP.Value = Convert.ToString(dr["data_type_string"]).Trim();
                                break;
                            case "WS8SRVUID":
                                hdnWS8SRVUID.Value = Convert.ToString(dr["data_type_string"]).Trim();
                                break;
                            case "WS8SRVPWD":
                                hdnWS8SRVPWD.Value = CoreCommon.DecryptString(Convert.ToString(dr["data_type_string"]).Trim());
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion

                    #region Status
                    DataRow dr5 = ds.Tables["Status"].NewRow();
                    dr5["status_id"] = "-1";
                    dr5["status_desc"] = "Select One";
                    ds.Tables["Status"].Rows.InsertAt(dr5, 0);
                    ds.Tables["Status"].AcceptChanges();

                    ddlStatus.DataSource = ds.Tables["Status"];
                    ddlStatus.DataValueField = "status_id";
                    ddlStatus.DataTextField = "status_desc";
                    ddlStatus.DataBind();
                    #endregion

                    #region Radiologist
                    intCnt = ds.Tables["Radiologists"].Rows.Count;
                    DataRow dr6 = ds.Tables["Radiologists"].NewRow();
                    dr6["id"] = "00000000-0000-0000-0000-000000000000";
                    dr6["name"] = "Select One";
                    ds.Tables["Radiologists"].Rows.InsertAt(dr6, 0);
                    ds.Tables["Radiologists"].AcceptChanges();

                    ddlRadiologist.DataSource = ds.Tables["Radiologists"];
                    ddlRadiologist.DataValueField = "id";
                    ddlRadiologist.DataTextField = "name";
                    ddlRadiologist.DataBind();

                    //if (intCnt == 1)
                    //{
                    //    ddlApprovedBy.SelectedIndex = 1;
                    //}
                    #endregion

                    #region Final Radiologist
                    intCnt = ds.Tables["FinalRadiologists"].Rows.Count;
                    DataRow dr7 = ds.Tables["FinalRadiologists"].NewRow();
                    dr7["id"] = "00000000-0000-0000-0000-000000000000";
                    dr7["name"] = "Select One";
                    ds.Tables["FinalRadiologists"].Rows.InsertAt(dr7, 0);
                    ds.Tables["FinalRadiologists"].AcceptChanges();

                    ddlApprovedBy.DataSource = ds.Tables["FinalRadiologists"];
                    ddlApprovedBy.DataValueField = "id";
                    ddlApprovedBy.DataTextField = "name";
                    ddlApprovedBy.DataBind();
                    #endregion

                    if (objCore.PAS_USER_PASSWORD != string.Empty)
                    {
                        hdnPACSCred.Value = CoreCommon.EncryptString(objCore.PACS_USER_ID + "±" + objCore.PAS_USER_PASSWORD);
                    }

                    #region Abnormal Report Reasons
                    intCnt = ds.Tables["AbnormalRptReasons"].Rows.Count;
                    DataRow dr8 = ds.Tables["AbnormalRptReasons"].NewRow();
                    dr8["id"] = "00000000-0000-0000-0000-000000000000";
                    dr8["reason"] = "Select One";
                    ds.Tables["AbnormalRptReasons"].Rows.InsertAt(dr8, 0);
                    ds.Tables["AbnormalRptReasons"].AcceptChanges();

                    ddlAbRptReason.DataSource = ds.Tables["AbnormalRptReasons"];
                    ddlAbRptReason.DataValueField = "id";
                    ddlAbRptReason.DataTextField = "reason";
                    ddlAbRptReason.DataBind();
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
                            default:
                                break;
                        }

                    }
                    #endregion

                    #region Database List
                    DataRow ddl = ds.Tables["DatabaseList"].NewRow();
                    ddl["db_year"] = DateTime.Now.Date.Year;
                    ddl["db_name"] = "Current Year";
                    ds.Tables["DatabaseList"].Rows.InsertAt(ddl, 0);
                    ds.Tables["DatabaseList"].AcceptChanges();

                    ddlDatabase.DataSource = ds.Tables["DatabaseList"];
                    ddlDatabase.DataValueField = "db_name";
                    ddlDatabase.DataTextField = "db_year";
                    ddlDatabase.DataBind();
                    #endregion
                }
                else
                    hdnError.Value = strCatchMessage.Trim();

                #region Physician
                ListItem objLI = new ListItem();
                objLI.Value = "00000000-0000-0000-0000-000000000000";
                objLI.Text = "Select One";
                ddlPhys.Items.Add(objLI);
                #endregion

            }
            catch (Exception ex)
            {
                hdnError.Value = ex.Message.Trim();
            }
            finally
            {
                ds.Dispose();
                objCore = null;
                client = null;
            }

        }
        #endregion

        #region CallBackBrw_Callback
        protected void CallBackBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            SearchRecord(e.Parameters);
            grdBrw.Width = Unit.Percentage(100);
            grdBrw.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region SearchRecord
        private void SearchRecord(string[] arrParams)
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            string strUserRole = string.Empty;
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.PATIENT_NAME = arrParams[0].Trim();
                objCore.MODALITY_ID = Convert.ToInt32(arrParams[1]);
                objCore.FILTER_BY_RECEIVED_DATE = arrParams[2].Trim();
                objCore.RECEIVED_DATE_FROM = Convert.ToDateTime(arrParams[3]);
                objCore.RECEIVED_DATE_TILL = Convert.ToDateTime(arrParams[4]);
                objCore.INSTITUTION_ID = new Guid(arrParams[5]);
                objCore.PHYSICIAN_ID = new Guid(arrParams[6]);
                objCore.CATEGORY_ID = Convert.ToInt32(arrParams[7]);
                objCore.FINAL_RADIOLOGIST_ID = new Guid(arrParams[8].Trim());
                objCore.RADIOLOGIST_ID = new Guid(arrParams[9].Trim());
                objCore.SHOW_ABNORMAL_REPORTS = arrParams[10].Trim();
                objCore.ABNORMAL_REPORT_REASON_ID = new Guid(arrParams[11].Trim());
                objCore.FINAL_REPORT_RELEASE_PENDING = arrParams[12].Trim();
                objCore.PACS_STATUS_ID = Convert.ToInt32(arrParams[13]);
                objCore.MARKED_FOR_TEACHING = arrParams[14].Trim();
                objCore.USER_ID = new Guid(arrParams[15].Trim());
                objCore.MENU_ID = Convert.ToInt32(arrParams[16]);
                objCore.USER_SESSION_ID = new Guid(arrParams[17].Trim());
                strUserRole = arrParams[18].Trim();
                objCore.PageSize = Convert.ToInt32(arrParams[19]);
                objCore.PageNo = Convert.ToInt32(arrParams[20]);
                objCore.DATABASE_NAME = Convert.ToString(arrParams[21]);
                objCore.STUDY_UID = arrParams[23].Trim();

                bReturn = objCore.SearchFinalReportBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    if (arrParams[10].Trim() == "Y")
                    {
                        DataView dv = new DataView(ds.Tables["BrowserList"]);
                        dv.RowFilter = "rating='A'";
                        grdBrw.DataSource = dv.ToTable();
                        dv.Dispose();
                    }
                    else
                    {
                        grdBrw.DataSource = ds.Tables["BrowserList"];
                    }

                    grdBrw.Levels[0].Columns[3].FormatString = objComm.DateFormat + " HH:mm";
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

            #region Set Column Accessibility
            if ((strUserRole == "SUPP"))
            {
                grdBrw.Levels[0].Columns[1].Visible = true;//study_uid
                grdBrw.Levels[0].Columns[8].Visible = true;//radiologist_pacs
                grdBrw.Levels[0].Columns[9].Visible = true;//final_radiologist
                grdBrw.Levels[0].Columns[14].Visible = true;//PROMO
                grdBrw.Levels[0].Columns[15].Visible = true;//RADACTION
                grdBrw.Levels[0].Columns[20].Visible = true;//rating_reason
            }
            else if ((strUserRole == "SYSADMIN"))
            {
                grdBrw.Levels[0].Columns[8].Visible = true;//radiologist_pacs
                grdBrw.Levels[0].Columns[9].Visible = true;//final_radiologist
                grdBrw.Levels[0].Columns[14].Visible = true;//PROMO
                grdBrw.Levels[0].Columns[15].Visible = true;//RADACTION
                grdBrw.Levels[0].Columns[20].Visible = true;//rating_reason
            }
            else if ((strUserRole == "RDL"))
            {
                grdBrw.Levels[0].Columns[5].Visible = false;//category_name
                grdBrw.Levels[0].Columns[7].Visible = false;//physician_name
                grdBrw.Levels[0].Columns[8].Visible = true;//radiologist_pacs
                grdBrw.Levels[0].Columns[9].Visible = true;//final_radiologist
                grdBrw.Levels[0].Columns[14].Visible = false;//PROMO
                grdBrw.Levels[0].Columns[15].Visible = true; ;//RADACTION
                grdBrw.Levels[0].Columns[20].Visible = true;//rating_reason
            }
            #endregion
        }
        #endregion

        #region FetchPhysicians (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchPhysicians(string[] arrParams)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            string strViewInstInfo = string.Empty;
            DataSet ds = new DataSet();
            objCore = new Core.Case.CaseStudy();

            int i = 0;
            string[] arrRet = new string[0];

            try
            {

                objCore.INSTITUTION_ID = new Guid(arrParams[0].Trim());
                strViewInstInfo = arrParams[1].Trim();

                bReturn = objCore.FetchInstitutionWisePhysician(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    if (ds.Tables["Physicians"].Rows.Count > 0)
                    {

                        arrRet = new string[(ds.Tables["Physicians"].Rows.Count * 2) + 3];
                        arrRet[0] = "true";
                        arrRet[1] = "00000000-0000-0000-0000-000000000000";
                        arrRet[2] = "Select One";
                        i = 3;

                        foreach (DataRow dr in ds.Tables["Physicians"].Rows)
                        {
                            if (strViewInstInfo == "Y")
                            {
                                arrRet[i] = Convert.ToString(dr["id"]);
                                arrRet[i + 1] = Convert.ToString(dr["name"]).Trim();
                            }
                            else
                            {
                                arrRet[i] = Convert.ToString(dr["id"]);
                                arrRet[i + 1] = Convert.ToString(dr["code"]).Trim();
                            }
                            i = i + 2;
                        }
                    }
                    else
                    {
                        arrRet = new string[3];
                        arrRet[0] = "true";
                        arrRet[1] = "00000000-0000-0000-0000-000000000000";
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
                objCore.MENU_ID = Convert.ToInt32(arrParams[3].Trim());
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
                        arrRet = new string[4];
                        arrRet[0] = "false";
                        arrRet[1] = strRefresh;
                        arrRet[2] = strReturnMessage.Trim();
                        arrRet[3] = objCore.PACS_STATUS_DESC;
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
            string[] arrDirs = new string[0];
            string[] arrDirsAlt = new string[0];
            string[] arrFilesTmp = new string[0];
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
            int intArrLength = 0;
            int intIdx = 0;

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

        #region ReleaseReport (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] ReleaseReport(string[] ArrRecord)
        {
            bool bReturn = false;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            int intCheckReleaseTime = 0;
            objCore = new Core.Case.CaseStudy();

            try
            {
                objCore.ID = new Guid(ArrRecord[0]);
                objCore.USER_ID = new Guid(ArrRecord[1]);
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[2]);
                objCore.USER_SESSION_ID = new Guid(ArrRecord[3]);

                bReturn = objCore.ReleaseFinalReport(Server.MapPath("~"), ref intCheckReleaseTime, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[3];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
                    arrRet[2] = Convert.ToString(intCheckReleaseTime);
                }
                else
                {
                    arrRet = new string[2];
                    if (strCatchMessage.Trim() != "")
                    {
                        arrRet[0] = "catch";
                        arrRet[1] = strCatchMessage.Trim();
                    }
                    else
                    {
                        arrRet[0] = "false";
                        arrRet[1] = strReturnMsg.Trim();
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

        #region RevertArchiveStudy (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] RevertArchiveStudy(string[] ArrRecord)
        {
            bool bReturn = false;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            objCore = new Core.Case.CaseStudy();

            try
            {
                objCore.ID = new Guid(ArrRecord[0]);
                objCore.STUDY_UID = ArrRecord[1].Trim();
                objCore.USER_ID = new Guid(ArrRecord[2]);

                bReturn = objCore.RevertArchiveStudy(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[2];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
                }
                else
                {
                    arrRet = new string[2];
                    if (strCatchMessage.Trim() != "")
                    {
                        arrRet[0] = "catch";
                        arrRet[1] = strCatchMessage.Trim();
                    }
                    else
                    {
                        arrRet[0] = "false";
                        arrRet[1] = strReturnMsg.Trim();
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

        #region Delete Study (Ajaxpro Methods)
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

                    if (strRecByRouter == "N")
                    {
                        objCore.ID = new Guid(ArrRecord[0]);
                        objCore.STUDY_UID = ArrRecord[1].Trim();
                        objCore.USER_ID = UserID = new Guid(ArrRecord[3]);
                        objCore.MENU_ID = Convert.ToInt32(ArrRecord[4]);
                        objCore.USER_SESSION_ID = new Guid(ArrRecord[5]);

                        bReturn = objCore.DeleteArchiveStudy(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

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
                                arrRet = new string[2];
                                arrRet[0] = "false";
                                arrRet[1] = strError.Trim();
                            }
                        }
                    }
                    #endregion
                }
                else if (strRecByRouter == "Y")
                {
                    #region if DICOM ROUTER == MANUAL
                    objCore.ID = new Guid(ArrRecord[0]);
                    objCore.STUDY_UID = ArrRecord[1].Trim();
                    objCore.USER_ID = UserID = new Guid(ArrRecord[3]);
                    objCore.USER_SESSION_ID = UserID = new Guid(ArrRecord[5]);

                    bReturn = objCore.DeleteArchiveStudy(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

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
                            arrRet = new string[2];
                            arrRet[0] = "false";
                            arrRet[1] = strReturnMsg.Trim();
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

        #region SetLicenseKey
        private void SetLicenseKey()
        {
            try
            {
                VETRIS.Core.CoreCommon.GetReportLicenseKey(Server.MapPath("~"));
                SpreadsheetInfo.SetLicense(CoreCommon.REPORT_LICENSE_KEY);

            }
            catch (Exception expErr)
            {
                Response.Write(expErr.Message);
            }
        }
        #endregion

        #region GenerateExcel(Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] GenerateExcel(string[] arrParams)
        {
            objCore = new Core.Case.CaseStudy();
            string[] arrRet = new string[0];
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            string strRptFile = string.Empty;
            string strUserRole = string.Empty;


            try
            {
                objCore.PATIENT_NAME = arrParams[0].Trim();
                objCore.MODALITY_ID = Convert.ToInt32(arrParams[1]);
                objCore.FILTER_BY_RECEIVED_DATE = arrParams[2].Trim();
                objCore.RECEIVED_DATE_FROM = Convert.ToDateTime(arrParams[3]);
                objCore.RECEIVED_DATE_TILL = Convert.ToDateTime(arrParams[4]);
                objCore.INSTITUTION_ID = new Guid(arrParams[5]);
                objCore.PHYSICIAN_ID = new Guid(arrParams[6]);
                objCore.CATEGORY_ID = Convert.ToInt32(arrParams[7]);
                objCore.FINAL_RADIOLOGIST_ID = new Guid(arrParams[8].Trim());
                objCore.RADIOLOGIST_ID = new Guid(arrParams[9].Trim());
                objCore.SHOW_ABNORMAL_REPORTS = arrParams[10].Trim();
                objCore.ABNORMAL_REPORT_REASON_ID = new Guid(arrParams[11].Trim());
                objCore.FINAL_REPORT_RELEASE_PENDING = arrParams[12].Trim();
                objCore.PACS_STATUS_ID = Convert.ToInt32(arrParams[13]);
                objCore.USER_ID = new Guid(arrParams[14].Trim());
                strUserRole = arrParams[15].Trim();
                objCore.DATABASE_NAME = Convert.ToString(arrParams[16]);
                bReturn = objCore.FetchFinalReportListExcelRecords(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    
                    strRptFile = CreateReportExcel(ds, objCore, arrParams[14].Trim(), arrParams[10].Trim() == "Y", ref strCatchMessage);
                    if (strCatchMessage.Trim() == string.Empty)
                    {
                        arrRet = new string[4];
                        arrRet[0] = "true";
                        arrRet[1] = "CaseList/Temp/" + arrParams[14].Trim() + "/" + strRptFile;
                        arrRet[2] = Server.MapPath("CaseList/Temp/" + arrParams[14].Trim() + "/" + strRptFile).Replace("ajaxpro\\", "");
                        arrRet[3] = "XLS";
                    }
                    else
                    {
                        arrRet = new string[2];
                        arrRet[0] = "catch";
                        arrRet[1] = strCatchMessage;
                    }
                }
                else
                {
                    arrRet = new string[2];
                    arrRet[0] = "false";
                    arrRet[1] = strCatchMessage;
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";
                }

            }
            catch (Exception ex)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = ex.Message.Trim();
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
            return arrRet;
        }
        #endregion

        #region CreateReportExcel
        public string CreateReportExcel(DataSet ds, Core.Case.CaseStudy core, string strUserID, bool isAbNormal, ref string CatchMessage)
        {
            string strReportName = string.Empty;
            bool bReturn = false;
            SetLicenseKey();
            ExcelFile objExcelFile = null; string strRowfiltertring = string.Empty;
            ExcelWorksheet sheet = null;
            //ExcelWorksheet objExcelWorksheet2 = null;
            string strRName = "FinalReport_" + (isAbNormal ? "Abnormal_" : "") + string.Format("{0:yyyy_MM_dd_HH_mm_ss}", DateTime.Now) + ".xlsx";

            try
            {

                if (!System.IO.Directory.Exists(Server.MapPath("~") + "/CaseList/Temp/" + strUserID)) { System.IO.Directory.CreateDirectory(Server.MapPath("~") + "/CaseList/Temp/" + strUserID); }
                strReportName = Server.MapPath("~") + "/CaseList/Temp/" + strUserID + "/" + strRName;
                strReportName = strReportName.Replace("ajaxpro\\", "");
                if (System.IO.File.Exists(strReportName) == true) { System.IO.File.Delete(strReportName); }

                objExcelFile = new ExcelFile();
                sheet = objExcelFile.Worksheets.Add("Final Report" + (isAbNormal ? " (Abnormal)" : ""));

                int intRowIndex = 0;


                #region Details
                DataTable tbl = null;
                if (isAbNormal)
                {
                    DataView dv = new DataView(ds.Tables["BrowserList"]);
                    dv.RowFilter = "rating='A'";
                    tbl = dv.ToTable();
                    dv.Dispose();
                }
                else
                {
                    tbl = ds.Tables["BrowserList"];
                }

                var data = tbl.TableToList<VETRIS.Core.Case.ReportData>();

                #endregion

                CreateHeader(ref intRowIndex, sheet);
                foreach (var item in data)
                {
                    CreateBodyRow(ref intRowIndex, item, sheet);
                }
                sheet.Columns[0].AutoFit();
                sheet.Columns[1].AutoFit();
                sheet.Columns[2].AutoFit();
                sheet.Columns[3].AutoFit();
                sheet.Columns[4].AutoFit();
                sheet.Columns[5].AutoFit();
                sheet.Columns[6].AutoFit();
                sheet.Columns[7].AutoFit();
                sheet.Columns[8].AutoFit();
                sheet.Columns[9].AutoFit();
                sheet.Columns[10].AutoFit();
                sheet.Columns[11].AutoFit();
                sheet.Columns[12].AutoFit();
                sheet.Columns[13].AutoFit();
                sheet.Columns[14].AutoFit();
                sheet.Columns[15].AutoFit();

                var printOptions = sheet.PrintOptions;
                printOptions.Portrait = false;
                printOptions.LeftMargin = 0.50;
                printOptions.RightMargin = 0.3;
                printOptions.TopMargin = 0.3;
                printOptions.BottomMargin = 0.3;
                printOptions.PaperSize = 9;
                printOptions.AutomaticPageBreakScalingFactor = 80;

                objExcelFile.SaveXlsx(strReportName);
                strReportName = strRName;
            }
            catch (Exception expErr)
            {
                strReportName = "";
                CatchMessage = expErr.Message;
            }
            finally
            {
                if (objExcelFile != null) objExcelFile = null;
                if (sheet != null) sheet = null;
                //ds.Dispose();

            }
            return strReportName;
        }
        #endregion

        #region CreateHeader
        void CreateHeader(ref int row, ExcelWorksheet sheet)
        {
            var style = new CellStyle();
            style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.Font.Weight = ExcelFont.BoldWeight;
            style.Borders.SetBorders(MultipleBorders.Bottom, Color.Gray, LineStyle.Thin);

            sheet.Cells.GetSubrange("A1", "P1").Style = style;

            //sheet.Cells[row, 0].Value = "Sl. No.";
            sheet.Cells[row, 0].Value = "Received Date";
            sheet.Cells[row, 1].Value = "Study UID";
            sheet.Cells[row, 2].Value = "Patient";
            sheet.Cells[row, 3].Value = "Species";
            sheet.Cells[row, 4].Value = "Received Date/Time";
            sheet.Cells[row, 5].Value = "Submitted Date/Time";

            sheet.Cells[row, 6].Value = "Modality";
            sheet.Cells[row, 7].Value = "Category";
            sheet.Cells[row, 8].Value = "Priority";
            sheet.Cells[row, 9].Value = "Image/Object Count";

            sheet.Cells[row, 10].Value = "Institution";
            sheet.Cells[row, 11].Value = "Ref. Physician";
            sheet.Cells[row, 12].Value = "Radiologist";
            sheet.Cells[row, 13].Value = "Dictated Date/Time";
            sheet.Cells[row, 14].Value = "Approved By";
            sheet.Cells[row, 15].Value = "Report Rating";

            row++;
        }
        #endregion

        #region CreateBodyRow
        void CreateBodyRow(ref int row, VETRIS.Core.Case.ReportData data, ExcelWorksheet sheet)
        {
            var style = new CellStyle();
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.Borders.SetBorders(MultipleBorders.Bottom, Color.Gray, LineStyle.Thin);
            style.Borders.SetBorders(MultipleBorders.Top, Color.Gray, LineStyle.Thin);
            sheet.Cells.GetSubrange("A" + (row + 1).ToString(), "M" + (row + 1).ToString()).Style = style;

            sheet.Cells[row, 0].Value = objComm.IMDateFormat(data.Received_Date, objComm.DateFormat);
            sheet.Cells[row, 1].Value = data.Study_Uid;
            sheet.Cells[row, 2].Value = data.Patient_Name;
            sheet.Cells[row, 3].Value = data.Species_Name;
            sheet.Cells[row, 4].Value = objComm.IMDateFormat(data.Received_Date, objComm.DateFormat) + " " + data.Received_Date.ToString("HH:mm");
            sheet.Cells[row, 5].Value =data.Submitted_Date.ToString().Contains("1900")?"": objComm.IMDateFormat(data.Submitted_Date, objComm.DateFormat) + " " + data.Submitted_Date.ToString("HH:mm");
            sheet.Cells[row, 6].Value = data.Modality_Name;
            sheet.Cells[row, 7].Value = data.Category_Name;
            sheet.Cells[row, 8].Value = data.Priority_Desc;
            sheet.Cells[row, 9].Value = data.Image_Object_Count;
            sheet.Cells[row, 10].Value = data.Institution_Name;
            sheet.Cells[row, 11].Value = data.Physician_Name;
            sheet.Cells[row, 12].Value = data.Radiologist_Pacs;
            sheet.Cells[row, 13].Value = objComm.IMDateFormat(data.Date_Dictated, objComm.DateFormat) + " " + data.Date_Dictated.ToString("HH:mm");
            sheet.Cells[row, 14].Value = data.Final_Radiologist;
            sheet.Cells[row, 15].Value = data.Rating_Reason;
            row++;
            objComm = null;
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

                        objCore.ID = new Guid(strID);
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