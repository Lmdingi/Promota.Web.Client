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
        public User CurrentUser { get; set; }        

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

                if(string.IsNullOrWhiteSpace(token))
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

        public async Task<User?> GetCurrentUserState()
        {
            try
            {
                var state = await GetAuthenticationStateAsync();
                var user = state.User;

                if (user.Identity != null && user.Identity.IsAuthenticated)
                {
                    var currentuser = new User();
                    currentuser.IsAuthenticated = user.Identity.IsAuthenticated;
                    currentuser.UserName = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                    currentuser.ProfilePictureUrl = "https://reddoorescape.com/wp-content/uploads/DP.png";

                    CurrentUser = currentuser;
                }               

                return CurrentUser;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
    }
}
