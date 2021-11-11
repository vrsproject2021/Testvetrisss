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

namespace VETRIS.Invoicing
{
    [AjaxPro.AjaxNamespace("VRSChargesDiscount")]
    public partial class VRSChargesDiscount : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Invoicing.ChargesDiscount objCore = null;
        classes.CommonClass objComm;
        #endregion
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSChargesDiscount));
            SetAttributes();
            SetPageValue();
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);

            hdnID.Value = Request.QueryString["id"];
            //LoadDetails(intMenuID, UserID);
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {

            btnSave1.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnSave2.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            //btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            //btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

        }
        #endregion

        #region CallBackChargesDiscount_Callback
        protected void CallBackChargesDiscount_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            SearchRecord(e.Parameters);
            grdChargesDiscount.Width = Unit.Percentage(100);
            grdChargesDiscount.RenderControl(e.Output);
        }
        #endregion

        #region SearchRecord
        private void SearchRecord(string[] arrRecord)
        {
            objCore = new Core.Invoicing.ChargesDiscount();
            string strCatchMessage = ""; bool bReturn = false; string strMailType = string.Empty;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.USER_ID = new Guid(arrRecord[0]);
                objCore.MENU_ID = Convert.ToInt32(arrRecord[1]);
                
                bReturn = objCore.LoadRecords(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    grdChargesDiscount.DataSource = ds;
                    grdChargesDiscount.DataBind();

                    //grdChargesDiscount.ExpandAll();
                }
                else
                    Response.Write(strCatchMessage);

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.Trim());
            }
            finally
            {
                ds.Dispose(); objComm = null;

            }
        }
        #endregion

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord,string[] ArrParams)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Invoicing.ChargesDiscount();
            objComm = new classes.CommonClass();
            Core.Invoicing.ChargesDiscount[] objRecords = new Core.Invoicing.ChargesDiscount[0];

            try
            {
                objCore.USER_ID = new Guid(ArrParams[0].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrParams[1]);

                objRecords = new Core.Invoicing.ChargesDiscount[(ArrRecord.Length / 3)];

                for (int i = 0; i < objRecords.Length; i++)
                {
                    objRecords[i] = new Core.Invoicing.ChargesDiscount();
                    objRecords[i].BILLING_ACCOUNT_ID = new Guid(ArrRecord[intListIndex]);
                    objRecords[i].CODE = ArrRecord[intListIndex + 1].Trim();
                    objRecords[i].CHARGES_DISCOUNT = Convert.ToDecimal( ArrRecord[intListIndex + 2]);
                    intListIndex = intListIndex + 3;
                }

                intListIndex = 0;

                bReturn = objCore.SaveRecord(Server.MapPath("~"),objRecords,ref strReturnMsg,ref strCatchMessage);
                if (bReturn)
                {
                    arrRet = new string[3];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
                    arrRet[2] = objCore.BILLING_ACCOUNT_ID.ToString();
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