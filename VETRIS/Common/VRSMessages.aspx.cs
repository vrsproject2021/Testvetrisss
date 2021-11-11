using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace VETRIS.Common
{
    public partial class VRSMessages : System.Web.UI.Page
    {
        #region Member Variables
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            bool bErrorHeading = true;
            string strErrorCode = null;
            string strMethod = null;
            string strForm = null;
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
                Request.QueryString.ToString().Replace("+&+", "&amp;");

                if (Request.QueryString["FORM"] != null)
                    strForm = Request.QueryString["FORM"].ToString();

                if (Request.QueryString["METHOD"] != null)
                    strMethod = Request.QueryString["METHOD"].ToString();

                //Error Heading Type
                if (Request.QueryString["ShowErr"] != null)
                {
                    if (Request.QueryString["ShowErr"].ToString() == "undefined")
                    {
                        bErrorHeading = true;
                        hdnMsgType.Value = "true";
                    }
                    else
                    {
                        bErrorHeading = Convert.ToBoolean(Request.QueryString["ShowErr"]);
                        hdnMsgType.Value = Request.QueryString["ShowErr"].ToLower();
                    }
                }

                if (hdnMsgType.Value == "true") lblMsgHdr.ForeColor = System.Drawing.Color.Red;

                if (Request.QueryString["TEXT1"] != null)
                    strText1 = Convert.ToString(Request.QueryString["TEXT1"]);

                if (Request.QueryString["TEXT2"] != null)
                    strText2 = Convert.ToString(Request.QueryString["TEXT2"]);


                if (Request.QueryString["RETVAL"] != null)
                    hdnReturn.Value = Request.QueryString["RETVAL"];

                if (Request.QueryString["ERRCODE"] != null)
                {
                    strErrorCode = Request.QueryString["ERRCODE"].ToString();
                    string[] arrayCode = strErrorCode.Split(objComm.RecordDivider);
                    hdnErrCode.Value = strErrorCode;
                    this.lblErrorDesc.Text = objComm.SetErrorResources(arrayCode, "N", bErrorHeading, strText1, strText2);
                    this.lblErrorDesc.Text = this.lblErrorDesc.Text.Replace("\n", "<br/>");
                }


                string strMessage = string.Empty;

                if (bErrorHeading)
                    strMessage = ConfigurationManager.AppSettings["WindowTitle"] + " : Error";
                else
                    strMessage = ConfigurationManager.AppSettings["WindowTitle"] + " : Message";


                lblMsgHdr.Text = strMessage.Trim();
            }
            catch (Exception LexcErr) { throw LexcErr; }
            finally
            {
                strErrorCode = null;
                strMethod = null;
                strForm = null;
                objComm = null;
            }
        }
        #endregion
    }
}