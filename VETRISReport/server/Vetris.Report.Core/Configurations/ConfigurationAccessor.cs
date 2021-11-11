
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vetris.Report.Core.Configurations
{
    public class ConfigurationAccessor : IConfigurationAccessor
    {
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ConfigurationAccessor(IHostingEnvironment env)
        {
            _hostingEnvironment = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public string Get(string key) => _appConfiguration[key];
        public string ConnectionString => _appConfiguration["ConnectionStrings:Default"];
        
    }
}
