using NetShopAPI.Models.GeneralWarehouseModel.PositionItem;

namespace NetShopAPI.DTOs.PositionsItemDTO
{
    public class BulkPositionResponse
    {
        public bool IsSuccess { get; set; }

        public int? PositionId { get; set; }
        public int? ProductId { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? Amount { get; set; }
        public decimal? LastPurchasePrice { get; set; }
        public string? AdditionalInformation { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? CategoryName { get; set; }

        public List<Errors> ErrorsAddPosition { get; set; } = new();
        public List<ErrorsCategories> ErrorsAddCategory { get; set; } = new();
        public record Errors(string Code, string Message);
        public record ErrorsCategories(string Code, string Message);
    }
}
