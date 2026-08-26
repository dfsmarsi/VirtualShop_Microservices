using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualShop.ProductApi.DTOs;
using VirtualShop.ProductApi.Roles;
using VirtualShop.ProductApi.Services;

namespace VirtualShop.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var productsDto = await _productService.GetProducts();

            if (productsDto == null)
                return NotFound("Products not found!");

            return Ok(productsDto);
        }

        [HttpGet("{id:int}", Name = "GetProductById")]
        [Authorize]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var productDto = await _productService.GetProductById(id);

            if (productDto == null)
                return NotFound("Product not found!");

            return Ok(productDto);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] ProductDTO productDto)
        {
            if (productDto == null)
                return BadRequest("Product data is null!");

            await _productService.AddProduct(productDto);

            return CreatedAtAction(nameof(GetById), new { id = productDto.ProductId }, productDto);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> Put([FromBody] ProductDTO productDto)
        {
            if (productDto == null)
                return BadRequest("Product data is invalid!");

            await _productService.UpdateProduct(productDto);

            return Ok(productDto);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
            var productDto = await _productService.GetProductById(id);

            if (productDto == null)
                return NotFound("Product not found!");

            await _productService.RemoveProduct(id);

            return Ok(productDto);
        }
    }
}
