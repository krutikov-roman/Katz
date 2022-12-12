/*

By: Roman Krutikov

Description: This class is used to store the Cats and Adoption Form database setup. This class
             will then be used by other controllers/services to interact with the database with
             Cat and Form objects.
             
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Services
{
    public class Database: DbContext
    {
        public DbSet<Cat> Cats {get; set;}

        public DbSet<CatAdoptionForm> CatAdoptionForms {get; set;}

        public DbSet<CatPutUpForAdoptionForm> CatPutUpForAdoptionForms {get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Database.db");
        }
    }
}