using Client.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
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
using System.Text.Json;
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
        private readonly JWTAuthenticationStateProvider _authStateProvider;

        // ctors
        public APIService(IHttpClientFactory httpClientFactory, IAccessTokenService accessTokenService, IRefreshTokenService refreshTokenService, NavigationManager navigationManager, AuthenticationStateProvider authStateProvider)
        {
            _client = httpClientFactory.CreateClient("ApiClient");
            _accessTokenService = accessTokenService;
            _refreshTokenService = refreshTokenService;
            _navigationManager = navigationManager;
            _authStateProvider = (JWTAuthenticationStateProvider)authStateProvider; 
        }

        #region GetAsync
        public async Task<T> GetAsync<T>(string endpoint)
        {
            await AddTokens();

            var response = await _client.GetAsync(endpoint);

            // Retry once on 401 Unauthorized
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshJwtTokenByRefreshTokenAsync();
                await AddTokens();
                response = await _client.GetAsync(endpoint);
            }

            if (!response.IsSuccessStatusCode)
            {
                var problem = await ReadJsonProblemFromResponse(response);
                throw new InvalidOperationException(problem);
            }

            // Handle success
            var content = await response.Content.ReadAsStringAsync();

            // For bool, int, double, etc., try direct conversion
            if (typeof(T).IsPrimitive || typeof(T) == typeof(decimal) || typeof(T) == typeof(bool) || typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(content, typeof(T));
            }

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
            return result;
        }
        #endregion

        #region PostAsync
        public async Task<T?> PostAsync<T>(string endpoint, object obj)
        {
            await AddTokens();

            var response = await _client.PostAsJsonAsync(endpoint, obj);

            // Retry once on 401 Unauthorized
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshJwtTokenByRefreshTokenAsync();
                await AddTokens();
                response = await _client.PostAsJsonAsync(endpoint, obj);
            }  

            if (!response.IsSuccessStatusCode)
            {
                var problem = await ReadJsonProblemFromResponse(response);
                throw new InvalidOperationException(problem);
            }

            // Handle success
            var content = await response.Content.ReadAsStringAsync();

            // For bool, int, double, etc., try direct conversion
            if (typeof(T).IsPrimitive || typeof(T) == typeof(decimal) || typeof(T) == typeof(bool) || typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(content, typeof(T)); 
            }

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
            return result;
        }
        #endregion

        #region PutAsync
        public async Task<T?> PutAsync<T>(string endpoint, object obj)
        {
            await AddTokens();

            var response = await _client.PutAsJsonAsync(endpoint, obj);

            // Retry once on 401 Unauthorized
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshJwtTokenByRefreshTokenAsync();
                await AddTokens();
                response = await _client.PutAsJsonAsync(endpoint, obj);
            }

            if (!response.IsSuccessStatusCode)
            {
                var problem = await ReadJsonProblemFromResponse(response);
                throw new InvalidOperationException(problem);
            }

            // Handle success
            var content = await response.Content.ReadAsStringAsync();

            // For bool, int, double, etc., try direct conversion
            if (typeof(T).IsPrimitive || typeof(T) == typeof(decimal) || typeof(T) == typeof(bool) || typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(content, typeof(T));
            }

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
            return result;
        }
        #endregion

        #region Helpers
        private async Task<string> ReadJsonProblemFromResponse(HttpResponseMessage response)
        {
            string problem = string.Empty;

            var json = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(json))
            {
                var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(json);
                //var validationProblemDetails = JsonSerializer.Deserialize<ValidationProblemDetails>(json);

                problem = $"{problemDetails?.Title + " " + problemDetails?.Detail}";
                //problem = $"{validationProblemDetails?.Title + " " + validationProblemDetails?.Detail}";
            }

            return $"{response.ReasonPhrase}: {problem}";
        }
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
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponseDto>(token);

                await _accessTokenService.RemoveToken();
                await _accessTokenService.SetToken(result.AccessToken ?? "");
                await _refreshTokenService.Set(result.RefreshToken ?? "");

                // Notify state change
                _authStateProvider.MarkUserAsAuthenticated(result.AccessToken ?? "");
            }
        }


        #endregion
    }
}
