using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AccessTokenService : IAccessTokenService
    {
        // props

        // fields
        private readonly ICookieService _cookieService;
        private readonly string _tokenKey = "access_token";

        // ctors
        public AccessTokenService(ICookieService cookieService)
        {
            _cookieService = cookieService;
        }

        // methods
        public async Task<string> GetToken()
        {
            return await _cookieService.Get(_tokenKey);
        }

        public async Task RemoveToken()
        {
            await _cookieService.Remove(_tokenKey);
        }

        public async Task SetToken(string accessToken)
        {
            await _cookieService.Set(_tokenKey, accessToken, 1);
        }
    }
}
