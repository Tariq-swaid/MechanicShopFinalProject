using MechanicShop.Domin.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.Biling.Commands.SettleInvoice
{
    public sealed record SettleInvoiceCommand(Guid InvoiceId):IRequest<Result<Success>> ; 

}
