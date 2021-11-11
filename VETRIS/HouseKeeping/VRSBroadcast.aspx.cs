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

namespace VETRIS.HouseKeeping
{
    [AjaxPro.AjaxNamespace("VRSBroadcast")]
    public partial class VRSBroadcast : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.HouseKeeping.Broadcast objCore = null;
        #endregion
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSBroadcast));
            SetAttributes();
            if (!CallBackInst.CausedCallback)
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {

            rdoEmail.Attributes.Add("onclick", "javascript:rdoMessage_OnCheckedChange('E');");
            rdoSMS.Attributes.Add("onclick", "javascript:rdoMessage_OnCheckedChange('S');");
            //btnRefresh.Attributes.Add("onclick", "javascript:ResetRecord();view_Searchform();");
            btnSend.Attributes.Add("onclick", "javascript:btnSend_OnClick();");
            btnClose.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_Onclick();");
            ddlInstitution.Attributes.Add("onchange", "javascript:ddlInstitution_OnChange(this.value);");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid lUserID = new Guid(Request.QueryString["uid"].ToString());
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
        #region SearchRecord
        private void SearchRecord(string[] arrRecord)
        {
            objCore = new Core.HouseKeeping.Broadcast();
            string strCatchMessage = "";
            string strFlag = "";
            bool bReturn = false;
            int recipient_id = 0;
            DataSet ds = new DataSet();

            try
            {
               
                objCore.USER_ID = new Guid(arrRecord[0]);
                recipient_id = Convert.ToInt32(arrRecord[1]);

                bReturn = objCore.SearchBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    if (recipient_id == 1)
                    {
                        grdInst.DataSource = ds.Tables["Institution"];
                        grdInst.DataBind();
                    }
                    if (recipient_id == 2)
                    {
                        grdInst.DataSource = ds.Tables["Salesperson"];
                        grdInst.DataBind();
                    }
                    if (recipient_id == 3)
                    {
                        grdInst.DataSource = ds.Tables["Radiologist"];
                        grdInst.DataBind();
                    }
                    if (recipient_id == 4)
                    {
                        grdInst.DataSource = ds.Tables["Technician"];
                        grdInst.DataBind();
                    }
                    if (recipient_id == 5)
                    {
                        grdInst.DataSource = ds.Tables["Transcriptionist"];
                        grdInst.DataBind();
                    }
                    
                }
                else
                    Response.Write(strCatchMessage);



            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.Trim());
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region CallBackInst_Callback
        protected void CallBackInst_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strUnlock = e.Parameters[0];
            string strFlag = e.Parameters[2];
            SearchRecord(e.Parameters);
            if (strFlag == "E")
            {
                grdInst.Levels[0].Columns[3].Visible = true;
                grdInst.Levels[0].Columns[4].Visible = false;
            }
            else if (strFlag == "S")
            {
                grdInst.Levels[0].Columns[3].Visible = false;
                grdInst.Levels[0].Columns[4].Visible = true;
            }

            grdInst.Width = Unit.Percentage(100);
            grdInst.RenderControl(e.Output);
            
        }
        #endregion

        #region SendMessage (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SendMessage(string[] ArrRecord, string[] ArrParams)
        {
            bool bReturn            = false;
            int intCompanyID        = 0;
            int intListIndex        = 0;
 
            string[] arrRet         = new string[0];
            string strReturnMsg     = string.Empty; 
            string strCatchMessage  = string.Empty;
            string strFileName      = string.Empty;
            string strBroadcastFlag = string.Empty; 

            Guid UserID             = Guid.Empty;

            objComm                 = new classes.CommonClass();
            objCore                 = new Core.HouseKeeping.Broadcast();
            Core.HouseKeeping.Broadcast[] objRecords = new Core.HouseKeeping.Broadcast[0];

            try
            {

                objCore.USER_ID         = UserID = new Guid(ArrParams[0]);
                objCore.MENU_ID         = Convert.ToInt32(ArrParams[1]);
                objCore.EMAIL_SUBJECT   = ArrParams[2].Trim();
                objCore.EMAIL_BODY      = ArrParams[3].Trim();
                objCore.SMS_TEXT        = ArrParams[4].Trim();
                strBroadcastFlag        = ArrParams[5].Trim();

                objRecords = new Core.HouseKeeping.Broadcast[(ArrRecord.Length / 4)];

                for (int i = 0; i < objRecords.Length; i++)
                {
                    objRecords[i]                   = new Core.HouseKeeping.Broadcast();
                    //objRecords[i].ROW_ID            = Convert.ToInt32(ArrRecord[intListIndex]);
                    objRecords[i].ID                = new  Guid(ArrRecord[intListIndex]);
                    objRecords[i].NAME              = ArrRecord[intListIndex + 1].Trim();
                    objRecords[i].RECIPEINT_EMAIL   = ArrRecord[intListIndex + 2].Trim();
                    objRecords[i].RECIPIENT_NO      = ArrRecord[intListIndex + 3].Trim();
                    intListIndex                    = intListIndex + 4;
                }

                intListIndex = 0;


                bReturn = objCore.SaveRecord(Server.MapPath("~"), objRecords,strBroadcastFlag, ref strReturnMsg, ref strCatchMessage);

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