using NetShopAPI.Data;
using NetShopAPI.Features.Catalog.Commands.BulkCreatePositions;
using NetShopAPI.Features.Catalog.Repositories.ProductPositionRepository.WriteRepository;
using NetShopAPI.Models;

namespace NetShopAPI.Infrastructure.Persistence.Repositories.CatalogRepositories.CommandRepositories
{
    public class EfCommandPositionRepository : ICommandPositionRepository
    {

        private readonly ShopDbContext _db;

        public EfCommandPositionRepository(ShopDbContext db) => _db = db;

        public async Task<Product> CreateProductAsync(BulkCreatePositionsCommand cmd)
        {
            return 
        }

    }
}
