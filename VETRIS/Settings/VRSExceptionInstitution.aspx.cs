using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using VETRIS.Core;
using AjaxPro;

namespace VETRIS.Settings
{
    [AjaxPro.AjaxNamespace("VRSExceptionInstitution")]
    public partial class VRSExceptionInstitution : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Settings.Configuration objCore = new VETRIS.Core.Settings.Configuration();
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSExceptionInstitution));
            SetPageValue();
            SetAttributes();

        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            SetCSS(Request.QueryString["th"]);

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

        #region SetAttributes
        private void SetAttributes()
        {
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnOk1.Attributes.Add("onclick", "javascript:btnOk_OnClick();");
            btnOk2.Attributes.Add("onclick", "javascript:btnOk_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
        }
        #endregion

        #region CallBackInst_Callback
        protected void CallBackInst_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadData(e.Parameters);
            grdInst.Width = Unit.Percentage(99);
            grdInst.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadData
        private void LoadData(string[] arrParams)
        {
            objCore = new VETRIS.Core.Settings.Configuration();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                objCore.FILTER_BY = arrParams[0].Trim();
                objCore.RECORD_ID = Convert.ToInt32(arrParams[1]);
                objCore.SERVICE_ID = Convert.ToInt32(arrParams[2]);
                objCore.AFTER_HOURS = arrParams[3];
                objCore.MENU_ID = Convert.ToInt32(arrParams[4]);
                objCore.USER_ID = new Guid(arrParams[5]);

                bReturn = objCore.ExceptionInstitutionFetch(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    grdInst.DataSource = ds;
                    grdInst.DataBind();
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

        }
        #endregion

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord, string[] ArrData)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string[] arrayCode = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            string strMsg = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Settings.Configuration();
            objComm = new classes.CommonClass();
            Core.Settings.ExceptionInstitutions[] objInst = new Core.Settings.ExceptionInstitutions[0];

            try
            {
                if (ArrData.Length > 0)
                {
                    objCore.SERVICE_ID = Convert.ToInt32(ArrRecord[0]);
                    objCore.RECORD_ID = Convert.ToInt32(ArrRecord[1]);
                    objCore.AFTER_HOURS = ArrRecord[2].Trim();
                    objCore.AVAILABLE = ArrRecord[3].Trim();
                    objCore.DISPLAY_MESSAGE = ArrRecord[4].Trim();
                    objCore.FILTER_BY = ArrRecord[5].Trim();
                    objCore.USER_ID = new Guid(ArrRecord[6].Trim());
                    objCore.MENU_ID = Convert.ToInt32(ArrRecord[7].Trim());

                    objInst = new Core.Settings.ExceptionInstitutions[(ArrData.Length)];

                    #region Populate Institutions
                    for (int i = 0; i < objInst.Length; i++)
                    {
                        objInst[i] = new Core.Settings.ExceptionInstitutions();
                        objInst[i].id = new Guid(ArrData[intListIndex]);
                        intListIndex = intListIndex + 1;
                    }
                    #endregion

                    bReturn = objCore.SaveExceptionInstitutions(Server.MapPath("~"), objInst, ref strReturnMsg, ref strCatchMessage);

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
                            arrayCode = new string[1];
                            arrayCode[0] = strCatchMessage.Trim();
                            strMsg = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);

                            arrRet = new string[2];
                            arrRet[0] = "catch";
                            arrRet[1] = strMsg;
                        }
                        else
                        {
                            arrayCode = new string[1];
                            arrayCode[0] = strReturnMsg.Trim();
                            strMsg = objComm.SetErrorResources(arrayCode, "N", true, objCore.USER_NAME, string.Empty);

                            arrRet = new string[2];
                            arrRet[0] = "false";
                            arrRet[1] = strMsg;
                        }
                    }
                }
                else
                {
                    arrayCode = new string[1];
                    arrayCode[0] = "218";
                    strMsg = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);

                    arrRet = new string[2];
                    arrRet[0] = "false";
                    arrRet[1] = strMsg;
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
                objCore = null; objComm = null; objInst = null;
                strReturnMsg = null; strCatchMessage = null;
            }



            return arrRet;
        }
        #endregion
    }
}