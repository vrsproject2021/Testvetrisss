using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using AjaxPro;
using VETRIS.Core;
using System.Data;

namespace VETRIS
{
    [AjaxPro.AjaxNamespace("VRSRegistrationChangePwd")]
    public partial class VRSRegistrationChangePwd : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        Core.Login.Login objCore = null;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSRegistrationChangePwd));
            SetPageValue();
            ClearCache();
            SetAttributes();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnChange.Attributes.Add("onclick", "javascript:btnChangePwd_OnClick();");
            //txtPwd.Attributes.Add("onkeypress", "javascript:txtPwd_OnKeyPress(event);");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            //this.Page.Title = "Registration Change Password  | " + ConfigurationManager.AppSettings["ProductHeading"];
            lblAppName.Text = ConfigurationManager.AppSettings["ProductHeading"];

            Guid AcctID = Guid.Empty;
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            hdnDivider.Value = objComm.RecordDivider.ToString().Trim();
            hdnRootDirectory.Value = ConfigurationManager.AppSettings["RootDirectory"];
            objComm = null;

            //hdnTempInstID.Value = Request.QueryString["tmp"];
            hdnTempInstID.Value = "72CA47DF-7693-41D0-BE72-98CA572A9EDC";
            GetRegistrationInstitutionDetail(hdnTempInstID.Value);
            
            //else
            //{
            //    if (ReadCookies("vrsrememberid") == "1")
            //    {
            //        //chkRemember.Checked = true;

            //        //txtLoginID.Text = ReadCookies("vrsLoginId");
            //        //txtPwd.Focus();
            //    }
            //    else
            //    {
            //        //chkRemember.Checked = false;
            //        //txtLoginID.Focus();
            //    }
            //}

       
        }
        #endregion

        #region GetRegistrationInstitutionDetail
        private void GetRegistrationInstitutionDetail(string insId)
        {
            bool bReturn = false;
            string strReturnMsg = ""; string strCatchMessage = "";
            DataSet ds = new DataSet();
            var objCore = new Core.Case.CaseStudy();

            objComm = new classes.CommonClass();
            int i = 0;
            string[] arrRet = new string[0];
            try
            {

                objCore.INSTITUTION_ID = new Guid(insId.Trim());
                bReturn = objCore.FetchRegInstitutionDetails(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    if (ds.Tables["Institutions_Reg"].Rows.Count > 0)
                    {
                        txtInstitutionName.Attributes.Add("value", Convert.ToString(ds.Tables["Institutions_Reg"].Rows[0]["name"]));
                        hdnLoginId.Attributes.Add("value", Convert.ToString(ds.Tables["Institutions_Reg"].Rows[0]["login_id"]));
                        hdnEmailId.Attributes.Add("value", Convert.ToString(ds.Tables["Institutions_Reg"].Rows[0]["email_id"]));
                        txtUserName.Attributes.Add("value", Convert.ToString(ds.Tables["Institutions_Reg"].Rows[0]["login_id"]));
                        txtPassword.Attributes.Add("value",CoreCommon.DecryptString(Convert.ToString(ds.Tables["Institutions_Reg"].Rows[0]["login_password"])));
                        Session["uid"] = insId;
                    }
                    else
                    {
                        arrRet[i] = "N";
                        arrRet[i + 1] = "";
                        arrRet[i + 2] = "";
                    }
                }
                else
                {

                    if (strCatchMessage.Trim() != "")
                    {
                        hdnError.Value = "catch" + objComm.RecordDivider + strCatchMessage.Trim();
                    }
                    else
                    {
                        hdnError.Value = "false" + objComm.RecordDivider + strReturnMsg.Trim();
                    }
                }

            }
            catch (Exception expErr)
            {
                bReturn = false;
                hdnError.Value = "catch" + objComm.RecordDivider + expErr.Message.Trim();

            }
            finally
            {
                objCore = null;
                strReturnMsg = null; strCatchMessage = null;
            }

        }
        #endregion

        

        #region ClearCache
        private void ClearCache()
        {
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
        }
        #endregion

        #region ReadCookies
        /**************************************************************************************
        Method Name        :ReadCookies
        Written by	       :Pavel Guha
        Written on         :16/09/2009
        Modified by	       :<Last modified person>
        Modified on	       :<date the method was last modified>
        Input parameters   : 	
        Output parameters  : 	
        Returns            : 	
        Description        :reads the cookies to retrieve the login name
        ****************************************************************************************/
        private string ReadCookies(string sCookieName)
        {
            if (Request.Cookies[sCookieName] == null)
                return ("");

            return (Request.Cookies[sCookieName].Value);

        }
        #endregion

        #region SetCookies
        /**************************************************************************************
        Method Name        :ReadCookies
        Written by	       :Pavel Guha
        Written on         :16/09/2009
        Modified by	       :<Last modified person>
        Modified on	       :<date the method was last modified>
        Input parameters   : 	
        Output parameters  : 	
        Returns            : 	
        Description        :reads the cookies to retrieve the login name
        ****************************************************************************************/
        private void SetCookies(string strName, string strValue)
        {
            HttpCookie sCookie = new HttpCookie(strName);
            DateTime dtNow = DateTime.Now;

            // Set the cookie value.
            sCookie.Value = strValue;
            // Set the cookie expiration date.
            sCookie.Expires = dtNow.AddDays(7);
            // Add the cookie.
            Response.Cookies.Add(sCookie);

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
                objCore.Registration_ID = new Guid(ArrRecord[0]);
                objCore.REG_LOGIN_ID = Convert.ToString(ArrRecord[1]);
                objCore.PASSWORD = Convert.ToString(ArrRecord[2]).ToLower();
                objCore.NEW_PASSWORD = Convert.ToString(ArrRecord[3]).ToLower();
                objCore.CONFIRM_PASSWORD = Convert.ToString(ArrRecord[4]).ToLower();
                objCore.USER_EMAIL_ID = Convert.ToString(ArrRecord[5]).ToLower();

                bReturn = objCore.RegistrationChangePassword(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

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