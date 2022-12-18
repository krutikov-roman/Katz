using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Models.DTOs;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatController : ControllerBase
    {
        private Database _database;

        public CatController(Database database)
        {
            _database = database;
        }

        [HttpGet("getAdoptableCats")]
        public IActionResult GetCatsAvailableForAdoption()
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
    }
}