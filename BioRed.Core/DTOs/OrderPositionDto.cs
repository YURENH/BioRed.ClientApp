using System;
using System.Text.Json.Serialization;

namespace BioRed.Core.Dtos
{
    public class OrderPositionDto
    {
        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }

        [JsonPropertyName("orderStatus")]
        public string OrderStatus { get; set; }

        [JsonPropertyName("lastDriverLocation")]
        public DriverLocationDto LastDriverLocation { get; set; }
    }

    public class DriverLocationDto
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("receivedAtUtc")]
        public DateTime? ReceivedAtUtc { get; set; }
    }
}