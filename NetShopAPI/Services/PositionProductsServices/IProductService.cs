using NetShopAPI.Common;
using NetShopAPI.DTOs.CatalogDTO.Products.Request;
using NetShopAPI.DTOs.CatalogDTO.Products.Response;
using NetShopAPI.DTOs.PositionsItemDTO;
using NetShopAPI.Models;

namespace NetShopAPI.Services.PositionProductsServices
{
    public interface IProductService
    {
        Task<List<BulkPositionResponse>> CreatePositionProductAsync(List<ProductRequest> req,
            CancellationToken ct);
        Task<Result<List<PositionResponse>>> GetPositionProductsByCategoryAsync(int categoryId,
            CancellationToken ct);
        Task<Result<PositionResponse>> GetPositionProductByIdAsync(int id, CancellationToken ct);
        Task<Result<PositionAddStockResponse>> AddToStock(int productId, int quantity,
            CancellationToken ct);
    }
}
