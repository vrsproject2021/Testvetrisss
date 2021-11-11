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

namespace VETRIS.HouseKeeping
{
    public partial class VRSStudyAuditTrailDlg : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.HouseKeeping.StudyAuditTrail objCore;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            
            SetAttributes();
            if ((!CallBackST.CausedCallback) && (!CallBackDoc.CausedCallback))
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);

           
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            hdnID.Value = Request.QueryString["id"];
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
           
            hdnFilePath.Value = Server.MapPath("~");
            LoadHeader(intMenuID, UserID);

            SetCSS(Request.QueryString["th"]);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
            lnkCAL.Attributes["href"] = strServerPath + "/css/" + strTheme + "/CalendarStyle.css";
        }
        #endregion

        #region CreateUserDirectory
        private void CreateUserDirectory(Guid UserID)
        {
            if (!Directory.Exists(Server.MapPath("~/HouseKeeping/Temp/" + UserID.ToString())))
            {
                Directory.CreateDirectory(Server.MapPath("~/HouseKeeping/Temp/" + UserID.ToString()));
            }
        }
        #endregion

        #region LoadHeader
        private void LoadHeader(int intMenuID, Guid UserID)
        {
            objCore = new Core.HouseKeeping.StudyAuditTrail();
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
                        lblPName.Text = objCore.PATIENT_NAME;
                        lblPID.Text = objCore.PATIENT_ID;
                        lblSex.Text = objCore.PATIENT_GENDER;


                        lblSN.Text = objCore.SEX_NEUTERED;

                        lblWt.Text = objComm.IMNumeric(objCore.PATIENT_WEIGHT, 3);
                        lblUOM.Text = objCore.WEIGHT_UOM;
                        if (objCore.PATIENT_DOB.Year > 1900) lblFromDt.Text = objComm.IMDateFormat(objCore.PATIENT_DOB, objComm.DateFormat);


                        lblAge.Text = objCore.PATIENT_AGE;
                        lblSpecies.Text = objCore.SPECIES_NAME.ToString();
                        lblBreed.Text = objCore.BREED_NAME.ToString();
                        lblOwnerFN.Text = objCore.OWNER_FIRST_NAME; lblOwnerLN.Text = objCore.OWNER_LAST_NAME;
                        lblStudyDt.Text = objComm.IMDateFormat(objCore.STUDY_DATE, objComm.DateFormat);
                        divReason.InnerHtml = objCore.REASON;
                        divPhysNote.InnerHtml = objCore.PHYSICIAN_NOTE;
                        lblAccnNo.Text = objCore.ACCESSION_NO;
                        lblPriority.Text = objCore.PRIORITY_DESCRIPTION;
                        lblModality.Text = objCore.MODALITY_NAME;
                        hdnModalityID.Value = objCore.MODALITY_ID.ToString();
                        lblInstitution.Text = objCore.INSTITUTION_NAME;
                        lblPhys.Text = objCore.PHYSICIAN_NAME;
                        
                        lblImgCnt.Text = objCore.IMAGE_COUNT.ToString();
                        lblObjCnt.Text = objCore.OBJECT_COUNT.ToString();
                        lblConfImgCnt.Text = objCore.IMAGE_COUNT_ACCEPTED; 
                        

                        
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

        #region Study Type Grid

        #region CallBackST_Callback
        protected void CallBackST_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadStudyTypes(e.Parameters);
            grdST.Width = Unit.Percentage(100);
            grdST.RenderControl(e.Output);
            spnErrST.RenderControl(e.Output);
            spnTrackBy.RenderControl(e.Output);
        }
        #endregion

        #region LoadStudyTypes
        private void LoadStudyTypes(string[] arrParams)
        {
            objCore = new Core.HouseKeeping.StudyAuditTrail();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";

            try
            {

                objCore.ID = new Guid(arrParams[0]);
                objCore.MODALITY_ID = Convert.ToInt32(arrParams[1]);

                bReturn = objCore.LoadStudyTypes(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdST.DataSource = ds.Tables["StudyTypes"];
                    grdST.DataBind();
                    spnTrackBy.InnerHtml = "<input type=\"hidden\" id=\"hdnModTrackBy\" value=\"I\" />";

                    foreach (DataRow dr in ds.Tables["TrackBy"].Rows)
                    {
                        spnTrackBy.InnerHtml = "<input type=\"hidden\" id=\"hdnModTrackBy\" value=\"" + Convert.ToString(dr["track_by"]) + "\" />";
                    }


                    spnErrST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrST\" value=\"\" />";
                }
                else
                {
                    spnTrackBy.InnerHtml = "<input type=\"hidden\" id=\"hdnModTrackBy\" value=\"I\" />";
                    spnErrST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrST\" value=\"" + strCatchMessage + "\" />";
                }



            }
            catch (Exception ex)
            {
                spnErrST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrST\" value=\"" + ex.Message.Trim() + "\" />";
                spnTrackBy.InnerHtml = "<input type=\"hidden\" id=\"hdnModTrackBy\" value=\"I\" />";
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
            objCore = new Core.HouseKeeping.StudyAuditTrail();
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
                        SetFile((byte[])dr["document_file"], Convert.ToString(dr["document_link"]).Trim(), "HouseKeeping/Temp/" + UserID.ToString());

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