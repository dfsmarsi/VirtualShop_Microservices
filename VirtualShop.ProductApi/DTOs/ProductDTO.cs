using System.ComponentModel.DataAnnotations;
using VirtualShop.ProductApi.Models;

namespace VirtualShop.ProductApi.DTOs;

public class ProductDTO
{
    public int ProductId { get; set; }

    [Required(ErrorMessage = "O nome do produto é obrigatório")]
    [MaxLength(100)]
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public string? Description { get; set; }
    public long Stock { get; set; }
    public string? ImageUrl { get; set; }
    public Category? Category { get; set; }
    public int CategoryId { get; set; }
}
