using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SalesDatePrediction.Core.Application.DTOs;
using SalesDatePrediction.Core.Domain.Interfaces;

namespace SalesDatePrediction.Infrastructure.Repositories
{
    public class ShipperRepository : IShipperRepository
    {
        private readonly string _connectionString;

        public ShipperRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<ShipperDto>> GetAllShippersAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
                SELECT 
                    ShipperID as ShipperId,
                    CompanyName
                FROM Sales.Shippers
                ORDER BY CompanyName";

            return await connection.QueryAsync<ShipperDto>(sql);
        }
    }
}