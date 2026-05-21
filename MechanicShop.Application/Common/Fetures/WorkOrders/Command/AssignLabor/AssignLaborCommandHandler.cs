using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Common.Fetures.WorkOrders.Command.AssignLabor
{
    public sealed class AssignLaborCommandHandler(IAppDbContext context, ILogger<AssignLaborCommandHandler> logger,IWorkOrderPolicy policy , HybridCache cache) : IRequestHandler<AssignLaborCommand, Result<Updated>>

    {
        private readonly IAppDbContext _context = context;
        private readonly ILogger<AssignLaborCommandHandler> _logger = logger;
        private readonly IWorkOrderPolicy _policy = policy;
        private readonly HybridCache _cache = cache;

        public async Task<Result<Updated>> Handle(AssignLaborCommand request, CancellationToken cancellationToken)
        {
            var workorder = await _context.WorkOrders.FirstOrDefaultAsync(x => x.Id == request.WorkOrderId, cancellationToken);

            if (workorder == null)
            {
                _logger.LogWarning("WorkOrder with id {WorkOrderId} was not found.", request.WorkOrderId);
                return ApplicationError.WorkOrderNotFound;
            }

            var lobar = await _context.Employes.FindAsync([request.LaborId]);

            if (lobar == null)
            { 
            _logger.LogWarning("Labor with id {LaborId} was not found.", request.LaborId);
                return ApplicationError.LaborNotFound;
            }
            if (await _policy.IsLaborOccupied(request.LaborId, request.WorkOrderId, workorder.StartAtUtc, workorder.EndAtUtc))
            {
                    _logger.LogWarning("Labor with id {LaborId} is occupied during the scheduled time for WorkOrder with id {WorkOrderId}.", request.LaborId, request.WorkOrderId);
                    return ApplicationError.LaborOccupied;
            }
            var UpdateLoaberResult = workorder.UpdateLabor(request.LaborId);
            
            if (UpdateLoaberResult.IsError)
            {
                foreach (var error in UpdateLoaberResult.Errors!)
                {
                    _logger.LogError("[LaborUpdate] {ErrorCode}: {ErrorDescription}", error.Code, error.Description);
                }

                return UpdateLoaberResult.Errors;
            }
            await _context.SaveChangesAsync(cancellationToken);

            await _cache.RemoveByTagAsync("work-order", cancellationToken);

            return Result.Updated;

        }
    }
}
