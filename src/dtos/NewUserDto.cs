using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerlaMetro.src.dtos
{
    /// <summary>
    /// Data Transfer Object for new user information.
    /// </summary>
    public class NewUserDto
    {
        /// <summary>
        /// The user's name.
        /// </summary>
        public string UserName { get; set; } = null!;

        /// <summary>
        /// The user's email address.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// The user's authentication token.
        /// </summary>
        public string Token { get; set; } = null!;
    }
}