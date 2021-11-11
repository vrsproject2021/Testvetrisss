using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Vetris.Report.Core.Extensions;
using Vetris.Report.Service.Login;

namespace Vetris.Report.Server.Authorization
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ILoginService _login;
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ILoginService login) : base(options, logger, encoder, clock)
        {
            _login = login;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization") || Request.Headers["Authorization"].ToString()=="null")
                return AuthenticateResult.Fail("Missing Authorization Header");
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                var tokenInfo = _login.ParseToken(authHeader);
                var claims = new[] {
                        new Claim(ClaimTypes.NameIdentifier, tokenInfo.UserId.ToString()),
                        new Claim(ClaimTypes.Name, tokenInfo.Name),
                    };
                var identity = new ClaimsIdentity(claims, "Bearer Token");
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                await Task.FromResult(0);
                return AuthenticateResult.Success(ticket);
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
        }
    }
}
