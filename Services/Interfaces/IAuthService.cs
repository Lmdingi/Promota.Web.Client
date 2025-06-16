using Services.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Login(LoginRequestDto loginModel);
        //Task<bool> Logout();
        //Task<bool> RefreshTokenAsync();
    }
}
