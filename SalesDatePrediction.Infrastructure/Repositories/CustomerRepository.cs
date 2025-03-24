using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SalesDatePrediction.Core.Application.DTOs;
using SalesDatePrediction.Core.Domain.Entities;
using SalesDatePrediction.Core.Domain.Interfaces;

namespace SalesDatePrediction.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<CustomerPredictionDto>> GetCustomerPredictionsAsync(string nameFilter = null)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
                SELECT
                    c.Custid, -- Incluir Custid
                    c.CompanyName AS CustomerName,
                    MAX(o.OrderDate) AS LastOrderDate,
                    DATEADD(DAY,
                        ISNULL((
                            SELECT AVG(DATEDIFF(DAY, o1.OrderDate, o2.OrderDate))
                            FROM Sales.Orders o1
                            JOIN Sales.Orders o2
                                ON o1.Custid = o2.Custid
                                AND o1.OrderDate < o2.OrderDate
                            WHERE o1.Custid = c.Custid
                        ), 30),
                        MAX(o.OrderDate)
                    ) AS NextPredictedOrder
                FROM Sales.Customers c
                LEFT JOIN Sales.Orders o ON c.Custid = o.Custid
                WHERE (@CustomerName IS NULL OR c.CompanyName LIKE '%' + @CustomerName + '%')
                GROUP BY c.Custid, c.CompanyName
                ORDER BY c.CompanyName;";

            return await connection.QueryAsync<CustomerPredictionDto>(sql, new { CustomerName = nameFilter });
        }

        public async Task<Customer> GetCustomerByIdAsync(string customerId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
                SELECT *
                FROM Sales.Customers
                WHERE Custid = @CustomerId;";

            return await connection.QueryFirstOrDefaultAsync<Customer>(sql, new { CustomerId = customerId });
        }
    }
}
