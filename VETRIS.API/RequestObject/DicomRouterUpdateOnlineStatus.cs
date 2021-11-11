using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VETRIS.API.RequestObject
{
    public class DicomRouterUpdateOnlineStatus
    {
        public string institutionCode { get; set; }
        public string versionNo { get; set; }
    }
}