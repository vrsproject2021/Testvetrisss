using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Collections;
using System.Text;
using VETRIS.Core;

namespace VETRIS.CaseList
{
    /// <summary>
    /// Summary description for ImageUploadHandler
    /// </summary>
    public class ImageUploadHandler : IHttpHandler
    {
        classes.CommonClass objComm;

        public void ProcessRequest(HttpContext context)
        {
            string strRandom = string.Empty;
            string strRandomPrev = string.Empty;
            var data = context.Request;
            string pathToUpload = data.Params[0] + "/" + data.Params[1];
            string strFileWithPath = string.Empty; var strFileContentType = string.Empty;
            string strFileName = string.Empty;
            string strFileList = string.Empty;

            HttpFileCollection files = context.Request.Files;
            objComm = new classes.CommonClass();
            pathToUpload = pathToUpload.Replace("\\", "/");

            if (!Directory.Exists(pathToUpload)) { Directory.CreateDirectory(pathToUpload); }

            foreach (string key in files)
            {
                HttpPostedFile file = files[key];
                strFileContentType = file.ContentType.Trim();

                if ((strFileContentType == "image/pjpeg") || (strFileContentType == "image/jpeg") || (strFileContentType == "image/x-png") || (strFileContentType == "image/png") || (strFileContentType == "image/gif") || (strFileContentType == "image/bmp"))
                {
                   
                    strFileName = file.FileName;
                    strFileName = strFileName.Replace("\\", "/");
                    if (strFileName.Contains("/"))
                    {
                        strFileName = strFileName.Substring(strFileName.LastIndexOf("/") + 1, (strFileName.Length - (strFileName.LastIndexOf("/") + 1)));
                        while (strRandom == strRandomPrev) { strRandom = RandomString(6); }
                        strFileName = strRandom + "_" + strFileName;
                    }
                    strRandomPrev = strRandom;
                    strFileWithPath = pathToUpload + "/" + strFileName;
                    file.SaveAs(strFileWithPath);
                    strFileWithPath = strFileWithPath.Replace("\\", "/");
                   
                    if (strFileList.Trim() != string.Empty) strFileList += objComm.RecordDivider;
                    strFileList += strFileWithPath;
                    //strRandom = string.Empty;
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
    }
}