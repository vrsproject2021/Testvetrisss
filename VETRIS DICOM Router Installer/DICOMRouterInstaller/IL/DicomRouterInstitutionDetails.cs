using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMRouterInstaller.IL
{
    public class DicomRouterLatestVersionResponseStatus
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
    }

    public class DicomRouterInstitutionDetails
    {
        public DicomRouterLatestVersionResponseStatus responseStatus { get; set; }
        public string InstitutionName { get; set; }
        public string Address_1 { get; set; }
        public string Address_2 { get; set; }
        public string Zip { get; set; }
        public string InstitutionLoginID { get; set; }
        public string StudyImageFilesReceivingPath { get; set; }
        public string CompressFilesToTransfer { get; set; }
    }
}
