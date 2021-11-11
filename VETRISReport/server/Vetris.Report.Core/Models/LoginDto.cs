using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vetris.Report.Core.Models
{
    public class LoginDto
    {
        [Required]
        [MaxLength(100)]
        public string UserId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Password { get; set; }
    }
}
