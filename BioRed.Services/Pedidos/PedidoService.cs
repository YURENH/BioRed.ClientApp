using BioRed.Core.Dtos;
using BioRed.Core.DTOs;
using BioRed.Services.Pedidos.Interfaces;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BioRed.Services.Pedidos
{
    public class PedidoService : IPedidoService
    {
        private readonly HttpClient _httpClient;

        public PedidoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private async Task AdjuntarTokenSeguridadAsync()
        {
            // ⚠️ Revisa si en tu LoginService usaste "auth_token", "token" o "jwt_token"
            string? token = await SecureStorage.Default.GetAsync("jwt_token");

            System.Diagnostics.Debug.WriteLine($"[PAGO TOKEN LEÍDO]: {(string.IsNullOrEmpty(token) ? "NULL O VACÍO ❌" : "OK LEÍDO CON ÉXITO 🟢")}");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<int> CrearPedidoYObtenerIdAsync(CreateOrderDto dto)
        {
            try
            {
                // 🟢 1. Obtener token del almacenamiento seguro
                string? token = await SecureStorage.Default.GetAsync("jwt_token"); // O la clave que usas al hacer Login

                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }

                // 2. Enviar petición
                var response = await _httpClient.PostAsJsonAsync("api/v1/orders", dto);

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<OrderDetailsDto>();
                    return resultado?.OrderId ?? 0;
                }

                // 🟢 3. Leer el error exacto de Azure para saber qué falló
                string errorApi = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[ERROR API HTTP {(int)response.StatusCode}]: {errorApi}");

                return 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[EXCEPCION PEDIDO]: {ex.Message}");
                return 0;
            }
        }

        public async Task<List<OrderDetailsDto>> ObtenerMisPedidosAsync()
        {
            return await ObtenerMisPedidosAsync(1, 50);
        }

        public async Task<List<OrderDetailsDto>> ObtenerMisPedidosAsync(int pageIndex, int pageSize)
        {
            try
            {
                await AdjuntarTokenSeguridadAsync();

                // Petición al endpoint del historial
                var response = await _httpClient.GetAsync($"api/v1/orders?page={pageIndex}&pageSize={pageSize}");

                // Si el servidor rechaza query string, se intenta la ruta limpia
                if (!response.IsSuccessStatusCode && response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                {
                    response = await _httpClient.GetAsync("api/v1/orders");
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[HISTORIAL HTTP {response.StatusCode}]: {jsonResponse}");

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        NumberHandling = JsonNumberHandling.AllowReadingFromString
                    };

                    using var doc = JsonDocument.Parse(jsonResponse);
                    JsonElement root = doc.RootElement;
                    JsonElement arrayElement = root;

                    // Desempaqueta la respuesta si viene en un objeto contenedor (Paginado)
                    if (root.ValueKind == JsonValueKind.Object)
                    {
                        if (root.TryGetProperty("items", out var items)) arrayElement = items;
                        else if (root.TryGetProperty("Items", out items)) arrayElement = items;
                        else if (root.TryGetProperty("data", out var data)) arrayElement = data;
                        else if (root.TryGetProperty("Data", out data)) arrayElement = data;
                        else if (root.TryGetProperty("orders", out var orders)) arrayElement = orders;
                        else if (root.TryGetProperty("Orders", out orders)) arrayElement = orders;
                        else if (root.TryGetProperty("result", out var result)) arrayElement = result;
                        else if (root.TryGetProperty("Result", out result)) arrayElement = result;
                        else if (root.TryGetProperty("value", out var val)) arrayElement = val;
                    }

                    if (arrayElement.ValueKind == JsonValueKind.Array)
                    {
                        var lista = JsonSerializer.Deserialize<List<OrderDetailsDto>>(arrayElement.GetRawText(), options);
                        return lista ?? new List<OrderDetailsDto>();
                    }
                }

                return new List<OrderDetailsDto>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[HISTORIAL PEDIDOS ERROR]: {ex.Message}");
                return new List<OrderDetailsDto>();
            }
        }

        public async Task<OrderDetailsDto?> ObtenerSeguimientoAsync(int orderId)
        {
            try
            {
                await AdjuntarTokenSeguridadAsync();
                var response = await _httpClient.GetAsync($"api/v1/orders/{orderId}");

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        NumberHandling = JsonNumberHandling.AllowReadingFromString
                    };

                    using var doc = JsonDocument.Parse(jsonResponse);
                    JsonElement root = doc.RootElement;

                    if (root.ValueKind == JsonValueKind.Object)
                    {
                        if (root.TryGetProperty("data", out var data)) root = data;
                        else if (root.TryGetProperty("result", out var result)) root = result;
                    }

                    return JsonSerializer.Deserialize<OrderDetailsDto>(root.GetRawText(), options);
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SEGUIMIENTO ERROR]: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CancelarPedidoAsync(int orderId, string motivo = "Cancelado por el usuario")
        {
            try
            {
                // Preparamos el objeto con los datos que espera la API
                var requestBody = new
                {
                    status = "Cancelado", // Ajusta si tu backend espera el ID de la tabla estados (ej. statusId = 4)
                    reason = motivo
                };

                // PatchAsJsonAsync serializa a JSON y asigna los encabezados correctos automáticamente
                var response = await _httpClient.PatchAsJsonAsync($"api/v1/orders/{orderId}/status", requestBody);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                // Imprimimos el error exacto que retorna el backend en la consola de depuración
                var errorResponseBody = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[ERROR API {response.StatusCode}]: {errorResponseBody}");

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[EXCEPCION CANCELAR PEDIDO]: {ex.Message}");
                return false;
            }
        }
    }
}