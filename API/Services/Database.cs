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
    public class Database: DbContext, IListableDatabase
    {
        private DbSet<Cat> Cats {get; set;}

        private DbSet<CatAdoptionForm> CatAdoptionForms {get; set;}

        private DbSet<CatPutUpForAdoptionForm> CatPutUpForAdoptionForms {get; set;}

        public List<CatAdoptionForm> GetCatAdoptionFormsAsList(bool includeCat)
        {
            if (includeCat){
                return CatAdoptionForms.Include(x => x.Cat).ToList();
            }
            return CatAdoptionForms.ToList();
        }

        public List<CatPutUpForAdoptionForm> GetCatPutUpForAdoptionFormsAsList(bool includeCat)
        {
            if (includeCat){
                return CatPutUpForAdoptionForms.Include(x => x.Cat).ToList();
            }
            return CatPutUpForAdoptionForms.ToList();
        }

        public Task SaveChangesAsync() {
            return base.SaveChangesAsync();
        }

        public void SaveChanges() {
            base.SaveChanges();
        }

        public List<Cat> GetCatsAsList()
        {
            return Cats.ToList();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Database.db");
        }
    }
}