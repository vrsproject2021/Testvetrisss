using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VETRIS.API.ResponseObject
{
    public class DicomRouterInstitutionDetailsResult
    {
        public ResponseStatus responseStatus { get; set; }
        public string InstitutionName { get; set; }
        public string Address_1 { get; set; }
        public string Address_2 { get; set; }
        public string Zip { get; set; }
        public string InstitutionLoginID { get; set; }
        public string StudyImageFilesReceivingPath { get; set; }
        public string CompressFilesToTransfer { get; set; }
    }
}