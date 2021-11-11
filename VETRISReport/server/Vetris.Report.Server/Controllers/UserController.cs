
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vetris.Report.Core.Models;
using Vetris.Report.Core.Models.Metadatas;
using Vetris.Report.Core.Models.Queries;
using Vetris.Report.Service.Datasets;
using Vetris.Report.Service.Login;

namespace Vetris.Report.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        public readonly ILoginService LoginService;
        public UserController(IConfiguration config, ILoginService service) :base(config)
        {
            LoginService = service;
        }

        [HttpPost]
        [Route("login")]
        public TokenResponse Login([FromBody] LoginDto input)
        {
            return LoginService.ValidateLogin(input.UserId, input.Password);
            
        }

        [HttpGet]
        [Route("loginstatus")]
        public TokenResponse LoginStatus([FromQuery] Guid userId)
        {
            return LoginService.GetLoginByUserId(userId);
            
        }

        [HttpGet]
        [Route("getviewer")]
        public string GetViewer([FromQuery] string accno, [FromQuery] string patient)
        {
            return LoginService.GetViewerUrl(accno, patient);
        }
        [HttpGet]
        [Route("getreport")]
        public async Task<ReportResponse> GetReport([FromQuery] string id, [FromQuery] string patient, [FromQuery] string type)
        {
            return await LoginService.GetReportUrl(id,patient, type);
        }
        [HttpGet]
        [Route("getinvoice")]
        public async Task<ReportResponse> GetInvoice([FromQuery] string cycleId, [FromQuery] string accountId)
        {
            return await LoginService.GetInvoiceUrl(cycleId, accountId);
        }
    }
}
