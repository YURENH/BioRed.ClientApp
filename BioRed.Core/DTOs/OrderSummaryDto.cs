using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BioRed.Core.Dtos
{
    public class PagedResultDto<T>
    {
        [JsonPropertyName("page")] public int Page { get; set; }
        [JsonPropertyName("pageSize")] public int PageSize { get; set; }
        [JsonPropertyName("totalItems")] public int TotalItems { get; set; }
        [JsonPropertyName("totalPages")] public int TotalPages { get; set; }
        [JsonPropertyName("items")] public List<T> Items { get; set; } = new();
    }

    public class OrderSummaryDto
    {
        [JsonPropertyName("orderId")] public int OrderId { get; set; }
        [JsonPropertyName("orderCode")] public string OrderCode { get; set; } = string.Empty;
        [JsonPropertyName("clientCode")] public string ClientCode { get; set; } = string.Empty;
        [JsonPropertyName("clientName")] public string ClientName { get; set; } = string.Empty;
        [JsonPropertyName("branchCode")] public string BranchCode { get; set; } = string.Empty;
        [JsonPropertyName("branchName")] public string BranchName { get; set; } = string.Empty;
        [JsonPropertyName("driverCode")] public string? DriverCode { get; set; }
        [JsonPropertyName("status")] public string Status { get; set; } = string.Empty;
        [JsonPropertyName("subtotal")] public decimal Subtotal { get; set; }
        [JsonPropertyName("deliveryCost")] public decimal DeliveryCost { get; set; }
        [JsonPropertyName("totalCharged")] public decimal TotalCharged { get; set; }
        [JsonPropertyName("createdAtUtc")] public DateTime CreatedAtUtc { get; set; }
        [JsonPropertyName("deliveredAtUtc")] public DateTime? DeliveredAtUtc { get; set; }
    }
}