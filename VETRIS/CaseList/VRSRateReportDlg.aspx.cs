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
using eRADCls;
using AjaxPro;
using VETRIS.Core;
using VETRIS.Core.Translations;

namespace VETRIS.CaseList
{
    [AjaxPro.AjaxNamespace("VRSRateReportDlg")]
    public partial class VRSRateReportDlg : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.Case.CaseStudy objCore;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSRateReportDlg));
            SetAttributes();
            if ((!CallBackSelST.CausedCallback) && (!CallBackDoc.CausedCallback) && (!CallBackPS.CausedCallback))
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);

            btnSave1.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnSave2.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            rdoAbnormal.Attributes.Add("onclick", "javascript:Rating_OnClick();");
            rdoNormal.Attributes.Add("onclick", "javascript:Rating_OnClick();");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            hdnID.Value = Request.QueryString["id"];
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            Guid SessionID = new Guid(Request.QueryString["sid"]);
            if (Request.QueryString["unm"] != null) hdnLockedUser.Value = Request.QueryString["unm"];
            hdnFilePath.Value = Server.MapPath("~");
            LoadHeader(intMenuID, UserID, SessionID);

            SetCSS(Request.QueryString["th"]);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css?v=" + DateTime.Now.Ticks.ToString();
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css?v=" + DateTime.Now.Ticks.ToString();
        }
        #endregion

        #region LoadHeader
        private void LoadHeader(int intMenuID, Guid UserID, Guid SessionID)
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            string strRptText=string.Empty;
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            
            DataSet ds = new DataSet();
            string[] arrayCode = new string[0];
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
                        hdnPName.Value = objCore.PATIENT_NAME;
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
                        hdnInstID.Value = objCore.INSTITUTION_ID.ToString();
                        if (hdnRadFnRights.Value.Trim().IndexOf("VWINSTINFO") > -1)
                        {
                            lblInstitution.Text = objCore.INSTITUTION_NAME;
                            lblPhys.Text = objCore.PHYSICIAN_NAME;
                        }
                        else 
                        {
                            lblInstitution.Text = objCore.INSTITUTION_CODE;
                            lblPhys.Text = objCore.PHYSICIAN_CODE;
                        }
                        hdnPhysCode.Value = objCore.PHYSICIAN_CODE;

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
                       

                        hdnPACSURL.Value = objCore.PACS_URL;
                        hdnImgVwrURL.Value = objCore.PACS_IMAGE_VIEWER_URL;
                        hdnStudyDelUrl.Value = objCore.PACS_STUDY_DELETE_URL;
                        hdnWS8SYVWRURL.Value = objCore.WEB_SERVICE_STUDY_VIEW_URL;
                        hdnWS8IMGVWRURL.Value = objCore.WEB_SERVICE_IMAGE_VIEW_URL;
                        hdnAssnRadID.Value = objCore.PRELIMINARY_RADIOLOGIST_ID.ToString();
                        lblAssnRad.Text = objCore.PRELIMINARY_RADIOLOGIST_ASSIGNED;
                        if (objCore.ABNORMAL_REPORT_REASON_ID == new Guid("00000000-0000-0000-0000-000000000000"))
                        {
                            //rdoAbnormal.Checked = false;
                            rdoNormal.Checked = true;
                            
                        }
                        else {
                            //rdoNormal.Checked = false;
                            rdoAbnormal.Checked = true; 
                        }
                        
                        lblPrevRad.Text = objCore.PRELIMINARY_RADIOLOGIST_ASSIGNED;

                        if (objCore.PAS_USER_PASSWORD != string.Empty)
                        {
                            hdnPACSCred.Value = CoreCommon.EncryptString(objCore.PACS_USER_ID+ "±" + objCore.PAS_USER_PASSWORD);
                        }
                       
                        hdnRecByRouter.Value = objCore.RECEIVE_DUCOM_FILES_VIA_ROUTER;
                        hdnWS8SRVIP.Value = objCore.WEB_SERVICE_SERVER_URL;
                        hdnWS8CLTIP.Value = objCore.WEB_SERVICE_CLIENT_URL;
                        hdnWS8SRVUID.Value = objCore.WEB_SERVICE_USER_ID;
                        hdnWS8SRVPWD.Value = objCore.WEB_SERVICE_PASSWORD;
                        hdnAPIVER.Value = objCore.API_VERSION;
                        hdnVRSAPPLINK.Value = objCore.VETRIS_APPLICATION_LINK;
                        hdnTrackBy.Value = objCore.TRACK_COUNT_BY;
                        hdnInvBy.Value = objCore.INVOICING_BY;
                        hdnSyncMode.Value = objCore.SYNC_MODE;

                        #region Populate Disclaimer Reason                       
                        divRptDisclText.InnerHtml = objCore.REPORT_DISCLAIMER_REASON_DESCRIPTION;
                        #endregion

                        #region Populate Abnormal Report Reason
                        DataRow dr1 = ds.Tables["AbnormalRptReasons"].NewRow();
                        dr1["id"] = "00000000-0000-0000-0000-000000000000";
                        dr1["reason"] = "Select One";
                        ds.Tables["AbnormalRptReasons"].Rows.InsertAt(dr1, 0);
                        ds.Tables["AbnormalRptReasons"].AcceptChanges();

                        ddlAbRptReason.DataSource = ds.Tables["AbnormalRptReasons"];
                        ddlAbRptReason.DataValueField = "id";
                        ddlAbRptReason.DataTextField = "reason";
                        ddlAbRptReason.DataBind();

                        ddlAbRptReason.SelectedValue = Convert.ToString(objCore.ABNORMAL_REPORT_REASON_ID);
                        #endregion

                        hdnAbnormalReportId.Value = objCore.ABNORMAL_REPORT_REASON_ID.ToString();
                        ddlAbRptReason.SelectedValue = objCore.ABNORMAL_REPORT_REASON_ID.ToString();
                        divRpt.InnerHtml = objCore.FINAL_REPORT_HTML;
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

        #region Previous Study List Grid

        #region CallBackPS_Callback
        protected void CallBackPS_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadPreviousStudyList(e.Parameters);
            grdPS.Width = Unit.Percentage(100);
            grdPS.RenderControl(e.Output);
            spnErrStudy.RenderControl(e.Output);
        }
        #endregion

        #region LoadPreviousStudyList
        private void LoadPreviousStudyList(string[] arrParams)
        {
            objCore = new Core.Case.CaseStudy();
            objComm = new classes.CommonClass();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                objComm.SetRegionalFormat();
                objCore.ID = new Guid(arrParams[0]);
                objCore.PATIENT_NAME = arrParams[1].Trim();
                objCore.INSTITUTION_ID = new Guid(arrParams[2]);
                objCore.USER_ID = new Guid(arrParams[3]);

                bReturn = objCore.LoadPreviousStudyList(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdPS.DataSource = ds.Tables["StudyList"];
                    grdPS.DataBind();
                    grdPS.Levels[0].Columns[2].FormatString = objComm.DateFormat + " HH:mm";
                    grdPS.Levels[0].Columns[3].FormatString = objComm.DateFormat + " HH:mm";

                    spnErrStudy.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrStudy\" value=\"\" />";
                }
                else
                    spnErrStudy.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrStudy\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnErrStudy.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrStudy\" value=\"" + ex.Message.Trim() + "\" />";
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

        #region SaveReRate (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveReRate(string[] ArrRecord)
        {
            bool bReturn = false;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            objComm = new classes.CommonClass();
            objCore = new Core.Case.CaseStudy();
            try
            {
                objCore.REPORT_ID = new Guid(ArrRecord[0]);
                objCore.ID = new Guid(ArrRecord[1]);
                objCore.ABNORMAL_REPORT_REASON_ID = new Guid(ArrRecord[2]);
                objCore.USER_ID = new Guid(ArrRecord[3]);
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[4]);
                objCore.USER_SESSION_ID = new Guid(ArrRecord[5]);

                bReturn = objCore.SaveReRate(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
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

       
    }
}