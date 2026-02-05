using Microsoft.EntityFrameworkCore;
using NetShopAPI.Data;
using NetShopAPI.Features.Abstractions.Persistence;
using NetShopAPI.Models;
using NetShopAPI.Models.GeneralWarehouseModel.PositionItem;

namespace NetShopAPI.Infrastructure.Persistence.Repositories.CatalogRepositories.Queries
{
    public class EfPositionRepository : IPositionRepository
    {
        private readonly ShopDbContext _db;

        public EfPositionRepository(ShopDbContext db) => _db = db;

        public Task<Position?> GetByProductIdAsync(int productId, CancellationToken ct)
            => _db.Positions.FirstOrDefaultAsync(p => p.ProductId == productId, ct);


        public async Task<HashSet<string>> GetExistingNamesAsync(IEnumerable<string> names, CancellationToken ct)
        {
            var nameList = names
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select(n => n.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (nameList.Count is 0) return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var existsNames = await _db.Positions
                .Where(p => nameList.Contains(p.Name))
                .Select(p => p.Name)
                .ToListAsync(ct);

            return existsNames.ToHashSet(StringComparer.OrdinalIgnoreCase);
        }


        public Task AddRangeAsync(IEnumerable<Position> positions, CancellationToken ct) => _db.AddRangeAsync(positions, ct);

    }
}
