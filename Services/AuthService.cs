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
        #region Register
        public async Task<bool> RegisterAsync(RegisterRequestDto registerModel)
        {
            var response = await _aPIService.PostAsync<LoginResponseDto>("Auth/Register", registerModel);

            if (response == null)
            {
                return false;
            }

            await _accessTokenService.RemoveToken();
            await _accessTokenService.SetToken(response?.AccessToken ?? "");
            await _refreshTokenService.Set(response?.RefreshToken ?? "");

            return true;
        }
        #endregion
        #region Login
        public async Task<bool> LoginAsync(LoginRequestDto loginModel)
        {
            var response = await _aPIService.PostAsync<LoginResponseDto>("Auth/Login", loginModel);
            
            if(response == null)
            {
                return false;
            }

            await _accessTokenService.RemoveToken();
            await _accessTokenService.SetToken(response?.AccessToken ?? "");
            await _refreshTokenService.Set(response?.RefreshToken ?? "");            

            return true;      
        }
        #endregion

        #region Logout
        public async Task<bool> LogoutAsync()
        {  
            var isLogedout = await _aPIService.PostAsync<bool>("Auth/Logout", null);
            
            if (!isLogedout)
            {
                return false;               
            }

            await _accessTokenService.RemoveToken();
            await _refreshTokenService.Remove();

            return true;
        }        
        #endregion
    }
}
