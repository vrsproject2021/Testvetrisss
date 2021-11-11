using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using VETRIS.Core;

namespace VETRIS
{
    public partial class VRSHome : System.Web.UI.Page
    {
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageValue();
            ClearCache();
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
             Guid UserID = Guid.Empty;
             Guid SessionID = Guid.Empty;
             string strTheme = string.Empty;
             strTheme = Request.QueryString["th"];
            
            if (Request.QueryString["uid"] != null)
            {
                UserID = new Guid(Request.QueryString["uid"]);
                SessionID = new Guid(Request.QueryString["sid"]);
                UnlockUserLockedRecords(UserID,SessionID);
            }
            SetCss(strTheme);
        }
        #endregion

        #region SetCSS
        private void SetCss(string theme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];

            switch (theme)
            {
                case "DARK":
                    imgBg.Src = strServerPath + "/images/pawBG_Dark.png";
                    break;
                case "DEFAULT":
                    imgBg.Src = strServerPath + "/images/pawBG_Default.png";
                    break;
            }
        }
        #endregion

        #region ClearCache
        private void ClearCache()
        {
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
        }
        #endregion

        #region UnlockUserLockedRecords
        private void UnlockUserLockedRecords(Guid UserID,Guid SessionID)
        {
            Core.CommonFunctions objCF = new CommonFunctions();
            classes.CommonClass objComm = new classes.CommonClass();
            bool bReturn = false; string strReturnMessage = ""; string strCatchMessage = "";


            try
            {
                objCF.USER_ID = UserID;
                objCF.USER_SESSION_ID = SessionID;
                bReturn = objCF.UnlockUserLockedRecords(Server.MapPath("~"),ref strCatchMessage);
              
                if (!bReturn)
                {
                    if (strCatchMessage.Trim() != String.Empty)
                        hdnError.Value = "catch" + objComm.RecordDivider + strCatchMessage.Trim();
                    else
                        hdnError.Value = "false" + objComm.RecordDivider + strReturnMessage.Trim();
                }

            }
            catch (Exception ex)
            {
                hdnError.Value = "catch" + objComm.RecordDivider + ex.Message.Trim();
            }
            finally
            {
                objComm = null; objCF = null;
            }
        }
        #endregion
    }
}