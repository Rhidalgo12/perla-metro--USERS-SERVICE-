using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PerlaMetro.src.services;

namespace PerlaMetro.src.controllers
{
    /// <summary>
    /// Controller for editing user profiles and changing passwords.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EditController : ControllerBase
    {
        /// <summary>
        /// Edit service for handling user profile updates and password changes.
        /// </summary>
        private readonly EditService _editService;
        /// <summary>
        /// Initializes a new instance of the <see cref="EditController"/> class.
        /// </summary>
        /// <param name="editService">The edit service.</param>
        public EditController(EditService editService)
        {
            _editService = editService;
        }
        /// <summary>
        /// Updates the profile of the authenticated user.
        /// </summary>
        /// <param name="editProfileDto">The edit profile data transfer object.</param>
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile( [FromBody] dtos.EditProfileDto editProfileDto)
        {
            /// <summary> Validates the provided profile details.
            /// </summary> <returns>BadRequest if the model state is invalid.</returns>
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            /// <summary> Retrieves the user ID from the JWT token.
            /// </summary> <returns>Unauthorized if the user is not found or not authorized.</returns>
            var userIdJwt = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _editService.UpdateProfileAsync( editProfileDto, userIdJwt);
            if (!result)
            {
                return Unauthorized(new { message = "No autorizado o usuario no encontrado." });
            }

            return Ok(new { message = "Perfil actualizado exitosamente." });
        }
        /// <summary>
        /// Changes the password of the authenticated user.
        /// </summary>
        /// <param name="changePasswordDto">The change password data transfer object.</param>
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] dtos.ChangePasswordDto changePasswordDto)
        {
            /// <summary> Validates the provided password details.
            /// </summary> <returns>BadRequest if the model state is invalid.</returns>
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            /// <summary> Retrieves the user ID from the JWT token.
            /// </summary> <returns>Unauthorized if the user is not found or not authorized.</returns>
            var userIdJwt = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _editService.ChangePasswordAsync(changePasswordDto, userIdJwt);
            if (!result)
            {
                return Unauthorized(new { message = "No autorizado, usuario no encontrado o contraseña actual incorrecta." });
            }

            return Ok(new { message = "Contraseña cambiada exitosamente." });
        }
    }
}