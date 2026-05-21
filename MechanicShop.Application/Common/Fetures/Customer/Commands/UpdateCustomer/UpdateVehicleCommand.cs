using MechanicShop.Domin.Common.Results;
using MediatR;

namespace MechanicShop.Application.Common.Fetures.Customer.Commands.UpdateCustomer
{
    public sealed record UpdateVehicleCommand(
 Guid? VehicleId,
 string Make,
 string Model,
 int Year,
 string LicensePlate) : IRequest<Result<Updated>>;
}