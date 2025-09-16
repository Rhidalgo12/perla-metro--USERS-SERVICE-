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
    }
}