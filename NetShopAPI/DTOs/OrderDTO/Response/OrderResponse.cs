using NetShopAPI.Common.Order;
using NetShopAPI.Models.OrderModel;
using static NetShopAPI.DTOs.OrderDTO.Response.OrderManagerDTO;

namespace NetShopAPI.DTOs.OrderDTO.Response
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public string UserEmail { get; set; } = null!;
        public OrderStatusDTO Status { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemResponse> Items { get; set; } = new();
    }
}
