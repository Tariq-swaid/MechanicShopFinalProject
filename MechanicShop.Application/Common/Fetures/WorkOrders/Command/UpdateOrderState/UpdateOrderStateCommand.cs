using MechanicShop.Domin.Common.Results;
using MechanicShop.Domin.WorkOrders.Enums;
using MediatR;

namespace MechanicShop.Application.Common.Fetures.WorkOrders.Command.UpdateOrderState
{
    public sealed record UpdateOrderStateCommand(Guid WorkOrderId, WorkOrderState State): IRequest<Result<Updated>>;
}
