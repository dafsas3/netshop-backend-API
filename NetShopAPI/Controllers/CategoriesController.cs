using Microsoft.AspNetCore.Mvc;
using NetShopAPI.Common;
using NetShopAPI.Features.Catalog.Categories.Command;
using NetShopAPI.Features.Catalog.Categories.Query;

namespace NetShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly CreateCategoryHandler _createHandler;
        private readonly GetAllCategoriesHandler _getHandler;

        public CategoriesController(CreateCategoryHandler createHandler, GetAllCategoriesHandler getHandler)
        {
            _createHandler = createHandler;
            _getHandler = getHandler;
        }



        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory(CreateCategoryCommand cmd, CancellationToken ct)
        {
            var result = await _createHandler.Handle(cmd, ct);
            return this.ToActionResult(result);
        }


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCategories(CancellationToken ct)
        {
            return this.ToActionResult(await _getHandler.Handle(ct));
        }


    }
}
