using Microsoft.EntityFrameworkCore;
using NetShopAPI.Data;
using NetShopAPI.Features.Catalog.Repositories.CategoryRepositories;
using NetShopAPI.Models;

namespace NetShopAPI.Infrastructure.Persistence.Repositories.CatalogRepositories.Queries
{
    public class EfCategoryRepository : ICategoryRepository
    {
        private readonly ShopDbContext _db;

        public EfCategoryRepository(ShopDbContext db) => _db = db;

        public async Task<Dictionary<int, Category>> GetCategoriesByIdAsync(
            IEnumerable<int> categoryId, CancellationToken ct)
        {
            var existsCategories = await _db.ProductCategories
                .Where(c => categoryId.Contains(c.Id)).ToDictionaryAsync(c => c.Id, ct);

            return existsCategories;
        }
    }
}
