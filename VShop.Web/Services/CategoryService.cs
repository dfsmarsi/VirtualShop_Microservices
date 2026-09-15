using System.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text.Json;
using VShop.Web.Models;
using VShop.Web.Services.Interfaces;

namespace VShop.Web.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly JsonSerializerOptions _options;
        private const string endpoint = "/api/categories";

        public CategoryService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategories(string token)
        {
            var client = _clientFactory.CreateClient("ProductsApi");
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<IEnumerable<CategoryViewModel>>(content, _options);
            }

            return null;
        }
    }
}
