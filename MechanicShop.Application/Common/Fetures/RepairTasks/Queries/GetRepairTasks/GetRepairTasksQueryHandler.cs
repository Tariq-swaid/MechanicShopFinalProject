using MechanicShop.Application.Common.Fetures.RepairTasks.Dtos;
using MechanicShop.Application.Common.Fetures.RepairTasks.Mappers;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace MechanicShop.Application.Common.Fetures.RepairTasks.Queries.GetRepairTasks
{
    public class GetRepairTasksQueryHandler(IAppDbContext context)
     : IRequestHandler<GetRepairTasksQuery, Result<List<RepairTaskDto>>>
    {
        private readonly IAppDbContext _context = context;

        public async Task<Result<List<RepairTaskDto>>> Handle(GetRepairTasksQuery query, CancellationToken ct)
        {
            var repairTasks = await _context.RepairTasks.Include(rt => rt.Parts).AsNoTracking().ToListAsync(ct);

            return repairTasks.ToDtos();
        }
    }
}
