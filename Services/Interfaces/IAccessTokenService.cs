using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAccessTokenService
    {
        Task<string> GetToken();
        Task RemoveToken();
        Task SetToken(string accessToken);
    }
}
