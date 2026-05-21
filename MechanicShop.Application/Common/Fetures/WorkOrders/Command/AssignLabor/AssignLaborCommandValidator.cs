using FluentValidation;

namespace MechanicShop.Application.Common.Fetures.WorkOrders.Command.AssignLabor
{
    public class AssignLaborCommandValidator : AbstractValidator<AssignLaborCommand>
    {
        public AssignLaborCommandValidator()
        {
             RuleFor(re=> re.WorkOrderId).NotEmpty().WithMessage("WorkOrderId is required").WithErrorCode("WorkOrderId_Required")
;
            RuleFor(re=> re.LaborId).NotEmpty().WithMessage("LaborId is required").WithErrorCode("LaborId_Required")
;
        }
    }
}
