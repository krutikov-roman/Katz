/*

By: Roman Krutikov

Description: This class is used to store the Users database setup. This class will 
             then be used by other controllers/services for logging users in and logging
             them out with tokens.
             
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Services
{
    public class DatabaseIdentities : IdentityDbContext<AppUser>
    {
        public DbSet<LoggedInToken> LoggedInTokens {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=DatabaseIdentities.db");
        }
    }
}


