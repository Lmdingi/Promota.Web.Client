using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthService : IAuthService
    {
        // props

        // fields
        private readonly IAccessTokenService _accessTokenService;
        private readonly NavigationManager _nav;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly HttpClient _client;

        // ctors
        public AuthService(IAccessTokenService accessTokenService, NavigationManager nav, IHttpClientFactory httpClientFactory, IRefreshTokenService refreshTokenService)
        {
            _accessTokenService = accessTokenService;
            _nav = nav;
            _refreshTokenService = refreshTokenService;
            _client = httpClientFactory.CreateClient("ApiClient");
        }

        // methods
        public async Task<bool> Login(string email, string password)
        {
            try
            {
                var responseMessage = await _client.PostAsJsonAsync("Auth/Login", new { email, password });

                if (responseMessage.IsSuccessStatusCode)
                {
                    var token = await responseMessage.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<AuthResponseDto>(token);

                    await _accessTokenService.RemoveToken();
                    await _accessTokenService.SetToken(result.AccessToken);
                    await _refreshTokenService.Set(result.RefreshToken);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task Logout()
        {
            try
            {
                var refreshToken = await _refreshTokenService.Get();
                _client.DefaultRequestHeaders.Add("Cookie", $"refreshtoken={refreshToken}");
                var responseMessage = await _client.PostAsync("Auth/Logout",null);

                if (responseMessage.IsSuccessStatusCode)
                {
                    await _accessTokenService.RemoveToken();
                    await _refreshTokenService.Remove();
                    _nav.NavigateTo("/login", forceLoad: true);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<bool> RefreshTokenAsync()
        {
            try
            {
                var refreshToken = await _refreshTokenService.Get();
                _client.DefaultRequestHeaders.Add("Cookie", $"refreshtoken={refreshToken}");
                var responseMessage = await _client.PostAsync("Auth/Refresh", null);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var token = await responseMessage.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(token))
                    {
                        var result = JsonConvert.DeserializeObject<AuthResponseDto>(token);

                        await _accessTokenService.SetToken(result.AccessToken);
                        await _refreshTokenService.Set(result.RefreshToken);

                        return true;
                    }                    
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
