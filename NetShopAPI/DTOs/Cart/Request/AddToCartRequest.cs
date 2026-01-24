namespace NetShopAPI.DTOs.Cart.Request
{
    public class AddToCartRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
