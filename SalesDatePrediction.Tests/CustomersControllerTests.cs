using Microsoft.AspNetCore.Mvc;
using Moq;
using SalesDatePrediction.API.Controllers;
using SalesDatePrediction.Core.Application.DTOs;
using SalesDatePrediction.Core.Domain.Interfaces;

namespace SalesDatePrediction.Tests;

public class CustomersControllerTests
{
    [Fact]
    public async Task GetPredictions_WithNameFilter_ReturnsOkWithPredictions()
    {
        var mockRepo = new Mock<ICustomerRepository>();
        var filter = "John";
        var expectedPredictions = new List<CustomerPredictionDto>
        {
            new()
            {
                CustId = "C001", CustomerName = "John Corp", LastOrderDate = DateTime.Today.AddDays(-10),
                NextPredictedOrder = DateTime.Today.AddDays(20)
            }
        };
        mockRepo.Setup(repo => repo.GetCustomerPredictionsAsync(It.IsAny<string>()))
            .ReturnsAsync(expectedPredictions);
        var controller = new CustomersController(mockRepo.Object);


        var result = await controller.GetPredictions(filter);


        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedList = Assert.IsType<List<CustomerPredictionDto>>(okResult.Value);
        Assert.Same(expectedPredictions, returnedList);
        mockRepo.Verify(repo => repo.GetCustomerPredictionsAsync(filter), Times.Once());
    }

    [Fact]
    public async Task GetPredictions_WhenRepositoryThrows_ExceptionPropagates()
    {
        var mockRepo = new Mock<ICustomerRepository>();
        mockRepo.Setup(repo => repo.GetCustomerPredictionsAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Database failure"));
        var controller = new CustomersController(mockRepo.Object);


        var ex = await Assert.ThrowsAsync<Exception>(() => controller.GetPredictions("Test"));
        Assert.Equal("Database failure", ex.Message);
        mockRepo.Verify(repo => repo.GetCustomerPredictionsAsync("Test"), Times.Once());
    }
}

public class EmployeesControllerTests
{
    [Fact]
    public async Task GetAllEmployees_ReturnsOkWithEmployees()
    {
        var mockRepo = new Mock<IEmployeeRepository>();
        var expectedEmployees = new List<EmployeeDto>
        {
            new() { EmpId = 1, FullName = "Alice Smith" },
            new() { EmpId = 2, FullName = "Bob Johnson" }
        };
        mockRepo.Setup(repo => repo.GetAllEmployeesAsync())
            .ReturnsAsync(expectedEmployees);
        var controller = new EmployeesController(mockRepo.Object);


        var result = await controller.GetAll();


        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedList = Assert.IsType<List<EmployeeDto>>(okResult.Value);
        Assert.Same(expectedEmployees, returnedList);
        mockRepo.Verify(repo => repo.GetAllEmployeesAsync(), Times.Once());
    }

    [Fact]
    public async Task GetAllEmployees_WhenRepositoryThrows_ExceptionPropagates()
    {
        var mockRepo = new Mock<IEmployeeRepository>();
        mockRepo.Setup(repo => repo.GetAllEmployeesAsync())
            .ThrowsAsync(new Exception("DB error"));
        var controller = new EmployeesController(mockRepo.Object);


        var ex = await Assert.ThrowsAsync<Exception>(() => controller.GetAll());
        Assert.Equal("DB error", ex.Message);
        mockRepo.Verify(repo => repo.GetAllEmployeesAsync(), Times.Once());
    }
}

public class OrdersControllerTests
{
    [Fact]
    public async Task GetCustomerOrders_ReturnsOkWithOrders()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var customerId = "C001";
        var expectedOrders = new List<OrderDto>
        {
            new()
            {
                OrderId = 101, RequiredDate = DateTime.Today, ShippedDate = DateTime.Today.AddDays(1),
                ShipName = "Ship1", ShipAddress = "Addr1", ShipCity = "City1"
            },
            new()
            {
                OrderId = 102, RequiredDate = DateTime.Today.AddDays(2), ShippedDate = null, ShipName = "Ship2",
                ShipAddress = "Addr2", ShipCity = "City2"
            }
        };
        mockRepo.Setup(repo => repo.GetCustomerOrdersAsync(It.IsAny<string>()))
            .ReturnsAsync(expectedOrders);
        var controller = new OrdersController(mockRepo.Object);


        var result = await controller.GetCustomerOrders(customerId);


        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedList = Assert.IsType<List<OrderDto>>(okResult.Value);
        Assert.Same(expectedOrders, returnedList);
        mockRepo.Verify(repo => repo.GetCustomerOrdersAsync(customerId), Times.Once());
    }

    [Fact]
    public async Task GetCustomerOrders_WhenRepositoryThrows_ExceptionPropagates()
    {
        var mockRepo = new Mock<IOrderRepository>();
        mockRepo.Setup(repo => repo.GetCustomerOrdersAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Query failed"));
        var controller = new OrdersController(mockRepo.Object);


        var ex = await Assert.ThrowsAsync<Exception>(() => controller.GetCustomerOrders("C001"));
        Assert.Equal("Query failed", ex.Message);
        mockRepo.Verify(repo => repo.GetCustomerOrdersAsync("C001"), Times.Once());
    }

    [Fact]
    public async Task CreateOrder_WithValidModel_ReturnsCreatedAtAction()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var newOrderId = 555;
        var orderDto = new CreateOrderDto
        {
            CustomerId = "C001",
            EmpId = 10,
            ShipperId = 20,
            ShipName = "Test Ship",
            ShipAddress = "123 Lane",
            ShipCity = "TestCity",
            OrderDate = DateTime.Today,
            RequiredDate = DateTime.Today.AddDays(5),
            ShippedDate = null,
            Freight = 15.5m,
            ShipCountry = "USA",
            ProductId = 100,
            UnitPrice = 9.99m,
            Qty = 3,
            Discount = 0.1f
        };
        mockRepo.Setup(repo => repo.CreateOrderAsync(It.IsAny<CreateOrderDto>()))
            .ReturnsAsync(newOrderId);
        var controller = new OrdersController(mockRepo.Object);


        var result = await controller.CreateOrder(orderDto);


        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("GetCustomerOrders", createdResult.ActionName);
        Assert.NotNull(createdResult.RouteValues);
        Assert.Equal(orderDto.CustomerId, createdResult.RouteValues["customerId"]);

        var responseBody = createdResult.Value;
        Assert.NotNull(responseBody);
        var propInfo = responseBody.GetType().GetProperty("OrderId");
        Assert.NotNull(propInfo);
        Assert.Equal(newOrderId, (int)propInfo.GetValue(responseBody));

        mockRepo.Verify(repo => repo.CreateOrderAsync(orderDto), Times.Once());
    }

    [Fact]
    public async Task CreateOrder_WithInvalidModel_ReturnsBadRequest()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var orderDto = new CreateOrderDto();
        var controller = new OrdersController(mockRepo.Object);

        controller.ModelState.AddModelError("CustomerId", "Required");


        var result = await controller.CreateOrder(orderDto);


        var badResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badResult.Value);
        Assert.True(badResult.Value is SerializableError || badResult.Value is ValidationProblemDetails);

        mockRepo.Verify(repo => repo.CreateOrderAsync(It.IsAny<CreateOrderDto>()), Times.Never());
    }

    [Fact]
    public async Task CreateOrder_WhenRepositoryThrows_ExceptionPropagates()
    {
        var mockRepo = new Mock<IOrderRepository>();
        mockRepo.Setup(repo => repo.CreateOrderAsync(It.IsAny<CreateOrderDto>()))
            .ThrowsAsync(new Exception("Insert failed"));
        var controller = new OrdersController(mockRepo.Object);
        var orderDto = new CreateOrderDto { CustomerId = "C001" };


        var ex = await Assert.ThrowsAsync<Exception>(() => controller.CreateOrder(orderDto));
        Assert.Equal("Insert failed", ex.Message);
        mockRepo.Verify(repo => repo.CreateOrderAsync(orderDto), Times.Once());
    }
}

public class ProductsControllerTests
{
    [Fact]
    public async Task GetAllProducts_ReturnsOkWithProducts()
    {
        var mockRepo = new Mock<IProductRepository>();
        var expectedProducts = new List<ProductDto>
        {
            new() { ProductId = 1, ProductName = "ProductA" },
            new() { ProductId = 2, ProductName = "ProductB" }
        };
        mockRepo.Setup(repo => repo.GetAllProductsAsync())
            .ReturnsAsync(expectedProducts);
        var controller = new ProductsController(mockRepo.Object);


        var result = await controller.GetAll();


        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedList = Assert.IsType<List<ProductDto>>(okResult.Value);
        Assert.Same(expectedProducts, returnedList);
        mockRepo.Verify(repo => repo.GetAllProductsAsync(), Times.Once());
    }

    [Fact]
    public async Task GetAllProducts_WhenRepositoryThrows_ExceptionPropagates()
    {
        var mockRepo = new Mock<IProductRepository>();
        mockRepo.Setup(repo => repo.GetAllProductsAsync())
            .ThrowsAsync(new Exception("DB fail"));
        var controller = new ProductsController(mockRepo.Object);


        var ex = await Assert.ThrowsAsync<Exception>(() => controller.GetAll());
        Assert.Equal("DB fail", ex.Message);
        mockRepo.Verify(repo => repo.GetAllProductsAsync(), Times.Once());
    }
}

public class ShippersControllerTests
{
    [Fact]
    public async Task GetAllShippers_ReturnsOkWithShippers()
    {
        var mockRepo = new Mock<IShipperRepository>();
        var expectedShippers = new List<ShipperDto>
        {
            new() { ShipperId = 1, CompanyName = "Speedy Express" },
            new() { ShipperId = 2, CompanyName = "United Package" }
        };
        mockRepo.Setup(repo => repo.GetAllShippersAsync())
            .ReturnsAsync(expectedShippers);
        var controller = new ShippersController(mockRepo.Object);


        var result = await controller.GetAll();


        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedList = Assert.IsType<List<ShipperDto>>(okResult.Value);
        Assert.Same(expectedShippers, returnedList);
        mockRepo.Verify(repo => repo.GetAllShippersAsync(), Times.Once());
    }

    [Fact]
    public async Task GetAllShippers_WhenRepositoryThrows_ExceptionPropagates()
    {
        var mockRepo = new Mock<IShipperRepository>();
        mockRepo.Setup(repo => repo.GetAllShippersAsync())
            .ThrowsAsync(new Exception("Connection lost"));
        var controller = new ShippersController(mockRepo.Object);


        var ex = await Assert.ThrowsAsync<Exception>(() => controller.GetAll());
        Assert.Equal("Connection lost", ex.Message);
        mockRepo.Verify(repo => repo.GetAllShippersAsync(), Times.Once());
    }
}