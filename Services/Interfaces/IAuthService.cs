using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Login(string email, string password);
        Task<bool> Logout();
        Task<bool> RefreshTokenAsync();
    }
}
