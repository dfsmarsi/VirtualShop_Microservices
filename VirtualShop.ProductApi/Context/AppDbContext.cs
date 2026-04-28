using Microsoft.EntityFrameworkCore;
using VirtualShop.ProductApi.Models;

namespace VirtualShop.ProductApi.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
            
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        //Category
        mb.Entity<Category>().HasKey(c => c.CategoryId);
        mb.Entity<Category>().Property(c => c.Name).IsRequired().HasMaxLength(100);

        //Product
        mb.Entity<Product>().HasKey(p => p.ProductId);
        mb.Entity<Product>().Property(p => p.Name).IsRequired().HasMaxLength(100);
        mb.Entity<Product>().Property(p => p.Description).IsRequired().HasMaxLength(255);
        mb.Entity<Product>().Property(p => p.Price).IsRequired().HasPrecision(12, 2);
        mb.Entity<Product>().Property(p => p.ImageUrl).IsRequired().HasMaxLength(255);
        mb.Entity<Category>().HasMany(p => p.Products)
            .WithOne(c => c.Category)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);


        // Seed data
        mb.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Electronics" },
            new Category { CategoryId = 2, Name = "Books" },
            new Category { CategoryId = 3, Name = "Clothing" }
        );

        mb.Entity<Product>().HasData(
            new Product { ProductId = 1, Name = "Notebook Gamer", Price = 4999.99m, Description = "Notebook com RTX 4060 e 16GB RAM", Stock = 10, ImageUrl = "notebook-gamer.jpg", CategoryId = 1 },
            new Product { ProductId = 2, Name = "Clean Code", Price = 89.90m, Description = "Livro sobre boas práticas de programação", Stock = 50, ImageUrl = "clean-code.jpg", CategoryId = 2 },
            new Product { ProductId = 3, Name = "Camiseta Dev", Price = 59.90m, Description = "Camiseta 100% algodão estampa desenvolvedor", Stock = 100, ImageUrl = "camiseta-dev.jpg", CategoryId = 3 }
        );
    }
}
