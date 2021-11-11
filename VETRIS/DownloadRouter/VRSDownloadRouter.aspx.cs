using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Configuration;
using System.Data;
using VETRIS.Core;
using AjaxPro;

namespace VETRIS.DownloadRouter
{
    [AjaxPro.AjaxNamespace("VRSDownloadRouter")]
    public partial class VRSDownloadRouter : System.Web.UI.Page
    {
        #region Members & Variables
        Core.DownloadRouter.DownloadRouter objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSDownloadRouter));
            SetAttributes();
            SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnClose.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");
            btndownload.Attributes.Add("onclick", "javascript:btndownload_Onclick();");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"].ToString());
            LoadDetails(UserID);
            string strTheme = Request.QueryString["th"];
            SetCSS(strTheme);
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
        private void LoadDetails(Guid UserID)
        {
            objCore = new Core.DownloadRouter.DownloadRouter();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objCore.USER_ID = UserID;
                bReturn = objCore.FetchDetails(Server.MapPath("~"),ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    hdnVer.Value = objCore.VERSION;
                    btndownload.InnerHtml = "<i class='fa fa-download' aria-hidden='true'></i>&nbsp;Click here to download the DICOM Router - Version " + objCore.VERSION;

                    #region Institutions
                    DataRow dr = ds.Tables["Institutions"].NewRow();
                    dr["code"] = "";
                    dr["name"] = "Select One";
                    ds.Tables["Institutions"].Rows.InsertAt(dr, 0);
                    ds.Tables["Institutions"].AcceptChanges();

                    ddlInstitution.DataSource = ds.Tables["Institutions"];
                    ddlInstitution.DataValueField = "code";
                    ddlInstitution.DataTextField = "name";
                    ddlInstitution.DataBind();

                    if (ds.Tables["Institutions"].Rows.Count == 2)
                    {
                        ddlInstitution.SelectedIndex = 1;
                    }
                    #endregion
                }
                else
                {
                    hdnError.Value = "catch" + objComm.RecordDivider + strCatchMessage.Trim();
                }


            }

            catch (Exception ex)
            {
                hdnError.Value = "catch" + objComm.RecordDivider + ex.Message.Trim();
            }
            finally
            {
                objComm = null; objCore = null;
            }
        }
        #endregion

        #region PrepareDownload (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] PrepareDownload(string strVersion,string strInstCode,string strUserID)
        {
            string[] arrRet = new string[0];
            string strSourceDirPath = string.Empty;
            string strTargetDirPath = string.Empty;
            string strSourceFileName = string.Empty;
            string strTargetFileName = string.Empty;
            string strExtractPath = string.Empty;
            string strLicFilePath = string.Empty;
            string strEncInstCode = string.Empty;
            //string strDtToday = DateTime.Today.ToString("yyyyMMdd");
            string[] arrFiles = new string[0];
            objComm = new classes.CommonClass();
            


            try
            {
                strSourceDirPath = Server.MapPath("~") + "\\DownloadRouter\\Version_" + strVersion;
                strTargetDirPath = Server.MapPath("~") + "\\DownloadRouter\\Temp\\"  + strUserID;
                strSourceFileName = "DICOM_ROUTER_SETUP.zip";
                strTargetFileName = "DICOM_ROUTER_SETUP_" + strVersion.Replace(".","")+ "_" + strInstCode + ".zip";
                strExtractPath = strTargetDirPath + "\\" + "DICOM_ROUTER_SETUP_" + strVersion.Replace(".", "") + "_" + strInstCode;

                //Copy & Decompress
                if (!Directory.Exists(strTargetDirPath)) Directory.CreateDirectory(strTargetDirPath);
                if (File.Exists(strTargetDirPath + "\\" + strTargetFileName)) File.Delete(strTargetDirPath + "\\" + strTargetFileName);
                File.Copy(strSourceDirPath + "\\" + strSourceFileName, strTargetDirPath + "\\" + strTargetFileName);
                ZipFile.ExtractToDirectory(strTargetDirPath + "\\" + strTargetFileName, strExtractPath);
                File.Delete(strTargetDirPath + "\\" + strTargetFileName);

                //Create License File
                strLicFilePath = strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter";
                if (File.Exists(strLicFilePath + "\\DRLicense.lic")) File.Delete(strLicFilePath + "\\DRLicense.lic");
                strEncInstCode = CoreCommon.EncryptString(strInstCode);
                TextWriter tw = new StreamWriter(strLicFilePath + "\\DRLicense.lic");
                tw.WriteLine(strEncInstCode);
                tw.Close();

                //Commpress the setup
                ZipFile.CreateFromDirectory(strExtractPath, strTargetDirPath + "\\" + strTargetFileName);

                #region deletion of uncompressed setup
                arrFiles = Directory.GetFiles(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\bin");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\bin");
                arrFiles = Directory.GetFiles(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\etc\\dcmtk");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\etc\\dcmtk");
                Directory.Delete(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\etc");
                arrFiles = Directory.GetFiles(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\dcmtk\\wlistdb\\OFFIS");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\dcmtk\\wlistdb\\OFFIS");
                arrFiles = Directory.GetFiles(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\dcmtk\\wlistdb");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\dcmtk\\wlistdb");
                arrFiles = Directory.GetFiles(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\dcmtk\\wlistqry");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\dcmtk\\wlistqry");
                arrFiles = Directory.GetFiles(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\dcmtk");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\dcmtk");
                arrFiles = Directory.GetFiles(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\doc\\dcmtk");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\doc\\dcmtk");
                Directory.Delete(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share\\doc");
                Directory.Delete(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs\\share");
                Directory.Delete(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\EXEs");

                if (Directory.Exists(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\IMGToDCM"))
                {
                    arrFiles = Directory.GetFiles(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\IMGToDCM");
                    for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                    Directory.Delete(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs\\IMGToDCM");
                }
                Directory.Delete(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs\\DICOM-EXEs");
                arrFiles = Directory.GetFiles(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter\\Configs");
                arrFiles = Directory.GetFiles(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(strExtractPath + "\\DICOM_ROUTER_SETUP\\DicomRouter");
                arrFiles = Directory.GetFiles(strExtractPath + "\\DICOM_ROUTER_SETUP");
                for (int i = 0; i < arrFiles.Length; i++) File.Delete(arrFiles[i]);
                Directory.Delete(strExtractPath + "\\DICOM_ROUTER_SETUP");
                Directory.Delete(strExtractPath);
                #endregion

                arrRet = new string[2];
                arrRet[0] = "true";
                arrRet[1] = "/DownloadRouter/Temp/" + strUserID + "/" + strTargetFileName;
            }
            catch (Exception expErr)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = expErr.Message.Trim();
            }
            finally
            {
                objComm = null;
                
            }
            return arrRet;
        }
        #endregion 
    }
}