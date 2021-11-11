using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VETRIS.API.RequestObject
{
    public class DicomRouterCreateDownloadNotification
    {
        public string institutionCode { get; set; }
        public string importSessionID { get; set; }
        public int importFileCount { get; set; }
        public DateTime downloadDate { get; set; }
    }
}