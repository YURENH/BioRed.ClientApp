using BioRed.Core.Dtos;
using BioRed.Core.DTOs;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BioRed.Services.RealTime
{
    public class GeolocationService : IGeolocationService
    {
        private readonly HttpClient _httpClient;

        public GeolocationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OrderPositionDto> GetOrderPositionAsync(int orderId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<OrderPositionDto>($"api/v1/geolocation/orders/{orderId}/position");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR POLLED GEOLOCATION]: {ex.Message}");
                return null;
            }
        }
    }
}