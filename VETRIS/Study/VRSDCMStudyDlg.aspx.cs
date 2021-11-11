using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.Data;
using System.IO;
using System.Net;
using VETRIS.Core;
using AjaxPro;

namespace VETRIS.Study
{
    [AjaxPro.AjaxNamespace("VRSDCMStudyDlg")]
    public partial class VRSDCMStudyDlg : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.Study.DCMStudy objCore;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSDCMStudyDlg));
            SetAttributes();
            if ((!CallBackFiles.CausedCallback))
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);

            btnSave1.Attributes.Add("onclick", "javascript:btnSubmit_OnClick('N','X');");
            btnSave2.Attributes.Add("onclick", "javascript:btnSubmit_OnClick('N','X');");
            btnSubmit1.Attributes.Add("onclick", "javascript:btnSubmit_OnClick('Y','X');");
            btnSubmit2.Attributes.Add("onclick", "javascript:btnSubmit_OnClick('Y','X');");
            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
           
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

            txtFromDt.Attributes.Add("onblur", "javascript:txtFromDt_OnBlur();");
            imgFrom.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtFromDt.ClientID + "','CalFrom');");

           

            //lblViewLog.Attributes.Add("onclick", "javascript:ViewLogUI('" + intMenuID.ToString() + "');");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            hdnID.Value = Request.QueryString["id"];
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            txtFromDt.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            objComm = null;
           
            LoadHeader(intMenuID, UserID);
        }
        #endregion

        #region LoadHeader
        private void LoadHeader(int intMenuID, Guid UserID)
        {
            objCore = new Core.Study.DCMStudy();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objCore.ID = new Guid(hdnID.Value);
                objCore.MENU_ID = intMenuID;
                objCore.USER_ID = UserID;

                bReturn = objCore.LoadHeader(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    objComm.SetRegionalFormat();

                    if (objCore.STUDY_UID != string.Empty)
                    {

                       
                        lblSUID.Text = objCore.STUDY_UID; hdnSUID.Value = objCore.STUDY_UID;
                        txtPID.Text = objCore.PATIENT_ID;
                        txtPFName.Text = objCore.PATIENT_FIRST_NAME;
                        txtPLName.Text = objCore.PATIENT_LAST_NAME;




                        if (objCore.STUDY_DATE.Year > 1900) txtFromDt.Text = objComm.IMDateFormat(objCore.STUDY_DATE, objComm.DateFormat);
                        else txtFromDt.Text = string.Empty;

                        hdnInstID.Value = objCore.INSTITUTION_ID.ToString();
                        lblInstName.Text = "[" + objCore.INSTITUTION_CODE.Trim() + "] "  + objCore.INSTITUTION_NAME.Trim();
                       
                        lblImgCount.Text = objCore.FILE_COUNT.ToString() + "/" + objCore.FILE_TRANSFERED.ToString();
                        hdnAppv.Value = objCore.APPROVED;
                        
                    }
                    else
                    {
                        hdnError.Value = "false" + objComm.RecordDivider + "094";
                    }
                }
                else
                {
                    if (strCatchMessage.Trim() != string.Empty)
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
                ds.Dispose(); objComm = null; objCore = null;
            }
        }
        #endregion

        #region File Grid

        #region CallBackFiles_Callback
        protected void CallBackFiles_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadFiles(e.Parameters);
            grdFiles.Width = Unit.Percentage(100);
            grdFiles.RenderControl(e.Output);
            spnErrFiles.RenderControl(e.Output);
        }
        #endregion

        #region LoadFiles
        private void LoadFiles(string[] arrParams)
        {
            objCore = new Core.Study.DCMStudy();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";

            try
            {

                objCore.ID = new Guid(arrParams[0]);
              

                bReturn = objCore.LoadFiles(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdFiles.DataSource = ds.Tables["Files"];
                    grdFiles.DataBind();


                    spnErrFiles.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrFiles\" value=\"\" />";
                }
                else
                    spnErrFiles.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrFiles\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnErrFiles.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrFiles\" value=\"" + ex.Message.Trim() + "\" />";
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
        public string[] SaveRecord(string[] ArrRecord)
        {
            bool bReturn = false;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            objComm = new classes.CommonClass();
            objCore = new Core.Study.DCMStudy();
            try
            {
                objCore.ID = new Guid(ArrRecord[0]);
                objCore.STUDY_UID = ArrRecord[1].Trim();
                objCore.PATIENT_ID = ArrRecord[2].Trim();
                objCore.PATIENT_FIRST_NAME = ArrRecord[3].Trim();
                objCore.PATIENT_LAST_NAME = ArrRecord[4].Trim();
                objCore.PATIENT_NAME = ArrRecord[3].Trim() + " " + ArrRecord[4].Trim();
                objCore.STUDY_DATE = Convert.ToDateTime(ArrRecord[5]);
                objCore.APPROVED = ArrRecord[6].Trim();
                objCore.USER_ID = new Guid(ArrRecord[7]);
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[8]);


                bReturn = objCore.SaveRecord(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

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