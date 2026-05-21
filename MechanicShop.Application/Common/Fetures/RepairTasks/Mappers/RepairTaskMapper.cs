using MechanicShop.Application.Common.Fetures.RepairTasks.Dtos;
using MechanicShop.Domin.RepierTask;
using MechanicShop.Domin.RepierTask.Parts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.RepairTasks.Mappers
{
    public static  class RepairTaskMapper
    {
        public static RepairTaskDto ToDto(this RepairTask task)

        {
            ArgumentNullException.ThrowIfNull(task);

            return new RepairTaskDto
            {
                RepairTaskId = task.Id,
                Name = task.Name,
                LaborCost = task.LaborCost,
                TotalCost = task.TotalCost,
                EstimatedDurationInMins = task.RepairDurationInMinutes,
                Parts = task.Parts is not null ? task.Parts.Select(p => p.ToDto()).ToList() : new List<PartDto>()
            };
        }
         public static List<RepairTaskDto> ToDtos(this IEnumerable<RepairTask> entities)
        {
            return [.. entities.Select(e => e.ToDto())];
        }

        public static PartDto ToDto(this Part entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new PartDto
            {
                PartId = entity.Id,
                Name = entity.Name!,
                Cost = entity.Cost,
                Quantity = entity.Quantity
            };
        }

        public static List<PartDto> ToDtos(this IEnumerable<Part> entities)
        {
            return [.. entities.Select(e => e.ToDto())];
        }

    }
}
