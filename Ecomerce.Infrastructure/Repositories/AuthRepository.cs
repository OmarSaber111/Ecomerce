using Ecomerce.Core.DTOs.Auth_DTOs;
using Ecomerce.Core.Entities;
using Ecomerce.Core.Interfaces;
using Ecomerce.Core.IService;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomerce.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthRepository(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<string> RegisterAsync(RegisterDto model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            return "User Registered Successfully";
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid username or password");

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Invalid username or password");

            user.LastLoginTime = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return await _tokenService.GenerateTokenAsync(user);
        }

        public async Task<AuthResultDto?> RefreshTokenAsync(string refreshToken)
        {
            return await _tokenService.RefreshTokenAsync(refreshToken);
        }
    }
}

