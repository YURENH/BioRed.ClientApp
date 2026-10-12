using System.Text.Json.Serialization;
using System.Collections.Generic; // Necesario para la lista de roles

namespace BioRed.Core.DTOs
{
    public class LoginRequestDto
    {
        [JsonPropertyName("userNameOrEmail")]
        public string UserNameOrEmail { get; set; } = string.Empty;

        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonPropertyName("userName")]
        public string UserName { get; set; } = string.Empty;

       
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; } = new();

        [JsonPropertyName("userType")]
        public string UserType { get; set; } = string.Empty;
    }

    // DTO para solicitar el correo de recuperación
    public class ForgotPasswordRequestDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
    }

    public class ChangeTemporaryPasswordRequestDto
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("temporaryPassword")]
        public string TemporaryPassword { get; set; } = string.Empty;

        [JsonPropertyName("newPassword")]
        public string NewPassword { get; set; } = string.Empty;
    }



}