using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetShopAPI.DTOs.RecipeDTO.IngredientDTO.Request;
using NetShopAPI.DTOs.RecipeDTO.Request;
using NetShopAPI.Services.RecipeServices;

namespace NetShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {

        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }



        [HttpPost("create-ingredients")]
        public async Task<IActionResult> CreateIngredients(List<IngredientCreateRequest> req)
        {
            var result = await _recipeService.CreateIngredients(req);
            return Ok(result);
        }


        [HttpPost("create-recipes")]
        public async Task<IActionResult> CreateRecipes(List<RecipeCreateRequest> req)
        {
            var result = await _recipeService.CreateRecipe(req);
            return Ok(result);
        }


    }
}
