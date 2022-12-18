using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Models.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormsController : ControllerBase
    {
        private Database _database;

        public FormsController(Database database)
        {
            _database = database;
        }

        [HttpGet("getAdoptableCats")]
        public IActionResult GetAdoptableCats()
        {
            try
            {
                IEnumerable<Cat> cats = _database.GetCatsAsList().Where(c => c.CatStatus.Equals(CatStatus.WaitingForAdoption));

                ResponseDTO responseDTOOk = new ResponseDTO()
                {
                    Status = 200,
                    Message = "Successfully fetched adoptable cats",
                    Data = cats
                };

                return Ok(responseDTOOk);
            }
            catch (Exception e)
            {
                ResponseDTO responseDTOError = new ResponseDTO
                {
                    Status = 400,
                    Message = "An unexpected server error occurred",
                    Errors = e
                };

                return BadRequest(responseDTOError);
            }
        }

        [HttpPost("requestCatForAdoption")]
        public async Task<IActionResult> CreateCatForAdoptionAsync(CatPutUpForAdoptionForm requestCatForAdoptionForm)
        {
            try
            {
                requestCatForAdoptionForm.FormStatus = FormStatus.New;

                Cat cat = requestCatForAdoptionForm.Cat;
                cat.CatStatus = CatStatus.New;

                ResponseDTO responseDTOOk = new ResponseDTO()
                {
                    Status = 200,
                    Message = "Successfully sent the request to put a cat up for adoption",
                    Data = requestCatForAdoptionForm
                };

                _database.AddToCatAdoptionForms(requestCatForAdoptionForm);
                await _database.SaveChangesAsync();

                return Ok(responseDTOOk);
            }
            catch (Exception e)
            {
                ResponseDTO responseDTOError = new ResponseDTO
                {
                    Status = 400,
                    Message = "An unexpected server error occurred",
                    Errors = e
                };

                return BadRequest(responseDTOError);
            }
        }

        [HttpPost("requestToAdoptCat")]
        public async Task<IActionResult> AdoptCatAsync(AdoptCatDto adoptionFormDto)
        {
            try
            {
                Cat? cat = _database.GetCatsAsList().FirstOrDefault(c => c.Id.ToString().ToLower().Equals(adoptionFormDto.CatId.ToString().ToLower()))??null;
                if (cat == null)
                {
                    ResponseDTO responseDTOError = new ResponseDTO
                    {
                        Status = 400,
                        Message = "Cat is null",
                    };

                    return BadRequest(responseDTOError);
                }
                if (cat.CatStatus != CatStatus.WaitingForAdoption)
                {
                    ResponseDTO responseDTOError = new ResponseDTO
                    {
                        Status = 400,
                        Message = "Cat is not adoptable",
                    };

                    return BadRequest(responseDTOError);
                }

                CatAdoptionForm adoptionForm = new CatAdoptionForm 
                {
                    FormStatus = FormStatus.New,
                    OwnerName = adoptionFormDto.OwnerName,
                    OwnerEmail = adoptionFormDto.OwnerEmail,
                    Cat = cat
                };

                ResponseDTO responseDTOOk = new ResponseDTO
                {
                    Status = 200,
                    Message = "Successfully sent the request to adopt the cat",
                    Data = adoptionForm
                };

                _database.AddToAdoptCatForms(adoptionForm);
                await _database.SaveChangesAsync();

                return Ok(responseDTOOk);
            }
            catch (Exception e)
            {
                ResponseDTO responseDTOError = new ResponseDTO
                {
                    Status = 400,
                    Message = "An unexpected server error occurred",
                    Errors = e
                };

                return BadRequest(responseDTOError);
            }
        }
    }
}