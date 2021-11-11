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
using AjaxPro;

namespace VETRIS.Radiologist
{
   [AjaxPro.AjaxNamespace("VRSGroups")]
    public partial class VRSGroups : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Radiologist.Group objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSGroups));
            SetAttributes();
            if ((!CallBackBrw.CausedCallback))
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {

            btnRefresh.Attributes.Add("onclick", "javascript:ResetRecord();view_Searchform();");
            btnSearch.Attributes.Add("onclick", "javascript:SearchRecord();view_Searchform();");
            btnSave.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnClose.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");

        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"].ToString());
            string strID = string.Empty;
            string strTheme = Request.QueryString["th"];
            SetCSS(strTheme);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
        }
        #endregion
        #region Grid

        #region CallBackBrw_Callback
        protected void CallBackBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];

            switch (strAction)
            {
                case "L":
                    LoadRecords(e.Parameters);
                    break;
               
            }
            grdBrw.Width = Unit.Percentage(100);
            grdBrw.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadRecords
        private void LoadRecords(string[] arrParams)
        {
            objCore = new Core.Radiologist.Group();
            string strCatchMessage = ""; string strReturnMessage = "";
            bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";

            try
            {

                objCore.NAME = Convert.ToString(arrParams[1]).Trim();
                objCore.USER_ID = new Guid(arrParams[2]);
                objCore.MENU_ID = Convert.ToInt32(arrParams[3]);

                bReturn = objCore.SearchRecords(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);
                if (bReturn)
                {
                    grdBrw.DataSource = ds.Tables["BrowserList"];
                    grdBrw.DataBind();
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";
                }
                else
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";

            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #endregion

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord, string[] ArrParams)
        {
            bool bReturn = false;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            int intCompanyID = 0; string strFileName = string.Empty; Guid UserID = Guid.Empty;
            objComm = new classes.CommonClass();
            objCore = new Core.Radiologist.Group();
            int intListIndex = 0;
            Core.Radiologist.Group[] objRecords = new Core.Radiologist.Group[0];

            try
            {

                objCore.USER_ID = UserID = new Guid(ArrParams[0]);
                objCore.MENU_ID = Convert.ToInt32(ArrParams[1]);


                objRecords = new Core.Radiologist.Group[(ArrRecord.Length / 3)];

                for (int i = 0; i < objRecords.Length; i++)
                {
                    objRecords[i] = new Core.Radiologist.Group();
                    objRecords[i].ROW_ID = Convert.ToInt32(ArrRecord[intListIndex]);
                    objRecords[i].ID = Convert.ToInt32(ArrRecord[intListIndex + 1]);
                    objRecords[i].DISPLAY_ORDER = Convert.ToInt32(ArrRecord[intListIndex + 2]);
                    intListIndex = intListIndex + 3;
                }

                intListIndex = 0;


                bReturn = objCore.SaveRecord(Server.MapPath("~"), objRecords, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[2];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
                }
                else
                {

                    if (strCatchMessage.Trim() != "")
                    {
                        arrRet = new string[2];
                        arrRet[0] = "catch";
                        arrRet[1] = strCatchMessage.Trim();

                    }
                    else
                    {
                        arrRet = new string[3];
                        arrRet[0] = "false";
                        arrRet[1] = strReturnMsg.Trim();
                        arrRet[2] = objCore.USER_NAME;
                    }
                }
            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = expErr.Message.Trim();
            }
            finally
            {
                objCore = null; objComm = null;
                strReturnMsg = null; strCatchMessage = null;
            }
            return arrRet;
        }
        #endregion 
    }
}