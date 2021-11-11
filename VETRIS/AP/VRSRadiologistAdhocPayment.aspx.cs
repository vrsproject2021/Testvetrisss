using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.Data;
using System.IO;
using System.Net;
using VETRIS.Core;
using AjaxPro;

namespace VETRIS.AP
{
    [AjaxPro.AjaxNamespace("VRSRadiologistAdhocPayment")]
    public partial class VRSRadiologistAdhocPayment : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.AP.RadiologistPayment objCore;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSRadiologistAdhocPayment));
            SetAttributes();
            if (!CallBackPmt.CausedCallback) SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);

            btnSave1.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnSave2.Attributes.Add("onclick", "javascript:btnSave_OnClick();");

            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");

            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

           

            //lblViewLog.Attributes.Add("onclick", "javascript:ViewLogUI('" + intMenuID.ToString() + "');");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            hdnRadID.Value = Request.QueryString["radID"];
            hdnCycleID.Value = Request.QueryString["cycleID"];
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            hdnMenuID.Value = intMenuID.ToString();
            hdnUserID.Value = UserID.ToString();
        }
        #endregion

        #region CallBackPmt_Callback
        protected void CallBackPmt_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadAdhocPayments(e.Parameters);
            grdPmt.Width = Unit.Percentage(99);
            grdPmt.RenderControl(e.Output);
            spnErr.RenderControl(e.Output);
        }
        #endregion

        #region LoadAdhocPayments
        private void LoadAdhocPayments(string[] arrRecord)
        {
            objCore = new Core.AP.RadiologistPayment();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            string[] arrID = new string[0];
            objComm = new classes.CommonClass();

            try
            {

                objCore.RADIOLOGIST_ID = new Guid(arrRecord[0]);
                objCore.BILLING_CYCLE_ID= new Guid(arrRecord[1]);

                bReturn = objCore.LoadOtherAdhocPayments(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    grdPmt.DataSource = ds.Tables["AdhoctPmt"];
                    grdPmt.DataBind();
                    spnErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";
                }
                else
                    spnErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";

            }
            catch (Exception ex)
            {
                spnErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
                objComm = null;
            }


        }
        #endregion

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord,string[] ArrPayments)
        {

            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            string[] arrayCode = new string[0];
            string strMsg = string.Empty;
            int intListIndex = 0;

            Core.AP.AdhocPaymentList[] objPmt = new Core.AP.AdhocPaymentList[0];
            objCore = new Core.AP.RadiologistPayment();
            objComm = new classes.CommonClass();

            try
            {
                objCore.RADIOLOGIST_ID = new Guid(ArrRecord[0]);
                objCore.BILLING_CYCLE_ID = new Guid(ArrRecord[1]);
                objCore.USER_ID = new Guid(ArrRecord[2].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[3]);

                objPmt = new Core.AP.AdhocPaymentList[(ArrPayments.Length / 3)];

                #region Populate Payments
                for (int i = 0; i < objPmt.Length; i++)
                {
                    objPmt[i] = new Core.AP.AdhocPaymentList();
                    objPmt[i].HEAD_ID = Convert.ToInt32(ArrPayments[intListIndex]);
                    objPmt[i].ADHOC_AMOUNT = Convert.ToDouble(ArrPayments[intListIndex + 1]);
                    objPmt[i].REMARKS = ArrPayments[intListIndex + 2].Trim();
                   
                    intListIndex = intListIndex + 3;
                }
                #endregion

                intListIndex = 0;

                bReturn = objCore.SaveAdhocPayment(Server.MapPath("~"),objPmt, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    arrayCode = new string[1];
                    arrayCode[0] = strReturnMsg.Trim();
                    strMsg = objComm.SetErrorResources(arrayCode, "N", false, string.Empty, string.Empty);

                    arrRet = new string[2];
                    arrRet[0] = "true";
                    arrRet[1] = strMsg.Trim();
                }
                else
                {


                    if (strCatchMessage.Trim() != "")
                    {
                        arrayCode = new string[1];
                        arrayCode[0] = strCatchMessage.Trim();
                        strMsg = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);

                        arrRet = new string[2];
                        arrRet[0] = "catch";
                        arrRet[1] = strMsg.Trim();
                    }
                    else
                    {
                        arrayCode = new string[strReturnMsg.Split(objComm.RecordDivider).Length];
                        arrayCode = strReturnMsg.Split(objComm.RecordDivider);
                        strMsg = objComm.SetErrorResources(arrayCode, "N", true, objCore.USER_NAME, string.Empty);

                        arrRet = new string[3];
                        arrRet[0] = "false";
                        arrRet[1] = strMsg.Trim();
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