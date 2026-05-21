using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.Biling.Queries.GetInvoicePdf
{
    public class GetInvoicePdfQueryValidator : AbstractValidator <GetInvoicePdfQuery>
    {
        public GetInvoicePdfQueryValidator()
        {
            RuleFor(x => x.InvoiceId).NotEmpty().WithMessage("Invoice Id is required.");
        }
    }
}
