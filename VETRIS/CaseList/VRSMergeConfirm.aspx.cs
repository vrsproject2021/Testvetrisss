using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace VETRIS.CaseList
{
    public partial class VRSMergeConfirm : System.Web.UI.Page
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
                btnMerge.Attributes.Add("onclick", "javascript:parent.HideGeneralSmall('M');");
                btnComp.Attributes.Add("onclick", "javascript:parent.HideGeneralSmall('C');");
                btnNone.Attributes.Add("onclick", "javascript:parent.HideGeneralSmall('N');");

                arrayCode[0] = "165";
                this.lblConfMsg.Text = objComm.SetErrorResources(arrayCode, "Y", false, string.Empty, string.Empty);
                strMessage = ConfigurationManager.AppSettings["WindowTitle"] + " : Please Confirm";
                lblMsgHdr.Text = strMessage.Trim();
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