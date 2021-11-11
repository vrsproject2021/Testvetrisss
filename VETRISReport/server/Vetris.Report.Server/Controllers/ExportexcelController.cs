
using Microsoft.AspNetCore.Authorization;
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
using Vetris.Report.Service.Excel;

namespace Vetris.Report.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExportexcelController : BaseApiController
    {
        public readonly IDataToExcelExporter DataToExcelExporter;
        public ExportexcelController(IConfiguration config, IDataToExcelExporter service) :base(config)
        {
            DataToExcelExporter = service;
        }

        [HttpPost]
        public FileDto ExportData([FromBody] QueryDto query)
        {
            return DataToExcelExporter.ExportToFile(query);
        }

       
    }
}
