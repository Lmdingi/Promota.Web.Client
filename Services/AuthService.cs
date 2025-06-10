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
        private readonly HttpClient _client;

        // ctors
        public AuthService(IAccessTokenService accessTokenService, NavigationManager nav, IHttpClientFactory httpClientFactory)
        {
            _accessTokenService = accessTokenService;
            _nav = nav;
            _client = httpClientFactory.CreateClient("ApiClient");
        }

        // methods
        public async Task<bool> Login(string email, string password)
        {
            try
            {
                var status = await _client.PostAsJsonAsync("Auth/Login"/*login route*/, new { email, password });

                if (status.IsSuccessStatusCode)
                {
                    var token = await status.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<AuthResponseDto>(token);
                    await _accessTokenService.SetToken(result.AccessToken);

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
    }
}
