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
                        ProfilePictureUrl = "https://lh3.googleusercontent.com/aida-public/AB6AXuDc3WSbUkMWeQ5DqwbsE75otJLohmePjWAxBx2G8TGsi1vGNLwnDP6spJCjjl5DwjR1bkbYunPkuwI-v3eKWIZyPui6V2fiFBTPq0QiZ9dcr-EJh3JGwIDhXm2UrfhXlUmu11k-HzDFWJ0gmgWJh2V7YCPUuI0JEgxaoOIo5AZx7gM2HCKvtW8C9i-j0DgcThIEbEE01BcWzA9d3BvC_94SedAJA5bZ_iPHXZpOCpestSWRJAjYo7wHMp2bKxXld05WH1yP8udZuEM"
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
