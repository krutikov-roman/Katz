using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.DTOs
{
    public class ResponseDTO
    {
        public int Status {get; set; }
        public string Message { get; set; }
        public object? Data {get; set;}
        public object? Errors {get; set; }
    }
}