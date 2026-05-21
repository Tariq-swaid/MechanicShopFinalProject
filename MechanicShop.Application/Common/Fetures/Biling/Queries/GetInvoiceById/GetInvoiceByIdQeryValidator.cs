using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.Biling.Queries.GetInvoiceById
{
    public  class GetInvoiceByIdQeryValidator : AbstractValidator<GetInvoiceByIdQery>
    {
        public GetInvoiceByIdQeryValidator()
        {
                RuleFor(request => request.InvoiceId)
                .NotEmpty()
                .WithErrorCode("InvoiceId_Is_Required")
                .WithMessage("InvoiceId is required.");
        }
    }
}
