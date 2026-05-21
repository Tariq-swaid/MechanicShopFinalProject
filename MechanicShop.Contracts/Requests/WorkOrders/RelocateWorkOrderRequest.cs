using MechanicShop.Contracts.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MechanicShop.Contracts.Requests.WorkOrders
{
    public class RelocateWorkOrderRequest
    {
        public DateTimeOffset NewStartAtUtc { get; set; }
        
        
        [JsonPropertyName("newSpot")]
        public Spot NewSpot { get; set; }
    }
}
