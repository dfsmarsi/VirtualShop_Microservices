using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using VShop.Web.Models;

namespace VShop.Web.Services.Interfaces
{
    public class CartService : ICartService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private const string apiEndpoint = "/api/cart";
        private CartViewModel _cartViewModel = new CartViewModel();

        public CartService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<CartViewModel> AddItemToCartAsync(CartViewModel cartVM, string token)
        {
            var client = _httpClientFactory.CreateClient("CartApi");
            PutTokenInHeaderAuthorization(token, client);

            StringContent content = new StringContent(JsonSerializer.Serialize(cartVM), Encoding.UTF8, "application/json");

            using (var response = await client.PostAsync($"{apiEndpoint}/addcart/", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    _cartViewModel = JsonSerializer.Deserialize<CartViewModel>(apiResponse, _jsonSerializerOptions);
                }
                else
                {
                    return null;
                }
            }

            return _cartViewModel;
        }

        public Task<bool> ClearCartAsync(string userId, string token)
        {
            throw new NotImplementedException();
        }

        public async Task<CartViewModel> GetCartByUserIdAsync(string userId, string token)
        {
            var client = _httpClientFactory.CreateClient("CartApi");
            PutTokenInHeaderAuthorization(token, client);

            using (var response = await client.GetAsync($"{apiEndpoint}/GetCart/{userId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    _cartViewModel = JsonSerializer.Deserialize<CartViewModel>(apiResponse, _jsonSerializerOptions);
                }
                else
                {
                    return null;
                }
            }

            return _cartViewModel;
        }

        public async Task<bool> RemoveItemFromCartAsync(int cartItemId, string token)
        {
            var client = _httpClientFactory.CreateClient("CartApi");
            PutTokenInHeaderAuthorization(token, client);

            using (var response = await client.DeleteAsync($"{apiEndpoint}/deletecart/" + cartItemId))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<CartViewModel> UpdateCartAsync(CartViewModel cartVM, string token)
        {
            var client = _httpClientFactory.CreateClient("CartApi");
            PutTokenInHeaderAuthorization(token, client);

            CartViewModel cartUpdated = new CartViewModel();

            using (var response = await client.PutAsJsonAsync($"{apiEndpoint}/updatecart/", cartVM))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    _cartViewModel = JsonSerializer.Deserialize<CartViewModel>(apiResponse, _jsonSerializerOptions);
                }
                else
                {
                    return null;
                }
            }

            return cartUpdated;
        }

        public Task<CartViewModel> CheckoutAsync(CartHeaderViewModel cartHeaderVM, string token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ApplyCouponAsync(CartViewModel cartVM, string couponCode, string token)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveCouponAsync(string userId, string token)
        {
            throw new NotImplementedException();
        }


        private void PutTokenInHeaderAuthorization(string token, HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
