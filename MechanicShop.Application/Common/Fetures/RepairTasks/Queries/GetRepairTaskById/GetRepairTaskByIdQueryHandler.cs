using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Fetures.RepairTasks.Dtos;
using MechanicShop.Application.Common.Fetures.RepairTasks.Mappers;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.RepairTasks.Queries.GetRepairTaskById
{
    public class GetRepairTaskByIdQueryHandler(
      ILogger<GetRepairTaskByIdQueryHandler> logger,
      IAppDbContext context
      )
      : IRequestHandler<GetRepairTaskByIdQuery, Result<RepairTaskDto>>
    {
        private readonly ILogger<GetRepairTaskByIdQueryHandler> _logger = logger;
        private readonly IAppDbContext _context = context;

        public async Task<Result<RepairTaskDto>> Handle(GetRepairTaskByIdQuery query, CancellationToken ct)
        {
            var repairTask = await _context.RepairTasks.AsNoTracking().Include(c => c.Parts)
                                         .FirstOrDefaultAsync(c => c.Id == query.RepairTaskId, ct);

            if (repairTask is null)
            {
                _logger.LogWarning("Repair task with id {RepairTaskId} was not found", query.RepairTaskId);

                return ApplicationError.RepairTaskNotFound;
            }

            return repairTask.ToDto();
        }
    }
}
