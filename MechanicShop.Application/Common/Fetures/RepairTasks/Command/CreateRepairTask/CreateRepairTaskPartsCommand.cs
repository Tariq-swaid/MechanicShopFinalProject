using MechanicShop.Domin.Common.Results;
using MediatR;
namespace MechanicShop.Application.Common.Fetures.RepairTasks.Command.CreateRepairTask
{
    public sealed record CreateRepairTaskPartsCommand (string Name, decimal Cost,int Quantity) : IRequest<Result<Success>>;

}
