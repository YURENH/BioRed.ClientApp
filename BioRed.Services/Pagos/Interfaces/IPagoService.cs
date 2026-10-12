using System.Threading.Tasks;
using BioRed.Core.DTOs;

namespace BioRed.Services.Pagos.Interfaces
{
    public interface IPagoService
    {
        Task<PaypalOrderPaymentResponseDto?> IniciarPagoPaypalAsync(int orderId);
        Task<PaypalOrderPaymentResponseDto?> CapturarPagoPaypalAsync(string providerOrderId);
    }
}