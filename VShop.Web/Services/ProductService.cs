using System.Text.Json;
using VShop.Web.Models;
using VShop.Web.Services.Interfaces;

namespace VShop.Web.Services;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _clientFactory;
    private const string endpoint = "/api/products";
    private readonly JsonSerializerOptions _options;
    private ProductViewModel productVM;
    private IEnumerable<ProductViewModel> productsListVM;

    public ProductService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IEnumerable<ProductViewModel>> GetAllProducts()
    {
        var client = _clientFactory.CreateClient("ProductsApi");

        using (var response = await client.GetAsync(endpoint))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productsListVM = await JsonSerializer
                    .DeserializeAsync<IEnumerable<ProductViewModel>>(apiResponse, _options);
            }
            else
                return null; 
        }

        return productsListVM;
    }

    public async Task<ProductViewModel?> FindProductById(int id)
    {
        var client = _clientFactory.CreateClient("ProductsApi");

        using (var response = await client.GetAsync(endpoint + "/" + id))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productVM = await JsonSerializer
                    .DeserializeAsync<ProductViewModel>(apiResponse, _options);
            }
            else
                return null;
        }
        return productVM;
    }

    public async Task<ProductViewModel> CreateProduct(ProductViewModel productVM)
    {
        var client = _clientFactory.CreateClient("ProductsApi");

        StringContent content = new StringContent(JsonSerializer
            .Serialize(productVM), System.Text.Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync(endpoint, content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productVM = await JsonSerializer
                    .DeserializeAsync<ProductViewModel>(apiResponse, _options);
            }
            else
                return null;
        }

        return productVM;
    }

    public async Task<ProductViewModel> UpdateProduct(ProductViewModel product)
    {
        var client = _clientFactory.CreateClient("ProductsApi");
        ProductViewModel productUpdated = new ProductViewModel();

        using (var response = await client.PutAsJsonAsync(endpoint, product))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                productUpdated = await JsonSerializer
                    .DeserializeAsync<ProductViewModel>(apiResponse, _options);
            }
            else
                return null;
        }

        return productUpdated;
    }

    public async Task<bool> DeleteProduct(int id)
    {
        var client = _clientFactory.CreateClient("ProductsApi");

        using (var response = await client.DeleteAsync(endpoint + "/" + id))
        {
            if (response.IsSuccessStatusCode)
                return true;
        }
        return false;
    }
}
