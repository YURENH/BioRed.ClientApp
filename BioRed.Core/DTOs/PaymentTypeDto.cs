namespace BioRed.Core.DTOs
{
    public class PaymentTypeDto
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Texto que se mostrará en el Picker
        public string DisplayName => string.IsNullOrWhiteSpace(Description)
            ? Name
            : $"{Name} ({Description})";
    }
}