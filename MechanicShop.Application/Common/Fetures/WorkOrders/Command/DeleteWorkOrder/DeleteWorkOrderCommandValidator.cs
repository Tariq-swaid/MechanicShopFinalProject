using FluentValidation;

namespace MechanicShop.Application.Common.Fetures.WorkOrders.Command.DeleteWorkOrder
{
    public class DeleteWorkOrderCommandValidator : AbstractValidator<DeleteWorkOrderCommand>
    {
        public DeleteWorkOrderCommandValidator()
        {
            RuleFor(x => x.WorkOrderId).NotEmpty().WithErrorCode("WorkOrderId_Required").WithMessage("Work order ID is required.");
        }
    }
}
