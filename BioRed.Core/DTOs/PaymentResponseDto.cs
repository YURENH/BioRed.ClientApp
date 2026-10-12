using System;
using System.Text.Json.Serialization;

namespace BioRed.Core.Dtos
{
    public class PaymentResponseDto
    {
        [JsonPropertyName("paymentId")] public int PaymentId { get; set; }
        [JsonPropertyName("orderId")] public int OrderId { get; set; }
        [JsonPropertyName("orderCode")] public string? OrderCode { get; set; }
        [JsonPropertyName("clientCode")] public string? ClientCode { get; set; }
        [JsonPropertyName("paymentTypeCode")] public string? PaymentTypeCode { get; set; }
        [JsonPropertyName("provider")] public string? Provider { get; set; }
        [JsonPropertyName("status")] public string Status { get; set; } = string.Empty;
        [JsonPropertyName("localAmount")] public decimal LocalAmount { get; set; }
        [JsonPropertyName("localCurrency")] public string? LocalCurrency { get; set; }
        [JsonPropertyName("providerAmount")] public decimal ProviderAmount { get; set; }
        [JsonPropertyName("providerCurrency")] public string? ProviderCurrency { get; set; }
        [JsonPropertyName("exchangeRate")] public decimal ExchangeRate { get; set; }
        [JsonPropertyName("providerOrderId")] public string? ProviderOrderId { get; set; }
        [JsonPropertyName("providerCaptureId")] public string? ProviderCaptureId { get; set; }
        [JsonPropertyName("approvalUrl")] public string? ApprovalUrl { get; set; }
        [JsonPropertyName("idempotencyKey")] public string? IdempotencyKey { get; set; }
        [JsonPropertyName("createdByAccountId")] public int CreatedByAccountId { get; set; }
        [JsonPropertyName("createdAtUtc")] public DateTime CreatedAtUtc { get; set; }
        [JsonPropertyName("completedAtUtc")] public DateTime? CompletedAtUtc { get; set; }
        [JsonPropertyName("failureDetail")] public string? FailureDetail { get; set; }
    }
}