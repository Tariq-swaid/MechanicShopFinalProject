using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.RepairTasks.Command.UpdateRepairTask
{
    public sealed record UpdateRepairTaskPartsCommand(Guid? PartId,
    string Name,
    decimal Cost,
    int Quantity);
}
