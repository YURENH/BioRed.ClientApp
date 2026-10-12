using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BioRed.Core.DTOs
{
    public class CreateOrderDto
    {
        [JsonPropertyName("branchCode")]
        public string? BranchCode { get; set; }

        [JsonPropertyName("deliveryAddressId")]
        public int DeliveryAddressId { get; set; }

        [JsonPropertyName("paymentTypeId")]
        public int? PaymentTypeId { get; set; }

        [JsonPropertyName("paymentTypeCode")]
        public string? PaymentTypeCode { get; set; }

        [JsonPropertyName("subtotal")]
        public decimal Subtotal { get; set; }

        [JsonPropertyName("deliveryCost")]
        public decimal DeliveryCost { get; set; }

        [JsonPropertyName("totalCharged")]
        public decimal TotalCharged { get; set; }

        [JsonPropertyName("discountApplied")]
        public decimal DiscountApplied { get; set; } = 0;

        [JsonPropertyName("promotionCode")]
        public string? PromotionCode { get; set; }

        [JsonPropertyName("prescriptionId")]
        public int? PrescriptionId { get; set; }

        [JsonPropertyName("items")]
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }

    public class CreateOrderItemDto
    {
        [JsonPropertyName("productCode")]
        public string ProductCode { get; set; } = string.Empty;

        [JsonPropertyName("productName")]
        public string? ProductName { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonPropertyName("subtotal")]
        public decimal Subtotal { get; set; }
    }
}