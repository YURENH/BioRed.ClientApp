using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BioRed.Core.DTOs;
using BioRed.Services.Api;

namespace BioRed.Services.Direcciones
{
    public class DireccionService
    {
        private readonly ApiClient _apiClient;

        public DireccionService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<DeliveryAddressDto>> ObtenerDireccionesClienteAsync()
        {
            try
            {
                var response = await _apiClient.GetAsync<List<DeliveryAddressDto>>("api/v1/clients/me/addresses");
                return response ?? new List<DeliveryAddressDto>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al consultar direcciones: {ex.Message}");
                return new List<DeliveryAddressDto>();
            }
        }
    }
}