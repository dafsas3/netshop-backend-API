using NetShopAPI.Common.Order;

namespace NetShopAPI.Models.OrderModel
{
    public class Order
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string UserEmail { get; set; } = null!;
        public OrderStatus Status { get; set; } = OrderStatus.Created;
        public DateTime CreatedAtUtc { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
