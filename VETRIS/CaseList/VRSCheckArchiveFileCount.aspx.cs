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
     [AjaxPro.AjaxNamespace("VRSCheckArchiveFileCount")]
    public partial class VRSCheckArchiveFileCount : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Case.CaseStudy objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSCheckArchiveFileCount));
            SetPageValue();
            SetAttributes();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
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
            bool bArchFldrExists = false;
            bool bArchFldrAltExists = false;


            try
            {
                objCore.ID = new Guid(hdnID.Value);
                objCore.MENU_ID = intMenuID;
                objCore.USER_ID = UserID;

                if (IsArchive == "N")
                    bReturn = objCore.LoadHeader(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);
                else
                    bReturn = objCore.LoadArchiveHeader(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    if (objCore.STUDY_UID != string.Empty)
                    {
                        lblPatientName.Text = objCore.PATIENT_NAME;
                        hdnStudyUID.Value = objCore.STUDY_UID;
                        hdnInstCode.Value = objCore.INSTITUTION_CODE;
                        hdnInstName.Value = objCore.INSTITUTION_NAME;

                        strFolderName = objCore.INSTITUTION_CODE + "_" + objCore.INSTITUTION_NAME + "_" + objCore.STUDY_UID;
                        if (Directory.Exists(objCore.PACS_ARCHIVE_FOLDER + "/" + strFolderName)) bArchFldrExists = true;
                        if (Directory.Exists(objCore.PACS_ARCHIVE_ALTERNATE_FOLDER + "/" + strFolderName)) bArchFldrAltExists = true;

                        if (bArchFldrExists || bArchFldrAltExists)
                        {
                            
                            hdnArchFolderPath.Value = objCore.PACS_ARCHIVE_FOLDER + "/" + strFolderName;
                            hdnArchAltFolderPath.Value = objCore.PACS_ARCHIVE_ALTERNATE_FOLDER + "/" + strFolderName;
                           
                        }
                        else
                        {
                            hdnError.Value = "399";
                            arrayCode = new string[1];
                            arrayCode[0] = "399";
                            lblMsg.Text = objComm.SetErrorResources(arrayCode, "N", true, string.Empty, string.Empty);
                        }
                    }
                    else
                    {
                        hdnError.Value = "094";
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

        #region CheckFileCount (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] CheckFileCount(string[] ArrRecord)
        {
            bool bReturn = true;
            string[] arrRet = new string[0];
            Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
            string strArchFolderPath = string.Empty;
            string strArchFolderAltPath = string.Empty;
            int intActualFileCount = 0;
           
            string[] arrayCode = new string[0];
            string[] arrFiles = new string[0];
            string[] arrFilesAlt = new string[0];

            string strCatchMessage = string.Empty; string strReturnMessage = string.Empty;

            objComm = new classes.CommonClass();
            objCore = new Core.Case.CaseStudy();


            try
            {
                Id = new Guid(ArrRecord[0]);
                strArchFolderPath = ArrRecord[1].Trim();
                strArchFolderAltPath = ArrRecord[2].Trim();
                
                if (Directory.Exists(strArchFolderPath)) arrFiles = Directory.GetFiles(strArchFolderPath);
                if (Directory.Exists(strArchFolderAltPath)) arrFilesAlt = Directory.GetFiles(strArchFolderAltPath);

                intActualFileCount = arrFiles.Length + arrFilesAlt.Length;

                if (intActualFileCount == 0)
                {
                    bReturn = false;
                    arrRet = new string[2];
                    arrayCode = new string[1];
                    arrayCode[0] = "492";
                    arrRet[0] = "true";
                    arrRet[1] = objComm.SetErrorResources(arrayCode, "N", false, string.Empty, string.Empty);
                }
                else
                {
                    objCore.ID = Id;
                    objCore.ACTUAL_FILE_COUNT = intActualFileCount;

                    bReturn = objCore.CheckArchivedFiles(Server.MapPath("~"),ref strReturnMessage, ref strCatchMessage);

                    if (bReturn)
                    {
                        arrRet = new string[2];
                        arrayCode = new string[1];
                        arrayCode[0] = strReturnMessage;
                        arrRet[0] = "true";
                        arrRet[1] = objComm.SetErrorResources(arrayCode, "N", false, string.Empty, string.Empty);
                    }
                    else
                    {
                        arrRet = new string[2];
                        arrayCode = new string[1];
                        if(strCatchMessage.Trim()!= string.Empty) arrayCode[0] = strCatchMessage;
                        else arrayCode[0] = strReturnMessage;
                        arrRet[0] = "false";
                        arrRet[1] = objComm.SetErrorResources(arrayCode, "N", false, string.Empty, string.Empty);
                    }
                }

            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrayCode = new string[1];
                arrRet = new string[2];
                arrayCode[0] = expErr.Message;
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
    }
}