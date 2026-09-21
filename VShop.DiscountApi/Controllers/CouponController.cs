using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VShop.DiscountApi.DTOs;
using VShop.DiscountApi.Repositories;

namespace VShop.DiscountApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CouponController : ControllerBase
{
    private ICouponRepository _repository;

    public CouponController(ICouponRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{couponcode}")]
    public async Task<ActionResult<CouponDTO>> GetDiscountCouponByCode(string couponcode)
    {
        var coupon = await _repository.GetCouponByCode(couponcode);
        if (coupon == null)
        {
            return NotFound($"Coupon with code '{couponcode}' not found.");
        }
        return Ok(coupon);
    }

}
