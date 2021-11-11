using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Data;
using VETRIS.Core;
using CompareFIles;


namespace VETRIS.CaseList
{
    [AjaxPro.AjaxNamespace("VRSCompareDocuments")]
    public partial class VRSCompareDocuments : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.Case.CaseStudy objCore;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSCompareDocuments));
            SetAttributes();
            SetPageValue();
        } 
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            this.Page.Title = ConfigurationManager.AppSettings["ProductHeading"];
            objComm = new classes.CommonClass();
            Guid UserID = new Guid(Request.QueryString["uid"]);
            try
            {
                hdnID.Value = Request.QueryString["id"];
                hdnUserID.Value = UserID.ToString();
                hdnDivider.Value = objComm.RecordDivider.ToString();
                hdnRootDirectory.Value = ConfigurationManager.AppSettings["RootDirectory"];
                hdnServerPath.Value = ConfigurationManager.AppSettings["ServerPath"];
                hdnFolder.Value = Server.MapPath("~") + "/CaseList/DocPrint/CompareTemp/" + UserID.ToString();
                if (!Directory.Exists(Server.MapPath("~") + "/CaseList/DocPrint/CompareTemp/" + UserID))
                    Directory.CreateDirectory(Server.MapPath("~") + "/CaseList/DocPrint/CompareTemp/" + UserID);
                objComm = null;
                SetCSS(Request.QueryString["th"]);
                LoadDetails();
            }
            catch (Exception ex)
            {
                hdnError.Value = "catch" + objComm.RecordDivider + ex.Message.Trim();
            }
            finally { objComm = null; }
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
        }
        #endregion

        #region LoadDetails
        private void LoadDetails()
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; bool bReturn = false; 
            objComm = new classes.CommonClass();

            try
            {
                objCore.ID = new Guid(hdnID.Value);

                bReturn = objCore.FetchReportCompareDetails(Server.MapPath("~"),ref strCatchMessage);

                if (bReturn)
                {
                    if (objCore.TRANSCRIPTIONIST_ID.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        hdnTrans.Value = "Y";
                        lblPrepBy1.Text = objCore.TRANSCRIPTIONIST_NAME + " (Transcriptionist)";
                        lblPrepBy3.Text = objCore.PRELIMINARY_RADIOLOGIST_ASSIGNED + " (Radiologist)";
                        
                    }
                    else
                    {
                        hdnTrans.Value = "N";
                        lblPrepBy1.Text = objCore.PRELIMINARY_RADIOLOGIST_ASSIGNED;
                    }

                    lblPrepBy2.Text = objCore.FINAL_RADIOLOGIST_ASSIGNED;
                    lblPrepBy4.Text = objCore.TRANSCRIPTIONIST_NAME + " (Transcriptionist)";
                    hdnPName.Value = objCore.PATIENT_NAME;
                    hdnCustomRpt.Value = objCore.CUSTOM_REPORT;
                    lblAbRptReason.Text = objCore.ABNORMAL_REPORT_REASON;

                }
                else
                    hdnError.Value = strCatchMessage.Trim();

               
            }
            catch (Exception ex)
            {
                hdnError.Value = "catch" + objComm.RecordDivider + ex.Message.Trim();
            }
            finally
            {
              
                objCore = null;
                objComm = null;
            }

        }
        #endregion

        #region GenerateSourceReport(AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] GenerateSourceReport(string StudyID, string strTranscribed, string strPatientName, string FolderName, string UserID)
        {
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[3];

            string[] arrRet = new string[0];
            

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(Server.MapPath("~"));
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);

                objDoc.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportServerURL"]);
                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/CompareSourceReport";

                strDocName = "SOURCE_REPORT_" + strPatientName.ToUpper().Trim().Replace(" ", "_") + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf";

                objCred[0] = new DataSourceCredentials();
                objCred[0].Name = "dsRpt";
                objCred[0].UserId = strDBUID;
                objCred[0].Password = strDBPwd;

                objDoc.SetDataSourceCredentials(objCred);

                objParam[0] = new ReportParameter();
                objParam[0].Name = "id";
                objParam[0].Values.Add(StudyID);

                objParam[1] = new ReportParameter();
                objParam[1].Name = "transcribed";
                objParam[1].Values.Add(strTranscribed);

                objParam[2] = new ReportParameter();
                objParam[2].Name = "user_id";
                objParam[2].Values.Add(UserID);

                objDoc.SetParameters(objParam);
                objDoc.Refresh();
                btData = objDoc.Render("PDF");


                System.IO.FileStream objFS = new System.IO.FileStream(FolderName + "/" + strDocName , System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();

                arrRet = new string[2];
                arrRet[0] = "true";
                arrRet[1] = strDocName;
            }
            catch (Exception expErr)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = expErr.Message.Trim();
            }
            finally
            {
                strReturnMsg = null; strCatchMessage = null;
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }

            return arrRet;
        }
        #endregion

        #region GenerateRadiologistReport(AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] GenerateRadiologistReport(string StudyID, string strPatientName, string FolderName, string UserID)
        {
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];

            string[] arrRet = new string[0];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(Server.MapPath("~"));
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);

                objDoc.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportServerURL"]);
                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/CompareRadReport";

                strDocName = "RADIOLOGIST_REPORT_" + strPatientName.ToUpper().Trim().Replace(" ", "_") + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf";

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


                System.IO.FileStream objFS = new System.IO.FileStream(FolderName + "/" + strDocName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();

                arrRet = new string[2];
                arrRet[0] = "true";
                arrRet[1] = strDocName;
            }
            catch (Exception expErr)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = expErr.Message.Trim();
            }
            finally
            {
                strReturnMsg = null; strCatchMessage = null;
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }

            return arrRet;
        }
        #endregion

        #region GenerateFinalReport(AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] GenerateFinalReport(string StudyID, string strPatientName, string FolderName, string UserID)
        {
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            string strConnStr = string.Empty; string strDBUID = string.Empty; string strDBPwd = string.Empty; string strDocName = string.Empty;
            string[] arrConnStr = new string[0];

            byte[] btData = null;
            ServerReport objDoc = new ServerReport();
            DataSourceCredentials[] objCred = new DataSourceCredentials[1];
            ReportParameter[] objParam = new ReportParameter[2];

            string[] arrRet = new string[0];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(Server.MapPath("~"));
                strConnStr = CoreCommon.CONNECTION_STRING;
                arrConnStr = strConnStr.Split(';');
                strDBUID = arrConnStr[2].Substring(arrConnStr[2].IndexOf('=') + 1);
                strDBPwd = arrConnStr[3].Substring(arrConnStr[3].IndexOf('=') + 1);

                objDoc.ReportServerUrl = new Uri(System.Configuration.ConfigurationManager.AppSettings["ReportServerURL"]);
                objDoc.ReportPath = System.Configuration.ConfigurationManager.AppSettings["ServerReportFolder"] + "/Docs/CompareFinalRpt";

                strDocName = "FINAL_REPORT_" + strPatientName.ToUpper().Trim().Replace(" ", "_") + "_" + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".pdf";

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


                System.IO.FileStream objFS = new System.IO.FileStream(FolderName + "/" + strDocName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                objFS.Write(btData, 0, btData.Length);
                objFS.Close();

                arrRet = new string[2];
                arrRet[0] = "true";
                arrRet[1] = strDocName;
            }
            catch (Exception expErr)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = expErr.Message.Trim();
            }
            finally
            {
                strReturnMsg = null; strCatchMessage = null;
                objDoc = null; objCred = null; objParam = null;
                btData = null;
            }

            return arrRet;
        }
        #endregion

        #region CompareReports(AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] CompareReports(string SourceFileName, string FinalFileName, string FolderName)
        {
            ComparePDFFiles objComp = new ComparePDFFiles();
            string[] arrRet = new string[0];
             string[] arrComp = new string[0];

            try
            {
                objComp.FirstFile = SourceFileName.Trim();
                objComp.SecondFile = FinalFileName.Trim();
                objComp.Location = FolderName;
                objComp.FileName1_WithoutExtension = SourceFileName.Trim().Substring(0, SourceFileName.Trim().LastIndexOf("."));
                objComp.FileName2_WithoutExtension = FinalFileName.Trim().Substring(0, FinalFileName.Trim().LastIndexOf("."));

                arrComp = objComp.Compare();

                arrRet = new string[3];
                arrRet[0] = "true";
                arrRet[1] = Path.GetFileName(arrComp[0]);
                arrRet[2] = Path.GetFileName(arrComp[1]);
                

            }
            catch (Exception expErr)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = expErr.Message.Trim();
            }
            finally
            {
                objComp = null;
            }

            return arrRet;
        }
        #endregion
    }
}