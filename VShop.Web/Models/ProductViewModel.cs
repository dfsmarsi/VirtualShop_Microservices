using System.ComponentModel.DataAnnotations;

namespace VShop.Web.Models;

public class ProductViewModel
{
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100)]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
    public decimal? Price { get; set; }

    [Required(ErrorMessage = "Description is required")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Stock is required")]
    [Range(0, long.MaxValue, ErrorMessage = "Stock must be zero or greater")]
    public long Stock { get; set; }

    [Required(ErrorMessage = "Image URL is required")]
    public string? ImageUrl { get; set; }
    public string? CategoryName { get; set; }

    [Required(ErrorMessage = "Category is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Select a category")]
    public int CategoryId { get; set; }
}
