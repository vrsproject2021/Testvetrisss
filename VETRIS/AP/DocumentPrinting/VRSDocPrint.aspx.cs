using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Microsoft.Reporting.WebForms;
using VETRIS.Core;

namespace VETRIS.AP.DocumentPrinting
{
    public partial class VRSDocPrint : System.Web.UI.Page
    {
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            PrintDOC();
        }
        #endregion

        #region PrintDOC
        private void PrintDOC()
        {
            string strDocID = string.Empty;
            string strCycleID = string.Empty;
            string strRadID = string.Empty;
            string strTransID = string.Empty;
            string strSMID = string.Empty;
            string strUserID = string.Empty;
            string strType = string.Empty;

            strDocID = Request.QueryString["DocID"];

            switch (strDocID)
            {
                case "1":
                    strCycleID = Request.QueryString["CYCLE"];
                    strRadID = Request.QueryString["RADID"];
                    strUserID = Request.QueryString["UID"];
                    PrintRadiologistPayment(strCycleID, strRadID, strUserID);
                    break;
                case "2":
                    strCycleID = Request.QueryString["CYCLE"];
                    strRadID = Request.QueryString["RADID"];
                    strUserID = Request.QueryString["UID"];
                    PrintRadiologistStatement(strCycleID, strRadID, strUserID);
                    break;
                case "3":
                    strCycleID = Request.QueryString["CYCLE"];
                    strTransID = Request.QueryString["TRANSID"];
                    strUserID = Request.QueryString["UID"];
                    PrintTranscriptionistPayment(strCycleID, strTransID, strUserID);
                    break;
                case "4":
                    strCycleID = Request.QueryString["CYCLE"];
                    strRadID = Request.QueryString["TRANSID"];
                    strUserID = Request.QueryString["UID"];
                    PrintTranscriptionistStatement(strCycleID, strRadID, strUserID);
                    break;
            }


        }
        #endregion

        #region PrintRadiologistPayment
        private void PrintRadiologistPayment(string CycleID, string RadiologistID, string UserID)
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
                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/RadiologistPmt";

                DeleteFiles("RadiologistPmt_" + UserID);
                strDocName = "RadiologistPmt_" + UserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "billing_cycle_id";
                objParam[0].Values.Add(CycleID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "radiologist_id";
                objParam[1].Values.Add(RadiologistID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(Server.MapPath("~") + "/AP/DocumentPrinting/Temp"))
                    Directory.CreateDirectory(Server.MapPath("~") + "/AP/DocumentPrinting/Temp");

                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/AP/DocumentPrinting/Temp/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();
                hdnServerDocPath.Value = "AP/DocumentPrinting/Temp/" + strDocName + ".pdf";
                hdnDocPath.Value = Server.MapPath("~") + "/AP/DocumentPrinting/Temp/" + strDocName + ".pdf";


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

        #region PrintRadiologistStatement
        private void PrintRadiologistStatement(string CycleID, string RadiologistID, string UserID)
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
                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/RadiologistPaymentStatement";

                DeleteFiles("RadiologistPaymentStatement_" + UserID);
                strDocName = "RadiologistPaymentStatement_" + UserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "billing_cycle_id";
                objParam[0].Values.Add(CycleID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "radiologist_id";
                objParam[1].Values.Add(RadiologistID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(Server.MapPath("~") + "/AP/DocumentPrinting/Temp"))
                    Directory.CreateDirectory(Server.MapPath("~") + "/AP/DocumentPrinting/Temp");

                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/AP/DocumentPrinting/Temp/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();
                hdnServerDocPath.Value = "AP/DocumentPrinting/Temp/" + strDocName + ".pdf";
                hdnDocPath.Value = Server.MapPath("~") + "/AP/DocumentPrinting/Temp/" + strDocName + ".pdf";


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

        #region PrintTranscriptionistPayment
        private void PrintTranscriptionistPayment(string CycleID, string TranscriptionistID, string UserID)
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
                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/TranscriptionistPmt";

                DeleteFiles("TranscriptionistPmt_" + UserID);
                strDocName = "TranscriptionistPmt_" + UserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "billing_cycle_id";
                objParam[0].Values.Add(CycleID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "transcriptionist_id";
                objParam[1].Values.Add(TranscriptionistID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(Server.MapPath("~") + "/AP/DocumentPrinting/Temp"))
                    Directory.CreateDirectory(Server.MapPath("~") + "/AP/DocumentPrinting/Temp");

                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/AP/DocumentPrinting/Temp/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();
                hdnServerDocPath.Value = "AP/DocumentPrinting/Temp/" + strDocName + ".pdf";
                hdnDocPath.Value = Server.MapPath("~") + "/AP/DocumentPrinting/Temp/" + strDocName + ".pdf";


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

        #region PrintTranscriptionistStatement
        private void PrintTranscriptionistStatement(string CycleID, string TranscriptionistID, string UserID)
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
                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/TranscriptionistPaymentStatement";

                DeleteFiles("TranscriptionistPaymentStatement_" + UserID);
                strDocName = "TranscriptionistPaymentStatement_" + UserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "billing_cycle_id";
                objParam[0].Values.Add(CycleID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "transcriptionist_id";
                objParam[1].Values.Add(TranscriptionistID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(Server.MapPath("~") + "/AP/DocumentPrinting/Temp"))
                    Directory.CreateDirectory(Server.MapPath("~") + "/AP/DocumentPrinting/Temp");

                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/AP/DocumentPrinting/Temp/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();
                hdnServerDocPath.Value = "AP/DocumentPrinting/Temp/" + strDocName + ".pdf";
                hdnDocPath.Value = Server.MapPath("~") + "/AP/DocumentPrinting/Temp/" + strDocName + ".pdf";


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

        #region DeleteFiles
        private void DeleteFiles(string strFilePattern)
        {
            string[] arrFiles = new string[0];

            try
            {
                arrFiles = Directory.GetFiles(Server.MapPath("~") + "\\AP\\DocumentPrinting\\Temp", strFilePattern + "*.pdf");
                foreach (string strFile in arrFiles)
                {
                    if (File.Exists(strFile)) File.Delete(strFile);
                }
            }
            catch (Exception ex)
            {
                ;
            }
        } 
        #endregion
    }
}