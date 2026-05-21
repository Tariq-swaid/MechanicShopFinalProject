using MechanicShop.Application.Common.Fetures.RepairTasks.Dtos;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;


namespace MechanicShop.Application.Common.Fetures.RepairTasks.Queries.GetRepairTaskById
{

    public sealed record GetRepairTaskByIdQuery(Guid RepairTaskId) : ICachedQuery<Result<RepairTaskDto>>
    {
        public string CacheKey => $"repair-task_{RepairTaskId}";

        public string[] Tags => ["repair-task"];

        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    }
}
