using SalesDatePrediction.Core.Application.DTOs;

namespace SalesDatePrediction.Core.Domain.Interfaces
{
    public interface IShipperRepository
    {
        Task<IEnumerable<ShipperDto>> GetAllShippersAsync();
    }
}
