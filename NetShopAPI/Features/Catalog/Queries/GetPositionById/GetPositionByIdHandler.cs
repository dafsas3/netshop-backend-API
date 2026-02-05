using Microsoft.EntityFrameworkCore;
using NetShopAPI.Common;
using NetShopAPI.Data;
using NetShopAPI.DTOs.PositionsItemDTO;

namespace NetShopAPI.Features.Catalog.Queries.GetPositionById
{
    public class GetPositionByIdHandler
    {

        private readonly ShopDbContext _db;

        public GetPositionByIdHandler(ShopDbContext db) => _db = db;

        public async Task<Result<PositionResponse>> Handle(int positionId, CancellationToken ct)
        {
            var positionDto = await _db.Positions
                .Where(p => p.Id == positionId)
                .Select(p => new PositionResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    CategoryName = p.Category.Name,
                    Amount = p.Amount,
                    Price = p.Price,
                    LastPurchasePrice = p.LastPurchasePrice,
                    AdditionalInformation = p.AdditionalInformation,
                    TotalPrice = p.Amount * p.Price
                })
                .FirstOrDefaultAsync(ct);

            return positionDto is null
                ? Result<PositionResponse>.NotFound("INVALID_POSITION_ID", "Позиция по указанному ID не найдена.")
                : Result<PositionResponse>.Ok(positionDto);
        }

    }
}
