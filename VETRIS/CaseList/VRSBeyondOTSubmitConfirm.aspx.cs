using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace VETRIS.CaseList
{
    public partial class VRSBeyondOTSubmitConfirm : System.Web.UI.Page
    {
        #region Member Variables
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] arrayCode = new string[1];
            string strRootDir = ConfigurationManager.AppSettings["RootDirectory"];
            this.Page.Title = ConfigurationManager.AppSettings["WindowTitle"];
            string strMessage = string.Empty;

            try
            {

                objComm = new classes.CommonClass();
                btnSubmit.Attributes.Add("onclick", "javascript:btnSubmit_OnClick();");

                arrayCode[0] = Request.QueryString["msg"];
                strMessage = ConfigurationManager.AppSettings["WindowTitle"] ;
                lblMsgHdr.Text = strMessage.Trim();

                lblBusinessHr.Text = "Our business hour starts as " + Request.QueryString["ot"] + " (" + Request.QueryString["tz"] + ")";
                
                //this.lblConfMsg.Text = objComm.SetErrorResources(arrayCode, "N", false, string.Empty, string.Empty);
                lblStat.Text = lblStat.Text + " <b>" + Request.QueryString["stat"] + " ( " + Request.QueryString["tz"] + " )</b>";
                lblStd.Text = lblStd.Text + " <b>" + Request.QueryString["std"]+ " ( " + Request.QueryString["tz"] + " )</b>";
            }
            catch (Exception expErr) { throw expErr; }
            finally
            {
                objComm = null;
            }
        }
        #endregion
    }
}