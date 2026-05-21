using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;
using MechanicShop.Domin.Customer.Vehicles;
using MechanicShop.Domin.Customers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
//using MechanicShop.Application.Fetures.Customer.Mappers;
using MechanicShop.Application.Common.Fetures.Customer.Dtos;
using MechanicShop.Application.Common.Fetures.Customer.Mappers;

namespace MechanicShop.Application.Common.Fetures.Customer.Commands.CreateCustomer
{

    
    public sealed class CreateCustmoerCommandHandler(IAppDbContext context , ILogger<CreateCustmoerCommandHandler> logger , HybridCache cache ) : IRequestHandler<CreateCustomerCommand, Result<CustomerDto>>
    {
        private readonly IAppDbContext _context= context;
        private readonly ILogger<CreateCustmoerCommandHandler> _logger = logger;
        private readonly HybridCache _cache = cache;

        
    public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
                
            var email = command.Email.Trim().ToLower();

            var existing = await _context.Customers.AnyAsync(x => x.Email!.ToLower() == email,cancellationToken);

            if (existing)
            {
                _logger.LogWarning("Customer with email {Email} already exists.", email);
                return ErrorCustomer.CustomerExists;

            }

            List<Vehicle> vehicles = [];

            foreach (var vehicle in command.Vehicles)
            {
                var vehicleResult = Vehicle.Create(Guid.NewGuid(), vehicle.Make, vehicle.Year, vehicle.Model, vehicle.LicensePlate);

                if (vehicleResult.IsError)
                { 
                    return vehicleResult.Errors!;
                }
                vehicles.Add(vehicleResult.Value);

            }

            var CustomerResult = Domin.Customers.Customer.Create(Guid.NewGuid(), command.Name, email, command.PhoneNumber, vehicles);

            if (CustomerResult.IsError)
            {
                return CustomerResult.Errors!;

            }

             _context.Customers.Add(CustomerResult.Value);
            await _context.SaveChangesAsync(cancellationToken);
            var customer = CustomerResult.Value;
            _logger.LogInformation("Customer was created successfuly id : {CustomerId}", CustomerResult.Value.Id);

            await _cache.RemoveByTagAsync("customer", cancellationToken);

            return customer.ToDto();



        }
    }

 }

