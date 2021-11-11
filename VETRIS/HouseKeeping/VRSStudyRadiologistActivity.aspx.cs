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

namespace VETRIS.HouseKeeping
{
    public partial class VRSStudyRadiologistActivity : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.HouseKeeping.RadiologistActivity objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CallBackBrw.CausedCallback) SetPageValue();
            SetAttributes();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {

            btnClose1.Attributes.Add("onclick", "javascript:btnClose_Onclick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_Onclick();");
            btnOK.Attributes.Add("onclick", "javascript:btnOK_Onclick();");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            if (Request.QueryString["cf"] != null)
            {
                hdnCF.Value = Request.QueryString["cf"];
                txtSUID.Text = Request.QueryString["suid"];
            }
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
        }
        #endregion

        #region CallBackBrw_Callback
        protected void CallBackBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            FetchActivity(e.Parameters);
            grdBrw.Width = Unit.Percentage(100);
            grdBrw.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
            spnDtls.RenderControl(e.Output);
        }
        #endregion

        #region FetchActivity
        private void FetchActivity(string[] arrRecord)
        {
            objCore = new Core.HouseKeeping.RadiologistActivity();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();


            try
            {
                objComm.SetRegionalFormat();
                objCore.STUDY_UID = arrRecord[0].Trim();
                objCore.USER_ID = new Guid(arrRecord[1].Trim());
                objCore.USER_SESSION_ID = new Guid(arrRecord[2].Trim());

                bReturn = objCore.FetchActivity(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    grdBrw.DataSource = ds.Tables["ActivityList"];
                    grdBrw.Levels[0].Columns[4].FormatString = objComm.DateFormat + " HH:mm:ss";
                    grdBrw.DataBind();
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";
                    
                }
                else
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";

                spnDtls.InnerHtml = "<input type=\"hidden\" id=\"hdnPDtls\" value=\"" + objCore.PATIENT_NAME + "\" />";

            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
                spnDtls.InnerHtml = "<input type=\"hidden\" id=\"hdnPDtls\" value=\"\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }

        }
        #endregion
    }
}