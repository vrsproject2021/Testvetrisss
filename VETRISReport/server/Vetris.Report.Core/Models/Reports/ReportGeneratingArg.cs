using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vetris.Report.Core.Models.Reports
{
    public class ReportGeneratingArg
    {
        public Guid Id { get; set; }
        public string RenderType { get; set; }
        public List<ParameterInput> Parameters { get; set; }
    }

    public class ReportPreviewGeneratingArg
    {
        public string Report { get; set; }
    }

}
