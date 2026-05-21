using MechanicShop.Application.Common.Fetures.RepairTasks.Dtos;
using MechanicShop.Application.Common.Fetures.RepairTasks.Mappers;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;
using MechanicShop.Domin.RepierTask;
using MechanicShop.Domin.RepierTask.Parts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Common.Fetures.RepairTasks.Command.CreateRepairTask
{
    public class CreateRepairTaskCommandHandler(
    ILogger<CreateRepairTaskCommandHandler> logger,
    IAppDbContext context,
    HybridCache cache
    )
    : IRequestHandler<CreateRepairTaskCommand, Result<RepairTaskDto>>
    {
        private readonly ILogger<CreateRepairTaskCommandHandler> _logger = logger;
        private readonly IAppDbContext _context = context;
        private readonly HybridCache _cache = cache;

        public async Task<Result<RepairTaskDto>> Handle(CreateRepairTaskCommand command, CancellationToken ct)
        {
            var nameExists = await _context.RepairTasks
               .AnyAsync(p => EF.Functions.Like(p.Name, command.Name), ct);

            if (nameExists)
            {
                _logger.LogWarning("Duplicate part name '{PartName}'.", command.Name);

                return RepairTaskErros.DuplicateName;
            }

            List<Part> parts = [];

            foreach (var p in command.Parts)
            {
                var partResult = Part.Create(Guid.NewGuid(), p.Name,p.Quantity, p.Cost);

                if (partResult.IsError)
                {
                    return partResult.Errors!;
                }

                parts.Add(partResult.Value);
            }

            var createRepairTaskResult = RepairTask.Create(
                        id: Guid.NewGuid(),
                        name: command.Name!,
                        laborCost: command.LaborCost,
                        repairDurationInMinutes: command.EstimatedDurationInMins!.Value,
                        parts: parts);

            if (createRepairTaskResult.IsError)
            {
                return createRepairTaskResult.Errors!;
            }

            var repairTask = createRepairTaskResult.Value;

            _context.RepairTasks.Add(repairTask);

            await _context.SaveChangesAsync(ct);

            await _cache.RemoveByTagAsync("repair-task", ct);

            return repairTask.ToDto();
        }
    }
}
