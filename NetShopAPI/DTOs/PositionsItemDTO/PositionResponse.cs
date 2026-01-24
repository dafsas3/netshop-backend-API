namespace NetShopAPI.DTOs.PositionsItemDTO
{
    public class PositionResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public string? AdditionalInformation { get; set; }
        public decimal LastPurchasePrice { get; set; }
        public string? CategoryName { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
