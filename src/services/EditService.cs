using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PerlaMetro.data;
using PerlaMetro.src.dtos;

namespace PerlaMetro.src.services
{
    public class EditService
    {
        private readonly DataContext _context;

        public EditService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateProfileAsync( EditProfileDto editProfileDto, Guid UserIdJwt)
        {
            

            var user = await _context.Users.FindAsync(UserIdJwt);
            if (user == null)
            {
                return false;
            }

            user.Name = editProfileDto.FirstName;
            user.LastName = editProfileDto.LastName;
            user.Email = editProfileDto.Email;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto, Guid UserIdJwt)
        {


            var user = await _context.Users.FindAsync(UserIdJwt);
            if (user == null)
            {
                return false;
            }
            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash))
            {
                return false;
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}