namespace NetShopAPI.Features.Stock.DTOs
{
    public class PositionAddStockResponse
    {
        public int PositionId { get; set; }
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public int Amount { get; set; }

        public string? ExInfo { get; set; }
    }
}
