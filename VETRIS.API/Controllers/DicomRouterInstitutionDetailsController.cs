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
    public class DicomRouterInstitutionDetailsController : ApiController
    {
        public DicomRouterInstitutionDetailsResult Post([FromBody]RequestObject.DicomRouterInstitutionDetails oReqInst)
        {
            DicomRouter oDR = new DicomRouter();
            ResponseStatus oRS = new ResponseStatus();
            DicomRouterInstitutionDetailsResult oRR = new DicomRouterInstitutionDetailsResult();
            bool bReturn = false;


            try
            {
                oDR.INSTITUTION_CODE = oReqInst.institutionCode.Trim();
                bReturn = oDR.FetchInstitutionDetails();

                if (bReturn)
                {
                    oRR.InstitutionName = oDR.INSTITUTION_NAME;
                    oRR.Address_1 = oDR.ADDRESS_1;
                    oRR.Address_2 = oDR.ADDRESS_2;
                    oRR.Zip = oDR.ZIP;
                    oRR.InstitutionLoginID = oDR.INSTITUTION_LOGIN_ID;
                    oRR.StudyImageFilesReceivingPath = oDR.STUDY_IMAGE_FILES_MANUAL_RECEIVING_PATH;
                    oRR.CompressFilesToTransfer = oDR.COMPRESS_DICOM_FILES_TO_TRANSFER;
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
