using BioRed.Core.DTOs;
using BioRed.Services.Api;

namespace BioRed.Services.Productos
{
    public class ProductoService
    {
        private readonly ApiClient _apiClient;

        public ProductoService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<BranchDto>> ObtenerSucursalesAsync()
        {
            // Catálogo completo de sucursales reales mapeadas directamente de la base de datos SQL
            var listaSucursales = new List<BranchDto>
            {
                new BranchDto { BranchCode = "SUC-BIO-02", BranchName = "BioRed Zona 10", Active = true },
                new BranchDto { BranchCode = "CENTRAL", BranchName = "Farmacia Central", Active = true },
                new BranchDto { BranchCode = "SUC-ADE-03", BranchName = "Adeph Hospital", Active = true },
                new BranchDto { BranchCode = "SUC-ADE-02", BranchName = "Adeph Las Palmas", Active = true },
                new BranchDto { BranchCode = "SUC-ADE-01", BranchName = "Adeph Parque Central", Active = true },
                new BranchDto { BranchCode = "SUC-AHO-02", BranchName = "Ahorro Barrio San Francisco", Active = true },
                new BranchDto { BranchCode = "SUC-AHO-03", BranchName = "Ahorro Salida Sur", Active = true },
                new BranchDto { BranchCode = "SUC-AHO-01", BranchName = "Ahorro Terminal", Active = true },
                new BranchDto { BranchCode = "SUC-ADZ-03", BranchName = "Sucursal Prueba Azure Actualizada", Active = true }
            };

            return await Task.FromResult(listaSucursales.Where(b => b != null && b.Active).ToList());
        }

        public async Task<List<ProductoDto>> ObtenerProductosAsync(string branchCode)
        {
            if (string.IsNullOrWhiteSpace(branchCode))
            {
                return new List<ProductoDto>();
            }

            // Endpoint del catálogo habilitado para el rol Cliente:
            // GET /api/v1/catalog/branches/{branchCode}/products
            var response = await _apiClient.GetAsync<PagedProductResponseDto>($"api/v1/catalog/branches/{branchCode}/products?page=1&pageSize=50");

            var lista = response?.Items ?? new List<ProductoDto>();

            return lista.Where(p => p != null).ToList();
        }
    }
}