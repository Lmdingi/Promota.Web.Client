using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Client.Security
{
    public class JWTAuthenticationHandler : AuthenticationHandler<CustomOption>
    {
        private readonly IAuthService _authService;

        public JWTAuthenticationHandler(IOptionsMonitor<CustomOption> options, ILoggerFactory logger, IAuthService authService, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _authService = authService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                var token = Request.Cookies["access_token"];

                if (string.IsNullOrWhiteSpace(token))
                {
                    return Task.FromResult(AuthenticateResult.NoResult());
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // You can inspect the token or verify audience/issuer here if needed

                var identity = new ClaimsIdentity(jwtToken.Claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid token."));
            }
        }


        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            _authService.LogoutAsync();
            Response.Redirect("/login");
            return Task.CompletedTask;
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.Redirect("/accessdenied");
            return Task.CompletedTask;
        }
    }

    public class CustomOption : AuthenticationSchemeOptions
    {

    }
}
