using FluentValidation;
using MechanicShop.Domin.Common.Results;
using MediatR;


namespace MechanicShop.Application.Common.Fetures.Identity.Queries.GenerateTokens
{
    public   class GenerateTokensQueryValidtor : AbstractValidator <GenerateTokensQuery>
    {
        public GenerateTokensQueryValidtor()
        {
            RuleFor(request => request.Email)
                .NotNull().NotEmpty()
                .WithErrorCode("Email_Null_Or_Empty")
                .WithMessage("Email cannot be null or empty");

            RuleFor(request => request.Password)
                .NotNull().NotEmpty()
                .WithErrorCode("Password_Null_Or_Empty")
                .WithMessage("Password cannot be null or empty.");

        }
    }


}
