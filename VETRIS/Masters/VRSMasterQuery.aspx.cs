using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using VETRIS.Core;
using AjaxPro;

namespace VETRIS.Masters
{
    [AjaxPro.AjaxNamespace("VRSMasterQuery")]
    public partial class VRSMasterQuery : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Master.MasterQuery objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSMasterQuery));
            SetAttributes();
            if (!CallBackBrw.CausedCallback) SetPageValue();
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            hdnID.Value = Request.QueryString["id"];
            string strTheme = Request.QueryString["th"];
            SetCSS(strTheme);

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

            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");
            btnSearch.Attributes.Add("onclick", "javascript:SearchRecord();");
            btnRefresh.Attributes.Add("onclick", "javascript:ResetRecord();");

        }
        #endregion

        #region CallBackBrw_Callback
        protected void CallBackBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadDetails(e.Parameters);
            grdBrw.Width = Unit.Percentage(100);
            grdBrw.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadDetails
        private void LoadDetails(string[] arrRecord)
        {
            objCore = new Core.Master.MasterQuery();
            bool bReturn = false; string strCatchMessage = "";
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();


            try
            {
                objComm.SetRegionalFormat();
                objCore.BILLING_ACCOUNT_CODE = arrRecord[0].Trim();
                objCore.BILLING_ACCOUNT_NAME = arrRecord[1].Trim();
                objCore.BILLING_ACCOUNT_IS_ACTIVE = arrRecord[2].Trim();
                objCore.INSTITUTION_CODE = arrRecord[3].Trim();
                objCore.INSTITUTION_NAME = arrRecord[4].Trim();
                objCore.INSTITUTION_IS_ACTIVE = arrRecord[5].Trim();
                objCore.LOGIN_ID = arrRecord[6].Trim();
                objCore.USER_IS_ACTIVE = arrRecord[7].Trim();
                objCore.USER_ID = new Guid(arrRecord[8]);

                bReturn = objCore.SearchBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    ds.Relations.Add(ds.Tables["BillingAccount"].Columns["id"], ds.Tables["Institutions"].Columns["billing_account_id"]);
                    ds.Relations.Add(ds.Tables["Institutions"].Columns["institution_id"], ds.Tables["Users"].Columns["institution_id"]);
                    grdBrw.DataSource = ds;
                    grdBrw.DataBind();
                    grdBrw.PageSize = ds.Tables["BillingAccount"].Rows.Count;
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

                }
                else
                {

                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";

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

           
        }
        #endregion
    }
}