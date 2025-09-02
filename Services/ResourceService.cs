using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ResourceService : IResourceService
    {
        // props

        // fields
        private readonly IAPIService _aPIService;

        // ctors
        public ResourceService(IAPIService aPIService)
        {
            _aPIService = aPIService;
        }

        public Task<bool> Verify()
        {
            throw new NotImplementedException();
        }

        // methods
        //public async Task<bool> Verify()
        //{
        //    var result = await _aPIService.GetAsync("Resource/Verify");
        //    return result.IsSuccessStatusCode;
        //}
    }
}
