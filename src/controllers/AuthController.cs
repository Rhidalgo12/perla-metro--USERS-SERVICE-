using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PerlaMetro.src.dtos;
using PerlaMetro.src.interfaces;
using PerlaMetro.src.models.User;

namespace PerlaMetro.src.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(ITokenService tokenService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try {
                if (User.Identity?.IsAuthenticated == true)
                {
                    return BadRequest(new { message = "Active session." });
                }

                if(!ModelState.IsValid) return BadRequest(ModelState);

                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
                if(user == null) return Unauthorized("Invalid email or password.");

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if(!result.Succeeded) return Unauthorized("Invalid email or password.");
                if(user.IsActive == 0) return Unauthorized("User is not active.");

                await _signInManager.SignInAsync(user, isPersistent: true);

                var token = _tokenService.CreateToken(user);

                if (string.IsNullOrEmpty(token)) return Unauthorized("Invalid token.");

                return Ok(
                    new NewUserDto
                    {
                        UserName = user.UserName!,
                        Email = user.Email!,
                        Token = token
                    }
                );
                
            }catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }
    }
}