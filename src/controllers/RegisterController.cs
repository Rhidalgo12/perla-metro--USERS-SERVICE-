using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PerlaMetro.src.dtos;
using PerlaMetro.src.models.User;

namespace PerlaMetro.src.controllers
{
    /// <summary>
    /// Controller for user registration.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        /// <summary>
        /// UserManager instance for managing user-related operations.
        /// </summary>
        private readonly UserManager<AppUser> _userManager;
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        /// <summary>
        /// Registers a new user with the provided registration details.
        /// </summary>
        /// <param name="registerDto">The registration details.</param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            /// <summary> Validates the provided registration details.
            /// </summary> <returns>BadRequest if the model state is invalid.</returns>
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            /// <summary> Checks if a user with the provided email already exists.
            /// </summary> <returns>BadRequest if the email is already in use.</returns
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "El correo electrónico ya está en uso." });
            }
            /// <summary> Creates a new user with the provided registration details.
            /// </summary> <returns>Ok if the user was successfully created, otherwise BadRequest with error details.</returns>
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = registerDto.Email,
                Email = registerDto.Email,
                Name = registerDto.FirstName,
                LastName = registerDto.LastName,
                IsActive = 1,
                
            };
            /// <summary>
            /// Creates a new user with the provided registration details.
            /// </summary>
            /// <returns></returns>
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                return Ok(new { message = "Usuario registrado exitosamente." });
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { message = "Error al registrar el usuario.", errors });
            }
        }
    }
}