/*

By: Karen James
Description: used for testing methods in the CatController

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
    public class TestCatController
    {

        [Fact]
        public void GetCatsAvailableForAdoption_GetFrom2Total_Return1MatchingResult()
        {
            Guid id = Guid.NewGuid();

            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            List<Cat> cats = new List<Cat> {
                new Cat {
                    Id = id,
                    Name = "TestName1",
                    Description = "Desc1",
                    CatStatus = CatStatus.WaitingForAdoption
                },
                new Cat {
                    Id = Guid.NewGuid(),
                    Name = "TestName2",
                    Description = "Desc2",
                    CatStatus = CatStatus.Denied
                }
            };
            databaseServiceMock.Setup(m => m.GetCatsAsList()).Returns(cats);

            CatController catController = new CatController(null,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)catController.GetCatsAvailableForAdoption();
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<Cat> resultingCatList = ((IEnumerable<Cat>)resultingResponseDto.Data).ToList();

            List<Cat> catsExpected = new List<Cat> {
                new Cat {
                    Id = id,
                    Name = "TestName1",
                    Description = "Desc1",
                    CatStatus = CatStatus.WaitingForAdoption
                },
            };
            bool catsInListMatch = resultingCatList.Count.Equals(catsExpected.Count) &&
                resultingCatList[0].Id.Equals(catsExpected[0].Id);

            Assert.True(catsInListMatch);
        }


        [Fact]
        public void GetCatsAvailableForAdoption_GetFrom2Total_Return0MatchingResult()
        {
            Guid id = Guid.NewGuid();

            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            List<Cat> cats = new List<Cat> {
                new Cat {
                    Id = Guid.NewGuid(),
                    Name = "TestName1",
                    Description = "Desc1",
                    CatStatus = CatStatus.Adopted
                },
                new Cat {
                    Id = Guid.NewGuid(),
                    Name = "TestName2",
                    Description = "Desc2",
                    CatStatus = CatStatus.Denied
                }
            };
            databaseServiceMock.Setup(m => m.GetCatsAsList()).Returns(cats);

            CatController cat = new CatController(null,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)cat.GetCatsAvailableForAdoption();
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<Cat> resultingCatList = ((IEnumerable<Cat>)resultingResponseDto.Data).ToList();

            List<Cat> catsExpected = new List<Cat> ();
            bool catsInListMatch = resultingCatList.Count.Equals(catsExpected.Count);

            Assert.True(catsInListMatch);
        }

        [Fact]
        public void GetCatsAvailableForAdoption_GetFrom2Total_Return2MatchingResult()
        {
            Guid id = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();


            Mock<IListableDatabase> databaseServiceMock = new Mock<IListableDatabase>();
            List<Cat> cats = new List<Cat> {
                new Cat {
                    Id = id,
                    Name = "TestName1",
                    Description = "Desc1",
                    CatStatus = CatStatus.WaitingForAdoption
                },
                new Cat {
                    Id = id2,
                    Name = "TestName2",
                    Description = "Desc2",
                    CatStatus = CatStatus.WaitingForAdoption
                }
            };
            databaseServiceMock.Setup(m => m.GetCatsAsList()).Returns(cats);

            CatController cat = new CatController(null,databaseServiceMock.Object);
            OkObjectResult result = (OkObjectResult)cat.GetCatsAvailableForAdoption();
            ResponseDTO resultingResponseDto = (ResponseDTO) result.Value;
            List<Cat> resultingCatList = ((IEnumerable<Cat>)resultingResponseDto.Data).ToList();

            List<Cat> catsExpected = new List<Cat> {
                new Cat {
                    Id = id,
                    Name = "TestName1",
                    Description = "Desc1",
                    CatStatus = CatStatus.WaitingForAdoption
                },
                new Cat {
                    Id = id2,
                    Name = "TestName2",
                    Description = "Desc2",
                    CatStatus = CatStatus.WaitingForAdoption
                }
            };
            bool catsInListMatch =  resultingCatList.Count.Equals(catsExpected.Count) &&
                                resultingCatList[0].Id.Equals(catsExpected[0].Id) && 
                                resultingCatList[1].Id.Equals(catsExpected[1].Id);

            Assert.True(catsInListMatch);
        }
    }
}