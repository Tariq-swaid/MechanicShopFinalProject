using MechanicShop.Application.Common.Fetures.RepairTasks.Command.CreateRepairTask;
using MechanicShop.Application.Common.Fetures.RepairTasks.Command.RemoveRepairTask;
using MechanicShop.Application.Common.Fetures.RepairTasks.Command.UpdateRepairTask;
using MechanicShop.Application.Common.Fetures.RepairTasks.Dtos;
using MechanicShop.Application.Common.Fetures.RepairTasks.Queries.GetRepairTaskById;
using MechanicShop.Application.Common.Fetures.RepairTasks.Queries.GetRepairTasks;
using MechanicShop.Contracts.Requests.RepairTasks;
using MechanicShop.Domin.Identity;
using MechanicShop.Domin.RepierTask.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Controllers
{
    [Route("api/v{version:apiVersion}/repair-tasks")]
    [ApiVersion("1.0")]
    [Authorize]
    public sealed class RepairTasksController(ISender sender) : ApiController
    {
        private readonly ISender _sender = sender;

        [HttpGet]
        [ProducesResponseType(typeof(List<RepairTaskDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        // تركنا هذا السطر لأنه يخبر المتصفح ألا يخزن البيانات، وحذفنا الـ OutputCache الذي كان يخزنها في السيرفر
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [EndpointSummary("Retrieves all repair tasks.")]
        [EndpointDescription("Returns a list of all repair tasks available in the system.")]
        [EndpointName("GetRepairTasks")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Get(CancellationToken ct)
        {
            var result = await _sender.Send(new GetRepairTasksQuery(), ct);

            return result.Match(
                response => Ok(response),
                Problem);
        }

        [HttpGet("{repairTaskId:guid}", Name = nameof(GetById))]
        [ProducesResponseType(typeof(RepairTaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)] // أضفناه هنا أيضاً للأمان
        [EndpointSummary("Retrieves a repair task by ID.")]
        [EndpointDescription("Returns detailed information for the specified repair task if it exists.")]
        [EndpointName("GetRepairTaskById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetById(Guid repairTaskId, CancellationToken ct)
        {
            var result = await _sender.Send(new GetRepairTaskByIdQuery(repairTaskId), ct);

            return result.Match(
                response => Ok(response),
                Problem);
        }

        [HttpPost]
        [Authorize(Roles = nameof(Role.Manager))]
        [ProducesResponseType(typeof(RepairTaskDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Creates a new repair task.")]
        [EndpointDescription("Creates a repair task and optionally includes parts.")]
        [EndpointName("CreateRepairTask")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Create([FromBody] CreateRepairTaskRequest request, CancellationToken ct)
        {
            var parts = request.Parts
                .ConvertAll(p => new CreateRepairTaskPartsCommand(p.Name, p.Cost, p.Quantity));

            var command = new CreateRepairTaskCommand(
                request.Name,
                request.LaborCost,
                request.EstimatedDurationInMins is not null ? (RepairDurationInMinutes)request.EstimatedDurationInMins : null,
                parts);

            var result = await _sender.Send(command, ct);

            return result.Match(
                response => CreatedAtAction(nameof(GetById), new { repairTaskId = response.RepairTaskId }, response),
                Problem);
        }

        
        [HttpPut("{repairTaskId:guid}")]
        [Authorize(Roles = nameof(Role.Manager))]
        [ProducesResponseType(typeof(RepairTaskDto), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Updates an existing repair task.")]
        [EndpointDescription("Updates a repair task and its associated parts.")]
        [EndpointName("UpdateRepairTask")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Update(Guid repairTaskId, [FromBody] UpdateRepairTaskRequest request, CancellationToken ct)
        {
            var parts = request.Parts
                .ConvertAll(p => new UpdateRepairTaskPartsCommand(p.PartId, p.Name, p.Cost, p.Quantity));

            var command = new UpdateRepairTaskCommand(
                repairTaskId,
                request.Name,
                request.LaborCost,
                (RepairDurationInMinutes)request.EstimatedDurationInMins,
                parts);

            var result = await _sender.Send(command, ct);

            return result.Match(
                response => Ok(response),
                Problem);
        }

        [HttpDelete("{repairTaskId:guid}")]
        [Authorize(Roles = nameof(Role.Manager))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [EndpointSummary("Removes a repair task.")]
        [EndpointDescription("Deletes the specified repair task from the system.")]
        [EndpointName("RemoveRepairTask")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete(Guid repairTaskId, CancellationToken ct)
        {
            var result = await _sender.Send(new RemoveRepairTaskCommand(repairTaskId), ct);

            return result.Match(
                _ => NoContent(),
                Problem);
        }
    }
}