using MechanicShop.Application.Common.Fetures.Biling.Dtos;
using MechanicShop.Application.Common.Fetures.Customer.Mappers;
using MechanicShop.Domin.WorkOrders.Biling;

namespace MechanicShop.Application.Common.Fetures.Biling.Mappers
{
    public static class InvoiceMapper
    {
        public static InvoiceDto ToDto(this Invoice invoice)
        {
            ArgumentNullException.ThrowIfNull(invoice);

            return new InvoiceDto
            {
                InvoiceId = invoice.Id,
                WorkOrderId = invoice.WorkOrderId,
                Customer = invoice.WorkOrder!.Vehicle!.Customer!.ToDto(),
                IssuedAtUtc = invoice.IssuedAtUtc,
                Subtotal = invoice.Subtotal,
                TaxAmount = invoice.TaxAmount,
                DiscountAmount = invoice.DiscountAmount,
                Total = invoice.Total,
                PaymentStatus = invoice.Status.ToString(),
                Items = invoice.LineItems.Select(li => li.ToDto()).ToList()

            };
        }

        public static List<InvoiceDto> ToDtos(this IEnumerable<Invoice> invoices)
        {
            return invoices.Select(e => e.ToDto()).ToList();
        }
    
        public static InvoiceListItemDto ToDto(this InvioceLineItem item)
        { 
            return new InvoiceListItemDto
            {
                InvoicedId = item.InvoiceId,
                LineNumber = item.LineNumber,
                Description = item.Description,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                LineTotal = item.LineTotal
            };
        }
        
        public static List<InvoiceListItemDto> ToDos(this IEnumerable<InvioceLineItem> items)
        {
            return [.. items.Select(e => e.ToDto())];
        }
    }
}
