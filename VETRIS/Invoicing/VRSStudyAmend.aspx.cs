using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using System.Net;
using VETRIS.Core;
using eRADCls;
using AjaxPro;

namespace VETRIS.Invoicing
{
    [AjaxPro.AjaxNamespace("VRSStudyAmend")]
    public partial class VRSStudyAmend : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Invoicing.ARStudyAmendment objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSStudyAmend));
            SetAttributes();
            if (!CallBackStudy.CausedCallback) SetPageValue();
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            hdnID.Value = Request.QueryString["id"];
            if (Request.QueryString["bcid"] != null) hdnBCID.Value = Request.QueryString["bcid"];
            if (Request.QueryString["aid"] != null) hdnAID.Value = Request.QueryString["aid"];
            if (Request.QueryString["iid"] != null) hdnIID.Value = Request.QueryString["iid"];
            if (Request.QueryString["cf"] != null) hdnCF.Value = Request.QueryString["cf"];
            FetchParameters(UserID);
            SetCSS(Request.QueryString["th"]);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
            lnkTHEME.Attributes["href"] = strServerPath + "/css/" + strTheme + "/theme.css";
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnSave1.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnSave2.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnOk.Attributes.Add("onclick", "javascript:btnOk_OnClick();");

            ddlBillingCycle.Attributes.Add("onchange", "javascript:ddlBillingCycle_OnChange();");
            ddlInstitution.Attributes.Add("onchange", "javascript:ddlInstitution_OnChange();");
        }
        #endregion

        #region FetchParameters
        private void FetchParameters(Guid UserID)
        {
            objCore = new Core.Invoicing.ARStudyAmendment();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            string strControlCode = string.Empty;
            objComm = new classes.CommonClass();

            try
            {
                objCore.USER_ID = UserID;
                objCore.BILLING_ACCOUNT_ID = new Guid(hdnAID.Value);
                bReturn = objCore.FetchParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    #region Billing Cycles
                    DataRow dr1 = ds.Tables["Cycle"].NewRow();
                    dr1["id"] = "00000000-0000-0000-0000-000000000000";
                    dr1["name"] = "Select One";
                    ds.Tables["Cycle"].Rows.InsertAt(dr1, 0);
                    ds.Tables["Cycle"].AcceptChanges();

                    ddlBillingCycle.DataSource = ds.Tables["Cycle"];
                    ddlBillingCycle.DataValueField = "id";
                    ddlBillingCycle.DataTextField = "name";
                    ddlBillingCycle.DataBind();
                    ddlBillingCycle.SelectedValue = hdnBCID.Value;
                    #endregion

                    #region Institutions
                    DataRow dr2 = ds.Tables["Institution"].NewRow();
                    dr2["id"] = "00000000-0000-0000-0000-000000000000";
                    dr2["name"] = "Select One";
                    ds.Tables["Institution"].Rows.InsertAt(dr2, 0);
                    ds.Tables["Institution"].AcceptChanges();

                    ddlInstitution.DataSource = ds.Tables["Institution"];
                    ddlInstitution.DataValueField = "id";
                    ddlInstitution.DataTextField = "name";
                    ddlInstitution.DataBind();
                    ddlInstitution.SelectedValue = hdnIID.Value;

                    foreach (DataRow dr in ds.Tables["Institution"].Rows)
                    {
                        if (hdnInst.Value.Trim() != string.Empty) hdnInst.Value += objComm.RecordDivider;
                        hdnInst.Value += Convert.ToString(dr["id"]) + objComm.RecordDivider;
                        hdnInst.Value += Convert.ToString(dr["name"]).Trim();
                    }
                    #endregion

                    #region Modality
                    DataRow dr3 = ds.Tables["Modality"].NewRow();
                    dr3["id"] = "0";
                    dr3["name"] = "Select One";
                    ds.Tables["Modality"].Rows.InsertAt(dr3, 0);
                    ds.Tables["Modality"].AcceptChanges();

                    ddlModality.DataSource = ds.Tables["Modality"];
                    ddlModality.DataValueField = "id";
                    ddlModality.DataTextField = "name";
                    ddlModality.DataBind();

                    foreach (DataRow dr in ds.Tables["Modality"].Rows)
                    {
                        if (hdnModality.Value.Trim() != string.Empty) hdnModality.Value += objComm.RecordDivider;
                        hdnModality.Value += Convert.ToString(dr["id"]) + objComm.RecordDivider;
                        hdnModality.Value += Convert.ToString(dr["name"]).Trim();
                    }
                    #endregion

                    #region Priority
                    DataRow dr4 = ds.Tables["Priority"].NewRow();
                    dr4["priority_id"] = "0";
                    dr4["priority_desc"] = "Select One";
                    ds.Tables["Priority"].Rows.InsertAt(dr4, 0);
                    ds.Tables["Priority"].AcceptChanges();

                    foreach (DataRow dr in ds.Tables["Priority"].Rows)
                    {
                        if (hdnPriority.Value.Trim() != string.Empty) hdnPriority.Value += objComm.RecordDivider;
                        hdnPriority.Value += Convert.ToString(dr["priority_id"]) + objComm.RecordDivider;
                        hdnPriority.Value += Convert.ToString(dr["priority_desc"]).Trim();
                    }
                    #endregion

                    #region API Params
                    foreach (DataRow dr in ds.Tables["APIParams"].Rows)
                    {
                        strControlCode = Convert.ToString(dr["control_code"]).Trim();
                        switch (strControlCode)
                        {
                            case "APIVER":
                                hdnAPIVER.Value = Convert.ToString(dr["data_type_string"]).Trim();
                                break;
                            case "WS8SRVIP":
                                hdnWS8SRVIP.Value = Convert.ToString(dr["data_type_string"]).Trim();
                                break;
                            case "WS8CLTIP":
                                hdnWS8CLTIP.Value = Convert.ToString(dr["data_type_string"]).Trim();
                                break;
                            case "WS8SRVUID":
                                hdnWS8SRVUID.Value = Convert.ToString(dr["data_type_string"]).Trim();
                                break;
                            case "WS8SRVPWD":
                                hdnWS8SRVPWD.Value = Convert.ToString(dr["data_type_string"]).Trim();
                                break;
                            case "PACSTUDYDELURL":
                                hdnStudyDelUrl.Value = Convert.ToString(dr["data_type_string"]).Trim();
                                break;
                        }
                    }
                    #endregion

                    #region Category
                    DataRow dr5 = ds.Tables["Category"].NewRow();
                    dr5["id"] = "0";
                    dr5["name"] = "Select One";
                    ds.Tables["Category"].Rows.InsertAt(dr5, 0);
                    ds.Tables["Category"].AcceptChanges();

                    ddlCategory.DataSource = ds.Tables["Category"];
                    ddlCategory.DataValueField = "id";
                    ddlCategory.DataTextField = "name";
                    ddlCategory.DataBind();

                    foreach (DataRow dr in ds.Tables["Category"].Rows)
                    {
                        if (hdnCategory.Value.Trim() != string.Empty) hdnCategory.Value += objComm.RecordDivider;
                        hdnCategory.Value += Convert.ToString(dr["id"]) + objComm.RecordDivider;
                        hdnCategory.Value += Convert.ToString(dr["name"]).Trim();
                    }
                    #endregion

                }
                else
                    hdnError.Value = strCatchMessage.Trim();
            }
            catch (Exception ex)
            {
                hdnError.Value = ex.Message.Trim();
            }
            finally
            {
                ds.Dispose();
                objCore = null;
                objComm = null;
            }

        }
        #endregion

        #region CallBackStudy_Callback
        protected void CallBackStudy_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadDetails(e.Parameters);
            grdStudy.Width = Unit.Percentage(99);
            grdStudy.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadDetails
        private void LoadDetails(string[] arrRecord)
        {
            objCore = new Core.Invoicing.ARStudyAmendment();
            bool bReturn = false; string strReturnMessage = ""; string strCatchMessage = "";
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();


            try
            {
                objComm.SetRegionalFormat();
                objCore.BILLING_CYCLE_ID = new Guid(arrRecord[0]);
                objCore.INSTITUTION_ID = new Guid(arrRecord[1]);
                objCore.PATIENT_NAME = arrRecord[2].Trim();
                objCore.MODALITY_ID = Convert.ToInt32(arrRecord[3]);
                objCore.CATEGORY_ID = Convert.ToInt32(arrRecord[4]);
                objCore.MENU_ID = Convert.ToInt32(arrRecord[5]);
                objCore.USER_ID = new Guid(arrRecord[6]);

                bReturn = objCore.LoadDetails(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    ds.Relations.Add(ds.Tables[0].Columns["id"], ds.Tables[1].Columns["study_id"]);

                    grdStudy.Levels[0].Columns[2].FormatString = objComm.DateFormat + " HH:mm";
                    grdStudy.DataSource = ds;
                    grdStudy.DataBind();
                    grdStudy.PageSize = ds.Tables["Details"].Rows.Count;
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

                }
                else
                {
                    if (strCatchMessage.Trim() != string.Empty)
                        spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";
                    else
                        spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strReturnMessage + "\" />";
                }

            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose(); objComm = null;

            }


        }
        #endregion

        #region DeleteRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] DeleteRecord(string[] ArrRecord)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;

            objCore = new Core.Invoicing.ARStudyAmendment();
            objComm = new classes.CommonClass();
            try
            {
                objCore.BILLING_CYCLE_ID = new Guid(ArrRecord[0].Trim());
                objCore.STUDY_ID = new Guid(ArrRecord[1].Trim());
                objCore.USER_ID = new Guid(ArrRecord[2].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[3]);


                bReturn = objCore.DeleteRecord(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {

                    arrRet = new string[4];
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

        #region DeleteStudyFromPACS

        #region API 7.2

        #region DeleteStudyPACS_72 (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] DeleteStudyPACS_72(string[] ArrRecord, string strURL)
        {
            bool bReturn = false;
            WebClient client = new WebClient();
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strResult = string.Empty;
            string[] err = new string[0];
            objComm = new classes.CommonClass();

            try
            {
                IgnoreBadCertificates();
                byte[] data = client.DownloadData(strURL);
                strResult = System.Text.Encoding.Default.GetString(data);
                strResult = strResult.Replace("### Begin_Table's_Content ###", "");
                strResult = strResult.Replace("### End_Table's_Content ###", "");

                if (strResult.IndexOf("#ERROR:") >= 0)
                {
                    err = strResult.Split('#');
                    arrRet = new string[2];
                    arrRet[0] = "false";

                    if (err.Length == 4)
                    {
                        arrRet[1] = err[3].Replace("ERROR: ", "");
                    }
                    else if (err.Length == 3)
                    {
                        if (err[2].Split(':')[1].Trim() == "OK")
                            arrRet[1] = "Study is already deleted from PACS";
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

        #region IgnoreBadCertificates
        public static void IgnoreBadCertificates()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
        }
        #endregion

        #region AcceptAllCertifications
        private static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        #endregion

        #endregion

        #region WS8
        #region DeleteStudyPACS_80 (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] DeleteStudyPACS_80(string[] ArrRecord, string[] ArrWSParams)
        {
            bool bReturn = false;
            RadWebClass client = new RadWebClass();
            string strResult = string.Empty;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty; string strError = string.Empty;
            Guid UserID = Guid.Empty;
            string strStudyUID = string.Empty;
            string strWSURL = string.Empty;
            string strClientIP = string.Empty;
            string strWSUserID = string.Empty;
            string strWSPwd = string.Empty;
            string strSession = string.Empty;
            objComm = new classes.CommonClass();

            try
            {
                strStudyUID = ArrRecord[1].Trim();

                strWSURL = ArrWSParams[0].Trim();
                strClientIP = ArrWSParams[1].Trim();
                strWSUserID = ArrWSParams[2].Trim();
                strWSPwd = CoreCommon.DecryptString(ArrWSParams[3].Trim());

                bReturn = client.DeleteStudyData(strSession, strWSURL, strStudyUID, ref strCatchMessage, ref strError);

                if (bReturn)
                {
                    arrRet = new string[2];
                    arrRet[0] = "true";
                    arrRet[1] = "312";

                }
                else
                {
                    if (strCatchMessage.Trim() != string.Empty)
                    {
                        arrRet = new string[2];
                        arrRet[0] = "catch";
                        arrRet[1] = strCatchMessage.Trim();
                    }
                    else
                    {
                        arrRet = new string[2];
                        arrRet[0] = "false";
                        arrRet[1] = strError.Trim();
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

        #endregion

       
        #endregion

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord, string[] ArrStudy, string[] ArrRates)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Invoicing.ARStudyAmendment();
            objComm = new classes.CommonClass();

            Core.Invoicing.ARStudyAmendment[] objStudy = new Core.Invoicing.ARStudyAmendment[0];
            Core.Invoicing.ARAmendedRateList[] objRate = new Core.Invoicing.ARAmendedRateList[0];

            try
            {
                objCore.BILLING_CYCLE_ID = new Guid(ArrRecord[0].Trim());
                objCore.USER_ID = new Guid(ArrRecord[1].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[2]);

                objStudy = new Core.Invoicing.ARStudyAmendment[(ArrStudy.Length / 7)];

                #region populate Study details
                for (int i = 0; i < objStudy.Length; i++)
                {
                    objStudy[i] = new Core.Invoicing.ARStudyAmendment();
                    objStudy[i].STUDY_ID = new Guid(ArrStudy[intListIndex]);
                    objStudy[i].PATIENT_NAME = ArrStudy[intListIndex + 1].Trim();
                    objStudy[i].INSTITUTION_ID = new Guid(ArrStudy[intListIndex + 2]);
                    objStudy[i].MODALITY_ID = Convert.ToInt32(ArrStudy[intListIndex + 3].Trim());
                    objStudy[i].PRIORITY_ID = Convert.ToInt32(ArrStudy[intListIndex + 4].Trim());
                    objStudy[i].CATEGORY_ID = Convert.ToInt32(ArrStudy[intListIndex + 5].Trim());
                    objStudy[i].SERVICE_CODES = ArrStudy[intListIndex + 6].Trim();
                    intListIndex = intListIndex + 7;
                }
                #endregion

                intListIndex = 0;

                objRate = new Core.Invoicing.ARAmendedRateList[(ArrRates.Length / 6)];

                #region populate Rate details
                for (int i = 0; i < objRate.Length; i++)
                {
                    objRate[i] = new Core.Invoicing.ARAmendedRateList();
                    objRate[i].INSTITUTION_ID = new Guid(ArrRates[intListIndex]);
                    objRate[i].STUDY_ID = new Guid(ArrRates[intListIndex + 1]);
                    objRate[i].STUDY_UID = ArrRates[intListIndex + 2].Trim();
                    objRate[i].HEAD_ID = Convert.ToInt32(ArrRates[intListIndex + 3].Trim());
                    objRate[i].HEAD_TYPE = ArrRates[intListIndex + 4].Trim();
                    objRate[i].RATE = Convert.ToDouble(ArrRates[intListIndex + 5].Trim());
                    intListIndex = intListIndex + 6;
                }
                #endregion

                bReturn = objCore.SaveRecord(Server.MapPath("~"), objStudy,objRate, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {

                    arrRet = new string[4];
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

        #region ReprocessInvoice (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] ReprocessInvoice(string[] ArrRecord)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;

            objCore = new Core.Invoicing.ARStudyAmendment();
            objComm = new classes.CommonClass();


            try
            {
                objCore.BILLING_CYCLE_ID = new Guid(ArrRecord[0].Trim());
                objCore.BILLING_ACCOUNT_ID = new Guid(ArrRecord[1].Trim());
                objCore.USER_ID = new Guid(ArrRecord[2].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[3]);




                bReturn = objCore.ReprocessInvoice(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {

                    arrRet = new string[4];
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