using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class Event
    {
        public string? Id { get; set; }
        // Event Title
        [Required]
        public string Title { get; set; } =  string.Empty;

        // Event Date and Time
        public string TimeZone { get; set; } = TimeZoneInfo.Local.Id;

        [Required]
        [Display(Name = "Start Date")]
        [FutureDate(ErrorMessage = "Start Date cannot be in the past.")]
        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime? EndDate { get; set; }

        // Location Mode Selection
        public bool IsRomote { get; set; }

        public string? RemoteLink { get; set; }

        public string? PhysicalLocation { get; set; }

        // Pricing Section
        public string? Currency { get; set; }
        public decimal Price { get; set; }
        public string? TicketLink { get; set; }

        // Creator Infor Section
        public UserModel Creator { get; set; }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return true; 

            DateTime dateValue = (DateTime)value;
            return dateValue.Date >= DateTime.Today;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} cannot be in the past.";
        }
    } 
}
