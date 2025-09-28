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
    /// <summary>
    /// Controller for managing users, including retrieval and soft deletion.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserManagementController : ControllerBase
    {
        /// <summary>
        /// UserManager instance for managing user-related operations.
        /// </summary>
        private readonly UserManager<AppUser> _userManager;
        /// <summary>
        /// Initializes a new instance of the <see cref="UserManagementController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        public UserManagementController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        /// <summary>
        /// Retrieves a list of users with optional filtering by name, email, and active status.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers( [FromQuery] string? name, [FromQuery] string? email, [FromQuery] int? isActive)
        {
            /// <summary>
            /// Builds the query for retrieving users based on the provided filters.
            /// </summary>
            /// <returns></returns>
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(u => (u.Name + " " + u.LastName).Contains(name));
            }
            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(u => u.Email != null && u.Email.Contains(email));
            }
            if (isActive.HasValue)
            {
                query = query.Where(u => u.IsActive == isActive.Value);
            }
            /// <summary>
            /// List of users matching the specified criteria.
            /// </summary>
            /// <value></value>
            var users = await query
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
        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The user with the specified identifier, or NotFound if not found.</returns>
        [HttpGet("user/{id}")]
        public IActionResult GetUserById( [FromRoute] Guid id)
        {
            /// <summary> Validates the provided user ID.
            /// </summary> <returns>BadRequest if the ID is empty.</returns>
            if (id == Guid.Empty)
                return BadRequest("El ID del usuario no puede estar vacío.");
            try
            {
                /// <summary> Retrieves the user with the specified ID.
                /// </summary> <returns>The user if found, otherwise NotFound.</returns>
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
        /// <summary>
        /// Soft deletes a user by setting their IsActive status to 0.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>Ok if the user was successfully deleted, otherwise an error response.</returns>
        [HttpDelete("user/{id}")]
        public async Task<IActionResult> SoftDeleteUser([FromRoute] Guid id)
        {
            /// <summary> Validates the provided user ID.
            /// </summary> <returns>BadRequest if the ID is empty.</returns>
            if (id == Guid.Empty)
                return BadRequest("El ID del usuario no puede estar vacío.");
            try
            {
                /// <summary>
                /// Retrieves the user with the specified ID.
                /// </summary>
                /// <returns>The user if found, otherwise NotFound.</returns>
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