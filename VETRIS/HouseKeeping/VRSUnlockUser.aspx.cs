using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using System.Configuration;
using VETRIS.Core;

namespace VETRIS.HouseKeeping
{
    [AjaxPro.AjaxNamespace("VRSUnlockUser")]
    public partial class VRSUnlockUser : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.HouseKeeping.UnlockUser objCore = null;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSUnlockUser));
            SetAttributes();
            if (!CallBackBrw.CausedCallback)
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {

            
            btnRefresh.Attributes.Add("onclick", "javascript:ResetRecord();view_Searchform();");
            btnSearch.Attributes.Add("onclick", "javascript:SearchRecord();view_Searchform();");
            btnClose.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid lUserID = new Guid(Request.QueryString["uid"].ToString());
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
        #region SearchRecord
        private void SearchRecord(string[] arrRecord)
        {
            objCore = new Core.HouseKeeping.UnlockUser();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                objCore.CODE = arrRecord[1].Trim();
                objCore.NAME = arrRecord[2].Trim();
                objCore.USER_ID = new Guid(arrRecord[3]);


                bReturn = objCore.SearchBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    grdBrw.DataSource = ds.Tables["BrowserList"];
                    grdBrw.DataBind();
                }
                else
                    Response.Write(strCatchMessage);



            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.Trim());
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region CallBackBrw_Callback
        protected void CallBackBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strUnlock = e.Parameters[0];
            SearchRecord(e.Parameters);
            //if (strUnlock == "N")
            //    grdBrw.Levels[0].Columns[4].Visible = false;
            grdBrw.Width = Unit.Percentage(100);
            grdBrw.RenderControl(e.Output);
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
            objCore = new Core.HouseKeeping.UnlockUser();


            try
            {
                objCore.USER_ID = new Guid(ArrRecord[0]);
                objCore.USER_SESSION_ID = new Guid(ArrRecord[1]);
                objCore.UPDATED_BY = new Guid(ArrRecord[2]);
                objCore.SESSION_ID = new Guid(ArrRecord[3]);

                bReturn = objCore.UserUnlock(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

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
                        arrRet[1] = strReturnMsg.Trim();
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
    }
}