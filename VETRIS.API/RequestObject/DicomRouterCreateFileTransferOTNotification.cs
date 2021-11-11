using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VETRIS.API.RequestObject
{
    public class DicomRouterCreateFileTransferOTNotification
    {
        private DateTime dt = DateTime.Now;
        private int impfCount = 0;
        private int transfCount = 0;
        private int timeTaken = 0;

        public string institutionCode { get; set; }
        public string importSessionID { get; set; }
        public int importFileCount
        {
            get { return impfCount; }
            set { impfCount = value; }
        }
        public int transferFileCount
        {
            get { return transfCount; }
            set { transfCount = value; }
        }
        public DateTime uploadDate
        {
            get { return dt; }
            set { dt = value; }
        }
        public int timeTakenInMinutes
        {
            get { return timeTaken; }
            set { timeTaken = value; }
        }
    }
}