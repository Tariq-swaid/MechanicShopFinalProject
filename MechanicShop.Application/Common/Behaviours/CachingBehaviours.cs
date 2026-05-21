using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
namespace MechanicShop.Application.Common.Behaviours
{
    public class CachingBehaviours<TRequest, TResponse>(HybridCache hybrid, ILogger<CachingBehaviours<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly HybridCache _hybrid = hybrid;

        private readonly ILogger<CachingBehaviours<TRequest, TResponse>> _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is not ICachedQuery cachedRequest) {
                    
                    return await next(cancellationToken);
            }

            _logger.LogInformation("Cheking for {RequestName}", typeof(TRequest).Name);

            var result = await _hybrid.GetOrCreateAsync(
                key: cachedRequest.CacheKey,
                factory: async ct =>
                {
                    var innerResult = await next(ct);
                    if (innerResult is IResult r && r.IsSuccess)
                    {
                        return innerResult;
                    }
                    return default!;
                },
                options: new HybridCacheEntryOptions
                {
                    Expiration = cachedRequest.Expiration
                },
                tags: cachedRequest.Tags,
                cancellationToken: cancellationToken);
            return result;
        
        }
    }
}