using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VETRIS_DICOM_ROUTER_FILE_UPLOAD.AppClasses
{
    public class DicomRouterCreateDownloadNotificationResponseStatus
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
    }
    public class DicomRouterCreateDownloadNotification
    {
        public DicomRouterCreateDownloadNotificationResponseStatus responseStatus { get; set; }
    }
}
