namespace NetShopAPI.Models.OrderModel
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int ProductId { get; set; }
        public string ProductNameSnapshot { get; set; } = null!;
        public string ProductDescriptionSnapshot { get; set; } = null!;
        public decimal ProductPriceSnapshot { get; set; }
        public int Quantity { get; set; }
    }
}