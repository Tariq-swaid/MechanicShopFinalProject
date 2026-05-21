using MechanicShop.Application.Common.Fetures.Biling.Dtos;
using MechanicShop.Domin.Common.Results;
using MediatR;
namespace MechanicShop.Application.Common.Fetures.Biling.Queries.GetInvoicePdf
{
    public sealed record GetInvoicePdfQuery(Guid InvoiceId): IRequest<Result<InvoicePdfDto>>;
    
}
