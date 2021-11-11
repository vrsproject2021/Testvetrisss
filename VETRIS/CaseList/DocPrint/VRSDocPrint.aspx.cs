using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Drawing;
using System.Windows;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Text.RegularExpressions;
//using ESCommon;
//using ESCommon.Rtf;
using VETRIS.Core;

namespace VETRIS.CaseList.DocPrint
{
    public partial class VRSDocPrint : System.Web.UI.Page
    {
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            hdnReqURL.Value = Request.Url.ToString();
            PrintDOC();
        }
        #endregion

        #region PrintDOC
        private void PrintDOC()
        {
            string strDocID = string.Empty;
            string strStudyID = string.Empty;
            string strUserID = string.Empty;
            string strPatientName = string.Empty;


            strDocID = Request.QueryString["DocID"];

            switch (strDocID)
            {
                case "1":
                    strStudyID = Request.QueryString["ID"];
                    strPatientName = Request.QueryString["PNM"];
                    strUserID = Request.QueryString["UID"];

                    if (Request.QueryString["DIRECT"] != null) hdnDirect.Value = Request.QueryString["DIRECT"];
                    if (Request.QueryString["FMT"] != null) hdnFmt.Value = Request.QueryString["FMT"]; else hdnFmt.Value = "P";

                    if(hdnFmt.Value=="R")
                        GenerateFinalReportRTF(strStudyID, strPatientName, strUserID);
                    else if (hdnFmt.Value == "P")
                        PrintFinalReport(strStudyID, strPatientName, strUserID);

                    break;
                case "2":
                    strStudyID = Request.QueryString["ID"];
                    strPatientName = Request.QueryString["PNM"];
                    strUserID = Request.QueryString["UID"];

                    if (Request.QueryString["DIRECT"] != null) hdnDirect.Value = Request.QueryString["DIRECT"];
                    if (Request.QueryString["FMT"] != null) hdnFmt.Value = Request.QueryString["FMT"]; else hdnFmt.Value = "P";

                    if (hdnFmt.Value == "R")
                        GeneratePreliminaryReportRTF(strStudyID, strPatientName, strUserID);
                    else if (hdnFmt.Value == "P")
                        PrintPreliminaryReport(strStudyID, strPatientName, strUserID);
                    break;
                case "3":
                    strStudyID = Request.QueryString["ID"];
                    strPatientName = Request.QueryString["PNM"];
                    strUserID = Request.QueryString["UID"];

                    if (Request.QueryString["DIRECT"] != null) hdnDirect.Value = Request.QueryString["DIRECT"];
                    if (Request.QueryString["FMT"] != null) hdnFmt.Value = Request.QueryString["FMT"]; else hdnFmt.Value = "P";

                    if (hdnFmt.Value == "R")
                        GenerateFinalReportRTF(strStudyID, strPatientName, strUserID);
                    else if (hdnFmt.Value == "P")
                        PrintCustomFinalReport(strStudyID, strPatientName, strUserID);
                    break;
                case "4":
                    strStudyID = Request.QueryString["ID"];
                    strPatientName = Request.QueryString["PNM"];
                    strUserID = Request.QueryString["UID"];

                    if (Request.QueryString["DIRECT"] != null) hdnDirect.Value = Request.QueryString["DIRECT"];
                    if (Request.QueryString["FMT"] != null) hdnFmt.Value = Request.QueryString["FMT"]; else hdnFmt.Value = "P";

                    if (hdnFmt.Value == "R")
                        GeneratePreliminaryReportRTF(strStudyID, strPatientName, strUserID);
                    else if (hdnFmt.Value == "P")
                         PrintCustomPreliminaryReport(strStudyID, strPatientName, strUserID);
                    break;
                case "5":
                    strStudyID = Request.QueryString["ID"];
                    strPatientName = Request.QueryString["PNM"];
                    strUserID = Request.QueryString["UID"];
                    if (Request.QueryString["DIRECT"] != null) hdnDirect.Value = Request.QueryString["DIRECT"];
                    PrintTranscriptionistReport(strStudyID, strPatientName, strUserID);
                    break;
                case "6":
                    strStudyID = Request.QueryString["ID"];
                    strPatientName = Request.QueryString["PNM"];
                    strUserID = Request.QueryString["UID"];
                    if (Request.QueryString["DIRECT"] != null) hdnDirect.Value = Request.QueryString["DIRECT"];
                    PrintCustomTranscriptionistReport(strStudyID, strPatientName, strUserID);
                    break;
            }


        }
        #endregion

        #region PrintPreliminaryReport
        private void PrintPreliminaryReport(string StudyID, string strPatientName, string UserID)
        {
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];
            lblError.Text = ""; hdnErr.Value = "N";

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(Server.MapPath("~"));
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);


                objDoc.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportServerURL"]);

                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/PrelimRpt";
                strDocName = "PRELIMININARY_REPORT_" + strPatientName.ToUpper().Trim().Replace(" ", "_") + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "id";
                objParam[0].Values.Add(StudyID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "user_id";
                objParam[1].Values.Add(UserID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID))
                    Directory.CreateDirectory(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID);

                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();
                if (hdnDirect.Value == "N")
                    hdnServerDocPath.Value = "CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";
                else
                    hdnServerDocPath.Value = "Temp/" + UserID + "/" + strDocName + ".pdf";
                hdnDocPath.Value = Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";


            }
            catch (Exception expErr)
            {
                hdnErr.Value = "Y";
                lblError.Text = expErr.Message.Trim();
            }
            finally
            {
                strReturnMsg = null; strCatchMessage = null;
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }
        }
        #endregion

        #region PrintCustomPreliminaryReport
        private void PrintCustomPreliminaryReport(string StudyID, string strPatientName, string UserID)
        {
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];
            lblError.Text = ""; hdnErr.Value = "N";

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(Server.MapPath("~"));
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);


                objDoc.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportServerURL"]);

                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/CustomPrelimRpt";
                strDocName = "PRELIMININARY_REPORT_" + strPatientName.ToUpper().Trim().Replace(" ", "_") + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "id";
                objParam[0].Values.Add(StudyID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "user_id";
                objParam[1].Values.Add(UserID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID))
                    Directory.CreateDirectory(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID);

                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();
                if (hdnDirect.Value == "N")
                    hdnServerDocPath.Value = "CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";
                else
                    hdnServerDocPath.Value = "Temp/" + UserID + "/" + strDocName + ".pdf";
                hdnDocPath.Value = Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";


            }
            catch (Exception expErr)
            {
                hdnErr.Value = "Y";
                lblError.Text = expErr.Message.Trim();
            }
            finally
            {
                strReturnMsg = null; strCatchMessage = null;
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }
        }
        #endregion

        #region GeneratePreliminaryReportRTF
        private void GeneratePreliminaryReportRTF(string StudyID, string PatientName, string UserID)
        {
            DataSet ds = new DataSet();
            StringBuilder sbAddn = new StringBuilder();
            classes.CommonClass objComm = new classes.CommonClass();
            SautinSoft.HtmlToRtf objHTML2RTF = new SautinSoft.HtmlToRtf();
            string strRandom = CoreCommon.RandomString(6);
            string strHTML = string.Empty;
            string strCatchMessage = string.Empty;
            string strHTMLFilePath = string.Empty;
            string strDocName = string.Empty;


            try
            {
                objHTML2RTF.Serial = "80021972924";
                if (FetchPreliminaryReport(StudyID, UserID, ref ds, ref strCatchMessage))
                {
                    strDocName = PatientName.Replace(" ", "_") + "_" + strRandom + "_PRELIMINARY_REPORT";
                    objComm.SetRegionalFormat();
                    TextReader tr = new StreamReader(Server.MapPath("~") + "/CaseList/DocPrint/HTMLs/PrelimReport.html");
                    strHTML = tr.ReadToEnd();
                    tr.Dispose();

                    #region HTML String

                    #region Report Title
                    foreach (DataRow dr in ds.Tables["Company"].Rows)
                    {
                        strHTML = strHTML.Replace("#COMPANY#", Convert.ToString(dr["company_name"]).Trim());
                        strHTML = strHTML.Replace("#ADDRESS#", Convert.ToString(dr["company_address"]).Trim());
                    }
                    #endregion

                    foreach (DataRow dr in ds.Tables["Details"].Rows)
                    {
                        #region Header
                        strHTML = strHTML.Replace("#DOS#", objComm.IMDateFormat(dr["study_date"], objComm.DateFormat) + Convert.ToDateTime(dr["study_date"]).ToString("HH:mm") + " (CST)");
                        strHTML = strHTML.Replace("#DOR#", objComm.IMDateFormat(dr["received_date"], objComm.DateFormat) + Convert.ToDateTime(dr["received_date"]).ToString("HH:mm") + " (CST)");
                        strHTML = strHTML.Replace("#PNAME#", Convert.ToString(dr["patient_name"]).Trim());
                        strHTML = strHTML.Replace("#MODALITY#", Convert.ToString(dr["modality_name"]).Trim());
                        strHTML = strHTML.Replace("#PID#", Convert.ToString(dr["patient_id"]).Trim());
                        strHTML = strHTML.Replace("#INSTITUTION#", Convert.ToString(dr["institution_name"]).Trim());
                        strHTML = strHTML.Replace("#AGE#", Convert.ToString(dr["patient_age_accepted"]).Trim());
                        strHTML = strHTML.Replace("#REFPHYS#", Convert.ToString(dr["physician_name"]).Trim());
                        strHTML = strHTML.Replace("#SEX#", Convert.ToString(dr["patient_sex"]).Trim());
                        strHTML = strHTML.Replace("#IMG#", Convert.ToString(dr["image_count"]).Trim());
                        strHTML = strHTML.Replace("#SPECIES#", Convert.ToString(dr["species_name"]).Trim());
                        strHTML = strHTML.Replace("#ACCNNO#", Convert.ToString(dr["accession_no"]).Trim());
                        strHTML = strHTML.Replace("#BREED#", Convert.ToString(dr["breed_name"]).Trim());
                        strHTML = strHTML.Replace("#OWNER#", Convert.ToString(dr["owner_name"]).Trim());
                        strHTML = strHTML.Replace("#SPAYED#", Convert.ToString(dr["patient_spayed_neutered"]).Trim());
                        strHTML = strHTML.Replace("#WEIGHT#", Convert.ToString(dr["patient_weight"]).Trim());
                        strHTML = strHTML.Replace("#UOM#", Convert.ToString(dr["wt_uom"]).Trim());
                        #endregion

                        #region Study Types
                        strHTML = strHTML.Replace("#SYTYPE#", Convert.ToString(dr["study_types"]).Trim());
                        #endregion

                        #region History
                        strHTML = strHTML.Replace("#HIST#", Convert.ToString(dr["reason_accepted"]).Trim());
                        #endregion

                        #region Observation
                        strHTML = strHTML.Replace("#OBVS#", Convert.ToString(dr["report_text_html"]).Trim());
                        #endregion

                        #region Disclaimer
                        if (Convert.ToString(dr["disclaimer_reason"]).Trim() != string.Empty)
                        {
                            strHTML = strHTML.Replace("#DISCL#", Convert.ToString(dr["disclaimer_reason"]).Trim());
                        }
                        else
                        {
                            strHTML = strHTML.Replace("<tr><td style=\"text-align: left;font-weight: bold; font-size: 14px;\">DISCLAIMER</td></tr><tr><td style=\"text-align: left; font-size: 14px;\">#DISCL#</td></tr><tr><td style=\"text-align: left;\">&nbsp;</td></tr>", string.Empty);
                        }
                        #endregion
                    }

              
                    #endregion

                    if (!Directory.Exists(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID)) Directory.CreateDirectory(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID);
                    hdnServerDocPath.Value = "Temp/" + UserID + "/" +strDocName + ".rtf";
                    hdnDocPath.Value = Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".rtf";
                    strHTMLFilePath = Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName +".html";

                    using (StreamWriter outputFile = new StreamWriter(strHTMLFilePath))
                    {
                        outputFile.Write(strHTML);
                    }

                    if (objHTML2RTF.OpenHtml(strHTMLFilePath))
                    {
                        bool ok = objHTML2RTF.ToRtf(hdnDocPath.Value);
                        if (ok) File.Delete(strHTMLFilePath);

                        //// Open the result for demonstration purposes.
                        //if (ok)
                        //    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(hdnDocPath.Value) { UseShellExecute = true });
                    }


                }
                else
                {
                    hdnErr.Value = "Y";
                    lblError.Text = strCatchMessage;
                }

            }
            catch (Exception expErr)
            {
                hdnErr.Value = "Y";
                lblError.Text = expErr.Message.Trim();
            }
            finally
            {
                ds.Dispose();
                objComm = null;
                objHTML2RTF = null;
            }
        }
        #endregion

        #region PrintFinalReport
        private void PrintFinalReport(string StudyID, string strPatientName, string UserID)
        {
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];
            lblError.Text = ""; hdnErr.Value = "N";

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(Server.MapPath("~"));
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);


                objDoc.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportServerURL"]);

                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/FinalRpt";
                strDocName = "FINAL_REPORT_" + strPatientName.ToUpper().Trim().Replace(" ", "_") + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "id";
                objParam[0].Values.Add(StudyID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "user_id";
                objParam[1].Values.Add(UserID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");

                if (!Directory.Exists(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID))
                    Directory.CreateDirectory(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID);

                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);

                objFS.Write(btData, 0, btData.Length);
                objFS.Close();
                if (hdnDirect.Value == "N")
                    hdnServerDocPath.Value = "CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";
                else
                    hdnServerDocPath.Value = "Temp/" + UserID + "/" + strDocName + ".pdf";
                    //hdnServerDocPath.Value = ConfigurationManager.AppSettings["ServerPath"] + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";

                hdnDocPath.Value = Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";



            }
            catch (Exception expErr)
            {
                hdnErr.Value = "Y";
                lblError.Text = expErr.Message.Trim();
            }
            finally
            {
                strReturnMsg = null; strCatchMessage = null;
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }
        }
        #endregion

        #region PrintCustomFinalReport
        private void PrintCustomFinalReport(string StudyID, string strPatientName, string UserID)
        {
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];
            lblError.Text = ""; hdnErr.Value = "N";

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(Server.MapPath("~"));
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);


                objDoc.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportServerURL"]);


                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/CustomFinalRpt";

                strDocName = "FINAL_REPORT_" + strPatientName.ToUpper().Trim().Replace(" ", "_") + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "id";
                objParam[0].Values.Add(StudyID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "user_id";
                objParam[1].Values.Add(UserID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID))
                    Directory.CreateDirectory(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID);

                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();
                if (hdnDirect.Value == "N")
                    hdnServerDocPath.Value = "CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";
                else
                    hdnServerDocPath.Value = "Temp/" + UserID + "/" + strDocName + ".pdf";
                hdnDocPath.Value = Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";


            }
            catch (Exception expErr)
            {
                hdnErr.Value = "Y";
                lblError.Text = expErr.Message.Trim();
            }
            finally
            {
                strReturnMsg = null; strCatchMessage = null;
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }
        }
        #endregion

        #region GenerateFinalReportRTF
        private void GenerateFinalReportRTF(string StudyID, string PatientName, string UserID)
        {
            DataSet ds = new DataSet();
            StringBuilder sbAddn = new StringBuilder();
            classes.CommonClass objComm = new classes.CommonClass();
            SautinSoft.HtmlToRtf objHTML2RTF = new SautinSoft.HtmlToRtf();
            string strRandom = CoreCommon.RandomString(6);
            string strHTML = string.Empty;
            string strCatchMessage = string.Empty;
            string strHTMLFilePath = string.Empty;
            string strDocName = string.Empty;


            try
            {
                objHTML2RTF.Serial = "80021972924";
                if (FetchFinalReport(StudyID, UserID, ref ds, ref strCatchMessage))
                {
                    strDocName = PatientName.Replace(" ", "_") + "_" + strRandom + "_FINAL_REPORT";
                    objComm.SetRegionalFormat();
                    TextReader tr = new StreamReader(Server.MapPath("~") + "/CaseList/DocPrint/HTMLs/FinalReport.html");
                    strHTML = tr.ReadToEnd();
                    tr.Dispose();
                    #region HTML String

                    #region Report Title
                    foreach (DataRow dr in ds.Tables["Company"].Rows)
                    {
                        strHTML = strHTML.Replace("#COMPANY#", Convert.ToString(dr["company_name"]).Trim());
                        strHTML = strHTML.Replace("#ADDRESS#", Convert.ToString(dr["company_address"]).Trim());
                    }
                    #endregion

                    foreach (DataRow dr in ds.Tables["Details"].Rows)
                    {
                        #region Header
                        strHTML = strHTML.Replace("#DOS#", objComm.IMDateFormat(dr["study_date"], objComm.DateFormat) + Convert.ToDateTime(dr["study_date"]).ToString("HH:mm") + " (CST)");
                        strHTML = strHTML.Replace("#DOR#", objComm.IMDateFormat(dr["received_date"], objComm.DateFormat) + Convert.ToDateTime(dr["received_date"]).ToString("HH:mm") + " (CST)");
                        strHTML = strHTML.Replace("#PNAME#", Convert.ToString(dr["patient_name"]).Trim());
                        strHTML = strHTML.Replace("#MODALITY#", Convert.ToString(dr["modality_name"]).Trim());
                        strHTML = strHTML.Replace("#PID#", Convert.ToString(dr["patient_id"]).Trim());
                        strHTML = strHTML.Replace("#INSTITUTION#", Convert.ToString(dr["institution_name"]).Trim());
                        strHTML = strHTML.Replace("#AGE#", Convert.ToString(dr["patient_age_accepted"]).Trim());
                        strHTML = strHTML.Replace("#REFPHYS#", Convert.ToString(dr["physician_name"]).Trim());
                        strHTML = strHTML.Replace("#SEX#", Convert.ToString(dr["patient_sex"]).Trim());
                        strHTML = strHTML.Replace("#IMG#", Convert.ToString(dr["image_count"]).Trim());
                        strHTML = strHTML.Replace("#SPECIES#", Convert.ToString(dr["species_name"]).Trim());
                        strHTML = strHTML.Replace("#ACCNNO#", Convert.ToString(dr["accession_no"]).Trim());
                        strHTML = strHTML.Replace("#BREED#", Convert.ToString(dr["breed_name"]).Trim());
                        strHTML = strHTML.Replace("#OWNER#", Convert.ToString(dr["owner_name"]).Trim());
                        strHTML = strHTML.Replace("#SPAYED#", Convert.ToString(dr["patient_spayed_neutered"]).Trim());
                        strHTML = strHTML.Replace("#WEIGHT#", Convert.ToString(dr["patient_weight"]).Trim());
                        strHTML = strHTML.Replace("#UOM#", Convert.ToString(dr["wt_uom"]).Trim());
                        #endregion

                        #region Study Types
                        strHTML = strHTML.Replace("#SYTYPE#", Convert.ToString(dr["study_types"]).Trim());
                        #endregion

                        #region History
                        strHTML = strHTML.Replace("#HIST#", Convert.ToString(dr["reason_accepted"]).Trim());
                        #endregion

                        #region Observation
                        strHTML = strHTML.Replace("#OBVS#", Convert.ToString(dr["report_text_html"]).Trim());
                        #endregion

                        #region Disclaimer
                        if (Convert.ToString(dr["disclaimer_reason"]).Trim() != string.Empty)
                        {
                            strHTML = strHTML.Replace("#DISCL#", Convert.ToString(dr["disclaimer_reason"]).Trim());
                        }
                        else
                        {
                            strHTML = strHTML.Replace("<tr><td style=\"text-align: left;font-weight: bold; font-size: 14px;\">DISCLAIMER</td></tr><tr><td style=\"text-align: left; font-size: 14px;\">#DISCL#</td></tr><tr><td style=\"text-align: left;\">&nbsp;</td></tr>", string.Empty);
                        }
                        #endregion
                    }

                    #region Addendums
                    if (ds.Tables["Addendum"].Rows.Count > 0)
                    {
                        sbAddn.Append("<table style=\"width: 100%; border-collapse: collapse; border-spacing: 2px;\"> ");
                        sbAddn.Append("<tr>");
                        sbAddn.Append("<td style=\"width: 15%; text-align: left; font-weight: bold; font-size: 14px;\">Appoved On/At</td>");
                        sbAddn.Append("<td style=\"width: 15%; text-align: left; font-size: 14px;\">Approved By</td>");
                        sbAddn.Append("<td style=\"width: 70%; text-align: left; font-weight: bold; font-size: 14px;\">Addendum Details</td>");
                        sbAddn.Append("</tr>");

                        foreach (DataRow dr in ds.Tables["Addendum"].Rows)
                        {
                            sbAddn.Append("<tr>");
                            sbAddn.Append("<td style=\"width: 15%; text-align: left; font-weight: bold; font-size: 14px;\">" + objComm.IMDateFormat(dr["date_approved"], objComm.DateFormat) + " " + Convert.ToDateTime(dr["date_approved"]).ToString("HH:mm") + "</td>");
                            sbAddn.Append("<td style=\"width: 15%; text-align: left; font-size: 14px;\">" + Convert.ToString(dr["approved_by"]).Trim() + "</td>");
                            sbAddn.Append("<td style=\"width: 70%; text-align: left; font-weight: bold; font-size: 14px;\">" + Convert.ToString(dr["addendum_text"]).Trim() + "</td>");
                            sbAddn.Append("</tr>");
                        }

                        sbAddn.Append("</table>");
                        strHTML = strHTML.Replace("#ADDN#", sbAddn.ToString());
                    }
                    else
                    {
                        strHTML = strHTML.Replace("<tr><td style=\"text-align: left;font-weight: bold; font-size: 14px;\">ADDENDUM(S)</td></tr><tr><td style=\"text-align: left; font-size: 14px;\">#ADDN#</td></tr>", string.Empty);
                    }
                    #endregion

                    #region Footer
                    foreach (DataRow dr in ds.Tables["Signage"].Rows)
                    {
                        strHTML = strHTML.Replace("#SIGN#", Convert.ToString(dr["signage"]).Trim());
                    }
                    #endregion

                    #endregion

                    if (!Directory.Exists(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID)) Directory.CreateDirectory(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID);
                    //hdnServerDocPath.Value = "CaseList/DocPrint/Temp/" + UserID + "/" + PatientName.Replace(" ", "_") + "_" + strRandom + "_FINAL_REPORT.rtf";
                    hdnServerDocPath.Value = "Temp/" + UserID + "/" + strDocName+ ".rtf";

                    hdnDocPath.Value = Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName  + ".rtf";
                    strHTMLFilePath = Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".html";

                    using (StreamWriter outputFile = new StreamWriter(strHTMLFilePath))
                    {
                        outputFile.Write(strHTML);
                    }

                    if (objHTML2RTF.OpenHtml(strHTMLFilePath))
                    {
                        bool ok = objHTML2RTF.ToRtf(hdnDocPath.Value);
                        if (ok) File.Delete(strHTMLFilePath);

                        //// Open the result for demonstration purposes.
                        //if (ok)
                        //    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(hdnDocPath.Value) { UseShellExecute = true });
                    }

                    
                }
                else
                {
                    hdnErr.Value = "Y";
                    lblError.Text = strCatchMessage;
                }

            }
            catch (Exception expErr)
            {
                hdnErr.Value = "Y";
                lblError.Text = expErr.Message.Trim();
            }
            finally
            {
                ds.Dispose();
                objComm = null;
                objHTML2RTF = null;
            }
        }
        #endregion

        #region PrintTranscriptionistReport
        private void PrintTranscriptionistReport(string StudyID, string strPatientName, string UserID)
        {
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];
            lblError.Text = ""; hdnErr.Value = "N";

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(Server.MapPath("~"));
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);


                objDoc.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportServerURL"]);

                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/TransRpt";

                strDocName = "TRANSCRIPTIONIST_REPORT_" + strPatientName.ToUpper().Trim().Replace(" ", "_") + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "id";
                objParam[0].Values.Add(StudyID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "user_id";
                objParam[1].Values.Add(UserID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID))
                    Directory.CreateDirectory(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID);

                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();
                if (hdnDirect.Value == "N")
                    hdnServerDocPath.Value = "CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";
                else
                    hdnServerDocPath.Value = "Temp/" + UserID + "/" + strDocName + ".pdf";
                hdnDocPath.Value = Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";


            }
            catch (Exception expErr)
            {
                hdnErr.Value = "Y";
                lblError.Text = expErr.Message.Trim();
            }
            finally
            {
                strReturnMsg = null; strCatchMessage = null;
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }
        }
        #endregion

        #region PrintCustomTranscriptionistReport
        private void PrintCustomTranscriptionistReport(string StudyID, string strPatientName, string UserID)
        {
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];
            lblError.Text = ""; hdnErr.Value = "N";

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(Server.MapPath("~"));
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);


                objDoc.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportServerURL"]);
                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/CustomTransRpt";

                strDocName = "TRANSCRIPTIONIST_REPORT_" + strPatientName.ToUpper().Trim().Replace(" ", "_") + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "id";
                objParam[0].Values.Add(StudyID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "id";
                objParam[1].Values.Add(UserID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID))
                    Directory.CreateDirectory(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID);

                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();
                if (hdnDirect.Value == "N")
                    hdnServerDocPath.Value = "CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";
                else
                    hdnServerDocPath.Value = "Temp/" + UserID + "/" + strDocName + ".pdf";
                hdnDocPath.Value = Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";


            }
            catch (Exception expErr)
            {
                hdnErr.Value = "Y";
                lblError.Text = expErr.Message.Trim();
            }
            finally
            {
                strReturnMsg = null; strCatchMessage = null;
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }
        }
        #endregion

        #region RTF

        //#region GenerateFinaRTF
        //private void GenerateFinaRTF(string StudyID, string PatientName, string UserID)
        //{
        //    RtfDocument rtf = new RtfDocument();
        //    RtfWriter rtfWriter = new RtfWriter();
        //    RtfDocumentContentBase[] rtfDocBase = new RtfDocumentContentBase[0];
        //    StringBuilder sb = new StringBuilder();
        //    string strRandom = CoreCommon.RandomString(6);
        //    DataSet ds = new DataSet();
        //    string strCatchMessage = string.Empty;
        //    classes.CommonClass objComm = new classes.CommonClass();
        //    string strRptText = string.Empty;
        //    int intRowIndex = 0;
        //    int intDocBaseLength = 0;
        //    int intDocBaseIdx = 0;

        //    try
        //    {
        //        if (FetchFinalReport(StudyID, UserID, ref ds, ref strCatchMessage))
        //        {
        //            objComm.SetRegionalFormat();

        //            intDocBaseLength = 6;
        //            if (ds.Tables["Details"].Rows.Count > 0)
        //            {
        //                if (Convert.ToString(ds.Tables["Details"].Rows[0]["disclaimer_reason"]).Trim() != string.Empty)
        //                    intDocBaseLength = intDocBaseLength + 1;
        //            }

        //            if (ds.Tables["Addendum"].Rows.Count > 0) intDocBaseLength = intDocBaseLength + 1;
        //            rtfDocBase = new RtfDocumentContentBase[intDocBaseLength];

        //            #region Generate RTF
        //            rtf.FontTable.Add(new RtfFont("Calibri"));
        //            rtf.ColorTable.AddRange(new RtfColor[] {
        //                new RtfColor(System.Drawing.Color.Red),
        //                new RtfColor(0, 0, 255)
        //               });

        //            RtfParagraphFormatting LeftAligned10 = new RtfParagraphFormatting(10, RtfTextAlign.Left);
        //            RtfParagraphFormatting LeftAligned14 = new RtfParagraphFormatting(14, RtfTextAlign.Left);

        //            #region Report Title
        //            RtfTable tTitle = new RtfTable(RtfTableAlign.Center, 1, 2);
        //            tTitle.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tTitle[0, 0].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned14, RtfTableCellVerticalAlign.Center);
        //            tTitle[0, 0].AppendText(new RtfFormattedText("Final Report", RtfCharacterFormatting.Bold));
        //            foreach (DataRow dr in ds.Tables["Company"].Rows)
        //            {
        //                tTitle[0, 1].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tTitle[0, 1].AppendText(new RtfFormattedText(Convert.ToString(dr["company_address"]).Trim(), RtfCharacterFormatting.Bold));
        //            }
        //            rtfDocBase[intDocBaseIdx] = tTitle;
        //            intDocBaseIdx = intDocBaseIdx + 1;
        //            #endregion

        //            #region Header
        //            RtfTable tHdr = new RtfTable(RtfTableAlign.Center, 4, 10);

        //            tHdr.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tHdr[0, 1].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tHdr[0, 1].AppendText(new RtfFormattedText("Date/Time Of Service", RtfCharacterFormatting.Bold));
        //            tHdr.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tHdr[2, 1].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tHdr[2, 1].AppendText(new RtfFormattedText("Receive Date/Time", RtfCharacterFormatting.Bold));

        //            tHdr.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tHdr[0, 2].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tHdr[0, 2].AppendText(new RtfFormattedText("Patient Name", RtfCharacterFormatting.Bold));
        //            tHdr.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tHdr[2, 2].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tHdr[2, 2].AppendText(new RtfFormattedText("Modality", RtfCharacterFormatting.Bold));

        //            tHdr.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tHdr[0, 3].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tHdr[0, 3].AppendText(new RtfFormattedText("Patient ID", RtfCharacterFormatting.Bold));
        //            tHdr.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tHdr[2, 3].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tHdr[2, 3].AppendText(new RtfFormattedText("Institution", RtfCharacterFormatting.Bold));

        //            tHdr.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tHdr[0, 4].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tHdr[0, 4].AppendText(new RtfFormattedText("Age", RtfCharacterFormatting.Bold));
        //            tHdr.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tHdr[2, 4].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tHdr[2, 4].AppendText(new RtfFormattedText("Referring Veterinarian", RtfCharacterFormatting.Bold));

        //            tHdr.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tHdr[0, 5].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tHdr[0, 5].AppendText(new RtfFormattedText("Sex", RtfCharacterFormatting.Bold));
        //            tHdr.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tHdr[2, 5].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tHdr[2, 5].AppendText(new RtfFormattedText("Image Count", RtfCharacterFormatting.Bold));

        //            tHdr.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tHdr[0, 6].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tHdr[0, 6].AppendText(new RtfFormattedText("Species", RtfCharacterFormatting.Bold));
        //            tHdr.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tHdr[2, 6].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tHdr[2, 6].AppendText(new RtfFormattedText("Accession No.", RtfCharacterFormatting.Bold));

        //            tHdr.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tHdr[0, 7].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tHdr[0, 7].AppendText(new RtfFormattedText("Breed", RtfCharacterFormatting.Bold));
        //            tHdr.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tHdr[2, 7].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tHdr[2, 7].AppendText(new RtfFormattedText("Owner", RtfCharacterFormatting.Bold));

        //            tHdr.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tHdr[0, 8].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tHdr[0, 8].AppendText(new RtfFormattedText("Spayed/Neutered", RtfCharacterFormatting.Bold));

        //            tHdr.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tHdr[0, 9].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tHdr[0, 9].AppendText(new RtfFormattedText("Weight", RtfCharacterFormatting.Bold));

        //            rtfDocBase[intDocBaseIdx] = tHdr;
        //            intDocBaseIdx = intDocBaseIdx + 1;
        //            #endregion

        //            #region Study Types
        //            RtfTable tST = new RtfTable(RtfTableAlign.Center, 1, 3);
        //            tST.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tST[0, 1].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tST[0, 1].AppendText(new RtfFormattedText("STUDY TYPE(S)", RtfCharacterFormatting.Bold));

        //            rtfDocBase[intDocBaseIdx] = tST;
        //            intDocBaseIdx = intDocBaseIdx + 1;
        //            #endregion

        //            #region History
        //            RtfTable tHist = new RtfTable(RtfTableAlign.Center, 1, 3);
        //            tHist.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tHist[0, 1].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tHist[0, 1].AppendText(new RtfFormattedText("HISTORY / REASON FOR STUDY", RtfCharacterFormatting.Bold));

        //            rtfDocBase[intDocBaseIdx] = tHist;
        //            intDocBaseIdx = intDocBaseIdx + 1;
        //            #endregion

        //            #region Observations
        //            RtfTable tObvs = new RtfTable(RtfTableAlign.Center, 1, 3);
        //            tObvs.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //            tObvs[0, 1].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tObvs[0, 1].AppendText(new RtfFormattedText("OBSERVATIONS :", RtfCharacterFormatting.Bold));

        //            rtfDocBase[intDocBaseIdx] = tObvs;
        //            intDocBaseIdx = intDocBaseIdx + 1;
        //            #endregion

        //            #region Signature
        //            RtfTable tSign = new RtfTable(RtfTableAlign.Center, 2, 7);
        //            tSign.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);

        //            tSign[0, 1].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tSign[0, 1].AppendText(new RtfFormattedText("Electronically Signed By", RtfCharacterFormatting.Bold));

        //            tSign[0, 2].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tSign[0, 2].AppendText(new RtfFormattedText("Approval Date/Time (CST)", RtfCharacterFormatting.Bold));

        //            tSign[0, 3].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tSign[0, 3].AppendText(new RtfFormattedText("Patient Name", RtfCharacterFormatting.Bold));

        //            tSign[0, 4].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tSign[0, 4].AppendText(new RtfFormattedText("Patient ID ", RtfCharacterFormatting.Bold));

        //            tSign[0, 5].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //            tSign[0, 5].AppendText(new RtfFormattedText("Site Code", RtfCharacterFormatting.Bold));

        //            tSign.MergeCellsHorizontally(0, 6, 2);
        //            #endregion

        //            #region Populate data
        //            foreach (DataRow dr in ds.Tables["Details"].Rows)
        //            {

        //                #region Header
        //                tHdr[1, 1].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tHdr[1, 1].AppendText(new RtfFormattedText(objComm.IMDateFormat(dr["study_date"], objComm.DateFormat) + Convert.ToDateTime(dr["study_date"]).ToString("HH:mm") + " (CST)"));
        //                tHdr[3, 1].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tHdr[3, 1].AppendText(new RtfFormattedText(objComm.IMDateFormat(dr["received_date"], objComm.DateFormat) + Convert.ToDateTime(dr["received_date"]).ToString("HH:mm") + " (CST)"));

        //                tHdr[1, 2].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tHdr[1, 2].AppendText(new RtfFormattedText(Convert.ToString(dr["patient_name"]).Trim()));
        //                tHdr[3, 2].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tHdr[3, 2].AppendText(new RtfFormattedText(Convert.ToString(dr["modality_name"]).Trim()));

        //                tHdr[1, 3].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tHdr[1, 3].AppendText(new RtfFormattedText(Convert.ToString(dr["patient_id"]).Trim()));
        //                tHdr[3, 3].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tHdr[3, 3].AppendText(new RtfFormattedText(Convert.ToString(dr["institution_name"]).Trim()));

        //                tHdr[1, 4].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tHdr[1, 4].AppendText(new RtfFormattedText(Convert.ToString(dr["patient_age_accepted"]).Trim()));
        //                tHdr[3, 4].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tHdr[3, 4].AppendText(new RtfFormattedText(Convert.ToString(dr["physician_name"]).Trim()));

        //                tHdr[1, 5].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tHdr[1, 5].AppendText(new RtfFormattedText(Convert.ToString(dr["patient_sex"]).Trim()));
        //                tHdr[3, 5].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tHdr[3, 5].AppendText(new RtfFormattedText(Convert.ToString(dr["image_count"]).Trim()));

        //                tHdr[1, 6].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tHdr[1, 6].AppendText(new RtfFormattedText(Convert.ToString(dr["species_name"]).Trim()));
        //                tHdr[3, 6].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tHdr[3, 6].AppendText(new RtfFormattedText(Convert.ToString(dr["accession_no"]).Trim()));

        //                tHdr[1, 7].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tHdr[1, 7].AppendText(new RtfFormattedText(Convert.ToString(dr["breed_name"]).Trim()));
        //                tHdr[3, 7].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tHdr[3, 7].AppendText(new RtfFormattedText(Convert.ToString(dr["owner_name"]).Trim()));

        //                tHdr[1, 8].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tHdr[1, 8].AppendText(new RtfFormattedText(Convert.ToString(dr["patient_spayed_neutered"]).Trim()));

        //                tHdr[1, 9].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tHdr[1, 9].AppendText(new RtfFormattedText(Convert.ToString(dr["patient_weight"]).Trim() + " " + Convert.ToString(dr["wt_uom"]).Trim()));


        //                #endregion

        //                #region Study Types
        //                tST[0, 2].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tST[0, 2].AppendText(new RtfFormattedText(Convert.ToString(dr["study_types"]).Trim()));
        //                #endregion

        //                #region History
        //                tHist[0, 2].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tHist[0, 2].AppendText(new RtfFormattedText(Convert.ToString(dr["reason_accepted"]).Trim()));
        //                #endregion

        //                #region Observation
        //                tObvs[0, 2].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                strRptText = HtmlToPlainText(Convert.ToString(dr["report_text_html"]).Trim());
        //                //strRptText = ConvertToRtf(strRptText);
        //                strRptText = HtmlToPlainText(Convert.ToString(dr["report_text"]).Trim());
        //                tObvs[0, 2].AppendText(new RtfFormattedText(strRptText));
        //                #endregion

        //                if (Convert.ToString(dr["disclaimer_reason"]).Trim() != string.Empty)
        //                {
        //                    #region Disclaimer
        //                    RtfTable tDisc = new RtfTable(RtfTableAlign.Center, 1, 3);
        //                    tDisc.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);
        //                    tDisc[0, 1].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                    tDisc[0, 1].AppendText(new RtfFormattedText("DISCLAIMER", RtfCharacterFormatting.Bold));

        //                    tDisc[0, 2].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                    tDisc[0, 2].AppendText(new RtfFormattedText(Convert.ToString(dr["disclaimer_reason"]).Trim()));

        //                    rtfDocBase[intDocBaseIdx] = tDisc;
        //                    intDocBaseIdx = intDocBaseIdx + 1;
        //                    #endregion
        //                }
        //            }

        //            #region Addendums
        //            if (ds.Tables["Addendum"].Rows.Count > 0)
        //            {
        //                RtfTable tAddn = new RtfTable(RtfTableAlign.Center, 3, ds.Tables["Addendum"].Rows.Count);
        //                tAddn.DefaultCellStyle = new RtfTableCellStyle(RtfBorderSetting.None);

        //                tAddn[0, 0].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tAddn[0, 0].AppendText(new RtfFormattedText("Appoved On/At", RtfCharacterFormatting.Bold));

        //                tAddn[1, 0].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tAddn[1, 0].AppendText(new RtfFormattedText("Approved By", RtfCharacterFormatting.Bold));

        //                tAddn[2, 0].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tAddn[2, 0].AppendText(new RtfFormattedText("Addendum details", RtfCharacterFormatting.Bold));

        //                intRowIndex = intRowIndex + 1;

        //                foreach (DataRow dr in ds.Tables["Addendum"].Rows)
        //                {
        //                    tAddn[0, intRowIndex].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                    tAddn[0, intRowIndex].AppendText(new RtfFormattedText(objComm.IMDateFormat(dr["date_approved"], objComm.DateFormat) + " " + Convert.ToDateTime(dr["date_approved"]).ToString("HH:mm")));

        //                    tAddn[1, intRowIndex].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                    tAddn[1, intRowIndex].AppendText(new RtfFormattedText(Convert.ToString(dr["approved_by"]).Trim()));

        //                    tAddn[2, intRowIndex].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                    tAddn[2, intRowIndex].AppendText(new RtfFormattedText(Convert.ToString(dr["addendum_text"]).Trim()));

        //                    intRowIndex = intRowIndex + 1;
        //                }

        //                rtfDocBase[intDocBaseIdx] = tAddn;
        //                intDocBaseIdx = intDocBaseIdx + 1;
        //            }

        //            #endregion

        //            #region Footer
        //            foreach (DataRow dr in ds.Tables["Footer"].Rows)
        //            {
        //                tSign[1, 1].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tSign[1, 1].AppendText(new RtfFormattedText(Convert.ToString(dr["radiologist_name"]).Trim()));

        //                tSign[1, 2].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tSign[1, 2].AppendText(new RtfFormattedText(objComm.IMDateFormat(dr["rpt_approve_date"], objComm.DateFormat)));

        //                tSign[1, 3].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tSign[1, 3].AppendText(new RtfFormattedText(Convert.ToString(dr["patient_name"]).Trim()));

        //                tSign[1, 4].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tSign[1, 4].AppendText(new RtfFormattedText(Convert.ToString(dr["patient_id"]).Trim()));

        //                tSign[1, 5].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tSign[1, 5].AppendText(new RtfFormattedText(Convert.ToString(dr["site_code"]).Trim()));

        //                tSign[1, 6].Definition.Style = new RtfTableCellStyle(RtfBorderSetting.None, LeftAligned10, RtfTableCellVerticalAlign.Center);
        //                tSign[1, 6].AppendText(new RtfFormattedText(Convert.ToString(dr["signage"]).Trim()));

        //                rtfDocBase[intDocBaseIdx] = tSign;
        //            }
        //            #endregion

        //            #endregion


        //            //rtf.Contents.AddRange(new RtfDocumentContentBase[] {
        //            //        tTitle,
        //            //        tHdr,
        //            //        tST,
        //            //        tHist,
        //            //        tObvs,
        //            //        tSign
        //            //       });

        //            rtf.Contents.AddRange(rtfDocBase);

        //            if (!Directory.Exists(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID))
        //                Directory.CreateDirectory(Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID);

        //            hdnServerDocPath.Value = "Temp/" + UserID + "/" + PatientName.Replace(" ", "_") + "_" + strRandom + "_FINAL_REPORT.rtf";
        //            hdnDocPath.Value = Server.MapPath("~") + "/CaseList/DocPrint/Temp/" + UserID + "/" + PatientName.Replace(" ", "_") + "_" + strRandom + "_FINAL_REPORT.rtf";

        //            using (TextWriter writer = new StreamWriter(hdnDocPath.Value))
        //            {
        //                rtfWriter.Write(writer, rtf);
        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            hdnErr.Value = "Y";
        //            lblError.Text = strCatchMessage;
        //        }

        //    }
        //    catch (Exception expErr)
        //    {
        //        hdnErr.Value = "Y";
        //        lblError.Text = expErr.Message.Trim();
        //    }
        //    finally
        //    {
        //        rtf = null;
        //        rtfWriter = null;
        //        ds.Dispose();
        //        objComm = null;
        //    }
        //}
        //#endregion

        //#region HtmlToPlainText
        //private string HtmlToPlainText(string html)
        //{
        //    const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
        //    const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
        //    const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
        //    var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
        //    var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
        //    var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);
        //    var text = html;

        //    text = text.Replace("<p class=\"pasted\">", string.Empty);
        //    text = text.Replace("<p>", string.Empty);
        //    text = text.Replace("</p>", "<br/>");
        //    text = text.Replace("<li>", string.Empty);
        //    text = text.Replace("</li>", "<br/>");
        //    text = text.Replace("<span style=\"white-space:pre\"> </span>", " ");
        //    text = text.Replace("<br>", "<br/>");
        //    text = text.Replace("<div>", string.Empty);
        //    text = text.Replace("</div>", "<br/>");

        //    //Decode html specific characters
        //    text = System.Net.WebUtility.HtmlDecode(text);
        //    //Remove tag whitespace/line breaks
        //    text = tagWhiteSpaceRegex.Replace(text, "><");
        //    //Replace <br /> with line breaks
        //    text = lineBreakRegex.Replace(text, Environment.NewLine);
        //    //Strip formatting
        //    text = stripFormattingRegex.Replace(text, string.Empty);

        //    return text;
        //}
        //#endregion

        //#region ConvertToRtf
        //private string ConvertToRtf(string text)
        //{

        //    //first take care of special RTF chars
        //    StringBuilder backslashed = new StringBuilder(text);
        //    backslashed.Replace(@"\", @"\\");
        //    backslashed.Replace(@"{", @"\{");
        //    backslashed.Replace(@"}", @"\}");

        //    //then convert the string char by char
        //    StringBuilder sb = new StringBuilder();
        //    foreach (char character in backslashed.ToString())
        //    {
        //        if (character <= 0x7f)
        //            sb.Append(character);
        //        else
        //            sb.Append("\\u" + Convert.ToUInt32(character) + "?");
        //    }
        //    return sb.ToString();

        //}
        //#endregion

        #endregion

        #region FetchPreliminaryReport
        private bool FetchPreliminaryReport(string StudyID, string UserID, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            Core.Reports.Report objCore = new Core.Reports.Report();

            try
            {
                objCore.ID = new Guid(StudyID);
                objCore.USER_ID = new Guid(UserID);

                bReturn = objCore.FetchPreliminaryReport(Server.MapPath("~"), ref ds, ref CatchMessage);
            }
            catch (Exception expErr)
            {
                bReturn = false;
                CatchMessage = expErr.Message.Trim();
            }
            finally
            {
                objCore = null;
            }
            return bReturn;
        }
        #endregion

        #region FetchFinalReport
        private bool FetchFinalReport(string StudyID, string UserID, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            Core.Reports.Report objCore = new Core.Reports.Report();

            try
            {
                objCore.ID = new Guid(StudyID);
                objCore.USER_ID = new Guid(UserID);

                bReturn = objCore.FetchFinalReport(Server.MapPath("~"), ref ds, ref CatchMessage);
            }
            catch (Exception expErr)
            {
                bReturn = false;
                CatchMessage = expErr.Message.Trim();
            }
            finally
            {
                objCore = null;
            }
            return bReturn;
        }
        #endregion
    }
}