﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Dtos
{
    public class ErrorResponseDto
    {
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
