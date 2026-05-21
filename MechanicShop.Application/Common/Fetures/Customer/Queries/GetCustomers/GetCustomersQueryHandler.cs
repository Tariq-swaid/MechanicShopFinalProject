using MechanicShop.Application.Common.Fetures.Customer.Dtos;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Common.Fetures.Customer.Mappers;
using MechanicShop.Domin.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.Customer.Queries.GetCustomers
{
    public sealed class GetCustomersQueryHandler(IAppDbContext context) : IRequestHandler<GetCustomersQuery, Result<List<CustomerDto>>>
    {
        private readonly IAppDbContext _context = context;

        public async Task<Result<List<CustomerDto>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Customers.Include(c => c.Vehicles)
                .AsNoTracking().Select(c => c.ToDto()).
                ToListAsync(cancellationToken);
        }
    }
}
