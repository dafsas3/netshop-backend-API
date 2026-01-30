using NetShopAPI.Common;
using NetShopAPI.DTOs.CatalogDTO.Categoryes.Request;
using NetShopAPI.DTOs.CatalogDTO.Categoryes.Response;

namespace NetShopAPI.Services.CatalogServices
{
    public interface ICategoryService
    {
        Task<Result<CategoryResponse>> CreateCategoryAsync(CategoryRequest request, CancellationToken ct);
        Task<List<CategoryResponse>> GetAllAsync(CancellationToken ct);
    }
}
