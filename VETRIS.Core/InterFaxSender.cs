using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Net.Http.Headers;

namespace VETRIS.Core
{
    public class InterFaxSender
    {
        #region Variables
        public string base_url = string.Empty;
        private string csid;
        private string contact;
        private string reference;
        private int? retriesToPerform;
        private string replyAddress;

        private string usersername;
        private string password;
        private List<string> Files;
        #endregion

        #region Property
        public InterFaxSender URL(string value)
        {
            base_url = value;
            return this;
        }
        public InterFaxSender Authorize(string username, string password)
        {
            this.usersername = username;
            this.password = password;
            return this;
        }
        public InterFaxSender CSID(string value)
        {
            csid = value;
            return this;
        }
        public InterFaxSender Contact(string value)
        {
            contact = value;
            return this;
        }
        public InterFaxSender Reference(string value)
        {
            reference = value;
            return this;
        }
        public InterFaxSender RetriesToPerform(int value)
        {
            retriesToPerform = value;
            return this;
        }
        public InterFaxSender ReplyAddress(string value)
        {
            replyAddress = value;
            return this;
        }
        public InterFaxSender AddFiles(params string[] files)
        {
            if (files.Length > 0)
            {
                if (Files == null) Files = new List<string>();
                files.ToList().ForEach(file =>
                {
                    if (!Files.Any(i => i.ToLower() == file.ToLower()))
                    {
                        Files.Add(file);
                    }
                });
            }
            return this;
        }
        #endregion

        #region Send
        public bool Send(string faxno,ref string ReturnStatus,ref string CatchMessage)
        {
            bool bRet = false;
            //var url = string.Format("{0}/outbound/faxes", BASE_URL);
            var url = string.Format("{0}/outbound/faxes", base_url);
            //?faxNumber={FAXNUMBER}&contact={CONTACT}&csid={CSID}
            //&reference={REFERENCE}&resolution={RESOLUTION}&rendering={RENDERING}
            //&pageOrientation={PAGEORIENTATION}&pageSize={PAGESIZE}&fitToPage={FITTOPAGE}
            //&retriesToPerform={RETRIESTOPERFORM}&postponeTime={POSTPONETIME}&pageHeader={PAGEHEADER}
            //&barcode={BARCODE}&replyAddress={REPLYADDRESS}
            UriBuilder builder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["faxNumber"] = faxno;
            query["csid"] = csid;
            query["contact"] = contact;
            query["reference"] = reference;
            if (retriesToPerform > 0)
                query["retriesToPerform"] = retriesToPerform.Value.ToString();
            query["replyAddress"] = replyAddress;
            query["rendering"] = "grayscale";
            builder.Query = query.ToString();

            try
            {
                using (var client = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    MultipartFormDataContent content = new MultipartFormDataContent();
                    if (Files.Count > 0)
                    {
                        var index = 1;
                        foreach (var f in Files)
                        {
                            FileStream fs = File.OpenRead(f);
                            var stream = new StreamContent(fs);
                            if (f.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase))
                                stream.Headers.Add("Content-Type", "application/pdf");
                            else if (f.EndsWith(".docx", StringComparison.InvariantCultureIgnoreCase))
                                stream.Headers.Add("Content-Type", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                            else if (f.EndsWith(".txt", StringComparison.InvariantCultureIgnoreCase))
                                stream.Headers.Add("Content-Type", "text/plaint");
                            else
                                stream.Headers.Add("Content-Type", "application/octet-stream");
                            content.Add(stream, string.Format("file{0}", index++), Path.GetFileName(f));
                        }
                    }
                    var authToken = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", usersername, password));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));
                    var result = client.PostAsync(builder.Uri.AbsoluteUri, content).Result;
                    if (!result.IsSuccessStatusCode)
                    {
                        CatchMessage = result.ReasonPhrase;
                        bRet = false;
                    }
                    else
                        bRet = true;

                    var response = result.Content.ReadAsStringAsync().Result;
                    ReturnStatus = response;
                }
            }
            catch (Exception ex)
            {
                CatchMessage = ex.Message;
            }
            return bRet;
        } 
        #endregion

    }
}
