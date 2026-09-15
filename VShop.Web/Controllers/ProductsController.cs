using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VShop.Web.Models;
using VShop.Web.Roles;
using VShop.Web.Services.Interfaces;

namespace VShop.Web.Controllers;

[Authorize(Roles = Role.Admin)]
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductsController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    private async Task<string> GetAccessToken()
    {
        return await HttpContext.GetTokenAsync("access_token");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductViewModel>>> Index()
    {

        var products = await _productService.GetAllProducts(await GetAccessToken());

        if (products is null)
            return View("Error");

        var categories = await _categoryService.GetAllCategories(await GetAccessToken());
        ViewBag.CategoryId = new SelectList(categories, "CategoryId", "Name");

        return View(products.OrderBy(p => p.Name));
    }

    [HttpPost]
    public async Task<ActionResult<ProductViewModel>> CreateProduct(ProductViewModel productVM)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _categoryService.GetAllCategories(await GetAccessToken());
            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "Name");
            var products = await _productService.GetAllProducts(await GetAccessToken());
            return View("Index", products);
        }

        var result = await _productService.CreateProduct(productVM, await GetAccessToken());

        if (result is null)
            return View("Error");

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> UpdateProduct(int id)
    {
        ViewBag.CategoryId = new SelectList(await _categoryService.GetAllCategories(await GetAccessToken()), "CategoryId", "Name");
        var result = await _productService.FindProductById(id, await GetAccessToken());

        if (result is null)
            return View("Error");

        return View(result);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProduct(ProductViewModel productVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _productService.UpdateProduct(productVM, await GetAccessToken());

            if (result is null)
                return View("Error");

            return RedirectToAction(nameof(Index));
        }

        ViewBag.CategoryId = new SelectList(await _categoryService.GetAllCategories(await GetAccessToken()), "CategoryId", "Name");
        return View(productVM);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _productService.FindProductById(id, await GetAccessToken());

        if (result is null)
            return View("Error");

        return View(result);
    }

    [HttpPost, ActionName("DeleteProduct")]
    public async Task<IActionResult> DeleteProductConfirmed(int id)
    {
        var success = await _productService.DeleteProduct(id, await GetAccessToken());

        if (!success)
            return View("Error");

        return RedirectToAction(nameof(Index));
    }
}
