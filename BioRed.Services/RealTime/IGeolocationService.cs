using System.Threading.Tasks;
using BioRed.Core.Dtos;

namespace BioRed.Services.RealTime
{
    public interface IGeolocationService
    {
        Task<OrderPositionDto> GetOrderPositionAsync(int orderId);
    }
}