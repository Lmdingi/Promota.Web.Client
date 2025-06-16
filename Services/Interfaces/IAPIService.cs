using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAPIService
    {
        //Task<HttpResponseMessage> GetAsync(string endpoint);
        Task<T?> PostAsync<T>(string endpoint, object obj);
    }
}
