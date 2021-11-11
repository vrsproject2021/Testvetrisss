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
    /// Summary description for DICOMUploadHandler
    /// </summary>
    public class DICOMUploadHandler : IHttpHandler
    {
        classes.CommonClass objComm;

        public void ProcessRequest(HttpContext context)
        {
            string strRandom = string.Empty;
            string strRandomPrev = string.Empty;
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
            pathToUpload = pathToUpload.Replace("\\", "/");

            if (!Directory.Exists(pathToUpload)) { Directory.CreateDirectory(pathToUpload); }

            foreach (string key in files)
            {
                HttpPostedFile file = files[key];
               
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