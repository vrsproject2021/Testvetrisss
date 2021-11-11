using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vetris.Report.Core.Models.Reports
{
    public class GenerateFieldsWithParamArg
    {
        [Required]
        public string CommandText { get; set; }
        public List<ParamArg> Parameters { get; set; }
    }

    public class ParamArg
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string DataType { get; set; }
        public string Value { get; set; }
    }
}
