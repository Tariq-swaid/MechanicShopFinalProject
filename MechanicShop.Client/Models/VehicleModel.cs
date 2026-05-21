using System;
using System.Text.Json.Serialization; // Make sure this is at the top!

namespace MechanicShop.Client.Models // Adjust namespace if needed
{
    public class VehicleModel
    {
        // This tells Blazor: "When you see 'vehicalId' in the JSON, put it here!"
        [JsonPropertyName("vehicalId")]
        public Guid VehicleId { get; set; }

        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
    }
}