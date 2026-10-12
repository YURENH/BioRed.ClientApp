using System.Text.Json.Serialization;

namespace BioRed.Core.DTOs
{
    // Representa un solo producto dentro de la lista "items"
    public class ProductoDto
    {
        [JsonPropertyName("productCode")]
        public string ProductCode { get; set; } = string.Empty;

        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("salePrice")]
        public decimal SalePrice { get; set; }

        [JsonPropertyName("availableQuantity")]
        public int AvailableQuantity { get; set; }

        [JsonPropertyName("inStock")]
        public bool InStock { get; set; }

        [JsonPropertyName("requiresPrescription")]
        public bool RequiresPrescription { get; set; }
    }

    // Representa la respuesta completa de la API con la paginación
    public class PagedProductResponseDto
    {
        [JsonPropertyName("branchCode")]
        public string BranchCode { get; set; } = string.Empty;

        [JsonPropertyName("branchName")]
        public string BranchName { get; set; } = string.Empty;

        [JsonPropertyName("items")]
        public List<ProductoDto> Items { get; set; } = new();
    }
}