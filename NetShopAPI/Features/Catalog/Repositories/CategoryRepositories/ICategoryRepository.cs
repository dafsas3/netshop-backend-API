using NetShopAPI.Models;

namespace NetShopAPI.Features.Catalog.Repositories.CategoryRepositories
{
    public interface ICategoryRepository
    {
        Task<Dictionary<int, Category>> GetCategoriesByIdAsync(IEnumerable<int> categoryId, CancellationToken ct);
    }
}
