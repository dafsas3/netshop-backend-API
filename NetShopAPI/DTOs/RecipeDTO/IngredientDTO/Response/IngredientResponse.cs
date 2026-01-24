namespace NetShopAPI.DTOs.RecipeDTO.IngredientDTO.Response
{
    public class IngredientResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public int Quantity { get; set; }
    }
}
