using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetShopAPI.Common;
using NetShopAPI.Data;
using NetShopAPI.DTOs.CatalogDTO.Products.Request;
using NetShopAPI.DTOs.CatalogDTO.Products.Response;
using NetShopAPI.Features.Stock.Commands.AddToStock;
using NetShopAPI.Models;
using NetShopAPI.Services.PositionProductsServices;

namespace NetShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly AddToStockHandler _handler;

        public ProductController(IProductService productService, AddToStockHandler handler)
        {
            _productService = productService;
            _handler = handler;
        }



        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct(List<ProductRequest> req, CancellationToken ct)
        {
            var result = await _productService.CreatePositionProductAsync(req, ct);
            return Ok(result);
        }


        [HttpGet("by-category/{categoryId:int}")]
        public async Task<IActionResult> GetByCategory(int categoryId, CancellationToken ct)
        {
            var result = await _productService.GetPositionProductsByCategoryAsync(categoryId, ct);
            return this.ToActionResult(result);
        }


        [HttpGet("by-id/{id:guid}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _productService.GetPositionProductByIdAsync(id, ct);

            return this.ToActionResult(result);
        }


        [HttpPut("addToStock/{productId:int},{addAmount:int}")]
        public async Task<IActionResult> AddProductToStock([FromBody] AddToStockCommand command, CancellationToken ct)
        {
            var result = await _handler.Handle(command, ct);

            return this.ToActionResult(result);
        }

    }
}
