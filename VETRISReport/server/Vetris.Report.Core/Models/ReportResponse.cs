using System;
using System.Collections.Generic;
using System.Text;

namespace Vetris.Report.Core.Models
{
    public class ReportResponse
    {
        public string Path { get; set; }
        public string HasError { get; set; }
        public string ErrorMessage { get; set; }
    }
}
