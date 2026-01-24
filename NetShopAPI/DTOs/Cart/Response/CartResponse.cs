namespace NetShopAPI.DTOs.Cart.Response
{
    public class CartResponse
    {
        public List<CartItemResponse> Items { get; set; } = new();
        public decimal Total => Items.Sum(x => x.TotalPrice);
    }
}
