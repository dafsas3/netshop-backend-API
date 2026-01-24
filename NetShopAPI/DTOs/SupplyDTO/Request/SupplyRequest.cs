namespace NetShopAPI.DTOs.SupplyDTO.Request
{
    public class SupplyRequest
    {
        public required string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}
