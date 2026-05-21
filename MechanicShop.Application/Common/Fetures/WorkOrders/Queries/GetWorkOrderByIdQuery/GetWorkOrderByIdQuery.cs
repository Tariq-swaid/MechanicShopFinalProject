using MechanicShop.Application.Common.Fetures.WorkOrders.Dtos;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;

namespace MechanicShop.Application.Common.Fetures.WorkOrders.Queries.GetWorkOrderByIdQuery
{
    public sealed record GetWorkOrderByIdQuery(Guid WorkOrderId) : ICachedQuery<Result<WorkOrdersDto>>
    {
        public string CacheKey => $"work-order:{WorkOrderId}";
        public string[] Tags => ["work-order"];

       public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    }
}
