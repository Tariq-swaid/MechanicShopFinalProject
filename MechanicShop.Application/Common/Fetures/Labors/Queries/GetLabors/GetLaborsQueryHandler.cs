using MechanicShop.Application.Common.Fetures.Labors.Dtos;
using MechanicShop.Application.Common.Fetures.Labors.Mappers;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;
using MechanicShop.Domin.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace MechanicShop.Application.Common.Fetures.Labors.Queries.GetLabors
{
    public sealed class GetLaborsQueryHandler(IAppDbContext context, ILogger<GetLaborsQueryHandler> logger) :
        IRequestHandler<GetLaborsQuery, Result<List<LaborDto>>>
    {
        private readonly IAppDbContext _context = context;
        private readonly ILogger<GetLaborsQueryHandler> _logger = logger;

        public async  Task<Result<List<LaborDto>>> Handle(GetLaborsQuery request, CancellationToken cancellationToken)
        {
            var labors = await _context.Employes.AsNoTracking().Where(x=> x.Role == Role.Labor).ToListAsync(cancellationToken);

            if (labors is null || labors.Count == 0)
            {
                _logger.LogWarning("No labors found in the database.");
                return Error.NotFound (code:"NoLaborsFound",description: " No Labots are found create a new labor now ! ");
            }

            return labors.ToDtos();

        }
    }
}
