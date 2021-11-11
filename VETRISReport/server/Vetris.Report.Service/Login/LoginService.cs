using Newtonsoft.Json;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Vetris.Report.Core.Configurations;
using Vetris.Report.Core.Helper;
using Vetris.Report.Core.Models;
using Vetris.Report.Core.Session;
using Vetris.Report.DataAccess;

namespace Vetris.Report.Service.Login
{
    public class LoginService : ApplicationService, ILoginService
    {
        private readonly IConfigurationAccessor _configuration;
        public LoginService(IDatabaseContext database, ISessionInfo session, IConfigurationAccessor configuration) : base(database,session)
        {
            _configuration = configuration;
        }

        public TokenResponse ValidateLogin(string username, string plainpassword)
        {
            Guid id = Guid.Empty;
            int returnStatus = 0;
            string error_code = "", code = "", name = "";
            var ret = _db.StoredProcedure("login_validate")
                            .AddParameter("@login_id", username, SqlDbType.NVarChar, 100)
                            .AddParameter("@password", EncryptionHelper.EncryptString(plainpassword), SqlDbType.NVarChar, 200)
                            .AddParameter("@id", id, SqlDbType.UniqueIdentifier, ParameterDirection.Output)
                            .AddParameter("@code", code, SqlDbType.NVarChar, 5, ParameterDirection.Output)
                            .AddParameter("@name", name, SqlDbType.NVarChar, 100, ParameterDirection.Output)
                            .AddParameter("@error_code", error_code, SqlDbType.NVarChar, ParameterDirection.Output)
                            .AddParameter("@return_status", returnStatus, SqlDbType.Int)
                            .ExecuteNonQuery(c =>
                            {
                                try
                                {
                                    returnStatus = Convert.ToInt32(c.Parameters["@return_status"].Value);
                                    error_code = Convert.ToString(c.Parameters["@error_code"].Value);
                                    name = Convert.ToString(c.Parameters["@name"].Value);
                                    code = Convert.ToString(c.Parameters["@code"].Value);
                                    id = (Guid)c.Parameters["@id"].Value;
                                }
                                catch { }
                            });
            if (id!=Guid.Empty)
            {
                var expiry = DateTime.UtcNow.AddDays(30);
                return new TokenResponse
                {
                    AccessToken = GenerateToken(new LoginResponse { Code = code, Name = name, Id = id, ExpiryDate=expiry }),
                    Name=name,
                    ExpiryDate=expiry,
                    Success = true
                };
            }
            throw new Exception("Invalid Login");
           
        }
        public TokenResponse GetLoginByUserId(Guid userid)
        {
            _db.OpenConnection();
            var data = _db.QueryFactory()
                    .Query("users")
                    .Where("id", userid)
                    .Select("id as Id", "name as Name", "code as Code")
                    .Get<LoginResponse>().FirstOrDefault();
            if (data != null)
            {
                data.ExpiryDate= DateTime.UtcNow.AddDays(30);
                return new TokenResponse
                {
                    AccessToken = GenerateToken(data),
                    Name = data.Name,
                    ExpiryDate = data.ExpiryDate.Value,
                    Success = true
                };
            }

            throw new Exception("Invalid Login");
        }

        public string GetViewerUrl(string accno, string patient)
        {
            _db.OpenConnection();
            var pacs_user = _db.QueryFactory()
                    .Query("general_settings")
                    .Where("control_code", "WS8SRVUID")
                    .Select("data_type_string as value")
                    .Get<string>().FirstOrDefault();
            var pacs_password = _db.QueryFactory()
                    .Query("general_settings")
                    .Where("control_code", "WS8SRVPWD")
                    .Select("data_type_string as value")
                    .Get<string>().FirstOrDefault();
            if (!string.IsNullOrEmpty(pacs_password)) pacs_password = EncryptionHelper.DecryptString(pacs_password);
            var pacs_imageviewerurl = _db.QueryFactory()
                    .Query("general_settings")
                    .Where("control_code", "WS8IMGVWRURL")
                    .Select("data_type_string as value")
                    .Get<string>().FirstOrDefault();
            pacs_imageviewerurl = pacs_imageviewerurl.Replace("#V1", accno);
            pacs_imageviewerurl = pacs_imageviewerurl.Replace("#V2", patient);
            pacs_imageviewerurl = pacs_imageviewerurl.Replace("#V3", "");
            pacs_imageviewerurl = pacs_imageviewerurl.Replace("#V4", pacs_user);
            pacs_imageviewerurl = pacs_imageviewerurl.Replace("#V5", pacs_password);

            return pacs_imageviewerurl;
        }

        public async Task<ReportResponse> GetReportUrl(string id, string name, string type)
        {
            string url = _configuration.Get("App:Vetris")+$@"/caselist/docprint/docprintwebservice.asmx/GetReportDocument?" +
                    $"id={id}&"+
                    $"patientName={name}&" +
                    $"UserId={SessionInfo.UserId.ToString()}&" +
                    $"type={type}&" +
                    $"direct=Y&";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept","application/json");
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<ReportResponse>((await response.Content.ReadAsStringAsync()));
            if (result.Path != null) result.Path = "caselist/docprint/" + result.Path;
            return result;
        }

        public async Task<ReportResponse> GetInvoiceUrl(string cycleId, string accountId)
        {
            string url = _configuration.Get("App:Vetris") + $@"/Invoicing/DocumentPrinting/InvoiceDocPrintWebService.asmx/GetReportDocument?" +
                    $"cycleId={cycleId}&" +
                    $"accountId={accountId}&" +
                    $"UserId={SessionInfo.UserId.ToString()}";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<ReportResponse>((await response.Content.ReadAsStringAsync()));

            return result;
        }
        public string GenerateToken(LoginResponse input)
        {
            
            var data = new TokenInfo { UserId=input.Id.Value, Name = input.Name, ExpiryDate=input.ExpiryDate.Value };
            return EncryptionHelper.EncryptString(JsonConvert.SerializeObject(data), "TX012MMZt");
        }

        public TokenInfo ParseToken(string token)
        {
            return JsonConvert.DeserializeObject<TokenInfo>(EncryptionHelper.DecryptString(token, "TX012MMZt"));
        }
    }
}
