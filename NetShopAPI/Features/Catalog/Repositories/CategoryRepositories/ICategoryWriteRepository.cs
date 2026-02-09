using NetShopAPI.Features.Catalog.Categories.Command;
using NetShopAPI.Models;

namespace NetShopAPI.Features.Catalog.Repositories.CategoryRepositories
{
    public interface ICategoryWriteRepository
    {
        Task<Dictionary<int, Category>> GetCategoriesByIdAsync(IEnumerable<int> categoryId, CancellationToken ct);
        Task<Category> GetCategoryEntityByNameAsync(CreateCategoryCommand cmd, CancellationToken ct);
        void Add(Category category);
    }
}
