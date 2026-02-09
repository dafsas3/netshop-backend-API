using Microsoft.EntityFrameworkCore;
using NetShopAPI.Common;
using NetShopAPI.Data;
using NetShopAPI.DTOs.PositionsItemDTO;

namespace NetShopAPI.Features.Catalog.Positions.Queries.GetPositionsByCategory
{
    public class GetPositionsByCategoryHandler
    {

        private readonly ShopDbContext _db;

        public GetPositionsByCategoryHandler(ShopDbContext db) => _db = db;

        public async Task<Result<List<PositionResponse>>> Handle(int categoryId, CancellationToken ct)
        {
            var categoryExists = await _db.ProductCategories.AnyAsync(c => c.Id == categoryId, ct);

            if (!categoryExists)
                return Result<List<PositionResponse>>.NotFound("CATEGORY_NOT_FOUND", "Данная категория не существует.");

            var positionsDto = await _db.Positions
                .AsNoTracking()
                .Where(p => p.Category.Id == categoryId)
                .Select(p => new PositionResponse
                {
                    Id = p.Id,
                    ProductId = p.ProductId,
                    Name = p.Name,
                    CategoryName = p.Category.Name,
                    Amount = p.Amount,
                    Price = p.Price,
                    AdditionalInformation = p.AdditionalInformation,
                    TotalPrice = p.Amount * p.Price
                })
                .ToListAsync(ct);

            return Result<List<PositionResponse>>.Ok(positionsDto);
        }

    }
}
