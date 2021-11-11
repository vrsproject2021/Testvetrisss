using System;
using System.Collections.Generic;
using System.Text;

namespace Vetris.Report.Core.Models
{
    public class TokenResponse
    {
        public string Name { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Success { get; set; }
    }
}
