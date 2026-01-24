namespace NetShopAPI.Models.CartModel
{
    public class Cart
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
