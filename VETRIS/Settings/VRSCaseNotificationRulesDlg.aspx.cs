using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using System.Net;
using System.Configuration;
using VETRIS.Core;
using AjaxPro;

namespace VETRIS.Settings
{
    [AjaxPro.AjaxNamespace("VRSCaseNotificationRulesDlg")]
    public partial class VRSCaseNotificationRulesDlg : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Settings.CaseNotificationRules objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSCaseNotificationRulesDlg));
            SetAttributes();
            if ((!CallBackRadiologist.CausedCallback) && (!CallBackMatrix.CausedCallback))
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
            hdnID.Value = Request.QueryString["id"];
            LoadDetails(intMenuID, UserID);
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

        #region SetAttributes
        private void SetAttributes()
        {
            btnAdd1.Attributes.Add("onclick", "javascript:btnNew_OnClick();");
            btnAdd2.Attributes.Add("onclick", "javascript:btnNew_OnClick();");
            btnSave1.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnSave2.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

            rdoNotifyE.Attributes.Add("onclick", "javascript:NotifyByTime_OnClick();");
            rdoNotifyL.Attributes.Add("onclick", "javascript:NotifyByTime_OnClick();");
           
        }
        #endregion

        #region LoadDetails
        private void LoadDetails(int intMenuID, Guid UserID)
        {
            objCore = new Core.Settings.CaseNotificationRules();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.RULE_NUMBER = Convert.ToInt32(hdnID.Value);
                objCore.MENU_ID = intMenuID;
                objCore.USER_ID = UserID;

                bReturn = objCore.LoadDetails(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {

                    PopulateDropdowns(ds);
                    txtRuleNo.Text = objCore.RULE_NUMBER.ToString();
                    txtDesc.Text = objCore.RULE_DESCRIPTION;
                    ddlStudyStatus.SelectedValue = objCore.PACS_STATUS_ID.ToString();
                    ddlPriority.SelectedValue = objCore.PRIORITY_ID.ToString();
                    ddlEllapseHr.SelectedValue = objCore.ELLAPSED_HOURS;
                    ddlEllapseMin.SelectedValue = objCore.ELLAPSED_MINUTES;
                    ddlLeftHr.SelectedValue = objCore.LEFT_HOURS;
                    ddlLeftMin.SelectedValue = objCore.LEFT_MINUTES;

                    if (objCore.NOTIFY_BY_TIME == "E")
                    {
                        rdoNotifyE.Checked = true;
                        ddlLeftHr.Enabled = false;
                        ddlLeftMin.Enabled = false;
                    }
                    else if (objCore.NOTIFY_BY_TIME == "L")
                    {
                        rdoNotifyL.Checked = true;
                        ddlEllapseHr.Enabled = false;
                        ddlEllapseMin.Enabled = false;
                    }
                   
                    if (objCore.IS_ACTIVE == "Y") rdoStatYes.Checked = true;
                    else if (objCore.IS_ACTIVE == "N") rdoStatNo.Checked = true;
               
                }
                else
                {
                    if (strCatchMessage.Trim() != string.Empty)
                        hdnError.Value = "catch" + objComm.RecordDivider + strCatchMessage.Trim();
                    else
                        hdnError.Value = "false" + objComm.RecordDivider + strReturnMessage.Trim();
                }

            }
            catch (Exception ex)
            {
                hdnError.Value = "catch" + objComm.RecordDivider + ex.Message.Trim();
            }
            finally
            {
                ds.Dispose(); objCore = null; objComm = null;
            }
        }
        #endregion

        #region PopulateDropdowns
        private void PopulateDropdowns(DataSet ds)
        {
            #region PACS Status
            DataRow dr1 = ds.Tables["PACSStatus"].NewRow();
            dr1["status_id"] = -999;
            dr1["status_desc"] = "Select One";
            ds.Tables["PACSStatus"].Rows.InsertAt(dr1, 0);
            ds.Tables["PACSStatus"].AcceptChanges();

            ddlStudyStatus.DataSource = ds.Tables["PACSStatus"];
            ddlStudyStatus.DataValueField = "status_id";
            ddlStudyStatus.DataTextField = "status_desc";
            ddlStudyStatus.DataBind();
            #endregion

            #region Priority
            DataRow dr2 = ds.Tables["Priority"].NewRow();
            dr2["priority_id"] = 0;
            dr2["priority_desc"] = "Select One";
            ds.Tables["Priority"].Rows.InsertAt(dr2, 0);
            ds.Tables["Priority"].AcceptChanges();

            ddlPriority.DataSource = ds.Tables["Priority"];
            ddlPriority.DataValueField = "priority_id";
            ddlPriority.DataTextField = "priority_desc";
            ddlPriority.DataBind();
            #endregion

            #region Ellapse Time

            for (int i = 0; i <= 23; i++)
            {
                ListItem item1 = new ListItem();
                ListItem item2 = new ListItem();
                item1.Value = objComm.padZero(i);
                item1.Text = objComm.padZero(i);
                ddlEllapseHr.Items.Add(item1);
                item2.Value = objComm.padZero(i);
                item2.Text = objComm.padZero(i);
                ddlLeftHr.Items.Add(item2);
            }
            for (int i = 0; i <= 59; i++)
            {
                ListItem item3 = new ListItem();
                ListItem item4 = new ListItem();
                item3.Value = objComm.padZero(i);
                item3.Text = objComm.padZero(i);
                ddlEllapseMin.Items.Add(item3);
                item4.Value = objComm.padZero(i);
                item4.Text = objComm.padZero(i);
                ddlLeftMin.Items.Add(item4);
            }


            #endregion
        }
        #endregion

        #region Radiologist grid

        #region CallBackRadiologist_Callback
        protected void CallBackRadiologist_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadRadiologists(e.Parameters);
            grdRad.Width = Unit.Percentage(100);
            grdRad.RenderControl(e.Output);
            spnRADErr.RenderControl(e.Output);
        }
        #endregion

        #region LoadRadiologists
        private void LoadRadiologists(string[] arrParams)
        {
            objCore = new Core.Settings.CaseNotificationRules();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();


            try
            {

                objCore.RULE_NUMBER = Convert.ToInt32(arrParams[0]);

                bReturn = objCore.LoadRadiologists(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {

                    grdRad.DataSource = ds.Tables["Radiologists"];
                    grdRad.DataBind();
                    spnRADErr.InnerHtml = "<input type=\"hidden\" id=\"hdnRADCBErr\" value=\"\" />";
                }
                else
                    spnRADErr.InnerHtml = "<input type=\"hidden\" id=\"hdnRADCBErr\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnRADErr.InnerHtml = "<input type=\"hidden\" id=\"hdnRADCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #endregion

        #region Other Recipient Matrix grid

        #region CallBackMatrix_Callback
        protected void CallBackMatrix_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadOtherRecepients(e.Parameters);
            grdMatrix.Width = Unit.Percentage(100);
            grdMatrix.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadOtherRecepients
        private void LoadOtherRecepients(string[] arrParams)
        {
            objCore = new Core.Settings.CaseNotificationRules();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();


            try
            {

                objCore.RULE_NUMBER = Convert.ToInt32(arrParams[0]);

                bReturn = objCore.LoadOtherRecepients(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    ds.Relations.Add(ds.Tables["UserRoles"].Columns["user_role_id"], ds.Tables["Users"].Columns["user_role_id"]);
                    grdMatrix.DataSource = ds;
                    grdMatrix.DataBind();
                   // grdMatrix.ExpandAll();

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

        #endregion

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord, string[] ArrRadiologist, string[] ArrRecepient)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Settings.CaseNotificationRules();
            objComm = new classes.CommonClass();

            Core.Settings.RadiologistList[] objRadiologist = new Core.Settings.RadiologistList[0];
            Core.Settings.RecepientList[] objRecepient = new Core.Settings.RecepientList[0];


            try
            {
                objCore.RULE_NUMBER = Convert.ToInt32(ArrRecord[0].Trim());
                objCore.RULE_DESCRIPTION = ArrRecord[1].Trim();
                objCore.PACS_STATUS_ID = Convert.ToInt32(ArrRecord[2]);
                objCore.PRIORITY_ID = Convert.ToInt32(ArrRecord[3]);
                objCore.NOTIFY_BY_TIME = ArrRecord[4].Trim();
                objCore.TIME_ELLAPSED_IN_MINUTES = Convert.ToInt32(ArrRecord[5]);
                objCore.TIME_LEFT_IN_MINUTES = Convert.ToInt32(ArrRecord[6]);
                objCore.IS_ACTIVE = ArrRecord[7].Trim();
                objCore.USER_ID = new Guid(ArrRecord[8].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[9]);

                objRadiologist = new Core.Settings.RadiologistList[(ArrRadiologist.Length / 4)];

                #region Populate Radiologist Details
                for (int i = 0; i < objRadiologist.Length; i++)
                {
                    objRadiologist[i] = new Core.Settings.RadiologistList();
                    objRadiologist[i].RADIOLOGIST_ID = new Guid(ArrRadiologist[intListIndex]);
                    objRadiologist[i].USER_ID = new Guid(ArrRadiologist[intListIndex + 1]);
                    objRadiologist[i].NOTIFY_IF_SCHEDULED= ArrRadiologist[intListIndex + 2].Trim();
                    objRadiologist[i].NOTIFY_ALWAYS = ArrRadiologist[intListIndex + 3].Trim();
                    intListIndex = intListIndex + 4;
                }
                #endregion

                intListIndex = 0;
                objRecepient = new Core.Settings.RecepientList[(ArrRecepient.Length / 4)];

                #region Populate Other User Details
                for (int i = 0; i < objRecepient.Length; i++)
                {
                    objRecepient[i] = new Core.Settings.RecepientList();
                    objRecepient[i].USER_ROLE_ID = Convert.ToInt32(ArrRecepient[intListIndex]);
                    objRecepient[i].SCHEDULED = ArrRecepient[intListIndex + 1].Trim();
                    objRecepient[i].NOTIFY_ALL = ArrRecepient[intListIndex + 2].Trim();
                    objRecepient[i].USER_ID = new Guid(ArrRecepient[intListIndex + 3].Trim());
                    intListIndex = intListIndex + 4;
                }
                #endregion

                bReturn = objCore.SaveRecord(Server.MapPath("~"),objRadiologist,objRecepient, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[3];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
                    arrRet[2] = objCore.RULE_NUMBER.ToString();
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
    }
}