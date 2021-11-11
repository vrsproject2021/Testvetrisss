using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace VETRIS.Common
{
    public partial class VRSNotes : System.Web.UI.Page
    {
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] arrayCode = new string[1];
            string strRootDir = ConfigurationManager.AppSettings["RootDirectory"];
            this.Page.Title = ConfigurationManager.AppSettings["WindowTitle"];
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            string strTheme = Request.QueryString["th"];

            try
            {
                btnOk.Attributes.Add("onclick", "javascript:btnOk_OnClick();");
                btnClose.Attributes.Add("onclick", "javascript:parent.HideGeneralSmall();");
                lblMsgHdr.Text = Request.QueryString["hdr"];
                lblHelp.Text = "(" + Request.QueryString["hlp"] + ")";
                txtNotes.MaxLength = Convert.ToInt32(Request.QueryString["mc"]);
                hdnMaxChar.Value = Request.QueryString["mc"];
                lnkCUSTOM.Attributes["href"] = strServerPath + "/css/" + strTheme + "/custome-css-style.css";
            }
            catch (Exception expErr) { throw expErr; }
            
        }
        #endregion
    }
}