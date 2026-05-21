using MechanicShop.Application.Common.Fetures.Customer.Mappers;
using MechanicShop.Application.Common.Fetures.Labors.Dtos;
using MechanicShop.Application.Common.Fetures.RepairTasks.Mappers;
using MechanicShop.Application.Common.Fetures.WorkOrders.Dtos;
using MechanicShop.Domin.WorkOrders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.WorkOrders.Mappers
{
    public static class WorkOrderMapper
    {
        public static WorkOrdersDto ToDto(this WorkOrder entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new WorkOrdersDto
            {
                WorkOrderId = entity.Id,
                Spot = entity.Spot,
                StartAtUtc = entity.StartAtUtc,
                EndAtUtc = entity.EndAtUtc,
                Labor = entity.Labor is null ? null : new LaborDto
                {
                    LaborId = entity.Labor.Id,
                    Name = $" {entity.Labor.FisrtName} {entity.Labor.LastName} "
                },
                RepairTasks = entity.RepairTasks.ToDtos(),
                Vehicle = entity.Vehicle is null ? null : entity.Vehicle.ToDto(),
                State = entity.State,
                TotalPartCost = entity.RepairTasks.SelectMany(t => t.Parts).Sum(p => p.Cost * p.Quantity),
                TotalLaborCost = entity.RepairTasks.Sum(p => p.LaborCost),
                TotalCost = entity.RepairTasks.Sum(rt => rt.TotalCost),
                TotalDurationInMins = entity.RepairTasks.Sum(rt => (int)rt.RepairDurationInMinutes),
                InvoiceId = entity.Invoice?.Id,
                CreatedAt = entity.CreatedAtUtc

            };
       
              
        }
        public static List<WorkOrdersDto> ToDtos(this IEnumerable<WorkOrder> entities)
        {
            return [.. entities.Select(e => e.ToDto())];
        }
        public static WorkOederListItemDto ToListItemDto(this WorkOrder entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new WorkOederListItemDto
            {
                WorkOrderId = entity.Id,
                Spot = entity.Spot,
                StartAtUtc = entity.StartAtUtc,
                EndAtUtc = entity.EndAtUtc,
                Vehicle = entity.Vehicle!.ToDto(),
                Labor = entity.Labor is null ? null :
                    $"{entity.Labor.FisrtName} {entity.Labor.LastName}",
                State = entity.State,
                RepairTasks = entity.RepairTasks.Select(rt => rt.Name).ToList()
            };
        }

    }
}
