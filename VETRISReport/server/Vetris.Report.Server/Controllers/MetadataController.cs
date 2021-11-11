
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
using Vetris.Report.Core.Models.Reports;
using Vetris.Report.Service.Datasets;

namespace Vetris.Report.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MetadataController : BaseApiController
    {
        public readonly IReportMetadataService ReportMetadataService;
        public MetadataController(IConfiguration config, IReportMetadataService service) :base(config)
        {
            ReportMetadataService = service;
        }
        [HttpGet]
        [Route("columns/{id}")]
        public dynamic Columns(string id)
        {
            return ReportMetadataService.Columns(new Guid(id));
        }

        [HttpPost]
        [Route("createorupdateview")]
        public bool CreateOrModifyView([FromBody] CreateViewInputDto input)
        {
            return ReportMetadataService.CreateOrModifyView(input);
        }

        [HttpGet]
        [Route("datasets")]
        public dynamic GetDataset()
        {
            return ReportMetadataService.GetDatasets();
        }
        [HttpGet]
        [Route("dataset")]
        public dynamic GetDataset([FromQuery] string id)
        {
            return ReportMetadataService.GetDataset(new Guid(id));
        }

        [HttpPost]
        [Route("validate")]
        public dynamic Validate([FromBody] ValidateDataSetDto input)
        {
            return ReportMetadataService.ValidateDatasetQuery(input.Query);
        }

        [HttpPost]
        [Route("generatefields")]
        public dynamic GenerateFields([FromBody] SQLText input)
        {
            return ReportMetadataService.GetFields(input.CommandText);
        }

        [HttpPost]
        [Route("generatefieldswithparameters")]
        public dynamic GenerateFieldsWithParameter([FromBody] GenerateFieldsWithParamArg input)
        {
            return ReportMetadataService.GetFields(input.CommandText, input.Parameters);
        }
        [HttpGet]
        [Route("reportfonts")]
        public dynamic GetFonts()
        {
            return ((ReportMetadataService)ReportMetadataService).GetFonts();
        }
    }
}
