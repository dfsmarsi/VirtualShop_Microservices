using System.ComponentModel.DataAnnotations;
using VirtualShop.ProductApi.Models;

namespace VirtualShop.ProductApi.DTOs;

public class CategoryDTO
{
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "O nome da categoria é obrigatório")]
    [MaxLength(100)]
    public string? Name { get; set; }
    public ICollection<Product>? Products { get; set; }
}
