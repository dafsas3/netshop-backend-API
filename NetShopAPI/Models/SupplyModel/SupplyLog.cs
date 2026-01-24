namespace NetShopAPI.Models.SupplyModel
{
    public class SupplyLog
    {
        public Guid Id { get; set; }
        public required string PositionName { get; set; }
        public int AmountReceived { get; set; }
        public decimal PriceAtSupply { get; set; }
        public DateTime Date { get; set; }
        public string AdditionalInformation { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}
