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

    }
}