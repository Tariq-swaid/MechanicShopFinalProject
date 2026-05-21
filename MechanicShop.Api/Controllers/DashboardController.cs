using MechanicShop.Application.Common.Fetures.Dashboard.Dtos;
using MechanicShop.Application.Common.Fetures.Dashboard.Queries.GetWorkOrderStats;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Controllers
{
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/v{version:apiVersion}/dashboard")]
    public class DashboardController(ISender sender) : ApiController
    {
        private readonly ISender _sender = sender;

        [HttpGet("stats")]
        [ProducesResponseType(typeof(TodatWorkOrderStatsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTodayStats([FromQuery] DateOnly? date,CancellationToken ct )
        {
            var statsDate = date ?? DateOnly.FromDateTime(DateTime.UtcNow);

            var result = await _sender.Send(new GetWorkOrderStatsQuery(statsDate), ct);

            return result.Match(
                request =>
                Ok(request),
                Problem
                );


        }
    }
}
