using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VETRIS.CaseList
{
    public partial class VRSUploadImageFiles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Uploader1.FILE_PATH_TO_SAVE = Server.MapPath("~") + "/CaseList/IMGTemp";
            Uploader1.FOLDER_USER_ID = Request.QueryString["uid"];
        }
    }
}