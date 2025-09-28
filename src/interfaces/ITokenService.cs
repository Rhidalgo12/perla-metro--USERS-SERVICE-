using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PerlaMetro.src.models.User;

namespace PerlaMetro.src.interfaces
{
    /// <summary>
    /// Interface for token service to create JWT tokens.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Creates a JWT token for the specified user.
        /// </summary>
        string CreateToken(AppUser user);
    }
}