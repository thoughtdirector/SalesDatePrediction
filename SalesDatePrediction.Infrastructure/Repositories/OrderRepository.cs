using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SalesDatePrediction.Core.Application.DTOs;
using SalesDatePrediction.Core.Domain.Interfaces;

namespace SalesDatePrediction.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<OrderDto>> GetCustomerOrdersAsync(string customerId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
                SELECT 
                    OrderID as OrderId,
                    RequiredDate,
                    ShippedDate,
                    ShipName,
                    ShipAddress,
                    ShipCity
                FROM Sales.Orders
                WHERE Custid = @CustomerId
                ORDER BY OrderDate DESC";

            return await connection.QueryAsync<OrderDto>(sql, new { CustomerId = customerId });
        }

        public async Task<int> CreateOrderAsync(CreateOrderDto orderDto)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
                BEGIN TRANSACTION;

                DECLARE @NewOrderID INT;

                INSERT INTO Sales.Orders (
                    Custid,
                    EmpID,
                    ShipperID,
                    ShipName,
                    ShipAddress,
                    ShipCity,
                    OrderDate,
                    RequiredDate,
                    ShippedDate,
                    Freight,
                    ShipCountry
                )
                VALUES (
                    @CustomerId,
                    @EmpId,
                    @ShipperId,
                    @ShipName,
                    @ShipAddress,
                    @ShipCity,
                    @OrderDate,
                    @RequiredDate,
                    @ShippedDate,
                    @Freight,
                    @ShipCountry
                );

                SET @NewOrderID = SCOPE_IDENTITY();

                INSERT INTO Sales.OrderDetails (
                    OrderID,
                    ProductID,
                    UnitPrice,
                    Qty,
                    Discount
                )
                VALUES (
                    @NewOrderID,
                    @ProductId,
                    @UnitPrice,
                    @Qty,
                    @Discount
                );

                COMMIT TRANSACTION;

                SELECT @NewOrderID AS OrderID;";

            var parameters = new
            {
                orderDto.CustomerId,
                orderDto.EmpId,
                orderDto.ShipperId,
                orderDto.ShipName,
                orderDto.ShipAddress,
                orderDto.ShipCity,
                orderDto.OrderDate,
                orderDto.RequiredDate,
                orderDto.ShippedDate,
                orderDto.Freight,
                orderDto.ShipCountry,
                orderDto.ProductId,
                orderDto.UnitPrice,
                orderDto.Qty,
                orderDto.Discount
            };

            return await connection.ExecuteScalarAsync<int>(sql, parameters);
        }
    }
}