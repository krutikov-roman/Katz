/*

By: Roman Krutikov

Description: This class is used as a template for the properties that are needed
             for when a user is registering to use Admin page.
             
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.DTOs
{
    public class RegisterDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}