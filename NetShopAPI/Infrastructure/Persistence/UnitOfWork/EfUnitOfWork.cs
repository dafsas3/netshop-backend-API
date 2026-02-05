using NetShopAPI.Data;
using NetShopAPI.Features.Abstractions.Persistence;
using NetShopAPI.Models.GeneralWarehouseModel.PositionItem;

namespace NetShopAPI.Infrastructure.Persistence.UnitOfWork
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly ShopDbContext _db;

        public EfUnitOfWork(ShopDbContext db) => _db = db;

        public Task<int> SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);      
    }
}
