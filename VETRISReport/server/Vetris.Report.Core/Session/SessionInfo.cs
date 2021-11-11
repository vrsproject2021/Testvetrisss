using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Vetris.Report.Core.Session
{
    public class SessionInfo : ISessionInfo
    {
        private readonly IHttpContextAccessor _accessor;
        public SessionInfo(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Guid? UserId 
        {
            get
            {
                if (_accessor.HttpContext.User.Identities.Count() == 0) return null;
                var claim = _accessor.HttpContext.User.Identities.FirstOrDefault().Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(claim?.Value))
                {
                    return null;
                }

                Guid sessionId = new Guid(claim.Value);

                return sessionId;
            }
        }
        public string UserName {
            get
            {
                if (_accessor.HttpContext.User.Identities.Count() == 0) return null;
                var claim = _accessor.HttpContext.User.Identities.FirstOrDefault().Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                if (string.IsNullOrEmpty(claim?.Value))
                {
                    return null;
                }

                string userName = claim.Value;

                return userName;
            }
        }
    }
}
