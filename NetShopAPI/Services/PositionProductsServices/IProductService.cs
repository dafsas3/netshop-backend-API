using NetShopAPI.Common;
using NetShopAPI.DTOs.CatalogDTO.Products.Request;
using NetShopAPI.DTOs.CatalogDTO.Products.Response;
using NetShopAPI.DTOs.PositionsItemDTO;
using NetShopAPI.Models;

namespace NetShopAPI.Services.PositionProductsServices
{
    public interface IProductService
    {
        Task<List<BulkPositionResponse>> CreatePositionProductAsync(List<ProductRequest> req);
        Task<Result<List<PositionResponse>>> GetPositionProductsByCategoryAsync(int categoryId);
        Task<Result<PositionResponse>> GetPositionProductByIdAsync(Guid id);
        Task<Result<PositionAddStockResponse>> AddToStock(int productId, int quantity);
    }
}
