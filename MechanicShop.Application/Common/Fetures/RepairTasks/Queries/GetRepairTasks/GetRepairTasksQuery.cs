using MechanicShop.Application.Common.Fetures.RepairTasks.Dtos;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.RepairTasks.Queries.GetRepairTasks
{
    public sealed record GetRepairTasksQuery() : ICachedQuery<Result<List<RepairTaskDto>>>
    {
        public string CacheKey => "repair-tasks";


        public string[] Tags => ["repair-tasks"];

        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    }
}
