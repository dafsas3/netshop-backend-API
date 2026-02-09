using Microsoft.EntityFrameworkCore;
using NetShopAPI.Data;
using NetShopAPI.Features.Catalog.Categories.Command;
using NetShopAPI.Features.Catalog.Repositories.CategoryRepositories;
using NetShopAPI.Models;

namespace NetShopAPI.Infrastructure.Persistence.Repositories.CatalogRepositories
{
    public class EfCategoryRepository : ICategoryWriteRepository
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


        public async Task<Category> GetCategoryEntityByNameAsync(CreateCategoryCommand cmd, CancellationToken ct)
        {
            var normalizedName = cmd.Name.Trim();

            var category = await _db.ProductCategories
                .FirstOrDefaultAsync(c => c.Name == normalizedName);

            return category!;
        }


        public void Add(Category category)
        {
            _db.ProductCategories.Add(category);
        }
    }
}