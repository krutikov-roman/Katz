using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using API.Models;
using API.Models.DTOs;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace TestingProject
{
    public class TestFormController
    {
        [Fact]
        public async Task CreateCatForAdoptionAsync_AddingForm_FormUpdatesDatabaseAsync()
        {
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Cat cat  = new Cat
            {
                CatStatus = CatStatus.New,
                Name = "Snowflake",
                Description = "White cat"
            };
            CatPutUpForAdoptionForm adoptionForm = new CatPutUpForAdoptionForm
            {
                FormStatus = FormStatus.New,
                OwnerName = "Andreea",
                OwnerEmail = "popaan@sheridancollege.ca",
                Cat = cat
            };

            databaseServiceMock.Setup(m => m.AddToCatAdoptionForms(adoptionForm));

            FormsController formsController = new FormsController(null);
            Task<IActionResult> result = (Task<IActionResult>)await formsController.CreateCatForAdoptionAsync(adoptionForm);

            ResponseDTO expectedResponseDTO = new ResponseDTO() {
                Status= 200,
                Message= "Successfully sent the request to put a cat up for adoption",
            };

            Assert.True(expectedResponseDTO.Message == "Successfully sent the request to put a cat up for adoption");
        }
    }
}