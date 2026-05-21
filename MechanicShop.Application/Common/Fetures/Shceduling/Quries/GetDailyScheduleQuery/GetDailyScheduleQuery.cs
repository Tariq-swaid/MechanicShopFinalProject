using MechanicShop.Application.Common.Fetures.Shceduling.Dtos;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domin.Common.Results;


namespace MechanicShop.Application.Common.Fetures.Shceduling.Quries.GetDailyScheduleQuery
{
    public sealed record GetDailyScheduleQuery(
    TimeZoneInfo TimeZone,
    DateOnly ScheduleDate,
    Guid? LaborId = null) : ICachedQuery<Result<ScheduleDto>>
    {
        public string CacheKey => $"work-order:{ScheduleDate:yyyy-MM-dd}:labor={LaborId?.ToString() ?? "-"}";
        public string[] Tags => ["work-order"];
        public TimeSpan? Expiration =>TimeSpan.FromMinutes(10);
    }
}
