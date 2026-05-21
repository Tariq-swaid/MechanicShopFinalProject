using FluentValidation;
//using MechanicShop.Application.Fetures.Customer.Commands.CreateCustomer;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.Customer.Commands.CreateCustomer
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().
                WithMessage("Name requried").
                MaximumLength(100);

            RuleFor(x => x.Email).NotEmpty().
                WithMessage("Email requried").
                EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.PhoneNumber).NotEmpty().
                WithMessage("Phone number requried").
                Matches(@"^\+?\d{7,15}$").WithMessage("phone number must be start with + ");

            RuleFor(x => x.Vehicles).NotNull().WithMessage("vehcil List can't be null")
                .Must(p=> p.Count > 0).WithMessage("At Lest one vehicle is required");

            RuleForEach(x => x.Vehicles).SetValidator(new CreateVehicleCommandValidetor());

        }
    }
}
