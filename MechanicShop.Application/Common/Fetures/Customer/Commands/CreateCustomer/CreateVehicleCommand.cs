using MechanicShop.Application.Common.Fetures.Customer.Dtos;
using MechanicShop.Domin.Common.Results;
using MediatR;

namespace MechanicShop.Application.Common.Fetures.Customer.Commands.CreateCustomer
{
    public sealed record CreateVehicleCommand(string Make, string Model, int Year, string LicensePlate) :IRequest<Result<VehicleDto>>;

}
