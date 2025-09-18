using Ecomerce.Core.DTOs.Auth_DTOs;
using Ecomerce.Core.Entities;
using Ecomerce.Core.IService;
using Ecomerce.Infrastructure.Data.IdentityData;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecomerce.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly EcomerceIdentityDbContext _context;

        public TokenService(IConfiguration config, UserManager<User> userManager, EcomerceIdentityDbContext context)
        {
            _config = config;
            _userManager = userManager;
            _context = context;
        }

        public async Task<AuthResultDto> GenerateTokenAsync(User user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: authClaims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

           
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResultDto
            {
                Token = jwt,
                RefreshToken = refreshToken.Token,
                Expiration = token.ValidTo
            };
        }

        public async Task<AuthResultDto> RefreshTokenAsync(string refreshToken)
        {
            var storedToken = _context.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken && !rt.IsRevoked);

            if (storedToken == null || storedToken.ExpiryDate < DateTime.UtcNow)
                return null;

            var user = await _userManager.FindByIdAsync(storedToken.UserId);
            if (user == null) return null;

            storedToken.IsRevoked = true;
            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync();

            return await GenerateTokenAsync(user);
        }
    }
}

