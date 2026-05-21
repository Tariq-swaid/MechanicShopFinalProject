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

namespace MechanicShop.Application.Common.Fetures.WorkOrders.Command.UpdateOrderState
{
    public sealed class UpdateOrderStateCommandHandler(IAppDbContext context , ILogger<UpdateOrderStateCommandHandler> logger , HybridCache cache , TimeProvider provider) : IRequestHandler<UpdateOrderStateCommand, Result<Updated>>
    {
        private readonly IAppDbContext _context = context;
        private readonly ILogger<UpdateOrderStateCommandHandler> _logger = logger;
        private readonly HybridCache _cache = cache;
        private readonly TimeProvider _provider = provider;

        public async Task<Result<Updated>> Handle(UpdateOrderStateCommand request, CancellationToken cancellationToken)
        {

            var workOrder = await _context.WorkOrders.FirstOrDefaultAsync(w => w.Id == request.WorkOrderId, cancellationToken);
            if (workOrder is null)
            { 
                _logger.LogWarning("Work order with id {WorkOrderId} was not found.", request.WorkOrderId);
                return ApplicationError.WorkOrderNotFound;
            }

            if (workOrder.StartAtUtc > _provider.GetUtcNow())
            {
                _logger.LogWarning("Work order with id {WorkOrderId} cannot be updated because it has not started yet.", request.WorkOrderId);
                return WorkOrdersErros.StateTransitionNotAllowed(workOrder.StartAtUtc);

            }

            var UpdateWorkOrderStateResult = workOrder.UpdateState(request.State);

            if (UpdateWorkOrderStateResult.IsError)
            {
                _logger.LogError("Failed to update status: {Error}", UpdateWorkOrderStateResult.TopError.Description);
                   return UpdateWorkOrderStateResult.Errors!;
            }

            if (request.State == WorkOrderState.Completed)
            {
                workOrder.AddDomainEvent(new WorkOrderCompleted { WorkOrderId = request.WorkOrderId });
            }
            await _context.SaveChangesAsync(cancellationToken);
            await _cache.RemoveByTagAsync("work-order", cancellationToken);
            return Result.Updated;
        }
    }
}
