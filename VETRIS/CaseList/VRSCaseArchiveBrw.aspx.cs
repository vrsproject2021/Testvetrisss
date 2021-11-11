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
using System.Net;
using VETRIS.Core;
using eRADCls;
using VETRIS.Core.Radiologist;
using GemBox.Spreadsheet;
using System.Drawing;

namespace VETRIS.CaseList
{
    [AjaxPro.AjaxNamespace("VRSCaseArchiveBrw")]
    public partial class VRSCaseArchiveBrw : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Case.CaseStudy objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSCaseArchiveBrw));
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
            //ddlSpecies.Attributes.Add("onchange", "javascript:ddlSpecies_OnChange();");
            ddlInstitution.Attributes.Add("onchange", "javascript:ddlInstitution_OnChange();");
            txtFromDt.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgFrom.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtFromDt.ClientID + "','CalFrom');");
            txtTillDt.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgTill.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtTillDt.ClientID + "','CalTill');");
            chkShowAbRpt.Attributes.Add("onclick", "javascript:chkShowAbRpt_OnClick();");
            btnExcel.Attributes.Add("onclick", "javascript:btnExcel_OnClick();");

        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"].ToString());
            string strID = string.Empty;


            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            txtFromDt.Text = objComm.IMDateFormat(DateTime.Today.AddDays(-7), objComm.DateFormat);
            txtTillDt.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            objComm = null;

            FetchSearchParameters(UserID);
            DeleteUserDirectory(UserID);

        }
        #endregion

        #region DeleteUserDirectory
        private void DeleteUserDirectory(Guid UserID)
        {
            string[] arrTemp = new string[0];

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
            string strControlCode = string.Empty;
            DataSet ds = new DataSet();
            RadWebClass client = new RadWebClass();
            string strSession = string.Empty;
            string strErr = string.Empty;
            int intCnt = 0;
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
                                hdnWS8SRVPWD.Value = Convert.ToString(dr["data_type_string"]).Trim();
                                break;
                            case "PACSTUDYDELURL":
                                hdnStudyDelUrl.Value = Convert.ToString(dr["data_type_string"]).Trim();
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

                    ddlApprovedBy.DataSource = ds.Tables["Radiologists"];
                    ddlApprovedBy.DataValueField = "id";
                    ddlApprovedBy.DataTextField = "name";
                    ddlApprovedBy.DataBind();

                    ddlRadiologist.DataSource = ds.Tables["Radiologists"];
                    ddlRadiologist.DataValueField = "id";
                    ddlRadiologist.DataTextField = "name";
                    ddlRadiologist.DataBind();

                    if (intCnt == 1)
                    {
                        ddlApprovedBy.SelectedIndex = 1;
                        ddlRadiologist.SelectedIndex = 1;
                    }
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
                objCore.PACS_STATUS_ID = Convert.ToInt32(arrParams[8]);
                objCore.FINAL_RADIOLOGIST_ID = new Guid(arrParams[9].Trim());
                objCore.RADIOLOGIST_ID = new Guid(arrParams[10].Trim());
                objCore.SHOW_ABNORMAL_REPORTS = arrParams[11].Trim();
                objCore.ABNORMAL_REPORT_REASON_ID = new Guid(arrParams[12].Trim());
                objCore.PACS_STATUS_ID = Convert.ToInt32(arrParams[13]);
                objCore.USER_ID = new Guid(arrParams[14].Trim());
                strUserRole = arrParams[15].Trim();
                objCore.PageSize = Convert.ToInt32(arrParams[16]);
                objCore.PageNo = Convert.ToInt32(arrParams[17]);

                bReturn = objCore.SearchFinalReportBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    if (arrParams[11].Trim() == "Y")
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

            if ((strUserRole == "SUPP"))
            {
                grdBrw.Levels[0].Columns[1].Visible = true;
                grdBrw.Levels[0].Columns[8].Visible = true;
                grdBrw.Levels[0].Columns[9].Visible = true;
                grdBrw.Levels[0].Columns[16].Visible = true;
                grdBrw.Levels[0].Columns[17].Visible = true;
                grdBrw.Levels[0].Columns[24].Visible = true;
            }
            else if ((strUserRole == "SYSADMIN"))
            {
                grdBrw.Levels[0].Columns[8].Visible = true;
                grdBrw.Levels[0].Columns[9].Visible = true;
                grdBrw.Levels[0].Columns[16].Visible = true;
                grdBrw.Levels[0].Columns[17].Visible = true;
                grdBrw.Levels[0].Columns[24].Visible = true;
            }
            else if ((strUserRole == "RDL"))
            {
                grdBrw.Levels[0].Columns[5].Visible = false;
                grdBrw.Levels[0].Columns[7].Visible = false;
                grdBrw.Levels[0].Columns[8].Visible = true;
                grdBrw.Levels[0].Columns[9].Visible = true;
                grdBrw.Levels[0].Columns[16].Visible = false;
                grdBrw.Levels[0].Columns[17].Visible = true;
                grdBrw.Levels[0].Columns[24].Visible = true;
                
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
                else if (strRecByRouter == "Y")
                {
                    #region if DICOM ROUTER == MANUAL
                    objCore.ID = new Guid(ArrRecord[0]);
                    objCore.STUDY_UID = ArrRecord[1].Trim();
                    objCore.USER_ID = UserID = new Guid(ArrRecord[3]);

                    bReturn = objCore.DeleteArchiveStudy(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                    if (bReturn)
                    {
                        arrRet = new string[2];
                        arrRet[0] = "true";
                        arrRet[1] = "true";
                    }
                    else
                    {
                        if (strCatchMessage.Trim() != "")
                        {
                            arrRet = new string[2];
                            arrRet[0] = "catch";
                            arrRet[1] = strReturnMsg.Trim();

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

                    //if (bReturn)
                    //{
                        if (strRecByRouter == "N")
                        {
                            objCore.ID = new Guid(ArrRecord[0]);
                            objCore.STUDY_UID = ArrRecord[1].Trim();
                            objCore.USER_ID = UserID = new Guid(ArrRecord[3]);
                            objCore.MENU_ID = Convert.ToInt32(ArrRecord[4]);

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

                    //}
                    //else
                    //{
                    //    if (strCatchMessage.Trim() != string.Empty)
                    //    {
                    //        arrRet = new string[2];
                    //        arrRet[0] = "catch";
                    //        arrRet[1] = strCatchMessage.Trim();
                    //    }
                    //    else
                    //    {
                    //        arrRet = new string[2];
                    //        arrRet[0] = "false";
                    //        arrRet[1] = strError.Trim();
                    //    }

                    //}
                    #endregion
                }
                else if (strRecByRouter == "Y")
                {
                    #region if DICOM ROUTER == MANUAL
                    objCore.ID = new Guid(ArrRecord[0]);
                    objCore.STUDY_UID = ArrRecord[1].Trim();
                    objCore.USER_ID = UserID = new Guid(ArrRecord[3]);

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

        #region UpdateReportAddendum

        #region UpdateReportAddendum
        [AjaxPro.AjaxMethod()]
        public string[] UpdateReportAddendum(string[] ArrRecord)
        {
            bool bReturn = false;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            DataTable dtbl= new DataTable();
            
            RadWebClass client = new RadWebClass();
            string strResult = string.Empty;
            string sError = string.Empty;
            string strWSURL = string.Empty;
            string strClientIP = string.Empty;
            string strStudyUID=string.Empty;
            int intRecordID=0;


            objCore = new Core.Case.CaseStudy();
            dtbl = CreateAddendumTable();

            try
            {
                objCore.ID = new Guid(ArrRecord[0]);
                strStudyUID = ArrRecord[1].Trim();
                objCore.USER_ID = new Guid(ArrRecord[2]);
                strWSURL = ArrRecord[3].Trim();
                strClientIP = ArrRecord[4].Trim();

                bReturn = client.GetStudyData(string.Empty, strWSURL, strStudyUID, ref strResult, ref strCatchMessage, ref sError);
                if (bReturn)
                {
                    DataSet ds = new DataSet();
                    strResult = strResult.Trim();
                    System.IO.StringReader xmlSR = new System.IO.StringReader(strResult);
                    ds.ReadXml(xmlSR);

                    #region Get Addendum
                    if (ds.Tables.Contains("Field"))
                    {
                        DataView dv = new DataView(ds.Tables["Field"]);
                        dv.RowFilter = "Colid ='SRFT'";

                        if (dv.ToTable().Rows.Count > 0)
                        {
                            foreach (DataRow dr in dv.ToTable().Rows)
                            {
                                intRecordID = Convert.ToInt32(dr["Record_Id"]);
                                if (intRecordID > 0)
                                {
                                    DataRow drAdd = dtbl.NewRow();
                                    drAdd["srl_no"] = intRecordID;
                                    if (dr["Value"] != DBNull.Value)
                                        drAdd["addendum_text"] = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                    else
                                        drAdd["addendum_text"] = string.Empty;
                                    dtbl.Rows.Add(drAdd);

                                }
                            }
                            dv.Dispose();

                            #region Get Addendum HTML Text
                            dv.RowFilter = "Colid ='SRFH'";
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                foreach (DataRow dr in dv.ToTable().Rows)
                                {
                                    intRecordID = Convert.ToInt32(dr["Record_Id"]);
                                    if (intRecordID > 0)
                                    {
                                        DataRow[] foundRow = dtbl.Select("srl_no =" + intRecordID.ToString());
                                        if (foundRow.Length == 0)
                                        {
                                            DataRow drAddn = dtbl.NewRow();
                                            drAddn["srl_no"] = intRecordID;
                                            if (dr["Value"] != DBNull.Value)
                                                drAddn["addendum_text_html"] = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                            else
                                                drAddn["addendum_text_html"] = string.Empty;
                                            dtbl.Rows.Add(drAddn);
                                        }
                                        else
                                        {
                                            if (dr["Value"] != DBNull.Value)
                                                foundRow[0]["addendum_text_html"] = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                            else
                                                foundRow[0]["addendum_text_html"] = string.Empty;
                                        }
                                        dtbl.AcceptChanges();

                                    }
                                }
                                dv.Dispose();
                            }
                            #endregion

                            #region update db

                            if (dtbl.Rows.Count > 0)
                            {
                                bReturn = objCore.UpdateArchiveReportAddendum(Server.MapPath("~"), dtbl, ref strReturnMsg, ref strCatchMessage);

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
                            else
                            {
                                arrRet = new string[2];
                                arrRet[0] = "true";
                                arrRet[1] = "";
                            }
                            #endregion
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
                        arrRet[0] = "true";
                        arrRet[1] = "";
                    }
                    #endregion
                }
                else
                {
                    bReturn = false;
                    arrRet = new string[2];
                    arrRet[0] = "false";
                    if (sError.Trim() != string.Empty)
                    {
                        arrRet[1] = sError;
                    }
                    else
                    {
                        arrRet[1] = strCatchMessage;
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
                dtbl= null;
            }
            return arrRet;
        }
        #endregion

        #region CreateAddendumTable
        private DataTable CreateAddendumTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("srl_no", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("addendum_text", System.Type.GetType("System.String"));
            dtbl.Columns.Add("addendum_text_html", System.Type.GetType("System.String"));
            dtbl.TableName = "Addendum";
            return dtbl;
        }
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
            string[] arrRet = new string[4];
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            string strRptFile = string.Empty;
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
                objCore.PACS_STATUS_ID = Convert.ToInt32(arrParams[8]);
                objCore.FINAL_RADIOLOGIST_ID = new Guid(arrParams[9].Trim());
                objCore.RADIOLOGIST_ID = new Guid(arrParams[10].Trim());
                objCore.SHOW_ABNORMAL_REPORTS = arrParams[11].Trim();
                objCore.ABNORMAL_REPORT_REASON_ID = new Guid(arrParams[12].Trim());
                objCore.USER_ID = new Guid(arrParams[13].Trim());
                strUserRole = arrParams[14].Trim();

                bReturn = objCore.SearcArchiveBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    strRptFile = CreateReportExcel(ds, objCore, arrParams[13].Trim(),arrParams[11].Trim() == "Y");
                    arrRet[0] = "true";
                    arrRet[1] = "CaseList/Temp/" + arrParams[13].Trim() + "/" + strRptFile;
                    arrRet[2] = Server.MapPath("CaseList/Temp/" + arrParams[13].Trim() + "/" + strRptFile).Replace("ajaxpro\\", "");
                    arrRet[3] = "XLS";
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
            return arrRet;
        }
        #endregion

        #region CreateReportExcel
        public string CreateReportExcel(DataSet ds, Core.Case.CaseStudy core, string strUserID, bool isAbNormal = false)
        {
            string strReportName = string.Empty;
            string strCatchMsg = ""; bool bReturn = false;
            SetLicenseKey();
            ExcelFile objExcelFile = null; string strRowfiltertring = string.Empty;
            ExcelWorksheet sheet = null;
            //ExcelWorksheet objExcelWorksheet2 = null;
            string strRName = "ArchivedReport_" + (isAbNormal ? "Abnormal_" : "") + string.Format("{0:yyyy_MM_dd_HH_mm_ss}", DateTime.Now) + ".xlsx";
            objComm = new classes.CommonClass();

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
                strCatchMsg = expErr.Message;
            }
            finally
            {
                if (objExcelFile != null) objExcelFile = null;
                if (sheet != null) sheet = null;
                //ds.Dispose();
                objComm = null;

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

            sheet.Cells.GetSubrange("A1", "L1").Style = style;

            //sheet.Cells[row, 0].Value = "Sl. No.";
            sheet.Cells[row, 0].Value = "Study UID";
            sheet.Cells[row, 1].Value = "Patient";
            sheet.Cells[row, 2].Value = "Received Date/Time";
            sheet.Cells[row, 3].Value = "Modality";
            sheet.Cells[row, 4].Value = "Category";
            sheet.Cells[row, 5].Value = "Priority";

            sheet.Cells[row, 6].Value = "Institution";
            sheet.Cells[row, 7].Value = "Ref. Physician";
            sheet.Cells[row, 8].Value = "Radiologist";
            sheet.Cells[row, 9].Value = "Dictated Date/Time";
            sheet.Cells[row, 10].Value = "Approved By";
            sheet.Cells[row, 11].Value = "Report Rating";

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
            sheet.Cells.GetSubrange("A" + (row + 1).ToString(), "L" + (row + 1).ToString()).Style = style;

            sheet.Cells[row, 0].Value = data.Study_Uid;
            sheet.Cells[row, 1].Value = data.Patient_Name;
            sheet.Cells[row, 2].Value = objComm.IMDateFormat(data.Received_Date, objComm.DateFormat) + " " + data.Received_Date.ToString("HH:mm");
            sheet.Cells[row, 3].Value = data.Modality_Name;
            sheet.Cells[row, 4].Value = data.Category_Name;
            sheet.Cells[row, 5].Value = data.Priority_Desc;
            sheet.Cells[row, 6].Value = data.Institution_Name;
            sheet.Cells[row, 7].Value = data.Physician_Name;
            sheet.Cells[row, 8].Value = data.Radiologist_Pacs;
            sheet.Cells[row, 9].Value = objComm.IMDateFormat(data.Date_Dictated, objComm.DateFormat) + " " + data.Date_Dictated.ToString("HH:mm");
            sheet.Cells[row, 10].Value = data.Final_Radiologist;
            sheet.Cells[row, 11].Value = data.Rating_Reason;
            row++;
            objComm = null;
        }


        #endregion
    }
}