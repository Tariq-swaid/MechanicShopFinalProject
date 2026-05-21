using MechanicShop.Application.Common.Fetures.Dashboard.Dtos;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;
using MechanicShop.Domin.WorkOrders.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

// this Handler is responsible for handling the GetWorkOrderStatsQuery and returning the statistics for work orders on a specific date.
// It retrieves the relevant work orders from the database, calculates various metrics such as total revenue,
// costs, and profit, and returns the results in a TodatWorkOrderStatsDto object.
// The handler also handles cases where there are no work orders for the specified date,
// returning an empty stats object in that case.

namespace MechanicShop.Application.Common.Fetures.Dashboard.Queries.GetWorkOrderStats
{
    public class GetWorkOrderStatsQueryHandler(IAppDbContext context) : IRequestHandler<GetWorkOrderStatsQuery,Result<TodatWorkOrderStatsDto>>
    {
        private readonly IAppDbContext _context = context;

        public async Task<Result<TodatWorkOrderStatsDto>> Handle(GetWorkOrderStatsQuery request, CancellationToken cancellationToken)
        {
            var start = request.Date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc); // Start of the day in UTC
            var end = request.Date.AddDays(1).ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc); // End of the day in UTC

            var query = _context.WorkOrders // Get all work orders for the specified date
                .Include(wo => wo.Vehicle)
                .Include(wo => wo.RepairTasks).ThenInclude(rt => rt.Parts).
                Include(wo => wo.Invoice).
                Where(wo => wo.StartAtUtc >= start && wo.StartAtUtc < end);

            var total = await query.CountAsync(cancellationToken);



            if (total == 0)  // If there are no work orders for the specified date, return an empty stats object
            {
                return new TodatWorkOrderStatsDto
                {
                    Total = 0,
                    Scheduled = 0,
                    InProgress = 0,
                    Completed = 0,
                    Cancelled = 0,
                    TotalRevenue = 0,
                    TotalPartsCost = 0,
                    TotalLaborCost = 0,
                    UniqueVehicles = 0,
                    UniqueCustomers = 0
                };
            }
            var stats = await query.ToListAsync(cancellationToken);

            // Calculate revenue, costs, and other metrics

            var totalRevenue = stats.Sum(x => x.Invoice?.Total ?? 0);
            var totalPartCost = stats.Where(x => x.Invoice != null).Sum(x => x.TotalPartsCost ?? 0);
            var totalLaborCost = stats.Where(x => x.Invoice != null).Sum(x => x.TotalLaborCost ?? 0);
            var uniqueVehicles = stats.Select(x => x.VehicleId).Distinct().Count();
            var uniqueCustomers = stats.Select(x => x.Vehicle!.CustmoerId).Distinct().Count();

            var netProfit = totalRevenue - totalPartCost - totalLaborCost;

            // Return the stats in a DTO
            return new TodatWorkOrderStatsDto
            {
                Date = request.Date,
                Total = total,
                Scheduled = stats.Count(x => x.State == WorkOrderState.Scheduled),
                InProgress = stats.Count(x => x.State == WorkOrderState.InProgress),
                Completed = stats.Count(x => x.State == WorkOrderState.Completed),
                Cancelled = stats.Count(x => x.State == WorkOrderState.Canceled),
                TotalRevenue = totalRevenue,
                TotalPartsCost = totalPartCost,
                TotalLaborCost = totalLaborCost,
                UniqueVehicles = uniqueVehicles,
                UniqueCustomers = uniqueCustomers,
                NetProfit = netProfit,
                ProfitMargin = totalRevenue > 0 ? (netProfit / totalRevenue) * 100 : 0,
                CompletionRate = total > 0 ? ((decimal)stats.Count(x => x.State == WorkOrderState.Completed) / total) * 100 : 0,
                AverageRevenuePerOrder = total > 0 ? totalRevenue / total : 0,
                OrdersPerVehicle = uniqueVehicles > 0 ? (decimal)total / uniqueVehicles : 0,
                PartsCostRatio = totalRevenue > 0 ? (totalPartCost / totalRevenue) * 100 : 0,
                LaborCostRatio = totalRevenue > 0 ? (totalLaborCost / totalRevenue) * 100 : 0,
                CancellationRate = total > 0 ? ((decimal)stats.Count(x => x.State == WorkOrderState.Canceled) / total) * 100 : 0
            };
        }
    }
}
