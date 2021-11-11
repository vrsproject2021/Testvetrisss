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
using AjaxPro;

namespace VETRIS.CaseList
{
    [AjaxPro.AjaxNamespace("VRSWorkListDlg")]
    public partial class VRSWorkListDlg : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.Case.CaseStudy objCore;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSWorkListDlg));
            SetAttributes();
            if ((!CallBackSelST.CausedCallback) && (!CallBackDoc.CausedCallback))
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);

            //btnSave1.Attributes.Add("onclick", "javascript:btnView_OnClick('N','X');");
            //btnSave2.Attributes.Add("onclick", "javascript:btnView_OnClick('N','X');");
            btnView1.Attributes.Add("onclick", "javascript:NavigateToPACS();");
            btnView2.Attributes.Add("onclick", "javascript:NavigateToPACS();");
            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            //btnDel1.Attributes.Add("onclick", "javascript:btnDel_OnClick();");
            //btnDel2.Attributes.Add("onclick", "javascript:btnDel_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

            //lblViewLog.Attributes.Add("onclick", "javascript:ViewLogUI('" + intMenuID.ToString() + "');");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            hdnID.Value = Request.QueryString["id"];
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            hdnCF.Value = Request.QueryString["cf"];
            hdnFilePath.Value = Server.MapPath("~");
            LoadHeader(intMenuID, UserID);
        }
        #endregion

        #region LoadHeader
        private void LoadHeader(int intMenuID, Guid UserID)
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

                bReturn = objCore.LoadHeader(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    objComm.SetRegionalFormat();

                    if (objCore.STUDY_UID != string.Empty)
                    {


                        lblSUID.Text = objCore.STUDY_UID; hdnSUID.Value = objCore.STUDY_UID;
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
                        lblDOS.Text = objComm.IMDateFormat(objCore.STUDY_DATE, objComm.DateFormat);
                        divHistory.InnerText = objCore.REASON;
                        divPhysNote.InnerText = objCore.PHYSICIAN_NOTE;
                        lblAccnNo.Text = objCore.ACCESSION_NO;
                        lblInstitution.Text = objCore.INSTITUTION_NAME;
                        lblPhys.Text = objCore.REFERRING_PHYSICIAN;
                        lblImgCnt.Text = objCore.IMAGE_COUNT.ToString();
                        lblPriority.Text = objCore.PRIORITY_DESCRIPTION;
                        lblModality.Text = objCore.MODALITY_NAME;
                        hdnModalityID.Value = objCore.MODALITY_ID.ToString();
                        lblCategory.Text = objCore.CATEGORY_NAME;
                        lblServices.Text = objCore.SERVICE_CODES;
                        hdnStatusID.Value = objCore.PACS_STATUS_ID.ToString();
                        lblCurrStat.Text = objCore.PACS_STATUS_DESC;
                        hdnWS8SRVPWD.Value = CoreCommon.DecryptString(objCore.WEB_SERVICE_PASSWORD);
                        hdnPACSURL.Value = objCore.PACS_URL;
                        hdnImgVwrURL.Value = objCore.PACS_IMAGE_VIEWER_URL;
                        hdnStudyDelUrl.Value = objCore.PACS_STUDY_DELETE_URL;
                        hdnWS8SYVWRURL.Value = objCore.WEB_SERVICE_STUDY_VIEW_URL;

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
                        //hdnWS8SRVIP.Value = objCore.WEB_SERVICE_SERVER_URL;
                        //hdnWS8CLTIP.Value = objCore.WEB_SERVICE_CLIENT_URL;
                        //hdnWS8SRVUID.Value = objCore.WEB_SERVICE_USER_ID;
                        //
                        //hdnAPIVER.Value = objCore.API_VERSION;


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

    }
}