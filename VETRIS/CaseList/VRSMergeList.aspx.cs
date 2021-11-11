using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using VETRIS.Core;

namespace VETRIS.CaseList
{
    public partial class VRSMergeList : System.Web.UI.Page
    {
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            SetAttributes();
            if (!CallBackStudy.CausedCallback) SetPageValue();
        } 
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnMerge.Attributes.Add("onclick", "javascript:UpdateTypeAll('M');");
            btnComp.Attributes.Add("onclick", "javascript:UpdateTypeAll('C');");
            btnNone.Attributes.Add("onclick", "javascript:UpdateTypeAll('N');");
            btnOK.Attributes.Add("onclick", "javascript:ReturnStudies();");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            hdnID.Value = Request.QueryString["id"];
            hdnSUID.Value = Request.QueryString["suid"];
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

        #region CallBackStudy_Callback
        protected void CallBackStudy_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadStudies(e.Parameters);

            
            grdStudy.Width = Unit.Percentage(100);
            grdStudy.RenderControl(e.Output);
  
        }
        #endregion

        #region LoadStudies
        private void LoadStudies(string[] arrParams)
        {
            Core.Case.CaseStudy objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            classes.CommonClass objComm = new classes.CommonClass();

            try
            {

                objCore.ID = new Guid(arrParams[0]);
                objCore.STUDY_UID = arrParams[1].Trim();

                bReturn = objCore.LoadStudyToMerge(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    objComm.SetRegionalFormat();
                    grdStudy.DataSource = ds.Tables["Study"];
                    grdStudy.Levels[0].Columns[2].FormatString = objComm.DateFormat + " HH:mm";
                    grdStudy.Levels[0].Columns[3].FormatString = objComm.DateFormat + " HH:mm";
                    grdStudy.DataBind();
                }
                


            }
            catch (Exception ex)
            {
                ;
            }
            finally
            {
                ds.Dispose();
                objCore = null;
                objComm = null;
            }
        }
        #endregion


    }
}