using Microsoft.AspNetCore.Components;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class APIService : IAPIService
    {
        // props

        // fields
        private readonly HttpClient _client;
        private readonly IAccessTokenService _tokenService;
        private readonly IAuthService _authService;
        private readonly NavigationManager _nav;

        // ctors
        public APIService(IHttpClientFactory httpClientFactory, IAccessTokenService tokenService, IAuthService authService, NavigationManager nav)
        {
            _client = httpClientFactory.CreateClient("ApiClient");
            _tokenService = tokenService;
            _authService = authService;
            _nav = nav;
        }

        // methods
        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            var token = await _tokenService.GetToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var responseMessage = await _client.GetAsync(endpoint);

            if(responseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                // call refresh token
                var refreshTokenResult = await _authService.RefreshTokenAsync();

                if (!refreshTokenResult)
                {
                    await _authService.Logout();
                }

                var newToken = await _tokenService.GetToken();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var newResponseMessage = await _client.GetAsync(endpoint);
                return newResponseMessage;
            }

            return responseMessage;
        }

        public Task<HttpResponseMessage> PostAsync(string endpoint, object obj)
        {
            throw new NotImplementedException();
        }

        
    }
}
