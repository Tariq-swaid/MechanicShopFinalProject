using MechanicShop.Domin.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.RepairTasks.Command.RemoveRepairTask
{
    public sealed record RemoveRepairTaskCommand(Guid RepairTaskId)
         : IRequest<Result<Deleted>>;
}
