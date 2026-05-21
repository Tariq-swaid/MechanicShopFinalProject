using MechanicShop.Domin.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Domin.WorkOrders.Events
{
    public class WorkOrderCompleted : DomainEventEntity
    {
        public Guid WorkOrderId { get; set; }
    }
}
