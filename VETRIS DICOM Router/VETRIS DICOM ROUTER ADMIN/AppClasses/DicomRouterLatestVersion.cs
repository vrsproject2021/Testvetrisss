using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VETRIS_DICOM_ROUTER_ADMIN.AppClasses
{
    public class DicomRouterLatestVersionResponseStatus
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
    }
    public class DicomRouterLatestVersion
    {
        public DicomRouterLatestVersionResponseStatus responseStatus { get; set; }
        public string LatestVersion { get; set; }
    }
}
