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
    public partial class VRSStatusAuditTrail : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.HouseKeeping.StudyAuditTrail objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CallBackStat.CausedCallback)
                SetPageValue();
            SetAttributes();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnClose.Attributes.Add("onclick", "javascript:parent.HideDataList();");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            hdnID.Value = Request.QueryString["id"].ToString();
            lblHdr.Text = Request.QueryString["suid"].ToString();
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
            lnkCAL.Attributes["href"] = strServerPath + "/css/" + strTheme + "/CalendarStyle.css";
        }
        #endregion

        #region CallBackStat_Callback
        protected void CallBackStat_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            SearchRecord(e.Parameters);
            grdStat.Width = Unit.Percentage(100);
            grdStat.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region SearchRecord
        private void SearchRecord(string[] arrRecord)
        {
            objCore = new Core.HouseKeeping.StudyAuditTrail();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.ID = new Guid(arrRecord[0].Trim());

                bReturn = objCore.FetchAuditTrail(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    grdStat.DataSource = ds.Tables["AuditTrail"];
                    grdStat.Levels[0].Columns[4].FormatString = objComm.DateFormat + " HH:mm:ss";
                    grdStat.DataBind();
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

    }
}