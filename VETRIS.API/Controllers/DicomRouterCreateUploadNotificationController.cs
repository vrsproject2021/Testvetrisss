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
    public class DicomRouterCreateUploadNotificationController : ApiController
    {
        public DicomRouterCreateUploadNotificationResult Post([FromBody]RequestObject.DicomRouterCreateUploadNotification oReq)
        {
            DicomRouter oDR = new DicomRouter();
            ResponseStatus oRS = new ResponseStatus();
            DicomRouterCreateUploadNotificationResult oRR = new DicomRouterCreateUploadNotificationResult();
            bool bReturn = false;

            try
            {
                oRS.responseMessage = "1 " ;
                oDR.INSTITUTION_CODE = oReq.institutionCode.Trim();
                oDR.IMPORT_SESSION_ID = oReq.importSessionID.Trim();
                oDR.IMPORTED_FILE_COUNT = oReq.importFileCount;
                oDR.DATE = oReq.uploadDate;
               
                bReturn = oDR.CreateFileUploadNotification();

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
