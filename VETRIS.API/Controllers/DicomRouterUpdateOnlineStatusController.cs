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


namespace VETRIS.API.Controllers
{
    public class DicomRouterUpdateOnlineStatusController : ApiController
    {
        public DicomRouterUpdateOnlineStatusResult Post([FromBody]RequestObject.DicomRouterUpdateOnlineStatus oReq)
        {
            DicomRouter oDR = new DicomRouter();
            ResponseStatus oRS = new ResponseStatus();
            DicomRouterUpdateOnlineStatusResult oRR = new DicomRouterUpdateOnlineStatusResult();
            bool bReturn = false;


            try
            {
                oDR.INSTITUTION_CODE = oReq.institutionCode.Trim();
                oDR.LATEST_VERSION = oReq.versionNo.Trim();
                bReturn = oDR.UpdateOnlineStatus();

               
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
