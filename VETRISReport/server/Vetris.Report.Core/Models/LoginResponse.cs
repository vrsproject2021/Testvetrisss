using System;
using System.Collections.Generic;
using System.Text;

namespace Vetris.Report.Core.Models
{
    public class LoginResponse
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool Success { get; set; }
        public int ReturnStatus { get; set; }
        public string ErrorCode { get; set; }
    }
}
