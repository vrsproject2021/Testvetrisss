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

namespace VETRIS
{
    [AjaxPro.AjaxNamespace("VRSPACSLink")]
    public partial class VRSPACSLink : System.Web.UI.Page
    {
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSPACSLink));
            SetPageValue();
        } 
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            hdnID.Value = Request.QueryString["id"];
            if (Request.QueryString["vw"] != null) hdnView.Value = Request.QueryString["vw"];
            else hdnView.Value = "rpt";
            if (Request.QueryString["fmt"] != null) hdnFmt.Value = Request.QueryString["fmt"];

        }
        #endregion

        //#region FetchReportViewDetails (AjaxPro Method)
        //[AjaxPro.AjaxMethod()]
        //public string[] FetchReportViewDetails(string strId)
        //{
        //    string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
        //    DataSet ds = new DataSet();
        //    Core.Case.CaseStudy objCore = new Core.Case.CaseStudy();
        //    string strSUID = string.Empty;
        //    string strLoginURL = string.Empty;
        //    string strRptURL = string.Empty;
        //    string strResult = string.Empty;
        //    WebClient client = new WebClient();

        
        //    string[] arrRet = new string[0];
        //    string[] err = new string[0];

        //    try
        //    {

        //        objCore.ID = new Guid(strId.Trim());


        //        bReturn = objCore.FetchReportViewDetails(Server.MapPath("~"), ref ds, ref strCatchMessage);

        //        if (bReturn)
        //        {
        //            foreach (DataRow dr in ds.Tables["Details"].Rows)
        //            {
        //                strSUID = Convert.ToString(dr["study_uid"]).Trim();
        //                strLoginURL = Convert.ToString(dr["PACSLOGINURL"]).Trim();
        //                strRptURL = Convert.ToString(dr["PACMAILRPTURL"]).Trim();
        //            }

        //            IgnoreBadCertificates();
        //            byte[] data = client.DownloadData(strLoginURL);
        //            strResult = System.Text.Encoding.Default.GetString(data);

        //            if (strResult.IndexOf("#ERROR:") <= 0)
        //            {
        //                arrRet = new string[2];
        //                strRptURL = strRptURL + strSUID;
        //                arrRet[0] = "true";
        //                arrRet[1] = strRptURL;
        //            }
        //            else
        //            {
        //                err = strResult.Split('#');
        //                arrRet = new string[2];
        //                arrRet[0] = "false";

        //                if (err.Length == 4)
        //                {
        //                    arrRet[1] = err[3].Replace("ERROR: ", "");
        //                }
        //                else if (err.Length == 3)
        //                {
        //                    if (err[2].Split(':')[1].Trim() == "OK")
        //                        arrRet[1] = "Study is already deleted from PACS";
        //                }
        //            }
                    
                   

        //                //arrRet = new string[(ds.Tables["Breed"].Rows.Count * 2) + 3];
        //                //arrRet[0] = "true";
        //                //arrRet[1] = "00000000-0000-0000-0000-000000000000";
        //                //arrRet[2] = "Select One";
        //                //i = 3;

        //                //foreach (DataRow dr in ds.Tables["Breed"].Rows)
        //                //{
        //                //    arrRet[i] = Convert.ToString(dr["id"]);
        //                //    arrRet[i + 1] = Convert.ToString(dr["name"]).Trim();
        //                //    i = i + 2;
        //                //}
                    
                   



        //        }
        //        else
        //        {

        //            arrRet = new string[2];
        //            arrRet[0] = "false";
        //            arrRet[1] = strCatchMessage.Trim();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        arrRet = new string[2];
        //        arrRet[0] = "catch";
        //        arrRet[1] = ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //    }
        //    return arrRet;
        //}
        //#endregion

        #region FetchReportViewDetails (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchReportViewDetails(string strId,string strView,string strFmt)
        {
            string strReturnMessage = string.Empty; 
            string strCatchMessage = string.Empty; 
            bool bReturn = false;
            DataSet ds = new DataSet();
            Core.Case.CaseStudy objCore = new Core.Case.CaseStudy();
            
            string strRptURL = string.Empty;
            string strImgURL = string.Empty;
            string strUserID = string.Empty;
            string strVETUserID = string.Empty;
            string strPatientName = string.Empty;
            string strCustomRpt = string.Empty;
            string strPwd = string.Empty;
            string strErr = string.Empty;
            int intStatusID = 0;
            string strAPIVer = string.Empty;
            string strSessID= string.Empty;
            string strWS8CLTIP= string.Empty;
            string strWS8SRVIP=string.Empty;
            string strWS8SRVUID=string.Empty;
            string strWS8SRVPWD = string.Empty;
            string[] arrRet = new string[0];


            try
            {
                arrRet = new string[2];
                objCore.ID = new Guid(strId.Trim());


                bReturn = objCore.FetchReportViewDetails(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    foreach (DataRow dr in ds.Tables["Details"].Rows)
                    {
                        if (Convert.ToString(dr["pacs_password"]).Trim() != "") strPwd = CoreCommon.DecryptString(Convert.ToString(dr["pacs_password"]).Trim());
                        strRptURL = Convert.ToString(dr["PACSRPTVWRURL"]).Trim();
                        strImgURL = Convert.ToString(dr["PACIMGVWRURL"]).Trim();
                        strUserID = Convert.ToString(dr["pacs_user_id"]).Trim();
                        intStatusID = Convert.ToInt32(dr["status_id"]);
                        strVETUserID = Convert.ToString(dr["user_id"]).Trim();
                        strPatientName = Convert.ToString(dr["patient_name"]).Trim();
                        strCustomRpt = Convert.ToString(dr["custom_report"]).Trim();
                        strFmt = Convert.ToString(dr["rpt_fmt"]).Trim();
                        strAPIVer    = Convert.ToString(dr["APIVER"]).Trim();
                        strWS8CLTIP  = Convert.ToString(dr["WS8CLTIP"]).Trim();
                        strWS8SRVIP  = Convert.ToString(dr["WS8SRVIP"]).Trim();
                        strWS8SRVUID = Convert.ToString(dr["WS8SRVUID"]).Trim();
                        strWS8SRVPWD = Convert.ToString(dr["WS8SRVPWD"]).Trim();
                        if (Convert.ToString(dr["WS8SRVPWD"]).Trim() != "") strWS8SRVPWD = CoreCommon.DecryptString(Convert.ToString(dr["WS8SRVPWD"]).Trim());
                        
                        if (strUserID.Trim() == string.Empty) strErr = "Failed to log into PACS...User for the institution is not configured.<br/>";

                        if (strAPIVer == "7.2") if (strPwd.Trim() == string.Empty) strErr += "Failed to log into PACS...Password for the institution is not configured.<br/>";
                         else if (strAPIVer == "8") if (strWS8SRVPWD.Trim() == string.Empty) strErr += "Failed to log into PACS...Password for the institution is not configured.<br/>";

                        if (strView == "rpt")
                        {
                            if (strRptURL.Trim() == string.Empty) strErr += "Can't reach the PACS Url <br/>";
                        }
                        else if (strView == "img")
                        {
                            if (strImgURL.Trim() == string.Empty) strErr += "Can't reach the PACS Url <br/>";
                        }

                        if (strErr.Trim() != string.Empty)
                        {
                            arrRet[0] = "false";
                            arrRet[1] = strErr;
                        }
                        else
                        {
                            arrRet[0] = "true";
                            if (strView == "rpt")
                            {
                                if (intStatusID == 80)
                                {
                                    //strRptURL = strRptURL.Replace("#V3", strPwd);
                                    if(strCustomRpt=="N")
                                        strRptURL = "CaseList/DocPrint/VRSDocPrint.aspx?DocID=2&ID=" + strId.Trim() + "&PNM=" + strPatientName + "&UID=" + strVETUserID + "&FMT=" + strFmt +"&DIRECT=Y";
                                    else
                                        strRptURL = "CaseList/DocPrint/VRSDocPrint.aspx?DocID=4&ID=" + strId.Trim() + "&PNM=" + strPatientName + "&UID=" + strVETUserID + "&FMT=" + strFmt + "&DIRECT=Y";
                                }
                                else
                                {
                                    if (strCustomRpt == "N")
                                        strRptURL = "CaseList/DocPrint/VRSDocPrint.aspx?DocID=1&ID=" + strId.Trim() + "&PNM=" + strPatientName + "&UID=" + strVETUserID + "&FMT=" + strFmt + "&DIRECT=Y";
                                    else
                                        strRptURL = "CaseList/DocPrint/VRSDocPrint.aspx?DocID=3&ID=" + strId.Trim() + "&PNM=" + strPatientName + "&UID=" + strVETUserID + "&FMT=" + strFmt + "&DIRECT=Y";
                                }
                                arrRet[1] = strRptURL;
                            }
                            else if (strView == "img")
                            {
                                if (strAPIVer == "7.2")
                                {
                                    strImgURL = strImgURL.Replace("#V3", strPwd);
                                    arrRet[1] = strImgURL;
                                }
                                else if (strAPIVer == "8")
                                {

                                    strImgURL = strImgURL.Replace("#V5", strWS8SRVPWD);
                                    if (CreateSession(strWS8CLTIP, strWS8SRVIP, strWS8SRVUID, strWS8SRVPWD, ref strSessID, ref strReturnMessage, ref strCatchMessage))
                                    {
                                        strImgURL = strImgURL.Replace("#V3", strSessID);
                                        arrRet[1] = strImgURL;
                                    }
                                    else
                                    {
                                        if(strReturnMessage.Trim() != string.Empty)
                                        {
                                            arrRet[0]="false";
                                            arrRet[1] = strReturnMessage.Trim();
                                        }
                                        else if(strCatchMessage.Trim() != string.Empty)
                                        {
                                            arrRet[0]="catch";
                                            arrRet[1] = strCatchMessage.Trim();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {

                    arrRet[0] = "false";
                    arrRet[1] = strCatchMessage.Trim();
                }

            }
            catch (Exception ex)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return arrRet;
        }
        #endregion

        #region CreateSession
        private bool CreateSession(string ClientIP,string WSUrl,string UserID,string Password,ref string SessionID, ref string ErrorMessage, ref string CatchMessage)
        {
            bool bReturn=false;
            RadWebClass client= new RadWebClass(); 

            try{
                
                bReturn = client.GetSession(ClientIP, WSUrl, UserID,Password, ref SessionID, ref CatchMessage, ref ErrorMessage);
            }
            catch(Exception ex)
            {
                bReturn=false;
                CatchMessage = ex.Message;
            }
            finally{client = null;}

            return bReturn;
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
    }
}