<%@ WebHandler Language="C#" Class="upload" %>

using System;
using System.Web;
using System.Configuration;

public class upload : IHttpHandler 
{
    
    public void ProcessRequest (HttpContext context) 
    {
        string CKEditorFuncNum = context.Request["CKEditorFuncNum"];
        string CKEditor = context.Request["CKEditor"];
        string langCode = context.Request["langCode"];
        string url; // url to return 
        string message; // message to display (optional) 
        try
        {


            //no files upload just send an alert script 
            if (context.Request.Files.Count == 0)
            {
                context.Response.Write("<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"\", \"No file upload. Upload cancelled\");</script></body></html>");
                context.Response.End();
            }

            //get our file from the context. 
            HttpPostedFile upload = context.Request.Files[0];

            // here logic to upload image 
            // and get file path of the image 
            // path of the image 
            string path = upload.FileName;
            path = path.Substring(path.LastIndexOf("\\")+1);

            //create your URL path 
            string LsContentImgPath = ConfigurationManager.AppSettings["ContentImgPath"];
            url = context.Request.Url.GetLeftPart(UriPartial.Authority) + LsContentImgPath + path;

            //create our absolute store path 
            var savePath = context.Server.MapPath(string.Format("~/Parameters/images/{0}" , path));

            //save the upload 
            upload.SaveAs(savePath);

            // passing message success/failure 
            message = "Image was saved correctly";
            // since it is an ajax request it requires this string 
            string output = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + url + "\", \"" + message + "\");</script></body></html>";

            //write out our output 
            context.Response.Write(output);
        }
        catch
        {
            context.Response.Write("<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"\", \"An Error occurred while uploading your file. Upload cancelled\");</script></body></html>");

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