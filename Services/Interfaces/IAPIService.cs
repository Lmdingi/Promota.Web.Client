using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAPIService
    {
        Task DeleteAsync(string endpoint);
        Task<T> GetAsync<T>(string endpoint);
        Task<T?> PostAsync<T>(string endpoint, object obj);
        Task<T?> PutAsync<T>(string endpoint, object obj);
    }
}
