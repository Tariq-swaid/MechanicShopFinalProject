using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Fetures.WorkOrders.Dtos;
using MechanicShop.Application.Common.Fetures.WorkOrders.Mappers;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;
using MechanicShop.Domin.WorkOrders;
using MechanicShop.Domin.WorkOrders.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;


namespace MechanicShop.Application.Common.Fetures.WorkOrders.Command.CreateWorkOrder
{
    public sealed class CreateWorkOrderCommandHandler(IAppDbContext context, ILogger<CreateWorkOrderCommandHandler> logger, IWorkOrderPolicy policy, HybridCache cache ):

        IRequestHandler<CreateWorkOrderCommand, Result<WorkOrdersDto>>
    {
        private readonly IAppDbContext _context = context;
        private readonly ILogger<CreateWorkOrderCommandHandler> _logger = logger;
        private readonly IWorkOrderPolicy _policy = policy;
        private readonly HybridCache _cache = cache;

        public async Task<Result<WorkOrdersDto>> Handle(CreateWorkOrderCommand request, CancellationToken cancellationToken)
        {
                
            var repairTask = await _context.RepairTasks.Where(x=> request.RepairTaskIds.Contains(x.Id)).ToListAsync(cancellationToken);

            if (repairTask.Count != request.RepairTaskIds.Count)

            {
                var missingIds = request.RepairTaskIds.Except(repairTask.Select(t => t.Id)).ToArray(); // Get the missing IDs
                _logger.LogWarning("Some RepairTaskIds were not found: {MissingIds}", string.Join(", ", missingIds));
                return ApplicationError.RepairTaskNotFound;
            }


            var totalEstimatedDuration = TimeSpan.FromMinutes(repairTask.Sum(r => (int)r.RepairDurationInMinutes));
            var endAt = request.StartAt.Add(totalEstimatedDuration);

            if (_policy.IsOutsideOperatingHours(request.StartAt, totalEstimatedDuration))
            {
                _logger.LogError("The WorkOrder time ({StartAt} ? {EndAt}) is outside of store operating hours.", request.StartAt, endAt);

                return ApplicationError.WorkOrderOutsideOperatingHour(request.StartAt, endAt);
            }

            var checkMinRequirementResult = _policy.ValidateMinimumRequirement(request.StartAt, endAt);

            if (checkMinRequirementResult.IsError)
            {
                _logger.LogError("WorkOrder duration is shorter than the configured minimum.");

                return checkMinRequirementResult.Errors!;
            }

            var checkSpotAvailabilityResult = await _policy.CheckSpotAvailabilityAsync(
                request.Spot,
                request.StartAt,
                endAt,
                excludeWorkOrderId: null,
                ct: cancellationToken
                );

            if (checkSpotAvailabilityResult.IsError)
            {
                _logger.LogError("Spot: {Spot} is not available.", request.Spot.ToString());
                return checkSpotAvailabilityResult.Errors!;
            }

            var vehicle = await _context.Vehicles.Include(v => v.Customer).FirstOrDefaultAsync(v => v.Id == request.VehicleId, cancellationToken: cancellationToken);

            if (vehicle is null)
            {
                _logger.LogError("Vehicle with Id '{VehicleId}' does not exist.", request.VehicleId);
                return ApplicationError.VehicleNotFound;
            }

            var labor = await _context.Employes.FindAsync([request.LaborId], cancellationToken);

            if (labor is null)
            {
                _logger.LogError("Invalid LaborId: {LaborId}", request.LaborId.ToString());
                return ApplicationError.LaborNotFound;
            }

            var hasVehicleConflict = await _context.WorkOrders
                .AnyAsync(
                    a =>
                    a.VehicleId == request.VehicleId &&
                    a.StartAtUtc.Date == request.StartAt.Date &&
                    a.StartAtUtc < endAt &&
                    a.EndAtUtc > request.StartAt,
                    cancellationToken);
            if (hasVehicleConflict)
            {
                _logger.LogError("Vehicle with Id '{VehicleId}' already has an overlapping WorkOrder.", request.VehicleId);
                return Error.Conflict(
                    code: "Vehicle_Overlapping_WorkOrders",
                    description: "The vehicle already has an overlapping WorkOrder.");
            }

            var isLaborOccupied = await _context.WorkOrders
                .AnyAsync(
                    a =>
                    a.LaborId == request.LaborId &&
                    a.StartAtUtc < endAt &&
                    a.EndAtUtc > request.StartAt,
                    cancellationToken);

            if (isLaborOccupied)
            {
                _logger.LogError("Labor with Id '{LaborId}' is already occupied during the requested time.", request.LaborId);
                return Error.Conflict(
                    code: "Labor_Occupied",
                    description: "Labor is already occupied during the requested time.");
            }

            var createWorkOrderResult = WorkOrder.Create(
                Guid.NewGuid(),
                request.VehicleId,
                request.StartAt,
                endAt,
                request.LaborId!.Value,
                request.Spot,
                repairTask);

            if (createWorkOrderResult.IsError)
            {
                _logger.LogError("Failed to create WorkOrder: {Error}", createWorkOrderResult.TopError.Description);

                return createWorkOrderResult.Errors!;
            }

            var workOrder = createWorkOrderResult.Value;

            _context.WorkOrders.Add(workOrder);

            workOrder.AddDomainEvent(new WorkOrderCollectionModified());

            await _context.SaveChangesAsync(cancellationToken);

            workOrder.Vehicle = vehicle;
            workOrder.Labor = labor;

            _logger.LogInformation("WorkOrder with Id '{WorkOrderId}' created successfully.", workOrder.Id);

            await _cache.RemoveByTagAsync("work-order", cancellationToken);

            return workOrder.ToDto();
        }

    }
    }

