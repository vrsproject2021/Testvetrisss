using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using VETRIS.Core;

namespace VETRIS.Registration
{
    [AjaxPro.AjaxNamespace("VRSRegConfirmation")]
    public partial class VRSRegConfirmation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSRegConfirmation));
        }
    }
}