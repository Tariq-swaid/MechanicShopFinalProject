using MechanicShop.Application.Common.Fetures.Biling.Dtos;
using MechanicShop.Domin.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.Biling.Commands.IssueInvoice
{
    public sealed record IssueInvoiceCommand (Guid WorkOrderId) : IRequest<Result<InvoiceDto>>;

}
