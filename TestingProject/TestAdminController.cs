/*

By: Roman Krutikov

Description: This test class is used for testing some of the methods/api endpoints of the AdminController.
             
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using API.Controllers;
using API.Models.DTOs;
using API.Models;
using API.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace TestingProject
{
    public class TestAdminController: ControllerBase
    {
        [Fact]
        public void GetNewAdoptableCatForms_GetFrom2Total_Return1MatchingResult()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = new Guid(),
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New
                },
                new CatAdoptionForm {
                    Id = new Guid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.Accepted
                }
            };
            databaseServiceMock.Setup(m => m.GetCatAdoptionFormsAsList(true)).Returns(catAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)admin.GetNewAdoptableCatForms(testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<CatAdoptionForm> resultingCatAdoptionForms = ((IEnumerable<CatAdoptionForm>)resultingResponseDto.Data).ToList();

            // Define the expected result and check to see if the expected output matches the actual output
            List<CatAdoptionForm> catAdoptionFormsExcepted = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = new Guid(),
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New
                }
            };
            bool formsMatch = resultingCatAdoptionForms.Count.Equals(catAdoptionFormsExcepted.Count) &&
                                resultingCatAdoptionForms[0].Id.Equals(catAdoptionFormsExcepted[0].Id);

            Assert.True(formsMatch);
        }

        [Fact]
        public void GetNewAdoptableCatForms_GetFrom2Total_Return0MatchingResult()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = new Guid(),
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.Accepted
                },
                new CatAdoptionForm {
                    Id = new Guid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.Accepted
                }
            };
            databaseServiceMock.Setup(m => m.GetCatAdoptionFormsAsList(true)).Returns(catAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)admin.GetNewAdoptableCatForms(testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<CatAdoptionForm> resultingCatAdoptionForms = ((IEnumerable<CatAdoptionForm>)resultingResponseDto.Data).ToList();

            // Define the expected result and check to see if the expected output matches the actual output
            List<CatAdoptionForm> catAdoptionFormsExcepted = new List<CatAdoptionForm> ();
            bool formsMatch = resultingCatAdoptionForms.Count.Equals(catAdoptionFormsExcepted.Count);

            Assert.True(formsMatch);
        }

        [Fact]
        public void GetNewAdoptableCatForms_GetFrom2Total_Return2MatchingResult()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = new Guid(),
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New
                },
                new CatAdoptionForm {
                    Id = new Guid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New
                }
            };
            databaseServiceMock.Setup(m => m.GetCatAdoptionFormsAsList(true)).Returns(catAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)admin.GetNewAdoptableCatForms(testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<CatAdoptionForm> resultingCatAdoptionForms = ((IEnumerable<CatAdoptionForm>)resultingResponseDto.Data).ToList();

            // Define the expected result and check to see if the expected output matches the actual output
            List<CatAdoptionForm> catAdoptionFormsExcepted = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = new Guid(),
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New
                },
                new CatAdoptionForm {
                    Id = new Guid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New
                }
            };
            bool formsMatch =  resultingCatAdoptionForms.Count.Equals(catAdoptionFormsExcepted.Count) &&
                                resultingCatAdoptionForms[0].Id.Equals(catAdoptionFormsExcepted[0].Id) && 
                                resultingCatAdoptionForms[1].Id.Equals(catAdoptionFormsExcepted[1].Id);

            Assert.True(formsMatch);
        }

        [Fact]
        public async void AcceptNewAdoptableCatForms_ValidFormValidCat_Success()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid approvingFormGuid = new Guid();
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = approvingFormGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = new Guid(),
                        Name = "TestCat1",
                        Description = "TestCat1",
                        CatStatus = CatStatus.WaitingForAdoption
                    }
                },
                new CatAdoptionForm {
                    Id = new Guid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = new Guid(),
                        Name = "TestCat2",
                        Description = "TestCat2",
                        CatStatus = CatStatus.WaitingForAdoption
                    }
                }
            };
            databaseServiceMock.Setup(m => m.GetCatAdoptionFormsAsList(true)).Returns(catAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            SearchFormDTO searchFormDTO = new SearchFormDTO {
                Id = approvingFormGuid
            };
            IActionResult result = await (Task<IActionResult>)admin.AcceptAdoptableCatForm(searchFormDTO, testToken);
            OkObjectResult resultAsOk = (OkObjectResult)result;
            ResponseDTO resultingResponseDto = (ResponseDTO) resultAsOk.Value;

            // Define the expected result and check to see if the expected output matches the actual output
            ResponseDTO expectedResponseDTO = new ResponseDTO() {
                Status= 200,
                Message= "Successfully approved cat adoptable form!",
            };

            bool formResponsesMatch =  expectedResponseDTO.Status.Equals(resultingResponseDto.Status) && 
                                expectedResponseDTO.Message.Equals(resultingResponseDto.Message);

            Assert.True(formResponsesMatch);
        }

        [Fact]
        public async void AcceptNewAdoptableCatForms_ValidFormInvalidCat_Failed()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testFormId = new Guid();
            Guid testCatId = new Guid();
            CatStatus badCatStatus = CatStatus.Adopted;
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = testFormId,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = testCatId,
                        Name = "TestCat1",
                        Description = "TestCat1",
                        CatStatus = badCatStatus
                    }
                },
                new CatAdoptionForm {
                    Id = new Guid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = new Guid(),
                        Name = "TestCat2",
                        Description = "TestCat2",
                        CatStatus = CatStatus.WaitingForAdoption
                    }
                }
            };
            databaseServiceMock.Setup(m => m.GetCatAdoptionFormsAsList(true)).Returns(catAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            SearchFormDTO searchFormDTO = new SearchFormDTO {
                Id = testFormId
            };
            IActionResult result = await (Task<IActionResult>)admin.AcceptAdoptableCatForm(searchFormDTO, testToken);
            BadRequestObjectResult resultAsNotFound = (BadRequestObjectResult)result;
            ResponseDTO resultingResponseDto = (ResponseDTO) resultAsNotFound.Value;

            // Define the expected result and check to see if the expected output matches the actual output
            ResponseDTO expectedResponseDTO = new ResponseDTO() {
                Status= 400,
                Message= $"Cat with Guid '{testCatId}' has CatStatus '{badCatStatus}' when '{CatStatus.WaitingForAdoption}' was expected...",
            };

            bool formResponsesMatch =  expectedResponseDTO.Status.Equals(resultingResponseDto.Status) && 
                                expectedResponseDTO.Message.Equals(resultingResponseDto.Message);

            Assert.True(formResponsesMatch);
        }

        [Fact]
        public async void AcceptNewAdoptableCatForms_InvalidFormValidCat_Failed()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testFormId = new Guid();
            Guid testCatId = new Guid();
            FormStatus badFormStatus = FormStatus.Denied;
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = testFormId,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = badFormStatus,
                    Cat = new Cat {
                        Id = testCatId,
                        Name = "TestCat1",
                        Description = "TestCat1",
                        CatStatus = CatStatus.WaitingForAdoption
                    }
                },
                new CatAdoptionForm {
                    Id = new Guid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = new Guid(),
                        Name = "TestCat2",
                        Description = "TestCat2",
                        CatStatus = CatStatus.WaitingForAdoption
                    }
                }
            };
            databaseServiceMock.Setup(m => m.GetCatAdoptionFormsAsList(true)).Returns(catAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            SearchFormDTO searchFormDTO = new SearchFormDTO {
                Id = testFormId
            };
            IActionResult result = await (Task<IActionResult>)admin.AcceptAdoptableCatForm(searchFormDTO, testToken);
            BadRequestObjectResult resultAsNotFound = (BadRequestObjectResult)result;
            ResponseDTO resultingResponseDto = (ResponseDTO) resultAsNotFound.Value;

            // Define the expected result and check to see if the expected output matches the actual output
            ResponseDTO expectedResponseDTO = new ResponseDTO() {
                Status= 400,
                Message= $"Cat Adoption Form with Guid '{testFormId}' has FormStatus '{badFormStatus}' when '{FormStatus.New}' was expected...",
            };

            bool formResponsesMatch =  expectedResponseDTO.Status.Equals(resultingResponseDto.Status) && 
                                expectedResponseDTO.Message.Equals(resultingResponseDto.Message);

            Assert.True(formResponsesMatch);
        }

        [Fact]
        public async void AcceptNewAdoptableCatForms_InvalidFormInvalidCat_Failed()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testFormId = new Guid();
            Guid testCatId = new Guid();
            FormStatus badFormStatus = FormStatus.Denied;
            CatStatus badCatStatus = CatStatus.New;
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = testFormId,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = badFormStatus,
                    Cat = new Cat {
                        Id = testCatId,
                        Name = "TestCat1",
                        Description = "TestCat1",
                        CatStatus = badCatStatus
                    }
                },
                new CatAdoptionForm {
                    Id = new Guid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = new Guid(),
                        Name = "TestCat2",
                        Description = "TestCat2",
                        CatStatus = CatStatus.WaitingForAdoption
                    }
                }
            };
            databaseServiceMock.Setup(m => m.GetCatAdoptionFormsAsList(true)).Returns(catAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            SearchFormDTO searchFormDTO = new SearchFormDTO {
                Id = testFormId
            };
            IActionResult result = await (Task<IActionResult>)admin.AcceptAdoptableCatForm(searchFormDTO, testToken);
            BadRequestObjectResult resultAsNotFound = (BadRequestObjectResult)result;
            ResponseDTO resultingResponseDto = (ResponseDTO) resultAsNotFound.Value;

            // Define the expected result and check to see if the expected output matches the actual output
            ResponseDTO expectedResponseDTO = new ResponseDTO() {
                Status= 400,
                Message= $"Cat Adoption Form with Guid '{testFormId}' has FormStatus '{badFormStatus}' when '{FormStatus.New}' was expected...",
            };

            bool formResponsesMatch =  expectedResponseDTO.Status.Equals(resultingResponseDto.Status) && 
                                expectedResponseDTO.Message.Equals(resultingResponseDto.Message);

            Assert.True(formResponsesMatch);
        }
    }
    
}