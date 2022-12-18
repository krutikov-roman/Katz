/*

By: Roman Krutikov

Description: This interface is used by the Database to interact with the various tables through
             methods which can be mocked during testing.
             
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Services
{
    public interface IListableDatabase
    {

        public List<Cat> GetCatsAsList();

        public List<CatAdoptionForm> GetCatAdoptionFormsAsList(bool includeCat);

        public List<CatPutUpForAdoptionForm> GetCatPutUpForAdoptionFormsAsList(bool includeCat);

        public void SaveChanges();

        public Task SaveChangesAsync();

        public void AddToCatAdoptionForms(CatPutUpForAdoptionForm requestCatForAdoptionForm);

        public void AddToAdoptCatForms(CatAdoptionForm requestAdoptCatForm);
    }
}