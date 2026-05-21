using MechanicShop.Application.Common.Fetures.Customer.Dtos;
using MechanicShop.Domin.Common.Results;
using MechanicShop.Domin.Customer.Vehicles;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.Customer.Commands.CreateCustomer
{
    public sealed record  CreateCustomerCommand(string Name, string Email, string PhoneNumber,List<CreateVehicleCommand> Vehicles) : IRequest<Result<CustomerDto>>;

}
