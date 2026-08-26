using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VShop.Web.Models;
using VShop.Web.Roles;
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
    [Authorize]
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

    [HttpGet]
    public async Task<IActionResult> UpdateProduct(int id)
    {
        ViewBag.CategoryId = new SelectList(await _categoryService.GetAllCategories(), "CategoryId", "Name");
        var result = await _productService.FindProductById(id);

        if (result is null)
            return View("Error");

        return View(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateProduct(ProductViewModel productVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _productService.UpdateProduct(productVM);

            if (result is null)
                return View("Error");

            return RedirectToAction(nameof(Index));
        }

        ViewBag.CategoryId = new SelectList(await _categoryService.GetAllCategories(), "CategoryId", "Name");
        return View(productVM);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _productService.FindProductById(id);

        if (result is null)
            return View("Error");

        return View(result);
    }

    [HttpPost, ActionName("DeleteProduct")]
    [Authorize(Roles = Role.Admin)]
    public async Task<IActionResult> DeleteProductConfirmed(int id)
    {
        var success = await _productService.DeleteProduct(id);

        if (!success)
            return View("Error");

        return RedirectToAction(nameof(Index));
    }
}
