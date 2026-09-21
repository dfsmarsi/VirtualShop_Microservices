using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VShop.CartApi.DTOs;
using VShop.CartApi.Repositories;

namespace VShop.CartApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartRepository _cartRepository;

    public CartController(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    [HttpGet("getcart/{id}")]
    public async Task<ActionResult<CartDTO>> GetByUserId(string id)
    {
        var cartDto = await _cartRepository.GetCartByUserIdAsync(id);

        if (cartDto is null)
            return NotFound();

        return Ok(cartDto);
    }

    [HttpPost("addcart")]
    public async Task<ActionResult<CartDTO>> AddCart(CartDTO cartDto)
    {
        var cart = await _cartRepository.UpdateCartAsync(cartDto);

        if (cart is null)
            return NotFound();

        return Ok(cart);
    }

    [HttpPut("updatecart")]
    public async Task<ActionResult<CartDTO>> UpdateCart(CartDTO cartDto)
    {
        var cart = await _cartRepository.UpdateCartAsync(cartDto);

        if (cart is null)
            return NotFound();

        return Ok(cart);
    }

    [HttpDelete("deletecart/{id}")]
    public async Task<ActionResult<bool>> DeleteCart(int id)
    {
        var status = await _cartRepository.DeleteItemCartAsync(id);

        if (!status)
            return BadRequest("Problemas ao tentar deletar item do carrinho");

        return Ok(status);
    }

    [HttpPost("applycoupon")]
    public async Task<ActionResult<CartDTO>> ApplyCoupon(CartDTO cartDTO)
    {
        var result = await _cartRepository.ApplyCouponAsync(cartDTO.CartHeader.UserId, cartDTO.CartHeader.CouponCode);

        if (!result)
            return BadRequest($"Cart not found for UserId = {cartDTO.CartHeader.UserId}");

        return Ok(result);
    }

    [HttpDelete("removecoupon/{userId}")]
    public async Task<ActionResult<CartDTO>> RemoveCoupon(string userId)
    {
        var result = await _cartRepository.RemoveCouponAsync(userId);

        if (!result)
            return BadRequest($"Discount coupon not found for UserId = {userId}");

        return Ok(result);
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<CheckoutHeaderDTO>> Checkout(CheckoutHeaderDTO checkoutHeaderDto)
    {
        var cart = await _cartRepository.GetCartByUserIdAsync(checkoutHeaderDto.UserId);

        if (cart is null)
            return BadRequest("Cart not found for the specified user.");

        checkoutHeaderDto.CartItems = cart.CartItems;
        checkoutHeaderDto.DateTime = DateTime.UtcNow;

        return Ok(checkoutHeaderDto);
    }
}
