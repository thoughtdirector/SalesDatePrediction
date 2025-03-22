using Microsoft.AspNetCore.Mvc;
using SalesDatePrediction.Core.Application.DTOs;
using SalesDatePrediction.Core.Domain.Interfaces;

namespace SalesDatePrediction.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetCustomerOrders(string customerId)
        {
            var orders = await _orderRepository.GetCustomerOrdersAsync(customerId);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderId = await _orderRepository.CreateOrderAsync(orderDto);
            return CreatedAtAction(nameof(GetCustomerOrders), new { customerId = orderDto.CustomerId }, new { OrderId = orderId });
        }
    }
}