using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Interfaces
{
    public interface IWorkOrderNotifier
    {
        Task NotifyWorkOrdersChangedAsync(CancellationToken ct = default);


    }
}
