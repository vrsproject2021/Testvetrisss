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
using eRADCls;
using AjaxPro;

namespace VETRIS.HouseKeeping
{
    [AjaxPro.AjaxNamespace("VRSMissingStudy")]
    public partial class VRSMissingStudy : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.HouseKeeping.MissingStudy objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSMissingStudy));
            if (!CallBackBrw.CausedCallback)
                SetPageValue();
            SetAttributes();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);

            btnSynch.Attributes.Add("onclick", "javascript:btnSynch_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

            txtFromDt.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgFrom.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtFromDt.ClientID + "','CalFrom');");
            txtTillDt.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgTill.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtTillDt.ClientID + "','CalTill');");
            btnSearch.Attributes.Add("onclick", "javascript:btnSearch_OnClick();");

        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            txtFromDt.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat); CalFrom.SelectedDate = DateTime.Today;
            txtTillDt.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat); CalFrom.SelectedDate = DateTime.Today;
            objComm = null;
            FetchParameters(UserID);
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
            lnkCAL.Attributes["href"] = strServerPath + "/css/" + strTheme + "/CalendarStyle.css";
        }
        #endregion
        #region FetchParameters
        private void FetchParameters(Guid UserID)
        {
            objCore = new Core.HouseKeeping.MissingStudy();
            string strCatchMessage = ""; bool bReturn = false;

            objComm = new classes.CommonClass();

            try
            {
                objCore.USER_ID = UserID;

                bReturn = objCore.FetchParameters(Server.MapPath("~"), ref strCatchMessage);

                if (bReturn)
                {

                    hdnURL.Value = objCore.PACS_IMAGE_COUNT_URL;
                    hdnPACSUserID.Value = objCore.PACS_USER_ID;
                    hdnPACSPwd.Value = objCore.PACS_USER_PASSWORD;
                    hdnStudyCheckURL.Value = objCore.PACS_STUDY_CHECK_URL;
                    hdnWS8SRVIP.Value = objCore.WEB_SERVICE_SERVER_URL;
                    hdnWS8URLMSK.Value = objCore.WEB_SERVICE_SERVER_URL_MASKED;
                    hdnWS8CLTIP.Value = objCore.WEB_SERVICE_CLIENT_URL;
                    hdnWS8SRVUID.Value = objCore.WEB_SERVICE_USER_ID;
                    hdnWS8SRVPWD.Value = objCore.WEB_SERVICE_PASSWORD;
                    hdnAPIVER.Value = objCore.API_VERSION;
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

                objCore = null;
            }

        }
        #endregion


        #region CallBackBrw_Callback
        protected void CallBackBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strVer = e.Parameters[e.Parameters.Length - 1].Trim();
            if (strVer == "7.2") LoadStudies_72(e.Parameters);
            else if (strVer == "8") LoadStudies_80(e.Parameters);
            grdBrw.Width = Unit.Percentage(99);
            grdBrw.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region API 7.2

        #region LoadStudies_72
        private void LoadStudies_72(string[] arrParams)
        {
            WebClient client = new WebClient();
            string strURL = string.Empty; string strPACSUserID = string.Empty; string strPACSPwd = string.Empty;
            string strCatchMessage = ""; bool bReturn = false; string strErr = string.Empty;
            string strResult = string.Empty; string strCount = string.Empty; string strField = string.Empty;
            string strDtFrom = string.Empty; string strDtTill = string.Empty;
            string[] arrData = new string[0];
            string[] arrFields = new string[0];
            string[] arrRecSep = { "\n" };
            string[] arrFldSep = { "\t" };
            string strDt = string.Empty; string strTblDate = string.Empty;
            string[] err = new string[0];
            DataSet ds = new DataSet();

            objCore = new Core.HouseKeeping.MissingStudy();
            Core.HouseKeeping.MissingStudy[] objList = new Core.HouseKeeping.MissingStudy[0];
            objComm = new classes.CommonClass();


            try
            {
                objComm.SetRegionalFormat();
                strURL = arrParams[0].Trim();
                strPACSUserID = arrParams[1].Trim();
                strPACSPwd = arrParams[2].Trim(); if (strPACSPwd != "") strPACSPwd = CoreCommon.DecryptString(strPACSPwd);
                strDtFrom = Convert.ToDateTime(arrParams[3]).ToString("yyyyMMdd") + "_000000";
                strDtTill = Convert.ToDateTime(arrParams[4]).ToString("yyyyMMdd") + "_235959";

                strURL += "SYUI%0ARCVD%0APANM%0AINSN%0ASTAT";
                strURL += "&qf_RCVD=" + strDtFrom + "-" + strDtTill;
                strURL += "&cUser=" + strPACSUserID + "&cPasswd=" + strPACSPwd;
                //strURL = strURL.Replace("https://pacs.vcradiology.com", "https://172.21.247.65");

                IgnoreBadCertificates();
                byte[] data = client.DownloadData(strURL);
                strResult = System.Text.Encoding.Default.GetString(data);
                strResult = strResult.Replace("### Begin_Table's_Content ###", "");
                strResult = strResult.Replace("### End_Table's_Content ###", "");

                if (strResult.IndexOf("#RESULT: OK") >= 0)
                {
                    strErr = "OK";
                    strResult = strResult.Substring(1, strResult.IndexOf("#USERID:") - 1);
                    strResult = strResult.Replace("\r", "");
                    strResult = strResult.Trim();

                    /******************************COMMENT CODE***************************************/
                    //strResult = "1.2.826.0.1.3680043.2.950.212843.5158.20190916121349	20200312_131337	Goldfarb^Dolly^^^	Everglades Animal Hospital	100";
                    /******************************COMMENT CODE***************************************/

                    if (strResult.Trim() != string.Empty)
                    {
                        arrData = strResult.Split(arrRecSep, StringSplitOptions.None);
                        objList = new Core.HouseKeeping.MissingStudy[arrData.Length];

                        #region Capture Data
                        //strErr = "CD";

                        for (int i = 0; i < arrData.Length; i++)
                        {
                            arrFields = arrData[i].Split(arrFldSep, StringSplitOptions.None);
                            objList[i] = new Core.HouseKeeping.MissingStudy();

                            //strErr = "SD";

                            strField = arrFields[0].Trim();
                            if (strField == "___NULL___") strField = "";
                            objList[i].STUDY_UID = strField;

                            strErr = arrFields.Length.ToString();
                            strDt = arrFields[1].Trim();
                            if ((strDt == "___NULL___") || (strDt == "00000000_000000")) strTblDate = "01jan1900";
                            else strTblDate = strDt.Substring(0, 4) + "-" + strDt.Substring(4, 2) + "-" + strDt.Substring(6, 2) + " " + strDt.Substring(9, 2) + ":" + strDt.Substring(11, 2) + ":" + strDt.Substring(13, 2);
                            objList[i].RECEIVED_DATE = Convert.ToDateTime(strTblDate);

                            strField = arrFields[2].Trim();
                            if (strField == "___NULL___") strField = "";
                            strField = strField.Replace("^", " ").Trim();
                            strField = strField.Replace("  ", " ");
                            objList[i].PATIENT_NAME = strField;

                            strField = arrFields[3].Trim();
                            if (strField == "___NULL___") strField = "";
                            strField = strField.Replace("  ", " ").Trim();
                            objList[i].INSTITUTION_NAME = strField.Replace("^", " ").Trim();

                            strField = arrFields[4].Trim();
                            if (strField == "___NULL___") strField = "0";
                            objList[i].STATUS = Convert.ToInt32(strField);
                        }
                        #endregion


                    }

                    if (objList.Length > 0)
                    {
                        bReturn = objCore.CheckMissingStudies(Server.MapPath("~"), objList, ref ds, ref strCatchMessage);

                        if (bReturn)
                        {
                            grdBrw.DataSource = ds.Tables["Studies"];
                            grdBrw.Levels[0].Columns[3].FormatString = objComm.DateFormat + " HH:mm";
                            grdBrw.DataBind();


                            spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";
                        }
                        else
                            spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";
                    }
                    else
                        spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

                }
                else if (strResult.IndexOf("#ERROR:") >= 0)
                {
                    strErr = "ERROR";
                    strResult = strResult.Substring(strResult.IndexOf("#ERROR:") + 1, strResult.Length - (strResult.IndexOf("#ERROR:") + 1));
                    strResult = strResult.Replace("\r", "");
                    strResult = strResult.Trim();
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strResult + "\" />";
                }

            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null; objList = null; objComm = null;
            }
        }
        #endregion

        #region SynchStudy_72 (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SynchStudy_72(string[] ArrRecord)
        {
            bool bReturn = false;
            WebClient client = new WebClient();
            string strSUID = string.Empty; string strURL = string.Empty; string strPACSUserID = string.Empty; string strPACSPwd = string.Empty;
            string strField = string.Empty;
            string[] arrRet = new string[2];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            Guid UserID = Guid.Empty; int intMenuID = 0; string strErr = string.Empty;
            string strResult = string.Empty; string strCount = string.Empty;
            string[] arrData = new string[0];
            string[] arrFields = new string[0];
            string[] arrRecSep = { "\n" };
            string[] arrFldSep = { "\t" };
            string strDt = string.Empty; string strTblDate = string.Empty;
            string[] err = new string[0];
            objComm = new classes.CommonClass();
            objCore = new Core.HouseKeeping.MissingStudy();


            try
            {
                strSUID = ArrRecord[0].Trim().Replace(" ", "");
                strURL = ArrRecord[1].Trim();
                strPACSUserID = ArrRecord[2].Trim();
                strPACSPwd = ArrRecord[3].Trim();
                UserID = new Guid(ArrRecord[4].Trim());
                intMenuID = Convert.ToInt32(ArrRecord[5]);

                if (strPACSPwd != "") strPACSPwd = CoreCommon.DecryptString(strPACSPwd);
                strURL = strURL.Replace("qf_SYUI=", "qf_SYUI=" + strSUID);
                strURL = strURL.Replace("#V3", strPACSPwd);

                arrRet[0] = "false";
                arrRet[1] = strURL;

                IgnoreBadCertificates();
                //strURL = "https://pacs.vcradiology.com/iface/worklist.jsp?cFields=SYDT%0ARCVD%0AACCN%0APAID%0APANM%0APDOB%0APAGE%0APSEX%0A9PWT%0A9SPC%0A9BRD%0A9RSP%0AMODY%0ABDYP%0APMAL%0AINSN%0APHRF%0AMFCT%0AMFMD%0ASTNM%0A9PSN%0ANIMG%0APSAE%0APRST%0ATRAD%0AUDF1%0AUDF8%0ASTAT%0AUDF4%0ADSCR%0AUDF7%0AUDF9%0ASYUI%0ANOBJ&qf_SYUI=1.2.826.0.1.3680043.2.950.212843.5158.20190916121349&cUser=Doylek&cPasswd=walkingdead1";

                /******************************ACTUAL CODE***************************************/
                byte[] data = client.DownloadData(strURL);
                strResult = System.Text.Encoding.Default.GetString(data);

                strResult = strResult.Replace("### Begin_Table's_Content ###", "");
                strResult = strResult.Replace("### End_Table's_Content ###", "");
                /*************************************************************************************/
                /******************************COMMENT CODE***************************************/
                //strResult = "#RESULT: OK";
                /*************************************************************************************/
                arrRet[0] = "false";
                arrRet[1] = strResult;
                if (strResult.IndexOf("#RESULT: OK") >= 0)
                {
                    strErr = "OK";
                    /******************************ACTUAL CODE***************************************/
                    strResult = strResult.Substring(1, strResult.IndexOf("#USERID:") - 1);
                    /*************************************************************************************/

                    /******************************COMMENT CODE***************************************/
                    //strResult = "20191224_024951	20191224_083927	0191224024951-2	___NULL___	LAWS^CODY^JUNIOR^^	20050105_000000	14 y	F	8	Canine	Pomeranian	Laws^Randy^^^	US	___NULL___	Liver Enzymes elevated.	Veterinary Ultrasound Services	Glass^Sharon^^^DVM	Sonoscape	___NULL___	___NULL___	YES	49	___NULL___	20	Waller^Kenneth^^^DVM, MS, DACVR	Alex Shapiro	3.632	100	___NULL___	Abdomen - Complete	___NULL___	___NULL___	1.2.826.0.1.3680043.2.93.2.363610615.3890.1577194765.562646	51";
                    /*************************************************************************************/

                    strResult = strResult.Replace("\r", "");
                    strResult = strResult.Trim();

                    if (strResult.Trim() != string.Empty)
                    {
                        arrData = strResult.Split(arrRecSep, StringSplitOptions.None);

                        #region Capture Data
                        strErr = "CD";
                        for (int i = 0; i < arrData.Length; i++)
                        {
                            arrFields = arrData[i].Split(arrFldSep, StringSplitOptions.None);
                            //arrFields = strResult.Split(arrFldSep, StringSplitOptions.None);
                            //strErr = "SD";

                            strErr = arrFields.Length.ToString();
                            strDt = arrFields[0].Trim();
                            if ((strDt == "___NULL___") || (strDt == "00000000_000000")) strTblDate = "01jan1900";
                            else strTblDate = strDt.Substring(0, 4) + "-" + strDt.Substring(4, 2) + "-" + strDt.Substring(6, 2) + " " + strDt.Substring(9, 2) + ":" + strDt.Substring(11, 2) + ":" + strDt.Substring(13, 2);
                            objCore.STUDY_DATE = Convert.ToDateTime(strTblDate);

                            //strErr = "RD";
                            strDt = arrFields[1].Trim();
                            if ((strDt == "___NULL___") || (strDt == "00000000_000000")) strTblDate = "01jan1900";
                            else strTblDate = strDt.Substring(0, 4) + "-" + strDt.Substring(4, 2) + "-" + strDt.Substring(6, 2) + " " + strDt.Substring(9, 2) + ":" + strDt.Substring(11, 2) + ":" + strDt.Substring(13, 2);
                            objCore.RECEIVED_DATE = Convert.ToDateTime(strTblDate);

                            strField = arrFields[2].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.ACCESSION_NO = strField.Replace("^", " ").Trim();

                            strField = arrFields[3].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.PATIENT_ID = strField.Replace("^", " ").Trim();

                            strField = arrFields[4].Trim();
                            if (strField == "___NULL___") strField = "";
                            strField = strField.Replace("^", " ").Trim();
                            strField = strField.Replace("  ", " ");
                            objCore.PATIENT_NAME = strField;

                            //strErr = "DOB";
                            strDt = arrFields[5].Trim();
                            if ((strDt == "___NULL___") || (strDt == "00000000_000000")) strTblDate = "01jan1900";
                            else strTblDate = strDt.Substring(0, 4) + "-" + strDt.Substring(4, 2) + "-" + strDt.Substring(6, 2);
                            objCore.PATIENT_DOB = Convert.ToDateTime(strTblDate);

                            strField = arrFields[6].Trim();
                            if (strField == "___NULL___") strField = "";
                            //else strField = strField.Replace("Y", "");
                            objCore.PATIENT_AGE = strField;

                            strField = arrFields[7].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.PATIENT_GENDER = strField;

                            strField = arrFields[8].Trim();
                            if (strField == "___NULL___") strField = "0";
                            objCore.PATIENT_WEIGHT_POUNDS = Convert.ToDecimal(strField);

                            strField = arrFields[9].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.SPECIES_NAME = strField.Replace("^", " ").Trim();

                            strField = arrFields[10].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.BREED_NAME = strField.Replace("^", " ").Trim();

                            strField = arrFields[11].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.OWNER_NAME_PACS = strField.Replace("^", " ").Trim();

                            strField = arrFields[12].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.MODALITY = strField.Replace("^", " ").Trim();

                            strField = arrFields[13].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.BODY_PART = strField.Replace("^", " ").Trim();

                            strField = arrFields[14].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.REASON = strField.Replace("^", " ").Trim();

                            strField = arrFields[15].Trim();
                            if (strField == "___NULL___") strField = "";
                            strField = strField.Replace("  ", " ").Trim();
                            objCore.INSTITUTION_NAME = strField.Replace("^", " ").Trim();

                            strField = arrFields[16].Trim();
                            if (strField == "___NULL___") strField = "";
                            strField = strField.Replace("^", " ").Trim();
                            strField = strField.Replace("  ", " ").Trim();
                            objCore.REFERRING_PHYSICIAN = strField.Replace("^", " ").Trim();

                            strField = arrFields[17].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.MANUFACTURER_NAME = strField.Replace("^", " ").Trim();

                            strField = arrFields[18].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.MANUFACTURER_MODEL_NUMBER = strField.Replace("^", " ").Trim();

                            strField = arrFields[19].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.DEVICE_SERIAL_NUMBER = strField.Replace("^", " ").Trim();

                            strField = arrFields[20].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.SPAYED_NEUTERED = strField.Replace("^", " ").Trim();

                            strField = arrFields[21].Trim();
                            if (strField == "___NULL___") strField = "0";
                            objCore.IMAGE_COUNT = Convert.ToInt32(strField);

                            strField = arrFields[22].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.MODALITY_AE_TITLE = strField.Replace("^", " ").Trim();

                            strField = arrFields[23].Trim();
                            if (strField == "___NULL___") strField = "0";
                            objCore.PRIORITY_ID = Convert.ToInt32(strField);

                            strField = arrFields[24].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.RADIOLOGIST_NAME = strField.Replace("^", " ").Trim();

                            strField = arrFields[25].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.SALES_PERSON = strField.Replace("^", " ").Trim();

                            strField = arrFields[26].Trim();
                            if (strField == "___NULL___") strField = "0";
                            objCore.PATIENT_WEIGHT_KGS = Convert.ToDecimal(strField);

                            strField = arrFields[27].Trim();
                            if (strField == "___NULL___") strField = "0";
                            objCore.STATUS = Convert.ToInt32(strField);

                            strField = arrFields[28].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.STUDY_TYPE_NAME_2 = strField.Replace("^", " ").Trim();

                            strField = arrFields[29].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.STUDY_TYPE_NAME_1 = strField.Replace("^", " ").Trim();

                            //strErr = "STN3";
                            strField = arrFields[30].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.STUDY_TYPE_NAME_3 = strField.Replace("^", " ").Trim();

                            strField = arrFields[31].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.STUDY_TYPE_NAME_4 = strField.Replace("^", " ").Trim();

                            strField = arrFields[32].Trim();
                            if (strField == "___NULL___") strField = "";

                            strField = arrFields[33].Trim();
                            if (strField == "___NULL___") strField = "0";
                            else if (Convert.ToInt32(strField) > 0) strField = Convert.ToString(Convert.ToInt32(strField) - 1);
                            else strField = "0";
                            objCore.OBJECT_COUNT = Convert.ToInt32(strField);

                            strField = arrFields[34].Trim();
                            if (strField == "___NULL___") strField = "";
                            objCore.PHYSICIAN_NOTE = strField.Replace("^", " ").Trim();

                            //strField = arrFields[35].Trim();
                            //if (strField == "___NULL___") strField = "";
                            //objCore.PRELIMINARY_RADIOLOGIST = strField.Replace("^", " ").Trim();

                            //strField = arrFields[36].Trim();
                            //if (strField == "___NULL___") strField = "";
                            //objCore.FINAL_RADIOLOGIST = strField.Replace("^", " ").Trim();




                        }
                        #endregion

                        objCore.STUDY_UID = strSUID;
                        objCore.USER_ID = UserID;
                        objCore.MENU_ID = intMenuID;

                        bReturn = objCore.SaveSynchedData(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                        if (bReturn)
                        {
                           
                            arrRet[0] = "true";
                            arrRet[1] = strReturnMsg.Trim();

                        }
                        else
                        {
                            if (strCatchMessage.Trim() != "")
                            {
                                arrRet[0] = "catch";
                                arrRet[1] = strCatchMessage.Trim();

                            }
                            else
                            {
                                arrRet[0] = "false";
                                arrRet[1] = strReturnMsg.Trim();
                            }
                        }
                    }
                    else
                    {
                        arrRet[0] = "false";
                        arrRet[1] = "162";
                    }

                }
                else if (strResult.IndexOf("#ERROR:") >= 0)
                {
                    strErr = "ERROR";
                    strResult = strResult.Substring(strResult.IndexOf("#ERROR:") + 1, strResult.Length - (strResult.IndexOf("#ERROR:") + 1));
                    strResult = strResult.Replace("\r", "");
                    strResult = strResult.Trim();

                    arrRet[0] = "false";
                    arrRet[1] = strResult;
                }

            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrRet[0] = "catch";
                arrRet[1] = strErr + "::" + expErr.Message.Trim();
            }
            finally
            {
                objCore = null; objComm = null;
                strReturnMsg = null; strCatchMessage = null;
            }

            if (arrRet[0].Trim() == "")
            {
                arrRet[0] = "false";
                arrRet[1] = strResult;
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

        #region LoadStudies_80
        private void LoadStudies_80(string[] arrParams)
        {
            bool bReturn = false;
            RadWebClass client = new RadWebClass();
            string strWSURL = string.Empty; string strClientIP = string.Empty; string strWSUserID = string.Empty; string strWSPwd = string.Empty;
            string strCatchMessage = string.Empty; string strErr = string.Empty; string strSession = string.Empty;
            string strResult = string.Empty; string strColID = string.Empty; string strValue = string.Empty;
            DateTime DtFrom = DateTime.Now; DateTime DtTill = DateTime.Now;
            string[] arrFields = new string[0];
            string strDt = string.Empty; string strTblDate = string.Empty;
            string[] err = new string[0];
            DataSet ds = new DataSet();
            int intCount = 0; int intRecID = 0; int idx = 0;

            objComm = new classes.CommonClass();
            objCore = new Core.HouseKeeping.MissingStudy();
            Core.HouseKeeping.MissingStudy[] objList = new Core.HouseKeeping.MissingStudy[0];
            objComm = new classes.CommonClass();


            try
            {
                objComm.SetRegionalFormat();
                strWSURL = arrParams[0].Trim();
                strClientIP = arrParams[1].Trim();
                //strClientIP = "192.168.65.83";
                strWSUserID = arrParams[2].Trim();
                //strWSPwd = CoreCommon.DecryptString(arrParams[3].Trim());
                strWSPwd = CoreCommon.DecryptString(arrParams[3].Trim());
                strSession = arrParams[4].Trim();
                DtFrom = Convert.ToDateTime(Convert.ToDateTime(arrParams[5]).ToString("yyyy" + objComm.DateSeparator + "MM" + objComm.DateSeparator + "dd") + " 00:00:00");
                DtTill = Convert.ToDateTime(Convert.ToDateTime(arrParams[6]).ToString("yyyy" + objComm.DateSeparator + "MM" + objComm.DateSeparator + "dd") + " 23:59:59");

                DtFrom = new DateTime(DtFrom.Year, DtFrom.Month, DtFrom.Day, DtFrom.Hour, DtFrom.Minute, DtFrom.Second);
                DtTill = new DateTime(DtTill.Year, DtTill.Month, DtTill.Day, DtTill.Hour, DtTill.Minute, DtTill.Second);

                //bReturn = client.GetSession(strClientIP, strWSURL, strWSUserID,strWSPwd, ref strSession, ref strCatchMessage, ref strErr);

                //if (bReturn)
                //{
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";
                    arrFields = new string[5];
                    arrFields[0] = "SYUI";
                    arrFields[1] = "RCVD";
                    arrFields[2] = "PANM";
                    arrFields[3] = "INSN";
                    arrFields[4] = "STAT";


                    #region Get Data
                    bReturn = client.GetStudyDateWise(strSession, strWSURL, arrFields, DtFrom, DtTill, ref strResult, ref strCatchMessage, ref strErr);
                    if (bReturn)
                    {
                        spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

                        System.IO.StringReader xmlSR = new System.IO.StringReader(strResult);
                        ds.ReadXml(xmlSR);

                        if (ds.Tables.Count == 5)
                        {
                            intCount = Convert.ToInt32(ds.Tables["Record"].Rows[ds.Tables["Record"].Rows.Count - 1]["Record_Id"]) + 1;
                            objList = new Core.HouseKeeping.MissingStudy[intCount];

                            #region Capture Data
                            for (int i = 0; i < ds.Tables["Field"].Rows.Count; i = i + 5)
                            {
                                intRecID = Convert.ToInt32(ds.Tables["Field"].Rows[i]["Record_Id"]);

                                DataView dv = new DataView(ds.Tables["Field"]);
                                dv.RowFilter = "Record_Id =" + Convert.ToString(intRecID);
                                objList[idx] = new Core.HouseKeeping.MissingStudy();

                                foreach (DataRow drRec in dv.ToTable().Rows)
                                {
                                    strColID = Convert.ToString(drRec["Colid"]).Trim();
                                    strValue = Convert.ToString(drRec["Value"]).Trim();

                                    switch (strColID)
                                    {
                                        case "SYUI":
                                            if (drRec["Value"] != DBNull.Value)
                                                objList[idx].STUDY_UID = Convert.ToString(drRec["Value"]).Trim();
                                            else
                                                objList[idx].STUDY_UID = string.Empty;
                                            break;
                                        case "RCVD":
                                            if (drRec["Value"] != DBNull.Value)
                                            {
                                                if (IsDate(Convert.ToString(drRec["Value"])))
                                                    objList[idx].RECEIVED_DATE = Convert.ToDateTime(drRec["Value"]);
                                                else
                                                    objList[idx].RECEIVED_DATE = DateTime.Now;
                                            }
                                            else if ((drRec["Value"] == DBNull.Value) || (Convert.ToString(drRec["Value"]).Trim() == string.Empty))
                                                objList[idx].RECEIVED_DATE = DateTime.Now;
                                            break;
                                        case "PANM":
                                            if (drRec["Value"] != DBNull.Value)
                                                objList[idx].PATIENT_NAME = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                            else
                                                objList[idx].PATIENT_NAME = string.Empty;
                                            break;
                                        case "INSN":
                                            if (drRec["Value"] != DBNull.Value)
                                                objList[idx].INSTITUTION_NAME = Convert.ToString(drRec["Value"]).Replace("^", " ").Trim();
                                            else
                                                objList[idx].INSTITUTION_NAME = string.Empty;
                                            break;
                                        case "STAT":
                                            if (dv.ToTable().Rows[0]["Value"] != DBNull.Value)
                                            {
                                                if (IsInteger(Convert.ToString(drRec["Value"])))
                                                    objList[idx].STATUS = Convert.ToInt32(drRec["Value"]);
                                                else
                                                    objList[idx].STATUS = 0;
                                            }
                                            else
                                                objList[idx].STATUS = 0;
                                            break;
                                    }
                                }
                                dv.Dispose();
                                idx = idx + 1;
                            }
                            #endregion
                        }

                    }
                    else
                    {
                        if (strCatchMessage.Trim() != "")
                        {
                            spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage.Trim() + "\" />";

                        }
                        else
                        {
                            spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strErr.Trim() + "\" />";

                        }
                    }
                    #endregion

                //}
                //else
                //{
                //    if (strCatchMessage.Trim() != "")
                //    {
                //        spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage.Trim() + "\" />";
                //    }
                //    else
                //    {
                //        spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strErr.Trim() + "\" />";
                //    }
                //}

                #region Check Studies
                if (objList.Length > 0)
                {
                    bReturn = objCore.CheckMissingStudies(Server.MapPath("~"), objList, ref ds, ref strCatchMessage);

                    if (bReturn)
                    {
                        grdBrw.DataSource = ds.Tables["Studies"];
                        grdBrw.DataBind();


                        spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";
                    }
                    else
                        spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";
                }
                else
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

                #endregion

            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null; objList = null; objComm = null;
            }
        }
        #endregion

        #region SynchStudy_80 (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SynchStudy_80(string[] ArrRecord)
        {
            bool bReturn = false;
            RadWebClass client = new RadWebClass();
            string strSUID = string.Empty;
            string strWSURL = string.Empty; string strClientIP = string.Empty; string strWSUserID = string.Empty; string strWSPwd = string.Empty;
            string strColID = string.Empty; string strValue = string.Empty;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            Guid UserID = Guid.Empty; int intMenuID = 0; string strErr = string.Empty;
            string strResult = string.Empty; string strSession = string.Empty;
            string[] arrFields = new string[0];
            string strDt = string.Empty; string strTblDate = string.Empty;
            string[] err = new string[0];
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();
            objCore = new Core.HouseKeeping.MissingStudy();


            try
            {
                strSUID = ArrRecord[0].Trim().Replace(" ", "");
                strWSURL = ArrRecord[1].Trim();
                strClientIP = ArrRecord[2].Trim();
                strWSUserID = ArrRecord[3].Trim();
                strWSPwd = CoreCommon.DecryptString(ArrRecord[4].Trim());
                strSession = ArrRecord[5].Trim();
                UserID = new Guid(ArrRecord[6].Trim());
                intMenuID = Convert.ToInt32(ArrRecord[7]);

                //bReturn = client.GetSession(strClientIP, strWSURL, strWSUserID,strWSPwd, ref strSession, ref strCatchMessage, ref strErr);
                //if (bReturn)
                //{
                    #region Fields
                    arrFields = new string[35];
                    arrFields[0] = "SYDT";
                    arrFields[1] = "RCVD";
                    arrFields[2] = "ACCN";
                    arrFields[3] = "PAID";
                    arrFields[4] = "PANM";
                    arrFields[5] = "PDOB";
                    arrFields[6] = "PAGE";
                    arrFields[7] = "PSEX";
                    arrFields[8] = "9PWT";
                    arrFields[9] = "9SPC";
                    arrFields[10] = "9BRD";
                    arrFields[11] = "9RSP";
                    arrFields[12] = "MODY";
                    arrFields[13] = "BDYP";
                    arrFields[14] = "PMAL";
                    arrFields[15] = "INSN";
                    arrFields[16] = "PHRF";
                    arrFields[17] = "MFCT";
                    arrFields[18] = "MFMD";
                    arrFields[19] = "STNM";
                    arrFields[20] = "9PSN";
                    arrFields[21] = "NIMG";
                    arrFields[22] = "PSAE";
                    arrFields[23] = "PRST";
                    arrFields[24] = "NOBJ";
                    arrFields[25] = "STAT";
                    arrFields[26] = "DSCR";
                    arrFields[27] = "UDF4";
                    arrFields[28] = "UDF7";
                    arrFields[29] = "UDF9";
                    arrFields[30] = "9C11";
                    arrFields[31] = "9PSN";
                    arrFields[32] = "UDF1";
                    arrFields[33] = "UDF8";
                    arrFields[34] = "TRAD";
                    #endregion

                    bReturn = client.GetStudyData(strSession, strWSURL, strSUID, ref strResult, ref strCatchMessage, ref strErr);

                    if (bReturn)
                    {
                        System.IO.StringReader xmlSR = new System.IO.StringReader(strResult);
                        ds.ReadXml(xmlSR);

                        if (ds.Tables.Contains("Field"))
                        {
                            foreach (DataRow dr in ds.Tables["Field"].Rows)
                            {
                                strColID = Convert.ToString(dr["Colid"]).Trim();
                                strValue = Convert.ToString(dr["Value"]).Trim();

                                #region Manipulate Data
                                switch (strColID)
                                {

                                    case "SYDT":
                                        if (dr["Value"] != DBNull.Value)
                                        {
                                            if (IsDate(Convert.ToString(dr["Value"])))
                                                objCore.STUDY_DATE = Convert.ToDateTime(dr["Value"]);
                                            else
                                                objCore.STUDY_DATE = Convert.ToDateTime("01jan1900");
                                        }
                                        else if ((dr["Value"] == DBNull.Value) || (Convert.ToString(dr["Value"]).Trim() == string.Empty))
                                            objCore.STUDY_DATE = Convert.ToDateTime("01jan1900");
                                        break;
                                    case "RCVD":
                                        if (dr["Value"] != DBNull.Value)
                                        {
                                            if (IsDate(Convert.ToString(dr["Value"])))
                                                objCore.RECEIVED_DATE = Convert.ToDateTime(dr["Value"]);
                                            else
                                                objCore.RECEIVED_DATE = DateTime.Now;
                                        }
                                        else if ((dr["Value"] == DBNull.Value) || (Convert.ToString(dr["Value"]).Trim() == string.Empty))
                                            objCore.RECEIVED_DATE = DateTime.Now;
                                        break;
                                    case "ACCN":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.ACCESSION_NO = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.ACCESSION_NO = string.Empty;
                                        break;
                                    case "REAS":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.REASON = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.REASON = string.Empty;
                                        break;
                                    case "INSN":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.INSTITUTION_NAME = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.INSTITUTION_NAME = string.Empty;
                                        break;
                                    case "MFCT":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.MANUFACTURER_NAME = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.MANUFACTURER_NAME = string.Empty;
                                        break;
                                    case "STNM":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.DEVICE_SERIAL_NUMBER = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.DEVICE_SERIAL_NUMBER = string.Empty;
                                        break;
                                    case "PHRF":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.REFERRING_PHYSICIAN = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.REFERRING_PHYSICIAN = string.Empty;
                                        break;
                                    case "PAID":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.PATIENT_ID = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.PATIENT_ID = string.Empty;
                                        break;
                                    case "PANM":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.PATIENT_NAME = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.PATIENT_NAME = string.Empty;
                                        break;
                                    case "PSEX":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.PATIENT_GENDER = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.PATIENT_GENDER = string.Empty;
                                        break;
                                    case "PDOB":
                                        if (dr["Value"] != DBNull.Value)
                                        {
                                            strDt = Convert.ToString(dr["Value"]).Trim();
                                            if (strDt == "00000000_000000") strTblDate = "01jan1900";
                                            else if (strDt == "") strTblDate = "01jan1900";
                                            else strTblDate = strDt.Substring(0, 4) + "-" + strDt.Substring(4, 2) + "-" + strDt.Substring(6, 2);
                                            if (IsDate(strTblDate)) objCore.PATIENT_DOB = Convert.ToDateTime(strTblDate); else objCore.PATIENT_DOB = Convert.ToDateTime("01Jan1900");
                                        }
                                        else
                                            objCore.PATIENT_DOB = Convert.ToDateTime("01jan1900");
                                        break;
                                    case "PAGE":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.PATIENT_AGE = Convert.ToString(dr["Value"]).Trim();
                                        else
                                            objCore.PATIENT_AGE = "0";
                                        break;
                                    case "9PWT":
                                        if (dr["Value"] != DBNull.Value)
                                        {
                                            if (IsDecimal(Convert.ToString(dr["Value"])))
                                                objCore.PATIENT_WEIGHT_POUNDS = Convert.ToDecimal(dr["Value"]);
                                            else
                                                objCore.PATIENT_WEIGHT_POUNDS = 0;
                                        }
                                        else if ((dr["Value"] == DBNull.Value) || (Convert.ToString(dr["Value"]).Trim() == string.Empty))
                                            objCore.PATIENT_WEIGHT_POUNDS = 0;
                                        break;
                                    case "9RSP":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.OWNER_NAME_PACS = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.OWNER_NAME_PACS = string.Empty;
                                        break;
                                    case "9SPC":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.SPECIES_NAME = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.SPECIES_NAME = string.Empty;
                                        break;
                                    case "9BRD":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.BREED_NAME = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.BREED_NAME = string.Empty;
                                        break;
                                    case "MODY":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.MODALITY = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.MODALITY = string.Empty;
                                        break;
                                    case "BDYP":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.BODY_PART = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.BODY_PART = string.Empty;
                                        break;
                                    case "MFMD":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.MANUFACTURER_MODEL_NUMBER = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.MANUFACTURER_MODEL_NUMBER = string.Empty;
                                        break;
                                    case "9PSN":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.SPAYED_NEUTERED = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.SPAYED_NEUTERED = string.Empty;
                                        break;
                                    case "NIMG":
                                        if (dr["Value"] != DBNull.Value)
                                        {
                                            if (IsInteger(Convert.ToString(dr["Value"])))
                                                objCore.IMAGE_COUNT = Convert.ToInt32(dr["Value"]);
                                            else
                                                objCore.IMAGE_COUNT = 0;
                                        }
                                        else
                                            objCore.IMAGE_COUNT = 0;
                                        break;
                                    case "PSAE":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.MODALITY_AE_TITLE = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.MODALITY_AE_TITLE = string.Empty;
                                        break;
                                    case "PRST":
                                        if (dr["Value"] != DBNull.Value)
                                        {
                                            if (IsInteger(Convert.ToString(dr["Value"])))
                                                objCore.PRIORITY_ID = Convert.ToInt32(dr["Value"]);
                                            else
                                                objCore.PRIORITY_ID = 0;

                                        }
                                        else if ((dr["Value"] == DBNull.Value) || (Convert.ToString(dr["Value"]).Trim() == string.Empty))
                                            objCore.PRIORITY_ID = 0;
                                        break;
                                    case "TRAD":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.RADIOLOGIST_NAME = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.RADIOLOGIST_NAME = string.Empty;
                                        break;
                                    case "STAT":
                                        if (dr["Value"] != DBNull.Value)
                                        {
                                            if (IsInteger(Convert.ToString(dr["Value"])))
                                                objCore.STATUS = Convert.ToInt32(dr["Value"]);
                                            else
                                                objCore.STATUS = 0;
                                        }
                                        else
                                            objCore.STATUS = 0;
                                        break;
                                    case "DSCR":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.STUDY_TYPE_NAME_1 = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.STUDY_TYPE_NAME_1 = string.Empty;
                                        break;
                                    case "UDF4":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.STUDY_TYPE_NAME_2 = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.STUDY_TYPE_NAME_2 = string.Empty;
                                        break;
                                    case "UDF7":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.STUDY_TYPE_NAME_3 = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.STUDY_TYPE_NAME_3 = string.Empty;
                                        break;
                                    case "UDF9":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.STUDY_TYPE_NAME_4 = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.STUDY_TYPE_NAME_4 = string.Empty;
                                        break;
                                    case "UDF1":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.SALES_PERSON = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.SALES_PERSON = string.Empty;
                                        break;
                                    case "UDF8":
                                        if (dr["Value"] != DBNull.Value)
                                        {
                                            if (IsDecimal(Convert.ToString(dr["Value"])))
                                                objCore.PATIENT_WEIGHT_KGS = Convert.ToDecimal(dr["Value"]);
                                            else
                                                objCore.PATIENT_WEIGHT_KGS = 0;
                                        }
                                        else if ((dr["Value"] == DBNull.Value) || (Convert.ToString(dr["Value"]).Trim() == string.Empty))
                                            objCore.PATIENT_WEIGHT_KGS = 0;
                                        break;
                                    case "NOBJ":
                                        if (dr["Value"] != DBNull.Value)
                                        {
                                            if (Convert.ToInt32(dr["Value"]) > 0)
                                            {
                                                if (IsInteger(Convert.ToString(dr["Value"])))
                                                    objCore.OBJECT_COUNT = Convert.ToInt32(dr["Value"]) - 1;
                                                else
                                                    objCore.OBJECT_COUNT = 0;
                                            }
                                            else if ((dr["Value"] == DBNull.Value) || (Convert.ToString(dr["Value"]).Trim() == string.Empty))
                                            {
                                                if (IsInteger(Convert.ToString(dr["Value"])))
                                                    objCore.OBJECT_COUNT = Convert.ToInt32(dr["Value"]);
                                                else
                                                    objCore.OBJECT_COUNT = 0;
                                            }
                                        }
                                        else
                                            objCore.OBJECT_COUNT = 0;
                                        break;
                                    case "9C11":
                                        if (dr["Value"] != DBNull.Value)
                                            objCore.PHYSICIAN_NOTE = Convert.ToString(dr["Value"]).Replace("^", " ").Trim();
                                        else
                                            objCore.PHYSICIAN_NOTE = string.Empty;
                                        break;

                                }
                                #endregion
                            }

                            objCore.STUDY_UID = strSUID;
                            objCore.USER_ID = UserID;
                            objCore.MENU_ID = intMenuID;

                            bReturn = objCore.SaveSynchedData(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

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
                                    arrRet = new string[2];
                                    arrRet[0] = "false";
                                    arrRet[1] = strReturnMsg.Trim();
                                }
                            }
                        }
                        else
                        {
                            arrRet = new string[2];
                            arrRet[0] = "false";
                            arrRet[1] = "162";
                        }

                    }
                    else
                    {
                        arrRet = new string[2];
                        arrRet[0] = "catch";
                        if (strCatchMessage.Trim() != "")
                        {
                            arrRet[1] = strCatchMessage.Trim();
                        }
                        else
                        {
                            arrRet[1] = strErr.Trim();

                        }
                    }
                //}

            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = strErr + "::" + expErr.Message.Trim();
            }
            finally
            {
                objCore = null; objComm = null; ds.Dispose();
                strReturnMsg = null; strCatchMessage = null;
            }
            return arrRet;
        }
        #endregion

        #region IsDate
        protected bool IsDate(String date)
        {
            DateTime Temp;
            if (DateTime.TryParse(date, out Temp) == true)
                return true;
            else
                return false;
        }
        #endregion

        #region IsDecimal
        protected bool IsDecimal(String decimalValue)
        {
            Decimal Temp;
            if (Decimal.TryParse(decimalValue, out Temp) == true)
                return true;
            else
                return false;
        }
        #endregion

        #region IsInteger
        protected bool IsInteger(String integerValue)
        {
            Decimal Temp;
            if (Decimal.TryParse(integerValue, out Temp) == true)
                return true;
            else
                return false;
        }
        #endregion

        #endregion

    }
}