namespace NetShopAPI.DTOs.OrderDTO.Request
{
    public class CreateOrderRequest
    {
        public List<int> ProductIds { get; set; } = new();
    }
}
