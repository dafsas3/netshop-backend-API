namespace NetShopAPI.DTOs.RecipeDTO.IngredientDTO.Request
{
    public class IngredientCreateRequest
    {
        public required string Name { get; set; }
        public int Quantity { get; set; }
    }
}