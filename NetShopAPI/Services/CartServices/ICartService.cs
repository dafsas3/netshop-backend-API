using NetShopAPI.Common;
using NetShopAPI.DTOs.Cart.Request;
using NetShopAPI.DTOs.Cart.Response;
using NetShopAPI.Models.CartModel;

namespace NetShopAPI.Services.CartServices
{
    public interface ICartService
    {
        Task<Result<CartResponse>> GetUserCartAsync(CancellationToken ct);
        Task<Result<CartResponse>> AddProductToUserCart(AddToCartRequest req, CancellationToken ct);
    }
}
