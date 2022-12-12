/*

By: Roman Krutikov

Description: This class is used to extend an IdentityUser to include an IsAdmin
             boolean attribute to distigush logged in admin and nonadmin users.
             
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace API.Models
{
    public class AppUser : IdentityUser
    {
        public bool? IsAdmin { get; set; }
    }
}