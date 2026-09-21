using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VShop.DiscountApi.DTOs;

public class CouponDTO
{
    public int Id { get; set; }

    [Required]
    public string? CouponCode { get; set; }

    [Required]
    public decimal Discount { get; set; }
}
