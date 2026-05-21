using MechanicShop.Application.Common.Fetures.Customer.Dtos;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Common.Models;
using MechanicShop.Domin.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.Customer.Queries.GetCustomers
{
    public sealed record GetCustomersQuery :
       ICachedQuery<Result<List<CustomerDto>>>
    {
        public string CacheKey => "customer";

        public string[] Tags => ["customer"];

        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    }
}
