
using FluentValidation;

namespace MechanicShop.Application.Common.Fetures.Dashboard.Queries.GetWorkOrderStats
{
    public class GetWorkOrderStatsQueryVaildetor : AbstractValidator<GetWorkOrderStatsQuery>
    {
        public GetWorkOrderStatsQueryVaildetor()
        {
            RuleFor(r => r.Date).NotEmpty().WithErrorCode("Date_Is_Required")
                .WithMessage("Date is required.");
        }
    }
}
