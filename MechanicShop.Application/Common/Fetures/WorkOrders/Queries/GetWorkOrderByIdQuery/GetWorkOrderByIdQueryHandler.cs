using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Fetures.WorkOrders.Dtos;
using MechanicShop.Application.Common.Fetures.WorkOrders.Mappers;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace MechanicShop.Application.Common.Fetures.WorkOrders.Queries.GetWorkOrderByIdQuery
{
    public class GetWorkOrderByIdQueryHandler(
        ILogger<GetWorkOrderByIdQueryHandler> logger,
        IAppDbContext context
        )
        : IRequestHandler<GetWorkOrderByIdQuery, Result<WorkOrdersDto>>
    {
        private readonly ILogger<GetWorkOrderByIdQueryHandler> _logger = logger;
        private readonly IAppDbContext _context = context;

        public async Task<Result<WorkOrdersDto>> Handle(GetWorkOrderByIdQuery query, CancellationToken ct)
        {
            var workOrder = await _context.WorkOrders.AsNoTracking()
                                                .Include(a => a.RepairTasks)
                                                   .ThenInclude(a => a.Parts)
                                                .Include(a => a.Labor)
                                                .Include(a => a.Vehicle!)
                                                    .ThenInclude(v => v.Customer)
                                                .Include(a => a.Invoice)
                                            .FirstOrDefaultAsync(a => a.Id == query.WorkOrderId, ct);

            if (workOrder is null)
            {
                _logger.LogWarning("WorkOrder with id {WorkOrderId} was not found", query.WorkOrderId);

                return ApplicationError.WorkOrderNotFound;
            }

            return workOrder.ToDto();
        }
    }
}
