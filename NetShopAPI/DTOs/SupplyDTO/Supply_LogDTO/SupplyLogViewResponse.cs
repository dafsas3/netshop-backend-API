namespace NetShopAPI.DTOs.SupplyDTO.Supply_LogDTO
{
    public class SupplyLogViewResponse
    {
        public Guid Id { get; set; }
        public required string PositionName { get; set; }
        public int AmountReceived { get; set; }
        public decimal PriceAtSupply { get; set; }
        public DateTime Date { get; set; }
        public string AdditionalInformation { get; set; }
    }
}
