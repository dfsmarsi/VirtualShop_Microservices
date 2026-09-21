using System.Net.Http.Headers;
using System.Text.Json;
using VShop.Web.Models;

namespace VShop.Web.Services.Interfaces;

public class CouponService : ICouponService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly JsonSerializerOptions? _serializerOptions;
    private const string apiEndpoint = "/api/coupon";
    private CouponViewModel couponVM = new CouponViewModel();

    public CouponService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _serializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<CouponViewModel> GetDiscountCoupon(string couponCode, string token)
    {
        var client = _clientFactory.CreateClient("DiscountApi");

        PutTokenInHeaderAuthorization(client, token);

        using (var response = await client.GetAsync($"{apiEndpoint}/{couponCode}"))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStringAsync();
                couponVM = JsonSerializer.Deserialize<CouponViewModel>(apiResponse, _serializerOptions);
            }
            else
                return null;
        }

        return couponVM;
    }

    private void PutTokenInHeaderAuthorization(HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
