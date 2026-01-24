using NetShopAPI.DTOs.RecipeDTO.IngredientDTO.Response;
using NetShopAPI.Models.TestInfo;

namespace NetShopAPI.DTOs.RecipeDTO.Response
{
    public class RecipeCreationResponse
    {
        public Guid Id { get; set; }
        public required string RecipeTitle { get; set; }
        public List<IngredientResponse> AddedIngredients { get; set; } = new List<IngredientResponse>();
        public List<IngredientError> Errors { get; set; } = new();

        public record IngredientError(string Name, string Message);
    }
}
