using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthService : IAuthService
    {
        // props

        // fields
        private readonly IAPIService _aPIService;
        private readonly IAccessTokenService _accessTokenService;
        private readonly NavigationManager _navManager;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly HttpClient _client;

        // ctors
        public AuthService(IAPIService aPIService,IAccessTokenService accessTokenService, NavigationManager navManager, IHttpClientFactory httpClientFactory, IRefreshTokenService refreshTokenService)
        {
            _aPIService = aPIService;
            _accessTokenService = accessTokenService;
            _navManager = navManager;
            _refreshTokenService = refreshTokenService;
            _client = httpClientFactory.CreateClient("ApiClient");
        }

        // methods 
        #region Login
        public async Task<bool> Login(LoginRequestDto loginModel)
        {
            try
            {
                var response = await _aPIService.PostAsync<LoginResponseDto>("Auth/Login", loginModel);

                await _accessTokenService.RemoveToken();
                await _accessTokenService.SetToken(response?.AccessToken ?? "");
                await _refreshTokenService.Set(response?.RefreshToken ?? "");

                return true;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Login failed: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Login failed: An unexpected error occurred.", ex);
            }
        }
        #endregion

        #region Logout
        //public async Task<bool> Logout()
        //{
        //    try
        //    {
        //        var refreshToken = await _refreshTokenService.Get();
        //        _client.DefaultRequestHeaders.Add("Cookie", $"refreshtoken={refreshToken}");
        //        var responseMessage = await _client.PostAsync("Auth/Logout",null);

        //        if (responseMessage.IsSuccessStatusCode)
        //        {
        //            await _accessTokenService.RemoveToken();
        //            await _refreshTokenService.Remove();
        //            //_navManager.NavigateTo("/", forceLoad: true);
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        #endregion

        #region Refresh (remove)
        //public async Task<bool> RefreshTokenAsync()
        //{
        //    try
        //    {
        //        var refreshToken = await _refreshTokenService.Get();
        //        _client.DefaultRequestHeaders.Add("Cookie", $"refreshtoken={refreshToken}");
        //        var responseMessage = await _client.PostAsync("Auth/Refresh", null);

        //        if (responseMessage.IsSuccessStatusCode)
        //        {
        //            var token = await responseMessage.Content.ReadAsStringAsync();
        //            if (!string.IsNullOrEmpty(token))
        //            {
        //                var result = JsonConvert.DeserializeObject<LoginResponseDto>(token);

        //                await _accessTokenService.SetToken(result.AccessToken);
        //                await _refreshTokenService.Set(result.RefreshToken);

        //                return true;
        //            }                    
        //        }

        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        #endregion
    }
}
