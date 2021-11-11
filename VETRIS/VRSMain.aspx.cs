using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Text;
using System.IO;
using System.Net;
using VETRIS.Core;
using eRADCls;
using AjaxPro;

namespace VETRIS
{
    [AjaxPro.AjaxNamespace("VRSMain")]
    public partial class VRSMain : System.Web.UI.Page
    {
        #region Variables
        classes.CommonClass objComm;
        Core.Login.Login objCore = null;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSMain));
            // SetAttributes();
            SetPageValue();
            ClearCache();
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            this.Page.Title = ConfigurationManager.AppSettings["ProductHeading"];
            lblVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            if (Session["uid"] == null)
            {
                if (Request.QueryString["ref"] == null) Response.Redirect("VRSLogin.aspx");
            }
            else
                Session.Abandon();

            lblAppName.Text = ConfigurationManager.AppSettings["ProductHeading"];
            hdnRootDirectory.Value = ConfigurationManager.AppSettings["RootDirectory"];
            hdnServerPath.Value = ConfigurationManager.AppSettings["ServerPath"];


            if (Request.QueryString["uid"] != null) hdnUserID.Value = Request.QueryString["uid"];
            if (Request.QueryString["mid"] != null) hdnMenuID.Value = Request.QueryString["mid"];
            if (Request.QueryString["sid"] != null) hdnSessionID.Value = Request.QueryString["sid"];


            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();


            hdnDecPlaces.Value = objComm.DecimalDigit.ToString();
            hdnDateFormat.Value = objComm.DateFormat;
            hdnDateSep.Value = objComm.DateSeparator;
            hdnDivider.Value = objComm.RecordDivider.ToString();
            hdnSecDivider.Value = objComm.SecondaryRecordDivider.Trim();

            objComm = null;
            
            if (Request.QueryString["tmpid"] == null)
            {
                FetchPostLoginData();
            }
            else
            {
                objComm = new classes.CommonClass(); objComm.SetRegionalFormat();
                hdnUserID.Value = "00000000-0000-0000-0000-000000000000";
                hdnTempInstID.Value = Request.QueryString["tmpid"].ToString();
                hdnUserCode.Value = Request.QueryString["ucd"].ToString();
                hdnUserName.Value = Request.QueryString["unm"].ToString();
                spnUserName.InnerText = objComm.ToInitcap(hdnUserName.Value);
                objComm = null;
                DeleteUserDirectory(new Guid(hdnUserID.Value.ToString()));
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

        #region FetchPostLoginData
        private void FetchPostLoginData()
        {
            DataSet ds = new DataSet();
            bool bReturn = true;
            string strSessionID = string.Empty;
            string strReturnMessage = string.Empty;
            string strCatchMessage = string.Empty;
            objCore = new Core.Login.Login();
            objComm = new classes.CommonClass(); objComm.SetRegionalFormat();


            try
            {
                objCore.USER_ID = new Guid(hdnUserID.Value.ToString());

                bReturn = objCore.FetchPostLoginData(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    hdnUserRoleID.Value = objCore.USER_ROLE_ID.ToString();
                    hdnUserRoleCode.Value = objCore.USER_ROLE_CODE.Trim();
                    hdnUserCode.Value = objCore.USER_CODE.Trim();
                    hdnUserName.Value = objCore.USER_NAME.Trim();
                    hdnUserContNo.Value = objCore.USER_CONTACT_NUMBER.Trim();
                    hdnPACSUID.Value = objCore.PACS_USER_ID.Trim();
                    hdnPACSPwd.Value = objCore.PACS_USER_PASSWORD.Trim();
                    hdnAllowMS.Value = objCore.ALLOW_MANUAL_SUBMISSION.Trim();
                    hdnAllowDB.Value = objCore.ALLOW_DASHBOARD_VIEW.Trim();
                    spnUserName.InnerText = objComm.ToInitcap(hdnUserName.Value);
                    hdnInstCode.Value = objCore.INSTITUTION_CODE;
                    hdnInstName.Value = objCore.INSTITUTION_NAME;
                    hdnBillAcctID.Value = objCore.BILLING_ACCOUNT_ID.ToString();
                    hdnBillAcctName.Value = objCore.BILLING_ACCOUNT_NAME;
                    hdnRadiologistID.Value = objCore.RADIOLOGIST_ID.ToString();
                    hdnRadiologistTimeZone.Value = objCore.RADIOLOGIST_TIMEZONE.ToString();
                    hdnCHATURL.Value = objCore.CHAT_URL;
                    hdnENBLCHAT.Value = objCore.ENABLE_CHAT;
                    hdnAPIVER.Value = objCore.API_VERSION;
                    hdnWS8SRVIP.Value = objCore.WEB_SERVICE_SERVER_URL;
                    hdnWS8CLTIP.Value = objCore.WEB_SERVICE_CLIENT_URL;
                    hdnWS8SRVUID.Value = objCore.WEB_SERVICE_USER_ID;
                    hdnWS8SRVPWD.Value = CoreCommon.DecryptString(objCore.WEB_SERVICE_PASSWORD);
                    hdnWS8Session.Value = objCore.WEB_SERVICE_SESSION_ID;
                    hdnRPTEGNURL.Value = objCore.REPORT_ENGINE_URL;
                    hdnDefTZID.Value = objCore.DEFAULT_TIME_ZONE_ID.ToString();
                    hdnDefTZStdName.Value = objCore.DEFAULT_TIME_ZONE_STANDARD_NAME.ToString();
                    hdnPrefTheme.Value = objCore.PREFERED_THEME;

                    SetCss();
                    App_Menu.InnerHtml = PopulateMenu(ds.Tables["Menu"]);
                    mobile_menu.InnerHtml = PopulateMobileMenu(ds.Tables["Menu"]);

                    foreach (DataRow dr in ds.Tables["DashboardSettings"].Rows)
                    {
                        hdnDashboardId.Value = Convert.ToString(dr["id"]);
                    }

                    //if (objCore.API_VERSION == "8")
                    //{
                    //    if (objCore.WEB_SERVICE_SESSION_ID == string.Empty)
                    //    {
                    //        if (CreateSession(ref strSessionID, ref strReturnMessage, ref strCatchMessage))
                    //        {
                    //            hdnWS8Session.Value = strSessionID;
                    //            if (!UpdateWebServiceSession(ref strReturnMessage, ref strCatchMessage))
                    //            {
                    //                if (strCatchMessage.Trim() != string.Empty)
                    //                {
                    //                    hdnError.Value = "catch" + objComm.RecordDivider + strCatchMessage.Trim();
                    //                }
                    //                else
                    //                {
                    //                    hdnError.Value = "false" + objComm.RecordDivider + strReturnMessage.Trim();
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                }
                else
                {
                    hdnError.Value = "catch" + objComm.RecordDivider + strCatchMessage.Trim();
                }

                DeleteUserDirectory(new Guid(hdnUserID.Value.ToString()));
            }
            catch (Exception ex)
            {
                hdnError.Value = "catch" + objComm.RecordDivider + ex.Message;
            }
            finally
            {
                ds.Dispose(); objComm = null; objCore = null;
            }
        }
        #endregion

        #region SetCss
        private void SetCss()
        {
            string strServerPath = hdnServerPath.Value;
            lnkSTYLE.Attributes["href"] = "~/css/" + hdnPrefTheme.Value + "/style.css?v=" + DateTime.Now.Ticks.ToString();
            lnkLBOX.Attributes["href"] = "~/css/" + hdnPrefTheme.Value + "/lightbox.css?v=" + DateTime.Now.Ticks.ToString();
            imgLogo.Src = strServerPath + "/images/logo/logo_" + hdnPrefTheme.Value + ".png";
            imgLogoSN.Src = strServerPath + "/images/logo/logosn_" + hdnPrefTheme.Value + ".png";
            imgMainLogo.Src = strServerPath + "/images/logo/logo_" + hdnPrefTheme.Value + ".png";
        }
        #endregion

        #region PopulateMenu
        public string PopulateMenu(DataTable dtbl)
        {
            string strReturn = string.Empty;

            int intPreDefMenuID = Convert.ToInt32(hdnMenuID.Value);
            string strPreDefMenu = "N";
            int intMenuID0 = 0;

            string strMenu0Desc = string.Empty;
            int intMenuID1 = 0;
            string strMenu1Desc = string.Empty;

            StringBuilder sb = new StringBuilder();
            string strCSS = string.Empty;
            string strIco = string.Empty;
            string strIsDD = string.Empty;
            string strShowRC = string.Empty;
            string strNavMethod = string.Empty;
            int intRecCount = 0;

            if (dtbl.Rows.Count > 0)
            {
                sb.Append("<nav class='sidebar-nav left-sidebar-menu-pro'>");
                sb.Append("<ul class='metismenu' id='menu1'>");



                DataView dv0 = new DataView(dtbl);
                dv0.RowFilter = "menu_level=0";
                dv0.Sort = "display_index asc";

                foreach (DataRow dr0 in dv0.ToTable().Rows)
                {
                    intMenuID0 = Convert.ToInt32(dr0["menu_id"]);
                    strMenu0Desc = Convert.ToString(dr0["menu_desc"]);
                    strIco = Convert.ToString(dr0["menu_icon"]);
                    strIsDD = Convert.ToString(dr0["is_dropdown"]);
                    strNavMethod = Convert.ToString(dr0["nav_method"]);

                    DataView dv1 = new DataView(dtbl);
                    dv1.RowFilter = "menu_level=1 and parent_id=" + intMenuID0.ToString();
                    if (intMenuID0 == 46) hdnDLDRENBL.Value = "Y";
                    if (intPreDefMenuID > 0 && strPreDefMenu == "N")
                    {
                        if (intMenuID0 == intPreDefMenuID) strPreDefMenu = "Y";
                    }

                    if (Convert.ToString(dr0["nav_url"]).Trim() != string.Empty)
                    {
                        sb.Append("<li>");
                        if(strNavMethod =="PA")
                            sb.Append("<a title='" + strMenu0Desc + " href='javascript:void(0);' onclick=\"javascript:PopupApplication('" + intMenuID1.ToString() + "','" + strMenu0Desc + "')\" aria-expanded='false' style='cursor:pointer;'>");
                        else
                            sb.Append("<a title='" + strMenu0Desc + " href='javascript:void(0);' onclick=\"javascript:NavMenu('" + Convert.ToString(dr0["nav_url"]) + "','" + intMenuID1.ToString() + "','" + Convert.ToString(dr0["is_browser"]) + "','1','N')\" aria-expanded='false' style='cursor:pointer;'>");

                        //sb.Append("<span class='educate-icon educate-event icon-wrap sub-icon-mg' aria-hidden='true'></span>");
                        sb.Append("<div class='menu_icon'><img src='images/menu/" + strIco + "' alt='' title='" + strMenu0Desc + "' class='img-responsive'/></div>&nbsp;");
                        sb.Append("<span class='mini-click-non menu_name'>" + strMenu0Desc + "</span>");
                        sb.Append("</a>");
                        sb.Append("</li>");
                    }
                    else
                    {
                        if (strIsDD == "Y")
                        {
                            sb.Append("<li>");
                            sb.Append("<a title='" + strMenu0Desc + "' class='has-arrow' href='javascript:void(0);'>");
                            sb.Append("<div class='menu_icon'><img src='images/menu/" + strIco + "' alt='' title='" + strMenu0Desc + "' class='img-responsive'/></div>&nbsp;");
                            sb.Append("<span class='mini-click-non menu_name'>" + strMenu0Desc + "</span>");
                            sb.Append("</a>");

                            #region First Level Menu
                            dv1.Sort = "display_index asc";

                            if (dv1.ToTable().Rows.Count > 0)
                            {
                                sb.Append("<ul class='submenu-angle collapse' aria-expanded='false'>");

                                foreach (DataRow dr1 in dv1.ToTable().Rows)
                                {

                                    intMenuID1 = Convert.ToInt32(dr1["menu_id"]);
                                    strMenu1Desc = Convert.ToString(dr1["menu_desc"]);
                                    strShowRC = Convert.ToString(dr1["show_rec_count"]);
                                    intRecCount = Convert.ToInt32(dr1["record_count"]);
                                    strNavMethod = Convert.ToString(dr1["nav_method"]);

                                    if (intMenuID1 == 46) hdnDLDRENBL.Value = "Y";
                                    if (intPreDefMenuID > 0 && strPreDefMenu == "N")
                                    {
                                        if (intMenuID1 == intPreDefMenuID) strPreDefMenu = "Y";
                                    }

                                    sb.Append("<li>");
                                    if (strShowRC == "Y")
                                    {
                                        sb.Append("<a href='javascript:void(0);' onclick=\"javascript:NavMenu('" + Convert.ToString(dr1["nav_url"]) + "','" + intMenuID1.ToString() + "','" + Convert.ToString(dr1["is_browser"]) + "','1','N')\">");
                                        sb.Append("<span class='mini-sub-pro' style='position: relative;'>");
                                        sb.Append("<span style='z-index: 2;'>" + Convert.ToString(dr1["menu_desc"]) + "</span>");
                                        sb.Append("<span id='spnCnt_" + intMenuID1.ToString() + "' style='color: #fff; background: #ff0000; border-radius: 50%; width: 20px;height: 20px; padding: 3px; font-size: 11px;position: absolute; z-index: 1; top: -10px;right: -20px;'>" + objComm.padZero(intRecCount) + "</span>");
                                        sb.Append("</span>");
                                        sb.Append("</a>");
                                    }
                                    else
                                    {
                                        if (strNavMethod == "PA")
                                            sb.Append("<a href='javascript:void(0);' onclick=\"javascript:PopupApplication('" + intMenuID1.ToString() + "','" + strMenu1Desc + "')\">" + Convert.ToString(dr1["menu_desc"]) + "</a>");
                                        else
                                            sb.Append("<a href='javascript:void(0);' onclick=\"javascript:NavMenu('" + Convert.ToString(dr1["nav_url"]) + "','" + intMenuID1.ToString() + "','" + Convert.ToString(dr1["is_browser"]) + "','1','N')\">" + Convert.ToString(dr1["menu_desc"]) + "</a>");
                                    }
                                    sb.Append("</li>");

                                }

                                sb.Append("</ul>");
                                sb.Append("</li>");
                            }
                            #endregion

                            dv1.Dispose();
                            //sb.Append("</li>");
                        }
                        //else
                        //{
                        //    sb.Append("<li class='sub'>");
                        //    sb.Append("<a href='#'><div class='menu_icon'><img src='images/" + strIco + "' alt='' class='img-responsive' style='height:24px;width:24px;'/></div>");
                        //    sb.Append("<div class='menu_name'>" + strMenu0Desc + "</div>");
                        //    sb.Append("</a>");
                        //    //sb.Append("</li>");
                        //}
                    }
                }

                sb.Append("</ul>");
                sb.Append("</nav>");

                dv0.Dispose();
            }

            strReturn = sb.ToString();
            hdnPreDefMenu.Value = strPreDefMenu;
            return strReturn;
        }
        #endregion

        #region PopulateMobileMenu
        public string PopulateMobileMenu(DataTable dtbl)
        {
            string strReturn = string.Empty;

            int intPreDefMenuID = Convert.ToInt32(hdnMenuID.Value);
            string strPreDefMenu = "N";
            int intMenuID0 = 0;

            string strMenu0Desc = string.Empty;
            int intMenuID1 = 0;
            string strMenu1Desc = string.Empty;
            int intDDMenuCnt = 0;

            StringBuilder sb = new StringBuilder();
            string strCSS = string.Empty;
            string strIco = string.Empty;
            string strIsDD = string.Empty;
            string strShowRC = string.Empty;
            int intRecCount = 0;

            if (dtbl.Rows.Count > 0)
            {

                sb.Append("<ul class='nav navbar-nav'>");

                DataView dv0 = new DataView(dtbl);
                dv0.RowFilter = "menu_level=0";
                dv0.Sort = "display_index asc";

                foreach (DataRow dr0 in dv0.ToTable().Rows)
                {
                    intMenuID0 = Convert.ToInt32(dr0["menu_id"]);
                    strMenu0Desc = Convert.ToString(dr0["menu_desc"]);
                    strIco = Convert.ToString(dr0["menu_icon"]);
                    strIsDD = Convert.ToString(dr0["is_dropdown"]);
                    DataView dv1 = new DataView(dtbl);
                    dv1.RowFilter = "menu_level=1 and parent_id=" + intMenuID0.ToString();
                    if (intPreDefMenuID > 0 && strPreDefMenu == "N")
                    {
                        if (intMenuID0 == intPreDefMenuID) strPreDefMenu = "Y";
                    }

                    if (Convert.ToString(dr0["nav_url"]).Trim() != string.Empty)
                    {
                        sb.Append("<li>");

                        sb.Append("<a href='javascript:void(0);' onclick=\"javascript:SetMobleMainMenu();NavMenu('" + Convert.ToString(dr0["nav_url"]) + "','" + intMenuID1.ToString() + "','" + Convert.ToString(dr0["is_browser"]) + "','1','N')\">" + strMenu0Desc);
                        sb.Append("<div class='menu_icon'><img src='images/menu/" + strIco + "' alt='' title='" + strMenu0Desc + "' class='img-responsive'/></div>&nbsp;");
                        sb.Append("</a>");
                        sb.Append("</li>");
                    }
                    else
                    {
                        if (strIsDD == "Y")
                        {
                            intDDMenuCnt = intDDMenuCnt + 1;
                            sb.Append("<li>");
                            sb.Append("<a href='javascript:void(0);' onclick=\"javascript:ToggleMobileMenu('" + intDDMenuCnt.ToString() + "');\">" + strMenu0Desc);
                            sb.Append("<div class='menu_icon'><img src='images/menu/" + strIco + "' alt='' title='" + strMenu0Desc + "' class='img-responsive'/></div>&nbsp;");
                            sb.Append("</a>");



                            #region First Level Menu
                            dv1.Sort = "display_index asc";

                            if (dv1.ToTable().Rows.Count > 0)
                            {
                                sb.Append("<ul id='mobmenugrp_" + intDDMenuCnt.ToString() + "'>");

                                foreach (DataRow dr1 in dv1.ToTable().Rows)
                                {

                                    intMenuID1 = Convert.ToInt32(dr1["menu_id"]);
                                    strMenu1Desc = Convert.ToString(dr1["menu_desc"]);
                                    strShowRC = Convert.ToString(dr1["show_rec_count"]);
                                    intRecCount = Convert.ToInt32(dr1["record_count"]);
                                    if (intPreDefMenuID > 0 && strPreDefMenu == "N")
                                    {
                                        if (intMenuID1 == intPreDefMenuID) strPreDefMenu = "Y";
                                    }

                                    sb.Append("<li>");
                                    if (strShowRC == "Y")
                                    {
                                        sb.Append("<a href='javascript:void(0);' onclick=\"javascript:SetMobleMainMenu();NavMenu('" + Convert.ToString(dr1["nav_url"]) + "','" + intMenuID1.ToString() + "','" + Convert.ToString(dr1["is_browser"]) + "','1')\">");
                                        sb.Append(Convert.ToString(dr1["menu_desc"]));
                                        sb.Append("<span id='spnCnt_" + intMenuID1.ToString() + "' style='color: #fff; background: #ff0000; border-radius: 50%; width: 20px;height: 20px; padding: 3px; font-size: 11px;position: absolute; z-index: 1; top: -10px;right: -20px;'>" + objComm.padZero(intRecCount) + "</span>");
                                        sb.Append("</a>");
                                    }
                                    else
                                    {
                                        sb.Append("<a href='javascript:void(0);' onclick=\"javascript:SetMobleMainMenu();NavMenu('" + Convert.ToString(dr1["nav_url"]) + "','" + intMenuID1.ToString() + "','" + Convert.ToString(dr1["is_browser"]) + "','1')\">" + Convert.ToString(dr1["menu_desc"]) + "</a>");
                                    }
                                    sb.Append("</li>");

                                }

                                sb.Append("</ul>");
                                sb.Append("</li>");
                            }
                            #endregion

                            dv1.Dispose();
                            //sb.Append("</li>");
                        }

                    }
                }

                sb.Append("</ul>");


                dv0.Dispose();
            }
            sb.Append("<input type='hidden' id='hdnDDMenuCnt' value='" + intDDMenuCnt.ToString() + "'/>");
            strReturn = sb.ToString();
            hdnPreDefMenu.Value = strPreDefMenu;
            return strReturn;
        }
        #endregion

        #region CreateSession
        private bool CreateSession(ref string SessionID, ref string ErrorMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            RadWebClass client = new RadWebClass();
            string strPassword = string.Empty;

            try
            {
                //strPassword = CoreCommon.DecryptString(hdnWS8SRVPWD.Value);
                bReturn = client.GetSession(hdnWS8CLTIP.Value, hdnWS8SRVIP.Value, hdnWS8SRVUID.Value, hdnWS8SRVPWD.Value, ref SessionID, ref CatchMessage, ref ErrorMessage);
            }
            catch (Exception ex)
            {
                bReturn = false;
                CatchMessage = ex.Message;
            }
            finally { client = null; }

            return bReturn;
        }
        #endregion

        #region UpdateWebServiceSession
        private bool UpdateWebServiceSession(ref string strReturnMsg, ref string strCatchMessage)
        {
            bool bReturn = false;
            try
            {
                objCore.WEB_SERVICE_SESSION_ID = hdnWS8Session.Value.Trim();
                objCore.USER_ID = new Guid(hdnUserID.Value.ToString());
                bReturn = objCore.UpdateWebServiceSession(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);
            }
            catch (Exception expErr)
            {
                bReturn = false;
                strCatchMessage = expErr.Message;
            }

            return bReturn;
        }
        #endregion

        #region FetchMenuRecordCount (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchMenuRecordCount(string[] arrParams)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objCore = new Core.Login.Login();
            int i = 0;

            string[] arrRet = new string[0];

            try
            {

                objCore.USER_ID = new Guid(arrParams[0]);
                bReturn = objCore.FetchMenuRecordCount(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[(ds.Tables["RecordCount"].Rows.Count * 2) + 1];
                    arrRet[0] = "true";
                    i = 1;
                    foreach (DataRow dr in ds.Tables["RecordCount"].Rows)
                    {
                        arrRet[i] = Convert.ToString(dr["menu_id"]);
                        arrRet[i + 1] = Convert.ToString(dr["record_count"]);
                        i = i + 2;
                    }
                }
                else
                {
                    arrRet = new string[2];
                    arrRet[0] = "false";
                    arrRet[1] = strCatchMessage.Trim(); ;
                }

            }
            catch (Exception ex)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return arrRet;
        }
        #endregion

        #region DeleteUserDirectory
        private void DeleteUserDirectory(Guid UserID)
        {
            string[] arrTemp = new string[0];

            try
            {
                #region Delete files under "Study"
                if (Directory.Exists(Server.MapPath("~/Study/Temp/" + UserID.ToString())))
                {
                    if (Directory.Exists(Server.MapPath("~/Study/Temp/" + UserID.ToString())))
                    {
                        arrTemp = Directory.GetFiles(Server.MapPath("~") + "/Study/Temp/" + UserID.ToString());
                        if (arrTemp.Length > 0)
                        {
                            for (int i = 0; i < arrTemp.Length; i++)
                            {
                                File.Delete(arrTemp[i]);
                            }
                        }

                    }

                    Directory.Delete(Server.MapPath("~") + "/Study/Temp/" + UserID.ToString());

                }
                #endregion
            }
            catch (Exception ex)
            {
                ;
            }

            try
            {
                #region Delete files under "CaseList"
                arrTemp = new string[0];
                if (Directory.Exists(Server.MapPath("~/CaseList/Temp/" + UserID.ToString())))
                {
                    if (Directory.Exists(Server.MapPath("~/CaseList/Temp/" + UserID.ToString())))
                    {
                        arrTemp = Directory.GetFiles(Server.MapPath("~") + "/CaseList/Temp/" + UserID.ToString());
                        if (arrTemp.Length > 0)
                        {
                            for (int i = 0; i < arrTemp.Length; i++)
                            {
                                File.Delete(arrTemp[i]);
                            }
                        }

                    }

                    Directory.Delete(Server.MapPath("~") + "/CaseList/Temp/" + UserID.ToString());

                }
                arrTemp = new string[0];
                if (Directory.Exists(Server.MapPath("~/CaseList/DCMTemp/" + UserID.ToString())))
                {
                    if (Directory.Exists(Server.MapPath("~/CaseList/DCMTemp/" + UserID.ToString())))
                    {
                        arrTemp = Directory.GetFiles(Server.MapPath("~") + "/CaseList/DCMTemp/" + UserID.ToString());
                        if (arrTemp.Length > 0)
                        {
                            for (int i = 0; i < arrTemp.Length; i++)
                            {
                                File.Delete(arrTemp[i]);
                            }
                        }

                    }

                    Directory.Delete(Server.MapPath("~") + "/CaseList/DCMTemp/" + UserID.ToString());

                }
                arrTemp = new string[0];
                if (Directory.Exists(Server.MapPath("~/CaseList/IMGTemp/" + UserID.ToString())))
                {
                    if (Directory.Exists(Server.MapPath("~/CaseList/IMGTemp/" + UserID.ToString())))
                    {
                        arrTemp = Directory.GetFiles(Server.MapPath("~") + "/CaseList/IMGTemp/" + UserID.ToString());
                        if (arrTemp.Length > 0)
                        {
                            for (int i = 0; i < arrTemp.Length; i++)
                            {
                                File.Delete(arrTemp[i]);
                            }
                        }

                    }

                    Directory.Delete(Server.MapPath("~") + "/CaseList/IMGTemp/" + UserID.ToString());

                }

                #endregion
            }
            catch (Exception ex)
            {
                ;
            }

            try
            {
                #region Delete files under "DownloadRouter"
                arrTemp = new string[0];
                if (Directory.Exists(Server.MapPath("~/DownloadRouter/Temp/" + UserID.ToString())))
                {
                    if (Directory.Exists(Server.MapPath("~/DownloadRouter/Temp/" + UserID.ToString())))
                    {
                        arrTemp = Directory.GetFiles(Server.MapPath("~") + "/DownloadRouter/Temp/" + UserID.ToString());
                        if (arrTemp.Length > 0)
                        {
                            for (int i = 0; i < arrTemp.Length; i++)
                            {
                                File.Delete(arrTemp[i]);
                            }
                        }

                    }

                    Directory.Delete(Server.MapPath("~") + "/DownloadRouter/Temp/" + UserID.ToString());

                }
                #endregion
            }
            catch (Exception ex)
            {
                ;
            }

            try
            {
                #region Delete files under "CaseList (Reports)"
                arrTemp = new string[0];
                if (Directory.Exists(Server.MapPath("~/CaseList/DocPrint/Temp/" + UserID.ToString())))
                {
                    if (Directory.Exists(Server.MapPath("~/CaseList/DocPrint/Temp/" + UserID.ToString())))
                    {
                        arrTemp = Directory.GetFiles(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID.ToString());
                        if (arrTemp.Length > 0)
                        {
                            for (int i = 0; i < arrTemp.Length; i++)
                            {
                                File.Delete(arrTemp[i]);
                            }
                        }

                    }

                    Directory.Delete(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID.ToString());

                }
                #endregion
            }
            catch (Exception ex)
            {
                ;
            }

            try
            {
                #region Delete files under "Manual Submission"
                if (Directory.Exists(Server.MapPath("~/Study/MSTemp/" + UserID.ToString())))
                {
                    if (Directory.Exists(Server.MapPath("~/Study/MSTemp/" + UserID.ToString())))
                    {
                        arrTemp = Directory.GetFiles(Server.MapPath("~") + "/Study/MSTemp/" + UserID.ToString());
                        if (arrTemp.Length > 0)
                        {
                            for (int i = 0; i < arrTemp.Length; i++)
                            {
                                File.Delete(arrTemp[i]);
                            }
                        }

                    }

                    Directory.Delete(Server.MapPath("~") + "/Study/MSTemp/" + UserID.ToString());

                }
                #endregion
            }
            catch (Exception ex)
            {
                ;
            }

            try
            {
                #region Delete Invoice Files
                arrTemp = new string[0];
                if (Directory.Exists(Server.MapPath("~/Invoicing/DocumentPrinting/Temp")))
                {

                    arrTemp = Directory.GetFiles(Server.MapPath("~") + "/Invoicing/DocumentPrinting/Temp/", "*" + UserID.ToString() + "*.pdf");
                    if (arrTemp.Length > 0)
                    {
                        for (int i = 0; i < arrTemp.Length; i++)
                        {
                            File.Delete(arrTemp[i]);
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                ;
            }

            try
            {

                #region Delete Master Files
                arrTemp = new string[0];
                if (Directory.Exists(Server.MapPath("~/Masters/Temp/" + UserID.ToString())))
                {
                    if (Directory.Exists(Server.MapPath("~/Masters/Temp/" + UserID.ToString())))
                    {
                        arrTemp = Directory.GetFiles(Server.MapPath("~") + "/Masters/Temp/" + UserID.ToString());
                        if (arrTemp.Length > 0)
                        {
                            for (int i = 0; i < arrTemp.Length; i++)
                            {
                                File.Delete(arrTemp[i]);
                            }
                        }

                    }

                    Directory.Delete(Server.MapPath("~") + "/Masters/Temp/" + UserID.ToString());

                }
                #endregion
            }
            catch (Exception ex)
            {
                ;
            }

            try
            {
            #region Delete Report Comaprison Files
            arrTemp = new string[0];
            try
            {
                if (Directory.Exists(Server.MapPath("~/CaseList/DocPrint/CompareTemp/" + UserID.ToString())))
                {
                    if (Directory.Exists(Server.MapPath("~/CaseList/DocPrint/CompareTemp/" + UserID.ToString())))
                    {
                        arrTemp = Directory.GetFiles(Server.MapPath("~") + "/CaseList/DocPrint/CompareTemp/" + UserID.ToString());
                        if (arrTemp.Length > 0)
                        {
                            for (int i = 0; i < arrTemp.Length; i++)
                            {
                                File.Delete(arrTemp[i]);
                            }
                        }

                    }

                    Directory.Delete(Server.MapPath("~") + "/CaseList/DocPrint/CompareTemp/" + UserID.ToString());
                }
            }
            catch (Exception ex)
            {
                ;
            }
            #endregion
            }
            catch (Exception ex)
            {
                ;
            }
        }
        #endregion

        #region DeleteFile (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] DeleteFile(string strFilePath)
        {
            string[] strReturn = new string[2];

            try
            {
                if (File.Exists(strFilePath))
                    File.Delete(strFilePath);
                strReturn[0] = "true";
            }
            catch (Exception ex)
            {
                strReturn = new string[2];
                strReturn[0] = "catch";
                strReturn[1] = ex.Message.ToString();
            }

            return strReturn;
        }
        #endregion

        #region FetchRadiologistProductivity (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public List<Core.Login.RadiologistProductivity> FetchRadiologistProductivity(string[] arrParams)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objCore = new Core.Login.Login();
            int i = 0;

            string[] arrRet = new string[0];
            List<Core.Login.RadiologistProductivity> rpList = new List<Core.Login.RadiologistProductivity>();
            Core.Login.RadiologistProductivity rp = null;
            try
            {

                objCore.RADIOLOGIST_ID = new Guid(arrParams[0]);
                objCore.USER_ID = new Guid(arrParams[1]);
                bReturn = objCore.FetchRadiologistProductivityCount(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    foreach (DataRow dr in ds.Tables["RecordCount"].Rows)
                    {
                        rp = new Core.Login.RadiologistProductivity();

                        rp.modality = Convert.ToString(dr["modality"]);
                        rp.assigned_count = Convert.ToInt32(dr["assigned_count"]);
                        rp.work_progress_count = Convert.ToInt32(dr["work_progress_count"]);
                        rp.today_count = Convert.ToInt32(dr["today_count"]);
                        rp.this_month_count = Convert.ToInt32(dr["this_month_count"]);
                        rp.last_month_count = Convert.ToInt32(dr["last_month_count"]);
                        rp.this_year_count = Convert.ToInt32(dr["this_year_count"]);
                        rpList.Add(rp);
                    }
                    
                }
                else
                {
                    arrRet = new string[2];
                    arrRet[0] = "false";
                    arrRet[1] = strCatchMessage.Trim(); ;
                }
                return rpList;
            }
            catch (Exception ex)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return rpList;
        }
        #endregion

        #region UpdatePreferedTheme (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] UpdatePreferedTheme(string[] arrParams)
        {
            string strReturn = string.Empty;
            string strCatchMessage = ""; string strReturnMessage = ""; bool bReturn = false;
            DateTime dt = DateTime.Today.AddDays(7);
            objCore = new Core.Login.Login();

            string[] arrRet = new string[0];

            try
            {
                objCore.PREFERED_THEME = arrParams[0].Trim();
                objCore.USER_ID = new Guid(arrParams[1]);

                bReturn = objCore.UpdatePreferedTheme(Server.MapPath("~"), ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[4];
                    arrRet[0] = "true";
                    arrRet[1] = dt.Year.ToString();
                    arrRet[2] = dt.Month.ToString();
                    arrRet[3] = dt.Day.ToString();
                    Session["uid"] = objCore.USER_ID;
                }
                else
                {
                    arrRet = new string[2];
                    if (strCatchMessage.Trim() != string.Empty)
                    {
                        arrRet[0] = "catch";
                        arrRet[1] = strCatchMessage.Trim();
                    }
                    else
                    {
                        arrRet[0] = "false";
                        arrRet[1] = strReturnMessage.Trim();
                    }
                }

            }
            catch (Exception ex)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = ex.Message.ToString();
            }
            finally
            {
                objCore = null;
            }
            return arrRet;
        }
        #endregion
    }
}