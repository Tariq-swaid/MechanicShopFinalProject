using MechanicShop.Domin.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.Identity.Queries.GenerateTokens
{
    public record GenerateTokensQuery(string Email, string Password) 
        : IRequest<Result<TokenResponse>>;
  

}
