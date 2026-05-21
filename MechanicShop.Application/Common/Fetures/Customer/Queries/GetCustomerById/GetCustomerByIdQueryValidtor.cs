using FluentValidation;


namespace MechanicShop.Application.Common.Fetures.Customer.Queries.GetCustomerById
{
 public class GetCustomerByIdQueryValidator  : AbstractValidator<GetCustmoerByIdQuery>
    {
        public GetCustomerByIdQueryValidator()
        {
            RuleFor(r=> r.CustomerId).NotEmpty().
                WithErrorCode("CustomerId_Is_Required").
                WithMessage("CustomerId is required");
        }

    }
 
}

