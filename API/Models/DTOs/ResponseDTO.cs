/*

By: Roman Krutikov

Description: This class is used as a template for the properties that are needed
             when the API is sending a response to a user who has performed an API call.
             
*/
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