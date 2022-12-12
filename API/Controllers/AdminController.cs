/*

By: Roman Krutikov

Description: This class is used for API endpoints that an Admin Page of the Cat Adoption website would use.
             This mainly involves the listing, accepting and denying Cat Adoption Forms and Cats Put Up For
             For Adoption Forms.
             This class also includes testing methods for authorization, such as testing how the API would
             response to requests with and without certain types of authentcation. (May be removed before final
             submission).
             
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using API.Models;
using API.Models.DTOs;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private UserManager<AppUser> _userManager;

        private SignInManager<AppUser> _signInManager;

        private TokenService _tokenService;

        private Database _database;

        public AdminController (UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TokenService tokenService, Database database){
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService; 
            _database = database;
        }

        #region Login and Test Methods

        [HttpPost("signup")]
        public async Task<IActionResult> Signup (RegisterDto registerDto){
            var user = new AppUser {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                IsAdmin = true
            };

            var result = await _userManager.CreateAsync(user,registerDto.Password);
            if (result.Succeeded){
                ResponseDTO responseDTO = new ResponseDTO() {
                    Status= 200,
                    Message= "Signup was successful!",
                };
                return Ok(responseDTO);
            }

            ResponseDTO responseDTOErrors = new ResponseDTO() {
                Status= 400,
                Message= "Signup resulted in errors...",
                Errors = result.Errors
            };
            return BadRequest(responseDTOErrors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user==null) {
                ResponseDTO responseDTONotFound = new ResponseDTO() {
                    Status= 404,
                    Message= "Email searched does not exist..."
                };
                return NotFound(responseDTONotFound);
            }
            
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (result.Succeeded) {
                string tokenCreated = _tokenService.CreateToken(user, ipAddress);
                ResponseDTO responseDTOOk = new ResponseDTO() {
                    Status= 200,
                    Message= "Successfully logged in!",
                    Data = tokenCreated
                };
                return Ok(responseDTOOk);
            }

            ResponseDTO responseDTOUnAuthorized = new ResponseDTO() {
                Status= 401,
                Message= "Unauthorized login..."
            };
            return Unauthorized(responseDTOUnAuthorized); 
        }

        
        [HttpPost("signout")]
        public async Task<IActionResult> SignOutAsync(LogoutDTO logoutDTO)
        {
            _tokenService.RevokeToken(logoutDTO.Token);
            ResponseDTO responseDTOOk = new ResponseDTO() {
                Status= 200,
                Message= "Successfully signed out!"
            };
            return Ok(responseDTOOk);
        }

        [HttpGet("notSecure")]
        public IActionResult GetUnsecureText()
        {
            ResponseDTO responseDTOOk = new ResponseDTO() {
                Status= 200,
                Message= "Do you have an account with us?"
            };
            return Ok(responseDTOOk);
        }

        [Authorize]
        [HttpGet("secure")]
        public IActionResult GetSecureText()
        {
            var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
            if (!_tokenService.isTokenActive(tokenAuthorization)){
                ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                    Status= 401,
                    Message= "Unauthorized token login..."
                };
                return Unauthorized(responseDTOUnauthorized);
            }
            
            ResponseDTO responseDTOOk = new ResponseDTO() {
                Status= 200,
                Message= "You have an account with us"
            };
            return Ok(responseDTOOk);
        }

        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpGet("secureAdmin")]
        public IActionResult GetSecureText2()
        {
            var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
            if (!_tokenService.isTokenActive(tokenAuthorization)){
                ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                    Status= 401,
                    Message= "Unauthorized token login..."
                };
                return Unauthorized(responseDTOUnauthorized);
            }

            ResponseDTO responseDTOOk = new ResponseDTO() {
                Status= 200,
                Message= "You are an admin"
            };
            return Ok(responseDTOOk);
        }

        #endregion

        #region Cat Adoption Methods
        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpGet("getNewAdoptableCatForms")]
        public IActionResult GetNewAdoptableCatForms()
        {
            var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
            if (!_tokenService.isTokenActive(tokenAuthorization)){
                ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                    Status= 401,
                    Message= "Unauthorized token login..."
                };
                return Unauthorized(responseDTOUnauthorized);
            }

            IEnumerable<CatAdoptionForm> catAdoptionForms = _database.CatAdoptionForms.Where(x => x.FormStatus.Equals(FormStatus.New));
            ResponseDTO responseDTOOk = new ResponseDTO() {
                Status= 200,
                Message= "Successfully fetched New Adoptable Cat Forms!",
                Data = catAdoptionForms
            };
            return Ok(responseDTOOk);
        }

        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpGet("getAcceptedAdoptableCatForms")]
        public IActionResult GetAcceptedAdoptableCatForms()
        {
            var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
            if (!_tokenService.isTokenActive(tokenAuthorization)){
                ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                    Status= 401,
                    Message= "Unauthorized token login..."
                };
                return Unauthorized(responseDTOUnauthorized);
            }

            IEnumerable<CatAdoptionForm> catAdoptionForms = _database.CatAdoptionForms.Where(x => x.FormStatus.Equals(FormStatus.Accepted));
            ResponseDTO responseDTOOk = new ResponseDTO() {
                Status= 200,
                Message= "Successfully fetched Accepted Adoptable Cat Forms!",
                Data = catAdoptionForms
            };
            return Ok(responseDTOOk);
        }

        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpGet("getDeniedAdoptableCatForms")]
        public IActionResult GetDeniedAdoptableCatForms()
        {
            var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
            if (!_tokenService.isTokenActive(tokenAuthorization)){
                ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                    Status= 401,
                    Message= "Unauthorized token login..."
                };
                return Unauthorized(responseDTOUnauthorized);
            }

            IEnumerable<CatAdoptionForm> catAdoptionForms = _database.CatAdoptionForms.Where(x => x.FormStatus.Equals(FormStatus.Denied));
            ResponseDTO responseDTOOk = new ResponseDTO() {
                Status= 200,
                Message= "Successfully fetched Denied Adoptable Cat Forms!",
                Data = catAdoptionForms
            };
            return Ok(responseDTOOk);
        }

        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpPost("acceptAdoptableCatForm")]
        public IActionResult AcceptAdoptableCatForm(CatAdoptionForm catAdoptionForm)
        {
            var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
            if (!_tokenService.isTokenActive(tokenAuthorization)){
                ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                    Status= 401,
                    Message= "Unauthorized token login..."
                };
                return Unauthorized(responseDTOUnauthorized);
            }

            CatAdoptionForm? catAdoptionFormFromDatabase = _database.CatAdoptionForms
                .Include(x => x.Cat).FirstOrDefault(x => x.Id.Equals(catAdoptionForm.Id));
            if (catAdoptionFormFromDatabase == null){
                ResponseDTO responseDTONotFound = new ResponseDTO() {
                    Status= 404,
                    Message= $"Cat Adoption Form with Guid '${catAdoptionForm.Id}' not found...",
                };
                return NotFound(responseDTONotFound);
            }
            catAdoptionFormFromDatabase.FormStatus = FormStatus.Accepted;
            
            Cat? catFromAdoptionFormFromDatabase = catAdoptionFormFromDatabase.Cat;
            if (catFromAdoptionFormFromDatabase == null){
                ResponseDTO responseDTONotFound = new ResponseDTO() {
                    Status= 404,
                    Message= "Cat specified in adoption form was null...",
                };
                return NotFound(responseDTONotFound);
            }
            catFromAdoptionFormFromDatabase.CatStatus = CatStatus.Adopted;

            ResponseDTO responseDTOOk = new ResponseDTO() {
                Status= 200,
                Message= "Successfully approved cat adoptable form!",
            };

            _database.SaveChanges();
            return Ok(responseDTOOk);
        }

        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpPost("denyAdoptableCatForm")]
        public IActionResult DenyAdoptableCatForm(CatAdoptionForm catAdoptionForm)
        {
            var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
            if (!_tokenService.isTokenActive(tokenAuthorization)){
                ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                    Status= 401,
                    Message= "Unauthorized token login..."
                };
                return Unauthorized(responseDTOUnauthorized);
            }

            CatAdoptionForm? catAdoptionFormFromDatabase = _database.CatAdoptionForms
                .Include(x => x.Cat).FirstOrDefault(x => x.Id.Equals(catAdoptionForm.Id));
            if (catAdoptionFormFromDatabase == null){
                ResponseDTO responseDTONotFound = new ResponseDTO() {
                    Status= 404,
                    Message= $"Cat Adoption Form with Guid '{catAdoptionForm.Id}' not found...",
                };
                return NotFound(responseDTONotFound);
            }
            catAdoptionFormFromDatabase.FormStatus = FormStatus.Denied;
            
            Cat? catFromAdoptionFormFromDatabase = catAdoptionFormFromDatabase.Cat;
            if (catFromAdoptionFormFromDatabase == null){
                ResponseDTO responseDTONotFound = new ResponseDTO() {
                    Status= 404,
                    Message= "Cat specified in adoption form was null...",
                };
                return NotFound(responseDTONotFound);
            }
            catFromAdoptionFormFromDatabase.CatStatus = CatStatus.WaitingForAdoption;

            ResponseDTO responseDTOOk = new ResponseDTO() {
                Status= 200,
                Message= "Successfully denied cat adoptable form!",
            };

            _database.SaveChanges();
            return Ok(responseDTOOk);
        }

        #endregion

        #region Cats Up For Adoption Region
        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpGet("getNewCatsUpForAdoptionForms")]
        public IActionResult GetNewCatsUpForAdoptionForms() {
            var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
            if (!_tokenService.isTokenActive(tokenAuthorization)){
                ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                    Status= 401,
                    Message= "Unauthorized token login..."
                };
                return Unauthorized(responseDTOUnauthorized);
            }

            IEnumerable<CatPutUpForAdoptionForm> catAdoptionForms = _database.CatPutUpForAdoptionForms.Where(x => x.FormStatus.Equals(FormStatus.New));
            ResponseDTO responseDTOOk = new ResponseDTO() {
                Status= 200,
                Message= "Successfully fetched New Cats Up For Adoption Forms!",
                Data = catAdoptionForms
            };
            return Ok(responseDTOOk);
        }

        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpGet("getAcceptedCatsUpForAdoptionForms")]
        public IActionResult GetAcceptedCatsUpForAdoptionForms() {
            var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
            if (!_tokenService.isTokenActive(tokenAuthorization)){
                ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                    Status= 401,
                    Message= "Unauthorized token login..."
                };
                return Unauthorized(responseDTOUnauthorized);
            }

            IEnumerable<CatPutUpForAdoptionForm> catAdoptionForms = _database.CatPutUpForAdoptionForms.Where(x => x.FormStatus.Equals(FormStatus.Accepted));
            ResponseDTO responseDTOOk = new ResponseDTO() {
                Status= 200,
                Message= "Successfully fetched Accepted Cats Up For Adoption Forms!",
                Data = catAdoptionForms
            };
            return Ok(responseDTOOk);
        }

        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpGet("getDeniedCatUpForAdoptionForm")]
        public IActionResult GetDeniedCatsUpForAdoptionForms() {
            var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
            if (!_tokenService.isTokenActive(tokenAuthorization)){
                ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                    Status= 401,
                    Message= "Unauthorized token login..."
                };
                return Unauthorized(responseDTOUnauthorized);
            }

            IEnumerable<CatPutUpForAdoptionForm> catAdoptionForms = _database.CatPutUpForAdoptionForms.Where(x => x.FormStatus.Equals(FormStatus.Denied));
            ResponseDTO responseDTOOk = new ResponseDTO() {
                Status= 200,
                Message= "Successfully fetched Denied Cats Up For Adoption Forms!",
                Data = catAdoptionForms
            };
            return Ok(responseDTOOk);
        }

        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpPost("acceptCatUpForAdoptionForm")]
        public IActionResult AcceptAdoptableCatForm(CatPutUpForAdoptionForm catPutUpForAdoptionForm)
        {
            var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
            if (!_tokenService.isTokenActive(tokenAuthorization)){
                ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                    Status= 401,
                    Message= "Unauthorized token login..."
                };
                return Unauthorized(responseDTOUnauthorized);
            }

            CatPutUpForAdoptionForm? catPutUpForAdoptionFormFromDatabase = _database.CatPutUpForAdoptionForms
                .Include(x => x.Cat).FirstOrDefault(x => x.Id.Equals(catPutUpForAdoptionForm.Id));
            if (catPutUpForAdoptionFormFromDatabase == null){
                ResponseDTO responseDTONotFound = new ResponseDTO() {
                    Status= 404,
                    Message= $"Cat Put Up For Adoption Form with Guid '{catPutUpForAdoptionForm.Id}' not found...",
                };
                return NotFound(responseDTONotFound);
            }
            catPutUpForAdoptionFormFromDatabase.FormStatus = FormStatus.Accepted;
            
            Cat? catFromAdoptionFormFromDatabase = catPutUpForAdoptionFormFromDatabase.Cat;
            if (catFromAdoptionFormFromDatabase == null){
                ResponseDTO responseDTONotFound = new ResponseDTO() {
                    Status= 404,
                    Message= "Cat specified in adoption form was null...",
                };
                return NotFound(responseDTONotFound);
            }
            catFromAdoptionFormFromDatabase.CatStatus = CatStatus.WaitingForAdoption;

            ResponseDTO responseDTOOk = new ResponseDTO() {
                Status= 200,
                Message= "Successfully approved cat put up for adoptable form!",
            };

            _database.SaveChanges();
            return Ok(responseDTOOk);
        }

        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpPost("denyCatUpForAdoptionForm")]
        public IActionResult DenyAdoptableCatForm(CatPutUpForAdoptionForm catPutUpForAdoptionForm)
        {
            var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
            if (!_tokenService.isTokenActive(tokenAuthorization)){
                ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                    Status= 401,
                    Message= "Unauthorized token login..."
                };
                return Unauthorized(responseDTOUnauthorized);
            }

            CatPutUpForAdoptionForm? catUpForAdoptionFormFromDatabase = _database.CatPutUpForAdoptionForms
                .Include(x => x.Cat).FirstOrDefault(x => x.Id.Equals(catPutUpForAdoptionForm.Id));
            if (catUpForAdoptionFormFromDatabase == null){
                ResponseDTO responseDTONotFound = new ResponseDTO() {
                    Status= 404,
                    Message= "Cat Adoption Form with Guid 'catAdoptionForm.Id' not found...",
                };
                return NotFound(responseDTONotFound);
            }
            catUpForAdoptionFormFromDatabase.FormStatus = FormStatus.Denied;
            
            Cat? catFromAdoptionFormFromDatabase = catUpForAdoptionFormFromDatabase.Cat;
            if (catFromAdoptionFormFromDatabase == null){
                ResponseDTO responseDTONotFound = new ResponseDTO() {
                    Status= 404,
                    Message= "Cat specified in adoption form was null...",
                };
                return NotFound(responseDTONotFound);
            }
            catFromAdoptionFormFromDatabase.CatStatus = CatStatus.Denied;

            ResponseDTO responseDTOOk = new ResponseDTO() {
                Status= 200,
                Message= "Successfully denied cat put up for adoptable form!",
            };

            _database.SaveChanges();
            return Ok(responseDTOOk);
        }
        #endregion

    }
}