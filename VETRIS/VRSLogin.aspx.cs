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
    [AjaxPro.AjaxNamespace("VRSLogin")]
    public partial class VRSLogin : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        Core.Login.Login objCore = null;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSLogin));
            SetPageValue();
            ClearCache();
            SetAttributes();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnLogin.Attributes.Add("onclick", "javascript:Login_Onclick();");
            txtPwd.Attributes.Add("onkeypress", "javascript:txtPwd_OnKeyPress(event);");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            this.Page.Title = "Login | " + ConfigurationManager.AppSettings["ProductHeading"];
            lblAppName.Text = ConfigurationManager.AppSettings["ProductHeading"];
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lblVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            Guid AcctID = Guid.Empty;
            string strTheme = string.Empty;
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            hdnDivider.Value = objComm.RecordDivider.ToString().Trim();
            hdnRootDirectory.Value = ConfigurationManager.AppSettings["RootDirectory"];
            objComm = null;

            if (CheckVersion())
            {
                if (Request.QueryString["UID"] != null)
                {
                    hdnUID.Value = Request.QueryString["UID"];
                    hdnInstCode.Value = Request.QueryString["INS"];
                    hdnMenuID.Value = Request.QueryString["MID"];
                    GetInstitutionUserPassword();
                }
                else if (Request.QueryString["AID"] != null)
                {
                    AcctID = new Guid(Request.QueryString["AID"]);
                    hdnMenuID.Value = "62";
                    GetBillingAccountCredentials(AcctID);
                }
                else if (Request.QueryString["tmp"] != null)
                {
                    hdnTempInstID.Value = Request.QueryString["tmp"];
                }
                else
                {
                    if (ReadCookies("vrsrememberid") == "1")
                    {
                        chkRemember.Checked = true;

                        txtLoginID.Text = ReadCookies("vrsLoginId");
                        txtPwd.Focus();
                    }
                    else
                    {
                        chkRemember.Checked = false;
                        txtLoginID.Focus();
                    }

                    strTheme = ReadCookies("vrsTheme");
                    if (strTheme == "")
                    {
                        lnkLOGIN.Attributes["href"] = "~/css/DEFAULT/login.css?v=" + DateTime.Now.Ticks.ToString(); 
                        imgLogo.Src = strServerPath + "/images/logo/logo_DEFAULT.png";
                        hdnTheme.Value = "DEFAULT";
                    }
                    else
                    {
                        lnkLOGIN.Attributes["href"] = "~/css/" + strTheme + "/login.css?v=" + DateTime.Now.Ticks.ToString(); 
                        imgLogo.Src = strServerPath + "/images/logo/logo_" + strTheme + ".png";
                        hdnTheme.Value = strTheme;
                    }

                }
            }

       
        }
        #endregion

        #region CheckVersion
        private bool CheckVersion()
        {
            bool bReturn = true;
            string strCatchMessage = ""; string strVersion = string.Empty;

            try
            {
                strVersion = CoreCommon.DATABASE_VERSION.Trim();
                if (strVersion == string.Empty)
                {
                    bReturn = CoreCommon.GetDBVersion(Server.MapPath("~"), ref strCatchMessage);

                    if (!bReturn)
                        hdnError.Value = "catch" + hdnDivider.Value + strCatchMessage;
                    else
                    {
                        strVersion = CoreCommon.DATABASE_VERSION.Trim();

                    }


                }

                if (lblVersion.Text != strVersion)
                {
                    hdnError.Value = "false" + hdnDivider.Value + "001";
                    hdnDBVer.Value = CoreCommon.DATABASE_VERSION.Trim();
                    btnLogin.Disabled = true;
                    bReturn = false;
                }

            }
            catch (Exception LexpErr)
            {
                bReturn = false;
                hdnError.Value = "catch" + hdnDivider.Value + LexpErr.Message;
            }
            finally
            {
                objComm = null;
                strCatchMessage = null;
            }
            return bReturn;
        }
        #endregion

        #region GetInstitutionUserPassword
        private void GetInstitutionUserPassword()
        {
            bool bReturn = false;
            string strReturnMsg = ""; string strCatchMessage = "";
            objCore = new Core.Login.Login();
            objComm = new classes.CommonClass();

            try
            {

                objCore.LOGIN_ID = hdnUID.Value.ToLower();
                objCore.INSTITUTION_CODE = hdnInstCode.Value;

                bReturn = objCore.FetchInstitutionUserPassword(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    txtPwd.Attributes.Add("value",objCore.CONFIRM_PASSWORD.Trim());
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

        #region GetBillingAccountCredentials
        private void GetBillingAccountCredentials(Guid ID)
        {
            bool bReturn = false;
            string strReturnMsg = ""; string strCatchMessage = "";
            objCore = new Core.Login.Login();
            objComm = new classes.CommonClass();

            try
            {

                objCore.BILLING_ACCOUNT_ID = ID;
                bReturn = objCore.FetchBillingAccountCredentials(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    txtLoginID.Text = objCore.LOGIN_ID.Trim();
                    txtPwd.Attributes.Add("value", objCore.CONFIRM_PASSWORD.Trim());
                    hdnUID.Value = objCore.LOGIN_ID.Trim();
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

        #region ValidateLogin (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] ValidateLogin(string[] strParams)
        {
            bool bReturn = false;
            string strReturnMsg = ""; string strCatchMessage = "";
            string strText1 = string.Empty; string strText2 = string.Empty;
            string[] arrRet = new string[0];
            DateTime dt = DateTime.Today.AddDays(7);
            objCore = new Core.Login.Login();

            try
            {

                objCore.LOGIN_ID = strParams[0].Trim().ToLower();
                //objCore.PASSWORD = strParams[1].Trim().ToLower();
                objCore.PASSWORD = strParams[1].Trim();

                bReturn = objCore.CheckLogin(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage, ref strText1, ref strText2);

                if (bReturn)
                {
                    arrRet = new string[8];
                    arrRet[0] = "true";
                    arrRet[1] = objCore.USER_ID.ToString();
                    arrRet[2] = objCore.USER_CODE.Trim();
                    arrRet[3] = objCore.USER_NAME.Trim();
                    arrRet[4] = dt.Year.ToString();
                    arrRet[5] = dt.Month.ToString();
                    arrRet[6] = dt.Day.ToString();
                    arrRet[7] = objCore.SESSION_ID.ToString();
                    Session["uid"] = objCore.USER_ID;
                }
                else
                {
                    arrRet = new string[4];
                    if (strCatchMessage.Trim() != "")
                    {
                        arrRet[0] = "catch";
                        arrRet[1] = strCatchMessage.Trim();
                    }
                    else
                    {
                        arrRet[0] = "false";
                        arrRet[1] = strReturnMsg.Trim();
                        arrRet[2] = strText1;
                        arrRet[3] = strText2;
                    }
                }

            }
            catch (Exception LexpErr)
            {
                bReturn = false;
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = LexpErr.Message.Trim();

            }
            finally
            {
                objCore = null;
                strReturnMsg = null; strCatchMessage = null;
            }
            return arrRet;
        }
        #endregion

        #region UserUnlock (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] UserUnlock(string[] ArrRecord)
        {
            bool bReturn = false;
            string[] arrRet = new string[3];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            int intCompanyID = 0;
            objCore = new Core.Login.Login();


            try
            {
                objCore.USER_EMAIL_ID =ArrRecord[0].Trim();

                bReturn = objCore.UserUnlock(Server.MapPath("~"), intCompanyID, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {

                    arrRet[0] = "true";
                    arrRet[1] = "046";
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
                        arrRet[1] = "047";
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
                objCore = null;
                strReturnMsg = null; strCatchMessage = null;
            }
            return arrRet;
        }
        #endregion 

        #region RegistrationLoginValidate (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] RegistrationLoginValidate(string strParam)
        {
            bool bReturn = false;
            string strReturnMsg = ""; string strCatchMessage = "";
            string strText1 = string.Empty; string strText2 = string.Empty;
            string[] arrRet = new string[0];
            DateTime dt = DateTime.Today.AddDays(7);
            objCore = new Core.Login.Login();

            try
            {
                objCore.Registration_ID = Guid.Parse(strParam);
                bReturn = objCore.GetRegistrationData(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage, ref strText1, ref strText2);

                if (bReturn)
                {
                    arrRet = new string[7];
                    arrRet[0] = "true";
                    arrRet[1] = objCore.USER_CODE.Trim();
                    arrRet[2] = objCore.USER_NAME.Trim();
                    arrRet[3] = dt.Year.ToString();
                    arrRet[4] = dt.Month.ToString();
                    arrRet[5] = dt.Day.ToString();
                    Session["uid"] = objCore.USER_CODE;
                }
                else
                {
                    arrRet = new string[3];
                    if (strCatchMessage.Trim() != "")
                    {
                        arrRet[0] = "catch";
                        arrRet[1] = strCatchMessage.Trim();
                    }
                    else
                    {
                        arrRet[0] = "false";
                        arrRet[1] = strReturnMsg.Trim();
                        arrRet[2] = strText1;
                        arrRet[3] = strText2;
                    }
                }
            }
            catch (Exception LexpErr)
            {
                bReturn = false;
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = LexpErr.Message.Trim();
            }
            finally
            {
                objCore = null;
                strReturnMsg = null; strCatchMessage = null;
            }
            return arrRet;
        }
        #endregion
    }
}