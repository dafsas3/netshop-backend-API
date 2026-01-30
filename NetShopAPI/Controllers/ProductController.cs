using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetShopAPI.Common;
using NetShopAPI.Data;
using NetShopAPI.DTOs.CatalogDTO.Products.Request;
using NetShopAPI.DTOs.CatalogDTO.Products.Response;
using NetShopAPI.Models;
using NetShopAPI.Services.PositionProductsServices;

namespace NetShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }



        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct(List<ProductRequest> req, CancellationToken ct)
        {
            var result = await _productService.CreatePositionProductAsync(req, ct);
            return Ok(result);
        }


        [HttpGet("by-category/{categoryId:int}")]
        public async Task<IActionResult> GetByCategory (int categoryId)
        {
            var result = await _productService.GetPositionProductsByCategoryAsync(categoryId);
            return this.ToActionResult(result);
        }


        [HttpGet("by-id/{id:guid}")]
        public async Task<IActionResult> GetById (Guid id)
        {
            var result = await _productService.GetPositionProductByIdAsync(id);

            return this.ToActionResult(result);
        }


        [HttpPut("addToStock/{productId:int},{addAmount:int}")]
        public async Task<IActionResult> AddAmountProductToStock(int productId, int addAmount)
        {
            var result = await _productService.AddToStock(productId, addAmount);

            return this.ToActionResult(result);
        }

    }
}
