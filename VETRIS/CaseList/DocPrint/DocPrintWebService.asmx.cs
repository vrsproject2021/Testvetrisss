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

namespace VETRIS.CaseList.DocPrint
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class DocPrintWebService : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public void GetReportDocument(string id, string patientName, string userId, string type, string direct)
        {
            if (type == "1")
                PrintFinalReport(id, patientName, userId, direct);
            else
                PrintCustomFinalReport(id, patientName, userId, direct);
        }


        #region PrintFinalReport
        private void PrintFinalReport(string StudyID, string strPatientName, string UserID, string hdnDirect)
        {
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];
            string lblError = "", hdnErr = "N", hdnServerDocPath=null;

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
                if (hdnDirect == "N")
                    hdnServerDocPath = "CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";
                else
                    hdnServerDocPath = "Temp/" + UserID + "/" + strDocName + ".pdf";


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
            Context.Response.Write(JsonConvert.SerializeObject(new { 
                hasError = hdnErr, 
                errorMessage= lblError,
                path= hdnServerDocPath,
                }, 
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() })
                );

        }
        #endregion

        #region PrintCustomFinalReport
        private void PrintCustomFinalReport(string StudyID, string strPatientName, string UserID, string hdnDirect)
        {
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];
            string lblError = "", hdnErr = "N", hdnServerDocPath=null;

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
                if (hdnDirect == "N")
                    hdnServerDocPath = "CaseList/DocPrint/Temp/" + UserID + "/" + strDocName + ".pdf";
                else
                    hdnServerDocPath = "Temp/" + UserID + "/" + strDocName + ".pdf";


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
    }
}
