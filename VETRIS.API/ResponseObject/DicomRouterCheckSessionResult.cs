using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VETRIS.API.ResponseObject
{
    public class DicomRouterCheckSessionResult
    {
        public ResponseStatus responseStatus { get; set; }
        public int ImportedFileCount { get; set; }
    }
}