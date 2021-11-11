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

namespace VETRIS.AP
{
    [AjaxPro.AjaxNamespace("VRSRadiologistPayment")]
    public partial class VRSRadiologistPayment : System.Web.UI.Page
    {
        #region Members & Variables

        VETRIS.Core.AP.RadiologistPayment objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSRadiologistPayment));
            SetAttributes();
            if (!CallBackRadPayment.CausedCallback) SetPageValue();
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            hdnID.Value = Request.QueryString["id"];
            FetchParameters();
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
            lnkTHEME.Attributes["href"] = strServerPath + "/css/" + strTheme + "/theme.css";
        }
        #endregion
        #region SetAttributes
        private void SetAttributes()
        {
            btnSave1.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnSave2.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnOk.Attributes.Add("onclick", "javascript:btnOk_OnClick();");
            btnApprove.Attributes.Add("onclick", "javascript:btnApprove_OnClick();");
            btnPrint.Attributes.Add("onclick", "javascript:btnPrint_OnClick();");
            ddlBillingCycle.Attributes.Add("onchange", "javascript:ddlBillingCycle_OnChange();");
            ddlRadiologist.Attributes.Add("onchange", "javascript:ddlRadiologist_OnChange();");
        }
        #endregion

        #region FetchParameters
        private void FetchParameters()
        {
            objCore = new Core.AP.RadiologistPayment();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                bReturn = objCore.FetchParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    #region Billing Cycles
                    DataRow dr1 = ds.Tables["Cycle"].NewRow();
                    dr1["id"] = "00000000-0000-0000-0000-000000000000";
                    dr1["name"] = "Select One";
                    ds.Tables["Cycle"].Rows.InsertAt(dr1, 0);
                    ds.Tables["Cycle"].AcceptChanges();

                    ddlBillingCycle.DataSource = ds.Tables["Cycle"];
                    ddlBillingCycle.DataValueField = "id";
                    ddlBillingCycle.DataTextField = "name";
                    ddlBillingCycle.DataBind();
                    #endregion

                    #region Radiologists
                    DataRow dr2 = ds.Tables["Radiologist"].NewRow();
                    dr2["id"] = "00000000-0000-0000-0000-000000000000";
                    dr2["name"] = "Select One";
                    ds.Tables["Radiologist"].Rows.InsertAt(dr2, 0);
                    ds.Tables["Radiologist"].AcceptChanges();

                    ddlRadiologist.DataSource = ds.Tables["Radiologist"];
                    ddlRadiologist.DataValueField = "id";
                    ddlRadiologist.DataTextField = "name";
                    ddlRadiologist.DataBind();
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

        #region CallBackRadPayment_Callback
        protected void CallBackRadPayment_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadDetails(e.Parameters);
            grdRadPayment.Width = Unit.Percentage(100);
            grdRadPayment.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
            spnUser.RenderControl(e.Output);
        }
        #endregion

        #region LoadDetails
        private void LoadDetails(string[] arrRecord)
        {
            objCore = new Core.AP.RadiologistPayment();
            bool bReturn = false; string strReturnMessage = ""; string strCatchMessage = "";
            string strOption = string.Empty;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.BILLING_CYCLE_ID = new Guid(arrRecord[0]);
                objCore.RADIOLOGIST_ID = new Guid(arrRecord[1]);
                objCore.MENU_ID = Convert.ToInt32(arrRecord[2]);
                objCore.USER_ID = new Guid(arrRecord[3]);
                strOption = arrRecord[4].Trim();

                if(strOption == "P")
                    bReturn = objCore.LoadDetails(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);
                else if (strOption == "V")
                    bReturn = objCore.FetchProcessedRecords(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    ds.Relations.Add(ds.Tables[0].Columns["radiologist_id"], ds.Tables[1].Columns["radiologist_id"]);
                  
                    grdRadPayment.Levels[1].Columns[5].FormatString = objComm.DateFormat + " HH:mm";
                    grdRadPayment.DataSource = ds;
                    grdRadPayment.DataBind();
                    grdRadPayment.PageSize = ds.Tables["RadPaymentHdr"].Rows.Count;
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

                }
                else
                {
                    if (strCatchMessage.Trim() != string.Empty)
                        spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";
                    else
                        spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strReturnMessage + "\" />";
                }

            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose(); objComm = null;

            }

            spnUser.InnerHtml = "<input type=\"hidden\" id=\"hdnUsr\" value=\"" + objCore.USER_NAME + "\" />";
        }
        #endregion

        #region FetchProcessStatus (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchProcessStatus(string[] ArrRecord)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;

            objCore = new Core.AP.RadiologistPayment();
            objComm = new classes.CommonClass();



            try
            {
                objCore.BILLING_CYCLE_ID = new Guid(ArrRecord[0].Trim());
                objCore.RADIOLOGIST_ID = new Guid(ArrRecord[1].Trim());

                bReturn = objCore.FetchProcessStatus(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {

                    arrRet = new string[3];
                    arrRet[0] = "true";
                    arrRet[1] = objCore.RECORD_PROCESSED_COUNT.ToString();
                    arrRet[2] = objCore.RECORD_APPROVED_COUNT.ToString();
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

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord, string[] ArrRad)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new Core.AP.RadiologistPayment();
            objComm = new classes.CommonClass();

            Core.AP.RadiologistList[] objRad = new Core.AP.RadiologistList[0];
           

            try
            {
                objCore.BILLING_CYCLE_ID = new Guid(ArrRecord[0].Trim());
                objCore.APPROVED = ArrRecord[1].Trim();
                objCore.USER_ID = new Guid(ArrRecord[2].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[3]);

                objRad = new Core.AP.RadiologistList[(ArrRad.Length / 4)];

                #region Populate Radiologist
                for (int i = 0; i < objRad.Length; i++)
                {
                    objRad[i] = new Core.AP.RadiologistList();
                    objRad[i].ID = new Guid(ArrRad[intListIndex]);
                    objRad[i].STUDY_ID = new Guid(ArrRad[intListIndex + 1]);
                    objRad[i].STUDY_UID = ArrRad[intListIndex + 2].Trim();
                    objRad[i].ADHOC_AMOUNT = Convert.ToDouble(ArrRad[intListIndex + 3]);
                    intListIndex = intListIndex + 4;
                }
                #endregion

                intListIndex = 0;


                bReturn = objCore.SaveRecord(Server.MapPath("~"), objRad, ref strReturnMsg, ref strCatchMessage);

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