using System;
using System.Threading.Tasks;
using BioRed.Core.DTOs;
using BioRed.Services.Api;
using Microsoft.Maui.Storage;

namespace BioRed.Services.Auth
{
    public class AuthService
    {
        private readonly ApiClient _apiClient;

        public AuthService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                var request = new LoginRequestDto
                {
                    UserNameOrEmail = email.Trim(),
                    Password = password.Trim()
                };

                var response = await _apiClient.PostAsync<AuthResponseDto>("api/v1/auth/login", request);

                if (response != null && !string.IsNullOrEmpty(response.AccessToken))
                {
                    await SecureStorage.Default.SetAsync("jwt_token", response.AccessToken);

                    if (response.Roles != null && response.Roles.Count > 0)
                    {
                        await SecureStorage.Default.SetAsync("user_role", response.Roles[0]);
                    }

                    _apiClient.SetBearerToken(response.AccessToken);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en el servidor: {ex.Message}", ex);
            }
        }

        public async Task<bool> SolicitarRecuperacionAsync(string email)
        {
            try
            {
                var request = new ForgotPasswordRequestDto
                {
                    Email = email.Trim()
                };

                var response = await _apiClient.PostAsync<object>("api/v1/auth/forgot-password", request);
                return response != null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[FORGOT PASSWORD ERROR]: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CambiarContrasenaTemporalAsync(string email, string claveTemporal, string nuevaContrasena)
        {
            try
            {
                var request = new ChangeTemporaryPasswordRequestDto
                {
                    Email = email.Trim(),
                    TemporaryPassword = claveTemporal.Trim(),
                    NewPassword = nuevaContrasena.Trim()
                };

                var response = await _apiClient.PostAsync<object>("api/v1/auth/change-temporary-password", request);
                return response != null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CHANGE TEMP PASS ERROR]: {ex.Message}");
                return false;
            }
        }

        public async Task<string?> GetTokenAsync()
        {
            return await SecureStorage.Default.GetAsync("jwt_token");
        }

        public void Logout()
        {
            SecureStorage.Default.Remove("jwt_token");
            SecureStorage.Default.Remove("user_role");
            _apiClient.SetBearerToken(string.Empty);
        }
    }
}