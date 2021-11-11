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
using eRADCls;
using VETRIS.Core;
using AjaxPro;

namespace VETRIS.CaseList
{
    [AjaxPro.AjaxNamespace("VRSCaseArchivelDlg")]
    public partial class VRSCaseArchivelDlg : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.Case.CaseStudy objCore;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSCaseArchivelDlg));
            SetAttributes();
            if ((!CallBackSelST.CausedCallback) && (!CallBackDoc.CausedCallback) && (!CallBackAddn.CausedCallback))
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);

            btnSave1.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnSave2.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnView1.Attributes.Add("onclick", "javascript:NavigateToPACS();");
            btnView2.Attributes.Add("onclick", "javascript:NavigateToPACS();");
            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            //btnDel1.Attributes.Add("onclick", "javascript:btnDel_OnClick();");
            //btnDel2.Attributes.Add("onclick", "javascript:btnDel_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnDownload1.Attributes.Add("onclick", "javascript:btnDownload_OnClick();");
            btnDownload2.Attributes.Add("onclick", "javascript:btnDownload_OnClick();");
            ddlDisclReason.Attributes.Add("onchange", "javascript:ddlDisclReason_OnChange();");
            //lblViewLog.Attributes.Add("onclick", "javascript:ViewLogUI('" + intMenuID.ToString() + "');");
            btnEditST.Attributes.Add("onclick", "javascript:btnEditST_OnClick();");
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
            if (Request.QueryString["unm"] != null) hdnLockedUser.Value = Request.QueryString["unm"];
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
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css?v=" + DateTime.Now.Ticks.ToString();
            editorFindings.ContentsCss = strServerPath + "/css/" + strTheme + "/contents.css?v=" + DateTime.Now.Ticks.ToString();
            editorAddendum.ContentsCss = strServerPath + "/css/" + strTheme + "/contents.css?v=" + DateTime.Now.Ticks.ToString(); 
        }
        #endregion

        #region LoadHeader
        private void LoadHeader(int intMenuID, Guid UserID, Guid SessionID)
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            DataSet ds = new DataSet();
            string[] arrayCode = new string[0];
            objComm = new classes.CommonClass();

            try
            {
                objCore.ID = new Guid(hdnID.Value);
                objCore.MENU_ID = intMenuID;
                objCore.USER_ID = UserID;
                objCore.USER_SESSION_ID = SessionID;

                bReturn = objCore.LoadArchiveHeader(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    objComm.SetRegionalFormat();

                    #region Radiologist Functional Rights
                    foreach (DataRow dr in ds.Tables["RadFnRights"].Rows)
                    {
                        if (hdnRadFnRights.Value.Trim() != string.Empty) hdnRadFnRights.Value += objComm.RecordDivider;
                        hdnRadFnRights.Value += Convert.ToString(dr["right_code"]);
                    }
                    #endregion

                    if (objCore.STUDY_UID != string.Empty)
                    {

                        lblSUID.Text = objCore.STUDY_UID; hdnSUID.Value = objCore.STUDY_UID;
                        hdnRptID.Value = objCore.REPORT_ID.ToString();
                        lblPatientID.Text = objCore.PATIENT_ID;
                        lblPatientName.Text = objCore.PATIENT_NAME;
                        lblSex.Text = objCore.PATIENT_GENDER;
                        lblSN.Text = objCore.SEX_NEUTERED;

                        lblPWt.Text = objComm.IMNumeric(objCore.PATIENT_WEIGHT, 3) + " " + objCore.WEIGHT_UOM;
                        if (objCore.PATIENT_DOB.Year > 1900) lblDOB.Text = objComm.IMDateFormat(objCore.PATIENT_DOB, objComm.DateFormat);
                        else lblDOB.Text = string.Empty;

                        lblAge.Text = objCore.PATIENT_AGE;
                        lblSpecies.Text = objCore.SPECIES_NAME;
                        lblBreed.Text = objCore.BREED_NAME;
                        lblOwner.Text = objCore.OWNER_FIRST_NAME + " " + objCore.OWNER_LAST_NAME;
                        lblCountry.Text = objCore.PATIENT_COUNTRY_NAME;
                        lblState.Text = objCore.PATIENT_STATE_NAME;
                        lblCity.Text = objCore.PATIENT_CITY;


                        lblDOS.Text = objComm.IMDateFormat(objCore.STUDY_DATE, objComm.DateFormat);
                        divHistory.InnerText = objCore.REASON;
                        divPhysNote.InnerText = objCore.PHYSICIAN_NOTE;
                        lblAccnNo.Text = objCore.ACCESSION_NO;
                        lblInstitution.Text = objCore.INSTITUTION_NAME;
                        lblPhys.Text = objCore.REFERRING_PHYSICIAN;
                        if (objCore.TRACK_COUNT_BY == "I")
                        {
                            lblObj.Visible = false;
                            lblCnt.Text = objCore.IMAGE_COUNT.ToString();
                        }
                        else if (objCore.TRACK_COUNT_BY == "O")
                        {
                            lblImg.Visible = false;
                            lblCnt.Text = objCore.OBJECT_COUNT.ToString();
                        }
                        lblPriority.Text = objCore.PRIORITY_DESCRIPTION;
                        lblModality.Text = objCore.MODALITY_NAME;
                        hdnModalityID.Value = objCore.MODALITY_ID.ToString();
                        lblCategory.Text = objCore.CATEGORY_NAME;
                        lblServices.Text = objCore.SERVICE_CODES;
                        hdnCurrStatusID.Value = objCore.PACS_STATUS_ID.ToString();
                        divFindings.InnerHtml = objCore.FINAL_REPORT_HTML;
                        if (hdnRadFnRights.Value.IndexOf("RPTONRTEDTR") == -1)
                            txtReport.Text = objCore.FINAL_REPORT;
                        else
                            editorFindings.Text = objCore.FINAL_REPORT_HTML;

                        arrayCode = new string[1];
                        arrayCode[0] = "396";
                        divDisclaimer.InnerHtml = objComm.SetErrorResources(arrayCode, "N", false, objCore.VETRIS_APPLICATION_LINK + "/CaseList/DownloadDCMViewerInstaller/Setup.msi", string.Empty);

                        //hdnWS8SRVPWD.Value = CoreCommon.DecryptString(objCore.WEB_SERVICE_PASSWORD);
                        hdnPACSURL.Value = objCore.PACS_URL;
                        hdnImgVwrURL.Value = objCore.PACS_IMAGE_VIEWER_URL;
                        hdnStudyDelUrl.Value = objCore.PACS_STUDY_DELETE_URL;
                        hdnWS8SYVWRURL.Value = objCore.WEB_SERVICE_STUDY_VIEW_URL;
                        hdnPrelimRadID.Value = objCore.PRELIMINARY_RADIOLOGIST_ID.ToString();
                        hdnAssnRadID.Value = objCore.FINAL_RADIOLOGIST_ID.ToString();
                        hdnVRSAPPLINK.Value = objCore.VETRIS_APPLICATION_LINK;
                        hdnWS8SRVIP.Value = objCore.WEB_SERVICE_SERVER_URL;
                        hdnWS8CLTIP.Value = objCore.WEB_SERVICE_CLIENT_URL;
                        hdnWS8SRVUID.Value = objCore.WEB_SERVICE_USER_ID;
                        hdnWS8SRVPWD.Value = objCore.WEB_SERVICE_PASSWORD;
                        hdnAPIVER.Value = objCore.API_VERSION;

                        if (objCore.PAS_USER_PASSWORD != string.Empty)
                        {
                            hdnPACSCred.Value = CoreCommon.EncryptString(objCore.PACS_USER_ID + "±" + objCore.PAS_USER_PASSWORD);
                        }

                        hdnInvBy.Value = objCore.INVOICING_BY;

                        //txtObjCnt.Text = objCore.OBJECT_COUNT.ToString();
                        //if (objCore.IMAGE_COUNT_ACCEPTED == "Y") rdoConfYes.Checked = true;
                        //else if (objCore.IMAGE_COUNT_ACCEPTED == "N") rdoConfNo.Checked = true;

                        //if (objCore.CONSULT_APPLIED == "Y") rdoConsY.Checked = true;
                        //else rdoConsN.Checked = true;

                        //
                        //hdnImgCntURL.Value = objCore.PACS_IMAGE_COUNT_URL;
                        //
                        //hdnRecByRouter.Value = objCore.RECEIVE_DUCOM_FILES_VIA_ROUTER;
                        //hdnInstConsAppl.Value = objCore.INSTITUTION_CONSULTATION_APPLICABLE;
                       

                        #region Populate Disclaimer Reason
                        DataRow dr = ds.Tables["RptDisclReasons"].NewRow();
                        dr["id"] = 0;
                        dr["type"] = "Select One";
                        ds.Tables["RptDisclReasons"].Rows.InsertAt(dr, 0);
                        ds.Tables["RptDisclReasons"].AcceptChanges();

                        ddlDisclReason.DataSource = ds.Tables["RptDisclReasons"];
                        ddlDisclReason.DataValueField = "id";
                        ddlDisclReason.DataTextField = "type";
                        ddlDisclReason.DataBind();
                        ddlDisclReason.SelectedValue = objCore.REPORT_DISCLAIMER_REASON_ID.ToString()   ;
                        txtRptDisclText.Text = objCore.REPORT_DISCLAIMER_REASON_DESCRIPTION;
                        //divRptDisc.InnerHtml = objCore.REPORT_DISCLAIMER_REASON_DESCRIPTION.Trim();

                        hdnTrackBy.Value = objCore.TRACK_COUNT_BY;
                        hdnSyncMode.Value = objCore.SYNC_MODE;
                        #endregion

                        if (objCore.MARKED_FOR_TEACHING == "Y") chkTeach.Checked = true;
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

        #region CreateUserDirectory
        private void CreateUserDirectory(Guid UserID)
        {
            if (!Directory.Exists(Server.MapPath("~/CaseList/Temp/" + UserID.ToString())))
            {
                Directory.CreateDirectory(Server.MapPath("~/CaseList/Temp/" + UserID.ToString()));
            }
        }
        #endregion

        #region FetchTypeWiseReportDisclaimerReasonDescription (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchTypeWiseReportDisclaimerReasonDescription(string[] arrParams)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            objCore = new Core.Case.CaseStudy();

            int i = 0;
            string[] arrRet = new string[0];

            try
            {

                objCore.REPORT_DISCLAIMER_REASON_ID = Convert.ToInt32(arrParams[0].Trim());


                bReturn = objCore.FetchTypeWiseReportDisclaimerReasonDescription(Server.MapPath("~"), ref strCatchMessage);

                if (bReturn)
                {

                    arrRet = new string[2];
                    arrRet[0] = "true";
                    arrRet[1] = objCore.REPORT_DISCLAIMER_REASON_DESCRIPTION.Trim();

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

            return arrRet;
        }
        #endregion

        #region Study Type Grid

        #region CallBackSelST_Callback
        protected void CallBackSelST_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadSelectedStudyTypes(e.Parameters);
            grdSelST.Width = Unit.Percentage(100);
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
            string strFileName = "";

            try
            {

                objCore.ID = new Guid(arrParams[0]);
                objCore.MODALITY_ID = Convert.ToInt32(arrParams[1]);

                bReturn = objCore.LoadSelectedStudyTypes(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdSelST.DataSource = ds.Tables["SelStudyTypes"];
                    grdSelST.DataBind();


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

        #endregion

        #region Document Grid

        #region CallBackDoc_Callback
        protected void CallBackDoc_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadUploadedDocs(e.Parameters);
            grdDoc.Width = Unit.Percentage(100);
            grdDoc.RenderControl(e.Output);
            spnERRDoc.RenderControl(e.Output);
        }
        #endregion

        #region LoadUploadedDocs
        private void LoadUploadedDocs(string[] arrParams)
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";

            try
            {

                objCore.ID = new Guid(arrParams[0]);
                UserID = new Guid(arrParams[1]);

                bReturn = objCore.LoadHeaderDocuments(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdDoc.DataSource = ds.Tables["Documents"];
                    grdDoc.DataBind();

                    foreach (DataRow dr in ds.Tables["Documents"].Rows)
                    {

                        strFileName = Convert.ToString(dr["document_link"]);
                        SetFile((byte[])dr["document_file"], Convert.ToString(dr["document_link"]).Trim(), "CaseList/Temp/" + UserID.ToString());

                    }

                    spnERRDoc.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDoc\" value=\"\" />";
                }
                else
                    spnERRDoc.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDoc\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnERRDoc.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDoc\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
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

        #region Addendum Grid

        #region CallBackAddn_Callback
        protected void CallBackAddn_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadReportAddendums(e.Parameters);
            grdAddn.Width = Unit.Percentage(100);
            grdAddn.RenderControl(e.Output);
            spnADDNErr.RenderControl(e.Output);
        }
        #endregion

        #region LoadReportAddendums
        private void LoadReportAddendums(string[] arrParams)
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;

            try
            {

                objCore.ID = new Guid(arrParams[0]);

                bReturn = objCore.LoadReportAddendums(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdAddn.DataSource = ds.Tables["Addendums"];
                    grdAddn.DataBind();
                    spnADDNErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrAddn\" value=\"\" />";
                }
                else
                    spnADDNErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrAddn\" value=\"" + strCatchMessage + "\" />";

            }
            catch (Exception ex)
            {
                spnADDNErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrAddn\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

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

        #region SaveReportAddendum (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveReportAddendum(string[] ArrRecord)
        {
            bool bReturn = false;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;

            objComm = new classes.CommonClass();
            objCore = new Core.Case.CaseStudy();


            try
            {
                objCore.ID = new Guid(ArrRecord[0]);
                objCore.REPORT_TEXT = ArrRecord[1].Trim();
                objCore.REPORT_TEXT_HTML = ArrRecord[2].Trim();
                objCore.ADDENDUM_TEXT = ArrRecord[3].Trim();
                objCore.ADDENDUM_TEXT_HTML = ArrRecord[4].Trim();
                objCore.REPORT_DISCLAIMER_REASON_ID = Convert.ToInt32(ArrRecord[5]);
                objCore.MARKED_FOR_TEACHING = ArrRecord[6].Trim();
                objCore.REPORT_DISCLAIMER_REASON_DESCRIPTION = ArrRecord[7].Trim();
                objCore.USER_ID = new Guid(ArrRecord[8]);
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[9]);
                objCore.USER_SESSION_ID = new Guid(ArrRecord[10]);

                bReturn = objCore.SaveReportAddendum(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

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