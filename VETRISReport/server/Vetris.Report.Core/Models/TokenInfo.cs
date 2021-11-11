using System;
using System.Collections.Generic;
using System.Text;

namespace Vetris.Report.Core.Models
{
    public class TokenInfo
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
