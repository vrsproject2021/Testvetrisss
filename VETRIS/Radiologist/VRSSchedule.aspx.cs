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
    [AjaxPro.AjaxNamespace("VRSSchedule")]
    public partial class VRSSchedule : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.Radiologist.Scheduling objCore;
        static Guid USER_ID = Guid.Empty;
        static int MENU_ID = 0;
        static string USER_ROLE= string.Empty;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSSchedule));
            SetAttributes();
            if (!IsPostBack)
            {
                if ((!CallBackRad.CausedCallback) && (!CallBackSch.CausedCallback))
                    SetPageValue();
            }

        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            btnClose.Attributes.Add("onclick", "javascript:btnClose_OnClick();");


            txtFromDt.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgFrom.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtFromDt.ClientID + "','CalFrom');");
            txtTillDt.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgTill.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtTillDt.ClientID + "','CalTill');");

            txtStartDt.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            txtStartDt.Attributes.Add("onchange", "javascript:DateChanged();");
            imgStart.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtStartDt.ClientID + "','CalStart');");
            txtEndDt.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            txtEndDt.Attributes.Add("onchange", "javascript:DateChanged();");
            imgEnd.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtEndDt.ClientID + "','CalEnd');");

            txtDays.Attributes.Add("onkeypress", "javascript:return parent.CheckInteger(event);");
            txtDays.Attributes.Add("onblur", "javascript:ResetValueDecimal(this);");
            txtDays.Attributes.Add("onfocus", "javascript:this.select();");

            chkNext.Attributes.Add("onclick", "javascript:chkNext_OnClick();");

            btnCreate.Attributes.Add("onclick", "javascript:btnCreate_OnClick();");
            btnCancel.Attributes.Add("onclick", "javascript:btnCancel_OnClick();");
            btnReset.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
                      
            btnPrev.Attributes.Add("onclick", "javascript:btnPrev_OnClick();");
            btnNext.Attributes.Add("onclick", "javascript:btnNext_OnClick();");
            ddlRadiologist.Attributes.Add("onchange", "javascript:LoadScheduler();");
            
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            hdnID.Value = Request.QueryString["id"];
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            string strTheme = Request.QueryString["th"];

            USER_ID = UserID;
            MENU_ID = intMenuID;

            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            txtFromDt.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            txtTillDt.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            CalFrom.DisabledDates.SelectRange(Convert.ToDateTime("01jan1900"), DateTime.Today.AddDays(-1));
            CalTill.DisabledDates.SelectRange(Convert.ToDateTime("01jan1900"), DateTime.Today.AddDays(-1));
            txtStartDt.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            txtEndDt.Text = objComm.IMDateFormat(DateTime.Today.AddDays(6), objComm.DateFormat);
            CalStart.DisabledDates.SelectRange(Convert.ToDateTime("01jan1900"), DateTime.Today.AddDays(-1));
            CalEnd.DisabledDates.SelectRange(Convert.ToDateTime("01jan1900"), DateTime.Today.AddDays(-1));
            LoadParameters();

            PopulateTimeDropDowns();

            ddlFromMin.SelectedValue = objComm.padZero(DateTime.Now.Minute);
            ddlFromTT.SelectedValue = DateTime.Now.ToString("tt");
            if (DateTime.Now.ToString("tt") == "PM" && DateTime.Now.Hour > 12)
                ddlFromHr.SelectedValue = objComm.padZero(DateTime.Now.Hour - 12);
            else if (DateTime.Now.ToString("tt") == "AM" && DateTime.Now.Hour == 0)
                ddlFromHr.SelectedValue = "12";
            else
                ddlFromHr.SelectedValue = objComm.padZero(DateTime.Now.Hour);

            ddlTillMin.SelectedValue = objComm.padZero(DateTime.Now.Minute);
            ddlTillTT.SelectedValue = DateTime.Now.ToString("tt");
            if (DateTime.Now.ToString("tt") == "PM" && DateTime.Now.Hour > 12)
                ddlTillHr.SelectedValue = objComm.padZero(DateTime.Now.Hour - 12);
            else if (DateTime.Now.ToString("tt") == "AM" && DateTime.Now.Hour == 0)
                ddlTillHr.SelectedValue = "12";
            else
                ddlTillHr.SelectedValue = objComm.padZero(DateTime.Now.Hour);

            txtDays.Text = "0";
            objComm = null;

            SetCSS(strTheme);

        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkCAL.Attributes["href"] = strServerPath + "/css/" + strTheme + "/CalendarStyle.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
            lnkSCHVW.Attributes["href"] = strServerPath + "/css/" + strTheme + "/schedulerView.css";
        }
        #endregion

        #region LoadParameters
        private void LoadParameters()
        {
            
            objCore = new Core.Radiologist.Scheduling();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                objCore.USER_ID = USER_ID;
                bReturn = objCore.LoadParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    DataRow dr = ds.Tables["Radiologist"].NewRow();
                    dr["id"] = new Guid("00000000-0000-0000-0000-000000000000");
                    dr["name"] = "All";
                    ds.Tables["Radiologist"].Rows.InsertAt(dr, 0);
                    ds.Tables["Radiologist"].AcceptChanges();
                    ddlRadiologist.DataSource = ds.Tables["Radiologist"];
                    ddlRadiologist.DataValueField = "id";
                    ddlRadiologist.DataTextField = "name";
                    ddlRadiologist.DataBind();

                    hdnRadID.Value = Convert.ToString(objCore.RADIOLOGIST_ID);
                    hdnViewSchedule.Value = objCore.VIEW_SCHEDULE;
                    USER_ROLE = objCore.USER_ROLE_CODE;
                    hdnRADCALSTARTTIME.Value = objCore.CALENDER_START_TIME;
                }
                else
                {
                    hdnError.Value = "catch" + objComm.RecordDivider + strCatchMessage;
                }
            }
            catch (Exception ex)
            {
                hdnError.Value = "catch" + objComm.RecordDivider + strCatchMessage;

            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region PopulateTimeDropDowns
        private void PopulateTimeDropDowns()
        {
            string strCalStartTime = hdnRADCALSTARTTIME.Value;
            int intStartHr = 0;
            int intStartMin = 0;
            string[] arr = strCalStartTime.Split(':');
            intStartHr = Convert.ToInt32(arr[0]);
            intStartMin = Convert.ToInt32(arr[1]);


            for (int i = 0; i <= 12; i++)
            {
                ListItem item1 = new ListItem();
                item1.Value = objComm.padZero(i);
                item1.Text = objComm.padZero(i);
                ddlFromHr.Items.Add(item1);
            }
            for (int i = 0; i <= 59; i++)
            {
                ListItem item2 = new ListItem();
                item2.Value = objComm.padZero(i);
                item2.Text = objComm.padZero(i);
                ddlFromMin.Items.Add(item2);
            }

            for (int i = 1; i <= 12; i++)
            {
                ListItem item3 = new ListItem();
                item3.Value = objComm.padZero(i);
                item3.Text = objComm.padZero(i);
                ddlTillHr.Items.Add(item3);
            }
            for (int i = 0; i <= 59; i++)
            {
                ListItem item4 = new ListItem();
                item4.Value = objComm.padZero(i);
                item4.Text = objComm.padZero(i);
                ddlTillMin.Items.Add(item4);
            }
            
         
        }
        #endregion

        #region Radiologist Grid

        #region CallBackRad_Callback
        protected void CallBackRad_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadRadiologist();
            grdRAD.Width = Unit.Percentage(98);
            grdRAD.RenderControl(e.Output);
            spnErrRAD.RenderControl(e.Output);
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
                    grdRAD.DataSource = ds.Tables["Radiologist"];
                    grdRAD.DataBind();
                    spnErrRAD.InnerHtml = "<input type=\"hidden\" id=\"hdnCBRADErr\" value=\"\" />";
                }
                else
                {
                    spnErrRAD.InnerHtml = "<input type=\"hidden\" id=\"hdnCBRADErr\" value=\"" + strCatchMessage + "\" />";
                }
            }
            catch (Exception ex)
            {
                spnErrRAD.InnerHtml = "<input type=\"hidden\" id=\"hdnCBRADErr\" value=\"" + ex.Message.Trim() + "\" />";

            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #endregion

        #region Scheduler

        #region CallBackSch_Callback
        protected void CallBackSch_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadScheduler(e.Parameters);
            schRad.RenderControl(e.Output);
            viewRad.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
            spnCnt.RenderControl(e.Output);
            spnModify.RenderControl(e.Output);
        }
        #endregion

        #region LoadScheduler
        private void LoadScheduler(string[] arrParams)
        {
            objCore = new Core.Radiologist.Scheduling();
            objComm = new classes.CommonClass();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            int Hr = 0;
            int Min = 0;
            int intIdx = -1;
            StringBuilder sbIDs = new StringBuilder();
            DateTime dtLast = Convert.ToDateTime("01Jan1900");
            DateTime dtStart = DateTime.Today;
            int inc = 0;
            string x = "";
            string strCalStartTime = string.Empty;

            try
            {
                objCore.START_DATE = Convert.ToDateTime(arrParams[0]);
                objCore.END_DATE = Convert.ToDateTime(arrParams[1]);
                objCore.RADIOLOGIST_ID = new Guid(arrParams[2]);
                strCalStartTime = arrParams[3];

                bReturn = objCore.LoadScheduleDetails(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    if (objCore.MONTH_NUMBER == DateTime.Today.Month)
                        viewRad.StartDate = DateTime.Today;
                    else
                        viewRad.StartDate = objCore.START_DATE;
                    viewRad.EndDate = objCore.END_DATE;
                    viewRad.Now = DateTime.Now;

                    viewRad.StartTime = new TimeSpan(Convert.ToInt32(strCalStartTime.Split(':')[0]), Convert.ToInt32(strCalStartTime.Split(':')[1]), 0);

                    //schRad.DataSource = ds.Tables["Schedule"];
                    //schRad.DataBind();
                    dtLast = objCore.START_DATE.AddDays(-1);

                    foreach (DataRow dr in ds.Tables["Schedule"].Rows)
                    {
                        if (Convert.ToDateTime(objComm.IMDBDateFormat(dr["start_datetime"])) >= viewRad.StartDate)
                        {
                            ComponentArt.Web.UI.SchedulerAppointment appointment = new ComponentArt.Web.UI.SchedulerAppointment();

                            appointment.AppointmentID = Convert.ToString(dr["id"]);
                            appointment.Title = Convert.ToString(dr["name"]).Trim();
                            appointment.Description = Convert.ToString(dr["notes"]).Trim();
                            appointment.Start = Convert.ToDateTime(dr["start_datetime"]);
                            dtStart = Convert.ToDateTime(dr["start_datetime"]);
                            if (dtLast.Year == 1900) dtLast = Convert.ToDateTime(objComm.IMDBDateFormat(dr["start_datetime"]));


                            if (dtLast < Convert.ToDateTime(objComm.IMDBDateFormat(dr["start_datetime"])))
                            {
                                inc = Convert.ToInt32((dtStart - dtLast).Days);
                                dtLast = Convert.ToDateTime(objComm.IMDBDateFormat(dr["start_datetime"]));
                                intIdx = intIdx + inc;
                            }
                           // appointment.SetAttribute("class", "appointment redTitle");
                            //appointment.End = Convert.ToDateTime(dr["end_datetime"]);

                            Hr = (Convert.ToInt32(dr["duration_in_ms"]) / (1000 * 60 * 60));
                            Min = (Convert.ToInt32(dr["duration_in_ms"]) / (1000 * 60)) - (Hr * 60);
                            TimeSpan ts = new TimeSpan(Hr, Min, 0);
                            appointment.Duration = ts;
                            appointment.Tag = Convert.ToString(dr["radiologist_id"]);
                           
                           //if(Convert.ToString(dr["id"]) == "f5e5c043-bed1-4648-8dc3-122930b65d9e")
                           //     x="";
                            
                            schRad.Appointments.Add(appointment);
                            if (sbIDs.Length == 0) sbIDs.Append(Convert.ToString(dr["id"]) + "_" + intIdx.ToString() + "," + Convert.ToString(dr["identity_color"]));
                            else sbIDs.Append(objComm.RecordDivider + Convert.ToString(dr["id"]) + "_" + intIdx.ToString() + "," + Convert.ToString(dr["identity_color"]));
                        }
                        
                       
                    }
                    
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";
                    spnCnt.InnerHtml = "<input type=\"hidden\" id=\"hdnCBCnt\" value=\"" + sbIDs.ToString() + "\" />";
                }
                else
                {
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";
                    spnCnt.InnerHtml = "<input type=\"hidden\" id=\"hdnCBCnt\" value=\"\" />";
                }
            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
                spnCnt.InnerHtml = "<input type=\"hidden\" id=\"hdnCBCnt\" value=\"\" />";

            }
            finally
            {
                ds.Dispose();
                objCore = null;
                objComm = null;
            }

            spnModify.InnerHtml = "<input type=\"hidden\" id=\"hdnModify\" value=\"N\" />";
        }
        #endregion

        #region schRad_AppointmentAdded
        protected void schRad_AppointmentAdded(object sender, ComponentArt.Web.UI.SchedulerAppointmentAddedEventArgs e)
        {

        }
        #endregion

        #region schRad_AppointmentRemoved
        protected void schRad_AppointmentRemoved(object sender, ComponentArt.Web.UI.SchedulerAppointmentRemovedEventArgs e)
        {

        }
        #endregion

        #region schRad_AppointmentModified
        protected void schRad_AppointmentModified(object sender, ComponentArt.Web.UI.SchedulerAppointmentModifiedEventArgs e)
        {
            StringBuilder sbSQL = new StringBuilder();
            ArrayList settingsList = new ArrayList();
            string columnName = string.Empty;
            string colName = string.Empty;
            object columnValue;
            StringBuilder sbSetting = new StringBuilder();
            string[] settingsArray = new string[0];
            string strQuery = string.Empty;
            bool bReturn = false; string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            objCore = new Core.Radiologist.Scheduling();
            objComm = new classes.CommonClass();

            objComm.SetRegionalFormat();

            try
            {
                if (USER_ROLE != "RDL")
                {
                    #region Build Update Query
                    sbSQL.Append("UPDATE radiologist_schedule SET ");
                    foreach (DictionaryEntry commandArgument in e.Command.Arguments)
                    {
                        if (sbSetting.Length > 0) sbSetting.Clear();
                        columnName = commandArgument.Key.ToString();

                        switch (columnName)
                        {
                            case "Description":
                                colName = "notes";
                                break;
                            case "Start":
                                colName = "start_datetime";
                                break;
                            case "Duration":
                                colName = "duration_in_ms";
                                break;
                        }

                        sbSetting.Append(colName);
                        sbSetting.Append(" = ");
                        columnValue = commandArgument.Value;

                        if (columnValue is DateTime)
                        {
                            sbSetting.Append("'").Append(objComm.IMDBDateFormat((DateTime)columnValue) + " " + ((DateTime)columnValue).ToString("HH:mm:ss")).Append("'");
                            sbSetting.Append(",");
                            sbSetting.Append("end_datetime = dateadd(ms,duration_in_ms,'");
                            sbSetting.Append(objComm.IMDBDateFormat((DateTime)columnValue) + " " + ((DateTime)columnValue).ToString("HH:mm:ss"));
                            sbSetting.Append("')");
                        }
                        else if (columnValue is TimeSpan)
                        {
                            sbSetting.Append((TimeSpan.Parse(columnValue.ToString()).TotalMilliseconds));
                            sbSetting.Append(",");
                            sbSetting.Append("end_datetime = dateadd(ms,");
                            sbSetting.Append((TimeSpan.Parse(columnValue.ToString()).TotalMilliseconds));
                            sbSetting.Append(",start_datetime)");
                        }
                        else if (columnValue is String)
                        {
                            sbSetting.Append("'").Append(columnValue).Append("'");
                        }
                        else
                        {
                            sbSetting.Append(columnValue);
                        }
                        settingsList.Add(sbSetting.ToString());
                    }

                    settingsArray = new string[settingsList.Count];
                    settingsList.CopyTo(settingsArray);
                    sbSQL.Append(String.Join(", ", settingsArray));
                    sbSQL.Append(" WHERE id ='" + e.AppointmentBefore.AppointmentID + "'");
                    strQuery = sbSQL.ToString();
                    #endregion

                    objCore.USER_ID = USER_ID;
                    objCore.MENU_ID = MENU_ID;

                    bReturn = objCore.UpdateSchedule(Server.MapPath("~"), strQuery, ref strReturnMsg, ref strCatchMessage);

                    if (!bReturn)
                    {

                        if (strCatchMessage.Trim() != "")
                        {
                            Response.Write(strCatchMessage.Trim());
                        }
                        else
                        {
                            Response.Write(strReturnMsg.Trim());
                        }
                    }
                }
                else
                    bReturn = true;

                

            }
            catch (Exception ex)
            { Response.Write(ex.Message); }
        }
        #endregion

        #endregion

        #region CreateSchedule (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] CreateSchedule(string[] ArrRecord, string[] ArrRad, string[] ArrWeek)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Radiologist.Scheduling();
            objComm = new classes.CommonClass();

            Core.Radiologist.RadiologistList[] objRAD = new Core.Radiologist.RadiologistList[0];
            Core.Radiologist.Weekdays[] objWD = new Core.Radiologist.Weekdays[0];


            try
            {
                objCore.START_DATE = Convert.ToDateTime(ArrRecord[0]);
                objCore.END_DATE = Convert.ToDateTime(ArrRecord[1]);
                objCore.FOR_NEXT_DAYS = ArrRecord[2].Trim();
                objCore.NEXT_DAYS = Convert.ToInt32(ArrRecord[3]);
                objCore.START_TIME = ArrRecord[4].Trim();
                objCore.END_TIME = ArrRecord[5].Trim();
                objCore.USER_ID = new Guid(ArrRecord[6].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[7]);

                objRAD = new Core.Radiologist.RadiologistList[(ArrRad.Length)];

                #region populate radiologist details
                for (int i = 0; i < objRAD.Length; i++)
                {
                    objRAD[i] = new Core.Radiologist.RadiologistList();
                    objRAD[i].ID = new Guid(ArrRad[intListIndex]);
                    intListIndex = intListIndex + 1;
                }
                #endregion

                intListIndex = 0;

                objWD = new Core.Radiologist.Weekdays[(ArrWeek.Length)];

                #region Populate week days
                for (int i = 0; i < objWD.Length; i++)
                {
                    objWD[i] = new Core.Radiologist.Weekdays();
                    objWD[i].DAY_NUMBER = Convert.ToInt32(ArrWeek[intListIndex]);

                    intListIndex = intListIndex + 1;
                }
                #endregion



                bReturn = objCore.CreateSchedule(Server.MapPath("~"), objWD, objRAD, ref strReturnMsg, ref strCatchMessage);

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
                        arrRet[1] = strReturnMsg.Trim();
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

        #region CancelSchedule (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] CancelSchedule(string[] ArrRecord, string[] ArrRad, string[] ArrWeek)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Radiologist.Scheduling();
            objComm = new classes.CommonClass();

            Core.Radiologist.RadiologistList[] objRAD = new Core.Radiologist.RadiologistList[0];
            Core.Radiologist.Weekdays[] objWD = new Core.Radiologist.Weekdays[0];


            try
            {
                objCore.START_DATE = Convert.ToDateTime(ArrRecord[0]);
                objCore.END_DATE = Convert.ToDateTime(ArrRecord[1]);
                objCore.FOR_NEXT_DAYS = ArrRecord[2].Trim();
                objCore.NEXT_DAYS = Convert.ToInt32(ArrRecord[3]);
                objCore.START_TIME = ArrRecord[4].Trim();
                objCore.END_TIME = ArrRecord[5].Trim();
                objCore.USER_ID = new Guid(ArrRecord[6].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[7]);

                objRAD = new Core.Radiologist.RadiologistList[(ArrRad.Length)];

                #region populate radiologist details
                for (int i = 0; i < objRAD.Length; i++)
                {
                    objRAD[i] = new Core.Radiologist.RadiologistList();
                    objRAD[i].ID = new Guid(ArrRad[intListIndex]);
                    intListIndex = intListIndex + 1;
                }
                #endregion

                intListIndex = 0;

                objWD = new Core.Radiologist.Weekdays[(ArrWeek.Length)];

                #region Populate week days
                for (int i = 0; i < objWD.Length; i++)
                {
                    objWD[i] = new Core.Radiologist.Weekdays();
                    objWD[i].DAY_NUMBER = Convert.ToInt32(ArrWeek[intListIndex]);

                    intListIndex = intListIndex + 1;
                }
                #endregion



                bReturn = objCore.CancelSchedule(Server.MapPath("~"), objWD, objRAD, ref strReturnMsg, ref strCatchMessage);

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
                        arrRet[1] = strReturnMsg.Trim();
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