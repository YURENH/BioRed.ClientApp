using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace BioRed.Services
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // 🟢 Lee el token guardado en el dispositivo
            // Asegúrate de usar el mismo nombre de clave que usas al hacer Login ("auth_token" o similar)
            string? token = await SecureStorage.Default.GetAsync("jwt_token");

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}