using Microsoft.EntityFrameworkCore;
using NetShopAPI.Data;
using NetShopAPI.Features.Abstractions.Persistence;
using NetShopAPI.Models;
using NetShopAPI.Models.GeneralWarehouseModel.PositionItem;

namespace NetShopAPI.Infrastructure.Persistence.Repositories.CatalogRepositories
{
    public class EfPositionRepository : IPositionWriteRepository
    {
        private readonly ShopDbContext _db;

        public EfPositionRepository(ShopDbContext db) => _db = db;



        public Task<Position?> GetByProductIdAsync(int productId, CancellationToken ct)
            => _db.Positions.FirstOrDefaultAsync(p => p.ProductId == productId, ct);


        public Task AddRangeAsync(IEnumerable<Position> positions, CancellationToken ct) => _db.AddRangeAsync(positions, ct);

    }
}
