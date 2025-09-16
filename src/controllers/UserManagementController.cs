using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PerlaMetro.src.dtos;
using PerlaMetro.src.models.User;

namespace PerlaMetro.src.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public UserManagementController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers([FromQuery] string adminEmail)
        {
            if (string.IsNullOrEmpty(adminEmail))
                return BadRequest("Debes ingresar un correo de administrador.");

            if (adminEmail != "admin@perlametro.cl")
                return BadRequest("No tienes permisos para ver la lista de usuarios.");

            var users = await _userManager.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.Name,
                    LastName = u.LastName,
                    Email = u.Email,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("user/{id}")]
        public IActionResult GetUserById([FromQuery] string adminEmail, [FromRoute] Guid id)
        {
            if (string.IsNullOrEmpty(adminEmail))
                return BadRequest("Debes ingresar un correo de administrador.");

            if (adminEmail != "admin@perlametro.cl")
                return BadRequest("No tienes permisos para ver la lista de usuarios.");

            if (id == Guid.Empty)
                return BadRequest("El ID del usuario no puede estar vacío.");
            try
            {
                var user = _userManager.Users
                    .Where(u => u.Id == id)
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        FirstName = u.Name,
                        LastName = u.LastName,
                        Email = u.Email,
                        IsActive = u.IsActive,
                        CreatedAt = u.CreatedAt
                    })
                    .FirstOrDefault();

                if (user != null)
                {
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
            return NotFound("Usuario no encontrado.");
        }

        [HttpDelete("user/{id}")]
        public async Task<IActionResult> SoftDeleteUser([FromQuery] string adminEmail, [FromRoute] Guid id)
        {
            if (string.IsNullOrEmpty(adminEmail))
                return BadRequest("Debes ingresar un correo de administrador.");
            if (adminEmail != "admin@perlametro.cl")
                return BadRequest("No tienes permisos para eliminar usuarios.");
            if (id == Guid.Empty)
                return BadRequest("El ID del usuario no puede estar vacío.");
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                    return NotFound("Usuario no encontrado.");
                if (user.IsActive == 0)
                    return BadRequest("El usuario ya está inactivo.");
                user.IsActive = 0;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return StatusCode(500, "Error al eliminar el usuario.");
                return Ok("Usuario eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}