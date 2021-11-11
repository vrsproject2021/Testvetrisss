using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Data;
using System.IO;
using System.Net;
using VETRIS.Core;
using AjaxPro;

namespace VETRIS.Radiologist
{
    [AjaxPro.AjaxNamespace("VRSScheduleUpdate")]
    public partial class VRSScheduleUpdate : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.Radiologist.Scheduling objCore;
        static Guid USER_ID = Guid.Empty;
        static int MENU_ID = 0;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSScheduleUpdate));
            SetAttributes();
            SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            btnClose.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnSave.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnDel.Attributes.Add("onclick", "javascript:btnDel_OnClick();");
            txtFromDt.Attributes.Add("onblur", "javascript:txtFromDt_OnBlur();");
            imgFrom.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtFromDt.ClientID + "','CalFrom');");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            lblHdr.Text = Request.QueryString["ttl"];
            hdnID.Value = Request.QueryString["id"];
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            string strRadID = Request.QueryString["radid"];
            DateTime dtStart = Convert.ToDateTime(Request.QueryString["sdt"]);
            DateTime dtEnd = Convert.ToDateTime(Request.QueryString["edt"]);
            string strTheme = Request.QueryString["th"];

            USER_ID = UserID;
            MENU_ID = intMenuID;

            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();

            txtFromDt.Text = objComm.IMDateFormat(dtStart, objComm.DateFormat);
            CalFrom.DisabledDates.SelectRange(Convert.ToDateTime("01jan1900"), DateTime.Today.AddDays(-1));

            PopulateTimeDropDowns();

            
            ddlFromMin.SelectedValue = objComm.padZero(dtStart.Minute); 
            ddlFromTT.SelectedValue = dtStart.ToString("tt");
            if (dtStart.ToString("tt") == "PM" && dtStart.Hour>12)
                ddlFromHr.SelectedValue = objComm.padZero(dtStart.Hour -12 );
            else if (dtStart.ToString("tt") == "AM" && dtStart.Hour == 0)
                ddlFromHr.SelectedValue = "12";
            else
                ddlFromHr.SelectedValue = objComm.padZero(dtStart.Hour);
            

            ddlTillMin.SelectedValue = objComm.padZero(dtEnd.Minute); 
            ddlTillTT.SelectedValue = dtEnd.ToString("tt");
            if (dtEnd.ToString("tt") == "PM" && dtEnd.Hour > 12)
                ddlTillHr.SelectedValue = objComm.padZero(dtEnd.Hour - 12);
            if (dtEnd.ToString("tt") == "AM" && dtEnd.Hour ==0)
                ddlTillHr.SelectedValue = "12";
            else
                ddlTillHr.SelectedValue = objComm.padZero(dtEnd.Hour);
             
               
    
            objComm = null;

            LoadRadiologist();
            ddlRadiologist.SelectedValue = strRadID;

            SetCSS(strTheme);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkCAL.Attributes["href"] = strServerPath + "/css/" + strTheme + "/CalendarStyle.css";
        }
        #endregion

        #region PopulateTimeDropDowns
        private void PopulateTimeDropDowns()
        {
            for (int i = 1; i <= 12; i++)
            {
                ListItem item1 = new ListItem();
                ListItem item3 = new ListItem();
                item1.Value = objComm.padZero(i);
                item1.Text = objComm.padZero(i);
                item3.Value = objComm.padZero(i);
                item3.Text = objComm.padZero(i);
                ddlFromHr.Items.Add(item1);
                ddlTillHr.Items.Add(item3);
            }
            for (int i = 0; i <= 59; i++)
            {
                ListItem item2 = new ListItem();
                ListItem item4 = new ListItem();
                item2.Value = objComm.padZero(i);
                item2.Text = objComm.padZero(i);
                item4.Value = objComm.padZero(i);
                item4.Text = objComm.padZero(i);
                ddlFromMin.Items.Add(item2);
                ddlTillMin.Items.Add(item4);
            }
            
        }
        #endregion

        #region LoadRadiologist
        private void LoadRadiologist()
        {
            objCore = new Core.Radiologist.Scheduling();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                bReturn = objCore.LoadRadiologist(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    DataRow dr = ds.Tables["Radiologist"].NewRow();
                    dr["id"] = "00000000-0000-0000-0000-000000000000";
                    dr["name"] = "Select One";
                    ds.Tables["Radiologist"].Rows.InsertAt(dr, 0);
                    ds.Tables["Radiologist"].AcceptChanges();

                    ddlRadiologist.DataSource = ds.Tables["Radiologist"];
                    ddlRadiologist.DataTextField = "name";
                    ddlRadiologist.DataValueField = "id";
                    ddlRadiologist.DataBind();
                    lblMsg.Text = "";
                }
                else
                {
                    lblMsg.Text = strCatchMessage.Trim();
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message.Trim();
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region SaveSchedule (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveSchedule(string[] ArrRecord)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            string[] arrayCode = new string[0];
           
            

            objCore = new Core.Radiologist.Scheduling();
            objComm = new classes.CommonClass();

            try
            {
                objCore.ID = new Guid(ArrRecord[0]);
                objCore.RADIOLOGIST_ID = new Guid(ArrRecord[1]);
                objCore.START_DATE = Convert.ToDateTime(ArrRecord[2]);
                objCore.START_TIME = ArrRecord[3].Trim();
                objCore.END_TIME = ArrRecord[4].Trim();
                objCore.NOTES = ArrRecord[5].Trim();
                objCore.USER_ID = USER_ID;
                objCore.MENU_ID = MENU_ID;

                bReturn = objCore.SaveSchedule(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {

                    arrRet = new string[3];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
                }
                else
                {
                    if (strCatchMessage.Trim() != "")
                    {
                        arrRet = new string[2];
                        arrRet[0] = "catch";
                        arrRet[1] = strCatchMessage.Trim();
                    }
                    else
                    {
                        arrRet = new string[2];
                        arrRet[0] = "false";
                        if (strReturnMsg.Trim().Contains(objComm.RecordDivider))
                        {
                            arrayCode = strReturnMsg.Trim().Split(objComm.RecordDivider);
                        }
                        else
                        {
                            arrayCode = new string[1];
                            arrayCode[0] = strReturnMsg.Trim();
                        }

                        arrRet[1] = objComm.SetErrorResources(arrayCode, "N", true, "", "");
                    }
                }
            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrRet = new string[2];
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

        #region DeleteSchedule (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] DeleteSchedule(string ID)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            string[] arrayCode = new string[0];

            objCore = new Core.Radiologist.Scheduling();
            objComm = new classes.CommonClass();

            try
            {
                objCore.ID = new Guid(ID);
                objCore.USER_ID = USER_ID;
                objCore.MENU_ID = MENU_ID;

                bReturn = objCore.DeleteSchedule(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {

                    arrRet = new string[3];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
                }
                else
                {
                    if (strCatchMessage.Trim() != "")
                    {
                        arrRet = new string[2];
                        arrRet[0] = "catch";
                        arrRet[1] = strCatchMessage.Trim();
                    }
                    else
                    {
                        arrRet = new string[2];
                        arrRet[0] = "false";
                        if (strReturnMsg.Trim().Contains(objComm.RecordDivider))
                        {
                            arrayCode = strReturnMsg.Trim().Split(objComm.RecordDivider);
                        }
                        else
                        {
                            arrayCode = new string[1];
                            arrayCode[0] = strReturnMsg.Trim();
                        }

                        arrRet[1] = objComm.SetErrorResources(arrayCode, "N", true, "", "");
                    }
                }
            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrRet = new string[2];
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