using MechanicShop.Domin.Common.Results;
using MechanicShop.Domin.RepierTask.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.RepairTasks.Command.UpdateRepairTask
{
    public sealed record UpdateRepairTaskCommand(
        Guid RepairTaskId,
        string Name,
        decimal LaborCost,
        RepairDurationInMinutes EstimatedDurationInMins,
        List<UpdateRepairTaskPartsCommand> Parts
    ) : IRequest<Result<Updated>>;
}
