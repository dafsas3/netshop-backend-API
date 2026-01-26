namespace NetShopAPI.DTOs.SupplyDTO.Response
{
    public class SupplyResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public string? AdditionalInformation { get; set; }
        public decimal Totalprice { get; set; }
    }
}
