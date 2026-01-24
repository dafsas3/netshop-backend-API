namespace NetShopAPI.Models.TestInfo
{
    public class Ingredient
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public int Quantity { get; set; }
        public bool IsDelete { get; set; } = false;
        public List<Recipe> Recipes { get; set; } = new();
    }
}
