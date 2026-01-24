using Microsoft.EntityFrameworkCore;
using NetShopAPI.Data;
using NetShopAPI.DTOs.RecipeDTO.IngredientDTO.Request;
using NetShopAPI.DTOs.RecipeDTO.IngredientDTO.Response;
using NetShopAPI.DTOs.RecipeDTO.Request;
using NetShopAPI.DTOs.RecipeDTO.Response;
using NetShopAPI.Models.TestInfo;

namespace NetShopAPI.Services.RecipeServices
{
    public class RecipeService : IRecipeService
    {

        private readonly ShopDbContext _db;

        public RecipeService(ShopDbContext db)
        {
            _db = db;
        }


        public async Task<List<IngredientResponse>> CreateIngredients(List<IngredientCreateRequest> req)
        {
            var uniqueNames = req
                .Where(n => !string.IsNullOrEmpty(n.Name))
                .Select(n => n.Name)
                .Distinct()
                .ToList();

            var existingIngredients = await _db.Ingredients
                .Where(i => uniqueNames.Contains(i.Name))
                .Select(i => i.Name)
                .ToListAsync();

            var newIngredients = req
                .Where(r => !existingIngredients.Contains(r.Name))
                .GroupBy(r => r.Name)
                .Select(g => new Ingredient
                {
                    Name = g.Key,
                    Quantity = g.First().Quantity
                })
                .ToList();

            if (newIngredients.Any())
            {
                _db.Ingredients.AddRange(newIngredients);
                await _db.SaveChangesAsync();
            }

            return newIngredients.Select(i => new IngredientResponse
            {
                Id = i.Id,
                Name = i.Name,
                Quantity = i.Quantity
            }).ToList();
        }


        public async Task<List<RecipeCreationResponse>> CreateRecipe(List<RecipeCreateRequest> req)
        {
            var recipeTitles = req.Select(r => r.RecipeTitle).Distinct().ToList();

            var allIngredientNames = req
                .SelectMany(i => i.IngredientsRequest)
                .Select(i => i.Name)
                .Distinct()
                .ToList();

            var existingRecipeTitles = await _db.Recipes
                .Where(r => recipeTitles.Contains(r.RecipeTitle))
                .Select(r => r.RecipeTitle)
                .ToListAsync();

            var existingIngredientsMap = await _db.Ingredients
                .Where(i => allIngredientNames.Contains(i.Name))
                .ToDictionaryAsync(i => i.Name);

            var createdRecipes = new List<Recipe>();
            var response = new List<RecipeCreationResponse>();

            var newRecipesRequests = req
                .Where(r => !existingRecipeTitles.Contains(r.RecipeTitle))
                .ToList();

            foreach (var recipeReq in newRecipesRequests)
            {
                var recipeEntity = new Recipe
                {
                    Id = Guid.NewGuid(),
                    RecipeTitle = recipeReq.RecipeTitle,
                    Ingredients = new List<Ingredient>()
                };

                var recipeResponse = new RecipeCreationResponse
                {
                    Id = recipeEntity.Id,
                    RecipeTitle = recipeEntity.RecipeTitle,
                    AddedIngredients = new(),
                    Errors = new()
                };

                foreach (var ingReq in recipeReq.IngredientsRequest)
                {
                    if (existingIngredientsMap.TryGetValue(ingReq.Name, out var ingregient))
                    {
                        recipeEntity.Ingredients.Add(ingregient);

                        recipeResponse.AddedIngredients.Add(new IngredientResponse
                        {
                            Id = ingregient.Id,
                            Name = ingregient.Name,
                            Quantity = ingregient.Quantity
                        });
                    }

                    else
                    {
                        recipeResponse.Errors.Add(new RecipeCreationResponse.IngredientError(
                            ingReq.Name, "- ингредиент не найден в базе данных."));
                    }                  
                }

                createdRecipes.Add(recipeEntity);
                response.Add(recipeResponse);
            }

            if (createdRecipes.Any())
            {
                _db.Recipes.AddRange(createdRecipes);
                await _db.SaveChangesAsync();
            }

            return response;
        }


    }
}
