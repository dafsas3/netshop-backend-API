using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NetShopAPI.Common;
using NetShopAPI.Data;
using NetShopAPI.DTOs.CatalogDTO.Products.Request;
using NetShopAPI.DTOs.CatalogDTO.Products.Response;
using NetShopAPI.DTOs.PositionsItemDTO;
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


        public async Task<Result<PositionAddStockResponse>> AddToStock(int productId, int quantity)
        {
            var itemStock = await _db.Positions
                .Where(p => p.ProductId == productId)
                .FirstOrDefaultAsync();

            if (itemStock is null)
                return Result<PositionAddStockResponse>.NotFound("INVALID_PRODUCT_ID",
                    $"ProductID: {productId} не найден в базе данных!");

            itemStock.Amount += quantity;
            await _db.SaveChangesAsync();

            var response = new PositionAddStockResponse
            {
                PositionId = itemStock.Id,
                ProductId = itemStock.ProductId,
                Name = itemStock.Name,
                Amount = itemStock.Amount
            };

            return Result<PositionAddStockResponse>.Ok(response);
        }


        public async Task<List<BulkPositionResponse>> CreatePositionProductAsync(List<ProductRequest> req)
        {
            var productNames = req.Select(p => p.Name).Distinct().ToList();

            var existingProduct = await _db.Products.Where(p => productNames.Contains(p.Name))
                .Select(p => p.Name)
                .ToListAsync();

            var reqCategIds = req.Select(c => c.CategoryId).Distinct().ToList();

            var existingCategories = await _db.ProductCategories
                .Where(c => reqCategIds.Contains(c.Id)).ToDictionaryAsync(c => c.Id);

            var createdPositions = new List<Position>();
            var responses = new List<BulkPositionResponse>(req.Count);
            var successResponseIndexes = new List<int>();

            foreach (var prod in req)
            {
                var response = new BulkPositionResponse();

                if (!existingCategories.TryGetValue(prod.CategoryId, out var category))
                {
                    response.ErrorsAddCategory.Add(new
                        BulkPositionResponse.ErrorsCategories(prod.CategoryId,
                        "В базе данных не существует ID данной категории."));

                }

                if (existingProduct.Contains(prod.Name))
                {
                    response.ErrorsAddPosition.Add(new BulkPositionResponse.Errors(prod.Name,
                        " - Продукт с данным именем уже существует в базе данных."));
                }

                if (response.ErrorsAddCategory.Any() || response.ErrorsAddPosition.Any())
                {
                    response.IsSuccess = false;
                    responses.Add(response);
                    continue;
                }

                var newProduct = PositionServiceManager.GetCreationProduct(prod);
                var newPosition = PositionServiceManager.GetCreationPosition(newProduct, prod, category);

                createdPositions.Add(newPosition);

                response.IsSuccess = true;
                responses.Add(response);
                successResponseIndexes.Add(responses.Count - 1);
            }

            if (createdPositions.Any())
            {
                _db.Positions.AddRange(createdPositions);
                await _db.SaveChangesAsync();

                for (int i = 0; i < createdPositions.Count; i++)
                {
                    var pos = createdPositions[i];
                    var successIndex = successResponseIndexes[i];

                    responses[successIndex].PositionId = pos.Id;
                    responses[successIndex].ProductId = pos.ProductId;
                    responses[successIndex].Name = pos.Name;
                    responses[successIndex].Price = pos.Price;
                    responses[successIndex].Amount = pos.Amount;
                    responses[successIndex].LastPurchasePrice = pos.LastPurchasePrice;
                    responses[successIndex].AdditionalInformation = pos.AdditionalInformation;
                    responses[successIndex].TotalPrice = pos.Price * pos.Amount;
                    responses[successIndex].CategoryName = pos.CategoryName;
                }
            }

            return responses;
        }


        public async Task<Result<List<PositionResponse>>> GetPositionProductsByCategoryAsync(int categoryId)
        {
            var category = await _db.ProductCategories
                .Where(c => c.Id == categoryId)
                .FirstOrDefaultAsync();

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
                }).ToListAsync();

            return Result<List<PositionResponse>>.Ok(positionsForCategory);
        }


        public async Task<Result<PositionResponse>> GetPositionProductByIdAsync(int id)
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
          .FirstOrDefaultAsync();

            if (position is null) return Result<PositionResponse>.NotFound("INVALID_POSITION_ID",
                $"PositionID: {id} не найден в базе данных!");

            return Result<PositionResponse>.Ok(position);
        }




    }
}
