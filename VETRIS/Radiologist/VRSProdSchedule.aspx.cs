using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;

namespace VETRIS.Radiologist
{
    public partial class VRSProdSchedule : System.Web.UI.Page
    {
        public string baseUrl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string strTheme = Request.QueryString["th"];
            SetCSS(strTheme);
        }

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkTABLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/table.css";
            lnkSEL.Attributes["href"] = strServerPath + "/css/" + strTheme + "/select2.css";
        }
        #endregion
    }
}