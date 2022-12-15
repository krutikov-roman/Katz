/*

By: Roman Krutikov

Description: This class is used as a template for the properties that are needed
             for when a user is logging out to no longer use the Admin page.
             
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.DTOs
{
    public class LogoutDTO
    {
        public string Token { get; set; }
    }
}