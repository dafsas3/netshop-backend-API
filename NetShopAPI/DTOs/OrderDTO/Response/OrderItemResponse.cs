namespace NetShopAPI.DTOs.OrderDTO.Response
{
    public class OrderItemResponse
    {
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total => Price * Quantity;
    }
}
