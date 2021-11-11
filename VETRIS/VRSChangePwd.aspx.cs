using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using AjaxPro;
using VETRIS.Core;

namespace VETRIS
{
     [AjaxPro.AjaxNamespace("VRSChangePwd")]
    public partial class VRSChangePwd : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        Core.Login.Login objCore;
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {

            Utility.RegisterTypeForAjax(typeof(VRSChangePwd));
            SetPageValue();
            SetAttributes();

        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            if (Request.QueryString["uid"] != null) hdnID.Value = Request.QueryString["uid"];
            if (Request.QueryString["unm"] != null) txtUserName.Text = Convert.ToString(Request.QueryString["unm"]);
            SetCss(Request.QueryString["th"]);
        }
        #endregion

        #region SetCss
        private void SetCss(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";

        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnClose1.Attributes.Add("onclick", "jabvascript:btnClose_OnClick();");
           // btnClose2.Attributes.Add("onclick", "jabvascript:btnClose_OnClick();");
            btnChange.Attributes.Add("onclick", "javascript:btnChangePwd_OnClick();");
        }
        #endregion

        #region ChangePassword (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] ChangePassword(string[] ArrRecord)
        {
            bool bReturn = false;
            string[] arrRet = new string[2];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            objComm = new classes.CommonClass();
            objCore = new Core.Login.Login();

            try
            {
                objCore.USER_ID = new Guid(ArrRecord[0]);
                objCore.PASSWORD = Convert.ToString(ArrRecord[1]);
                objCore.NEW_PASSWORD = Convert.ToString(ArrRecord[2]);
                objCore.CONFIRM_PASSWORD = Convert.ToString(ArrRecord[3]);

                bReturn = objCore.ChangePassword(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
                }
                else
                {
                    if (strCatchMessage.Trim() != "")
                    {
                        arrRet[0] = "catch";
                        arrRet[1] = strCatchMessage.Trim();
                    }
                    else
                    {
                        arrRet[0] = "false";
                        arrRet[1] = strReturnMsg;
                    }
                }
            }
            catch (Exception expErr)
            {
                bReturn = false;
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