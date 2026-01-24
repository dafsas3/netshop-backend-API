using NetShopAPI.Models.OrderModel;

namespace NetShopAPI.DTOs.OrderDTO.Request
{
    public class OrderRequest
    {

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
