using FluentValidation;


namespace MechanicShop.Application.Common.Fetures.WorkOrders.Queries.GetWorkOrderByIdQuery
{
    public  class GetAppointmentByIdQueryValidator : AbstractValidator<GetWorkOrderByIdQuery>
    {
        public GetAppointmentByIdQueryValidator()
        {


            RuleFor(request => request.WorkOrderId)
                .NotEmpty()
                .WithErrorCode("WorkOrderId_Is_Required")
                .WithMessage("WorkOrderId is required.");
        }
    }
}
