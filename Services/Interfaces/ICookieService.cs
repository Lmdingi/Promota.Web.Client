using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICookieService
    {
        Task<string> Get(string key);
        Task Remove(string key);
        Task Set(string key, string value, int days);
    }
}
