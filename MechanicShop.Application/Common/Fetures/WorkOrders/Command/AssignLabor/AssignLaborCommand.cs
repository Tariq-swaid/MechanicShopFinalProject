using MechanicShop.Domin.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.WorkOrders.Command.AssignLabor
{
    public sealed record AssignLaborCommand (Guid WorkOrderId , Guid LaborId) : IRequest<Result<Updated>>;
}
