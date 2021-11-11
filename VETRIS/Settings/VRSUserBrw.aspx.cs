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

namespace VETRIS.Settings
{
    public partial class VRSUserBrw : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Settings.User objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CallBackBrw.CausedCallback)
                SetPageValue();
            SetAttributes();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnAdd.Attributes.Add("onclick", "javascript:btnBrwAddUI_Onclick('Settings/VRSUserDlg.aspx');");
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
            Guid UserID = new Guid(Request.QueryString["uid"].ToString());
            string strID = string.Empty;
            string strTheme = Request.QueryString["th"];

            FetchSearchParameters();
            SetCSS(strTheme);

        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkTHEME.Attributes["href"] = strServerPath + "/css/" + strTheme + "/theme.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
        }
        #endregion

        #region FetchSearchParameters
        private void FetchSearchParameters()
        {
            objCore = new Core.Settings.User();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {

                bReturn = objCore.FetchBrowserParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    #region Country
                    DataRow dr1 = ds.Tables["UserRoles"].NewRow();
                    dr1["id"] = 0;
                    dr1["name"] = "Select One";
                    ds.Tables["UserRoles"].Rows.InsertAt(dr1, 0);
                    ds.Tables["UserRoles"].AcceptChanges();

                    ddlRole.DataSource = ds.Tables["UserRoles"];
                    ddlRole.DataValueField = "id";
                    ddlRole.DataTextField = "name";
                    ddlRole.DataBind();
                    #endregion
                }
                else
                    hdnError.Value = strCatchMessage.Trim();

              

            }
            catch (Exception ex)
            {
                hdnError.Value = ex.Message.Trim();
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
            SearchRecord(e.Parameters);
            grdBrw.Width = Unit.Percentage(100);
            grdBrw.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region SearchRecord
        private void SearchRecord(string[] arrRecord)
        {
            objCore = new Core.Settings.User();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objCore.CODE = arrRecord[0].Trim();
                objCore.NAME = arrRecord[1].Trim();
                objCore.IS_ACTIVE = arrRecord[2].Trim();
                objCore.ROLE_ID = Convert.ToInt32(arrRecord[3]);
                objCore.LOGIN_ID= arrRecord[4].Trim();
                objCore.INSTITUTION_NAME = arrRecord[5].Trim();
                objCore.BILLING_ACCOUNT_NAME= arrRecord[6].Trim();
                objCore.ALLOW_MANUAL_SUBMISSION = arrRecord[7].Trim();
                objCore.USER_ID = new Guid(arrRecord[8].Trim());
                objCore.MENU_ID = Convert.ToInt32(arrRecord[9]);
                objCore.SESSION_ID = new Guid(arrRecord[10].Trim());

                bReturn = objCore.SearchBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    grdBrw.DataSource = ds.Tables["BrowserList"];
                    grdBrw.DataBind();
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
    }
}