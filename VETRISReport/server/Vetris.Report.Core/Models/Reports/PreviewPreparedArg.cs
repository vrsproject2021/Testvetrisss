using System;
using System.Collections.Generic;
using System.Text;

namespace Vetris.Report.Core.Models.Reports
{
    public class PreviewPreparedArg
    {
        public string DatasetName { get; set; }
        public string CommandText { get; set; }
        public string RDLC { get; set; }
        public List<ParameterInput> Parameters { get; set; }
        public List<FilterInput> Filters { get; set; }
    }

    public class ParameterInput
    {
        public string ParameterName { get; set; }
        public object Value { get; set; }
        public string PassedValue { get; set; }

    }
    public class FilterInput
    {
        public string Column { get; set; }
        public string Operator { get; set; }
        public string ParameterName1 { get; set; }
        public string ParameterName2 { get; set; }
    }
}
