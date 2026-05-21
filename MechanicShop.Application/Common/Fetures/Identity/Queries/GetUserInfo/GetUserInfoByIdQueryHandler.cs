using MechanicShop.Application.Common.Fetures.Identity.Dtos;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Common.Fetures.Identity.Queries.GetUserInfo
{
    public class GetUserByIdQueryHanlder(ILogger<GetUserByIdQueryHanlder> logger, IIdentityService identityService)
       : IRequestHandler<GetUserInfoByIdQuery, Result<AppUserDto>>
    {
        private readonly ILogger<GetUserByIdQueryHanlder> _logger = logger;
        private readonly IIdentityService _identityService = identityService;

        public async Task<Result<AppUserDto>> Handle(GetUserInfoByIdQuery request, CancellationToken ct)
        {
            var getUserByIdResult = await _identityService.GetUserByIdAsync(request.UserId!);

            if (getUserByIdResult.IsError)
            {
                _logger.LogError("User with Id { UserId }{ErrorDetails}", request.UserId, getUserByIdResult.TopError.Description);

                return getUserByIdResult.Errors!;
            }

            return getUserByIdResult.Value;
        }
    }
}
