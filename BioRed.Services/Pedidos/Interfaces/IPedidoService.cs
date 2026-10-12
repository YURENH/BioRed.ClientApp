
using BioRed.Core.Dtos;
using BioRed.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BioRed.Services.Pedidos.Interfaces
{
    public interface IPedidoService
    {
        Task<int> CrearPedidoYObtenerIdAsync(CreateOrderDto dto);
        Task<List<OrderDetailsDto>> ObtenerMisPedidosAsync();
        Task<List<OrderDetailsDto>> ObtenerMisPedidosAsync(int pageIndex, int pageSize);
        Task<OrderDetailsDto?> ObtenerSeguimientoAsync(int orderId);
        Task<bool> CancelarPedidoAsync(int orderId, string reason = "");
    }
}