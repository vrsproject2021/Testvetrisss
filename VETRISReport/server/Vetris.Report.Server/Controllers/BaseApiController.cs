using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vetris.Report.Server.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected IConfiguration _config;
        protected ILog Logger;
        public BaseApiController(IConfiguration config)
        {
            _config = config;
            Logger = LogManager.GetLogger("Log");
        }
    }
}
