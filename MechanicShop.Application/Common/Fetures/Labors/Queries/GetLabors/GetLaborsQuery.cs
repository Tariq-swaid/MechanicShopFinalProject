using MechanicShop.Application.Common.Fetures.Labors.Dtos;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;


namespace MechanicShop.Application.Common.Fetures.Labors.Queries.GetLabors
{
    public class GetLaborsQuery : ICachedQuery<Result<List<LaborDto>>>
    {
        public string CacheKey => $"labors";

        public string[] Tags => ["labors"];

        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    }
}
