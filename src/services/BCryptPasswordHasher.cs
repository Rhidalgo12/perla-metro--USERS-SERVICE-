using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PerlaMetro.src.services
{
    /// <summary>
    /// BCrypt implementation of IPasswordHasher for hashing and verifying passwords.
    /// </summary>
    public class BCryptPasswordHasher<TUser> : IPasswordHasher<TUser> where TUser : class
    {
        /// <summary>
        /// Hashes the password for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the password is being hashed.</param>
        /// <param name="password">The plain text password to hash.</param>
        /// <returns>The hashed password.</returns>
        public string HashPassword(TUser user, string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        /// <summary>
        /// Verifies the hashed password against the provided plain text password.
        /// </summary>
        /// <param name="user">The user for whom the password is being verified.</param>
        /// <param name="hashedPassword">The hashed password to verify against.</param>
        /// <param name="providedPassword">The plain text password provided for verification.</param>
        /// <returns>The result of the password verification.</returns>
        public PasswordVerificationResult VerifyHashedPassword(
            TUser user, string hashedPassword, string providedPassword)
        {
            bool isValid = BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
            return isValid ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;
        }
    }
}