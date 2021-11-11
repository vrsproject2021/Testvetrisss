using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using VETRIS.Core;

namespace VETRIS.Invoicing.DocumentPrinting
{
    
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class InvoiceDocPrintWebService : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public void GetReportDocument(string cycleId, string accountId, string userId)
        {
            PrintBillingAccountInvoice(cycleId, accountId, userId);
        }

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
            string lblError = "", hdnErr = "N", hdnServerDocPath=null;
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

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
                strDocName = "BillingAccountInvoice_" + UserID + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss");

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
                hdnServerDocPath = "Invoicing/DocumentPrinting/Temp/" + strDocName + ".pdf";


            }
            catch (Exception expErr)
            {
                hdnErr = "Y";
                lblError = expErr.Message.Trim();
            }
            finally
            {
                strReturnMsg = null; strCatchMessage = null;
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }
            Context.Response.Write(JsonConvert.SerializeObject(new
                {
                    hasError = hdnErr,
                    errorMessage = lblError,
                    path = hdnServerDocPath,
                },
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() })
                );
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
