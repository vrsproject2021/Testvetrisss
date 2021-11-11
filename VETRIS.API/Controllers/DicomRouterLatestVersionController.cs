using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VETRIS.API.RequestObject;
using VETRIS.API.ResponseObject;
using VETRIS.API.Models;
//using VETRIS.API.Helper;

namespace VETRIS.API.Controllers
{
    public class DicomRouterLatestVersionController : ApiController
    {

        public DicomRouterLatestVersionResult Post([FromBody]RequestObject.DicomRouterLatestVersion oReqDRLV)
        {
            DicomRouter oDR = new DicomRouter();
            ResponseStatus oRS = new ResponseStatus();
            DicomRouterLatestVersionResult oRR = new DicomRouterLatestVersionResult();
            bool bReturn = false;
           

            try
            {

                bReturn = oDR.GetLatestVersion();

                if (bReturn)
                {
                    oRR.LatestVersion = oDR.LATEST_VERSION;
                   
                }
                oRS.responseCode = string.Empty;
                oRS.responseMessage = oDR.RESPONSE_MESSAGE;
                oRR.responseStatus = oRS;
                


               
            }
            catch (Exception expErr)
            {
                oRS.responseCode = "ERR";
                oRS.responseMessage = expErr.Message;
                oRR.responseStatus = oRS;
               
            }
            finally
            {
                oDR = null; oRS = null;
            }

            return oRR;
        }

    }
}
