using MechanicShop.Application.Common.Fetures.Identity.Dtos;
using MechanicShop.Domin.Common.Results;
using MediatR;
namespace MechanicShop.Application.Common.Fetures.Identity.Queries.GetUserInfo
{
    public sealed record GetUserInfoByIdQuery(string UserId):IRequest<Result<AppUserDto>>;
    
}
