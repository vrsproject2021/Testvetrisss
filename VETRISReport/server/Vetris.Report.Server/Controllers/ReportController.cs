using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Reporting.NETCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Vetris.Report.Core.Configurations;
using Vetris.Report.Core.Extensions;
using Vetris.Report.Core.Models;
using Vetris.Report.Core.Models.Reports;
using Vetris.Report.Service.Datasets;
using Vetris.Report.Service.Reports;

namespace Vetris.Report.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : BaseApiController
    {
        private readonly IAppFolders AppFolders;
        private readonly IReportDataFetchService ReportDataFetchService;
        private readonly IReportingService ReportingService;
        private readonly IApplicationLifetime App;
        public ReportController(IConfiguration config, IAppFolders folders, IReportDataFetchService dataFetch, IReportingService reporting, IApplicationLifetime app) : base(config)
        {
            App = app;
            AppFolders = folders;
            ReportDataFetchService = dataFetch;
            ReportingService = reporting;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }
        
        [HttpPost]
        [Route("preview/pdf")]
        public async Task<FileDto> GeneratePreview(ReportPreviewGeneratingArg input)
        {
            var data = await ReportingService.PreparePreviewArguments(input);
            if (data.DatasetName.IsNullOrEmpty()) throw new Exception("Dataset missing");
            var dbparameters = new Dictionary<string, object>();
            if (data.Parameters.Count > 0)
            {
                foreach (var p in data.Parameters)
                {
                    dbparameters.Add($"@{p.ParameterName}", p.Value);
                }
            }
            var ds = ReportDataFetchService.GetReportData(data.CommandText, dbparameters);
 
            var parameters = new List<ReportParameter>();
            if (data.Parameters.Count > 0)
            {
                foreach (var p in data.Parameters)
                {
                    parameters.Add(new ReportParameter(p.ParameterName, p.PassedValue));
                }
            }

           
            try
            {
                var filename = $"{Guid.NewGuid().ToString("N")}.pdf";
                using (var fs = new FileStream(data.RDLC, FileMode.Open))
                {
                    LocalReport lr = new LocalReport();
                    lr.LoadReportDefinition(fs);
                    lr.DataSources.Add(new ReportDataSource(data.DatasetName, ds.Tables[0]));

                    lr.SetParameters(parameters);
                    var outStream = lr.Render("PDF");


                    outStream.SaveAsFile(Path.Combine(AppFolders.TempFileDownloadFolder, filename));
                    fs.Close();
                }
                System.IO.File.Delete(data.RDLC);
                return new FileDto
                {
                    FileName = filename,
                    FileToken = filename,
                    FileType = "application/pdf"
                };
            }
            catch (Exception e)
            {
                throw;
            }
            

        }

        [HttpPost]
        [Route("generate")]
        public async Task<FileDto> Generate(ReportGeneratingArg input)
        {
            if( !(new List<string> { "pdf", "xlsx", "docx", "html" }).Contains(input.RenderType.ToLower())){
                throw new Exception($"Render '{input.RenderType}' type not supported.");
            }
            var data = await ReportingService.PrepareReportArguments(input.Id, input.Parameters);
            if (data.DatasetName.IsNullOrEmpty()) throw new Exception("Dataset missing");
            var dbparameters = new Dictionary<string, object>();
            if (data.Parameters.Count > 0)
            {
                foreach (var p in data.Parameters)
                {
                    dbparameters.Add($"@{p.ParameterName}", p.Value);
                }
            }

            var ds = ReportDataFetchService.GetReportData(data.CommandText, dbparameters);

            var parameters = new List<ReportParameter>();
            if (data.Parameters.Count > 0)
            {
                foreach (var p in data.Parameters)
                {
                    parameters.Add(new ReportParameter(p.ParameterName, p.PassedValue));
                }
            }
            var render = "PDF";
            var mime = "application/pdf";
            switch (input.RenderType.ToLower())
            {
                case "pdf": render = "PDF"; mime = "application/pdf"; break;
                case "html": render = "HTML5"; mime = "text/html"; break;
                case "xlsx": render = "EXCELOPENXML"; mime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; break;
                case "docx": render = "WORDOPENXML"; mime = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; break;
            }
            try
            {
                var filename = $"{Guid.NewGuid().ToString("N")}.{input.RenderType.ToLower()}";
                using (var fs = new FileStream(data.RDLC, FileMode.Open))
                {
                    LocalReport lr = new LocalReport();
                    lr.LoadReportDefinition(fs);
                    lr.DataSources.Add(new ReportDataSource(data.DatasetName, ds.Tables[0]));

                    lr.SetParameters(parameters);

                    var outStream = lr.Render(render);


                    outStream.SaveAsFile(Path.Combine(AppFolders.TempFileDownloadFolder, filename));
                    fs.Close();
                }
                System.IO.File.Delete(data.RDLC);
                return new FileDto
                {
                    FileName = filename,
                    FileToken = filename,
                    FileType = mime
                };
            }
            catch (Exception e)
            {
                throw;
            }
        }
        [HttpGet]
        [Route("callstatement/{id:guid}")]
        public async Task<dynamic> GetCallStatement(Guid id)
        {
            return await ReportingService.GetForExecute(id);
        }

        [HttpGet]
        [Route("categories")]
        public async Task<dynamic> GetAllCategories()
        {
            return await ReportingService.GetCategories();
        }

        [HttpPost]
        [Route("getall")]
        public async Task<dynamic> GetAll([FromBody] ReportListingQueryArg input)
        {
            return await ReportingService.GetAll(input);
        }

        [HttpPost]
        [Route("create")]
        public async Task Create([FromBody] SysReportGetTempEditModelDto input)
        {
            await ReportingService.Create(input.ToCreateModel());
        }
        [HttpGet]
        //[AutoWrapIgnore]
        [Route("getforedit")]
        public async Task<dynamic> GetForEdit([FromQuery] Guid Id)
        {
            var data = await ReportingService.GetForEdit(Id);
            //data.ReportData = null;
            return data;
        }
        [HttpPost]
        
        [Route("save")]
        public async Task Save([FromBody] SysReportSaveModelDto input)
        {
            await ReportingService.Save(input);
        }

    }
}
