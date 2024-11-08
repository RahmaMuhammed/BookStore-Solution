using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookStore.Core.Entities.Identity;
using BookStore.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;

namespace BookStore.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configration;

        public TokenService(IConfiguration configration)
        {
            this.configration = configration;
        }
        public async Task<string> CreateTokenAsync(AppUser User, UserManager<AppUser> userManager)
        {
           
            // Payload
            // Private Claim (User - Define)

            var AuthClaims = new List<Claim>()
            {
                new Claim (ClaimTypes.GivenName, User.DisplayName),
                new Claim (ClaimTypes.Email, User.Email)
            };
            var UserRole = await userManager.GetRolesAsync(User);
            foreach (var role in UserRole)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configration["JWT:Key"]));
            var Token = new JwtSecurityToken(
                issuer: configration["JWT:ValidIssur"],
                audience: configration["JWT:ValidAudiance"],
                expires: DateTime.Now.AddDays(double.Parse(configration["JWT:DurationInDays"])),
                claims : AuthClaims,
                signingCredentials : new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256Signature)
            );
            return new JwtSecurityTokenHandler().WriteToken(Token);

        }
    }
}
