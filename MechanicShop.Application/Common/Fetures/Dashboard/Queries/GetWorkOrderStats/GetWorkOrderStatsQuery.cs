using MechanicShop.Application.Common.Fetures.Dashboard.Dtos;
using MechanicShop.Domin.Common.Results;
using MediatR;

namespace MechanicShop.Application.Common.Fetures.Dashboard.Queries.GetWorkOrderStats
{
    public sealed record GetWorkOrderStatsQuery(DateOnly Date) : IRequest<Result<TodatWorkOrderStatsDto>>;

}
