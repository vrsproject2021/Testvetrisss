using System;
using System.Collections.Generic;
using System.Text;

namespace Vetris.Report.Core.Configurations
{
    public interface IAppFolders
    {
        
        string TempFileDownloadFolder { get; set; }
        string WebLogsFolder { get; set; }
        string ReportsFolder { get; set; }
        string TempReportsFolder { get; set; }
    }
}
