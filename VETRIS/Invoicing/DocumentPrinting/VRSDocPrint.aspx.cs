using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Microsoft.Reporting.WebForms;
using VETRIS.Core;

namespace VETRIS.Invoicing.DocumentPrinting
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
            string strInstID = string.Empty;
            string strAcctID = string.Empty;
            string strUserID = string.Empty;
            string strType = string.Empty;

            strDocID = Request.QueryString["DocID"];

            switch (strDocID)
            {
                case "1":
                    strCycleID = Request.QueryString["CYCLE"];
                    strInstID = Request.QueryString["INST"];
                    strUserID = Request.QueryString["UID"];
                    PrintInstitutionInvoice(strCycleID, strInstID, strUserID);
                    break;
                case "2":
                    strCycleID = Request.QueryString["CYCLE"];
                    strAcctID = Request.QueryString["ACCT"];
                    strUserID = Request.QueryString["UID"];
                    PrintBillingAccountInvoice(strCycleID, strAcctID, strUserID);
                    break;
                case "3":
                    strCycleID = Request.QueryString["CYCLE"];
                    strInstID = Request.QueryString["INST"];
                    strUserID = Request.QueryString["UID"];
                    PrintInstitutionAnnexure(strCycleID, strInstID, strUserID);
                    break;
                case "4":
                    strCycleID = Request.QueryString["CYCLE"];
                    strAcctID = Request.QueryString["ACCT"];
                    strUserID = Request.QueryString["UID"];
                    PrintInvoiceStatement(strCycleID, strAcctID, strUserID);
                    break;
            }


        }
        #endregion

        #region PrintInstitutionInvoice
        private void PrintInstitutionInvoice(string CycleID, string InstitutionID, string UserID)
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
                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/InstitutionInvoice";

                DeleteFiles("InstitutionInvoice_" + UserID);
                strDocName = "InstitutionInvoice_" + UserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "billing_cycle_id";
                objParam[0].Values.Add(CycleID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "institution_id";
                objParam[1].Values.Add(InstitutionID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(Server.MapPath("~") + "/Invoicing/DocumentPrinting/Temp"))
                    Directory.CreateDirectory(Server.MapPath("~") + "/Invoicing/DocumentPrinting/Temp");

                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/Invoicing/DocumentPrinting/Temp/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();
                hdnServerDocPath.Value = "Invoicing/DocumentPrinting/Temp/" + strDocName + ".pdf";
                hdnDocPath.Value = Server.MapPath("~") + "/Invoicing/DocumentPrinting/Temp/" + strDocName + ".pdf";


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

        #region PrintBillingAccountInvoice
        private void PrintBillingAccountInvoice(string CycleID, string AccountID, string UserID)
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
                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/BillingAccountInvoice";

                DeleteFiles("BillingAccountInvoice_" + UserID);
                strDocName = "BillingAccountInvoice_" +  UserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "billing_cycle_id";
                objParam[0].Values.Add(CycleID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "billing_account_id";
                objParam[1].Values.Add(AccountID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(Server.MapPath("~") + "/Invoicing/DocumentPrinting/Temp"))
                    Directory.CreateDirectory(Server.MapPath("~") + "/Invoicing/DocumentPrinting/Temp");

                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/Invoicing/DocumentPrinting/Temp/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();
                hdnServerDocPath.Value = "Invoicing/DocumentPrinting/Temp/" + strDocName + ".pdf";
                hdnDocPath.Value = Server.MapPath("~") + "/Invoicing/DocumentPrinting/Temp/" + strDocName + ".pdf";


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

        #region PrintInstitutionAnnexure
        private void PrintInstitutionAnnexure(string CycleID, string InstitutionID, string UserID)
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
                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/InstitutionAnnexure";

                DeleteFiles("InstitutionAnnexure_" + UserID);
                strDocName = "InstitutionAnnexure_" + UserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "billing_cycle_id";
                objParam[0].Values.Add(CycleID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "institution_id";
                objParam[1].Values.Add(InstitutionID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(Server.MapPath("~") + "/Invoicing/DocumentPrinting/Temp"))
                    Directory.CreateDirectory(Server.MapPath("~") + "/Invoicing/DocumentPrinting/Temp");

                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/Invoicing/DocumentPrinting/Temp/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();
                hdnServerDocPath.Value = "Invoicing/DocumentPrinting/Temp/" + strDocName + ".pdf";
                hdnDocPath.Value = Server.MapPath("~") + "/Invoicing/DocumentPrinting/Temp/" + strDocName + ".pdf";


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

        #region PrintInvoiceStatement
        private void PrintInvoiceStatement(string CycleID, string AccountID, string UserID)
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
                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/InvoiceStatement";

                DeleteFiles("InvoiceStatement_" + UserID);
                strDocName = "InvoiceStatement_" + UserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "billing_cycle_id";
                objParam[0].Values.Add(CycleID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "billing_account_id";
                objParam[1].Values.Add(AccountID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");
                if (!Directory.Exists(Server.MapPath("~") + "/Invoicing/DocumentPrinting/Temp"))
                    Directory.CreateDirectory(Server.MapPath("~") + "/Invoicing/DocumentPrinting/Temp");

                System.IO.FileStream objFS = new System.IO.FileStream(Server.MapPath("~") + "/Invoicing/DocumentPrinting/Temp/" + strDocName + ".pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();
                hdnServerDocPath.Value = "Invoicing/DocumentPrinting/Temp/" + strDocName + ".pdf";
                hdnDocPath.Value = Server.MapPath("~") + "/Invoicing/DocumentPrinting/Temp/" + strDocName + ".pdf";


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