using MechanicShop.Application.Common.Fetures.Identity.Dtos;
using MechanicShop.Domin.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> IsInRoleAsync(string userId, string role);

        Task<bool> AuthorizeAsync(string userId, string? policyName);

        Task<Result<AppUserDto>> AuthenticateAsync(string email, string password);

        Task<Result<AppUserDto>> GetUserByIdAsync(string userId);

        Task<string?> GetUserNameAsync(string userId);
    }
}
