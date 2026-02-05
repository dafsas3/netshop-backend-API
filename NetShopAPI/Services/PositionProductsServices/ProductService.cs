using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NetShopAPI.Common;
using NetShopAPI.Data;
using NetShopAPI.DTOs.CatalogDTO.Products.Request;
using NetShopAPI.DTOs.CatalogDTO.Products.Response;
using NetShopAPI.DTOs.PositionsItemDTO;
using NetShopAPI.Features.Stock.DTOs;
using NetShopAPI.Models;
using NetShopAPI.Models.GeneralWarehouseModel.PositionItem;
using System.Reflection.Metadata.Ecma335;

namespace NetShopAPI.Services.PositionProductsServices
{
    public class ProductService : IProductService
    {

        private readonly ShopDbContext _db;

        public ProductService(ShopDbContext db)
        {
            _db = db;
        }


        public async Task<Result<PositionAddStockResponse>> AddToStock(int productId, int quantity,
            CancellationToken ct)
        {
            if (quantity <= 0) 
                return Result<PositionAddStockResponse>.BadRequest(
                "INVALID_QUANTITY",
                $"Введённое количество добавляемого продукта не может быть: {quantity}");

            var itemStock = await _db.Positions
                .Where(p => p.ProductId == productId)
                .FirstOrDefaultAsync(ct);

            if (itemStock is null)
                return Result<PositionAddStockResponse>.NotFound(
                    "INVALID_PRODUCT_ID",
                    $"ProductID: {productId} не найден в базе данных!");

            itemStock.Amount += quantity;
            await _db.SaveChangesAsync(ct);

            var response = new PositionAddStockResponse
            {
                PositionId = itemStock.Id,
                ProductId = itemStock.ProductId,
                Name = itemStock.Name,
                Amount = itemStock.Amount
            };

            return Result<PositionAddStockResponse>.Ok(response);
        }


        public async Task<List<BulkPositionResponse>> CreatePositionProductAsync(List<ProductRequest> req,
            CancellationToken ct)
        {
            var productNames = req.Select(p => p.Name).Distinct().ToList();

            var existingProduct = (await _db.Products.Where(p => productNames.Contains(p.Name))
                .Select(p => p.Name)
                .ToListAsync(ct))
                .ToHashSet();

            var reqCategIds = req.Select(c => c.CategoryId).Distinct().ToList();

            var existingCategories = await _db.ProductCategories
                .Where(c => reqCategIds.Contains(c.Id)).ToDictionaryAsync(c => c.Id, ct);

            var createdPositions = new List<Position>();
            var responses = new List<BulkPositionResponse>(req.Count);
            var successResponseIndexes = new List<int>();

            foreach (var prod in req)
            {
                var response = new BulkPositionResponse();

                if (!PositionServiceManager.TryValidate(prod, response, existingCategories,
                    existingProduct, out var category))
                {
                    response.IsSuccess = false;
                    responses.Add(response);
                    continue;
                }

                var newProduct = PositionServiceManager.GetCreationProduct(prod);
                var newPosition = PositionServiceManager.GetCreationPosition(newProduct, prod, category!);

                createdPositions.Add(newPosition);

                response.IsSuccess = true;
                responses.Add(response);
                successResponseIndexes.Add(responses.Count - 1);
            }

            if (createdPositions.Any())
            {
                _db.Positions.AddRange(createdPositions);
                await _db.SaveChangesAsync(ct);

                for (int i = 0; i < createdPositions.Count; i++)
                {
                    var pos = createdPositions[i];
                    var successIndex = successResponseIndexes[i];

                    PositionServiceManager.FillSuccessResponse(responses[successIndex], pos);
                }
            }

            return responses;
        }


        public async Task<Result<List<PositionResponse>>> GetPositionProductsByCategoryAsync(int categoryId,
            CancellationToken ct)
        {
            var category = await _db.ProductCategories
                .Where(c => c.Id == categoryId)
                .FirstOrDefaultAsync(ct);

            if (category is null)
                return Result<List<PositionResponse>>.NotFound("INVALID_CATEGORY_ID",
                    $"CategoryID: {categoryId} не найден в базе данных!");

            var positionsForCategory = await _db.Positions.Where(p => p.CategoryId == category.Id).Select(
                p => new PositionResponse
                {
                    Id = p.Id,
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Price = p.Price,
                    Amount = p.Amount,
                    AdditionalInformation = p.AdditionalInformation,
                    TotalPrice = p.Price * p.Amount,
                    CategoryName = category.Name
                }).ToListAsync(ct);

            return Result<List<PositionResponse>>.Ok(positionsForCategory);
        }


        public async Task<Result<PositionResponse>> GetPositionProductByIdAsync(int id, CancellationToken ct)
        {
            var position = await _db.Positions
          .Where(p => p.Id == id)
          .Select(p => new PositionResponse
          {
              Name = p.Name,
              AdditionalInformation = p.AdditionalInformation,  
              Price = p.Price,
              CategoryName = p.Category.Name,
              Amount = p.Amount,
              Id = id,
              TotalPrice = p.Price * p.Amount
          })
          .FirstOrDefaultAsync(ct);

            if (position is null) return Result<PositionResponse>.NotFound("INVALID_POSITION_ID",
                $"PositionID: {id} не найден в базе данных!");

            return Result<PositionResponse>.Ok(position);
        }




    }
}
