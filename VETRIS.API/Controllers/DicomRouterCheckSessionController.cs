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
    public class DicomRouterCheckSessionController : ApiController
    {
        public DicomRouterCheckSessionResult Post([FromBody]RequestObject.DicomRouterCheckSession oReq)
        {
            DicomRouter oDR = new DicomRouter();
            ResponseStatus oRS = new ResponseStatus();
            DicomRouterCheckSessionResult oRR = new DicomRouterCheckSessionResult();
            bool bReturn = false;


            try
            {
                oDR.INSTITUTION_CODE = oReq.institutionCode.Trim();
                oDR.IMPORT_SESSION_ID = oReq.importSessionID.Trim();
                bReturn = oDR.CheckImportSession();

                if (bReturn)
                {
                    oRR.ImportedFileCount = oDR.IMPORTED_FILE_COUNT;
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
