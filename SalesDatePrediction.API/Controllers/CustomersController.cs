using Microsoft.AspNetCore.Mvc;
using SalesDatePrediction.Core.Application.DTOs;
using SalesDatePrediction.Core.Domain.Interfaces;

namespace SalesDatePrediction.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet("predictions")]
        public async Task<ActionResult<IEnumerable<CustomerPredictionDto>>> GetPredictions([FromQuery] string nameFilter = "")
        {
            var predictions = await _customerRepository.GetCustomerPredictionsAsync(nameFilter);
            return Ok(predictions);
        }
    }
}