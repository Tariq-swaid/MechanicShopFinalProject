using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Contracts.Responses
{
    public sealed record OperatingHoursResponse(TimeOnly OpeningTime, TimeOnly ClosingTime);
}
