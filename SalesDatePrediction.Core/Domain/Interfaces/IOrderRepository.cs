using SalesDatePrediction.Core.Application.DTOs;

namespace SalesDatePrediction.Core.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderDto>> GetCustomerOrdersAsync(string customerId);
        Task<int> CreateOrderAsync(CreateOrderDto orderDto);
    }
}
