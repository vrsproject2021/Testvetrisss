using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VETRIS.RND
{
    /// <summary>
    /// Summary description for FileHandler
    /// </summary>
    public class FileHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Request.Files.Count > 0)
                {
                    HttpFileCollection files = context.Request.Files;

                    foreach (string key in files)
                    {
                        HttpPostedFile file = files[key];
                        string fileName = file.FileName;
                        fileName = context.Server.MapPath("~/RND/UploadedFiles/" + fileName);
                        file.SaveAs(fileName);
                    }
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write("File(s) uploaded successfully!");
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(ex.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}