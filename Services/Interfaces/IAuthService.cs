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
        Task<bool> RegisterAsync(RegisterRequestDto loginModel);
        Task<bool> LoginAsync(LoginRequestDto loginModel);
        Task<bool> LogoutAsync();
        //Task<bool> RefreshTokenAsync();
    }
}
