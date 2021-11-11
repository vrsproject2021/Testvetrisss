using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.IO;
using System.Text;

namespace VETRIS.RND
{
    public partial class uploader : System.Web.UI.UserControl
    {
        protected static ArrayList arrFiles = new ArrayList(); // has to be static since Adding and then reusing
        protected int isUploaded = 0;
        protected string pathToUpload = HttpContext.Current.Server.MapPath("UploadedFiles");
        classes.CommonClass objComm;

        protected void Page_Load(object sender, EventArgs e)
        {
            rdoFile.Attributes.Add("onclick", "javascript:SetUploaderAttribute();");
            rdoFolder.Attributes.Add("onclick", "javascript:SetUploaderAttribute();");
            //fUpload.Attributes.Add("onchange", "javascript:this.form.submit();");

            //if (Page.IsPostBack && fUpload.HasFiles) AddFilestoList();
        }

        //protected void btnAdd_Click(object sender, EventArgs e)
        //{
            
        //}

        private void AddFilestoList()
        {
            string[] arrFiles = new string[0];
            string strFileName = string.Empty;
            string strFileWithPath = string.Empty;
            string strFileList = string.Empty;
            int intIdx = 0;
            string strRandom = string.Empty;
            int intValidFlag = 0;
            objComm = new classes.CommonClass();


            try
            {

                divMsg.InnerText = "";
                //arrFiles = new string[fUpload.PostedFiles.Count];
                //foreach (HttpPostedFile uploadedFile in fUpload.PostedFiles)
                //{
                //    strFileName =  uploadedFile.FileName;
                    
                //    strRandom = RandomString(6);
                //    strFileName = strFileName.Replace("\\", "/");
                //    if (strFileName.Contains("/"))
                //    {
                //        strFileName = strFileName.Substring(strFileName.LastIndexOf("/") + 1, (strFileName.Length - (strFileName.LastIndexOf("/") + 1)));
                //    }
                //    strFileName = strRandom + "_" + strFileName;
                //    strFileWithPath = pathToUpload + "/" + strFileName;
                //    strFileWithPath = strFileWithPath.Replace("\\", "/");
                //    uploadedFile.SaveAs(strFileWithPath);
                //    arrFiles[intIdx] = strFileWithPath;
                //    intIdx = intIdx + 1;
                //}

                //for (int i = 0; i < arrFiles.Length; i++)
                //{
                //    if (IsDicomFile(arrFiles[i]))
                //    {
                //        if (strFileList.Trim() != string.Empty) strFileList += objComm.RecordDivider;
                //        strFileList += arrFiles[i];
                //        intValidFlag = 1;
                //    }
                //    else
                //    {
                //        string[] arrTmp = new string[0];
                //        arrTmp = arrFiles[i].Split('/');

                //        divMsg.InnerHtml += "Format of the file " + arrTmp[arrTmp.Length - 1] + " is not supported. Discarded<br/>" ;
                //        intValidFlag = 0;
                //        if(File.Exists(arrFiles[i])) File.Delete(arrFiles[i]);
                        
                //    }
                //}

               
                //UpdateFileList(strFileList);
                
            }

            catch (Exception ex)
            {
                divMsg.InnerText = "An error has occured while adding file" + ex.Message;
            }
            finally { objComm = null; }
        }

        #region UpdateFileList
        private void UpdateFileList(string strFiles)
        {
            ClientScriptManager Lcs = Page.ClientScript;
            StringBuilder strbUpdateFileListScript = new StringBuilder();
            if (!Lcs.IsStartupScriptRegistered(this.GetType(), "UpdateFileListScript"))
            {
                strbUpdateFileListScript.Append("ReturnUploadData('" + strFiles + "');");
                Lcs.RegisterStartupScript(this.GetType(), "UpdateFileList", strbUpdateFileListScript.ToString(), true);
            }
        }
        #endregion

        //protected void btnRemove_Click(object sender, EventArgs e)
        //{
        //    //Functionality to remove files

        //    //Veryfirst you have to remove from the arraylist and similarly from the listbox

        //    if (lstFiles.Items.Count != 0)
        //    {

        //        arrFiles.Remove(fUpload);

        //        lstFiles.Items.Remove(lstFiles.SelectedItem.Text);

        //    }
        //}

        //protected void btnUpload_Click(object sender, EventArgs e)
        //{
        //    //Very first check if the files are present to upload or Selected to upload

        //    if ((lstFiles.Items.Count == 0) && (isUploaded == 0))
        //    {

        //        lblMessage.Text = "Please specify file name";

        //    }

        //    else
        //    {

        //        //Take every element from the arraylist as HTMLInputFile, iterate through

        //        //each InputFile and upload the files to the specified location           

        //        foreach (System.Web.UI.WebControls.FileUpload Ipf in arrFiles)
        //        {

        //            try
        //            {

        //                string strFileName = System.IO.Path.GetFileName(Ipf.PostedFile.FileName);

        //                Ipf.PostedFile.SaveAs(pathToUpload + "\\" + strFileName);

        //                isUploaded = isUploaded + 1;

        //            }

        //            catch (Exception ex)
        //            {

        //                lblMessage.Text = "An error has occured while uploading your files:<br>" + ex.Message;

        //            }

        //        }

        //        if (isUploaded == arrFiles.Count)
        //        {

        //            lblMessage.Text = "Files uploaded successfully";

        //        }

        //        //Empty the arraylist and listbox once the upload process finishes

        //        arrFiles.Clear();

        //        lstFiles.Items.Clear();

        //    }
        //}

        #region RandomString
        private string RandomString(int length)
        {
            string strChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var cString = new char[length];
            string strFinalString = string.Empty;
            var random = new Random();

            for (int i = 0; i < cString.Length; i++)
            {
                cString[i] = strChars[random.Next(strChars.Length)];
            }
            strFinalString = new String(cString);


            return strFinalString;
        }
        #endregion

        #region IsDicomFile
        public static bool IsDicomFile(string strFileWithPath)
        {

            bool bRet = false;

            BinaryReader br = new BinaryReader(new FileStream(strFileWithPath, FileMode.Open, FileAccess.Read), Encoding.ASCII);

            byte[] preamble = new byte[132];

            br.Read(preamble, 0, 132);

            if (preamble[128] != 'D' || preamble[129] != 'I' || preamble[130] != 'C' || preamble[131] != 'M')
            {

                bRet = false;

            }

            else
            {
                bRet = true;

            }
            br.Close();

            return bRet;

        }
        #endregion
    }
}