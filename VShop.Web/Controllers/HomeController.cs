using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using VShop.Web.Models;
using VShop.Web.Services.Interfaces;

namespace VShop.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public HomeController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProducts(string.Empty);

            if (products is null)
                return View("Error");

            var categories = await _categoryService.GetAllCategories(string.Empty);
            ViewBag.CategoryId = new SelectList(categories ?? Enumerable.Empty<CategoryViewModel>(), "CategoryId", "Name");

            return View(products.OrderBy(p => p.Name));
        }

        [HttpGet]
        public async Task<ActionResult<ProductViewModel>> ProductDetails(int id)
        {
            var product = await _productService.FindProductById(id, string.Empty);

            if (product is null)
                return View("Error");

            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Login()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            return SignOut(new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Index), "Home")
            }, "Cookies", "oidc");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
