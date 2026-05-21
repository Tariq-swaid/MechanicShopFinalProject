using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.Customer.Commands.CreateCustomer
{
    public class CreateVehicleCommandValidetor : AbstractValidator<CreateVehicleCommand> 
    {
        public CreateVehicleCommandValidetor()
        {
            RuleFor(x=> x.Model).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Make).NotEmpty().MaximumLength(50);
            RuleFor(x => x.LicensePlate).NotEmpty().MaximumLength(20);
        }
    }
}
