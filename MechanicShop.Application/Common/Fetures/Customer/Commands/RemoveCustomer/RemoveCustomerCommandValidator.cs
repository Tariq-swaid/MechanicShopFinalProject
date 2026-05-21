using FluentValidation;

namespace MechanicShop.Application.Common.Fetures.Customer.Commands.RemoveCustomer
{
    public class RemoveCustomerCommandValidator : AbstractValidator<RemoveCustomerCommand>
    {
        public RemoveCustomerCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer Id is required.");
        }
    }
}
