using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.Biling.Dtos
{
    public class InvoicePdfDto
    {
        public byte[]? Content { get; init; }
        public string? FileName { get; init; }
        public string? ContentType { get; init; } = "application/pdf";
    }
}
