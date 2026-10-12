using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BioRed.Core.Dtos
{
    public class OrderDetailsDto
    {
        [JsonPropertyName("orderId")] public int OrderId { get; set; }
        [JsonPropertyName("orderCode")] public string? OrderCode { get; set; }
        [JsonPropertyName("clientName")] public string? ClientName { get; set; }
        [JsonPropertyName("branchCode")] public string? BranchCode { get; set; }
        [JsonPropertyName("branchName")] public string? BranchName { get; set; }
        [JsonPropertyName("driverCode")] public string? DriverCode { get; set; }
        [JsonPropertyName("deliveryAddress")] public string? DeliveryAddress { get; set; }
        [JsonPropertyName("paymentTypeName")] public string? PaymentTypeName { get; set; }
        [JsonPropertyName("status")] public string Status { get; set; } = string.Empty;
        [JsonPropertyName("totalCharged")] public decimal TotalCharged { get; set; }
        [JsonPropertyName("createdAtUtc")] public DateTime CreatedAtUtc { get; set; }
        [JsonPropertyName("deliveredAtUtc")] public DateTime? DeliveredAtUtc { get; set; }
        [JsonPropertyName("history")] public List<OrderHistoryDto>? History { get; set; }
        [JsonPropertyName("lastLocation")] public OrderLocationDto? LastLocation { get; set; }
    }

    public class OrderHistoryDto
    {
        [JsonPropertyName("status")] public string Status { get; set; } = string.Empty;
        [JsonPropertyName("description")] public string? Description { get; set; }
        [JsonPropertyName("registeredAtUtc")] public DateTime RegisteredAtUtc { get; set; }
    }

    public class OrderLocationDto
    {
        [JsonPropertyName("locationId")] public int LocationId { get; set; }
        [JsonPropertyName("latitude")] public double Latitude { get; set; }
        [JsonPropertyName("longitude")] public double Longitude { get; set; }
        [JsonPropertyName("registeredAtUtc")] public DateTime RegisteredAtUtc { get; set; }
    }
}