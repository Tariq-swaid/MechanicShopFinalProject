using MechanicShop.Application.Common.Fetures.Customer.Dtos;
using MechanicShop.Application.Common.Fetures.Customer.Mappers;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Common.Fetures.Customer.Queries.GetCustomerById
{
    public sealed class GetCustomerByIdQueryHandler(IAppDbContext context , ILogger <GetCustomerByIdQueryHandler> logger) : IRequestHandler<GetCustmoerByIdQuery, Result<CustomerDto>>
    {
        private readonly IAppDbContext _context = context;

        public ILogger<GetCustomerByIdQueryHandler> _logger { get; } = logger;

        public async Task<Result<CustomerDto>> Handle(GetCustmoerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers
                .AsNoTracking()
                .Include(c => c.Vehicles)
                .FirstOrDefaultAsync(c => c.Id == request.CustomerId, cancellationToken);

            if (customer == null)
            {
                _logger.LogWarning("Customer with id {CustomerId} not found", request.CustomerId);

                return Error.NotFound(
                    code: "CustomerNotFound",
                    description: $"Customer with id {request.CustomerId} not found"

                    );
            }

            return customer.ToDto();
        
        }
    }
}
