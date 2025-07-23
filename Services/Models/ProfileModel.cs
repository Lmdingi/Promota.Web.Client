using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class ProfileModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string Bio { get; set; }
        public string? ProfilePicture { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? Country { get; set; }
        public int EventsPromotions { get; set; }
        public int Followers { get; set; }
        public int Reviews { get; set; }
    }
}
