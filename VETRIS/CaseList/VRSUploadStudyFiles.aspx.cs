using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VETRIS.CaseList
{
    public partial class VRSUploadStudyFiles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.QueryString["path"]==null) Uploader1.FILE_PATH_TO_SAVE = Server.MapPath("~") + "/CaseList/MSTemp";
            else Uploader1.FILE_PATH_TO_SAVE = Server.MapPath("~") + "/" + Request.QueryString["path"].Replace("_","/");
            Uploader1.FOLDER_USER_ID = Request.QueryString["uid"];

            string strTheme = Request.QueryString["th"];
            Uploader1.THEME = strTheme;

            SetCSS(strTheme);
        }

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";

        }
        #endregion
    }
}