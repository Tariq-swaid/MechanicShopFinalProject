using MechanicShop.Application.Common.Fetures.Biling.Dtos;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.Biling.Queries.GetInvoiceById
{
    public sealed record GetInvoiceByIdQery(Guid InvoiceId) : ICachedQuery<Result<InvoiceDto>>
    {
        public string CacheKey => $"invoice_{InvoiceId}";

        public string[] Tags =>["invoice"];

        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    }
}
