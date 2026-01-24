using NetShopAPI.DTOs.RecipeDTO.IngredientDTO.Request;
using NetShopAPI.DTOs.RecipeDTO.IngredientDTO.Response;
using NetShopAPI.DTOs.RecipeDTO.Request;
using NetShopAPI.DTOs.RecipeDTO.Response;

namespace NetShopAPI.Services.RecipeServices
{
    public interface IRecipeService
    {
        Task<List<IngredientResponse>> CreateIngredients(List<IngredientCreateRequest> req);
        Task<List<RecipeCreationResponse>> CreateRecipe(List<RecipeCreateRequest> req);
    }
}
