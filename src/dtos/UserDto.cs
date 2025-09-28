using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerlaMetro.src.dtos
{
    /// <summary>
    /// Data Transfer Object for user information.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// The unique identifier for the user.
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// The user's first name.
        /// </summary>
        public string? FirstName { get; set; }
        /// <summary>
        /// The user's last name.
        /// </summary>
        public string? LastName { get; set; }
        /// <summary>
        /// The user's email address.
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Indicates whether the user is active.
        /// </summary>
        public int IsActive { get; set; }
        /// <summary>
        /// The date and time when the user was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}