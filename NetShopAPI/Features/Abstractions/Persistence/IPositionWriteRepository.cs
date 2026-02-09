using NetShopAPI.Models;
using NetShopAPI.Models.GeneralWarehouseModel.PositionItem;

namespace NetShopAPI.Features.Abstractions.Persistence
{
    public interface IPositionWriteRepository
    {
        Task<Position?> GetByProductIdAsync(int productId, CancellationToken ct);
        Task AddRangeAsync(IEnumerable<Position> positions, CancellationToken ct);
    }
}
