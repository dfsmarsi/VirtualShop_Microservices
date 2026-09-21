using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VShop.DiscountApi.Models;

public class Coupon
{
    public int Id { get; set; }

    [StringLength(30)]
    public string? CouponCode { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Discount { get; set; }
}
