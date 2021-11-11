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
    [AjaxPro.AjaxNamespace("VRSRadAssignMultiDlg")]
    public partial class VRSRadAssignMultiDlg : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.Case.CaseStudy objCore;
        VETRIS.Core.Radiologist.AssignStudy objAsn;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSRadAssignMultiDlg));
            SetAttributes();
            if (!CallBackRad.CausedCallback) SetPageValue();
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

            rdoAll.Attributes.Add("onclick", "javascript:LoadRadiologist();");
            rdoSchedule.Attributes.Add("onclick", "javascript:LoadRadiologist();");
            rdoPrelim.Attributes.Add("onclick", "javascript:LoadRadiologist();");
            rdoFinal.Attributes.Add("onclick", "javascript:LoadRadiologist();");

            //lblViewLog.Attributes.Add("onclick", "javascript:ViewLogUI('" + intMenuID.ToString() + "');");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            hdnStatusID.Value = Request.QueryString["statID"];
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            Guid SessionID = new Guid(Request.QueryString["sid"]);
            hdnMenuID.Value = intMenuID.ToString();
            hdnUserID.Value = UserID.ToString();
            hdnSessionID.Value = Convert.ToString(SessionID);
            string strTheme = Request.QueryString["th"];
            SetCSS(strTheme);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
        }
        #endregion

        #region CallBackRad_Callback
        protected void CallBackRad_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadRadiologists(e.Parameters);
            grdRad.Width = Unit.Percentage(99);
            grdRad.RenderControl(e.Output);
            spnErr.RenderControl(e.Output);
        }
        #endregion

        #region LoadRadiologists
        private void LoadRadiologists(string[] arrRecord)
        {
            objAsn = new Core.Radiologist.AssignStudy();
            string strCatchMessage = ""; string strReturnMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            string[] arrID = new string[0];
            objComm = new classes.CommonClass();

            try
            {
               
                objAsn.RADIOLOGIST_TYPE = arrRecord[0].Trim();
                objAsn.FILTER = arrRecord[1].Trim();

                if (arrRecord[2].Trim().Contains(objComm.RecordDivider))
                {
                    arrID = arrRecord[2].Split(objComm.RecordDivider);
                }
                else
                {
                    arrID = new string[1];
                    arrID[0] = arrRecord[2].Trim();
                }

                objAsn.STUDY_ID = new Guid[arrID.Length];
                for (int i = 0; i< arrID.Length; i++)
                {
                    objAsn.STUDY_ID[i] = new Guid(arrID[i]);
                }


                objAsn.MENU_ID = Convert.ToInt32(arrRecord[3]);
                objAsn.USER_ID = new Guid(arrRecord[4]);

                bReturn = objAsn.LoadMultipleAssignmentRadiologist(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {

                    grdRad.DataSource = ds.Tables["Radiologists"];
                    grdRad.DataBind();
                    spnErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";
                }
                else
                    spnErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";

            }
            catch (Exception ex)
            {
                spnErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objAsn = null;
                objComm = null;
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
            string[] arrID = new string[0];

            objAsn = new Core.Radiologist.AssignStudy();
            objComm = new classes.CommonClass();

            try
            {
               
                objAsn.RADIOLOGIST_TYPE = ArrRecord[0].Trim();
                objAsn.RADIOLOGIST_ID = new Guid(ArrRecord[1]);

                if (ArrRecord[2].Trim().Contains(objComm.RecordDivider))
                {
                    arrID = ArrRecord[2].Split(objComm.RecordDivider);
                }
                else
                {
                    arrID = new string[1];
                    arrID[0] = ArrRecord[2].Trim();
                }
                objAsn.STUDY_ID = new Guid[arrID.Length];
                for (int i = 0; i < arrID.Length; i++)
                {
                    objAsn.STUDY_ID[i] = new Guid(arrID[i]);
                }

                objAsn.USER_ID = new Guid(ArrRecord[3].Trim());
                objAsn.MENU_ID = Convert.ToInt32(ArrRecord[4]);
                objAsn.USER_SESSION_ID = new Guid(ArrRecord[5].Trim());

                bReturn = objAsn.SaveMultipleAssignment(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    arrayCode = new string[1];
                    arrayCode[0] = strReturnMsg.Trim();
                    strMsg = objComm.SetErrorResources(arrayCode, "N", false, string.Empty, string.Empty);

                    arrRet = new string[3];
                    arrRet[0] = "true";
                    arrRet[1] = strMsg.Trim();
                    arrRet[2] = objAsn.ID.ToString();
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

                        arrRet = new string[3];
                        arrRet[0] = "false";
                        arrRet[1] = strMsg.Trim();
                        arrRet[2] = objAsn.USER_NAME;
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