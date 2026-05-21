using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
namespace MechanicShop.Application.Common.Fetures.Biling.Commands.SettleInvoice
{
    // this command handler is responsible for marking an invoice as paid.
    // It retrieves the invoice from the database,
    // calls the MarkAsPaid method on the invoice entity, and saves the changes to the database.
    // If the invoice is not found or if marking it as paid fails,
    // it logs a warning and returns an appropriate error result.
    // After successfully marking the invoice as paid,
    // it removes any cached data related to invoices and logs an informational message.
    public sealed class SettleInvioceCommandHandler(IAppDbContext context, ILogger<SettleInvioceCommandHandler> logger, HybridCache cache, TimeProvider provider) : IRequestHandler<SettleInvoiceCommand, Result<Success>>
    {
        private readonly IAppDbContext _context = context;
        private readonly ILogger<SettleInvioceCommandHandler> _logger = logger;
        private readonly HybridCache _cache = cache;
        private readonly TimeProvider _provider = provider;

        public async Task<Result<Success>> Handle(SettleInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.Id == request.InvoiceId, cancellationToken);

            if (invoice == null)
            {
                _logger.LogWarning("Invoice with id {InvoiceId} was not found.", request.InvoiceId);
                 return ApplicationError.InvoiceNotFound;
            }

           var payInvoiceResult = invoice.MarkAsPaid(_provider);

            if (payInvoiceResult.IsError)
            { 
            _logger.LogWarning("Failed to mark invoice with id {InvoiceId} as paid. Errors: {Errors}",
                invoice.Id,
                payInvoiceResult.Errors);

                return payInvoiceResult.Errors!;
            }

            await _context.SaveChangesAsync(cancellationToken);
            await _cache.RemoveByTagAsync("invoice", cancellationToken);

            // ADD THIS LINE to clear the schedule!
            await _cache.RemoveByTagAsync("work-order", cancellationToken);

            _logger.LogInformation("Invoice with id {InvoiceId} was marked as paid successfully.", request.InvoiceId);
            return Result.Success;
        }
    }
}
