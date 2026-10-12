using System.Text.Json.Serialization;

namespace BioRed.Core.DTOs
{
    public class UpdateOrderStatusRequestDto
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}