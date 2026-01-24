using NetShopAPI.Common;
using NetShopAPI.DTOs.OrderDTO.Request;
using NetShopAPI.DTOs.OrderDTO.Response;

namespace NetShopAPI.Services.OrderServices
{
    public interface IOrderService
    {
        Task<Result<OrderResponse>> CreateUserOrder(CreateOrderRequest req);
    }
}
