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

        #region Test Methods

        // Signs up a user (likely admin, but admin value set to false by default)
        [HttpPost("signup")]
        public async Task<IActionResult> Signup (RegisterDto registerDto){
            var user = new AppUser {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                IsAdmin = false
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

        // Test to see if an API response is given when no bearer token is used
        [HttpGet("notSecure")]
        public IActionResult GetUnsecureText()
        {
            ResponseDTO responseDTOOk = new ResponseDTO() {
                Status= 200,
                Message= "Do you have an account with us?"
            };
            return Ok(responseDTOOk);
        }

        // Test to see if an API response is given when a bearer token of a valid user is used
        [Authorize]
        [HttpGet("secure")]
        public IActionResult GetSecureText()
        {
            // Custom way of checking for if a signed out token is being used to log in
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

        // Test to see if an API response is given when a bearer token of an admin user
        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpGet("secureAdmin")]
        public IActionResult GetSecureText2()
        {
            // Custom way of checking for if a signed out token is being used to log in
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

        #region Login and Sign Out
        
        // Logs a user (likely admin) in and returns a bearer token
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

            // Use the IP address of the user when generating their token
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

        // Signs a user (likely admin) out and invalidates their bearer token
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
        #endregion

        #region Cat Adoption Methods

        // This method fetches all newly created cat adoption form applications
        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpGet("getNewAdoptableCatForms")]
        public IActionResult GetNewAdoptableCatForms()
        {

            try {
                // Custom way of checking for if a signed out token is being used to log in
                var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
                if (!_tokenService.isTokenActive(tokenAuthorization)){
                    ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                        Status= 401,
                        Message= "Unauthorized token login..."
                    };
                    return Unauthorized(responseDTOUnauthorized);
                }

                // Fetches all new cat adoption forms from the database
                IEnumerable<CatAdoptionForm> catAdoptionForms = _database.CatAdoptionForms.Include(x => x.Cat).Where(x => x.FormStatus.Equals(FormStatus.New));
                ResponseDTO responseDTOOk = new ResponseDTO() {
                    Status= 200,
                    Message= "Successfully fetched New Adoptable Cat Forms!",
                    Data = catAdoptionForms
                };
                return Ok(responseDTOOk);
            }
            catch (Exception e){
                ResponseDTO responseDTOError = new ResponseDTO {
                    Status = 400,
                    Message = "An unexpected server error occurred",
                    Errors = e
                };
                return BadRequest(responseDTOError);
            }
        }

        // This method fetches all accepted cat adoption form applications
        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpGet("getAcceptedAdoptableCatForms")]
        public IActionResult GetAcceptedAdoptableCatForms()
        {
            try {
                // Custom way of checking for if a signed out token is being used to log in
                var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
                if (!_tokenService.isTokenActive(tokenAuthorization)){
                    ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                        Status= 401,
                        Message= "Unauthorized token login..."
                    };
                    return Unauthorized(responseDTOUnauthorized);
                }

                // Fetches all accepted cat adoption forms from the database
                IEnumerable<CatAdoptionForm> catAdoptionForms = _database.CatAdoptionForms.Include(x => x.Cat).Where(x => x.FormStatus.Equals(FormStatus.Accepted));
                ResponseDTO responseDTOOk = new ResponseDTO() {
                    Status= 200,
                    Message= "Successfully fetched Accepted Adoptable Cat Forms!",
                    Data = catAdoptionForms
                };
                return Ok(responseDTOOk);
            }
            catch (Exception e){
                ResponseDTO responseDTOError = new ResponseDTO {
                    Status = 400,
                    Message = "An unexpected server error occurred",
                    Errors = e
                };
                return BadRequest(responseDTOError);
            }
        }

        // This method fetches all denied cat adoption form applications
        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpGet("getDeniedAdoptableCatForms")]
        public IActionResult GetDeniedAdoptableCatForms()
        {

            try {
                // Custom way of checking for if a signed out token is being used to log in
                var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
                if (!_tokenService.isTokenActive(tokenAuthorization)){
                    ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                        Status= 401,
                        Message= "Unauthorized token login..."
                    };
                    return Unauthorized(responseDTOUnauthorized);
                }

                // Fetches all denied cat adoption forms from the database
                IEnumerable<CatAdoptionForm> catAdoptionForms = _database.CatAdoptionForms.Include(x => x.Cat).Where(x => x.FormStatus.Equals(FormStatus.Denied));
                ResponseDTO responseDTOOk = new ResponseDTO() {
                    Status= 200,
                    Message= "Successfully fetched Denied Adoptable Cat Forms!",
                    Data = catAdoptionForms
                };
                return Ok(responseDTOOk);
            }
            catch (Exception e){
                ResponseDTO responseDTOError = new ResponseDTO {
                    Status = 400,
                    Message = "An unexpected server error occurred",
                    Errors = e
                };
                return BadRequest(responseDTOError);
            }
        }

        // This method accepts a cat adoption form if the form has not already been accepted/denied and
        // if the Cat has not already been adopted, or has not yet been processed, or if the Cat
        // is not sutable for adoption.
        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpPost("acceptAdoptableCatForm")]
        public async Task<IActionResult> AcceptAdoptableCatForm(CatAdoptionForm catAdoptionForm)
        {

            try {
                // Custom way of checking for if a signed out token is being used to log in
                var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
                if (!_tokenService.isTokenActive(tokenAuthorization)){
                    ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                        Status= 401,
                        Message= "Unauthorized token login..."
                    };
                    return Unauthorized(responseDTOUnauthorized);
                }
            
                // Fetch the ID from the form and see if a matching form exists in the database
                // As well, check to make sure the form is not already Accepted or Denied
                CatAdoptionForm? catAdoptionFormFromDatabase = _database.CatAdoptionForms.Include(x => x.Cat)
                    .FirstOrDefault(x => x.Id.ToString().ToLower().Equals(catAdoptionForm.Id.ToString().ToLower()));
                if (catAdoptionFormFromDatabase == null){
                    ResponseDTO responseDTONotFound = new ResponseDTO() {
                        Status= 404,
                        Message= $"Cat Adoption Form with Guid '{catAdoptionForm.Id}' not found...",
                    };
                    return NotFound(responseDTONotFound);
                }
                if (catAdoptionFormFromDatabase.FormStatus == FormStatus.Accepted || catAdoptionFormFromDatabase.FormStatus == FormStatus.Denied){
                    ResponseDTO responseDTOBadRequest = new ResponseDTO() {
                        Status= 400,
                        Message= $"Cat Adoption Form with Guid '{catAdoptionFormFromDatabase.Id}' has FormStatus '{catAdoptionFormFromDatabase.FormStatus}' when '{FormStatus.New}' was expected...",
                    };
                    return BadRequest(responseDTOBadRequest);
                }
                catAdoptionFormFromDatabase.FormStatus = FormStatus.Accepted;

                // Check to make sure the Cat from the form exists
                // As well, check to make sure the Cat is not already Adopted, or needs processing, or is not sutable for adoption
                Cat? catFromAdoptionFormFromDatabase = catAdoptionFormFromDatabase.Cat;
                if (catFromAdoptionFormFromDatabase == null){
                    ResponseDTO responseDTONotFound = new ResponseDTO() {
                        Status= 404,
                        Message= "Cat specified in adoption form was null...",
                    };
                    return NotFound(responseDTONotFound);
                }
                if (catFromAdoptionFormFromDatabase.CatStatus == CatStatus.Adopted || catFromAdoptionFormFromDatabase.CatStatus == CatStatus.Denied || catFromAdoptionFormFromDatabase.CatStatus == CatStatus.New){
                    ResponseDTO responseDTOBadRequest = new ResponseDTO() {
                        Status= 400,
                        Message= $"Cat with Guid '{catFromAdoptionFormFromDatabase.Id}' has CatStatus '{catFromAdoptionFormFromDatabase.CatStatus}' when '{CatStatus.WaitingForAdoption}' was expected...",
                    };
                    return BadRequest(responseDTOBadRequest);
                }
                catFromAdoptionFormFromDatabase.CatStatus = CatStatus.Adopted;

                ResponseDTO responseDTOOk = new ResponseDTO() {
                    Status= 200,
                    Message= "Successfully approved cat adoptable form!",
                };

                await _database.SaveChangesAsync();
                return Ok(responseDTOOk);
            }
            catch (Exception e){
                ResponseDTO responseDTOError = new ResponseDTO {
                    Status = 400,
                    Message = "An unexpected server error occurred",
                    Errors = e
                };
                return BadRequest(responseDTOError);
            }
        }

        // This method denies a cat adoption form if the form has not already been accepted/denied and
        // if the Cat has not already been adopted, or has not yet been processed, or if the Cat
        // is not sutable for adoption.
        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpPost("denyAdoptableCatForm")]
        public IActionResult DenyAdoptableCatForm(CatAdoptionForm catAdoptionForm)
        {

            try {
                // Custom way of checking for if a signed out token is being used to log in
                var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
                if (!_tokenService.isTokenActive(tokenAuthorization)){
                    ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                        Status= 401,
                        Message= "Unauthorized token login..."
                    };
                    return Unauthorized(responseDTOUnauthorized);
                }

                // Fetch the ID from the form and see if a matching form exists in the database
                // As well, check to make sure the form is not already Accepted or Denied
                CatAdoptionForm? catAdoptionFormFromDatabase = _database.CatAdoptionForms.Include(x => x.Cat)
                    .FirstOrDefault(x => x.Id.ToString().ToLower().Equals(catAdoptionForm.Id.ToString().ToLower()));
                if (catAdoptionFormFromDatabase == null){
                    ResponseDTO responseDTONotFound = new ResponseDTO() {
                        Status= 404,
                        Message= $"Cat Adoption Form with Guid '{catAdoptionForm.Id}' not found...",
                    };
                    return NotFound(responseDTONotFound);
                }
                if (catAdoptionFormFromDatabase.FormStatus == FormStatus.Accepted || catAdoptionFormFromDatabase.FormStatus == FormStatus.Denied){
                    ResponseDTO responseDTOBadRequest = new ResponseDTO() {
                        Status= 400,
                        Message= $"Cat Adoption Form with Guid '{catAdoptionFormFromDatabase.Id}' has FormStatus '{catAdoptionFormFromDatabase.FormStatus}' when '{FormStatus.New}' was expected...",
                    };
                    return BadRequest(responseDTOBadRequest);
                }
                catAdoptionFormFromDatabase.FormStatus = FormStatus.Denied;
                
                // Check to make sure the Cat from the form exists
                // As well, check to make sure the Cat is not already Adopted, or needs processing, or is not sutable for adoption
                Cat? catFromAdoptionFormFromDatabase = catAdoptionFormFromDatabase.Cat;
                if (catFromAdoptionFormFromDatabase == null){
                    ResponseDTO responseDTONotFound = new ResponseDTO() {
                        Status= 404,
                        Message= "Cat specified in adoption form was null...",
                    };
                    return NotFound(responseDTONotFound);
                }
                if (catFromAdoptionFormFromDatabase.CatStatus == CatStatus.Adopted || catFromAdoptionFormFromDatabase.CatStatus == CatStatus.Denied || catFromAdoptionFormFromDatabase.CatStatus == CatStatus.New){
                    ResponseDTO responseDTOBadRequest = new ResponseDTO() {
                        Status= 400,
                        Message= $"Cat with Guid '{catFromAdoptionFormFromDatabase.Id}' has CatStatus '{catFromAdoptionFormFromDatabase.CatStatus}' when '{CatStatus.WaitingForAdoption}' was expected...",
                    };
                    return BadRequest(responseDTOBadRequest);
                }
                catFromAdoptionFormFromDatabase.CatStatus = CatStatus.WaitingForAdoption;

                ResponseDTO responseDTOOk = new ResponseDTO() {
                    Status= 200,
                    Message= "Successfully denied cat adoptable form!",
                };

                _database.SaveChanges();
                return Ok(responseDTOOk);
            }
            catch (Exception e){
                ResponseDTO responseDTOError = new ResponseDTO {
                    Status = 400,
                    Message = "An unexpected server error occurred",
                    Errors = e
                };
                return BadRequest(responseDTOError);
            }
        }

        #endregion

        #region Cats Up For Adoption Region

        // This method gets all new forms that were submitted for putting a cat up for adoption
        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpGet("getNewCatsUpForAdoptionForms")]
        public IActionResult GetNewCatsUpForAdoptionForms() {

            try {
                // Custom way of checking for if a signed out token is being used to log in
                var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
                if (!_tokenService.isTokenActive(tokenAuthorization)){
                    ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                        Status= 401,
                        Message= "Unauthorized token login..."
                    };
                    return Unauthorized(responseDTOUnauthorized);
                }

                // Fetches all new cats up for adoption forms from the database
                IEnumerable<CatPutUpForAdoptionForm> catAdoptionForms = _database.CatPutUpForAdoptionForms.Include(x => x.Cat).Where(x => x.FormStatus.Equals(FormStatus.New));
                ResponseDTO responseDTOOk = new ResponseDTO() {
                    Status= 200,
                    Message= "Successfully fetched New Cats Up For Adoption Forms!",
                    Data = catAdoptionForms
                };
                return Ok(responseDTOOk);
            }
            catch (Exception e){
                ResponseDTO responseDTOError = new ResponseDTO {
                    Status = 400,
                    Message = "An unexpected server error occurred",
                    Errors = e
                };
                return BadRequest(responseDTOError);
            }
        }

        // This method gets all accepted forms that were submitted for putting a cat up for adoption
        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpGet("getAcceptedCatsUpForAdoptionForms")]
        public IActionResult GetAcceptedCatsUpForAdoptionForms() {

            try {
                // Custom way of checking for if a signed out token is being used to log in
                var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
                if (!_tokenService.isTokenActive(tokenAuthorization)){
                    ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                        Status= 401,
                        Message= "Unauthorized token login..."
                    };
                    return Unauthorized(responseDTOUnauthorized);
                }

                // Fetches all accepted cats up for adoption forms from the database
                IEnumerable<CatPutUpForAdoptionForm> catAdoptionForms = _database.CatPutUpForAdoptionForms.Include(x => x.Cat).Where(x => x.FormStatus.Equals(FormStatus.Accepted));
                ResponseDTO responseDTOOk = new ResponseDTO() {
                    Status= 200,
                    Message= "Successfully fetched Accepted Cats Up For Adoption Forms!",
                    Data = catAdoptionForms
                };
                return Ok(responseDTOOk);
            }
            catch (Exception e){
                ResponseDTO responseDTOError = new ResponseDTO {
                    Status = 400,
                    Message = "An unexpected server error occurred",
                    Errors = e
                };
                return BadRequest(responseDTOError);
            }
        }

        // This method gets all denied forms that were submitted for putting a cat up for adoption
        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpGet("getDeniedCatUpForAdoptionForm")]
        public IActionResult GetDeniedCatsUpForAdoptionForms() {
            try {
                // Custom way of checking for if a signed out token is being used to log in
                var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
                if (!_tokenService.isTokenActive(tokenAuthorization)){
                    ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                        Status= 401,
                        Message= "Unauthorized token login..."
                    };
                    return Unauthorized(responseDTOUnauthorized);
                }

                // Fetches all denied cats up for adoption forms from the database
                IEnumerable<CatPutUpForAdoptionForm> catAdoptionForms = _database.CatPutUpForAdoptionForms.Where(x => x.FormStatus.Equals(FormStatus.Denied));
                ResponseDTO responseDTOOk = new ResponseDTO() {
                    Status= 200,
                    Message= "Successfully fetched Denied Cats Up For Adoption Forms!",
                    Data = catAdoptionForms
                };
                return Ok(responseDTOOk);
            }
            catch (Exception e){
                ResponseDTO responseDTOError = new ResponseDTO {
                    Status = 400,
                    Message = "An unexpected server error occurred",
                    Errors = e
                };
                return BadRequest(responseDTOError);
            }
        }

        // This method accepts a form that were submitted for putting a cat up for adoption
        // The Id of the form is checked to make sure the form has not already been accepted or denied
        // The Cat's Id is also checked to make sure the Cat has not already been rejected, waiting for adoption or has already been adopted
        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpPost("acceptCatUpForAdoptionForm")]
        public IActionResult AcceptAdoptableCatForm(CatPutUpForAdoptionForm catPutUpForAdoptionForm)
        {
            try {
                // Custom way of checking for if a signed out token is being used to log in
                var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
                if (!_tokenService.isTokenActive(tokenAuthorization)){
                    ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                        Status= 401,
                        Message= "Unauthorized token login..."
                    };
                    return Unauthorized(responseDTOUnauthorized);
                }

                // Fetch the ID from the form and see if a matching form exists in the database
                // As well, check to make sure the form is not already Accepted or Denied
                CatPutUpForAdoptionForm? catPutUpForAdoptionFormFromDatabase = _database.CatPutUpForAdoptionForms
                    .Include(x => x.Cat).FirstOrDefault(x => x.Id.ToString().ToLower().Equals(catPutUpForAdoptionForm.Id.ToString().ToLower()));
                if (catPutUpForAdoptionFormFromDatabase == null){
                    ResponseDTO responseDTONotFound = new ResponseDTO() {
                        Status= 404,
                        Message= $"Cat Put Up For Adoption Form with Guid '{catPutUpForAdoptionForm.Id}' not found...",
                    };
                    return NotFound(responseDTONotFound);
                }
                if (catPutUpForAdoptionFormFromDatabase.FormStatus == FormStatus.Accepted || catPutUpForAdoptionFormFromDatabase.FormStatus == FormStatus.Denied){
                    ResponseDTO responseDTOBadRequest = new ResponseDTO() {
                        Status= 400,
                        Message= $"Cat Adoption Form with Guid '{catPutUpForAdoptionFormFromDatabase.Id}' has FormStatus '{catPutUpForAdoptionFormFromDatabase.FormStatus}' when '{FormStatus.New}' was expected...",
                    };
                    return BadRequest(responseDTOBadRequest);
                }
                catPutUpForAdoptionFormFromDatabase.FormStatus = FormStatus.Accepted;
                
                // Check to make sure the Cat from the form exists
                // As well, check to make sure the Cat is not already Adopted, or waiting to be adopted, or is not sutable for adoption
                Cat? catFromAdoptionFormFromDatabase = catPutUpForAdoptionFormFromDatabase.Cat;
                if (catFromAdoptionFormFromDatabase == null){
                    ResponseDTO responseDTONotFound = new ResponseDTO() {
                        Status= 404,
                        Message= "Cat specified in adoption form was null...",
                    };
                    return NotFound(responseDTONotFound);
                }
                if (catFromAdoptionFormFromDatabase.CatStatus == CatStatus.Adopted || catFromAdoptionFormFromDatabase.CatStatus == CatStatus.Denied || catFromAdoptionFormFromDatabase.CatStatus == CatStatus.WaitingForAdoption){
                    ResponseDTO responseDTOBadRequest = new ResponseDTO() {
                        Status= 400,
                        Message= $"Cat with Guid '{catFromAdoptionFormFromDatabase.Id}' has CatStatus '{catFromAdoptionFormFromDatabase.CatStatus}' when '{CatStatus.New}' was expected...",
                    };
                    return BadRequest(responseDTOBadRequest);
                }
                catFromAdoptionFormFromDatabase.CatStatus = CatStatus.WaitingForAdoption;

                ResponseDTO responseDTOOk = new ResponseDTO() {
                    Status= 200,
                    Message= "Successfully approved cat put up for adoptable form!",
                };

                _database.SaveChanges();
                return Ok(responseDTOOk);
            }
            catch (Exception e){
                ResponseDTO responseDTOError = new ResponseDTO {
                    Status = 400,
                    Message = "An unexpected server error occurred",
                    Errors = e
                };
                return BadRequest(responseDTOError);
            }
        }

        // This method denies a form that were submitted for putting a cat up for adoption
        // The Id of the form is checked to make sure the form has not already been accepted or denied
        // The Cat's Id is also checked to make sure the Cat has not already been rejected, waiting for adoption or has already been adopted
        [Authorize]
        [Authorize("AdminPolicy")]
        [HttpPost("denyCatUpForAdoptionForm")]
        public IActionResult DenyAdoptableCatForm(CatPutUpForAdoptionForm catPutUpForAdoptionForm)
        {
            try {
                // Custom way of checking for if a signed out token is being used to log in
                var tokenAuthorization = Request.Headers[HeaderNames.Authorization].First();
                if (!_tokenService.isTokenActive(tokenAuthorization)){
                    ResponseDTO responseDTOUnauthorized = new ResponseDTO() {
                        Status= 401,
                        Message= "Unauthorized token login..."
                    };
                    return Unauthorized(responseDTOUnauthorized);
                }

                // Fetch the ID from the form and see if a matching form exists in the database
                // As well, check to make sure the form is not already Accepted or Denied
                CatPutUpForAdoptionForm? catPutUpForAdoptionFormFromDatabase = _database.CatPutUpForAdoptionForms
                    .Include(x => x.Cat).FirstOrDefault(x => x.Id.ToString().ToLower().Equals(catPutUpForAdoptionForm.Id.ToString().ToLower()));
                if (catPutUpForAdoptionFormFromDatabase == null){
                    ResponseDTO responseDTONotFound = new ResponseDTO() {
                        Status= 404,
                        Message= $"Cat Adoption Form with Guid '{catPutUpForAdoptionFormFromDatabase.Id}' not found...",
                    };
                    return NotFound(responseDTONotFound);
                }
                if (catPutUpForAdoptionFormFromDatabase.FormStatus == FormStatus.Accepted || catPutUpForAdoptionFormFromDatabase.FormStatus == FormStatus.Denied){
                    ResponseDTO responseDTOBadRequest = new ResponseDTO() {
                        Status= 400,
                        Message= $"Cat Adoption Form with Guid '{catPutUpForAdoptionFormFromDatabase.Id}' has FormStatus '{catPutUpForAdoptionFormFromDatabase.FormStatus}' when '{FormStatus.New}' was expected...",
                    };
                    return BadRequest(responseDTOBadRequest);
                }
                catPutUpForAdoptionFormFromDatabase.FormStatus = FormStatus.Denied;
                
                // Check to make sure the Cat from the form exists
                // As well, check to make sure the Cat is not already Adopted, or waiting to be adopted, or is not sutable for adoption
                Cat? catFromAdoptionFormFromDatabase = catPutUpForAdoptionFormFromDatabase.Cat;
                if (catFromAdoptionFormFromDatabase == null){
                    ResponseDTO responseDTONotFound = new ResponseDTO() {
                        Status= 404,
                        Message= "Cat specified in adoption form was null...",
                    };
                    return NotFound(responseDTONotFound);
                }
                if (catFromAdoptionFormFromDatabase.CatStatus == CatStatus.Adopted || catFromAdoptionFormFromDatabase.CatStatus == CatStatus.Denied || catFromAdoptionFormFromDatabase.CatStatus == CatStatus.WaitingForAdoption){
                    ResponseDTO responseDTOBadRequest = new ResponseDTO() {
                        Status= 400,
                        Message= $"Cat with Guid '{catFromAdoptionFormFromDatabase.Id}' has CatStatus '{catFromAdoptionFormFromDatabase.CatStatus}' when '{CatStatus.New}' was expected...",
                    };
                    return BadRequest(responseDTOBadRequest);
                }
                catFromAdoptionFormFromDatabase.CatStatus = CatStatus.Denied;

                ResponseDTO responseDTOOk = new ResponseDTO() {
                    Status= 200,
                    Message= "Successfully denied cat put up for adoptable form!",
                };

                _database.SaveChanges();
                return Ok(responseDTOOk);
            }
            catch (Exception e){
                ResponseDTO responseDTOError = new ResponseDTO {
                    Status = 400,
                    Message = "An unexpected server error occurred",
                    Errors = e
                };
                return BadRequest(responseDTOError);
            }
        }
        #endregion

    }
}