using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetShopAPI.Common;
using NetShopAPI.Data;
using NetShopAPI.DTOs.CatalogDTO.Products.Request;
using NetShopAPI.DTOs.CatalogDTO.Products.Response;
using NetShopAPI.Features.Catalog.Commands.BulkCreatePositions;
using NetShopAPI.Features.Catalog.Positions.Queries.GetPositionById;
using NetShopAPI.Features.Catalog.Positions.Queries.GetPositionsByCategory;
using NetShopAPI.Features.Stock.Commands.AddToStock;
using NetShopAPI.Models;
using NetShopAPI.Services.PositionProductsServices;

namespace NetShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly AddToStockHandler _stockHandler;
        private readonly BulkCreatePositionsHandler _bulkCreateHandler;
        private readonly GetPositionByIdHandler _positionByIdHandler;
        private readonly GetPositionsByCategoryHandler _positionByCategoryHandler;

        public ProductController(
            AddToStockHandler handler, BulkCreatePositionsHandler bulkCreateHandler,
            GetPositionByIdHandler positionById, GetPositionsByCategoryHandler positionsByCategory)
        {
            _stockHandler = handler;
            _bulkCreateHandler = bulkCreateHandler;
            _positionByIdHandler = positionById;
            _positionByCategoryHandler = positionsByCategory;
        }



        [HttpPost("create-position/")]
        public async Task<IActionResult> CreatePositionProduct(
            [FromBody] List<BulkCreatePositionsCommand> cmd, CancellationToken ct)
        {
            var result = await _bulkCreateHandler.Handle(cmd, ct);

            return this.ToActionResult(result);
        }


        [HttpGet("by-category/{categoryId:int}")]
        public async Task<IActionResult> GetPositionsByCategory(int categoryId, CancellationToken ct)
        {
            var result = await _positionByCategoryHandler.Handle(categoryId, ct);

            return this.ToActionResult(result);
        }


        [HttpGet("by-id/{id:int}")]
        public async Task<IActionResult> GetPositionById(int id, CancellationToken ct)
        {
            var result = await _positionByIdHandler.Handle(id, ct);

            return this.ToActionResult(result);
        }


        [HttpPut("addToStock/")]
        public async Task<IActionResult> AddProductToStock([FromBody] AddToStockCommand command, CancellationToken ct)
        {
            var result = await _stockHandler.Handle(command, ct);

            return this.ToActionResult(result);
        }

    }
}
