namespace MechanicShop.Application.Common.Fetures.Customer.Dtos
{
    public sealed record class VehicleDto(Guid VehicalId,
     string Make,
    string Model,
    int Year,
    string LicensePlate
);
 
}
