using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace VETRIS.Common
{
    public partial class VRSConfirm : System.Web.UI.Page
    {
        #region Member Variables
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            string strErrorCode = null;
            string strText1 = string.Empty;
            string strText2 = string.Empty;
            string strTheme = string.Empty;

            string strRootDir = ConfigurationManager.AppSettings["RootDirectory"];
            this.Page.Title = ConfigurationManager.AppSettings["WindowTitle"];
            try
            {
                strTheme = Request.QueryString["TH"];
                lnkPOP.Attributes["href"] = "~/css/" + strTheme + "/popupCustom.css";

                objComm = new classes.CommonClass();
                btnYes.Attributes.Add("onclick", "javascript:parent.HideConfirm('Y');");
                btnNo.Attributes.Add("onclick", "javascript:parent.HideConfirm('N');");


                if (Request.QueryString["TEXT1"] != null)
                    strText1 = Convert.ToString(Request.QueryString["TEXT1"]);

                if (Request.QueryString["TEXT2"] != null)
                    strText2 = Convert.ToString(Request.QueryString["TEXT2"]);

                if (Request.QueryString["ERRCODE"] != null)
                {
                    strErrorCode = Request.QueryString["ERRCODE"].ToString();
                    string[] arrayCode = strErrorCode.Split(objComm.RecordDivider);

                    this.lblConfMsg.Text = objComm.SetErrorResources(arrayCode, "Y", false, strText1, strText2);
                }


                string strMessage = string.Empty;

                strMessage = ConfigurationManager.AppSettings["WindowTitle"] + " : Please Confirm";


                lblMsgHdr.Text = strMessage.Trim();
            }
            catch (Exception LexcErr) { throw LexcErr; }
            finally
            {
                strErrorCode = null;

                objComm = null;
            }
        }
        #endregion
    }
}