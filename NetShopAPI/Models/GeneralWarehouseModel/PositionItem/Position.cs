namespace NetShopAPI.Models.GeneralWarehouseModel.PositionItem
{
    public class Position
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public required string Name { get; set; }
        public required int Amount { get; set; }
        public required decimal Price { get; set; }
        public string AdditionalInformation { get; set; } = null!;
        public decimal LastPurchasePrice { get; set; }
        public bool IsDelete { get; set; } = false;
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public Category Category { get; set; } = null!;
    }
}
