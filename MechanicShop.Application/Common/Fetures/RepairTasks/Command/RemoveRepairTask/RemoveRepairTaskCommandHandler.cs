using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;
using MechanicShop.Domin.RepierTask;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.RepairTasks.Command.RemoveRepairTask
{
    public class RemoveRepairTaskCommandHandler(
      ILogger<RemoveRepairTaskCommandHandler> logger,
      IAppDbContext context,
      HybridCache cache
      )
      : IRequestHandler<RemoveRepairTaskCommand, Result<Deleted>>
    {
        private readonly ILogger<RemoveRepairTaskCommandHandler> _logger = logger;
        private readonly IAppDbContext _context = context;
        private readonly HybridCache _cache = cache;

        public async Task<Result<Deleted>> Handle(RemoveRepairTaskCommand command, CancellationToken ct)
        {
            var repairTask = await _context.RepairTasks
                .FindAsync([command.RepairTaskId], ct);

            if (repairTask is null)
            {
                _logger.LogWarning("RepairTask {RepairTaskId} not found for deletion.", command.RepairTaskId);
                return ApplicationError.RepairTaskNotFound;
            }

            var isInUse = await _context.WorkOrders.AsNoTracking()
                .SelectMany(x => x.RepairTasks)
                .AnyAsync(rt => rt.Id == command.RepairTaskId, ct);

            if (isInUse)
            {
                _logger.LogWarning("RepairTask {RepairTaskId} cannot be deleted — in use by work orders.", command.RepairTaskId);

                return RepairTaskErros.InUse;
            }

            _context.RepairTasks.Remove(repairTask);
            await _context.SaveChangesAsync(ct);

            await _cache.RemoveByTagAsync("repair-tasks", ct);

            _logger.LogInformation("RepairTask {RepairTaskId} deleted successfully.", command.RepairTaskId);

            return Result.Deleted;
        }
    }
}
