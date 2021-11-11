using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VETRIS.API.ResponseObject
{
    public class DicomRouterLatestVersionResult
    {
        public ResponseStatus responseStatus { get; set; }
        public string LatestVersion { get; set; }
    }
}