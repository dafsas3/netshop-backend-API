using NetShopAPI.Common.Order;

namespace NetShopAPI.DTOs.OrderDTO.Request
{
    public class ChangeOrderStatusRequest
    {
        public OrderStatus NewStatus { get; set; }
    }
}
