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

namespace VETRIS.Invoicing
{
    public partial class VRSServiceCodes : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Invoicing.StudyAmendment objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            SetAttributes();
            if ((!CallBackCodes.CausedCallback))
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {

            btnClose.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnDone.Attributes.Add("onclick", "javascript:btnDone_OnClick();");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            objComm = new classes.CommonClass();
            hdnID.Value = Request.QueryString["id"];
            objComm = null;
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

        #region CallBackCodes_Callback
        protected void CallBackCodes_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadServices(e.Parameters);
            grdCodes.Width = Unit.Percentage(100);
            grdCodes.RenderControl(e.Output);
            spnErr.RenderControl(e.Output);
        }
        #endregion

        #region LoadServices
        private void LoadServices(string[] arrParams)
        {
            string strCatchMessage = ""; 
            bool bReturn = false;
            DataSet ds= new DataSet();
            string strCodes = string.Empty;
            string[] arrCodes = new string[0];
            DataView dv= new DataView();
            objCore = new Core.Invoicing.StudyAmendment();


            try
            {

                objCore.STUDY_ID = new Guid(arrParams[0]);
                strCodes = arrParams[1].Trim();
                bReturn = objCore.FetchServiceCodes(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    if (strCodes.Trim() != string.Empty)
                    {
                        if (strCodes.Trim().Contains(','))
                            arrCodes = strCodes.Trim().Split(',');
                        else
                        {
                            arrCodes = new string[1];
                            arrCodes[0] = strCodes;
                        }

                        foreach (DataRow dr in ds.Tables["Services"].Rows)
                        {
                            for (int i = 0; i < arrCodes.Length; i++)
                            {
                                if (Convert.ToString(dr["code"]).Trim() == arrCodes[i])
                                {
                                    dr["sel"] = "Y";
                                }
                            }

                        }
                        ds.Tables["Services"].AcceptChanges();
                    }
                    
                    dv = new DataView(ds.Tables["Services"]);
                    dv.Sort = "sel desc,code";
                    grdCodes.DataSource = dv.ToTable();
                    grdCodes.DataBind();
                    spnErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";
                }
                else
                {
                    spnErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage.Trim() + "\" />";
                }


            }
            catch (Exception ex)
            {
                spnErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose(); objCore = null; dv.Dispose();
            }

        }
        #endregion


    }
}