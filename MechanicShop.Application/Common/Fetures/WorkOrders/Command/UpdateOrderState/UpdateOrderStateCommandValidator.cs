using FluentValidation;

namespace MechanicShop.Application.Common.Fetures.WorkOrders.Command.UpdateOrderState
{
    public class UpdateOrderStateCommandValidator : AbstractValidator<UpdateOrderStateCommand>
    {
        public UpdateOrderStateCommandValidator()
        {
            RuleFor(x => x.WorkOrderId).NotEmpty();
            RuleFor(x => x.State).IsInEnum();
        }
    }
}
