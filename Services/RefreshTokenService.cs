using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        // props

        // fields
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private readonly string _key = "refresh_token";

        // ctors
        public RefreshTokenService(ProtectedLocalStorage protectedLocalStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;
        }

        // methods
        public async Task<string> Get()
        {
            var result = await _protectedLocalStorage.GetAsync<string>(_key);
            if (result.Success)
            {
                return result.Value!;
            }

            return string.Empty;
        }

        public async Task Remove()
        {
            await _protectedLocalStorage.DeleteAsync(_key);
        }

        public async Task Set(string value)
        {
           await _protectedLocalStorage.SetAsync(_key, value);
        }
    }
}
