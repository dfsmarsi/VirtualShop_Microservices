using Microsoft.EntityFrameworkCore;
using VShop.DiscountApi.Models;

namespace VShop.DiscountApi.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Coupon> Coupons
    {
        get; set;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Coupon>().HasData(
            new Coupon
            {
                Id = 1,
                CouponCode = "PROMO10",
                Discount= 10
            },
            new Coupon
            {
                Id = 2,
                CouponCode = "PROMO20",
                Discount= 20
            }
        );
    }
}
