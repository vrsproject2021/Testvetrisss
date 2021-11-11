using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace VETRIS.CaseList
{
    public partial class ImageUploader : System.Web.UI.UserControl
    {
        public string FILE_PATH_TO_SAVE = string.Empty;
        public string FOLDER_USER_ID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            hdnPathToSave.Value = FILE_PATH_TO_SAVE.Trim();
            hdnUserID.Value = FOLDER_USER_ID;
            hdnRootDirectory.Value = ConfigurationManager.AppSettings["RootDirectory"];
            rdoFile.Attributes.Add("onclick", "javascript:SetUploaderAttribute();");
            rdoFolder.Attributes.Add("onclick", "javascript:SetUploaderAttribute();");

        }
    }
}