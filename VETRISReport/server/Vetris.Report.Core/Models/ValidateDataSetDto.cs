using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vetris.Report.Core.Models
{
    public class ValidateDataSetDto
    {
        [Required]
        [MinLength(5)]
        public string Query { get; set; }
    }
}
