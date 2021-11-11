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
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Net;
using VETRIS.Core;
using AjaxPro;

namespace VETRIS.CaseList
{
    [AjaxPro.AjaxNamespace("VRSDownloadImg")]
    public partial class VRSDownloadImg : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Case.CaseStudy objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSDownloadImg));
            SetPageValue();
            SetAttributes();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            btnClose1.Attributes.Add("onclick", "javascript:parent.HideGeneralMedium();");
           
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            hdnID.Value = Request.QueryString["id"];
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            string strIsArchive = Request.QueryString["arch"];
            hdnUserID.Value = UserID.ToString();
            SetCSS(Request.QueryString["th"]);
            LoadHeader(intMenuID, UserID, strIsArchive);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
        }
        #endregion

        #region LoadHeader
        private void LoadHeader(int intMenuID, Guid UserID, string IsArchive)
        {
            objCore = new Core.Case.CaseStudy();
            objComm = new classes.CommonClass();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            DataSet ds = new DataSet();
            string strArchFolderPath = string.Empty;
            string strArchFolderAltPath = string.Empty;
            string strFolderName = string.Empty;
            string[] arrayCode = new string[0];
            string strSuffix = string.Empty;
            bool bArchFldrExists = false;
            bool bArchFldrAltExists = false;


            try
            {
                objCore.ID = new Guid(hdnID.Value);
                objCore.MENU_ID = intMenuID;
                objCore.USER_ID = UserID;

                if(IsArchive=="N") 
                     bReturn = objCore.LoadHeader(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);
                else
                    bReturn = objCore.LoadArchiveHeader(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    if (objCore.STUDY_UID != string.Empty)
                    {
                        #region Radiologist Functional Rights
                        foreach (DataRow dr in ds.Tables["RadFnRights"].Rows)
                        {
                            if (hdnRadFnRights.Value.Trim() != string.Empty) hdnRadFnRights.Value += objComm.RecordDivider;
                            hdnRadFnRights.Value += Convert.ToString(dr["right_code"]);
                        }
                        #endregion

                        hdnDCMMODIFYEXEPATH.Value = objCore.DCM_FILE_MODIFY_EXE_PATH;
                        hdnInstCode.Value = objCore.INSTITUTION_CODE;
                        hdnPhysCode.Value = objCore.PHYSICIAN_CODE;

                        strFolderName = objCore.INSTITUTION_CODE + "_" + objCore.INSTITUTION_NAME + "_" + objCore.STUDY_UID;
                        if (Directory.Exists(objCore.PACS_ARCHIVE_FOLDER + "/" + strFolderName)) bArchFldrExists = true;
                        if (Directory.Exists(objCore.PACS_ARCHIVE_ALTERNATE_FOLDER + "/" + strFolderName)) bArchFldrAltExists = true;

                        if (bArchFldrExists || bArchFldrAltExists)
                        {
                            hdnArchFolderName.Value = strFolderName;
                            hdnArchFolderPath.Value = objCore.PACS_ARCHIVE_FOLDER + "/" + strFolderName;
                            hdnArchAltFolderPath.Value = objCore.PACS_ARCHIVE_ALTERNATE_FOLDER + "/" + strFolderName;
                            strSuffix = CoreCommon.RandomString(6);
                            hdnTgtFolderName.Value = objCore.INSTITUTION_CODE + "_" + strSuffix;
                            hdnSuffix.Value = strSuffix;
                        }
                        else
                        {
                            arrayCode = new string[1];
                            arrayCode[0] = "399";
                            lblMsg.Text = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
                        }
                    }
                    else
                    {
                        arrayCode = new string[1];
                        arrayCode[0] = "094";
                        lblMsg.Text = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
                    }
                }
                else
                {
                    arrayCode = new string[1];
                    if (strCatchMessage.Trim() != string.Empty)
                        arrayCode[0] = strCatchMessage.Trim();
                    else
                        arrayCode[0] = strReturnMessage.Trim();
                    lblMsg.Text = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
                    
                }

                CreateUserDirectory(UserID);
            }
            catch (Exception ex)
            {
                arrayCode = new string[1];
                arrayCode[0] = ex.Message.Trim();
                lblMsg.Text = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
            }
            finally
            {
                ds.Dispose(); objCore = null; objComm = null;
            }
        }
        #endregion

        #region CreateUserDirectory
        private void CreateUserDirectory(Guid UserID)
        {
            if (!Directory.Exists(Server.MapPath("~/CaseList/Temp/" + UserID.ToString())))
            {
                Directory.CreateDirectory(Server.MapPath("~/CaseList/Temp/" + UserID.ToString()));
            }
        }
        #endregion

        #region CopyFolder (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] CopyFolder(string[] ArrRecord)
        {
            bool bReturn = true;
            string[] arrRet = new string[0];
            string strArchFolderName = string.Empty;
            string strArchFolderPath = string.Empty;
            string strArchFolderAltPath = string.Empty;
            string strTgtFolderPath = string.Empty;
            string strTgtFolderName = string.Empty;
            string strFileName = string.Empty;
            string strDCMMODIFYEXEPATH = string.Empty;
            string strVWINSTINFO = string.Empty;
            string strUserID = string.Empty;
            string[] arrayCode = new string[0];
            string[] arrFiles = new string[0];
            string[] arrFilesAlt = new string[0];
            string[] pathElements = new string[0];
            string strInstCode = string.Empty;
            string strPhysCode = string.Empty;
            string strSuffix = string.Empty;
            string strNewFileName = string.Empty;
            string strExtn = string.Empty;
            string strOutMsg= string.Empty;
            string strRetMsg= string.Empty;

            objComm = new classes.CommonClass();
            objCore = new Core.Case.CaseStudy();


            try
            {
                strArchFolderPath = ArrRecord[0].Trim();
                strArchFolderAltPath = ArrRecord[1].Trim();
                strArchFolderName = ArrRecord[2].Trim();
                strTgtFolderName = ArrRecord[3].Trim();
                strDCMMODIFYEXEPATH = ArrRecord[4].Trim();
                strVWINSTINFO = ArrRecord[5].Trim();
                strInstCode = ArrRecord[6];
                strPhysCode = ArrRecord[7];
                strSuffix = ArrRecord[8];
                strUserID  = ArrRecord[9].Trim();
                
                strTgtFolderPath = Server.MapPath("~/CaseList/Temp/" + strUserID + "/" + strTgtFolderName);
                strTgtFolderPath = strTgtFolderPath.Replace("ajaxpro\\", "");
                if (!Directory.Exists(strTgtFolderPath))
                {
                    Directory.CreateDirectory(strTgtFolderPath);
                }

                if (Directory.Exists(strArchFolderPath)) arrFiles = Directory.GetFiles(strArchFolderPath);
                if (Directory.Exists(strArchFolderAltPath)) arrFilesAlt = Directory.GetFiles(strArchFolderAltPath);

                #region Files In Archive Path
                if (arrFiles.Length > 0)
                {
                    for (int i = 0; i < arrFiles.Length; i++)
                    {
                        strExtn = Path.GetExtension(arrFiles[i]);
                        pathElements = arrFiles[i].Split('\\');
                        strFileName = pathElements[(pathElements.Length - 1)];
                        strNewFileName = strInstCode + "_" + strSuffix + "_" + (i + 1).ToString();
                        if (strExtn.Trim() != string.Empty) strNewFileName = strNewFileName + strExtn;


                        if (File.Exists(strTgtFolderPath + "/" + strNewFileName)) File.Delete(strTgtFolderPath + "/" + strNewFileName);
                        File.Copy(arrFiles[i], strTgtFolderPath + "/" + strNewFileName);

                        if (strVWINSTINFO == "N")
                        {
                            ModifyDCMFile(strDCMMODIFYEXEPATH, strInstCode, strPhysCode, strTgtFolderPath + "/" + strNewFileName, ref strOutMsg, ref strRetMsg);
                        }

                        if(File.Exists(strTgtFolderPath + "/" + strNewFileName+ ".bak")) File.Delete(strTgtFolderPath + "/" + strNewFileName+ ".bak");
                    }
                }
                #endregion

                #region Files In Archive Path
                if (arrFilesAlt.Length > 0)
                {
                    for (int i = 0; i < arrFilesAlt.Length; i++)
                    {
                        strExtn = Path.GetExtension(arrFilesAlt[i]);
                        pathElements = arrFilesAlt[i].Split('\\');
                        strFileName = pathElements[(pathElements.Length - 1)];
                        strNewFileName = strInstCode + "_" + strSuffix + "_" + (i + 1).ToString();
                        if (strExtn.Trim() != string.Empty) strNewFileName = strNewFileName + strExtn;

                        if (File.Exists(strTgtFolderPath + "/" + strNewFileName)) File.Delete(strTgtFolderPath + "/" + strNewFileName);
                        File.Copy(arrFilesAlt[i], strTgtFolderPath + "/" + strNewFileName);

                        if (strVWINSTINFO == "N")
                        {
                            ModifyDCMFile(strDCMMODIFYEXEPATH, strInstCode, strPhysCode, strTgtFolderPath + "/" + strNewFileName, ref strOutMsg, ref strRetMsg);
                        }

                        if (File.Exists(strTgtFolderPath + "/" + strNewFileName + ".bak")) File.Delete(strTgtFolderPath + "/" + strNewFileName + ".bak");
                    }
                }
                #endregion


                if (arrFiles.Length == 0 && arrFilesAlt.Length == 0)
                {
                    bReturn = false;
                    arrayCode = new string[1];
                    arrRet = new string[2];
                    arrayCode[0] = "399";
                    arrRet[0] = "false";
                    arrRet[1] = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
                }
                else
                {
                    arrRet = new string[2];
                    arrayCode = new string[1];
                    arrayCode[0] = "402";
                    arrRet[0] = "true";
                    arrRet[1] = objComm.SetErrorResources(arrayCode, "N", false, string.Empty, string.Empty);
                }
            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrayCode = new string[1];
                arrRet = new string[2];
                arrayCode[0] = expErr.Message.Trim();
                arrRet[0] = "catch";
                arrRet[1] = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
            }
            finally
            {
                objCore = null; objComm = null;
            }
            return arrRet;
        }
        #endregion

        #region CompressFolder (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] CompressFolder(string[] ArrRecord)
        {
            bool bReturn = true;
            string[] arrRet = new string[0];
            string strTgtFolderName = string.Empty;
            string strTgtFolderPath = string.Empty;
            string strZipPath = string.Empty;
            string strFileName = string.Empty;
            string strUserID = string.Empty;
            string[] arrayCode = new string[0];
            string[] arrFiles = new string[0];
            string[] pathElements = new string[0];

            objComm = new classes.CommonClass();
            objCore = new Core.Case.CaseStudy();


            try
            {
                strTgtFolderName = ArrRecord[0].Trim();
                strUserID = ArrRecord[1].Trim();
                strTgtFolderPath = Server.MapPath("~/CaseList/Temp/" + strUserID + "/" + strTgtFolderName);
                strTgtFolderPath = strTgtFolderPath.Replace("ajaxpro\\", "");
                strZipPath = strTgtFolderPath + ".zip";

                if (Directory.Exists(strTgtFolderPath))
                {
                    ZipFile.CreateFromDirectory(strTgtFolderPath, strZipPath);

                    arrFiles = Directory.GetFiles(strTgtFolderPath);

                    if (arrFiles.Length > 0)
                    {
                        for (int i = 0; i < arrFiles.Length; i++)
                        {
                            File.Delete(arrFiles[i]);
                        }
                        if (Directory.GetFiles(strTgtFolderPath).Length == 0)
                        {
                            Directory.Delete(strTgtFolderPath);
                        }
                        
                    }

                
                    arrRet = new string[3];
                    arrayCode = new string[1];
                    arrayCode[0] = "403";
                    arrRet[0] = "true";
                    arrRet[1] = objComm.SetErrorResources(arrayCode, "N", false, string.Empty, string.Empty);
                    arrRet[2] = strTgtFolderName + ".zip";
                }
                else
                {
                    bReturn = false;
                    arrayCode = new string[1];
                    arrRet = new string[2];
                    arrayCode[0] = "401";
                    arrRet[0] = "false";
                    arrRet[1] = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
                }               
            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrayCode = new string[1];
                arrRet = new string[2];
                arrayCode[0] = expErr.Message.Trim();
                arrRet[0] = "catch";
                arrRet[1] = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
            }
            finally
            {
                objCore = null; objComm = null;
            }
            return arrRet;
        }
        #endregion

        #region ModifyDCMFile
        private bool ModifyDCMFile(string strDCMMODIFYEXEPATH, string strInsName, string strPhysName, string strDCMPath, ref string strOutputMsg, ref string strReturnMessage)
        {
            bool bRet = false;
            string strProcOutput = string.Empty;
            string strProcError = string.Empty;

            /*
             * (0008,0080) Institution
             * (0008,0081) Institution Address
             * (0008,0090) Referring Physician
             * (0008,1050) Performing Physician
             */

            try
            {
                Process ProcModInst = new Process();
                ProcModInst.StartInfo.UseShellExecute = false;
                ProcModInst.StartInfo.FileName = strDCMMODIFYEXEPATH;
                ProcModInst.StartInfo.Arguments = "-i \"(0008,0080)=" + strInsName + "\"" + " " + strDCMPath;
                ProcModInst.StartInfo.RedirectStandardOutput = true;
                ProcModInst.StartInfo.RedirectStandardError = true;
                ProcModInst.Start();
                strProcOutput = ProcModInst.StandardOutput.ReadToEnd();
                strProcError = ProcModInst.StandardError.ReadToEnd();
                strOutputMsg = strProcOutput.Trim();
                bRet = true;


            }
            catch (Exception ex)
            {
                bRet = false;
                strReturnMessage = ex.Message.Trim();
            }

            if (bRet)
            {
                try
                {
                    Process ProcModInstAddr = new Process();
                    ProcModInstAddr.StartInfo.UseShellExecute = false;
                    ProcModInstAddr.StartInfo.FileName = strDCMMODIFYEXEPATH;
                    ProcModInstAddr.StartInfo.Arguments = "-i \"(0008,0081)=" + string.Empty + "\"" + " " + strDCMPath;
                    ProcModInstAddr.StartInfo.RedirectStandardOutput = true;
                    ProcModInstAddr.StartInfo.RedirectStandardError = true;
                    ProcModInstAddr.Start();
                    strProcOutput = ProcModInstAddr.StandardOutput.ReadToEnd();
                    strProcError = ProcModInstAddr.StandardError.ReadToEnd();
                    strOutputMsg = strProcOutput.Trim();
                    bRet = true;


                }
                catch (Exception ex)
                {
                    bRet = false;
                    strReturnMessage = ex.Message.Trim();
                }
            }


            if (bRet)
            {
                try
                {
                    Process ProcModRefPhys = new Process();
                    ProcModRefPhys.StartInfo.UseShellExecute = false;
                    ProcModRefPhys.StartInfo.FileName = strDCMMODIFYEXEPATH;
                    ProcModRefPhys.StartInfo.Arguments = "-i \"(0008,0090)=" + strPhysName + "\"" + " " + strDCMPath;
                    ProcModRefPhys.StartInfo.RedirectStandardOutput = true;
                    ProcModRefPhys.StartInfo.RedirectStandardError = true;
                    ProcModRefPhys.Start();
                    strProcOutput = ProcModRefPhys.StandardOutput.ReadToEnd();
                    strProcError = ProcModRefPhys.StandardError.ReadToEnd();
                    strOutputMsg = strProcOutput.Trim();
                    bRet = true;


                }
                catch (Exception ex)
                {
                    bRet = false;
                    strReturnMessage = ex.Message.Trim();
                }

            }

            if (bRet)
            {
                try
                {
                    Process ProcModPerfPhys = new Process();
                    ProcModPerfPhys.StartInfo.UseShellExecute = false;
                    ProcModPerfPhys.StartInfo.FileName = strDCMMODIFYEXEPATH;
                    ProcModPerfPhys.StartInfo.Arguments = "-i \"(0008,1050)=" + strPhysName + "\"" + " " + strDCMPath;
                    ProcModPerfPhys.StartInfo.RedirectStandardOutput = true;
                    ProcModPerfPhys.StartInfo.RedirectStandardError = true;
                    ProcModPerfPhys.Start();
                    strProcOutput = ProcModPerfPhys.StandardOutput.ReadToEnd();
                    strProcError = ProcModPerfPhys.StandardError.ReadToEnd();
                    strOutputMsg = strProcOutput.Trim();
                    bRet = true;


                }
                catch (Exception ex)
                {
                    bRet = false;
                    strReturnMessage = ex.Message.Trim();
                }

            }
            return bRet;
        }
        #endregion
    }
}