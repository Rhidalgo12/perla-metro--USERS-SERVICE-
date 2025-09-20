using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PerlaMetro.data;
using PerlaMetro.src.interfaces;
using PerlaMetro.src.models.User;

namespace PerlaMetro.src.services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly SymmetricSecurityKey _key;

        private readonly DataContext _context;

        public TokenService(UserManager<AppUser> userManager, DataContext context)
        {
            _userManager = userManager;
            _context = context;

            var signingKey = Environment.GetEnvironmentVariable("JWT_SIGNING_KEY") ?? throw new ArgumentNullException("JWT_SIGNING_KEY environment variable is not set.");

            _key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(signingKey));
        }

        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = _userManager.GetRolesAsync(user);
            foreach (var role in userRoles.Result)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);


            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new ArgumentNullException("JWT Issuer cannot be null or empty."); ;
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new ArgumentNullException("JWT Audience cannot be null or empty.");
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), 
                Expires = DateTime.Now.AddDays(1), 
                SigningCredentials = creds, 
                Issuer = issuer, 
                Audience = audience
            };

            
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            
            return tokenHandler.WriteToken(token);
        }
    }
}