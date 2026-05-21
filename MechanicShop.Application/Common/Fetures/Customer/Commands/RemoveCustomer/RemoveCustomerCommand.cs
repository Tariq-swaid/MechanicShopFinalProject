using MechanicShop.Domin.Common.Results;
using MediatR;

namespace MechanicShop.Application.Common.Fetures.Customer.Commands.RemoveCustomer
{
    public sealed record RemoveCustomerCommand(Guid CustomerId)
        : IRequest<Result<Deleted>>;
}
