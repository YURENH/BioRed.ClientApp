using System.Text.Json.Serialization;

namespace BioRed.Core.DTOs
{
    public class DeliveryAddressDto
    {
        [JsonPropertyName("addressId")]
        public int AddressId { get; set; }

        // Propiedad de compatibilidad (devuelve AddressId)
        public int IdDireccion => AddressId;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("fullAddress")]
        public string FullAddress { get; set; } = string.Empty;

        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;

        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; }

        [JsonPropertyName("isDefault")]
        public bool IsDefault { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        // Texto que se mostrará en el Picker de MAUI
        public string DisplayText => string.IsNullOrWhiteSpace(Name)
            ? FullAddress
            : $"{Name} - {FullAddress}";
    }
}