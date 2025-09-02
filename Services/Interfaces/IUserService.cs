using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> GetUserProfileByUserIdAsync(string CreatorId);
        Task UpdateUserProfileAsync(UserModel model);
        //Task<ProfileModel> GetUserProfileByIdAsync(string userId);
    }
}
