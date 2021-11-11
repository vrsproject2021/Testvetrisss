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

namespace VETRIS.Radiologist
{
    [AjaxPro.AjaxNamespace("VRSRadUnassignDlg")]
    public partial class VRSRadUnassignDlg : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.Case.CaseStudy objCore;
        VETRIS.Core.Radiologist.AssignStudy objAsn;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSRadUnassignDlg));
            SetAttributes();
            SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);

            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");

            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

            btnUnassignPrelim.Attributes.Add("onclick", "javascript:btnUnassign_OnClick('P');");
            btnUnassignFinal.Attributes.Add("onclick", "javascript:btnUnassign_OnClick('F');");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            hdnID.Value = Request.QueryString["id"];
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            Guid SessionID = new Guid(Request.QueryString["sid"]);
            hdnMenuID.Value = intMenuID.ToString();
            hdnUserID.Value = UserID.ToString();
            hdnSessionID.Value = Convert.ToString(SessionID);
            string strTheme = Request.QueryString["th"];
            SetCSS(strTheme);
            LoadHeader(intMenuID, UserID, SessionID);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            
        }
        #endregion
        #region LoadHeader
        private void LoadHeader(int intMenuID, Guid UserID, Guid SessionID)
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
                objCore.USER_SESSION_ID = SessionID;

                bReturn = objCore.LoadHeader(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    objComm.SetRegionalFormat();

                    if (objCore.STUDY_UID != string.Empty)
                    {
                        lblSUID.Text = objCore.STUDY_UID; hdnSUID.Value = objCore.STUDY_UID;
                        lblPatientName.Text = objCore.PATIENT_NAME;
                        lblInstitution.Text = objCore.INSTITUTION_NAME;
                        lblPriority.Text = objCore.PRIORITY_DESCRIPTION;
                        lblModality.Text = objCore.MODALITY_NAME;
                        //hdnModalityID.Value = objCore.MODALITY_ID.ToString();
                        lblCategory.Text = objCore.CATEGORY_NAME;
                        lblServices.Text = objCore.SERVICE_CODES;
                        hdnStatusID.Value = objCore.PACS_STATUS_ID.ToString();
                        lblCurrStat.Text = objCore.PACS_STATUS_DESC;
                        lblPrelimRad.Text = objCore.PRELIMINARY_RADIOLOGIST_ASSIGNED;
                        hdnPrelimRadID.Value = Convert.ToString(objCore.PRELIMINARY_RADIOLOGIST_ID);
                        lblFinalRad.Text = objCore.FINAL_RADIOLOGIST_ASSIGNED;
                        hdnFinalRadID.Value = Convert.ToString(objCore.FINAL_RADIOLOGIST_ID);

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

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord)
        {

            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            string[] arrayCode = new string[0];
            string strMsg = string.Empty;

            objAsn = new Core.Radiologist.AssignStudy();
            objComm = new classes.CommonClass();

            try
            {
                objAsn.ID = new Guid(ArrRecord[0].Trim());
                objAsn.RADIOLOGIST_TYPE = ArrRecord[1].Trim();
                objAsn.RADIOLOGIST_ID = new Guid(ArrRecord[2]);
                objAsn.USER_ID = new Guid(ArrRecord[3].Trim());
                objAsn.MENU_ID = Convert.ToInt32(ArrRecord[4]);
                objAsn.USER_SESSION_ID = new Guid(ArrRecord[5].Trim());

                bReturn = objAsn.UnassignRadiologist(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    arrayCode = new string[1];
                    arrayCode[0] = strReturnMsg.Trim();
                    strMsg = objComm.SetErrorResources(arrayCode, "N", false, string.Empty, string.Empty);

                    arrRet = new string[2];
                    arrRet[0] = "true";
                    arrRet[1] = strMsg.Trim();
                }
                else
                {


                    if (strCatchMessage.Trim() != "")
                    {
                        arrayCode = new string[1];
                        arrayCode[0] = strCatchMessage.Trim();
                        strMsg = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);

                        arrRet = new string[2];
                        arrRet[0] = "catch";
                        arrRet[1] = strMsg.Trim();
                    }
                    else
                    {
                        arrayCode = new string[strReturnMsg.Split(objComm.RecordDivider).Length];
                        arrayCode = strReturnMsg.Split(objComm.RecordDivider);
                        strMsg = objComm.SetErrorResources(arrayCode, "N", true, objAsn.USER_NAME, string.Empty);

                        arrRet = new string[2];
                        arrRet[0] = "false";
                        arrRet[1] = strMsg.Trim();
                        //arrRet[2] = objAsn.USER_NAME;
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