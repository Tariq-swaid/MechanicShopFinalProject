using MechanicShop.Application.Common.Fetures.RepairTasks.Dtos;
using MechanicShop.Domin.Common.Results;
using MechanicShop.Domin.RepierTask.Enums;
using MediatR;

namespace MechanicShop.Application.Common.Fetures.RepairTasks.Command.CreateRepairTask
{
    public sealed record CreateRepairTaskCommand ( string? Name,
    decimal LaborCost,
    RepairDurationInMinutes? EstimatedDurationInMins,
    List<CreateRepairTaskPartsCommand> Parts
) : IRequest<Result<RepairTaskDto>>;
}

