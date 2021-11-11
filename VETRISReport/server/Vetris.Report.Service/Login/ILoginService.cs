using System;
using System.Threading.Tasks;
using Vetris.Report.Core.Dependency;
using Vetris.Report.Core.Models;

namespace Vetris.Report.Service.Login
{
    public interface ILoginService : ITransientDependency
    {
        TokenResponse ValidateLogin(string username, string plainpassword);
        TokenResponse GetLoginByUserId(Guid userid);
        string GetViewerUrl(string accno, string patient);
        string GenerateToken(LoginResponse input);
        Task<ReportResponse> GetReportUrl(string id, string name, string type);
        Task<ReportResponse> GetInvoiceUrl(string cycleId, string accountId);
        TokenInfo ParseToken(string token);
    }
}