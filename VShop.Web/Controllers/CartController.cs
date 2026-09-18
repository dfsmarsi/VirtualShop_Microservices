using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VShop.Web.Models;
using VShop.Web.Services.Interfaces;

namespace VShop.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            CartViewModel? cartVM = await GetCartByUser();

            if (cartVM is null)
            {
                ModelState.AddModelError("CartNotFound", "Does not exist a cart yet.");
                return View("/Views/Cart/CartNotFound.cshtml");
            }

            return View(cartVM);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> RemoveItem(int id)
        {
            var result = await _cartService.RemoveItemFromCartAsync(id, await GetAccessTokenAsync());

            if (result)
                return RedirectToAction(nameof(Index));

            return View(id);
        }

        private async Task<CartViewModel?> GetCartByUser()
        {
            var cart = await _cartService.GetCartByUserIdAsync(GetUserId(), await GetAccessTokenAsync());

            if(cart?.CartHeader is not null)
            {
                foreach (var item in cart.CartItems)
                {
                    cart.CartHeader.TotalAmount += (decimal)(item.Product.Price * item.Quantity);
                }
            }

            return cart;
        }

        private string GetUserId()
        {
            return User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        }

        private async Task<string> GetAccessTokenAsync()
        {
            return await HttpContext.GetTokenAsync("access_token");
        }
    }
}
