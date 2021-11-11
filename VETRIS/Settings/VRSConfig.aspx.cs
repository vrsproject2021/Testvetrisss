using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VETRIS.Core;

namespace VETRIS.Settings
{
    [AjaxPro.AjaxNamespace("VRSConfig")]
    public partial class VRSConfig : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Settings.Configuration objCore = new VETRIS.Core.Settings.Configuration();
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSConfig));
            SetAttributes();
            if ((!CallBackBrw.CausedCallback) && (!CallBackOT.CausedCallback) && (!CallBackSMA.CausedCallback) && (!CallBackSSA.CausedCallback) &&
                (!CallBackSAAH.CausedCallback) && (!CallBackDASH.CausedCallback) && (!CallBackSASPAH.CausedCallback))
                SetPageValue();

        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            string strTheme = Request.QueryString["th"];
            SetCSS(strTheme);
            FetchParameters();
           
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkTHEME.Attributes["href"] = strServerPath + "/css/" + strTheme + "/theme.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
            lnkTAB.Attributes["href"] = strServerPath + "/css/" + strTheme + "/tabStyle1.css";
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnSave1.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnSave2.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnSaveOT1.Attributes.Add("onclick", "javascript:btnSaveOT_OnClick();");
            btnSaveOT2.Attributes.Add("onclick", "javascript:btnSaveOT_OnClick();");
            btnSaveSMA1.Attributes.Add("onclick", "javascript:btnSaveSMA_OnClick();");
            btnSaveSMA2.Attributes.Add("onclick", "javascript:btnSaveSMA_OnClick();");
            btnSaveSAAH1.Attributes.Add("onclick", "javascript:btnSaveSAAH_OnClick();");
            btnSaveSAAH2.Attributes.Add("onclick", "javascript:btnSaveSAAH_OnClick();");
            btnSaveDashboard1.Attributes.Add("onclick", "javascript:btnSaveDashboard_OnClick();");
            btnSaveDashboard2.Attributes.Add("onclick", "javascript:btnSaveDashboard_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
        }
        #endregion

        #region FetchParameters
        private void FetchParameters()
        {
            Core.CommonFunctions objCF = new Core.CommonFunctions();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                bReturn = objCF.FetchUSTimeZones(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    #region Time US Time Zones

                    foreach (DataRow dr in ds.Tables["TimeZone"].Rows)
                    {
                        if (hdnTZ.Value.Trim() != string.Empty) hdnTZ.Value += objComm.RecordDivider;
                        hdnTZ.Value += Convert.ToString(dr["id"]) + objComm.RecordDivider;
                        hdnTZ.Value += Convert.ToString(dr["name"]).Trim() + objComm.RecordDivider;
                        hdnTZ.Value += Convert.ToString(dr["gmt_diff"]).Trim() + objComm.RecordDivider;
                        hdnTZ.Value += Convert.ToString(dr["is_default"]).Trim();
                    }
                    #endregion

                }
                else
                {
                    hdnError.Value = "catch" + objComm.RecordDivider + strCatchMessage.Trim();
                }

            }
            catch (Exception ex)
            {
                hdnError.Value = "catch" + objComm.RecordDivider + ex.Message.Trim();
            }
            finally
            {
                ds.Dispose(); objCF = null; objComm = null;
            }
        }
        #endregion

        #region CallBackBrw_Callback
        protected void CallBackBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            LoadData(intMenuID, UserID);
            grdBrw.Width = Unit.Percentage(100);
            grdBrw.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadData
        private void LoadData(int menuId, Guid userId)
        {
            objCore = new VETRIS.Core.Settings.Configuration();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.USER_ID = userId;
                objCore.MENU_ID = menuId;

                bReturn = objCore.ConfigurationBrowserListFetch(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    ds.Relations.Add(ds.Tables["sys_group"].Columns["group_id"], ds.Tables["sys_settings"].Columns["group_id"]);
                    grdBrw.Levels[1].Columns[2].FormatString = objComm.DateFormat;
                    grdBrw.DataSource = ds;

                    //grdBrw.Levels[0].Columns[2].FormatString = objComm.DateFormat;
                    grdBrw.PageSize = ds.Tables["sys_group"].Rows.Count;
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

        #region CallBackOT_Callback
        protected void CallBackOT_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            LoadOperationTime(e.Parameters);
            grdOT.Width = Unit.Percentage(100);
            grdOT.RenderControl(e.Output);
            spnOTERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadOperationTime
        private void LoadOperationTime(string[] arrParams)
        {
            objCore = new VETRIS.Core.Settings.Configuration();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.USER_ID = new Guid(arrParams[0]);
                objCore.MENU_ID = Convert.ToInt32(arrParams[1]);

                bReturn = objCore.OperationTimeListFetch(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    
                    grdOT.DataSource = ds;
                    grdOT.DataBind();
                    spnOTERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBOTErr\" value=\"\" />";
                }
                else
                    spnOTERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBOTErr\" value=\"" + strCatchMessage + "\" />";

            }
            catch (Exception ex)
            {
                spnOTERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBOTErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }

        }
        #endregion

        #region CallBackSMA_Callback
        protected void CallBackSMA_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            LoadServiceAvailability(e.Parameters);
            grdService.Width = Unit.Percentage(100);
            grdService.RenderControl(e.Output);
            spnSAERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadServiceAvailability
        private void LoadServiceAvailability(string[] arrParams)
        {
            objCore = new VETRIS.Core.Settings.Configuration();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.USER_ID = new Guid(arrParams[0]);
                objCore.MENU_ID = Convert.ToInt32(arrParams[1]);

                bReturn = objCore.ServiceAvailabilityListFetch(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    ds.Relations.Add(ds.Tables["Service"].Columns["id"], ds.Tables["Modality"].Columns["service_id"]);
                    grdService.DataSource = ds;
                    grdService.DataBind();
                    spnSAERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSAErr\" value=\"\" />";
                }
                else
                    spnSAERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSAErr\" value=\"" + strCatchMessage + "\" />";

            }
            catch (Exception ex)
            {
                spnSAERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSAErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }

        }
        #endregion

        #region CallBackSSA_Callback
        protected void CallBackSSA_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            LoadServiceSpeciesAvailability(e.Parameters);
            grdServiceSpc.Width = Unit.Percentage(100);
            grdServiceSpc.RenderControl(e.Output);
            spnSASPCERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadServiceSpeciesAvailability
        private void LoadServiceSpeciesAvailability(string[] arrParams)
        {
            objCore = new VETRIS.Core.Settings.Configuration();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.USER_ID = new Guid(arrParams[0]);
                objCore.MENU_ID = Convert.ToInt32(arrParams[1]);

                bReturn = objCore.ServiceAvailabilitySpeciesListFetch(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    ds.Relations.Add(ds.Tables["Service"].Columns["id"], ds.Tables["Species"].Columns["service_id"]);
                    grdServiceSpc.DataSource = ds;
                    grdServiceSpc.DataBind();
                    spnSASPCERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSASpcErr\" value=\"\" />";
                }
                else
                    spnSASPCERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSASpcErr\" value=\"" + strCatchMessage + "\" />";

            }
            catch (Exception ex)
            {
                spnSASPCERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSASpcErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }

        }
        #endregion

        #region CallBackSAAH_Callback
        protected void CallBackSAAH_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            LoadServiceAvailabilityAfterHours(e.Parameters);
            grdSAAH.Width = Unit.Percentage(100);
            grdSAAH.RenderControl(e.Output);
            spnSAAHERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadServiceAvailabilityAfterHours
        private void LoadServiceAvailabilityAfterHours(string[] arrParams)
        {
            objCore = new VETRIS.Core.Settings.Configuration();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.USER_ID = new Guid(arrParams[0]);
                objCore.MENU_ID = Convert.ToInt32(arrParams[1]);

                bReturn = objCore.ServiceAvailabilityAfterHoursListFetch(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    ds.Relations.Add(ds.Tables["Service"].Columns["id"], ds.Tables["Modality"].Columns["service_id"]);
                    grdSAAH.DataSource = ds;
                    grdSAAH.DataBind();
                    spnSAAHERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSAAHErr\" value=\"\" />";
                }
                else
                    spnSAAHERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSAAHErr\" value=\"" + strCatchMessage + "\" />";

            }
            catch (Exception ex)
            {
                spnSAAHERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSAAHErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }

        }
        #endregion

        #region CallBackSASPAH_Callback
        protected void CallBackSASPAH_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            LoadServiceAvailabilitySpeciesAfterHours(e.Parameters);
            grdSASPAH.Width = Unit.Percentage(100);
            grdSASPAH.RenderControl(e.Output);
            spnSAAHSPERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadServiceAvailabilitySpeciesAfterHours
        private void LoadServiceAvailabilitySpeciesAfterHours(string[] arrParams)
        {
            objCore = new VETRIS.Core.Settings.Configuration();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.USER_ID = new Guid(arrParams[0]);
                objCore.MENU_ID = Convert.ToInt32(arrParams[1]);

                bReturn = objCore.ServiceAvailabilitySpeciesAfterHoursListFetch(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    ds.Relations.Add(ds.Tables["Service"].Columns["id"], ds.Tables["Species"].Columns["service_id"]);
                    grdSASPAH.DataSource = ds;
                    grdSASPAH.DataBind();
                    spnSAAHSPERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSASPErr\" value=\"\" />";
                }
                else
                    spnSAAHSPERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSASPErr\" value=\"" + strCatchMessage + "\" />";

            }
            catch (Exception ex)
            {
                spnSAAHSPERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSASPErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }

        }
        #endregion
        
        #region CallBackDASH_Callback
        protected void CallBackDASH_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            LoadDashboard(e.Parameters);
            grdDASH.Width = Unit.Percentage(100);
            grdDASH.RenderControl(e.Output);
            spnDashErr.RenderControl(e.Output);
        }
        #endregion

        #region LoadDashboard
        private void LoadDashboard(string[] arrParams)
        {
            objCore = new VETRIS.Core.Settings.Configuration();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.USER_ID = new Guid(arrParams[0]);
                objCore.MENU_ID = Convert.ToInt32(arrParams[1]);

                bReturn = objCore.DashboardSettingsListFetch(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    ds.Relations.Add(ds.Tables["parent_dashboard"].Columns["id"], ds.Tables["dashboard_settings"].Columns["parent_id"]);
                    ds.Relations.Add(ds.Tables["dashboard_settings"].Columns["id"], ds.Tables["dashboard_settings_aging"].Columns["dashboard_menu_id"]);
                    grdDASH.DataSource = ds;
                    grdDASH.DataBind();
                    spnDashErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBODASHErr\" value=\"\" />";
                }
                else
                    spnDashErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBODASHErr\" value=\"" + strCatchMessage + "\" />";

            }
            catch (Exception ex)
            {
                spnDashErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBODASHErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }

        }
        #endregion
        
        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord, string[] ArrData)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Settings.Configuration();
            objComm = new classes.CommonClass();
            Core.Settings.General_Settings[] objSettings = new Core.Settings.General_Settings[0];

            try
            {
                objCore.USER_ID = new Guid(ArrRecord[0].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[1].Trim());

                objSettings = new Core.Settings.General_Settings[(ArrData.Length / 5)];

                #region Populate General Settings
                for (int i = 0; i < objSettings.Length; i++)
                {
                    objSettings[i] = new Core.Settings.General_Settings();
                    objSettings[i].control_code = ArrData[intListIndex];

                    if(ArrData[intListIndex + 4].Trim()=="Y") objSettings[i].data_type_string = CoreCommon.EncryptString(ArrData[intListIndex + 1]);
                    else objSettings[i].data_type_string = ArrData[intListIndex + 1];

                    objSettings[i].data_type_number = Convert.ToInt32(ArrData[intListIndex + 2]);
                    objSettings[i].data_type_decimal = Convert.ToDecimal(ArrData[intListIndex + 3]);
                    intListIndex = intListIndex + 5;
                }
                #endregion

                bReturn = objCore.SaveRecord(Server.MapPath("~"), objSettings, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {

                    arrRet = new string[4];
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
                        arrRet = new string[3];
                        arrRet[0] = "false";
                        arrRet[1] = strReturnMsg.Trim();
                        //arrRet[2] = objCore.USER_NAME;
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

        #region SaveOperationTime (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveOperationTime(string[] ArrRecord, string[] ArrData)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Settings.Configuration();
            objComm = new classes.CommonClass();
            Core.Settings.OperationTime[] objOT = new Core.Settings.OperationTime[0];

            try
            {
                objCore.USER_ID = new Guid(ArrRecord[0].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[1].Trim());

                objOT = new Core.Settings.OperationTime[(ArrData.Length / 5)];

                #region Populate Operation Time
                for (int i = 0; i < objOT.Length; i++)
                {
                    objOT[i] = new Core.Settings.OperationTime();
                    objOT[i].day_no = Convert.ToInt32(ArrData[intListIndex]);
                    objOT[i].from_time = ArrData[intListIndex + 1].Trim();
                    objOT[i].till_time = ArrData[intListIndex + 2].Trim();
                    objOT[i].time_zone_id = Convert.ToInt32(ArrData[intListIndex + 3]);
                    objOT[i].message_display = ArrData[intListIndex + 4].Trim();
                    intListIndex = intListIndex + 5;
                }
                #endregion

                bReturn = objCore.SaveOperationTime(Server.MapPath("~"), objOT, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {

                    arrRet = new string[2];
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
                        arrRet = new string[3];
                        arrRet[0] = "false";
                        arrRet[1] = strReturnMsg.Trim();
                        arrRet[2] = objCore.USER_NAME;
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

        #region SaveServiceAvailability (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveServiceAvailability(string[] ArrRecord, string[] ArrModData, string[] ArrSpeciesData)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Settings.Configuration();
            objComm = new classes.CommonClass();
            Core.Settings.ServiceModalityAvaiability[] objSAMod = new Core.Settings.ServiceModalityAvaiability[0];
            Core.Settings.ServiceSpeciesAvaiability[] objSASpecies = new Core.Settings.ServiceSpeciesAvaiability[0];

            try
            {
                objCore.USER_ID = new Guid(ArrRecord[0].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[1].Trim());

                objSAMod = new Core.Settings.ServiceModalityAvaiability[(ArrModData.Length / 4)];

                #region Populate Modality Availability
                for (int i = 0; i < objSAMod.Length; i++)
                {
                    objSAMod[i] = new Core.Settings.ServiceModalityAvaiability();
                    objSAMod[i].service_id = Convert.ToInt32(ArrModData[intListIndex]);
                    objSAMod[i].modality_id = Convert.ToInt32(ArrModData[intListIndex + 1]);
                    objSAMod[i].avaiable = ArrModData[intListIndex + 2].Trim();
                    objSAMod[i].message_display = ArrModData[intListIndex + 3].Trim();
                    intListIndex = intListIndex + 4;
                }
                #endregion

                intListIndex = 0;

                objSASpecies = new Core.Settings.ServiceSpeciesAvaiability[(ArrSpeciesData.Length / 4)];

                #region Populate Species Availability
                for (int i = 0; i < objSASpecies.Length; i++)
                {
                    objSASpecies[i] = new Core.Settings.ServiceSpeciesAvaiability();
                    objSASpecies[i].service_id = Convert.ToInt32(ArrSpeciesData[intListIndex]);
                    objSASpecies[i].species_id = Convert.ToInt32(ArrSpeciesData[intListIndex + 1]);
                    objSASpecies[i].avaiable = ArrSpeciesData[intListIndex + 2].Trim();
                    objSASpecies[i].message_display = ArrSpeciesData[intListIndex + 3].Trim();
                    intListIndex = intListIndex + 4;
                }
                #endregion

                intListIndex = 0;

                bReturn = objCore.SaveServiceAvaiability(Server.MapPath("~"), objSAMod, objSASpecies, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {

                    arrRet = new string[2];
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
                        arrRet = new string[3];
                        arrRet[0] = "false";
                        arrRet[1] = strReturnMsg.Trim();
                        arrRet[2] = objCore.USER_NAME;
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

        #region SaveServiceAvailabilityAfterHours (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveServiceAvailabilityAfterHours(string[] ArrRecord, string[] ArrModData, string[] ArrSpeciesData)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Settings.Configuration();
            objComm = new classes.CommonClass();
            Core.Settings.ServiceModalityAvaiability[] objSAMod = new Core.Settings.ServiceModalityAvaiability[0];
            Core.Settings.ServiceSpeciesAvaiability[] objSASpecies = new Core.Settings.ServiceSpeciesAvaiability[0];

            try
            {
                objCore.USER_ID = new Guid(ArrRecord[0].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[1].Trim());

                objSAMod = new Core.Settings.ServiceModalityAvaiability[(ArrModData.Length / 4)];

                #region Populate Modality Availability
                for (int i = 0; i < objSAMod.Length; i++)
                {
                    objSAMod[i] = new Core.Settings.ServiceModalityAvaiability();
                    objSAMod[i].service_id = Convert.ToInt32(ArrModData[intListIndex]);
                    objSAMod[i].modality_id = Convert.ToInt32(ArrModData[intListIndex + 1]);
                    objSAMod[i].avaiable = ArrModData[intListIndex + 2].Trim();
                    objSAMod[i].message_display = ArrModData[intListIndex + 3].Trim();
                    intListIndex = intListIndex + 4;
                }
                #endregion

                intListIndex = 0;

                objSASpecies = new Core.Settings.ServiceSpeciesAvaiability[(ArrSpeciesData.Length / 4)];

                #region Populate Species Availability
                for (int i = 0; i < objSASpecies.Length; i++)
                {
                    objSASpecies[i] = new Core.Settings.ServiceSpeciesAvaiability();
                    objSASpecies[i].service_id = Convert.ToInt32(ArrSpeciesData[intListIndex]);
                    objSASpecies[i].species_id = Convert.ToInt32(ArrSpeciesData[intListIndex + 1]);
                    objSASpecies[i].avaiable = ArrSpeciesData[intListIndex + 2].Trim();
                    objSASpecies[i].message_display = ArrSpeciesData[intListIndex + 3].Trim();
                    intListIndex = intListIndex + 4;
                }
                #endregion

                intListIndex = 0;

                bReturn = objCore.SaveServiceModalityAvaiabilityAfterHours(Server.MapPath("~"), objSAMod, objSASpecies, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {

                    arrRet = new string[2];
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
                        arrRet = new string[3];
                        arrRet[0] = "false";
                        arrRet[1] = strReturnMsg.Trim();
                        arrRet[2] = objCore.USER_NAME;
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

        #region SaveDashboardRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveDashboardRecord(string[] ArrRecord, string[] ArrData, string[] ChildArrData)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Settings.Configuration();
            objComm = new classes.CommonClass();
            Core.Settings.DashboardSettings[] objSettings = new Core.Settings.DashboardSettings[0];
            Core.Settings.DashboardSettingsAging[] objAgingSettings = new Core.Settings.DashboardSettingsAging[0];

            try
            {
                objCore.USER_ID = new Guid(ArrRecord[0].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[1].Trim());

                objSettings = new Core.Settings.DashboardSettings[(ArrData.Length / 11)];

                #region Populate General Settings
                for (int i = 0; i < objSettings.Length; i++)
                {
                    objSettings[i] = new Core.Settings.DashboardSettings();
                    objSettings[i].id = Convert.ToInt32(ArrData[intListIndex]);
                    objSettings[i].parent_id = Convert.ToInt32(ArrData[intListIndex + 1]);
                    objSettings[i].menu_desc = ArrData[intListIndex + 2];
                    objSettings[i].nav_url = ArrData[intListIndex + 3];
                    objSettings[i].icon = ArrData[intListIndex + 4];
                    objSettings[i].display_index = Convert.ToInt32(ArrData[intListIndex + 5]);
                    objSettings[i].refresh_time = ArrData[intListIndex + 6].ToString() == "" ? (int?)null : Convert.ToInt32(ArrData[intListIndex + 6]);
                    objSettings[i].is_enabled = ArrData[intListIndex + 7];
                    objSettings[i].is_default = ArrData[intListIndex + 8];
                    objSettings[i].is_refresh_button = ArrData[intListIndex + 9];
                    objSettings[i].title = ArrData[intListIndex + 10];
                    intListIndex = intListIndex + 11;
                }
                #endregion

                objAgingSettings = new Core.Settings.DashboardSettingsAging[(ChildArrData.Length / 6)];
                intListIndex = 0;
                #region Populate General Settings Aging
                for (int i = 0; i < objAgingSettings.Length; i++)
                {
                    objAgingSettings[i] = new Core.Settings.DashboardSettingsAging();
                    objAgingSettings[i].id = Convert.ToInt32(ChildArrData[intListIndex]);
                    objAgingSettings[i].dashboard_menu_id = Convert.ToInt32(ChildArrData[intListIndex + 1]);
                    objAgingSettings[i].key = ChildArrData[intListIndex + 2];
                    objAgingSettings[i].slot_count = Convert.ToInt32(ChildArrData[intListIndex + 3]);
                    objAgingSettings[i].slot_1 = Convert.ToInt32(ChildArrData[intListIndex + 4]);
                    objAgingSettings[i].slot_2 = Convert.ToInt32(ChildArrData[intListIndex + 5]);
                    objAgingSettings[i].slot_3 = ChildArrData[intListIndex + 6].ToString() == "" ? (int?)null : Convert.ToInt32(ChildArrData[intListIndex + 6]);

                    intListIndex = intListIndex + 7;
                }
                #endregion

                bReturn = objCore.SaveDashboardSettings(Server.MapPath("~"), objSettings, objAgingSettings, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {

                    arrRet = new string[4];
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
                        arrRet = new string[3];
                        arrRet[0] = "false";
                        arrRet[1] = strReturnMsg.Trim();
                        //arrRet[2] = objCore.USER_NAME;
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