using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Interfaces;
using Services.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class APIService : IAPIService
    {
        // props

        // fields
        private readonly HttpClient _client;
        private readonly IAccessTokenService _accessTokenService;
        private readonly IRefreshTokenService _refreshTokenService;    
        private readonly NavigationManager _navigationManager;

        // ctors
        public APIService(IHttpClientFactory httpClientFactory, IAccessTokenService accessTokenService, IRefreshTokenService refreshTokenService, NavigationManager navigationManager)
        {
            _client = httpClientFactory.CreateClient("ApiClient");
            _accessTokenService = accessTokenService;
            _refreshTokenService = refreshTokenService;
            _navigationManager = navigationManager;
        }

        #region GetAsync
        // methods
        //public async Task<HttpResponseMessage> GetAsync(string endpoint)
        //{
        //    var token = await _accessTokenService.GetToken();
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    var responseMessage = await _client.GetAsync(endpoint);

        //    if(responseMessage.StatusCode == HttpStatusCode.Unauthorized)
        //    {
        //        // call refresh token
        //        var refreshTokenResult = await _authService.RefreshTokenAsync();

        //        if (!refreshTokenResult)
        //        {
        //            await _authService.Logout();
        //        }

        //        var newToken = await _accessTokenService.GetToken();
        //        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //        var newResponseMessage = await _client.GetAsync(endpoint);
        //        return newResponseMessage;
        //    }

        //    return responseMessage;
        //}
        #endregion

        #region PostAsync
        public async Task<T?> PostAsync<T>(string endpoint, object obj)
        {
            await AddTokens();
            
            var response = await _client.PostAsJsonAsync(endpoint, obj);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshJwtTokenByRefreshTokenAsync();
                await AddTokens();
                response = await _client.PostAsJsonAsync(endpoint, obj);                    
            }  
            
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                throw new InvalidOperationException((problem?.Title + " "+ problem?.Detail) ?? "Bad Request");
            }
            else if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.BadRequest)
            {
                var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                throw new InvalidOperationException((problem?.Title + " " + problem?.Detail) ?? "An unexpected error occurred.");
            }
            
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(content);
            
            return result;
        }
        #endregion

        #region Helpers
        private async Task AddTokens()
        {
            var token = await _accessTokenService.GetToken();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var refreshToken = await _refreshTokenService.Get();
            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                if (_client.DefaultRequestHeaders.Contains("Cookie"))
                {
                    _client.DefaultRequestHeaders.Remove("Cookie");
                }

                _client.DefaultRequestHeaders.Add("Cookie", $"refreshtoken={refreshToken}");
            }
        }

        private async Task RefreshJwtTokenByRefreshTokenAsync()
        {
            var responseMessage = await _client.PostAsync("Auth/Refresh", null);

            var token = await responseMessage.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(token))
            {
                var result = JsonConvert.DeserializeObject<LoginResponseDto>(token);

                await _accessTokenService.SetToken(result.AccessToken);
            }
        }
        #endregion
    }
}
