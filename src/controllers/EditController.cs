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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EditController : ControllerBase
    {
        private readonly EditService _editService;
        public EditController(EditService editService)
        {
            _editService = editService;
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile( [FromBody] dtos.EditProfileDto editProfileDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdJwt = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _editService.UpdateProfileAsync( editProfileDto, userIdJwt);
            if (!result)
            {
                return Unauthorized(new { message = "No autorizado o usuario no encontrado." });
            }

            return Ok(new { message = "Perfil actualizado exitosamente." });
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] dtos.ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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