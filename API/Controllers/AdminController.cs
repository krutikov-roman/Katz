using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Models.DTOs;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

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
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user==null)
                return NotFound("email doesn't exist");
            
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (result.Succeeded)
                return Ok(_tokenService.CreateToken(user));

            return Unauthorized(); 
        }

        [HttpGet("notSecure")]
        public IActionResult GetUnsecureText()
        {
            return Ok("Do you have an account with us?");
        }

        [Authorize]
        [HttpGet("secure")]
        public IActionResult GetSecureText()
        {
            return Ok("You have an account with us");
        }

        [Authorize("AdminPolicy")]
        [HttpGet("secureAdmin")]
        public IActionResult GetSecureText2()
        {
            return Ok("You are an admin");
        }

    }
}