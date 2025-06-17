using Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class Alert
    {
        public string? Title { get; set; }
        public string? Message { get; set; }
        public string? Details { get; set; }
        public AlertType Type { get; set; } = AlertType.Info;

        public Alert()
        {
            
        }
        public Alert(string title, string message, AlertType type, string? details = null)
        {
            Title = title;
            Message = message;
            Details = details;
            Type = type;
        }
    }
}
