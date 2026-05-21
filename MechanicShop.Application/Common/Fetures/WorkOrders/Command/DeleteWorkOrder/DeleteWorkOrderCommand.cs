using MechanicShop.Domin.Common.Results;
using MediatR;


namespace MechanicShop.Application.Common.Fetures.WorkOrders.Command.DeleteWorkOrder
{
    public sealed record DeleteWorkOrderCommand(Guid WorkOrderId) : IRequest<Result<Deleted>>;
}
