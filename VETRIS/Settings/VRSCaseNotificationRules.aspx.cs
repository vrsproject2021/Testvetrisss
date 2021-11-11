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
using AjaxPro;

namespace VETRIS.Settings
{
    [AjaxPro.AjaxNamespace("VRSCaseNotificationRules")]
    public partial class VRSCaseNotificationRules : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Settings.CaseNotificationRulesOld objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSCaseNotificationRules));
            SetAttributes();
            if ((!CallBackBrw.CausedCallback))
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {

            btnSave.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
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
            FetchParameters();
        }
        #endregion

        #region FetchParameters
        private void FetchParameters()
        {
            objCore = new Core.Settings.CaseNotificationRulesOld();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {

                bReturn = objCore.FetchBrowserParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    #region ADMINS
                    DataRow dr1 = ds.Tables["ADMINS"].NewRow();
                    dr1["id"] = new Guid("00000000-0000-0000-0000-000000000000"); 
                    dr1["name"] = "Select One";
                    ds.Tables["ADMINS"].Rows.InsertAt(dr1, 0);
                    ds.Tables["ADMINS"].AcceptChanges();

                    foreach (DataRow dr in ds.Tables["ADMINS"].Rows)
                    {
                        if (hdnAdmins.Value.Trim() != string.Empty) hdnAdmins.Value += objComm.RecordDivider;
                        hdnAdmins.Value += Convert.ToString(dr["id"]) + objComm.RecordDivider;
                        hdnAdmins.Value += Convert.ToString(dr["name"]).Trim();
                    }
                    #endregion

                    #region RADIOLOGISTS
                    DataRow dr2 = ds.Tables["RADIOLOGISTS"].NewRow();
                    dr2["id"] = new Guid("00000000-0000-0000-0000-000000000000"); 
                    dr2["name"] = "Select One";
                    ds.Tables["RADIOLOGISTS"].Rows.InsertAt(dr2, 0);
                    ds.Tables["RADIOLOGISTS"].AcceptChanges();

                    foreach (DataRow dr in ds.Tables["RADIOLOGISTS"].Rows)
                    {
                        if (hdnRadiologists.Value.Trim() != string.Empty) hdnRadiologists.Value += objComm.RecordDivider;
                        hdnRadiologists.Value += Convert.ToString(dr["id"]) + objComm.RecordDivider;
                        hdnRadiologists.Value += Convert.ToString(dr["name"]).Trim();
                    }
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

        #region Grid

        #region CallBackBrw_Callback
        protected void CallBackBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];
            LoadRecords(e.Parameters);
            grdBrw.Width = Unit.Percentage(100);
            grdBrw.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadRecords
        private void LoadRecords(string[] arrParams)
        {
            objCore = new Core.Settings.CaseNotificationRulesOld();
            string strCatchMessage = ""; string strReturnMessage = "";
            bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";

            try
            {

              
                objCore.USER_ID = new Guid(arrParams[0]);
                objCore.MENU_ID = Convert.ToInt32(arrParams[1]);

                bReturn = objCore.LoadRecords(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);
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

        #endregion

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord, string[] ArrParams)
        {
            bool bReturn = false;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            int intCompanyID = 0; string strFileName = string.Empty; Guid UserID = Guid.Empty;
            objComm = new classes.CommonClass();
            objCore = new Core.Settings.CaseNotificationRulesOld();
            int intListIndex = 0;
            Core.Settings.CaseNotificationRulesOld[] objRecords = new Core.Settings.CaseNotificationRulesOld[0];

            try
            {

                objCore.USER_ID = UserID = new Guid(ArrParams[0]);
                objCore.MENU_ID = Convert.ToInt32(ArrParams[1]);


                objRecords = new Core.Settings.CaseNotificationRulesOld[(ArrRecord.Length / 3)];

                for (int i = 0; i < objRecords.Length; i++)
                {
                    objRecords[i] = new Core.Settings.CaseNotificationRulesOld();
                    objRecords[i].RULE_NUMBER = Convert.ToInt32(ArrRecord[intListIndex]);
                    objRecords[i].TIME_ELLAPSED_IN_MINUTES = Convert.ToInt32(ArrRecord[intListIndex + 1]);
                    objRecords[i].SECOND_LEVEL_RECEIPIENT_ID = new Guid(ArrRecord[intListIndex + 2].Trim());
                    intListIndex = intListIndex + 3;
                }

                intListIndex = 0;

                bReturn = objCore.SaveRecord(Server.MapPath("~"), objRecords, ref strReturnMsg, ref strCatchMessage);

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
    }
}