using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Contracts.Requests.WorkOrders
{
    public class ModifyRepairTaskRequest
    {
        public Guid[] RepairTaskIds { get; set; } = [];

    }
}
