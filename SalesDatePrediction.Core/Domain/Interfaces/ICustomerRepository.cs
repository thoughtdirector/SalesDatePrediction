using SalesDatePrediction.Core.Application.DTOs;
using SalesDatePrediction.Core.Domain.Entities;

namespace SalesDatePrediction.Core.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomerPredictionDto>> GetCustomerPredictionsAsync(string nameFilter = null);
        Task<Customer> GetCustomerByIdAsync(string customerId);
    }
}
