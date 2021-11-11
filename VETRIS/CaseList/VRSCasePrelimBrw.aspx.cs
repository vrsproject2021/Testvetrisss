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
using eRADCls;

namespace VETRIS.CaseList
{
    [AjaxPro.AjaxNamespace("VRSCasePrelimBrw")]
    public partial class VRSCasePrelimBrw : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Case.CaseStudy objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSCasePrelimBrw));
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
            btnDV.Attributes.Add("onclick", "javascript:btnDV_OnClick();");
            btnClose.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");
            //ddlSpecies.Attributes.Add("onchange", "javascript:ddlSpecies_OnChange();");
            ddlInstitution.Attributes.Add("onchange", "javascript:ddlInstitution_OnChange();");
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

                    #region Species
                    //DataRow dr2 = ds.Tables["Species"].NewRow();
                    //dr2["id"] = 0;
                    //dr2["name"] = "Select One";
                    //ds.Tables["Species"].Rows.InsertAt(dr2, 0);
                    //ds.Tables["Species"].AcceptChanges();

                    //ddlSpecies.DataSource = ds.Tables["Species"];
                    //ddlSpecies.DataValueField = "id";
                    //ddlSpecies.DataTextField = "name";
                    //ddlSpecies.DataBind();
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
                        }
                    }
                    #endregion

                    #region Create WS8 Session
                    //if (hdnAPIVER.Value != "7.2")
                    //{
                    //    bReturn = client.GetSession(hdnWS8CLTIP.Value, hdnWS8SRVIP.Value, hdnWS8SRVUID.Value,hdnWS8SRVPWD.Value, ref strSession, ref strCatchMessage, ref strErr);
                    //    if (bReturn)
                    //    {
                    //        hdnWS8Session.Value = strSession.Trim();
                    //    }
                    //    else
                    //    {
                    //        hdnError.Value = strErr.Trim();
                    //    }
                    //}
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

                    if (intCnt == 1) ddlRadiologist.SelectedIndex = 1;
                    #endregion

                    if (objCore.PAS_USER_PASSWORD != string.Empty)
                    {
                        hdnPACSCred.Value = CoreCommon.EncryptString(objCore.PACS_USER_ID + "±" + objCore.PAS_USER_PASSWORD);
                    }

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
                }
                else
                    hdnError.Value = strCatchMessage.Trim();

                #region Physician
                ListItem objLI = new ListItem();
                objLI.Value = "00000000-0000-0000-0000-000000000000";
                objLI.Text = "Select One";

                //ddlBreed.Items.Add(objLI);
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
        private void SearchRecord(string[] arrRecord)
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            string strUserRole = string.Empty;
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.PATIENT_NAME = arrRecord[0].Trim();
                objCore.MODALITY_ID = Convert.ToInt32(arrRecord[1]);
                objCore.FILTER_BY_RECEIVED_DATE = arrRecord[2].Trim();
                objCore.RECEIVED_DATE_FROM = Convert.ToDateTime(arrRecord[3]);
                objCore.RECEIVED_DATE_TILL = Convert.ToDateTime(arrRecord[4]);
                objCore.INSTITUTION_ID = new Guid(arrRecord[5]);
                objCore.PHYSICIAN_ID = new Guid(arrRecord[6]);
                objCore.CATEGORY_ID = Convert.ToInt32(arrRecord[7]);
                objCore.PRELIMINARY_RADIOLOGIST_ID = new Guid(arrRecord[8]);
                objCore.FINAL_RADIOLOGIST_ID = new Guid(arrRecord[9]);
                objCore.USER_ID = new Guid(arrRecord[10].Trim());
                objCore.MENU_ID = Convert.ToInt32(arrRecord[11]);
                strUserRole =arrRecord[12].Trim();
                objCore.USER_SESSION_ID = new Guid(arrRecord[13].Trim());
                objCore.STUDY_UID = arrRecord[13].Trim();

                bReturn = objCore.SearchPreliminaryBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    grdBrw.DataSource = ds.Tables["BrowserList"];
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

            if ((strUserRole == "SUPP"))
            {
                grdBrw.Levels[0].Columns[1].Visible = true;
                grdBrw.Levels[0].Columns[15].Visible = true;
                grdBrw.Levels[0].Columns[20].Visible = true;    
            }
            else if ((strUserRole == "SYSADMIN"))
            {
                grdBrw.Levels[0].Columns[15].Visible = true;
            }
            else if ((strUserRole == "RDL"))
            {
                grdBrw.Levels[0].Columns[6].Visible = false;
                grdBrw.Levels[0].Columns[8].Visible = false;
                grdBrw.Levels[0].Columns[9].Visible = true;
                grdBrw.Levels[0].Columns[10].Visible = true;
                grdBrw.Levels[0].Columns[20].Visible = true;
            }

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