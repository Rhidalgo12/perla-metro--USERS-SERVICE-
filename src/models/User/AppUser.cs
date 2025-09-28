using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PerlaMetro.src.models.User
{
    /// <summary>
    /// Application user model extending IdentityUser with additional properties.
    /// </summary>
    public class AppUser : IdentityUser<Guid>
    {
        /// <summary>
        /// The user's first name.
        /// </summary>
        public string? Name { get; set; } = string.Empty;
        /// <summary>
        /// The user's last name.
        /// </summary>
        public string? LastName { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the user is active.
        /// </summary>
        public int IsActive { get; set; }
        /// <summary>
        /// The date and time when the user was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// The unique identifier for the user.
        /// </summary>
        public override Guid Id { get; set; } = Guid.NewGuid();
    }
}