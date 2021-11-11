using System;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;


namespace VETRIS.CaseList
{
    public partial class VRSUploadFiles : System.Web.UI.Page
    {
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {

            string strLanguage = ConfigurationManager.AppSettings["Lang"];
            if (!IsPostBack)
            {
                if (Request.QueryString["dir"] != null) hdnDirName.Value = Request.QueryString["dir"];
                if (Request.QueryString["t"] != null) hdnUploadType.Value = Request.QueryString["t"];
                if (Request.QueryString["fileID"] != null) hdnFileID.Value = Request.QueryString["fileID"];
                //if (strLanguage == "en-GB")
                //lblUploadHdr.Text = ConfigurationManager.AppSettings["WindowTitle"] + " : Upload File";
                //lblUploadHdr.Text = "Upload File";
                //else
                //    lblUploadHdr.Text = ConfigurationManager.AppSettings["WindowTitle"] + " : " + LobjComm.GetResourcesValue("UploaderPageTitle");
                if (hdnUploadType.Value == "IMG") lblUploadHelpDoc.Visible = false;


            }
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

        #region btnUpload_Click
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string strServerPath = hdnDirName.Value;
            string strClientFileName = "";
            string strServerFileName = "";
            string strFileName = ""; int intValidFlag = 0; string strFileContentType = "";
            double dblFileSize = 0;

            try
            {
                strServerPath = strServerPath.Replace("\\", "/");

                if (!Directory.Exists(strServerPath))
                {
                    Directory.CreateDirectory(strServerPath);
                }

                if ((flUpload.PostedFile.ContentLength != 0) || (flUpload.PostedFile.FileName.Length != 0))
                {
                    dblFileSize = flUpload.PostedFile.ContentLength;
                    //dblFileSize = dblFileSize / (Math.Pow(1024, 5));
                    dblFileSize = Math.Round((dblFileSize / 1024) / 1024,2);
                    strFileContentType = flUpload.PostedFile.ContentType.Trim();

                    if (hdnFileID.Value != "")
                    {
                        strClientFileName = Path.GetFileName(flUpload.PostedFile.FileName);
                        //switch (strFileContentType)
                        //{
                        //    case "image/pjpeg":
                        //    case "image/jpeg":
                        //        strClientFileName = hdnFileID.Value + ".jpg";
                        //        break;
                        //    case "image/x-png":
                        //    case "image/png":
                        //        strClientFileName = hdnFileID.Value + ".png";
                        //        break;
                        //    case "image/gif":
                        //        strClientFileName = hdnFileID.Value + ".gif";
                        //        break;
                        //    case "image/bmp":
                        //        strClientFileName = hdnFileID.Value + ".bmp";
                        //        break;
                        //    case "text/plain":
                        //        strClientFileName = hdnFileID.Value + ".txt";
                        //        break;
                        //    case "application/msword":
                        //    case "vnd.openxmlformats-officedocument.wordprocessingml.document":
                        //        strClientFileName = hdnFileID.Value + ".docx";
                        //        break;
                        //    case "application/vnd.ms-excel":
                        //    case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                        //        strClientFileName = hdnFileID.Value + ".xlsx";
                        //        break;
                        //    case "application/pdf":
                        //        strClientFileName = hdnFileID.Value + ".pdf";
                        //        break;
                        //    case "application/vnd.ms-powerpoint":
                        //    case "application/vnd.openxmlformats-officedocument.presentationml.presentation":
                        //        strClientFileName = hdnFileID.Value + ".pptx";
                        //        break;

                        //}

                    }
                    if (txtDocName.Text.Trim() == "")
                    {
                        txtDocName.Text = strClientFileName;
                    }

                    strServerFileName = Path.Combine(strServerPath, strClientFileName);


                    if (hdnUploadType.Value == "IMG")
                    {
                        if ((strFileContentType == "image/pjpeg") || (strFileContentType == "image/jpeg") || (strFileContentType == "image/x-png") || (strFileContentType == "image/png") || (strFileContentType == "image/gif") || (strFileContentType == "image/bmp"))
                        {
                            if (dblFileSize <= 100) intValidFlag = 1;
                        }
                        else
                            intValidFlag = 0;
                    }
                    else
                    {
                        //if ((strFileContentType == "text/plain") || (strFileContentType == "image/pjpeg")
                        //|| (strFileContentType == "application/vnd.ms-visio.viewer") || (strFileContentType == "image/jpeg") || (strFileContentType == "image/png")
                        //|| (strFileContentType == "application/msword") || (strFileContentType == "application/vnd.ms-excel")
                        //|| (strFileContentType == "application/pdf") || (strFileContentType == "application/vnd.ms-powerpoint")
                        //|| (strFileContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                        //|| (strFileContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                        //|| (strFileContentType == "application/vnd.openxmlformats-officedocument.presentationml.presentation"))
                        if ((strFileContentType == "application/pdf"))
                        {
                            if (dblFileSize <= 100) intValidFlag = 1;
                        }
                        //else if ((strFileContentType == "image/pjpeg") || (strFileContentType == "image/jpeg") || (strFileContentType == "image/x-png") || (strFileContentType == "image/png") || (strFileContentType == "image/gif") || (strFileContentType == "image/bmp"))
                        else if ((strFileContentType == "image/pjpeg") || (strFileContentType == "image/jpeg") || (strFileContentType == "image/x-png") || (strFileContentType == "image/png"))
                        {
                            if (dblFileSize <=100) intValidFlag = 1;
                        }
                        else
                            intValidFlag = 0;
                    }

                    //intValidFlag = 1;
                    if (intValidFlag == 1)
                    {
                        strFileName = strServerFileName.Substring(strServerFileName.LastIndexOf("/") + 1, (strServerFileName.Length - (strServerFileName.LastIndexOf("/") + 1)));
                        flUpload.PostedFile.SaveAs(strServerFileName);
                        CloseUploadWindow(strFileName, txtDocName.Text.Trim(), "D");
                    }
                    else
                    {
                        if (hdnUploadType.Value == "IMG")
                            lblError.Text = lblUploadHelpStart.Text + lblUploadHelpImg.Text + lblUploadHelpSize.Text + lblUploadHelpEnd.Text;
                        else
                            lblError.Text = lblUploadHelpStart.Text + lblUploadHelpDoc.Text + lblUploadHelpImg.Text + lblUploadHelpSize.Text + lblUploadHelpEnd.Text;

                        lblError.Text +="<br/>Size ofthe file selected is "+ dblFileSize.ToString() +  " MB";
                    }
                }
                txtDocName.Text = "";
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message.ToString();
            }
        }
        #endregion

        #region CloseUploadWindow
        private void CloseUploadWindow(string strFileName, string DocName, string Mode)
        {
            ClientScriptManager Lcs = Page.ClientScript;
            StringBuilder strbCloseWindowScript = new StringBuilder();
            if (!Lcs.IsStartupScriptRegistered(this.GetType(), "CloseUploadWindowScript"))
            {
                strbCloseWindowScript.Append("ReturnUploadData('" + strFileName + "','" + DocName + "','" + Mode + "');");
                Lcs.RegisterStartupScript(this.GetType(), "CloseUploadWindow", strbCloseWindowScript.ToString(), true);
            }
        }
        #endregion
    }
}