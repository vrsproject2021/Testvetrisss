using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using VETRIS.Core;
using AjaxPro;

namespace VETRIS
{
    [AjaxPro.AjaxNamespace("VRSForgotPwd")]
    public partial class VRSForgotPwd : System.Web.UI.Page
    {
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSForgotPwd));
            lblHdr.Text = ConfigurationManager.AppSettings["WindowTitle"] + " : Retrieve Password";
            txtLoginID.Text = Request.QueryString["id"];
            SetAttributes();
            SetCss(Request.QueryString["th"]);
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {

            txtLoginID.Attributes.Add("onchange", "javascript:GetUserEmailID();");
        }
        #endregion

        #region SetCss
        private void SetCss(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";

        }
        #endregion

        #region GetUserEmailID(Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[]  GetUserEmailID(string strLoginID)
        {
            string[] arrRet = new string[3];
            Core.Login.Login objCore = new Core.Login.Login();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            classes.CommonClass objComm = new classes.CommonClass();

            try
            {
                objCore.LOGIN_ID = strLoginID.Trim();

                bReturn = objCore.FetchUserEmailID(Server.MapPath("~"), ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                   
                    arrRet[0] = "true";
                    arrRet[1] = objCore.USER_EMAIL_ID;
                }
                else
                {

                    string[] larry = new string[1];
                    if (strReturnMessage.Trim() == string.Empty)
                    {
                        arrRet[0] = "false";
                        larry[0] = strCatchMessage.Trim();
                    }
                    else
                    {
                        arrRet[0] = "catch";
                        larry[0] = strReturnMessage.Trim();
                    }
                    
                    
                    arrRet[1] = objComm.SetErrorResources(larry, "N", true, "", "");
                }
            }
            catch (Exception expErr)
            {
                bReturn = false;
                string[] larry = new string[1];
                larry[0] = expErr.Message.ToString();
                arrRet[0] = "catch";
                arrRet[1] = objComm.SetErrorResources(larry, "N", true, "", "");

            }
            finally
            {
                objCore = null; 
                strReturnMessage = null; strCatchMessage = null;
            }
            return arrRet;
        }
        #endregion

        #region MailPassWord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] MailPassWord(string[] ArrRecord)
        {
            bool bReturn = false;
            string[] arrRet = new string[3];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            Core.Login.Login objCore = new Core.Login.Login();
            classes.CommonClass objComm = new classes.CommonClass();

            try
            {
                objCore.LOGIN_ID = ArrRecord[0].Trim();
                objCore.USER_EMAIL_ID = ArrRecord[1].Trim();
                bReturn = objCore.ForgotPassword(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    string[] larry = new string[1];
                    larry[0] = "009";
                    arrRet[0] = "true";
                    arrRet[1] = objComm.SetErrorResources(larry, "N", false, "", "");
                }
                else
                {

                    string[] larry = new string[0];
                    if (strReturnMsg.Contains(objComm.RecordDivider))
                        larry = strReturnMsg.Trim().Split(objComm.RecordDivider);
                    else
                    {
                        larry = new string[1];
                        larry[0] = strReturnMsg.Trim();
                    }
                    arrRet[0] = "catch";
                    arrRet[1] = objComm.SetErrorResources(larry, "N", true, "", "");
                }
            }
            catch (Exception expErr)
            {
                bReturn = false;
                string[] larry = new string[1];
                larry[0] = expErr.Message.ToString();
                arrRet[0] = "catch";
                arrRet[1] = objComm.SetErrorResources(larry, "N", true, "", "");

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