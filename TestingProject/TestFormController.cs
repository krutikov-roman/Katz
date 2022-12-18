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
        public async Task CreateCatForAdoptionAsync_AddingForm_Error()
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
            IActionResult result = await (Task<IActionResult>)formsController.CreateCatForAdoptionAsync(adoptionForm);
            BadRequestObjectResult resultAsBad = (BadRequestObjectResult)result;
            ResponseDTO resultingResponseDto = (ResponseDTO) resultAsBad.Value;

            ResponseDTO expectedResponseDTO = new ResponseDTO() {
                Status= 400,
                Message= "An unexpected server error occurred",
            };

            bool formResponsesMatch =  expectedResponseDTO.Status.Equals(resultingResponseDto.Status) && 
                                expectedResponseDTO.Message.Equals(resultingResponseDto.Message);

            Assert.True(formResponsesMatch);
        }

        [Fact]
         public async Task AdoptCatAsync_AddingForm_Error()
        {
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Cat cat  = new Cat
            {
                CatStatus = CatStatus.WaitingForAdoption,
                Name = "Snowflake",
                Description = "White cat"
            };
            CatAdoptionForm adoptingForm = new CatAdoptionForm
            {
                OwnerName = "Andreea",
                OwnerEmail = "popaan@sheridancollege.ca",
                Cat = cat
            };
            AdoptCatDto adoptingFormDto = new AdoptCatDto
            {
                OwnerName = adoptingForm.OwnerName,
                OwnerEmail = adoptingForm.OwnerEmail,
                CatId = cat.Id
            };

            databaseServiceMock.Setup(m => m.AddToAdoptCatForms(adoptingForm));

            FormsController formsController = new FormsController(null);
            IActionResult result = await (Task<IActionResult>)formsController.AdoptCatAsync(adoptingFormDto);
            BadRequestObjectResult resultAsBad = (BadRequestObjectResult)result;
            ResponseDTO resultingResponseDto = (ResponseDTO) resultAsBad.Value;

            ResponseDTO expectedResponseDTO = new ResponseDTO() {
                Status= 400,
                Message= "An unexpected server error occurred",
            };

            bool formResponsesMatch =  expectedResponseDTO.Status.Equals(resultingResponseDto.Status) && 
                                expectedResponseDTO.Message.Equals(resultingResponseDto.Message);

            Assert.True(formResponsesMatch);
        }

        [Fact]
        public async Task AdoptCatAsync_CheckCat_Error()
        {
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Cat cat  = new Cat();
            CatAdoptionForm adoptingForm = new CatAdoptionForm
            {
                OwnerName = "Andreea",
                OwnerEmail = "popaan@sheridancollege.ca",
                Cat = cat
            };
            AdoptCatDto adoptingFormDto = new AdoptCatDto
            {
                OwnerName = adoptingForm.OwnerName,
                OwnerEmail = adoptingForm.OwnerEmail,
                CatId = cat.Id
            };

            databaseServiceMock.Setup(m => m.AddToAdoptCatForms(adoptingForm));

            FormsController formsController = new FormsController(null);
            IActionResult result = await (Task<IActionResult>)formsController.AdoptCatAsync(adoptingFormDto);
            BadRequestObjectResult resultAsBad = (BadRequestObjectResult)result;
            ResponseDTO resultingResponseDto = (ResponseDTO) resultAsBad.Value;

            ResponseDTO expectedResponseDTO = new ResponseDTO() {
                Status= 400,
                Message= "An unexpected server error occurred",
            };

            bool formResponsesMatch =  expectedResponseDTO.Status.Equals(resultingResponseDto.Status) && 
                                expectedResponseDTO.Message.Equals(resultingResponseDto.Message);

            Assert.True(formResponsesMatch);
        }
    }
}