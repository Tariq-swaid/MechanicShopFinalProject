using MechanicShop.Application.Common.Fetures.WorkOrders.Dtos;
using MechanicShop.Domin.Common.Results;
using MechanicShop.Domin.WorkOrders.Enums;
using MediatR;


namespace MechanicShop.Application.Common.Fetures.WorkOrders.Command.CreateWorkOrder
{
    public sealed record CreateWorkOrderCommand(Spot Spot, Guid VehicleId, DateTimeOffset StartAt, List<Guid> RepairTaskIds, Guid? LaborId) : IRequest<Result<WorkOrdersDto>>;
    }

