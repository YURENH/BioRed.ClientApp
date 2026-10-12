using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using BioRed.Core.DTOs;
using BioRed.Services.Pagos.Interfaces;

namespace BioRed.Services.Pagos
{
    public class PagoService : IPagoService
    {
        private readonly HttpClient _httpClient;

        public PagoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaypalOrderPaymentResponseDto?> IniciarPagoPaypalAsync(int orderId)
        {
            try
            {
                var content = new StringContent("{}", Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"api/v1/payments/orders/{orderId}/paypal", content);

                string responseBody = await response.Content.ReadAsStringAsync();

                // 🔴 IMPRIMIR LOGS RESALTADOS EN LA CONSOLA
                Console.WriteLine("==================================================");
                Console.WriteLine($"[PAYPAL RESPUESTA STATUS]: {(int)response.StatusCode} {response.StatusCode}");
                Console.WriteLine($"[PAYPAL RESPUESTA BODY]: {responseBody}");
                Console.WriteLine("==================================================");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<PaypalOrderPaymentResponseDto>();
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("==================================================");
                Console.WriteLine($"[PAYPAL EXCEPCION]: {ex.Message}");
                Console.WriteLine("==================================================");
                return null;
            }
        }

        public async Task<PaypalOrderPaymentResponseDto?> CapturarPagoPaypalAsync(string providerOrderId)
        {
            try
            {
                // 🟢 Enviamos un JSON vacío para definir Content-Type: application/json
                var content = new StringContent("{}", Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"api/v1/payments/paypal/{providerOrderId}/capture", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<PaypalOrderPaymentResponseDto>();
                }

                string errorText = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[PAGO CAPTURAR FAIL {(int)response.StatusCode}]: {errorText}");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[PAGO CAPTURAR ERROR]: {ex.Message}");
                return null;
            }
        }
    }
}