using MechanicShop.Application.Common.Fetures.Identity;
using MechanicShop.Application.Common.Fetures.Identity.Dtos;
using MechanicShop.Domin.Common.Results;
using System.Security.Claims;


namespace MechanicShop.Application.Common.Interfaces
{
    public interface ITokenProvider
    {
        Task<Result<TokenResponse>> GenerateJwtTokenAsync(AppUserDto user, CancellationToken ct = default);

        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
