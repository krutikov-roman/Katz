/*

By: Roman Krutikov

Description: This test class is used for testing some of the methods/api endpoints of the AdminController.
             I know the requirement was only 3 methods per student, but I thought "more tests can't hurt :)"
             
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
            Guid testGuid = Guid.NewGuid();
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New
                },
                new CatAdoptionForm {
                    Id = Guid.NewGuid(),
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
                    Id = testGuid,
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
            Guid testGuid = Guid.NewGuid();
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.Accepted
                },
                new CatAdoptionForm {
                    Id = Guid.NewGuid(),
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
            Guid testGuid = Guid.NewGuid();
            Guid testGuid2 = Guid.NewGuid();
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New
                },
                new CatAdoptionForm {
                    Id = testGuid2,
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
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New
                },
                new CatAdoptionForm {
                    Id = testGuid2,
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
        public void GetAcceptedAdoptableCatForms_GetFrom2Total_Return1MatchingResult()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testGuid = Guid.NewGuid();
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.Accepted
                },
                new CatAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New
                }
            };
            databaseServiceMock.Setup(m => m.GetCatAdoptionFormsAsList(true)).Returns(catAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)admin.GetAcceptedAdoptableCatForms(testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<CatAdoptionForm> resultingCatAdoptionForms = ((IEnumerable<CatAdoptionForm>)resultingResponseDto.Data).ToList();

            // Define the expected result and check to see if the expected output matches the actual output
            List<CatAdoptionForm> catAdoptionFormsExcepted = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = testGuid,
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
        public void GetAcceptedAdoptableCatForms_GetFrom2Total_Return0MatchingResult()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testGuid = Guid.NewGuid();
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New
                },
                new CatAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New
                }
            };
            databaseServiceMock.Setup(m => m.GetCatAdoptionFormsAsList(true)).Returns(catAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)admin.GetAcceptedAdoptableCatForms(testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<CatAdoptionForm> resultingCatAdoptionForms = ((IEnumerable<CatAdoptionForm>)resultingResponseDto.Data).ToList();

            // Define the expected result and check to see if the expected output matches the actual output
            List<CatAdoptionForm> catAdoptionFormsExcepted = new List<CatAdoptionForm> ();
            bool formsMatch = resultingCatAdoptionForms.Count.Equals(catAdoptionFormsExcepted.Count);

            Assert.True(formsMatch);
        }

        [Fact]
        public void GetAcceptedAdoptableCatForms_GetFrom2Total_Return2MatchingResult()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testGuid = Guid.NewGuid();
            Guid testGuid2 = Guid.NewGuid();
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.Accepted
                },
                new CatAdoptionForm {
                    Id = testGuid2,
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.Accepted
                }
            };
            databaseServiceMock.Setup(m => m.GetCatAdoptionFormsAsList(true)).Returns(catAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)admin.GetAcceptedAdoptableCatForms(testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<CatAdoptionForm> resultingCatAdoptionForms = ((IEnumerable<CatAdoptionForm>)resultingResponseDto.Data).ToList();

            // Define the expected result and check to see if the expected output matches the actual output
            List<CatAdoptionForm> catAdoptionFormsExcepted = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New
                },
                new CatAdoptionForm {
                    Id = testGuid2,
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
        public void GetDeniedAdoptableCatForms_GetFrom2Total_Return1MatchingResult()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testGuid = Guid.NewGuid();
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.Denied
                },
                new CatAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New
                }
            };
            databaseServiceMock.Setup(m => m.GetCatAdoptionFormsAsList(true)).Returns(catAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)admin.GetDeniedAdoptableCatForms(testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<CatAdoptionForm> resultingCatAdoptionForms = ((IEnumerable<CatAdoptionForm>)resultingResponseDto.Data).ToList();

            // Define the expected result and check to see if the expected output matches the actual output
            List<CatAdoptionForm> catAdoptionFormsExcepted = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.Denied
                }
            };
            bool formsMatch = resultingCatAdoptionForms.Count.Equals(catAdoptionFormsExcepted.Count) &&
                                resultingCatAdoptionForms[0].Id.Equals(catAdoptionFormsExcepted[0].Id);

            Assert.True(formsMatch);
        }

        [Fact]
        public void GetDeniedAdoptableCatForms_GetFrom2Total_Return0MatchingResult()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testGuid = Guid.NewGuid();
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New
                },
                new CatAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New
                }
            };
            databaseServiceMock.Setup(m => m.GetCatAdoptionFormsAsList(true)).Returns(catAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)admin.GetDeniedAdoptableCatForms(testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<CatAdoptionForm> resultingCatAdoptionForms = ((IEnumerable<CatAdoptionForm>)resultingResponseDto.Data).ToList();

            // Define the expected result and check to see if the expected output matches the actual output
            List<CatAdoptionForm> catAdoptionFormsExcepted = new List<CatAdoptionForm> ();
            bool formsMatch = resultingCatAdoptionForms.Count.Equals(catAdoptionFormsExcepted.Count);

            Assert.True(formsMatch);
        }

        [Fact]
        public void GetDeniedAdoptableCatForms_GetFrom2Total_Return2MatchingResult()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testGuid = Guid.NewGuid();
            Guid testGuid2 = Guid.NewGuid();
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.Denied
                },
                new CatAdoptionForm {
                    Id = testGuid2,
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.Denied
                }
            };
            databaseServiceMock.Setup(m => m.GetCatAdoptionFormsAsList(true)).Returns(catAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)admin.GetDeniedAdoptableCatForms(testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<CatAdoptionForm> resultingCatAdoptionForms = ((IEnumerable<CatAdoptionForm>)resultingResponseDto.Data).ToList();

            // Define the expected result and check to see if the expected output matches the actual output
            List<CatAdoptionForm> catAdoptionFormsExcepted = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.Denied
                },
                new CatAdoptionForm {
                    Id = testGuid2,
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.Denied
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
            Guid approvingFormGuid = Guid.NewGuid();
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = approvingFormGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = Guid.NewGuid(),
                        Name = "TestCat1",
                        Description = "TestCat1",
                        CatStatus = CatStatus.WaitingForAdoption
                    }
                },
                new CatAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = Guid.NewGuid(),
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
            Guid testFormId = Guid.NewGuid();
            Guid testCatId = Guid.NewGuid();
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
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = Guid.NewGuid(),
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
        public async void AcceptNewAdoptableCatForms_ValidFormNullCat_Failed()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testFormId = Guid.NewGuid();
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = testFormId,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New,
                    Cat = null
                },
                new CatAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = Guid.NewGuid(),
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
            NotFoundObjectResult resultAsNotFound = (NotFoundObjectResult)result;
            ResponseDTO resultingResponseDto = (ResponseDTO) resultAsNotFound.Value;

            // Define the expected result and check to see if the expected output matches the actual output
            ResponseDTO expectedResponseDTO = new ResponseDTO() {
                Status= 404,
                Message= $"Cat specified in adoption form was null...",
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
            Guid testFormId = Guid.NewGuid();
            Guid testCatId = Guid.NewGuid();
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
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = Guid.NewGuid(),
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
            Guid testFormId = Guid.NewGuid();
            Guid testCatId = Guid.NewGuid();
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
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = Guid.NewGuid(),
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
        public async void AcceptNewAdoptableCatForms_NullForm_Failed()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testFormId = Guid.NewGuid();
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = Guid.NewGuid(),
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
            NotFoundObjectResult resultAsNotFoundObject = (NotFoundObjectResult) result;
            ResponseDTO resultingResponseDto = (ResponseDTO) resultAsNotFoundObject.Value;

            // Define the expected result and check to see if the expected output matches the actual output
            ResponseDTO expectedResponseDTO = new ResponseDTO() {
                Status= 404,
                Message= $"Cat Adoption Form with Guid '{testFormId}' not found...",
            };

            bool formResponsesMatch =  expectedResponseDTO.Status.Equals(resultingResponseDto.Status) && 
                                expectedResponseDTO.Message.Equals(resultingResponseDto.Message);

            Assert.True(formResponsesMatch);
        }

        [Fact]
        public async void DenyNewAdoptableCatForms_ValidFormValidCat_Success()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid approvingFormGuid = Guid.NewGuid();
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = approvingFormGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = Guid.NewGuid(),
                        Name = "TestCat1",
                        Description = "TestCat1",
                        CatStatus = CatStatus.WaitingForAdoption
                    }
                },
                new CatAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = Guid.NewGuid(),
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
            OkObjectResult result = (OkObjectResult)admin.DenyAdoptableCatForm(searchFormDTO, testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;

            // Define the expected result and check to see if the expected output matches the actual output
            ResponseDTO expectedResponseDTO = new ResponseDTO() {
                Status= 200,
                Message= "Successfully denied cat adoptable form!",
            };

            bool formResponsesMatch =  expectedResponseDTO.Status.Equals(resultingResponseDto.Status) && 
                                expectedResponseDTO.Message.Equals(resultingResponseDto.Message);

            Assert.True(formResponsesMatch);
        }

        [Fact]
        public async void DenyNewAdoptableCatForms_ValidFormInvalidCat_Failed()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testFormId = Guid.NewGuid();
            Guid testCatId = Guid.NewGuid();
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
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = Guid.NewGuid(),
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
            BadRequestObjectResult result = (BadRequestObjectResult)admin.DenyAdoptableCatForm(searchFormDTO, testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;

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
        public async void DenyNewAdoptableCatForms_ValidFormNullCat_Failed()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testFormId = Guid.NewGuid();
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = testFormId,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New,
                    Cat = null
                },
                new CatAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = Guid.NewGuid(),
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
            NotFoundObjectResult result = (NotFoundObjectResult)admin.DenyAdoptableCatForm(searchFormDTO, testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;

            // Define the expected result and check to see if the expected output matches the actual output
            ResponseDTO expectedResponseDTO = new ResponseDTO() {
                Status= 404,
                Message= $"Cat specified in adoption form was null...",
            };

            bool formResponsesMatch =  expectedResponseDTO.Status.Equals(resultingResponseDto.Status) && 
                                expectedResponseDTO.Message.Equals(resultingResponseDto.Message);

            Assert.True(formResponsesMatch);
        }

        [Fact]
        public async void DenyNewAdoptableCatForms_InvalidFormValidCat_Failed()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testFormId = Guid.NewGuid();
            Guid testCatId = Guid.NewGuid();
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
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = Guid.NewGuid(),
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
            BadRequestObjectResult result = (BadRequestObjectResult)admin.DenyAdoptableCatForm(searchFormDTO, testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;

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
        public async void DenyNewAdoptableCatForms_NullForm_Failed()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testFormId = Guid.NewGuid();
            List<CatAdoptionForm> catAdoptionFormsAll = new List<CatAdoptionForm> {
                new CatAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = Guid.NewGuid(),
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
            NotFoundObjectResult result = (NotFoundObjectResult)admin.DenyAdoptableCatForm(searchFormDTO, testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;

            // Define the expected result and check to see if the expected output matches the actual output
            ResponseDTO expectedResponseDTO = new ResponseDTO() {
                Status= 404,
                Message= $"Cat Adoption Form with Guid '{testFormId}' not found...",
            };

            bool formResponsesMatch =  expectedResponseDTO.Status.Equals(resultingResponseDto.Status) && 
                                expectedResponseDTO.Message.Equals(resultingResponseDto.Message);

            Assert.True(formResponsesMatch);
        }

        [Fact]
        public async void DenyNewAdoptableCatForms_InvalidFormInvalidCat_Failed()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testFormId = Guid.NewGuid();
            Guid testCatId = Guid.NewGuid();
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
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = Guid.NewGuid(),
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
            BadRequestObjectResult result = (BadRequestObjectResult)admin.DenyAdoptableCatForm(searchFormDTO, testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;

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
        public void GetNewCatsUpForAdoptionForms_GetFrom2Total_Return1MatchingResult()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testGuid = Guid.NewGuid();
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsAll = new List<CatPutUpForAdoptionForm> {
                new CatPutUpForAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New
                },
                new CatPutUpForAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.Accepted
                }
            };
            databaseServiceMock.Setup(m => m.GetCatPutUpForAdoptionFormsAsList(true)).Returns(catsPutUpForAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)admin.GetNewCatsUpForAdoptionForms(testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<CatPutUpForAdoptionForm> resultingCatsPutUpForAdoptionForms = ((IEnumerable<CatPutUpForAdoptionForm>)resultingResponseDto.Data).ToList();

            // Define the expected result and check to see if the expected output matches the actual output
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsExcepted = new List<CatPutUpForAdoptionForm> {
                new CatPutUpForAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New
                }
            };
            bool formsMatch = resultingCatsPutUpForAdoptionForms.Count.Equals(catsPutUpForAdoptionFormsExcepted.Count) &&
                                resultingCatsPutUpForAdoptionForms[0].Id.Equals(catsPutUpForAdoptionFormsExcepted[0].Id);

            Assert.True(formsMatch);
        }

        [Fact]
        public void GetNewCatsUpForAdoptionForms_GetFrom2Total_Return0MatchingResult()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testGuid = Guid.NewGuid();
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsAll = new List<CatPutUpForAdoptionForm> {
                new CatPutUpForAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.Accepted
                },
                new CatPutUpForAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.Accepted
                }
            };
            databaseServiceMock.Setup(m => m.GetCatPutUpForAdoptionFormsAsList(true)).Returns(catsPutUpForAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)admin.GetNewCatsUpForAdoptionForms(testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<CatPutUpForAdoptionForm> resultingCatsPutUpForAdoptionForms = ((IEnumerable<CatPutUpForAdoptionForm>)resultingResponseDto.Data).ToList();

            // Define the expected result and check to see if the expected output matches the actual output
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsExcepted = new List<CatPutUpForAdoptionForm>();
            bool formsMatch = resultingCatsPutUpForAdoptionForms.Count.Equals(catsPutUpForAdoptionFormsExcepted.Count);

            Assert.True(formsMatch);
        }

        [Fact]
        public void GetNewCatsUpForAdoptionForms_GetFrom2Total_Return2MatchingResult()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testGuid = Guid.NewGuid();
            Guid testGuid2 = Guid.NewGuid();
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsAll = new List<CatPutUpForAdoptionForm> {
                new CatPutUpForAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New
                },
                new CatPutUpForAdoptionForm {
                    Id = testGuid2,
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New
                }
            };
            databaseServiceMock.Setup(m => m.GetCatPutUpForAdoptionFormsAsList(true)).Returns(catsPutUpForAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)admin.GetNewCatsUpForAdoptionForms(testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<CatPutUpForAdoptionForm> resultingCatsPutUpForAdoptionForms = ((IEnumerable<CatPutUpForAdoptionForm>)resultingResponseDto.Data).ToList();

            // Define the expected result and check to see if the expected output matches the actual output
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsExcepted = new List<CatPutUpForAdoptionForm> {
                new CatPutUpForAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New
                },
                new CatPutUpForAdoptionForm {
                    Id = testGuid2,
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New
                }
            };
            bool formsMatch = resultingCatsPutUpForAdoptionForms.Count.Equals(catsPutUpForAdoptionFormsExcepted.Count) &&
                                resultingCatsPutUpForAdoptionForms[0].Id.Equals(catsPutUpForAdoptionFormsExcepted[0].Id) && 
                                resultingCatsPutUpForAdoptionForms[1].Id.Equals(catsPutUpForAdoptionFormsExcepted[1].Id);

            Assert.True(formsMatch);
        }

        [Fact]
        public void GetAcceptedCatsUpForAdoptionForms_GetFrom2Total_Return1MatchingResult()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testGuid = Guid.NewGuid();
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsAll = new List<CatPutUpForAdoptionForm> {
                new CatPutUpForAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.Accepted
                },
                new CatPutUpForAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New
                }
            };
            databaseServiceMock.Setup(m => m.GetCatPutUpForAdoptionFormsAsList(true)).Returns(catsPutUpForAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)admin.GetAcceptedCatsUpForAdoptionForms(testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<CatPutUpForAdoptionForm> resultingCatsPutUpForAdoptionForms = ((IEnumerable<CatPutUpForAdoptionForm>)resultingResponseDto.Data).ToList();

            // Define the expected result and check to see if the expected output matches the actual output
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsExcepted = new List<CatPutUpForAdoptionForm> {
                new CatPutUpForAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.Accepted
                }
            };
            bool formsMatch = resultingCatsPutUpForAdoptionForms.Count.Equals(catsPutUpForAdoptionFormsExcepted.Count) &&
                                resultingCatsPutUpForAdoptionForms[0].Id.Equals(catsPutUpForAdoptionFormsExcepted[0].Id);

            Assert.True(formsMatch);
        }

        [Fact]
        public void GetAcceptedCatsUpForAdoptionForms_GetFrom2Total_Return0MatchingResult()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testGuid = Guid.NewGuid();
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsAll = new List<CatPutUpForAdoptionForm> {
                new CatPutUpForAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New
                },
                new CatPutUpForAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New
                }
            };
            databaseServiceMock.Setup(m => m.GetCatPutUpForAdoptionFormsAsList(true)).Returns(catsPutUpForAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)admin.GetAcceptedCatsUpForAdoptionForms(testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<CatPutUpForAdoptionForm> resultingCatsPutUpForAdoptionForms = ((IEnumerable<CatPutUpForAdoptionForm>)resultingResponseDto.Data).ToList();

            // Define the expected result and check to see if the expected output matches the actual output
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsExcepted = new List<CatPutUpForAdoptionForm>();
            bool formsMatch = resultingCatsPutUpForAdoptionForms.Count.Equals(catsPutUpForAdoptionFormsExcepted.Count);

            Assert.True(formsMatch);
        }

        [Fact]
        public void GetAcceptedCatsUpForAdoptionForms_GetFrom2Total_Return2MatchingResult()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testGuid = Guid.NewGuid();
            Guid testGuid2 = Guid.NewGuid();
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsAll = new List<CatPutUpForAdoptionForm> {
                new CatPutUpForAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.Accepted
                },
                new CatPutUpForAdoptionForm {
                    Id = testGuid2,
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.Accepted
                }
            };
            databaseServiceMock.Setup(m => m.GetCatPutUpForAdoptionFormsAsList(true)).Returns(catsPutUpForAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)admin.GetAcceptedCatsUpForAdoptionForms(testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<CatPutUpForAdoptionForm> resultingCatsPutUpForAdoptionForms = ((IEnumerable<CatPutUpForAdoptionForm>)resultingResponseDto.Data).ToList();

            // Define the expected result and check to see if the expected output matches the actual output
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsExcepted = new List<CatPutUpForAdoptionForm> {
                new CatPutUpForAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.Accepted
                },
                new CatPutUpForAdoptionForm {
                    Id = testGuid2,
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.Accepted
                }
            };
            bool formsMatch = resultingCatsPutUpForAdoptionForms.Count.Equals(catsPutUpForAdoptionFormsExcepted.Count) &&
                                resultingCatsPutUpForAdoptionForms[0].Id.Equals(catsPutUpForAdoptionFormsExcepted[0].Id) && 
                                resultingCatsPutUpForAdoptionForms[1].Id.Equals(catsPutUpForAdoptionFormsExcepted[1].Id);

            Assert.True(formsMatch);
        }

        [Fact]
        public void GetDeniedCatsUpForAdoptionForms_GetFrom2Total_Return1MatchingResult()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testGuid = Guid.NewGuid();
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsAll = new List<CatPutUpForAdoptionForm> {
                new CatPutUpForAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.Denied
                },
                new CatPutUpForAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New
                }
            };
            databaseServiceMock.Setup(m => m.GetCatPutUpForAdoptionFormsAsList(true)).Returns(catsPutUpForAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)admin.GetDeniedCatsUpForAdoptionForms(testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<CatPutUpForAdoptionForm> resultingCatsPutUpForAdoptionForms = ((IEnumerable<CatPutUpForAdoptionForm>)resultingResponseDto.Data).ToList();

            // Define the expected result and check to see if the expected output matches the actual output
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsExcepted = new List<CatPutUpForAdoptionForm> {
                new CatPutUpForAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.Denied
                }
            };
            bool formsMatch = resultingCatsPutUpForAdoptionForms.Count.Equals(catsPutUpForAdoptionFormsExcepted.Count) &&
                                resultingCatsPutUpForAdoptionForms[0].Id.Equals(catsPutUpForAdoptionFormsExcepted[0].Id);

            Assert.True(formsMatch);
        }

        [Fact]
        public void GetDeniedCatsUpForAdoptionForms_GetFrom2Total_Return0MatchingResult()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testGuid = Guid.NewGuid();
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsAll = new List<CatPutUpForAdoptionForm> {
                new CatPutUpForAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New
                },
                new CatPutUpForAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New
                }
            };
            databaseServiceMock.Setup(m => m.GetCatPutUpForAdoptionFormsAsList(true)).Returns(catsPutUpForAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)admin.GetAcceptedCatsUpForAdoptionForms(testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<CatPutUpForAdoptionForm> resultingCatsPutUpForAdoptionForms = ((IEnumerable<CatPutUpForAdoptionForm>)resultingResponseDto.Data).ToList();

            // Define the expected result and check to see if the expected output matches the actual output
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsExcepted = new List<CatPutUpForAdoptionForm>();
            bool formsMatch = resultingCatsPutUpForAdoptionForms.Count.Equals(catsPutUpForAdoptionFormsExcepted.Count);

            Assert.True(formsMatch);
        }

        [Fact]
        public void GetDeniedCatsUpForAdoptionForms_GetFrom2Total_Return2MatchingResult()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid testGuid = Guid.NewGuid();
            Guid testGuid2 = Guid.NewGuid();
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsAll = new List<CatPutUpForAdoptionForm> {
                new CatPutUpForAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.Denied
                },
                new CatPutUpForAdoptionForm {
                    Id = testGuid2,
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.Denied
                }
            };
            databaseServiceMock.Setup(m => m.GetCatPutUpForAdoptionFormsAsList(true)).Returns(catsPutUpForAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)admin.GetDeniedCatsUpForAdoptionForms(testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<CatPutUpForAdoptionForm> resultingCatsPutUpForAdoptionForms = ((IEnumerable<CatPutUpForAdoptionForm>)resultingResponseDto.Data).ToList();

            // Define the expected result and check to see if the expected output matches the actual output
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsExcepted = new List<CatPutUpForAdoptionForm> {
                new CatPutUpForAdoptionForm {
                    Id = testGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.Denied
                },
                new CatPutUpForAdoptionForm {
                    Id = testGuid2,
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.Denied
                }
            };
            bool formsMatch = resultingCatsPutUpForAdoptionForms.Count.Equals(catsPutUpForAdoptionFormsExcepted.Count) &&
                                resultingCatsPutUpForAdoptionForms[0].Id.Equals(catsPutUpForAdoptionFormsExcepted[0].Id) && 
                                resultingCatsPutUpForAdoptionForms[1].Id.Equals(catsPutUpForAdoptionFormsExcepted[1].Id);

            Assert.True(formsMatch);
        }

        [Fact]
        public async void AcceptCatUpForAdoptionForm_ValidFormValidCat_Success()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid approvingFormGuid = Guid.NewGuid();
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsAll = new List<CatPutUpForAdoptionForm> {
                new CatPutUpForAdoptionForm {
                    Id = approvingFormGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = Guid.NewGuid(),
                        Name = "TestCat1",
                        Description = "TestCat1",
                        CatStatus = CatStatus.New
                    }
                },
                new CatPutUpForAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = Guid.NewGuid(),
                        Name = "TestCat2",
                        Description = "TestCat2",
                        CatStatus = CatStatus.WaitingForAdoption
                    }
                }
            };
            databaseServiceMock.Setup(m => m.GetCatPutUpForAdoptionFormsAsList(true)).Returns(catsPutUpForAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            SearchFormDTO searchFormDTO = new SearchFormDTO {
                Id = approvingFormGuid
            };
            OkObjectResult result = (OkObjectResult)admin.AcceptCatUpForAdoptionForm(searchFormDTO, testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;

            // Define the expected result and check to see if the expected output matches the actual output
            ResponseDTO expectedResponseDTO = new ResponseDTO() {
                Status= 200,
                Message= "Successfully approved cat put up for adoptable form!",
            };

            bool formResponsesMatch =  expectedResponseDTO.Status.Equals(resultingResponseDto.Status) && 
                                expectedResponseDTO.Message.Equals(resultingResponseDto.Message);

            Assert.True(formResponsesMatch);
        }

        [Fact]
        public async void AcceptCatUpForAdoptionForm_ValidFormInvalidCat_Success()
        {
            // Mock the usage of a bearer token when the API call is happening
            string testToken = "123";
            Mock<ITokenService> tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.isTokenActive(testToken)).Returns(true);

            // Mock the database with some data
            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            Guid approvingFormGuid = Guid.NewGuid();
            Guid testingCatGuid = Guid.NewGuid();
            CatStatus badCatStatus = CatStatus.Denied;
            List<CatPutUpForAdoptionForm> catsPutUpForAdoptionFormsAll = new List<CatPutUpForAdoptionForm> {
                new CatPutUpForAdoptionForm {
                    Id = approvingFormGuid,
                    OwnerName = "TestName1",
                    OwnerEmail = "TestEmail1",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = testingCatGuid,
                        Name = "TestCat1",
                        Description = "TestCat1",
                        CatStatus = badCatStatus
                    }
                },
                new CatPutUpForAdoptionForm {
                    Id = Guid.NewGuid(),
                    OwnerName = "TestName2",
                    OwnerEmail = "TestEmail2",
                    FormStatus = FormStatus.New,
                    Cat = new Cat {
                        Id = Guid.NewGuid(),
                        Name = "TestCat2",
                        Description = "TestCat2",
                        CatStatus = CatStatus.WaitingForAdoption
                    }
                }
            };
            databaseServiceMock.Setup(m => m.GetCatPutUpForAdoptionFormsAsList(true)).Returns(catsPutUpForAdoptionFormsAll);

            // Create the controller, apply mocking objects, and attempt to get the expected result
            AdminController admin = new AdminController(null,null,null,null,tokenServiceMock.Object,databaseServiceMock.Object);
            SearchFormDTO searchFormDTO = new SearchFormDTO {
                Id = approvingFormGuid
            };
            BadRequestObjectResult result = (BadRequestObjectResult)admin.AcceptCatUpForAdoptionForm(searchFormDTO, testToken);
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;

            // Define the expected result and check to see if the expected output matches the actual output
            ResponseDTO expectedResponseDTO = new ResponseDTO() {
                Status= 400,
                Message= $"Cat with Guid '{testingCatGuid}' has CatStatus '{badCatStatus}' when '{CatStatus.New}' was expected...",
            };

            bool formResponsesMatch =  expectedResponseDTO.Status.Equals(resultingResponseDto.Status) && 
                                expectedResponseDTO.Message.Equals(resultingResponseDto.Message);

            Assert.True(formResponsesMatch);
        }

    }
    
}