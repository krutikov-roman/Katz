/*

By: Roman Krutikov

Description: This class is used as a template for the properties that are needed
             for when a user is logging in to use the Admin Page.
             
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}