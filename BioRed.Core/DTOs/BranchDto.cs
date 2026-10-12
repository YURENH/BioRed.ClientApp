using System.Text.Json.Serialization;

namespace BioRed.Core.DTOs
{
    public class BranchDto
    {
        [JsonPropertyName("branchCode")]
        public string BranchCode { get; set; } = string.Empty;

        [JsonPropertyName("branchName")]
        public string BranchName { get; set; } = string.Empty;

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }
    }

    // Objeto contenedor que recibe el JSON de la API
    public class BranchListResponseDto
    {
        [JsonPropertyName("items")]
        public List<BranchDto>? Items { get; set; }

        [JsonPropertyName("data")]
        public List<BranchDto>? Data { get; set; }
    }
}