using MechanicShop.Application.Common.Fetures.Customer.Dtos;
using MechanicShop.Domin.WorkOrders.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MechanicShop.Application.Common.Fetures.WorkOrders.Dtos
{
    public class WorkOederListItemDto
    {
        public Guid WorkOrderId { get; set; }
        public Guid? InvoiceId { get; set; }
        public VehicleDto Vehicle { get; set; } = default!;
        public string? Customer { get; set; }
        public string? Labor { get; set; }
        public WorkOrderState State { get; set; }
        public Spot Spot { get; set; }
        public DateTimeOffset StartAtUtc { get; set; }
        public DateTimeOffset EndAtUtc { get; set; }
        public List<string> RepairTasks { get; set; } = [];
    }
}
