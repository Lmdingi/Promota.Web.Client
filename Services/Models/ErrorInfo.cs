using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class ErrorInfo
    {
        // props
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }

        // fields

        // ctors
        public ErrorInfo() { }
        public ErrorInfo(string title, string message, string? details = null)
        {
            Title = title;
            Message = message;
            Details = details;
        }

        // methods

    }
}
