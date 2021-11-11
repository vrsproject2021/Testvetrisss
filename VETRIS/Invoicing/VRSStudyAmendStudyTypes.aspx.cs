using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using System.Net;
using System.Configuration;
using VETRIS.Core;

namespace VETRIS.Invoicing
{
     [AjaxPro.AjaxNamespace("VRSStudyAmendStudyTypes")]
    public partial class VRSStudyAmendStudyTypes : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Invoicing.StudyAmendment objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSStudyAmendStudyTypes));
            SetAttributes();
            if ((!CallBackST.CausedCallback) && (!CallBackSelST.CausedCallback))
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {

            btnClose.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnDone.Attributes.Add("onclick", "javascript:btnDone_OnClick();");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            objComm = new classes.CommonClass();
            hdnID.Value = Request.QueryString["id"];
            hdnModalityID.Value = Request.QueryString["mod"];

            if(Request.QueryString["cycle"]!=null) hdnCycleID.Value = Request.QueryString["cycle"];
            objComm = null;
            SetCSS(Request.QueryString["th"]);
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

        #region Study Type Grid

        #region CallBackST_Callback
        protected void CallBackST_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadStudyTypes(e.Parameters);
            grdST.Width = Unit.Percentage(100);
            grdST.RenderControl(e.Output);
            spnErrST.RenderControl(e.Output);
        }
        #endregion

        #region LoadStudyTypes
        private void LoadStudyTypes(string[] arrParams)
        {
            string strCatchMessage = "";
            bool bReturn = false;
            DataSet ds = new DataSet();
            string strCodes = string.Empty;
            string[] arrCodes = new string[0];
            DataView dv = new DataView();
            objCore = new Core.Invoicing.StudyAmendment();


            try
            {

                objCore.STUDY_ID = new Guid(arrParams[0]);
                objCore.MODALITY_ID = Convert.ToInt32(arrParams[1]);
               

                bReturn = objCore.FetchStudyTypes(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    grdST.DataSource = ds.Tables["StudyTypes"];
                    grdST.DataBind();
                    spnErrST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrST\" value=\"\" />";
                }
                else
                {
                    spnErrST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrST\" value=\"" + strCatchMessage.Trim() + "\" />";
                }


            }
            catch (Exception ex)
            {
                spnErrST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrST\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose(); objCore = null; 
            }

        }
        #endregion

        #endregion

        #region Selected Study Type Grid

        #region CallBackSelST_Callback
        protected void CallBackSelST_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];
            switch (strAction)
            {
                case "L":
                    LoadSelectedStudyTypes(e.Parameters);
                    break;
                case "U":
                    UpdateStudyTypes(e.Parameters);
                    break;
            }

            grdSelST.Width = Unit.Percentage(100);
            grdSelST.RenderControl(e.Output);
            spnErrSelST.RenderControl(e.Output);
        }
        #endregion

        #region LoadSelectedStudyTypes
        private void LoadSelectedStudyTypes(string[] arrParams)
        {
            objCore = new Core.Invoicing.StudyAmendment();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";

            try
            {

                objCore.STUDY_ID= new Guid(arrParams[1]);
                objCore.MODALITY_ID = Convert.ToInt32(arrParams[2]);

                bReturn = objCore.FetchSelectedStudyTypes(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdSelST.DataSource = ds.Tables["SelStudyTypes"];
                    grdSelST.DataBind();
                    grdSelST.GroupingNotificationText = "Study Type Count : " + ds.Tables["SelStudyTypes"].Rows.Count.ToString();

                    spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelST\" value=\"\" />";
                }
                else
                    spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelST\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelST\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region UpdateStudyTypes
        private void UpdateStudyTypes(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;

            try
            {
                dtbl = CreateSTTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 2)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["srl_no"] = intSrl;
                            dr["study_type_id"] = arrRecords[i].Trim();
                            dr["study_type_name"] = arrRecords[i + 1].Trim();
                            dtbl.Rows.Add(dr);
                        }

                    }
                }



                grdSelST.DataSource = dtbl;
                grdSelST.DataBind();
                grdSelST.GroupingNotificationText = "Study Type Count : " + dtbl.Rows.Count.ToString();
                spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelST\" value=\"UPDATE\" />";
            }
            catch (Exception ex)
            {
                spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelST\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region CreateSTTable
        private DataTable CreateSTTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("srl_no", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("study_type_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("study_type_name", System.Type.GetType("System.String"));
            dtbl.TableName = "SelStudyTypes";
            return dtbl;
        }
        #endregion

        #endregion

        #region SaveStudyTypes (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveStudyTypes(string[] ArrRecord, string[] ArrSTs)
        {
            bool bReturn = false;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string[] arrayCode = new string[0];
            int intListIndex = 0;

            objComm = new classes.CommonClass();
            objCore = new Core.Invoicing.StudyAmendment();
            Core.Invoicing.StudyTypeList[] objSTs = new Core.Invoicing.StudyTypeList[0];


            try
            {
                objCore.BILLING_CYCLE_ID = new Guid(ArrRecord[0]);
                objCore.STUDY_ID = new Guid(ArrRecord[1]);
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[2]);
                objCore.USER_ID = new Guid(ArrRecord[3]);

                #region Populate Study Types
                objSTs = new Core.Invoicing.StudyTypeList[(ArrSTs.Length)];

                for (int i = 0; i < objSTs.Length; i++)
                {
                    objSTs[i] = new Core.Invoicing.StudyTypeList();
                    objSTs[i].SERIAL_NUMBER = i + 1;
                    objSTs[i].ID = new Guid(ArrSTs[intListIndex]);
                    intListIndex = intListIndex + 1;
                }
                #endregion

                bReturn = objCore.SaveStudyTypes(Server.MapPath("~"),objSTs, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[3];
                    arrRet[0] = "true";
                    strReturnMsg = strReturnMsg.Trim();
                    arrayCode = new string[1];
                    arrayCode[0] = strReturnMsg;
                    arrRet[1] = objComm.SetErrorResources(arrayCode, "N", false, string.Empty, string.Empty);
                    arrRet[1] = arrRet[1].Replace("&nbsp;", "");
                }
                else
                {

                    if (strCatchMessage.Trim() != "")
                    {
                        arrRet = new string[2];
                        arrRet[0] = "catch";
                        arrayCode = new string[1];
                        arrayCode[0] = strCatchMessage.Trim();
                        arrRet[1] = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
                        arrRet[1] = arrRet[1].Replace("&nbsp;", "");

                    }
                    else
                    {
                        arrRet = new string[2];
                        arrRet[0] = "false";
                        strReturnMsg = strReturnMsg.Trim();
                        arrayCode = strReturnMsg.Split(objComm.RecordDivider);
                        arrRet[1] = objComm.SetErrorResources(arrayCode, "N", true, objCore.USER_NAME, string.Empty);
                        arrRet[1] = arrRet[1].Replace("&nbsp;", "");
                    }
                }
            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrayCode = new string[1];
                arrayCode[0] = expErr.Message.Trim();
                arrRet[1] = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
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