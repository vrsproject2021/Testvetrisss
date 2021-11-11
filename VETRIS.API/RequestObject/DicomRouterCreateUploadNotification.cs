using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VETRIS.API.RequestObject
{
    public class DicomRouterCreateUploadNotification
    {
        private DateTime dt = DateTime.Now;
        private int fCount = 0;

        public string institutionCode { get; set; }
        public string importSessionID { get; set; }
        public int importFileCount
        {
            get { return fCount; }
            set { fCount = value; }
        }
        public DateTime uploadDate
        {
            get { return dt; }
            set { dt = value; }
        }
    }
}