using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VETRIS.API.ResponseObject
{
    public class ResponseStatus
    {
        bool respStat = false;
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
        public bool responseStatus { get { return respStat; } set { respStat = value; } }
    }
}