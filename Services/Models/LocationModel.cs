using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class LocationModel
    {
        public bool IsRomote { get; set; }
        public string? RemoteLink { get; set; }

        public string? VenueName { get; set; }   
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public string? Country { get; set; }
        public string? GoogleMapsEmbedLink { get; set; }
    }
}
