using Microsoft.EntityFrameworkCore;
using NetShopAPI.Data;

namespace NetShopAPI.Infrastructure.Persistence.Repositories.CatalogRepositories
{
    public class EfPositionUniquenessReadRepository
    {

        private readonly ShopDbContext _db;

        public EfPositionUniquenessReadRepository(ShopDbContext db) => _db = db;

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
    }
}
