using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VETRIS.API.RequestObject
{
    public class DicomRouterCreateFileTransferNotification
    {
        private DateTime dtStart = DateTime.Now;
        private DateTime dtEnd = DateTime.Now;
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
            get { return dtStart; }
            set { dtStart = value; }
        }
        public DateTime downloadDate
        {
            get { return dtEnd; }
            set { dtEnd = value; }
        }
    }
}