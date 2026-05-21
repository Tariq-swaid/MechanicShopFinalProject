using MechanicShop.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Contracts.Requests.WorkOrders
{
    public class UpdateWorkOrderStateRequest
    {
        public WorkOrderState State { get; set; }
    }
}
