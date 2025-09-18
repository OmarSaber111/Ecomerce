using Ecomerce.Core.DTOs.Auth_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomerce.Core.Interfaces
{
    public interface IAuthRepository
    {
        Task<string> RegisterAsync(RegisterDto model);
        Task<AuthResultDto> LoginAsync(LoginDto model);
        Task<AuthResultDto?> RefreshTokenAsync(string refreshToken);
    }
}
