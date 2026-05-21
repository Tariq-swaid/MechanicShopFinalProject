using FluentValidation;


namespace MechanicShop.Application.Common.Fetures.Biling.Commands.IssueInvoice
{
    public sealed class IssueInvoiceCommadnValidator : AbstractValidator<IssueInvoiceCommand>
    {
        public IssueInvoiceCommadnValidator()
        {
            RuleFor(r => r.WorkOrderId)
     . NotEmpty().WithMessage("WorkOrderId is required.");
        }
    }
}
