using FluentValidation;
using MechanicShop.Application.Features.WorkOrders.Commands.RelocateWorkOrder;

namespace MechanicShop.Application.Common.Fetures.WorkOrders.Command.RelocateWorkOrder
{
    public class RelocateWorkOrderCommandValidator : AbstractValidator<RelocateWorkOrderCommand>
    {
        public RelocateWorkOrderCommandValidator()
        {
            RuleFor(x => x.WorkOrderId).NotEmpty().WithErrorCode("WorkOrderIdRequired");
            RuleFor(x => x.NewSpot).IsInEnum().WithErrorCode("SpotInvalid");
            RuleFor(x => x.NewStartAt).GreaterThan(DateTimeOffset.UtcNow).WithErrorCode("InvalidTiming");
        }
    }
}
