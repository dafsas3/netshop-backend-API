namespace NetShopAPI.Models.TestInfo
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public required string RecipeTitle { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public bool IsDelete { get; set; } = false;
    }
}
