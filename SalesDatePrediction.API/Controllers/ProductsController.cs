using Microsoft.AspNetCore.Mvc;
using SalesDatePrediction.Core.Application.DTOs;
using SalesDatePrediction.Core.Domain.Interfaces;

namespace SalesDatePrediction.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return Ok(products);
        }
    }
}