using Microsoft.JSInterop;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CookieService : ICookieService
    {
        // props

        // fields
        private readonly IJSRuntime _jSRuntime;

        // ctors
        public CookieService(IJSRuntime jSRuntime)
        {
            _jSRuntime = jSRuntime;
        }

        // methods
        public async Task<string> Get(string key)
        {
            return await _jSRuntime.InvokeAsync<string>("getCookie", key);
        }

        public async Task Remove(string key)
        {
            await _jSRuntime.InvokeVoidAsync("deleteCookie", key);
        }

        public async Task Set(string key, string value, int days)
        {
            await _jSRuntime.InvokeVoidAsync("setCookie", key, value, days);
        }

        

    }
}
