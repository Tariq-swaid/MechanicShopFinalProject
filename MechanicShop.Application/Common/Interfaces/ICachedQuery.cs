using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Interfaces
{
    public interface ICachedQuery
    {
        string CacheKey { get; }
        string[] Tags { get; }
        TimeSpan? Expiration { get; }
    }

    public interface ICachedQuery<TRequest> : IRequest<TRequest>, ICachedQuery;


}