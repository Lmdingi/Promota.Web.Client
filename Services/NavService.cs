using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class NavService : INavService
    {
        // props
        public string? UserName { get; set; } 

        // fields

        // ctors
        public NavService()
        {
            
        }

        // methods
        //private async Task<bool> IsAuthenticatedUser()
        //{
        //    var state = await _AuthStateProvider.GetAuthenticationStateAsync();
        //    var user = state.User;

        //    if (user.Identity.IsAuthenticated)
        //    {
        //        _isLoggedIn = true;
        //        _userName = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        //    }
        //    else
        //    {
        //        _isLoggedIn = false;
        //    }
        //}

    }
}
