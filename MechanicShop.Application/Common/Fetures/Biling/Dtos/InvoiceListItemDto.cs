using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.Biling.Dtos
{
    public class InvoiceListItemDto
    {
        public Guid InvoicedId { get; set; }
        public int LineNumber { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? LineTotal { get; set; }
    }
}
