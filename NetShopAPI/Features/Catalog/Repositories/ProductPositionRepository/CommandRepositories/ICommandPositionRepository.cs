using NetShopAPI.Features.Catalog.Commands.BulkCreatePositions;
using NetShopAPI.Models;

namespace NetShopAPI.Features.Catalog.Repositories.ProductPositionRepository.WriteRepository
{
    public interface ICommandPositionRepository
    {
        Task<Product> CreateProductAsync(BulkCreatePositionsCommand cmd);
    }
}
