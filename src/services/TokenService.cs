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
    /// <summary>
    /// Service for generating JWT tokens for authenticated users.
    /// </summary>
    public class TokenService : ITokenService
    {
        /// <summary>
        /// UserManager for managing user accounts.
        /// </summary>
        private readonly UserManager<AppUser> _userManager;
        /// <summary>
        /// Security key for signing JWT tokens.
        /// </summary>
        private readonly SymmetricSecurityKey _key;
        /// <summary>
        /// DataContext for accessing the database.
        /// </summary>
        private readonly DataContext _context;
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService"/> class.
        /// </summary>
        /// <param name="userManager">UserManager for managing user accounts.</param>
        /// <param name="context">DataContext for accessing the database.</param>
        public TokenService(UserManager<AppUser> userManager, DataContext context)
        {
            _userManager = userManager;
            _context = context;
    
            var signingKey = Environment.GetEnvironmentVariable("JWT_SIGNING_KEY") ?? throw new ArgumentNullException("JWT_SIGNING_KEY environment variable is not set.");

            _key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(signingKey));
        }
        /// <summary>
        /// Creates a JWT token for the specified user.
        /// </summary>
        /// <param name="user"></param>
        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {  
                /// <summary>
                /// Claim for the user's given name.
                /// </summary>
                /// <param name="Claim(JwtRegisteredClaimNames.GivenName"></param>
                /// <param name="Claim(ClaimTypes.NameIdentifier"></param>
                /// <param name="Claim(JwtRegisteredClaimNames.Jti"></param>
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            /// <summary>
            /// Gets the roles for the specified user.
            /// </summary>
            var userRoles = _userManager.GetRolesAsync(user);
            foreach (var role in userRoles.Result)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            /// <summary>
            /// Gets the signing credentials for creating the JWT token.
            /// </summary>
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            /// <summary>
            /// Gets the issuer and audience for the JWT token from environment variables.
            /// </summary>
            var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new ArgumentNullException("JWT Issuer cannot be null or empty."); ;
            var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new ArgumentNullException("JWT Audience cannot be null or empty.");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                /// <summary>
                /// Subject containing the claims for the JWT token.
                /// </summary>
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
                Issuer = issuer,
                Audience = audience
            };

            /// <summary>
            /// Creates and returns the JWT token as a string.
            /// </summary>
            /// <returns>The JWT token as a string.</returns>
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);


            return tokenHandler.WriteToken(token);
        }
    }
}