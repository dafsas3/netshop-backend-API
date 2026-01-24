namespace NetShopAPI.DTOs.PositionsItemDTO
{
    public class PositionAddStockResponse
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public int Amount { get; set; }

        public string? ExInfo { get; set; }
    }
}
