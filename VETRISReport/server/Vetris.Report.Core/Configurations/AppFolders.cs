using System;
using System.Collections.Generic;
using System.Text;
using Vetris.Report.Core.Dependency;

namespace Vetris.Report.Core.Configurations
{
    public class AppFolders : IAppFolders, ISingletonDependency
    {
        public string TempFileDownloadFolder { get; set; }
        public string WebLogsFolder { get; set; }
        public string ReportsFolder { get; set; }
        public string TempReportsFolder { get; set; }
        
    }
}
