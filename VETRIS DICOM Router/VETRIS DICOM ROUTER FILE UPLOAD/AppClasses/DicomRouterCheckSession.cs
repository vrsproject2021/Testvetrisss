﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VETRIS_DICOM_ROUTER_FILE_UPLOAD.AppClasses
{
    public class DicomRouterCheckSessionResponseStatus
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
    }
    public class DicomRouterCheckSession
    {
        public DicomRouterCheckSessionResponseStatus responseStatus { get; set; }
        public int ImportedFileCount { get; set; }
    }
}
