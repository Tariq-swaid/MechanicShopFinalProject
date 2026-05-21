using MechanicShop.Application.Common.Fetures.Biling.Dtos;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Common.Fetures.Biling.Queries.GetInvoicePdf
{
    public sealed class GetInvoicePdfQueryHandler(IAppDbContext context, IInvoicePdfGenerator pdfGenerator,
        ILogger<GetInvoicePdfQueryHandler> logger): IRequestHandler<GetInvoicePdfQuery, Result<InvoicePdfDto>>
    {
        private readonly IAppDbContext _context = context;
        private readonly IInvoicePdfGenerator _pdfGenerator = pdfGenerator;
        private readonly ILogger<GetInvoicePdfQueryHandler> _logger = logger;

        public async Task<Result<InvoicePdfDto>> Handle(GetInvoicePdfQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _context.Invoices.AsNoTracking()
       .Include(i => i.LineItems)
       .FirstOrDefaultAsync(i => i.Id == request.InvoiceId, cancellationToken);

            if (invoice is null)
            {
                _logger.LogWarning("Invoice not found. InvoiceId: {InvoiceId}", request.InvoiceId);
                return Error.NotFound("Invoice not found.");
            }

            try
            {
                var pdfBytes = _pdfGenerator.Generate(invoice);

                var invoicePdf = new InvoicePdfDto
                {
                    Content = pdfBytes,
                    FileName = $"invoice_{request.InvoiceId}.pdf"
                };

                return invoicePdf;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate PDF for InvoiceId: {InvoiceId}", request.InvoiceId);
                return Error.Failure("An error occurred while generating the invoice PDF.");
            }
        }
    }
}
