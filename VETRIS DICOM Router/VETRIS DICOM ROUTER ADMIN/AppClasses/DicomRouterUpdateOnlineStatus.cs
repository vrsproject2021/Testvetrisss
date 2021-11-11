using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VETRIS_DICOM_ROUTER_ADMIN.AppClasses
{
    public class DicomRouterOnlineStatusResponseStatus
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
    }
    public class DicomRouterOnlineStatusResponseDetails
    {
        public DicomRouterOnlineStatusResponseStatus responseStatus { get; set; }
    }
}
