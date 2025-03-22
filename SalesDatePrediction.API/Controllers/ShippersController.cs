using Microsoft.AspNetCore.Mvc;
using SalesDatePrediction.Core.Application.DTOs;
using SalesDatePrediction.Core.Domain.Interfaces;

namespace SalesDatePrediction.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippersController : ControllerBase
    {
        private readonly IShipperRepository _shipperRepository;

        public ShippersController(IShipperRepository shipperRepository)
        {
            _shipperRepository = shipperRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShipperDto>>> GetAll()
        {
            var shippers = await _shipperRepository.GetAllShippersAsync();
            return Ok(shippers);
        }
    }
}