using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.IO;
using VETRIS.Core;

namespace VETRIS.RND
{
    [AjaxPro.AjaxNamespace("DCMUploader")]
    public partial class DCMUploader : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(DCMUploader));
            SetAttributes();
            if (!CallBackDCM.IsCallback)
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnSubmit.Attributes.Add("onclick", "javascript:btnSubmit_OnClick();");
        }
        #endregion


        #region SetPageValue
        private void SetPageValue()
        {
            objComm = new classes.CommonClass();
            hdnRecDivider.Value = objComm.RecordDivider.ToString();
            hdnSecDivider.Value = objComm.SecondaryRecordDivider.ToString();
            hdnTempFolder.Value = Server.MapPath("~") + "/RND/UploadedFiles"; ;
            hdnRootDirectory.Value = ConfigurationManager.AppSettings["RootDirectory"];
            objComm = null;
            
        }
        #endregion

        #region DICOM Grid

        #region CallBackDCM_Callback
        protected void CallBackDCM_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];

            switch (strAction)
            {
                case "A":
                    AddDCMFiles(e.Parameters);
                    break;
                case "D":
                    DeleteDCMFiles(e.Parameters);
                    break;
            }
            grdDCM.Width = Unit.Percentage(100);
            grdDCM.RenderControl(e.Output);
            spnERRDCM.RenderControl(e.Output);
        }
        #endregion

        #region AddDCMFiles
        private void AddDCMFiles(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);
            string[] arrNew = arrParams[2].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;

            try
            {
                dtbl = CreateFileTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i=i+2)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["srl_no"] = intSrl;
                            dr["dcm_file_name"] = arrRecords[i + 1].Trim().Substring(arrRecords[i + 1].Trim().LastIndexOf("/") + 1, (arrRecords[i + 1].Trim().Length - (arrRecords[i + 1].Trim().LastIndexOf("/") + 1)));
                            dtbl.Rows.Add(dr);
                        }

                    }
                }

                for (int i = 0; i < arrNew.Length; i++)
                {
                    DataRow drNew = dtbl.NewRow();
                    intSrl = intSrl + 1;
                    drNew["srl_no"] = intSrl;
                    drNew["dcm_file_name"] = arrNew[i].Trim().Substring(arrNew[i].Trim().LastIndexOf("/") + 1, (arrNew[i].Trim().Length - (arrNew[i].Trim().LastIndexOf("/") + 1)));
                    dtbl.Rows.Add(drNew);
                }

                grdDCM.DataSource = dtbl;
                grdDCM.DataBind();
                spnERRDCM.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDoc\" value=\"\" />";
            }
            catch (Exception ex)
            {
                spnERRDCM.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDoc\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region DeleteDCMFiles
        private void DeleteDCMFiles(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            int intID = Convert.ToInt32(arrParams[1]);
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            string strFolder = arrParams[3];
            string strFileName = string.Empty;
            int intSrl = 0;

            try
            {
                dtbl = CreateFileTable();
                if (arrRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrRecords.Length; i= i+2)
                    {
                        if (Convert.ToInt32(arrRecords[i]) != intID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["srl_no"] = intSrl;
                            dr["dcm_file_name"] = arrRecords[i + 1].Trim();
                            dtbl.Rows.Add(dr);
                        }
                        else
                            strFileName = arrRecords[i + 1].Trim();
                    }
                }

                grdDCM.DataSource = dtbl;
                grdDCM.DataBind();
                spnERRDCM.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDoc\" value=\"\" />";

                strFolder = strFolder.Replace("\\", "/");
                if (strFileName.Trim() != string.Empty)
                {
                    if(File.Exists(strFolder + "/" + strFileName))
                    {
                        File.Delete(strFolder + "/" + strFileName);
                    }
                }

            }
            catch (Exception ex)
            {
                spnERRDCM.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrDoc\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region CreateFileTable
        private DataTable CreateFileTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("srl_no", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("dcm_file_name", System.Type.GetType("System.String"));
            dtbl.TableName = "DCM";
            return dtbl;
        }
        #endregion

        #region SetFile
        private void SetFile(byte[] DocData, string strFileName, string strPath)
        {
            string strFilePath = Server.MapPath("~") + "/" + strPath + "/" + strFileName;
            using (FileStream fs = new FileStream(strFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(DocData, 0, DocData.Length);
                fs.Flush();
                fs.Close();
            }

        }
        #endregion

        #region GetFileBytes
        private byte[] GetFileBytes(string strFileName)
        {
            byte[] buff = File.ReadAllBytes(strFileName);
            return buff;
        }
        #endregion

        #endregion

        #region SubmitFiles (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SubmitFiles(string[] arrFiles)
        {
            bool bReturn = false;
            string[] arrRet = new string[2];
           
            string strFileName = string.Empty;
            string strTargetPath = "E:/VetChoice/FTP_DOWNLOAD_TEMP";
            string strSourcePath = "E:/VetChoice/VETRIS/RND/UploadedFiles";
            
            try
            { 
                for (int i = 0; i < arrFiles.Length; i++)
                {
                    if (File.Exists(strSourcePath + "/" + arrFiles[i].Trim()))
                    {
                        if (File.Exists(strTargetPath + "/" + arrFiles[i].Trim())) File.Delete(strTargetPath + "/" + arrFiles[i].Trim());
                        File.Move(strSourcePath + "/" + arrFiles[i].Trim(), strTargetPath + "/" + arrFiles[i].Trim());
                    }
                }

                arrRet[0] = "true";
                arrRet[1] = "";
            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrRet[0] = "catch";
                arrRet[1] = expErr.Message.Trim();
            }
            finally
            {
                
                
            }
            return arrRet;
        }
        #endregion
    }


}