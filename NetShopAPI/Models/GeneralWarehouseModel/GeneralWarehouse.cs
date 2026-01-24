using NetShopAPI.Models.GeneralWarehouseModel.PositionItem;

namespace NetShopAPI.Models.GeneralWarehouseModel
{
    public class GeneralWarehouse
    {
        public int TotalQuantity { get; set; }
        public decimal TotalCost { get; set; }
        public List<Position> PositionsItem { get; set; } = new();
    }
}
