using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mechanic.Infrastructure.RealTime
{
    public sealed class WorkOrderHub : Hub
    {
        public const string HubUrl = "/hubs/workorders";
    }
}
