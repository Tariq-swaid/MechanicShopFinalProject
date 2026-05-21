using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.Biling.Commands.SettleInvoice
{
    public  class SettleInvioceCommandValidator : AbstractValidator<SettleInvoiceCommand>
    {
        public SettleInvioceCommandValidator()
        {
            RuleFor(x => x.InvoiceId).NotEmpty().WithMessage("InvoiceId is required.");
        }
    }
}
