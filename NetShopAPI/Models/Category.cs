using NetShopAPI.Models.GeneralWarehouseModel.PositionItem;

namespace NetShopAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; } = null;
        public ICollection<Position> Positions { get; set; } = new List<Position>();
    }
}
