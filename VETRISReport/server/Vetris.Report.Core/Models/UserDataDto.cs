using System;
using System.Collections.Generic;
using System.Text;

namespace Vetris.Report.Core.Models
{
    public class UserDataDto
    {
        public Guid id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string pacs_user_id { get; set; }
        public string pacs_password { get; set; }
    }
}
