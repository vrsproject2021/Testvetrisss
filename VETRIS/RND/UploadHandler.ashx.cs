using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Collections;
using System.Text;

namespace VETRIS.RND
{
    /// <summary>
    /// Summary description for UploadHandler
    /// </summary>
    public class UploadHandler : IHttpHandler
    {
        classes.CommonClass objComm;
        
        public void ProcessRequest(HttpContext context)
        {
            
            string strRandom = string.Empty;
            var data = context.Request;
            //string pathToUpload = HttpContext.Current.Server.MapPath("UploadedFiles");
            string pathToUpload = data.Params[0] + "/" + data.Params[1];
            string strFileWithPath = string.Empty;  
            int intIdx = 0;
            string strFileName = string.Empty;
            string strFileList = string.Empty;
            string[] arrFiles = new string[0];
            

            HttpFileCollection files = context.Request.Files;
            arrFiles = new string[files.Count];
            objComm = new classes.CommonClass();


            foreach (string key in files)
            {
                HttpPostedFile file = files[key];
                strRandom = string.Empty;
                strFileName = file.FileName;
                strFileName = strFileName.Replace("\\", "/");
                if (strFileName.Contains("/"))
                {
                    strFileName = strFileName.Substring(strFileName.LastIndexOf("/") + 1, (strFileName.Length - (strFileName.LastIndexOf("/") + 1)));
                }
                strRandom = RandomString(6);
                strFileName = strRandom + "_" + strFileName;
                if (!Directory.Exists(pathToUpload)) { Directory.CreateDirectory(pathToUpload); }

                strFileWithPath = pathToUpload + "/" + strFileName;
                //strFileWithPath = strFileWithPath.Replace("\\", "/");
                //fileName = context.Server.MapPath("~/UploadedFiles/" + strRandom + "_" + fileName);
                file.SaveAs(strFileWithPath);
                strFileWithPath = strFileWithPath.Replace("\\", "/");
                arrFiles[intIdx] = strFileWithPath;
                intIdx = intIdx + 1;
               
            }

            for (int i = 0; i < arrFiles.Length; i++)
            {
                if (IsDicomFile(arrFiles[i]))
                {
                    if (strFileList.Trim() != string.Empty) strFileList += objComm.RecordDivider;
                    strFileList += arrFiles[i];
                    //intValidFlag = 1;
                }
                else
                {
                    string[] arrTmp = new string[0];
                    arrTmp = arrFiles[i].Split('/');

                    //divMsg.InnerHtml += "Format of the file " + arrTmp[arrTmp.Length - 1] + " is not supported. Discarded<br/>";
                    //intValidFlag = 0;
                    if (File.Exists(arrFiles[i])) File.Delete(arrFiles[i]);

                }
            }


            context.Response.ContentType = "text/plain";
            context.Response.Write(strFileList);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

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