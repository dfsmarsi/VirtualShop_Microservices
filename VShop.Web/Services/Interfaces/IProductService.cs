using VShop.Web.Models;

namespace VShop.Web.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetAllProducts(string accessToken);
        Task<ProductViewModel> FindProductById(int id, string accessToken);
        Task<ProductViewModel> CreateProduct(ProductViewModel product, string accessToken);
        Task<ProductViewModel> UpdateProduct(ProductViewModel product, string accessToken);
        Task<bool> DeleteProduct(int id, string accessToken);
    }
}
