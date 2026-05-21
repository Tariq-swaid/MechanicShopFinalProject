using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;
using MechanicShop.Domin.WorkOrders;
using MechanicShop.Domin.WorkOrders.Enums;
using MechanicShop.Domin.WorkOrders.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Common.Fetures.WorkOrders.Command.DeleteWorkOrder
{
    public sealed class DeleteWorkOrderCommandHandler (IAppDbContext context , ILogger<DeleteWorkOrderCommand> logger , HybridCache cache , IWorkOrderPolicy workOrderPolicy) : IRequestHandler<DeleteWorkOrderCommand, Result<Deleted>>
    {
        private readonly IAppDbContext _context = context;
        private readonly ILogger<DeleteWorkOrderCommand> _logger = logger;
        private readonly HybridCache cache = cache;
        private readonly IWorkOrderPolicy _workOrderPolicy = workOrderPolicy;

        public async Task<Result<Deleted>> Handle(DeleteWorkOrderCommand request, CancellationToken cancellationToken)
        {
            var workOrder = await _context.WorkOrders.FirstOrDefaultAsync(x=> x.Id == request.WorkOrderId, cancellationToken);

            if (workOrder is null)
            { 
                   _logger.LogWarning("Work order with id {WorkOrderId} not found.", request.WorkOrderId);
                return ApplicationError.WorkOrderNotFound;
            }

            if (workOrder.State is not WorkOrderState.Scheduled)
            {
                _logger.LogWarning("Work order with id {WorkOrderId} is not in a scheduled state.", request.WorkOrderId);
                return WorkOrdersErros.Readonly;
            }
            _context.WorkOrders.Remove(workOrder);
            await _context.SaveChangesAsync(cancellationToken);
            workOrder.AddDomainEvent(new WorkOrderCollectionModified());
            await cache.RemoveAsync($"work-order_{request.WorkOrderId}", cancellationToken);

            // ADD THIS LINE to clear the schedule!
            await cache.RemoveByTagAsync("work-order", cancellationToken);

            return Result.Deleted;

        }
    }
}
