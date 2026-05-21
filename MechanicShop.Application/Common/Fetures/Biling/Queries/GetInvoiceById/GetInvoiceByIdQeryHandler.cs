using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Fetures.Biling.Dtos;
using MechanicShop.Application.Common.Fetures.Biling.Mappers;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.Biling.Queries.GetInvoiceById
{
    public sealed class GetInvoiceByIdQeryHandler(IAppDbContext context, ILogger<GetInvoiceByIdQeryHandler> logger) :
        IRequestHandler<GetInvoiceByIdQery, Result<InvoiceDto>>

    {
        private readonly IAppDbContext _context = context;
        private readonly ILogger<GetInvoiceByIdQeryHandler> _logger = logger;

        public async Task<Result<InvoiceDto>> Handle(GetInvoiceByIdQery request, CancellationToken cancellationToken)
        {
         
            var invoice = await  _context.Invoices.AsNoTracking(). // AsNoTracking is used here because we are only reading the data and not modifying it, which can improve performance. 
                Include(i=> i.LineItems).
                Include(i=> i.WorkOrder!)
                .ThenInclude(v=> v.Vehicle!)
                .ThenInclude(c=> c.Customer!).
                FirstOrDefaultAsync(i => i.Id == request.InvoiceId, cancellationToken);

            if (invoice == null)
            {
                _logger.LogWarning("Invoice with id {InvoiceId} not found", request.InvoiceId);
                return ApplicationError.InvoiceNotFound;
            }

            return invoice.ToDto();


        }
    }
}
