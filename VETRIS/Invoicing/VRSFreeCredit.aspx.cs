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
    [AjaxPro.AjaxNamespace("VRSFreeCredit")]
    public partial class VRSFreeCredit : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Invoicing.FreeCredit objCore = null;
        classes.CommonClass objComm;
        #endregion
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSInvoiceProcess));
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

            //btnSave1.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            //btnSave2.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            //btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            //btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

        }
        #endregion

        #region CallBackFreeCredit_Callback
        protected void CallBackFreeCredit_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            SearchRecord(e.Parameters);
            grdFreeCredit.Width = Unit.Percentage(100);
            grdFreeCredit.RenderControl(e.Output);
        }
        #endregion

        #region SearchRecord
        private void SearchRecord(string[] arrRecord)
        {

            objCore = new Core.Invoicing.FreeCredit();
            string strCatchMessage = ""; string strReturnMessage = string.Empty;
            bool bReturn = false; 
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();

                objCore.MENU_ID = Convert.ToInt32(arrRecord[0]);
                objCore.USER_ID = new Guid(arrRecord[1]);

                bReturn = objCore.LoadRecords(Server.MapPath("~"), ref ds,ref strReturnMessage, ref strCatchMessage);


                if (bReturn)
                {

                    ds.Tables["FreeCreditHdr"].Columns.Add("proc");
                    ds.Relations.Add(ds.Tables[0].Columns["billing_account_id"], ds.Tables[1].Columns["billing_account_id"]);
                    grdFreeCredit.DataSource = ds;
                    grdFreeCredit.DataBind();

                    //grdFreeCredit.ExpandAll();

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
    }
}