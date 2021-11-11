using AutoWrapper.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Vetris.Report.Core.Configurations;

namespace Vetris.Report.Server.Controllers
{
    public class FileController : Controller
    {
        private readonly IAppFolders AppFolders;
        public FileController(IAppFolders appFolders)
        {
            AppFolders = appFolders;
        }
        [HttpGet]
        [AutoWrapIgnore]
        public IActionResult DownloadTemporaryFile(string id, string contentType, string fileName)
        {
            var filepath = Path.Combine(AppFolders.TempFileDownloadFolder, id);
            var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose); 
            return File(fs, contentType, fileDownloadName: fileName);
        }
        [HttpGet]
        [AutoWrapIgnore]
        public IActionResult GetTemporaryFile(string id, string contentType)
        {
            var filepath = Path.Combine(AppFolders.TempFileDownloadFolder, id);
            var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose);
            return File(fs, contentType);
        }
    }
}
