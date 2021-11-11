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
    public partial class VRSDRLog : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.HouseKeeping.DRLog objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!CallBackBrw.CausedCallback) && (!CallBackUA.CausedCallback))
                SetPageValue();
            SetAttributes();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {

            btnOkDR.Attributes.Add("onclick", "javascript:btnOkDR_Onclick();");
            btnClose.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");
            btnOKUA.Attributes.Add("onclick", "javascript:btnOKUA_Onclick();");

            txtFromDtUA.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgFromUA.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtFromDtUA.ClientID + "','CalFromUA');");
            txtTillDtUA.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgTillUA.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtTillDtUA.ClientID + "','CalTillUA');");

            txtFromDt.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgFrom.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtFromDt.ClientID + "','CalFrom');");
            txtTillDt.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgTill.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtTillDt.ClientID + "','CalTill');");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"].ToString());
            string strID = string.Empty;


            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            txtFromDtUA.Text = txtFromDt.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            txtTillDtUA.Text = txtTillDt.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            objComm = null;

            FetchSearchParameters(UserID);
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
            lnkCAL.Attributes["href"] = strServerPath + "/css/" + strTheme + "/CalendarStyle.css";
            lnkTHEME.Attributes["href"] = strServerPath + "/css/" + strTheme + "/theme.css";
            lnkTAB.Attributes["href"] = strServerPath + "/css/" + strTheme + "/tabStyle1.css";
        }
        #endregion
        #region FetchSearchParameters
        private void FetchSearchParameters(Guid UserID)
        {
            objCore = new Core.HouseKeeping.DRLog();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            int intCnt = 0;
            objComm = new classes.CommonClass();

            try
            {
                objCore.USER_ID = UserID;

                bReturn = objCore.FetchBrowserParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    #region Institution
                    intCnt = ds.Tables["Institutions"].Rows.Count;
                    DataRow dr3 = ds.Tables["Institutions"].NewRow();
                    dr3["id"] = "00000000-0000-0000-0000-000000000000";
                    dr3["name"] = "Select One";
                    ds.Tables["Institutions"].Rows.InsertAt(dr3, 0);
                    ds.Tables["Institutions"].AcceptChanges();

                    ddlInstitution.DataSource = ds.Tables["Institutions"];
                    ddlInstitution.DataValueField = "id";
                    ddlInstitution.DataTextField = "name";
                    ddlInstitution.DataBind();

                    if (intCnt == 1) ddlInstitution.SelectedIndex = 1;
                    #endregion

                    #region Users
                    DataRow dr1 = ds.Tables["Users"].NewRow();
                    dr1["id"] = "00000000-0000-0000-0000-000000000000";
                    dr1["name"] = "Select One";
                    ds.Tables["Users"].Rows.InsertAt(dr1, 0);
                    ds.Tables["Users"].AcceptChanges();

                    ddlUserName.DataSource = ds.Tables["Users"];
                    ddlUserName.DataValueField = "id";
                    ddlUserName.DataTextField = "name";
                    ddlUserName.DataBind();

                   
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
            objCore = new Core.HouseKeeping.DRLog();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            int intUserRoleID = 0;
            objComm = new classes.CommonClass();


            try
            {
                objComm.SetRegionalFormat();
                objCore.INSTITUTION_ID = new Guid(arrRecord[0]);
                objCore.SERVICE_ID = Convert.ToInt32(arrRecord[1]);
                objCore.LOG_TYPE= arrRecord[2].Trim();
                objCore.DATE_FROM = Convert.ToDateTime(arrRecord[3]);
                objCore.DATE_TILL = Convert.ToDateTime(arrRecord[4]);
                objCore.USER_ID = new Guid(arrRecord[5].Trim());

                bReturn = objCore.SearchBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    grdBrw.DataSource = ds.Tables["BrowserList"];
                    grdBrw.Levels[0].Columns[2].FormatString = objComm.DateFormat + " HH:mm:ss";
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
            if ((intUserRoleID == 1) || (intUserRoleID == 2)) grdBrw.Levels[0].Columns[1].Visible = true;
        }
        #endregion

        #region CallBackUA_Callback
        protected void CallBackUA_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            SearchUserActivityList(e.Parameters);
            grdUA.Width = Unit.Percentage(100);
            grdUA.RenderControl(e.Output);
            spnUAErr.RenderControl(e.Output);
        }
        #endregion

        #region SearchUserActivityList
        private void SearchUserActivityList(string[] arrRecord)
        {
            objCore = new Core.HouseKeeping.DRLog();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            int intUserRoleID = 0;
            objComm = new classes.CommonClass();


            try
            {
                objComm.SetRegionalFormat();
                objCore.USER_ID = new Guid(arrRecord[0].Trim());
                objCore.LOG_MESSAGE = arrRecord[1].Trim();
                objCore.DATE_FROM = Convert.ToDateTime(arrRecord[2]);
                objCore.DATE_TILL = Convert.ToDateTime(arrRecord[3]);


                bReturn = objCore.SearchUserActivityList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    grdUA.DataSource = ds.Tables["UserActivity"];
                    grdUA.Levels[0].Columns[1].FormatString = objComm.DateFormat + " HH:mm:ss";
                    grdUA.DataBind();
                    spnUAErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBUAErr\" value=\"\" />";
                }
                else
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBUAErr\" value=\"" + strCatchMessage + "\" />";

            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBUAErr\" value=\"" + ex.Message.Trim() + "\" />";
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