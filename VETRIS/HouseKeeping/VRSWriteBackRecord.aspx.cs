using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.Data;
using VETRIS.Core;
using AjaxPro;

namespace VETRIS.HouseKeeping
{
    [AjaxPro.AjaxNamespace("VRSWriteBackRecord")]
    public partial class VRSWriteBackRecord : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.HouseKeeping.WriteBackRecord objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSWriteBackRecord));
            if (!CallBackBrw.CausedCallback)
                SetPageValue();
            SetAttributes();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

            txtFromDt.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgFrom.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtFromDt.ClientID + "','CalFrom');");
            txtTillDt.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgTill.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtTillDt.ClientID + "','CalTill');");
            btnSearch.Attributes.Add("onclick", "javascript:btnSearch_OnClick();");

        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            txtFromDt.Text = objComm.IMDateFormat(DateTime.Today.AddDays(-3), objComm.DateFormat); CalFrom.SelectedDate = DateTime.Today.AddDays(-3);
            txtTillDt.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat); CalFrom.SelectedDate = DateTime.Today;
            objComm = null;
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
        }
        #endregion
        #region CallBackBrw_Callback
        protected void CallBackBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadStudies(e.Parameters);
            grdBrw.Width = Unit.Percentage(99);
            grdBrw.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadStudies
        private void LoadStudies(string[] arrParams)
        {
            objCore = new Core.HouseKeeping.WriteBackRecord();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.FROM_DATE = Convert.ToDateTime(arrParams[0]);
                objCore.TILL_DATE = Convert.ToDateTime(arrParams[1]);


                bReturn = objCore.FetchStudies(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    grdBrw.DataSource = ds.Tables["Studies"];
                    grdBrw.Levels[0].Columns[3].FormatString = objComm.DateFormat + " HH:mm:ss";
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

        #region UpdateWriteBack (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] UpdateWriteBack(string[] ArrRecord)
        {
            bool bReturn = false;
            string[] arrRet = new string[3];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            objCore = new Core.HouseKeeping.WriteBackRecord();


            try
            {
                objCore.ID = new Guid(ArrRecord[0]);
                objCore.WRITE_BACK = ArrRecord[1].Trim();

                bReturn = objCore.UpdateStudy(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

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