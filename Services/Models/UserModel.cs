using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class UserModel
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? ProfilePicture = "images/profile-pic-placeholder.jpg";

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName => $"{FirstName} {LastName}".Trim();
        public string? Bio { get; set; }
        public LocationModel? Location { get; set; }
        public MediaModel Media { get; set; }
        public int EventsPromotions { get; set; }
        public int Followers { get; set; }
        public int Stars { get; set; }
    }
}
