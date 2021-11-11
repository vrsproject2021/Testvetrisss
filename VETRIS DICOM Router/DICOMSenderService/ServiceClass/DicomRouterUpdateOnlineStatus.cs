using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMSenderService.ServiceClass
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
