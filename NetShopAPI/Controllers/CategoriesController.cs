using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetShopAPI.Data;
using NetShopAPI.DTOs.CatalogDTO.Categoryes.Request;
using NetShopAPI.DTOs.CatalogDTO.Categoryes.Response;
using NetShopAPI.Models;
using NetShopAPI.Services.CatalogServices;

namespace NetShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService service)
        {
            _categoryService = service;
        }



        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory(CategoryRequest req, CancellationToken ct)
        {
            var result = await _categoryService.CreateCategoryAsync(req, ct);
            return Ok(result);
        }


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCategories(CancellationToken ct)
        {
            return Ok(await _categoryService.GetAllAsync(ct));
        }


    }
}
