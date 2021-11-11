
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vetris.Report.Core.Models.Metadatas;
using Vetris.Report.Core.Models.Queries;
using Vetris.Report.Service.Datasets;

namespace Vetris.Report.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FetchController : BaseApiController
    {
        public readonly IReportDataFetchService ReportDataFetchService;
        public FetchController(IConfiguration config, IReportDataFetchService service) :base(config)
        {
            ReportDataFetchService = service;
        }

        [HttpPost]
        [Route("data")]
        public dynamic Data([FromBody] QueryDto query)
        {
            return ReportDataFetchService.FetchData(query);
        }

       
    }
}
