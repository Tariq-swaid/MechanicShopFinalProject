using MechanicShop.Domin.Customers;
using MechanicShop.Domin.Customer.Vehicles;
using MechanicShop.Application.Common.Fetures.Customer.Dtos;
using MechanicShop.Application.Common.Fetures.Customer.Mappers;
namespace MechanicShop.Application.Common.Fetures.Customer.Mappers;

public static class CustomerMapper
{
    public static CustomerDto ToDto(this Domin.Customers.Customer entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CustomerDto
        {
            Id = entity.Id,
            Name = entity.Name!,
            Email = entity.Email!,
            PhoneNumber = entity.PhoneNumber!,
            Vehicles = entity.Vehicles?.Select(v => v.ToDto()).ToList() ?? []
        };
    }

    public static List<CustomerDto> ToDtos(this IEnumerable<Domin.Customers.Customer> entities)
    {
     
        return [.. entities.Select(e => ToDto(e))];
    }

    public static VehicleDto ToDto(this Vehicle entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new VehicleDto(entity.Id, entity.Make!, entity.Model!, entity.Year, entity.LicensePlate!);
    }

    public static List<VehicleDto> ToDtos(this IEnumerable<Vehicle> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }
}