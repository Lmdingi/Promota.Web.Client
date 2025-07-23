using Services.Interfaces;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IAPIService _aPIService;

        public UserService(IAPIService aPIService)
        {
            _aPIService = aPIService;
        }
        public async Task<UserModel> GetUserProfileByUserIdAsync(string CreatorId)
        {
            var userInformation = await _aPIService.GetAsync<UserModel>($"users?userId={Uri.EscapeDataString(CreatorId)}");
            return userInformation;
        }

        public async Task UpdateUserProfileAsync(UserModel model)
        {
            var response = await _aPIService.PutAsync<bool>($"Users", model);

            //return response;
        }

        //public Task<ProfileModel> GetUserProfileByIdAsync(string userId)
        //{
        //    //throw new NotImplementedException();
        //}
    }
}
