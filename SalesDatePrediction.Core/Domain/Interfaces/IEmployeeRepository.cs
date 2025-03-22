using SalesDatePrediction.Core.Application.DTOs;

namespace SalesDatePrediction.Core.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    }
}
