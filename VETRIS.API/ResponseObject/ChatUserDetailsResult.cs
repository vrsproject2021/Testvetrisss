using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VETRIS.API.ResponseObject
{
    public class ChatUserDetailsResult
    {
        public ResponseStatus responseStatus { get; set; }
        public string UserRoleCode { get; set; }
        public string UserRoleName { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string ContactNumber { get; set; }
    }
}