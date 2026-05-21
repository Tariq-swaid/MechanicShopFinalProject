using MechanicShop.Application.Common.Fetures.Customer.Dtos;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;


namespace MechanicShop.Application.Common.Fetures.Customer.Queries.GetCustomerById
{
    public sealed record GetCustmoerByIdQuery(Guid CustomerId) : ICachedQuery<Result<CustomerDto>>
    {
        public string CacheKey => $"customer_{CustomerId}";

        public string[] Tags => ["customer"];

        public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    }
}
