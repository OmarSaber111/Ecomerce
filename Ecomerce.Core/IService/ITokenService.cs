using Ecomerce.Core.DTOs.Auth_DTOs;
using Ecomerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomerce.Core.IService
{
    public interface ITokenService
    {
        Task<AuthResultDto> GenerateTokenAsync(User user);
        Task<AuthResultDto> RefreshTokenAsync(string refreshToken);
    }
}
