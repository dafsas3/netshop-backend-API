using NetShopAPI.DTOs.RecipeDTO.IngredientDTO.Request;
using NetShopAPI.Models.TestInfo;

namespace NetShopAPI.DTOs.RecipeDTO.Request
{
    public class RecipeCreateRequest
    {
        public required string RecipeTitle { get; set; }
        public List<IngredientCreateRequest> IngredientsRequest { get; set; } = new();
    }
}
