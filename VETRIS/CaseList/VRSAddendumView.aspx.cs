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

namespace VETRIS.CaseList
{
    public partial class VRSAddendumView : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Case.CaseStudy objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            SetAttributes();
            SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnClose1.Attributes.Add("onclick", "javascript:parent.HideDataList();");
            btnClose2.Attributes.Add("onclick", "javascript:parent.HideDataList();");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            hdnDivider.Value = objComm.RecordDivider.ToString();
            objComm = null;
            Guid ID = new Guid(Request.QueryString["Id"]);
            int intSrlNo = Convert.ToInt32(Request.QueryString["Srl"]);
            lblSrl.Text = intSrlNo.ToString();
            SetCSS(Request.QueryString["th"]);
            FetchAddendum(ID, intSrlNo);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
        }
        #endregion

        #region FetchAddendum
        private void FetchAddendum(Guid ID,int SrlNo)
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; bool bReturn = false;
            string[] arrayCode = new string[0];
            objComm = new classes.CommonClass();
            string strSender = string.Empty;
            StringBuilder sb = new StringBuilder();


            try
            {
                objComm.SetRegionalFormat();
                objCore.ID = ID;
                objCore.ADDENDUM_SERIAL = SrlNo;


                bReturn = objCore.FetchAddendum(Server.MapPath("~"),  ref strCatchMessage);

                if (bReturn)
                {
                    divAddnText.InnerHtml = objCore.ADDENDUM_TEXT;
                }
                else
                {
                    arrayCode = new string[1];
                    arrayCode[0] = strCatchMessage.Trim();
                    lblMsg.Text = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
                    hdnError.Value = "Y";
                }
            }
            catch (Exception ex)
            {
                arrayCode = new string[1];
                arrayCode[0] = ex.Message.Trim();
                lblMsg.Text = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
                hdnError.Value = "Y";
            }
            finally
            {
                objCore = null;
                objComm = null;
            }
        }
        #endregion
    }
}