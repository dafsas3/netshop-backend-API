using NetShopAPI.Models;
using NetShopAPI.Models.GeneralWarehouseModel.PositionItem;

namespace NetShopAPI.Features.Abstractions.Persistence
{
    public interface IPositionRepository
    {
        Task<Position?> GetByProductIdAsync(int productId, CancellationToken ct);
        Task<HashSet<string>> GetExistingNamesAsync(IEnumerable<string> names, CancellationToken ct);
        Task AddRangeAsync(IEnumerable<Position> positions, CancellationToken ct);
    }
}
