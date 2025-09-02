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

            userInformation.Bio = "Passionate about bringing people together through unforgettable events, I am dedicated to creating experiences that leave lasting memories. With a keen eye for trends and a talent for connecting with diverse audiences, I work closely with artists, brands, and venues to craft events that excite, inspire, and entertain.";
            userInformation.Email = "email@gma]il.com";
            userInformation.Phone = "+27476921582";
            userInformation.Website = "www.google.com";


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
