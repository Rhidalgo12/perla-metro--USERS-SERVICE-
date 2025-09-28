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
    /// <summary>
    /// Controller for user authentication, including login functionality.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Token service for generating and validating JWT tokens.
        /// </summary>
        private readonly ITokenService _tokenService;
        /// <summary>
        /// User manager for managing user accounts.
        /// </summary>
        private readonly UserManager<AppUser> _userManager;
        /// <summary>
        /// Sign-in manager for handling user sign-in operations.
        /// </summary>
        private readonly SignInManager<AppUser> _signInManager;
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="tokenService">The token service.</param>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign-in manager.</param>
        public AuthController(ITokenService tokenService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        /// <summary>
        /// Authenticates a user and returns a JWT token if successful.
        /// </summary>
        /// <param name="loginDto">The login details.</param>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            /// <summary>
            /// verifies if there is an active session.
            /// </summary> <returns>BadRequest if there is an active session.</returns>
            try
            {
                if (User.Identity?.IsAuthenticated == true)
                {
                    return BadRequest(new { message = "Active session." });
                }
                /// <summary> Validates the provided login details.
                /// </summary> <returns>BadRequest if the model state is invalid.</returns>
                if (!ModelState.IsValid) return BadRequest(ModelState);
                /// <summary> Attempts to find the user by email and validate the password.
                /// </summary> <returns>Unauthorized if the email or password is incorrect, or if the user is not active.</returns>
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
                if (user == null) return Unauthorized("Invalid email or password.");

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if (!result.Succeeded) return Unauthorized("Invalid email or password.");
                if (user.IsActive == 0) return Unauthorized("User is not active.");
                /// <summary>
                /// Signs in the user and creates a JWT token.
                /// </summary>
                /// <returns></returns>
                await _signInManager.SignInAsync(user, isPersistent: true);

                var token = _tokenService.CreateToken(user);
                if (string.IsNullOrEmpty(token)) return Unauthorized("Invalid token.");
                /// <summary>
                /// Returns the authenticated user's details along with the JWT token.
                /// </summary>
                return Ok(
                    new NewUserDto
                    {
                        UserName = user.UserName!,
                        Email = user.Email!,
                        Token = token
                    }
                );

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}