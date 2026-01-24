namespace NetShopAPI.DTOs.PositionsItemDTO
{
    public class BulkPositionResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public string AdditionalInformation { get; set; } = null!;
        public decimal LastPurchasePrice { get; set; }
        public decimal TotalPrice { get; set; }
        public List<Errors> ErrorsAddPosition { get; set; } = new();
        public List<ErrorsCategories> ErrorsAddCategory { get; set; } = new();
        public record Errors(string Name, string Message);
        public record ErrorsCategories(int id, string Message);
        public string? CategoryName { get; set; }
    }
}
