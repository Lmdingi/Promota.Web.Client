using Microsoft.AspNetCore.Components.Authorization;
using Services.Interfaces;
using Services.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client.Security
{
    public class JWTAuthenticationStateProvider : AuthenticationStateProvider
    {
        // props

        // fields
        private readonly IAccessTokenService _accessTokenService;

        // ctors
        public JWTAuthenticationStateProvider(IAccessTokenService accessTokenService)
        {
            _accessTokenService = accessTokenService;
        }

        // methods
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _accessTokenService.GetToken();

                if (string.IsNullOrWhiteSpace(token))
                {
                    return await MarkAsUnauthorize();
                }

                var readJWT = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var identity = new ClaimsIdentity(readJWT.Claims, "JWT");
                var principal = new ClaimsPrincipal(identity);

                return await Task.FromResult(new AuthenticationState(principal));
            }
            catch (Exception ex)
            {
                return await MarkAsUnauthorize();
            }
        }

        private async Task<AuthenticationState> MarkAsUnauthorize()
        {
            try
            {
                var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                NotifyAuthenticationStateChanged(Task.FromResult(state));

                return state;
            }
            catch (Exception ex)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task<UserModel?> GetCurrentUserState()
        {
            try
            {
                var state = await GetAuthenticationStateAsync();
                var user = state.User;
                var currentUser = new UserModel();

                if (user.Identity != null && user.Identity.IsAuthenticated)
                {
                    currentUser = new UserModel
                    {
                        Id = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                        IsAuthenticated = user.Identity.IsAuthenticated,
                        UserName = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                    };
                }

                return currentUser;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public void MarkUserAsAuthenticated(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var identity = new ClaimsIdentity(jwtToken.Claims, "JWT");
            var principal = new ClaimsPrincipal(identity);

            var authState = Task.FromResult(new AuthenticationState(principal));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var identity = new ClaimsIdentity(); // empty
            var principal = new ClaimsPrincipal(identity);
            var authState = Task.FromResult(new AuthenticationState(principal));
            NotifyAuthenticationStateChanged(authState);
        }

    }
}
