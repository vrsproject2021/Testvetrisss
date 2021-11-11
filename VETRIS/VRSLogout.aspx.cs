using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using VETRIS.Core;

namespace VETRIS
{
    public partial class VRSLogout : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.Login.Login objCore = null;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageValue();
            ClearCache();

        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            Guid UserID = new Guid(Request.QueryString["uid"]);
            Guid SessionID = new Guid("00000000-0000-0000-0000-000000000000");
            string strTheme = Request.QueryString["th"];

            this.Page.Title = "Thank you | " + ConfigurationManager.AppSettings["ProductHeading"];
            lblVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            if (Request.QueryString["sid"] != null) SessionID = new Guid(Request.QueryString["sid"]);
            DeleteLoggedUser(UserID, SessionID);
            if (Session["uid"] != null) Session["uid"] = null;
            SetCss(strTheme);
        }
        #endregion

        #region SetCss
        private void SetCss(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkLOGIN.Attributes["href"] = "~/css/" + strTheme + "/login.css?v=" + DateTime.Now.Ticks.ToString();

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

        #region DeleteLoggedUser
        private bool DeleteLoggedUser(Guid UserID, Guid SessionID)
        {
            bool bReturn = false;
            string strReturnMsg = ""; string strCatchMessage = "";
            string[] arrErrorCode = new string[1];
            objComm = new classes.CommonClass();
            objCore = new Core.Login.Login();
            try
            {
                objCore.USER_ID = UserID;
                objCore.SESSION_ID = SessionID;
                bReturn = objCore.DeleteLoggedUser(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (!bReturn)
                {

                    if (strCatchMessage.Trim() != string.Empty)
                        arrErrorCode[0] = strCatchMessage.Trim();
                    else
                        arrErrorCode[0] = strReturnMsg.Trim();

                    //lblShowMsg.Text = objComm.SetErrorResources(arrErrorCode, "N", true, "", "");
                }

                DeleteUserDirectory(UserID);
            }
            catch (Exception LexpErr)
            {
                bReturn = false;
                arrErrorCode[0] = LexpErr.Message;
                lblShowMsg.Text = objComm.SetErrorResources(arrErrorCode, "N", true, "", "");
            }
            finally
            {
                objCore = null; objComm = null;
                strReturnMsg = null; strCatchMessage = null;
            }
            return bReturn;
        }
        #endregion

        #region DeleteUserDirectory
        private void DeleteUserDirectory(Guid UserID)
        {
            string[] arrTemp = new string[0];
            string[] arrFiles = new string[0];
            try
            {
                #region Delete files under "CaseList"
                arrTemp = new string[0];
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

                    arrTemp = new string[0];
                    arrTemp = Directory.GetDirectories(Server.MapPath("~") + "/CaseList/Temp/" + UserID.ToString());
                    if (arrTemp.Length > 0)
                    {
                        for (int i = 0; i < arrTemp.Length; i++)
                        {
                            arrFiles = new string[0];
                            arrFiles = Directory.GetFiles(arrTemp[i]);
                            for (int j = 0; j < arrFiles.Length; j++)
                            {
                                File.Delete(arrFiles[j]);
                            }

                            Directory.Delete(arrTemp[i]);
                        }
                    }

                    Directory.Delete(Server.MapPath("~") + "/CaseList/Temp/" + UserID.ToString());

                }
                #endregion

                #region Delete files under "Study"
                arrTemp = new string[0];

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

                #region Manual Submission Files

                #region DCM
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
                #endregion

                #region Images
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

                arrTemp = new string[0];
                if (Directory.Exists(Server.MapPath("~/CaseList/MSTemp/" + UserID.ToString())))
                {
                    if (Directory.Exists(Server.MapPath("~/CaseList/MSTemp/" + UserID.ToString())))
                    {
                        arrTemp = Directory.GetFiles(Server.MapPath("~") + "/CaseList/MSTemp/" + UserID.ToString());
                        if (arrTemp.Length > 0)
                        {
                            for (int i = 0; i < arrTemp.Length; i++)
                            {
                                File.Delete(arrTemp[i]);
                            }
                        }

                    }

                    Directory.Delete(Server.MapPath("~") + "/CaseList/MSTemp/" + UserID.ToString());

                }
                #endregion

                #region Dicom Router
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

                #region Reports
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

                #region Invoices
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

                #region Comapare Reports
                arrTemp = new string[0];
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
                #endregion
            }
            catch (Exception ex)
            {
                ;
            }
        }
        #endregion
    }
}