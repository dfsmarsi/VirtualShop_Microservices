using Microsoft.EntityFrameworkCore;
using VShop.CartApi.Models;

namespace VShop.CartApi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Product
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Product>().Property(p => p.Id).ValueGeneratedNever();
            modelBuilder.Entity<Product>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Product>().Property(p => p.Description).HasMaxLength(500);
            modelBuilder.Entity<Product>().Property(p => p.Price).IsRequired().HasPrecision(12, 2);
            modelBuilder.Entity<Product>().Property(p => p.ImageUrl).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Product>().Property(p => p.CategoryName).IsRequired().HasMaxLength(100);

            //CartHeader
            modelBuilder.Entity<CartHeader>().Property(c => c.UserId).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<CartHeader>().Property(c => c.CouponCode).HasMaxLength(100);

            //CartItem
            modelBuilder.Entity<CartItem>()
                .HasOne<CartHeader>()
                .WithMany()
                .HasForeignKey(ci => ci.CartHeaderId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
