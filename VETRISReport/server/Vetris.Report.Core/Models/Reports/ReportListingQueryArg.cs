using System;
using System.Collections.Generic;
using System.Text;

namespace Vetris.Report.Core.Models.Reports
{
    public class ReportListingQueryArg
    {
        public string Category { get; set; }
        public string Search { get; set; }

        public string SortDirection { get; set; }
    }
}
