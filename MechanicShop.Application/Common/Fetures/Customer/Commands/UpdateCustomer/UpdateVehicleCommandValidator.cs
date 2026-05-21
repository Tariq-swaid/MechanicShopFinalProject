using FluentValidation;

namespace MechanicShop.Application.Common.Fetures.Customer.Commands.UpdateCustomer
{
    public sealed class UpdateVehicleCommandValidator : AbstractValidator<UpdateVehicleCommand>
    {
        public UpdateVehicleCommandValidator()
        {
            RuleFor(x => x.Make)
                .NotEmpty().MaximumLength(50);

            RuleFor(x => x.Model)
                .NotEmpty().MaximumLength(50);

            RuleFor(x => x.LicensePlate)
                .NotEmpty().MaximumLength(10);
        }
    }
}