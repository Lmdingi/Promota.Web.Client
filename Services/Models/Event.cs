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
        // Basic information
        [Required]
        public string Title { get; set; } =  string.Empty;
        [Required]
        public string Category { get; set; } = string.Empty;
        public List<string> Keywords { get; set; } = [];

        //Event Date and Time
        public string TimeZone { get; set; } = TimeZoneInfo.Local.Id;

        [Required]
        [Display(Name = "Start Date")]
        [FutureDate(ErrorMessage = "Start Date cannot be in the past.")]
        public DateTime StartDate { get; set; } = DateTime.Now;


        [Display(Name = "End Date")]
        [FutureDate(ErrorMessage = "End Date cannot be in the past.")]
        public DateTime? EndDate { get; set; } = DateTime.Now;

        // Location Mode Selection
        public EventLocationModel? Location { get; set; }

        // Pricing Section
        public string? Currency { get; set; }
        public decimal Price { get; set; }
        public string? TicketLink { get; set; }

        // Permissions
        public bool AllowComments { get; set; } = true;
        public bool AllowChats { get; set; } = true;

        // Description
        [Display(Name = "Description")]
        public string? DescriptionHtml { get; set; }

        // Media
        public MediaModel? Media { get; set; }

        // Creator Infor Section
        public UserModel? Creator { get; set; }
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
