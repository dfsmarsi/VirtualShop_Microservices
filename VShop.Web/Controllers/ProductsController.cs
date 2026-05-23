using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VShop.Web.Models;
using VShop.Web.Services.Interfaces;

namespace VShop.Web.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductsController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductViewModel>>> Index()
    {
        var products = await _productService.GetAllProducts();

        if (products is null)
            return View("Error");

        var categories = await _categoryService.GetAllCategories();
        ViewBag.CategoryId = new SelectList(categories, "CategoryId", "Name");

        return View(products.OrderBy(p => p.Name));
    }

    [HttpPost]
    public async Task<ActionResult<ProductViewModel>> CreateProduct(ProductViewModel productVM)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _categoryService.GetAllCategories();
            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "Name");
            var products = await _productService.GetAllProducts();
            return View("Index", products);
        }

        var result = await _productService.CreateProduct(productVM);

        if (result is null)
            return View("Error");

        return RedirectToAction(nameof(Index));
    }
}
