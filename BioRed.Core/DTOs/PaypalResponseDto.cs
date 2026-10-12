using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
namespace BioRed.Core.DTOs
{
    public class PaypalOrderPaymentResponseDto
    {
        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }

        [JsonPropertyName("orderCode")]
        public string OrderCode { get; set; } = string.Empty;

        [JsonPropertyName("orderTotal")]
        public decimal OrderTotal { get; set; }

        [JsonPropertyName("paymentTypeCode")]
        public string PaymentTypeCode { get; set; } = string.Empty;

        [JsonPropertyName("isPaid")]
        public bool IsPaid { get; set; }

        [JsonPropertyName("payments")]
        public List<PaymentItemDto> Payments { get; set; } = new();

        // 🟢 Agregamos ambas propiedades helper para evitar errores de sintaxis en el ViewModel:
        public string ApprovalUrl => Payments?.FirstOrDefault(p => !string.IsNullOrEmpty(p.ApprovalUrl))?.ApprovalUrl ?? string.Empty;
        public string ApproveUrl => ApprovalUrl; // Soporte para ambos nombres
        public string ProviderOrderId => Payments?.FirstOrDefault(p => !string.IsNullOrEmpty(p.ProviderOrderId))?.ProviderOrderId ?? string.Empty;
    }

// DTO de cada elemento dentro de la lista "payments"
public class PaymentItemDto
    {
        [JsonPropertyName("paymentId")]
        public int PaymentId { get; set; }

        [JsonPropertyName("provider")]
        public string Provider { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("providerOrderId")]
        public string? ProviderOrderId { get; set; }

        [JsonPropertyName("approvalUrl")]
        public string? ApprovalUrl { get; set; }
    }

    // DTO unificado para mantener compatibilidad con el ViewModel
    public class PaypalResponseDto
    {
        public string ApproveUrl { get; set; } = string.Empty;
        public string ProviderOrderId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}