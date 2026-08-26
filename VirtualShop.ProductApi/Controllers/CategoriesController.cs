using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualShop.ProductApi.DTOs;
using VirtualShop.ProductApi.Roles;
using VirtualShop.ProductApi.Services;

namespace VirtualShop.ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            var categoriesDto = await _categoryService.GetCategories();

            if (categoriesDto == null)
                return NotFound("Categories not found!");

            return Ok(categoriesDto);
        }

        [HttpGet("{id:int}", Name = "GetCategoryById")]
        [Authorize]
        public async Task<ActionResult<CategoryDTO>> GetById(int id)
        {
            var categoryDto = await _categoryService.GetCategoryById(id);
            if (categoryDto == null)
                return NotFound("Category not found!");
            return Ok(categoryDto);
        }

        [HttpGet("products")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesProducts()
        {
            var categoriesDto = await _categoryService.GetCategoriesProducts();
            if (categoriesDto == null)
                return NotFound("Categories not found!");
            return Ok(categoriesDto);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] CategoryDTO categoryDto)
        {
            if (categoryDto == null)
                return BadRequest("Category data is null!");
            await _categoryService.AddCategory(categoryDto);
            return new CreatedAtActionResult("GetCategoryById", null, new { id = categoryDto.CategoryId }, categoryDto);
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<ActionResult> Put(int id, [FromBody] CategoryDTO categoryDto)
        {
            if (categoryDto == null || id != categoryDto.CategoryId)
                return BadRequest("Invalid category data!");

            await _categoryService.UpdateCategory(categoryDto);

            return Ok(categoryDto);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult<CategoryDTO>> Delete(int id)
        {
            var categoryDto = await _categoryService.GetCategoryById(id);

            if (categoryDto == null)
                return NotFound("Category not found!");

            await _categoryService.RemoveCategory(id);

            return Ok(categoryDto);
        }
    }
}
