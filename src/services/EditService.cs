using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PerlaMetro.data;
using PerlaMetro.src.dtos;

namespace PerlaMetro.src.services
{
    /// <summary>
    /// Service for editing user profiles and changing passwords.
    /// </summary>
    public class EditService
    {
        /// <summary>
        /// DataContext for accessing the database.
        /// </summary>
        private readonly DataContext _context;
        /// <summary>
        /// Initializes a new instance of the <see cref="EditService"/> class.
        /// </summary>
        /// <param name="context">DataContext for accessing the database.</param>
        public EditService(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <param name="editProfileDto">The edited profile data.</param>
        /// <param name="UserIdJwt">The ID of the user to update.</param>
        public async Task<bool> UpdateProfileAsync(EditProfileDto editProfileDto, Guid UserIdJwt)
        {

            /// <summary>
            /// Gets the user with the specified ID.
            /// </summary>
            /// <returns>The user with the specified ID, or null if not found.</returns>
            var user = await _context.Users.FindAsync(UserIdJwt);
            if (user == null)
            {
                return false;
            }
            /// <summary>
            /// Updates the user's profile information.
            /// </summary>
            /// <returns>True if the update was successful.</returns>
            user.Name = editProfileDto.FirstName;
            user.LastName = editProfileDto.LastName;
            user.Email = editProfileDto.Email;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="changePasswordDto">The change password data.</param>
        /// <param name="UserIdJwt">The ID of the user to update.</param>
        public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto, Guid UserIdJwt)
        {

            /// <summary>
            /// Gets the user with the specified ID.
            /// </summary>
            var user = await _context.Users.FindAsync(UserIdJwt);
            if (user == null)
            {
                return false;
            }
            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
            {
                return false;
            }
            /// <summary>
            /// Updates the user's password.
            /// </summary>
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}