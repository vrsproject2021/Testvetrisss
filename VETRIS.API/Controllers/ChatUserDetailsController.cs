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
    public class ChatUserDetailsController : ApiController
    {
        public ChatUserDetailsResult Post([FromBody]RequestObject.ChatUserDetails oReqInst)
        {
            Chat oDR = new Chat();
            ResponseStatus oRS = new ResponseStatus();
            ChatUserDetailsResult oRR = new ChatUserDetailsResult();
            bool bReturn = false;


            try
            {
                oDR.USER_ID = new Guid(oReqInst.userId.Trim());
                bReturn = oDR.FetchUserDetails();

                if (bReturn)
                {
                    oRR.UserRoleCode = oDR.USER_ROLE_CODE;
                    oRR.UserRoleName = oDR.USER_ROLE_DESCRIPTION;
                    oRR.UserName = oDR.USER_NAME;
                    oRR.EmailID= oDR.EMAIL_ID;
                    oRR.ContactNumber= oDR.CONTACT_NUMBER;
                }
                oRS.responseCode = string.Empty;
                oRS.responseMessage = oDR.RESPONSE_MESSAGE;
                oRS.responseStatus = oDR.RESPONSE_STATUS;
                oRR.responseStatus = oRS;
            }
            catch (Exception expErr)
            {
                oRS.responseCode = "ERR";
                oRS.responseMessage = expErr.Message;
                oRS.responseStatus = false;
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
